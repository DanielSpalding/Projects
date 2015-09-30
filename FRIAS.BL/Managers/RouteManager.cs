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
    public class RouteManager : TotalManager, IFactoryManager
    {
        private User _user;
        private IDBManager _dbmgr;                                                          // database manager
        private Route _route;                                                               // instance of primary object
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
                id = ((Route)obj).node_id;
                type = "all";                                                               // object is a cable and get all information
            }
            _route = new Route();                                                           // create new instance of object
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            try
            {
                _dbmgr.Open();
                
                switch (type)
                {
                    case "routeloca":
                        FetchRoutelocaList(id, _dbmgr);
                        return _route.routelocalist;
                    case "routelocadwg":
                        FetchRoutelocaList(id, _dbmgr);
                        FetchDrawingList(_route.routelocalist, _dbmgr);
                        return _route.drawinglist;
                    case "bdendpoint":
                        FetchBDEndpointList(id, _dbmgr);
                        return _route.bdendpointlist;
                    case "cable":
                        FetchCableList(id, _dbmgr);
                        return _route.cablelist;
                    default:
                        FetchRoute(id, _dbmgr);
                        FetchRoutelocaList(id, _dbmgr);
                        FetchBDEndpointList(id, _dbmgr);
                        FetchDrawingList(_route.routelocalist, _dbmgr);
                        FetchCableList(id, _dbmgr);
                        return _route;
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
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];

            DataSet ds = new DataSet();
            string qryString;

            switch (_user.plant)
            {
                case "BLN":
                case "TFAC":
                    qryString = "SELECT * FROM viewROUTELIST_FR ORDER BY NODE";
                    break;
                default:
                    qryString = "SELECT * FROM viewROUTELIST ORDER BY NODE";
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
            ArrayList list = new ArrayList();
            ListItem listItem = new ListItem();
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
                case "FRIAS.Common.Entity.Route":
                    DeleteRoute(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.RoutelocaDwg":
                    DeleteDrawing(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.Routeloca":
                    DeleteRouteloca(obj, _dbmgr);
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
                case "FRIAS.Common.Entity.Route":
                    UpdateRoute(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.Routeloca":
                    UpdateRouteloca(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.RoutelocaDwg":
                    UpdateDrawing(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.RouteBDEndpoint":
                    UpdateBDEndpoint(obj, _dbmgr);
                    break;
            }
        }

        #endregion

        #region route
        
        private void FetchRoute(int id, IDBManager dbmgr)
        {
            string qryLocal = "SELECT * FROM viewROUTELIST WHERE NODE_ID=@id ORDER BY NODE";
            
            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryLocal);
            if (dbmgr.DataReader.Read())
            {
                PropertyInfo[] p = _route.GetType().GetProperties();
                _route = (Route)FetchObject(_route, p, dbmgr);
            }

            dbmgr.CloseReader();
        }

        private void DeleteRoute(object obj, IDBManager dbmgr)
        {
            Route item = (Route)obj;
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(2);												    // create required paramaters
                dbmgr.AddParameters(0, "@node_id", item.node_id);
                dbmgr.AddParameters(1, "@error_num", 0, true);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.ROUTELIST_d");	    // execute stored procedure
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

        private void UpdateRoute(object obj, IDBManager dbmgr)
        {
            Route item = (Route)obj;
            System.Data.Common.DbParameter param;

            try
            {
                dbmgr.Open();
                dbmgr.BeginTransaction();

                // check to see if new object
                if (item.node_id == 0)
                {
                    // if id is null then new object
                    dbmgr.CreateParameters(2);
                    dbmgr.AddParameters(0, "@id", 0, true);
                    dbmgr.AddParameters(1, "@node", item.node);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.ROUTELIST_i");

                    // get item id
                    param = (System.Data.Common.DbParameter)dbmgr.Parameters.GetValue(0);
                    item.node_id = Convert.ToInt32(param.Value);
                }

                // otherwise update existing object
                else
                {
                    // update ROUTELIST table
                    dbmgr.CreateParameters(2);
                    dbmgr.AddParameters(0, "@node_id", item.node_id);
                    dbmgr.AddParameters(1, "@node", item.node);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.ROUTELIST_u");
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

        #region routeloca
        
        private void FetchRoutelocaList(int id, IDBManager dbmgr)
        {
            string qryString;
            Routeloca item = new Routeloca();
            ArrayList list = new ArrayList();
            PropertyInfo[] p = item.GetType().GetProperties();

            switch (_user.plant)
            {
                case "BLN":
                case "TFAC":
                    qryString = "SELECT * FROM viewROUTELOCA_FR rl WHERE rl.NODE_ID = @node_id ORDER BY rl.RM, rl.FZ";
                    break;
                default:
                    qryString = "SELECT * FROM viewROUTELOCA rl WHERE rl.NODE_ID = @node_id ORDER BY rl.RM, rl.FZ";
                    break;
            }
														        
            dbmgr.CreateParameters(1);											        
            dbmgr.AddParameters(0, "@node_id", id);								        
            dbmgr.ExecuteReader(CommandType.Text, qryString);					        
                
            while (dbmgr.DataReader.Read())
            {
                item = new Routeloca();											        
                item = (Routeloca)FetchObject(item, p, dbmgr);
                list.Add(item);													        
            }

            dbmgr.CloseReader();
            _route.routelocalist = list;
        }
        
        private void DeleteRouteloca(object obj, IDBManager dbmgr)
        {
            Routeloca item = (Routeloca)obj;  											        // cast object to proper item type
            try
            {
                dbmgr.Open();															        // open database
                dbmgr.CreateParameters(2);												        // create required paramaters
                dbmgr.AddParameters(0, "@node_id", item.node_id);
                
                switch (_user.locaType)
                {
                    case Constant.type_FR:
                        dbmgr.AddParameters(1, "@loca_id", item.rm_id);
                        break;
                    default:
                        dbmgr.AddParameters(1, "@loca_id", item.fz_id);
                        break;
                }

                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.ROUTELOCA_d");	        // execute stored procedure
                
                // Delete subsequent ROUTELOCADWGS
                dbmgr.CreateParameters(1);												        // create required paramaters
                dbmgr.AddParameters(0, "@nodeloca_id", item.nodeloca_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.ROUTELOCADWGS_ALL_d");  //execute stored procedure

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
        
        private void UpdateRouteloca(object obj, IDBManager dbmgr)
        {
            Routeloca item = (Routeloca)obj;							                        // cast object to proper item type
            try
            {
                dbmgr.Open();															        // open database
                dbmgr.CreateParameters(3);												        // create required paramaters
                dbmgr.AddParameters(0, "@node_id", item.node_id);

                switch (_user.locaType)
                {
                    case Constant.type_FR:
                        dbmgr.AddParameters(1, "@loca_id", item.rm_id);
                        break;
                    default:
                        dbmgr.AddParameters(1, "@loca_id", item.fz_id);
                        break;
                }

                dbmgr.AddParameters(2, "@locatype_id", _user.locaType);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.ROUTELOCA_u");          // execute stored procedure
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

        #region drawing
        
        // fetches drawings for every fz_id associated with a gived node_id
        // different from other fetches
        private void FetchDrawingList(ArrayList localist, IDBManager dbmgr)
        {
            if (localist.Count == 0)
                _route.drawinglist = null;
            else
            {
                string qryString = "SELECT * FROM viewROUTELOCADWGS rd WHERE rd.NODE_ID = @node_id AND rd.FZ_ID = @fz_id ORDER BY rd.DWG_REF";
                ArrayList list = new ArrayList();										    // create new ArrayList to house objects

                foreach (Routeloca litem in localist)
                {
                    RoutelocaDwg item = new RoutelocaDwg();							        // create new object type to be able to get property info
                    PropertyInfo[] p = item.GetType().GetProperties();					    // get property info for item

                    dbmgr.CreateParameters(2);											    // create parameters
                    dbmgr.AddParameters(0, "@node_id", litem.node_id);					    // node id
                    dbmgr.AddParameters(1, "@fz_id", litem.fz_id);						    // loca id
                    dbmgr.ExecuteReader(CommandType.Text, qryString);					    // execute query
                    
                    while (dbmgr.DataReader.Read())
                    {
                        item = new RoutelocaDwg();										    // create new item
                        item = (RoutelocaDwg)FetchObject(item, p, dbmgr);
                        list.Add(item);													    // add item to the ArrayList
                    }
                    dbmgr.CloseReader();
                }

                _route.drawinglist = list;										            // update item list
            }
        }
        
        private void DeleteDrawing(object obj, IDBManager dbmgr)
        {
            RoutelocaDwg item = (RoutelocaDwg)obj;  									    // cast object to proper item type
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(2);												    // create required paramaters
                dbmgr.AddParameters(0, "@nodeloca_id", item.nodeloca_id);
                dbmgr.AddParameters(1, "@dwg_id", item.dwg_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.ROUTELOCADWGS_d");  // execute stored procedure
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
            RoutelocaDwg item = (RoutelocaDwg)obj;							                // cast object to proper item type
            
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(5);												    // create required paramaters
                dbmgr.AddParameters(0, "@nodeloca_id", item.nodeloca_id);
                dbmgr.AddParameters(1, "@dwg_id", item.dwg_id);
                dbmgr.AddParameters(2, "@dwg_rev", item.dwg_rev);
                dbmgr.AddParameters(3, "@dwgtype_id", 4);                                   // "Other" drawing type
                dbmgr.AddParameters(4, "@col_ref", item.col_ref);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.ROUTELOCADWGS_u");  // execute stored procedure
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

        #region bdendpoint
        
        private void FetchBDEndpointList(int id, IDBManager dbmgr)
        {
            string bd_endpt = Get_BD_ENDPT(id, dbmgr);
            _route.bd_endpt = bd_endpt.Replace("'", "");

            if (_route.bd_endpt=="")
                _route.bdendpointlist=null;
            else
            {
                string qryString = "SELECT * FROM viewBDENDPOINTS e WHERE e.BD_ENDPT IN (" + bd_endpt + ") ORDER BY e.ENDPT";
                RouteBDEndpoint item = new RouteBDEndpoint();							    // create new object type to be able to get property info
                ArrayList list = new ArrayList();										    // create new ArrayList to house objects
                PropertyInfo[] p = item.GetType().GetProperties();					        // get property info for item

                dbmgr.ExecuteReader(CommandType.Text, qryString);					        // execute query

                while (dbmgr.DataReader.Read())
                {
                    item = new RouteBDEndpoint();										    // create new item
                    item = (RouteBDEndpoint)FetchObject(item, p, dbmgr);
                    list.Add(item);													        // add item to the ArrayList
                }

                dbmgr.CloseReader();
                _route.bdendpointlist = list;										        //update item list
            }
        }
        
        private string Get_BD_ENDPT(int id, IDBManager dbmgr)
        {
            string bd_endpt = "";
            string qryString;

            switch (_user.locaType)
            {
                case Constant.type_FR:
                    qryString = "SELECT * FROM viewBDENDPOINTS_FR AS e WHERE e.NODE_ID = @node_id ORDER BY e.BD_ENDPT";
                    break;
                default:
                    qryString = "SELECT * FROM viewBDENDPOINTS AS e WHERE e.NODE_ID = @node_id ORDER BY e.BD_ENDPT";
                    break;
            }
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@node_id", id);								            // node id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                if (bd_endpt == "")
                    bd_endpt = "'" + dbmgr.DataReader["BD_ENDPT"].ToString() + "'";
                else
                    bd_endpt = bd_endpt + ", " + "'" + dbmgr.DataReader["BD_ENDPT"].ToString() + "'";
            }

            dbmgr.CloseReader();
            return bd_endpt;
        }
        
        private void UpdateBDEndpoint(object obj, IDBManager dbmgr)
        {
            RouteBDEndpoint item = (RouteBDEndpoint)obj;							        // cast object to proper item type
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(2);												    // create required paramaters
                dbmgr.AddParameters(0, "@endpt", item.endpt);
                dbmgr.AddParameters(1, "@bdendpt", item.bd_endpt);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.BDENDPOINTS_u");    // execute stored procedure
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

        #region cable
        
        private void FetchCableList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCABROUTE_CHKD c WHERE c.NODE_ID = @node_id ORDER BY c.CABLE";
            CabRoute item = new CabRoute();							                        // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item

            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@node_id", id);								            // node id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new CabRoute();											            // create new item
                item = (CabRoute)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _route.cablelist = list;										                // update item list
        }
        
        #endregion

    }
}
