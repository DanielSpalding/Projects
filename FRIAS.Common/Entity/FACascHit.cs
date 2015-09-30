using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire area - cascading hit (intlk or psid) records
    /// </summary>
    public class FACascHit : BaseEntity
    {
        #region private variables
        private int _fa_id;                                                                 // fire area id
        private int _comp_id;                                                               // comp id
        private string _comp;                                                               // comp
        private string _failure;                                                            // failure cause
        #endregion

        public FACascHit(){}
        public FACascHit(FACascHit obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
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
        public string failure
        {
            get
            {
                return _failure;
            }
            set
            {
                _failure = value;
            }
        }
        #endregion
    }
}
