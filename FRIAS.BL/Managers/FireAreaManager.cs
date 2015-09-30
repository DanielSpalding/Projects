using System;
using System.Data;
using System.Collections;
using System.Reflection;

using FRIAS.Common;
using FRIAS.Common.Entity;
using DL.Data;

namespace FRIAS.BL.Managers
{
    public class FireAreaManager : TotalManager, IFactoryManager
    {
        private User _user;
        private IDBManager _dbmgr;                                                          // database manager
        private FireArea _firearea;                                                         // instance of primary object
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
            int fa_id;
            int comp_id = 0;
            string type;                                                                    // type of object passed
            string objtype = obj.GetType().ToString();                                      // get object type
            if (objtype == "System.String")
            {
                string[] strTemp = ((string)obj).Split(new char[] { '|' });                 // if the object type is a string then extract the id and type
                fa_id = Convert.ToInt32(strTemp[0]);
                type = strTemp[1];
                comp_id = Convert.ToInt32(strTemp[2]);
            }
            else
            {
                fa_id = ((FireArea)obj).fa_id;
                type = "all";                                                               // object is a cable and get all information
            }

            _firearea = new FireArea();
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            try
            {
                _dbmgr.Open();

                switch (type)
                {
                    //case "facompdisp":
                    //    FetchCompDispList(fa_id, _dbmgr);
                    //    return _firearea.compdisplist;
                    case "facabdisp":
                        FetchCabDispList(fa_id, comp_id, _dbmgr);
                        break;
                        //return _firearea.cabdisplist;
                    case "facompinfo":
                        FetchCabDispList(fa_id, comp_id, _dbmgr);
                        break;
                        //return _firearea;
                    case "faB3Table":
                        FetchFireArea(fa_id, _dbmgr);
                        FetchVFDRList(fa_id, _dbmgr);
                        FetchPGList(fa_id, _dbmgr);
                        FetchGENdocList(fa_id, _dbmgr);
                        FetchEEdocList(fa_id, _dbmgr);
                        FetchLICdocList(fa_id, _dbmgr);
                        FetchIgnitionList(fa_id, _dbmgr);
                        FetchProtectionList(fa_id, _dbmgr);
                        break;
                        //return _firearea;
                    case "all":
                        FetchFireArea(fa_id, _dbmgr);
                        FetchCompDispList(fa_id, _dbmgr);
                        FetchCascPowerList("SSD", fa_id, _dbmgr);
                        FetchCascIntlkList("SSD", fa_id, _dbmgr);
                        break;
                        //return _firearea;
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

            return _firearea;
        }

        public DataSet FetchDataSet(string criteria)
        {
            DataSet ds = new DataSet();
            string qryString = "SELECT * FROM viewFALIST ORDER BY FA";

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
            string qryString = "SELECT DISTINCT CABLE_ID, CABLE FROM viewFADATA_SSD WHERE FA_ID = @fa_id AND COMP_ID = @comp_id ORDER BY CABLE";
            ArrayList list = new ArrayList();

            string[] strTemp = ((string)param).Split(new char[] { '|' });
            int fa_id = Convert.ToInt32(strTemp[0]);
            int comp_id = Convert.ToInt32(strTemp[1]);

            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            try
            {
                // initialize
                if (initMsg.Length > 0)
                    list.Add(new ComboBoxItem(initMsg, "0"));

                _dbmgr.Open();
                _dbmgr.CreateParameters(2);
                _dbmgr.AddParameters(0, "@fa_id", fa_id);
                _dbmgr.AddParameters(1, "@comp_id", comp_id);
                _dbmgr.ExecuteReader(CommandType.Text, qryString);

                while (_dbmgr.DataReader.Read() == true)
                {
                    if (_dbmgr.DataReader.GetInt32(0) != 0)
                        list.Add(new ComboBoxItem(_dbmgr.DataReader.GetString(1), _dbmgr.DataReader.GetInt32(0).ToString())); ;
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
                case "FRIAS.Common.Entity.FireArea":
                    DeleteFireArea(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FACompDisp":
                    DeleteCompDisp(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FACabDisp":
                    DeleteCabDisp(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FAVFDR":
                    DeleteVFDR(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FADOC":
                    DeleteDocument(obj, _dbmgr);
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
                case "FRIAS.Common.Entity.FireArea":
                    UpdateFireArea(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FACompDisp":
                    UpdateCompDisp(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FACabDisp":
                    UpdateCabDisp(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FAVFDR":
                    UpdateVFDR(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FAPG":
                    UpdatePG(obj, _dbmgr);
                    break;
                case "FRIAS.Common.Entity.FADOC":
                    UpdateDocument(obj, _dbmgr);
                    break;
            }
        }

        #endregion

        #region firearea

        private void FetchFireArea(int id, IDBManager dbmgr)
        {
            string qryLocal = "SELECT * FROM viewFALIST WHERE FA_ID=@id ORDER BY FA";
            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryLocal);
            if (dbmgr.DataReader.Read())
            {
                PropertyInfo[] p = _firearea.GetType().GetProperties();
                _firearea = (FireArea)FetchObject(_firearea, p, dbmgr);
            }

            dbmgr.CloseReader();
        }

        private void DeleteFireArea(object obj, IDBManager dbmgr)
        {
            FireArea item = (FireArea)obj;											        //cast object to proper item type
            try
            {
                dbmgr.Open();															    //open database
                dbmgr.CreateParameters(1);												    //create required paramaters
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FALIST_d");	    //execute stored procedure
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

        private void UpdateFireArea(object obj, IDBManager dbmgr)
        {
            FireArea item = (FireArea)obj;
            System.Data.Common.DbParameter param;
            try
            {
                dbmgr.Open();
                dbmgr.BeginTransaction();

                // check to see if new object
                if (item.fa_id == 0)
                {
                    dbmgr.CreateParameters(3);
                    dbmgr.AddParameters(0, "@id", 0, true);
                    dbmgr.AddParameters(1, "@fa", item.fa);
                    dbmgr.AddParameters(2, "@fa_desc", item.fa_desc);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FALIST_i");

                    // get item id
                    param = (System.Data.Common.DbParameter)dbmgr.Parameters.GetValue(0);
                    item.fa_id = (int)param.Value;
                }
                // otherwise update existing object
                else
                {
                    dbmgr.CreateParameters(14);
                    dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                    dbmgr.AddParameters(1, "@fa", item.fa);
                    dbmgr.AddParameters(2, "@fa_desc", item.fa_desc);
                    dbmgr.AddParameters(3, "@bldg", item.bldg);
                    dbmgr.AddParameters(4, "@elev", item.elev);
                    dbmgr.AddParameters(5, "@ssd_path", item.ssd_path);
                    dbmgr.AddParameters(6, "@req", item.req);
                    dbmgr.AddParameters(7, "@supp", item.supp);
                    dbmgr.AddParameters(8, "@det", item.det);
                    dbmgr.AddParameters(9, "@comment", item.comment);
                    dbmgr.AddParameters(10, "@dwg_id", item.dwg_id);
                    dbmgr.AddParameters(11, "@dwg_rev", item.dwg_rev);
                    dbmgr.AddParameters(12, "@risk_summary", item.risk_summary);
                    dbmgr.AddParameters(13, "@reg_basis", item.reg_basis);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FALIST_u");	        // execute the stored procedure

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

        private void UpdateStatus(IDBManager dbmgr, FireArea item, int statustype_id, string user_id, DateTime status_date)
        {
            if ((user_id == "N/A") || (user_id == ""))
            {
                // delete status
                dbmgr.CreateParameters(2);
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.AddParameters(1, "@statustype_id", statustype_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FASTATUS_d");
            }
            else
            {
                // update status
                dbmgr.CreateParameters(4);
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.AddParameters(1, "@statustype_id", statustype_id);
                dbmgr.AddParameters(2, "@user_id", user_id);
                if (status_date == Convert.ToDateTime(null))
                    // set date to null
                    dbmgr.AddParameters(3, "@status_date", null);
                else
                    // update date
                    dbmgr.AddParameters(3, "@status_date", status_date);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FASTATUS_u");
            }
        }

        #endregion

        #region compdisp

        private void FetchCompDispList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFACOMPDISP_LIVE WHERE FA_ID = @fa_id ORDER BY COMP, DISP";
            FACompDisp item = new FACompDisp();											    // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@fa_id", id);							                // fire area id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new FACompDisp();
                item = (FACompDisp)FetchObject(item, p, dbmgr);
                item.in_fa = Convert.ToBoolean(dbmgr.DataReader["in_fa"]);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _firearea.compdisplist = list;
        }

        private void DeleteCompDisp(object obj, IDBManager dbmgr)
        {
            FACompDisp item = (FACompDisp)obj;											    // cast object to proper item type
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(3);												    // create required paramaters
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.AddParameters(1, "@comp_id", item.comp_id);
                dbmgr.AddParameters(2, "@disp_id", item.disp_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FACOMPDISP_d");	    // execute stored procedure
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

        private void UpdateCompDisp(object obj, IDBManager dbmgr)
        {
            FACompDisp item = (FACompDisp)obj;										        // cast object to proper item type

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(5);												    // create required paramaters
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.AddParameters(1, "@comp_id", item.comp_id);
                dbmgr.AddParameters(2, "@old_disp_id", item.old_disp_id);
                dbmgr.AddParameters(3, "@disp_id", item.disp_id);
                dbmgr.AddParameters(4, "@status", item.status);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FACOMPDISP_u");	    // execute stored procedure
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

        #region cabdisp

        private void FetchCabDispList(int fa_id, int comp_id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFACABDISP_LIVE WHERE FA_ID = @fa_id AND COMP_ID = @comp_id ORDER BY COMP, CABLE, DISP";
            FACabDisp item = new FACabDisp();									            // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(2);											            // create parameters
            dbmgr.AddParameters(0, "@fa_id", fa_id);							            // fire area id
            dbmgr.AddParameters(1, "@comp_id", comp_id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new FACabDisp();
                item = (FACabDisp)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }
            dbmgr.CloseReader();
            _firearea.cabdisplist = list;											        // update item list
        }

        private void DeleteCabDisp(object obj, IDBManager dbmgr)
        {
            FACabDisp item = (FACabDisp)obj;									            // cast object to proper item type
            try
            {
                dbmgr.Open();															    // open database
                dbmgr.CreateParameters(4);												    // create required paramaters
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.AddParameters(1, "@comp_id", item.comp_id);
                dbmgr.AddParameters(2, "@cable_id", item.cable_id);
                dbmgr.AddParameters(3, "@disp_id", item.disp_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FACABDISP_d");	    // execute stored procedure
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

        private void UpdateCabDisp(object obj, IDBManager dbmgr)
        {
            FACabDisp item = (FACabDisp)obj;										        // cast object to proper item type
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(6);												    // create required paramaters
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.AddParameters(1, "@comp_id", item.comp_id);
                dbmgr.AddParameters(2, "@cable_id", item.cable_id);
                dbmgr.AddParameters(3, "@old_disp_id", item.old_disp_id);
                dbmgr.AddParameters(4, "@disp_id", item.disp_id);
                dbmgr.AddParameters(5, "@status", item.status);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FACABDISP_u");	    // execute stored procedure
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

        #region cascading power

        private void FetchCascPowerList(string analysis, int fa_id, IDBManager dbmgr)
        {
            FACascHit item = new FACascHit();											    // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(2);											            // create parameters
            dbmgr.AddParameters(0, "@analysis", analysis);							            // fire area id
            dbmgr.AddParameters(1, "@fa_id", fa_id);
            dbmgr.ExecuteReader(CommandType.StoredProcedure, "dbo.FACASC_POWER_s"); // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new FACascHit();											            // create new item
                item = (FACascHit)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }
            dbmgr.CloseReader();
            _firearea.cascpowerlist = list;											        // update item list
        }

        #endregion

        #region cascading interlock

        private void FetchCascIntlkList(string analysis, int fa_id, IDBManager dbmgr)
        {
            FACascHit item = new FACascHit();											    // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects

            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item
            dbmgr.CreateParameters(2);											            // create parameters
            dbmgr.AddParameters(0, "@analysis", analysis);							            // fire area id
            dbmgr.AddParameters(1, "@fa_id", fa_id);
            dbmgr.ExecuteReader(CommandType.StoredProcedure, "dbo.FACASC_INTLK_s"); // execute query

            while (dbmgr.DataReader.Read())
            {
                item = new FACascHit();
                item = (FACascHit)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _firearea.cascintlklist = list;											        // update item list
        }

        #endregion

        #region VFDR

        private void FetchVFDRList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT f.FA, v.* FROM FAVFDR v INNER JOIN FALIST f ON f.FA_ID = v.FA_ID WHERE v.FA_ID = @fa_id ORDER BY v.VFDR_ID";
            FAVFDR item = new FAVFDR();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@fa_id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new FAVFDR();
                item = (FAVFDR)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _firearea.vfdrlist = list;
        }

        private void DeleteVFDR(object obj, IDBManager dbmgr)
        {
            FAVFDR item = (FAVFDR)obj;
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(2);
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.AddParameters(1, "@vfdr_id", item.vfdr_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FAVFDR_d");
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

        private void UpdateVFDR(object obj, IDBManager dbmgr)
        {
            FAVFDR item = (FAVFDR)obj;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(6);
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.AddParameters(1, "@vfdr_id", item.vfdr_id);
                dbmgr.AddParameters(2, "@vfdr", item.vfdr);
                dbmgr.AddParameters(3, "@vfdr_disp", item.vfdr_disp);
                dbmgr.AddParameters(4, "@status", item.status);
                dbmgr.AddParameters(5, "@fre_ref", item.fre_ref);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FAVFDR_u");
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

        #region performance goal

        private void FetchPGList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT f.FA, p.* FROM FAPG p INNER JOIN FALIST f ON f.FA_ID = p.FA_ID WHERE p.FA_ID = @fa_id ORDER BY p.PG";
            FAPG item = new FAPG();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@fa_id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new FAPG();
                item = (FAPG)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _firearea.pglist = list;
        }

        private void UpdatePG(object obj, IDBManager dbmgr)
        {
            FAPG item = (FAPG)obj;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(4);
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.AddParameters(1, "@pg", item.pg);
                dbmgr.AddParameters(2, "@method", item.method);
                dbmgr.AddParameters(3, "@comment", item.comment);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FAPG_u");
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

        #region references

        private void FetchLICdocList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFADOCS WHERE FA_ID = @fa_id AND DOC_TYPE = @doc_type ORDER BY DOC";
            FADoc item = new FADoc();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.CreateParameters(2);
            dbmgr.AddParameters(0, "@fa_id", id);
            dbmgr.AddParameters(1, "@doc_type", "LIC");
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new FADoc();
                item = (FADoc)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _firearea.licdoclist = list;
        }

        private void FetchEEdocList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFADOCS WHERE FA_ID = @fa_id AND DOC_TYPE = @doc_type ORDER BY DOC";
            FADoc item = new FADoc();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.CreateParameters(2);
            dbmgr.AddParameters(0, "@fa_id", id);
            dbmgr.AddParameters(1, "@doc_type", "EE");
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new FADoc();
                item = (FADoc)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _firearea.eedoclist = list;
        }

        private void FetchGENdocList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFADOCS WHERE FA_ID = @fa_id AND DOC_TYPE = @doc_type ORDER BY DOC";
            FADoc item = new FADoc();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.CreateParameters(2);
            dbmgr.AddParameters(0, "@fa_id", id);
            dbmgr.AddParameters(1, "@doc_type", "GEN");
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new FADoc();
                item = (FADoc)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _firearea.gendoclist = list;
        }

        private void DeleteDocument(object obj, IDBManager dbmgr)
        {
            FADoc item = (FADoc)obj;
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(2);
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.AddParameters(1, "@doc_id", item.doc_id);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FADOC_d");
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

        private void UpdateDocument(object obj, IDBManager dbmgr)
        {
            FADoc item = (FADoc)obj;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(3);
                dbmgr.AddParameters(0, "@fa_id", item.fa_id);
                dbmgr.AddParameters(1, "@doc_id", item.doc_id);
                dbmgr.AddParameters(2, "@comment", item.comment);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.FADOC_u");
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
            string qryString = "SELECT * FROM viewFZIGNITION WHERE FA_ID = @fa_id ORDER BY FZ, IG";
            FZIgnition item = new FZIgnition();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@fa_id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new FZIgnition();
                item = (FZIgnition)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _firearea.ignitionlist = list;
        }

        #endregion

        #region fire protection

        private void FetchProtectionList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFZPROTECTION WHERE FA_ID = @fa_id ORDER BY FZ, SYS_CATEGORY, SYS_NAME";
            FZProtection item = new FZProtection();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@fa_id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new FZProtection();
                item = (FZProtection)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _firearea.protectionlist = list;
        }

        #endregion
    }
}
