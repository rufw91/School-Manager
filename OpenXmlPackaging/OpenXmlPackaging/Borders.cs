using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Drawing;

namespace OpenXmlPackaging {

	/// <summary>
	/// Represents a <border> element in the Stylesheet XML
	/// </summary>
	public class Borders : XElementWriter {

		#region Public Properties
		
		public Border Top { get; set; }
		public Border Right { get; set; }
		public Border Bottom { get; set; }
		public Border Left { get; set; }
		public Border Diagonal { get; set; } 

		#endregion

		#region Constructor

		public Borders() : this(BorderStyles.None)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Borders" /> class.
		/// By Default, it creates all border elements with BorderStyles.None
		/// </summary>
		/// <param name="border">The border.</param>
		public Borders(BorderStyles border) {
			Top = new Border() { BorderType = BorderType.Top, BorderStyle = border, Color = Color.Black };
			Bottom = new Border() { BorderType = BorderType.Bottom, BorderStyle = border, Color = Color.Black };
			Right = new Border() { BorderType = BorderType.Right, BorderStyle = border, Color = Color.Black };
			Left = new Border() { BorderType = BorderType.Left, BorderStyle = border, Color = Color.Black };
			Diagonal = new Border() { BorderType = BorderType.Diagonal, BorderStyle = border, Color = Color.Black };
		}

		#endregion

		#region XElementWriter Members
		
		internal override string ParentNode {
			get { return "borders"; }
		}

		internal override XElement Element {
			get {
				XElement borders = new XElement(Constants.MainXNamespace + "border");
				AddBorder(borders, Left);
				AddBorder(borders, Right);
				AddBorder(borders, Top);
				AddBorder(borders, Bottom);
				AddBorder(borders, Diagonal);
				return borders;
			}
		}

		#endregion

		#region Private Methods

		private void AddBorder(XElement borders, Border side) {
			if (side != null) {
				borders.Add(side.Element);
			}
		}
		
		#endregion        
	}
}
