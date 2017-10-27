using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayTimer : MonoBehaviour
{
    private int playTime;       

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        StartCoroutine(Timer());
    }

    // Update is called once per frame
    void Update()
    {

    }

    private IEnumerator Timer()
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            playTime++;
        }
    }

    public void Reset()
    {
        playTime = 0;
    }

    public void OnRecordPersistentData()
    {
        PixelCrushers.DialogueSystem.DialogueLua.SetVariable("PlayTime", playTime);
    }

    public void OnApplyPersistentData()
    {
        playTime = PixelCrushers.DialogueSystem.DialogueLua.GetVariable("PlayTime").AsInt;
    }

    public int PlayTime
    {
        //set
        //{
        //    playTime = value;
        //}
        get
        {
            return playTime;
        }
    }
}
