using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMove : MonoBehaviour
{

    [Header("Move")]
    public float moveSpeed = 8.0f;
    private float OneStep = 0.1f;
    private float h, v;

    [Header("Audio")]
    public AudioClip[] walkClip;
    float audTime = 0.0f;

    [Header("FootStep")]
    public GameObject FootPos;
    public GameObject FootStepObj;
    public FloorMtrl Mtrl;

    //벽을 통과하거나 낑기는 현상을 방지하기위해 NavMeshAgent로 이동
    [Header("Nav")]
    NavMeshAgent agent;
    public GameObject NavCam;

    public static float PlayerHeight = 1.2f;
    public static NavMove Inst;

    Vector3 MoveStepPos = Vector3.zero;

    private void Awake()
    {
        Inst = this;
        agent = GetComponent<NavMeshAgent>();
        CapsuleCollider Capsule = GetComponent<CapsuleCollider>();
        Capsule.height = GlobalValue.PlayerHeight;
        agent.height = GlobalValue.PlayerHeight;
        agent.baseOffset = GlobalValue.PlayerHeight * 0.5f;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (GlobalValue.g_GameState == GameState.Die)
            return;

        if (GlobalValue.Deltatime <= 0.0f)
        {
            return;
        }
            

        Mtrl = FootPos.GetComponent<FloorSoundMtrl>().Mtrl;         //플레이어뿐만 아니라 Enemy의 발소리도 전환하기위해 별도의 클래스 생성

        Move();
    }//void FixedUpdate()

    void Move()
    {
        h = 0.0f;
        v = 0.0f;

        //키보드 움직임
        if (Input.GetKey(GlobalValue.Move_Forward))     //(KeyCode)PlayerPrefs.GetInt("Move_Forward", (int)KeyCode.W)))
            v += OneStep;
        else if (Input.GetKey(GlobalValue.Move_Back))   //(KeyCode)PlayerPrefs.GetInt("Move_Back", (int)KeyCode.S)))
            v -= OneStep;

        if (Input.GetKey(GlobalValue.Move_Right))       //(KeyCode)PlayerPrefs.GetInt("Move_Right", (int)KeyCode.D)))
            h += OneStep;
        else if (Input.GetKey(GlobalValue.Move_Left))   //(KeyCode)PlayerPrefs.GetInt("Move_Left", (int)KeyCode.A)))
            h -= OneStep;
        //키보드 움직임

        if (h != 0 || v != 0)
        {
            MoveStepPos = ((NavCam.transform.forward * v) + (NavCam.transform.right * h)).normalized * GlobalValue.Deltatime * 15.0f;

            OneStep = 0.1f;         //한걸음 걷고나서 다음 걸음 다시 계산하기위해 초기화

            if (Input.GetKey(GlobalValue.Move_Back))    // KeyCode.S))      //뒤로 걷
                moveSpeed = 4.0f;
            else                                                                //앞,좌,우 이동
                moveSpeed = 7.0f;

            if (Input.GetKey(GlobalValue.Sit))  // KeyCode.LeftControl))    //앉기
                moveSpeed *= 0.5f;
            else
            {
                if (Input.GetKey(GlobalValue.Sprint) && Input.GetKey(GlobalValue.Move_Back)) //뒤로달리기 X
                {
                    //Debug.Log("Back Run");
                }
                else if (Input.GetKey(GlobalValue.Sprint))  //앞,좌,우 달리기
                {
                    OneStep = 0.2f;         //이동 보폭증가
                    moveSpeed *= 1.5f;
                }
            }

            agent.speed = moveSpeed;
            agent.velocity = MoveStepPos * moveSpeed;
            StepSound();

            //걸을 수록 손전등 게이지 ++
            if (!GlobalValue.FlashOnOff) //끈 상태에서만
            {
                if (GlobalValue.FlashGage < 100.0f)
                {
                    GlobalValue.FlashGage += GlobalValue.Deltatime * 2;
                    if (GlobalValue.FlashGage >= 100.0f)
                    {
                        GlobalValue.FlashGage = 100.0f;
                    }
                }
            }
            //걸을 수록 손전등 게이지 ++
        }
    }

    void StepSound()    //발 아래 재질에 따라 다른 사운드
    {
        Vector3 FootStepPosition = FootPos.transform.position;
        audTime += GlobalValue.Deltatime;                           //발자국 소리 딜레이

        if (audTime >= 1.2f && v < 0 && Input.GetKey(GlobalValue.Sit) && Input.GetKey(GlobalValue.Move_Back))   //앉으면서 뒤로 갈 때
        {
            if (Mtrl == FloorMtrl.Grass)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[3], FootStepPosition, 1.5f, 1f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV1)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[5], FootStepPosition, 1.5f, 1f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV2)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[7], FootStepPosition, 1.5f, 1f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Tile)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[1], FootStepPosition, 1.5f, 1f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Wood)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[9], FootStepPosition, 1.5f, 1f);   //발자국 사운드
            }
            audTime = 0.0f;
            return;
        }
        else if (audTime >= 1.0f && v > 0 && Input.GetKey(GlobalValue.Sit) && !Input.GetKey(GlobalValue.Move_Back))     //앉으면서 앞으로 갈 때
        {
            if (Mtrl == FloorMtrl.Grass)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[2], FootStepPosition, 1.5f, 1f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV1)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[4], FootStepPosition, 1.5f, 1f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV2)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[6], FootStepPosition, 1.5f, 1f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Tile)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[0], FootStepPosition, 1.5f, 1f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Wood)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[8], FootStepPosition, 1.5f, 1f);   //발자국 사운드
            }
            audTime = 0.0f;
            return;
        }


        if (audTime >= 0.8f && v < 0 && !Input.GetKey(GlobalValue.Sit))   //뒤로 걸을 때
        {
            if (Mtrl == FloorMtrl.Grass)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[3], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV1)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[5], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV2)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[7], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Tile)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[1], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Wood)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[9], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            audTime = 0.0f;
        }
        else if (audTime >= 0.5f && v >= 0 && !(Input.GetKey(GlobalValue.Sit))) //걸을 때
        {
            if (Mtrl == FloorMtrl.Grass)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[2], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV1)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[4], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV2)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[6], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Tile)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[0], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Wood)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[8], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            audTime = 0.0f;
        }
        else if (audTime >= 0.3f        //달릴 때
            && Input.GetKey(GlobalValue.Sprint)     //달리면서
            && !(Input.GetKey(GlobalValue.Move_Back))   //뒤로가지않고
            && !(Input.GetKey(GlobalValue.Sit)))    //앉지 않았을 때
        {
            if (Mtrl == FloorMtrl.Grass)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[2], FootStepPosition, 3.5f, 2.5f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV1)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[4], FootStepPosition, 3.5f, 2.5f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV2)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[6], FootStepPosition, 3.5f, 2.5f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Tile)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[0], FootStepPosition, 3.5f, 2.5f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Wood)
            {
                Instantiate(FootStepObj).GetComponent<FootStep>().Range(walkClip[8], FootStepPosition, 3.5f, 2.5f);   //발자국 사운드
            }
            audTime = 0.0f;
        }

    }
}
