using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonToggle : MonoBehaviour
{
    public bool Toggle = false;
    public GameObject Obj = null;
    // Start is called before the first frame update
    void Start()
    {
        Toggle = false;
        Obj.gameObject.SetActive(Toggle);
    }

    // Update is called once per frame
    void Update()
    {
        Obj.gameObject.SetActive(Toggle);
    }
}
