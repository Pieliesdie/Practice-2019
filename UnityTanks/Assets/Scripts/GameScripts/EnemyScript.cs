using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyScript : MonoBehaviour
{
    // Start is called before the first frame update
    MoveScript move;
    WeaponScript weapon;
    void Awake()
    {
        weapon = GetComponent<WeaponScript>();
        move = GetComponent<MoveScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (move != null && Random.Range(0, 100) < 1)
        {
            move.direction = new Vector2(Random.Range(-1f, 1), Random.Range(-1f, 1));
        }
        if(weapon!=null&& Random.Range(0, 100) < 1)
        {
            weapon.Shoot();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (move != null)
        {
            //Debug.Log("Move not null");
            if (collision.gameObject.tag == "Enemy")
            {
                //Debug.Log("Do something here");
                move.direction = new Vector2(-move.direction.x, -move.direction.y);
            }
            else
            {
                move.direction = new Vector2(Random.Range(-1f, 1), Random.Range(-1f, 1));
            }
        }
    }
    void OnDestroy()
    {
        if (GameObject.FindGameObjectWithTag("Enemy") == null)
        {
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        }
    }
}
