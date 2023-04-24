using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    float grav = 1f;

    Vector3 vel = Vector3.zero;

    private CharacterController c_c;

    // Start is called before the first frame update
    void Start()
    {
        c_c = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (c_c.isGrounded)
        {
            vel.y = -grav;
        }
        else
        {
            vel.y -= grav;
        }

        c_c.Move(vel * Time.deltaTime);
    }
}