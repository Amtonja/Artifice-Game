using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoogleSheetsSettings : ScriptableObject {

    public string[] sheetTitles;

    public static string settingsAssetPath = "Assets/Resources/GoogleSheets/GoogleSheetsSettings.asset";


#if UNITY_EDITOR
    public string sheetsURL = "";
    public string specificSheets = "";
#endif
}
