using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        MoveScript move = this.gameObject.GetComponent<MoveScript>();
        if (move != null)
        {
            move.speed = new Vector2(move.speed.x * 2, move.speed.y * 2);
        }
    }

    void Start()
    {
        Destroy(gameObject, 5);
    }

    // Update is called once per frame
    void Update()
    {

    }

}
