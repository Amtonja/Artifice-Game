using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LoadGameButton : MonoBehaviour
{
    public string savePath;

    // Use this for initialization
    void Start()
    {
        if (Directory.GetFiles(savePath, "Save*.binary").Length <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnLoad()
    {
        SaveManager.instance.OpenLoadMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
