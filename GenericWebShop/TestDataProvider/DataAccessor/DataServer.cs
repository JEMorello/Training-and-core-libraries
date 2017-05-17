/*
 * Created by SharpDevelop.
 * User: JMorello
 * Date: 4/6/2017
 * Time: 11:16 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Configuration;
using  DBConfig = GenericBackend.DataAccessor.Configuration;


namespace GenericBackend.DataAccessor
{
	/// <summary>
	/// Description of DataServer.
	/// </summary>
	public class DataServer
	{
		public Dictionary<string, ServerConnection> Servers;
		private bool _ready;
		public DataServer()
		{
			//load config values
			DBConfig.ServerAccessRules Rules =  (DBConfig.ServerAccessRules)ConfigurationManager.GetSection("ServerAccessRules");
			//if no servers listed, no need to prebuild for connections
			if ((Rules != null) && (Rules.Servers.Count>0))
			{
			    foreach (DBConfig.ServerElement Element in Rules.Servers)
			    {
			    	
			    	Servers.Add(Element.Name,new ServerConnection(Element));
			    	
			    }
			    if(Servers.Count>0)
			    {
			    	_ready =true;
			    }
			}
			
		}
		public bool state
		{
			get{
				return _ready;
			}
		}
	}
}
