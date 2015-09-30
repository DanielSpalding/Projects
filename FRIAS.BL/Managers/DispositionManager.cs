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
    public class DispositionManager : TotalManager, IFactoryManager
    {
        private User _user;
        private IDBManager _dbmgr;                                                          // database manager
        private Disposition _disp;                                                          // instance of primary object
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
        
        public object Fetch(object obj)
        {
            int id;
            string type;                                                                    //type of object passed
            string objtype = obj.GetType().ToString();                                      //get object type
            if (objtype == "System.String")
            {
                string[] strTemp = ((string)obj).Split(new char[] { '|' });                 //if the object type is a string then extract the id and type
                id = Convert.ToInt32(strTemp[0]);
                type = strTemp[1];
            }
            else
            {
                id = ((Disposition)obj).disp_id;
                type = "all";                                                               //object is a cable and get all information
            }
            
            _disp = new Disposition();                                                     //create new instance of object
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;
            
            switch (type)
            {
                case "facompcabdisp":
                    FetchFACompCabList(id, _dbmgr);
                    return _disp.facompcablist;
                case "all":
                    FetchDisposition(id, _dbmgr);
                    FetchFACompCabList(id, _dbmgr);
                    return _disp;
            }
            return _disp;
        }

        public DataSet FetchDataSet(string criteria)
        {
            DataSet ds = new DataSet();                                                     //create new dataset
            string qryString = "SELECT * FROM viewDISPLIST ORDER BY DISP";                  //create query string

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

        public ArrayList FetchComboList(string initMsg, string param)
        {
            ArrayList list = new ArrayList();                                               //create new array
            ListItem listItem = new ListItem();                                             //create new list item
            return list;
        }
        
        public void Delete(object obj)
        {
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;
            DeleteDisposition(obj, _dbmgr);
        }
        
        public void Update(object obj)
        {
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;
            UpdateDisposition(obj, _dbmgr);
        }
        
        #endregion

        #region disposition
        
        private void FetchDisposition(int id, IDBManager dbmgr)
        {
            string qryLocal = "SELECT * FROM viewDISPLIST WHERE DISP_ID=@id";
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(1);
                dbmgr.AddParameters(0, "@id", id);
                dbmgr.ExecuteReader(CommandType.Text, qryLocal);
                if (dbmgr.DataReader.Read())
                {
                    PropertyInfo[] p = _disp.GetType().GetProperties();                     // get properties of object
                    _disp = (Disposition)FetchObject(_disp, p, dbmgr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbmgr.Dispose();
            }
        }
        
        private void DeleteDisposition(object obj, IDBManager dbmgr)
        {
            Disposition item = (Disposition)obj;									        //cast object to proper item type
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(2);
                dbmgr.AddParameters(0, "@disp_id", item.disp_id);
                dbmgr.AddParameters(1, "@error_num", 0, true);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.DISPLIST_d");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbmgr.Dispose();
            }
        }
        
        private void UpdateDisposition(object obj, IDBManager dbmgr)
        {
            Disposition item = (Disposition)obj;
            System.Data.Common.DbParameter new_disp_id;
            try
            {
                dbmgr.Open();
                dbmgr.BeginTransaction();
                if (item.disp_id == 0)
                {
                    // if is id null then create new object
                    dbmgr.CreateParameters(3);
                    dbmgr.AddParameters(0, "@id", 0, true);
                    dbmgr.AddParameters(1, "@disp", item.disp);
                    dbmgr.AddParameters(2, "@disp_desc", item.disp_desc);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.DISPLIST_i");

                    // get item id
                    new_disp_id = (System.Data.Common.DbParameter)dbmgr.Parameters.GetValue(0);
                    item.disp_id = Convert.ToInt32(new_disp_id.Value);
                }
                else
                {
                    // update table
                    dbmgr.CreateParameters(3);
                    dbmgr.AddParameters(0, "@disp_id", item.disp_id);
                    dbmgr.AddParameters(1, "@disp", item.disp);
                    dbmgr.AddParameters(2, "@disp_desc", item.disp_desc);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.DISPLIST_u");
                }
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
        
        #endregion

        #region facompcabdisp
        
        private void FetchFACompCabList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFACOMPCABDISP WHERE DISP_ID = @disp_id ORDER BY FA, COMP, CABLE";
            FACabDisp item = new FACabDisp();									            //create new object type to be able to get property info
            ArrayList list = new ArrayList();										        //create new ArrayList to house objects
            try
            {
                PropertyInfo[] p = item.GetType().GetProperties();					        //get property info for item
                dbmgr.Open();														        //open database manager
                dbmgr.CreateParameters(1);											        //create parameters
                dbmgr.AddParameters(0, "@disp_id", id);							            //disp id
                dbmgr.ExecuteReader(CommandType.Text, qryString);					        //execute query
                while (dbmgr.DataReader.Read())
                {
                    item = new FACabDisp();											        //create new item
                    item = (FACabDisp)FetchObject(item, p, dbmgr);
                    list.Add(item);													        //add item to the ArrayList
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbmgr.Dispose();
            }
            _disp.facompcablist = list;											            //update item list
        }
        
        #endregion

    }
}
