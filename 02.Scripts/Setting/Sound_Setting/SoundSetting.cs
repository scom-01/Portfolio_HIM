using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private Slider BGM_Slider;
    [SerializeField] private Toggle BGM_OnOff;
    [SerializeField] private SliderSetting m_BGM;

    [SerializeField] private Slider SE_Slider;
    [SerializeField] private Toggle SE_OnOff;
    [SerializeField] private SliderSetting m_SE;

    [Header("Button")]
    [SerializeField] private Button SaveBtn;
    [SerializeField] private Button CancleBtn;
    [SerializeField] private Button ResetBtn;

    [Header("Message")]
    [SerializeField] private GameObject Message;
    [SerializeField] private Button OkBtn;
    [SerializeField] private Button NoBtn;

    
    
    void Awake()
    {
        GlobalValue.BGM_Value = PlayerPrefs.GetFloat("BGM", 0.5f);
        GlobalValue.BGM_Bool = PlayerPrefs.GetInt("BGM_Bool", 1);
        GlobalValue.SE_Value = PlayerPrefs.GetFloat("SE", 0.5f);
        GlobalValue.SE_Bool = PlayerPrefs.GetInt("SE_Bool", 1);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SaveBtn != null)
            SaveBtn.onClick.AddListener(Save);

        if (CancleBtn != null)
            CancleBtn.onClick.AddListener(Cancle);

        if (ResetBtn != null)
            ResetBtn.onClick.AddListener(ResetKey);

        if(Message != null)
            Message.SetActive(false);

        if (Message != null)
        {
            if (OkBtn != null)
                OkBtn.onClick.AddListener(Ok);

            if (NoBtn != null)
                NoBtn.onClick.AddListener(No);
        }

        BGM_OnOff.isOn = GlobalValue.BGM_Bool == 0 ? true : false;
        SE_OnOff.isOn = GlobalValue.SE_Bool == 0 ? true : false;
    }

    // Update is called once per frame
    void Update()
    {
        Setting();
    }

    void Setting()
    {
        //설정값에 따른 사운드 On Off
        if (BGM_OnOff.isOn)
            GlobalValue.BGM_Bool = 0;
        else
            GlobalValue.BGM_Bool = 1;

        if (SE_OnOff.isOn)
            GlobalValue.SE_Bool = 0;
        else
            GlobalValue.SE_Bool = 1;

        GlobalValue.BGM_Value = m_BGM.m_CurValue;
        GlobalValue.SE_Value = m_SE.m_CurValue;
    }

    void Save()
    {
        #region//----------저장
        //BGM 저장        
        if(m_BGM.m_OldValue != m_BGM.m_CurValue)
        {
            m_BGM.m_OldValue = m_BGM.m_CurValue;
            PlayerPrefs.SetFloat("BGM", m_BGM.m_OldValue/ m_BGM.MaxValue);
        }

        PlayerPrefs.SetInt("BGM_Bool", BGM_OnOff.isOn == false ? 1 : 0);
        //BGM 저장

        //SE 저장        
        if (m_SE.m_OldValue != m_SE.m_CurValue)
        {
            m_SE.m_OldValue = m_SE.m_CurValue;
            PlayerPrefs.SetFloat("SE", m_SE.m_OldValue/ m_SE.MaxValue);
        }

        PlayerPrefs.SetInt("SE_Bool", SE_OnOff.isOn == false ? 1 : 0);
        //SE 저장
        #endregion
    }

    public void Cancle()
    {
        #region//----------셋팅 저장 취소        
        m_BGM.m_CurValue = m_BGM.m_OldValue;
        m_BGM.Slider.value = m_BGM.m_CurValue / m_BGM.MaxValue;
                
        m_SE.m_CurValue = m_SE.m_OldValue;
        m_SE.Slider.value = m_SE.m_CurValue / m_SE.MaxValue;

        BGM_OnOff.isOn = PlayerPrefs.GetInt("BGM_Bool", BGM_OnOff.isOn == false ? 1 : 0) == 1 ? false : true;
        SE_OnOff.isOn = PlayerPrefs.GetInt("SE_Bool", SE_OnOff.isOn == false ? 1 : 0) == 1 ? false : true;
        #endregion
    }

    void ResetKey()
    {
        Message.SetActive(true);
    }

    void Ok()
    {
        //초기화
        #region //----------마우스  저장 취소
        PlayerPrefs.SetFloat("BGM", 0.5f);        
        m_BGM.m_OldValue = PlayerPrefs.GetFloat(m_BGM.name) * m_BGM.MaxValue;
        m_BGM.m_CurValue = m_BGM.m_OldValue;
        m_BGM.Slider.value = m_BGM.m_CurValue / m_BGM.MaxValue;

        PlayerPrefs.SetInt("BGM_Bool", 1);
        BGM_OnOff.isOn = false;

        PlayerPrefs.SetFloat("SE", 0.5f);
        m_SE.m_OldValue = PlayerPrefs.GetFloat(m_SE.name) * m_SE.MaxValue;
        m_SE.m_CurValue = m_SE.m_OldValue;
        m_SE.Slider.value = m_SE.m_CurValue / m_SE.MaxValue;

        PlayerPrefs.SetInt("SE_Bool", 1);
        SE_OnOff.isOn = false;
        #endregion
        Message.SetActive(false);
    }

    void No()
    {
        Message.SetActive(false);
    }
}
