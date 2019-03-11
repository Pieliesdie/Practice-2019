using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpliceScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        BulletScript bullet = collision.gameObject.GetComponent<BulletScript>();
        if (bullet != null)
        {
            EffectsHelper.Instance.Explosion1(collision.gameObject.transform.position);
            Destroy(collision.gameObject);            
        }
    }
}
