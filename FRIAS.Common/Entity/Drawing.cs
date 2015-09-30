using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain drawing records
    /// </summary>
    public class Drawing : BaseEntity
    {
        #region private variables
        private int _dwg_id;                                                                // drawing id
        private string _dwg_ref;                                                            // drawing reference
        private string _dwg_rev;                                                            // drawing revision
        private string _dwg_cp;
        private string _status;                                                             // drawing status
        private string _dwg_type;                                                           // type of drawing
        private string _dwg_desc;                                                           // drawing description
        private string _dwg_sys;                                                            // drawing system
        private string _dwg_unit;                                                           // drawing unit
        private string _dwg_location;                                                       // drawing location (can be a link)
        private string _dwg_status;                                                         // drawing status
        private ArrayList _componentlist;                                                   // list of associated components (use Entity.CompDwg)
        private ArrayList _cablelist;                                                       // list of associated cables (use Entity.CabDwg)
        private ArrayList _routelocalist;                                                   // list of associated node location drawing (use Entity.RouteDwg)
        #endregion

        public Drawing(){}
        public Drawing(Drawing obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
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
        public string dwg_cp
        {
            get
            {
                return _dwg_cp;
            }
            set
            {
                _dwg_cp = value;
            }
        }
        public string status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public string dwg_type
        {
            get
            {
                return _dwg_type;
            }
            set
            {
                _dwg_type = value;
            }
        }
        public string dwg_desc
        {
            get
            {
                return _dwg_desc;
            }
            set
            {
                _dwg_desc = value;
            }
        }
        public string dwg_sys
        {
            get
            {
                return _dwg_sys;
            }
            set
            {
                _dwg_sys = value;
            }
        }
        public string dwg_unit
        {
            get
            {
                return _dwg_unit;
            }
            set
            {
                _dwg_unit = value;
            }
        }
        public string dwg_location
        {
            get
            {
                return _dwg_location;
            }
            set
            {
                _dwg_location = value;
            }
        }
        public string dwg_status
        {
            get
            {
                return _dwg_status;
            }
            set
            {
                _dwg_status = value;
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
        public ArrayList routelocalist
        {
            get
            {
                return _routelocalist;
            }
            set
            {
                _routelocalist = value;
            }
        }
        #endregion
    }
}
