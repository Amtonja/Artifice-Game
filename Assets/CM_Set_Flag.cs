using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CM_Set_Flag : MonoBehaviour
{
    public string cutsceneName;
    
    public void Activate()
    {
        DialogueLua.SetVariable(cutsceneName, true);
        Debug.Log("Cutscene " + cutsceneName + " flagged as complete");
    }
}
