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
	/// A Summary description for NumberDisplay.
	/// </summary>
	public class NumberDisplay
	{
		/// <summary>
		/// Trim the number to the desired length.
		/// </summary>
		/// <param name="sum">The number that you want the display for.</param>
		/// <param name="numberLength">The length of the trim that you want.</param>
		/// <param name="useBase10">The base of the number you want, (10)true or (16)false.</param>
		private static uint	TrimCheckSum(uint sum, int numberLength, bool useBase10)
		{
			uint	tlen;
			int		lop;
			uint	utemp;

			// Determine the base we are using
			if ( useBase10 ) 
			{
				// if we are using base 10 then
				utemp = (uint) (10 * numberLength);
				utemp = (uint) Math.Pow(10, numberLength);
				while ( sum > utemp ) 
				{
					sum = sum - utemp;
				}
			}
			else 
			{
				// if we are using base 16 then shift them out
				tlen = 0x0;
				for ( lop = 0; lop < numberLength; lop++ ) 
				{
					tlen = tlen << 4;
					tlen = tlen + 0xf;
				}
				sum = sum & tlen;
			}
			return(sum);
		}


		/// <summary>
		/// Create a string representing the number. 
		/// </summary>
		/// <param name="sum">The number that you want the display for.</param>
		/// <param name="numberLength">The length of the display (visible) that you want.</param>
		/// <param name="useBase10">The base of the number you want, (10)true or (16)false.</param>
		/// <example>
		/// <code>
		/// txtChkNumStr.Text = NumberDisplay.CreateNumberString(csum, numberLength, useBase10); 
		/// </code>
		/// </example>
		/// <returns>The resulting string.</returns>
		/// <remarks>Remember that the length should be the visible part.
		///  This has nothing to do with bytes or bits.</remarks>
		public static string CreateNumberString(uint sum, int numberLength, bool useBase10)
		{
			string	ostr;
			int		diflen;
			int		slen;
			// Trim the number
			sum = TrimCheckSum(sum, numberLength, useBase10);
			//
			// if the number is a decimal then convert it, otherwise do the hex
			//
			if (useBase10) 
			{
				slen = sum.ToString("D").Length;
			}
			else 
			{
				slen = sum.ToString("X").Length;
			}
			// find out the number of fields we now have to fill in.
			diflen = numberLength - slen;
			ostr = "";
			// for each we we missed add a leading 0
			while ( diflen > 0 ) 
			{
				ostr = ostr + "0";
				diflen--;
			}
			// now convert our number and add its leading 0 string 
			if (useBase10) 
			{
				ostr = ostr + sum.ToString("D");
			}
			else 
			{
				ostr = ostr + sum.ToString("X");
			}
			return(ostr);
		}

	}
}
