/*
 * Created by SharpDevelop.
 * User: JMorello
 * Date: 4/7/2017
 * Time: 2:54 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Data;
using System.Data.SqlClient;
using  DBConfig = TestDataProvider.DataAccessor.Configuration;

namespace TestDataProvider.DataAccessor
{
	/// <summary>
	/// Description of ServerConnection.
	/// </summary>
	public class ServerConnection
	{
		private string _constring;
		private string _name;
		//*--future design property
		private string _adaptertype;
		public ServerConnection(DBConfig.ServerElement config)
		{
			_name = config.Name;
			_adaptertype = config.AdapterType;
			_constring = ConnectionString(config);
		}
		
		public string ServerName
		{
			get{
				return _name;
			}
		}
		
		/// <summary>
		/// Adapter
		/// This method is currently only build for a SQL connection, but there are plans for a
		/// reflection based type return that will use an additional property value to find said type.
		/// </summary>
		/// <param name="sqlcommand"></param>
		/// <returns></returns>
		public SqlDataAdapter Adapter(string sqlcommand)
		{
			return new SqlDataAdapter(sqlcommand,_constring);
		}
		
		private string ConnectionString(DBConfig.ServerElement configvalue)
		{
			return "Data Source="+configvalue.Address+";" +
						"Initial Catalog="+configvalue.InitialCatalog+";" +
						"User id="+configvalue.Username+";" +
						"Password="+configvalue.Password+";";
		}
	}
}
