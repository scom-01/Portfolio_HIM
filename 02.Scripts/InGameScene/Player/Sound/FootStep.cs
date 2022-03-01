using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootStep : MonoBehaviour
{
    public SphereCollider DetectionRange_spherecoll;
    float MaxSize = 5.0f;
    float FootStepSoundCountTime;
    AudioSource aud;
    private void Awake()
    {
        aud = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    void Start()
    {
        MaxSize = 5.0f;        
    }

    // Update is called once per frame
    void Update()
    {
        if (FootStepSoundCountTime > 0.0f)
        {
            FootStepSoundCountTime -= GlobalValue.Deltatime;
            if (DetectionRange_spherecoll.radius < MaxSize)
                DetectionRange_spherecoll.radius += GlobalValue.Deltatime;
            else
                DetectionRange_spherecoll.radius = MaxSize;
        }
        else if (FootStepSoundCountTime <= 0.0f)
        {
            Destroy(this.gameObject);
        }

    }

    public void Range(AudioClip audClip, Vector3 position, float RadiusSize, float CountTime = 3.0f)
    {
        if (this.tag.Contains("TargetSound"))
            aud.PlayOneShot(audClip, GlobalValue.SE_Value * GlobalValue.SE_Bool * 0.5f);    //플레이어 본인 발소리는 작게
        else
            aud.PlayOneShot(audClip, GlobalValue.SE_Value * GlobalValue.SE_Bool);

        this.transform.position = position;
        MaxSize = RadiusSize;
        aud.maxDistance = RadiusSize * 8 ;
        FootStepSoundCountTime = CountTime;
    }
}
