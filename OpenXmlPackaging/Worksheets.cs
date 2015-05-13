using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.IO.Packaging;
using System.Xml;
using System.Xml.Linq;

namespace OpenXmlPackaging {
	/// <summary>
	/// Handles all the Worksheets in the Workbook
	/// </summary>
	public class Worksheets : IEnumerable {
		
		#region Private Variables
		
		private readonly Package _package;
		private readonly PackagePart _workbookPart;
		private readonly List<Worksheet> _worksheet;
		private readonly Stylesheet _stylesheet;

		#endregion

		#region Private Methods
		
		private Worksheet AddWorksheet(string name) {
			try {
				PackageRelationship sheetRelationship = null;
				using (Stream workbookStream = _workbookPart.GetStream(FileMode.Open, FileAccess.ReadWrite)) {
					Uri sheetUri = null;
					using (var writer = XmlWriter.Create(workbookStream)) {
						var document = XDocument.Load(workbookStream);

						XElement sheets = document.Descendants().FirstOrDefault(d => d.Name.Equals(Constants.MainXNamespace + "sheets"));

						var sheetElements = sheets.Descendants(Constants.MainXNamespace + "sheet");
						int sheetId;

						if (sheetElements.Any()) {

							if (sheetElements.Any(se => se.Attribute("name").Value == name)) {
								throw new Exception(String.Format("Sheet with name {0} already exists", name));
							}

							Int32.TryParse(sheetElements.Last().Attribute("sheetId").Value, out sheetId);
							sheetId++;
						}
						else {
							
							sheetId = 1;
						}
						sheetUri = new Uri(String.Format(Constants.SheetUriFormatPath, sheetId), UriKind.Relative);
						sheetRelationship = _workbookPart.CreateRelationship(sheetUri, TargetMode.Internal, Constants.RelationshipNamespace + "/worksheet");

						sheets.Add(new XElement(Constants.MainXNamespace + "sheet"
													, new XAttribute("name", name)
													, new XAttribute("sheetId", sheetId)
													, new XAttribute(Constants.RelationshipXNamespace + "id", sheetRelationship.Id)));

						// Clear the workbook xml file
						workbookStream.SetLength(0);

						document.WriteTo(writer);

						
					}


					PackagePart worksheetPart = _package.CreatePart(sheetUri, Constants.WorksheetContentType);

					var worksheet = new Worksheet(_stylesheet, worksheetPart) {
																				Name = name,
																				RelationshipId = sheetRelationship.Id,
																			};

					using (var writer = XmlWriter.Create(worksheetPart.GetStream(FileMode.Create, FileAccess.Write))) {
						worksheet.WorksheetXml = new XDocument(new XElement(Constants.MainXNamespace + "worksheet",
													new XAttribute(XNamespace.Xmlns + "r", Constants.RelationshipNamespace)));

						worksheet.WorksheetXml.WriteTo(writer);
					}

					return worksheet;
				}
			}
			catch {
				// TODO :: Add exception handling logic
				throw;
			}
		} 

		#endregion

		#region Constructor
		
		public Worksheets(Package package, Stylesheet styleSheet) {
			_package = package;
			_stylesheet = styleSheet;

			if (package.PartExists(Constants.WorkbookUri)) {
				_workbookPart = package.GetPart(Constants.WorkbookUri);
			}
			else {
				throw new Exception("Please add a workbook before instantiating a Sheet");
			}

            _worksheet = GetWorkSheets(package);
		}

        private List<Worksheet> GetWorkSheets(Package package)
        {
            List<Worksheet> temp = new List<Worksheet>();

            try
            {
                using (Stream workbookStream = _workbookPart.GetStream(FileMode.Open, FileAccess.Read))
                {

                    using (var reader = XmlReader.Create(workbookStream))
                    {
                        var document = XDocument.Load(reader);

                        XElement sheets = document.Descendants().FirstOrDefault(d => d.Name.Equals(Constants.MainXNamespace + "sheets"));

                        var sheetElements = sheets.Descendants(Constants.MainXNamespace + "sheet");
                        PackagePart sharedStringsPart = package.GetPart(Constants.SharedStringsUri);
                        Dictionary<int, string> strings = GetSharedStrings(sharedStringsPart);
                        foreach (var s in sheetElements)
                        {
                            int sheetId = int.Parse(s.Attribute("sheetId").Value);
                            string name = s.Attribute("name").Value;
                            Uri sheetUri = new Uri(String.Format(Constants.SheetUriFormatPath, sheetId), UriKind.Relative);
                            PackagePart worksheetPart = _package.GetPart(sheetUri);
                            PackageRelationship sheetRelationship = _workbookPart.GetRelationships().FirstOrDefault(d => d.TargetUri == sheetUri);
                            var worksheet = new Worksheet(_stylesheet, worksheetPart)
                            {
                                Name = name,
                                // RelationshipId = sheetRelationship.Id,
                            };

                            using (var rx = XmlReader.Create(worksheetPart.GetStream(FileMode.Open, FileAccess.Read)))
                            {
                                worksheet.Open(rx, strings);

                            }
                            using (var rx = XmlReader.Create(worksheetPart.GetStream(FileMode.Open, FileAccess.Read)))
                            {
                                worksheet.WorksheetXml = XDocument.Load(rx);
                            }
                            temp.Add(worksheet);
                        }
                    }
                }
            }
            catch
            {
                // TODO :: Add exception handling logic
                //throw;
            }

            return temp;
        }

        private Dictionary<int, string> GetSharedStrings(PackagePart sharedStringsPart)
        {
            Dictionary<int, string> temp = new Dictionary<int, string>();
            int count = 0;
            using (var reader = XmlReader.Create(sharedStringsPart.GetStream(FileMode.Open, FileAccess.Read)))
            {
                try
                {
                    while (reader.Read())
                    {
                        switch (reader.NodeType)
                        {
                            case XmlNodeType.Element:
                                if (reader.Name == "t")
                                {
                                    string s = reader.ReadElementString();
                                    temp.Add(count, s);
                                    count++;
                                }
                                break;
                            default: break;
                        }
                    }
                }
                catch { }
            }
            return temp;
        }
    

		#endregion

		#region IEnumerable

		public IEnumerator GetEnumerator() {
			return _worksheet.GetEnumerator();
		}

		#endregion

		#region Manage worksheets

		public Worksheet Add(string name) {
			if (String.IsNullOrWhiteSpace(name)) {
				throw new NullReferenceException("item cannot be null");
			}

			Worksheet sheet = AddWorksheet(name);
			_worksheet.Add(sheet);
			return sheet;
		}

		public void Clear() {
			_worksheet.Clear();
		}

		public int Count {
			get { return _worksheet.Count; }
		}

		public bool Remove(Worksheet item) {
			return _worksheet.Remove(item);
		}

		public Worksheet this[int index] {
			get { return _worksheet[index]; }
		}

		public Worksheet this[string name] {
			get { return _worksheet.FirstOrDefault(w => w.Name == name); }
		}

		public void Save() {
			foreach (var worksheet in _worksheet) {
				worksheet.Save();
			}
		}

		#endregion

	}
}
