

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
                        prop.SetValue(obj, reader[prop.Name]);
                    }
                }

                results.Add(obj);
            }

            return results;
        }
    }
}
