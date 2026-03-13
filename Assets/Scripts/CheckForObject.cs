using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForObject : MonoBehaviour
{
    private GameObject holderObject;
    private GameObject holdingObject;
    private GameObject pickupTarget;
    private GameObject putdownTarget;

    void Start()
    {
        // Find the ancestor object with the Body name, or the top-level object
        Transform rootLevel = transform.root;
        Transform ancestor = transform;
        while (ancestor.name != "Body" && ancestor != rootLevel)
        {
            ancestor = ancestor.parent;
        }
        holderObject = ancestor.gameObject;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (holdingObject)
        {
            PutDownable otherComponent = other.gameObject.GetComponent<PutDownable>();
            if (otherComponent && otherComponent.isPutDownNode)
            {
                putdownTarget = other.gameObject;
            }
        }
        else
        {
            PickUppable otherComponent = other.gameObject.GetComponent<PickUppable>();
            if (otherComponent && otherComponent.isPickUppable)
            {
                pickupTarget = other.gameObject;
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (holdingObject)
        {
            PutDownable otherComponent = other.gameObject.GetComponent<PutDownable>();
            if (otherComponent && otherComponent.isPutDownNode)
            {
                putdownTarget = null;
            }
        }
        else
        {
            PickUppable otherComponent = other.gameObject.GetComponent<PickUppable>();
            if (otherComponent && otherComponent.isPickUppable)
            {
                pickupTarget = null;
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (pickupTarget && !holdingObject)
            {
                // Pick up the object
                holdingObject = pickupTarget;
                holdingObject.transform.parent = holderObject.transform;
                holdingObject.transform.position = transform.position;
                
                // Mark the object as picked up, so nobody else can take it
                holdingObject.GetComponent<PickUppable>().isPickUppable = false;
            }
            else if (holdingObject)
            {
                // Put down the object
                holdingObject.transform.parent = null;
                
                // Check if this is near a put down node
                if (putdownTarget)
                {
                    // Position the object on the put-down node
                    holdingObject.transform.position = putdownTarget.transform.position;
                }
                
                // Mark the object as dropped, so someone else can take it
                holdingObject.GetComponent<PickUppable>().isPickUppable = true;
                
                holdingObject = null;
            }
        }
    }
}
