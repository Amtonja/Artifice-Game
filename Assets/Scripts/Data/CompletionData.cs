using System.Collections.Generic;

namespace Artifice.Data {
    /// <summary>
    /// Serializes a dictionary for string ids and bool completions into two lists to be saved and loaded
    /// </summary>
    [System.Serializable]
    public class CompletionData {
        // The keys and the values of the dictionary
        private List<string> keys;
        private List<bool> values;

        /// <summary>
        /// Constructor
        /// </summary>
        public CompletionData() {
            keys = new List<string>();
            values = new List<bool>();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="keys">The keys of the dictionary (strings)</param>
        /// <param name="values">The values of the dictionary (bools)</param>
        public CompletionData(List<string> keys, List<bool> values) {
            this.keys = keys;
            this.values = values;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dict">The dictionary of IDs to Fired</param>
        public CompletionData(Dictionary<string, bool> dict) {
            keys = new List<string>(dict.Keys);
            values = new List<bool>(dict.Values);
        }

        /// <summary>
        /// Gets the dictionary on deserialization
        /// </summary>
        /// <returns>The dictionary of IDs to Fired</returns>
        public Dictionary<string, bool> GetDictionary() {
            Dictionary<string, bool> dict = new Dictionary<string, bool>();
            for (int i = 0; i < keys.Count; i++) {
                dict.Add(keys[i], values[i]);
            }
            return dict;
        }

        #region C# Properties
        public List<string> Keys {
            get { return keys; }
        }
        public List<bool> Values {
            get { return values; }
        }
        #endregion
    }
}
