using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderSetting : MonoBehaviour
{
    public Slider Slider;
    public Text SliderText;
    public Text Text;

    public Button UP_btn;
    public Button DOWN_btn;

    public float PrefsValue;

    [HideInInspector] public float m_CurValue;
    [HideInInspector] public float m_OldValue;
    public float MaxValue;
    
    // Start is called before the first frame update
    void Start()
    {
        //MaxValue = 10.0f;
        m_CurValue = PlayerPrefs.GetFloat(this.name, PrefsValue);
        m_OldValue = m_CurValue;
        Text.text = m_CurValue.ToString("F1");
        Slider.value = m_CurValue / MaxValue;

        if (UP_btn != null)
            UP_btn.onClick.AddListener(()=>
            {
                m_CurValue += MaxValue/ 100f;
                Slider.value = m_CurValue / MaxValue;
            });

        if (DOWN_btn != null)
            DOWN_btn.onClick.AddListener(()=>
            {
                m_CurValue -= MaxValue / 100f;
                Slider.value = m_CurValue / MaxValue;
            });
    }

    // Update is called once per frame
    void Update()
    {
        if (m_OldValue != m_CurValue)
            SliderText.color = new Color32(0, 255, 0, 255);
        else
            SliderText.color = new Color32(255, 255, 255, 255);


        SliderText.text = (Slider.value * 10.0f).ToString("F1");
        Text.text = SliderText.text;

        m_CurValue = Slider.value * MaxValue;

        if (Slider.value <= 0.01f)
            Slider.value = 0.01f;

        //Debug.Log(m_CurmouseSpeed);
    }
}
