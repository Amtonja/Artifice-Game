namespace Artiface.Characters {
    public class CombatStats {
        private int level;

        private int strength, baseStrength;
        private int defense, baseDefense;
        private int speed, baseSpeed;
        private int evasion, baseEvasion;
        private int maxHealth, baseMaxHealth;
        private int magic, baseMagic;
        private int magicDefense, baseMagicDefense;
        private int accuracy, baseAccuracy;
        private int loyalty, baseLoyalty;

        public CombatStats() { }

        public CombatStats(int _level, string id) {
            level = _level;
            // Load stats from level and ID
        }

        #region C# Properties
        public int Level {
            get { return level; }
            set { level = value; }
        }

        public int Strength {
            get { return strength; }
            set { strength = value; }
        }

        public int BaseStrength {
            get { return baseStrength; }
            set { baseStrength = value; }
        }

        public int Defense {
            get { return defense; }
            set { defense = value; }
        }

        public int BaseDefense {
            get { return baseDefense; }
            set { baseDefense = value; }
        }

        public int Speed {
            get { return speed; }
            set { speed = value; }
        }

        public int BaseSpeed {
            get { return baseSpeed; }
            set { baseSpeed = value; }
        }

        public int Evasion {
            get { return evasion; }
            set { evasion = value; }
        }

        public int BaseEvasion {
            get { return baseEvasion; }
            set { baseEvasion = value; }
        }

        public int MaxHealth {
            get { return maxHealth; }
            set { maxHealth = value; }
        }

        public int BaseMaxHealth {
            get { return baseMaxHealth; }
            set { baseMaxHealth = value; }
        }

        public int Magic {
            get { return magic; }
            set { magic = value; }
        }

        public int BaseMagic {
            get { return baseMagic; }
            set { baseMagic = value; }
        }

        public int MagicDefense {
            get { return magicDefense; }
            set { magicDefense = value; }
        }

        public int BaseMagicDefense {
            get { return baseMagicDefense; }
            set { baseMagicDefense = value; }
        }

        public int Accuracy {
            get { return accuracy; }
            set { accuracy = value; }
        }

        public int BaseAccuracy {
            get { return baseAccuracy; }
            set { baseAccuracy = value; }
        }

        public int Loyalty {
            get { return loyalty; }
            set { loyalty = value; }
        }

        public int BaseLoyalty {
            get { return baseLoyalty; }
            set { baseLoyalty = value; }
        }
        #endregion
    }
}