using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain component-basic event records
    /// </summary>
    public class CompBE : BaseEntity
    {
        #region private variables
        private int _comp_id;                                                               // component id
        private int _be_id;                                                                 // basic event id
        private string _be;                                                                 // basic event
        private string _be_desc;                                                            // basic even description
        private string _code;                                                               // code for basic event
        private string _pra_id;                                                             // link/pra id
        #endregion

        public CompBE(){}
        public CompBE(CompBE obj)
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
        public int be_id
        {
            get
            {
                return _be_id;
            }
            set
            {
                _be_id = value;
            }
        }
        public string be
        {
            get
            {
                return _be;
            }
            set
            {
                _be = value;
            }
        }
        public string be_desc
        {
            get
            {
                return _be_desc;
            }
            set
            {
                _be_desc = value;
            }
        }
        public string code
        {
            get
            {
                return _code;
            }
            set
            {
                _code = value;
            }
        }
        public string pra_id
        {
            get
            {
                return _pra_id;
            }
            set
            {
                _pra_id = value;
            }
        }
        #endregion
    }
}
