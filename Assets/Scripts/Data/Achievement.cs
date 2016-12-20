using UnityEngine;
using Artifice.Interfaces;

namespace Artifice.Data {
    /// <summary>
    /// Holds data about achievements
    /// </summary>
    public class Achievement : IXML {
        private string title = "__ERROR_NO_TITLE__";
        private string description = "__ERROR_NO_DESCRIPTION__";
        private string id = "__ERROR_NO_ID__";
        private bool hidden = false;
        private float percentage = 0f;
        private int steps = 0;
        private float stepAmount;
        private bool fired = false;
        private int points = 0;
        private Sprite icon;
        private string iconPath;

        public Achievement() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="_title">Title of the achievement</param>
        /// <param name="_description">Description of the achievement</param>
        /// <param name="_id">ID of the achievement</param>
        /// <param name="_points">Number of points the achievement is worth</param>
        /// <param name="_steps">Number of steps the achievement has before it can be completed</param>
        /// <param name="_hidden">Whether or not the achievement is hidden</param>
        /// <param name="_icon">Icon of the achievement</param>
        public Achievement(string _title, string _description, string _id, int _points, int _steps, bool _hidden, string _iconPath) {
            title = _title;
            description = _description;
            id = _id;
            points = _points;
            steps = _steps;
            hidden = _hidden;
            iconPath = _iconPath;
            icon = Resources.Load<Sprite>(iconPath);

            stepAmount = 100.0f / steps;
        }

        /// <summary>
        /// Automatically updates an achievement's completion progress by the appropriate step amount
        /// All steps carry equal weight
        /// </summary>
        public void UpdateProgress() {
            Percentage += stepAmount;
        }

        #region C# Properties
        #region Properties for XML
        public string Title {
            get { return title; }
        }
        public string Description {
            get { return description; }
        }
        public string ID {
            get { return id; }
        }
        public int Points {
            get { return points; }
            set { points = value; }
        }
        public int Steps {
            get { return steps; }
            set { steps = value; }
        }
        public bool Hidden {
            get { return hidden; }
            set { hidden = value; }
        }
        public string IconPath {
            get { return iconPath; }
        }

        #endregion
        public Sprite Icon {
            get { return icon; }
        }
        public float Percentage {
            get { return percentage; }
            set {
                percentage = Mathf.Clamp(value, 0, 100);
                if (percentage == 100) AchievementManager.Instance.Fire(id);
            }
        }
        public float StepAmount {
            get { return stepAmount; }
        }
        public bool Fired {
            get { return fired; }
            set { fired = value; }
        }

        public string ResourcesDir {
            get { return "Data/Achievements"; }
        }
        #endregion
    }
}