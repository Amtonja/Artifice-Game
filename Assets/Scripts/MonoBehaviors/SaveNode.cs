using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class SaveNode : MonoBehaviour
{
    public AreaInfo area;

    public void Activate()
    {
        DialogueLua.SetVariable("AreaInfo", area.name);
        SaveManager.instance.OpenSaveMenu();
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
