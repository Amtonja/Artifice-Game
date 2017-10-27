using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class ContinueButton : MonoBehaviour
{
    public string savePath;
    private FileInfo lastSave;

    // Use this for initialization
    void Start()
    {
        if (Directory.GetFiles(savePath, "Save*.binary").Length <= 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            DirectoryInfo directory = new DirectoryInfo(savePath);
            lastSave = directory.GetFiles("Save*.binary").OrderByDescending(f => f.LastWriteTime).First();
        }

    }

    public void OnContinue()
    {
        if (lastSave != null)
        {
            Debug.Log("Loading saved game " + lastSave.Name);
            SaveManager.instance.LoadGame(lastSave.Name);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
