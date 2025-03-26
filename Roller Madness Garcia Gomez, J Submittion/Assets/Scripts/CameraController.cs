using UnityEngine;

public class CameraController : MonoBehaviour
{
    
    public GameObject target;
    public float XDistance = 5f, YDistance = 10f, ZDistance = 40f;
    public Camera PlayerCamera;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {

    }

    // Update is called once per frame 
    void Update()
    {
        
        Vector3 v1 = new Vector3(XDistance, 0, ZDistance);
        Vector3 wantedPosition = target.transform.position - v1;
        wantedPosition.y = YDistance;
        transform.position = wantedPosition;
        transform.LookAt(target.transform);

        
    }
}
