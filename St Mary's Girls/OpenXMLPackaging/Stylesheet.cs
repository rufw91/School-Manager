﻿using System;
using System.Linq;
using System.Xml.Linq;
using System.IO.Packaging;
using System.Xml;
using System.IO;
using System.Collections.Generic;

namespace OpenXmlPackaging {
    /// <summary>
    /// Represents the Stylesheet
    /// </summary>
    public class Stylesheet {
        #region Private Members
        
        private Package _package;
        private PackagePart _workbookPart;
        private List<Style> _styles;
    
        /// <summary>
        /// Gets the style XML.
        /// </summary>
        private XDocument _styleXml;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Stylesheet"/> class.
        /// </summary>
        /// <param name="package">The package.</param>
        internal Stylesheet(Package package) {
            _package = package;
            if (_package.PartExists(Constants.WorkbookUri)) {
                _workbookPart = package.GetPart(Constants.WorkbookUri);
            } else {
                throw new Exception("Please add a workbook before instantiating Stylesheet");
            }

            _styles = new List<Style>();

            CreateStylesheet();
        }

        internal Stylesheet() { }
        #endregion

        #region Public Methods
        
        /// <summary>
        /// Adds the style to the list if it does not exist
        /// </summary>
        /// <param name="style">Style Element</param>
        public void AddStyle(Style style) {
            if (style != null) {
                if (!_styles.Contains(style)) {
                    _styles.Add(style);
                }
            }
        }

        /// <summary>
        /// Saves this Stylesheet.
        /// </summary>
        public void Save() {
            FormatCells();
            PackagePart worksheetPart = _package.CreatePart(Constants.StylesUri, "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml");
            _workbookPart.CreateRelationship(new Uri("/xl/styles.xml", UriKind.Relative), TargetMode.Internal, Constants.RelationshipXNamespace.NamespaceName + "/styles");

            using (Stream stream = worksheetPart.GetStream(FileMode.Create, FileAccess.Write)) {
                using (XmlWriter writer = XmlWriter.Create(stream)) {
                    _styleXml.WriteTo(writer);
                }
            }
        }
        
        #endregion

        #region Private and Internal Methods
        
        /// <summary>
        /// Adds the style.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="styleElement">The style element.</param>
        private void AddStyle<T>(T styleElement) where T : XElementWriter {

            // Get the parent node
            var parentNode = _styleXml.Descendants(Constants.MainXNamespace + styleElement.ParentNode).FirstOrDefault() ??
                             new XElement(Constants.MainXNamespace + styleElement.ParentNode);

            var element = styleElement.Element;
            // Add the style element
            parentNode.Add(element);

            if (styleElement.HasCount) {
                if (parentNode.Attribute("count") == null) {
                    parentNode.Add(new XAttribute("count", "0"));
                }

                // Get the count
                var count = Int32.Parse(parentNode.Attribute("count").Value);
                count++;

                // update count value
                parentNode.Attribute("count").SetValue(count);

                // Set the index of element in the list
                styleElement.Index = count - 1;

                styleElement.PostProcess(element);
            }
        }

        /// <summary>
        /// Formats the cell.
        /// </summary>
        /// <param name="format">The format.</param>
        internal void FormatCells() {
            if (_styles.Count > 0) {
                foreach (var format in _styles) {
                    bool shouldApplyFormat = false;

                    if (format.Borders != null) {
                        AddStyle(format.Borders);
                        shouldApplyFormat = true;
                    }

                    if (format.Fill != null) {
                        AddStyle(format.Fill);
                        shouldApplyFormat = true;
                    }

                    if (format.Font != null) {
                        AddStyle(format.Font);
                        shouldApplyFormat = true;
                    }

                    if (format.NumberFormat != null) {
                        AddStyle(format.NumberFormat);
                        shouldApplyFormat = true;
                    }

                    if (shouldApplyFormat) {
                        AddStyle<Style>(format);
                    }
                }
            }
        }

        /// <summary>
        /// Creates the stylesheet with basic structure.
        /// </summary>
        private void CreateStylesheet() {

            _styleXml = new XDocument(
                            new XElement(Constants.MainXNamespace + "styleSheet",
                                new XElement(Constants.MainXNamespace + "numFmts", new XAttribute("count", "0")),
                                new XElement(Constants.MainXNamespace + "fonts", new XAttribute("count", "1"),
                                    new XElement(Constants.MainXNamespace + "font")),
                                new XElement(Constants.MainXNamespace + "fills", new XAttribute("count", "2"),
                                    new XElement(Constants.MainXNamespace + "fill",
                                        new XElement(Constants.MainXNamespace + "patternFill",
                                                        new XAttribute("patternType", "none"))),
                                    new XElement(Constants.MainXNamespace + "fill",
                                        new XElement(Constants.MainXNamespace + "patternFill",
                                                        new XAttribute("patternType", "gray125")))),
                                new XElement(Constants.MainXNamespace + "borders", new XAttribute("count", "1"),
                                    new XElement(Constants.MainXNamespace + "border")),
                                new XElement(Constants.MainXNamespace + "cellXfs", new XAttribute("count", "1"),
                                    new XElement(Constants.MainXNamespace + "xf",
                                        new XAttribute("fillId", "0"))),
                                new XElement(Constants.MainXNamespace + "cellStyles", new XAttribute("count", "1"),
                                    new XElement(Constants.MainXNamespace + "cellStyle",
                                        new XAttribute("name", "Normal"),
                                        new XAttribute("xfId", "0")))
                            ));

            PackagePart styleSheetPart = _package.CreatePart(Constants.StylesUri, "application/vnd.openxmlformats-officedocument.spreadsheetml.styles+xml");
            using (var writer = XmlWriter.Create(styleSheetPart.GetStream(FileMode.Create, FileAccess.Write))) {
               _workbookPart.CreateRelationship(new Uri("/xl/styles.xml", UriKind.Relative), TargetMode.Internal, Constants.RelationshipXNamespace.NamespaceName + "/styles");
               _styleXml.WriteTo(writer);
            }
        }
        
        #endregion

        internal void Open(Package package)
        {
            _package = package;
            if (_package.PartExists(Constants.WorkbookUri))
            {
                _workbookPart = package.GetPart(Constants.WorkbookUri);
            }
            else
            {
                throw new Exception("Please add a workbook before instantiating Stylesheet");
            }

            _styles = new List<Style>();

            PackagePart styleSheetPart = package.GetPart(Constants.StylesUri);
            using (var reader = XmlReader.Create(styleSheetPart.GetStream(FileMode.Open, FileAccess.Read))) 
            {
                _styleXml = XDocument.Load(reader);               
            }
        }
    }
}
