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
    // class handles transactions pertaining to Component Entity
    public class BlockDiagramManager : TotalManager, IFactoryManager
    {
        private User _user;
        private IDBManager _dbmgr;
        private CableBlock _cabBlock;

        private object _oldobj;                                                             // old object, used for tracking history
        private string[] _fieldlist;                                                        // list of fields whose control appears in web form

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

        #region public methods

        // fetches object and object items
        // user has option of passing object itself OR a string with id and type delimited by '|'
        public object Fetch(object obj)
        {
            int id = ((CableBlock)obj).comp_id;

            // create new object and database connection
            _cabBlock = new CableBlock();
            _user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            _dbmgr = new DBManager(_user.plantDBStr);
            _dbmgr.ConnectionString = _user.plantDBStr;

            try
            {
                _dbmgr.Open();

                FetchComponent(id, _dbmgr);
                FetchCableBlock(id, _dbmgr);
                FetchVertexList(id, _dbmgr);
                return _cabBlock;
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

        // fetches a dataset
        public DataSet FetchDataSet(string criteria)
        {
            DataSet ds = new DataSet();
            return ds;
        }

        // fetches a arraylist
        public ArrayList FetchComboList(string initMsg, string param)
        {
            ArrayList list = new ArrayList();
            ListItem listItem = new ListItem();
            return list;
        }

        // deletes given object or object item
        public void Delete(object obj)
        {
        }

        // updates/adds given object or object item
        public void Update(object obj)
        {
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
                PropertyInfo[] p = _cabBlock.GetType().GetProperties();
                _cabBlock = (CableBlock)FetchObject(_cabBlock, p, dbmgr);
            }

            dbmgr.CloseReader();

        }
        
        #endregion

        #region cable block

        private void FetchCableBlock(int id, IDBManager dbmgr)
        {
            string qryString = "SELECT cc.COMP_ID, cc.COMP, cc.COMP_SSD_REQ, cc.COMP_PRA_REQ, cc.COMP_NPO_REQ," +
                                    "cc.CABLE_ID, cc.CABLE, cc.SSD_REQ, cc.PRA_REQ, cc.NPO_REQ, dbo.GET_CAB_LOCATION(cc.CABLE_ID) AS CABLE_LOCA, " +
	                                "cc.FR_EQUIP_ID, cc.FR_EQUIP, c.FR_EQUIP_BD, dbo.GET_LOCATION(cc.FR_EQUIP) AS FR_LOCA, " +
	                                "cc.TO_EQUIP_ID, cc.TO_EQUIP, c.TO_EQUIP_BD, dbo.GET_LOCATION(cc.TO_EQUIP) AS TO_LOCA " +
                                "FROM viewCOMPCAB cc " +
                                "INNER JOIN CABLIST c ON c.CABLE_ID = cc.CABLE_ID " +
                                "WHERE cc.COMP_ID = @comp_id " +
                                "ORDER BY cc.COMP, cc.CABLE ";

            CompCab item = new CompCab();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@comp_id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new CompCab();
                item = (CompCab)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _cabBlock.cablelist = list;

        }

        #endregion

        #region drawing

        // procedure fetches drawings given a component id
        private void FetchVertexList(int id, IDBManager dbmgr)
        {
            Vertex item = new Vertex();
            ArrayList list = new ArrayList();

            PropertyInfo[] p = item.GetType().GetProperties();
            string qryString = "SELECT DISTINCT c.FR_EQUIP_BD AS NAME, dbo.GET_LOCATION(cc.FR_EQUIP) AS LOCATION " +
                                "FROM viewCOMPCAB cc " +
                                "INNER JOIN CABLIST c ON c.CABLE_ID = cc.CABLE_ID " +
                                "WHERE cc.COMP_ID = @comp_id " +
                                "UNION " +
                                "SELECT DISTINCT c.TO_EQUIP_BD AS NAME, dbo.GET_LOCATION(cc.TO_EQUIP) AS LOCATION " +
                                "FROM viewCOMPCAB cc " +
                                "INNER JOIN CABLIST c ON c.CABLE_ID = cc.CABLE_ID " +
                                "WHERE cc.COMP_ID = @comp_id " +
                                "ORDER BY NAME";

            dbmgr.CreateParameters(1);
            dbmgr.AddParameters(0, "@comp_id", id);
            dbmgr.ExecuteReader(CommandType.Text, qryString);

            while (dbmgr.DataReader.Read())
            {
                item = new Vertex();
                item = (Vertex)FetchObject(item, p, dbmgr);
                list.Add(item);
            }

            dbmgr.CloseReader();
            _cabBlock.vertexlist = list;

        }

        #endregion

    }
}
