using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain block diagram vertex information
    /// </summary>
    public class Vertex : BaseEntity
    {
        #region private variables
        private string _name;
        private string _label;
        private string _location;
        #endregion

        public Vertex(){}
        public Vertex(Vertex obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public string label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }
        public string location
        {
            get
            {
                return _location;
            }
            set
            {
                _location = value;
            }
        }
        #endregion
    }
}
