using UnityEngine;
using System.Collections.Generic;

namespace Artifice.Data {
    /// <summary>
    /// Manages everything dealing with achievements except UI
    /// </summary>
    public class AchievementManager : MonoBehaviour {
        private static AchievementManager instance;                         // Singleton setup

        private List<Achievement> achievements;                             // List of all the achievements
        private List<Achievement> queue;                                    // Queue to hold activated achievements that haven't been fired yet

        public delegate void AchievementAction(Achievement achievement);    // Delegate for sending out an achievement action
        public static event AchievementAction AchievementEarned;            // Event for letting things know that an achievement has been earned

        // Enable and Disable events this class is tied to
        void OnEnable() {
            /********
             * Add your custom UI event here or tell me what it is
             ********/

            // _________ += UpdateQueue;                                    // Add the UpdateQueue to be called once the Achievement UI is finished displaying
        }
        void OnDisable() {
            /********
             * Remove your custom UI event here or tell me what it is
             ********/

            // _________ -= UpdateQueue;
        }

        // Init everything on awake
        void Awake() {
            if(instance == null) {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if(instance != this) {
                Destroy(gameObject);
                return;
            }

            queue = new List<Achievement>();
            achievements = new List<Achievement>();
            achievements = SaveLoadManager.Instance.ReadXML<Achievement>();
            SaveLoadManager.Instance.LoadAchievementFiredData(ref achievements);
            /*
            for(int i = 0; i < achievements.Count; i++) {
                Debug.Log(achievements[i].Title + "\n");
                Debug.Log(achievements[i].ID + "\n");
                Debug.Log(achievements[i].Description + "\n");
                Debug.Log(achievements[i].Order + "\n");
                Debug.Log(achievements[i].Icon.name + "\n");
                Debug.Log(achievements[i].Fired + "\n");
                Debug.Log("\n");
            }
            */

            //Fire(achievements[0].ID);
            //Fire(achievements[0].ID);
            
        }

        // FOR DEBUG USE ONLY
        void Update() {
            if (Input.GetKeyDown(KeyCode.O)) {
                SaveLoadManager.Instance.ResetAllAchievements(ref achievements);
                
                for (int i = 0; i < achievements.Count; i++) {
                    Debug.Log(achievements[i].Title + "\n");
                    Debug.Log(achievements[i].ID + "\n");
                    Debug.Log(achievements[i].Description + "\n");
                    //Debug.Log(achievements[i].Icon.name + "\n");
                    Debug.Log(achievements[i].Fired + "\n");
                    Debug.Log("\n");
                }
                
                Debug.Log("Achievements Reset");
            }

            if (Input.GetKeyDown(KeyCode.P)) {
                SaveLoadManager.Instance.SaveAchievementFiredData(achievements);
                Debug.Log("Achievements Saved");
            }
        }

        /// <summary>
        /// Fires an achievement that has never been fired before
        /// </summary>
        /// <param name="ID">ID of the achievement to fire</param>
        public void Fire(string ID) {
            // Find the achievement in the list of achievements
            Achievement a = achievements.Find(x => x.ID.Equals(ID));
            if(a != null) {
                if (!a.Fired) {
                    a.Fired = true;
                    // If the queue is empty, add the achievement to the queue and send out an event
                    // saying the achievement was fired
                    if(queue.Count == 0) {
                        if (AchievementEarned != null) AchievementEarned(a);
                    }
                    queue.Add(a);
                    SaveLoadManager.Instance.SaveAchievementFiredData(achievements);
                    Debug.Log("Achievement: " + a.Title + " was fired");
                }
            } else {
                Debug.LogError("Achievement with ID: " + ID + " is not found.");
            }
        }

        /// <summary>
        /// Updates the achievements in the queue
        /// </summary>
        private void UpdateQueue() {
            if (queue.Count > 0) {
                // Removed the last fired achievement
                queue.RemoveAt(0);
            }
            if(queue.Count > 0) {
                // Send out the next achievement if multiple were fired at the same time
                if (AchievementEarned != null) AchievementEarned(queue[0]);
            }
        }

        /// <summary>
        /// Automatically updates an achievement's completion progress by the appropriate step amount
        /// All steps carry equal weight
        /// </summary>
        /// <param name="ID">The ID of the achievement to be incremented</param>
        public void UpdateProgress(string ID) {
            Achievement a = achievements.Find(x => x.ID.Equals(ID));
            if (a != null) {
                a.UpdateProgress();
            }
        }

        #region Comparison Predicates
        //comparisons for sorting
        private static int CompareByTitle(Achievement first, Achievement second) {
            return string.Compare(first.Title, second.Title);
        }
        private static int CompareByID(Achievement first, Achievement second) {
            return string.Compare(first.ID, second.ID);
        }
        #endregion

        /// <summary>
        /// Getter that instantiates the manager if it does not exist
        /// </summary>
        public static AchievementManager Instance {
            get {
                if (instance == null) {
                    instance = new GameObject("Achievement Manager").AddComponent<AchievementManager>();
                }
                return instance;
            }
        }

        /// <summary>
        /// Gets an achievement based on the id given
        /// </summary>
        /// <param name="id">The id of the achievement to be returned</param>
        /// <returns>An achievement</returns>
        public Achievement GetAchievement(string id) {
            return achievements.Find(x => x.ID == id);
        }
    }
}
