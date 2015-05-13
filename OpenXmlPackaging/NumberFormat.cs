using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace OpenXmlPackaging {

    /// <summary>
    /// Represents a <numFmt> element in the Stylesheet
    /// </summary>
    public class NumberFormat :XElementWriter {

        #region Private Members
        
        private int _formatId; 

        #endregion

        #region Public Properties
        
        public string FormatCode { get; set; }

        #endregion

        #region XElementWriter Members
        
        internal override string ParentNode {
            get { return "numFmts"; }
        }

        internal override XElement Element {
            get {
                XElement numFmt = new XElement(Constants.MainXNamespace + "numFmt");
                if (!String.IsNullOrWhiteSpace(FormatCode)) { 
                    numFmt.Add(new XAttribute("formatCode", FormatCode));
                }
                return numFmt;
            }
        }

        internal override void PostProcess(XElement element) {
            element.Add(new XAttribute("numFmtId", Index));            
        }

        public override int Index {
            get {
                return _formatId;
            }
            set {
                _formatId = value == -1 ? value : value + 164;
            }
        }

        #endregion

        #region Constructor
        
        public NumberFormat(string formatCode) {

            // TODO :: Format ID??
            FormatCode = formatCode;
        } 

        #endregion

        //internal override bool Write(System.Xml.XmlWriter writer) {
        //    writer.WriteStartElement("numFmt", Constants.MainNamespace);

        //    if (!String.IsNullOrWhiteSpace(FormatCode)) {
        //        writer.WriteAttributeString("formatCode", FormatCode);
        //    }

        //    writer.WriteEndElement();

        //    return true;

        //}
    }
}
