using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace Util
{
	/// <summary>
	/// Basic Encrption/decryption functionaility
	/// </summary>
	public sealed class Crypto
	{
		#region Public Enums
		/// <summary>
		/// Types of symmetric encyption
		/// </summary>
		public enum CryptoType
		{
			// DES and RC2 have been removed due to reported compromises
			//DES = 0,
			//RC2,
			/// <summary>Rihndael/AES encyrption</summary>
			Rijndael,
			/// <summary>Triple DES encryption</summary>
			TripleDES
		}
		#endregion

		#region Encrypt/Decrypt
		/// <summary>
		/// Encrypt a text string
		/// </summary>
		/// <param name="cType">Type of encryption</param>
		/// <param name="key">Key aka password</param>
		/// <param name="inputText">Text to encrypt</param>
		/// <returns>Encrypted text</returns>
		public static string Encrypt(CryptoType cType, string key, string inputText)
		{	
			//declare a new encoder
			UTF8Encoding UTF8Encoder = new UTF8Encoding();
			//get byte representation of string
			byte[] inputBytes = UTF8Encoder.GetBytes(inputText);

			//convert back to a string
			return Convert.ToBase64String(Encrypt(cType, key, inputBytes));
		}

		/// <summary>
		/// Decrypt a text string
		/// </summary>
		/// <param name="cType">Type of encryption</param>
		/// <param name="key">Key aka password</param>
		/// <param name="inputText">Text to decrypt</param>
		/// <returns>Decrypted text</returns>
		public static string Decrypt(CryptoType cType, string key, string inputText)
		{	
			//declare a new encoder
			UTF8Encoding UTF8Encoder = new UTF8Encoding();
			//get byte representation of string
			byte[] inputBytes;
			try
			{
				inputBytes = Convert.FromBase64String(inputText);
			}
			catch(FormatException)
			{
				throw new ArgumentException("Input text is not Base64", "inputText");
			}

			//convert back to a string
			return UTF8Encoder.GetString(Decrypt(cType, key, inputBytes));
		}

		/// <summary>
		/// Encrypt a byte array
		/// </summary>
		/// <param name="cType">Type of encryption</param>
		/// <param name="key">Key aka password</param>
		/// <param name="input">Data to encrypt</param>
		/// <returns>Encrypted data</returns>
		public static byte[] Encrypt(CryptoType cType, string key, byte[] input)
		{
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(key, SALT);
			
			SymmetricAlgorithm algo = selectAlgorithm(cType);
			algo.Key = pdb.GetBytes(algo.KeySize / 8);
			algo.IV = pdb.GetBytes(algo.BlockSize / 8);

			return DoCrypt(algo.CreateEncryptor(), input);
		}

		/// <summary>
		/// Decrypt a byte array
		/// </summary>
		/// <param name="cType">Type of encryption</param>
		/// <param name="key">Key aka password</param>
		/// <param name="input">Data to encrypt</param>
		/// <returns>Decrypted data</returns>
		public static byte[] Decrypt(CryptoType cType, string key, byte[] input)
		{
			PasswordDeriveBytes pdb = new PasswordDeriveBytes(key, SALT);
			
			SymmetricAlgorithm algo = selectAlgorithm(cType);
			algo.Key = pdb.GetBytes(algo.KeySize / 8);
			algo.IV = pdb.GetBytes(algo.BlockSize / 8);

			return DoCrypt(algo.CreateDecryptor(), input);
		}		

		#endregion

		#region Symmetric Engine

		/// <summary>
		/// Perform the encryption/decryption
		/// </summary>
		/// <param name="transform">Direction</param>
		/// <param name="input">Data to work with</param>
		/// <returns>Cipher'ed data</returns>
		private static byte[] DoCrypt(ICryptoTransform transform, byte[] input)
		{
			//memory stream for output
			MemoryStream memStream = new MemoryStream();	

			try 
			{
				//setup the cryption - output written to memstream
				CryptoStream cryptStream = new CryptoStream(memStream,transform,CryptoStreamMode.Write);

				//write data to cryption engine
				cryptStream.Write(input, 0, input.Length);

				//we are finished
				cryptStream.FlushFinalBlock();
				
				//get result
				byte[] output = memStream.ToArray();

				//finished with engine, so close the stream
				cryptStream.Close();

				return output;
			}
			catch (Exception e) 
			{
				throw new Exception("Error in symmetric engine. Error : " + e.Message,e);
			}
		}

		/// <summary>
		///		returns the specific symmetric algorithm acc. to the cryptotype
		/// </summary>
		/// <returns>SymmetricAlgorithm</returns>
		private static SymmetricAlgorithm selectAlgorithm(CryptoType cType)
		{
			switch (cType)
			{
				/*
				case CryptoTypes.encTypeDES:
					SA = DES.Create();
					break;
				case CryptoTypes.encTypeRC2:
					SA = RC2.Create();
					break;
				*/
				case CryptoType.Rijndael:
					return Rijndael.Create();
				case CryptoType.TripleDES:
					return TripleDES.Create();
				// Default is Rijndael
				default:
					return Rijndael.Create();
			}
		}

		#endregion

		#region Fields
		private static readonly byte[] SALT = new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 };
		#endregion
	}

	/// <summary>
	/// Hash functionality
	/// </summary>
	public class Hash
	{
		#region Public Enums
		/// <summary>
		/// Types of hashing available
		/// </summary>
		public enum HashType
		{
			/// <summary>Secure Hash Algorithm - 256bit</summary>
			SHA256, 
			/// <summary>Secure Hash Algorithm - 384bit</summary>
			SHA384, 
			/// <summary>Secure Hash Algorithm - 512bit</summary>
			SHA512
			// SHA and MD5 have been removed due to reported compromises
		}
		#endregion

		#region Statics

		/// <summary>
		/// Gets the hash.
		/// </summary>
		/// <param name="inputText">Input text.</param>
		/// <param name="hashType">Hash type.</param>
		/// <returns>Hash of inputText</returns>
		public static string GetHash(String inputText, HashType hashType)
		{
			return ComputeHash(inputText,hashType);
		}

		/// <summary>
		/// returns true if the input text is equal to hashed text
		/// </summary>
		/// <param name="inputText">unhashed text to test</param>
		/// <param name="hashText">already hashed text</param>
		/// <param name="hashType">Type of hash to use</param>
		/// <returns>boolean true or false</returns>
		public static bool isHashEqual(string inputText, string hashText, HashType hashType)
		{
			return (GetHash(inputText, hashType) == hashText);
		}

		#endregion

		#region Engine

		/// <summary>
		/// Computes the hash code and converts it to string
		/// </summary>
		/// <param name="inputText">input text to be hashed</param>
		/// <param name="hashType">type of hashing to use</param>
		/// <returns>hashed string</returns>
		private static string ComputeHash(string inputText, HashType hashType)
		{
			HashAlgorithm HA = getHashAlgorithm(hashType);

			//declare a new encoder
			UTF8Encoding UTF8Encoder = new UTF8Encoding();
			//get byte representation of input text
			byte[] inputBytes = UTF8Encoder.GetBytes(inputText);
			
			
			//hash the input byte array
			byte[] output = HA.ComputeHash(inputBytes);

			//convert output byte array to a string
			return Convert.ToBase64String(output);
		}

		/// <summary>
		/// Retrieve the specific hashing alorithm
		/// </summary>
		/// <param name="hashType">Type of hash to use</param>
		/// <returns>.NET HashAlgorithm</returns>
		private static HashAlgorithm getHashAlgorithm(HashType hashType)
		{
			switch (hashType)
			{
				/*
				case HashType.MD5 :
					return new MD5CryptoServiceProvider();
				case HashType.SHA :
					return new SHA1CryptoServiceProvider();
				*/
				case HashType.SHA256 :
					return new SHA256Managed();
				case HashType.SHA384 :
					return new SHA384Managed();
				case HashType.SHA512 :
					return new SHA512Managed();
				default :
					return new SHA256Managed();
			}
		}

		#endregion

	}
}


