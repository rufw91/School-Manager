using System.Xml.Linq;

namespace OpenXmlPackaging {

    /// <summary>
    /// Represents a <mergeCell> element in the Worksheet
    /// </summary>
    internal class MergeCell : DataReaderAndWriter
    {

        public string Reference { get; set; }

        #region DataWriter Members

        internal override bool Write(System.Xml.XmlWriter writer) {

            writer.WriteStartElement("mergeCell", Constants.MainNamespace);
            writer.WriteAttributeString("ref", Reference);
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
