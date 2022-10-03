using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // Make the sprite face the camera. This doesn't account for slight differences in perspective views, but good enough for now
        
        float cameraFacing = mainCamera.transform.eulerAngles.y % 360;
        float parentFacing = transform.parent.eulerAngles.y % 360;
        Vector3 newRotation = transform.eulerAngles;
        
        // Clip the sprite rotation to either exactly left or right in relation to the camera
        newRotation.y = (parentFacing > 180 ? 0 : 180) + cameraFacing;
        
        // Copy the camera rotation to appear to "stand up". Depending on the Y rotation, this might need to be inversed
        newRotation.x = mainCamera.transform.eulerAngles.x * (parentFacing > 180 ? 1 : -1);

        transform.eulerAngles = newRotation;
    }
}
