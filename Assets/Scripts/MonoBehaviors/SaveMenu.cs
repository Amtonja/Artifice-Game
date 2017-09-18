using UnityEngine;
using System.IO;

public class SaveMenu : MonoBehaviour
{ 
    public GameObject saveFileDisplayPrefab;    
    public static int numSaveFiles = 99;

    public Transform contentPane;

    // Use this for initialization
    void Start()
    {        
        for (int i = 0; i < numSaveFiles; i++)
        {
            // Load in save files and such
            GameObject currentFile = Instantiate(saveFileDisplayPrefab, contentPane, false);
            //currentFile.transform.FindChild("FileNumber").GetComponent<Text>().text = "File " + (i + 1).ToString();
            currentFile.GetComponent<SaveFileButton>().FileNumber = i + 1;
            currentFile.GetComponent<SaveFileButton>().ShowFileNumber();
            if (i < SaveManager.instance.MetadataList.Count)
            {
                SaveGameMetadata currentMetadata = SaveManager.instance.MetadataList[i];
                currentFile.GetComponent<SaveFileButton>().ShowInformation(currentMetadata.timeStamp, currentMetadata.location,
                    currentMetadata.elapsedTime);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
