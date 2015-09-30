using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Reporting.WebForms;

using FRIAS.Common;
using FRIAS.Common.Entity;
using DL.Data;

namespace FRIAS.BL
{   
    // methods used for reports
    public class ReportManager
    {
        // fetches all parameters given a report id
        public static ArrayList FetchReportParams(int id)
        {

            ArrayList list = new ArrayList();
            DBReportParam param = new DBReportParam();
            PropertyInfo[] p = param.GetType().GetProperties();

            string qryString = "SELECT * FROM REPORT_PARAMS WHERE RPT_ID = @id";
            
            // create database connection
            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(1);
                dbmgr.AddParameters(0, "@id", id);
                dbmgr.ExecuteReader(System.Data.CommandType.Text, qryString);

                while (dbmgr.DataReader.Read())
                {
                    param = new DBReportParam();
                    param = (DBReportParam)TotalManager.FetchObject(param, p, dbmgr);
                    list.Add(param);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                dbmgr.Dispose();
            }

            return list;
        }

        // fetches list of reports given a report type
        // if rpt_type = "ALL" then fetches all reports
        public static object FetchReportList(string rpt_type)
        {
            ArrayList list = new ArrayList();
            DBReport item = new DBReport();
            string qryString;
            PropertyInfo[] p = item.GetType().GetProperties();

            if (rpt_type == "ALL")
                qryString = "SELECT * FROM REPORTLIST WHERE RPT IS NOT NULL ORDER BY RPT_ID";
            else
                qryString = "SELECT * FROM REPORTLIST WHERE RPT_TYPE LIKE @rpt_type ORDER BY RPT_ID";

            // create database connection
            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(1);
                dbmgr.AddParameters(0, "@rpt_type", rpt_type);
                dbmgr.ExecuteReader(System.Data.CommandType.Text, qryString);

                while (dbmgr.DataReader.Read())
                {
                    item = new DBReport();
                    item = (DBReport)TotalManager.FetchObject(item, p, dbmgr);
                    list.Add(item);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                dbmgr.Dispose();
            }

            return list;
        }

        // fetches report infomration for a given report id
        public static DBReport FetchReport(int id)
        {
            DBReport report = new DBReport();
            string qryString;

            qryString = "SELECT * FROM REPORTLIST WHERE RPT_ID = @rpt_id";

            // create database connection
            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            IDBManager dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(1);
                dbmgr.AddParameters(0, "@rpt_id", id);
                dbmgr.ExecuteReader(System.Data.CommandType.Text, qryString);

                if (dbmgr.DataReader.Read())
                {
                    // get properties of object and fetch object
                    PropertyInfo[] p = report.GetType().GetProperties();
                    report = (DBReport)TotalManager.FetchObject(report, p, dbmgr);
                }

            }
            catch (Exception ex)
            {
                throw (ex);
            }
            finally
            {
                dbmgr.Dispose();
            }

            return report;
        }

        public static List<ReportParameter> GenerateParams(ArrayList paramlist)
        {
            List<ReportParameter> list = new List<ReportParameter>();
            ReportParameter rparam;

            foreach (DBReportParam dbparam in paramlist)
            {
                rparam = new ReportParameter(dbparam.param_name, dbparam.param_value);
                list.Add(rparam);
            }
            return list;
        }

        // update reportlist
        public static void Update(object obj)
        {
            // create database connection
            IDBManager dbmgr;
            User user = (User)System.Web.HttpContext.Current.Session[Constant.session.User];
            dbmgr = new DBManager(user.plantDBStr);
            dbmgr.ConnectionString = user.plantDBStr;

            DBReport item = (DBReport)obj;

            try
            {
                dbmgr.Open();
                dbmgr.CreateParameters(5);
                dbmgr.AddParameters(0, "@rpt_id", item.rpt_id);
                dbmgr.AddParameters(1, "@title_hdr", item.title_hdr);
                dbmgr.AddParameters(2, "@project_hdr", item.project_hdr);
                dbmgr.AddParameters(3, "@doc_hdr", item.doc_hdr);
                dbmgr.AddParameters(4, "@plant_hdr", item.plant_hdr);
                dbmgr.ExecuteNonQuery(CommandType.StoredProcedure, "dbo.REPORTLIST_u");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbmgr.Dispose();
            }
        }
    }
}
