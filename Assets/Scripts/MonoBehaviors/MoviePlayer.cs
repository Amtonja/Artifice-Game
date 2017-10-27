using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoviePlayer : MonoBehaviour
{
    RawImage ri;
    MovieTexture mt;
    //AudioSource _audio;

    // Use this for initialization
    void Start()
    {
        ri = GetComponent<RawImage>();
        mt = ri.texture as MovieTexture;
        //_audio.clip = mt.audioClip;

        mt.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mt.isPlaying || Input.GetButtonDown("Cancel"))
        {
            mt.Stop();
            ri.transform.SetAsFirstSibling();   
        }
    }
}
