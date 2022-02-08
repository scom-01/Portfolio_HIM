using UnityEngine;
using System.Collections;

public class OpenDoor : MonoBehaviour
{

    public float smooth = 2.0f;
    public float DoorOpenAngle = 90.0f;

    private Vector3 defaultRot;
    private Vector3 openRot;
    [HideInInspector]public bool open;
    private bool enter;

    public GameObject Key;
    public AudioClip audioclip;
    AudioSource aud;
    // Use this for initialization
    void Start()
    {
        defaultRot = transform.eulerAngles;
        openRot = new Vector3(defaultRot.x, defaultRot.y + DoorOpenAngle, defaultRot.z);
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (open)
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, openRot, GameMgr.Deltatime * smooth);
        }
        else
        {
            transform.eulerAngles = Vector3.Slerp(transform.eulerAngles, defaultRot, GameMgr.Deltatime * smooth);
        }


        if (Input.GetKeyDown(GlobalValue.Use) && enter)
        {
            if(Key != null) //키가 필요하다면
            {
                if(GlobalValue.PlayerInvenList !=null)
                {
                    for (int i = 0; i < GlobalValue.PlayerInvenList.Count; i++)
                    {
                        if (GlobalValue.PlayerInvenList[i].Contains(Key.name))    //플레이어가 키가 있는지 확인
                        {
                            open = !open;
                            if(!aud.isPlaying)
                                aud.PlayOneShot(audioclip, GlobalValue.SE_Value * GlobalValue.SE_Bool * 0.1f);
                            break;
                        }
                    }
                }                
            }
            else
            {
                open = !open;
                if (!aud.isPlaying)
                    aud.PlayOneShot(audioclip, GlobalValue.SE_Value * GlobalValue.SE_Bool * 0.1f);
            }
        }
                
    }

    void OnTriggerEnter(Collider col)
    {   
        if (col.tag == "Enemy")
        {
            if (Key != null) //키가 필요하다면
            {
                if (GlobalValue.PlayerInvenList != null)
                    for (int i = 0; i < GlobalValue.PlayerInvenList.Count; i++)
                    {
                        if (GlobalValue.PlayerInvenList[i].Contains(Key.name)) //플레이어가 키가 있으면 똑같이 문을 열 수 있도록 //if (GlobalValue.EnemyInvenList[i].gameObject == Key)    //키가 있는지 확인
                        {
                            open = true;
                            if (!aud.isPlaying)
                                aud.PlayOneShot(audioclip, GlobalValue.SE_Value * GlobalValue.SE_Bool * 0.1f);
                            break;
                        }
                    }
            }
            else
            {
                open = true;
                if (!aud.isPlaying)
                    aud.PlayOneShot(audioclip, GlobalValue.SE_Value * GlobalValue.SE_Bool * 0.1f);
            }
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            enter = false;
        }
        else if(col.tag == "Enemy")
        {
            if (Key != null) //키가 필요하다면
            {
                if (GlobalValue.PlayerInvenList != null)
                {
                    for (int i = 0; i < GlobalValue.PlayerInvenList.Count; i++)
                    {
                        if (GlobalValue.PlayerInvenList[i].Contains(Key.name))    //플레이어가 키가 있는지 확인
                        {
                            open = true;
                            if (!aud.isPlaying)
                                aud.PlayOneShot(audioclip, GlobalValue.SE_Value * GlobalValue.SE_Bool * 0.1f);
                            break;
                        }
                    }                    
                }
            }
            else
            {
                open = true;
                if (!aud.isPlaying)
                    aud.PlayOneShot(audioclip, GlobalValue.SE_Value * GlobalValue.SE_Bool * 0.1f);
            }
        }
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Player")
        {
            enter = true;
        }        
    }
}