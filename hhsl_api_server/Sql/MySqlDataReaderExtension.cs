using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace hhsl_api_server.Sql
{
    public static class MySqlDataReaderExtension
    {
        public static string GetString2(this MySqlDataReader reader, string filedName)
        {
            string result = "";
            int i = -1;
            try
            {
                 i = reader.GetOrdinal(filedName);
            }
            catch (Exception e)
            {
                // ignored
            }

            if (i == -1)
            {
                return result;
            }

            var isNull = reader.IsDBNull(i);
            if (!isNull)
            {
                result = reader.GetString(i);
            }
            return result;
        }

        public static double GetDouble2(this MySqlDataReader reader, string filedName)
        {
            double result = 0.0;
            int i = -1;
            try
            {
                i = reader.GetOrdinal(filedName);
            }
            catch (Exception e)
            {
                // ignored
            }

            if (i == -1)
            {
                return result;
            }

            var isNull = reader.IsDBNull(i);
            if (!isNull)
            {
                result = reader.GetDouble(i);
            }
            return result;
        }

        public static int GetInt322(this MySqlDataReader reader, string filedName)
        {
            int result = 0;
            int i = -1;
            try
            {
                i = reader.GetOrdinal(filedName);
            }
            catch (Exception e)
            {
                // ignored
            }

            if (i == -1)
            {
                return result;
            }

            var isNull = reader.IsDBNull(i);
            if (!isNull)
            {
                result = reader.GetInt32(i);
            }
            return result;
        }
    }
}