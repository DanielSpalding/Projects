using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain power source records
    /// </summary>
    public class Power : BaseEntity
    {
        #region private variables
        private int _power_id;                                                              // power source id
        private string _power;                                                              // power source (itself a comp)
        private string _power_desc;                                                         // description
        private bool _ssd_req;                                                              // required for ssd
        private bool _pra_req;                                                              // required for pra
        private bool _npo_req;                                                              // required for npo
        private bool _cfp_req;                                                              // required for cfp
        private ArrayList _componentlist;                                                   // component load list (use Entity.CompPwr)
        private ArrayList _psloadlist;                                                      // power source load list (use Entity.PSLoad)
        private ArrayList _psbkrcablist;                                                    // ps breaker cable list (use Entity.PSBkrCab)
        private ArrayList _cabroutelist;                                                    // route list for a cable (use Entity.CabRoute)
        #endregion

        public Power(){}
        public Power(Power obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public int power_id
        {
            get
            {
                return _power_id;
            }
            set
            {
                _power_id = value;
            }
        }
        public string power
        {
            get
            {
                return _power;
            }
            set
            {
                _power = value;
            }
        }
        public string power_desc
        {
            get
            {
                return _power_desc;
            }
            set
            {
                _power_desc = value;
            }
        }
        public bool ssd_req
        {
            get
            {
                return _ssd_req;
            }
            set
            {
                _ssd_req = value;
            }
        }
        public bool pra_req
        {
            get
            {
                return _pra_req;
            }
            set
            {
                _pra_req = value;
            }
        }
        public bool npo_req
        {
            get
            {
                return _npo_req;
            }
            set
            {
                _npo_req = value;
            }
        }
        public bool cfp_req
        {
            get
            {
                return _cfp_req;
            }
            set
            {
                _cfp_req = value;
            }
        }
        public ArrayList componentlist
        {
            get
            {
                return _componentlist;
            }
            set
            {
                _componentlist = value;
            }
        }
        public ArrayList psloadlist
        {
            get
            {
                return _psloadlist;
            }
            set
            {
                _psloadlist = value;
            }
        }
        public ArrayList psbkrcablist
        {
            get
            {
                return _psbkrcablist;
            }
            set
            {
                _psbkrcablist = value;
            }
        }
        public ArrayList cabroutelist
        {
            get
            {
                return _cabroutelist;
            }
            set
            {
                _cabroutelist = value;
            }
        }
        #endregion
    }
}
