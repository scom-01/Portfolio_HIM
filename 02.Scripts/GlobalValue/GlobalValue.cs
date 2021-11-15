using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalValue : MonoBehaviour
{
    public static bool FPSON;
    public static float PlayerHeight = 1.2f;

    public static bool ItemUIOnOff = false;
    public static bool SettingBool = false;

    public static float mouseSpeed = 5.0f;      //MouseSpeed

    public static float BGM_Value = 0.0f;       //BackGroundMusic
    public static int BGM_Bool = 1;
    public static float SE_Value = 0.0f;        //SoundEffect
    public static int SE_Bool = 1;

    public static bool FlashOnOff = false;

    public static List<string> PlayerInvenList = new List<string>();
    //public static List<GameObject> EnemyInvenList;

    public static GameState g_GameState = GameState.Starting;

}

public enum FloorMtrl
{
    Grass,
    Graval,
    MetalV1,
    MetalV2,
    Tile,
    Wood
}