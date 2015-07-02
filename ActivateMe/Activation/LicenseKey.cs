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
	/// A Summary description for GenerateKey.
	/// </summary>
	/// <remarks>
	/// This is the License Key class for generating License keys.
	/// It provides implementations of all License key operations. 
	/// </remarks>
	public class GenerateKey
	{
		/// <summary>
		/// Enumerated data types.
		/// </summary>
		public enum TokenTypes 
		{
			/// <summary>
			/// The data type is Numeric.
			/// </summary>
			NUMBER,
			/// <summary>
			///  The data type is Date.
			/// </summary>
			DATE,
			/// <summary>
			/// The data type is Character.
			/// </summary>
			CHARACTER
		};

		/// <summary>
		/// Internal structure for the tokens.
		/// </summary>
		internal struct Tokens 
		{
			/// <summary>
			/// The data type
			/// </summary>
			TokenTypes	tokenType;

			/// <summary>
			/// The character token
			/// </summary>
			char	characterToken;

			/// <summary>
			/// The value of the item
			/// </summary>
			string	initialValue;

			/// <summary>
			/// 
			/// </summary>
			public Datatype	datatype;

			/// <summary>
			/// Add a token into the collection.
			/// </summary>
			/// <param name="characterTokenAdd">The character token.</param>
			/// <param name="tokenTypeAdd">The data type.</param>
			/// <param name="dataValue">The data value.</param>
			public void	Add(char characterTokenAdd, TokenTypes tokenTypeAdd, string dataValue)
			{
				characterToken = characterTokenAdd;
				tokenType = tokenTypeAdd;
				initialValue = dataValue;

				switch(tokenTypeAdd) 
				{
					case TokenTypes.NUMBER:
						datatype = new NumberDatatype();
						break;
					case TokenTypes.CHARACTER:
						datatype = new CharacterDatatype();
						break;
					case TokenTypes.DATE:
						// TODO  Fill in the date code later. 
						//datatype = new CharacterDatatype(); // not done yet
						break;
				}
			}


			/// <summary>
			/// Property to set/get the character token.
			/// </summary>		
			public char CharacterToken
			{
				get { return characterToken; }
				set	
				{
					characterToken = value;
				}
			}


			/// <summary>
			/// Property to set/get the character token.
			/// </summary>		
			public TokenTypes TokenType
			{
				get { return tokenType; }
				set	
				{
					tokenType = value;
				}
			}


			/// <summary>
			/// Property to set/get the current value.
			/// </summary>		
			public string InitialValue
			{
				get { return initialValue; }
				set	
				{
					initialValue = value;
				}
			}
		}
		Tokens [] tokens;		// the list of tokens that can be used in the lciense string. 
		int		maxTokens;		// the maximum number of tokens that was set
		string	licenseTemplate;// the template string that will be used to create the license key
		bool	flagBase10;		// do we use base 10 or base 16
		bool	flagBytes;		// do we use bytes or bits
		Randomm	rnd;			// out random number class
		string	strLicensekey;	// the final license key that was made. 
		Checksum.ChecksumType	chktype;	// Checksum Algorithm type

		/// <summary>
		/// Find the token in the array. return the offset otherwise return a -1.
		/// </summary>
		/// <param name="tokenToFind">Token to find in the array structre.</param>
		/// <returns>Offset otherwise a -1</returns>
		private int	FindToken(char tokenToFind)
		{
			int		retoff=-1; 
			int		lop;

			// for each entry find the token in question. 
			for ( lop = 0; lop < maxTokens; lop++) 
			{
				// Is this the token we are looking for? 
				if ( tokens[lop].CharacterToken == tokenToFind) 
				{
					retoff = lop;
					break;
				}
			}
			return(retoff);
		}


		/// <summary>
		/// GenerateKey constructor.
		/// </summary>
		public GenerateKey()
		{
			// initialize our variables. 
			flagBase10 = false;		// default to use base 16
			flagBytes = false;		// default to use bits

			// create the random object
			rnd = new Randomm();

			// initialzie the license key
			strLicensekey = "";
		}


		/// <summary>
		/// Method used to get the final license key.
		/// </summary>
		/// <example>
		/// <code>
		/// finalkey = gkey.GetLicenseKey(); 
		/// </code>
		/// </example>
		/// <returns>The license key.</returns>
		public string GetLicenseKey()
		{
			return(strLicensekey);
		}


		/// <summary>
		/// Get/Set the property to use bytes or bits in the template string
		/// and for all operations. 
		/// </summary>
		/// <example>
		/// <code>
		/// gkey.UseBytes = false; 
		/// </code>
		/// </example>
		/// <returns>Use bytes (true) or bits (false)</returns>
		public bool UseBytes
		{
			get { return flagBytes; }
			set	
			{
				flagBytes = value;
			}
		}


		/// <summary>
		/// Get/Set the property to use base 10 (true) or base 16 (false) for the numbers.
		/// </summary>
		/// <example>
		/// <code>
		/// gkey.UseBase10 = true; 
		/// </code>
		/// </example>
		/// <returns>Base 10 (true) or base 16 (false).</returns>
		public bool UseBase10
		{
			get { return flagBase10; }
			set	
			{
				flagBase10 = value;
			}
		}


		/// <summary>
		/// Get/Set the property to the Checksum Algorithm.
		/// </summary>
		/// <example>
		/// <code>
		/// gkey.CheckSumAlgorithm = ChecksumType.Type1; 
		/// </code>
		/// </example>
		/// <returns>The Algorithm type.</returns>
		public Checksum.ChecksumType ChecksumAlgorithm
		{
			get { return chktype; }
			set	
			{
				chktype = value;
			}
		}


		/// <summary>
		/// Get/Set the maximum number of tokens that will be used.
		/// </summary>
		/// <example>
		/// <code>
		/// gkey.MaxTokens = 3; 
		/// </code>
		/// </example>
		/// <returns>The maximum number of tokens.</returns>
		public int MaxTokens
		{
			get { return maxTokens; }
			set	
			{
				tokens = new Tokens[value] ;
				maxTokens = value;
			}
		}
		
		
		/// <summary>
		/// Get/Set the license template string.
		/// </summary>
		/// <example>
		/// <code>
		/// gkey.LicenseTemplate = "xxvv-xxxxxxxx-xxxxxxxx-ppxx"; 
		/// </code>
		/// </example>
		/// <returns>The license template.</returns>
		public string LicenseTemplate
		{
			get { return licenseTemplate; }
			set	
			{
				licenseTemplate = value;
			}
		}


		/// <summary>
		/// Add a token into the array. 
		/// </summary>
		/// <param name="location">Location of the data within the array.</param>
		/// <param name="characterToken">The string representation of the characer token.</param>
		/// <param name="tokenTypeAdd">The type of value that will be used in the transformation.</param>
		/// <param name="initialValue">The initial value of the data.</param>
		/// <example>
		/// <code>
		/// gkey.AddToken(0, "v", LicenseKey.GenerateKey.TokenTypes.NUMBER, "1"); 
		/// </code>
		/// </example>
		/// <exception cref="System.ApplicationException">Thrown when the location is out of bounds</exception>
		/// <returns>None</returns>
		public void AddToken(int location, string characterToken, TokenTypes tokenTypeAdd, string initialValue)
		{
			char		ch; // temp variable to hold the character token

			// Has the caller requested a valid location? 
			if ( location < maxTokens) 
			{
				// add the data
				ch = Convert.ToChar(characterToken);
				tokens[location].Add(ch, tokenTypeAdd, initialValue);
				return;
			}
			throw new ApplicationException("Location is not with array bounds");
		}


		/// <summary>
		/// Get a random number so we can add it into the license key string. 
		/// </summary>
		/// <param name="numberLength">The size of the field.</param>
		private void GetRandomNumber(int numberLength)
		{
			int		rndnum;
			int		remain;
			//
			// if bits has been selected then we need to check for a 4 bit multiple 
			//
			if (flagBytes == false) 
			{
				remain = numberLength % 4;
				if ( remain != 0 ) 
				{
					throw new ApplicationException("For Bits values each block should be a multiple of 4");
				}
				numberLength = numberLength / 4;
			}
			rnd.SetMaxLength(numberLength);
			rndnum = rnd.GetRandomNumber();
			strLicensekey = strLicensekey + NumberDisplay.CreateNumberString((uint)rndnum, numberLength, flagBase10);
			return;
		}

		/// <summary>
		/// Get the checksum number so we can add it into the license key string. 
		/// </summary>
		/// <param name="Licensekey">The license key string.</param>
		/// <param name="numberLength">The size of the field.</param>
		/// <param name="includeLicensekey">Include the original license key as part of the return value.</param>
		private string  GetChecksumNumber(string Licensekey, int numberLength, bool includeLicensekey)
		{
			int			remain;
			Checksum	chk;
			//
			// create the checksum class
			//
			chk = new Checksum();
			//
			// if bits has been selected then we need to check for a 4 bit multiple 
			//
			if (flagBytes == false) 
			{
				remain = numberLength % 4;
				if ( remain != 0 ) 
				{
					throw new ApplicationException("For Bits values each block should be a multiple of 4");
				}
				numberLength = numberLength / 4;
			}
			chk.ChecksumAlgorithm = chktype;
			chk.CalculateChecksum(Licensekey);
			if ( includeLicensekey ) 
			{
				uint	csum;

				csum = chk.ChecksumNumber;
				Licensekey = Licensekey + NumberDisplay.CreateNumberString(csum, numberLength, this.UseBase10);
			}
			else 
			{
				uint	csum;

				csum = chk.ChecksumNumber;
				Licensekey = NumberDisplay.CreateNumberString(csum, numberLength, this.UseBase10);
			}
			return (Licensekey);
		}



		/// <summary>
		/// Create the License key.
		/// </summary>
		/// <example>
		/// <code>
		/// gkey = new GenerateKey();
		/// gkey.LicenseTemplate = "vvvvppppxxxxxxxxxxxx-wwwwwwwwxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx-xxxxxxxxxxxxxxxxxxxx";
		/// gkey.MaxTokens = 3;
		/// gkey.AddToken(0, "v", LicenseKey.GenerateKey.TokenTypes.NUMBER, "1");
		/// gkey.AddToken(1, "p", LicenseKey.GenerateKey.TokenTypes.NUMBER, "2");
		/// gkey.AddToken(2, "w", LicenseKey.GenerateKey.TokenTypes.CHARACTER, "QR");
		/// gkey.UseBase10 = false;
		/// gkey.UseBytes = false;
		/// gkey.<b>CreateKey</b>();
		/// finalkey = gkey.GetLicenseKey();
		/// </code>
		/// </example>
		/// <exception cref="System.ApplicationException">Thrown when no key template has been entered.</exception>
		/// <exception cref="System.ApplicationException">Thrown when no the Token is not found in input list.</exception>
		/// <returns>None</returns>
		public void CreateKey()
		{
			int		lop;
			int		slen;
			char	slast;
			char	stok;
			int		scnt;
			int		i;
			int		retoff;
			//
			// Initialize the finalkey so we do not reuse it. 
			//
			strLicensekey = "";
			//
			// Check the length of the template string 
			//
			slen = licenseTemplate.Length;
			if ( slen == 0 ) 
			{
				// throw a exception that the user can understand. 
				throw new ApplicationException("Enter a key template");
			}
			//
			// if a token is in the license string then make sure it fits into it's field for size.
			// make sure it is as large as what is entered in the tempate string
			//
			for ( lop = 0; lop < maxTokens; lop++ ) 
			{
				try 
				{
					tokens[lop].datatype.UseBase10 = flagBase10;
					tokens[lop].datatype.UseBytes = flagBytes;
					tokens[lop].datatype.CheckToken(licenseTemplate, tokens[lop].InitialValue, tokens[lop].CharacterToken);
				}
				catch
				{
					throw;
				}
			}
			// initialize the variables. 
			slast = '\0';
			scnt = 0; 
			// now go through the license template to see what tokens are found. 
			// fill in the license key string now that we know everything will fit. 
			for (i = 0; i < slen; i++)
			{
				stok = licenseTemplate[i];
				if ( stok != slast ) 
				{
					if ( scnt != 0 ) 
					{
						// find the token in the token class
						retoff = FindToken(slast);
						if ( retoff == -1) 
						{
							// we will not see a x in the list so if it is an
							// x then handle it here. 
							switch(slast) 
							{
								case 'x':
									try 
									{
										GetRandomNumber(scnt);
									}
									catch 
									{
										throw;
									}
									break;
								case 'c':
									try 
									{
										strLicensekey = GetChecksumNumber(strLicensekey, scnt, true);
									}
									catch 
									{
										throw;
									}
									break;
								case '-':
									strLicensekey = strLicensekey + "-";
									break;
								default:
									// we could not find the token so this is illegal
									throw new ApplicationException("Token not found in input list");
							}
						}
						else 
						{
							strLicensekey = strLicensekey + tokens[retoff].datatype.CreateStringRepresentation(scnt);
						}
						scnt = 1;
					}
					else 
					{
						scnt++;
					}
				}
				else 
				{
					scnt++;
				}
				slast = stok; 
			}
			//
			// handle anything that was left over. 
			//
			if ( scnt != 0 ) 
			{
				// find the token in the token class
				retoff = FindToken(slast);
				if ( retoff == -1) 
				{
					// we will not see a x in the list so if it is an
					// x then handle it here. 
					switch(slast) 
					{
						case 'x':
							try 
							{
								GetRandomNumber(scnt);
							}
							catch 
							{
								throw;
							}
							break;
						case 'c':
							try 
							{
								strLicensekey = GetChecksumNumber(strLicensekey, scnt, true);
							}
							catch (Exception ex) 
							{
								throw;
							}
							break;
						case '-':
							strLicensekey = strLicensekey + "-";
							break;
						default:
							// we could not find the token so this is illegal
							throw new ApplicationException("Token not found in input list");
					}
				}
				else 
				{
					strLicensekey = strLicensekey + tokens[retoff].datatype.CreateStringRepresentation(scnt);
				}
			}
			return;
		}

		/// <summary>
		/// See if the input string has a checksum character.
		/// </summary>
		/// <param name="strIn">The input string to check.</param>
		/// <returns>bool flag.</returns>
		static bool MatchInput(string strIn)
		{
			bool	bans;
			// Replace invalid characters with empty strings.
			// such as the dash that we have in the string
			Match m = Regex.Match(strIn, @"c", RegexOptions.IgnoreCase); 
			if ( m.Success ) 
			{
				bans = true;
			}
			else 
			{
				bans = false;
			}
			return(bans);
		}

		/// <summary>
		/// Clean the input string of any unwanted characters.
		/// </summary>
		/// <param name="strIn">The input string to clean unwanted characters.</param>
		/// <returns>The cleaned string.</returns>
		static string CleanInput(string strIn)
		{
			// Replace invalid characters with empty strings.
			// such as the dash that we have in the string
			return Regex.Replace(strIn, @"-", ""); 
		}

		static int GetChecksumToklength(string	lickey)
		{
			int		i;
			int		cnt; 
			int		slen;

			// determine the length of the key
			slen = lickey.Length;

			// now go through the license template to see what tokens are found. 
			// fill in the license key string now that we know everything will fit. 
			for (i = 0, cnt = 0; i < slen; i++)
			{
				if ( lickey[i] == 'c' ) 
				{
					cnt++;
				}
			}
			return(cnt);
		}

		/// <summary>
		/// Check the legal input for bit templates.
		/// </summary>
		/// <param name="lictemp">The license Template.</param>
		/// <param name="lickey">The license Key.</param>
		/// <returns></returns>
		bool CheckInputLegalBit(string lictemp, string lickey)
		{
			bool	answer = true;
			int		remain;
			int		tokenCnt; 
			int		slen;
			int		i;
			int		j;
			char	tokTem;
			char	tokLic = ' ';
			char	tokJunk;
			string	chkLicString;
			string	chkTemString;
			string	chktemp1;
			string	chktemp2;

			// initialize the string
			chkLicString = "";
			chkTemString = "";

			// check the length of the strings. 
			if ( lictemp.Length / 4 != lickey.Length ) 
			{
				throw new ApplicationException("Template and key string lengths are not equal");
			}

			// since we are looking at the tokens being on a byte boundary  

			// determine the length of the template
			slen = lictemp.Length;

			// now go through the license template to see what tokens are found. 
			// fill in the license key string now that we know everything will fit. 
			for (i = 0; i < slen; i++)
			{
				tokTem = lictemp[i];
				// every four move the license as well. 
				remain = i % 4;
				if ( (remain == 0) ) 
				{
					tokLic = lickey[i/4];
				}
				if ( tokTem == 'c' ) 
				{
					// now that we have found the first token. 
					tokenCnt = i / 4;		// remember we are on a byte boundary
					if ( tokenCnt <= 0 )
					{
						throw new ApplicationException("No space for the checksum value");
					}
					int	toklen = GetChecksumToklength(lictemp);
					// remember we are in bits mode
					toklen = toklen /4;
					// put together the checksum from the license key
					chktemp1 = "";
					for (j = 0; j < toklen; j++)
					{
						tokJunk = lickey[tokenCnt + j];
						chktemp1 = chktemp1 + tokJunk.ToString();
					}
					// get the calculated checksum
					chktemp2 = GetChecksumNumber(chkLicString, (toklen*4), false);
					// now see if they are equal
					if ( chktemp1 != chktemp2 ) 
					{
						answer = false;
					}
					break;
				}
				// add to the checksum string. 
				chkTemString = chkTemString + tokTem.ToString();
				if ( (remain == 0) ) 
				{
					chkLicString = chkLicString + tokLic.ToString();
				}
			}
			return(answer);
		}
		

		/// <summary>
		/// Check the legal input for byte templates.
		/// </summary>
		/// <param name="lictemp">The license Template.</param>
		/// <param name="lickey">The license Key.</param>
		/// <returns></returns>
		bool CheckInputLegalByte(string lictemp, string lickey)
		{
			bool	answer = true;
			int		slen;
			int		i;
			int		j;
			char	tokTem;
			char	tokLic;
			char	tokJunk;
			string	chkLicString;
			string	chkTemString;
			string	chktemp1;
			string	chktemp2;

			// initialize the string
			chkLicString = "";
			chkTemString = "";

			// determine the length of the template
			slen = lictemp.Length;

			// now go through the license template to see what tokens are found. 
			// fill in the license key string now that we know everything will fit. 
			for (i = 0; i < slen; i++)
			{
				tokTem = lictemp[i];
				tokLic = lickey[i];
				if ( tokTem == 'c' ) 
				{
					int	toklen = GetChecksumToklength(lictemp);
					// put together the checksum from the license key
					chktemp1 = "";
					for (j = 0; j < toklen; j++)
					{
						tokJunk = lickey[i + j];
						chktemp1 = chktemp1 + tokJunk.ToString();
					}
					// get the calculated checksum
					chktemp2 = GetChecksumNumber(chkLicString, toklen, false);
					// now see if they are equal
					if ( chktemp1 != chktemp2 ) 
					{
						answer = false;
					}
					break;
				}
				// add to the checksum string. 
				chkTemString = chkTemString + tokTem.ToString();
				chkLicString = chkLicString + tokLic.ToString();
			}
			return(answer);
		}

		/// <summary>
		/// Check the legal input for all templates.
		/// </summary>
		/// <param name="lictemp">The license Template.</param>
		/// <param name="lickey">The license Key.</param>
		/// <returns></returns>
		bool CheckInputLegal(string lictemp, string lickey)
		{
			bool	answer;
			if ( flagBytes ) 
			{
				answer = CheckInputLegalByte(lictemp, lickey);
			}
			else 
			{
				answer = CheckInputLegalBit(lictemp, lickey);
			}
			return(answer);
		}

		/// <summary>
		/// Disassemble the Key for the Bytes mode.
		/// </summary>
		/// <param name="token">What token to search and dissemble for.</param>
		/// <returns>The string with the result.</returns>
		string DisassembleKeyBytes(string token)
		{
			string	localLicenseTemplate;	// the template that will be used to create the license key
			string	localLicensekey;		// the final license key that was made. 
			string	result;
			string	LicTokenvalue="";
			string	TemTokenvalue="";
			char	tokTem;
			char	tokLic;
			int		slen;
			int		i;
			bool	hasChecksum;
			bool	answer;


			// use local variable so we can strip the dashes
			localLicenseTemplate = this.licenseTemplate;
			localLicensekey = this.strLicensekey;

			// clean the input of the dashes
			localLicenseTemplate = CleanInput(localLicenseTemplate);
			localLicensekey = CleanInput(localLicensekey);

			// determine the length of the template
			slen = localLicenseTemplate.Length;

			// check to see if any checksum
			hasChecksum = MatchInput(localLicenseTemplate);
			if ( hasChecksum ) 
			{
				answer = CheckInputLegal(localLicenseTemplate, localLicensekey);
				if ( !answer ) 
				{
					// we could not find the token so this is illegal
					throw new ApplicationException("License key Checksum does not match");
				}
			}

			// now go through the license template to see what tokens are found. 
			// fill in the license key string now that we know everything will fit. 
			for (i = 0; i < slen; i++)
			{
				tokTem = localLicenseTemplate[i];
				tokLic = localLicensekey[i];
				if ( tokTem == token[0] ) 
				{
					LicTokenvalue = LicTokenvalue + tokLic.ToString();
					TemTokenvalue = TemTokenvalue + tokTem.ToString();
				}
			}
			result = LicTokenvalue;
			return(result);
		}



		/// <summary>
		/// Disassemble the Key for the Bits mode.
		/// </summary>
		/// <param name="token">What token to search and dissemble for.</param>
		/// <returns>The string with the result.</returns>
		string DisassembleKeyBits(string token)
		{
			string	localLicenseTemplate;	// the template that will be used to create the license key
			string	localLicensekey;		// the final license key that was made. 
			string	finalString;
			string	result;
			char	tokTem;
			char	tokLic;
			int		itokLic;
			char	tokLicOut;
			int		itokLicOut;
			int		sLenTem;
			int		i;
			int		j;
			int		modvalue;
			bool	hasChecksum;

			// use local variable so we can strip the dashes
			localLicenseTemplate = this.licenseTemplate;
			localLicensekey = this.strLicensekey;

			// clean the input of the dashes
			localLicenseTemplate = CleanInput(localLicenseTemplate);
			localLicensekey = CleanInput(localLicensekey);

			// Determine the length of the template
			sLenTem = localLicenseTemplate.Length;

			// check to see if any checksum
			hasChecksum = MatchInput(localLicenseTemplate );
			if ( hasChecksum ) 
			{
				CheckInputLegal(localLicenseTemplate, localLicensekey);
			}

			// Now go through the license template to see what tokens are found. 
			// fill in the license key string now that we know everything will fit. 
			// remember that for this version the tokens must start and end on a
			// byte boundary anyways. 
			tokLic = '\x0';		// Hexadecimal
			tokLicOut = '\x0';	// Hexadecimal
			finalString = "";
			for (i = 0, j = 0; i < sLenTem; i++)
			{
				tokTem = localLicenseTemplate[i];
				tokLic = localLicensekey[j];
				// if we are on a byte boundary then move to the next byte
				modvalue = i % 4;
				if ( tokTem == token[0] ) 
				{
					result = tokLic.ToString();
					if (this.UseBase10) 
					{
						itokLic = int.Parse(result);
					}
					else 
					{
						itokLic = int.Parse(result,System.Globalization.NumberStyles.AllowHexSpecifier);
					}

					
					switch(modvalue) 
					{
						case 0:
							itokLicOut = tokLicOut;
							itokLicOut = (itokLicOut << 1);  // move it over one
							if ( (itokLic & 0x8) != 0 ) 
							{
								itokLicOut = itokLicOut | 0x1;	// Set it if it is set
							}
							tokLicOut = Convert.ToChar(itokLicOut);
							break;
						case 1:
							itokLicOut = tokLicOut;
							itokLicOut = itokLicOut << 1;  // move it over one
							if ( (itokLic & 0x4 ) != 0 ) 
							{
								itokLicOut = itokLicOut | 0x1;
							}
							tokLicOut = Convert.ToChar(itokLicOut);
							break;
						case 2:
							itokLicOut = tokLicOut;
							itokLicOut = itokLicOut << 1;  // move it over one
							if ( (itokLic & 0x2 ) != 0 ) 
							{
								itokLicOut = itokLicOut | 0x1;
							}
							tokLicOut = Convert.ToChar(itokLicOut);
							break;
						case 3:
							itokLicOut = tokLicOut;
							itokLicOut = itokLicOut << 1;  // move it over one
							if ( (itokLic & 0x1 ) != 0)
							{
								itokLicOut = itokLicOut | 0x1;
							}
							tokLicOut = Convert.ToChar(itokLicOut);
							break;
					}					
				}
				if ( modvalue == 3 ) 
				{
					j++;	// go to the next charater
					if ( tokTem == token[0] ) 
					{
						itokLicOut = tokLicOut;
						if (this.UseBase10) 
						{
							finalString = finalString + itokLicOut.ToString("D");
						}
						else 
						{
							finalString = finalString + itokLicOut.ToString("X");
						}
					}
					tokLicOut = '\x0';	// Hexadecimal
				}
			}
			result = finalString;
			return(result);
		}

		/// <summary>
		/// Dissassemble the license key based on the template.
		/// </summary>
		/// <param name="token">The token that you want disassembled.</param>
		/// <example>
		/// <code>
		/// result = gkey.DisassembleKey("p"); 
		/// </code>
		/// </example>
		/// <exception cref="System.ApplicationException">Thrown when the Licensekey Is Empty</exception>
		/// <exception cref="System.ApplicationException">Thrown when the Licensekey Template Is Empty</exception>
		/// <returns>The token value represented as a string.</returns>
		public string DisassembleKey(string token)
		{
			string	result;
			int		slen;
			//
			// If the final License key is empty then error.  
			//
			if ( strLicensekey.Length == 0 ) 
			{
				// we could not find the token so this is illegal
				throw new ApplicationException("License key Is Empty");
			}
			//
			// If the final License key is empty then error.  
			//
			slen = licenseTemplate.Length;
			if ( slen == 0 ) 
			{
				// we could not find the token so this is illegal
				throw new ApplicationException("License key Template Is Empty");
			}
			// what type of license key was used? 
			if ( UseBytes ) 
			{
				result = DisassembleKeyBytes(token);	// disassemble the license key for bytes
			}
			else 
			{
				result = DisassembleKeyBits(token);		// disassemble the license key for bits
			}
			return(result);
		}
	}
}
