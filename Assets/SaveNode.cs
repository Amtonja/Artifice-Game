using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class SaveNode : MonoBehaviour
{
    public void Activate()
    {
        string saveGameData = PersistentDataManager.GetSaveData();
        PlayManager.instance.OutputSaveGame(saveGameData);
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
