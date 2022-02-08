using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllSetting : MonoBehaviour
{
    public static ArrayList ControlKeySet = new ArrayList();
    //public static KeyCode Move_Forward;
    //public static KeyCode Move_Left;
    //public static KeyCode Move_Right;
    //public static KeyCode Move_Back;
    //public static KeyCode Peeking_Left;
    //public static KeyCode Peeking_Right;
    //public static KeyCode Sprint;
    //public static KeyCode Sit;
    //public static KeyCode Use;
    //public static KeyCode Detection;

    //public static float mouseSpeed = 6.0f;  //MoveCtrl.cs 마우스x축 움직임 //MoveCam.cs 마우스y축 움직임

    [Header("Button")]
    public Button SaveBtn;
    public Button CancleBtn;
    public Button ResetBtn;

    [Header("Message")]
    public GameObject Message;
    public Button OkBtn;
    public Button NoBtn;

    SliderSetting m_mousesetting;
    // Start is called before the first frame update
    void Awake()
    {
        //추후 다형성을 위해 for문으로 바꿀예정

        GlobalValue.Move_Forward = (KeyCode)PlayerPrefs.GetInt("Move_Forward", (int)KeyCode.W);
        GlobalValue.Move_Left = (KeyCode)PlayerPrefs.GetInt("Move_Left", (int)KeyCode.A);
        GlobalValue.Move_Right = (KeyCode)PlayerPrefs.GetInt("Move_Right", (int)KeyCode.D);
        GlobalValue.Move_Back = (KeyCode)PlayerPrefs.GetInt("Move_Back", (int)KeyCode.S);
        GlobalValue.Peeking_Left = (KeyCode)PlayerPrefs.GetInt("Peeking_Left", (int)KeyCode.Q);
        GlobalValue.Peeking_Right = (KeyCode)PlayerPrefs.GetInt("Peeking_Right", (int)KeyCode.E);
        GlobalValue.Sprint = (KeyCode)PlayerPrefs.GetInt("Sprint", (int)KeyCode.LeftShift);
        GlobalValue.Sit = (KeyCode)PlayerPrefs.GetInt("Sit", (int)KeyCode.LeftControl);
        GlobalValue.Use = (KeyCode)PlayerPrefs.GetInt("Use", (int)KeyCode.F);
        GlobalValue.Detection = (KeyCode)PlayerPrefs.GetInt("Detection", (int)KeyCode.V);

        GlobalValue.mouseSpeed = PlayerPrefs.GetFloat("mouseSpeed", 5.0f);
    }
    void Start()
    {
        if (ControlKeySet != null)
        {
            ControlKeySet.Add(GlobalValue.Move_Forward);    //KeyCode
            ControlKeySet.Add(GlobalValue.Move_Left);
            ControlKeySet.Add(GlobalValue.Move_Right);
            ControlKeySet.Add(GlobalValue.Move_Back);
            ControlKeySet.Add(GlobalValue.Peeking_Left);
            ControlKeySet.Add(GlobalValue.Peeking_Right);
            ControlKeySet.Add(GlobalValue.Sprint);
            ControlKeySet.Add(GlobalValue.Sit);
            ControlKeySet.Add(GlobalValue.Use);
            ControlKeySet.Add(GlobalValue.Detection);
        }

        if (SaveBtn != null)
            SaveBtn.onClick.AddListener(Save);

        if (CancleBtn != null)
            CancleBtn.onClick.AddListener(Cancle);

        if (ResetBtn != null)
            ResetBtn.onClick.AddListener(ResetKey);

        Message.SetActive(false);

        if (Message != null)
        {
            if (OkBtn != null)
                OkBtn.onClick.AddListener(Ok);

            if (NoBtn != null)
                NoBtn.onClick.AddListener(No);
        }

        m_mousesetting = this.GetComponentInChildren<SliderSetting>();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    void Save()
    {
        #region//----------컨트롤 키 저장
        CtrlKeyChange[] CtrlKey = GetComponentsInChildren<CtrlKeyChange>();
        for (int i = 0; i < CtrlKey.Length; i++)
        {
            //설정 했던 키가 중복된 키라면
            if (CtrlKey[i].m_CurKeyState == CtrlKeyChange.KeyState.Duplicate)
            {
                //그 전에 설정되 있던 키로
                CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt(CtrlKey[i].name)).ToString();
            }

            //저장
            if (CtrlKey[i].m_CurKeyState == CtrlKeyChange.KeyState.Change)
            {
                ControlKeySet.Remove((KeyCode)PlayerPrefs.GetInt(CtrlKey[i].name)); //그 전의 KeyCode를 빼고 
                Debug.Log((KeyCode)PlayerPrefs.GetInt(CtrlKey[i].name) + " / 삭제");
                PlayerPrefs.SetInt(CtrlKey[i].name, (int)CtrlKey[i].CtrlKeyCode);   //로컬에 저장
                Debug.Log(CtrlKey[i].name + " : " + CtrlKey[i].CtrlKeyCode + "로 저장");                
                //Debug.Log((KeyCode)PlayerPrefs.GetInt(CtrlKey[i].name));
                //ControlKeySet.Add(CtrlKey[i].CtrlKeyCode);  //바꾼 KeyCode를 추가
                ControlKeySet.Insert(i, CtrlKey[i].CtrlKeyCode);
                for (int h = 0; h < ControlKeySet.Count; h++)
                {
                    Debug.Log(h+" / " +ControlKeySet[h].ToString());                    
                }
                GlobalValue.Move_Forward = (KeyCode)ControlKeySet[0];
                GlobalValue.Move_Left = (KeyCode)ControlKeySet[1];
                GlobalValue.Move_Right = (KeyCode)ControlKeySet[2];
                GlobalValue.Move_Back = (KeyCode)ControlKeySet[3];
                GlobalValue.Peeking_Left = (KeyCode)ControlKeySet[4];
                GlobalValue.Peeking_Right = (KeyCode)ControlKeySet[5];
                GlobalValue.Sprint = (KeyCode)ControlKeySet[6];
                GlobalValue.Sit = (KeyCode)ControlKeySet[7];
                GlobalValue.Use = (KeyCode)ControlKeySet[8];
                GlobalValue.Detection = (KeyCode)ControlKeySet[9];
            }
                
            //저장

            CtrlKey[i].m_CurKeyState = CtrlKeyChange.KeyState.None;
            CtrlKey[i].m_OldKeyState = CtrlKey[i].m_CurKeyState;
            //키 색 흰색으로

            CtrlKey[i].ClickOnOff = false;
            //키 설정 취소
        }
        //----------컨트롤 키 저장
        #endregion

        #region//---------마우스 셋팅 저장        
        if (m_mousesetting.m_OldValue != m_mousesetting.m_CurValue)
        {
            m_mousesetting.m_OldValue = m_mousesetting.m_CurValue;
            PlayerPrefs.SetFloat(m_mousesetting.name, m_mousesetting.m_OldValue);
        }

        #endregion
    }

    public void Cancle()
    {
        #region //----------컨트롤 키 저장 취소
        CtrlKeyChange[] CtrlKey = GetComponentsInChildren<CtrlKeyChange>();
        //키 설정중에 취소버튼을 눌렀을 때
        for (int i = 0; i < CtrlKey.Length; i++)
        {
            //Debug.Log("키 초기화");
            if (CtrlKey[i].name == "Move_Forward")
                CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt("Move_Forward", (int)KeyCode.W)).ToString();
            if (CtrlKey[i].name == "Move_Left")
                CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt("Move_Left", (int)KeyCode.A)).ToString();
            if (CtrlKey[i].name == "Move_Right")
                CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt("Move_Right", (int)KeyCode.D)).ToString();
            if (CtrlKey[i].name == "Move_Back")
                CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt("Move_Back", (int)KeyCode.S)).ToString();
            if (CtrlKey[i].name == "Peeking_Left")
                CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt("Peeking_Left", (int)KeyCode.Q)).ToString();
            if (CtrlKey[i].name == "Peeking_Right")
                CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt("Peeking_Right", (int)KeyCode.E)).ToString();
            if (CtrlKey[i].name == "Sprint")
                CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt("Sprint", (int)KeyCode.LeftShift)).ToString();
            if (CtrlKey[i].name == "Sit")
                CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt("Sit", (int)KeyCode.LeftControl)).ToString();
            if (CtrlKey[i].name == "Use")
                CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt("Use", (int)KeyCode.F)).ToString();
            if (CtrlKey[i].name == "Detection")
                CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt("Detection", (int)KeyCode.V)).ToString();
            //CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt(CtrlKey[i].name)).ToString();
            //설정 중이던 키를 원래 키로

            CtrlKey[i].m_CurKeyState = CtrlKeyChange.KeyState.None;
            CtrlKey[i].m_OldKeyState = CtrlKey[i].m_CurKeyState;
            //키 색 흰색으로
            CtrlKey[i].ClickOnOff = false;
            //키 설정 취소
        }
        //키 설정중에 취소버튼을 눌렀을 때

        GlobalValue.Move_Forward = (KeyCode)PlayerPrefs.GetInt("Move_Forward", (int)KeyCode.W);
        GlobalValue.Move_Left = (KeyCode)PlayerPrefs.GetInt("Move_Left", (int)KeyCode.A);
        GlobalValue.Move_Right = (KeyCode)PlayerPrefs.GetInt("Move_Right", (int)KeyCode.D);
        GlobalValue.Move_Back = (KeyCode)PlayerPrefs.GetInt("Move_Back", (int)KeyCode.S);
        GlobalValue.Peeking_Left = (KeyCode)PlayerPrefs.GetInt("Peeking_Left", (int)KeyCode.Q);
        GlobalValue.Peeking_Right = (KeyCode)PlayerPrefs.GetInt("Peeking_Right", (int)KeyCode.E);
        GlobalValue.Sprint = (KeyCode)PlayerPrefs.GetInt("Sprint", (int)KeyCode.LeftShift);
        GlobalValue.Sit = (KeyCode)PlayerPrefs.GetInt("Sit", (int)KeyCode.LeftControl);
        GlobalValue.Use = (KeyCode)PlayerPrefs.GetInt("Use", (int)KeyCode.F);
        GlobalValue.Detection = (KeyCode)PlayerPrefs.GetInt("Detection", (int)KeyCode.V);
        #endregion

        #region//----------마우스 셋팅 저장 취소        
        m_mousesetting.m_CurValue = m_mousesetting.m_OldValue;
        m_mousesetting.Slider.value = m_mousesetting.m_CurValue / m_mousesetting.MaxValue;        
        #endregion
    }

    void ResetKey()
    {
        Message.SetActive(true);
    }

    void Ok()
    {
        //초기화
        #region //----------컨트롤 키 저장 취소

        PlayerPrefs.SetInt("Move_Forward", (int)KeyCode.W);
        PlayerPrefs.SetInt("Move_Left", (int)KeyCode.A);
        PlayerPrefs.SetInt("Move_Right", (int)KeyCode.D);
        PlayerPrefs.SetInt("Move_Back", (int)KeyCode.S);
        PlayerPrefs.SetInt("Peeking_Left", (int)KeyCode.Q);
        PlayerPrefs.SetInt("Peeking_Right", (int)KeyCode.E);
        PlayerPrefs.SetInt("Sprint", (int)KeyCode.LeftShift);
        PlayerPrefs.SetInt("Sit", (int)KeyCode.LeftControl);
        PlayerPrefs.SetInt("Use", (int)KeyCode.F);
        PlayerPrefs.SetInt("Detection", (int)KeyCode.V);

        GlobalValue.Move_Forward = (KeyCode)PlayerPrefs.GetInt("Move_Forward", (int)KeyCode.W);
        GlobalValue.Move_Left = (KeyCode)PlayerPrefs.GetInt("Move_Left", (int)KeyCode.A);
        GlobalValue.Move_Right = (KeyCode)PlayerPrefs.GetInt("Move_Right", (int)KeyCode.D);
        GlobalValue.Move_Back = (KeyCode)PlayerPrefs.GetInt("Move_Back", (int)KeyCode.S);
        GlobalValue.Peeking_Left = (KeyCode)PlayerPrefs.GetInt("Peeking_Left", (int)KeyCode.Q);
        GlobalValue.Peeking_Right = (KeyCode)PlayerPrefs.GetInt("Peeking_Right", (int)KeyCode.E);
        GlobalValue.Sprint = (KeyCode)PlayerPrefs.GetInt("Sprint", (int)KeyCode.LeftShift);
        GlobalValue.Sit = (KeyCode)PlayerPrefs.GetInt("Sit", (int)KeyCode.LeftControl);
        GlobalValue.Use = (KeyCode)PlayerPrefs.GetInt("Use", (int)KeyCode.F);
        GlobalValue.Detection = (KeyCode)PlayerPrefs.GetInt("Detection", (int)KeyCode.V);

        if (ControlKeySet != null)
        {
            ControlKeySet.Clear();
            ControlKeySet.Add(GlobalValue.Move_Forward);
            ControlKeySet.Add(GlobalValue.Move_Left);
            ControlKeySet.Add(GlobalValue.Move_Right);
            ControlKeySet.Add(GlobalValue.Move_Back);
            ControlKeySet.Add(GlobalValue.Peeking_Left);
            ControlKeySet.Add(GlobalValue.Peeking_Right);
            ControlKeySet.Add(GlobalValue.Sprint);
            ControlKeySet.Add(GlobalValue.Sit);
            ControlKeySet.Add(GlobalValue.Use);
            ControlKeySet.Add(GlobalValue.Detection);
        }
        //초기화

        CtrlKeyChange[] CtrlKey = GetComponentsInChildren<CtrlKeyChange>();
        for (int i = 0; i < CtrlKey.Length; i++)
        {
            CtrlKey[i].CtrlKeyCode = (KeyCode)PlayerPrefs.GetInt(CtrlKey[i].name);
            CtrlKey[i].CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt(CtrlKey[i].name)).ToString();
            CtrlKey[i].m_CurKeyState = CtrlKeyChange.KeyState.None;
            CtrlKey[i].m_OldKeyState = CtrlKey[i].m_CurKeyState;
        }
        #endregion

        #region //----------마우스 셋팅 저장 취소
        PlayerPrefs.SetFloat("mouseSpeed", 5.0f);        
        m_mousesetting.m_OldValue = PlayerPrefs.GetFloat(m_mousesetting.name);
        m_mousesetting.m_CurValue = m_mousesetting.m_OldValue;
        m_mousesetting.Slider.value = m_mousesetting.m_CurValue / m_mousesetting.MaxValue;
        #endregion
        Message.SetActive(false);
    }

    void No()
    {
        Message.SetActive(false);
    }
}
