using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Setting : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector] public static bool SettingBool = false;

    [Header("SettingPanel")]
    public GameObject SettingPanel;
    public Toggle ControllBtn;
    public Toggle SoundBtn;
    public Button SaveBtn;
    public Button BackBtn;
    public Button ExitGameBtn;

    [Header("GlobalValue")]
    public GameObject ControllSettingObj;

    [Header("SoundSetting")]
    public GameObject SoundSettingObj;

    public static Setting Inst;

    void Awake()
    {
        Inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (SaveBtn != null)
            SaveBtn.onClick.AddListener(SaveGame);

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

            //ClearSetting();
            SettingBool = !SettingBool;
            GameMgr.Deltatime = 0.02f;
            //Time.timeScale = 1.0f;           
            //Destroy(this.gameObject);
        }

        ControllSettingObj.SetActive(ControllBtn.isOn);
        
        SoundSettingObj.SetActive(SoundBtn.isOn);

        SettingPanel.gameObject.SetActive(SettingBool);
    }

    void SaveGame()
    {
        
        //GameMgr.Deltatime = 0.02f;

        //---SettingSave

        PlayerPrefs.SetInt("Move_Forward", (int)GlobalValue.Move_Forward);
        //Debug.Log((KeyCode)PlayerPrefs.GetInt("Move_Forward"));   //W
        PlayerPrefs.SetInt("Move_Left", (int)GlobalValue.Move_Left);
        PlayerPrefs.SetInt("Move_Right", (int)GlobalValue.Move_Right);
        PlayerPrefs.SetInt("Move_Back", (int)GlobalValue.Move_Back);
        PlayerPrefs.SetInt("Peeking_Left", (int)GlobalValue.Peeking_Left);
        PlayerPrefs.SetInt("Peeking_Right", (int)GlobalValue.Peeking_Right);
        PlayerPrefs.SetInt("Sprint", (int)GlobalValue.Sprint);
        PlayerPrefs.SetInt("Sit", (int)GlobalValue.Sit);
        PlayerPrefs.SetInt("Use", (int)GlobalValue.Use);
        PlayerPrefs.SetInt("Detection", (int)GlobalValue.Detection);
        //---SettingSave

        SettingBool = !SettingBool;
        //Destroy(this.gameObject);        
    }

    void Back()
    {
    //    ClearSetting();
        SettingBool = !SettingBool;
        ControllSettingObj.GetComponent<ControllSetting>().Cancle();
        SoundSettingObj.GetComponent<SoundSetting>().Cancle();
        GameMgr.Deltatime = 0.02f;       
        //Destroy(this.gameObject);
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
