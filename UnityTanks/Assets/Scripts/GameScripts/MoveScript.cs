using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    private Rigidbody2D Rigidbody2D;
    public Vector2 speed = new Vector2(200, 200);
    public Vector2 direction = new Vector2(-1, 0);
    private Vector2 movement;
    // Start is called before the first frame update
    void Start()
    {

    }
    void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        movement = new Vector2(speed.x * direction.x*Time.deltaTime, speed.y * direction.y*Time.deltaTime);
    }

    private void FixedUpdate()
    {
        float A = Mathf.Atan2(0 - direction.y, 0 - direction.x) / Mathf.PI * 180; 
        Rigidbody2D.MoveRotation(A + 180);
        Rigidbody2D.velocity = movement;
    }
}
