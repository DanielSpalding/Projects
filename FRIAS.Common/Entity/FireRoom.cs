using System;
using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire zone records
    /// </summary>
    public class FireRoom : BaseEntity
    {
        #region private variables
        private int _rm_id;                                                                 // fire zone id
        private string _rm;                                                                 // fire zone
        private string _rm_desc;                                                            // fire zone description
        private string _bldg;                                                               // building
        private string _elev;                                                               // elevation
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
        private int _fz_id;                                                                 // corresponding fire area id
        private string _fz;                                                                 // corresponding fire area
        private bool _fa_chkd;                                                              // date checked
        private ArrayList _componentlist;                                                   // list of components (use Entity.FZComp)
        private ArrayList _cablelist;                                                       // list of cables (use Entity.FZCab)
        private ArrayList _routelist;                                                       // list of nodes (use Entity.FZRoute)
        #endregion

        public FireRoom(){}
        public FireRoom(FireZone obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public int rm_id
        {
            get
            {
                return _rm_id;
            }
            set
            {
                _rm_id = value;
            }
        }
        public string rm
        {
            get
            {
                return _rm;
            }
            set
            {
                _rm = value;
            }
        }
        public string rm_desc
        {
            get
            {
                return _rm_desc;
            }
            set
            {
                _rm_desc = value;
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
        #endregion
    }
}
