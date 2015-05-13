using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Drawing;

namespace OpenXmlPackaging {
    public class Border : XElementWriter {

        #region Public Properties

        public BorderType BorderType { get; set; }

        public BorderStyles BorderStyle { get; set; }

        public Color Color { get; set; }

        #endregion

        #region XElementWriter Members

        internal override string ParentNode {
            get { return "borders"; }
        }

        internal override XElement Element {
            get {
                return new XElement(Constants.MainXNamespace + BorderType.GetAttribute<ElementAttribute>().Name, 
                                        new XAttribute("style", BorderStyle.GetAttribute<ElementAttribute>().Name),
                                        new XElement(Constants.MainXNamespace + "color", new XAttribute("rgb", Utilities.GetColorInHex(Color))));
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Border"/> class.
        /// </summary>
        public Border() {
            Color = Color.Black;
        }

        #endregion

        //<right style="dashDot" xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
        //  <color rgb="FFFF0000" />
        //</right>

        //internal override bool Write(System.Xml.XmlWriter writer) {
        //    writer.WriteStartElement(BorderType.GetAttribute<ElementAttribute>().Name, Constants.MainNamespace);
        //    writer.WriteAttributeString("style", BorderStyle.GetAttribute<ElementAttribute>().Name);
            
        //    writer.WriteStartElement("color", Constants.MainNamespace);
        //    writer.WriteAttributeString("rgb", Utilities.GetColorInHex(Color));
        //    writer.WriteEndElement();

        //    writer.WriteEndElement();

        //    return false;
        //}
    }
}
