using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace SFRM.Synchronizer.Main.Farmatic
{
    public class DbFarmatic
    {
        public static readonly string ConnectionString = @"Data Source=FEDERICO-PC\SQLEXPRESS;Initial Catalog=Farmatic;Persist Security Info=true;User ID=sa;Password=sqlserver";
        public static void Connect()
        {            
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                try
                {
                    string sql = @"SELECT * FROM Cliente";
                    conn.Open();
                    Debug.WriteLine("[INFO] Open database");
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        Debug.WriteLine("[DATA] " + dr[0] + " -- " + dr[1]);
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
    }
}