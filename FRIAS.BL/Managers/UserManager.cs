using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using System.Reflection;

using FRIAS.Common.Entity;
using DL;
using DL.Data;
using Util;


namespace FRIAS.BL.Managers
{
    public class UserManager : TotalManager, IFactoryManager
    {
        private User _user;
        ArrayList _accesslist;
        private IDBManager _dbmgr;
        private object oldObject;
        private string[] _fieldlist;

        #region public properties
        public string[] FieldList
        {
            get { return _fieldlist; }
            set { _fieldlist = value; }
        }
        public object OldObject
        {
            get { return oldObject; }
            set { oldObject = value; }
        }
        #endregion

        #region public methods

        // fetches user information
        public object Fetch(object obj)
        {
            string type;

            string objtype = obj.GetType().ToString();
            if (objtype == "System.String")
                type = "userlist";
            else
                type = "user";

            _user = new User();
            _dbmgr = new DBManager(DBHelper.getProvider(Config.getValue("DBStr", "")));
            _dbmgr.ConnectionString = Config.getValue("DBStr", "");

            try
            {
                _dbmgr.Open();

                switch (type)
                {
                    case "userlist":
                        FetchAccessList(_dbmgr);
                        return _accesslist; 
                    default:
                        FetchUser(obj, _dbmgr);
                        //if (_user.initial != null)
                        //    FetchPlantInfo(_user, _dbmgr);
                        return _user;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dbmgr.Dispose();
            }
        }

        public DataSet FetchDataSet(string criteria)
        {
            DataSet ds = new DataSet();
            return ds;
        }

        public ArrayList FetchComboList(string initMsg, string param)
        {
            string qryString = "SELECT PLANT + ': ' + PLANT_DESC AS PLANT_DESC, CONNSTRING FROM PLANTLIST";
            ArrayList list = new ArrayList();
            ListItem listItem = new ListItem();

            _dbmgr = new DBManager(DBHelper.getProvider(Config.getValue("DBStr", "")));
            _dbmgr.ConnectionString = Config.getValue("DBStr", "");
            try
            {
                // initialize
                listItem.Text = initMsg;
                listItem.Value = "0";
                list.Add(listItem);

                _dbmgr.Open();
                _dbmgr.ExecuteReader(CommandType.Text, qryString);

                while (_dbmgr.DataReader.Read() == true)
                {
                    listItem = new ListItem();
                    listItem.Text = (string)_dbmgr.DataReader["PLANT_DESC"];
                    listItem.Value = (string)_dbmgr.DataReader["CONNSTRING"];
                    list.Add(listItem);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dbmgr.Dispose();
            }
            return list;
        }

        public void Delete(object obj)
        {
            // create database connection
            _dbmgr = new DBManager(DBHelper.getProvider(Config.getValue("DBStr", "")));
            _dbmgr.ConnectionString = Config.getValue("DBStr", "");

            DeleteUser(obj, _dbmgr);
        }

        public void Update(object obj)
        {
            // create database connection
            _dbmgr = new DBManager(DBHelper.getProvider(Config.getValue("DBStr", "")));
            _dbmgr.ConnectionString = Config.getValue("DBStr", "");

            UpdateUser(obj, _dbmgr);
        }

        #endregion

        // fetches user information
        private void FetchUser(object obj, IDBManager dbmgr)
        {
            string qryLocal = "SELECT * FROM USERLIST WHERE NAME=@name";
            _user = (User)obj;

            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@name", _user.name);
            dbmgr.ExecuteReader(CommandType.Text, qryLocal);

            if (dbmgr.DataReader.Read())
            {
                // get properties of object and fetch object
                PropertyInfo[] p = _user.GetType().GetProperties();
                _user = (User)FetchObject(_user, p, dbmgr);
            }
            else
            {
                _user.initial = "";
                _user.password = null;
            }

            dbmgr.CloseReader();
        }

        // fetches user access list
        private void FetchAccessList(IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM USERLIST ORDER BY NAME";

            User item = new User();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new User();
                item = (User)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _accesslist = list;

        }

        private void DeleteUser(object obj, IDBManager dbmgr)
        {
            User item = (User)obj;

            try
            {
                dbmgr.Open();
                dbmgr.BeginTransaction();

                dbmgr.CreateParameters(1);
                dbmgr.AddParameters(0, "NAME", item.name);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.USERLIST_d");

                dbmgr.CommitTransaction();
            }
            catch (Exception ex)
            {
                dbmgr.RollbackTransaction();
                throw ex;
            }
            finally
            {
                dbmgr.Dispose();
            }
        }

        private void UpdateUser(object obj, IDBManager dbmgr)
        {
            User item = (User)obj;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(7);
                dbmgr.BeginTransaction();
                dbmgr.AddParameters(0, "@name", item.name);
                dbmgr.AddParameters(1, "@lname", item.lname);
                dbmgr.AddParameters(2, "@fname", item.fname);
                dbmgr.AddParameters(3, "@initial", item.initial);
                dbmgr.AddParameters(4, "@level", item.level);
                dbmgr.AddParameters(5, "@password", item.password);
                dbmgr.AddParameters(6, "@plant", item.plant);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.USERLIST_u");

                dbmgr.CommitTransaction();
            }
            catch (Exception ex)
            {
                dbmgr.RollbackTransaction();
                throw ex;
            }
            finally
            {
                dbmgr.Dispose();
            }
        }

        //private void FetchPlantInfo(User _user)
        //{
        //    string qryString = "SELECT PLANT_DESC FROM PLANTLIST WHERE PLANT = @plant";

        //    _dbmgr.CreateParameters(1);
        //    _dbmgr.AddParameters(0, "@plant", _user.plant);
        //    _dbmgr.ExecuteReader(CommandType.Text, qryString);

        //    if (_dbmgr.DataReader.Read())
        //    {
        //        if (_dbmgr.DataReader.GetValue(0) != System.DBNull.Value)
        //            _user.plant_desc = _dbmgr.DataReader.GetString(0);
        //        else
        //            _user.plant_desc = "";
        //    }

        //    _dbmgr.CloseReader();
        //}
    }
}
