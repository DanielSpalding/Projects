using System;
using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire zone records
    /// </summary>
    public class FireZone : BaseEntity
    {
        #region private variables
        private int _fz_id;                                                                 // fire zone id
        private string _fz;                                                                 // fire zone
        private string _fz_desc;                                                            // fire zone description
        private string _bldg;                                                               // building
        private string _elev;                                                               // elevation
        private bool _req;                                                                  // required?
        private string _supp;                                                               // suppression
        private string _det;                                                                // detection
        private string _ventilation;
        private string _barrier;
        private string _otherprotection;
        private int _dwg_id;                                                                // drawing id
        private string _dwg_ref;                                                            // drawing reference
        private string _dwg_rev;                                                            // fa-dwg revision
        private string _comment;                                                            // comment
        private string _prep_by;                                                            // prepared by
        private DateTime _prep_date;                                                        // date prepared
        private string _chkd_by;                                                            // checked by
        private DateTime _chkd_date;                                                        // date checked
        private int _fa_id;                                                                 // corresponding fire area id
        private string _fa;                                                                 // corresponding fire area
        private bool _fa_chkd;                                                              // date checked
        private ArrayList _componentlist;                                                   // list of components (use Entity.FZComp)
        private ArrayList _cablelist;                                                       // list of cables (use Entity.FZCab)
        private ArrayList _routelist;                                                       // list of nodes (use Entity.FZRoute)
        private ArrayList _protectionlist;                                                  // list of fire protection
        private ArrayList _ignitionlist;                                                    // list of ignition sources
        private ArrayList _impactlist;
        #endregion

        public FireZone(){}
        public FireZone(FireZone obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
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
        public string fz_desc
        {
            get
            {
                return _fz_desc;
            }
            set
            {
                _fz_desc = value;
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
        public string ventilation
        {
            get
            {
                return _ventilation;
            }
            set
            {
                _ventilation = value;
            }
        }
        public string barrier
        {
            get
            {
                return _barrier;
            }
            set
            {
                _barrier = value;
            }
        }
        public string otherprotection
        {
            get
            {
                return _otherprotection;
            }
            set
            {
                _otherprotection = value;
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
        public bool fa_chkd
        {
            get
            {
                return _fa_chkd;
            }
            set
            {
                _fa_chkd = value;
            }
        }
        public ArrayList componentlist
        {
            get
            {
                return _componentlist;
            }
            set
            {
                _componentlist = value;
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
        public ArrayList routelist
        {
            get
            {
                return _routelist;
            }
            set
            {
                _routelist = value;
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
        public ArrayList impactlist
        {
            get
            {
                return _impactlist;
            }
            set
            {
                _impactlist = value;
            }
        }
        #endregion
    }
}
