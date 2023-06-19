using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDown : MonoBehaviour
{
    public bool above = true;
    bool aboveTic = false;
    public float down = 2;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(above == true && aboveTic == false) 
        {
            //int x = 1;
            //while (x != down + 1)
            //{
            //    Debug.Log("down");
            //    transform.position -= new Vector3(0, 1, 0);
            //    x++;
            //}
            transform.position -= new Vector3(0, down, 0);
            aboveTic = true;
        }
        else if(above == false && aboveTic == true) 
        {
            transform.position += new Vector3(0, down, 0);
            aboveTic = false;
        }
    }
}
