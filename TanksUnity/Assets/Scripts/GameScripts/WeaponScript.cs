using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public Transform Bullet;
    public float ShootingRate = 2;
    private float ShootDelay;
    
    public void Update()
    {
        if (ShootDelay > 0)
        {
            ShootDelay -= Time.deltaTime;
        }
    }

    public void Shoot()
    {
        if (CanAttack)
        {
            ShootDelay = ShootingRate;
            var bullet = Instantiate(Bullet) as Transform;
            bullet.parent = this.transform;
            bullet.position = transform.position;
            MoveScript move = bullet.gameObject.GetComponent<MoveScript>();
            if (move != null)
            {
                move.direction = this.transform.right;
            }
        }
    }

    private bool CanAttack => ShootDelay <= 0;
}
