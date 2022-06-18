using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Charisma.Infrastructure.Core.Extentions
{
    public static class CustomExtention
    {
        public static byte ToByte(this object obj)
        {
            if (obj == null) return default;

            return (byte)Convert.ChangeType(obj, typeof(byte));
        }

        public static int ToInt(this object obj)
        {
            if (obj == null) return default;

            return (int)Convert.ChangeType(obj, typeof(int));
        }

        public static double ToDouble(this object obj)
        {
            if (obj == null) return default;

            return (double)Convert.ChangeType(obj, typeof(double));
        }

        public static decimal ToDecimal(this object obj)
        {
            if (obj == null) return default;

            return (decimal)Convert.ChangeType(obj, typeof(decimal));
        }

        public static T ConvertTo<T>(this object obj)
        {
            if (obj == null) return default;

            return (T)Convert.ChangeType(obj, typeof(T));
        }

        public static string GetHash(this string value, string secretKey)
        {
            var valueBytes = Encoding.UTF8.GetBytes(value);
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            using var hmacsha256 = new HMACSHA256(keyBytes);
            byte[] hashValue = hmacsha256.ComputeHash(valueBytes);
            return Convert.ToBase64String(hashValue);
        }

        public static bool VerifyHash(this string value, string hash, string secretKey)
        {
            var hashOfValue = value.GetHash(secretKey);

            return StringComparer.OrdinalIgnoreCase.Compare(hashOfValue, hash) == 0;
        }

        public static bool IsEmail(this string value)
        {
            var checker = new EmailAddressAttribute();
            return checker.IsValid(value);
        }

        public static bool IsNumber(this string value)
        {
            return long.TryParse(value, out long result);
        }

        public static bool IsNumber(this string value, out long result)
        {
            return long.TryParse(value, out result);
        }

        public static bool IsNull(this string value)
        {
            if (value == null || string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value))
                return true;
            return false;
        }

        public static bool IsNull(this int? value)
        {
            if (value == null)
                return true;
            return false;
        }

        public static bool IsNull(this long? value)
        {
            if (value == null)
                return true;
            return false;
        }

        public static DataTable ToDataTable<T>(this IEnumerable<T> iList)
        {
            DataTable dataTable = new DataTable();
            PropertyDescriptorCollection propertyDescriptorCollection =
                TypeDescriptor.GetProperties(typeof(T));
            for (int i = 0; i < propertyDescriptorCollection.Count; i++)
            {
                PropertyDescriptor propertyDescriptor = propertyDescriptorCollection[i];
                Type type = propertyDescriptor.PropertyType;

                if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                    type = Nullable.GetUnderlyingType(type);


                dataTable.Columns.Add(propertyDescriptor.Name, type);
            }
            object[] values = new object[propertyDescriptorCollection.Count];
            foreach (T iListItem in iList)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = propertyDescriptorCollection[i].GetValue(iListItem);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }
    }
}
