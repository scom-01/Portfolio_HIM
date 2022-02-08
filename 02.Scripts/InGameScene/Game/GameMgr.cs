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
    public static float Deltatime = 0.02f;
    
    public GameObject Pointer;

    public GameObject StoryObj;
    public GameObject EndObj;
    public GameObject DieObj;

    public Image Hp_Img;

    public GameObject EnemyObj;
    Vector3 EnemySpawnPos = new Vector3(13, 7.45f, 5.75f);
    bool Spawn = false;

    public Text[] Txt = new Text[2];

    public AudioClip[] BGM_Clip;
    public AudioSource aud;
    float Delay = 0.0f;
    int HelpIdx = 0;
    // Start is called before the first frame update
    void Start()
    {
        DieObj.SetActive(false);
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        Pointer.SetActive(true);

        Delay = 0.0f;
        HelpIdx = 0;
        Spawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalValue.PlayerInvenList.Count > 0 && !Spawn)
        {
            Instantiate(EnemyObj).transform.localPosition = EnemySpawnPos;
            Spawn = true;
        }
            

        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        if (Setting.SettingBool)            
            Deltatime = 0.0f;    
        else    
            GameMgr.Deltatime = 0.02f;            
        //}

        Pointer.SetActive(!Setting.SettingBool);     
        
        if(GlobalValue.g_GameState == GameState.Starting || GlobalValue.g_GameState == GameState.End)
        {
            GameMgr.Deltatime = 0;
        }        

        if (Delay > 0.0f)
        {
            Delay -= Time.deltaTime;
            if (Delay <= 0.0f)
                Delay = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.Return) && Delay == 0.0f && GlobalValue.g_GameState == GameState.Starting)
        {
            _Enter();
            Delay = 1.0f;
        }

        BGM_Controll();
    }

    void _Enter()
    {
        HelpIdx++;
        if(HelpIdx > 1)
        {
            StoryObj.gameObject.SetActive(false);
            GameMgr.Deltatime = 0.02f;            
            GlobalValue.g_GameState = GameState.Playing;
            aud.Stop();
        }
        Txt[0].gameObject.SetActive(false);
        Txt[1].gameObject.SetActive(true);
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
            EndObj.SetActive(true);
        yield return new WaitForSeconds(10f);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 어플리케이션 종료
#endif
        yield break;
    }

    IEnumerator You_Die()
    {
        if (DieObj != null)
            DieObj.SetActive(true);
        yield return StartCoroutine(Retry());

    }

    IEnumerator Retry()
    {
        Destroy(EnemyCtrl.Inst.gameObject);
        GlobalValue.PlayerInvenList.Clear();
        GlobalValue.g_GameState = GameState.Starting;
        yield return new WaitForSeconds(3.0f);
        SceneManager.LoadScene("InGameScene");            
    }

}
