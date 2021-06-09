using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace Imt.Common.I18N.WebControls {
    /// <summary>
    /// Summary description for LinkButton.
    /// </summary>
    [ToolboxData("<{0}:LinkButton TextKey=\"\" runat=\"server\"></{0}:LinkButton>"),]
    public class LinkButton : System.Web.UI.WebControls.LinkButton {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkButton"/> class.
        /// </summary>
        public LinkButton() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkButton"/> class.
        /// </summary>
        /// <param name="textKey">The text key.</param>
        public LinkButton(string textKey) {
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
