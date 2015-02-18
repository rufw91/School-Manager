using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenXmlPackaging {

    /// <summary>
    /// Represents a <mergeCells> element in Worksheet
    /// </summary>
    public class MergedCells : DataReaderAndWriter {

        #region Internal Members
        
        internal List<MergeCell> Cells { get; set; } 

        #endregion

        #region Constructor

        public MergedCells() {
            Cells = new List<MergeCell>();
        }
        
        #endregion

        #region DataWriter Members
        
        internal override bool Write(System.Xml.XmlWriter writer) {

            writer.WriteStartElement("mergeCells", Constants.MainNamespace);

            writer.WriteAttributeString("count", Cells.Count.ToString());

            foreach (var cell in Cells) {
                cell.Write(writer);
            }

            writer.WriteEndElement();

            return false;
        } 

        #endregion

        internal override bool Read(System.Xml.XmlReader writer)
        {
            throw new System.NotImplementedException();
        }
    }
}
