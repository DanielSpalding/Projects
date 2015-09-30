using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain fire area - document information
    /// </summary>
    public class FADoc : BaseEntity
    {
        #region private variables
        private int _fa_id;                                                                 // fire area id
        private string _fa;                                                                 // fire area
        private int _doc_id;                                                                // document id
        private string _doc;                                                                // document
        private string _doc_desc;                                                           // description
        private string _doc_type;                                                           // reference type: GEN, LIC, EE, OTR
        private string _comment;
        #endregion
        
        public FADoc(){}
        public FADoc(FADoc obj)
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
        #endregion
    }
}
