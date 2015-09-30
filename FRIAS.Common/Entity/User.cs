using System;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain user records
    /// </summary>
    public class User
    {
        public const int deactivated = 0;                                                   // deactivated user, not allowed to login
        public const int read_only = 1;                                                     // user with ready only access
        public const int partial_edit = 2;                                                  // partial/limited edit, cannot add/delete objects or edit certain fields
        public const int full_edit = 3;                                                     // full-edit, cannot unlock checked objects
        public const int admin = 4;                                                         // system administrator

        #region private vairables
        private string _name;                                                               // name of the user
        private string _initial;                                                            // user initials
        private string _lname;                                                              // last name
        private string _fname;                                                              // first name
        private string _password;                                                           // password
        private int _level;                                                                 // access level
        private string _plantDBStr;
        private string _plant;                                                              // plant name
        private string _plant_desc;                                                         // plant description
        private int _locaType;                                                              // 1=Fire Zone, 2=Fire Area, 3=Fire Room
        private string _logo;
        private DateTime _logondate;                                                        // date last logged in
        private DateTime _logoffdate;                                                       // date last logged off
        private bool _loggedin;                                                             // log in status
        private string _ipaddress;                                                          // IP Address of user
        private int _idattempt;                                                             // number of attempts to log in by user
        #endregion

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
        public string initial
        {
            get
            {
                return _initial;
            }
            set
            {
                _initial = value;
            }
        }
        public string lname
        {
            get
            {
                return _lname;
            }
            set
            {
                _lname = value;
            }
        }
        public string fname
        {
            get
            {
                return _fname;
            }
            set
            {
                _fname = value;
            }
        }
        public string password
        {
            get
            {
                return _password;
            }
            set
            {
                _password = value;
            }
        }
        public int level
        {
            get
            {
                return _level;
            }
            set
            {
                _level = value;
            }
        }
        public string plantDBStr
        {
            get
            {
                return _plantDBStr;
            }
            set
            {
                _plantDBStr = value;
            }
        }
        public string plant
        {
            get
            {
                return _plant;
            }
            set
            {
                _plant = value;
            }
        }
        public string plant_desc
        {
            get
            {
                return _plant_desc;
            }
            set
            {
                _plant_desc = value;
            }
        }
        public int locaType
        {
            get
            {
                return _locaType;
            }
            set
            {
                _locaType = value;
            }
        }
        public string logo
        {
            get
            {
                return _logo;
            }
            set
            {
                _logo = value;
            }
        }
        public DateTime logondate
        {
            get
            {
                return _logondate;
            }
            set
            {
                _logondate = value;
            }
        }
        public DateTime logoffdate
        {
            get
            {
                return _logoffdate;
            }
            set
            {
                _logoffdate = value;
            }
        }
        public bool loggedin
        {
            get
            {
                return _loggedin;
            }
            set
            {
                _loggedin = value;
            }
        }
        public string ipaddress
        {
            get
            {
                return _ipaddress;
            }
            set
            {
                _ipaddress = value;
            }
        }
        public int idattempt
        {
            get
            {
                return _idattempt;
            }
            set
            {
                _idattempt = value;
            }
        }
        #endregion
    }
}
