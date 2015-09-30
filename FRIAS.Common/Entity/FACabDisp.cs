using System;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire area - cable - disposition records
    /// </summary>
    public class FACabDisp : BaseEntity
    {
        #region private variables
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private int _comp_id;                                                               // component id
        private string _comp;                                                               // component
        private string _unit;                                                               // unit
        private string _sys;                                                                // system
        private string _train;                                                              // train
        private string _comp_desc;                                                          // component description
        private int _cable_id;                                                              // cable id
        private string _cable;                                                              // cable
        private int _old_disp_id;                                                           // old disposition id (needed to update disp_id)
        private string _old_disp;                                                           // old disposition (needed to reload disp if invalid disp entered)
        private int _disp_id;                                                               // disposition id
        private string _disp;                                                               // disposition
        private string _disp_desc;                                                          // disposition description
        private string _status;                                                             // status
        private string _fault;                                                              // component cable fault code
        private DateTime _chkd_date;                                                        // date fa checked
        #endregion

        public FACabDisp(){}
        public FACabDisp(FACabDisp obj)
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
        public string unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
            }
        }
        public string sys
        {
            get
            {
                return _sys;
            }
            set
            {
                _sys = value;
            }
        }
        public string train
        {
            get
            {
                return _train;
            }
            set
            {
                _train = value;
            }
        }
        public string comp_desc
        {
            get
            {
                return _comp_desc;
            }
            set
            {
                _comp_desc = value;
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
        public int old_disp_id
        {
            get
            {
                return _old_disp_id;
            }
            set
            {
                _old_disp_id = value;
            }
        }
        public string old_disp
        {
            get
            {
                return _old_disp;
            }
            set
            {
                _old_disp = value;
            }
        }
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
        public string status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
            }
        }
        public string fault
        {
            get
            {
                return _fault;
            }
            set
            {
                _fault = value;
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
