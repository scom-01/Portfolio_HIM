using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flash_Gage : MonoBehaviour
{
    [SerializeField]
    private Image Gage_Img;
    [SerializeField]
    private Text Gage_Text;
    
    private bool Blink_OnOff = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //게이지 연동
        Gage_Img.fillAmount = GlobalValue.FlashGage / 100.0f;
        //게이지 연동
        Gage_Img.color = Img_Color();

        Gage_Text.text = string.Format("{0:F0}%", GlobalValue.FlashGage);   //소수점 표시 없음


    }

    Color Img_Color()
    {
        Color RGB = Color.green;

        if (GlobalValue.FlashGage <= 70.0f)
        {
            RGB = Color.yellow;
            if (GlobalValue.FlashGage <= 30.0f)
            {
                RGB = Color.red;
                if (GlobalValue.FlashGage <= 20.0f)
                {
                    Blink(ref RGB);
                }
                else
                {
                    GlobalValue.Blink_Alpha = 0.5f;
                }
            }
        }

        RGB.a = GlobalValue.Blink_Alpha;
        return RGB;
    }

    void Blink(ref Color RGB)   //Color의 알파값을 조절하여 깜빡이도록
    {
        if (GlobalValue.Blink_Alpha >= 0.5f)
        {
            Blink_OnOff = true;
        }
        else if (GlobalValue.Blink_Alpha <= 0.0f)
        {
            Blink_OnOff = false;
        }

        if (Blink_OnOff)
        {
            GlobalValue.Blink_Alpha -= GameMgr.Deltatime;
        }
        else
        {
            GlobalValue.Blink_Alpha += GameMgr.Deltatime;
        }
    }
}
