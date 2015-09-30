using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire area - ignition source
    /// </summary>
    public class FZIgnition : BaseEntity
    {
        #region private variables
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private int _fz_id;                                                                 // fire zone id
        private string _fz;                                                                 // fire zone
        private string _ig;                                                                 // ignition source
        private string _ig_desc;                                                            // ignition description
        private string _ig_loc;                                                             // detailed location of ignition source
        private int _bin_id;                                                                // bin id
        private string _bin;                                                                // bin
        private string _bin_desc;                                                           // bin desciption
        private string _bin_count;
        private string _zoi;
        private string _note;
        #endregion
        
        public FZIgnition(){}
        public FZIgnition(FZIgnition obj)
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
        public string ig
        {
            get
            {
                return _ig;
            }
            set
            {
                _ig = value;
            }
        }
        public string ig_desc
        {
            get
            {
                return _ig_desc;
            }
            set
            {
                _ig_desc = value;
            }
        }
        public string ig_loc
        {
            get
            {
                return _ig_loc;
            }
            set
            {
                _ig_loc = value;
            }
        }
        public int bin_id
        {
            get
            {
                return _bin_id;
            }
            set
            {
                _bin_id = value;
            }
        }
        public string bin
        {
            get
            {
                return _bin;
            }
            set
            {
                _bin = value;
            }
        }
        public string bin_desc
        {
            get
            {
                return _bin_desc;
            }
            set
            {
                _bin_desc = value;
            }
        }
        public string bin_count
        {
            get
            {
                return _bin_count;
            }
            set
            {
                _bin_count = value;
            }
        }
        public string zoi
        {
            get
            {
                return _zoi;
            }
            set
            {
                _zoi = value;
            }
        }
        public string note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
            }
        }
        #endregion
    }
}
