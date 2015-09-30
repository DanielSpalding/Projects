using System;
using System.Data;
using System.Collections;
using System.Reflection;

using FRIAS.Common;
using FRIAS.Common.Entity;
using DL.Data;

namespace FRIAS.BL.Managers
{
    public class PowerManager : TotalManager, IFactoryManager
    {
        private User _user;
        private IDBManager _dbmgr;                                                          // database manager
        private Power _power;                                                               // instance of primary object
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
            string type;                                                                    // type of object passed
            string objtype = obj.GetType().ToString();                                      // get object type
            
            if (objtype == "System.String")
            {
                string[] strTemp = ((string)obj).Split(new char[] { '|' });                 // if object type is a string then extract the id and type
                id = Convert.ToInt32(strTemp[0]);
                type = strTemp[1];
            }
            else
            {
                id = ((Power)obj).power_id;
                type = "all";                                                               // object is  drawing and get all information
            }
            
            _power = new Power();
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            try
            {
                _dbmgr.Open();

                switch (type)
                {
                    case "component":
                        FetchComponentList(id, _dbmgr);
                        return _power.componentlist;
                    case "psload":
                        FetchPSLoadList(id, _dbmgr);
                        return _power.psloadlist;
                    case "psbkrcab":
                        FetchPSBkrCabList(id, _dbmgr);
                        return _power.psbkrcablist;
                    case "cabroutelist":
                        FetchCabRoutelist(id, _dbmgr);
                        return _power.cabroutelist;
                    default:
                        FetchPower(id, _dbmgr);
                        FetchComponentList(id, _dbmgr);
                        FetchPSLoadList(id, _dbmgr);
                        return _power;
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
            string qryString = "SELECT * FROM viewPOWERLIST ORDER BY POWER";

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
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;
            
            string type = obj.GetType().ToString();
            
            switch (type)
            {
                case "FRIAS.Common.Entity.PSLoad":
                    DeletePSLoad(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.PSBkrCab":
                    DeletePSBkrCab(obj, _dbmgr);
                    break;
            }
        }
        
        public void Update(object obj)
        {
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            string type = obj.GetType().ToString();
            
            switch (type)
            {
                case "FRIAS.Common.Entity.PSLoad":
                    UpdatePSLoad(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.PSBkrCab":
                    UpdatePSBkrCab(obj, _dbmgr);
                    break;
                case "System.Collections.ArrayList":
                    UpdateCabRoute(obj, _dbmgr);
                    break;
            }
        }
        
        #endregion

        #region power
        
        private void FetchPower(int id, IDBManager dbmgr)
        {
            string qryLocal = "SELECT * FROM viewPOWERLIST WHERE POWER_ID=@id ORDER BY POWER";
                
            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryLocal);
            if (dbmgr.DataReader.Read())
            {
                PropertyInfo[] p = _power.GetType().GetProperties();                        // get properties of object
                _power = (Power)FetchObject(_power, p, dbmgr);
            }

            dbmgr.CloseReader();
        }
        
        #endregion

        #region component

        private void FetchComponentList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCOMPPWR cp WHERE cp.POWER_ID = @power_id ORDER BY cp.COMP";
            CompPwr item = new CompPwr();											        // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@power_id", id);								        // power id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new CompPwr();											            // create new item
                item = (CompPwr)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _power.componentlist = list;									                // update item list

        }

        #endregion

        #region psload

        private void FetchPSLoadList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewPSLOADS ps WHERE ps.POWER_ID = @power_id ORDER BY ps.BKRFUSE";
            PSLoad item = new PSLoad();											            // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@power_id", id);								        // power id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new PSLoad();											            // create new item
                item = (PSLoad)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _power.psloadlist = list;									                    // update item list

        }

        private void DeletePSLoad(object obj, IDBManager dbmgr)
        {
            PSLoad item = (PSLoad)obj;												        // cast object to proper item type
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(2);												    // create required paramaters
                dbmgr.AddParameters(0, "@psload_id", item.psload_id);
                dbmgr.AddParameters(1, "@error_num", 0, true);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.PSLOADS_d");	    // execute stored procedure
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

        private void UpdatePSLoad(object obj, IDBManager dbmgr)
        {
            PSLoad item = (PSLoad)obj;												        // cast object to proper item type
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(11);												    // create required paramaters
                dbmgr.AddParameters(0, "@psload_id", item.psload_id);
                dbmgr.AddParameters(1, "@power_id", item.power_id);
                dbmgr.AddParameters(2, "@bkrfuse", item.bkrfuse);
                dbmgr.AddParameters(3, "@load_id", item.load_id);
                dbmgr.AddParameters(4, "@load_desc", item.load_desc);
                dbmgr.AddParameters(5, "@is_incoming", item.is_incoming);
                dbmgr.AddParameters(6, "@up_feed", item.up_feed);
                dbmgr.AddParameters(7, "@dn_feed", item.dn_feed);
                dbmgr.AddParameters(8, "@coord", item.coord);
                dbmgr.AddParameters(9, "@coord_len", item.coord_len);
                dbmgr.AddParameters(10, "@tcc_ref", item.tcc_ref);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.PSLOADS_u");	    // execute stored procedure
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

        #region psbkrcab
        
        private void FetchPSBkrCabList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewPSBKRCAB ps WHERE ps.PSLOAD_ID = @psload_id ORDER BY ps.CABLE";
            PSBkrCab item = new PSBkrCab();											        // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
         
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@psload_id", id);								        // power id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new PSBkrCab();											            // create new item
                item = (PSBkrCab)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _power.psbkrcablist = list;									                    // update item list
        }
        
        private void DeletePSBkrCab(object obj, IDBManager dbmgr)
        {
            PSBkrCab item = (PSBkrCab)obj;												    // cast object to proper item type
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(2);												    // create required paramaters
                dbmgr.AddParameters(0, "@psbkrcab_id", item.psbkrcab_id);
                dbmgr.AddParameters(1, "@error_num", 0, true);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.PSBKRCAB_d");	    // execute stored procedure
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
        
        private void UpdatePSBkrCab(object obj, IDBManager dbmgr)
        {
            PSBkrCab item = (PSBkrCab)obj;
            System.Data.Common.DbParameter new_psbkrcab_id;
            try
            {
                dbmgr.Open();
                dbmgr.BeginTransaction();

                // check if new item
                if (item.psbkrcab_id == 0)
                {
                    // if id is null then create new item
                    dbmgr.CreateParameters(3);
                    dbmgr.AddParameters(0, "@id", 0, true);
                    dbmgr.AddParameters(1, "@psload_id", item.psload_id);
                    dbmgr.AddParameters(2, "@cable_id", item.cable_id);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.PSBKRCAB_i");

                    // get item id
                    new_psbkrcab_id = (System.Data.Common.DbParameter)dbmgr.Parameters.GetValue(0);
                    item.psbkrcab_id = Convert.ToInt32(new_psbkrcab_id.Value);
                }
                else
                {
                    // update PSBKRCAB table
                    dbmgr.CreateParameters(3);
                    dbmgr.AddParameters(0, "@psbkrcab_id", item.psbkrcab_id);
                    dbmgr.AddParameters(1, "@psload_id", item.psload_id);
                    dbmgr.AddParameters(2, "@cable_id", item.cable_id);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.PSBKRCAB_u");
                }

                // update status
                UpdateStatus(dbmgr, item, 1, item.prep_by, item.prep_date);
                UpdateStatus(dbmgr, item, 5, item.chkd_by, item.chkd_date);
                
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

        private void UpdateStatus(IDBManager dbmgr, PSBkrCab item, int statustype_id, string user_id, DateTime status_date)
        {
            if ((user_id == "N/A") || (user_id == ""))
            {
                // delete status
                dbmgr.CreateParameters(2);
                dbmgr.AddParameters(0, "@psbkrcab_id", item.psbkrcab_id);
                dbmgr.AddParameters(1, "@statustype_id", statustype_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.PSBKRCABSTATUS_d");
            }
            else
            {
                // update status
                dbmgr.CreateParameters(4);
                dbmgr.AddParameters(0, "@psbkrcab_id", item.psbkrcab_id);
                dbmgr.AddParameters(1, "@statustype_id", statustype_id);
                dbmgr.AddParameters(2, "@user_id", user_id);
                if (status_date == Convert.ToDateTime(null))
                    // set date to null
                    dbmgr.AddParameters(3, "@status_date", null);
                else
                    // update date
                    dbmgr.AddParameters(3, "@status_date", status_date);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.PSBKRCABSTATUS_u");
            }
        }

        #endregion

        #region cabroute

        private void FetchCabRoutelist(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCABROUTE_COORD AS cr WHERE cr.PSBKRCAB_ID = @psbkrcab_id ORDER BY cr.SEQ, cr.NODE";

            CabRoute item = new CabRoute();
            ArrayList list = new ArrayList();
            PropertyInfo[] p = item.GetType().GetProperties();

            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@psbkrcab_id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new CabRoute();
                item = (CabRoute)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _power.cabroutelist = list;
        }

        private void UpdateCabRoute(object obj, IDBManager dbmgr)
        {
            ArrayList list = (ArrayList)obj;

            try
            {
                dbmgr.Open();

                foreach (CabRoute item in list)
                {
                    dbmgr.CreateParameters(5);
                    dbmgr.AddParameters(0, "@psbkrcab_id", item.psbkrcab_id);
                    dbmgr.AddParameters(1, "@seq", item.seq);
                    dbmgr.AddParameters(2, "@node_id", item.node_id);
                    dbmgr.AddParameters(3, "@fz_id", item.fz_id);
                    dbmgr.AddParameters(4, "@uncoord", item.uncoord);

                    if(item.uncoord == false)
                        dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.CABROUTE_COORD_d");
                    else
                        dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.CABROUTE_COORD_i");
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

        #endregion
    }
}
