using System;
using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire zone - ignition source - impact items
    /// </summary>
    public class FZIgnitionImpact : BaseEntity
    {
        #region private variables
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private int _fz_id;                                                                 // fire zone id
        private string _fz;                                                                 // fire zone
        private string _ig;                                                                 // ignition source
        private int _item_id;                                                               // item id
        private string _item;                                                               // item
        private string _item_table;
        private bool _wrap;
        #endregion
        
        public FZIgnitionImpact(){}
        public FZIgnitionImpact(FZIgnitionImpact obj)
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
        public string ig
        {
            get
            {
                return _ig;
            }
            set
            {
                _ig = value;
            }
        }
        public int item_id
        {
            get
            {
                return _item_id;
            }
            set
            {
                _item_id = value;
            }
        }
        public string item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }
        public string item_table
        {
            get
            {
                return _item_table;
            }
            set
            {
                _item_table = value;
            }
        }
        public bool wrap
        {
            get
            {
                return _wrap;
            }
            set
            {
                _wrap = value;
            }
        }
        #endregion
    }
}
