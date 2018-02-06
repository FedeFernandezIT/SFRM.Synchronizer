using System;
using System.Data.SqlClient;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using SFRM.Synchronizer.Main.Farmatic;
using SFRM.Synchronizer.Main.Fisiotes;

namespace SFRM.Synchronizer.Main
{
    public static class Synchronizer
    {
        public static void SynchronizeClients()
        {
            string movil, email, fechaNacimiento, sexo = string.Empty;
            bool hasSexField = false;
            ushort baja, lopd;

            using (MySqlConnection connFisiotes = new MySqlConnection(DbFisiotes.ConnectionString))
            {
                using (SqlConnection connFarmatic = new SqlConnection(DbFarmatic.ConnectionString))
                {
                    try
                    {
                        string sql =
                            @"SELECT * FROM Cliente WHERE IdCliente > 0 ORDER BY CAST(Idcliente AS DECIMAL(20)) ASC";

                        connFarmatic.Open(); Debug.WriteLine("[INFO] Open Farmatic database");
                        SqlCommand cmdFarmatic = new SqlCommand(sql, connFarmatic);
                        SqlDataReader drFarmaticClientes = cmdFarmatic.ExecuteReader();
                        while (drFarmaticClientes.Read())
                        {
                            sql = @"SELECT * FROM clientes WHERE dni='" + drFarmaticClientes["IdCliente"] + "'";
                            MySqlCommand cmdFisiotes = new MySqlCommand(sql, connFisiotes);
                            MySqlDataReader drFisiotesClientes = cmdFisiotes.ExecuteReader();

                            sql = @"SELECT * FROM Destinatario WHERE fk_Cliente_1 ='" +
                                  drFarmaticClientes["IdCliente"] + "'";
                            cmdFarmatic = new SqlCommand(sql, connFarmatic);
                            SqlDataReader drFarmaticDestinatarios = cmdFarmatic.ExecuteReader();
                            if (drFarmaticDestinatarios.HasRows && drFarmaticDestinatarios.Read())
                            {
                                movil = ((string) drFarmaticDestinatarios["TlfMovil"])?.Trim() ?? string.Empty;
                                email = ((string) drFarmaticDestinatarios["Email"])?.Trim() ?? string.Empty;
                            }
                            else
                            {
                                movil = string.Empty;
                                email = string.Empty;
                            }
                            drFarmaticDestinatarios.Close();
                            drFarmaticDestinatarios.Dispose();

                            sql = @"SELECT * FROM ClienteAux WHERE idCliente = '" + drFarmaticClientes["IdCliente"] + "'";
                            cmdFarmatic = new SqlCommand(sql, connFarmatic);
                            SqlDataReader drFarmaticClientesAux = cmdFarmatic.ExecuteReader();
                            if (drFarmaticClientesAux.HasRows && drFarmaticClientesAux.Read())
                            {
                                fechaNacimiento = ((DateTime?) drFarmaticClientesAux["FechaNac"])?.ToString("yyyyMMdd") ?? string.Empty;
                                if (hasSexField)
                                {
                                    sexo = "V".Equals((string) drFarmaticClientesAux["Sexo"],
                                        StringComparison.OrdinalIgnoreCase)
                                        ? "Hombre"
                                        : "M".Equals((string) drFarmaticClientesAux["Sexo"],
                                            StringComparison.OrdinalIgnoreCase)
                                            ? "Mujer"
                                            : string.Empty;
                                }
                            }
                            else
                            {
                                fechaNacimiento = string.Empty;
                            }
                            drFarmaticClientesAux.Close();
                            drFarmaticClientesAux.Dispose();

                            var nif = ((string)drFarmaticClientes["FIS_NIF"])?.Trim() ?? string.Empty;
                            baja = (ushort) (string.Empty.Equals(nif) || "N".Equals(nif) || "No".Equals(nif) ? 0 : 1);

                            if (drFarmaticClientes["TipoTarifa"] == null)
                            {
                                lopd = 0;
                            }
                            else if (((string)drFarmaticClientes["TipoTarifa"]).Equals("No") || ((string)drFarmaticClientes["XClie_IdClienteFact"]).Equals("Si"))
                            {
                                lopd = 0;
                            }
                            else
                            {
                                lopd = 1;
                            }


                            if (string.IsNullOrEmpty(sexo) && drFarmaticClientes["Fis_Nombre"] != null)
                            {
                                sexo = (string) drFarmaticClientes["Fis_Nombre"];
                            }

                            Debug.WriteLine("[DATA] " + drFarmaticClientes[0] + " -- " + drFarmaticClientes[1]);
                        }
                        drFarmaticClientes.Close();
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine("[ERROR] " + e.Message);
                    }
                    finally
                    {
                        connFarmatic.Close(); Debug.WriteLine("[INFO] Close Farmatic database");
                        connFisiotes.Close(); Debug.WriteLine("[INFO] Close Fisiotes database");
                    }
                }
            }
        }
    }
}