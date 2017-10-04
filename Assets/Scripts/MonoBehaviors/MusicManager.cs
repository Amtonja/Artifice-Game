using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    private AudioSource musicSource;

    public AudioClip BGM;

    // Use this for initialization
    void Start()
    {
        instance = this;
        instance.musicSource = instance.GetComponent<AudioSource>();

        PlayBGM();
    }

    // Update is called once per frame
    void Update()
    {       
        
    }

    public IEnumerator PlayCombatMusic()
    {
        AudioClip combatIntro = Resources.Load("Music/Combat Theme A - Intro") as AudioClip;
        AudioClip combatLoop = Resources.Load("Music/Combat Theme A - Main Loop") as AudioClip;
        musicSource.clip = combatIntro;
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
        musicSource.clip = combatLoop;
        musicSource.loop = true;
        musicSource.Play();
    }
    
    public IEnumerator PlayCombatEnding()
    {
        AudioClip fanfare = Resources.Load("Music/Victory Fanfare - Triumph") as AudioClip;
        musicSource.clip = fanfare;
        musicSource.Play();
        yield return new WaitForSeconds(musicSource.clip.length);
        PlayBGM();      
    }    

    public void PlayBGM()
    {
        musicSource.clip = BGM;
        musicSource.Play();
    }
}
