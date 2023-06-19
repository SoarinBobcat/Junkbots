using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_shot : MonoBehaviour
{
    public int speed = 1000;
    float lifetime = 1.5f;
 
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) 
            Destroy(gameObject);
    }
}
