using System;
using System.Data;
using System.Collections;
using System.Reflection;

using FRIAS.Common;
using FRIAS.Common.Entity;
using DL.Data;

namespace FRIAS.BL.Managers
{
    public class DrawingManager : TotalManager, IFactoryManager
    {
        private User _user;
        private IDBManager _dbmgr;                                                          // database manager
        private Drawing _drawing;                                                           // instance of primary object
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
                id = ((Drawing)obj).dwg_id;
                type = "all";                                                               //object is  drawing and get all information
            }

            _drawing = new Drawing();
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
                        return _drawing.componentlist;
                    case "cable":
                        FetchCableList(id, _dbmgr);
                        return _drawing.cablelist;
                    case "routeloca":
                        FetchRoutelocaList(id, _dbmgr);
                        return _drawing.routelocalist;
                    default:
                        FetchDrawing(id, _dbmgr);
                        FetchComponentList(id, _dbmgr);
                        FetchCableList(id, _dbmgr);
                        FetchRoutelocaList(id, _dbmgr);
                        return _drawing;
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
            DataSet ds = new DataSet();                                                     //create new dataset
            string qryString = "SELECT * FROM viewDWGLIST ORDER BY DWG_REF";                //create query string

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
                case "FRIAS.Common.Entity.Drawing":
                    DeleteDrawing(obj, _dbmgr);
                    break;
            }
        }
        
        public void Update(object obj)
        {
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;
            string type = obj.GetType().ToString();										    //get the type of object
            
            switch (type)																    //update respective object
            {
                case "FRIAS.Common.Entity.Drawing":
                    UpdateDrawing(obj, _dbmgr);
                    break;
            }
        }
        
        #endregion

        #region drawing
        
        private void FetchDrawing(int id, IDBManager dbmgr)
        {
            string qryLocal = "SELECT * FROM viewDWGLIST WHERE DWG_ID=@id";

            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryLocal);
            if (dbmgr.DataReader.Read())
            {
                // get properties of object and fetch object
                PropertyInfo[] p = _drawing.GetType().GetProperties();
                _drawing = (Drawing)FetchObject(_drawing, p, dbmgr);
            }

            dbmgr.CloseReader();
        }

        private void DeleteDrawing(object obj, IDBManager dbmgr)
        {
            Drawing item = (Drawing)obj;
            
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(2);
                dbmgr.AddParameters(0, "@dwg_id", item.dwg_id);
                dbmgr.AddParameters(1, "@error_num", 0, true);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.DWGLIST_d");
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
            Drawing item = (Drawing)obj;
            System.Data.Common.DbParameter param;
            
            try
            {
                dbmgr.Open();
                dbmgr.BeginTransaction();

                // check to see if new object
                if (item.dwg_id == 0)
                {
                    // if id is null then new object
                    dbmgr.CreateParameters(3);
                    dbmgr.AddParameters(0, "@id", 0, true);
                    dbmgr.AddParameters(1, "@dwg_ref", item.dwg_ref);
                    dbmgr.AddParameters(2, "@dwg_rev", item.dwg_rev);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.DWGLIST_i");

                    // get item id
                    param = (System.Data.Common.DbParameter)dbmgr.Parameters.GetValue(0);
                    item.dwg_id = Convert.ToInt32(param.Value);
                }

                // otherwise update existing object
                else
                {
                    dbmgr.CreateParameters(5);
                    dbmgr.AddParameters(0, "@dwg_id", item.dwg_id);
                    dbmgr.AddParameters(1, "@dwg_ref", item.dwg_ref);
                    dbmgr.AddParameters(2, "@dwg_rev", item.dwg_rev);
                    dbmgr.AddParameters(3, "@dwg_type", item.dwg_type);
                    dbmgr.AddParameters(4, "@dwg_desc", item.dwg_desc);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.DWGLIST_u");
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

        #region component
        
        private void FetchComponentList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCOMPDWGS cd WHERE cd.DWG_ID = @dwg_id ORDER BY cd.COMP";
            CompDwg item = new CompDwg();											        // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item

            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@dwg_id", id);							                // drawing id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new CompDwg();											            // create new item
                item = (CompDwg)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _drawing.componentlist = list;												    // update object item list
        }
        
        #endregion

        #region cable
        
        private void FetchCableList(int id, IDBManager dbmgr)
        {
            string qryString;

            switch (_user.plant)
            {
                case "ANO":
                    qryString = "SELECT cd.COMP_ID,  " +
                        " COMP = CASE WHEN c.CHKD_DATE IS NULL AND c.CABLE_CODE LIKE '%YES%' THEN 'R' ELSE '-' END, " +
                        " cd.CABLE_ID, cd.CABLE, cd.DWG_ID, cd.DWG_REF, cd.DWG_REV, cd.DWGTYPE_ID " +
                        " FROM viewCOMPCABDWGS AS cd " +
                        " LEFT OUTER JOIN viewCABLIST c ON c.CABLE_ID = cd.CABLE_ID " +
                        " WHERE cd.DWG_ID = @dwg_id " +
                        " ORDER BY cd.CABLE";
                    break;
                default:
                    qryString = "SELECT cd.*, '' AS COMP FROM viewCABDWGS AS cd " +
                        "INNER JOIN viewCABLIST_USED AS c ON c.CABLE_ID = cd.CABLE_ID " +
                        "WHERE cd.DWG_ID = @dwg_id ORDER BY cd.CABLE";
                    break;
            }
            
            CabDwg item = new CabDwg();											            // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
                
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@dwg_id", id);							                // drawing id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
                
            while (dbmgr.DataReader.Read())
            {
                item = new CabDwg();											            // create new item
                item = (CabDwg)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _drawing.cablelist = list;												        // update object item list
        }
        
        #endregion

        #region routeloca
        
        private void FetchRoutelocaList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewROUTELOCADWGS_ALL rd WHERE rd.DWG_ID = @dwg_id ORDER BY rd.NODE";
            RoutelocaDwg item = new RoutelocaDwg();											// create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item

            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@dwg_id", id);							                // drawing id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new RoutelocaDwg();											        // create new item
                item = (RoutelocaDwg)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _drawing.routelocalist = list;												    // update object item list
        }
        
        #endregion
    }
}
