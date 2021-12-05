using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float walkSpeed;
    [SerializeField] private float frictionModifier;
    [SerializeField] private float spinSpeed;
    private float hSpeed = 0;
    private float vSpeed = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameObject body = transform.Find("Body").gameObject;
        if (Input.GetKey(KeyCode.W) && vSpeed < walkSpeed)
        {
            vSpeed = vSpeed + frictionModifier;
            
            if (body.transform.rotation.eulerAngles.y > 0 && body.transform.rotation.eulerAngles.y < 180)
            {
                body.transform.Rotate(0,-spinSpeed,0);
            } else if (body.transform.rotation.eulerAngles.y < 360)
            {
                body.transform.Rotate(0,spinSpeed,0);
            }
            
        } else if (Input.GetKey(KeyCode.S) && vSpeed > -walkSpeed)
        {
            vSpeed = vSpeed - frictionModifier;
            if (body.transform.rotation.eulerAngles.y > 180)
            {
                body.transform.Rotate(0,-spinSpeed,0);
            } else if (body.transform.rotation.eulerAngles.y < 180)
            {
                body.transform.Rotate(0,spinSpeed,0);
            }
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
            
            if (body.transform.rotation.eulerAngles.y > 90 && body.transform.rotation.eulerAngles.y < 270)
            {
                body.transform.Rotate(0,-spinSpeed,0);
            } else if (body.transform.rotation.eulerAngles.y < 90 || body.transform.rotation.eulerAngles.y > 270)
            {
                body.transform.Rotate(0,spinSpeed,0);
            }
            
        } else if (Input.GetKey(KeyCode.A) && hSpeed > -walkSpeed)
        {
            hSpeed = hSpeed - frictionModifier;
            
            if (body.transform.rotation.eulerAngles.y > 270 || body.transform.rotation.eulerAngles.y < 90 )
            {
                body.transform.Rotate(0,-spinSpeed,0);
            } else if (body.transform.rotation.eulerAngles.y < 270 && body.transform.rotation.eulerAngles.y > 90)
            {
                body.transform.Rotate(0,spinSpeed,0);
            }
        }
        else if (hSpeed > 0)
        {
            hSpeed = hSpeed - frictionModifier;
        } else if (hSpeed < 0)
        {
            hSpeed = hSpeed + frictionModifier;
        }
        
        
        transform.Translate(hSpeed * Time.deltaTime,0,vSpeed * Time.deltaTime);
        print("rotation = "+ body.transform.rotation.eulerAngles.y);
        
    }
}