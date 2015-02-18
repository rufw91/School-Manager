using System;
using System.IO.Packaging;
using System.IO;


namespace OpenXmlPackaging {
    /// <summary>
    /// Represents the Workbook
    /// </summary>
    public sealed class SpreadsheetDocument : IDisposable {
        
        #region Private Members

        private Package _package;

        private Workbook _workbook;

        private Stylesheet _stylesheet;

        private Worksheets _worksheets;
        internal bool IsOpener{get;set;}
        #endregion

        #region Constructors

        public SpreadsheetDocument(string path) {
            IsOpener = false;
            CreateSpreadsheetDocument(path, FileMode.Create);
        }

        SpreadsheetDocument()
        {
            IsOpener = false;
        }
        
        #endregion

        #region Public Methods
        public static SpreadsheetDocument Open(string path)
        {
            SpreadsheetDocument s = new SpreadsheetDocument();
            s.IsOpener = true;
            s.Package = Package.Open(path, FileMode.Open);

            s.Workbook = new Workbook();
            s.Workbook.Open(s.Package);
            s.Stylesheet = new Stylesheet();
            s.Stylesheet.Open(s.Package);
            s.Worksheets = new Worksheets();
            s.Worksheets.Open(s.Package, s.Stylesheet);
            return s;
        }
        public static bool Test(string path)
        {
            try
            {
                var p =Package.Open(path, FileMode.Open);
                p.Close();
                return true;
            }
            catch { return false; }
        }

#       endregion

        #region Public Properties

        public Workbook Workbook {
            get { return _workbook; }
            set { _workbook = value; }
        }

        public Stylesheet Stylesheet {
            get { return _stylesheet; }
            set { _stylesheet = value; }
        }

        public Worksheets Worksheets {
            get { return _worksheets; }
            set { _worksheets = value; }
        } 

        public Package Package {
            get { return _package; }
            set { _package = value; }
        }

        #endregion
        
        #region Private Methods

        private void CreateSpreadsheetDocument(string path, FileMode mode) {
            _package = Package.Open(path, mode);
            _workbook = new Workbook(_package);
            _stylesheet = new Stylesheet(_package);
            _worksheets = new Worksheets(_package, _stylesheet);
        }
        
        #endregion
        
        #region IDisposable Member

        public void Dispose() {
            try {
                if (!IsOpener)
                {
                    _stylesheet.Save();
                    _worksheets.Save();
                    _package.Flush();
                }               
                _package.Close();
            } catch {
                throw;
                // TODO :: Exception handling logic goes here
            }
        } 
        
        #endregion

        public static string GetDimension(string pathToFile)
        {
            string dim = "";
            using (SpreadsheetDocument s = SpreadsheetDocument.Open(pathToFile))
            {
                dim = s.Worksheets[0].Dimension;
                s.Dispose();
            }
            return dim;
        }
    }
}
