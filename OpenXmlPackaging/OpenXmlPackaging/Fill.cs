using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml.Linq;

namespace OpenXmlPackaging {
    /// <summary>
    /// Represents a <fill> element in Stylesheet
    /// </summary>
    public class Fill : XElementWriter {

        #region Public Properties
        
        public Color Color { get; set; } 

        #endregion

        #region XElementWriter Members
        
        internal override string ParentNode {
            get { return "fills"; }
        }

        internal override XElement Element {
            get {
                return new XElement(Constants.MainXNamespace + "fill",
                      new XElement(Constants.MainXNamespace + "patternFill", new XAttribute("patternType", "solid"),
                          new XElement(Constants.MainXNamespace + "fgColor", new XAttribute("rgb", Utilities.GetColorInHex(Color)))));
            }
        } 

        #endregion

        #region Constructor
        
        public Fill(Color color = default(Color)) {
            Color = color;
        } 

        #endregion
        
        //<fill>
        //    <patternFill patternType="solid">
        //        <fgColor rgb="FFFFFF00"/>
        //        <bgColor indexed="64"/>
        //    </patternFill>
        //</fill>
            
        //internal override bool Write(System.Xml.XmlWriter writer) {
    
        //    writer.WriteStartElement("fill", Constants.MainNamespace);

        //    writer.WriteStartElement("patternFill", Constants.MainNamespace);
        //    writer.WriteAttributeString("patternType", "solid");

        //    writer.WriteStartElement("fgColor", Constants.MainNamespace);
        //    writer.WriteAttributeString("rgb", Utilities.GetColorInHex(Color));
        //    writer.WriteEndElement();

        //    writer.WriteEndElement();

        //    writer.WriteEndElement();

        //    return false;
        //}
    }
}
