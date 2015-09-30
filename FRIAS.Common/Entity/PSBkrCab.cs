using System;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain power source - breaker - cable records
    /// </summary>
    public class PSBkrCab : BaseEntity
    {
        #region private variables
        private int _psbkrcab_id;                                                           // power source breaker cable id
        private int _psload_id;                                                             // power source load id
        private int _cable_id;                                                              // cable id
        private string _cable;                                                              // cable name
        private string _cable_len;                                                          // length of cable
        private string _cable_size;                                                         // size of cable
        private string _fr_equip;                                                           // from endpoint
        private string _to_equip;                                                           // to endpoint
        private string _prep_by;                                                            // prepared by
        private DateTime _prep_date;                                                        // date prepared
        private string _chkd_by;                                                            // checked by
        private DateTime _chkd_date;                                                        // date checked
        #endregion

        public PSBkrCab(){}
        public PSBkrCab(PSBkrCab obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public int psbkrcab_id
        {
            get
            {
                return _psbkrcab_id;
            }
            set
            {
                _psbkrcab_id = value;
            }
        }
        public int psload_id
        {
            get
            {
                return _psload_id;
            }
            set
            {
                _psload_id = value;
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
        public string cable_len
        {
            get
            {
                return _cable_len;
            }
            set
            {
                _cable_len = value;
            }
        }
        public string cable_size
        {
            get
            {
                return _cable_size;
            }
            set
            {
                _cable_size = value;
            }
        }
        public string fr_equip
        {
            get
            {
                return _fr_equip;
            }
            set
            {
                _fr_equip = value;
            }
        }
        public string to_equip
        {
            get
            {
                return _to_equip;
            }
            set
            {
                _to_equip = value;
            }
        }
        public string prep_by
        {
            get
            {
                return _prep_by;
            }
            set
            {
                _prep_by = value;
            }
        }
        public DateTime prep_date
        {
            get
            {
                return _prep_date;
            }
            set
            {
                _prep_date = value;
            }
        }
        public string chkd_by
        {
            get
            {
                return _chkd_by;
            }
            set
            {
                _chkd_by = value;
            }
        }
        public DateTime chkd_date
        {
            get
            {
                return _chkd_date;
            }
            set
            {
                _chkd_date = value;
            }
        }
        #endregion
    }
}
