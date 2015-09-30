using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain component-interlock records
    /// </summary>
    public class CompIntlk : BaseEntity
    {
        #region private variables
        private int _comp_id;                                                               // component id
        private string _comp;                                                               // component
        private bool _comp_ssd_req;                                                         // component required for ssd
        private bool _comp_pra_req;                                                         // component required for pra
        private bool _comp_npo_req;                                                         // component required for npo
        private bool _comp_cfp_req;                                                         // component required for cfp
        private int _intlk_id;                                                              // interlock id
        private string _intlk;                                                              // interlock
        private bool _intlk_ssd_req;                                                        // interlock required for ssd
        private bool _intlk_pra_req;                                                        // interlock required for pra
        private bool _intlk_npo_req;                                                        // interlock required for npo
        private bool _intlk_cfp_req;                                                        // interlock required for cfp
        private string _device;                                                             // device
        private string _contacts;                                                           // contacts
        private bool _ssd_req;                                                              // comp-intlk required for ssd
        private bool _pra_req;                                                              // comp-intlk required for pra
        private bool _npo_req;                                                              // comp-intlk required for npo
        private bool _cfp_req;                                                              // comp-intlk required for cfp
        private string _note;                                                               // comp-intlk note
        private string _field1;
        private string _field2; 
        private string _field3;
        private string _field4;
        #endregion

        public CompIntlk(){}
        public CompIntlk(CompIntlk obj)
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
        public int intlk_id
        {
            get
            {
                return _intlk_id;
            }
            set
            {
                _intlk_id = value;
            }
        }
        public string intlk
        {
            get
            {
                return _intlk;
            }
            set
            {
                _intlk = value;
            }
        }
        public bool intlk_ssd_req
        {
            get
            {
                return _intlk_ssd_req;
            }
            set
            {
                _intlk_ssd_req = value;
            }
        }
        public bool intlk_pra_req
        {
            get
            {
                return _intlk_pra_req;
            }
            set
            {
                _intlk_pra_req = value;
            }
        }
        public bool intlk_npo_req
        {
            get
            {
                return _intlk_npo_req;
            }
            set
            {
                _intlk_npo_req = value;
            }
        }
        public bool intlk_cfp_req
        {
            get
            {
                return _intlk_cfp_req;
            }
            set
            {
                _intlk_cfp_req = value;
            }
        }
        public string device
        {
            get
            {
                return _device;
            }
            set
            {
                _device = value;
            }
        }
        public string contacts
        {
            get
            {
                return _contacts;
            }
            set
            {
                _contacts = value;
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
        public string field1
        {
            get
            {
                return _field1;
            }
            set
            {
                _field1 = value;
            }
        }
        public string field2
        {
            get
            {
                return _field2;
            }
            set
            {
                _field2 = value;
            }
        }
        public string field3
        {
            get
            {
                return _field3;
            }
            set
            {
                _field3 = value;
            }
        }
        public string field4
        {
            get
            {
                return _field4;
            }
            set
            {
                _field4 = value;
            }
        }
        #endregion
    }
}
