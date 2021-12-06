using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkForObject : MonoBehaviour
{
    private bool canPickUp = false;
    private bool canPutDown = false;
    private bool hasObject = false;
    private string otherObjectName;
    private string putDownObjectName;
    private Vector3 pickedUpPosition;
    private Vector3 putDownPosition;
    
    private void OnTriggerEnter(Collider other)
    {
        if (hasObject == false)
        {
            if (other.gameObject.GetComponent<pickUppable>().isPickUppable == true)
            {
                print("PICKUPPABLE");
                canPickUp = true;

                otherObjectName = other.gameObject.name;
            }
        }
        else
        {
            if (other.gameObject.GetComponent<putDownable>().isPutDownNode == true)
            {
                print("PUTDOWNABLE");
                putDownObjectName = other.gameObject.name;
                canPutDown = true;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (hasObject == true)
        {
            if (other.gameObject.GetComponent<putDownable>().isPutDownNode == true)
            {
                print("LEAVENODE");
                canPutDown = false;
            }
        }
        else
        {
            if (other.gameObject.GetComponent<pickUppable>().isPickUppable == true)
            {
                print("LEAVE");
                canPickUp = false;
            }
        }
    }

    void Update()
    {
        pickedUpPosition = transform.position;
        if (Input.GetMouseButtonDown(0))
        {
            if (canPickUp == true && hasObject == false)
            {
                //pick up the object
                print("PICKED UP");
                hasObject = true;
                GameObject.Find(otherObjectName).transform.parent = GameObject.Find("Body").transform;
                GameObject.Find(otherObjectName).transform.position = pickedUpPosition;
            }
            else if (hasObject == true)
            {   
                print("DROP");
                hasObject = false;
                GameObject.Find(otherObjectName).transform.parent = transform.root.transform;
                
                //Check if this is near a put down node
                if (canPutDown == true)
                {
                    //position the object
                    GameObject.Find(otherObjectName).transform.position = GameObject.Find(putDownObjectName).transform.position;
                }
            }
        }
    }
}
