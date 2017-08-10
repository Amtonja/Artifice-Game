using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class OranElevator : MonoBehaviour
{
    private AudioSource _audio;
    private float audioTimer = 0f;
    private float audioThreshold;

    // Use this for initialization
    void Start()
    {
        transform.hasChanged = false;
        _audio = GetComponent<AudioSource>();
        audioThreshold = _audio.clip.length;
    }

    // Update is called once per frame
    void Update()
    {
        audioTimer += Time.deltaTime;
        if (transform.hasChanged && audioTimer >= audioThreshold)
        {
            _audio.Play();
            audioTimer = 0f;
            transform.hasChanged = false;
        }
    }
}
