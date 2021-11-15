using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Twinkle_Light : MonoBehaviour
{

    private Light TwinkelLight;
    private float LightRange;
    private float currIntensity;
    private float targetIntensity;

    // Start is called before the first frame update
    void Start()
    {
        TwinkelLight = GetComponent<Light>();
        LightRange = TwinkelLight.range;
        currIntensity = TwinkelLight.intensity;
        targetIntensity = Random.Range(TwinkelLight.intensity + 6f, TwinkelLight.intensity + 10f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(targetIntensity - TwinkelLight.intensity) >= 1f)
        {
            if (targetIntensity - TwinkelLight.intensity >= 0)
                TwinkelLight.intensity += GameMgr.Deltatime * 30f;
            else
                TwinkelLight.intensity -= GameMgr.Deltatime * 30f;

            //TwinkelLight.intensity = currIntensity;
            TwinkelLight.range = TwinkelLight.intensity + LightRange;
        }
        else
            targetIntensity = Random.Range(currIntensity + 6f, currIntensity+ 10f);
    }
}
