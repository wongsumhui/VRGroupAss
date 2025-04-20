using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Camera cameraToFace;

    void Update()
    {
        if (cameraToFace == null)
        {
            cameraToFace = Camera.main; // Default to the main camera if none is assigned
        }

        // Make the canvas face the camera
        transform.LookAt(cameraToFace.transform);

        // Lock rotation to prevent tilting
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
    }
}
