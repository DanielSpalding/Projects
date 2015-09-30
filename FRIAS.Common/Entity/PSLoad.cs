using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to powr source - load records
    /// </summary>
    public class PSLoad : BaseEntity
    {
        #region private variables
        private int _psload_id;                                                             // power source load id
        private int _power_id;                                                              // power source id
        private string _power;                                                              // power source (itself a comp)
        private string _bkrfuse;                                                            // breaker/fuse
        private string _up_feed;                                                            // up feed
        private string _dn_feed;                                                            // down feed
        private bool _is_incoming;                                                          // is incoming?
        private string _load_desc;                                                          // load description
        private string _load_id;                                                            // load id
        private string _coord;                                                              // coordinated
        private string _coord_len;                                                          // coordination length
        private string _tcc_ref;
        #endregion

        public PSLoad(){}
        public PSLoad(PSLoad obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public int psload_id
        {
            get
            {
                return _psload_id;
            }
            set
            {
                _psload_id = value;
            }
        }
        public int power_id
        {
            get
            {
                return _power_id;
            }
            set
            {
                _power_id = value;
            }
        }
        public string power
        {
            get
            {
                return _power;
            }
            set
            {
                _power = value;
            }
        }
        public string bkrfuse
        {
            get
            {
                return _bkrfuse;
            }
            set
            {
                _bkrfuse = value;
            }
        }
        public string up_feed
        {
            get
            {
                return _up_feed;
            }
            set
            {
                _up_feed = value;
            }
        }
        public string dn_feed
        {
            get
            {
                return _dn_feed;
            }
            set
            {
                _dn_feed = value;
            }
        }
        public bool is_incoming
        {
            get
            {
                return _is_incoming;
            }
            set
            {
                _is_incoming = value;
            }
        }
        public string load_desc
        {
            get
            {
                return _load_desc;
            }
            set
            {
                _load_desc = value;
            }
        }
        public string load_id
        {
            get
            {
                return _load_id;
            }
            set
            {
                _load_id = value;
            }
        }
        public string coord
        {
            get
            {
                return _coord;
            }
            set
            {
                _coord = value;
            }
        }
        public string coord_len
        {
            get
            {
                return _coord_len;
            }
            set
            {
                _coord_len = value;
            }
        }
        public string tcc_ref
        {
            get
            {
                return _tcc_ref;
            }
            set
            {
                _tcc_ref = value;
            }
        }
        #endregion
    }
}
