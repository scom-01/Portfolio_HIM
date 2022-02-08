using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash_ambient_Materials : MonoBehaviour
{
    public MeshRenderer MR;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalValue.Blink_Alpha <= 0.0f)
        {
            MR.enabled = false;
        }
        else
        {
            MR.enabled = true;
        }
    }
}
