using System;
using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain component records
    /// </summary>
    public class CableBlock : BaseEntity
    {
        #region private variables
        private int _comp_id;                                                               // component id
        private string _comp;                                                               // component
        private bool _is_ps;                                                                // power source?
        private string _pri_sub;                                                            // primary or sub
        private string _unit;                                                               // unit
        private string _sys;                                                                // system
        private string _train;                                                              // train
        private string _comp_type;                                                          // component type
        private string _comp_desc;                                                          // component description
        private string _np;                                                                 // normal position
        private string _hsbp;                                                               // hot standby position
        private string _hsp;                                                                // hot shutdown position
        private string _csp;                                                                // cold shutdown position
        private string _prap;                                                               // pra position
        private string _cfpp;                                                               // cfp position
        private string _npop;                                                               // npo position
        private string _npo_np;                                                             // normal npo position
        private bool _hi_lo;                                                                // hi-low pressure interface valves
        private string _fail_elect;                                                         // fail electrical position
        private string _fail_air;                                                           // fail air position
        private bool _ssd_req;                                                              // required for ssd
        private bool _pra_req;                                                              // required for pra
        private bool _npo_req;                                                              // required for npo
        private bool _cfp_req;                                                              // required for cfp
        private ArrayList _cablelist;                                                       // list of associated cables (use Entity.CompCab)
        private ArrayList _vertexlist;
        private ArrayList _edgelist;
        #endregion

        public CableBlock(){}
        public CableBlock(CableBlock oldobj)
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
        public ArrayList vertexlist
        {
            get
            {
                return _vertexlist;
            }
            set
            {
                _vertexlist = value;
            }
        }
        public ArrayList edgelist
        {
            get
            {
                return _edgelist;
            }
            set
            {
                _edgelist = value;
            }
        }
        #endregion

    }
}
