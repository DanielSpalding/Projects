using System;
using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain component records
    /// </summary>
    public class Component : BaseEntity
    {
        #region private variables
        private int _comp_id;                                                               // component id
        private string _comp;                                                               // component
        private bool _is_ps;                                                                // power source?
        private int _box_id;                                                                // box where the package is located
        private string _pri_sub;                                                            // primary or sub
        private int _unit_id;                                                               // unit id
        private string _unit;                                                               // unit
        private int _sys_id;                                                                // system id
        private string _sys;                                                                // system
        private int _train_id;                                                              // train id
        private string _train;                                                              // train
        private int _comp_type_id;                                                          // component type
        private string _comp_type;                                                          // component type
        private string _comp_desc;                                                          // component description
        private string _path;                                                               // component path
        private string _location;                                                           // location (FA) of component
        private string _np;                                                                 // normal position
        private int _np_id;                                                                 // normal posiiton id
        private string _hsbp;                                                               // hot standby position
        private int _hsbp_id;                                                               // hot standby position id
        private string _hsp;                                                                // hot shutdown position
        private int _hsp_id;                                                                // hot shutdown position id
        private string _csp;                                                                // cold shutdown position
        private int _csp_id;                                                                // cold shutdown position id
        private string _prap;                                                               // pra position
        private int _prap_id;                                                               // pra position id
        private string _cfpp;                                                               // cfp position
        private int _cfpp_id;                                                               // cfp posiiton id
        private string _npop;                                                               // npo position
        private int _npop_id;                                                               // npo position id
        private string _npo_np;                                                             // normal npo position
        private int _npo_np_id;                                                             // normal npo posiiton id
        private int _pid_id;                                                                // pid drawing id
        private string _pid_ref;                                                            // pid drawing reference
        private string _pid_rev;                                                            // pid drawing revision
        private string _pid_cp;
        private int _ee_id;                                                                 // shecmatic drawing id
        private string _ee_ref;                                                             // schematic drawing reference
        private string _ee_rev;                                                             // schematic drawing revision
        private string _ee_cp;
        private int _ol_id;                                                                 // one-line drawing id
        private string _ol_ref;                                                             // one-line drawing reference
        private string _ol_rev;                                                             // one-line drawing revision
        private string _ol_cp;
        private bool _hi_lo;                                                                // hi-low pressure interface valves
        private string _fail_elect;                                                         // fail electrical position
        private int _fail_elect_id;                                                         // fail elecrical position id
        private string _fail_air;                                                           // fail air position
        private int _fail_air_id;                                                           // fail air position id
        private bool _ssd_req;                                                              // required for ssd
        private bool _pra_req;                                                              // required for pra
        private bool _npo_req;                                                              // required for npo
        private bool _cfp_req;                                                              // required for cfp
        private bool _nsca_req;                                                             // required for nsca
        private int _method_id;                                                             // ssd method id
        private string _method;                                                             // ssd method
        private string _comment;                                                            // comments
        private string _prep_by;                                                            // prepared by
        private DateTime _prep_date;                                                        // date prepared
        private string _chkd_by;                                                            // checked by
        private DateTime _chkd_date;                                                        // date checked
        private ArrayList _drawinglist;                                                     // list of associated drawings (use Entity.CompDwg)
        private ArrayList _powerlist;                                                       // list of associated power sources (use Entity.CompPwr)
        private ArrayList _subcomplist;                                                     // list of associated subcomponents (use Entity.Subcomp)
        private ArrayList _pricomplist;                                                     // list of associated primarycomponents (use Entity.Subcomp)
        private ArrayList _incomingintlklist;                                               // list of associated incoming interlocks (use Entity.CompIntlk)
        private ArrayList _outgoingintlklist;                                               // list of associated incoming interlocks (use Entity.CompIntlk)
        private ArrayList _cablelist;                                                       // list of associated cables (use Entity.CompCab)
        private ArrayList _loadlist;                                                        // list of associated components being fed (use Entity.CompPwr)
        private ArrayList _belist;                                                          // list of assocuated basic events (use Entity.CompBE)
        private ArrayList _ksflist;                                                         // list of key safety functions (use Entity.CompKSF)
        private ArrayList _facompdisplist;                                                  // list of fire are component dispositions (use Entity.FACompDisp)
        private ArrayList _ralist;                                                          // list of recovery actions (use Entity.CompRA)
        private Component _argold;                                                          // component before update
        #endregion

        public Component(){}
        public Component(Component oldobj)
        {
            PropertyInfo[] p = oldobj.GetType().GetProperties();                            // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(oldobj, null), null);                 // set entity's property values to obj properties
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
        public bool is_ps
        {
            get
            {
                return _is_ps;
            }
            set
            {
                _is_ps = value;
            }
        }
        public int box_id
        {
            get
            {
                return _box_id;
            }
            set
            {
                _box_id = value;
            }
        }
        public string pri_sub
        {
            get
            {
                return _pri_sub;
            }
            set
            {
                _pri_sub = value;
            }
        }
        public int unit_id
        {
            get
            {
                return _unit_id;
            }
            set
            {
                _unit_id = value;
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
        public int sys_id
        {
            get
            {
                return _sys_id;
            }
            set
            {
                _sys_id = value;
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
        public int train_id
        {
            get
            {
                return _train_id;
            }
            set
            {
                _train_id = value;
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
        public int comp_type_id
        {
            get
            {
                return _comp_type_id;
            }
            set
            {
                _comp_type_id = value;
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
        public string path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }
        public string location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        public string np
        {
            get
            {
                return _np;
            }
            set
            {
                _np = value;
            }
        }
        public int np_id
        {
            get
            {
                return _np_id;
            }
            set
            {
                _np_id = value;
            }
        }
        public string hsbp
        {
            get
            {
                return _hsbp;
            }
            set
            {
                _hsbp = value;
            }
        }
        public int hsbp_id
        {
            get
            {
                return _hsbp_id;
            }
            set
            {
                _hsbp_id = value;
            }
        }
        public string hsp
        {
            get
            {
                return _hsp;
            }
            set
            {
                _hsp = value;
            }
        }
        public int hsp_id
        {
            get
            {
                return _hsp_id;
            }
            set
            {
                _hsp_id = value;
            }
        }
        public string csp
        {
            get
            {
                return _csp;
            }
            set
            {
                _csp = value;
            }
        }
        public int csp_id
        {
            get
            {
                return _csp_id;
            }
            set
            {
                _csp_id = value;
            }
        }
        public string prap
        {
            get
            {
                return _prap;
            }
            set
            {
                _prap = value;
            }
        }
        public int prap_id
        {
            get
            {
                return _prap_id;
            }
            set
            {
                _prap_id = value;
            }
        }
        public string cfpp
        {
            get
            {
                return _cfpp;
            }
            set
            {
                _cfpp = value;
            }
        }
        public int cfpp_id
        {
            get
            {
                return _cfpp_id;
            }
            set
            {
                _cfpp_id = value;
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
        public int npop_id
        {
            get
            {
                return _npop_id;
            }
            set
            {
                _npop_id = value;
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
        public int npo_np_id
        {
            get
            {
                return _npo_np_id;
            }
            set
            {
                _npo_np_id = value;
            }
        }
        public int pid_id
        {
            get
            {
                return _pid_id;
            }
            set
            {
                _pid_id = value;
            }
        }
        public string pid_ref
        {
            get
            {
                return _pid_ref;
            }
            set
            {
                _pid_ref = value;
            }
        }
        public string pid_rev
        {
            get
            {
                return _pid_rev;
            }
            set
            {
                _pid_rev = value;
            }
        }
        public string pid_cp
        {
            get
            {
                return _pid_cp;
            }
            set
            {
                _pid_cp = value;
            }
        }
        public int ee_id
        {
            get
            {
                return _ee_id;
            }
            set
            {
                _ee_id = value;
            }
        }
        public string ee_ref
        {
            get
            {
                return _ee_ref;
            }
            set
            {
                _ee_ref = value;
            }
        }
        public string ee_rev
        {
            get
            {
                return _ee_rev;
            }
            set
            {
                _ee_rev = value;
            }
        }
        public string ee_cp
        {
            get
            {
                return _ee_cp;
            }
            set
            {
                _ee_cp = value;
            }
        }
        public int ol_id
        {
            get
            {
                return _ol_id;
            }
            set
            {
                _ol_id = value;
            }
        }
        public string ol_ref
        {
            get
            {
                return _ol_ref;
            }
            set
            {
                _ol_ref = value;
            }
        }
        public string ol_rev
        {
            get
            {
                return _ol_rev;
            }
            set
            {
                _ol_rev = value;
            }
        }
        public string ol_cp
        {
            get
            {
                return _ol_cp;
            }
            set
            {
                _ol_cp = value;
            }
        }
        public bool hi_lo
        {
            get
            {
                return _hi_lo;
            }
            set
            {
                _hi_lo = value;
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
        public int fail_elect_id
        {
            get
            {
                return _fail_elect_id;
            }
            set
            {
                _fail_elect_id = value;
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
        public int fail_air_id
        {
            get
            {
                return _fail_air_id;
            }
            set
            {
                _fail_air_id = value;
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
        public bool nsca_req
        {
            get
            {
                return _nsca_req;
            }
            set
            {
                _nsca_req = value;
            }
        }
        public int method_id
        {
            get
            {
                return _method_id;
            }
            set
            {
                _method_id = value;
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
        public string prep_by
        {
            get
            {
                return _prep_by;
            }
            set
            {
                _prep_by = value;
            }
        }
        public DateTime prep_date
        {
            get
            {
                return _prep_date;
            }
            set
            {
                _prep_date = value;
            }
        }
        public string chkd_by
        {
            get
            {
                return _chkd_by;
            }
            set
            {
                _chkd_by = value;
            }
        }
        public DateTime chkd_date
        {
            get
            {
                return _chkd_date;
            }
            set
            {
                _chkd_date = value;
            }
        }
        public ArrayList drawinglist
        {
            get
            {
                return _drawinglist;
            }
            set
            {
                _drawinglist = value;
            }
        }
        public ArrayList powerlist
        {
            get
            {
                return _powerlist;
            }
            set
            {
                _powerlist = value;
            }
        }
        public ArrayList subcomplist
        {
            get
            {
                return _subcomplist;
            }
            set
            {
                _subcomplist = value;
            }
        }
        public ArrayList pricomplist
        {
            get
            {
                return _pricomplist;
            }
            set
            {
                _pricomplist = value;
            }
        }
        public ArrayList incomingintlklist
        {
            get
            {
                return _incomingintlklist;
            }
            set
            {
                _incomingintlklist = value;
            }
        }
        public ArrayList cablelist
        {
            get
            {
                return _cablelist;
            }
            set
            {
                _cablelist = value;
            }
        }
        public ArrayList loadlist
        {
            get
            {
                return _loadlist;
            }
            set
            {
                _loadlist = value;
            }
        }
        public ArrayList outgoingintlklist
        {
            get
            {
                return _outgoingintlklist;
            }
            set
            {
                _outgoingintlklist = value;
            }
        }
        public ArrayList ksflist
        {
            get
            {
                return _ksflist;
            }
            set
            {
                _ksflist = value;
            }
        }
        public ArrayList belist
        {
            get
            {
                return _belist;
            }
            set
            {
                _belist = value;
            }
        }
        public ArrayList facompdisplist
        {
            get
            {
                return _facompdisplist;
            }
            set
            {
                _facompdisplist = value;
            }
        }
        public ArrayList ralist
        {
            get
            {
                return _ralist;
            }
            set
            {
                _ralist = value;
            }
        }
        public Component argold
        {
            get
            {
                return _argold;
            }
            set
            {
                _argold = value;
            }
        }
        #endregion

    }
}
