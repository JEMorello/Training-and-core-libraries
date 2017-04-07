/*
 * Created by SharpDevelop.
 * User: JMorello
 * Date: 4/6/2017
 * Time: 11:31 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Configuration;
using System.xml;
using System.Collections;
using System.Text;

namespace TestDataProvider.DataAccessor.Configuration
{
	/// <summary>
	/// Description of ServerAccessRules.
	/// </summary>
	public class ServerAccessRules:ConfigurationSection
	{
		
        public const string SectionName = "ServerAccessRules";
 
        private const string ServerCollectionName = "Servers";
		[ConfigurationProperty(ServerAccessRules)]
        [ConfigurationCollection(typeof(ServerCollection), AddItemName = "add")]
        public ServerCollection Servers { get { return (ServerCollection)base[ServerCollectionName]; } }
		
//        [ConfigurationProperty("Server")]
//        public ServerElement DBServer
//        {
//            get
//            { 
//                return (ServerElement)this["Server"]; }
//            set
//            { this["Server"] = value; }
//        	
//        }
	}
	
    public class ServerCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServerElement();
        }
 
        protected override object GetElementKey(ServerElement element)
        {
            return ((ServerElement)element).Name;
        }
    }
    
	public class ServerElement : ConfigurationElement
	{
		[ConfigurationProperty("name", IsRequired = true)]
        public String Name
        {
        	  get
            {
                return (String)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }
        
		[ConfigurationProperty("address", IsRequired = true)]
        public String Address
        {
        	  get
            {
                return (String)this["address"];
            }
            set
            {
                this["address"] = value;
            }
        
        }
        
        [ConfigurationProperty("initCat", IsRequired = true)]
        public String InitialCatalog
        {
        	get{
        		return (String)this["initCat"];
        	}
        	set{
        		this["initCat"] = value;
        	}
        }
        [ConfigurationProperty("userName", IsRequired = true)]
        public String Username
        {
        	get{
        		return(String)this["userName"];
        	}
        	set{
        		this["userName"] = value;
        	}
        }
        [ConfigurationProperty("password", IsRequired = true)]
        public String Password
        {
        	get{
        		return(String)this["password"];
        	}
        	set{
        		this["password"] = value;
        	}
        }
        [ConfigurationProperty("adapter", IsRequired = true)]
        public String AdapterType
        {
        	get{
        		return(String)this["adapter"];
        	}
        	set{
        		this["adapter"] = value;
        	}
        }
	}
		
}
