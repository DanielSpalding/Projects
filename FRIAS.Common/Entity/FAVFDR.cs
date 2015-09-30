using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire area - VFDR information
    /// </summary>
    public class FAVFDR : BaseEntity
    {
        #region private variables
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private string _vfdr_id;                                                            // vfdr id
        private string _vfdr;                                                               // vfdr description
        private string _vfdr_disp;                                                          // vfdr disposition
        private string _status;                                                             // status
        private string _fre_ref;                                                            // fre reference
        #endregion
        
        public FAVFDR(){}
        public FAVFDR(FAVFDR obj)
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
        public string vfdr_id
        {
            get
            {
                return _vfdr_id;
            }
            set
            {
                _vfdr_id = value;
            }
        }
        public string vfdr
        {
            get
            {
                return _vfdr;
            }
            set
            {
                _vfdr = value;
            }
        }
        public string vfdr_disp
        {
            get
            {
                return _vfdr_disp;
            }
            set
            {
                _vfdr_disp = value;
            }
        }
        public string status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public string fre_ref
        {
            get
            {
                return _fre_ref;
            }
            set
            {
                _fre_ref = value;
            }
        }
        #endregion
    }
}
