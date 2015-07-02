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
using System.Collections;
using System.ComponentModel;

namespace LicenseKey
{
	/// <summary>
	/// A Summary description for Random.
	/// </summary>
	/// <remarks>
	/// This is an random number class for licensekey.
	/// It provides implementations of all random numbers. 
	/// </remarks>
	public class Randomm 
	{
		int		maxNumber;	// the maximum number allowed from the random generator
		Random	rnd;		// the random number 

		/// <summary>
		/// The random class returns random numbers based on a base. 
		/// </summary>
		public Randomm()
		{
			rnd = new Random(unchecked((int)DateTime.Now.Ticks));
		}

		
		/// <summary>
		/// Sets the max Length of the number. 
		/// </summary>
		/// <param name="maxLength">The length of the number. Needs to be 1-9.</param>
		/// <example>
		/// <code>
		/// rnd.SetMaxLength(numberLength); 
		/// </code>
		/// </example>
		/// <exception cref="System.ApplicationException">Thrown when the Length is too large</exception>
		/// <returns>None</returns>
		public void SetMaxLength(int maxLength)
		{
			int         lopcnt;
	
			// if maxnumber is too large
			if ( maxLength > 9 ) 
			{
				throw new ApplicationException("Length is too large");
			}
			// determine the maximum number. 
			maxNumber = 0;
			for ( lopcnt = 0; lopcnt < maxLength; lopcnt++) 
			{
				maxNumber = maxNumber * 10 + 9;
			}
		}

		
		/// <summary>
		/// Get a random number.
		/// </summary>
		/// <example>
		/// <code>
		/// rannum = rnd.GetRandomNumber(); 
		/// </code>
		/// </example>
		/// <returns>The random number.</returns>
		public int GetRandomNumber()
		{
			int	rannum; 

			rannum = rnd.Next(maxNumber);
			return(rannum);
		}
	}
}
