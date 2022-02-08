using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionRay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Detection();
    }

    void Detection()
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = transform.forward;
        Debug.DrawRay(ray.origin, ray.direction * 10.0f, Color.red);
    }
}
