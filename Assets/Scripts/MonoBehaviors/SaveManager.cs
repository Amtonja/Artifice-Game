using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{   
    /// <summary>
    /// Singleton instance
    /// </summary>
    public static SaveManager instance;

    private GameObject menu;

    private List<SaveGameMetadata> metadataList;    

    // Use this for initialization
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SaveGame(int fileNumber)
    {
        string saveGameData = PersistentDataManager.GetSaveData();       

        if (!Directory.Exists("Saves"))
        {
            Directory.CreateDirectory("Saves");
        }

        string savepath = "Saves/Save" + fileNumber + ".binary";
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(savepath);

        SaveGameMetadata myMetadata = GetMetadata(fileNumber);

        // Find out if an entry for this file is already in the list and overwrite it if so
        SaveGameMetadata entry = MetadataList.Find(x => x.fileNumber == fileNumber);
        if (entry != null)
        {
            MetadataList[MetadataList.IndexOf(entry)] = myMetadata;
        }
        else
        {
            MetadataList.Insert(fileNumber - 1, myMetadata);
        }

        SaveAllMetadata();

        formatter.Serialize(saveFile, saveGameData);

        saveFile.Close();

        Debug.Log("The game has been saved in file " + fileNumber);

        CloseMenu();
    }

    public void LoadGame(string fileName)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open("Saves/" + fileName, FileMode.Open);

        string saveData = formatter.Deserialize(saveFile) as string;

        //PersistentDataManager.ApplySaveData(saveData);
        DialogueManager.Instance.GetComponent<LevelManager>().LoadGame(saveData);


        saveFile.Close();        
    }

    SaveGameMetadata GetMetadata(int fileNumber)
    {
        SaveGameMetadata data = new SaveGameMetadata();

        data.fileNumber = fileNumber;
        data.timeStamp = System.DateTime.Now.ToString();
        data.location = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        data.elapsedTime = GameObject.Find("PlayTimer").GetComponent<PlayTimer>().PlayTime;
        
        return data;
    }

    void SaveAllMetadata()
    {
        string path = "Saves/savedata.json";

        string json = JsonHelper.ToJson(MetadataList.ToArray());
                
        File.WriteAllText(path, json);
    }

    List<SaveGameMetadata> LoadAllMetadata()
    {
        List<SaveGameMetadata> mdList = new List<SaveGameMetadata>();

        string path = "Saves/savedata.json";

        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);

            mdList = new List<SaveGameMetadata>(JsonHelper.FromJson<SaveGameMetadata>(data));
        }

        return mdList;
    }

    public void OpenSaveMenu()
    {
        MetadataList = LoadAllMetadata();
        menu = Instantiate(Resources.Load("Prefabs/SaveMenu")) as GameObject;                
    }

    public void OpenLoadMenu()
    {
        MetadataList = LoadAllMetadata();
        menu = Instantiate(Resources.Load("Prefabs/LoadMenu")) as GameObject;
    }

    public void CloseMenu()
    {
        if (menu != null)
        {
            Destroy(menu);
        }
    }

    public List<SaveGameMetadata> MetadataList
    { 
        get
        {
            return metadataList;
        }

        set
        {
            metadataList = value;
        }
    }
}

/// <summary>
/// Represents the metadata we want to track for each save game file
/// so that it can be displayed to the user in the save menu.
/// </summary>
[System.Serializable]
public class SaveGameMetadata
{
    public int fileNumber;
    public string timeStamp; // Real-world time when the save was made
    public string location; // Name of the in-game location where the save was made
    public int elapsedTime; // hh:mm:ss since this game was started
}
