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
        //ADDING NEW DATA TO THE TABLE - HYDROFORM INPUT - DONE
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

                Reg.Id_all = Convert.ToInt32(cmd.ExecuteScalar());
                ID = Reg.Id_all;
            }
            return Reg;
        }
        //UPDATE TABLE (THE WHOLE TABLE) -  NOT DONE
        public static Registro ActualizarRegistro(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                // Update statement for LogInput
                string query = @"UPDATE LogInput 
                         SET EntryDate = @assignedDate, 
                             From_time = @assignedFromtime, 
                             To_time = @assignedTotime, 
                             Part_number = @partNumber, 
                             Id_cliente = @Id_cliente, 
                             Id_planta = @Id_planta, 
                             Id_carrier = @Id_carrier, 
                             Bill_of_Lading = @assignedBOL, 
                             Quantity = @assignedQTY, 
                             Dock = @assignedDock, 
                             shipStatus = @shipStatus, 
                             shipReason = @shipReason, 
                             shipComment = @shipComment 
                         WHERE Id_all = @Id_all";

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

                cmd.Parameters.AddWithValue("@Id_all", Reg.Id_all); // Assuming Id_all is the primary key
                cmd.ExecuteNonQuery(); // Execute the update query
            }
            return Reg;
        }
        //DELETE RECORDS FROM THE TABLE - HYDROFORM - DONE
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

        //OBTENER REGISTRO BY ID - NOT DONE
        public static Registro ObtenerById(int Id_all)
        {
            Registro Reg = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all, H.EntryDate, H.From_time,H.To_time,H.Part_number, H.Id_cliente,H.Id_planta,H.Id_carrier,H.Bill_of_Lading, H.Quantity, H.Dock, 
                            H.shipStatus,H.shipReason, H.shipComment, C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant'
                            FROM [dbo].[LogInput] H
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier
                            INNER JOIN [dbo].[Planta]  P ON H.Id_planta = P.id_planta
                            WHERE  H.Id_all=@Id_all";
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

        //RETURN LIST TO DIPSLAY IN DAILY LOG INPUT - HYDROFORM - DONE
        public static List<Registro> ListadoRegistros(int Status)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all, H.EntryDate, H.From_time, H.To_time, H.Part_number, H.Id_cliente, H.Id_planta, H.Id_carrier, H.Bill_of_Lading, H.Quantity, H.Dock, H.shipStatus,
                            H.shipReason, H.shipComment, 
                            C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant'
                    FROM [dbo].[LogInput] H
                    INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                    INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier
                    INNER JOIN [dbo].[Planta] P ON H.Id_planta = P.id_planta
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
        //                                                                 HYDROFROM - OUTPUT 

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
                // Update statement for LogOutput
                string query = @"UPDATE LogOutput 
                         SET EntryDate_output = @assignedDate, 
                             From_time_output = @assignedFromtime, 
                             To_time_output = @assignedTotime, 
                             Part_number_output = @partNumber, 
                             Id_cliente_output = @Id_cliente, 
                             Id_planta_output = @Id_planta, 
                             Id_carrier_output = @Id_carrier, 
                             Bill_of_Lading_output = @assignedBOL, 
                             Quantity_output = @assignedQTY, 
                             Dock_output = @assignedDock, 
                             shipStatus_output = @shipStatus, 
                             shipReason_output = @shipReason, 
                             shipComment_output = @shipComment 
                         WHERE Id_all_output = @Id_all";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@assignedDate", Reg.assignedDate_output);
                cmd.Parameters.AddWithValue("@assignedFromtime", Reg.assignedFromtime_output);
                cmd.Parameters.AddWithValue("@assignedTotime", Reg.assignedTotime_output);
                cmd.Parameters.AddWithValue("@partNumber", Reg.partNumber_output);
                cmd.Parameters.AddWithValue("@Id_cliente", Reg.Id_cliente_output);
                cmd.Parameters.AddWithValue("@Id_planta", Reg.Id_planta_output);
                cmd.Parameters.AddWithValue("@Id_carrier", Reg.Id_carrier_output);
                cmd.Parameters.AddWithValue("@assignedBOL", Reg.assignedBOL_output);
                cmd.Parameters.AddWithValue("@assignedQTY", Reg.assignedQTY_output);
                cmd.Parameters.AddWithValue("@assignedDock", Reg.assignedDock_output);
                cmd.Parameters.AddWithValue("@shipStatus", Reg.shipStatus_output);
                cmd.Parameters.AddWithValue("@shipReason", Reg.shipReason_output);
                cmd.Parameters.AddWithValue("@shipComment", Reg.shipComment_output);

                cmd.Parameters.AddWithValue("@Id_all", Reg.Id_all_output); // Assuming Id_all is the primary key
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
        public static Registro ObtenerById_output(int Id_all_output)
        {
            Registro Reg = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all_output, H.EntryDate_output, H.From_time_output,H.To_time_output,H.Part_number_output, H.Id_cliente_output,H.Id_planta_output,H.Id_carrier_output,H.Bill_of_Lading_output, H.Quantity_output, H.Dock_output, 
                            H.shipStatus_output,H.shipReason_output, H.shipComment_output, C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant'
                            FROM [dbo].[LogOutput] H
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente_output = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier_output = L.id_carrier
                            INNER JOIN [dbo].[Planta]  P ON H.Id_planta_output = P.id_planta
                            WHERE  H.Id_all_output=@Id_all";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_all", Id_all_output);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Reg = ConvertirRegistro_output(reader);
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

        //RETURN LIST TO DIPSLAY IN DAILY LOG OUTPUT - HYDROFROM
        public static List<Registro> ListadoRegistros_output(int Status)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all_output,H.EntryDate_output,H.From_time_output, H.To_time_output, H.Part_number_output, H.Id_cliente_output, H.Id_planta_output, H.Id_carrier_output, H.Bill_of_Lading_output, H.Quantity_output, H.Dock_output,
                    H.shipStatus_output,
                    H.shipReason_output, H.shipComment_output, 
                    C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant'
                    FROM [dbo].[LogOutput] H
                    INNER JOIN [dbo].[Cliente] C ON H.Id_cliente_output = C.id_cliente
                    INNER JOIN [dbo].[Carrier] L ON H.Id_carrier_output = L.id_carrier
                    INNER JOIN [dbo].[Planta]  P ON H.Id_planta_output  = P.id_planta
                    ORDER BY H.EntryDate_output ASC";
                //WHERE H.shipStatus=@shipStatus

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
            Reg.ClienteName_output = Convert.ToString(reader["Cliente"]);
            Reg.CarrierName_output = Convert.ToString(reader["Carrier"]);
            Reg.Id_cliente_output = Convert.ToInt32(reader["Id_cliente_output"]);
            Reg.Id_planta_output = Convert.ToInt32(reader["Id_planta_output"]);
            Reg.PlantName_output = Convert.ToString(reader["Plant"]);
            Reg.Id_carrier_output = Convert.ToInt32(reader["Id_carrier_output"]);
            Reg.assignedBOL_output = Convert.ToInt32(reader["Bill_of_Lading_output"]);
            Reg.assignedQTY_output = Convert.ToInt32(reader["Quantity_output"]);
            Reg.assignedDock_output = Convert.ToString(reader["Dock_output"]);
            Reg.shipStatus_output = Convert.ToString(reader["shipStatus_output"]);
            Reg.shipReason_output = Convert.ToString(reader["shipReason_output"]);
            Reg.shipComment_output = Convert.ToString(reader["shipComment_output"]);
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
        //SHIPMENTS - INPUT - DOCK
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
        //SHIPMENT - OUTPUTS - DOCK

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
        //DASHBOARD ALL - HYDROFORM
        public static List<Registro> ListDashboard(int Status)
        {
            List<Registro> list_dashboard = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"
        SELECT 'LogInput' AS SourceTable, H.Id_all, H.EntryDate AS EntryDate,H.From_time,H.To_time,H.Part_number,H.Id_cliente,H.Id_planta,H.Id_carrier,H.Bill_of_Lading,H.Quantity,H.Dock,H.shipStatus,
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
        ORDER BY 
            EntryDate ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Registro Reg = new Registro
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
                    list_dashboard.Add(Reg);
                }
            }
            return list_dashboard;
        }

        public static List<Registro> ListDashboard_COATINGS(int Status)
{
            List<Registro> list_dashboard_coatings = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"
        SELECT 'LogInput_coatings' AS SourceTable, H.Id_all_coatings, H.EntryDate_coatings AS EntryDate,H.From_time_coatings,H.To_time_coatings,H.Part_number_coatings,H.Id_cliente_coatings,H.Id_planta_coatings,H.Id_carrier_coatings,H.Bill_of_Lading_coatings,H.Quantity_coatings,H.Dock_coatings,H.shipStatus_coatings,
                       H.shipReason_coatings,H.shipComment_coatings,
                       C.description AS Cliente,
                       L.description AS Carrier,
                       P.description AS Plant
        FROM 
            [dbo].[LogInput_coatings] H
        INNER JOIN 
            [dbo].[Cliente] C ON H.Id_cliente_coatings = C.id_cliente
        INNER JOIN 
            [dbo].[Carrier] L ON H.Id_carrier_coatings = L.id_carrier
        INNER JOIN 
            [dbo].[Planta] P ON H.Id_planta_coatings = P.id_planta

        UNION

        SELECT 
            'LogOutput_coatings' AS SourceTable,
            H.Id_all_output_coatings,
            H.EntryDate_output_coatings AS EntryDate_coatings,
            H.From_time_output_coatings AS From_time_coatings,
            H.To_time_output_coatings AS To_time_coatings,
            H.Part_number_output_coatings AS Part_number_coatings,
            H.Id_cliente_output_coatings AS Id_cliente_coatings,
            H.Id_planta_output_coatings AS Id_planta_coatings,
            H.Id_carrier_output_coatings AS Id_carrier_coatings,
            H.Bill_of_Lading_output_coatings AS Bill_of_Lading_coatings,
            H.Quantity_output_coatings AS Quantity_coatings,
            H.Dock_output_coatings AS Dock_coatings,
            H.shipStatus_output_coatings AS shipStatus_coatings,
            H.shipReason_output_coatings AS shipReason_coatings,
            H.shipComment_output_coatings AS shipComment_coatings,
            C.description AS Cliente,
            L.description AS Carrier,
            P.description AS Plant
        FROM 
            [dbo].[LogOutput_coatings] H
        INNER JOIN 
            [dbo].[Cliente] C ON H.Id_cliente_output_coatings = C.id_cliente
        INNER JOIN 
            [dbo].[Carrier] L ON H.Id_carrier_output_coatings = L.id_carrier
        INNER JOIN 
            [dbo].[Planta] P ON H.Id_planta_output_coatings = P.id_planta
        ORDER BY 
            EntryDate ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Registro Reg = new Registro
                    {
                        Id_all_coatings = Convert.ToInt32(reader["Id_all_coatings"]),
                        assignedDate_coatings = Convert.ToDateTime(reader["EntryDate"]),
                        assignedFromtime_coatings = Convert.ToString(reader["From_time_coatings"]),
                        assignedTotime_coatings = Convert.ToString(reader["To_time_coatings"]),
                        partNumber_coatings = Convert.ToString(reader["Part_number_coatings"]),
                        Id_cliente_coatings = Convert.ToInt32(reader["Id_cliente_coatings"]),
                        Id_planta_coatings = Convert.ToInt32(reader["Id_planta_coatings"]),
                        Id_carrier_coatings = Convert.ToInt32(reader["Id_carrier_coatings"]),
                        PlantName_coatings = Convert.ToString(reader["Plant"]),
                        assignedBOL_coatings = Convert.ToInt32(reader["Bill_of_Lading_coatings"]),
                        assignedQTY_coatings = Convert.ToInt32(reader["Quantity_coatings"]),
                        assignedDock_coatings = Convert.ToString(reader["Dock_coatings"]),
                        shipStatus_coatings = Convert.ToString(reader["shipStatus_coatings"]),
                        shipReason_coatings = Convert.ToString(reader["shipReason_coatings"]),
                        shipComment_coatings = Convert.ToString(reader["shipComment_coatings"]),
                        ClienteName_coatings = Convert.ToString(reader["Cliente"]),
                        CarrierName_coatings = Convert.ToString(reader["Carrier"]),
                        IsInput_coatings = String.Equals(Convert.ToString(reader["SourceTable"]), "LogInput_coatings", StringComparison.OrdinalIgnoreCase)
                    };
                    list_dashboard_coatings.Add(Reg);
                }
            }
            return list_dashboard_coatings;
        }



        //                                                                     LIST DASHBOARD BUT WITH DOCKS SEPARATED - HYDROFORM

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

        public static List<Registro> ListDashboard_Dock_COATINGS(string dockName)
        {
            List<Registro> list_dashboard_dock_coatings = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"
        SELECT 'LogInput_coatings' AS SourceTable, H.Id_all_coatings, H.EntryDate_coatings AS EntryDate,H.From_time_coatings,H.To_time_coatings,H.Part_number_coatings,H.Id_cliente_coatings,H.Id_planta_coatings,H.Id_carrier_coatings,H.Bill_of_Lading_coatings,H.Quantity_coatings,H.Dock_coatings,H.shipStatus_coatings,
                       H.shipReason_coatings,H.shipComment_coatings,
                       C.description AS Cliente,
                       L.description AS Carrier,
                       P.description AS Plant

        FROM 
            [dbo].[LogInput_coatings] H
        INNER JOIN 
            [dbo].[Cliente] C ON H.Id_cliente_coatings = C.id_cliente
        INNER JOIN 
            [dbo].[Carrier] L ON H.Id_carrier_coatings = L.id_carrier
        INNER JOIN 
            [dbo].[Planta] P ON H.Id_planta_coatings = P.id_planta
        WHERE H.Dock_coatings = @dockName

        UNION

        SELECT 
            'LogOutput_coatings' AS SourceTable,
            H.Id_all_output_coatings,
            H.EntryDate_output_coatings AS EntryDate_coatings,
            H.From_time_output_coatings AS From_time_coatings,
            H.To_time_output_coatings AS To_time_coatings,
            H.Part_number_output_coatings AS Part_number_coatings,
            H.Id_cliente_output_coatings AS Id_cliente_coatings,
            H.Id_planta_output_coatings AS Id_planta_coatings,
            H.Id_carrier_output_coatings AS Id_carrier_coatings,
            H.Bill_of_Lading_output_coatings AS Bill_of_Lading_coatings,
            H.Quantity_output_coatings AS Quantity_coatings,
            H.Dock_output_coatings AS Dock_coatings,
            H.shipStatus_output_coatings AS shipStatus_coatings,
            H.shipReason_output_coatings AS shipReason_coatings,
            H.shipComment_output_coatings AS shipComment_coatings,
            C.description AS Cliente,
            L.description AS Carrier,
            P.description AS Plant

        FROM 
            [dbo].[LogOutput_coatings] H
        INNER JOIN 
            [dbo].[Cliente] C ON H.Id_cliente_output_coatings = C.id_cliente
        INNER JOIN 
            [dbo].[Carrier] L ON H.Id_carrier_output_coatings = L.id_carrier
        INNER JOIN 
            [dbo].[Planta] P ON H.Id_planta_output_coatings = P.id_planta

        WHERE H.Dock_output_coatings = @dockName

        ORDER BY 
            EntryDate ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dockName", dockName);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Registro Reg = new Registro
                    {
                        Id_all_coatings = Convert.ToInt32(reader["Id_all_coatings"]),
                        assignedDate_coatings = Convert.ToDateTime(reader["EntryDate"]),
                        assignedFromtime_coatings = Convert.ToString(reader["From_time_coatings"]),
                        assignedTotime_coatings = Convert.ToString(reader["To_time_coatings"]),
                        partNumber_coatings = Convert.ToString(reader["Part_number_coatings"]),
                        Id_cliente_coatings = Convert.ToInt32(reader["Id_cliente_coatings"]),
                        Id_planta_coatings = Convert.ToInt32(reader["Id_planta_coatings"]),
                        Id_carrier_coatings = Convert.ToInt32(reader["Id_carrier_coatings"]),
                        PlantName_coatings = Convert.ToString(reader["Plant"]),
                        assignedBOL_coatings = Convert.ToInt32(reader["Bill_of_Lading_coatings"]),
                        assignedQTY_coatings = Convert.ToInt32(reader["Quantity_coatings"]),
                        assignedDock_coatings = Convert.ToString(reader["Dock_coatings"]),
                        shipStatus_coatings = Convert.ToString(reader["shipStatus_coatings"]),
                        shipReason_coatings = Convert.ToString(reader["shipReason_coatings"]),
                        shipComment_coatings = Convert.ToString(reader["shipComment_coatings"]),
                        ClienteName_coatings = Convert.ToString(reader["Cliente"]),
                        CarrierName_coatings = Convert.ToString(reader["Carrier"]),
                        IsInput_coatings = String.Equals(Convert.ToString(reader["SourceTable"]), "LogInput_coatings", StringComparison.OrdinalIgnoreCase)
                    };
                    list_dashboard_dock_coatings.Add(Reg);
                }
            }
            return list_dashboard_dock_coatings;
        }

        public static List<Registro> control_panel_dock(string dockName)
        {
            List<Registro> list_control_panel_dock = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"WITH CombinedLogs AS (
                SELECT 
                    'LogInput' AS SourceTable,
                    Id_all AS Id_all,
                    EntryDate,
                    From_time,
                    To_time,
                    Part_number,
                    shipStatus
                FROM 
                    [dbo].[LogInput] H
                WHERE 
                    H.Dock = @dockName
    
                UNION ALL
    
                SELECT 
                    'LogOutput' AS SourceTable,
                    Id_all_output AS Id_all,
                    EntryDate_output AS EntryDate,
                    From_time_output AS From_time,
                    To_time_output AS To_time,
                    Part_number_output AS Part_number,
                    shipStatus_output AS shipStatus
                FROM 
                    [dbo].[LogOutput] H
                WHERE 
                    H.Dock_output = @dockName
            )

            SELECT TOP 5
                SourceTable,
                Id_all,
                EntryDate,
                From_time,
                To_time,
                Part_number,
                shipStatus
            FROM 
                CombinedLogs
            ORDER BY 
                From_time ASC";

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
                        shipStatus = Convert.ToString(reader["shipStatus"]),
                    };
                    list_control_panel_dock.Add(Reg);
                }
            }
            return list_control_panel_dock;
        }

        public static List<Registro> control_panel_dock_coatings(string dockName)
        {
            List<Registro> list_control_panel_dock_coatings = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"WITH CombinedLogs AS (
                SELECT 
                    'LogInput_coatings' AS SourceTable,
                    Id_all_coatings AS Id_all_coatings,
                    EntryDate_coatings,
                    From_time_coatings,
                    To_time_coatings,
                    Part_number_coatings,
                    shipStatus_coatings
                FROM 
                    [dbo].[LogInput_coatings] H
                WHERE 
                    H.Dock_coatings = @dockName
    
                UNION ALL
    
                SELECT 
                    'LogOutput_coatings' AS SourceTable,
                    Id_all_output_coatings AS Id_all_coatings,
                    EntryDate_output_coatings AS EntryDate_coatings,
                    From_time_output_coatings AS From_time_coatings,
                    To_time_output_coatings AS To_time_coatings,
                    Part_number_output_coatings AS Part_number_coatings,
                    shipStatus_output_coatings AS shipStatus_coatings
                FROM 
                    [dbo].[LogOutput_coatings] H
                WHERE 
                    H.Dock_output_coatings = @dockName
            )

            SELECT TOP 5
                SourceTable,
                Id_all_coatings,
                EntryDate_coatings,
                From_time_coatings,
                To_time_coatings,
                Part_number_coatings,
                shipStatus_coatings
            FROM 
                CombinedLogs
            ORDER BY 
                From_time_coatings ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dockName", dockName);
                //cmd.Parameters.AddWithValue("@Id_planta", Id_Planta);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Registro Reg = new Registro()
                    {
                        Id_all = Convert.ToInt32(reader["Id_all_coatings"]),
                        assignedDate = Convert.ToDateTime(reader["EntryDate_coatings"]),
                        assignedFromtime = Convert.ToString(reader["From_time_coatings"]),
                        assignedTotime = Convert.ToString(reader["To_time_coatings"]),
                        partNumber = Convert.ToString(reader["Part_number_coatings"]),
                        shipStatus = Convert.ToString(reader["shipStatus_coatings"]),
                    };
                    list_control_panel_dock_coatings.Add(Reg);
                }
            }
            return list_control_panel_dock_coatings;
        }



        //                                                                    COATINGS - REGISTER AND LIST INPUTS AND OUTPUTS

        public static Registro AgregarNuevo_coatings(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                //OBTENER EL ID UNICO DEL ISSUE
                int ID;
                string query = @"INSERT INTO LogInput_coatings (EntryDate_coatings,From_time_coatings,To_time_coatings,Part_number_coatings,Id_cliente_coatings,Id_planta_coatings,Id_carrier_coatings,Bill_of_Lading_coatings,Quantity_coatings,Dock_coatings,shipStatus_coatings, shipReason_coatings, shipComment_coatings)
                                 VALUES (@assignedDate_coatings, @assignedFromtime_coatings,@assignedTotime_coatings,@partNumber_coatings,@Id_cliente_coatings,@Id_planta_coatings,@Id_carrier_coatings,@assignedBOL_coatings,@assignedQTY_coatings,@assignedDock_coatings,@shipStatus_coatings,@shipReason_coatings,@shipComment_coatings); SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@assignedDate_coatings", Reg.assignedDate_coatings);
                cmd.Parameters.AddWithValue("@assignedFromtime_coatings", Reg.assignedFromtime_coatings);
                cmd.Parameters.AddWithValue("@assignedTotime_coatings", Reg.assignedTotime_coatings);
                cmd.Parameters.AddWithValue("@partNumber_coatings", Reg.partNumber_coatings);
                cmd.Parameters.AddWithValue("@Id_cliente_coatings", Reg.Id_cliente_coatings);
                cmd.Parameters.AddWithValue("@Id_planta_coatings", Reg.Id_planta_coatings);
                cmd.Parameters.AddWithValue("@Id_carrier_coatings", Reg.Id_carrier_coatings);
                cmd.Parameters.AddWithValue("@assignedBOL_coatings", Reg.assignedBOL_coatings);
                cmd.Parameters.AddWithValue("@assignedQTY_coatings", Reg.assignedQTY_coatings);
                cmd.Parameters.AddWithValue("@assignedDock_coatings", Reg.assignedDock_coatings);
                cmd.Parameters.AddWithValue("@shipStatus_coatings", Reg.shipStatus_coatings);
                cmd.Parameters.AddWithValue("@shipReason_coatings", Reg.shipReason_coatings);
                cmd.Parameters.AddWithValue("@shipComment_coatings", Reg.shipComment_coatings);

                Reg.Id_all_coatings = Convert.ToInt32(cmd.ExecuteScalar());
                ID = Reg.Id_all_coatings;
            }
            return Reg;
        }
        //UPDATE TABLE (THE WHOLE TABLE) -  NOT DONE - COATINGS
        public static Registro ActualizarRegistro_coatings(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                // Update statement for LogInput
                string query = @"UPDATE LogInput_coatings 
                         SET EntryDate_coatings = @EntryDate_coatings, 
                             From_time_coatings = @From_time_coatings, 
                             To_time_coatings = @To_time_coatings, 
                             Part_number_coatings = @Part_number_coatings, 
                             Id_cliente_coatings = @Id_cliente_coatings, 
                             Id_planta_coatings = @Id_planta_coatings, 
                             Id_carrier_coatings = @Id_carrier_coatings, 
                             Bill_of_Lading_coatings = @Bill_of_Lading_coatings, 
                             Quantity_coatings = @Quantity_coatings, 
                             Dock_coatings = @Dock_coatings, 
                             shipStatus_coatings = @shipStatus_coatings, 
                             shipReason_coatings = @shipReason_coatings, 
                             shipComment_coatings = @shipComment_coatings 
                         WHERE Id_all_coatings = @Id_all_coatings";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EntryDate_coatings", Reg.assignedDate_coatings);
                cmd.Parameters.AddWithValue("@From_time_coatings", Reg.assignedFromtime_coatings);
                cmd.Parameters.AddWithValue("@To_time_coatings", Reg.assignedTotime_coatings);
                cmd.Parameters.AddWithValue("@Part_number_coatings", Reg.partNumber_coatings);
                cmd.Parameters.AddWithValue("@Id_cliente_coatings", Reg.Id_cliente_coatings);
                cmd.Parameters.AddWithValue("@Id_planta_coatings", Reg.Id_planta_coatings);
                cmd.Parameters.AddWithValue("@Id_carrier_coatings", Reg.Id_carrier_coatings);
                cmd.Parameters.AddWithValue("@Bill_of_Lading_coatings", Reg.assignedBOL_coatings);
                cmd.Parameters.AddWithValue("@Quantity_coatings", Reg.assignedQTY_coatings);
                cmd.Parameters.AddWithValue("@Dock_coatings", Reg.assignedDock_coatings);
                cmd.Parameters.AddWithValue("@shipStatus_coatings", Reg.shipStatus_coatings);
                cmd.Parameters.AddWithValue("@shipReason_coatings", Reg.shipReason_coatings);
                cmd.Parameters.AddWithValue("@shipComment_coatings", Reg.shipComment_coatings);
                cmd.Parameters.AddWithValue("@Id_all_coatings", Reg.Id_all_coatings); // Assuming Id_all is the primary key
                cmd.ExecuteNonQuery(); // Execute the update query
            }
            return Reg;
        }
        //DELETE RECORDS FROM THE TABLE - COATINGS - DONE
        public static void EliminarRegistro_coatings(int Id_all_coatings)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //Eliminar registros de categorias
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
                {
                    conn.Open();
                    string query = @"DELETE FROM LogInput_coatings WHERE Id_all_coatings = @Id_all_coatings";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id_all_coatings", Id_all_coatings);
                    cmd.ExecuteNonQuery();
                }
                scope.Complete();
            }
        }

        //OBTENER REGISTRO BY ID - NOT DONE
        public static Registro ObtenerById_coatings(int Id_all_coatings)
        {
            Registro Reg = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all_coatings, H.EntryDate_coatings, H.From_time_coatings,H.To_time_coatings,H.Part_number_coatings, H.Id_cliente_coatings,H.Id_planta_coatings,H.Id_carrier_coatings,H.Bill_of_Lading_coatings, H.Quantity_coatings, H.Dock_coatings, 
                            H.shipStatus_coatings,H.shipReason_coatings, H.shipComment_coatings, C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant'
                            FROM [dbo].[LogInput_coatings] H
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente_coatings = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier_coatings = L.id_carrier
                            INNER JOIN [dbo].[Planta]  P ON H.Id_planta_coatings = P.id_planta
                            WHERE  H.Id_all_coatings=@Id_all";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_all", Id_all_coatings);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Reg = ConvertirRegistro_coatings(reader);
                }
            }
            return Reg;
        }

        //RETURN LIST TO DIPSLAY IN DAILY LOG INPUT - COATINGS - DONE
        public static List<Registro> ListadoRegistros_coatings(int Status)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all_coatings, H.EntryDate_coatings, H.From_time_coatings, H.To_time_coatings, H.Part_number_coatings, H.Id_cliente_coatings, H.Id_planta_coatings, H.Id_carrier_coatings, H.Bill_of_Lading_coatings, H.Quantity_coatings, H.Dock_coatings, H.shipStatus_coatings,
                            H.shipReason_coatings, H.shipComment_coatings, 
                            C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant'
                    FROM [dbo].[LogInput_coatings] H
                    INNER JOIN [dbo].[Cliente] C ON H.Id_cliente_coatings = C.id_cliente
                    INNER JOIN [dbo].[Carrier] L ON H.Id_carrier_coatings = L.id_carrier
                    INNER JOIN [dbo].[Planta] P ON H.Id_planta_coatings = P.id_planta
                    ORDER BY H.EntryDate_coatings ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirRegistro_coatings(reader));
                }
            }
            return lista;
        }

        // CONVERTIR REGISTRO - DONE

        private static Registro ConvertirRegistro_coatings(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all_coatings = Convert.ToInt32(reader["Id_all_coatings"]);
            Reg.assignedDate_coatings = Convert.ToDateTime(reader["EntryDate_coatings"]);
            Reg.assignedFromtime_coatings = Convert.ToString(reader["From_time_coatings"]);
            Reg.assignedTotime_coatings = Convert.ToString(reader["To_time_coatings"]);
            Reg.partNumber_coatings = Convert.ToString(reader["Part_number_coatings"]);
            Reg.ClienteName_coatings = Convert.ToString(reader["Cliente"]);
            Reg.CarrierName_coatings = Convert.ToString(reader["Carrier"]);
            Reg.Id_cliente_coatings = Convert.ToInt32(reader["Id_cliente_coatings"]);
            Reg.Id_planta_coatings = Convert.ToInt32(reader["Id_planta_coatings"]);
            Reg.PlantName_coatings = Convert.ToString(reader["Plant"]);
            Reg.Id_carrier_coatings = Convert.ToInt32(reader["Id_carrier_coatings"]);
            Reg.assignedBOL_coatings = Convert.ToInt32(reader["Bill_of_Lading_coatings"]);
            Reg.assignedQTY_coatings = Convert.ToInt32(reader["Quantity_coatings"]);
            Reg.assignedDock_coatings = Convert.ToString(reader["Dock_coatings"]);
            Reg.shipStatus_coatings = Convert.ToString(reader["shipStatus_coatings"]);
            Reg.shipReason_coatings = Convert.ToString(reader["shipReason_coatings"]);
            Reg.shipComment_coatings = Convert.ToString(reader["shipComment_coatings"]);
            return Reg;
        }

        private static Registro ConvertirDash_coatings(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all_coatings = Convert.ToInt32(reader["Id_all"]);
            Reg.assignedDate_coatings = Convert.ToDateTime(reader["EntryDate_coatings"]);
            Reg.assignedFromtime_coatings = Convert.ToString(reader["From_time_coatings"]);
            Reg.assignedTotime_coatings = Convert.ToString(reader["To_time_coatings"]);
            Reg.partNumber_coatings = Convert.ToString(reader["Part_number_coatings"]);
            Reg.Id_cliente_coatings = Convert.ToInt32(reader["Id_cliente_coatings"]);
            Reg.Id_planta_coatings = Convert.ToInt32(reader["Id_planta_coatings"]);
            Reg.PlantName_coatings = Convert.ToString(reader["Plant_coatings"]);
            Reg.Id_carrier_coatings = Convert.ToInt32(reader["Id_carrier_coatings"]);
            Reg.assignedBOL_coatings = Convert.ToInt32(reader["Bill_of_Lading_coatings"]);
            Reg.assignedQTY_coatings = Convert.ToInt32(reader["Quantity_coatings"]);
            Reg.assignedDock_coatings = Convert.ToString(reader["Dock_coatings"]);
            Reg.shipStatus_coatings = Convert.ToString(reader["shipStatus_coatings"]);
            Reg.shipReason_coatings = Convert.ToString(reader["shipReason_coatings"]);
            Reg.shipComment_coatings = Convert.ToString(reader["shipComment_coatings"]);
            Reg.ClienteName_coatings = Convert.ToString(reader["Cliente_coatings"]);
            Reg.CarrierName_coatings = Convert.ToString(reader["Carrier_coatings"]);
            return Reg;
        }

        public static Registro ObtenerRegistros_coatings(int Id_planta)
        {
            Registro list = null;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                con.Open();
                string query = @"
                        SELECT DISTINCT  
                        (SELECT ISNULL((Count(H.shipStatus_coatings)),0) FROM LogInput_coatings H  WHERE H.shipStatus_coatings = 3 = CONVERT(DATE,GETDATE()) AND H.Id_planta_coatings=@Id_planta_coatings) AS 'SHIPPED',

                        (SELECT ISNULL((Count(H.shipStatus_coatings)),0) FROM LogInput_coatings H  WHERE H.shipStatus_coatings = 1 = CONVERT(DATE,GETDATE()) AND H.Id_planta_coatings=@Id_planta_coatings) AS 'EARRING',

                        (SELECT ISNULL((Count(H.shipStatus_coatings)),0) FROM LogInput_coatings H  

                        WHERE shipStatus_coatings = 2 = CONVERT(DATE,GETDATE()) AND H.Id_planta_coatings=@Id_planta)AS 'ONTIME' ,

                        (SELECT ISNULL((Count(H.shipStatus)),0) AS 'DELAYED' FROM LogInput H 
                        WHERE H.shipStatus_coatings = 2  = CONVERT(DATE,GETDATE()) AND H.Id_planta_coatings=@Id_planta) AS 'DELAYED' 
                        FROM LogInput_coatings ";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id_planta", Id_planta);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    list = Convertir_coatings(reader);
                }
            }
            return list;
        }

        //DONT KNOW WHAT THIS IS FOR - NOT DONE

        private static Registro Convertir_coatings(IDataReader reader)
        {
            Registro list = new Registro();
            list.Completed = Convert.ToString(reader["SHIPPED"]);
            list.Pendiente = Convert.ToString(reader["EARRING"]);
            list.Ontime = Convert.ToString(reader["ONTIME"]);
            list.DELAYED = Convert.ToString(reader["DELAYED"]);
            return list;
        }

        //                                                                                                    COATINGS OUTPUT

        public static Registro AgregarNuevo_output_coatings(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                //OBTENER EL ID UNICO DEL ISSUE
                int ID;

                string query = @"INSERT INTO LogOutput_coatings (EntryDate_output_coatings,From_time_output_coatings,To_time_output_coatings,Part_number_output_coatings,Id_cliente_output_coatings,Id_planta_output_coatings,Id_carrier_output_coatings,Bill_of_Lading_output_coatings,Quantity_output_coatings,Dock_output_coatings,shipStatus_output_coatings, shipReason_output_coatings, shipComment_output_coatings)
                                 VALUES (@assignedDate_output_coatings, @assignedFromtime_output_coatings,@assignedTotime_output_coatings,@partNumber_output_coatings,@Id_cliente_output_coatings,@Id_planta_output_coatings,@Id_carrier_output_coatings,@assignedBOL_output_coatings,@assignedQTY_output_coatings,@assignedDock_output_coatings,@shipStatus_output_coatings,@shipReason_output_coatings,@shipComment_output_coatings); SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@assignedDate_output_coatings", Reg.assignedDate_output_coatings);
                cmd.Parameters.AddWithValue("@assignedFromtime_output_coatings", Reg.assignedFromtime_output_coatings);
                cmd.Parameters.AddWithValue("@assignedTotime_output_coatings", Reg.assignedTotime_output_coatings);
                cmd.Parameters.AddWithValue("@partNumber_output_coatings", Reg.partNumber_output_coatings);
                cmd.Parameters.AddWithValue("@Id_cliente_output_coatings", Reg.Id_cliente_output_coatings);
                cmd.Parameters.AddWithValue("@Id_planta_output_coatings", Reg.Id_planta_output_coatings);
                cmd.Parameters.AddWithValue("@Id_carrier_output_coatings", Reg.Id_carrier_output_coatings);
                cmd.Parameters.AddWithValue("@assignedBOL_output_coatings", Reg.assignedBOL_output_coatings);
                cmd.Parameters.AddWithValue("@assignedQTY_output_coatings", Reg.assignedQTY_output_coatings);
                cmd.Parameters.AddWithValue("@assignedDock_output_coatings", Reg.assignedDock_output_coatings);
                cmd.Parameters.AddWithValue("@shipStatus_output_coatings", Reg.shipStatus_output_coatings);
                cmd.Parameters.AddWithValue("@shipReason_output_coatings", Reg.shipReason_output_coatings);
                cmd.Parameters.AddWithValue("@shipComment_output_coatings", Reg.shipComment_output_coatings);

                //RECUPERAR ID GENERADO POR LA TAB
                Reg.Id_all_output_coatings = Convert.ToInt32(cmd.ExecuteScalar());
                ID = Reg.Id_all_output_coatings;
            }
            return Reg;
        }

        //UPDATE TABLE - DONE
        public static Registro ActualizarRegistro_output_coatings(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                // Update statement for LogInput
                string query = @"UPDATE LogOutput_coatings 
                         SET EntryDate_output_coatings = @EntryDate_output_coatings, 
                             From_time_output_coatings = @From_time_output_coatings, 
                             To_time_output_coatings = @To_time_output_coatings, 
                             Part_number_output_coatings = @Part_number_output_coatings, 
                             Id_cliente_output_coatings = @Id_cliente_output_coatings, 
                             Id_planta_output_coatings = @Id_planta_output_coatings, 
                             Id_carrier_output_coatings = @Id_carrier_output_coatings, 
                             Bill_of_Lading_output_coatings = @Bill_of_Lading_output_coatings, 
                             Quantity_output_coatings = @Quantity_output_coatings, 
                             Dock_output_coatings = @Dock_output_coatings, 
                             shipStatus_output_coatings = @shipStatus_output_coatings, 
                             shipReason_output_coatings = @shipReason_output_coatings, 
                             shipComment_output_coatings = @shipComment_output_coatings 
                         WHERE Id_all_output_coatings = @Id_all_output_coatings";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@EntryDate_output_coatings", Reg.assignedDate_output_coatings);
                cmd.Parameters.AddWithValue("@From_time_output_coatings", Reg.assignedFromtime_output_coatings);
                cmd.Parameters.AddWithValue("@To_time_output_coatings", Reg.assignedTotime_output_coatings);
                cmd.Parameters.AddWithValue("@Part_number_output_coatings", Reg.partNumber_output_coatings);
                cmd.Parameters.AddWithValue("@Id_cliente_output_coatings", Reg.Id_cliente_output_coatings);
                cmd.Parameters.AddWithValue("@Id_planta_output_coatings", Reg.Id_planta_output_coatings);
                cmd.Parameters.AddWithValue("@Id_carrier_output_coatings", Reg.Id_carrier_output_coatings);
                cmd.Parameters.AddWithValue("@Bill_of_Lading_output_coatings", Reg.assignedBOL_output_coatings);
                cmd.Parameters.AddWithValue("@Quantity_output_coatings", Reg.assignedQTY_output_coatings);
                cmd.Parameters.AddWithValue("@Dock_output_coatings", Reg.assignedDock_output_coatings);
                cmd.Parameters.AddWithValue("@shipStatus_output_coatings", Reg.shipStatus_output_coatings);
                cmd.Parameters.AddWithValue("@shipReason_output_coatings", Reg.shipReason_output_coatings);
                cmd.Parameters.AddWithValue("@shipComment_output_coatings", Reg.shipComment_output_coatings);
                cmd.Parameters.AddWithValue("@Id_all_output_coatings", Reg.Id_all_output_coatings); // Assuming Id_all is the primary key
                cmd.ExecuteNonQuery(); // Execute the update query
            }
            return Reg;
        }

        //DELETE RECORDS FROM THE TABLE - DONE
        public static void EliminarRegistro_output_coatings(int Id_all_output_coatings)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //Eliminar registros de categorias
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
                {
                    conn.Open();
                    string query = @"DELETE FROM LogOutput_coatings WHERE Id_all_output_coatings = @Id_all_output_coatings";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id_all_output_coatings", Id_all_output_coatings);
                    cmd.ExecuteNonQuery();
                }
                scope.Complete();
            }
        }

        //OBTENER REGISTRO BY ID - DONE
        public static Registro ObtenerById_output_coatings(int Id_all_output_coatings)
        {
            Registro Reg = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all_output_coatings, H.EntryDate_output_coatings, H.From_time_output_coatings,H.To_time_output_coatings,H.Part_number_output_coatings, H.Id_cliente_output_coatings,H.Id_planta_output_coatings,H.Id_carrier_output_coatings,H.Bill_of_Lading_output_coatings, H.Quantity_output_coatings, H.Dock_output_coatings, 
                            H.shipStatus_output_coatings,H.shipReason_output_coatings, H.shipComment_output_coatings, C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant'
                            FROM [dbo].[LogOutput_coatings] H
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente_output_coatings = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier_output_coatings = L.id_carrier
                            INNER JOIN [dbo].[Planta]  P ON H.Id_planta_output_coatings = P.id_planta
                            WHERE  H.Id_all_output_coatings=@Id_all";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_all", Id_all_output_coatings);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Reg = ConvertirRegistro_output_coatings(reader);
                }
            }
            return Reg;
        }

        public static List<Registro> ListadoRegistros_output_coatings(int Status)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all_output_coatings, H.EntryDate_output_coatings, H.From_time_output_coatings, H.To_time_output_coatings, H.Part_number_output_coatings, H.Id_cliente_output_coatings, H.Id_planta_output_coatings, H.Id_carrier_output_coatings, H.Bill_of_Lading_output_coatings, H.Quantity_output_coatings, H.Dock_output_coatings, H.shipStatus_output_coatings,
                            H.shipReason_output_coatings, H.shipComment_output_coatings, 
                            C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant'
                    FROM [dbo].[LogOutput_coatings] H
                    INNER JOIN [dbo].[Cliente] C ON H.Id_cliente_output_coatings = C.id_cliente
                    INNER JOIN [dbo].[Carrier] L ON H.Id_carrier_output_coatings = L.id_carrier
                    INNER JOIN [dbo].[Planta] P ON H.Id_planta_output_coatings = P.id_planta
                    ORDER BY H.EntryDate_output_coatings ASC";

                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirRegistro_output_coatings(reader));
                }
            }
            return lista;
        }

        // CONVERTIR REGISTRO - DONE

        private static Registro ConvertirRegistro_output_coatings(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all_output_coatings = Convert.ToInt32(reader["Id_all_output_coatings"]);
            Reg.assignedDate_output_coatings = Convert.ToDateTime(reader["EntryDate_output_coatings"]);
            Reg.assignedFromtime_output_coatings = Convert.ToString(reader["From_time_output_coatings"]);
            Reg.assignedTotime_output_coatings = Convert.ToString(reader["To_time_output_coatings"]);
            Reg.partNumber_output_coatings = Convert.ToString(reader["Part_number_output_coatings"]);
            Reg.ClienteName_output_coatings = Convert.ToString(reader["Cliente"]);
            Reg.CarrierName_output_coatings = Convert.ToString(reader["Carrier"]);
            Reg.Id_cliente_output_coatings = Convert.ToInt32(reader["Id_cliente_output_coatings"]);
            Reg.Id_planta_output_coatings = Convert.ToInt32(reader["Id_planta_output_coatings"]);
            Reg.PlantName_output_coatings = Convert.ToString(reader["Plant"]);
            Reg.Id_carrier_output_coatings = Convert.ToInt32(reader["Id_carrier_output_coatings"]);
            Reg.assignedBOL_output_coatings = Convert.ToInt32(reader["Bill_of_Lading_output_coatings"]);
            Reg.assignedQTY_output_coatings = Convert.ToInt32(reader["Quantity_output_coatings"]);
            Reg.assignedDock_output_coatings = Convert.ToString(reader["Dock_output_coatings"]);
            Reg.shipStatus_output_coatings = Convert.ToString(reader["shipStatus_output_coatings"]);
            Reg.shipReason_output_coatings = Convert.ToString(reader["shipReason_output_coatings"]);
            Reg.shipComment_output_coatings = Convert.ToString(reader["shipComment_output_coatings"]);

            return Reg;
        }
        private static Registro ConvertirDash_output_coatings(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all_output_coatings = Convert.ToInt32(reader["Id_all"]);
            Reg.assignedDate_output_coatings = Convert.ToDateTime(reader["EntryDate_output_coatings"]);
            Reg.assignedFromtime_output_coatings = Convert.ToString(reader["From_time_output_coatings"]);
            Reg.assignedTotime_output_coatings = Convert.ToString(reader["To_time_output_coatings"]);
            Reg.partNumber_output_coatings = Convert.ToString(reader["Part_number_output_coatings"]);
            Reg.Id_cliente_output_coatings = Convert.ToInt32(reader["Id_cliente_output_coatings"]);
            Reg.Id_planta_output_coatings = Convert.ToInt32(reader["Id_planta_output_coatings"]);
            Reg.PlantName_output_coatings = Convert.ToString(reader["Plant_output_coatings"]);
            Reg.Id_carrier_output_coatings = Convert.ToInt32(reader["Id_carrier_output_coatings"]);
            Reg.assignedBOL_output_coatings = Convert.ToInt32(reader["Bill_of_Lading_output_coatings"]);
            Reg.assignedQTY_output_coatings = Convert.ToInt32(reader["Quantity_output_coatings"]);
            Reg.assignedDock_output_coatings = Convert.ToString(reader["Dock_output_coatings"]);
            Reg.shipStatus_output_coatings = Convert.ToString(reader["shipStatus_output_coatings"]);
            Reg.shipReason_output_coatings = Convert.ToString(reader["shipReason_output_coatings"]);
            Reg.shipComment_output_coatings = Convert.ToString(reader["shipComment_output_coatings"]);
            Reg.ClienteName_output_coatings = Convert.ToString(reader["Cliente_output_coatings"]);
            Reg.CarrierName_output_coatings = Convert.ToString(reader["Carrier_output_coatings"]);
            return Reg;
        }


        // DASHBOARD - DOCK COATINGS
        public static List<Registro> ListDashboard_Dock_coatings(string dockName)
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


        //                                                                    COATINGS - DOCK QUERY (EACH DOCK INS AND OUTS)


        // DOCK FOR INPUT AND OUTPUT - COATINGS
        public static List<Registro> dockQueryInput_coatings(string dockName)
        {
            List<Registro> regList = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all_coatings,H.EntryDate_coatings,H.From_time_coatings, H.To_time_coatings, H.Part_number_coatings, H.Id_cliente_coatings, H.Id_planta_coatings, H.Id_carrier_coatings, H.Bill_of_Lading_coatings, H.Quantity_coatings, H.Dock_coatings,H.shipStatus_coatings, H.shipReason_coatings, H.shipComment_coatings, 
                            C.description AS 'Cliente', L.description AS 'Carrier', P.description AS 'Plant'
                            FROM [dbo].[LogInput_coatings] H
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente_coatings = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier_coatings = L.id_carrier
                            INNER JOIN [dbo].[Planta]  P ON H.Id_planta_coatings  = P.id_planta
                            WHERE H.Dock_coatings = @dockName
                            ORDER BY H.EntryDate_coatings ASC";


                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dockName", dockName);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Registro Reg = new Registro();
                    Reg.Id_all_coatings = Convert.ToInt32(reader["Id_all_coatings"]);
                    Reg.assignedDate_coatings = Convert.ToDateTime(reader["EntryDate_coatings"]);
                    Reg.assignedFromtime_coatings = Convert.ToString(reader["From_time_coatings"]);
                    Reg.assignedTotime_coatings = Convert.ToString(reader["To_time_coatings"]);
                    Reg.partNumber_coatings = Convert.ToString(reader["Part_number_coatings"]);
                    Reg.Id_cliente_coatings = Convert.ToInt32(reader["Id_cliente_coatings"]);
                    Reg.Id_planta_coatings = Convert.ToInt32(reader["Id_planta_coatings"]);
                    Reg.Id_carrier_coatings = Convert.ToInt32(reader["Id_carrier_coatings"]);
                    Reg.PlantName_coatings = Convert.ToString(reader["Plant"]);
                    Reg.assignedBOL_coatings = Convert.ToInt32(reader["Bill_of_Lading_coatings"]);
                    Reg.assignedQTY_coatings = Convert.ToInt32(reader["Quantity_coatings"]);
                    Reg.assignedDock_coatings = Convert.ToString(reader["Dock_coatings"]);
                    Reg.shipStatus_coatings = Convert.ToString(reader["shipStatus_coatings"]);
                    Reg.shipReason_coatings = Convert.ToString(reader["shipReason_coatings"]);
                    Reg.shipComment_coatings = Convert.ToString(reader["shipComment_coatings"]);
                    Reg.ClienteName_coatings = Convert.ToString(reader["Cliente"]);
                    Reg.CarrierName_coatings = Convert.ToString(reader["Carrier"]);
                    regList.Add(Reg);
                }
            }
            return regList;
        }

        //                                                                    DOCK FILTER - OUTPUT 
        //SHIPMENT - OUTPUTS - DOCK

        public static List<Registro> dockQueryOutput_coatings(string dockName)
        {
            List<Registro> regList = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all_output_coatings,H.EntryDate_output_coatings,H.From_time_output_coatings, H.To_time_output_coatings, H.Part_number_output_coatings, H.Id_cliente_output_coatings, H.Id_planta_output_coatings, H.Id_carrier_output_coatings, H.Bill_of_Lading_output_coatings, H.Quantity_output_coatings, H.Dock_output_coatings,H.shipStatus_output_coatings, H.shipReason_output_coatings, H.shipComment_output_coatings, 
                            C.description AS 'Cliente_output', L.description AS 'Carrier_output', P.description AS 'Plant_output'
                            FROM [dbo].[LogOutput_coatings] H
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente_output_coatings = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier_output_coatings = L.id_carrier
                            INNER JOIN [dbo].[Planta]  P ON H.Id_planta_output_coatings  = P.id_planta
                            WHERE H.Dock_output_coatings = @dockName
                            ORDER BY H.EntryDate_output_coatings ASC";


                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@dockName", dockName);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Registro Reg = new Registro();
                    Reg.Id_all_output_coatings = Convert.ToInt32(reader["Id_all_output_coatings"]);
                    Reg.assignedDate_output_coatings = Convert.ToDateTime(reader["EntryDate_output_coatings"]);
                    Reg.assignedFromtime_output_coatings = Convert.ToString(reader["From_time_output_coatings"]);
                    Reg.assignedTotime_output_coatings = Convert.ToString(reader["To_time_output_coatings"]);
                    Reg.partNumber_output_coatings = Convert.ToString(reader["Part_number_output_coatings"]);
                    Reg.Id_cliente_output_coatings = Convert.ToInt32(reader["Id_cliente_output_coatings"]);
                    Reg.Id_planta_output_coatings = Convert.ToInt32(reader["Id_planta_output_coatings"]);
                    Reg.Id_carrier_output_coatings = Convert.ToInt32(reader["Id_carrier_output_coatings"]);
                    Reg.PlantName_output_coatings = Convert.ToString(reader["Plant_output"]);
                    Reg.assignedBOL_output_coatings = Convert.ToInt32(reader["Bill_of_Lading_output_coatings"]);
                    Reg.assignedQTY_output_coatings = Convert.ToInt32(reader["Quantity_output_coatings"]);
                    Reg.assignedDock_output_coatings = Convert.ToString(reader["Dock_output_coatings"]);
                    Reg.shipStatus_output_coatings = Convert.ToString(reader["shipStatus_output_coatings"]);
                    Reg.shipReason_output_coatings = Convert.ToString(reader["shipReason_output_coatings"]);
                    Reg.shipComment_output_coatings = Convert.ToString(reader["shipComment_output_coatings"]);
                    Reg.ClienteName_output_coatings = Convert.ToString(reader["Cliente_output"]);
                    Reg.CarrierName_output_coatings = Convert.ToString(reader["Carrier_output"]);
                    regList.Add(Reg);
                }
            }
            return regList;
        }
    }
}