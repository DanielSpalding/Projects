using System.Collections;
using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain drawing records
    /// </summary>
    public class Document : BaseEntity
    {
        #region private variables
        private int _doc_id;                                                                // document id
        private string _doc;                                                                // document
        private string _doc_desc;                                                           // document description
        private string _doc_type;                                                           // document type
        private string _doc_status;
        private ArrayList _falist;                                                          // list of associated fire area (use Entity.FADOC)
        #endregion

        public Document(){}
        public Document(Document obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public int doc_id
        {
            get
            {
                return _doc_id;
            }
            set
            {
                _doc_id = value;
            }
        }
        public string doc
        {
            get
            {
                return _doc;
            }
            set
            {
                _doc = value;
            }
        }
        public string doc_desc
        {
            get
            {
                return _doc_desc;
            }
            set
            {
                _doc_desc = value;
            }
        }
        public string doc_type
        {
            get
            {
                return _doc_type;
            }
            set
            {
                _doc_type = value;
            }
        }
        public string doc_status
        {
            get
            {
                return _doc_status;
            }
            set
            {
                _doc_status = value;
            }
        }
        public ArrayList falist
        {
            get
            {
                return _falist;
            }
            set
            {
                _falist = value;
            }
        }
        #endregion
    }
}
