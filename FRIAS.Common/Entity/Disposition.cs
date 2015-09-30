using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain disposition records
    /// </summary>
    public class Disposition : BaseEntity
    {
        #region private variables
        private int _disp_id;                                                               // disposition id
        private string _disp;                                                               // disposition
        private string _disp_desc;                                                          // disposition description
        private bool _used;                                                                 // check to see if the disposition has been used (fa prep_by !=null)
        private ArrayList _facompcablist;                                                   // fire area comp & cab disp (use Entity.FACabDisp)
        #endregion

        public Disposition(){}
        public Disposition(Disposition obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public int disp_id
        {
            get
            {
                return _disp_id;
            }
            set
            {
                _disp_id = value;
            }
        }
        public string disp
        {
            get
            {
                return _disp;
            }
            set
            {
                _disp = value;
            }
        }
        public string disp_desc
        {
            get
            {
                return _disp_desc;
            }
            set
            {
                _disp_desc = value;
            }
        }
        public bool used
        {
            get
            {
                return _used;
            }
            set
            {
                _used = value;
            }
        }
        public ArrayList facompcablist
        {
            get
            {
                return _facompcablist;
            }
            set
            {
                _facompcablist = value;
            }
        }
        #endregion
    }
}
