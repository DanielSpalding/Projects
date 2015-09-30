using System;
using System.Data;
using System.Collections;
using System.Reflection;

using FRIAS.Common;
using FRIAS.Common.Entity;
using DL.Data;

namespace FRIAS.BL
{
    // common methods used by other managers
    public class TotalManager
    {
        // fetches object properties given an object, property info and database manager
        public static Object FetchObject(Object item, PropertyInfo[] p, IDBManager dbmgr)
        {
            for (int i = 0; i < (p.Length); i++)
            {
                // check to see if column exists in database
                if (ColumnExists(p[i].Name, dbmgr))
                {
                    // properties that are *list or arg* are base entity properties that are fetched separately and are therefore ignored here
                    if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    {
                        // check to see if value is null
                        if (dbmgr.DataReader[p[i].Name] != System.DBNull.Value)
                        {
                            // check to see if item is decimal (eg. sequence), if it is convert to decimal
                            if (p[i].PropertyType.Name.Contains("Decimal"))
                                p[i].SetValue(item, Convert.ToDecimal(dbmgr.DataReader[p[i].Name]), null);
                            // otherwise set value from database
                            else
                                p[i].SetValue(item, dbmgr.DataReader[p[i].Name], null);
                        }

                        // otherwise determine type of property to pass approiate null value
                        else
                        {
                            switch (p[i].PropertyType.Name.ToString())
                            {
                                case "Decimal":
                                case "Int32":
                                    p[i].SetValue(item, 0, null);                           // integer fields get "0"
                                    break;
                                case "DateTime":
                                    p[i].SetValue(item, Convert.ToDateTime(null), null);    // date fields get "1/1/0001 12:00:00 AM"
                                    break;
                                default:
                                    p[i].SetValue(item, "", null);                          // everything else gets ""
                                    break;
                            }
                        }

                    }
                }
            }
            return item;
        }

        // fetches drawing revision from drawing list for a given drawing id
        public static string FetchDrawingRev(int dwg_id)
        {
            // NOTE: if multiple dwg_rev exists then the latest rev is selected (b/c sorting dwg_rev in descending order)
            string qryString = "SELECT DWG_REV FROM DWGLIST d WHERE d.DWG_ID = @dwg_id ORDER BY d.DWG_ID, d.DWG_REV DESC";
            string rev = "";

            // create database connection
            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(1);
                dbmgr.AddParameters(0, "@dwg_id", dwg_id);
                dbmgr.ExecuteReader(CommandType.Text, qryString);
                
                if (dbmgr.DataReader.Read())
                    rev = dbmgr.DataReader["DWG_REV"].ToString();
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbmgr.Dispose();
            }

            return rev;
        }

        // fetches document information given document id
        public static string FetchDocumentInfo(int doc_id)
        {
            string qryString = "SELECT DOC_DESC FROM DOCLIST d WHERE d.DOC_ID = @doc_id ORDER BY d.DOC_ID, d.DOC_DESC";
            string doc_desc = "";

            // create database connection
            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(1);
                dbmgr.AddParameters(0, "@doc_id", doc_id);
                dbmgr.ExecuteReader(CommandType.Text, qryString);

                if (dbmgr.DataReader.Read())
                    doc_desc = dbmgr.DataReader["DOC_DESC"].ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbmgr.Dispose();
            }

            return doc_desc;
        }

        // function determines if a given column exists in database
        // returns -1 if column does not exist otherwise returns ordinal of existing column
        public static bool ColumnExists(string colName, IDBManager dbmgr)
        {
            bool exists = true;

            try
            {
                // get ordinal of given column from database
                int col = dbmgr.DataReader.GetOrdinal(colName);
            }
            catch
            {
                // if ordinal does not exist exception occurs
                exists = false;
            }
            return exists;
        }

        // procedure saves object to history
        // definition of variables passed:
        //      obj         : current object
        //      old_obj     : old object prior to update
        //      table_chg   : name of table that was changed
        //      data_chg    : data that was changed
        //      key_field   : name of key_field (column) within table 
        //                    (eg. changes in COMPLIST will have COMP as key_field, changes in CABLIST will have CABLE
        //      dbmgr       : database manager
        //      fieldlist   : list of fields that were involved
        //                    (this is needed because more fields may exisit for a given entity than was changed in webform)
        public void SaveHistory(Object obj, Object old_obj, string user_id, string table_chg, string data_chg, string key_field, IDBManager dbmgr, string[] fieldList)
        {
            History hist;                                                                   // create new entity
            ArrayList listChgs = new ArrayList();                                           // array to hold list of changes
            
            // determine type of change: addition, deletion or modification
            // and set values accordingly

            // ADDITION: if old_obj is null then user entering a new record
            if (old_obj == null)
            {
                hist = new History();                                                       // create new instance
                hist.field_chg = "RECORD ADDED";                                            // new record added
                hist.new_data = data_chg;
                hist.old_data = "";                                                         // old data is null
                hist.user_id = user_id;
                hist.table_chg = table_chg;
                hist.key_field = key_field;
                listChgs.Add(hist);                                                         // add to arraylist
            }
            
            // DELETION: if obj is null then user deleting a record
            else if (obj == null)
            {
                hist = new History();                                                       // create new instance
                hist.field_chg = "RECORD DELETED";                                          // record deleted
                hist.new_data = "";                                                         // new data is null
                hist.old_data = data_chg;
                hist.user_id = user_id;
                hist.table_chg = table_chg;
                hist.key_field = key_field;
                listChgs.Add(hist);                                                         // add to arraylist
            }
            
            // MODIFICATION: otherwise user is modifying an existing record
            else
            {
                PropertyInfo[] properties = obj.GetType().GetProperties();
                foreach (PropertyInfo info in properties)
                {
                    // initialize by setting new and old data to empty string
                    string new_data = "";
                    string old_data = "";

                    // properties that are *list or arg* are used base properties used only for application
                    // these will not exist in table therefore screen for these
                    if (!info.PropertyType.Name.Contains("list") && !info.Name.Contains("arg"))
                    {
                        // check to see if control for given name
                        if (ControlExists(info.Name, fieldList))
                        {
                            if (info.GetValue(old_obj, null) != null)
                                old_data = info.GetValue(old_obj, null).ToString();
                            if (info.GetValue(obj, null) != null)
                                new_data = info.GetValue(obj, null).ToString();
                            
                            // if the new and old data are not equal to one another
                            if (!new_data.Equals(old_data))
                            {
                                hist = new History();                                       // create new instance
                                hist.new_data = new_data;
                                hist.old_data = old_data;
                                hist.field_chg = info.Name.ToUpper();                       // set field changed (convert to upper case)
                                hist.user_id = user_id;
                                hist.table_chg = table_chg;
                                hist.key_field = key_field;
                                listChgs.Add(hist);                                         // add to arraylist
                            }
                        }
                    }
                }
            }

            // update HISTORY_LOG table as long as there is changes
            if (listChgs.Count > 0)
            {
                foreach (History h in listChgs)
                {
                    try
                    {
                        dbmgr.CreateParameters(6);
                        dbmgr.AddParameters(0, "@user_id", h.user_id);
                        dbmgr.AddParameters(1, "@field_chg", h.field_chg);
                        dbmgr.AddParameters(2, "@new_data", h.new_data);
                        dbmgr.AddParameters(3, "@old_data", h.old_data);
                        dbmgr.AddParameters(4, "@table_chg", h.table_chg);
                        dbmgr.AddParameters(5, "@key_field", h.key_field);
                        dbmgr.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "HISTORY_LOG_i");
                    }
                    catch (Exception ex)
                    {
                        throw (ex);
                    }
                }
            }
        }

        // determines if control exists within a given set of fields passed
        // used to determine if the control is a property in SaveHistory
        private bool ControlExists(string controlName, string[] fields)
        {
            bool found = false;

            // if no field exists then found = false
            if (fields == null)
                return found;

            // for each substring within field check to see if control name matches field
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i] != null)
                {
                    // if field equals the control name then control exists
                    if (fields[i].Equals(controlName))
                        return true;
                }
            }

            return found;
        }

    }
}
