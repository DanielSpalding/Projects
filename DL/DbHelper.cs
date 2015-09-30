using System;
using System.Text;
using System.Text.RegularExpressions;

using Util;

namespace DL
{
	public sealed class DBHelper
	{
		#region public methods
		/// <summary>
		/// Clean a connection string
		/// </summary>
		/// <param name="origConnString">Connection string to be cleaned</param>
		/// <param name="stripUnsupported">Strip unsupported tuples</param>
		/// <returns>Sparkly connection string</returns>
		public static string CleanConnectionString(string origConnString, bool stripUnsupported)
		{

			StringBuilder connectionString = new StringBuilder();

			try
			{
				// Sample connection string
				//@"Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;User ID=aauser;Initial Catalog=as_0014;Data Source=SCHMIDTNJ\TTE_DEV";
				
				// The connection_string could have more data than what we need.  Some of 
				// this extra data is known to cause connection issues, such as the PLSQLRSet=1.
				// To be safe, we are parsing the string to only contain the data that we know
				// we need
				string sDBSProvider = null;
			
				int index = origConnString.IndexOf(";");
				string comparison = origConnString.Substring(0, index);

				while (comparison.Length > 0)
				{
					switch(comparison.Substring(0, comparison.IndexOf("=")))
					{
						// Only add a few select items to the new connection string
						case "Provider":
						case "Server":
							sDBSProvider = comparison.Substring(comparison.IndexOf("=") + 1);
							break;
						case "Integrated Security":
						case "User ID":
						case "Initial Catalog":
						case "Data Source":
						case "Password":
                        case "Database":
						case "Persist Security Info":
							connectionString.Append(comparison);
							connectionString.Append(";");
							break;
						default:
							// Ignoring
							break;
					}

					// If the index from the previous loop is < 0, then we are done here.
					if(index < 0)
						break;
					
					origConnString = origConnString.Substring(index + 1);
					
					// Anything left after the ";"?  If not, this is the last item
					index = origConnString.IndexOf(";");
					if(index < 0)
					{
						comparison = origConnString;
					}
					else
					{
						comparison = origConnString.Substring(0, index);	
					}
				}

				if(!stripUnsupported)
				{
					// Get ready for the provider
					connectionString.Append(";Provider=");

					if(Regex.IsMatch(sDBSProvider, "SQLOLEDB"))
					{
						connectionString.Append("SQLOLEDB");
					}
				
					else if(Regex.IsMatch(sDBSProvider, "ORAOLEDB", RegexOptions.IgnoreCase))
					{
						connectionString.Append("ORAOLEDB.ORACLE.1");
					}
				
					else
					{
						throw new InvalidOperationException("Invalid database provider");
					}
				}
			}
			catch (Exception e)
			{
				throw new InvalidOperationException("Application access failed", e);
			}
	
			return connectionString.ToString();
		}

        public static DataProvider getProvider(string connString)
        {
			int index = connString.IndexOf(";");
		    string comparison = connString.Substring(0, index);

            while (comparison.Length > 0)
            {
                switch (comparison.Substring(0, comparison.IndexOf("=")))
                {
                    //Only add a few select items to the new connection string
                    case "Provider":
                        return DataProvider.OleDb;
                    case "Server":
                        return DataProvider.SqlServer;
                    case "Integrated Security":
                    case "User ID":
                    case "Password":
                    case "Initial Catalog":
                    case "Data Source":
                        return DataProvider.MySql;
                    case "Persist Security Info":
                    default:
                        return DataProvider.SqlServer;
                }
            }
            return DataProvider.SqlServer;

        }

		#endregion
	}
}
