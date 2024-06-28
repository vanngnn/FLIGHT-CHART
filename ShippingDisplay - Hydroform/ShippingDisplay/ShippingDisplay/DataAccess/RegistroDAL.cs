using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Transactions;
using System.Data.SqlClient;
using System.Configuration;
using ShippingDisplay.ShippingDisplay.DataAccess.Entidades;
using System.Net.NetworkInformation;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ShippingDisplay.ShippingDisplay.DataAccess
{
    public class RegistroDAL
    {
        //ADDING NEW DATA TO THE TABLE - DONE
        public static Registro AgregarNuevo(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                //OBTENER EL ID UNICO DEL ISSUE
                int ID;
                string query = @"INSERT INTO LogInput (EntryDate,From_time,To_time,Part_number,Id_cliente,Id_planta,Id_carrier,Bill_of_Lading,Quantity,Dock,shipStatus, shipReason, shipComment)
                                 VALUES (@assignedDate, @assignedFromtime,@assignedTotime,@partNumber,@Id_cliente,@Id_planta,@Id_carrier,@assignedBOL,@assignedQTY,@assignedDock,@shipStatus,@shipReason,@shipComment); SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@assignedDate", Reg.assignedDate);
                cmd.Parameters.AddWithValue("@assignedFromtime", Reg.assignedFromtime);
                cmd.Parameters.AddWithValue("@assignedTotime", Reg.assignedTotime);
                cmd.Parameters.AddWithValue("@partNumber", Reg.partNumber);
                cmd.Parameters.AddWithValue("@Id_cliente", Reg.Id_cliente);
                cmd.Parameters.AddWithValue("@Id_planta", Reg.Id_planta);
                cmd.Parameters.AddWithValue("@Id_carrier", Reg.Id_carrier);
                cmd.Parameters.AddWithValue("@assignedBOL", Reg.assignedBOL);
                cmd.Parameters.AddWithValue("@assignedQTY", Reg.assignedQTY);
                cmd.Parameters.AddWithValue("@assignedDock", Reg.assignedDock);
                cmd.Parameters.AddWithValue("@shipStatus", Reg.shipStatus);
                cmd.Parameters.AddWithValue("@shipReason", Reg.shipReason);
                cmd.Parameters.AddWithValue("@shipComment", Reg.shipComment);

                //RECUPERAR ID GENERADO POR LA TAB
                Reg.Id_all = Convert.ToInt32(cmd.ExecuteScalar());
                ID = Reg.Id_all;
            }
            return Reg;
        }
        //UPDATE TABLE (THE WHOLE TABLE) - DONE
        public static Registro ActualizarRegistro(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                // Update statement for LogInput
                string query = @"UPDATE LogInput 
                         SET EntryDate = @EntryDate, 
                             From_time = @From_time, 
                             To_time = @To_time, 
                             Part_number = @Part_number, 
                             Id_cliente = @Id_cliente, 
                             Id_planta = @Id_planta, 
                             Id_carrier = @Id_carrier, 
                             Bill_of_Lading = @Bill_of_Lading, 
                             Quantity = @Quantity, 
                             Dock = @Dock, 
                             shipStatus = @shipStatus, 
                             shipReason = @shipReason, 
                             shipComment = @shipComment 
                         WHERE Id_all = @Id_all";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EntryDate", Reg.assignedDate);
                cmd.Parameters.AddWithValue("@From_time", Reg.assignedFromtime);
                cmd.Parameters.AddWithValue("@To_time", Reg.assignedTotime);
                cmd.Parameters.AddWithValue("@Part_number", Reg.partNumber);
                cmd.Parameters.AddWithValue("@Id_cliente", Reg.Id_cliente);
                cmd.Parameters.AddWithValue("@Id_planta", Reg.Id_planta);
                cmd.Parameters.AddWithValue("@Id_carrier", Reg.Id_carrier);
                cmd.Parameters.AddWithValue("@Bill_of_Lading", Reg.assignedBOL);
                cmd.Parameters.AddWithValue("@Quantity", Reg.assignedQTY);
                cmd.Parameters.AddWithValue("@Dock", Reg.assignedDock);
                cmd.Parameters.AddWithValue("@shipStatus", Reg.shipStatus);
                cmd.Parameters.AddWithValue("@shipReason", Reg.shipReason);
                cmd.Parameters.AddWithValue("@shipComment", Reg.shipComment);
                cmd.Parameters.AddWithValue("@Id_all", Reg.Id_all); // Assuming Id_all is the primary key
                cmd.ExecuteNonQuery(); // Execute the update query
            }
            return Reg;
        }
        //DELETE RECORDS FROM THE TABLE - DONE
        public static void EliminarRegistro(int Id_all)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //Eliminar registros de categorias
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
                {
                    conn.Open();
                    string query = @"DELETE FROM LogInput WHERE Id_all = @Id_all";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id_all", Id_all);
                    cmd.ExecuteNonQuery();
                }
                scope.Complete();
            }
        }

        //OBTENER REGISTRO BY ID - DONE
        public static Registro ObtenerById(int Id_all)
        {
            Registro Reg = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all, H.Id_cliente, H.Id_carrier, H.From_time, H.Tarjeta, D.Placas,  D.Caja, D.NombreOperador, D.Telefono, 
                            C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant', H.Id_planta
                            FROM [dbo].[LogInput] H
                            INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier WHERE  H.Id_all=@Id_all
                            INNER JOIN [dbo].[Planta]  P ON H.Id_planta = P.id_planta";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_all", Id_all);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Reg = ConvertirRegistro(reader);
                }
            }
            return Reg;
        }
        //OBTAIN BY TARGET? - NOT DONE
        //public static Registro ObtenerByTarget(int Tarjeta)
        //{
        //    Registro Reg = null;
        //   using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
        // {
        //   conn.Open();
        // string query = @"SELECT H.Id_all, H.Id_cliente, H.Id_carrier, H.Entrada, H.Salida, H.Tarjeta, D.Placas,  D.Caja, D.NombreOperador, D.Telefono, 
        //           C.description AS 'Cliente', L.description AS 'Carrier',  H.Id_planta
        //         FROM [dbo].[LogInput] H
        //       INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
        //     INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
        //   INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier WHERE Tarjeta";
        //SqlCommand cmd = new SqlCommand(query, conn);
        //cmd.Parameters.AddWithValue("@Tarjeta", Tarjeta);
        //SqlDataReader reader = cmd.ExecuteReader();
        //if (reader.Read())
        //{
        //  Reg = ConvertirRegistro(reader);
        //}
        //}
        //return Reg;
        //}
        //UPDATE THE OUTPUT? - NOT DONE
        //public static Registro ActualizarSalida(Registro Reg)
        //{
        //  using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
        //{
        //  conn.Open();
        //string query = @"UPDATE LogInput SET  Salida = GETDATE(), shipStatus=@shipStatus WHERE Id_all = @Id_all";
        //SqlCommand cmd = new SqlCommand(query, conn);
        //cmd.Parameters.AddWithValue("@Id_all", Reg.Id_all);
        //cmd.Parameters.AddWithValue("@shipStatus", Reg.Status);
        //cmd.ExecuteNonQuery();
        //}
        //return Reg;
        //}

        //RETURN LIST TO DIPSLAY IN DAILY LOG INPUT
        public static List<Registro> ListadoRegistros(int Status)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all,H.EntryDate,H.From_time, H.To_time, H.Part_number, H.Id_cliente, H.Id_planta, H.Id_carrier, H.Bill_of_Lading, H.Quantity, H.Dock,
                    CASE 
                    WHEN GETDATE() < H.EntryDate THEN 'ONTIME'
                    WHEN GETDATE() >= H.EntryDate AND CAST(GETDATE() AS TIME) < CAST(H.From_time AS TIME) THEN 'ON TIME'
                    WHEN GETDATE() >= H.EntryDate AND CAST(GETDATE() AS TIME) BETWEEN CAST(H.From_time AS TIME) AND CAST(H.To_time AS TIME) THEN 'ONTIME'
                    ELSE 'DELAYED' END AS shipStatus,
                    H.shipReason, H.shipComment, 
                    C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant'
                    FROM [dbo].[LogInput] H
                    INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                    INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier
                    INNER JOIN [dbo].[Planta]  P ON H.Id_planta  = P.id_planta
                    ORDER BY H.EntryDate ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirRegistro(reader));
                }
            }
            return lista;
        }

        // CONVERTIR REGISTRO - DONE

        private static Registro ConvertirRegistro(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all = Convert.ToInt32(reader["Id_all"]);
            Reg.assignedDate = Convert.ToDateTime(reader["EntryDate"]);
            Reg.assignedFromtime = Convert.ToString(reader["From_time"]);
            Reg.assignedTotime = Convert.ToString(reader["To_time"]);
            Reg.partNumber = Convert.ToString(reader["Part_number"]);
            Reg.ClienteName = Convert.ToString(reader["Cliente"]);
            Reg.CarrierName = Convert.ToString(reader["Carrier"]);
            Reg.Id_cliente = Convert.ToInt32(reader["Id_cliente"]);
            Reg.Id_planta = Convert.ToInt32(reader["Id_planta"]);
            Reg.PlantName = Convert.ToString(reader["Plant"]);
            Reg.Id_carrier = Convert.ToInt32(reader["Id_carrier"]);
            Reg.assignedBOL = Convert.ToInt32(reader["Bill_of_Lading"]);
            Reg.assignedQTY = Convert.ToInt32(reader["Quantity"]);
            Reg.assignedDock = Convert.ToString(reader["Dock"]);
            Reg.shipStatus = Convert.ToString(reader["shipStatus"]);
            Reg.shipReason = Convert.ToString(reader["shipReason"]);
            Reg.shipComment = Convert.ToString(reader["shipComment"]);
            //Reg.Entrada = Convert.ToDateTime(reader["Entrada"]);
            //Reg.Salida = Convert.ToString(reader["Salida"]);
            //Reg.Tarjeta = Convert.ToInt32(reader["Tarjeta"]);
            //Reg.Placas = Convert.ToString(reader["Placas"]);
            //Reg.Caja = Convert.ToString(reader["Caja"]);
            //Reg.NombreOperador = Convert.ToString(reader["NombreOperador"]);
            //Reg.Telefono = Convert.ToString(reader["Telefono"]);

            return Reg;
        }
        //GRIDVIEW X PLANTA - LIST DASHBOARD - DONE

        //public static List<Registro> ListadoDashboard (int Id_planta)
        //{
        //   List<Registro> lista = new List<Registro>();
        // using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
        //{
        //    conn.Open();
        //   string query = @"SELECT H.Id_all,H.EntryDate,H.From_time,H.To_time AS TimeRange,H.Part_number, H.Id_cliente, H.Id_planta,H.Id_carrier,H.Bill_of_Lading, H.Quantity,H.Dock,H.shipStatus, H.shipReason,H.shipComment, C.description AS 'Cliente',L.description AS 'Carrier',
        //          CASE WHEN shipStatus = 3 THEN 'SHIPPED' WHEN shipStatus = 1 THEN 'EARRING'
        //               --WHEN shipStatus = 2 THEN 'ONTIME'
        //               WHEN shipStatus = 2 THEN 'ONTIME'
        //               WHEN shipStatus = 2 THEN 'DELAYED' END AS ESTADO 
        //          FROM [dbo].[LogInput] H
        //         INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
        //         INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
        //         INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier 
        //         WHERE  H.Id_planta=@Id_planta ";
        // SqlCommand cmd = new SqlCommand(query, conn);
        //cmd.Parameters.AddWithValue("@Id_planta", Id_planta);
        // SqlDataReader reader = cmd.ExecuteReader();
        //while (reader.Read())
        //{
        //   lista.Add(ConvertirDash(reader));
        //    }
        // }
        //   return lista;
        //}

        //CONVERT VALUES - DONE
        private static Registro ConvertirDash(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all = Convert.ToInt32(reader["Id_all"]);
            Reg.assignedDate = Convert.ToDateTime(reader["EntryDate"]);
            Reg.assignedFromtime = Convert.ToString(reader["From_time"]);
            Reg.assignedTotime = Convert.ToString(reader["To_time"]);
            Reg.partNumber = Convert.ToString(reader["Part_number"]);
            Reg.Id_cliente = Convert.ToInt32(reader["Id_cliente"]);
            Reg.Id_planta = Convert.ToInt32(reader["Id_planta"]);
            Reg.PlantName = Convert.ToString(reader["Plant"]);
            Reg.Id_carrier = Convert.ToInt32(reader["Id_carrier"]);
            Reg.assignedBOL = Convert.ToInt32(reader["Bill_of_Lading"]);
            Reg.assignedQTY = Convert.ToInt32(reader["Quantity"]);
            Reg.assignedDock = Convert.ToString(reader["Dock"]);
            Reg.shipStatus = Convert.ToString(reader["shipStatus"]);
            Reg.shipReason = Convert.ToString(reader["shipReason"]);
            Reg.shipComment = Convert.ToString(reader["shipComment"]);
            Reg.ClienteName = Convert.ToString(reader["Cliente"]);
            Reg.CarrierName = Convert.ToString(reader["Carrier"]);

            //Reg.Salida = Convert.ToString(reader["Salida"]);
            //Reg.Caja = Convert.ToString(reader["Caja"]);
            //Reg.RutaName = Convert.ToString(reader["Ruta"]);
            //Reg.Shipper = Convert.ToInt32(reader["Shipper"]);
            //Reg.Estado = Convert.ToString(reader["Estado"]);
            //Reg.Id_planta = Convert.ToInt32(reader["Id_planta"]);
            return Reg;
        }
        //ACTUALIZAR SHIPPER  - NOT DONE (DONT NEED TO BUT SHOULD BE LOOKED INTO)
        public static Registro ActualizarShipper(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"UPDATE LogInput SET  Shipper = @Shipper,shipStatus=@shipStatus WHERE Id_all = @Id_all";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_all", Reg.Id_all);
                cmd.Parameters.AddWithValue("@Shipper", Reg.Shipper);
                cmd.Parameters.AddWithValue("@shipStatus", Reg.Status);
                cmd.ExecuteNonQuery();
            }
            return Reg;
        }
        //OBTENER WIDGETS - NOT DONE
        public static Registro ObtenerRegistros(int Id_planta)
        {
            Registro list = null;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                con.Open();
                string query = @"
                        SELECT DISTINCT  
                        (SELECT ISNULL((Count(H.shipStatus)),0) FROM LogInput H  WHERE H.shipStatus = 3 = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta ) AS 'SHIPPED',

                        (SELECT ISNULL((Count(H.shipStatus)),0) FROM LogInput H  WHERE H.shipStatus = 1 = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta) AS 'EARRING',

                        (SELECT ISNULL((Count(H.shipStatus)),0) FROM LogInput H  

                        WHERE shipStatus = 2 = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta)AS 'ONTIME' ,

                        (SELECT ISNULL((Count(H.shipStatus)),0) AS 'DELAYED' FROM LogInput H 
                        WHERE H.shipStatus = 2  = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta) AS 'DELAYED' 
                        FROM LogInput ";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id_planta", Id_planta);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    list = Convertir(reader);
                }
            }
            return list;
        }

        //DONT KNOW WHAT THIS IS FOR - NOT DONE

        private static Registro Convertir(IDataReader reader)
        {
            Registro list = new Registro();
            list.Completed = Convert.ToString(reader["SHIPPED"]);
            list.Pendiente = Convert.ToString(reader["EARRING"]);
            list.Ontime = Convert.ToString(reader["ONTIME"]);
            list.DELAYED = Convert.ToString(reader["DELAYED"]);
            return list;
        }

        //FITLER REPORT - NOT DONE
        public static List<Registro> FiltroReporte(int Id_planta, string FechaIni, string FechaFin)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"
                        SELECT H.Id_all, H.Id_cliente, H.Id_carrier, CASE WHEN H.Salida='1900-01-01 00:00:00.000' THEN '-' WHEN H.Salida<>'1900-01-01 00:00:00.000' THEN Convert(nvarchar,H.Salida,21) END AS 'Salida',
                        C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant', H.Part_number, H.Bill_of_Lading, H.Quantity, H.Dock, H.shipStatus, H.shipReason, H.shipComment, H.Entrada, H.Salida,
                        R.input, R.output,
                        H.Shipper, H.Id_planta, 
                        CASE WHEN shipStatus = 1 THEN 'NO SHIPPER ASSIGNMENT'
                        WHEN H.Salida ='1900-01-01 00:00:00.000' THEN 'DO NOT REGISTER OUT;
                        FROM [dbo].[LogInput] H
                        INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
                        INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                        INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier 
                        WHERE  H.Id_planta=@Id_planta AND CONVERT(Date, ) BETWEEN '" + FechaIni + "' AND ' " + @FechaFin + "'  ORDER BY H.Entrada desc ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_planta", Id_planta);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirFiltro(reader));
                }
            }
            return lista;
        }

        //DONT KNOW WHAT THIS IS FOR - NOT DONE
        private static Registro ConvertirFiltro(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all = Convert.ToInt32(reader["Id_all"]);
            Reg.assignedDate = Convert.ToDateTime(reader["EntryDate"]);
            Reg.assignedFromtime = Convert.ToString(reader["From_time"]);
            Reg.assignedTotime = Convert.ToString(reader["To_time"]);
            Reg.partNumber = Convert.ToString(reader["Part_number"]);
            Reg.Id_cliente = Convert.ToInt32(reader["Id_cliente"]);
            Reg.Id_planta = Convert.ToInt32(reader["Id_planta"]);
            Reg.Id_carrier = Convert.ToInt32(reader["Id_carrier"]);
            Reg.PlantName = Convert.ToString(reader["Plant"]);
            Reg.assignedBOL = Convert.ToInt32(reader["Bill_of_Lading"]);
            Reg.assignedQTY = Convert.ToInt32(reader["Quantity"]);
            Reg.assignedDock = Convert.ToString(reader["Dock"]);
            Reg.shipStatus = Convert.ToString(reader["shipStatus"]);
            Reg.shipReason = Convert.ToString(reader["shipReason"]);
            Reg.shipComment = Convert.ToString(reader["shipComment"]);
            Reg.ClienteName = Convert.ToString(reader["Cliente"]);
            Reg.CarrierName = Convert.ToString(reader["Carrier"]);

            Reg.PlantName = Convert.ToString(reader["Plant"]);

            return Reg;
        }
        //                                                                 OUTPUT 

        //ADDING NEW DATA TO THE TABLE - DONE
        public static Registro AgregarNuevo_output(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                //OBTENER EL ID UNICO DEL ISSUE
                int ID;

                string query = @"INSERT INTO LogOutput (EntryDate_output,From_time_output,To_time_output,Part_number_output,Id_cliente_output,Id_planta_output,Id_carrier_output,Bill_of_Lading_output,Quantity_output,Dock_output,shipStatus_output, shipReason_output, shipComment_output)
                                 VALUES (@assignedDate_output, @assignedFromtime_output,@assignedTotime_output,@partNumber_output,@Id_cliente_output,@Id_planta_output,@Id_carrier_output,@assignedBOL_output,@assignedQTY_output,@assignedDock_output,@shipStatus_output,@shipReason_output,@shipComment_output); SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@assignedDate_output", Reg.assignedDate_output);
                cmd.Parameters.AddWithValue("@assignedFromtime_output", Reg.assignedFromtime_output);
                cmd.Parameters.AddWithValue("@assignedTotime_output", Reg.assignedTotime_output);
                cmd.Parameters.AddWithValue("@partNumber_output", Reg.partNumber_output);
                cmd.Parameters.AddWithValue("@Id_cliente_output", Reg.Id_cliente_output);
                cmd.Parameters.AddWithValue("@Id_planta_output", Reg.Id_planta_output);
                cmd.Parameters.AddWithValue("@Id_carrier_output", Reg.Id_carrier_output);
                cmd.Parameters.AddWithValue("@assignedBOL_output", Reg.assignedBOL_output);
                cmd.Parameters.AddWithValue("@assignedQTY_output", Reg.assignedQTY_output);
                cmd.Parameters.AddWithValue("@assignedDock_output", Reg.assignedDock_output);
                cmd.Parameters.AddWithValue("@shipStatus_output", Reg.shipStatus_output);
                cmd.Parameters.AddWithValue("@shipReason_output", Reg.shipReason_output);
                cmd.Parameters.AddWithValue("@shipComment_output", Reg.shipComment_output);

                //RECUPERAR ID GENERADO POR LA TAB
                Reg.Id_all_output = Convert.ToInt32(cmd.ExecuteScalar());
                ID = Reg.Id_all_output;
            }
            return Reg;
        }

        //UPDATE TABLE - DONE
        public static Registro ActualizarRegistro_output(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                // Update statement for LogInput
                string query = @"UPDATE LogOutput 
                         SET EntryDate_output = @EntryDate_output, 
                             From_time_output = @From_time_output, 
                             To_time_output = @To_time_output, 
                             Part_number_output = @Part_number_output, 
                             Id_cliente_output = @Id_cliente_output, 
                             Id_planta_output = @Id_planta_output, 
                             Id_carrier_output = @Id_carrier_output, 
                             Bill_of_Lading_output = @Bill_of_Lading_output, 
                             Quantity_output = @Quantity_output, 
                             Dock_output = @Dock_output, 
                             shipStatus_output = @shipStatus_output, 
                             shipReason_output = @shipReason_output, 
                             shipComment_output = @shipComment_output 
                         WHERE Id_all_output = @Id_all_output";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EntryDate_output", Reg.assignedDate_output);
                cmd.Parameters.AddWithValue("@From_time_output", Reg.assignedFromtime_output);
                cmd.Parameters.AddWithValue("@To_time_output", Reg.assignedTotime_output);
                cmd.Parameters.AddWithValue("@Part_number_output", Reg.partNumber_output);
                cmd.Parameters.AddWithValue("@Id_cliente_output", Reg.Id_cliente_output);
                cmd.Parameters.AddWithValue("@Id_planta_output", Reg.Id_planta_output);
                cmd.Parameters.AddWithValue("@Id_carrier_output", Reg.Id_carrier_output);
                cmd.Parameters.AddWithValue("@Bill_of_Lading_output", Reg.assignedBOL_output);
                cmd.Parameters.AddWithValue("@Quantity_output", Reg.assignedQTY_output);
                cmd.Parameters.AddWithValue("@Dock_output", Reg.assignedDock_output);
                cmd.Parameters.AddWithValue("@shipStatus_output", Reg.shipStatus_output);
                cmd.Parameters.AddWithValue("@shipReason_output", Reg.shipReason_output);
                cmd.Parameters.AddWithValue("@shipComment_output", Reg.shipComment_output);
                cmd.Parameters.AddWithValue("@Id_all_output", Reg.Id_all_output); // Assuming Id_all is the primary key
                cmd.ExecuteNonQuery(); // Execute the update query
            }
            return Reg;
        }

        //DELETE RECORDS FROM THE TABLE - DONE
        public static void EliminarRegistro_output(int Id_all_output)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //Eliminar registros de categorias
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
                {
                    conn.Open();
                    string query = @"DELETE FROM LogOutput WHERE Id_all_output = @Id_all_output";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id_all_output", Id_all_output);
                    cmd.ExecuteNonQuery();
                }
                scope.Complete();
            }
        }

        //OBTENER REGISTRO BY ID - NOT TO BOTHER RIGHT NOW
        public static Registro ObtenerById_output(int Id_all)
        {
            Registro Reg = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all_output, H.Id_cliente_output, H.Id_carrier_output, H.From_time_output, H.Tarjeta, D.Placas,  D.Caja, D.NombreOperador, D.Telefono, 
                            C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant', H.Id_planta
                            FROM [dbo].[LogInput] H
                            INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier WHERE  H.Id_all=@Id_all
                            INNER JOIN [dbo].[Planta]  P ON H.Id_planta = P.id_planta";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_all", Id_all);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Reg = ConvertirRegistro(reader);
                }
            }
            return Reg;
        }
        //OBTAIN BY TARGET? - NOT DONE
        //public static Registro ObtenerByTarget(int Tarjeta)
        //{
        //    Registro Reg = null;
        //   using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
        // {
        //   conn.Open();
        // string query = @"SELECT H.Id_all, H.Id_cliente, H.Id_carrier, H.Entrada, H.Salida, H.Tarjeta, D.Placas,  D.Caja, D.NombreOperador, D.Telefono, 
        //           C.description AS 'Cliente', L.description AS 'Carrier',  H.Id_planta
        //         FROM [dbo].[LogInput] H
        //       INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
        //     INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
        //   INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier WHERE Tarjeta";
        //SqlCommand cmd = new SqlCommand(query, conn);
        //cmd.Parameters.AddWithValue("@Tarjeta", Tarjeta);
        //SqlDataReader reader = cmd.ExecuteReader();
        //if (reader.Read())
        //{
        //  Reg = ConvertirRegistro(reader);
        //}
        //}
        //return Reg;
        //}
        //UPDATE THE OUTPUT? - NOT DONE
        //public static Registro ActualizarSalida(Registro Reg)
        //{
        //  using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
        //{
        //  conn.Open();
        //string query = @"UPDATE LogInput SET  Salida = GETDATE(), shipStatus=@shipStatus WHERE Id_all = @Id_all";
        //SqlCommand cmd = new SqlCommand(query, conn);
        //cmd.Parameters.AddWithValue("@Id_all", Reg.Id_all);
        //cmd.Parameters.AddWithValue("@shipStatus", Reg.Status);
        //cmd.ExecuteNonQuery();
        //}
        //return Reg;
        //}

        //RETURN LIST TO DIPSLAY IN DAILY LOG INPUT
        public static List<Registro> ListadoRegistros_output(int Status)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all_output,H.EntryDate_output,H.From_time_output, H.To_time_output, H.Part_number_output, H.Id_cliente_output, H.Id_planta_output, H.Id_carrier_output, H.Bill_of_Lading_output, H.Quantity_output, H.Dock_output,
                    CASE 
                    WHEN GETDATE() < H.EntryDate_output THEN 'ONTIME'
                    WHEN GETDATE() >= H.EntryDate_output AND CAST(GETDATE() AS TIME) < CAST(H.From_time_output AS TIME) THEN 'ON TIME'
                    WHEN GETDATE() >= H.EntryDate_output AND CAST(GETDATE() AS TIME) BETWEEN CAST(H.From_time_output AS TIME) AND CAST(H.To_time_output AS TIME) THEN 'ONTIME'
                    ELSE 'DELAYED' END AS shipStatus_output,
                    H.shipReason_output, H.shipComment_output, 
                    C.description AS 'Cliente_output', L.description AS 'Carrier_output', P.description AS 'Plant_output'
                    FROM [dbo].[LogOutput] H
                    INNER JOIN [dbo].[Cliente] C ON H.Id_cliente_output = C.id_cliente
                    INNER JOIN [dbo].[Carrier] L ON H.Id_carrier_output = L.id_carrier
                    INNER JOIN [dbo].[Planta]  P ON H.Id_planta_output  = P.id_planta
                    ORDER BY H.EntryDate_output ASC";
                //WHERE H.shipStatus=@shipStatus AND H.Id_planta=@Id_planta ORDER BY H.Entrada desc (belong in query)

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirRegistro_output(reader));
                }
            }
            return lista;
        }

        // CONVERTIR REGISTRO - DONE

        private static Registro ConvertirRegistro_output(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all_output = Convert.ToInt32(reader["Id_all_output"]);
            Reg.assignedDate_output = Convert.ToDateTime(reader["EntryDate_output"]);
            Reg.assignedFromtime_output = Convert.ToString(reader["From_time_output"]);
            Reg.assignedTotime_output = Convert.ToString(reader["To_time_output"]);
            Reg.partNumber_output = Convert.ToString(reader["Part_number_output"]);
            Reg.ClienteName_output = Convert.ToString(reader["Cliente_output"]);
            Reg.CarrierName_output = Convert.ToString(reader["Carrier_output"]);
            Reg.Id_cliente_output = Convert.ToInt32(reader["Id_cliente_output"]);
            Reg.Id_planta_output = Convert.ToInt32(reader["Id_planta_output"]);
            Reg.PlantName_output = Convert.ToString(reader["Plant_output"]);
            Reg.Id_carrier_output = Convert.ToInt32(reader["Id_carrier_output"]);
            Reg.assignedBOL_output = Convert.ToInt32(reader["Bill_of_Lading_output"]);
            Reg.assignedQTY_output = Convert.ToInt32(reader["Quantity_output"]);
            Reg.assignedDock_output = Convert.ToString(reader["Dock_output"]);
            Reg.shipStatus_output = Convert.ToString(reader["shipStatus_output"]);
            Reg.shipReason_output = Convert.ToString(reader["shipReason_output"]);
            Reg.shipComment_output = Convert.ToString(reader["shipComment_output"]);
            //Reg.Entrada = Convert.ToDateTime(reader["Entrada"]);
            //Reg.Salida = Convert.ToString(reader["Salida"]);
            //Reg.Tarjeta = Convert.ToInt32(reader["Tarjeta"]);
            //Reg.Placas = Convert.ToString(reader["Placas"]);
            //Reg.Caja = Convert.ToString(reader["Caja"]);
            //Reg.NombreOperador = Convert.ToString(reader["NombreOperador"]);
            //Reg.Telefono = Convert.ToString(reader["Telefono"]);

            return Reg;
        }
        //GRIDVIEW X PLANTA - LIST DASHBOARD - DONE

        //public static List<Registro> ListadoDashboard (int Id_planta)
        //{
        //   List<Registro> lista = new List<Registro>();
        // using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
        //{
        //    conn.Open();
        //   string query = @"SELECT H.Id_all,H.EntryDate,H.From_time,H.To_time AS TimeRange,H.Part_number, H.Id_cliente, H.Id_planta,H.Id_carrier,H.Bill_of_Lading, H.Quantity,H.Dock,H.shipStatus, H.shipReason,H.shipComment, C.description AS 'Cliente',L.description AS 'Carrier',
        //          CASE WHEN shipStatus = 3 THEN 'SHIPPED' WHEN shipStatus = 1 THEN 'EARRING'
        //               --WHEN shipStatus = 2 THEN 'ONTIME'
        //               WHEN shipStatus = 2 THEN 'ONTIME'
        //               WHEN shipStatus = 2 THEN 'DELAYED' END AS ESTADO 
        //          FROM [dbo].[LogInput] H
        //         INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
        //         INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
        //         INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier 
        //         WHERE  H.Id_planta=@Id_planta ";
        // SqlCommand cmd = new SqlCommand(query, conn);
        //cmd.Parameters.AddWithValue("@Id_planta", Id_planta);
        // SqlDataReader reader = cmd.ExecuteReader();
        //while (reader.Read())
        //{
        //   lista.Add(ConvertirDash(reader));
        //    }
        // }
        //   return lista;
        //}

        //DONT KNOW WHAT THIS IS FOR - DONE
        private static Registro ConvertirDash_output(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all_output = Convert.ToInt32(reader["Id_all"]);
            Reg.assignedDate_output = Convert.ToDateTime(reader["EntryDate_output"]);
            Reg.assignedFromtime_output = Convert.ToString(reader["From_time_output"]);
            Reg.assignedTotime_output = Convert.ToString(reader["To_time_output"]);
            Reg.partNumber_output = Convert.ToString(reader["Part_number_output"]);
            Reg.Id_cliente_output = Convert.ToInt32(reader["Id_cliente_output"]);
            Reg.Id_planta_output = Convert.ToInt32(reader["Id_planta_output"]);
            Reg.PlantName_output = Convert.ToString(reader["Plant_output"]);
            Reg.Id_carrier_output = Convert.ToInt32(reader["Id_carrier_output"]);
            Reg.assignedBOL_output = Convert.ToInt32(reader["Bill_of_Lading_output"]);
            Reg.assignedQTY_output = Convert.ToInt32(reader["Quantity_output"]);
            Reg.assignedDock_output = Convert.ToString(reader["Dock_output"]);
            Reg.shipStatus_output = Convert.ToString(reader["shipStatus_output"]);
            Reg.shipReason_output = Convert.ToString(reader["shipReason_output"]);
            Reg.shipComment_output = Convert.ToString(reader["shipComment_output"]);
            Reg.ClienteName_output = Convert.ToString(reader["Cliente_output"]);
            Reg.CarrierName_output = Convert.ToString(reader["Carrier_output"]);

            //Reg.Salida = Convert.ToString(reader["Salida"]);
            //Reg.Caja = Convert.ToString(reader["Caja"]);
            //Reg.RutaName = Convert.ToString(reader["Ruta"]);
            //Reg.Shipper = Convert.ToInt32(reader["Shipper"]);
            //Reg.Estado = Convert.ToString(reader["Estado"]);
            //Reg.Id_planta = Convert.ToInt32(reader["Id_planta"]);
            return Reg;
        }
        //ACTUALIZAR SHIPPER  - NOT DONE (DONT NEED TO BUT SHOULD BE LOOKED INTO)
        public static Registro ActualizarShipper_output(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"UPDATE LogOutput SET  Shipper = @Shipper,shipStatus=@shipStatus WHERE Id_all = @Id_all";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_all", Reg.Id_all);
                cmd.Parameters.AddWithValue("@Shipper", Reg.Shipper);
                cmd.Parameters.AddWithValue("@shipStatus", Reg.Status);
                cmd.ExecuteNonQuery();
            }
            return Reg;
        }
        //OBTENER WIDGETS - NOT DONE
        public static Registro ObtenerRegistros_output(int Id_planta)
        {
            Registro list = null;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                con.Open();
                string query = @"
                        SELECT DISTINCT  
                        (SELECT ISNULL((Count(H.shipStatus)),0) FROM LogInput H  WHERE H.shipStatus = 3 = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta ) AS 'SHIPPED',

                        (SELECT ISNULL((Count(H.shipStatus)),0) FROM LogInput H  WHERE H.shipStatus = 1 = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta) AS 'EARRING',

                        (SELECT ISNULL((Count(H.shipStatus)),0) FROM LogInput H  

                        WHERE shipStatus = 2 = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta)AS 'ONTIME' ,

                        (SELECT ISNULL((Count(H.shipStatus)),0) AS 'DELAYED' FROM LogInput H 
                        WHERE H.shipStatus = 2  = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta) AS 'DELAYED' 
                        FROM LogInput ";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id_planta", Id_planta);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    list = Convertir_output(reader);
                }
            }
            return list;
        }

        //DONT KNOW WHAT THIS IS FOR - NOT DONE

        private static Registro Convertir_output(IDataReader reader)
        {
            Registro list = new Registro();
            list.Completed = Convert.ToString(reader["SHIPPED"]);
            list.Pendiente = Convert.ToString(reader["EARRING"]);
            list.Ontime = Convert.ToString(reader["ONTIME"]);
            list.DELAYED = Convert.ToString(reader["DELAYED"]);
            return list;
        }

        //FITLER REPORT - NOT DONE
        public static List<Registro> FiltroReporte_output(int Id_planta, string FechaIni, string FechaFin)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"
                        SELECT H.Id_all, H.Id_cliente, H.Id_carrier, CASE WHEN H.Salida='1900-01-01 00:00:00.000' THEN '-' WHEN H.Salida<>'1900-01-01 00:00:00.000' THEN Convert(nvarchar,H.Salida,21) END AS 'Salida',
                        C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant', H.Part_number, H.Bill_of_Lading, H.Quantity, H.Dock, H.shipStatus, H.shipReason, H.shipComment, H.Entrada, H.Salida,
                        R.input, R.output,
                        H.Shipper, H.Id_planta, 
                        CASE WHEN shipStatus = 1 THEN 'NO SHIPPER ASSIGNMENT'
                        WHEN H.Salida ='1900-01-01 00:00:00.000' THEN 'DO NOT REGISTER OUT;
                        FROM [dbo].[LogInput] H
                        INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
                        INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                        INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier 
                        WHERE  H.Id_planta=@Id_planta AND CONVERT(Date, ) BETWEEN '" + FechaIni + "' AND ' " + @FechaFin + "'  ORDER BY H.Entrada desc ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_planta", Id_planta);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirFiltro_output(reader));
                }
            }
            return lista;
        }

        //DONT KNOW WHAT THIS IS FOR - NOT DONE
        private static Registro ConvertirFiltro_output(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all = Convert.ToInt32(reader["Id_all"]);
            Reg.assignedDate = Convert.ToDateTime(reader["EntryDate"]);
            Reg.assignedFromtime = Convert.ToString(reader["From_time"]);
            Reg.assignedTotime = Convert.ToString(reader["To_time"]);
            Reg.partNumber = Convert.ToString(reader["Part_number"]);
            Reg.Id_cliente = Convert.ToInt32(reader["Id_cliente"]);
            Reg.Id_planta = Convert.ToInt32(reader["Id_planta"]);
            Reg.Id_carrier = Convert.ToInt32(reader["Id_carrier"]);
            Reg.PlantName = Convert.ToString(reader["Plant"]);
            Reg.assignedBOL = Convert.ToInt32(reader["Bill_of_Lading"]);
            Reg.assignedQTY = Convert.ToInt32(reader["Quantity"]);
            Reg.assignedDock = Convert.ToString(reader["Dock"]);
            Reg.shipStatus = Convert.ToString(reader["shipStatus"]);
            Reg.shipReason = Convert.ToString(reader["shipReason"]);
            Reg.shipComment = Convert.ToString(reader["shipComment"]);
            Reg.ClienteName = Convert.ToString(reader["Cliente"]);
            Reg.CarrierName = Convert.ToString(reader["Carrier"]);

            return Reg;
        }

        //                                                                     DOCK FILTER - INPUT
        public static List<Registro> dockQueryInput(string dockName)
        {
            List<Registro> regList = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all,H.EntryDate,H.From_time, H.To_time, H.Part_number, H.Id_cliente, H.Id_planta, H.Id_carrier, H.Bill_of_Lading, H.Quantity, H.Dock,H.shipStatus, H.shipReason, H.shipComment, 
                            C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant'
                            FROM [dbo].[LogInput] H
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier
                            INNER JOIN [dbo].[Planta]  P ON H.Id_planta  = P.id_planta
                            WHERE H.Dock = @dockName  
                            ORDER BY H.EntryDate ASC";


                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dockName", dockName);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Registro Reg = new Registro();
                    Reg.Id_all = Convert.ToInt32(reader["Id_all"]);
                    Reg.assignedDate = Convert.ToDateTime(reader["EntryDate"]);
                    Reg.assignedFromtime = Convert.ToString(reader["From_time"]);
                    Reg.assignedTotime = Convert.ToString(reader["To_time"]);
                    Reg.partNumber = Convert.ToString(reader["Part_number"]);
                    Reg.Id_cliente = Convert.ToInt32(reader["Id_cliente"]);
                    Reg.Id_planta = Convert.ToInt32(reader["Id_planta"]);
                    Reg.Id_carrier = Convert.ToInt32(reader["Id_carrier"]);
                    Reg.PlantName = Convert.ToString(reader["Plant"]);
                    Reg.assignedBOL = Convert.ToInt32(reader["Bill_of_Lading"]);
                    Reg.assignedQTY = Convert.ToInt32(reader["Quantity"]);
                    Reg.assignedDock = Convert.ToString(reader["Dock"]);
                    Reg.shipStatus = Convert.ToString(reader["shipStatus"]);
                    Reg.shipReason = Convert.ToString(reader["shipReason"]);
                    Reg.shipComment = Convert.ToString(reader["shipComment"]);
                    Reg.ClienteName = Convert.ToString(reader["Cliente"]);
                    Reg.CarrierName = Convert.ToString(reader["Carrier"]);
                    regList.Add(Reg);
                }
            }
            return regList;
        }

        //                                                                    DOCK FILTER - OUTPUT


        public static List<Registro> dockQueryOutput(string dockName)
        {
            List<Registro> regList = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all_output,H.EntryDate_output,H.From_time_output, H.To_time_output, H.Part_number_output, H.Id_cliente_output, H.Id_planta_output, H.Id_carrier_output, H.Bill_of_Lading_output, H.Quantity_output, H.Dock_output,H.shipStatus_output, H.shipReason_output, H.shipComment_output, 
                            C.description AS 'Cliente_output', L.description AS 'Carrier_output', P.description AS 'Plant_output'
                            FROM [dbo].[LogOutput] H
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente_output = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier_output = L.id_carrier
                            INNER JOIN [dbo].[Planta]  P ON H.Id_planta_output  = P.id_planta
                            WHERE H.Dock_output = @dockName  
                            ORDER BY H.EntryDate_output ASC";


                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dockName", dockName);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Registro Reg = new Registro();
                    Reg.Id_all_output = Convert.ToInt32(reader["Id_all_output"]);
                    Reg.assignedDate_output = Convert.ToDateTime(reader["EntryDate_output"]);
                    Reg.assignedFromtime_output = Convert.ToString(reader["From_time_output"]);
                    Reg.assignedTotime_output = Convert.ToString(reader["To_time_output"]);
                    Reg.partNumber_output = Convert.ToString(reader["Part_number_output"]);
                    Reg.Id_cliente_output = Convert.ToInt32(reader["Id_cliente_output"]);
                    Reg.Id_planta_output = Convert.ToInt32(reader["Id_planta_output"]);
                    Reg.Id_carrier_output = Convert.ToInt32(reader["Id_carrier_output"]);
                    Reg.PlantName_output = Convert.ToString(reader["Plant_output"]);
                    Reg.assignedBOL_output = Convert.ToInt32(reader["Bill_of_Lading_output"]);
                    Reg.assignedQTY_output = Convert.ToInt32(reader["Quantity_output"]);
                    Reg.assignedDock_output = Convert.ToString(reader["Dock_output"]);
                    Reg.shipStatus_output = Convert.ToString(reader["shipStatus_output"]);
                    Reg.shipReason_output = Convert.ToString(reader["shipReason_output"]);
                    Reg.shipComment_output = Convert.ToString(reader["shipComment_output"]);
                    Reg.ClienteName_output = Convert.ToString(reader["Cliente_output"]);
                    Reg.CarrierName_output = Convert.ToString(reader["Carrier_output"]);
                    regList.Add(Reg);
                }
            }
            return regList;
        }

        //                                                              MUTUAL LIST - FOR BOTH INS AND OUTS (TO DISPLAY ON DASHBOARD)
        public static List<Registro> ListDashboard(int Status, int Id_planta)
        {
            List<Registro> list_dashboard = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all, H.EntryDate AS EntryDate,H.From_time,H.To_time,H.Part_number,H.Id_cliente,H.Id_planta,H.Id_carrier,H.Bill_of_Lading,H.Quantity,H.Dock,H.shipStatus,
                              H.shipReason,H.shipComment,
                              C.description AS Cliente,
                              L.description AS Carrier,
                              P.description AS Plant
              FROM 
                  [dbo].[LogInput] H
              INNER JOIN 
                   [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
              INNER JOIN 
                   [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier
              INNER JOIN 
                   [dbo].[Planta] P ON H.Id_planta = P.id_planta

              UNION

              SELECT 
                   H.Id_all_output,
                   H.EntryDate_output AS EntryDate,
                   H.From_time_output AS From_time,
                   H.To_time_output AS To_time,
                   H.Part_number_output AS Part_number,
                   H.Id_cliente_output AS Id_cliente,
                   H.Id_planta_output AS Id_planta,
                   H.Id_carrier_output AS Id_carrier,
                   H.Bill_of_Lading_output AS Bill_of_Lading,
                   H.Quantity_output AS Quantity,
                   H.Dock_output AS Dock,
                   H.shipStatus_output AS shipStatus,
                   H.shipReason_output AS shipReason,
                   H.shipComment_output AS shipComment,
                   C.description AS Cliente,
                   L.description AS Carrier,
                   P.description AS Plant
              FROM 
                 [dbo].[LogOutput] H
              INNER JOIN 
                   [dbo].[Cliente] C ON H.Id_cliente_output = C.id_cliente
              INNER JOIN 
                    [dbo].[Carrier] L ON H.Id_carrier_output = L.id_carrier
              INNER JOIN 
                    [dbo].[Planta] P ON H.Id_planta_output = P.id_planta
              ORDER BY 
                   EntryDate ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Status", Status);
                cmd.Parameters.AddWithValue("@Id_planta", Id_planta);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Registro Reg = new Registro();
                    {
                        Reg.Id_all = Convert.ToInt32(reader["Id_all"]);
                        Reg.assignedDate = Convert.ToDateTime(reader["EntryDate"]);
                        Reg.assignedFromtime = Convert.ToString(reader["From_time"]);
                        Reg.assignedTotime = Convert.ToString(reader["To_time"]);
                        Reg.partNumber = Convert.ToString(reader["Part_number"]);
                        Reg.Id_cliente = Convert.ToInt32(reader["Id_cliente"]);
                        Reg.Id_planta = Convert.ToInt32(reader["Id_planta"]);
                        Reg.Id_carrier = Convert.ToInt32(reader["Id_carrier"]);
                        Reg.PlantName = Convert.ToString(reader["Plant"]);
                        Reg.assignedBOL = Convert.ToInt32(reader["Bill_of_Lading"]);
                        Reg.assignedQTY = Convert.ToInt32(reader["Quantity"]);
                        Reg.assignedDock = Convert.ToString(reader["Dock"]);
                        Reg.shipStatus = Convert.ToString(reader["shipStatus"]);
                        Reg.shipReason = Convert.ToString(reader["shipReason"]);
                        Reg.shipComment = Convert.ToString(reader["shipComment"]);
                        Reg.ClienteName = Convert.ToString(reader["Cliente"]);
                        Reg.CarrierName = Convert.ToString(reader["Carrier"]);
                        list_dashboard.Add(Reg);
                    }

                }
            }
            return list_dashboard;

        }

        //                                                                     LIST DASHBOARD BUT WITH DOCKS SEPARATED

        public static List<Registro> ListDashboard_Dock(string dockName)
        {
            List<Registro> list_dashboard_dock = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT 'LogInput' AS SourceTable, H.Id_all, H.EntryDate AS EntryDate,H.From_time,H.To_time,H.Part_number,H.Id_cliente,H.Id_planta,H.Id_carrier,H.Bill_of_Lading,H.Quantity,H.Dock,H.shipStatus,
                              H.shipReason,H.shipComment,
                              C.description AS Cliente,
                              L.description AS Carrier,
                              P.description AS Plant
               FROM 
                  [dbo].[LogInput] H
               INNER JOIN 
                   [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
               INNER JOIN 
                   [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier
               INNER JOIN 
                   [dbo].[Planta] P ON H.Id_planta = P.id_planta
               WHERE H.Dock = @dockName    

               UNION

               SELECT 
                   'LogOutput' AS SourceTable,
                   H.Id_all_output,
                   H.EntryDate_output AS EntryDate,
                   H.From_time_output AS From_time,
                   H.To_time_output AS To_time,
                   H.Part_number_output AS Part_number,
                   H.Id_cliente_output AS Id_cliente,
                   H.Id_planta_output AS Id_planta,
                   H.Id_carrier_output AS Id_carrier,
                   H.Bill_of_Lading_output AS Bill_of_Lading,
                   H.Quantity_output AS Quantity,
                   H.Dock_output AS Dock,
                   H.shipStatus_output AS shipStatus,
                   H.shipReason_output AS shipReason,
                   H.shipComment_output AS shipComment,
                   C.description AS Cliente,
                   L.description AS Carrier,
                   P.description AS Plant
               FROM 
                 [dbo].[LogOutput] H
               INNER JOIN 
                   [dbo].[Cliente] C ON H.Id_cliente_output = C.id_cliente
               INNER JOIN 
                    [dbo].[Carrier] L ON H.Id_carrier_output = L.id_carrier
               INNER JOIN 
                    [dbo].[Planta] P ON H.Id_planta_output = P.id_planta
               WHERE H.Dock_output = @dockName
               ORDER BY 
                   EntryDate ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dockName", dockName);
                //cmd.Parameters.AddWithValue("@Id_planta", Id_Planta);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Registro Reg = new Registro()
                    {
                        Id_all = Convert.ToInt32(reader["Id_all"]),
                        assignedDate = Convert.ToDateTime(reader["EntryDate"]),
                        assignedFromtime = Convert.ToString(reader["From_time"]),
                        assignedTotime = Convert.ToString(reader["To_time"]),
                        partNumber = Convert.ToString(reader["Part_number"]),
                        Id_cliente = Convert.ToInt32(reader["Id_cliente"]),
                        Id_planta = Convert.ToInt32(reader["Id_planta"]),
                        Id_carrier = Convert.ToInt32(reader["Id_carrier"]),
                        PlantName = Convert.ToString(reader["Plant"]),
                        assignedBOL = Convert.ToInt32(reader["Bill_of_Lading"]),
                        assignedQTY = Convert.ToInt32(reader["Quantity"]),
                        assignedDock = Convert.ToString(reader["Dock"]),
                        shipStatus = Convert.ToString(reader["shipStatus"]),
                        shipReason = Convert.ToString(reader["shipReason"]),
                        shipComment = Convert.ToString(reader["shipComment"]),
                        ClienteName = Convert.ToString(reader["Cliente"]),
                        CarrierName = Convert.ToString(reader["Carrier"]),
                        IsInput = String.Equals(Convert.ToString(reader["SourceTable"]), "LogInput", StringComparison.OrdinalIgnoreCase)
                    };
                    list_dashboard_dock.Add(Reg);
                }
            }
            return list_dashboard_dock;
        }
    }
}