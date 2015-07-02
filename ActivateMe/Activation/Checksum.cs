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
using System.Text.RegularExpressions;

namespace LicenseKey
{
	/// <summary>
	/// A Summary description for Checksum.
	/// </summary>
	public class Checksum
	{
		/// <summary>
		/// The enumerated types of the different Algorithms 
		/// </summary>
		public enum ChecksumType 
		{
			/// <summary>
			/// Algorithm type 1, a simple add
			/// </summary>
			Type1,
			/// <summary>
			///  Algorithm type 2, a simple add
			/// </summary>
			Type2,
		};

		ChecksumType	checksumType; 
		ushort	r;
		ushort	c1;
		ushort	c2;
		uint	sum;

		/// <summary>
		/// The constructor for the checksum class.
		/// </summary>
		public Checksum()
		{
		}


		/// <summary>
		/// Gets the Checksum Number.
		/// </summary>
		/// <example>
		/// <code>
		/// chknum = chk.ChecksumNumber; 
		/// </code>
		/// </example>
		public uint ChecksumNumber
		{
			get { return sum; }
		}
		

		/// <summary>
		/// Initialize any of the internal variables.
		/// </summary>
		private void Init()
		{
			sum = 0;
			r   = 55665;
			c1  = 52845;
			c2  = 22719;
		}


		/// <summary>
		/// Get/Set the property to use the different Checksum Algorithms.
		/// </summary>
		/// <example>
		/// <code>
		/// chk.CheckSumAlgorithm = Checksum.ChecksumType.Type2; 
		/// </code>
		/// </example>
		/// <returns>The selected checksum Algorithm.
		/// </returns>
		public ChecksumType ChecksumAlgorithm 
		{
			get { return checksumType; }
			set	
			{
				checksumType = value;
			}
		}


		/// <summary>
		/// Add the selected bytes together. 
		/// </summary>
		/// <param name="vvalue">The byte to add into the sum.</param>
		private void Add(byte vvalue)
		{
			byte cipher = (byte)((ulong)((vvalue ^ (r >> 8))));
			r = (ushort)((cipher + r) * c1 + c2);
			sum += cipher;
		}

		/// <summary>
		/// Algorithm 1. A simple byte add
		/// </summary>
		/// <param name="buffer">The input string to calculate the checksum.</param>
		private void Calcchk1(string buffer)
		{
			byte ba; 

			for(int i = 0; i < buffer.Length; i++)
			{
				ba = (byte)buffer[i];
				Add(ba);
			}
		} 

		/// <summary>
		/// Algorithm 2. A simple byte add
		/// </summary>
		/// <param name="buffer">The input string to calculate the checksum.</param>
		private void Calcchk2(string buffer)
		{
			string	mystring;	// my local string incase the string is an odd length
			ushort	word16a;	// short is an int16
			ushort	word16b;	// short is an int16
			ushort	word16;		// short is an int16
			uint	sumtemp;
			int		count;

			mystring = buffer;
			count = mystring.Length;
			mystring = mystring + "00";	// add one to the length

			for(int i = 0; i < count; i= i + 2)
			{
				word16a = (ushort)(((int)mystring[i]<<8)&0xFF00);
				word16b = (ushort)((int)mystring[i+1]&0xFF);
				word16 = (ushort)(word16a + word16b);
				sum = sum + (uint) word16;
			}
			// take only 16 bits out of the 32 bit sum and add up the carries
			sumtemp = sum>>16;
			while ((sumtemp) != 0) 
			{
				sum = (sum & 0xFFFF)+(sum >> 16);
				sumtemp = sum>>16;
			}

			// one's complement the result
			sum = ~sum;

		}

		/// <summary>
		/// Clean the input string of any unwanted characters.
		/// </summary>
		/// <param name="strIn">The input string to clean unwanted characters.</param>
		/// <returns>The cleaned string.</returns>
		private static string CleanInput(string strIn)
		{
			// Replace invalid characters with empty strings.
			// such as the dash that we have in the string
			return Regex.Replace(strIn, @"-", ""); 
		}

		/// <summary>
		/// Calculate the checksum based on the selected Algorithm. 
		/// </summary>
		/// <param name="licenseKey">The completed license key string.</param>
		public void CalculateChecksum(string licenseKey)
		{
			Init();					// initialize any internal variables.
			licenseKey = CleanInput(licenseKey);	// strip out any illegal characters such as a dash. 
			switch(checksumType)
			{
				case ChecksumType.Type1:
					Calcchk1(licenseKey);	// add the values. 
					break;
				case ChecksumType.Type2:
					Calcchk2(licenseKey);	// checksum the value using method 2
					break;
			}
		}
	}
}
