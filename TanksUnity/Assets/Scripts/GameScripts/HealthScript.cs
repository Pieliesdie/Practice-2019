using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    // Start is called before the first frame update
    public int hp = 3;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        BulletScript bullet = collision.gameObject.GetComponent<BulletScript>();
        if (bullet != null && collision.gameObject.transform.parent.gameObject.tag !=this.gameObject.tag)
        {
            Destroy(collision.gameObject);
            if (hp > 0)
            {
                hp--;
            }
            else
            {
                EffectsHelper.Instance.Explosion(transform.position);
                Destroy(gameObject);
            }
        }
    }
}
