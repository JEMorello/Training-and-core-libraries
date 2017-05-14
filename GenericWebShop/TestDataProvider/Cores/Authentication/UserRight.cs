/*
 * Created by SharpDevelop.
 * User: JMorello
 * Date: 4/19/2017
 * Time: 4:07 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;

namespace TestDataProvider.Cores.Authentication
{
	/// <summary>
	/// generic for claims object for usage in collections and data driving user rights
	/// </summary>
	public class UserRight
	{
		private string _name;
		private int _ID;
		private string _Description;
		
		public string Name{get {return _name;}}
		public int ID{get {return _ID;}}
		public string Description{get{return _Description;}}
		
		public UserRight(DataRow value)
		{
			_name =(string)value["ClaimName"];
			_ID = (int)value["ClaimID"];
			_Description = (string)value["ClaimDescription"];
			
		}
	}
}
