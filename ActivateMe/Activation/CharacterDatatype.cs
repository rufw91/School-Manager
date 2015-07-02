// Copyright (C) 2005  Don Sweitzer
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 2 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
using System;

namespace LicenseKey
{
	/// <summary>
	/// Summary description for CharacterDatatype.
	/// </summary>
	class CharacterDatatype : Datatype
	{
		string		dataValue;	// the data value of this data type

		/// <summary>
		/// The CharacterDatatype contructor.
		/// </summary>
		public CharacterDatatype()
		{
		}

		/// <summary>
		/// Check the token size.
		/// </summary>
		/// <param name="tokStream">The input stream of the token.</param>
		/// <param name="rcnt">The size of the input token.</param>
		/// <returns>true/false if the token is the right size.</returns>
		public override bool CheckTokenSize(string tokStream, int rcnt)
		{
			int		inputTokenSize; 
			bool	flgret;

			// get the size of the token input stream 
			inputTokenSize = tokStream.Length;
			// if we are using bytes then just compare the sizes. 
			if ( !flagBytes ) 
			{
				// otherwise make sure the sizes are divided by 4
				rcnt = rcnt / 4;
			}
			//
			// Check to see if the calculated len is greater that the temp length
			//
			if ( inputTokenSize == rcnt ) 
			{
				flgret = true;		// they are the same so all is OK
				dataValue = tokStream;
			}
			else 
			{
				flgret = false;	// they are not the same so error the return value
			}
			return(flgret);
		}


		/// <summary>
		/// Check the token.
		/// </summary>
		/// <param name="LicTempInp">The license template.</param>
		/// <param name="tokinp">The token value.</param>
		/// <param name="tok">The token.</param>
		public override void CheckToken(string LicTempInp, string tokinp, char tok)
		{
			string	tempstr;
			char	stok;
			int		i;
			int		rcnt;
			int		slen;
			int		ipos;
			bool	flgchk;
			bool	flgfound;

			ipos = 0; 
			tempstr  = LicTempInp;
			//
			// if no tokinput (string representation) is defined in the template
			//
			slen = tokinp.Length;
			if ( slen <= 0 ) 
			{
				// this is ok, there just is not tokens at this point. 
				return;
			}
			//
			// if no token is defined in the template
			//
			ipos = tempstr.IndexOf(tok, ipos);
			if ( ipos < 0 ) 
			{
				// throw a exception that the user can understand. 
				throw new ApplicationException("If you enter a token you must also have an entry in the template string");
			}
			slen = tempstr.Length;
			rcnt = 0;
			flgfound = false;
			for (i = 0; i < slen; i++)
			{
				stok = LicTempInp[i];
				if ( stok == tok ) 
				{
					rcnt++;
				}
				else 
				{
					if ( rcnt != 0 ) 
					{
						if ( flgfound == true ) 
						{
							// throw a exception that the user can understand. 
							throw new ApplicationException("You can not specify more than one of the same token");
						}
						flgchk = CheckTokenSize(tokinp, rcnt);
						if ( flgchk == false) 
						{
							// throw a exception that the user can understand. 
							throw new ApplicationException("Please enter a token that will fit into the size to the specified token");
						}
						flgfound = true;
					}
					rcnt = 0;
				}
			}
			if ( rcnt != 0 ) 
			{
				flgchk = CheckTokenSize(tokinp, rcnt);
				if ( flgchk == false) 
				{
					// throw a exception that the user can understand. 
					throw new ApplicationException("Please enter a token that will fit into the size to the specified token");
				}
			}
		}


		/// <summary>
		/// Create the string representation of the token.
		/// </summary>
		/// <param name="targetLength">The length of the string.</param>
		/// <returns>The string value.</returns>
		public override string CreateStringRepresentation(int targetLength)
		{
			int		inputTokenSize;

			// get the string length
			inputTokenSize = dataValue.Length;
			if ( !flagBytes ) 
			{
				// otherwise make sure the sizes are divided by 4
				targetLength = targetLength / 4;
			}
			if ( inputTokenSize != targetLength ) 
			{
				// throw a exception that the user can understand. 
				throw new ApplicationException("Please enter a token that will fit into the size to the specified token");
			}
			return(dataValue);
		}
	}
}
