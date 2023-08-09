using System;

namespace App.Infra.Implementation.Filter.Extensions
{
    public static class StringExtension
    {
        public static T GetValue<T>(this string value)
        {
            try
            {                           
                //Get type required
                var type = typeof(T);

                //check types and return value parsed
                if (type == typeof(int))
                    return (T)(object)int.Parse(value);
                else if (type == typeof(long))
                    return (T)(object)long.Parse(value);
                else if (type == typeof(float))
                    return (T)(object)float.Parse(value);
                else if (type == typeof(double))
                    return (T)(object)double.Parse(value);
                else if (type == typeof(decimal))
                    return (T)(object)decimal.Parse(value);
                else if (type == typeof(bool))
                    return (T)(object)decimal.Parse(value);
                else if (type == typeof(string))
                    return (T)(object)value.Replace("\"", string.Empty);
                else
                    throw new NotImplementedException("The type not implemented in this converter.");
            }
            catch (FormatException)
            {
                return default;
            }
        }
    }
}
