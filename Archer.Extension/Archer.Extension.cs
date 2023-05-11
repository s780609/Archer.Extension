using System.Collections;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace Archer.Extension
{
    public static class ExtensionMethods
    {
        public static Exception? ThrowIfNull(this object checkTarget, string message = "")
        {
            if (checkTarget is null)
            {
                throw new Exception(message == string.Empty ? $"{nameof(checkTarget)} is null" : message);
            }

            return null;
        }

        public static Exception? ThrowIfNullOrWhiteSpace(this string checkTarget, string message = "")
        {
            if (string.IsNullOrWhiteSpace(checkTarget))
            {
                throw new Exception(message != string.Empty ? message : $"{nameof(checkTarget)} is NullOrWhiteSpace");
            }

            return null;
        }

        public static Exception? ThrowIfNullOrEmpty(this string checkTarget, string message)
        {
            if (string.IsNullOrEmpty(checkTarget))
            {
                throw new Exception(message != string.Empty ? message : $"{nameof(checkTarget)} is NullOrWhiteSpace");
            }

            return null;
        }

        public static Exception? ThrowIfZero(this object[] checkTarget, string message = "")
        {
            if (message == string.Empty)
            {
                message = $"{nameof(checkTarget)}'s length is zero.";
            }

            if (checkTarget.Count() == 0)
            {
                throw new Exception(message);
            }

            return null;
        }

        public static async Task CopyProxyHttpResponse(this HttpContext context, HttpResponseMessage responseMessage)
        {
            if (responseMessage == null)
            {
                throw new ArgumentNullException(nameof(responseMessage));
            }

            var response = context.Response;

            response.StatusCode = (int)responseMessage.StatusCode;
            foreach (var header in responseMessage.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            foreach (var header in responseMessage.Content.Headers)
            {
                response.Headers[header.Key] = header.Value.ToArray();
            }

            // SendAsync removes chunking from the response. This removes the header so it doesn't expect a chunked response.
            response.Headers.Remove("transfer-encoding");

            using (var responseStream = await responseMessage.Content.ReadAsStreamAsync())
            {
                await responseStream.CopyToAsync(response.Body);
            }
        }

        public static string[] GetPropsName(this object dataObject, string[]? skipProp = null)
        {
            Type dataType = dataObject.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(dataType.GetProperties());

            Hashtable hashtable = new Hashtable();
            int count = 0;

            for (int i = 0; i < props.Count; i++)
            {
                if (skipProp != null)
                {
                    if (skipProp.Contains(props[i].Name))
                        continue;
                }

                object? propValue = props[i].Name;

                if (propValue != null)
                {
                    hashtable.Add(count, propValue.ToString());
                    count = count + 1;
                }
            }

            string[] temp = new string[hashtable.Count];
            for (int i = 0; i < hashtable.Count; i++)
            {
                temp[i] = hashtable[i].ToString();
            }

            return temp.ToArray();
        }

        public static string?[] GetPropsValue(this object dataObject, string[]? skipProp = null, string dateTimeValueFormat = "yyyy-MM-dd HH:mm:ss.fff")
        {
            Type dataType = dataObject.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(dataType.GetProperties());

            Hashtable hashtable = new Hashtable();
            int count = 0;

            for (int i = 0; i < props.Count; i++)
            {
                if (skipProp != null)
                {
                    if (skipProp.Contains(props[i].Name))
                        continue;
                }

                object? propValue = props[i].GetValue(dataObject, null);

                if (propValue != null)
                {
                    if (propValue.GetType() == typeof(DateTime))
                    {
                        hashtable.Add(count, ((DateTime)propValue).ToString(dateTimeValueFormat));
                    }
                    else
                    {
                        hashtable.Add(count, propValue.ToString());
                    }
                }
                else
                {
                    hashtable.Add(count, propValue);
                }

                count = count + 1;
            }

            string?[] temp = new string[hashtable.Count];
            for (int i = 0; i < hashtable.Count; i++)
            {
                if (hashtable[i] == null)
                {
                    temp[i] = null;
                }
                else
                {
                    temp[i] = hashtable[i].ToString();
                }
            }

            return temp;
        }

        public static string GetInsertSqlScript<T>(this List<T> list, string tableName, string[]? skipField = null)
        {
            StringBuilder sql = new StringBuilder();

            for (int j = 0; j < list.Count; j++)
            {
                string[] propsName = list[j].GetPropsName(skipField);
                string[] propsValue = list[j].GetPropsValue(skipField);

                sql.Append($" INSERT INTO dbo.{tableName} ");
                sql.Append(" ( ");

                for (int i = 0; i < propsName.Count(); i++)
                {
                    if (i == propsName.Count() - 1)
                    {
                        sql.Append($" {propsName[i]} ");
                        continue;
                    }

                    sql.Append($" {propsName[i]}, ");
                }

                sql.Append(" ) ");
                sql.Append(" VALUES ");
                sql.Append(" ( ");

                for (int i = 0; i < propsValue.Count(); i++)
                {
                    if (propsValue[i] == null)
                        sql.Append(" NULL ");
                    else
                        sql.Append($" '{propsValue[i]}'");

                    if (i != propsValue.Count() - 1)
                    {
                        sql.Append(", ");
                    }
                }

                sql.Append(" ); ");
            }

            return sql.ToString();
        }

        public static string GetInsertSqlScript<T>(this T recordObject, string tableName, string[]? skipField = null)
        {
            StringBuilder sql = new StringBuilder();

            string[] propsName = recordObject.GetPropsName(skipField);
            string[] propsValue = recordObject.GetPropsValue(skipField);

            sql.Append($" INSERT INTO dbo.{tableName} ");
            sql.Append(" ( ");

            for (int i = 0; i < propsName.Count(); i++)
            {
                if (i == propsName.Count() - 1)
                {
                    sql.Append($" {propsName[i]} ");
                    continue;
                }

                sql.Append($" {propsName[i]}, ");
            }

            sql.Append(" ) ");
            sql.Append(" VALUES ");
            sql.Append(" ( ");

            for (int i = 0; i < propsValue.Count(); i++)
            {
                if (propsValue[i] == null)
                    sql.Append(" NULL ");
                else
                    sql.Append($" '{propsValue[i]}'");

                if (i != propsValue.Count() - 1)
                {
                    sql.Append(", ");
                }
            }

            sql.Append(" ); ");

            return sql.ToString();
        }

        public static Stream GenerateStreamFromString(this string s)
        {
            Stream stream = new MemoryStream();
            StreamWriter sw = new StreamWriter(stream);
            sw.Write(s);
            sw.Flush();
            stream.Position = 0;

            return stream;
        }

        public static string WrapInTransactSql(this string sqlScript)
        {
            return @$"BEGIN TRY 
                      IF @@TRANCOUNT = 0
                      BEGIN TRANSACTION;
                      -- ************************
                      -- sql script start
                      {sqlScript}
                      -- sql script end
                      -- ************************
                      IF @@TRANCOUNT = 1
                      COMMIT TRANSACTION;
                      END TRY 
                      BEGIN CATCH 
                      PRINT 'An error occurred: ' + ERROR_MESSAGE();
                      IF @@TRANCOUNT > 0
                      ROLLBACK TRANSACTION;
                      THROW;
                      END CATCH";
        }
    }
}