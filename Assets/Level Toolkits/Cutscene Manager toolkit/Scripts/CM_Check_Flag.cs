using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class CM_Check_Flag : MonoBehaviour
{
    public string cutsceneName;

    // Use this for initialization
    void Awake()
    {
        if (DialogueLua.DoesVariableExist(cutsceneName) && DialogueLua.GetVariable(cutsceneName).AsBool)
        {
            gameObject.SetActive(false);
        }   
        else if (DialogueLua.DoesVariableExist(cutsceneName) && !DialogueLua.GetVariable(cutsceneName).AsBool)
        {
            gameObject.SetActive(true);
        }
    }
}
