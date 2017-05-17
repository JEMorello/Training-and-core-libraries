/*
 * Created by SharpDevelop.
 * User: JMorello
 * Date: 4/3/2017
 * Time: 1:21 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace GenericWeebShop.GenericBackend
{
	[ServiceContract]
	public interface IService
	{
	   [OperationContract]
	   [WebGet(UriTemplate = "operation/{name}")]
	   string MyOperation(string name);
	}
	
	public class Service : IService
	{
	   public string MyOperation(string name)
	   {
		  // implement the operation
		  return string.Format("Operation name: {0}", name);
	   }
	} 
}
