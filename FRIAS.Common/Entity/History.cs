using System;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain history/change records
    /// </summary>
    public class History: BaseEntity
    {
        #region private variables
        private int _hist_id;                                                               // history id
        private string _user_id;                                                            // user id
        private string _field_chg;                                                          // field changed
        private string _new_data;                                                           // new data
        private string _old_data;                                                           // old data
        private string _table_chg;                                                          // table name
        private string _key_field;                                                          // key value
        private DateTime _date_created;                                                     // date created
        #endregion

        #region public properties
        public int hist_id
        {
            set { _hist_id = value; }
            get { return _hist_id; }
        }
        public string user_id
        {
            set { _user_id = value; }
            get { return _user_id; }
        }
        public string field_chg
        {
            set { _field_chg = value; }
            get { return _field_chg; }
        }
        public string new_data
        {
            set { _new_data = value; }
            get { return _new_data; }
        }
        public string old_data
        {
            set { _old_data = value; }
            get { return _old_data; }
        }
        public string table_chg
        {
            set { _table_chg = value; }
            get { return _table_chg; }
        }
        public string key_field
        {
            set { _key_field = value; }
            get { return _key_field; }
        }
        public DateTime date_created
        {
            set { _date_created = value; }
            get { return _date_created; }
        }
        #endregion
    }
}
