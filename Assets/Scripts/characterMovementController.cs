using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovementController : MonoBehaviour
{
    public CharacterController controller;
    
    public float maxSpeed = 6f;
    public float turnSmoothTime = 0.01f;
    public float turnSmoothing;
    public float targetAngle = 0;
    private float speed = 0;
    private float speedStep = 0.5f;
    [SerializeField] private float frictionModifier;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float verical = Input.GetAxisRaw("Vertical");
        
        GameObject body = transform.Find("Body").gameObject;
        GameObject arm = body.transform.Find("Arm").gameObject;

        Vector3 direction = new Vector3(horizontal, 0f, verical).normalized;
        
        if (direction.magnitude > 0.1f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            if (speed < maxSpeed)
            {
                speed += speedStep;
            }
            controller.Move(direction * speed * Time.deltaTime);
        }
        else
        {
            if (speed > 0.1f)
            {
                speed -= speedStep;
            }
        }
        
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothing, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        
        if (Input.GetMouseButtonDown(0))
        {
            arm.transform.Rotate(-50, 0, 0);
            
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            arm.transform.Rotate(50, 0, 0);
        }
        
        //print("speed = " + speed);
    }
}
