using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

[RequireComponent(typeof(Collider2D))]
public class PersistentCollisionData : MonoBehaviour
{    
    public void OnRecordPersistentData()
    {
        DialogueLua.SetVariable(name + " Collision", GetComponent<Collider2D>().enabled);   
    }

    public void OnApplyPersistentData()
    {
        GetComponent<Collider2D>().enabled = DialogueLua.GetVariable(name + " Collision").AsBool;
    }
}
