using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain cable-drawing records
    /// </summary>
    public class CabDwg : BaseEntity
    {
        #region private variables
        private int _cable_id;                                                              // cable id
        private string _cable;                                                              // cable
        private int _comp_id;                                                               // component id
        private string _comp;                                                               // component
        private int _dwg_id;                                                                // drawing id
        private string _dwg_ref;                                                            // drawing reference
        private string _dwg_rev;                                                            // comp-dwg revision
        private string _dwg_cp;
        private int _dwgtype_id;                                                            // comp-dwg type id
        private string _dwgtype_desc;                                                       // drawing type
        private string _dwg_desc;                                                           // drawing description
        #endregion

        public CabDwg(){}
        public CabDwg(CabDwg obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
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
        public int dwg_id
        {
            get
            {
                return _dwg_id;
            }
            set
            {
                _dwg_id = value;
            }
        }
        public string dwg_ref
        {
            get
            {
                return _dwg_ref;
            }
            set
            {
                _dwg_ref = value;
            }
        }
        public string dwg_rev
        {
            get
            {
                return _dwg_rev;
            }
            set
            {
                _dwg_rev = value;
            }
        }
        public string dwg_cp
        {
            get
            {
                return _dwg_cp;
            }
            set
            {
                _dwg_cp = value;
            }
        }
        public int dwgtype_id
        {
            get
            {
                return _dwgtype_id;
            }
            set
            {
                _dwgtype_id = value;
            }
        }
        public string dwgtype_desc
        {
            get
            {
                return _dwgtype_desc;
            }
            set
            {
                _dwgtype_desc = value;
            }
        }
        public string dwg_desc
        {
            get
            {
                return _dwg_desc;
            }
            set
            {
                _dwg_desc = value;
            }
        }
        #endregion
    }
}
