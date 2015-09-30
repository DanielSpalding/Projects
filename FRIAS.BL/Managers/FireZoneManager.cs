using System;
using System.Data;
using System.Collections;
using System.Reflection;

using FRIAS.Common;
using FRIAS.Common.Entity;
using DL.Data;

namespace FRIAS.BL.Managers
{
    public class FireZoneManager : TotalManager, IFactoryManager
    {
        private User _user;
        private IDBManager _dbmgr;                                                          // database manager
        private FireZone _firezone;                                                         // instance of primary object
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
            string type;

            // determine id and type
            string objtype = obj.GetType().ToString();
            if (objtype == "System.String")
            {
                string[] strTemp = ((string)obj).Split(new char[] { '|' });
                id = Convert.ToInt32(strTemp[0]);
                type = strTemp[1];
            }
            else
            {
                id = ((FireZone)obj).fz_id;
                type = "all";
            }

            // initialize
            _firezone = new FireZone();
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            try
            {
                _dbmgr.Open();

                // fetch object information
                switch (type)
                {
                    case "firezone":
                        FetchFireZone(id, _dbmgr);
                        return _firezone;
                    case "protection":
                        FetchProtectionList(id, _dbmgr);
                        return _firezone;
                    case "ignition":
                        FetchIgnitionList(id, _dbmgr);
                        return _firezone;
                    case "ignitionimpact":
                        // TODO: Need to pass ignition information
                        //FetchIgnitionImpactList(id, _dbmgr);
                        return _firezone;
                    case "all":
                        FetchFireZone(id, _dbmgr);
                        FetchComponentList(id, _dbmgr);
                        FetchCableList(id, _dbmgr);
                        FetchRouteList(id, _dbmgr);
                        FetchProtectionList(id, _dbmgr);
                        FetchIgnitionList(id, _dbmgr);
                        return _firezone;
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

            return _firezone;
        }

        public DataSet FetchDataSet(string criteria)
        {
            DataSet ds = new DataSet();
            string qryString = "SELECT * FROM FZLIST ORDER BY FZ";

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
            
            string type = obj.GetType().ToString();
            
            // delete from database
            switch (type)
            {
                case "FRIAS.Common.Entity.FireZone":
                    DeleteFireZone(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FZProtection":
                    DeleteProtection(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FZIgnition":
                    DeleteIgnition(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FZIgnitionImpact":
                    DeleteIgnitionImpact(obj, _dbmgr);
                    break;
            }
            
        }

        public void Update(object obj)
        {
            // get connection
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            string type = obj.GetType().ToString();

            // update from database
            switch (type)
            {
                case "FRIAS.Common.Entity.FireZone":
                    UpdateFireZone(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FZProtection":
                    UpdateProtection(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FZIgnition":
                    UpdateIgnition(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FZIgnitionImpact":
                    UpdateIgnitionImpact(obj, _dbmgr);
                    break;
            }
        
        }

        #endregion

        #region fire zone

        // procedure fetches fire zone information
        private void FetchFireZone(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFZLIST WHERE FZ_ID=@id";

            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);
            if (dbmgr.DataReader.Read())
            {
                // get properties of object and fetch object
                PropertyInfo[] p = _firezone.GetType().GetProperties();
                _firezone = (FireZone)FetchObject(_firezone, p, dbmgr);
            }

            dbmgr.CloseReader();
        }

        private void DeleteFireZone(object obj, IDBManager dbmgr)
        {
            FireZone item = (FireZone)obj;
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.BeginTransaction();
                dbmgr.CreateParameters(1);												    // create required paramaters
                dbmgr.AddParameters(0, "@fz_id", item.fz_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FZLIST_d");	        // execute stored procedure

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
        
        private void UpdateFireZone(object obj, IDBManager dbmgr)
        {
            FireZone item = (FireZone)obj;
            System.Data.Common.DbParameter param;
            
            try
            {
                dbmgr.Open();
                dbmgr.BeginTransaction();

                // check to see if new object
                if (item.fz_id == 0)
                {
                    // if id is null then new object
                    dbmgr.CreateParameters(4);
                    dbmgr.AddParameters(0, "@fz", item.fz);
                    dbmgr.AddParameters(1, "@id", 0, true);
                    dbmgr.AddParameters(2, "@fa_id", item.fa_id);
                    dbmgr.AddParameters(3, "@fz_desc", item.fz_desc);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FZLIST_i");

                    // get item id
                    param = (System.Data.Common.DbParameter)dbmgr.Parameters.GetValue(1);
                    item.fz_id = Convert.ToInt32(param.Value);
                }

                // otherwise update existing object
                else
                {
                    // update FZLIST table
                    dbmgr.CreateParameters(14);
                    dbmgr.AddParameters(0, "@fz_id", item.fz_id);
                    dbmgr.AddParameters(1, "@fz", item.fz);
                    dbmgr.AddParameters(2, "@fz_desc", item.fz_desc);
                    dbmgr.AddParameters(3, "@bldg", item.bldg);
                    dbmgr.AddParameters(4, "@elev", item.elev);
                    dbmgr.AddParameters(5, "@fa_id", item.fa_id);
                    dbmgr.AddParameters(6, "@req", item.req);
                    dbmgr.AddParameters(7, "@supp", item.supp);
                    dbmgr.AddParameters(8, "@det", item.det);
                    dbmgr.AddParameters(9, "@ventilation", item.ventilation);
                    dbmgr.AddParameters(10, "@barrier", item.barrier);
                    dbmgr.AddParameters(11, "@otherprotection", item.otherprotection);
                    dbmgr.AddParameters(12, "@dwg_id", item.dwg_id);
                    dbmgr.AddParameters(13, "@dwg_rev", item.dwg_rev);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FZLIST_u");
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
            string qryString;

            switch (_user.locaType)
            {
                case Constant.type_FR:
                    qryString = "SELECT DISTINCT FZ_ID, FZ, COMP_ID, COMP, dbo.IN_FZ(COMP_ID, FZ_ID) AS IN_FZ FROM viewFRDATA " +
                        "WHERE COMP_SSD_REQ=1 AND CABLE_SSD_REQ=1 AND COMP IS NOT NULL AND FZ_ID=@fz_id ORDER BY COMP";
                    break;
                default:
                    qryString = "SELECT DISTINCT FZ_ID, FZ, COMP_ID, COMP, dbo.IN_FZ(COMP_ID, FZ_ID) AS IN_FZ FROM viewFZDATA " +
                        "WHERE COMP_SSD_REQ=1 AND CABLE_SSD_REQ=1 AND COMP IS NOT NULL AND FZ_ID=@fz_id ORDER BY COMP";
                    break;
            }

            FComp item = new FComp();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create required parameters
            dbmgr.AddParameters(0, "@fz_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new FComp();											            // create new item
                item = (FComp)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to ArrayList
            }

            dbmgr.CloseReader();
            _firezone.componentlist = list;												    // update object item list
        }

        #endregion

        #region cable

        private void FetchCableList(int id, IDBManager dbmgr)
        {
            string qryString;

            switch (_user.locaType)
            {
                case Constant.type_FR:
                    qryString = "SELECT DISTINCT FZ_ID, FZ, COMP_ID, COMP, CABLE_ID, CABLE FROM viewFRDATA " +
                        "WHERE COMP_SSD_REQ=1 AND CABLE_SSD_REQ=1 AND CABLE IS NOT NULL AND FZ_ID=@fz_id ORDER BY COMP, CABLE";
                    break;
                default:
                    qryString = "SELECT DISTINCT FZ_ID, FZ, COMP_ID, COMP, CABLE_ID, CABLE FROM viewFZDATA " +
                        "WHERE COMP_SSD_REQ=1 AND CABLE_SSD_REQ=1 AND CABLE IS NOT NULL AND FZ_ID=@fz_id ORDER BY COMP, CABLE";
                    break;
            }

            FCab item = new FCab();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item

            dbmgr.CreateParameters(1);											            // create required parameters
            dbmgr.AddParameters(0, "@fz_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new FCab();											                // create new item
                item = (FCab)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to ArrayList
            }

            dbmgr.CloseReader();
            _firezone.cablelist = list;												        // update object item list

        }

        #endregion

        #region route

        private void FetchRouteList(int id, IDBManager dbmgr)
        {
            string qryString;

            switch (_user.locaType)
            {
                case Constant.type_FR:
                    qryString = "SELECT DISTINCT FZ_ID, FZ, NODE_ID, NODE FROM viewFRDATA WHERE NODE IS NOT NULL AND FZ_ID=@fz_id " +
                        "UNION " +
                        "SELECT DISTINCT FZ_ID, FZ, NODE_ID, NODE FROM viewROUTELOCA_FR WHERE NODE_ID IS NOT NULL AND FZ_ID=@fz_id " +
                        "ORDER BY NODE";
                    break;
                default:
                    qryString = "SELECT DISTINCT FZ_ID, FZ, NODE_ID, NODE FROM viewFZDATA WHERE NODE IS NOT NULL AND FZ_ID=@fz_id " +
                        "UNION " +
                        "SELECT DISTINCT FZ_ID, FZ, NODE_ID, NODE FROM viewROUTELOCA WHERE NODE_ID IS NOT NULL AND FZ_ID=@fz_id " +
                        "ORDER BY NODE";
                    break;
            }

            FRoute item = new FRoute();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create required parameters
            dbmgr.AddParameters(0, "@fz_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new FRoute();											                // create new item
                item = (FRoute)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to ArrayList
            }

            dbmgr.CloseReader();
            _firezone.routelist = list;												        // update object item list

        }

        #endregion

        #region fire protection

        private void FetchProtectionList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFZPROTECTION WHERE FZ_ID = @fz_id ORDER BY SYS_CATEGORY";
            FZProtection item = new FZProtection();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@fz_id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new FZProtection();
                item = (FZProtection)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _firezone.protectionlist = list;
        }

        private void DeleteProtection(object obj, IDBManager dbmgr)
        {
            FZProtection item = (FZProtection)obj;
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(3);
                dbmgr.AddParameters(0, "@fz_id", item.fz_id);
                dbmgr.AddParameters(1, "@sys_category", item.sys_category);
                dbmgr.AddParameters(2, "@sys_name", item.sys_name);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FZPROTECTION_d");
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

        private void UpdateProtection(object obj, IDBManager dbmgr)
        {
            FZProtection item = (FZProtection)obj;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(6);
                dbmgr.AddParameters(0, "@fz_id", item.fz_id);
                dbmgr.AddParameters(1, "@sys_category", item.sys_category);
                dbmgr.AddParameters(2, "@sys_name", item.sys_name);
                dbmgr.AddParameters(3, "@sys_desc", item.sys_desc);
                dbmgr.AddParameters(4, "@sys_type", item.sys_type);
                dbmgr.AddParameters(5, "@comment", item.comment);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FZPROTECTION_u");
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

        #endregion

        #region ignition source

        private void FetchIgnitionList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFZIGNITION WHERE FZ_ID = @fz_id ORDER BY IG";
            FZIgnition item = new FZIgnition();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@fz_id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new FZIgnition();
                item = (FZIgnition)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _firezone.ignitionlist = list;
        }

        private void DeleteIgnition(object obj, IDBManager dbmgr)
        {
            FZIgnition item = (FZIgnition)obj;
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(2);
                dbmgr.AddParameters(0, "@fz_id", item.fz_id);
                dbmgr.AddParameters(1, "@ig", item.ig);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FZIGNITION_d");
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

        private void UpdateIgnition(object obj, IDBManager dbmgr)
        {
            FZIgnition item = (FZIgnition)obj;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(8);
                dbmgr.AddParameters(0, "@fz_id", item.fz_id);
                dbmgr.AddParameters(1, "@ig", item.ig);
                dbmgr.AddParameters(2, "@ig_desc", item.ig_desc);
                dbmgr.AddParameters(3, "@ig_loc", item.ig_loc);
                dbmgr.AddParameters(4, "@bin_id", item.bin_id);
                dbmgr.AddParameters(5, "@bin_count", item.bin_count);
                dbmgr.AddParameters(6, "@zoi", item.zoi);
                dbmgr.AddParameters(7, "@note", item.note);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FZIGNITION_u");
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

        #endregion

        #region ignition impact

        private void FetchIgnitionImpactList(int fz_id, string ig, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFZIGNITIONIMPACT WHERE FZ_ID = @fz_id AND IG = @ig ORDER BY ITEM";
            FZIgnitionImpact item = new FZIgnitionImpact();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.CreateParameters(2);
            dbmgr.AddParameters(0, "@fz_id", fz_id);
            dbmgr.AddParameters(1, "@ig", ig);
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new FZIgnitionImpact();
                item = (FZIgnitionImpact)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _firezone.impactlist = list;
        }

        private void DeleteIgnitionImpact(object obj, IDBManager dbmgr)
        {
            FZIgnitionImpact item = (FZIgnitionImpact)obj;
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(4);
                dbmgr.AddParameters(0, "@fz_id", item.fz_id);
                dbmgr.AddParameters(1, "@ig", item.ig);
                dbmgr.AddParameters(2, "@item_id", item.item_id);
                dbmgr.AddParameters(3, "@item_table", item.item_table);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FZIGNITIONIMPACT_d");
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

        private void UpdateIgnitionImpact(object obj, IDBManager dbmgr)
        {
            FZIgnitionImpact item = (FZIgnitionImpact)obj;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(6);
                dbmgr.AddParameters(0, "@fz_id", item.fz_id);
                dbmgr.AddParameters(1, "@ig", item.ig);
                dbmgr.AddParameters(2, "@item_id", item.item_id);
                dbmgr.AddParameters(3, "@item", item.item);
                dbmgr.AddParameters(4, "@item_table", item.item_table);
                dbmgr.AddParameters(5, "@wrap", item.wrap);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FZIGNITIONIMPACT_u");
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

        #endregion
    }
}
