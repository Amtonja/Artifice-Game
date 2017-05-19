using System;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Artifice.Characters
{
    public class CombatStats
    {
        private int level;

        private int attack, baseAttack;
        private int defense, baseDefense;
        private int speed, baseSpeed;
        private int evasion, baseEvasion;
        private int maxHealth, baseMaxHealth;
        private int magic, baseMagic;
        private int magicDefense, baseMagicDefense;
        private int accuracy, baseAccuracy;
        //private int loyalty, baseLoyalty; // not used

        public CombatStats() { }

        public CombatStats(int _level, string id)
        {
            level = _level;
            // Load stats from level and ID
            TextAsset statsFile = Resources.Load("Files/ArtificeCharacterStats") as TextAsset;
            LoadStats(statsFile, _level, id);
        }

        /// <summary>
        /// For the given character ID and level, loads character stats from the XML document specified by file.
        /// </summary>
        /// <param name="file">TextAsset containing the XML document of stats.</param>
        /// <param name="level">Character level that determines total stats.</param>
        /// <param name="id">Unique identifier of the character.</param>
        private void LoadStats(TextAsset file, int level, string id)
        {
            if (file == null)
            {
                Debug.LogError("Character stats XML file not found");
                return;
            }
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(file.text);

            //Debug.Log("Attempting to load stats for " + id);
            XmlNode root = xmlDoc.DocumentElement;
            XmlNode character = root.SelectSingleNode(string.Format("descendant::character[id = '{0}']", id));

            if (character == null)
            {
                Debug.LogError("Couldn't find any statistics on file for " + id);
                return;
            }

            BaseAttack = GetSingleStat(character, "attack");
            BaseDefense = GetSingleStat(character, "defense");
            BaseAccuracy = GetSingleStat(character, "accuracy");
            BaseMagic = GetSingleStat(character, "magic");
            BaseMagicDefense = GetSingleStat(character, "magic_defense");
            BaseEvasion = GetSingleStat(character, "evasion");
            BaseSpeed = GetSingleStat(character, "speed");
            BaseMaxHealth = GetSingleStat(character, "max_health");

        }

        /// <summary>
        /// Gets a single stat specified by argument from the node for the specified character, returns as an int.
        /// </summary>
        /// <param name="character">The XML node for the current character.</param>
        /// <param name="statName">The name of the stat to retrieve.</param>
        /// <returns>The specified stat as an integer.</returns>
        private int GetSingleStat(XmlNode character, string statName)
        {
            int value = 0;

            if (!int.TryParse(character[statName].InnerText, out value))
            {
                Debug.LogError("Couldn't load stat " + statName + " for character " + character["name"].InnerText);
            }

            return value;
        }

        /// <summary>
        /// Assembles a readable string showing all the stats contained in this instance.
        /// Can be output with Debug.Log().
        /// </summary>
        /// <returns>The formatted string.</returns>
        public override string ToString()
        {
            StringBuilder value = new StringBuilder();

            value.Append("Attack: ");
            value.Append(BaseAttack);
            value.Append("\n");

            value.Append("Defense: ");
            value.Append(BaseDefense);
            value.Append("\n");

            value.Append("Accuracy: ");
            value.Append(BaseAccuracy);
            value.Append("\n");

            value.Append("Magic: ");
            value.Append(BaseMagic);
            value.Append("\n");

            value.Append("Magic Defense: ");
            value.Append(BaseMagicDefense);
            value.Append("\n");

            value.Append("Evasion: ");
            value.Append(BaseEvasion);
            value.Append("\n");

            value.Append("Speed: ");
            value.Append(BaseSpeed);
            value.Append("\n");

            value.Append("Max Health: ");
            value.Append(BaseMaxHealth);
            value.Append("\n");

            return value.ToString();
        }

        #region C# Properties
        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public int Attack
        {
            get { return attack; }
            set { attack = value; }
        }

        public int BaseAttack
        {
            get { return baseAttack; }
            set { baseAttack = value; }
        }

        public int Defense
        {
            get { return defense; }
            set { defense = value; }
        }

        public int BaseDefense
        {
            get { return baseDefense; }
            set { baseDefense = value; }
        }

        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public int BaseSpeed
        {
            get { return baseSpeed; }
            set { baseSpeed = value; }
        }

        public int Evasion
        {
            get { return evasion; }
            set { evasion = value; }
        }

        public int BaseEvasion
        {
            get { return baseEvasion; }
            set { baseEvasion = value; }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public int BaseMaxHealth
        {
            get { return baseMaxHealth; }
            set { baseMaxHealth = value; }
        }

        public int Magic
        {
            get { return magic; }
            set { magic = value; }
        }

        public int BaseMagic
        {
            get { return baseMagic; }
            set { baseMagic = value; }
        }

        public int MagicDefense
        {
            get { return magicDefense; }
            set { magicDefense = value; }
        }

        public int BaseMagicDefense
        {
            get { return baseMagicDefense; }
            set { baseMagicDefense = value; }
        }

        public int Accuracy
        {
            get { return accuracy; }
            set { accuracy = value; }
        }

        public int BaseAccuracy
        {
            get { return baseAccuracy; }
            set { baseAccuracy = value; }
        }
        #endregion
    }
}