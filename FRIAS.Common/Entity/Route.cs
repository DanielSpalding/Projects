using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain route/endpoint records
    /// </summary>
    public class Route : BaseEntity
    {
        #region private variables
        private int _node_id;                                                               // node id
        private string _node;                                                               // node, endpoint
        private bool _iscomp;                                                               // node is component?
        private bool _comp_chkd;                                                            // node/comp checked?
        private string _bd_endpt;                                                           // block diagram endpoint
        private ArrayList _routelocalist;                                                   // route location list (use Entity.Routeloca)
        private ArrayList _bdendpointlist;                                                  // endpoint list (use Entity.RouteBDEndpoint)
        private ArrayList _drawinglist;                                                     // drawing list (use Entity.RoutelocaDwg)
        private ArrayList _cablelist;                                                       // cable list (use Entity.CabRoute)
        #endregion

        public Route(){}
        public Route(Route obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public int node_id
        {
            get
            {
                return _node_id;
            }
            set
            {
                _node_id = value;
            }
        }
        public string node
        {
            get
            {
                return _node;
            }
            set
            {
                _node = value;
            }
        }
        public bool iscomp
        {
            get
            {
                return _iscomp;
            }
            set
            {
                _iscomp = value;
            }
        }
        public bool comp_chkd
        {
            get
            {
                return _comp_chkd;
            }
            set
            {
                _comp_chkd = value;
            }
        }
        public string bd_endpt
        {
            get
            {
                return _bd_endpt;
            }
            set
            {
                _bd_endpt = value;
            }
        }
        public ArrayList routelocalist
        {
            get
            {
                return _routelocalist;
            }
            set
            {
                _routelocalist = value;
            }
        }
        public ArrayList bdendpointlist
        {
            get
            {
                return _bdendpointlist;
            }
            set
            {
                _bdendpointlist = value;
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
        public ArrayList cablelist
        {
            get
            {
                return _cablelist;
            }
            set
            {
                _cablelist = value;
            }
        }
        #endregion
    }
}
