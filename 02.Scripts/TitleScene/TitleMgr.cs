using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class TitleMgr : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("TitlePanel")]
    public GameObject TitlePanel;
    public Button GameStartBtn;
    public Button m_SettingBtn;

    [Header("Cine")]
    public PlayableDirector P_director;
    public TimelineAsset timeline;
    AudioSource aud;

    //[HideInInspector] public static bool SettingBool = false;

    void Awake()
    {
        QualitySettings.vSyncCount = 0;     //수직동기화 해제
        Application.targetFrameRate = 60;
    }
    // Start is called before the first frame update
    void Start()
    {
        //SetUp();
    }

    void OnEnable()
    {        
        SetUp();
    }

    void StartGame()
    {
        P_director.Play();
        StartCoroutine(CameraMove());
    }

    IEnumerator CameraMove()
    {        
        while(true)
        {
            if (P_director.time >= 4)
            {
                LoadMgr.LoadScene("InGameScene");                
                //SceneManager.LoadScene("InGameScene");
                yield break;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        aud.volume = GlobalValue.BGM_Value * GlobalValue.BGM_Bool;
        TitlePanel.gameObject.SetActive(!GlobalValue.SettingBool);
    }    

    void SetUp()
    {
        aud = GetComponent<AudioSource>();

        GlobalValue.SettingBool = false;
        TitlePanel.gameObject.SetActive(!GlobalValue.SettingBool);

        //다시시작 할 시
        Cursor.visible = true;  
        Cursor.lockState = CursorLockMode.Confined;

        if (TitlePanel != null)
        {
            if (m_SettingBtn != null)
                m_SettingBtn.onClick.AddListener(() =>
                {
                    SettingKey();
                    GlobalValue.SettingBool = !GlobalValue.SettingBool;
                });

            if (GameStartBtn != null)
                GameStartBtn.onClick.AddListener(StartGame);

        }

        List<Button> Go = new List<Button>();
        Go.Add(GameStartBtn);
        Go.Add(m_SettingBtn);

        //Button EventTrigger
        for (int i = 0; i < Go.Count; i++)
        {
            EventTrigger a_trigger = Go[i].GetComponent<EventTrigger>(); // Inspector에서 GameObject.Find("Button"); 에 꼭 AddComponent--> EventTrigger 가 되어 있어야 한다.
            EventTrigger.Entry a_entry = new EventTrigger.Entry();
            a_entry.eventID = EventTriggerType.PointerEnter;
            a_entry.callback.AddListener((data) => { OnPointerEnter((PointerEventData)data); });
            a_trigger.triggers.Add(a_entry);

            a_entry = new EventTrigger.Entry();
            a_entry.eventID = EventTriggerType.PointerExit;
            a_entry.callback.AddListener((data) => { OnPointerExit((PointerEventData)data); });
            a_trigger.triggers.Add(a_entry);
        }
    }

    void SettingKey()
    {
        if (GlobalValue.SettingBool)
            return;

        m_SettingBtn.GetComponentInChildren<Text>().color = new Color32(132, 132, 132, 255);    
        //클릭 후 다시 회색으로 만들어서 Setting에서 다시 돌아왔을 때 흰색으로 보이지 않도록
    }

    public void PointEnter(PointerEventData eventdata)
    {
        eventdata.pointerEnter.GetComponent<Text>().color = new Color32(246, 246, 246, 255);
    }

    public void OnPointerEnter(PointerEventData eventdata)
    {
        eventdata.pointerEnter.GetComponent<Text>().color= new Color32(246, 246, 246, 255);

    }

    public void OnPointerExit(PointerEventData eventdata)
    {
        eventdata.pointerEnter.GetComponent<Text>().color = new Color32(132, 132, 132, 255);
    }
}
