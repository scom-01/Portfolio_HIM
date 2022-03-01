using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [HideInInspector] public float Hp = 100;

    public AudioClip audClip;
    public AudioSource aud;

    public static PlayerState Inst;

    void Awake()
    {
        Inst = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Hp < 100.0f)
        {
            if (Hp < 0.0f)
            {
                Hp = 0.0f;
                GlobalValue.g_GameState = GameState.Die;    //체력소모시 사망
                return;
            }

            Hp += GlobalValue.Deltatime * 2;        //초당 2씩 체력회복

            if (Hp >= 100.0f)
                Hp = 100.0f;

            if(GlobalValue.SettingBool)
            {
                aud.Pause();
            }
            else
            {
                aud.UnPause();
            }

            if (!aud.isPlaying)
            {
                aud.volume = GlobalValue.SE_Value * GlobalValue.SE_Bool;
                aud.Play();
            }
        }
        else
        {
            aud.Stop();
        }

        FindObjectOfType<GameMgr>().Hp_Img.fillAmount = Hp / 100.0f;
    }
}
