using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain route/endpoint - block diagram endpoints records
    /// </summary>
    public class RouteBDEndpoint : BaseEntity
    {
        #region private variables
        private int _node_id;                                                               // node id
        private string _node;                                                               // node, endpoint
        private string _endpt;                                                              // from/to equip endpoint
        private string _bd_endpt;                                                           // block diagram endpoint
        private int _rm_id;
        private string _rm;
        private int _fz_id;                                                                 // fire zone id
        private string _fz;                                                                 // fire zone
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        #endregion

        public RouteBDEndpoint(){}
        public RouteBDEndpoint(RouteBDEndpoint obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
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
        public string endpt
        {
            get
            {
                return _endpt;
            }
            set
            {
                _endpt = value;
            }
        }
        public string bd_endpt
        {
            get
            {
                return _bd_endpt;
            }
            set
            {
                _bd_endpt = value;
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
        #endregion
    }
}
