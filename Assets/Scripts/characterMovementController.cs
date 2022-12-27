using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterMovementController : MonoBehaviour
{
    public float maxSpeed = 6f;
    public float turnSmoothTime = 0.01f;
    public float turnSmoothing;
    public float targetAngle = 0;
    private float speed = 20f;
    [SerializeField] private float frictionModifier;
    [SerializeField] private float gravity;

    private CharacterController controller;
    private GameObject body;
    private GameObject arm;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        body = transform.Find("Body").gameObject;
        arm = body.transform.Find("Arm").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        Vector3 vel = new Vector3();

        // Apply movement, if necessary
        if (direction.magnitude > 0.1f)
        {
            targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            vel += direction * speed;
        }
        
        // Apply gravity
        vel += new Vector3(0, -gravity, 0);

        controller.Move(vel * Time.deltaTime);
        
        // Animate the body turning in the direction we're heading
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothing, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);
        
        // Animate the arm
        if (Input.GetMouseButtonDown(0))
        {
            arm.transform.Rotate(-50, 0, 0);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            arm.transform.Rotate(50, 0, 0);
        }
    }
}
