using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml;
using System.Linq;
using System;
using Artifice.Interfaces;
using Artifice.Util;


namespace Artifice.Data {
    public class SaveLoadManager : MonoBehaviour {
        private static SaveLoadManager instance;

        protected string achievementFiredDataPath;

        protected const string ACHIEVEMENTS_DIR = "/Achievements";

        // Init everything on awake
        void Awake() {
            if (instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            } else if (instance != this) {
                Destroy(gameObject);
                return;
            }

            achievementFiredDataPath = Application.persistentDataPath + ACHIEVEMENTS_DIR + "/AFD.dat";
        }

        public List<T> ReadXML<T>() where T : IXML, new() {
            List<T> objs = new List<T>();
            TextAsset xmlFile = (TextAsset)Resources.Load(new T().ResourcesDir);
            List<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            if (xmlFile != null) {
                MemoryStream assetStream = new MemoryStream(xmlFile.bytes);
                XmlReader reader = XmlReader.Create(assetStream);
                List<object> parameters = new List<object>();
                while (!reader.EOF) {
                    reader.Read();
                    if (reader.IsStartElement()) {
                        PropertyInfo p = properties.Find(x => x.Name == reader.LocalName);
                        if (p != null) {
                            parameters.Add(reader.ReadElementContentAs(p.PropertyType, null));
                        }
                    } else if (reader.NodeType == XmlNodeType.EndElement) {
                        if (reader.LocalName == typeof(T).Name) {
                            objs.Add((T)Activator.CreateInstance(typeof(T), parameters.ToArray()));
                            parameters = new List<object>();
                        }
                    }
                }
                reader.Close();
            }
            return objs;
        }

        public static string WriteXML<T>(List<object> loadData) where T : IXML, new() {
            string output = "<?xml version = \"1.0\" encoding = \"Windows-1252\"?>\n<" + typeof(T).Name + "Data>\n";
            List<PropertyInfo> properties = typeof(T).GetProperties().ToList();
            foreach (List<object> obj in loadData) {
                int tabCounter = 1;
                output += XMLUtility.GetTag(typeof(T).Name, tabCounter++);
                for (int i = 0; i < obj.Count; i++) {
                    output += XMLUtility.GetSingleLineTag(properties[i].Name, obj[i].ToString(), tabCounter);
                }
                output += XMLUtility.GetEndTag(typeof(T).Name, --tabCounter);
            }
            output += XMLUtility.GetEndTag(typeof(T).Name + "Data", 0);
            Debug.Log(output);
            return output;
        }

        /// <summary>
        /// Saves a dictionary of achievements and whether or not they have been fired to disk to be loaded later
        /// </summary>
        /// <param name="achievements">The achievements data to save</param>
        public void SaveAchievementFiredData(List<Achievement> achievements) {
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            for (int i = 0; i < achievements.Count; i++) {
                dict.Add(achievements[i].ID, achievements[i].Fired);
            }

            CompletionData data = new CompletionData(new List<string>(dict.Keys), new List<bool>(dict.Values));
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(achievementFiredDataPath);

            bf.Serialize(file, data);
            file.Close();
        }

        /// <summary>
        /// Loads a whether or not the individual achievements have been fired or not
        /// </summary>
        /// <param name="achievements">Reference to the list of achievements to modify</param>
        public void LoadAchievementFiredData(ref List<Achievement> achievements) {

            if (FileExists(achievementFiredDataPath)) {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(achievementFiredDataPath, FileMode.Open);

                CompletionData data = new CompletionData();
                try {
                    data = (CompletionData)bf.Deserialize(file);
                } catch (Exception e) {
                    Debug.Log(e.Message);
                }

                for (int i = 0; i < data.Keys.Count; i++) {
                    if (achievements.Find(x => x.ID == data.Keys[i]) != null) achievements.Find(x => x.ID == data.Keys[i]).Fired = data.Values[i];
                }

                file.Close();
            }
        }

        /// <summary>
        /// Resets all the achievements to where none have been fired
        /// </summary>
        /// <param name="achievements">Reference to the list of achievements to modify</param>
        public void ResetAllAchievements(ref List<Achievement> achievements) {
            if (File.Exists(achievementFiredDataPath)) File.Delete(achievementFiredDataPath);
            for (int i = 0; i < achievements.Count; i++)
                achievements[i].Fired = false;
        }

        /// <summary>
        /// Resets the single achievement to not have been fired
        /// </summary>
        /// <param name="achievement">Reference to the achievement to modify</param>
        public void ResetAchievement(ref Achievement achievement) {
            if (File.Exists(achievementFiredDataPath)) File.Delete(achievementFiredDataPath);
            achievement.Fired = false;
        }

        /// <summary>
        /// Getter that instantiates the manager if it does not exist
        /// </summary>
        public static SaveLoadManager Instance {
            get {
                if (instance == null) {
                    instance = new GameObject("SaveLoad Manager").AddComponent<SaveLoadManager>();
                }
                return instance;
            }
        }

        /// <summary>
        /// Checks to see if a file exists at the specified path
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns>Whether or not the file exists</returns>
        public static bool FileExists(string path) {
            return File.Exists(path);
        }
    }
}