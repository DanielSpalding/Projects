using System.Data;

using Util;

namespace DL.Data
{
    public interface IDBManager
    {
        DataProvider ProviderType
        {
          get;
          set;
        }
        string ConnectionString
        {
          get;
          set;
        }
        IDbConnection Connection
        {
          get;
        }
        IDbTransaction Transaction
        {
          get;
        }     
        IDataReader DataReader
        {
          get;
        }
        IDbCommand Command
        {
          get;
        }   
        IDbDataParameter[]Parameters
        {
          get;
        }
     
        void Open();
        void BeginTransaction();
        void CommitTransaction();
        void RollbackTransaction();
        void CreateParameters(int paramsCount);
        void AddParameters(int index, string paramName, object objValue);
        void AddParameters(int index, string paramName, object objValue, bool direction);
        IDataReader ExecuteReader(CommandType commandType, string commandText);
        DataSet ExecuteDataSet(CommandType commandType, string commandText);
        object ExecuteScalar(CommandType commandType, string commandText);
        int ExecuteNonQuery(CommandType commandType,string commandText);
        void CloseReader();
        void Close();
        void Dispose();
      }
}
