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
    public class NPOManager : TotalManager, IFactoryManager
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
                    case "subcomp":
                        FetchSubcompList(id, _dbmgr);
                        return _comp.subcomplist;
                    case "interlock":
                        FetchIncomingIntlkList(id, _dbmgr);
                        return _comp.incomingintlklist;
                    case "cable":
                        FetchCableList(id, _dbmgr);
                        return _comp.cablelist;
                    case "ksf":
                        FetchKSFList(id, _dbmgr);
                        return _comp.ksflist;
                    default:
                        FetchComponent(id, _dbmgr);
                        FetchSubcompList(id, _dbmgr);
                        FetchIncomingIntlkList(id, _dbmgr);
                        FetchCableList(id, _dbmgr);
                        FetchKSFList(id, _dbmgr);
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
            string qryString = "SELECT * FROM viewCOMPLIST_NPO ORDER BY COMP";
        
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
           
            DeleteKSF(obj, _dbmgr);

        }

        // updates/adds given object or object item
        public void Update(object obj)
        {
            // create database connection
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;
            
            UpdateKSF(obj, _dbmgr);

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

        #region subcomponent

        // procedure fetches subcomponent list
        private void FetchSubcompList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewSUBCOMPS s WHERE s.COMP_ID = @comp_id ORDER BY s.SUBCOMP";

            Subcomp item = new Subcomp();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@comp_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new Subcomp();											            // create new item
                item = (Subcomp)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _comp.subcomplist = list;												        // update component item list

        }

        #endregion

        #region interlock
        
        // procedure fetches incoming interlocks
        private void FetchIncomingIntlkList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCOMPINTLKS ci WHERE ci.COMP_ID = @comp_id ORDER BY ci.INTLK, ci.DEVICE, ci.CONTACTS";
            
            CompIntlk item = new CompIntlk();
            ArrayList list = new ArrayList();
            
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@comp_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new CompIntlk();											            // create new item
                item = (CompIntlk)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _comp.incomingintlklist = list;												    // update component item list

        }

        #endregion

        #region cable

        // procedure fetches cable list
        private void FetchCableList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCOMPCAB cc WHERE cc.COMP_ID = @comp_id ORDER BY cc.CABLE";
            
            CompCab item = new CompCab();
            ArrayList list = new ArrayList();
            
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create required parameters
            dbmgr.AddParameters(0, "@comp_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new CompCab();											            // create new item
                item = (CompCab)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to ArrayList
            }

            dbmgr.CloseReader();
            _comp.cablelist = list;												            // update object item list

        }

        #endregion

        #region ksf

        // procedure fetches key safety function list
        private void FetchKSFList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCOMPKSF AS ck WHERE ck.COMP_ID = @comp_id ORDER BY ck.KSF";

            CompKSF item = new CompKSF();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create required parameters
            dbmgr.AddParameters(0, "@comp_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new CompKSF();											            // create new item
                item = (CompKSF)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _comp.ksflist = list;												            // update object item list

        }

        // deletes key safety function
        private void DeleteKSF(object obj, IDBManager dbmgr)
        {
            CompKSF item = (CompKSF)obj;

            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(2);												    // create required paramaters
                dbmgr.BeginTransaction();
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@ksf_id", item.ksf_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPKSF_d");	    // execute stored procedure
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

        // updates key safety function
        private void UpdateKSF(object obj, IDBManager dbmgr)
        {
            CompKSF item = (CompKSF)obj;

            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(4);												    // create required paramaters
                dbmgr.BeginTransaction();
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@ksf_id", item.ksf_id);
                dbmgr.AddParameters(2, "@mode_id", item.mode_id);
                dbmgr.AddParameters(3, "@note", item.note);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPKSF_u");	    // execute stored procedure
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
