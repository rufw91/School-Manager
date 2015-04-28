using System.ComponentModel;
using System.Xml.Linq;

namespace OpenXmlPackaging {
    /// <summary>
    /// Represents a style in the Stylesheet.
    /// Renders as <xf>
    /// </summary>
    public class Style : XElementWriter {

        #region Public Properties

        public NumberFormat NumberFormat { get; set; }

        public Fill Fill { get; set; }

        public Font Font { get; set; }

        public Borders Borders { get; set; }

        public Alignment Alignment { get; set; }

        #endregion

        #region XElementWriter Members

        internal override string ParentNode {
            get { return "cellXfs"; }
        }

        internal override XElement Element {
            get {
                return ApplyStyle();
            }
        }

      
        #endregion      

        #region Private Methods

        private XElement ApplyStyle() {
            var element = new XElement(Constants.MainXNamespace + "xf");

            if (NumberFormat != null) {
                element.Add(new XAttribute("numFmtId", NumberFormat.Index));
            }

            if (Fill != null) {
                element.Add(new XAttribute("fillId", Fill.Index));
            }

            if (Font != null) {
                element.Add(new XAttribute("fontId", Font.Index));
            }

            if (Borders != null) {
                element.Add(new XAttribute("borderId", Borders.Index));
            }

            if (Alignment != null) {
                element.Add(Alignment.Element);
            }

            return element;
        }

        #endregion

        //internal override bool Write(System.Xml.XmlWriter writer) {

        //    writer.WriteStartElement("xf", Constants.MainNamespace);

        //    if (NumberFormat != null) {
        //        writer.WriteAttributeString("numFmtId", NumberFormat.Index.ToString());
        //    }

        //    if (Fill != null) {
        //        writer.WriteAttributeString("fillId", Fill.Index.ToString());
        //    }

        //    if (Font != null) {
        //        writer.WriteAttributeString("fontId", Font.Index.ToString());
        //    }

        //    if (Borders != null) {
        //        writer.WriteAttributeString("borderId", Borders.Index.ToString());
        //    }

        //    if (Alignment != null) {
        //        Alignment.Write(writer);
        //    }

        //    writer.WriteEndElement();

        //    return false;
        //}

    }
}
