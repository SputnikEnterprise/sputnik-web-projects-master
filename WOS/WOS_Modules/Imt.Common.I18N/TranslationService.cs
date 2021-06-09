using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Imt.Common.I18N {

    /// <summary>
    /// Translation service 
    /// </summary>
    public class TranslationService {

        #region Public Properties

        public string Language {
            get {
                return (string)(HttpContext.Current.Session[KEY_LANGUAGE] ?? m_instance.m_defaultLanguage);
            }
            set {
                HttpContext.Current.Session[KEY_LANGUAGE] = value;
            }
        }

        #endregion

        #region Public Methods

        public static TranslationService Instance {
            get {
                if (m_instance == null) {
                    m_instance = new TranslationService();
                }

                return m_instance;
            }
        }

        /// <summary>
        /// get string value.
        /// </summary>
        /// <param name="textKey">
        /// The text key.
        /// </param>
        /// <returns>
        /// The get string value.
        /// </returns>
        public string GetStringValue(string textKey) {
            return GetStringValue(Language, textKey);
        }

        /// <summary>
        /// get string value.
        /// </summary>
        /// <param name="languageIso">
        /// The language iso.
        /// </param>
        /// <param name="textKey">
        /// The text key.
        /// </param>
        /// <returns>
        /// The get string value.
        /// </returns>
        public string GetStringValue(string languageIso, string textKey) {
            if (string.IsNullOrEmpty(textKey)) {
                return string.Empty;
            }
            var cacheKey = CACHEKEY_STRING_VALUES + languageIso;
            var stringManager = (StringManager)HttpContext.Current.Cache.Get(cacheKey);

            if (stringManager == null) {
                // read strings.config
                var stringFilePath = HttpContext.Current.Server.MapPath("~/Config/strings.xml");

                stringManager = new StringManager(stringFilePath, languageIso);

                // now add the object with dependencies to the global cache
                var dependency = new CacheDependency(stringFilePath);
                HttpContext.Current.Cache.Add(
                    cacheKey,
                    stringManager,
                    dependency,
                    DateTime.MaxValue,
                    TimeSpan.Zero,
                    CacheItemPriority.High,
                    null);
            }
            string translated = stringManager.GetString(textKey);
            if (string.IsNullOrEmpty(translated)) {
                translated = GetStringValue(m_defaultLanguage, textKey);
#if DEBUG
                translated = "[default] " + translated;
#endif
            }

            return translated;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Prevents a default instance of the <see cref="TranslationService"/> class from being created.
        /// </summary>
        private TranslationService() {
            // Eventually pick the default language out of a config file.
            // This is the reason this member is readonly and not const.
            m_defaultLanguage = "de";
        }

        #endregion

        #region Public Constants

        /// <summary>
        /// key for http context for language
        /// </summary>
        public const string KEY_LANGUAGE = "user_language";

        /// <summary>
        /// cachekey string values.
        /// </summary>
        public const string CACHEKEY_STRING_VALUES = "WOS_STRING_MANAGER_";

        #endregion

        #region Private Member

        /// <summary>
        /// default language
        /// </summary>
        private readonly string m_defaultLanguage;

        /// <summary>
        /// Single instance (Singleton Design Pattern)
        /// </summary>
        private static TranslationService m_instance;

        #endregion

    }
}
