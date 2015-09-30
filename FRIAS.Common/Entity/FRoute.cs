using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire zone - route records
    /// </summary>
    public class FRoute : BaseEntity
    {
        #region private variables
        private int _fz_id;                                                                 // fire zone id
        private string _fz;                                                                 // fire zone
        private int _node_id;                                                               // node id
        private string _node;                                                               // node
        #endregion
        
        public FRoute(){}
        public FRoute(FRoute obj)
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
        #endregion
    }
}
