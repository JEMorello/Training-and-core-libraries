/*
 * Created by SharpDevelop.
 * User: JMorello
 * Date: 4/13/2017
 * Time: 2:17 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TestDataProvider.Cores
{
	/// <summary>
	/// Description of Contact.
	/// </summary>
	public abstract class Contact
	{
		public int ContactID;
		public bool ForShipping;
		public bool ForBilling;
		public bool ForAdvertising;
		
	}
	public class Address:Contact{
		public Address(){
			
		}
		public string DisplayName{get;set;}
		public string StreetAddress{get;set;}
		public string State{get;set;}
		public string Country{get;set;}
		public string PostalCode{get;set;}
	}
	public class eMail:Contact{
		public eMail(){
			
		}
		public string emailAddress{get;set;}
		
	}
	public class Phone:Contact{
		public Phone(){
			
		}
		public string Number{get;set;}
	}
}
