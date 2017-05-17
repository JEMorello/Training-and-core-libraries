/*
 * Created by SharpDevelop.
 * User: JMorello
 * Date: 4/6/2017
 * Time: 10:56 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using GenericBackend.Cores.Authentication;

namespace GenericBackend.Cores
{
	/// <summary>
	/// Description of Customer.
	/// </summary>
	/// 
	[Serializable]
	public class Customer:AuthUser
	{
		public Customer(string LoginInfo)
		{
			//parse string
			
		}
		public AgeQualities Age{get; set;}
		public Membership MemberStatus{	get; set;}
		public List<Contact> Contacts;
		public object OrderHistory;
		public object ShoppingCart;
		public object ShippingHistory;
		private bool _isLoggedIn;
		public bool LoggedIn{get{return _isLoggedIn;}}
		
	}
	
	public enum AgeQualities
	{
		PastMinor,
		FullAdult
	}
	public enum Membership
	{
		Normal,
		Special,
		VIP
	}
}
