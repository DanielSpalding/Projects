using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain component-recover action records
    /// </summary>
    public class CompRA : BaseEntity
    {
        #region private variables
        private int _comp_id;                                                               // component id
        private string _comp;                                                               // component
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private string _ra_disp;                                                            // recovery action disposition
        private string _bin;                                                                // action bin
        private string _ra_type;                                                            // recovery action type
        private string _ra_feasible;                                                        // recovery action feasible
        #endregion

        public CompRA(){}
        public CompRA(CompRA obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
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
        public string ra_disp
        {
            get
            {
                return _ra_disp;
            }
            set
            {
                _ra_disp = value;
            }
        }
        public string bin
        {
            get
            {
                return _bin;
            }
            set
            {
                _bin = value;
            }
        }
        public string ra_type
        {
            get
            {
                return _ra_type;
            }
            set
            {
                _ra_type = value;
            }
        }
        public string ra_feasible
        {
            get
            {
                return _ra_feasible;
            }
            set
            {
                _ra_feasible = value;
            }
        }
        #endregion
    }
}
