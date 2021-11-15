using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Detection : MonoBehaviour
{
    GameObject Detection_Detection_Circle;
    GameObject Detection_Pivot;
    Image Detection_EarImg;
    [HideInInspector] public bool Detection_State = false;

    void Awake()
    {
        Detection_Detection_Circle = GameObject.Find("Sound_Detection_Circle");
        Detection_State = Detection_Detection_Circle.GetComponent<Image>().enabled;
        Detection_EarImg = Detection_Detection_Circle.GetComponent<Image>();
        //Debug.Log(Detection_EarImg.name.ToString());
        Detection_Pivot = Detection_Detection_Circle.GetComponent<SoundDetection>().Detection_Pivot;
        //Debug.Log(Detection_Pivot.name.ToString());
        //Debug.Log(Detection_State.ToString());
        //Debug.Log(Detection_Detection_Circle.GetComponent<SoundDetection>().Detection_Pivot.name.ToString());
        //Detection_Detection_Circle.GetComponent<SoundDetection>().Detection_Pivot.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Detection_Pivot != null)
            Detection_Pivot.SetActive(Detection_State);
        if (Detection_EarImg != null)
            Detection_EarImg.enabled = Detection_State;

        if (Input.GetKeyDown(ControllSetting.Detection))// KeyCode.V))
        {
            Detection_State = !Detection_State;
        }
    }
}
