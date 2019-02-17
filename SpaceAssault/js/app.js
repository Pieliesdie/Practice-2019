function getRandomInt(min, max) {
    return Math.floor(Math.random() * (max - min)) + min;
}

var requestAnimFrame = (function () {
    return window.requestAnimationFrame ||
        window.webkitRequestAnimationFrame ||
        window.mozRequestAnimationFrame ||
        window.oRequestAnimationFrame ||
        window.msRequestAnimationFrame ||
        function (callback) {
            window.setTimeout(callback, 1000 / 60);
        };
})();

var canvas = document.createElement("canvas");
var ctx = canvas.getContext("2d");
canvas.width = 1024;
canvas.height = 500;
document.body.appendChild(canvas);

var lastTime;

function main() {
    var now = Date.now();
    var dt = (now - lastTime) / 1000.0;

    update(dt);
    render();

    lastTime = now;
    requestAnimFrame(main);
};

function createMegaLiths() {
    let Megalith_Count = getRandomInt(MegalithCount["min"], MegalithCount["max"] + 1);

    for (let i = 0; i < Megalith_Count; i++) {
        let rand = getRandomInt(0, 2);
        let Megalith_type = {
            pos: [0, 212],
            size: [60, 60]
        }
        if (rand == 0) {
            Megalith_type.pos = [0, 272],
                Megalith_type.size = [55, 45]
        }

        do {
            var x = getRandomInt(60, canvas.width - 60);
            var y = getRandomInt(60, canvas.height - 60);
            var next = false;

            for (var j = 0; j < Megaliths.length; j++) {
                if (boxCollides(
                    [x - Megalith_type.size[0], y - Megalith_type.size[1]],
                    [Megalith_type.size[0] * 3, Megalith_type.size[1] * 3],
                    Megaliths[j].pos,
                    Megaliths[j].sprite.size)) {

                    next = true;
                    break;
                }
            }
        } while (next)

        Megaliths.push({
            pos: [x, y],
            sprite: new Sprite('img/sprites.png', Megalith_type.pos, Megalith_type.size)
        });
    }
}

function init() {
    terrainPattern = ctx.createPattern(resources.get('img/terrain.png'), 'repeat');

    document.getElementById('play-again').addEventListener('click', function () {
        reset();
    });
    reset();
    lastTime = Date.now();
    main();
}

resources.load([
    'img/sprites.png',
    'img/terrain.png'
]);
resources.onReady(init);


var player = {
    pos: [0, 0],
    sprite: new Sprite('img/sprites.png', [0, 0], [39, 39], 16, [0, 1])
};

var bullets = [];
var enemies = [];
var explosions = [];
var Megaliths = [];
var Manna = [];
var lastFire = Date.now();
var gameTime = 0;
var isGameOver;
var terrainPattern;

var mannaScore = 0;
var score = 0;
var scoreEl = document.getElementById('score');
var MannaScoreEl = document.getElementById('MannaScore');


var playerSpeed = 200;
var bulletSpeed = 500;
var enemySpeed = 100;

var MegalithCount = {
    min: 4,
    max: 8
}
var MannaCount = {
    min: 4,
    max: 12
}

function update(dt) {
    gameTime += dt;

    handleInput(dt);
    updateEntities(dt);
    if (Math.random() < 1 - Math.pow(.993, gameTime)) {
        enemies.push({
            pos: [canvas.width,
            Math.random() * (canvas.height - 39)],
            sprite: new Sprite('img/sprites.png', [0, 78], [80, 39],
                6, [0, 1, 2, 3, 2, 1])
        });
    }
    if (Manna.length < MannaCount["min"]) {
        point = PointForMana();
        Manna.push({
            pos: point,
            sprite: new Sprite('img/sprites.png', [0, 164], [44, 45], 6, [0, 1, 0])
        })
    }
    else if (Manna.length < MannaCount["max"]) {
        let chance = gameTime % 100;
        if (getRandomInt(chance, 500) == chance) {
            point = PointForMana();
            Manna.push({
                pos: point,
                sprite: new Sprite('img/sprites.png', [0, 164], [44, 45], 6, [0, 1, 0])
            })
        }
    }

    function PointForMana() {
        do {
            var x = getRandomInt(44, canvas.width - 44);
            var y = getRandomInt(45, canvas.height - 45);
            var next = false;
            for (var i = 0; i < Manna.length; i++) {
                if (boxCollides([x, y], [44, 45], Manna[i].pos, Manna[i].sprite.size)) {
                    next = true;
                    break;
                }
            }
            if (!next) {
                for (var j = 0; j < Megaliths.length; j++) {
                    if (boxCollides([x, y], [44, 45], Megaliths[j].pos, Megaliths[j].sprite.size)) {
                        next = true;
                        break;
                    }
                }
            }
        } while (next)
        return [x, y]
    }

    checkCollisions(dt);
    scoreEl.innerHTML = "Score: "+ score;
    MannaScoreEl.innerHTML = "Manna: " + mannaScore;
};

function handleInput(dt) {
    if (input.isDown('DOWN') || input.isDown('s')) {
        player.pos[1] += playerSpeed * dt;
    }

    if (input.isDown('UP') || input.isDown('w')) {
        player.pos[1] -= playerSpeed * dt;
    }

    if (input.isDown('LEFT') || input.isDown('a')) {
        player.pos[0] -= playerSpeed * dt;
    }

    if (input.isDown('RIGHT') || input.isDown('d')) {
        player.pos[0] += playerSpeed * dt;
    }

    if (input.isDown('SPACE') &&
        !isGameOver &&
        Date.now() - lastFire > 100) {
        var x = player.pos[0] + player.sprite.size[0] / 2;
        var y = player.pos[1] + player.sprite.size[1] / 2;

        bullets.push({
            pos: [x, y],
            dir: 'forward',
            sprite: new Sprite('img/sprites.png', [0, 39], [18, 8])
        });
        bullets.push({
            pos: [x, y],
            dir: 'up',
            sprite: new Sprite('img/sprites.png', [0, 50], [9, 5])
        });
        bullets.push({
            pos: [x, y],
            dir: 'down',
            sprite: new Sprite('img/sprites.png', [0, 60], [9, 5])
        });

        lastFire = Date.now();
    }
}

function updateEntities(dt) {
    player.sprite.update(dt);

    for (var i = 0; i < bullets.length; i++) {
        var bullet = bullets[i];

        switch (bullet.dir) {
            case 'up': bullet.pos[1] -= bulletSpeed * dt; break;
            case 'down': bullet.pos[1] += bulletSpeed * dt; break;
            default:
                bullet.pos[0] += bulletSpeed * dt;
        }

        if (bullet.pos[1] < 0 || bullet.pos[1] > canvas.height ||
            bullet.pos[0] > canvas.width) {
            bullets.splice(i, 1);
            i--;
        }
    }

    for (var i = 0; i < enemies.length; i++) {
        enemies[i].pos[0] -= enemySpeed * dt;
        enemies[i].sprite.update(dt);

        if (enemies[i].pos[0] + enemies[i].sprite.size[0] < 0) {
            enemies.splice(i, 1);
            i--;
        }
    }

    for (var i = 0; i < explosions.length; i++) {
        explosions[i].sprite.update(dt);

        if (explosions[i].sprite.done) {
            explosions.splice(i, 1);
            i--;
        }
    }

    for (var i = 0; i < Manna.length; i++) {
        Manna[i].sprite.update(dt);
    }
}

function collides(x, y, r, b, x2, y2, r2, b2) {
    return !(r <= x2 || x > r2 ||
        b <= y2 || y > b2);
}

function boxCollides(pos, size, pos2, size2) {
    return collides(pos[0], pos[1],
        pos[0] + size[0], pos[1] + size[1],
        pos2[0], pos2[1],
        pos2[0] + size2[0], pos2[1] + size2[1]);
}

function checkCollisions(dt) {
    checkPlayerBounds(dt);

    for (var i = 0; i < enemies.length; i++) {
        var pos = enemies[i].pos;
        var size = enemies[i].sprite.size;

        for (var k = 0; k < Megaliths.length; k++) {
            if (boxCollides(pos, size, Megaliths[k].pos, Megaliths[k].sprite.size)) {
                detourCollisions(Megaliths[k].pos, pos, enemySpeed, dt);
                break;
            }
        }

        for (var j = 0; j < bullets.length; j++) {
            var pos2 = bullets[j].pos;
            var size2 = bullets[j].sprite.size;

            if (boxCollides(pos, size, pos2, size2)) {

                enemies.splice(i, 1);
                i--;

                score += 100;

                explosions.push({
                    pos: pos,
                    sprite: new Sprite('img/sprites.png',
                        [0, 117],
                        [39, 39],
                        16,
                        [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12],
                        null,
                        true)
                });

                bullets.splice(j, 1);
                break;
            }
        }

        if (boxCollides(pos, size, player.pos, player.sprite.size)) {
            gameOver();
        }
    }

    for (var i = 0; i < Megaliths.length; i++) {
        var pos = Megaliths[i].pos;
        var size = Megaliths[i].sprite.size;

        for (var j = 0; j < bullets.length; j++) {
            var pos2 = bullets[j].pos;
            var size2 = bullets[j].sprite.size;

            if (boxCollides(pos, size, pos2, size2)) {
                bullets.splice(j, 1);
                j--;
                break;
            }
        }
    }
    for (var i = 0; i < Manna.length; i++) {
        if (boxCollides(Manna[i].pos, Manna[i].sprite.size, player.pos, player.sprite.size)) {
            explosions.push({
                pos: Manna[i].pos,
                sprite: new Sprite('img/sprites.png',
                    [0, 164],
                    [44, 45],
                    6,
                    [0, 1, 2, 3],
                    null,
                    true)
            });
            Manna.splice(i, 1);
            i--;
            mannaScore++;
        }
    }

}

function checkPlayerBounds(dt) {
    if (player.pos[0] < 0) {
        player.pos[0] = 0;
    }
    else if (player.pos[0] > canvas.width - player.sprite.size[0]) {
        player.pos[0] = canvas.width - player.sprite.size[0];
    }

    if (player.pos[1] < 0) {
        player.pos[1] = 0;
    }
    else if (player.pos[1] > canvas.height - player.sprite.size[1]) {
        player.pos[1] = canvas.height - player.sprite.size[1];
    }
    for (var i = 0; i < Megaliths.length; i++) {
        if (boxCollides(Megaliths[i].pos, Megaliths[i].sprite.size, player.pos, player.sprite.size)) {
            detourCollisions(Megaliths[i].pos, player.pos, playerSpeed, dt)
        }
    }

}

function detourCollisions(static_pos, move_pos, speed, dt) {
    var _angle = angle(move_pos[0], move_pos[1], static_pos[0], static_pos[1]);
    if (_angle > 0 && _angle < 90) {
        move_pos[0] += speed * dt;
        move_pos[1] += speed * dt;
    }
    else if (_angle > 90 && _angle < 180) {
        move_pos[0] -= speed * dt;
        move_pos[1] += speed * dt;
    }
    else if (_angle > -180 && _angle < -90) {
        move_pos[0] -= speed * dt;
        move_pos[1] -= speed * dt;
    }
    else if (_angle > -90 && _angle < 0) {
        move_pos[0] += speed * dt;
        move_pos[1] -= speed * dt;
    }
}

function angle(x, y, x2, y2) {
    return Math.atan2(y - y2, x - x2) * (180 / 3.14);
}

function render() {
    ctx.fillStyle = terrainPattern;
    ctx.fillRect(0, 0, canvas.width, canvas.height);

    if (!isGameOver) {
        renderEntity(player);
    }
    renderEntities(Manna);
    renderEntities(Megaliths);
    renderEntities(bullets);
    renderEntities(enemies);
    renderEntities(explosions);
};

function renderEntities(list) {
    for (var i = 0; i < list.length; i++) {
        renderEntity(list[i]);
    }
}

function renderEntity(entity) {
    ctx.save();
    ctx.translate(entity.pos[0], entity.pos[1]);
    entity.sprite.render(ctx);
    ctx.restore();
}

function gameOver() {
    document.getElementById('game-over').style.display = 'block';
    document.getElementById('game-over-overlay').style.display = 'block';
    isGameOver = true;
}

function reset() {
    document.getElementById('game-over').style.display = 'none';
    document.getElementById('game-over-overlay').style.display = 'none';
    isGameOver = false;
    gameTime = 0;
    score = 0;
    mannaScore=0;

    Megaliths = [];
    Manna = [];
    createMegaLiths();
    enemies = [];
    bullets = [];

    player.pos = [50, canvas.height / 2];
};  