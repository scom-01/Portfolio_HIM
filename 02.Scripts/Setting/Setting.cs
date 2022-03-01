using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Setting : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("SettingPanel")]
    public GameObject SettingPanel;
    public Toggle ControllBtn;
    public Toggle SoundBtn;
    public Button BackBtn;
    public Button ExitGameBtn;

    [Header("Controll")]
    public GameObject ControllSettingObj;

    [Header("Sound")]
    public GameObject SoundSettingObj;

    public static Setting Inst;

    void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (ExitGameBtn != null)
            ExitGameBtn.onClick.AddListener(ExitGame);


        if (BackBtn != null)
            BackBtn.onClick.AddListener(Back);

        ControllBtn.isOn = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ControllSettingObj.activeSelf)
            {
                for (int i = 0; i < ControllSettingObj.GetComponentsInChildren<CtrlKeyChange>().Length; i++)
                {
                    if (ControllSettingObj.GetComponentsInChildren<CtrlKeyChange>()[i].ClickOnOff)
                    {
                        return;
                    }
                }
                //모든 버튼이 비활성화 일 때만 //즉, 설정중에 ESC를 누르면 셋팅을 끄지말고 설정만 닫도록

                //ClearSetting();
            }

            GlobalValue.SettingBool = !GlobalValue.SettingBool;
            GlobalValue.Deltatime = 0.02f;
            //Time.timeScale = 1.0f;           
            //Destroy(this.gameObject);
        }

        ControllSettingObj.SetActive(ControllBtn.isOn);
        
        SoundSettingObj.SetActive(SoundBtn.isOn);

        SettingPanel.gameObject.SetActive(GlobalValue.SettingBool);
    }

    void Back()
    {
        ControllSettingObj.GetComponent<ControllSetting>().Cancle();
        SoundSettingObj.GetComponent<SoundSetting>().Cancle();
        GlobalValue.SettingBool = !GlobalValue.SettingBool;
    }

    void ExitGame()
    {
        //Screen.SetResolution()
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
    }
}
