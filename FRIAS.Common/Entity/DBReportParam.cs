using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain report-parameters
    /// </summary>
    public class DBReportParam
    {
        #region private variables
        private string _param_name;                                                         // paramater name
        private string _param_value;                                                        // parameter value
        #endregion

        public DBReportParam(){}
        public DBReportParam(DBReportParam obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }
        public DBReportParam(string name, string value)
        {
            _param_name = name;
            _param_value = value;
        }

        #region public properties
        public string param_name
        {
            get { return _param_name; }
            set { _param_name = value; }
        }
        public string param_value
        {
            get { return _param_value; }
            set { _param_value = value; }
        }
        #endregion
    }
}
