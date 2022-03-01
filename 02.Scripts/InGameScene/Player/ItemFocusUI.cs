using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemFocusUI : MonoBehaviour
{
    public GameObject Item_InteractionObj;
    public Text ImgText;
    public Text UIText;
    public static ItemFocusUI Inst = null;

    void Awake()
    {
        Inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        Item_InteractionObj.SetActive(GlobalValue.ItemUIOnOff);

        // "Use 키를 눌러 상호작용" 에서 Use키값
        ImgText.text = ((KeyCode)PlayerPrefs.GetInt("Use", (int)KeyCode.F)).ToString();
    }

    public void FocusUIText(string Txt)
    {
        UIText.text = Txt;
    }
}
