using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadMgr : MonoBehaviour
{
    static string nextScene;

    [SerializeField]
    Image FillAmountBar_Img;
    [SerializeField]
    Text Progress_Text;

    public static void LoadScene(string SceneName)
    {
        nextScene = SceneName;
        SceneManager.LoadScene("LoadingScene");
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene_Co());
    }

    IEnumerator LoadScene_Co()
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(nextScene);        
        ao.allowSceneActivation = false;    //90퍼센트 이상 로드 시 씬을 자동으로 로드 할 것 인가 여부

        float timer = 0.0f;
        while (!ao.isDone)   //씬 로드가 끝나지 않았다면
        {
            yield return null;  //화면 갱신을 위해 제어권을 넘김 

            if (ao.progress < 0.9f)   //진행도
            {
                FillAmountBar_Img.fillAmount = ao.progress;
                Progress_Text.text = "Loading . . .  " + (FillAmountBar_Img.fillAmount).ToString("P0");
            }
            else
            {
                timer += Time.unscaledDeltaTime;
                FillAmountBar_Img.fillAmount = Mathf.Lerp(0.9f, 1.0f, timer);
                Progress_Text.text = "Loading . . .  " + (FillAmountBar_Img.fillAmount).ToString("P0");
                if (FillAmountBar_Img.fillAmount >= 1.0f)
                {
                    ao.allowSceneActivation = true;
                    yield break;
                }
               
            }
        }
    }
}
