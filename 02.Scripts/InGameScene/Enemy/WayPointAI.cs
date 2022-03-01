using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointAI : MonoBehaviour
{
    public GameObject[] WayPosObj;

    [HideInInspector] public GameObject NextWayObj;
    [HideInInspector] public GameObject BeforeWayObj;
    //private Transform[] points;
    [HideInInspector] public int WayPosCount;

    //GameObject[] RandomNextWayObj;
    List<GameObject> RandomNextWayObj = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        WayPosCount = WayPosObj.Length;
    }

    private void OnDrawGizmos()
    {
        if (WayPosObj == null)
            return;

        //라인 색상 지정
        Gizmos.color = Color.white;
        //WayPointGroup 게임오브젝트 아래에 있는 모든 Point 게임오브젝트 추출
        for (int i = 0; i < WayPosCount; i++)
        {
            //points[i] = WayPosObj[i].GetComponent<Transform>();
            Gizmos.DrawLine(this.transform.position, WayPosObj[i].GetComponent<Transform>().position);
        }
    }
    void WayPointListInit()
    {
        if (RandomNextWayObj.Count > 0)
        {
            RandomNextWayObj.RemoveRange(0, RandomNextWayObj.Count);
        }

        for (int i = 0; i < WayPosCount; i++)
        {
            RandomNextWayObj.Add(WayPosObj[i]);
        }

    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.tag.Contains("Enemy"))  //Enemy의 Capsule Collider
        {
            if (coll.transform.parent.GetComponent<EnemyCtrl>().CurAnimState == AnimState.waypoint)  //waypoint를 이용해 이동하던 중에 도착했을 때만
            {
                //Debug.Log("적 waypoint옴!");
                if (WayPosCount > 1) //이동할 WayPoint가 1개일때는 제외. 즉, 더이상 갈 곳이 없는 방에서 방 밖으로 나가게하기
                {
                    WayPointListInit();
                    if (coll.transform.parent.GetComponent<EnemyCtrl>().BeforeObj != null)
                    {
                        for (int i = 0; i < RandomNextWayObj.Count; i++)
                        {
                            if (RandomNextWayObj[i].name == coll.transform.parent.GetComponent<EnemyCtrl>().BeforeObj.name)
                            {
                                RandomNextWayObj.RemoveAt(i);   //그 전에 왔던 WayPoint는 배제
                                break;
                            }
                        }
                        NextWayObj = RandomNextWayObj[Random.Range(0, RandomNextWayObj.Count)]; //그 전에 왔던 WayPoint는 배제한 랜덤 오브젝트
                        coll.transform.parent.GetComponent<EnemyCtrl>().BeforeObj = this.gameObject;  //오브젝트정보넘기기
                        EnemyCtrl.Inst.AggroTargetPos = NextWayObj.transform.position;
                    }
                    else
                    {
                        NextWayObj = WayPosObj[Random.Range(0, WayPosCount)];   //지정된 WayPoint중 하나 임의로 이동
                        coll.transform.parent.GetComponent<EnemyCtrl>().BeforeObj = this.gameObject;  //오브젝트정보넘기기
                        EnemyCtrl.Inst.AggroTargetPos = NextWayObj.transform.position;
                    }
                }
                else
                {
                    //Debug.Log("막다른 방");
                    coll.transform.parent.GetComponent<EnemyCtrl>().BeforeObj = this.gameObject;
                    //Debug.Log(this.gameObject.name);
                    coll.transform.parent.GetComponent<EnemyCtrl>().MySetAnim(AnimState.idle);
                    coll.transform.parent.GetComponent<EnemyCtrl>().CurAnimState = AnimState.idle;
                    coll.transform.parent.GetComponent<EnemyCtrl>().Go_WayPointTime = 3.0f;
                    NextWayObj = WayPosObj[Random.Range(0, WayPosCount)];   //지정된 WayPoint중 하나 임의로 이동 
                }
            }
            //Debug.DrawLine(this.transform.position, NextWayObj.transform.position, Color.red);
        }
    }
}
