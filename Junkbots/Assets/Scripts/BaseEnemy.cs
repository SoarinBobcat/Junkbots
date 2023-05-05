using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    float grav = 0.8f;
    float fric = 0.1f;

    public Vector3 vel = Vector3.zero;

    private CharacterController c_c;

    // Start is called before the first frame update
    void Start()
    {
        c_c = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (c_c.isGrounded)
        {
            Friction();
        }
        else
        {
            vel.y -= grav;
        }

        c_c.Move(vel * Time.deltaTime);
    }

    private void Friction()
    {
        Vector3 velocity = vel;
        velocity.y = 0;
        float speed = velocity.magnitude;
        float drop = 0;

        float control = speed < 0.5f ? 0.5f : speed;
        drop = control * fric;

        float newSpeed = speed - drop;
        if (newSpeed < 0)
        {
            newSpeed = 0;
        }

        if (newSpeed > 0)
        {
            newSpeed /= speed;
        }

        vel.x *= newSpeed;
        vel.z *= newSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Melee")
        {
            Vector3 dir = (transform.position - other.transform.position).normalized;
            vel = 21 * dir;
            vel.y = 16;
            var s = other.transform.parent.GetComponent<Player_Movement>();
            Debug.Log(s.damage);
        }
    }
}
