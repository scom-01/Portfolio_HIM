using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue : MonoBehaviour
{
    public static float Deltatime = 0.02f;      //일시정지를 위한 Deltatime

    public static float PlayerHeight = 1.2f;    //플레이어의 높이

    public static bool ItemUIOnOff = false;     //아이템UI On/Off
    public static bool SettingBool = false;     //Setting Panel On/Off

    public static float mouseSpeed = 5.0f;      //MouseSpeed

    public static float BGM_Value = 0.0f;       //BackGroundMusic
    public static int BGM_Bool = 1;             //PlayerPrefs로 저장하고 불러오기위해
    public static float SE_Value = 0.0f;        //SoundEffect
    public static int SE_Bool = 1;              //PlayerPrefs로 저장하고 불러오기위해

    public static bool FlashOnOff = false;      //Flash On Off
    public static float FlashGage = 100.0f;     //Flash 게이지 퍼센트
    public static float Blink_Alpha = 0.5f;     //Flash Alpha값

    public static List<string> PlayerInvenList = new List<string>();    //열쇠정보를 저장하는 리스트
    //public static List<GameObject> EnemyInvenList;

    public static GameState g_GameState = GameState.Starting;

    //KeySetting
    public static KeyCode Move_Forward;
    public static KeyCode Move_Left;
    public static KeyCode Move_Right;
    public static KeyCode Move_Back;
    public static KeyCode Peeking_Left;
    public static KeyCode Peeking_Right;
    public static KeyCode Sprint;
    public static KeyCode Sit;
    public static KeyCode Use;
    public static KeyCode Detection;
    //KeySetting
}

//바닥재질에 따른 발소리 전환을 위한 enum
public enum FloorMtrl
{
    Grass,
    Graval,
    MetalV1,
    MetalV2,
    Tile,
    Wood
}