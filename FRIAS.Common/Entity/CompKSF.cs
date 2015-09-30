using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain component-KSF records
    /// </summary>
    public class CompKSF : BaseEntity
    {
        #region private variables
        private int _comp_id;                                                               // component id
        private int _ksf_id;                                                                // key safety function id
        private string _ksf;                                                                // key safety function
        private string _ksf_desc;                                                           // key safety function description
        private string _ksf_detail;                                                         // key safety function detail
        private int _mode_id;                                                               // mode id
        private string _mode;                                                               // mode
        private string _note;                                                               // comment/note
        private string _unit;                                                               // unit
        private string _sys;                                                                // system
        private string _train;                                                              // train
        private string _comp_type;                                                          // component type
        private string _comp_desc;                                                          // component description
        private string _fail_elect;                                                         // fail electrical position
        private string _fail_air;                                                           // fail air position
        private string _npo_np;                                                             // normal npo position
        private string _npop;                                                               // npo position
        private string _verify;
        #endregion

        public CompKSF(){}
        public CompKSF(CompKSF obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public int comp_id
        {
            get
            {
                return _comp_id;
            }
            set
            {
                _comp_id = value;
            }
        }
        public int ksf_id
        {
            get
            {
                return _ksf_id;
            }
            set
            {
                _ksf_id = value;
            }
        }
        public string ksf
        {
            get
            {
                return _ksf;
            }
            set
            {
                _ksf = value;
            }
        }
        public string ksf_desc
        {
            get
            {
                return _ksf_desc;
            }
            set
            {
                _ksf_desc = value;
            }
        }
        public string ksf_detail
        {
            get
            {
                return _ksf_detail;
            }
            set
            {
                _ksf_detail = value;
            }
        }
        public int mode_id
        {
            get
            {
                return _mode_id;
            }
            set
            {
                _mode_id = value;
            }
        }
        public string mode
        {
            get
            {
                return _mode;
            }
            set
            {
                _mode = value;
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
        public string unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
            }
        }
        public string sys
        {
            get
            {
                return _sys;
            }
            set
            {
                _sys = value;
            }
        }
        public string train
        {
            get
            {
                return _train;
            }
            set
            {
                _train = value;
            }
        }
        public string comp_type
        {
            get
            {
                return _comp_type;
            }
            set
            {
                _comp_type = value;
            }
        }
        public string comp_desc
        {
            get
            {
                return _comp_desc;
            }
            set
            {
                _comp_desc = value;
            }
        }
        public string fail_elect
        {
            get
            {
                return _fail_elect;
            }
            set
            {
                _fail_elect = value;
            }
        }
        public string fail_air
        {
            get
            {
                return _fail_air;
            }
            set
            {
                _fail_air = value;
            }
        }
        public string npo_np
        {
            get
            {
                return _npo_np;
            }
            set
            {
                _npo_np = value;
            }
        }
        public string npop
        {
            get
            {
                return _npop;
            }
            set
            {
                _npop = value;
            }
        }
        public string verify
        {
            get
            {
                return _verify;
            }
            set
            {
                _verify = value;
            }
        }
        #endregion
    }
}
