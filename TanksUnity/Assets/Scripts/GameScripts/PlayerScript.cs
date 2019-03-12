using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public Text text;
    public int Score = 0;
    public Vector2 speed = new Vector2(220,220);
    private Vector2 movement;
    private float x;
    private float y;
    private float angle;
    private WeaponScript Weapon;
    private Rigidbody2D Rigidbody2D;
    // Start is called before the first frame update
    void Awake()
    {
        Weapon = GetComponent<WeaponScript>();
        Rigidbody2D = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        bool shoot = SimpleInput.GetButtonDown("Fire");
        shoot|=Input.GetButtonDown("Fire1");
        if (shoot)
        {
            Weapon?.Shoot();
        }
        x = SimpleInput.GetAxis("Horizontal");
        y = SimpleInput.GetAxis("Vertical");
        angle = Mathf.Atan2(0 - y, 0 - x) / Mathf.PI * 180;
        movement = new Vector2(speed.x * x * Time.deltaTime, speed.y * y*Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Rigidbody2D.velocity = movement;
        if (x != 0 || y != 0)
        {
            Rigidbody2D.MoveRotation(angle + 180);

        }
        text.text = $"Score:{Score}";
    }

    void OnDestroy(){
        EffectsHelper.Instance.gameObject.AddComponent<GameOverScript>();

        }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Star")
        {
            Score++;
            Destroy(collision.gameObject);
        }
    }    
}
