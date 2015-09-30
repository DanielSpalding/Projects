using System;
using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain cable records
    /// </summary>
    public class Cable : BaseEntity
    {
        #region private variables
        private int _cable_id;                                                              // cable id
        private string _cable;                                                              // cable name
        private string _cable_orig;
        private string _fault;                                                              // cable fault code
        private string _cable_len;                                                          // length of cable
        private string _cable_size;                                                         // size of cable
        private string _cable_code;                                                         // cable code
        private string _fr_equip_orig;                                                      // from endpoint original
        private string _fr_equip_orig_loc;                                                  // from equipment original (plant specified)
        private int _fr_equip_id;                                                           // from equipment id (endpoint)
        private string _fr_equip;                                                           // from endpoint
        private string _fr_equip_loc;
        private string _fr_equip_bd;                                                        // from equipment block diagram
        private string _fr_dwg_orig;                                                        // from drawing original
        private int _fr_dwg_id;                                                             // from drawing id
        private string _fr_dwg;                                                             // from endpoint drawing reference
        private string _fr_dwg_rev;                                                         // from endpoint drawing revision
        private string _to_equip_orig;                                                      // to endpoint original
        private string _to_equip_orig_loc;                                                  // to equipment original (plant specified)
        private int _to_equip_id;                                                           // from equipment id (endpoint)
        private string _to_equip;                                                           // to endpoint
        private string _to_equip_loc;
        private string _to_equip_bd;                                                        // to equipment block diagram
        private string _to_dwg_orig;                                                        // to drawing original
        private int _to_dwg_id;                                                             // to drawing id
        private string _to_dwg;                                                             // to endpoint drawing reference
        private string _to_dwg_rev;                                                         // to endpoint drawing revision
        private string _note;                                                               // comment/note
        private string _prep_by;                                                            // prepared by
        private DateTime _prep_date;                                                        // date prepared
        private string _chkd_by;                                                            // checked by
        private DateTime _chkd_date;                                                        // date checked
        private string _comment_orig;                                                       // original comments from plant
        private string _comment;                                                            // comments
        private string _status;                                                             // status of cable (USED/NOT USED)
        private ArrayList _componentlist;                                                   // list of associated components (use Entity.CompCab)
        private ArrayList _drawinglist;                                                     // list of associated drawings (use Entity.CabDwg)
        private ArrayList _routelist;                                                       // list of node in the cable route/raceway: seq + node (use Entity.CabRoute)
        private ArrayList _routechglist;                                                    // list of changes to the nodes (use Entity.CabRoute)
        private ArrayList _crdpowerlist;                                                    // list of coordination power supplies (use Entity.PSLoad)
        #endregion

        public Cable(){}
        public Cable(Cable obj)
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
        public string cable_orig
        {
            get
            {
                return _cable_orig;
            }
            set
            {
                _cable_orig = value;
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
        public string cable_code
        {
            get
            {
                return _cable_code;
            }
            set
            {
                _cable_code = value;
            }
        }
        public string fr_equip_orig
        {
            get
            {
                return _fr_equip_orig;
            }
            set
            {
                _fr_equip_orig = value;
            }
        }
        public string fr_equip_orig_loc
        {
            get
            {
                return _fr_equip_orig_loc;
            }
            set
            {
                _fr_equip_orig_loc = value;
            }
        }
        public int fr_equip_id
        {
            get
            {
                return _fr_equip_id;
            }
            set
            {
                _fr_equip_id = value;
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
        public string fr_equip_loc
        {
            get
            {
                return _fr_equip_loc;
            }
            set
            {
                _fr_equip_loc = value;
            }
        }
        public string fr_equip_bd
        {
            get
            {
                return _fr_equip_bd;
            }
            set
            {
                _fr_equip_bd = value;
            }
        }
        public string fr_dwg_orig
        {
            get
            {
                return _fr_dwg_orig;
            }
            set
            {
                _fr_dwg_orig = value;
            }
        }
        public int fr_dwg_id
        {
            get
            {
                return _fr_dwg_id;
            }
            set
            {
                _fr_dwg_id = value;
            }
        }
        public string fr_dwg
        {
            get
            {
                return _fr_dwg;
            }
            set
            {
                _fr_dwg = value;
            }
        }
        public string fr_dwg_rev
        {
            get
            {
                return _fr_dwg_rev;
            }
            set
            {
                _fr_dwg_rev = value;
            }
        }
        public string to_equip_orig
        {
            get
            {
                return _to_equip_orig;
            }
            set
            {
                _to_equip_orig = value;
            }
        }
        public string to_equip_orig_loc
        {
            get
            {
                return _to_equip_orig_loc;
            }
            set
            {
                _to_equip_orig_loc = value;
            }
        }
        public int to_equip_id
        {
            get
            {
                return _to_equip_id;
            }
            set
            {
                _to_equip_id = value;
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
        public string to_equip_loc
        {
            get
            {
                return _to_equip_loc;
            }
            set
            {
                _to_equip_loc = value;
            }
        }
        public string to_equip_bd
        {
            get
            {
                return _to_equip_bd;
            }
            set
            {
                _to_equip_bd = value;
            }
        }
        public string to_dwg_orig
        {
            get
            {
                return _to_dwg_orig;
            }
            set
            {
                _to_dwg_orig = value;
            }
        }
        public int to_dwg_id
        {
            get
            {
                return _to_dwg_id;
            }
            set
            {
                _to_dwg_id = value;
            }
        }
        public string to_dwg
        {
            get
            {
                return _to_dwg;
            }
            set
            {
                _to_dwg = value;
            }
        }
        public string to_dwg_rev
        {
            get
            {
                return _to_dwg_rev;
            }
            set
            {
                _to_dwg_rev = value;
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
        public string comment_orig
        {
            get
            {
                return _comment_orig;
            }
            set
            {
                _comment_orig = value;
            }
        }
        public string comment
        {
            get
            {
                return _comment;
            }
            set
            {
                _comment = value;
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
        public ArrayList drawinglist
        {
            get
            {
                return _drawinglist;
            }
            set
            {
                _drawinglist = value;
            }
        }
        public ArrayList routelist
        {
            get
            {
                return _routelist;
            }
            set
            {
                _routelist = value;
            }
        }
        public ArrayList routechglist
        {
            get
            {
                return _routechglist;
            }
            set
            {
                _routechglist = value;
            }
        }
        public ArrayList crdpowerlist
        {
            get
            {
                return _crdpowerlist;
            }
            set
            {
                _crdpowerlist = value;
            }
        }
        #endregion
    }
}
