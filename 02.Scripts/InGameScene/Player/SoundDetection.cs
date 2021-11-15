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
    Vector3 MinEnemyVec = Vector3.zero; //가장 가까운 적
    void Awake()
    {
        DetectionImg = Detection_Pivot.GetComponentInChildren<Image>();
    }
    // Start is called before the first frame update
    void Start()
    {
        DetectionFade = 140.0f;
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
        MinDetection = 5.0f;   //가장 가까운 적과의 거리 초기화
        MinEnemyVec = Vector3.zero;
        //플레이어가 움직이기에 항상 초기화 해줘야 한다. 안그러면 한 번 정해진 가까운 거리 외의 EnemySound는 감지되지 않는다.

        //--------------------EnemySound 감지
        for (int i = 0; i < EnemySoundList.Length; i++)
        {
            Vector3 DetectionDir = EnemySoundList[i].transform.position - a_RefPlayer.transform.position;
            
            if (DetectionDir.magnitude <= 5.0f)    //감지 범위 내에 적이 있는지
            {
                if (DetectionDir.magnitude <= MinDetection) //가장 가까운 적과의 거리
                {
                    MinDetection = DetectionDir.magnitude;
                    MinEnemyVec = EnemySoundList[i].transform.position;
                }

                //---------가장 가까운 적과의 거리에 반비례하여 DetectionImg의 알파값 변환
                if (DetectionFade < 17 * (10 - MinDetection))       //17 = 255/15
                    DetectionFade += GameMgr.Deltatime* 100; //Time.deltaTime * 100;

                if (DetectionFade >= 17 * (10 - MinDetection))
                    DetectionFade = 17 * (10 - MinDetection);

                DetectionImg.color = new Color32(0, 0, 0, (byte)DetectionFade);
                //---------가장 가까운 적과의 거리에 반비례하여 DetectionImg의 알파값 변환

                Vector3 EnemySoundDir = MinEnemyVec - a_RefPlayer.transform.position;    //감지방향
                EnemySoundDir.y = 0.0f; //y값은 무시

                //Detection_Pivot의 각도를 플레이어와 가장 가까운 EnemySound와의 각도로 변환
                Detection_Pivot.transform.localRotation = 
                    Quaternion.Euler(0, 0, Quaternion.FromToRotation(EnemySoundDir, a_RefPlayer.transform.forward).eulerAngles.y);  
                //EnemySoundDir와a_RefPlayer.transform.forward 두 벡터간의 각도 (0도 ~ 360도)
                //반시계방향으로 각도계산됨

            }//if (DetectionDir.magnitude <= 10.0f)    //감지 범위 내에 적이 있는지

        }//for (int i = 0; i < EnemySoundList.Length; i++)
        //--------------------EnemySound 감지

        //---------------감지된 적이 없으면 DetectionImg는 안보이도록
        if (MinEnemyVec == Vector3.zero)
        {
            if (DetectionFade > 0f)
                DetectionFade -= GameMgr.Deltatime * 50; //Time.deltaTime * 50;

            if (DetectionFade <= 0f)
                DetectionFade = 0f;

            DetectionImg.color = new Color32(0, 0, 0, (byte)DetectionFade);
        }
        //---------------감지된 적이 없으면 DetectionImg는 안보이도록

        #region -----계산중 Temp값 혹시 몰라서 남겨둠
        ////감지 이미지 FadeIn,Out
        //if (EnemySoundList.Length >= 1)
        //{
        //    if (DetectionFade < 140.0f + 11.5f * (10 - MinDetection))
        //        DetectionFade += Time.deltaTime * 50;

        //    if (DetectionFade >= 140.0f + 11.5f * (10 - MinDetection))
        //        DetectionFade = 140.0f + 11.5f * (10 - MinDetection);
        //    DetectionImg.color = new Color32(0, 0, 0, (byte)DetectionFade);

        //    //if (DetectionFade < 140.0f)
        //    //    DetectionFade += Time.deltaTime * 50;

        //    //if (DetectionFade >= 140.0f)
        //    //    DetectionFade = 140.0f;


        //}
        //else
        //{
        //    if (DetectionFade > 0f)
        //        DetectionFade -= Time.deltaTime * 50;

        //    if (DetectionFade <= 0f)
        //        DetectionFade = 0f;

        //    DetectionImg.color = new Color32(0, 0, 0, (byte)DetectionFade);
        //}

        //Vector3 EnemySoundDir = MinEnemyVec - a_RefPlayer.transform.position;    //감지방향
        //EnemySoundDir.y = 0.0f;
        //Detection_Pivot.transform.localRotation = Quaternion.Euler(0, 0, Quaternion.FromToRotation(EnemySoundDir, a_RefPlayer.transform.forward).eulerAngles.y);

        //감지 확인
        //for (int i = 0; i < EnemySoundList.Length; i++)
        //{
        //    Debug.Log(EnemySoundList[i].name.ToString());
        //}

        ////감지범위 이내의 소리만 감지
        //for (int i = 0; i < EnemySoundList.Length; i++)
        //{

        //}

        //감지 범위 내의 EnemySound방향으로 DetectionImg이미지 회전
        //for (int i = 0; i < EnemySoundList.Length; i++)
        //{
        //    Vector3 DetectionDir = EnemySoundList[i].transform.position - a_RefPlayer.transform.position;    //감지방향
        //    DetectionDir.y = 0.0f;
        //    Detection_Pivot.transform.localRotation = Quaternion.Euler(0, 0, Quaternion.FromToRotation(DetectionDir, a_RefPlayer.transform.forward).eulerAngles.y);
        //    //Debug.Log(EnemySoundList[i].name.ToString() + "와의 각도는 :" + Mathf.Acos(Vector3.Dot(DetectionDir, a_RefPlayer.transform.forward) / DetectionDir.magnitude / a_RefPlayer.transform.forward.magnitude) * Mathf.Rad2Deg);
        //    //Debug.Log(EnemySoundList[i].name.ToString() + "와의 각도는 :" + Quaternion.FromToRotation(DetectionDir, a_RefPlayer.transform.forward).eulerAngles.y);

        //    //Debug.Log(Vector3.Dot(DetectionDir, a_RefPlayer.transform.forward));
        //}
        //감지 범위 내의 EnemySound방향으로 DetectionImg이미지 회전
        #endregion
    }
}
