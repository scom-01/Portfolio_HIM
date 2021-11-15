using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CtrlKeyChange : MonoBehaviour
{
    public Button CtrlKey;
    [HideInInspector] public bool ClickOnOff = false;
    public KeyCode CtrlKeyCode;
    [HideInInspector] public KeyState m_CurKeyState;
    [HideInInspector] public KeyState m_OldKeyState = KeyState.None;
    public enum KeyState
    {
        None,
        Clicked,
        Change,
        Duplicate
    }

    // Start is called before the first frame update
    void Start()
    {
        ClickOnOff = false;

        if (CtrlKey != null)
        {
            CtrlKey.onClick.AddListener(KeyChange);
            CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt(this.name, (int)CtrlKeyCode)).ToString();            
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_CurKeyState == KeyState.None)
            CtrlKey.GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255); //변화없는 키
        else if (m_CurKeyState == KeyState.Clicked)
            CtrlKey.GetComponentInChildren<Text>().color = new Color32(100, 100, 100, 255); //눌린 키
        else if (m_CurKeyState == KeyState.Change)
            CtrlKey.GetComponentInChildren<Text>().color = new Color32(0, 255, 0, 255); //변경된 키
        else if (m_CurKeyState == KeyState.Duplicate)
            CtrlKey.GetComponentInChildren<Text>().color = new Color32(255, 0, 0, 255); //중복 키
        #region------뻘짓
        //if (!ClickOnOff)
        //    return;

        //if (Input.anyKeyDown)
        //{            
        //    string ChangeKey = Input.inputString.ToUpper();
        //    //CtrlKeyCode = ChangeKey;
        //    Debug.Log(ChangeKey);
        //    if (ChangeKey == "")
        //    {

        //        ClickOnOff = false;
        //        CtrlKey.GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
        //        return;
        //    }                

        //    if(Input.GetKeyDown(KeyCode.Return))
        //    {
        //        ClickOnOff = false;
        //        CtrlKey.GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
        //        return;
        //    }

        //    //선택 취소
        //    if(Input.GetKeyDown(KeyCode.Escape))
        //    {
        //        ClickOnOff = false;
        //        CtrlKey.GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
        //        return;
        //    }

        //    if (ChangeKey != "" && ChangeKey != null)
        //    {
        //        if (ControllSetting.ControlKeySet == null)
        //            return;

        //        for (int i = 0; i < ControllSetting.ControlKeySet.Count; i++)
        //        {

        //            if (ChangeKey == ((KeyCode)PlayerPrefs.GetInt(this.name)).ToString())   //기존과 같을 때
        //            {
        //                CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt(this.name)).ToString();
        //                //다른 키를 눌렀다가 다시 원래키로 설정할 수도있기에 ChangeKey로 설정해선 안된다.
        //                Debug.Log("전과 같음");
        //                break;
        //            }
        //            else if(ChangeKey == ControllSetting.ControlKeySet[i].ToString())
        //            {
        //                CtrlKey.GetComponentInChildren<Text>().text = ChangeKey;
        //                CtrlKey.GetComponentInChildren<Text>().color = new Color32(255, 0, 0, 255);
        //                Debug.Log("중복");
        //                break;
        //            }                     
        //            CtrlKey.GetComponentInChildren<Text>().text = ChangeKey;


        //        }

        //        if (CtrlKey.GetComponentInChildren<Text>().color == new Color32(255, 0, 0, 255))
        //        {
        //            //중복된 키가 있을 때
        //            Debug.Log("중복");
        //            ClickOnOff = false;
        //        }
        //        else
        //        {
        //            //중복된 키가 없을 때
        //            Debug.Log("노 중복");
        //            CtrlKey.GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
        //            ClickOnOff = false;
        //        }
        //    }
        //}
        #endregion
    }

    void KeyChange()
    {
        if (m_CurKeyState == KeyState.Clicked)
            return;
        ClickOnOff = true;
        
        #region----뻘짓2
        //키 입력받는 뭐시기
        //if (Input.anyKeyDown)
        //{
        //    string ChangeKey = Input.inputString.ToUpper();

        //    if (ChangeKey != "" && ChangeKey != null)
        //    {
        //        for (int i = 0; i < ControllSetting.ControlKeySet.Count; i++)
        //        {
        //            if (ChangeKey == ControllSetting.ControlKeySet[i].ToString())
        //            {
        //                CtrlKey.GetComponentInChildren<Text>().text = ChangeKey;
        //                CtrlKey.GetComponentInChildren<Text>().color = new Color32(255, 0, 0, 255);
        //                break;
        //            }
        //            CtrlKey.GetComponentInChildren<Text>().text = ChangeKey;
        //        }

        //        if(CtrlKey.GetComponentInChildren<Text>().color == new Color32(255, 0, 0, 255))
        //        {
        //            //중복된 키가 있을 때
        //            Debug.Log("중복");
        //            ClickOnOff = false;
        //        }
        //        else
        //        {
        //            //중복된 키가 없을 때
        //            Debug.Log("노 중복");
        //            ClickOnOff = false;
        //        }
        //    }
        //}

        //키 입력받는 뭐시기

        //if(Input.GetMouseButtonDown(0))
        //{
        //    ClickOnOff = false;
        //    Debug.Log(ClickOnOff);
        //}

        //if (Input.GetKeyDown(KeyCode.Return))
        //{
        //    ClickOnOff = false;
        //}
        //ClickOnOff = false;
        #endregion
    }

    public void OnGUI()
    {        
        if (!ClickOnOff)
            return;

        m_CurKeyState = KeyState.Clicked;

        //클릭된 상태
        Event e = Event.current;
        if (Input.anyKey)   //왜인지 Input.anyKeyDown은 여러번 받고, Input.anyKey는 한 번만 막는다.
        {
            ClickOnOff = false; //한 번만 입력받기 위해
            KeyCode K = e.keyCode;
            if (K == KeyCode.None || K == KeyCode.Escape || K == KeyCode.Return)
            {
                if (m_CurKeyState == KeyState.Clicked)
                    m_CurKeyState = m_OldKeyState;
                return;
            }

            #region//---------------현재 설정된 키중에 겹치는 키가 있는지 확인
            for (int k = 0; k < this.transform.parent.GetComponentsInChildren<CtrlKeyChange>().Length; k++)
            {
                //입력한 키와 현재 설정된 키가 같으면
                //ex) A키를 H로 바꾸고 B키도 H로 바꿀 때 즉 한 번에 여러키를 바꾸려고 할 때
                if (K.ToString() == this.transform.parent.GetComponentsInChildren<CtrlKeyChange>()[k].CtrlKey.GetComponentInChildren<Text>().text)
                {
                    //자기 자신을 찾았을 때 건너뜀
                    if (CtrlKey == this.transform.parent.GetComponentsInChildren<CtrlKeyChange>()[k].CtrlKey)
                    {
                        continue;
                    }
                    //여러개를 동시에 바꿀 때 겹치는 것이 있으면

                    //중복일 때 자신의 키를 붉은 색으로 바꾸고 break
                    m_CurKeyState = KeyState.Duplicate;
                    m_OldKeyState = m_CurKeyState;
                    break;
                    //중복일 때 자신의 키를 붉은 색으로 바꾸고 break
                }
            }
            //---------------현재 설정된 키중에 겹치는 키가 있는지 확인
            #endregion

            #region//---------------현재 PlayerPrefs에 저장된 키중에 겹치는 키가 있는지 확인
            for (int i = 0; i < ControllSetting.ControlKeySet.Count; i++)
            {
                //입력한 키와 PlayerPrefs에 같은 기능을 하는 키로 저장되어 있으면
                if (K == ((KeyCode)PlayerPrefs.GetInt(this.name)))   //기존과 같을 때
                {
                    CtrlKey.GetComponentInChildren<Text>().text = ((KeyCode)PlayerPrefs.GetInt(this.name)).ToString();
                    //다른 키를 눌렀다가 다시 원래키로 설정할 수도있기에 K 설정해선 안된다.
                    m_CurKeyState = KeyState.None;
                    m_OldKeyState = m_CurKeyState;
                    break;
                }
                else if (K.ToString() == ControllSetting.ControlKeySet[i].ToString()) //입력한 키와 ControlKeySet에 같은 키로 저장되어있으면 
                                                                                      //즉, 같은 기능은 아니지만 같은 키가 있다면
                {
                    //기존에 저장된 키와 PlayerPrefs에 저장된 키가 같다면
                    if (CtrlKeyCode.ToString() == ControllSetting.ControlKeySet[i].ToString())
                    {
                        m_CurKeyState = KeyState.None;
                        m_OldKeyState = m_CurKeyState;
                        break;
                    }
                    CtrlKey.GetComponentInChildren<Text>().text = K.ToString();
                    m_CurKeyState = KeyState.Duplicate;
                    m_OldKeyState = m_CurKeyState;
 
                    break;
                }
                //else if (K.ToString() == ControllSetting.FindObjectsOfType<CtrlKeyChange>())
                CtrlKey.GetComponentInChildren<Text>().text = K.ToString();
                m_CurKeyState = KeyState.Change;
                m_OldKeyState = m_CurKeyState;
            }
            //---------------현재 PlayerPrefs에 저장된 키중에 겹치는 키가 있는지 확인
            #endregion

            if(m_CurKeyState == KeyState.Change)
            {
                ClickOnOff = false;
                //현재 키코드
                CtrlKeyCode = K;
                //현재 키코드

                //키를 변경하면 녹색으로
                m_CurKeyState = KeyState.Change;
                m_OldKeyState = m_CurKeyState;
                //키를 변경하면 녹색으로
            }
            else
            {
                ClickOnOff = false;
                return;
            }
            
        }
    }
}
