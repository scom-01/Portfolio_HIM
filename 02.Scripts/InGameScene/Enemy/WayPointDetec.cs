using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPointDetec : MonoBehaviour
{
    private void OnTriggerExit(Collider coll)
    {
        if (EnemyCtrl.Inst.CurAnimState == AnimState.trace || EnemyCtrl.Inst.CurAnimState == AnimState.attack)    //공격중이거나 추적이 아닐 때
            return;

        if (coll.tag.Contains("WayPoint"))
        {
            if (coll.GetComponent<WayPointAI>().WayPosCount == 1 && EnemyCtrl.Inst.Go_WayPointTime == 0.0f)
            {
                EnemyCtrl.Inst.Go_WayPointTime = 5.0f;
            }
        }
    }
}
