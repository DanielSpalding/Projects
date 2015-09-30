using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire zone - component - cable records
    /// </summary>
    public class FCab : BaseEntity
    {
        #region private variables
        private int _fz_id;                                                                 // fire zone id
        private string _fz;                                                                 // fire zone
        private int _comp_id;                                                               // component id
        private string _comp;                                                               // component
        private int _cable_id;                                                              // cable id
        private string _cable;                                                              // cable
        #endregion

        public FCab(){}
        public FCab(FCab obj)
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
        #endregion
    }
}
