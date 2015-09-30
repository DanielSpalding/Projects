using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire area - ignition source
    /// </summary>
    public class FZProtection : BaseEntity
    {
        #region private variables
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private int _fz_id;                                                                 // fire zone id
        private string _fz;                                                                 // fire zone
        private string _sys_category;                                                       // category
        private string _sys_name;                                                           // system
        private string _sys_desc;                                                           // system description/location
        private string _sys_type;                                                           // system type
        private string _comment;                                                            // comment
        #endregion
        
        public FZProtection(){}
        public FZProtection(FZProtection obj)
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
        public int fz_id
        {
            get
            {
                return _fz_id;
            }
            set
            {
                _fz_id = value;
            }
        }
        public string fz
        {
            get
            {
                return _fz;
            }
            set
            {
                _fz = value;
            }
        }
        public string sys_category
        {
            get
            {
                return _sys_category;
            }
            set
            {
                _sys_category = value;
            }
        }
        public string sys_name
        {
            get
            {
                return _sys_name;
            }
            set
            {
                _sys_name = value;
            }
        }
        public string sys_desc
        {
            get
            {
                return _sys_desc;
            }
            set
            {
                _sys_desc = value;
            }
        }
        public string sys_type
        {
            get
            {
                return _sys_type;
            }
            set
            {
                _sys_type = value;
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
