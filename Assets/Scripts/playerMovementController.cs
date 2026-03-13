using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovementController : MonoBehaviour
{
    [SerializeField] private float walkSpeed;
    [SerializeField] private float frictionModifier;
    [SerializeField] private float spinSpeed;
    private float speed;
    public Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject body = transform.Find("Body").gameObject;
        GameObject arm = body.transform.Find("Arm").gameObject;
        
        if (Input.GetKey(KeyCode.W))
        {
            if (transform.rotation.eulerAngles.y is > 0 and < 180)
            {
                transform.Rotate(0,-spinSpeed,0);
            } else if (transform.rotation.eulerAngles.y < 360)
            {
                transform.Rotate(0,spinSpeed,0);
            }

            if (speed < walkSpeed)
            {
                speed += frictionModifier;
            }
        } else if (Input.GetKey(KeyCode.S))
        {
            if (transform.rotation.eulerAngles.y > 180)
            {
                transform.Rotate(0,-spinSpeed,0);
            } else if (transform.rotation.eulerAngles.y < 180)
            {
                transform.Rotate(0,spinSpeed,0);
            }
            
            if (speed < walkSpeed)
            {
                speed += frictionModifier;
            }
        } else if (Input.GetKey(KeyCode.D))
        {
            if (transform.rotation.eulerAngles.y is > 90 and < 270)
            {
                transform.Rotate(0,-spinSpeed,0);
            } else if (transform.rotation.eulerAngles.y is < 90 or > 270)
            {
                transform.Rotate(0,spinSpeed,0);
            }
            
            if (speed < walkSpeed)
            {
                speed += frictionModifier;
            }
        } else if (Input.GetKey(KeyCode.A))
        {
            if (transform.rotation.eulerAngles.y is > 270 or < 90 )
            {
                transform.Rotate(0,-spinSpeed,0);
            } else if (transform.rotation.eulerAngles.y is < 270 and > 90)
            {
                transform.Rotate(0,spinSpeed,0);
            }
            
            if (speed < walkSpeed)
            {
                speed += frictionModifier;
            }
        }
        else if (speed > 0)
        {
            speed -= frictionModifier;
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            arm.transform.Rotate(-50, 0, 0);
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            arm.transform.Rotate(50, 0, 0);
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            rb.AddRelativeForce(Vector3.forward * walkSpeed);
        } else if (rb.linearVelocity.z > 0.5)
        {
            rb.AddRelativeForce(Vector3.forward * -walkSpeed);
        } else
        {
            rb.AddForce(0,0,0);
        }
    }
}
