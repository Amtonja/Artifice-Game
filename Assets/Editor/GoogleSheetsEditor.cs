using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using Artifice.Data;
using Artifice.Items;

public class GoogleSheetsEditor : EditorWindow {
    private static GoogleSheetsSettings _settings = null;
    public static GoogleSheetsSettings settings {
        get {
            if (_settings == null) {
                if (File.Exists(GoogleSheetsSettings.settingsAssetPath))
                    _settings = (GoogleSheetsSettings)AssetDatabase.LoadAssetAtPath(GoogleSheetsSettings.settingsAssetPath, typeof(GoogleSheetsSettings));
                else {
                    _settings = (GoogleSheetsSettings)CreateInstance(typeof(GoogleSheetsSettings));
                    string settingsPath = Path.GetDirectoryName(GoogleSheetsSettings.settingsAssetPath);
                    if (!Directory.Exists(settingsPath)) {
                        Directory.CreateDirectory(settingsPath);
                        AssetDatabase.ImportAsset(settingsPath);
                    }
                    AssetDatabase.CreateAsset(settings, GoogleSheetsSettings.settingsAssetPath);
                }
            }
            return _settings;
        }
    }

    private const string SKIP = "#!#";

    [MenuItem("Tools/Google Sheets")]
    static void OpenWindow() {
        GetWindow(typeof(GoogleSheetsEditor));
    }

    string gDocsURL {
        get { return settings.sheetsURL; }
        set {
            settings.sheetsURL = value;
            EditorUtility.SetDirty(settings);
        }
    }

    string SpecificSheets {
        get { return settings.specificSheets; }
        set {
            settings.specificSheets = value;
            EditorUtility.SetDirty(settings);
        }
    }

    int unresolvedErrors = 0;
    int foundSheets = 0;
    bool loadedSettings = false;

    void LoadSettings() {
        if (loadedSettings || settings == null)
            return;
        loadedSettings = true;
    }

    Vector2 scrollView;

    void OnGUI() {
        if (EditorApplication.isPlaying) {
            GUILayout.Label("Editor is in play mode.");
            return;
        }
        LoadSettings();

        scrollView = GUILayout.BeginScrollView(scrollView);

        GUILayout.Label("Settings", EditorStyles.boldLabel);
        gDocsURL = EditorGUILayout.TextField("gDocs Link", gDocsURL);
        if (GUI.changed) {
            SaveSettingsFile();
        }
        if (GUILayout.Button("Update All Data")) {
            if (gDocsURL.Contains("/pubhtml")) {
                //https://docs.google.com/spreadsheets/d/KEY/pubhtml            
                //try if CSV (still) works               
                string str = gDocsURL;
                int index = str.LastIndexOf("/");
                str = str.Substring(0, index);
                index = str.LastIndexOf("/");
                str = str.Substring(index + 1);
                str = "https://docs.google.com/spreadsheet/pub?key=" + str + "&output=csv&single=true&gid=0";
                WWW w = new WWW(str);
                while (!w.isDone) { }
                if (!w.responseHeaders.ContainsKey("STATUS") && !w.responseHeaders["STATUS"].Contains("404")) {
                    //CSV URL does not work
                } else {
                    gDocsURL = str;
                }
            }

            gDocsURL = gDocsURL.Trim();
            if (gDocsURL.Contains("&output")) {
                if (!gDocsURL.Contains("&single="))
                    gDocsURL += "&single=true";
                if (!gDocsURL.Contains("&gid="))
                    gDocsURL += "&gid=0";
                if (!gDocsURL.Contains("&output=csv"))
                    gDocsURL += "&output=csv";
                if (gDocsURL.Contains("&output=html"))
                    gDocsURL = gDocsURL.Replace("&output=html", "&output=csv");
            }
            if (!gDocsURL.Contains(".google.com")) {
                EditorUtility.DisplayDialog("Error", "You have entered an incorrect spreadsheet URL. Please read the manuals instructions (See readme.txt)", "OK");
            } else {
                EditorPrefs.SetString(PlayerSettings.productName + "gDocs", gDocsURL);
                LoadSettings(gDocsURL);
            }
        }
        if (unresolvedErrors > 0) {
            Rect rec = GUILayoutUtility.GetLastRect();
            GUI.color = Color.red;
            EditorGUI.DropShadowLabel(new Rect(0, rec.yMin + 15, 200, 20), "Unresolved errors: " + unresolvedErrors);
            GUI.color = Color.white;
        }

        SpecificSheets = EditorGUILayout.TextField("Specific Sheets", SpecificSheets);
        if (GUI.changed) {
            SaveSettingsFile();
        }
        if (GUILayout.Button("Update Specific Sheets")) {
            if (gDocsURL.Contains("/pubhtml")) {
                //https://docs.google.com/spreadsheets/d/KEY/pubhtml            
                //try if CSV (still) works               
                string str = gDocsURL;
                int index = str.LastIndexOf("/");
                str = str.Substring(0, index);
                index = str.LastIndexOf("/");
                str = str.Substring(index + 1);
                str = "https://docs.google.com/spreadsheet/pub?key=" + str + "&output=csv&single=true&gid=0";
                WWW w = new WWW(str);
                while (!w.isDone) { }
                if (!w.responseHeaders.ContainsKey("STATUS") && !w.responseHeaders["STATUS"].Contains("404")) {
                    //CSV URL does not work
                } else {
                    gDocsURL = str;
                }
            }

            gDocsURL = gDocsURL.Trim();
            if (gDocsURL.Contains("&output")) {
                if (!gDocsURL.Contains("&single="))
                    gDocsURL += "&single=true";
                if (!gDocsURL.Contains("&gid="))
                    gDocsURL += "&gid=0";
                if (!gDocsURL.Contains("&output=csv"))
                    gDocsURL += "&output=csv";
                if (gDocsURL.Contains("&output=html"))
                    gDocsURL = gDocsURL.Replace("&output=html", "&output=csv");
            }
            if (!gDocsURL.Contains(".google.com")) {
                EditorUtility.DisplayDialog("Error", "You have entered an incorrect spreadsheet URL. Please read the manuals instructions (See readme.txt)", "OK");
            } else {
                EditorPrefs.SetString(PlayerSettings.productName + "gDocs", gDocsURL);
                LoadSettings(gDocsURL, SpecificSheets);
            }
        }

        GUILayout.EndScrollView();
    }

    void SaveFile(string file, string data) {
        if (File.Exists(file))
            File.Delete(file);
        try {
            TextWriter tw = new StreamWriter(file);
            tw.Write(data);
            tw.Close();
        } catch (IOException IOEx) {
            Debug.LogError("Incorrect file permissions? " + IOEx);
        }

        AssetDatabase.ImportAsset(file, ImportAssetOptions.ForceUpdate | ImportAssetOptions.ImportRecursive);
        AssetDatabase.Refresh();
    }

    void LoadSettings(string gDocsPage, string specificSheets = "") {
        List<string> newSheets = new List<string>();
        if(specificSheets != "") {
            newSheets = specificSheets.Split(new string[] { ", ", ","}, StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        SaveSettingsFile();

        gDocsPage += ((gDocsPage.Contains("&")) ? "&" : "?") + "timestamp=" + EditorApplication.timeSinceStartup;//Prevent caching

        float progress = 0.1f;

        List<SheetInfo> sheetIDs = GetSpreadSheetIDs(gDocsPage);
        if (newSheets.Count > 0) {
            for (int i = 0; i < sheetIDs.Count; i++) {
                if (!newSheets.Contains(sheetIDs[i].title)) {
                    sheetIDs.RemoveAt(i);
                    i--;
                }
            }
        }
        settings.sheetTitles = new string[0];
        unresolvedErrors = 0;
        foundSheets = 0;

        for (int i = sheetIDs.Count - 1; i >= 0; i--) {
            int downloadSheet = sheetIDs[i].ID;
            if (sheetIDs[i].title.Length > 3 && sheetIDs[i].title.Substring(0, 3) == "___") continue; //Skip sheets starting with "___"

            string thisURL = gDocsPage;
            bool oldFormat = false;
            if (gDocsPage.Contains("&output=")) {
                //Old format
                oldFormat = true;
                thisURL = gDocsPage.Replace("gid=0", "gid=" + (downloadSheet)) + "&" + EditorApplication.timeSinceStartup;
            }

            EditorUtility.DisplayProgressBar("Downloading gDoc data", "Page: " + downloadSheet, progress);
            string data = GetWebpage(thisURL);
            if (data != "") {
                if (oldFormat) {
                    data = CleanData(data);
                    if (data.StartsWith("<html>") || data.StartsWith("<!DOCTYPE html>")) {//Sheet does not exist
                        Debug.LogError("Sheet #" + downloadSheet + " does not exist!");
                        continue; //If more than X in a row do now exist, it's probably the end
                    }
                }
                if (!ParseData(data, sheetIDs[i].title, i)) {
                    //Failed
                }
            }
        }
        if (foundSheets == 0) {
            EditorUtility.DisplayDialog("Error", "No sheets could be imported. Either they were all empty, or you entered a wrong link. Please copy your LINK again and verify your spreadsheet.", "OK");
        }


        EditorUtility.ClearProgressBar();
        if (unresolvedErrors > 0) {
            EditorUtility.DisplayDialog("Errors", "There are " + unresolvedErrors + " open errors in your localization. See the console for more information.", "OK");
        }
    }

    bool ParseData(string data, string sheetTitle, int sheetID) {
        List<object> loadData = new List<object>();

        if (data.Contains("<html>")) {
            LoadHTML(ref loadData, data, sheetTitle, sheetID);
        } else {
            //CSV
            LoadCSV(ref loadData, data, sheetTitle);
        }

        string output = "";
        switch (sheetTitle) {
            case "Achievements":
                output = SaveLoadManager.WriteXML<Achievement>(loadData);
                break;
            case "Potions":
                output = SaveLoadManager.WriteXML<Potion>(loadData);
                break;
            default:
                Debug.LogWarning("Please Add Sheet: " + sheetTitle + " in an appropriate switch statement");
                break;
        }

        foundSheets++;
        string folder = GetFolder(sheetTitle);
        SaveFile("Assets" + folder + sheetTitle + ".xml", output);
        Debug.Log(sheetTitle + ".xml updated at " + folder);

        return true;
    }

    void LoadHTML(ref List<object> loadData, string data, string sheetTitle, int sheetID) {
       var document = new HtmlAgilityPack.HtmlDocument();
        document.LoadHtml(data);

        int tableNR = 0;
        foreach (HtmlAgilityPack.HtmlNode node in document.DocumentNode.SelectNodes("//table")) {
            if (tableNR == sheetID) {
                //Only parse 1 "sheet"
                ParseHTMLTable(ref loadData, node);
            }
            tableNR++;
            foundSheets++;
        }

        //Parse sheet titles
        List<string> sheetNames = new List<string>(settings.sheetTitles);

        List<string> newTitles = GetSheetTitles(document);
        foreach (string newTitl in newTitles) {
            if (!sheetNames.Contains(newTitl))
                sheetNames.Add(newTitl);
        }
        settings.sheetTitles = sheetNames.ToArray();
        SaveSettingsFile();

    }

    void ParseHTMLTable(ref List<object> loadData, HtmlAgilityPack.HtmlNode node) {
        foreach (HtmlAgilityPack.HtmlNode trNode in node.SelectNodes(".//tr")) {

            if (trNode.SelectNodes(".//td") == null) {
                continue;
            }
            int i = -1;
            List<object> rowData = new List<object>();


            foreach (HtmlAgilityPack.HtmlNode tdNode in trNode.SelectNodes(".//td")) {
                i++;
                rowData.Add(tdNode.InnerText);

            }
            loadData.Add(rowData);
        }
    }

    void LoadCSV(ref List<object> loadData, string data, string debugSheetTitle) {
        List<string> sheetNames = new List<string>(settings.sheetTitles);
        if (!sheetNames.Contains(debugSheetTitle))
            sheetNames.Add(debugSheetTitle);
        settings.sheetTitles = sheetNames.ToArray();
        SaveSettingsFile();

        List<string> lines = GetCVSLines(data);

        for (int i = 0; i < lines.Count; i++) {
            string line = lines[i];
            if (line.Contains(SKIP)) continue;
            List<string> contents = GetCVSLine(line);
            List<object> rowData = new List<object>();
            for (int j = 0; j < contents.Count; j++) {
                if (contents[j] == "") continue;
                rowData.Add(contents[j]);
                Debug.Log(contents[j]);
            }
            loadData.Add(rowData);
        }
    }

    List<string> GetCVSLines(string data) {
        List<string> lines = new List<string>();
        int i = 0;
        int searchCloseTags = 0;
        int lastSentenceStart = 0;
        while (i < data.Length) {
            if (data[i] == '"') {
                if (searchCloseTags == 0)
                    searchCloseTags++;
                else
                    searchCloseTags--;
            } else if (data[i] == '\n') {
                if (searchCloseTags == 0) {
                    lines.Add(data.Substring(lastSentenceStart, i - lastSentenceStart));
                    lastSentenceStart = i + 1;
                }
            }
            i++;
        }
        if (i - 1 > lastSentenceStart) {
            lines.Add(data.Substring(lastSentenceStart, i - lastSentenceStart));
        }
        return lines;
    }

    List<string> GetCVSLine(string line) {
        List<string> list = new List<string>();
        int i = 0;
        int searchCloseTags = 0;
        int lastEntryBegin = 0;
        while (i < line.Length) {
            if (line[i] == '"') {
                if (searchCloseTags == 0)
                    searchCloseTags++;
                else
                    searchCloseTags--;
            } else if (line[i] == ',') {
                if (searchCloseTags == 0) {
                    list.Add(StripQuotes(line.Substring(lastEntryBegin, i - lastEntryBegin)));
                    lastEntryBegin = i + 1;
                }
            }
            i++;
        }
        if (line.Length > lastEntryBegin) {
            list.Add(StripQuotes(line.Substring(lastEntryBegin)));//Add last entry
        }
        return list;
    }

    string GetFolder(string dir) {
        string folder = "/Resources";
        if (!Directory.Exists(folder)) {
            Directory.CreateDirectory(folder);
            AssetDatabase.ImportAsset(folder, ImportAssetOptions.ForceUpdate);
        }
        folder = folder + "/Files/";
        if (!Directory.Exists(folder)) {
            Directory.CreateDirectory(folder);
            AssetDatabase.ImportAsset(folder, ImportAssetOptions.ForceUpdate);
        }
        
        AssetDatabase.Refresh();

        return folder;
    }

    string CleanData(string data) {
        //Cut of formula data
        int formulaIndex = data.IndexOf("\n\n\n[");
        if (formulaIndex != -1)
            data = data.Substring(0, formulaIndex);

        string[] patterns = new string[4];
        string[] replacements = new string[4];
        int patrs = 0;
        int reps = 0;

        patterns[patrs++] = @" \[[0-9]+\],";
        replacements[reps++] = ",";

        patterns[patrs++] = @" \[[0-9]+\]""";
        replacements[reps++] = "\"";

        patterns[patrs++] = @" \[[0-9]+\]([\n\r$]+)";
        replacements[reps++] = "$1";
        patterns[patrs++] = @" \[[0-9]+\]\Z";
        replacements[reps++] = "";

        data = PregReplace(data, patterns, replacements);

        return data;

    }

    //Parse sheet IDs from the HTML view.
    List<SheetInfo> GetSheetIDs(string URL, List<SheetInfo> sheetList) {
        string output = GetWebpage(URL);

        MatchCollection matches = Regex.Matches(output, ";gid=(?<sheetID>[0-9]+)\">(?<sheetTitle>[^<]+)</");
        if (matches.Count < 1) {
            //Try new Google format: onclick="switchToSheet('0')" >Main</a>
            matches = Regex.Matches(output, "\'(?<sheetID>[0-9]+)\'.\" >(?<sheetTitle>[^<]+)</");
        }

        foreach (Match mat in matches) {
            int sheetID = int.Parse(mat.Groups["sheetID"].Value);
            string sheetTitle = mat.Groups["sheetTitle"].Value;
            bool present = false;
            foreach (SheetInfo info in sheetList) {
                if (info.ID == sheetID) {
                    present = true;
                    break;
                }
            }
            if (!present) {
                SheetInfo inf = new SheetInfo();
                inf.ID = sheetID;
                inf.title = sheetTitle;
                sheetList.Add(inf);
            }
        }

        return sheetList;
    }

    List<string> GetSheetTitles(HtmlAgilityPack.HtmlDocument document) {
        List<string> titles = new List<string>();

        if (document.DocumentNode.SelectNodes("//a") == null) {
            //Single sheet in this document
            HtmlAgilityPack.HtmlNode node = document.DocumentNode.SelectSingleNode("//span ");

            int cutOffFrom = node.InnerText.IndexOf(":") + 2;
            string title = node.InnerText.Substring(cutOffFrom);

            titles.Add(title);

        } else {
            //Parse sheet titles
            foreach (HtmlAgilityPack.HtmlNode node in document.DocumentNode.SelectNodes("//a")) {
                if (node.ParentNode.ChildNodes.Count != 1) //Filter out the REPORT ABUSE and BY GOOGLE links
                {
                    continue;
                }
                titles.Add(node.InnerText);
            }
        }
        return titles;
    }

    string GetWebpage(string url) {
        WWW wwwReq = new WWW(url);
        while (!wwwReq.isDone) { }
        return wwwReq.text;

    }

    List<SheetInfo> GetSpreadSheetIDs(string gDocsUrl) {
        List<SheetInfo> res = new List<SheetInfo>();

        if (!gDocsUrl.Contains("output=")) {
            //2014 format
            string output = GetWebpage(gDocsUrl);


            var document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(output);

            List<string> titles = GetSheetTitles(document);

            int i = 0;
            foreach (string titl in titles) {
                SheetInfo inf = new SheetInfo();
                inf.ID = i;
                inf.title = titl;
                res.Add(inf);
                i++;
            }

        } else {
            //2013 format
            Match match = Regex.Match(gDocsUrl, "key=(?<gDocsKey>[^&#]+)");
            string gDocskey = match.Groups["gDocsKey"].Value;
            string URL = "https://spreadsheets.google.com/spreadsheet/pub?key=" + gDocskey;
            res = GetSheetIDs(URL, res);

            //Also fetch the ID of the default sheet.
            if (res.Count > 0)
                GetSheetIDs(URL + "&gid=" + res[0].ID, res);
        }

        if (res.Count == 0) {
            Debug.LogWarning("No sheets found, or your spreadsheet has only 1 sheet. We are assuming that the first sheet has ID '0'. (You can fix this by simply adding a second sheet as this will allow ID lookup via HTML output)");
            SheetInfo info = new SheetInfo();
            info.ID = 0;
            info.title = "Sheet1";
            res.Add(info);
        }
        return res;
    }

    static string PregReplace(string input, string[] pattern, string[] replacements) {
        if (replacements.Length != pattern.Length)
            throw new ArgumentException("Replacement and Pattern Arrays must be balanced");

        for (var i = 0; i < pattern.Length; i++) {
            input = Regex.Replace(input, pattern[i], replacements[i]);
        }

        return input;
    }

    //Remove the double " that CVS adds inside the lines, and the two outer " as well
    string StripQuotes(string input) {
        if (input.Length < 1 || input[0] != '"')
            return input;//Not a " formatted line

        string output = "";
        int i = 1;
        bool allowNextQuote = false;
        while (i < input.Length - 1) {
            string curChar = input[i] + "";
            if (curChar == "\"") {
                if (allowNextQuote)
                    output += curChar;
                allowNextQuote = !allowNextQuote;
            } else {
                output += curChar;
            }
            i++;
        }
        return output;
    }

    struct SheetInfo {
        public int ID;
        public string title;
    }

    void SaveSettingsFile() {
        EditorUtility.SetDirty(settings);
    }
}
