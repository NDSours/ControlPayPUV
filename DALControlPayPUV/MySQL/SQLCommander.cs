using System;
using System.Collections.Generic;
using System.Data.Common;

namespace DALControlPayPUV.MySQL
{
    public static class SQLCommander
    {
        public static List<string[]> GetRowsFromDB(string sql)
        {
            MySql.Data.MySqlClient.MySqlConnection conn = DBUtils.GetDBConnection();
            DbDataReader reader = default;
            List<string[]> list = new List<string[]>();
            conn.Open();
            try
            {
                MySql.Data.MySqlClient.MySqlCommand cmd = new MySql.Data.MySqlClient.MySqlCommand();

                cmd.Connection = conn;
                cmd.CommandText = sql;
                cmd.CommandTimeout = 0;

                reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        string temp = "";
                        for(int i = 0; i<reader.FieldCount; i++)
                            temp += reader.GetValue(i).ToString() + ';';

                        temp = temp.Remove(temp.Length - 1);

                        list.Add(temp.Split(';'));
                    }
                }

            }
            catch (Exception e)
            {
                throw new Exception("Error: " + e);   
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }

            return list;
            
        }
    }
}
