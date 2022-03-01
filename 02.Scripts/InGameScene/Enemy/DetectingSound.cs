using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectingSound : MonoBehaviour
{
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
        }
    }
}
