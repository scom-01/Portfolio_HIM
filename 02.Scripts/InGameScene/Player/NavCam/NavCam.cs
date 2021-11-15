using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavCam : MonoBehaviour
{
    private float x, y;

    private Transform tr;
    public float MaxPeeckX = 0.3f;
    public float MaxPeeckRot = 17.0f;
    float PeekingX = 0.0f;
    float PeekingRot = 0.0f;
    float PeekSpeed = 20.0f;

    //private GameObject Player = null;
    //회전 속도 변수
    public float rotSpeed = 100.0f;
    public GameObject Player;
    public static NavCam Inst;

    float _height = NavMove.PlayerHeight;

    void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //스크립스 처음에 Transform 컴포넌트 할당
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.transform.localPosition = this.transform.right * PeekingX;
        Rotate();
    }//void FixedUpdate()

    void Rotate()
    {
        if (Setting.SettingBool)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (GameMgr.Deltatime <= 0.0f)
            return;

        y += Input.GetAxis("Mouse Y") * PlayerPrefs.GetFloat("mouseSpeed", 5.0f) * 0.2f;
        x += Input.GetAxis("Mouse X") * PlayerPrefs.GetFloat("mouseSpeed", 5.0f) * 0.2f;


        if (y >= 60)
            y = 60;
        else if (y <= -50)
            y = -50;


        if (Input.GetKey(ControllSetting.Sit))  // KeyCode.LeftControl))
        {
            if (_height > NavMove.PlayerHeight * 0.5f)
                _height -= GameMgr.Deltatime * 5.0f;   // == 0.1f;
            else if (_height <= NavMove.PlayerHeight * 0.5f)
                _height = NavMove.PlayerHeight * 0.5f;

            Player.GetComponent<NavMeshAgent>().height = _height;
            Player.GetComponent<CapsuleCollider>().height = _height;
            Player.GetComponent<NavMeshAgent>().baseOffset = _height * 0.5f;
        }
        else
        {
            if (_height < NavMove.PlayerHeight)
                _height += GameMgr.Deltatime * 5.0f;   // == 0.1f;
            else if (_height >= NavMove.PlayerHeight)
                _height = NavMove.PlayerHeight;

            Player.GetComponent<NavMeshAgent>().height = _height;
            Player.GetComponent<CapsuleCollider>().height = _height;
            Player.GetComponent<NavMeshAgent>().baseOffset = _height * 0.5f;
        }

        Player.GetComponentInChildren<FloorSoundMtrl>().gameObject.transform.localPosition = new Vector3(0, _height * -0.5f, 0);

        if (Input.GetKey(ControllSetting.Peeking_Left))    // KeyCode.Q))
        {
            Peeking_Left();
        }
        else if (Input.GetKey(ControllSetting.Peeking_Right))    // KeyCode.E))
        {
            Peeking_Right();
        }
        else if (!(Input.GetKey(ControllSetting.Peeking_Left) || Input.GetKey(ControllSetting.Peeking_Left)))
        {
            //부드럽게
            if (PeekingX != 0.0f)
            {
                if (PeekingX > 0)
                {
                    PeekingX -= GameMgr.Deltatime;
                    if (PeekingX <= 0.0f)
                        PeekingX = 0.0f;
                }
                else if (PeekingX < 0)
                {
                    PeekingX += GameMgr.Deltatime;
                    if (PeekingX >= 0.0f)
                        PeekingX = 0.0f;
                }
            }

            if (PeekingRot != 0.0f)
            {
                if (PeekingRot > 0)
                {
                    PeekingRot -= GameMgr.Deltatime * PeekSpeed;
                    if (PeekingRot <= 0.0f)
                        PeekingRot = 0.0f;
                }
                else
                {
                    PeekingRot += GameMgr.Deltatime * PeekSpeed;
                    if (PeekingRot >= 0.0f)
                        PeekingRot = 0.0f;
                }
            }
            //부드럽게
            tr.localRotation = Quaternion.Euler(-y, x, PeekingRot);            
        }
    }

    void Peeking_Right()
    {
        if (PeekingX < MaxPeeckX)
        {
            PeekingX += GameMgr.Deltatime; //Time.deltaTime;
            if (PeekingX >= MaxPeeckX)
                PeekingX = MaxPeeckX;
        }

        if (PeekingRot > -MaxPeeckRot)
        {
            PeekingRot -= GameMgr.Deltatime * PeekSpeed;
            if (PeekingRot <= -MaxPeeckRot)
                PeekingRot = -MaxPeeckRot;
        }

        tr.localRotation = Quaternion.Euler(-y, x, PeekingRot);
    }

    void Peeking_Left()
    {
        if (PeekingX > -MaxPeeckX)
        {
            PeekingX -= GameMgr.Deltatime; //Time.deltaTime;
            if (PeekingX <= -MaxPeeckX)
                PeekingX = -MaxPeeckX;
        }

        if (PeekingRot < MaxPeeckRot)
        {
            PeekingRot += GameMgr.Deltatime * PeekSpeed;
            if (PeekingRot >= MaxPeeckRot)
                PeekingRot = MaxPeeckRot;
        }

        tr.localRotation = Quaternion.Euler(-y, x, PeekingRot);
    }
}
