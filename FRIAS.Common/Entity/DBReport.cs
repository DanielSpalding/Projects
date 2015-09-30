using System.Reflection;

namespace FRIAS.Common.Entity
{
    /// <summary>
    /// Class to contain reports <para />
    /// Report is a reserved namespace, therefore using DBReport
    /// </summary>
    public class DBReport : BaseEntity
    {
        #region private variables
        private int _rpt_id;                                                                // report id
        private string _rpt;                                                                // report file
        private string _rpt_desc;                                                           // report description
        private string _rpt_type;                                                           // report type
        private string _plant_hdr;                                                          // plant header
        private string _project_hdr;                                                        // project header
        private string _doc_hdr;                                                            // document header
        private string _sub_hdr;                                                            // sub-title header
        private string _title_hdr;                                                          // title header
        #endregion

        public DBReport(){}
        public DBReport(DBReport obj)
        {
            PropertyInfo[] p = obj.GetType().GetProperties();                               // get entity properties
            for (int i = 0; i < (p.Length); i++)
            {
                if (!p[i].PropertyType.Name.Contains("list") && !p[i].Name.Contains("arg"))
                    p[i].SetValue(this, p[i].GetValue(obj, null), null);                    // set entity's property values to obj properties
            }
        }

        #region public properties
        public int rpt_id
        {
            get { return _rpt_id; }
            set { _rpt_id = value; }
        }
        public string rpt
        {
            get { return _rpt; }
            set { _rpt = value; }
        }
        public string rpt_desc
        {
            get { return _rpt_desc; }
            set { _rpt_desc = value; }
        }
        public string rpt_type
        {
            get { return _rpt_type; }
            set { _rpt_type = value; }
        }
        public string plant_hdr
        {
            get { return _plant_hdr; }
            set { _plant_hdr = value; }
        }
        public string project_hdr
        {
            get { return _project_hdr; }
            set { _project_hdr = value; }
        }
        public string doc_hdr
        {
            get { return _doc_hdr; }
            set { _doc_hdr = value; }
        }
        public string sub_hdr
        {
            get { return _sub_hdr; }
            set { _sub_hdr = value; }
        }
        public string title_hdr
        {
            get { return _title_hdr; }
            set { _title_hdr = value; }
        }
        #endregion
    }
}
