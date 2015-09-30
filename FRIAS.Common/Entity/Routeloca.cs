using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain route/endpoint - location records
    /// </summary>
    public class Routeloca : BaseEntity
    {

        #region private variables
        private int _nodeloca_id;                                                           // node location id
        private int _node_id;                                                               // node id
        private string _node;                                                               // node, endpoint
        private int _locatype_id;                                                           // location type id
        private int _rm_id;                                                                 // room id
        private string _rm;
        private int _fz_id;                                                                 // fire zone id (location id)
        private string _fz;                                                                 // fire zone (location)
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private bool _fa_chkd;                                                              // fire area checked?
        #endregion

        public Routeloca(){}
        public Routeloca(Routeloca obj)
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
        public int locatype_id
        {
            get
            {
                return _locatype_id;
            }
            set
            {
                _locatype_id = value;
            }
        }
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
        #endregion
    }
}
