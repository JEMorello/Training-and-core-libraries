/*
 * Created by SharpDevelop.
 * User: JMorello
 * Date: 5/15/2017
 * Time: 10:19 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using GenericBackend.DataAccessor;

namespace TestDataProvider.Cores.BusinessLayer
{
	/// <summary>
	/// Description of DataFactory.
	/// </summary>
	public class DataFactory
	{
		private DataServer Primary;
		
		public DataFactory()
		{
			//connect to DB, thru conffig values
			Primary = new DataServer();//auto loads configs in its own constructor
			
		}
		
		public 
	}
}
