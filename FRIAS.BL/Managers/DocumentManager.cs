using System;
using System.Data;
using System.Collections;
using System.Reflection;

using FRIAS.Common;
using FRIAS.Common.Entity;
using DL.Data;

namespace FRIAS.BL.Managers
{
    public class DocumentManager : TotalManager, IFactoryManager
    {
        private User _user;
        private IDBManager _dbmgr;                                                          // database manager
        private Document _document;                                                         // instance of primary object
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
                id = ((Document)obj).doc_id;
                type = "all";                                                               //object is  document and get all information
            }

            _document = new Document();
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            try
            {
                _dbmgr.Open();
                switch (type)
                {
                    case "firearea":
                        FetchFAList(id, _dbmgr);
                        return _document.falist;
                    default:
                        FetchDocument(id, _dbmgr);
                        FetchFAList(id, _dbmgr);
                        return _document;
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
            string qryString = "SELECT * FROM DOCLIST ORDER BY DOC";                        //create query string

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
                case "FRIAS.Common.Entity.Document":
                    DeleteDocument(obj, _dbmgr);
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
                case "FRIAS.Common.Entity.Document":
                    UpdateDocument(obj, _dbmgr);
                    break;
            }
        }
        
        #endregion

        #region document
        
        private void FetchDocument(int id, IDBManager dbmgr)
        {
            string qryLocal = "SELECT * FROM viewDOCLIST WHERE DOC_ID=@id";

            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryLocal);
            if (dbmgr.DataReader.Read())
            {
                // get properties of object and fetch object
                PropertyInfo[] p = _document.GetType().GetProperties();
                _document = (Document)FetchObject(_document, p, dbmgr);
            }

            dbmgr.CloseReader();
        }

        private void DeleteDocument(object obj, IDBManager dbmgr)
        {
            Document item = (Document)obj;
            
            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(2);
                dbmgr.AddParameters(0, "@doc_id", item.doc_id);
                dbmgr.AddParameters(1, "@error_num", 0, true);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.DOCLIST_d");
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
            Document item = (Document)obj;
            System.Data.Common.DbParameter param;
            
            try
            {
                dbmgr.Open();
                dbmgr.BeginTransaction();

                // check to see if new object
                if (item.doc_id == 0)
                {
                    // if id is null then new object
                    dbmgr.CreateParameters(4);
                    dbmgr.AddParameters(0, "@id", 0, true);
                    dbmgr.AddParameters(1, "@doc", item.doc);
                    dbmgr.AddParameters(2, "@doc_desc", item.doc_desc);
                    dbmgr.AddParameters(3, "@doc_type", item.doc_type);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.DOCLIST_i");

                    // get item id
                    param = (System.Data.Common.DbParameter)dbmgr.Parameters.GetValue(0);
                    item.doc_id = Convert.ToInt32(param.Value);
                }

                // otherwise update existing object
                else
                {
                    dbmgr.CreateParameters(4);
                    dbmgr.AddParameters(0, "@doc_id", item.doc_id);
                    dbmgr.AddParameters(1, "@doc", item.doc);
                    dbmgr.AddParameters(2, "@doc_desc", item.doc_desc);
                    dbmgr.AddParameters(3, "@doc_type", item.doc_type);
                    dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.DOCLIST_u");
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

        #region fire area
        
        private void FetchFAList(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT * FROM viewFADOCS d WHERE d.DOC_ID = @doc_id ORDER BY d.FA";
            FADoc item = new FADoc();											            // create new object type to be able to get property info
            ArrayList list = new ArrayList();										        // create new ArrayList to house objects
            PropertyInfo[] p = item.GetType().GetProperties();					            // get property info for item

            dbmgr.CreateParameters(1);											            // create parameters
            dbmgr.AddParameters(0, "@doc_id", id);							                // document id
            dbmgr.ExecuteReader(CommandType.Text, qryString);					            // execute query
            
            while (dbmgr.DataReader.Read())
            {
                item = new FADoc();											                // create new item
                item = (FADoc)FetchObject(item, p, dbmgr);
                list.Add(item);													            // add item to the ArrayList
            }

            dbmgr.CloseReader();
            _document.falist = list;												        // update object item list
        }
        
        #endregion
    }
}
