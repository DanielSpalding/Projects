using System;
using System.Collections;
using System.Text;

using System.Reflection;

namespace Util
{
    public static class StdFunc
    {
        /// <summary>
        /// compares two objects <para />
        /// resutns "0" if objects are different
        /// </summary>
        public static int Compare(object x, object y)
        {
            Type type = x.GetType();
            int comparevalue = 0;
            PropertyInfo[] properties = type.GetProperties();
            FieldInfo[] fields = type.GetFields();
            
            // check property information
            foreach (PropertyInfo p in properties)
            {
                IComparable valx = p.GetValue(x, null) as IComparable;
                if (valx == null)
                    continue;
                object valy = p.GetValue(y, null);
                comparevalue = valx.CompareTo(valy);
                if (comparevalue != 0)
                    return comparevalue;
            }
            
            // check field information
            foreach (FieldInfo f in fields)
            {
                IComparable valx = f.GetValue(x) as IComparable;
                if (valx == null)
                    continue;
                object valy = f.GetValue(y);
                comparevalue = valx.CompareTo(valy);
                if (comparevalue != 0)
                    return comparevalue;
            }
            return comparevalue;
        }

        /// <summary>
        /// gets property information of a given object <para />
        /// arraylist with: List[0]=ObjectType; List[1]=Number of properties; List[x]=All the property names
        /// </summary>
        public static ArrayList ObjInfo(object item)
        {
            ArrayList list = new ArrayList();
            Type type = item.GetType();
            
            // split string to only list the base type
            string[] strTemp = type.ToString().Split(new char[] { '.' });
            list.Add(strTemp[3]);
            
            PropertyInfo[] properties = type.GetProperties();
            list.Add(properties.Length.ToString());
            foreach (PropertyInfo p in properties)
            {
                list.Add(p.Name);
            }
            
            return list;
        }

    }
}
