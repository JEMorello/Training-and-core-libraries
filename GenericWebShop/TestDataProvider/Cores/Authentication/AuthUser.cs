/*
 * Created by SharpDevelop.
 * User: JMorello
 * Date: 4/6/2017
 * Time: 11:04 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Security.Principal;
using System.Collections.Generic;
using System.Linq;
//using System.Collections.ObjectModel;

namespace TestDataProvider.Cores.Authentication
{
	/// <summary>
	/// Description of AuthUser.
	/// </summary>
	/// 
	[Serializable]
	public abstract class AuthUser : IIdentity, IPrincipal
	{
		private int _UserId;
		private string _FirstName;
		private string _LastName;
		private string _Username;
		private string _Password;
		private bool _ready;
		private List<UserRight> _Rights;
		
		public string Name
		{get{
				return _FirstName + " " + _LastName;}
		}
		
		public string AuthenticationType{get{return "GenericClaims";}}
		
		[NonSerialized()]
		public IIdentity Identity	{
			get{return (IIdentity)this;}
		}
		
		[NonSerialized()]
		public bool IsInRole(string role)
		{
			return _Rights.Exists(uclaim=>uclaim.Name=role);
		}
		
		[NonSerialized()]
		public bool IsAuthenticated{get{
				return _ready;}
		}
		protected int UserID{get{return _UserId;}set{_UserId=value;}}
		protected string FirstName{get{return _FirstName;} set{_FirstName=value;}}
		protected string LastName{get{return _LastName;} set{_LastName =  value;}}
		protected string UserName{get{return _Username;} set{_Username = value;}}
		protected string Password{get{return _Password;} set{_Password=value;}}
		
		/// <summary>
		/// AuthUser Constructor
		/// Note this is designed to function from the Authentication system being created here, or the
		/// 		default admin user functions, allowing new user creation. This is not called 
		/// 		from any areas of the website directly, therefore maintaining security.
		/// 		Inheritors are similarly called from the Auth and Admin areas and the Admin area uses 
		/// 		Web Service to access this.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="isnew"></param>
		/// <param name="SqlAddendum"></param>
		protected AuthUser(bool isnew, DataRow dr)
		{
			if(isnew)
			{
				//creating a new user with new cerdentials, not for logging in
				
				
			}else{
				//for logging in
				
			}
		}
		public AuthUser()
		{
			
		}
		
		public bool MyAuth(bool loginsuccess)
		{
			_ready = loginsuccess;
		}
		
		
		[NonSerialized()]
		public List<UserRight> Rights {get{return _Rights;}set{_Rights=value;}}
		
	}
}
