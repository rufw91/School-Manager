using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace OpenXmlPackaging
{
    public abstract class DataReaderAndWriter
    {
        /// <summary>
        /// Gets or sets the zero based index of the element in respective file.
        /// </summary>
        /// <value>
        /// The index.
        /// </value>
        public virtual int Index { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance has count.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has count; otherwise, <c>false</c>.
        /// </value>
        internal virtual bool HasCount { get { return true; } }

        /// <summary>
        /// Write the element using XmlWriter
        /// </summary>
        /// <param name="writer">XmlWriter instance</param>
        /// <returns>true if the PostWrite method should be called after this method returns</returns>
        internal abstract bool Write(XmlWriter writer);

        /// <summary>
        /// Do operations post writing to the writer
        /// </summary>
        /// <param name="writer">XmlWriter instance</param>
        internal virtual void PostWrite(XmlWriter writer)
        {
            return;
        }

        /// <summary>
        /// Write the element using XmlWriter
        /// </summary>
        /// <param name="writer">XmlWriter instance</param>
        /// <returns>true if the PostWrite method should be called after this method returns</returns>
        internal abstract bool Read(XmlReader writer);
    }
}
