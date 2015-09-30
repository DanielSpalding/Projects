using System;
using System.Data;
using System.Collections;
using System.Reflection;

using FRIAS.Common;
using FRIAS.Common.Entity;
using DL.Data;

namespace FRIAS.BL.Managers
{
    public class FireRoomManager : TotalManager, IFactoryManager
    {
        private User _user;
        private IDBManager _dbmgr;                                                          // database manager
        private FireRoom _fireroom;                                                         // instance of primary object
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
            id = ((FireRoom)obj).rm_id;
            
            // initialize
            _fireroom = new FireRoom();
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            try
            {
                _dbmgr.Open();
                // fetch object information
                FetchFireRoom(id, _dbmgr);
                FetchComponentList(id, _dbmgr);
                FetchCableList(id, _dbmgr);
                FetchRouteList(id, _dbmgr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dbmgr.Dispose();
            }

            return _fireroom;
        }

        public DataSet FetchDataSet(string criteria)
        {
            DataSet ds = new DataSet();
            string qryString = "SELECT * FROM FRLIST ORDER BY RM";

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
            ArrayList list = new ArrayList();
            return list;
        }

        public void Delete(object obj)
        {
            // get connection
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;
            
            // delete from database
            DeleteFireRoom(obj, _dbmgr);
            
        }

        public void Update(object obj)
        {
            // get connection
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            UpdateFireRoom(obj, _dbmgr);
        
        }
        #endregion

        #region fire room

        // procedure fetches fire zone information
        private void FetchFireRoom(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFRLIST WHERE RM_ID=@id";

            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);
            if (dbmgr.DataReader.Read())
            {
                // get properties of object and fetch object
                PropertyInfo[] p = _fireroom.GetType().GetProperties();
                _fireroom = (FireRoom)FetchObject(_fireroom, p, dbmgr);
            }

            dbmgr.CloseReader();
        }

        private void DeleteFireRoom(object obj, IDBManager dbmgr)
        {
            FireRoom item = (FireRoom)obj;
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.BeginTransaction();
                dbmgr.CreateParameters(1);												    // create required paramaters
                dbmgr.AddParameters(0, "@rm_id", item.rm_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FRLIST_d");	        // execute stored procedure

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
        
        private void UpdateFireRoom(object obj, IDBManager dbmgr)
        {
            FireRoom item = (FireRoom)obj;
            System.Data.Common.DbParameter param;
            
            try
            {
                dbmgr.Open();
                dbmgr.BeginTransaction();

                // check to see if new object
                if (item.rm_id == 0)
                {
                    // if id is null then new object
                    dbmgr.CreateParameters(4);
                    dbmgr.AddParameters(0, "@rm", item.rm);
                    dbmgr.AddParameters(1, "@id", 0, true);
                    dbmgr.AddParameters(2, "@fz_id", item.fz_id);
                    dbmgr.AddParameters(3, "@rm_desc", item.rm_desc);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FRLIST_i");

                    // get item id
                    param = (System.Data.Common.DbParameter)dbmgr.Parameters.GetValue(1);
                    item.rm_id = Convert.ToInt32(param.Value);
                }

                // otherwise update existing object
                else
                {
                    // update FRLIST table
                    dbmgr.CreateParameters(11);
                    dbmgr.AddParameters(0, "@rm_id", item.rm_id);
                    dbmgr.AddParameters(1, "@rm", item.rm);
                    dbmgr.AddParameters(2, "@rm_desc", item.rm_desc);
                    dbmgr.AddParameters(3, "@bldg", item.bldg);
                    dbmgr.AddParameters(4, "@elev", item.elev);
                    dbmgr.AddParameters(5, "@fz_id", item.fz_id);
                    dbmgr.AddParameters(6, "@req", item.req);
                    dbmgr.AddParameters(7, "@supp", item.supp);
                    dbmgr.AddParameters(8, "@det", item.det);
                    dbmgr.AddParameters(9, "@dwg_id", item.dwg_id);
                    dbmgr.AddParameters(10, "@dwg_rev", item.dwg_rev);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FRLIST_u");
                }

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

        #region component

        private void FetchComponentList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT DISTINCT RM_ID, RM, FZ_ID, FZ, COMP_ID, COMP, dbo.IN_FR(COMP_ID, FZ_ID) AS IN_FZ FROM viewFRDATA " +
                "WHERE COMP_SSD_REQ=1 AND CABLE_SSD_REQ=1 AND COMP IS NOT NULL AND RM_ID=@rm_id ORDER BY COMP";

            FComp item = new FComp();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create required parameters
            dbmgr.AddParameters(0, "@rm_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new FComp();											            // create new item
                item = (FComp)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to ArrayList
            }

            dbmgr.CloseReader();
            _fireroom.componentlist = list;												    // update object item list
        
        }

        #endregion

        #region cable

        private void FetchCableList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT DISTINCT RM_ID, RM, FZ_ID, FZ, COMP_ID, COMP, CABLE_ID, CABLE FROM viewFrDATA " +
                "WHERE COMP_SSD_REQ=1 AND CABLE_SSD_REQ=1 AND CABLE IS NOT NULL AND RM_ID=@rm_id ORDER BY COMP, CABLE";

            FCab item = new FCab();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item

            dbmgr.CreateParameters(1);											            // create required parameters
            dbmgr.AddParameters(0, "@rm_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new FCab();											                // create new item
                item = (FCab)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to ArrayList
            }

            dbmgr.CloseReader();
            _fireroom.cablelist = list;												        // update object item list

        }

        #endregion

        #region route

        private void FetchRouteList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT DISTINCT RM_ID, RM, FZ_ID, FZ, NODE_ID, NODE FROM viewFRDATA WHERE NODE IS NOT NULL AND RM_ID=@rm_id " +
                "UNION " +
                "SELECT DISTINCT RM_ID, RM, FZ_ID, FZ, NODE_ID, NODE FROM viewROUTELOCA_FR WHERE NODE_ID IS NOT NULL AND RM_ID=@rm_id " +
                "ORDER BY NODE";

            FRoute item = new FRoute();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create required parameters
            dbmgr.AddParameters(0, "@rm_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new FRoute();											                // create new item
                item = (FRoute)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to ArrayList
            }

            dbmgr.CloseReader();
            _fireroom.routelist = list;												        // update object item list

        }

        #endregion

    }
}
