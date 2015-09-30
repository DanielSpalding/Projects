using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain cable-route records
    /// </summary>
    public class CabRoute : BaseEntity
    {
        #region private variables
        private int _cable_id;                                                              // cable id
        private string _cable;                                                              // cable
        private int _node_id;                                                               // node id
        private string _node;                                                               // node
        private decimal _seq;                                                               // sequence
        private int _fz_id;                                                                 // fire zone id
        private string _fz;                                                                 // fire zone
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private bool _fa_chkd;                                                              // fire area checked?
        private int _dwg_id;                                                                // drawing id
        private string _dwg_ref;                                                            // drawing reference
        private string _dwg_rev;                                                            // drawing revision
        private string _col_ref;                                                            // column reference
        private string _add_del;                                                            // add or del of sequence
        private string _status;                                                             // status of the CabRoute
        private int _psbkrcab_id;                                                           // psbkrcab_id (used for Power Page)
        private bool _uncoord;                                                              // uncoordinated (used for Power Page)
        #endregion

        public CabRoute(){}
        public CabRoute(CabRoute obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
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
        public decimal seq
        {
            get
            {
                return _seq;
            }
            set
            {
                _seq = value;
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
        public string add_del
        {
            get
            {
                return _add_del;
            }
            set
            {
                _add_del = value;
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
        public int psbkrcab_id
        {
            get
            {
                return _psbkrcab_id;
            }
            set
            {
                _psbkrcab_id = value;
            }
        }
        public bool uncoord
        {
            get
            {
                return _uncoord;
            }
            set
            {
                _uncoord = value;
            }
        }
        #endregion
    }
}
