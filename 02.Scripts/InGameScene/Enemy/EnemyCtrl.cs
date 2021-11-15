using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour
{
    Transform tr;

    //----MonsterAI
    [HideInInspector] public GameObject m_AggroTarget = null;
    [HideInInspector] public Vector3 AggroTargetPos;
    //int m_AggroTgID = -1;                 //이 몬스터가 공격해야할 캐럭터의 고유번호
    Vector3 m_MoveDir = Vector3.zero;       //수평 진행 노멀 방향 벡터
    Vector3 m_CacVLen = Vector3.zero;       //어그로타겟을 향하는 벡터
    float m_CacDist = 0.0f;                 //거리 계산용 변수
    float m_RayDist = 0.0f;                 //Ray 판정거리 계산용 변수
    float traceDist = 5.0f;                 //추적 거리 (시야판정)
    //float Detec_traceDist = 5.0f;           //추적 거리 (소리판정)
    float attackDist = 1.5f;                //공격 거리
    Quaternion a_TargetRot;                 //회전 계산용 변수
    float m_RotSpeed = 7.0f;                //초당 회전 속도
    //float m_NowStep = 0.0f;                 //이동 계산용 변수
    Vector3 a_MoveNextStep = Vector3.zero;  //이동 계산용 변수
    float m_MoveVelocity = 2.0f;            //평면 초당 이동 속도...    
    float Recog_time = 0.5f;
    //----MonsterAI

    //---- Navigation
    protected NavMeshAgent nvAgent;    //using UnityEngine.AI;
    protected NavMeshPath movePath;
    protected Vector3 m_PathEndPos = Vector3.zero;
    int m_CurPathIndex = 1;
    protected double m_MoveDurTime = 0.0;     //목표점까지 도착하는데 걸리는 시간
    protected double m_AddTimeCount = 0.0;    //누적시간 카운트 
    float m_MoveTick = 0.0f;
    //bool NvMoveBool = false;
    private RaycastHit hit;
    //---- Navigation

    //--- MoveToPath 관련 변수들...
    private bool a_isSucessed = false;
    private Vector3 a_CurCPos = Vector3.zero;
    private Vector3 a_CacDestV = Vector3.zero;
    private Vector3 a_TargetDir;
    private float a_CacSpeed = 0.0f;
    private float a_NowStep = 0.0f;
    private Vector3 a_Velocity = Vector3.zero;
    private Vector3 a_vTowardNom = Vector3.zero;
    private int a_OldPathCount = 0;
    Vector3 a_VecLen = Vector3.zero;
    //--- MoveToPath 관련 변수들...

    [Header("Audio")]
    public AudioClip[] walkClip;
    float audTime = 0.0f;
    public GameObject FootStepPrefab;
    public GameObject FootPos;
    public FloorMtrl Mtrl;

    //public SphereCollider DetectionSphere;
    //public Anim anim;

    //Animation m_RefAnimation = null;
    Animator m_RefAnimator = null;
    AnimatorStateInfo animatorStateInfo;
    [HideInInspector] public AnimState CurAnimState = AnimState.idle;
    [HideInInspector] public AnimState OldAnimState = AnimState.idle;
    //float CountTime = 5.0f;

    public float Go_WayPointTime = 3.0f;
    public GameObject BeforeObj;

    public static EnemyCtrl Inst;

    void Awake()
    {
        Inst = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        movePath = new NavMeshPath();
        nvAgent = this.gameObject.GetComponent<NavMeshAgent>();
        nvAgent.updateRotation = false;
        tr = this.transform;
        m_RefAnimator = this.gameObject.GetComponent<Animator>();        
    }

    // Update is called once per frame
    void Update()
    {
        if (m_RefAnimator == null)
            return;

        Mtrl = FootPos.GetComponent<FloorSoundMtrl>().Mtrl;

        if (GameMgr.Deltatime == 0.0f)
            m_RefAnimator.speed = 0.0f;
        else
            m_RefAnimator.speed = 1.0f;

        m_MoveTick -= GameMgr.Deltatime;
        if (m_MoveTick < 0.0f)
            m_MoveTick = 0.0f;

        StateUpdate();

        //플레이어 인식 시간 카운트
        if (Recog_time > 0.0f)
        {
            Recog_time -= GameMgr.Deltatime;
            if (Recog_time <= 0.0f)
                Recog_time = 0.0f;
        }

        if (Go_WayPointTime > 0.0f)
        {
            Go_WayPointTime -= GameMgr.Deltatime;
            if (Go_WayPointTime <= 0.0f)
                Go_WayPointTime = 0.0f;
        }
        //플레이어 인식 시간 카운트

        ActionUpdate();
    }

    //상태 업데이트
    void StateUpdate()
    {
        if (m_AggroTarget != null)                //어그로타겟이 있는경우
        {
            GameObject AggroTarget = GameObject.FindGameObjectWithTag("Player");
            m_CacVLen = m_AggroTarget.transform.position - this.transform.position;
            //m_CacVLen.y = 0.0f;                 //수평이동
            m_MoveDir = m_CacVLen.normalized;   //어그로타겟을 향하는 단위벡터
            m_CacDist = m_CacVLen.magnitude;    //어그로타겟과의 거리
            m_RayDist = (AggroTarget.transform.position - (transform.position + Vector3.up * 1.6f)).magnitude;

            if (m_CacDist <= attackDist)   //공격거리 범위 이내로 들어왔는지 확인
            {
                Ray ray = new Ray();
                ray.origin = transform.position;
                ray.direction = m_MoveDir;

                int layerMask = ((1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("DOORFRAME")) | (1 << LayerMask.NameToLayer("SOUND")));
                if (Physics.Raycast(ray, out hit, m_RayDist, ~layerMask))   //Default, DOORFRAME, SOUND Layer무시
                {
                    if (hit.transform.tag.Contains("Untagged") || hit.transform.tag.Contains("Wall") || hit.transform.tag.Contains("Door") || hit.transform.tag.Contains("Floor"))
                    {
                        m_AggroTarget = null;   //어그로타겟 null                       
                        return;
                    }

                    if (hit.transform.tag.Contains("Player"))    //처음맞은 오브젝트 판단  //태그로 판단가능
                    {
                        CurAnimState = AnimState.attack;
                        m_AggroTarget = AggroTarget.gameObject; //어그로타겟
                        AggroTargetPos = AggroTarget.GetComponent<NavMove>().FootPos.transform.position;
                    }
                }
                else
                {
                    CurAnimState = AnimState.idle;
                }
            }
            else if (m_CacDist <= traceDist)        //추적거리 범위 이내로 들어왔는지 확인
            {
                Ray ray = new Ray();
                ray.origin = transform.position + Vector3.up * 1.6f;
                ray.direction = m_AggroTarget.transform.position - ray.origin;

                Debug.DrawRay(ray.origin, ray.direction * m_RayDist, Color.blue);
                int layerMask = ((1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("DOORFRAME")) | (1 << LayerMask.NameToLayer("SOUND")));
                if (Physics.Raycast(ray, out hit, m_RayDist, ~layerMask))   //Default, DOORFRAME, SOUND Layer무시
                {
                    if (hit.transform.tag.Contains("Untagged") || hit.transform.tag.Contains("Wall")|| hit.transform.tag.Contains("Door") || hit.transform.tag.Contains("Floor"))
                    {
                        m_AggroTarget = null;   //어그로타겟 null                       
                        return;
                    }

                    if (hit.transform.tag.Contains("Player"))    //처음맞은 오브젝트 판단  //태그로 판단가능
                    {
                        //플레이어를 인식하는 시간
                        if (!a_isSucessed)
                            if (Recog_time <= 0.0f)
                            {
                                Recog_time = 0.5f;      //한번 주기
                            }
                        //플레이어를 인식하는 시간 

                        CurAnimState = AnimState.trace;     //추적상태
                        m_AggroTarget = AggroTarget.gameObject; //어그로타겟
                        AggroTargetPos = AggroTarget.GetComponent<NavMove>().FootPos.transform.position;

                    }
                }

            }
        }
        else    //어그로타겟이 없을 경우
        {
            GameObject AggroTarget = GameObject.FindGameObjectWithTag("Player");
            //GameObject[] TargetSound = GameObject.FindGameObjectsWithTag("TargetSound");

            m_CacVLen = AggroTarget.transform.position - this.transform.position;   //자신의 위치로부터 어그로타겟을 향하는벡터
            //m_CacVLen.y = 0.0f;                 //수평이동
            m_MoveDir = m_CacVLen.normalized;   //어그로타겟을 향하는 단위벡터
            m_CacDist = m_CacVLen.magnitude;    //어그로타겟과 자신의 거리
            m_RayDist = (AggroTarget.transform.position - (transform.position + Vector3.up * 1.6f)).magnitude;
            //네비
            //네비

            if (m_CacDist <= attackDist)                 //공격거리 범위 이내로 들어왔는지 확인  //*모드를 추가할시 하드모드일때는 바로 잡히도록
            {
                Ray ray = new Ray();
                ray.origin = transform.position;
                ray.direction = m_MoveDir;

                if (Physics.Raycast(ray, out hit, attackDist))
                {
                    if (hit.transform.tag.Contains("Player"))    //처음맞은 오브젝트 판단  //태그로 판단가능
                    {
                        CurAnimState = AnimState.attack;
                        m_AggroTarget = AggroTarget.gameObject; //어그로타겟
                        Debug.DrawRay(ray.origin, ray.direction * attackDist, Color.red);
                    }
                }
                return;
            }
            else if (m_CacDist <= traceDist)             //추적거리 범위 이내로 들어왔는지 확인
            {
                Ray ray = new Ray();
                ray.origin = transform.position + Vector3.up * 1.6f;
                ray.direction = AggroTarget.transform.position - ray.origin;

                Debug.DrawRay(ray.origin, ray.direction * m_RayDist, Color.green);

                int layerMask = ((1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("DOORFRAME")) | (1 << LayerMask.NameToLayer("SOUND")));
                if (Physics.Raycast(ray, out hit, m_RayDist, ~layerMask))   //Default, DOORFRAME, SOUND Layer무시
                {
                    if (hit.transform.tag.Contains("Untagged") || hit.transform.tag.Contains("Wall")|| hit.transform.tag.Contains("Door"))
                    {
                        return;
                    }

                    if (hit.transform.tag.Contains("Player"))   //태그로 판단가능
                    {
                        CurAnimState = AnimState.trace;     //추적상태
                        m_AggroTarget = AggroTarget.gameObject; //어그로타겟
                        AggroTargetPos = AggroTarget.GetComponent<NavMove>().FootPos.transform.position;
                    }
                }
                return;
            }

            if (Go_WayPointTime > 0.0f)
            {
                CurAnimState = AnimState.idle;
                return;
            }

            if (CurAnimState != AnimState.waypoint && CurAnimState != AnimState.detec_walk)
            {
                //가장 가까운 WayPoint
                List<WayPointAI> WaypointsList = new List<WayPointAI>();
                WayPointAI[] Waypoints = FindObjectsOfType<WayPointAI>();
                if(BeforeObj != null)
                {
                    for(int i =0;i<Waypoints.Length;i++)
                    {
                        if(Waypoints[i].name != BeforeObj.name)
                        {                            
                            WaypointsList.Add(Waypoints[i]);
                        }     
                    }
                }
                else
                {
                    for (int i = 0; i < Waypoints.Length; i++)
                    {
                        WaypointsList.Add(Waypoints[i]);
                    }
                }

                float MinDistWay = (WaypointsList[0].transform.position - this.transform.position).magnitude;
                GameObject MinDistObj = WaypointsList[0].gameObject;
                for (int i = 0; i < WaypointsList.Count; i++)
                {
                    if ((WaypointsList[i].transform.position - this.transform.position).magnitude < MinDistWay)
                    {
                        MinDistWay = (WaypointsList[i].transform.position - this.transform.position).magnitude;
                        MinDistObj = WaypointsList[i].gameObject;
                    }
                }
                //가장 가까운 WayPoint
                CurAnimState = AnimState.waypoint;
                AggroTargetPos = MinDistObj.transform.position;
            }

        }//else    //어그로타겟이 없을 경우
    }//void StateUpdate()
    //상태 업데이트

    //행동 업데이트
    void ActionUpdate()
    {
        if (CurAnimState == AnimState.attack) //공격상태
        {
            m_MoveVelocity = 0.7f;  //이동속도
            if (m_AggroTarget == null)    //어그로 타겟이 없는데 공격상태일 때
            {
                return;
            }

            if (0.0001f < m_MoveDir.magnitude)   //어그로타겟과 딱붙어있는게 아닌 경우
            {
                a_TargetRot = Quaternion.LookRotation(m_MoveDir);   //어그로타겟을 바라보는 벡터를 향하는 Quaternion Rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, a_TargetRot, GameMgr.Deltatime * m_RotSpeed); //보간회전
            }

            MySetAnim(AnimState.attack);
            //CurAnimState = AnimState.attack;

            //네비이동취소
            ClearMsPickPath();
            m_AggroTarget = null;
            //네비이동취소
        }
        else if (CurAnimState == AnimState.trace)    //추적상태
        {
            m_MoveVelocity = 1.5f;  //이동속도
            //공격중이라면 공격 끝난후에 추적이동
            if (IsAnimCheck(AnimState.attack))
                return;

            if (m_RefAnimator.GetBool(AnimState.attack.ToString()))
                m_RefAnimator.SetBool(AnimState.attack.ToString(), false);
            if (m_RefAnimator.GetBool(AnimState.waypoint.ToString()))
                m_RefAnimator.SetBool(AnimState.waypoint.ToString(), false);


            //공격중이라면 공격 끝난후에 추적이동

            animatorStateInfo = m_RefAnimator.GetCurrentAnimatorStateInfo(0);
            if (animatorStateInfo.IsName("detec_walk"))
            {
                ClearMsPickPath();
            }


            //네비 길찾기
            if (m_MoveTick <= 0.0f)
            {
                //주기적으로 피킹
                float a_PathLen = 0.0f;
                if (m_AggroTarget != null)
                {
                    if (MyNavCalcPath(this.transform.position, m_AggroTarget.transform.position, ref a_PathLen))
                    {
                        m_MoveDurTime = a_PathLen / m_MoveVelocity; //도착하는데 걸리는 시간
                        m_AddTimeCount = 0.0;
                    }
                }
                else
                {
                    if (MyNavCalcPath(this.transform.position, AggroTargetPos, ref a_PathLen))
                    {
                        m_MoveDurTime = a_PathLen / m_MoveVelocity; //도착하는데 걸리는 시간
                        m_AddTimeCount = 0.0;
                    }
                }
                m_MoveTick = 0.05f;
            }
            //네비 길찾기

            //네비 길찾기 이동
            MoveToPath(AnimState.trace);
            //네비 길찾기 이동
        }
        else if (CurAnimState == AnimState.detec_walk)      //소리 감지
        {
            m_MoveVelocity = 0.7f;  //이동속도
            //네비 길찾기
            if (m_MoveTick <= 0.0f)
            {
                //주기적으로 피킹
                float a_PathLen = 0.0f;
                //if (m_AggroTarget != null)
                //{
                //    if (MyNavCalcPath(this.transform.position, m_AggroTarget.transform.position, ref a_PathLen))
                //    {
                //        m_MoveDurTime = a_PathLen / m_MoveVelocity; //도착하는데 걸리는 시간
                //        m_AddTimeCount = 0.0;
                //    }
                //}
                //else
                //{
                if (MyNavCalcPath(this.transform.position, AggroTargetPos, ref a_PathLen))
                {
                    m_MoveDurTime = a_PathLen / m_MoveVelocity; //도착하는데 걸리는 시간
                    m_AddTimeCount = 0.0;
                }
                //}
                m_MoveTick = 0.05f;
            }
            //네비 길찾기

            //네비 길찾기 이동
            MoveToPath(AnimState.detec_walk);
            //네비 길찾기 이동
        }
        else if (CurAnimState == AnimState.waypoint)      //소리 감지 추적
        {
            m_MoveVelocity = 0.7f;  //이동속도
            float a_PathLen = 0.0f;
            if (MyNavCalcPath(this.transform.position, AggroTargetPos, ref a_PathLen))
            {
                m_MoveDurTime = a_PathLen / m_MoveVelocity; //도착하는데 걸리는 시간
                m_AddTimeCount = 0.0;
            }

            MoveToPath(AnimState.waypoint);//, AnimState.waypoint);

        }
        else if (CurAnimState == AnimState.idle)
        {
            MySetAnim(AnimState.idle);
            CurAnimState = AnimState.idle;
            //OldAnimState = AnimState.idle;
        }
    }//void ActionUpdate()
     //행동 업데이트

    public bool MyNavCalcPath(Vector3 a_StartPos, Vector3 a_TargetPos,
                                ref float a_PathLen) //길찾기...
    { //경로 탐색 함수
        //--- 피킹이 발생된 상황이므로 초기화 하고 계산한다.
        movePath.ClearCorners();  //경로 모두 제거 
        m_CurPathIndex = 1;       //진행 인덱스 초기화 
        m_PathEndPos = transform.position;
        //--- 피킹이 발생된 상황이므로 초기화 하고 계산한다.

        if (nvAgent == null || nvAgent.enabled == false)
            return false;

        int LayM = ((1 << LayerMask.NameToLayer("Default")) | (1 << LayerMask.NameToLayer("DOORFRAME")) | (1 << LayerMask.NameToLayer("SOUND")));
        if (NavMesh.CalculatePath(a_StartPos, a_TargetPos, LayM, movePath) == false)
        //CalculatePath :
        //두 점 사이의 위치를 계산하여, 네비 메쉬 오브젝트상에서 이동할 수 있는 범위의 경로를 만듭니다
        {   //CalculatePath() 함수 계산이 끝나고 정상적으로instance.final 
            //즉 목적지까지 계산에 도달했다는 뜻 
            //--> p.status == UnityEngine.AI.NavMeshPathStatus.PathComplete 
            //<-- 그럴때 정상적으로 타겟으로 설정해 준다.는 뜻
            // 길 찾기 실패 했을 때 점프하는 경향이 있다. 
            return false;
        }
        //Debug.Log(movePath.corners[movePath.corners.Length-1]);   //도착점 위치

        if (movePath.corners.Length < 2)
        {
            return false;
        }


        for (int i = 1; i < movePath.corners.Length; ++i)
        {
#if UNITY_EDITOR
            Debug.DrawLine(movePath.corners[i - 1], movePath.corners[i], Color.cyan, 10);
            //맨마지막 인자 duration 라인을 표시하는 시간
            Debug.DrawLine(movePath.corners[i], movePath.corners[i] + Vector3.up * i,
                           Color.cyan, 10);
#endif
            a_VecLen = movePath.corners[i] - movePath.corners[i - 1];
            //a_VecLen.y = 0.0f;
            a_PathLen = a_PathLen + a_VecLen.magnitude;
        }

        if (a_PathLen <= 0.0f)
            return false;

        //-- 주인공이 마지막 위치에 도착했을 때 정확한 방향을 
        // 바라보게 하고 싶은 경우 때문에 계산해 놓는다.
        m_PathEndPos = movePath.corners[(movePath.corners.Length - 1)];

        return true;
    }

    public bool MoveToPath(AnimState A_State, AnimState EndState = AnimState.idle, float overSpeed = 1.0f)
    {
        a_isSucessed = true;

        if (movePath == null)
        {
            movePath = new NavMeshPath();
        }

        a_OldPathCount = m_CurPathIndex;
        if (m_CurPathIndex < movePath.corners.Length) //경로상에 코너가 1개 초과일 경우 캐릭터 이동
                                                      //최소 m_CurPathIndex = 1 보다 큰 경우에는 캐릭터를 이동시켜 준다.
        {
            a_CurCPos = this.transform.position;
            a_CacDestV = movePath.corners[m_CurPathIndex];  //경로상에서 지나지않은 가장 가까운 코너
            //a_CurCPos.y = a_CacDestV.y;  //높이 오차가 있어서 도착 판정을 못하는 경우가 있다. 
            a_TargetDir = a_CacDestV - a_CurCPos;           //현재위치에서 경로상의 코너를 향하는 벡터
            a_TargetDir.y = 0.0f;
            a_TargetDir.Normalize();                        //단위벡터화

            a_CacSpeed = m_MoveVelocity;
            a_CacSpeed = a_CacSpeed * overSpeed;

            a_NowStep = a_CacSpeed * GameMgr.Deltatime;     //이번에 이동했을 때 이 안으로만 들어와도 무조건 도착한 것으로 본다.

            a_Velocity = a_CacSpeed * a_TargetDir;      //한 걸음 크기
            a_Velocity.y = 0.0f;

            //nvAgent움직임과 모션 멈추기
            if (GameMgr.Deltatime == 0.0f)
            {
                nvAgent.velocity = Vector3.zero;
                nvAgent.isStopped = true;
                nvAgent.updatePosition = false;
                nvAgent.updateRotation = false;
            }
            else
            {   
                nvAgent.velocity = a_Velocity;          //이동 처리...
                StepSound();
                nvAgent.isStopped = false;
                nvAgent.updatePosition = true;
                nvAgent.updateRotation = true;
            }
            //nvAgent움직임과 모션 멈추기


            //경로상의 코너를 지났을 때 다음 코너를 지정해주는 코드
            if ((a_CacDestV - a_CurCPos).magnitude <= a_NowStep)    //현재위치와 가까운코너까지의 거리가 한 걸음보다 작을 때
                                                                    //중간점에 도착한 것으로 본다.  여기서 a_CurCPos == Old Position의미
            {
                movePath.corners[m_CurPathIndex] = this.transform.position;
                m_CurPathIndex = m_CurPathIndex + 1;    //다음 코너를 가리킴
            }//if ((a_CacDestV - a_CurCPos).magnitude <= a_NowStep) //중간점에 도착한 것으로 본다.  
            //경로상의 코너를 지났을 때 다음 코너를 지정해주는 코드

            m_AddTimeCount = m_AddTimeCount + GameMgr.Deltatime;    //이동시간 카운트
            if (m_MoveDurTime <= m_AddTimeCount) //목표점에 도착한 것으로 판정한다.
            {
                m_CurPathIndex = movePath.corners.Length;
            }

        }//if (m_CurPathIndex < movePath.corners.Length) //최소 m_CurPathIndex = 1 보다 큰 경우에는 캐릭터를 이동시켜 준다.

        if (m_CurPathIndex < movePath.corners.Length)  //목적지에 아직 도착 하지 않은 경우 매 프레임 
        {
            //-------------캐릭터 회전 / 애니메이션 방향 조정
            a_vTowardNom = movePath.corners[m_CurPathIndex] - this.transform.position;
            a_vTowardNom.Normalize();        // 단위 벡터를 만든다.

            if (0.0001f < a_vTowardNom.magnitude)  //로테이션에서는 모두 들어가야 한다.
            {
                Quaternion a_TargetRot = Quaternion.LookRotation(a_vTowardNom);
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                            a_TargetRot, GameMgr.Deltatime * m_RotSpeed);
            }
            MySetAnim(A_State);
            //-------------캐릭터 회전 / 애니메이션 방향 조정
        }
        else //최종 목적지에 도착한 경우 매 플레임
        {
            if (a_OldPathCount < movePath.corners.Length) //최종 목적지에 도착한 경우 한번 발생시키기 위한 부분
            {
                ClearMsPickPath();
                MySetAnim(EndState); //idle 애니메이션 적용
                CurAnimState = EndState;
                //OldAnimState = AnimState.idle;
                m_AggroTarget = null;
                AggroTargetPos = Vector3.zero;
                Recog_time = 0.0f;
            }
            a_isSucessed = false; //아직 목적지에 도착하지 않았다면 다시 잡아 줄 것이기 때문에... 
        }
        return a_isSucessed;
    }

    void ClearMsPickPath() //마우스 픽킹이동 취소 함수
    {
        //----피킹을 위한 동기화 부분
        m_PathEndPos = transform.position;
        if (0 < movePath.corners.Length)
        {
            movePath.ClearCorners();  //경로 모두 제거 
        }
        m_CurPathIndex = 1;       //진행 인덱스 초기화
        //----피킹을 위한 동기화 부분
    }

    public void MySetAnim(AnimState newAnim)
    {
        if (m_RefAnimator == null)
            return;

        if (OldAnimState == newAnim)
            return;

        if (newAnim == AnimState.idle)
        {
            //m_RefAnimator.ResetTrigger(OldAnimState.ToString());
            m_RefAnimator.SetBool(newAnim.ToString(), true);
            if (m_RefAnimator.GetBool("trace"))
                m_RefAnimator.SetBool("trace", false);
            if (m_RefAnimator.GetBool("attack"))
                m_RefAnimator.SetBool("attack", false);
            if (m_RefAnimator.GetBool("detec_walk"))
                m_RefAnimator.SetBool("detec_walk", false);
            if (m_RefAnimator.GetBool("waypoint"))
                m_RefAnimator.SetBool("waypoint", false);
        }
        else if (newAnim == AnimState.trace)
        {        
            if (m_RefAnimator.GetBool("attack"))
                m_RefAnimator.SetBool("attack", false);
            if (m_RefAnimator.GetBool("detec_walk"))
                m_RefAnimator.SetBool("detec_walk", false);
            m_RefAnimator.SetBool(newAnim.ToString(), true);
        }
        else if (newAnim == AnimState.waypoint)
        {
            if (m_RefAnimator.GetBool("trace"))
                m_RefAnimator.SetBool("trace", false);
            if (m_RefAnimator.GetBool("attack"))
                m_RefAnimator.SetBool("attack", false);
            if (m_RefAnimator.GetBool("detec_walk"))
                m_RefAnimator.SetBool("detec_walk", false);

            m_RefAnimator.SetBool(newAnim.ToString(), true);
        }
        else if (newAnim == AnimState.detec_walk)
        {
            m_RefAnimator.SetBool(newAnim.ToString(), true);
        }
        else if (newAnim == AnimState.attack)
        {
            if (!m_RefAnimator.GetBool("trace"))
                m_RefAnimator.SetBool("trace", true);
            m_RefAnimator.SetBool("attack", true);
        }
        OldAnimState = newAnim;
    }

    public void Attack()
    {        
        Vector3 AttDir = NavMove.Inst.transform.position - this.transform.position;
        float AttDAttDist = AttDir.magnitude;
        float AttDot = Vector3.Dot(AttDir, this.transform.forward);
        if (AttDot > 0 && AttDot < 45 && AttDAttDist < attackDist)
        {
            PlayerState.Inst.Hp -= 30;
        }
    }

    public bool IsAnimCheck(AnimState CheckAnim)
    {
        if (m_RefAnimator != null)
            animatorStateInfo = m_RefAnimator.GetCurrentAnimatorStateInfo(0);   //현재 애니메이션의 상태 정보

        if (animatorStateInfo.IsName(CheckAnim.ToString()))     //애니메이션 작동중인가?
        {
            if (animatorStateInfo.normalizedTime >= 1.0f)   //애니메이션 작동이 완료되었나?
                return false;       //끝
            else
                return true;        //실행중
        }
        else
            return false;           //실행중 아님
    }

    void StepSound()
    {
        Vector3 FootStepPosition = FootPos.transform.position;
        audTime += GameMgr.Deltatime;

        if(audTime>=0.5f &&( CurAnimState == AnimState.detec_walk || CurAnimState == AnimState.waypoint))
        {
            if (Mtrl == FloorMtrl.Grass)
            {
                Instantiate(FootStepPrefab).GetComponent<FootStep>().Range(walkClip[3], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV1)
            {
                Instantiate(FootStepPrefab).GetComponent<FootStep>().Range(walkClip[5], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV2)
            {
                Instantiate(FootStepPrefab).GetComponent<FootStep>().Range(walkClip[7], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Tile)
            {
                Instantiate(FootStepPrefab).GetComponent<FootStep>().Range(walkClip[1], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Wood)
            {
                Instantiate(FootStepPrefab).GetComponent<FootStep>().Range(walkClip[9], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            audTime = 0.0f;
        }
        else if(audTime >= 0.3f && CurAnimState ==AnimState.trace)  //쫓아올때
        {
            if (Mtrl == FloorMtrl.Grass)
            {
                Instantiate(FootStepPrefab).GetComponent<FootStep>().Range(walkClip[2], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV1)
            {
                Instantiate(FootStepPrefab).GetComponent<FootStep>().Range(walkClip[4], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.MetalV2)
            {
                Instantiate(FootStepPrefab).GetComponent<FootStep>().Range(walkClip[6], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Tile)
            {
                Instantiate(FootStepPrefab).GetComponent<FootStep>().Range(walkClip[0], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            else if (Mtrl == FloorMtrl.Wood)
            {
                Instantiate(FootStepPrefab).GetComponent<FootStep>().Range(walkClip[8], FootStepPosition, 2, 2f);   //발자국 사운드
            }
            audTime = 0.0f;
        }
        
    }
}
