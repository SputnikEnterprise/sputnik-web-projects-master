using System;
using System.Collections.Specialized;
using System.Xml;
using System.IO;
using System.Collections;

namespace Imt.Common.I18N {
    /// <summary>
    /// 
    /// </summary>
    public class StringManager {

        #region Public Methods 

        /// <summary>
        /// Initializes a new instance of the <see cref="T:StringManager"/> class.
        /// </summary>
        /// <param name="absoluteFilePath">The absolute file path.</param>
        /// <param name="languageISO">The language ISO.</param>
        public StringManager(string absoluteFilePath, string languageISO) {
            m_filePath = absoluteFilePath;
            m_languageISO = languageISO.ToUpper();

            m_stringDictionary = new StringDictionary();

            LoadXml();
        }


        /// <summary>
        /// Gets all available keys.
        /// </summary>
        /// <returns>A collection with keys.</returns>
        public ICollection GetAllKeys() {
            return m_stringDictionary.Keys;
        }


        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="textKey">The text key.</param>
        /// <returns>The requested string or the string value [missing] if not defined for this language.</returns>
        public string GetString(string textKey) {
            textKey = textKey.ToUpper();

            if (m_languageISO.Length == 0) {
                throw new Exception("Missing parameter: LanguageISO = '" + m_languageISO + "'!");
            }
            if (textKey.Length == 0) {
                throw new Exception("Missing parameter: TextKey = '" + textKey + "'!");
            }

            if (m_stringDictionary.ContainsKey(textKey)) {
                return m_stringDictionary[textKey];
            }
            else {
                throw new Exception("String with key '" + textKey + "' does not exist in string file '" + m_filePath + "'!");
            }
        }

        #endregion

        #region Private Methods

        private void LoadXml() {
            if (!File.Exists(m_filePath)) {
                throw new Exception("Strings file '" + m_filePath + "' does not exist!");
            }

            // Load the file
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;

            try {
                doc.Load(m_filePath);
            }
            catch (Exception e) {
                throw new Exception("Could not load strings file '" + m_filePath + "'!", e);
            }

            // Find the root node
            XmlNode rootNode = doc.SelectSingleNode(ROOT_NODE);

            if (rootNode == null) {
                throw new Exception("Root node '" + ROOT_NODE + "' not found in strings file '" + m_filePath + "'!");
            }

            foreach (XmlNode node in rootNode.SelectNodes(STRING_NODE)) {
                string key = string.Empty;
#if DEBUG
                string value = "[missing]";
#else
                string value = string.Empty;
#endif

                XmlNode keyNode = node.SelectSingleNode(KEY_PARAM);

                if (keyNode != null) {
                    key = keyNode.Value.ToUpper();

                    XmlNode valueNode = node.SelectSingleNode(LANGUAGE_NODE + m_languageISO);
                    value = ( valueNode != null ? valueNode.InnerText : value);

                    if (m_stringDictionary.ContainsKey(key)) {
                        throw new Exception("Duplicate string node with key '" + key + "' in string file '" + m_filePath + "'!");
                    }
                    else {
                        m_stringDictionary.Add(key, value);
                    }                    
                }
            }
        }

        #endregion

        #region Private Fields


        /// <summary>
        /// xml root node
        /// </summary>
        private const string ROOT_NODE      = "Translations";
        private const string STRING_NODE    = "String";
        private const string KEY_PARAM      = "@key";
        private const string LANGUAGE_NODE  = "Language_";

        private string m_filePath;
        private string m_languageISO;

        private StringDictionary m_stringDictionary;

        #endregion
    }
}
