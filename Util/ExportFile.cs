using System;
using System.Collections;
using System.ComponentModel;
using System.Data;

namespace Util
{
    public class ExportFile
    {
        private Delimiter deltype;                                                          // delimiter type 
        private string delvalue;                                                            // delimiter value

        public ExportFile()
        {
            deltype = Delimiter.Tab;
            delvalue = DelimiterValue();
        }

        public ExportFile(Delimiter type)
        {
            deltype = type;
            delvalue = DelimiterValue();
        }

        /// <summary>
        /// Delimiter type
        /// </summary>
        public enum Delimiter
        {
            Comma, Tab, Pipe, SemiColon
        }

        /// <summary>
        /// Delimiter value
        /// </summary>
        /// <returns>Delimiter</returns>
        private string DelimiterValue()
        {

            string del = "";

            switch (deltype)
            {
                case Delimiter.Comma:
                    del = ",";
                    break;
                case Delimiter.Pipe:
                    del = "|";
                    break;
                case Delimiter.SemiColon:
                    del = ";";
                    break;
                case Delimiter.Tab:
                    del = "\t";
                    break;
            }
            return del;
        }

        public string Export(DataTable dt, bool exportcolumnheadings)
        {
            string header = "";
            string body = "";
            string record = "";

            if (exportcolumnheadings)
            {
                foreach (DataColumn col in dt.Columns)
                    header = header + col.ColumnName.ToLower() + ",";

                header = header.Substring(0, header.Length - 1);
            }

            foreach (DataRow row in dt.Rows)
            {
                object[] arr = row.ItemArray;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].ToString().IndexOf(",") > 0)
                        record = record + arr[i].ToString().Replace(",", "") + delvalue;
                    else
                        record = record + arr[i].ToString().Replace(",", "") + delvalue;

                }

                body = body + record.Substring(0, record.Length - 1) + "\n";
                record = "";
            }

            if (exportcolumnheadings)
                return header + "\n" + body;
            else
                return body;
        }

        public string Export(ArrayList list, ArrayList cols, bool exportcolumnheadings)
        {
            string header = "";
            string body = "";
            string record = "";

            if (exportcolumnheadings)
            {
                foreach (string col in cols)
                {
                    string[] columns = col.Split('.');
                    header = header + columns[0] + delvalue;
                }

                header = header.Substring(0, header.Length - 1);
            }

            foreach (Object row in list)
            {
                
                foreach (string col in cols)
                {
                    PropertyDescriptor prop;

                    string[] columns = col.Split('.');
                    if (columns.Length == 2)
                    {
                        prop = GetPropertyDescriptor(columns[1], list);

                        if (record.Length == 0)
                            record = GetPropValue(prop, row, columns[1]) + delvalue;
                        else
                            record = record + GetPropValue(prop, row, columns[1]) + delvalue;

                    }
                    else
                    {
                        prop = GetPropertyDescriptor(columns[1], list);

                        PropertyDescriptor prop2 = GetPropertyDescriptor(columns[2], prop);

                        Object subRow = prop.GetValue(row);

                        if (subRow != null)
                        {
                            if (record.Length == 0)
                                record = GetPropValue(prop2, subRow, columns[2]) + delvalue;
                            else
                                record = record + GetPropValue(prop2, subRow, columns[2]) + delvalue;
                        }
                    }
                }

                body = body + record.Substring(0, record.Length - 1) + "\n";
                record = "";
            }

            if (exportcolumnheadings)
                return header + "\n" + body;
            else
                return body;

        }

        // gets property descriptor, given property and arraylist
        private PropertyDescriptor GetPropertyDescriptor(string property, ArrayList objectList)
        {
            Type itemtype = null;
            PropertyDescriptorCollection props;

            string[] propArray = property.Split('.');

            if (property.Length > 0)
            {
                itemtype = objectList[0].GetType();

                props = TypeDescriptor.GetProperties(itemtype);
                foreach (PropertyDescriptor prop in props)
                {
                    if (prop.Name == property)
                    {
                        return prop;
                    }
                }
            }

            return null;
        }

        // gets property descriptor, given property and property descriptor
        private PropertyDescriptor GetPropertyDescriptor(string property, PropertyDescriptor propdesc)
        {
            Type itemtype = null;
            PropertyDescriptorCollection props;

            string[] propArray = property.Split('.');

            if (property.Length > 0)
            {

                itemtype = propdesc.PropertyType;

                props = TypeDescriptor.GetProperties(itemtype);
                foreach (PropertyDescriptor prop in props)
                {
                    if (prop.Name == property)
                    {
                        return prop;
                    }
                }
            }

            return null;
        }

        // get property value as a string
        private string GetPropValue(PropertyDescriptor propvalue, Object obj, string col)
        {

            string formattedpropvalue = "";
            propvalue.GetValue(obj);

            if (propvalue.PropertyType.Name.Equals("DateTime"))
            {
                DateTime checkDate = Convert.ToDateTime(propvalue.GetValue(obj));
                if (checkDate.Year > 1)
                {
                    formattedpropvalue = checkDate.ToShortDateString();
                }
                else
                    formattedpropvalue = "";
            }
            else if (propvalue.PropertyType.Name.Equals("Decimal"))
            {
                decimal value = Convert.ToDecimal(propvalue.GetValue(obj));
                value = System.Math.Round(value, 2);

                formattedpropvalue = Format.FormatDecimalToString(value, 2, false);
            }
            else
                formattedpropvalue = propvalue.GetValue(obj).ToString();

            return formattedpropvalue;
        }
    }
}
