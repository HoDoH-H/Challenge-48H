using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public CameraPosition cameraPosition;

    private void Update()
    {
        transform.position = cameraPosition.transform.position;
    }

    private void OnValidate()
    {
        if(cameraPosition != null)
            transform.position = cameraPosition.transform.position;
    }
}
