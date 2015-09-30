using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain component-cable records
    /// </summary>
    public class CompCab : BaseEntity
    {
        #region private variables
        private int _comp_id;                                                               // component id
        private string _comp;                                                               // component
        private bool _comp_ssd_req;                                                         // comp required for ssd
        private bool _comp_pra_req;                                                         // comp required for pra
        private bool _comp_npo_req;                                                         // comp required for npo
        private bool _comp_cfp_req;                                                         // comp required for cfp
        private int _cable_id;                                                              // cable id
        private string _cable;                                                              // cable name
        private string _cable_loca;
        private string _fault;                                                              // comp-cab fault code
        private string _fault_type;
        private string _cable_orig;
        private bool _ssd_req;                                                              // comp-cab required for ssd
        private bool _pra_req;                                                              // comp-cab required for pra
        private bool _npo_req;                                                              // comp-cab required for npo
        private bool _cfp_req;                                                              // comp-cab required for cfp
        private int _fr_equip_id;                                                           // from equipment id
        private string _fr_equip;                                                           // cable from endpoint
        private string _fr_equip_bd;
        private string _fr_loca;
        private int _fr_dwg_id;                                                             // from drawing id
        private string _fr_dwg;                                                             // cable from endpoint drawing reference
        private string _fr_dwg_rev;                                                         // cable from endpoint drawing revision
        private int _to_equip_id;                                                           // to equipment id
        private string _to_equip;                                                           // cable to endpoint
        private string _to_equip_bd;
        private string _to_loca;
        private int _to_dwg_id;                                                             // to drawing id
        private string _to_dwg;                                                             // cable to endpoint drawing reference
        private string _to_dwg_rev;                                                         // cable to endpoint drawing revision
        private string _note;                                                               // comp-cab note
        private bool _comp_chkd;                                                            // component checked
        #endregion

        public CompCab(){}
        public CompCab(CompCab obj)
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
        public int cable_id
        {
            get
            {
                return _cable_id;
            }
            set
            {
                _cable_id = value;
            }
        }
        public string cable
        {
            get
            {
                return _cable;
            }
            set
            {
                _cable = value;
            }
        }
        public string cable_loca
        {
            get
            {
                return _cable_loca;
            }
            set
            {
                _cable_loca = value;
            }
        }
        public string fault
        {
            get
            {
                return _fault;
            }
            set
            {
                _fault = value;
            }
        }
        public string fault_type
        {
            get
            {
                return _fault_type;
            }
            set
            {
                _fault_type = value;
            }
        }
        public string cable_orig
        {
            get
            {
                return _cable_orig;
            }
            set
            {
                _cable_orig = value;
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
        public int fr_equip_id
        {
            get
            {
                return _fr_equip_id;
            }
            set
            {
                _fr_equip_id = value;
            }
        }
        public string fr_equip
        {
            get
            {
                return _fr_equip;
            }
            set
            {
                _fr_equip = value;
            }
        }
        public string fr_equip_bd
        {
            get
            {
                return _fr_equip_bd;
            }
            set
            {
                _fr_equip_bd = value;
            }
        }
        public string fr_loca
        {
            get
            {
                return _fr_loca;
            }
            set
            {
                _fr_loca = value;
            }
        }
        public int fr_dwg_id
        {
            get
            {
                return _fr_dwg_id;
            }
            set
            {
                _fr_dwg_id = value;
            }
        }
        public string fr_dwg
        {
            get
            {
                return _fr_dwg;
            }
            set
            {
                _fr_dwg = value;
            }
        }
        public string fr_dwg_rev
        {
            get
            {
                return _fr_dwg_rev;
            }
            set
            {
                _fr_dwg_rev = value;
            }
        }
        public int to_equip_id
        {
            get
            {
                return _to_equip_id;
            }
            set
            {
                _to_equip_id = value;
            }
        }
        public string to_equip
        {
            get
            {
                return _to_equip;
            }
            set
            {
                _to_equip = value;
            }
        }
        public string to_equip_bd
        {
            get
            {
                return _to_equip_bd;
            }
            set
            {
                _to_equip_bd = value;
            }
        }
        public string to_loca
        {
            get
            {
                return _to_loca;
            }
            set
            {
                _to_loca = value;
            }
        }
        public int to_dwg_id
        {
            get
            {
                return _to_dwg_id;
            }
            set
            {
                _to_dwg_id = value;
            }
        }
        public string to_dwg
        {
            get
            {
                return _to_dwg;
            }
            set
            {
                _to_dwg = value;
            }
        }
        public string to_dwg_rev
        {
            get
            {
                return _to_dwg_rev;
            }
            set
            {
                _to_dwg_rev = value;
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
        public bool comp_chkd
        {
            get
            {
                return _comp_chkd;
            }
            set
            {
                _comp_chkd = value;
            }
        }
        #endregion
    }
}
