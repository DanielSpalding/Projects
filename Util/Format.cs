using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{
    public class Format
    {
        /// <summary>
        /// formats a date to its string equivalent
        /// </summary>
        public static int FormatDateToInt(DateTime curDate, string format)
        {
            return Convert.ToInt32(curDate.ToString(format));
        }

        /// <summary>
        /// formats an integer with zeros for specified length
        /// </summary>
        public static string FormatPadZeros(int value, int length)
        {
            string result = value.ToString();
            int i = 0;
            int curLen = result.Length;

            for (i = 0; i < length - curLen; i++)
            {
                result = "0" + result;
            }

            return result;
        }

        /// <summary>
        /// formats integer month with appropriate zero padding
        /// </summary>
        public static string FormatMonthStringPadZero(int month)
        {
            switch (month)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return "0" + month.ToString();
                case 10:
                case 11:
                case 12:
                    return month.ToString();
            }
            return month.ToString();
        }


        /// <summary>
        /// formats integer month to string equivalent
        /// </summary>
        public static string FormatMonthString(int month)
        {
            switch (month)
            {
                case 1:
                    return "Janurary";
                case 2:
                    return "Feburary";
                case 3:
                    return "March";
                case 4:
                    return "April";
                case 5:
                    return "May";
                case 6:
                    return "June";
                case 7:
                    return "July";
                case 8:
                    return "August";
                case 9:
                    return "September";
                case 10:
                    return "October";
                case 11:
                    return "November";
                case 12:
                    return "December";
            }

            return "";
        }

        /// <summary>
        /// formats decimal to string
        /// </summary>
        public static string FormatDecimalToString(decimal value, int dec, bool cur)
        {
            string format = value.ToString();
            string decPlaces = "";

            string[] values = format.Split('.');

            if (values.Length > 1)
            {
                if (values[1].Length < dec)
                    decPlaces = values[1].Substring(0, values[1].Length);
                else
                    decPlaces = values[1].Substring(0, dec);

                if (dec == 0)
                    format = values[0];
                else
                    format = values[0] + "." + decPlaces;
            }
            else
                format = values[0];


            if (cur)
                format = "$" + format;

            return format;

        }

        /// <summary>
        /// formats decimal to string
        /// </summary>
        public static string FormatDecimalToString(decimal value, int dec, bool cur, bool thous)
        {
            string format = "";

            if (thous)
                format = String.Format("{0:0,0.00}", value);
            else
                format = String.Format("{0:0.00}", value);

            if (cur)
                format = "$" + format;

            return format;

        }

        /// <summary>
        /// formats passed string to integer
        /// </summary>
        public static int FormatStringToInt(string value)
        {

            foreach (char i in value)
            {
                int ascii = Convert.ToInt16(i);

                if (!(ascii >= 48 && ascii <= 57))
                    value = value.Replace(i.ToString(), "");
            }

            if (value.Length == 0)
                value = "0";

            return Convert.ToInt32(value);
        }

        /// <summary>
        /// determines days from passed date
        /// </summary>
        public static int FormatDateFromToday(string monYr, string dateFormat)
        {
            System.IFormatProvider format = new System.Globalization.CultureInfo("en-US", true);
            DateTime testDate = DateTime.ParseExact(monYr, dateFormat, format);
            //DateTime testDate = DateTime.ParseExact(dateValue, "MM/yyyy", format);
            TimeSpan age = System.DateTime.Now.Subtract(testDate);
            return Convert.ToInt32(System.Math.Round(age.TotalDays / 365.25, 0));
        }

        /// <summary>
        /// formats passed time to integer
        /// </summary>
        public static int FormatTimeToInt(DateTime curTime, string format)
        {
            return Convert.ToInt32(curTime.ToString(format));
        }

        /// <summary>
        /// Formats time to string
        /// </summary>
        /// <param name="hours">Integer</param>
        /// <param name="minutes">Integer</param>
        /// <returns>Time string</returns>
        public static string FormatTimeToString(int hours, int minutes)
        {
            string formatTime = "[hr]:[min]";

            if (hours < 10)
                formatTime = formatTime.Replace("[hr]", "0" + hours.ToString());
            else
                formatTime = formatTime.Replace("[hr]", hours.ToString());

            if (minutes < 10)
                formatTime = formatTime.Replace("[min]", "0" + minutes.ToString());
            else
                formatTime = formatTime.Replace("[min]", minutes.ToString());

            return formatTime;
        }

        /// <summary>
        /// Determines if passed object is numeric
        /// </summary>
        /// <param name="Expression">System.Object (base class)</param>
        /// <returns>True/False</returns>
        public static System.Boolean IsNumeric(System.Object Expression)
        {
            if(Expression == null || Expression is DateTime)
                return false;

            if(Expression is Int16 || Expression is Int32 || Expression is Int64 || Expression is Decimal || Expression is Single || Expression is Double || Expression is Boolean)
                return true;
           
            try 
            {
                if (Expression is string)
                {
                    Double.Parse(Expression as string);
                    return true;
                }
                else
                {
                    Double.Parse(Expression.ToString());
                    return true;
                }
            }
            catch
            {
                return false;
            } 
        }
    }
}
