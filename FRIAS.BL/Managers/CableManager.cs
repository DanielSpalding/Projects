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
    public class CableManager : TotalManager, IFactoryManager
    {
        private User _user;
        private IDBManager _dbmgr;                                                          // database manager
        private Cable _cable;                                                               // instance of primary object
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
                string[] strTemp = ((string)obj).Split(new char[] { '|' });                 // if the object type is a string then extract the id and type
                id = Convert.ToInt32(strTemp[0]);
                type = strTemp[1];
            }
            else
            {
                id = ((Cable)obj).cable_id;
                type = "all";                                                               // object is a cable and get all information
            }
            
            _cable = new Cable();
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;
            
            switch (type)
            {
                case "component":
                    FetchComponentList(id, _dbmgr);
                    return _cable.componentlist;
                case "drawing":
                    FetchDrawingList(id, _dbmgr);
                    return _cable.drawinglist;
                case "routechg":
                    FetchRoutechgList(id, _dbmgr);
                    return _cable.routechglist;
                case "all":
                    FetchCable(id, _dbmgr);
                    FetchComponentList(id, _dbmgr);
                    FetchDrawingList(id, _dbmgr);
                    FetchRoutechgList(id, _dbmgr);
                    FetchRouteList(id, _dbmgr);
                    FetchCRDPowerList(id, _dbmgr);
                    return _cable;
            }
            return _cable;
        }

        public DataSet FetchDataSet(string criteria)
        {
            DataSet ds = new DataSet();
            string qryString;
            
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];

            switch (_user.plant)
            {
                case "ANO":
                    qryString = "SELECT * FROM viewCABLIST_LITE ORDER BY CABLE";
                    break;
                case "BLN":
                case "TFAC":
                    qryString = "SELECT * FROM viewCABLIST ORDER BY CABLE";
                    break;
                default:
                    qryString = "SELECT * FROM viewCABLIST_USED ORDER BY CABLE";
                    break;
            }
            
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
            
            string type = obj.GetType().ToString();
            
            switch (type)
            {
                case "FRIAS.Common.Entity.Cable":
                    DeleteCable(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.CabDwg":
                    DeleteDrawing(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.CabRoute":
                    DeleteRoutechg(obj, _dbmgr);
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
                case "FRIAS.Common.Entity.Cable":
                    UpdateCable(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.CabDwg":
                    UpdateDrawing(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.CabRoute":
                    UpdateRoutechg(obj, _dbmgr);
                    break;
            }
        }
        
        #endregion

        #region cable
        
        private void FetchCable(int id, IDBManager dbmgr)
        {
            string qryLocal = "SELECT *, dbo.GET_LOCATION(FR_EQUIP) AS FR_EQUIP_LOC, dbo.GET_LOCATION(TO_EQUIP) AS TO_EQUIP_LOC, " +
	            " dbo.GET_LOCATION(FR_EQUIP_ORIG) AS FR_EQUIP_ORIG_LOC, dbo.GET_LOCATION(TO_EQUIP_ORIG) AS TO_EQUIP_ORIG_LOC " +
                " FROM viewCABLIST WHERE CABLE_ID = @id";
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(1);
                dbmgr.AddParameters(0, "@id", id);
                dbmgr.ExecuteReader(CommandType.Text, qryLocal);
                if (dbmgr.DataReader.Read())
                {
                    PropertyInfo[] p = _cable.GetType().GetProperties();                    // get properties of object
                    _cable = (Cable)FetchObject(_cable, p, dbmgr);
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
        
        private void DeleteCable(object obj, IDBManager dbmgr)
        {
            Cable item = (Cable)obj;											            // cast object to proper item type
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(2);												    // create required paramaters
                dbmgr.AddParameters(0, "@cable_id", item.cable_id);
                dbmgr.AddParameters(1, "@error_num", 0, true);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.CABLIST_d");	    // execute stored procedure
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
        
        // updates cable
        private void UpdateCable(object obj, IDBManager dbmgr)
        {
            Cable item = (Cable)obj;
            System.Data.Common.DbParameter new_cable_id;

            try
            {
                dbmgr.Open();
                dbmgr.BeginTransaction();

                // check to see if new object
                if (item.cable_id == 0)
                {
                    // if id is null then create new object
                    dbmgr.CreateParameters(4);
                    dbmgr.AddParameters(0, "@id", 0, true);
                    dbmgr.AddParameters(1, "@cable", item.cable);
                    dbmgr.AddParameters(2, "@fr_equip_id", item.fr_equip_id);
                    dbmgr.AddParameters(3, "@to_equip_id", item.to_equip_id);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.CABLIST_i");

                    // get item id
                    new_cable_id = (System.Data.Common.DbParameter)dbmgr.Parameters.GetValue(0);
                    item.cable_id = Convert.ToInt32(new_cable_id.Value);
                }
                else
                {
                    // update CABLIST table
                    dbmgr.CreateParameters(12);
                    dbmgr.AddParameters(0, "@cable_id", item.cable_id);
                    dbmgr.AddParameters(1, "@cable", item.cable);
                    dbmgr.AddParameters(2, "@fr_equip_id", item.fr_equip_id);
                    dbmgr.AddParameters(3, "@to_equip_id", item.to_equip_id);
                    dbmgr.AddParameters(4, "@fr_dwg_id", item.fr_dwg_id);
                    dbmgr.AddParameters(5, "@to_dwg_id", item.to_dwg_id);
                    dbmgr.AddParameters(6, "@fr_equip_bd", item.fr_equip_bd);
                    dbmgr.AddParameters(7, "@to_equip_bd", item.to_equip_bd);
                    dbmgr.AddParameters(8, "@cable_len", item.cable_len);
                    dbmgr.AddParameters(9, "@cable_size", item.cable_size);
                    dbmgr.AddParameters(10, "@cable_code", item.cable_code);
                    dbmgr.AddParameters(11, "@comment", item.comment);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.CABLIST_u");	// execute the stored procedure

                    //update status
                    UpdateStatus(dbmgr, item, 1, item.prep_by, item.prep_date);
                    UpdateStatus(dbmgr, item, 5, item.chkd_by, item.chkd_date);
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

        private void UpdateStatus(IDBManager dbmgr, Cable item, int statustype_id, string user_id, DateTime status_date)
        {
            if ((user_id == "N/A") || (user_id == ""))
            {
                // delete status
                dbmgr.CreateParameters(2);
                dbmgr.AddParameters(0, "@cable_id", item.cable_id);
                dbmgr.AddParameters(1, "@statustype_id", statustype_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.CABSTATUS_d");
            }
            else
            {
                // update status
                dbmgr.CreateParameters(4);
                dbmgr.AddParameters(0, "@cable_id", item.cable_id);
                dbmgr.AddParameters(1, "@statustype_id", statustype_id);
                dbmgr.AddParameters(2, "@user_id", user_id);
                if (status_date == Convert.ToDateTime(null))
                    // set date to null
                    dbmgr.AddParameters(3, "@status_date", null);
                else
                    // update date
                    dbmgr.AddParameters(3, "@status_date", status_date);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.CABSTATUS_u");
            }
        }

        #endregion

        #region component
        
        private void FetchComponentList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCOMPCAB cc WHERE cc.CABLE_ID = @cable_id ORDER BY cc.COMP";
            CompCab item = new CompCab();											        //create new object type to be able to get property info
            ArrayList list = new ArrayList();										        //create new ArrayList to house objects
            try
            {
                PropertyInfo[] p = item.GetType().GetProperties();					        //get property info for item
                dbmgr.Open();														        //open database manager
                dbmgr.CreateParameters(1);											        //create parameters
                dbmgr.AddParameters(0, "@cable_id", id);							        //cable id
                dbmgr.ExecuteReader(CommandType.Text, qryString);					        //execute query
                while (dbmgr.DataReader.Read())
                {
                    item = new CompCab();											        //create new item
                    item = (CompCab)FetchObject(item, p, dbmgr);
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
            _cable.componentlist = list;												    //update item list
        }
        
        //private void DeleteComponent(object obj, IDBManager dbmgr)
        //{
        //    CompCab item = (CompCab)obj;													//cast object to proper item type
        //    try
        //    {
        //        dbmgr.Open();															    //open database
        //        dbmgr.CreateParameters(2);												    //create required paramaters
        //        dbmgr.AddParameters(0, "@comp_id", item.comp_id);
        //        dbmgr.AddParameters(1, "@cable_id", item.cable_id);
        //        dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPCAB_d");	    //execute stored procedure
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        dbmgr.Dispose();
        //    }
        //}
        
        #endregion

        #region crd power

        private void FetchCRDPowerList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCRDPOWERCAB cc WHERE cc.CABLE_ID = @cable_id ORDER BY cc.POWER, cc.BKRFUSE";
            PSLoad item = new PSLoad();											        
            ArrayList list = new ArrayList();										        
            try
            {
                PropertyInfo[] p = item.GetType().GetProperties();					        
                dbmgr.Open();														        
                dbmgr.CreateParameters(1);											        
                dbmgr.AddParameters(0, "@cable_id", id);							        
                dbmgr.ExecuteReader(CommandType.Text, qryString);					        
                while (dbmgr.DataReader.Read())
                {
                    item = new PSLoad();											        
                    item = (PSLoad)FetchObject(item, p, dbmgr);
                    list.Add(item);													        
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
            _cable.crdpowerlist = list;						    
        }
        #endregion

        #region drawing

        private void FetchDrawingList(int id, IDBManager dbmgr)
        {
            string qryString;

            switch (_user.plant)
            {
                case "ANO":
                    qryString = "SELECT * FROM viewCOMPCABDWGS cd WHERE cd.CABLE_ID = @cable_id ORDER BY DWG_REF";
                    break;
                default:
                    qryString = "SELECT * FROM viewCOMPCABDWGS cd WHERE cd.CABLE_ID = @cable_id AND cd.DWGTYPE_DESC = 'Other' ORDER BY cd.DWG_REF, cd.DWG_REV";
                    break;
            }
            
            CabDwg item = new CabDwg();											            // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects
            try
            {
                PropertyInfo[] p = item.GetType().GetProperties();					        // get property info for item
                dbmgr.Open();														        // open database manager
                dbmgr.CreateParameters(1);											        // create parameters
                dbmgr.AddParameters(0, "@cable_id", id);								    // cable id
                dbmgr.ExecuteReader(CommandType.Text, qryString);					        // execute query
                
                while (dbmgr.DataReader.Read())
                {
                    item = new CabDwg();											        // create new item
                    item = (CabDwg)FetchObject(item, p, dbmgr);
                    list.Add(item);													        // add item to the ArrayList
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
            _cable.drawinglist = list;												        // update item list
        }
        
        private void DeleteDrawing(object obj, IDBManager dbmgr)
        {
            CabDwg item = (CabDwg)obj;  												    //cast object to proper item type

            int dwgtype;
            switch (_user.plant)
            {
                case "ANO":
                    dwgtype = item.dwgtype_id;
                    break;
                default:
                    dwgtype = 4;
                    break;
            }
            
            try
            {
                dbmgr.Open();															    //open database
                dbmgr.CreateParameters(4);												    //create required paramaters
                dbmgr.AddParameters(0, "@cable_id", item.cable_id);
                dbmgr.AddParameters(1, "@comp_id", item.comp_id);
                dbmgr.AddParameters(2, "@dwg_id", item.dwg_id);
                dbmgr.AddParameters(3, "@dwgtype_id", dwgtype);                                   //other drawing type
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPCABDWGS_d");    //execute stored procedure
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
        
        private void UpdateDrawing(object obj, IDBManager dbmgr)
        {

            CabDwg item = (CabDwg)obj;										    		    //cast object to proper item type
            
            int dwgtype;
            switch (_user.plant)
            {
                case "ANO":
                    dwgtype = item.dwgtype_id;
                    break;
                default:
                    dwgtype = 4;
                    break;
            }
            
            try
            {
                dbmgr.Open();															    //open database
                dbmgr.CreateParameters(5);												    //create required paramaters
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@cable_id", item.cable_id);
                dbmgr.AddParameters(2, "@dwg_id", item.dwg_id);
                dbmgr.AddParameters(3, "@dwgtype_id", dwgtype);                                   //other drawing type
                dbmgr.AddParameters(4, "@dwg_rev", item.dwg_rev);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPCABDWGS_u");    //execute stored procedure
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

        #region route
        
        private void FetchRouteList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCABROUTE_PLANT cr WHERE cr.CABLE_ID = @cable_id ORDER BY cr.SEQ, cr.NODE";
            CabRoute item = new CabRoute();											        //create new object type to be able to get property info
            ArrayList list = new ArrayList();										        //create new ArrayList to house objects
            try
            {
                PropertyInfo[] p = item.GetType().GetProperties();					        //get property info for item
                dbmgr.Open();														        //open database manager
                dbmgr.CreateParameters(1);											        //create parameters
                dbmgr.AddParameters(0, "@cable_id", id);								    //cable id
                dbmgr.ExecuteReader(CommandType.Text, qryString);					        //execute query
                while (dbmgr.DataReader.Read())
                {
                    item = new CabRoute();											        //create new item
                    item = (CabRoute)FetchObject(item, p, dbmgr);
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
            _cable.routelist = list;												        //update item list
        }
        
        private void FetchRoutechgList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCABROUTE_CHGS cr WHERE cr.CABLE_ID = @cable_id ORDER BY cr.SEQ, cr.NODE";
            CabRoute item = new CabRoute();											        // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects
            
            try
            {
                PropertyInfo[] p = item.GetType().GetProperties();					        // get property info for item
                dbmgr.Open();														        // open database manager
                dbmgr.CreateParameters(1);											        // create parameters
                dbmgr.AddParameters(0, "@cable_id", id);								    // cable id
                dbmgr.ExecuteReader(CommandType.Text, qryString);					        // execute query

                while (dbmgr.DataReader.Read())
                {
                    item = new CabRoute();											        // create new item
                    item = (CabRoute)FetchObject(item, p, dbmgr);
                    list.Add(item);													        // add item to the ArrayList
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
            _cable.routechglist = list;												        // update item list
        }
        
        private void DeleteRoutechg(object obj, IDBManager dbmgr)
        {
            CabRoute item = (CabRoute)obj;													// cast object to proper item type
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(4);												    // create required paramaters
                dbmgr.AddParameters(0, "@cable_id", item.cable_id);
                dbmgr.AddParameters(1, "@seq", item.seq);
                dbmgr.AddParameters(2, "@node_id", item.node_id);
                dbmgr.AddParameters(3, "@fz_id", item.fz_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.CABROUTE_CHGS_d");  // execute stored procedure
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
        
        private void UpdateRoutechg(object obj, IDBManager dbmgr)
        {
            CabRoute item = (CabRoute)obj;									    		    // cast object to proper item type
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(8);												    // create required paramaters
                dbmgr.AddParameters(0, "@cable_id", item.cable_id);
                dbmgr.AddParameters(1, "@seq", item.seq);
                dbmgr.AddParameters(2, "@node_id", item.node_id);
                dbmgr.AddParameters(3, "@fz_id", item.fz_id);
                dbmgr.AddParameters(4, "@add_del", item.add_del);
                dbmgr.AddParameters(5, "@dwg_id", item.dwg_id);
                dbmgr.AddParameters(6, "@dwg_rev", item.dwg_rev);
                dbmgr.AddParameters(7, "@col_ref", item.col_ref);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.CABROUTE_CHGS_u");  // execute stored procedure
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
