using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OranPortal : MonoBehaviour
{
    private AudioSource _audio;
    public AudioClip activationSound;
    public AudioClip runningSound;

    // Use this for initialization
    void Start()
    {
        _audio = GetComponent<AudioSource>();
        StartCoroutine(PlayAudio());
    }

    IEnumerator PlayAudio()
    {
        _audio.PlayOneShot(activationSound);
        yield return new WaitForSeconds(activationSound.length);
        _audio.clip = runningSound;
        _audio.loop = true;
        _audio.Play();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
