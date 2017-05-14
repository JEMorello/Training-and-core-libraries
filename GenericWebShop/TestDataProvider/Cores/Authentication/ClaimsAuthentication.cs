/*
 * Created by SharpDevelop.
 * User: JMorello
 * Date: 4/6/2017
 * Time: 10:55 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Web;
using System.Text;
using System.Threading;
using System.Collections;
using System.IO;
using System.Security.Principal;
using System.Runtime.Serialization.Formatters.Binary;
using System.Configuration;

namespace TestDataProvider.Cores.Authentication
{
	/// <summary>
	/// Description of Authentication.
	/// </summary>
	public class ClaimsAuthentication : IHTTPModule
	{
		public ClaimsAuthentication()
		{
		}
		public bool Login(string Username, string Password)
		{
			
		}
		public string AuthToken(string Username, DateTime LoginTime)
		{
			
		}
		public AuthUser ActiveSession()
		{
			
		}
		HttpApplication app = null;
		const string LOGINURL_KEY				= "ClaimsAuth.LoginUrl";//change
		const string AUTHENTICATION_COOKIE_KEY	= "ClaimsAuth.Cookie.Name";//change

		const string AUTHENTICATION_COOKIE_EXPIRATION_KEY	= "ClaimsAuth.Cookie.Timeout";//change
		
		/// <summary>
		/// Initializes the module derived from IHttpModule when called by the HttpRuntime . 
		/// </summary>
		/// <param name="httpapp">The HttpApplication module</param>
		public void Init(HttpApplication httpapp)
		{
			this.app = httpapp;
			app.AuthenticateRequest += new EventHandler(this.OnAuthenticate);
		}

		void OnAuthenticate(object sender, EventArgs e)
		{
			app = (HttpApplication)sender;
			HttpRequest req = app.Request;
			HttpResponse res = app.Response;

			string loginUrl = ConfigurationSettings.AppSettings[LOGINURL_KEY];
			if(loginUrl == null || loginUrl.Trim() == String.Empty)
			{
				throw new Exception(" CustomAuthentication.LoginUrl entry not found in appSettings section of Web.config");
			}

			string cookieName = ConfigurationSettings.AppSettings[AUTHENTICATION_COOKIE_KEY];
			if(cookieName == null || cookieName.Trim() == String.Empty)
			{
				throw new Exception(" CustomAuthentication.Cookie.Name entry not found in appSettings section section of Web.config");
			}

			int i = req.Path.LastIndexOf("/");
			string page = req.Path.Substring(i+1, (req.Path.Length - (i + 1)));

			int j = loginUrl.LastIndexOf("/");
			string loginPage = loginUrl.Substring(j+1, (loginUrl.Length - (j + 1)));

			if(page != null && !(page.Trim().ToUpper().Equals(loginPage.ToUpper())))
			{
				if(req.Cookies.Count > 0 && req.Cookies[cookieName.ToUpper()] != null)
				{
					HttpCookie cookie = req.Cookies[cookieName.ToUpper()];
					if(cookie != null)
					{
						string str = cookie.Value;
						byte[] byteArray = Encoding.UTF8.GetBytes(Encryptor.Decrypt(str));
						AuthUser userIdentity = (AuthUser)BinaryFormatter.Deserialize(new MemoryStream(byteArray));

						//test the login
						userIdentity.MyAuth(Login(userIdentity.Username, userIdentity.Password));
						if(userIdentity.IsAuthenticated)
						{
							app.Context.User = (IPrincipal)userIdentity;
							Thread.CurrentPrincipal = (IPrincipal)userIdentity;
						}else
						{
							res.Redirect(req.ApplicationPath + loginUrl + "?ReturnUrl=" + req.Path, true);
						}
					}
				}
				else
				{
					res.Redirect(req.ApplicationPath + loginUrl + "?ReturnUrl=" + req.Path, true);
				}
			}
		}

		/// <summary>
		/// Redirects an authenticated user back to the originally requested URL.
		/// </summary>
		/// <param name="identity">CustomIdentity of an authenticated user</param>
		public static void RedirectFromLoginPage(AuthUser identity)
		{
			string cookieName = ConfigurationManager.AppSettings[AUTHENTICATION_COOKIE_KEY];
			if(cookieName == null || cookieName.Trim() == String.Empty)
			{
				throw new Exception(" CustomAuthentication.Cookie.Name entry not found in appSettings section section of Web.config");
			}

			string cookieExpr = ConfigurationManager.AppSettings[AUTHENTICATION_COOKIE_EXPIRATION_KEY];

			HttpRequest request = HttpContext.Current.Request;
			HttpResponse response = HttpContext.Current.Response;
			MemoryStream values = new MemoryStream();
			BinaryFormatter(values, identity);
			StreamReader reader = new StreamReader(values);
			string encryptedUserDetails = Encryptor.Encrypt(reader.ReadToEnd());

			HttpCookie userCookie = new HttpCookie(cookieName.ToUpper(), encryptedUserDetails);
			if(cookieExpr != null && cookieExpr.Trim() != String.Empty)
			{
				userCookie.Expires = DateTime.Now.AddMinutes(int.Parse(cookieExpr));
			}
			response.Cookies.Add(userCookie);

			string returnUrl = request["ReturnUrl"];
			if(returnUrl != null && returnUrl.Trim() != String.Empty)
			{
				response.Redirect(returnUrl, false);
			}
			else
			{
				response.Redirect(request.ApplicationPath + "/default.aspx", false);
			}
		}
		public void Dispose()
		{
		}
	}
}
