using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float damping = 0.5f, xMin = -2.5f, xMax = 19.5f, yMin = -2f, yMax = 22f;
    Vector3 velocity = Vector3.zero;
    [SerializeField]
    private Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update(){
        Vector3 targetPos = target.position + cameraOffset;
        Vector3 clampedPos = new Vector3(Mathf.Clamp(targetPos.x, xMin, xMax), Mathf.Clamp(targetPos.y, yMin, yMax), targetPos.z);
        Vector3 smoothPos = Vector3.SmoothDamp(transform.position, clampedPos, ref velocity, damping);
    
        transform.position = smoothPos;
    }
}
