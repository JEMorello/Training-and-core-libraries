/*
 * Created by SharpDevelop.
 * User: JMorello
 * Date: 4/20/2017
 * Time: 2:48 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;

namespace TestDataProvider.Cores.Authentication
{
	/// <summary>
	/// Description of Encryptor.
	/// </summary>
	public sealed class Encryptor
	{
		private static string key = "sfdjf48mdfdf3054" ;

		public Encryptor()
		{
		}

		public static string Encrypt(String plainText )
		{
			string encrypted = null;
			try
			{
				byte[] inputBytes = ASCIIEncoding.ASCII.GetBytes(plainText);
				byte[] pwdhash = null;
				MD5CryptoServiceProvider hashmd5;

				//generate an MD5 hash from the password. 
				//a hash is a one way encryption meaning once you generate
				//the hash, you cant derive the password back from it.
				hashmd5 = new MD5CryptoServiceProvider();
				pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
				hashmd5 = null;

				// Create a new TripleDES service provider 
				TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();
				tdesProvider.Key = pwdhash;
				tdesProvider.Mode = CipherMode.ECB;

				encrypted = Convert.ToBase64String(
					tdesProvider.CreateEncryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length));
			}
			catch(Exception e)
			{
				string str = e.Message;
				throw ;
			}
			return encrypted;
		}

		public static String Decrypt(string encryptedString)
		{
			string decyprted = null;
			byte[] inputBytes = null;

			try
			{
				inputBytes = Convert.FromBase64String(encryptedString);
				byte[] pwdhash = null;
				MD5CryptoServiceProvider hashmd5;

				//generate an MD5 hash from the password. 
				//a hash is a one way encryption meaning once you generate
				//the hash, you cant derive the password back from it.
				hashmd5 = new MD5CryptoServiceProvider();
				pwdhash = hashmd5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(key));
				hashmd5 = null;

				// Create a new TripleDES service provider 
				TripleDESCryptoServiceProvider tdesProvider = new TripleDESCryptoServiceProvider();
				tdesProvider.Key = pwdhash;
				tdesProvider.Mode = CipherMode.ECB;

				decyprted = ASCIIEncoding.ASCII.GetString(
					tdesProvider.CreateDecryptor().TransformFinalBlock(inputBytes, 0, inputBytes.Length));
			}
			catch(Exception e)
			{
				string str = e.Message;
				throw ;
			}
			return decyprted;
		}
		
		}
	}

