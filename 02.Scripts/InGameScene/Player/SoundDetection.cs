using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundDetection : MonoBehaviour
{
    public GameObject Detection_Pivot;
    Image DetectionImg;
    float DetectionFade = 140.0f;
    float MinDetection = 15.0f;         //감지 범위 내에 가장 가까운 적과의 거리
    float DetectionDistance = 10.0f;

    Vector3 MinEnemyVec = Vector3.zero; //가장 가까운 적
    void Awake()
    {
        DetectionImg = Detection_Pivot.GetComponentInChildren<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        DetectionFade = 140.0f;
        DetectionDistance = 10.0f;
        MinDetection = 15.0f;
        MinEnemyVec = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Detection_EnemySound();
    }

    void Detection_EnemySound()
    {
        GameObject[] EnemySoundList;
        EnemySoundList = GameObject.FindGameObjectsWithTag("E_Sound");   //적 사운드 리스트를 모아서
        GameObject a_RefPlayer = NavCam.Inst.transform.gameObject;
        MinDetection = 10.0f;   //가장 가까운 적과의 거리 초기화
        MinEnemyVec = Vector3.zero;
        //플레이어가 움직이기에 항상 초기화 해줘야 한다. 안그러면 한 번 정해진 가까운 거리 외의 EnemySound는 감지되지 않는다.

        //--------------------EnemySound 감지
        for (int i = 0; i < EnemySoundList.Length; i++)
        {
            Vector3 DetectionDir = EnemySoundList[i].transform.position - a_RefPlayer.transform.position;
            
            if (DetectionDir.magnitude <= DetectionDistance)    //감지 범위 내에 적이 있는지
            {
                if (DetectionDir.magnitude <= MinDetection) //가장 가까운 적과의 거리
                {
                    MinDetection = DetectionDir.magnitude;
                    MinEnemyVec = EnemySoundList[i].transform.position;
                }

                //---------가장 가까운 적과의 거리에 반비례하여 DetectionImg의 알파값 변환
                if (DetectionFade < 17 * (DetectionDistance - MinDetection))       //17 = 255/15
                    DetectionFade += GlobalValue.Deltatime* 100; //Time.deltaTime * 100;

                if (DetectionFade >= 17 * (DetectionDistance - MinDetection))
                    DetectionFade = 17 * (DetectionDistance - MinDetection);

                DetectionImg.color = new Color32(255, 255, 255, (byte)DetectionFade);
                //---------가장 가까운 적과의 거리에 반비례하여 DetectionImg의 알파값 변환

                Vector3 EnemySoundDir = MinEnemyVec - a_RefPlayer.transform.position;    //감지방향
                EnemySoundDir.y = 0.0f; //y값은 무시

                //Detection_Pivot의 각도를 플레이어와 가장 가까운 EnemySound와의 각도로 변환
                Detection_Pivot.transform.localRotation = 
                    Quaternion.Euler(0, 0, Quaternion.FromToRotation(EnemySoundDir, a_RefPlayer.transform.forward).eulerAngles.y);
                //EnemySoundDir와a_RefPlayer.transform.forward 두 벡터간의 각도 (0도 ~ 360도)
                //반시계방향으로 각도계산됨

            }//if (DetectionDir.magnitude <= DetectionDistance)    //감지 범위 내에 적이 있는지

        }//for (int i = 0; i < EnemySoundList.Length; i++)
        //--------------------EnemySound 감지

        //---------------감지된 적이 없으면 DetectionImg는 안보이도록
        if (MinEnemyVec == Vector3.zero)
        {
            DetectionImg.color = new Color32(255, 255, 255, 0);
        }
        //---------------감지된 적이 없으면 DetectionImg는 안보이도록

    }
}
