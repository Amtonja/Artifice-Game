using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveFileButton : MonoBehaviour
{
    private int fileNumber;
    private Text fileNumberDisplay;
    private Text informationDisplay; 

    // Use this for initialization
    void Awake()
    {
        fileNumberDisplay = transform.GetChild(0).GetComponent<Text>();       
        informationDisplay = transform.GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowFileNumber()
    {
        fileNumberDisplay.text = FileNumber.ToString();
    }

    public void ShowInformation(string timestamp, string location, string elapsedTime)
    {
        //fileNumberDisplay.text = "File " + FileNumber.ToString();        

        //fileNumberDisplay.text = "File 01";

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append(timestamp);
        sb.Append(",");
        sb.Append(location);
        sb.Append(",");
        sb.Append(elapsedTime);

        informationDisplay.text = sb.ToString();
    }

    public void Save()
    {
        SaveManager.instance.SaveGame(FileNumber);
    }

    public void Load()
    {
        SaveManager.instance.LoadGame("Save" + FileNumber.ToString() + ".binary");
    }

    public int FileNumber
    {
        get
        {
            return fileNumber;
        }

        set
        {
            fileNumber = value;                        
        }
    }
}
