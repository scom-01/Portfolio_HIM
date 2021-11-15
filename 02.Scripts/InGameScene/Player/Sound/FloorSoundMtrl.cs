using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorSoundMtrl : MonoBehaviour
{
    public FloorMtrl Mtrl;

    Ray ray;
    RaycastHit hit;

    void Start()
    {
        
    }

    void Update()
    {
        Debug.DrawRay(this.transform.position + this.transform.up * 0.1f, this.transform.up * -1f,Color.blue);
        ray.origin = this.transform.position;
        ray.direction = this.transform.up * -1f;
        if (Physics.Raycast(ray, out hit, 1)) 
        {
            //Debug.Log(hit.transform.name);
            if(hit.transform.gameObject.name.Contains("Ground"))
            {
                Mtrl = FloorMtrl.Grass;
            }
            else if (hit.transform.gameObject.name.Contains("Porch_00"))
            {
                Mtrl = FloorMtrl.Wood;
            }
            else if (hit.transform.gameObject.name.Contains("_stairs"))
            {
                Mtrl = FloorMtrl.Wood;
            }
            else if (hit.transform.gameObject.name.Contains("F_001"))
            {
                Mtrl = FloorMtrl.MetalV2;
            }
            else if (hit.transform.gameObject.name.Contains("F_002"))
            {
                Mtrl = FloorMtrl.MetalV1;
            }
            else if (hit.transform.gameObject.name.Contains("F_003"))
            {
                Mtrl = FloorMtrl.Tile;
            }
            else if (hit.transform.gameObject.name.Contains("F_004"))
            {
                Mtrl = FloorMtrl.Tile;
            }
        }
    }
}
