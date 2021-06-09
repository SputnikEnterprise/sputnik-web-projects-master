using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace Imt.Common.I18N.WebControls {
    /// <summary>
    /// Summary description for Label.
    /// </summary>
    [ToolboxData("<{0}:Label TextKey=\"\" runat=\"server\"></{0}:Label>"),]
    public class Label : System.Web.UI.WebControls.Label {
        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        public Label() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Label"/> class.
        /// </summary>
        /// <param name="textKey">The text key.</param>
        public Label(string textKey) {
            TextKey = textKey;
        }

        /// <summary>
        /// Gets or sets TextKey.
        /// </summary>
        [Description("TextKey for translation"), PersistenceMode(PersistenceMode.Attribute)]
        public string TextKey {
            get {
                object textKey = ViewState["TextKey"];
                if (textKey != null) {
                    return (string)textKey;
                }
                return string.Empty;
            }
            set {
                ViewState["TextKey"] = value;
            }
        }

        /// <summary>
        /// on pre render.
        /// </summary>
        /// <param name="e">The EventArgs</param>
        protected override void OnPreRender(EventArgs e) {
            string translated = TranslationService.Instance.GetStringValue(TextKey);

            if (!string.IsNullOrEmpty(translated)) {
                this.Text = translated;
            }

            base.OnPreRender(e);
        }
    }
}
