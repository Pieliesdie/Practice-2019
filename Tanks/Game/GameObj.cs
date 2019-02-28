using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Game
{
    public class GameObj
    {
        delegate void ImageChanged();
        event ImageChanged OnImageChanged;

        public string name => this.ToString();
        public int speed;
        public Point pos;
        protected Bitmap image;
        [Browsable(false)]//?????
        public Bitmap Image
        {
            get
            {
                if (image == null)
                {
                    return null;
                }
                Bitmap result = null;
                switch (Direction)
                {
                    case Direction.bot:
                        {
                            result = new Bitmap(RotateImage(image, RotateFlipType.Rotate90FlipNone));
                            break;
                        }
                    case Direction.left:
                        {
                            result = new Bitmap(RotateImage(image, RotateFlipType.Rotate180FlipNone));
                            break;
                        }
                    case Direction.right:
                        {
                            result = image;
                            break;
                        }
                    case Direction.top:
                        {
                            result = new Bitmap(RotateImage(image, RotateFlipType.Rotate270FlipNone));
                            break;
                        }
                }
                return result;
            }
            set
            {
                if (value != image)
                {
                    OnImageChanged?.Invoke();
                }
                image = value;
            }
        }
        public bool CanDestroy; //??????

        private Size size;
        [Browsable(false)]//??????
        public Size Size
        {
            get
            {
                if (Direction == Direction.top || Direction == Direction.bot)
                    return new Size(size.Height,size.Width);
                return size;
            }
            set => size = value;
        }

        protected Direction Direction;

        public GameObj(Point pos, Size size, int speed = 0, Direction direction = Direction.right, Bitmap image = null)
        {
            this.pos = pos;
            this.Size = size;
            this.Direction = direction;
            this.speed = speed;
            this.Image = image;
        }

        public Rectangle HitBox => new Rectangle(pos, new Size(Size.Width * 120 / 100, Size.Height * 120 / 100));

        public void Update()
        {
            switch (Direction)
            {
                case Direction.bot:
                    {
                        pos.Y += speed;
                        break;
                    }
                case Direction.left:
                    {
                        pos.X -= speed;
                        break;
                    }
                case Direction.right:
                    {
                        pos.X += speed;
                        break;
                    }
                case Direction.top:
                    {
                        pos.Y -= speed;
                        break;
                    }
            }
        }

        public GameObj Fire()
        {
            return new GameObj(new Point(pos.X + Size.Width / 2, pos.Y + Size.Height / 2), new Size(25, 15), speed * 150 / 100, Direction,Game.bulletSprite);
        }

        public static Image RotateImage(Image img, RotateFlipType flipType)
        {
            var bmp = new Bitmap(img);

            using (Graphics gfx = Graphics.FromImage(bmp))
            {
                gfx.DrawImage(img, 0, 0, img.Width, img.Height);
            }

            bmp.RotateFlip(flipType);

            return bmp;
        }
    }
}
