using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    public bool ZoomActive;
    public Vector3[] target;
    public Camera Cam;
    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        Cam = Camera.main; 
    }

    // Update is called once per frame
    void Update()
    {
        if(ZoomActive)
        {
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, 3, Speed);
            Cam.transform.position = Vector3.Lerp(Cam.transform.position, target[1], Speed);
        }
        else
        {
            Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, 5, Speed);
            Cam.transform.position = Vector3.Lerp(Cam.transform.position, target[0], Speed);
        }
    }
}
