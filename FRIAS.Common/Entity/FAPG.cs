using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire area - performance goal information
    /// </summary>
    public class FAPG : BaseEntity
    {
        #region private variables
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private string _pg;                                                                 // performance goal
        private string _method;                                                             // method
        private string _comment;                                                            // comment
        #endregion
        
        public FAPG(){}
        public FAPG(FAPG obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public int fa_id
        {
            get
            {
                return _fa_id;
            }
            set
            {
                _fa_id = value;
            }
        }
        public string fa
        {
            get
            {
                return _fa;
            }
            set
            {
                _fa = value;
            }
        }
        public string pg
        {
            get
            {
                return _pg;
            }
            set
            {
                _pg = value;
            }
        }
        public string method
        {
            get
            {
                return _method;
            }
            set
            {
                _method = value;
            }
        }
        public string comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
            }
        }
        #endregion
    }
}
