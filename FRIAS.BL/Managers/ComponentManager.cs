using System;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Web.UI.WebControls;
using System.Reflection;
using FRIAS.BL.Interfaces;
using FRIAS.Common;
using FRIAS.Common.Entity;
using DL.Data;

namespace FRIAS.BL.Managers
{
    // class handles transactions pertaining to Component Entity
    public class ComponentManager : TotalManager, IComponentManager
    {
        private User _user;
        private IDBManager _dbmgr;												            // database manager
        private Component _comp;														    // instance of primary object
        
        private object _oldobj;                                                             // old object, used for tracking history
        private string[] _fieldlist;                                                        // list of fields whose control appears in web form


        private const string OBJECT_NAMESPACE = "FRIAS.Common.Entity";
        private const string SYSTEM_USER = "System Generated";
        private Assembly _assembly;

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

        #region Instance Method

        public ComponentManager()
        {
            loadAssembly();
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

        }

        #endregion


        #region public methods

        // fetches object and object items
        // user has option of passing object itself OR a string with id and type delimited by '|'
        public object Fetch(object obj)
        {
            int id = 0;
            string name = "";
            string type = "all";

            string objtype = obj.GetType().ToString();
            if (objtype == "System.String")
            {
                // if the object type is a string then extract the id and type
                string[] strTemp = ((string)obj).Split(new char[] { '|' });
                type = strTemp[1];
                if (type == "pricomp")
                    name = strTemp[0];
                else
                    id = Convert.ToInt32(strTemp[0]);
            }
            else
                id = ((Component)obj).comp_id;

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
                    case "drawing":
                        FetchDrawingList(id, _dbmgr);
                        return _comp.drawinglist;
                    case "power":
                        FetchPowerList(id, _dbmgr);
                        return _comp.powerlist;
                    case "subcomp":
                        FetchSubcompList(id, _dbmgr);
                        return _comp.subcomplist;
                    case "pricomp":
                        FetchPriCompList(name, _dbmgr);
                        return _comp.pricomplist;
                    case "interlock":
                        FetchIncomingIntlkList(id, _dbmgr);
                        return _comp.incomingintlklist;
                    case "cable":
                        FetchCableList(id, _dbmgr);
                        return _comp.cablelist;
                    case "be":
                        FetchBEList(id, _dbmgr);
                        return _comp.belist;
                    default:
                        FetchComponent(id, _dbmgr);
                        FetchDrawingList(id, _dbmgr);
                        FetchSubcompList(id, _dbmgr);
                        FetchPowerList(id, _dbmgr);
                        FetchLoadList(id, _dbmgr);
                        FetchIncomingIntlkList(id, _dbmgr);
                        FetchOutgoingIntlkList(id, _dbmgr);
                        FetchCableList(id, _dbmgr);
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

        /// <summary>
        /// Fetches Dataset
        /// </summary>
        /// <param name="criteria">Prep_Only, Prep and Chkd, Show All</param>
        /// <returns>Dataset</returns>
        public DataSet FetchDataSet(string criteria)
        {
            DataSet ds = new DataSet();
            string qryString;
            switch (criteria)
            {
                case "Prep Only":
                    qryString = "SELECT * FROM viewCOMPLIST_LITE WHERE PREP_DATE IS NOT NULL AND CHKD_DATE IS NULL ORDER BY COMP";
                    break;
                case "Prep and Chkd":
                    qryString = "SELECT * FROM viewCOMPLIST_LITE WHERE PREP_DATE IS NOT NULL AND CHKD_DATE IS NOT NULL ORDER BY COMP";
                    break;
                case "Show All":
                default:
                    qryString = "SELECT * FROM viewCOMPLIST_LITE ORDER BY COMP";
                    break;
            }

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

        // fetches a arraylist
        // used to populate combolists with value (id) and text
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
           
           // determine type of object
           string type = obj.GetType().ToString();
           switch (type)
           {
               case "FRIAS.Common.Entity.Component":
                   DeleteComponent(obj, _dbmgr);
                   break;
               case "FRIAS.Common.Entity.CompDwg":
                   DeleteDrawing(obj, _dbmgr);
                   break;
               case "FRIAS.Common.Entity.Subcomp":
                   DeleteSubcomp(obj, _dbmgr);
                   break;
               case "FRIAS.Common.Entity.CompPwr":
                   DeletePower(obj, _dbmgr);
                   break;
               case "FRIAS.Common.Entity.CompIntlk":
                   DeleteInterlock(obj, _dbmgr);
                   break;
               case "FRIAS.Common.Entity.CompCab":
                   DeleteCable(obj, _dbmgr);
                   break;
           }

        }

        // updates/adds given object or object item
        public void Update(object obj)
        {
            // create database connection
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;
            
            // determine type of object
            string type = obj.GetType().ToString();
            switch (type)
            {
                case "FRIAS.Common.Entity.Component":
                    UpdateComponent(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.CompDwg":
                    UpdateDrawing(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.Subcomp":
                    UpdateSubcomp(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.CompPwr":
                    UpdatePower(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.CompIntlk":
                    UpdateInterlock(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.CompCab":
                    UpdateCable(obj, _dbmgr);
                    break;
            }

        }

        #endregion

        #region component

        // procedure fetches a component with a given id
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

        // deletes component
        private void DeleteComponent(object obj, IDBManager dbmgr)
        {
            Component item = (Component)obj;
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(2);
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@error_num", 0, true);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPLIST_d");

                // save to history, make current object null
                SaveHistory(null, _oldobj, item.argUser.name, "COMPLIST", item.comp, item.comp, dbmgr, _fieldlist);
                
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

        // updates component
		private void UpdateComponent(object obj, IDBManager dbmgr)
		{
			Component item = (Component)obj;
            System.Data.Common.DbParameter new_comp_id;
            
            try
			{
                dbmgr.Open();
                dbmgr.BeginTransaction();

                // check to see if new object
                if (item.comp_id == 0)
                {
                    // if id is null then create new object
                    dbmgr.CreateParameters(11);
                    dbmgr.AddParameters(0, "@id", 0, true);
                    dbmgr.AddParameters(1, "@comp", item.comp);
                    dbmgr.AddParameters(2, "@pri_sub", item.pri_sub);
                    dbmgr.AddParameters(3, "@unit_id", item.unit_id);
                    dbmgr.AddParameters(4, "@sys_id", item.sys_id);
                    dbmgr.AddParameters(5, "@comp_type_id", item.comp_type_id);
                    dbmgr.AddParameters(6, "@comp_desc", item.comp_desc);
                    dbmgr.AddParameters(7, "@ssd_req", item.ssd_req);
                    dbmgr.AddParameters(8, "@pra_req", item.pra_req);
                    dbmgr.AddParameters(9, "@npo_req", item.npo_req);
                    dbmgr.AddParameters(10, "@cfp_req", item.cfp_req);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPLIST_i");
                    
                    // get item id
                    new_comp_id = (System.Data.Common.DbParameter)dbmgr.Parameters.GetValue(0);
                    item.comp_id = Convert.ToInt32(new_comp_id.Value);

                    // set old object to null to prevent it from taking value from existing page
                    SaveHistory(item, null, item.argUser.name, "COMPLIST", item.comp, item.comp, dbmgr, null);
                }

                // otherwise updating existing object
                else
                {
                    // update COMPLIST table
                    dbmgr.CreateParameters(28);
                    dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                    dbmgr.AddParameters(1, "@pri_sub", item.pri_sub);
                    dbmgr.AddParameters(2, "@unit_id", item.unit_id);
                    dbmgr.AddParameters(3, "@sys_id", item.sys_id);
                    dbmgr.AddParameters(4, "@train_id", item.train_id);
                    dbmgr.AddParameters(5, "@comp_type_id", item.comp_type_id);
                    dbmgr.AddParameters(6, "@comp", item.comp);
                    dbmgr.AddParameters(7, "@comp_desc", item.comp_desc);
                    dbmgr.AddParameters(8, "@np_id", item.np_id);
                    dbmgr.AddParameters(9, "@hsbp_id", item.hsbp_id);
                    dbmgr.AddParameters(10, "@hsp_id", item.hsp_id);
                    dbmgr.AddParameters(11, "@csp_id", item.csp_id);
                    dbmgr.AddParameters(12, "@prap_id", item.prap_id);
                    dbmgr.AddParameters(13, "@cfpp_id", item.cfpp_id);
                    dbmgr.AddParameters(14, "@npop_id", item.npop_id);
                    dbmgr.AddParameters(15, "@npo_np_id", item.npo_np_id);
                    dbmgr.AddParameters(16, "@fail_elect_id", item.fail_elect_id);
                    dbmgr.AddParameters(17, "@fail_air_id", item.fail_air_id);
                    dbmgr.AddParameters(18, "@hi_lo", item.hi_lo);
                    dbmgr.AddParameters(19, "@ssd_req", item.ssd_req);
                    dbmgr.AddParameters(20, "@pra_req", item.pra_req);
                    dbmgr.AddParameters(21, "@npo_req", item.npo_req);
                    dbmgr.AddParameters(22, "@cfp_req", item.cfp_req);
                    dbmgr.AddParameters(23, "@nsca_req", item.nsca_req);
                    dbmgr.AddParameters(24, "@method_id", item.method_id);
                    dbmgr.AddParameters(25, "@box_id", item.box_id);
                    dbmgr.AddParameters(26, "@comment", item.comment);
                    dbmgr.AddParameters(27, "@error_num", 0, true);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPLIST_u");

                    // update master drawings
                    UpdateMasterDrawing(dbmgr, item, item.ee_id, item.ee_ref, item.ee_rev, item.ee_cp, 1);      // elementary
                    UpdateMasterDrawing(dbmgr, item, item.pid_id, item.pid_ref, item.pid_rev, item.pid_cp, 2);   // p&id
                    UpdateMasterDrawing(dbmgr, item, item.ol_id, item.ol_ref, item.ol_rev, item.ol_cp, 3);      // one-line

                    // update status
                    UpdateStatus(dbmgr, item, 1, item.prep_by, item.prep_date);
                    UpdateStatus(dbmgr, item, 5, item.chkd_by, item.chkd_date);

                    // save to history
                    SaveHistory(item, _oldobj, item.argUser.name, "COMPLIST", item.comp, item.comp, dbmgr, _fieldlist);
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

        private void UpdateMasterDrawing(IDBManager dbmgr, Component item, int dwg_id, string dwg_ref, string dwg_rev, string dwg_cp, int dwg_type)
        {
            // if drawing ref is empty or N/A then delete
            if (dwg_ref == "")
            {
                dbmgr.CreateParameters(3);
                dbmgr.BeginTransaction();
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@dwg_id", dwg_id);
                dbmgr.AddParameters(2, "@dwgtype_id", dwg_type);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPDWGS_d");
            }
            // otherwise update
            else
            {
                // add updated/new
                dbmgr.CreateParameters(5);
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@dwg_id", dwg_id);
                dbmgr.AddParameters(2, "@dwgtype_id", dwg_type);
                dbmgr.AddParameters(3, "@dwg_rev", dwg_rev);
                dbmgr.AddParameters(4, "@dwg_cp", dwg_cp);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPDWGS_MASTER_u");
            }
        }

        private void UpdateStatus(IDBManager dbmgr, Component item, int statustype_id, string user_id, DateTime status_date)
        {
            if ((user_id == "N/A") || (user_id == ""))
            {
                // delete status
                dbmgr.CreateParameters(2);
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@statustype_id", statustype_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPSTATUS_d");
            }
            else
            {

                if (status_date == Convert.ToDateTime(null))
                {
                    // update status
                    dbmgr.CreateParameters(3);
                    dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                    dbmgr.AddParameters(1, "@statustype_id", statustype_id);
                    dbmgr.AddParameters(2, "@user_id", user_id);
                }
                else
                {
                    // update status
                    dbmgr.CreateParameters(4);
                    dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                    dbmgr.AddParameters(1, "@statustype_id", statustype_id);
                    dbmgr.AddParameters(2, "@user_id", user_id);
                    dbmgr.AddParameters(3, "@status_date", status_date);
                }
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPSTATUS_u");
            }
        }

        #endregion

        #region drawing

        public ArrayList FetchPagedList(int id, object type, int startNum, int endNum)
        {
            string sql = FetchStoredProc(type);
            ArrayList list = new ArrayList();
            object item;

            PropertyInfo[] p = type.GetType().GetProperties();					            // get property info for item

            try
            {
                _dbmgr.Open();

                _dbmgr.CreateParameters(3); // create parameters
                _dbmgr.AddParameters(0, "@comp_id", id); // component id
                _dbmgr.AddParameters(1, "@start_num", startNum); // start row num
                _dbmgr.AddParameters(2, "@end_num", endNum); // end row num
                _dbmgr.ExecuteReader(CommandType.Text, sql); // execute query

                while (_dbmgr.DataReader.Read())
                {
                    var newObject = _assembly.CreateInstance(findAssemblyType(type.GetType().FullName).FullName);
                    item = (CompDwg)FetchObject(type, p, _dbmgr);
                    list.Add(item);													            // add item to the ArrayList
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                _dbmgr.CloseReader();
            }

            return list;

        }

        // procedure fetches drawings given a component id
        private void FetchDrawingList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCOMPDWGS cd WHERE cd.COMP_ID = @comp_id AND cd.DWGTYPE_desc = 'Other' ORDER BY cd.DWG_REF, cd.DWG_REV";
            //string qryString = "SELECT * FROM viewCOMPDWGS cd WHERE cd.COMP_ID = @comp_id AND DWGTYPE_ID <= 4 ORDER BY cd.DWGTYPE_ID, cd.DWG_REF, cd.DWG_REV";
            
            CompDwg item = new CompDwg();
            ArrayList list = new ArrayList();
            
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@comp_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new CompDwg();											            // create new item
                item = (CompDwg)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _comp.drawinglist = list;												        // update object item list

        }

        // deletes drawing
        private void DeleteDrawing(object obj, IDBManager dbmgr)
        {
            CompDwg item = (CompDwg)obj;

            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(3);												    // create required paramaters
                dbmgr.BeginTransaction();
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@dwg_id", item.dwg_id);
                dbmgr.AddParameters(2, "@dwgtype_id", item.dwgtype_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPDWGS_d");	    // execute stored procedure

                // save history, make current item null
                SaveHistory(null, _oldobj, item.argUser.name, "COMPDWGS", item.dwg_ref, item.comp, dbmgr, _fieldlist);

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

        // updates drawing
        private void UpdateDrawing(object obj, IDBManager dbmgr)
		{
			CompDwg item = (CompDwg)obj;

			try
			{
                dbmgr.Open();															    // open database
				dbmgr.CreateParameters(5);												    // create required paramaters
                dbmgr.BeginTransaction();
				dbmgr.AddParameters(0, "@comp_id", item.comp_id);
				dbmgr.AddParameters(1, "@dwg_id", item.dwg_id);
				dbmgr.AddParameters(2, "@dwgtype_id", 4);                                   // other drawing
				dbmgr.AddParameters(3, "@dwg_rev", item.dwg_rev);
                dbmgr.AddParameters(4, "@dwg_cp", item.dwg_cp);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPDWGS_u");	    // execute stored procedure
                
                // save history
                SaveHistory(item, _oldobj, item.argUser.name, "COMPDWGS", item.dwg_ref, item.comp, dbmgr, _fieldlist);
                
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

        private void DeleteSubcomp(object obj, IDBManager dbmgr)
        {
            Subcomp item = (Subcomp)obj;												    // cast object to proper item type
            
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.BeginTransaction();
                dbmgr.CreateParameters(2);												    // create required paramaters
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@subcomp", item.subcomp);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.SUBCOMPS_d");	    // execute stored procedure
                
                // save history, set current item to null
                SaveHistory(null, _oldobj, item.argUser.name, "SUBCOMPS", item.subcomp, item.comp, dbmgr, _fieldlist);
                
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

        // updates subcomp
        private void UpdateSubcomp(object obj, IDBManager dbmgr)
        {
            Subcomp item = (Subcomp)obj;
            
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(3);												    // create required paramaters
                dbmgr.BeginTransaction();
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@subcomp", item.subcomp);
                dbmgr.AddParameters(2, "@note", item.note);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.SUBCOMPS_u");	    // execute stored procedure
                
                // save history
                SaveHistory(item, _oldobj, item.argUser.name, "SUBCOMPS", item.subcomp, item.comp, dbmgr, _fieldlist);
                
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

        #region pricomponent

        // procedure fetches subcomponent list
        private void FetchPriCompList(string name, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewSUBCOMPS s WHERE s.SUBCOMP = @subcomp ORDER BY s.COMP";

            Subcomp item = new Subcomp();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@subcomp", name);								        // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new Subcomp();											            // create new item
                item = (Subcomp)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _comp.pricomplist = list;												        // update component item list

        }

        #endregion

        #region power

        // procedure fetches power list
        private void FetchPowerList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCOMPPWR cp WHERE cp.COMP_ID = @comp_id ORDER BY cp.POWER";
            
            CompPwr item = new CompPwr();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@comp_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new CompPwr();											            // create new item
                item = (CompPwr)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _comp.powerlist = list;													        // update component item list

        }

        // procedure fetches load list
        private void FetchLoadList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCOMPPWR cp WHERE cp.POWER_ID = @comp_id ORDER BY cp.COMP";
            
            CompPwr item = new CompPwr();
            ArrayList list = new ArrayList();
            
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@comp_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new CompPwr();											            // create new item
                item = (CompPwr)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _comp.loadlist = list;													        // update component item list

        }

        // deletes power source
        private void DeletePower(object obj, IDBManager dbmgr)
        {
            CompPwr item = (CompPwr)obj;

            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(2);												    // create required paramaters
                dbmgr.BeginTransaction();
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@power_id", item.power_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPPWR_d");	    // execute stored procedure
                
                // save history, set current item to null
                SaveHistory(null, _oldobj, item.argUser.name, "COMPPWR", item.power, item.comp, dbmgr, _fieldlist);
                
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

        // updates power
        private void UpdatePower(object obj, IDBManager dbmgr)
		{
			CompPwr item = (CompPwr)obj;
			
            try
			{
                dbmgr.Open();															    // open database
				dbmgr.CreateParameters(8);												    // create required paramaters
                dbmgr.BeginTransaction();
				dbmgr.AddParameters(0, "@comp_id", item.comp_id);
				dbmgr.AddParameters(1, "@power_id", item.power_id);
                dbmgr.AddParameters(2, "@note", item.note);
                dbmgr.AddParameters(3, "@alt_pwr", item.alt_pwr);
                dbmgr.AddParameters(4, "@ssd_req", item.ssd_req);
                dbmgr.AddParameters(5, "@pra_req", item.pra_req);
                dbmgr.AddParameters(6, "@npo_req", item.npo_req);
                dbmgr.AddParameters(7, "@cfp_req", item.cfp_req);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPPWR_u");	    // execute stored procedure
                
                // save history
                SaveHistory(item, _oldobj, item.argUser.name, "COMPPWR", item.power, item.comp, dbmgr, _fieldlist);
                
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

        // procedure fetches outgoing interlocks
        private void FetchOutgoingIntlkList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCOMPINTLKS ci WHERE ci.INTLK_ID = @comp_id ORDER BY ci.COMP";
            
            CompIntlk item = new CompIntlk();
            ArrayList list = new ArrayList();
            
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create required parameters
            dbmgr.AddParameters(0, "@comp_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new CompIntlk();											            // create new item
                item = (CompIntlk)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _comp.outgoingintlklist = list;											        // update component item list

        }

        // deletes incoming interlock
        private void DeleteInterlock(object obj, IDBManager dbmgr)
        {
            CompIntlk item = (CompIntlk)obj;
            
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(4);												    // create required paramaters
                dbmgr.BeginTransaction();
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@intlk_id", item.intlk_id);
                dbmgr.AddParameters(2, "@device", item.device);
                dbmgr.AddParameters(3, "@contacts", item.contacts);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPINTLKS_d");	    // execute stored procedure
                
                // save history, set current item to null
                SaveHistory(null, _oldobj, item.argUser.name, "COMPINTLKS", item.intlk, item.comp, dbmgr, _fieldlist);
                
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

        // porcedure update incominginterlock
        private void UpdateInterlock(object obj, IDBManager dbmgr)
		{
			CompIntlk item = (CompIntlk)obj;
			
            try
			{
                dbmgr.Open();															    // open database
				dbmgr.CreateParameters(9);												    // create required paramaters
                dbmgr.BeginTransaction();
				dbmgr.AddParameters(0, "@comp_id", item.comp_id);
				dbmgr.AddParameters(1, "@intlk_id", item.intlk_id);
				dbmgr.AddParameters(2, "@device", item.device);
				dbmgr.AddParameters(3, "@contacts", item.contacts);
                dbmgr.AddParameters(4, "@note", item.note);
                dbmgr.AddParameters(5, "@ssd_req", item.ssd_req);
                dbmgr.AddParameters(6, "@pra_req", item.pra_req);
                dbmgr.AddParameters(7, "@npo_req", item.npo_req);
                dbmgr.AddParameters(8, "@cfp_req", item.cfp_req);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPINTLKS_u");	    // execute stored procedure
                
                // save hisotry
                SaveHistory(item, _oldobj, item.argUser.name, "COMPINTLKS", item.intlk, item.comp, dbmgr, _fieldlist);
                
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

        // deletes cable
        private void DeleteCable(object obj, IDBManager dbmgr)
		{
			CompCab item = (CompCab)obj;
			
            try
			{
                dbmgr.Open();															    // open database
				dbmgr.CreateParameters(2);												    // create required paramaters
                dbmgr.BeginTransaction();
				dbmgr.AddParameters(0, "@comp_id", item.comp_id);
				dbmgr.AddParameters(1, "@cable_id", item.cable_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPCAB_d");	    // execute stored procedure
                
                // save history, set current item to null
                SaveHistory(null, _oldobj, item.argUser.name, "COMPCAB", item.cable, item.comp, dbmgr, _fieldlist);
                
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

        // updates cable
        private void UpdateCable(object obj, IDBManager dbmgr)
        {
            CompCab item = (CompCab)obj;
            
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(11);												    // create required paramaters
                dbmgr.BeginTransaction();
                dbmgr.AddParameters(0, "@comp_id", item.comp_id);
                dbmgr.AddParameters(1, "@cable_id", item.cable_id);
                dbmgr.AddParameters(2, "@note", item.note);
                dbmgr.AddParameters(3, "@fault", item.fault);
                dbmgr.AddParameters(4, "@fault_type", item.fault_type);
                dbmgr.AddParameters(5, "@fr_dwg_rev", item.fr_dwg_rev);
                dbmgr.AddParameters(6, "@to_dwg_rev", item.to_dwg_rev);
                dbmgr.AddParameters(7, "@ssd_req", item.ssd_req);
                dbmgr.AddParameters(8, "@pra_req", item.pra_req);
                dbmgr.AddParameters(9, "@npo_req", item.npo_req);
                dbmgr.AddParameters(10, "@cfp_req", item.cfp_req);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.COMPCAB_u");	    //execute stored procedure
                
                // save history
                SaveHistory(item, _oldobj, item.argUser.name, "COMPCAB", item.cable, item.comp, dbmgr, _fieldlist);
                
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

        // creates matrix representing cable block diagram
        private void FetchCableBlockMatrix(object obj)
        {
            // get the component cable list
            ArrayList list = (ArrayList)obj;
            ArrayList nodes = new ArrayList();

            // get unique list of nodes
            foreach (CompCab c in list)
            {
                if (!nodes.Contains(c.fr_equip))
                    nodes.Add(c.fr_equip);
                if (!nodes.Contains(c.to_equip))
                    nodes.Add(c.to_equip);
            }
            
            // create a matrix c x n
            int[,] matrix = new int[list.Count, nodes.Count];

            // fill matrix with 1 indicating connection

        }

        #endregion

        #region basic event
        
        // procedure fetches basic events list
        private void FetchBEList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewCOMPBE cb WHERE cb.COMP_ID = @comp_id ORDER BY cb.BE";
            
            CompBE item = new CompBE();
            ArrayList list = new ArrayList();
            
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create required parameters
            dbmgr.AddParameters(0, "@comp_id", id);								            // component id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new CompBE();											            // create new item
                item = (CompBE)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _comp.belist = list;												            // update object item list

        }
        
        #endregion

        #region "Dan Stuff"

        private string FetchStoredProc(object item)
        {
            switch (item.GetType().FullName)
            {
                case "FRIAS.Common.Entity.Component":
                    return "COMPDWGPAGE_q";
                case "FRIAS.Common.Entity.CompDwg":
                    return "COMPDWGPAGE_q";
                case "FRIAS.Common.Entity.Subcomp":
                    return "COMPDWGPAGE_q";
                case "FRIAS.Common.Entity.CompPwr":
                    return "COMPDWGPAGE_q";
                case "FRIAS.Common.Entity.CompIntlk":
                    return "COMPDWGPAGE_q";
                case "FRIAS.Common.Entity.CompCab":
                    return "COMPDWGPAGE_q";
                default:
                    return string.Empty;
            }
        }

        private void loadAssembly()
        {
            const string METHOD_NM = "loadAssembly ";

            // _log.Debug(METHOD_NM);

            try
            {
                _assembly = Assembly.Load(OBJECT_NAMESPACE);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        private Type findAssemblyType(string className)
        {
            const string METHOD_NM = "findAssemblyType ";

            //_log.Debug(METHOD_NM);

            foreach (Type type in _assembly.GetTypes())
            {
                if (type.IsClass == true)
                {

                    if (type.FullName.EndsWith("." + className))
                    {
                        return type;
                    }
                }
            }

            return null;


        }

        #endregion
    }
}
