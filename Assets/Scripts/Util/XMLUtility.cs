namespace Artifice.Util {
    /// <summary>
    /// Utility class for aiding in reading/writing xml
    /// </summary>
    public static class XMLUtility {
        /// <summary>
        /// Gets a single line tag and places the element in between the opening and closing tags
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="element">The content to put in the tag</param>
        /// <param name="tabs">How much this tag should be indented</param>
        /// <returns>The assembled inline tag</returns>
        public static string GetSingleLineTag(string tagName, string element, int tabs) {
            string tag = "";
            for (int i = 0; i < tabs; i++) {
                tag += "\t";
            }
            tag += "<" + tagName + ">" + element + "</" + tagName + ">\n";

            return tag;
        }

        /// <summary>
        /// Gets an xml tag of the given name
        /// </summary>
        /// <param name="tagName">The name of the tag</param>
        /// <param name="tabs">How much this tag should be indented</param>
        /// <returns>The assembled tag</returns>
        public static string GetTag(string tagName, int tabs) {
            string tag = "";
            for (int i = 0; i < tabs; i++) {
                tag += "\t";
            }
            tag += "<" + tagName + ">\n";
            return tag;
        }

        /// <summary>
        /// Gets an xml end tag of the given name
        /// </summary>
        /// <param name="tagName">The name of the end tag</param>
        /// <param name="tabs">How much this tag should be indented</param>
        /// <returns>The assembled end tag</returns>
        public static string GetEndTag(string tagName, int tabs) {
            string tag = "";
            for (int i = 0; i < tabs; i++) {
                tag += "\t";
            }
            tag += "</" + tagName + ">\n";
            return tag;
        }
    }

}