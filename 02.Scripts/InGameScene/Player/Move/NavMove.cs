﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMove : MonoBehaviour
{

    [Header("Move")]
    public float moveSpeed = 8.0f;
    private float OneStep = 0.1f;
    private float h, v;
    private float x;
    private Transform tr;
    private Rigidbody rigid;
    private Vector3 moveDir;
    //회전 속도 변수
    //float rotSpeed = 10.0f;

    [Header("Audio")]
    public AudioClip[] walkClip;
    float audTime = 0.0f;

    [Header("FootStep")]
    public GameObject FootPos;
    public GameObject FootStepObj;
    public FloorMtrl Mtrl;

    //[Header("Collider")]
    //public CapsuleCollider Capsule;

    [Header("Nav")]
    NavMeshAgent agent;
    public GameObject NavCam;

    public static float PlayerHeight = 1.2f;
    public static NavMove Inst;

    RaycastHit hit;
    Ray ray;
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
        rigid = GetComponent<Rigidbody>();
        //스크립스 처음에 Transform 컴포넌트 할당
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ray.origin = this.transform.position;
        ray.direction = NavCam.transform.forward;
        Debug.DrawRay(ray.origin, ray.direction * 5.0f, Color.red);

        if (GameMgr.Deltatime <= 0.0f)
            return;

        Mtrl = FootPos.GetComponent<FloorSoundMtrl>().Mtrl;

        Move();
    }//void FixedUpdate()

    void Move()
    {
        h = 0.0f;
        v = 0.0f;

        //키보드 움직임
        if (Input.GetKey(ControllSetting.Move_Forward))     //(KeyCode)PlayerPrefs.GetInt("Move_Forward", (int)KeyCode.W)))
            v += OneStep;
        else if (Input.GetKey(ControllSetting.Move_Back))   //(KeyCode)PlayerPrefs.GetInt("Move_Back", (int)KeyCode.S)))
            v -= OneStep;

        if (Input.GetKey(ControllSetting.Move_Right))       //(KeyCode)PlayerPrefs.GetInt("Move_Right", (int)KeyCode.D)))
            h += OneStep;
        else if (Input.GetKey(ControllSetting.Move_Left))   //(KeyCode)PlayerPrefs.GetInt("Move_Left", (int)KeyCode.A)))
            h -= OneStep;
        //키보드 움직임

        if (h != 0 || v != 0)
        {
            MoveStepPos = ((NavCam.transform.forward * v) + (NavCam.transform.right * h)).normalized * GameMgr.Deltatime * 15.0f;

            OneStep = 0.1f;         //한걸음 걷고나서 다음 걸음 다시 계산하기위해 초기화

            if (Input.GetKey(ControllSetting.Move_Back))    // KeyCode.S))      //뒤로 걷
                moveSpeed = 4.0f;
            else                                                                //앞,좌,우 이동
                moveSpeed = 7.0f;

            if (Input.GetKey(ControllSetting.Sit))  // KeyCode.LeftControl))    //앉기
                moveSpeed *= 0.5f;
            else
            {
                if (Input.GetKey(ControllSetting.Sprint) && Input.GetKey(ControllSetting.Move_Back)) //뒤로달리기 X
                {
                    //Debug.Log("Back Run");
                }
                else if (Input.GetKey(ControllSetting.Sprint))  //앞,좌,우 달리기
                {
                    OneStep = 0.2f;         //이동 보폭증가
                    moveSpeed *= 1.5f;
                }
            }

            agent.speed = moveSpeed;
            agent.velocity = MoveStepPos * moveSpeed;
            StepSound();
        }
    }

    void StepSound()    //발 아래 재질에 따라 다른 사운드
    {
        Vector3 FootStepPosition = FootPos.transform.position;
        audTime += GameMgr.Deltatime;

        if (audTime >= 1.2f && v < 0 && Input.GetKey(ControllSetting.Sit)&& Input.GetKey(ControllSetting.Move_Back))
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
            return;
        }
        else if(audTime >= 1.0f && v > 0 && Input.GetKey(ControllSetting.Sit) && !Input.GetKey(ControllSetting.Move_Back))
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
            return;
        }


        if (audTime >= 0.8f && v < 0 && !Input.GetKey(ControllSetting.Sit))   //뒤로 걸을 때
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
        else if (audTime >= 0.5f && v >= 0 && !(Input.GetKey(ControllSetting.Sit))) //걸을 때
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
            && Input.GetKey(ControllSetting.Sprint)     //달리면서
            && !(Input.GetKey(ControllSetting.Move_Back))   //뒤로가지않고
            && !(Input.GetKey(ControllSetting.Sit)))    //앉지 않았을 때
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

    }
}
