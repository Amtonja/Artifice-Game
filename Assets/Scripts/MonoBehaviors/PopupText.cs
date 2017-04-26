using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    float delay;
    float lifetime = 0f;

    void Start()
    {
        Animator anim = GetComponentInChildren<Animator>();
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        delay = clipInfo[0].clip.length;
        //Destroy(gameObject, clipInfo[0].clip.length);
    }	    

    void Update()
    {
        lifetime += Time.unscaledDeltaTime;
        if (lifetime >= delay)
        {
            Destroy(gameObject);
        }
    }
}
