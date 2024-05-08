

using System.Data.Common;

namespace Dfinance.Shared.Deserialize
{
    public class DataRederToObj
    {
        public List<T> Deserialize<T>(DbDataReader reader)
        {
            var results = new List<T>();

            while (reader.Read())
            {
                var obj = Activator.CreateInstance<T>();

                foreach (var prop in typeof(T).GetProperties())
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(prop.Name)))
                    {
                        var value = reader[prop.Name];
                        var propertyType = prop.PropertyType;

                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propertyType = Nullable.GetUnderlyingType(propertyType);
                        }

                        if (propertyType == typeof(int))
                        {
                            if (int.TryParse(value.ToString(), out int intValue))
                            {
                                prop.SetValue(obj, intValue);
                            }
                        }
                        else
                        {
                            prop.SetValue(obj, Convert.ChangeType(value, propertyType));
                        }
                    }
                }

                results.Add(obj);
            }

            return results;
        }
    }
}
