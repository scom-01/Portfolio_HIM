using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private RaycastHit hit;
        
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //메인 카메라에서 마우스 커서의 위치로 캐스팅되는 Ray를 생성
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //생성된 Ray를 Scene 뷰에 녹색 광선으로 표현
        Debug.DrawRay(ray.origin, ray.direction * 0.7f, Color.white);

        if (Physics.Raycast(ray, out hit, 0.7f/*Mathf.Infinity*/))//, 1 << LayerMask.NameToLayer("ITEM"))) //<== 관통하면서 LayerMask판단
            //벽뒤에서 오브젝트를 집지 못하도록
        {
            if (hit.transform.gameObject.layer == LayerMask.NameToLayer("ITEM"))    //처음맞은 오브젝트 판단  //태그로 판단가능
            {
                Debug.Log("키");
                string str = hit.transform.gameObject.name;
                if(str.Contains("key 1"))
                {
                    str = "1층 열쇠";
                }
                else if (str.Contains("key 2"))
                {
                    str = "2층 열쇠";
                }
                else if (str.Contains("key 3"))
                {
                    str = "지하실 열쇠";
                }
                GlobalValue.ItemUIOnOff = true;
                ItemFocusUI.Inst.FocusUIText("키를 눌러 "+ str + " 줍기");

                if(Input.GetKeyDown(ControllSetting.Use))
                {
                    Debug.Log(hit.transform.gameObject);
                    GlobalValue.PlayerInvenList.Add(hit.transform.name);  //인벤토리 리스트에 추가
                    Destroy(hit.transform.gameObject);              //Scene에서 삭제
                }
            }
            else if(hit.transform.tag == "Door")
            {
                GlobalValue.ItemUIOnOff = true;
                if(hit.transform.GetComponent<OpenDoor>().Key != null)
                {
                    Debug.Log(hit.transform.GetComponent<OpenDoor>().Key.name);
                    string str = "";
                    if(GlobalValue.PlayerInvenList.Count != 0)
                        for (int i = 0; i < GlobalValue.PlayerInvenList.Count; i++)
                        {
                            if (GlobalValue.PlayerInvenList[i].Contains(hit.transform.GetComponent<OpenDoor>().Key.name))    //플레이어가 키가 있는지 확인
                            {
                                str = "키를 눌러 상호작용";
                                break;
                            }
                            else
                            {
                                if (hit.transform.GetComponent<OpenDoor>().Key.name.Contains("1"))
                                    str = "1층 열쇠가 필요한 듯 하다.";
                                else if (hit.transform.GetComponent<OpenDoor>().Key.name.Contains("2"))
                                    str = "2층 열쇠가 필요한 듯 하다.";
                                else if (hit.transform.GetComponent<OpenDoor>().Key.name.Contains("3"))
                                    str = "지하실 열쇠가 필요한 듯 하다.";
                            }
                        }
                    else
                    {
                        str = "열쇠가 필요한 듯 하다.";
                    }
                    ItemFocusUI.Inst.FocusUIText(str);
                }
                else
                {
                    ItemFocusUI.Inst.FocusUIText("키를 눌러 상호작용");
                }
            }
            else
            {
                GlobalValue.ItemUIOnOff = false;
            }
        }
        else
        {
            GlobalValue.ItemUIOnOff = false;
        }

    }
}
