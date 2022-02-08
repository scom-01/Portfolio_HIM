using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEnd : MonoBehaviour
{
    [SerializeField]
    private OpenDoor m_OpenDoor;
    bool End;
    // Start is called before the first frame update
    void Start()
    {
        End = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_OpenDoor.open && End)
        {
            GlobalValue.g_GameState = GameState.End;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(m_OpenDoor.open && other.tag == "Player")
        {
            End = true;
        }
    }
}
