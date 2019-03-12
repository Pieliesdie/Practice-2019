using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjGeneratorScript : MonoBehaviour
{
    public float leftCoord;
    public float rightCoord;
    public float topCoord;
    public float botCoord;
    public Transform Obj;
    // Start is called before the first frame update
    public int Count = 5;

    // Update is called once per frame
    void Update()
    {
        int count = GameObject.FindGameObjectsWithTag(Obj.tag).Length;
        if (count < Count)
        {
            var obj = Instantiate(Obj) as Transform;
            float left = Random.Range(leftCoord, rightCoord);
            float height = Random.Range(topCoord, botCoord);
            obj.position = new Vector3(left, height, -1);
        }
    }
   
}
