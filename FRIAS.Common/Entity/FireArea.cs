using System;
using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire area records
    /// </summary>
    public class FireArea : BaseEntity
    {
        #region private variables
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private string _fa_desc;                                                            // fire area description
        private string _bldg;                                                               // building
        private string _elev;                                                               // elevation
        private string _ssd_path;                                                           // fire are shutdown path
        private bool _req;                                                                  // required?
        private string _supp;                                                               // suppression
        private string _det;                                                                // detection
        private int _dwg_id;                                                                // drawing id
        private string _dwg_ref;                                                            // drawing reference
        private string _dwg_rev;                                                            // fa-dwg revision
        private string _comment;                                                            // comment
        private string _prep_by;                                                            // prepared by
        private DateTime _prep_date;                                                        // date prepared
        private string _chkd_by;                                                            // checked by
        private DateTime _chkd_date;                                                        // date checked
        private ArrayList _fzlist;                                                          // list of associated fire zones
        private ArrayList _compdisplist;                                                    // component disposition list (use Entity.FACompDisp)
        private ArrayList _cabdisplist;                                                     // cable disposition list (use Entity.FACabDisp)
        private ArrayList _cascintlklist;                                                   // cascading interlock list (use Entity.FACascHit)
        private ArrayList _cascpowerlist;                                                   // cascading power list (use Entity.FACascHit)
        private ArrayList _compcablist;                                                     // list of unique cables associated with component
        
        private string _risk_summary;                                                       // fire area risk summary
        private string _reg_basis;                                                          // regulatory basis
        private ArrayList _vfdrlist;                                                        // list of vfdrs
        private ArrayList _gendoclist;                                                      // list of references (general)
        private ArrayList _eedoclist;                                                       // list of references (engineering eval)
        private ArrayList _licdoclist;                                                      // list of references (licening req)
        private ArrayList _pglist;                                                          // list of performance goals
        private ArrayList _ignitionlist;                                                    // list of ignition sources
        private ArrayList _protectionlist;                                                   // list of classical fire protection info
        #endregion

        public FireArea(){}
        public FireArea(FireArea obj)
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
        public string fa_desc
        {
            get
            {
                return _fa_desc;
            }
            set
            {
                _fa_desc = value;
            }
        }
        public string bldg
        {
            get
            {
                return _bldg;
            }
            set
            {
                _bldg = value;
            }
        }
        public string elev
        {
            get
            {
                return _elev;
            }
            set
            {
                _elev = value;
            }
        }
        public string ssd_path
        {
            get
            {
                return _ssd_path;
            }
            set
            {
                _ssd_path = value;
            }
        }
        public bool req
        {
            get
            {
                return _req;
            }
            set
            {
                _req = value;
            }
        }
        public string supp
        {
            get
            {
                return _supp;
            }
            set
            {
                _supp = value;
            }
        }
        public string det
        {
            get
            {
                return _det;
            }
            set
            {
                _det = value;
            }
        }
        public int dwg_id
        {
            get
            {
                return _dwg_id;
            }
            set
            {
                _dwg_id = value;
            }
        }
        public string dwg_ref
        {
            get
            {
                return _dwg_ref;
            }
            set
            {
                _dwg_ref = value;
            }
        }
        public string dwg_rev
        {
            get
            {
                return _dwg_rev;
            }
            set
            {
                _dwg_rev = value;
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
        public ArrayList fzlist
        {
            get
            {
                return _fzlist;
            }
            set
            {
                _fzlist = value;
            }
        }
        public ArrayList compdisplist
        {
            get
            {
                return _compdisplist;
            }
            set
            {
                _compdisplist = value;
            }
        }
        public ArrayList cabdisplist
        {
            get
            {
                return _cabdisplist;
            }
            set
            {
                _cabdisplist = value;
            }
        }
        public ArrayList cascintlklist
        {
            get
            {
                return _cascintlklist;
            }
            set
            {
                _cascintlklist = value;
            }
        }
        public ArrayList cascpowerlist
        {
            get
            {
                return _cascpowerlist;
            }
            set
            {
                _cascpowerlist = value;
            }
        }
        public ArrayList compcablist
        {
            get
            {
                return _compcablist;
            }
            set
            {
                _compcablist = value;
            }
        }
        public string risk_summary
        {
            get
            {
                return _risk_summary;
            }
            set
            {
                _risk_summary = value;
            }
        }
        public string reg_basis
        {
            get
            {
                return _reg_basis;
            }
            set
            {
                _reg_basis = value;
            }
        }
        public ArrayList vfdrlist
        {
            get
            {
                return _vfdrlist;
            }
            set
            {
                _vfdrlist = value;
            }
        }
        public ArrayList gendoclist
        {
            get
            {
                return _gendoclist;
            }
            set
            {
                _gendoclist = value;
            }
        }
        public ArrayList eedoclist
        {
            get
            {
                return _eedoclist;
            }
            set
            {
                _eedoclist = value;
            }
        }
        public ArrayList licdoclist
        {
            get
            {
                return _licdoclist;
            }
            set
            {
                _licdoclist = value;
            }
        }
        public ArrayList pglist
        {
            get
            {
                return _pglist;
            }
            set
            {
                _pglist = value;
            }
        }
        public ArrayList ignitionlist
        {
            get
            {
                return _ignitionlist;
            }
            set
            {
                _ignitionlist = value;
            }
        }
        public ArrayList protectionlist
        {
            get
            {
                return _protectionlist;
            }
            set
            {
                _protectionlist = value;
            }
        }
        #endregion
    }
}
