using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

[RequireComponent(typeof(SpriteRenderer))]
public class PersistentLayerData : MonoBehaviour
{
    public void OnRecordPersistentData()
    {
        DialogueLua.SetVariable(name + " Layer", GetComponent<SpriteRenderer>().sortingLayerID);
    }

    public void OnApplyPersistentData()
    {
        GetComponent<SpriteRenderer>().sortingLayerID = DialogueLua.GetVariable(name + " Layer").AsInt;
    }
}
