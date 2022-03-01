using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState
{
    Starting,
    Playing,
    Die,
    End
}

public class GameMgr : MonoBehaviour
{
    public GameObject Pointer;      //Mouse Pointer

    public GameObject StoryObj;     //HelpText UI
    public GameObject EndObj;       //Thanks for Playing (Text)
    public GameObject DieObj;       //You Die (Text)

    public Image Hp_Img;

    public GameObject EnemyObj;
    Vector3 EnemySpawnPos = new Vector3(13, 7.45f, 5.75f);
    bool Spawn = false;

    public Text[] Txt = new Text[2];

    public AudioClip[] BGM_Clip;
    public AudioSource aud;
    float Delay = 0.0f;                 //Enter키로 설명을 넘길 시 빠르게 두번 넘어가지않도록 하는 변수
    int HelpIdx = 0;                    //HelpText Idx
    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalValue.PlayerInvenList.Count > 0 && !Spawn)
        {
            Instantiate(EnemyObj).transform.localPosition = EnemySpawnPos;
            Spawn = true;
        }
            

        if (GlobalValue.SettingBool)            
            GlobalValue.Deltatime = 0.0f;    
        else
            GlobalValue.Deltatime = 0.02f;      
        

        Pointer.SetActive(!GlobalValue.SettingBool);     
        
        if(GlobalValue.g_GameState == GameState.Starting || GlobalValue.g_GameState == GameState.End)
        {
            GlobalValue.Deltatime = 0;
        }        

        if (Delay > 0.0f)
        {
            Delay -= Time.deltaTime;
            if (Delay <= 0.0f)
                Delay = 0.0f;
        }

        

        if (Input.GetKeyDown(KeyCode.X))
        {
            GlobalValue.g_GameState = GameState.End;
        }

        _Enter();
        BGM_Controll();
    }

    void SetUp()
    {
        DieObj.SetActive(false);            
        QualitySettings.vSyncCount = 0;     //Scene Setup
        Application.targetFrameRate = 60;  
        Pointer.SetActive(true);

        if (EndObj != null)
        {
            EndObj.SetActive(false);
        }

        if (DieObj != null)
        {
            DieObj.SetActive(false);
        }

        GlobalValue.g_GameState = GameState.Starting;
        GlobalValue.PlayerInvenList = new List<string>();    //열쇠정보를 저장하는 리스트

        Delay = 0.0f;                       
        HelpIdx = 0;
        Spawn = false;
    }

    void _Enter()
    {
        if (!(Input.GetKeyDown(KeyCode.Return) && Delay == 0.0f && GlobalValue.g_GameState == GameState.Starting))
        {
            return;
        }

        HelpIdx++;
        if(HelpIdx > 1)
        {
            StoryObj.gameObject.SetActive(false);
            GlobalValue.Deltatime = 0.02f;            
            GlobalValue.g_GameState = GameState.Playing;
            aud.Stop();
        }
        Txt[0].gameObject.SetActive(false);
        Txt[1].gameObject.SetActive(true);

        Delay = 1.0f;
    }

    void BGM_Controll()
    {
        if (GlobalValue.g_GameState == GameState.Starting)
        {
            if (aud.clip != BGM_Clip[0])
            {
                aud.clip = BGM_Clip[0];
                aud.Play();
            }
            aud.volume = GlobalValue.BGM_Value * GlobalValue.BGM_Bool;
            return;
        }
        else if (GlobalValue.g_GameState == GameState.End)
        {
            if (aud.clip != BGM_Clip[2])
            {
                aud.clip = BGM_Clip[2];
                aud.Play();
                StartCoroutine(ThxPlay());
            }
            aud.volume = GlobalValue.BGM_Value * GlobalValue.BGM_Bool;
        }
        else if(GlobalValue.g_GameState == GameState.Die)
        {
            if (aud.clip != BGM_Clip[2])
            {
                aud.clip = BGM_Clip[2];
                aud.Play();
                StartCoroutine(You_Die());
            }
            aud.volume = GlobalValue.BGM_Value * GlobalValue.BGM_Bool;
        }

        if (FindObjectOfType<EnemyCtrl>() == null)
            return;

        //Enemy가 쫓아올 시 사운드
        if (EnemyCtrl.Inst.CurAnimState == AnimState.trace)
        {
            if(!aud.isPlaying)
            {
                aud.clip = BGM_Clip[1];
                aud.Play();
            }
            aud.volume = GlobalValue.SE_Value * GlobalValue.SE_Bool;
        }
        else
        {
            if (aud.clip == BGM_Clip[1])
            {                
                aud.Stop();
            }
        }
    }

    IEnumerator ThxPlay()
    {
        if (EndObj != null)
        {
            EndObj.SetActive(true);
        }

        yield return new WaitForSeconds(12.0f);
        yield return StartCoroutine(Retry());
    }

    IEnumerator You_Die()
    {
        if (DieObj != null)
        {
            DieObj.SetActive(true);
        }

        yield return new WaitForSeconds(3.0f);
        yield return StartCoroutine(Retry());

    }

    IEnumerator Retry()
    {
        Destroy(GameObject.Find("DontDestroy").gameObject); //씬으로 전환 시 중복 생성을 막기위해 기존 Setting파괴
        SceneManager.LoadScene("TitleScene");
        yield return 0;
    }

}
