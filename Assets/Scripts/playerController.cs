using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float walkSpeed;
    [SerializeField] private float frictionModifier;
    private float hSpeed = 0;
    private float vSpeed = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && vSpeed < walkSpeed)
        {
            vSpeed = vSpeed + frictionModifier;
        } else if (Input.GetKey(KeyCode.S) && vSpeed > -walkSpeed)
        {
            vSpeed = vSpeed - frictionModifier;
        }
        else if (vSpeed > 0)
        {
            vSpeed = vSpeed - frictionModifier;
        } else if (vSpeed < 0)
        {
            vSpeed = vSpeed + frictionModifier;
        }
        
        if (Input.GetKey(KeyCode.D) && hSpeed < walkSpeed)
        {
            hSpeed = hSpeed + frictionModifier;
        } else if (Input.GetKey(KeyCode.A) && hSpeed > -walkSpeed)
        {
            hSpeed = hSpeed - frictionModifier;
        }
        else if (hSpeed > 0)
        {
            hSpeed = hSpeed - frictionModifier;
        } else if (hSpeed < 0)
        {
            hSpeed = hSpeed + frictionModifier;
        }
        
        
        transform.Translate(hSpeed * Time.deltaTime,0,vSpeed * Time.deltaTime);
        
        print("vSpeed: " + vSpeed);
    }
}