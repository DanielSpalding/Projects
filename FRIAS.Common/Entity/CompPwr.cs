using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain component-power source records
    /// </summary>
    public class CompPwr : BaseEntity
    {
        #region private variables
        private int _comp_id;                                                               // component id
        private string _comp;                                                               // component
        private bool _comp_ssd_req;                                                         // component required for ssd
        private bool _comp_pra_req;                                                         // component required for pra
        private bool _comp_npo_req;                                                         // component required for npo
        private bool _comp_cfp_req;                                                         // component required for cfp
        private string _comp_np;                                                            // normal position
        private string _comp_hsbp;                                                          // hot standby position
        private string _comp_hsp;                                                           // hot shutdown position
        private string _comp_csp;                                                           // cold shutdown position
        private string _comp_prap;                                                          // pra position
        private string _comp_cfpp;                                                          // cfp position
        private string _comp_npo_np;                                                        // normal npo position
        private string _comp_npop;                                                          // npo position
        private int _power_id;                                                              // power id
        private string _power;                                                              // power
        private bool _power_ssd_req;                                                        // power required for ssd
        private bool _power_pra_req;                                                        // power required for pra
        private bool _power_npo_req;                                                        // power required for npo
        private bool _power_cfp_req;                                                        // power required for cfp
        private bool _alt_pwr;                                                              // comp-pwr is an alt source
        private bool _ssd_req;                                                              // comp-pwr required for ssd
        private bool _pra_req;                                                              // comp-pwr required for pra
        private bool _npo_req;                                                              // comp-pwr required for npo
        private bool _cfp_req;                                                              // comp-pwr required for cfp
        private string _note;                                                               // comp-pwr note
        #endregion

        public CompPwr(){}
        public CompPwr(CompPwr obj)
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
        public string comp
        {
            get
            {
                return _comp;
            }
            set
            {
                _comp = value;
            }
        }
        public bool comp_ssd_req
        {
            get
            {
                return _comp_ssd_req;
            }
            set
            {
                _comp_ssd_req = value;
            }
        }
        public bool comp_pra_req
        {
            get
            {
                return _comp_pra_req;
            }
            set
            {
                _comp_pra_req = value;
            }
        }
        public bool comp_npo_req
        {
            get
            {
                return _comp_npo_req;
            }
            set
            {
                _comp_npo_req = value;
            }
        }
        public bool comp_cfp_req
        {
            get
            {
                return _comp_cfp_req;
            }
            set
            {
                _comp_cfp_req = value;
            }
        }
        public string comp_np
        {
            get
            {
                return _comp_np;
            }
            set
            {
                _comp_np = value;
            }
        }
        public string comp_hsbp
        {
            get
            {
                return _comp_hsbp;
            }
            set
            {
                _comp_hsbp = value;
            }
        }
        public string comp_hsp
        {
            get
            {
                return _comp_hsp;
            }
            set
            {
                _comp_hsp = value;
            }
        }
        public string comp_csp
        {
            get
            {
                return _comp_csp;
            }
            set
            {
                _comp_csp = value;
            }
        }
        public string comp_prap
        {
            get
            {
                return _comp_prap;
            }
            set
            {
                _comp_prap = value;
            }
        }
        public string comp_cfpp
        {
            get
            {
                return _comp_cfpp;
            }
            set
            {
                _comp_cfpp = value;
            }
        }
        public string comp_npo_np
        {
            get
            {
                return _comp_npo_np;
            }
            set
            {
                _comp_npo_np = value;
            }
        }
        public string comp_npop
        {
            get
            {
                return _comp_npop;
            }
            set
            {
                _comp_npop = value;
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
        public bool power_ssd_req
        {
            get
            {
                return _power_ssd_req;
            }
            set
            {
                _power_ssd_req = value;
            }
        }
        public bool power_pra_req
        {
            get
            {
                return _power_pra_req;
            }
            set
            {
                _power_pra_req = value;
            }
        }
        public bool power_npo_req
        {
            get
            {
                return _power_npo_req;
            }
            set
            {
                _power_npo_req = value;
            }
        }
        public bool power_cfp_req
        {
            get
            {
                return _power_cfp_req;
            }
            set
            {
                _power_cfp_req = value;
            }
        }
        public bool alt_pwr
        {
            get
            {
                return _alt_pwr;
            }
            set
            {
                _alt_pwr = value;
            }
        }
        public bool ssd_req
        {
            get
            {
                return _ssd_req;
            }
            set
            {
                _ssd_req = value;
            }
        }
        public bool pra_req
        {
            get
            {
                return _pra_req;
            }
            set
            {
                _pra_req = value;
            }
        }
        public bool npo_req
        {
            get
            {
                return _npo_req;
            }
            set
            {
                _npo_req = value;
            }
        }
        public bool cfp_req
        {
            get
            {
                return _cfp_req;
            }
            set
            {
                _cfp_req = value;
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
