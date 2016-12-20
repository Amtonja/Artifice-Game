namespace Artifice.Util {
    public static class XMLUtility {
        public static string GetSingleLineTag(string tagName, string element, int tabs) {
            string tag = "";
            for (int i = 0; i < tabs; i++) {
                tag += "\t";
            }
            tag += "<" + tagName + ">" + element + "</" + tagName + ">\n";

            return tag;
        }

        public static string GetTag(string tagName, int tabs) {
            string tag = "";
            for (int i = 0; i < tabs; i++) {
                tag += "\t";
            }
            tag += "<" + tagName + ">\n";
            return tag;
        }

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