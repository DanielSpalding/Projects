using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain component-subcomp records
    /// </summary>
    public class Subcomp : BaseEntity
    {
        #region private variables
        private string _subcomp;                                                            // subcomponent
        private int _comp_id;                                                               // component id
        private string _comp;                                                               // component
        private string _note;                                                               // subcomp note
        #endregion

        public Subcomp(){}
        public Subcomp(Subcomp obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public string subcomp
        {
            get
            {
                return _subcomp;
            }
            set
            {
                _subcomp = value;
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
        public string note
        {
            get
            {
                return _note;
            }
            set
            {
                _note = value;
            }
        }
        #endregion
    }
}
