using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain route/endpoint - location - drawing records
    /// </summary>
    public class RoutelocaDwg : BaseEntity
    {
        #region private variables
        private int _nodeloca_id;                                                           // node location id
        private int _node_id;                                                               // node id
        private string _node;                                                               // node
        private int _fz_id;                                                                 // fire zone id (location id)
        private string _fz;                                                                 // fire zone (location)
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private int _dwg_id;                                                                // drawing id
        private string _dwg_ref;                                                            // drawing reference
        private string _dwg_rev;                                                            // dwg revision
        private int _dwgtype_id;                                                            // dwg type id
        private string _dwgtype_desc;                                                       // drawing type
        private string _col_ref;                                                            // column reference
        #endregion

        public RoutelocaDwg(){}
        public RoutelocaDwg(RoutelocaDwg obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public int nodeloca_id
        {
            get
            {
                return _nodeloca_id;
            }
            set
            {
                _nodeloca_id = value;
            }
        }
        public int node_id
        {
            get
            {
                return _node_id;
            }
            set
            {
                _node_id = value;
            }
        }
        public string node
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
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
        public int dwgtype_id
        {
            get
            {
                return _dwgtype_id;
            }
            set
            {
                _dwgtype_id = value;
            }
        }
        public string dwgtype_desc
        {
            get
            {
                return _dwgtype_desc;
            }
            set
            {
                _dwgtype_desc = value;
            }
        }
        public string col_ref
        {
            get
            {
                return _col_ref;
            }
            set
            {
                _col_ref = value;
            }
        }
        #endregion
    }
}
