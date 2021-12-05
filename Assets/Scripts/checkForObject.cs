using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkForObject : MonoBehaviour
{
    private bool canPickUp = false;
    private bool hasObject = false;
    private string otherObjectName;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<pickUppable>().isPickUppable == true)
        {
            print("PICKUPPABLE");
            canPickUp = true;

            otherObjectName = other.gameObject.name;
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<pickUppable>().isPickUppable == true)
        {
            print("LEAVE");
            canPickUp = false;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canPickUp == true && hasObject == false)
            {
                //pick up the object
                print("PICKED UP");
                hasObject = true;
                GameObject.Find(otherObjectName).transform.parent = GameObject.Find("Body").transform;
            }
            else if (hasObject == true)
            {
                //drop the object
                print("DROP");
                hasObject = false;
                GameObject.Find(otherObjectName).transform.parent = transform.root.transform;
            }
        }
    }
}
