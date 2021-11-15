using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingSound : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (EnemyCtrl.Inst.CurAnimState == AnimState.trace || EnemyCtrl.Inst.CurAnimState == AnimState.attack)    //공격중이거나 추적이 아닐 때
            return;

        if (coll.tag.Contains("TargetSound"))
        {
            //소리 감지
            EnemyCtrl.Inst.CurAnimState = AnimState.detec_walk;
            EnemyCtrl.Inst.AggroTargetPos = coll.transform.position;
            //소리 감지

            //Vector3 DetecVec = coll.transform.position - this.transform.position;
            //float DetecDist = DetecVec.magnitude;
            //if(DetecDist <=)
        }
    }
}
