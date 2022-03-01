using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    [SerializeField]
    private Text Q1;
    [SerializeField]
    private Text Q2;
    [SerializeField]
    private Text Q3;

    Color RGB;
    // Start is called before the first frame update
    void Start()
    {
        RGB = Color.gray;
        RGB.a = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalValue.PlayerInvenList == null)
        {
            return;
        }

        if (GlobalValue.PlayerInvenList.Count == 0)
        {
            return;
        }

        if (GlobalValue.PlayerInvenList.Count < 1)
        {
            return;
        }

        Q1.text = "1층 열쇠 1 / 1";
        Q1.color = RGB;

        if (GlobalValue.PlayerInvenList.Count < 2)
        {
            return;
        }

        Q2.text = "2층 열쇠 1 / 1";
        Q2.color = RGB;

        if (GlobalValue.PlayerInvenList.Count < 3)
        {
            return;
        }
        
        Q3.text = "지하실 열쇠 1 / 1";
        Q3.color = RGB;

        //if (GlobalValue.PlayerInvenList != null)
        //{
        //    if(GlobalValue.PlayerInvenList.Count != 0)
        //    {
        //        if (GlobalValue.PlayerInvenList.Count >= 1)
        //        {
        //            Q1.text = "1층 열쇠 1 / 1";
        //            Q1.color = RGB;
        //            if (GlobalValue.PlayerInvenList.Count >= 2)
        //            {
        //                Q2.text = "2층 열쇠 1 / 1";
        //                Q2.color = RGB;
        //                if (GlobalValue.PlayerInvenList.Count >= 3)
        //                {
        //                    Q3.text = "지하실 열쇠 1 / 1";
        //                    Q3.color = RGB;
        //                }
        //            }
        //        }
        //    }
        //}
    }
}
