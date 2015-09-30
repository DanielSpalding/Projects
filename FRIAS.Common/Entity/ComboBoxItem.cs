
namespace FRIAS.Common.Entity
{
    /// <summary>
    /// combo box items specific to application
    /// </summary>
    public class ComboBoxItem
    {
        // constants, used to specify combo-box actions
        public static int INSERT = 1;
        public static int UPDATE = 2;
        public static int DELETE = 3;

        // types of combo-boxes
        public enum Type
        {
            Box, Unit, Sys, Train, Path, Method, Comp_Type, Position, Report, DrawingType, User, KSF,
            LookupTables, FArea, FZone, ExportTables, Bin, FAFZ
        }

        #region private variables
        private string _name;                                                           // name of combo box
        private string _value;                                                          // value of combo box
        private string _desc;                                                           // description
        private string _query;                                                          // query string
        private int _update_type;                                                       // type of update (1) insert, (2) update, (3) delete
        #endregion

        public ComboBoxItem(string name, string value)
        {
            this._name = name;
            this._value = value;
        }
        public ComboBoxItem(string name, string desc, string value, string query)
        {
            this._name = name;
            this._desc = desc;
            this._value = value;
            this._query = query;
        }

        #region public properties
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }
        public string desc
        {
            get { return _desc; }
            set { _desc = value; }
        }
        public string value
        {
            get { return _value; }
            set { _value = value; }
        }
        public string query
        {
            get { return _query; }
            set { _query = value; }
        }
        public int update_type
        {
            get { return _update_type; }
            set { _update_type = value; }
        }
        #endregion
    }
}
