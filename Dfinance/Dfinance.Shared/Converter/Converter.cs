using System.ComponentModel;
using System.Data;
using System.Reflection;

namespace Dfinance.Shared
{
    public static class Converter
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection properties =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
        public static Object IsNull(Object CheckExpression, Object ReplacementValue)
        {
            Object ReturnValue = null;
            if (CheckExpression == null || CheckExpression == DBNull.Value || CheckExpression.ToString() == String.Empty)
            {
                ReturnValue = ReplacementValue;
            }
            else
            {
                ReturnValue = CheckExpression;
            }
            return ReturnValue;
        }
        public static Boolean IsNull(Object CheckExpression)
        {
            return CheckExpression == null || CheckExpression == DBNull.Value || CheckExpression.ToString() == String.Empty;
        }

        public static Object IsEmpty(Object CheckExpression, Object ReplacementValue)
        {
            Object ReturnValue = null;
            if (CheckExpression == null || CheckExpression == DBNull.Value || (String)CheckExpression == "")
            {
                ReturnValue = ReplacementValue;
            }
            else
            {
                ReturnValue = CheckExpression;
            }
            return ReturnValue;
        }
        public static Dictionary<string, object> ToDictionary(object model)
        {
            Dictionary<string, object> dict = new Dictionary<string, object>();
            PropertyInfo[] properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                dict[property.Name] = property.GetValue(model, null);
            }

            return dict;
        }
        public static bool StringToBoolean(string? str)
        {
            if(str == null) return false;
            if(str=="1")
                return true;
            return false;
        }
    }
}
