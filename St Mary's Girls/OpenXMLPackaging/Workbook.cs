﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Packaging;
using System.Xml;
using System.IO;
using System.Xml.Linq;

namespace OpenXmlPackaging {

    /// <summary>
    /// Represents a workbook
    /// </summary>
    public class Workbook {
        private static Package _package;
        private static Worksheets _worksheets;
        private static PackagePart _workbookPart;

        public Worksheets Worksheets { get { return _worksheets; } }
        public static XDocument WorkbookXml { get; private set; }

        public Stylesheet Stylesheet { get; private set; }

        internal Workbook(Package package) {
            _package = package;

            AddWorkbook();
        }

        internal Workbook()
        { }

        private void AddWorkbook() {
            _workbookPart = _package.CreatePart(Constants.WorkbookUri, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml");

            _package.CreateRelationship(Constants.WorkbookUri, TargetMode.Internal, Constants.RelationshipNamespace + "/officeDocument");

            using (XmlWriter writer = XmlWriter.Create(_workbookPart.GetStream(FileMode.Create, FileAccess.Write))) {

                XElement e = new XElement(Constants.MainXNamespace + "workbook", new XAttribute(XNamespace.Xmlns + "r", Constants.RelationshipXNamespace.NamespaceName),
                                        new XElement(Constants.MainXNamespace + "sheets"));

                WorkbookXml = new XDocument(new XElement(Constants.MainXNamespace + "workbook", new XAttribute(XNamespace.Xmlns + "r", Constants.RelationshipXNamespace.NamespaceName),
                                        new XElement(Constants.MainXNamespace + "sheets")));
                WorkbookXml.WriteTo(writer);
            }
            _worksheets = new Worksheets(_package, Stylesheet);
        }

        internal void Open(Package package)
        {
            _package = package;
            if (package.PartExists(Constants.WorkbookUri))
            {
                _workbookPart = package.GetPart(Constants.WorkbookUri);
                _worksheets = new Worksheets(_package, Stylesheet);
            }
            else
            {
                throw new Exception("Please add a workbook before instantiating a Sheet");
            }
        }
    }
}
