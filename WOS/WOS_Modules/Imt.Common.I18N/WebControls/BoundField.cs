using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.ComponentModel;

namespace Imt.Common.I18N.WebControls {
    /// <summary>
    /// Summary description for BoundField.
    /// </summary>
    [ToolboxData("<{0}:BoundField TextKey=\"\" runat=\"server\"></{0}:BoundField>"),]
    public class BoundField : System.Web.UI.WebControls.BoundField {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoundField"/> class.
        /// </summary>
        public BoundField() {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundField"/> class.
        /// </summary>
        /// <param name="textKey">The text key.</param>
        public BoundField(string textKey) {
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
        /// Gets or sets the text that is displayed in the header of a data control.
        /// </summary>
        public override string HeaderText {
            get {
                string translated = TranslationService.Instance.GetStringValue(TextKey);

                if (!string.IsNullOrEmpty(translated)) {
                    base.HeaderText = translated;
                }
                return base.HeaderText;
            }

        }
    }
}
