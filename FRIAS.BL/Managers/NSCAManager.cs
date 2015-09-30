using System;
using System.Data;
using System.Collections;
using System.Web.UI.WebControls;
using System.Reflection;

using FRIAS.Common;
using FRIAS.Common.Entity;
using DL.Data;

namespace FRIAS.BL.Managers
{
    // class handles transactions pertaining to Component Entity
    public class NSCAManager : TotalManager, IFactoryManager
    {
        private User _user;
        private IDBManager _dbmgr;												            // database manager
        private Component _comp;														    // instance of primary object
        
        private object _oldobj;                                                             // old object, used for tracking history
        private string[] _fieldlist;                                                        // list of fields whose control appears in web form

        #region public properties
        public string[] FieldList
        {
            get { return _fieldlist; }
            set { _fieldlist = value; }
        }
        public object OldObject
        {
            get { return _oldobj; }
            set { _oldobj = value; }
        }
        #endregion

        #region public methods

        // fetches object and object items
        // user has option of passing object itself OR a string with id and type delimited by '|'
        public object Fetch(object obj)
        {
            int id;
            string type;
            string objtype = obj.GetType().ToString();

            if (objtype == "System.String")
            {
                string[] strTemp = ((string)obj).Split(new char[] { '|' });
                id = Convert.ToInt32(strTemp[0]);
                type = strTemp[1];
            }
            else
            {
                id = ((Component)obj).comp_id;
                type = "all";
            }

            // create new object and database connection
            _comp = new Component();
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            try
            {
                _dbmgr.Open();

                // determine type
                switch (type)
                {
                    case "ra":
                        FetchRAList(id, _dbmgr);
                        return _comp.ralist;
                    default:
                        FetchComponent(id, _dbmgr);
                        FetchFACompDispList(id, _dbmgr);
                        FetchRAList(id, _dbmgr);
                        return _comp;
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

        // fetches a dataset
        public DataSet FetchDataSet(string criteria)
        {
            DataSet ds = new DataSet();
            string qryString = "SELECT * FROM viewCOMPLIST_NSCA ORDER BY COMP";
        
            // create database connection
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            try
            {
                _dbmgr.Open();
                ds = (DataSet)_dbmgr.ExecuteDataSet(CommandType.Text, qryString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dbmgr.Dispose();
            }
            return ds;

        }

        // NOTE: not used. autocompletes are used instead of combolist due to large amount of data
        public ArrayList FetchComboList(string initMsg, string param)
        {
            ArrayList list = new ArrayList();
            ListItem listItem = new ListItem();
            return list;
        }

        // deletes given object or object item
        public void Delete(object obj)
        {
            // create database connection
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;
           
            DeleteRA(obj, _dbmgr);

        }

        // updates/adds given object or object item
        public void Update(object obj)
        {
            // create database connection
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;
            
            UpdateRA(obj, _dbmgr);

        }

        #endregion

        #region component

        private void FetchComponent(int id, IDBManager dbmgr)
        {
            string qryLocal = "SELECT * FROM viewCOMPLIST WHERE COMP_ID=@id";

            dbmgr.CreateParameters(1);                                                      // create required parameters
            dbmgr.AddParameters(0, "@id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryLocal);                                // execute reader
            if (dbmgr.DataReader.Read())
            {
                // get properties of object and fetch object
                PropertyInfo[] p = _comp.GetType().GetProperties();
                _comp = (Component)FetchObject(_comp, p, dbmgr);
            }

            dbmgr.CloseReader();

        }

        #endregion

        #region facompdisp

        private void FetchFACompDispList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFACOMPDISP WHERE COMP_ID = @comp_id ORDER BY FA, DISP";
            FACompDisp item = new FACompDisp();											    // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@comp_id", id);							                // fire area id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new FACompDisp();
                item = (FACompDisp)FetchObject(item, p, dbmgr);
                item.in_fa = Convert.ToBoolean(dbmgr.DataReader["in_fa"]);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _comp.facompdisplist = list;											        // update item list
        }

        #endregion

        #region recovery action

        // procedure fetches recovery action list
        private void FetchRAList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFACOMPRA AS r WHERE r.COMP_ID = @comp_id ORDER BY r.FA, r.RA_DISP";

            CompRA item = new CompRA();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create required parameters
            dbmgr.AddParameters(0, "@comp_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new CompRA();											            // create new item
                item = (CompRA)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _comp.ralist = list;												            // update object item list

        }

        // deletes recovery action
        private void DeleteRA(object obj, IDBManager dbmgr)
        {
            CompRA item = (CompRA)obj;

            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(2);												    // create required paramaters
                dbmgr.BeginTransaction();
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@fa_id", item.fa_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FACOMPRA_d");	    // execute stored procedure
                dbmgr.CommitTransaction();
            }
            catch (Exception ex)
            {
                // if there is problem with transaction roll back to original
                dbmgr.RollbackTransaction();
                throw ex;
            }
            finally
            {
                dbmgr.Dispose();
            }
        }

        // updates recovery action
        private void UpdateRA(object obj, IDBManager dbmgr)
        {
            CompRA item = (CompRA)obj;

            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(6);												    // create required paramaters
                dbmgr.BeginTransaction();
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@fa_id", item.fa_id);
                dbmgr.AddParameters(2, "@ra_disp", item.ra_disp);
                dbmgr.AddParameters(3, "@bin", item.bin);
                dbmgr.AddParameters(4, "@ra_type", item.ra_type);
                dbmgr.AddParameters(5, "@ra_feasible", item.ra_feasible);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FACOMPRA_u");	    // execute stored procedure
                dbmgr.CommitTransaction();
            }
            catch (Exception ex)
            {
                // if there is problem with transaction roll back to original
                dbmgr.RollbackTransaction();
                throw ex;
            }
            finally
            {
                dbmgr.Dispose();
            }
        }

        #endregion

    }
}
