using System;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace SFRM.Synchronizer.Main.Fisiotes
{
    public class DbFisiotes
    {        
        public static readonly string ConnectionString = @"server=localhost;user id=root;password=mysqlpass;database=fisiotes_pruebas";
        public static void Connect()
        {            
            using (MySqlConnection conn = new MySqlConnection(ConnectionString))
            {
                try
                {
                    string sql = @"SELECT * FROM clientes";
                    conn.Open();
                    Debug.WriteLine("[INFO] Open database");
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    MySqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Debug.WriteLine("[DATA] " + dr[0] + " -- " + dr[4]);
                    }
                    dr.Close();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("[ERROR] " + e.Message);
                }
                finally
                {
                    conn.Close();
                    Debug.WriteLine("[INFO] Close database");
                }
            }                        
        }

        public static void SetCeroClientes()
        {
            string connStr = @"server=localhost;user id=root;password=mysqlpass;database=fisiotes_pruebas";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                try
                {
                    string sql = @"UPDATE clientes SET dni_tra = 0";
                    conn.Open();
                    Debug.WriteLine("[INFO] Open database");
                    MySqlCommand cmd = new MySqlCommand(sql, conn);
                    int rows = cmd.ExecuteNonQuery();
                    Debug.WriteLine("[INFO] Updated " + rows + " rows.");
                }
                catch (Exception e)
                {
                    Debug.WriteLine("[ERROR] " + e.Message);
                }
                finally
                {
                    conn.Close();
                    Debug.WriteLine("[INFO] Close database");
                }
            }
        }
    }
}