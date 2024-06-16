using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Transactions;
using System.Data.SqlClient;
using System.Configuration;
using ShippingDisplay.ShippingDisplay.DataAccess.Entidades;

namespace ShippingDisplay.ShippingDisplay.DataAccess
{
    public class RegistroDAL
    {
        public static Registro AgregarNuevo(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                //OBTENER EL ID UNICO DEL ISSUE
                int ID;

                string query = @"INSERT INTO the_whole_table (Date,From_time,To_time,Part_number,Id_cliente,Id_planta,Id_carrier,Bill_of_Lading,Quantity,Dock,shipStatus, shipReason, shipComment)
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
                //cmd.Parameters.AddWithValue("@Tarjeta", Reg.Tarjeta);
                //cmd.Parameters.AddWithValue("@shipStatus", Reg.Status);
                //RECUPERAR ID GENERADO POR LA TAB
                Reg.Id_all = Convert.ToInt32(cmd.ExecuteScalar());
                ID = Reg.Id_all;

                //--------------------------DETALLE DEL HEADER
                string query2 = @"INSERT INTO Shipdet (Id_all, Placas, Caja, NombreOperador, Telefono) 
                                 VALUES (@Id_all, @Placas, @Caja, @NombreOperador, @Telefono); SELECT SCOPE_IDENTITY()";
                SqlCommand cmdd = new SqlCommand(query2, conn);
                cmdd.Parameters.AddWithValue("@Id_all", ID);
                cmdd.Parameters.AddWithValue("@Placas", Reg.Placas);
                cmdd.Parameters.AddWithValue("@Caja", Reg.Caja);
                cmdd.Parameters.AddWithValue("@NombreOperador", Reg.NombreOperador);
                cmdd.Parameters.AddWithValue("@Telefono", Reg.Telefono);
                Reg.Id_det = Convert.ToInt32(cmdd.ExecuteScalar());
            }
            return Reg;
        }
        public static Registro ActualizarRegistro(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"UPDATE Shipreg SET Id_cliente = @Id_cliente, Id_carrier = @Id_carrier, Tarjeta = @Tarjeta WHERE Id_all = @Id_all";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_cliente", Reg.Id_cliente);
                cmd.Parameters.AddWithValue("@Id_carrier", Reg.Id_carrier);
                cmd.Parameters.AddWithValue("@Tarjeta", Reg.Tarjeta);
                cmd.Parameters.AddWithValue("@Id_all", Reg.Id_all);
                cmd.ExecuteNonQuery();

                string query2 = @"UPDATE Shipdet SET Placas = @Placas, Caja=@Caja, NombreOperador = @NombreOperador, Telefono = @Telefono WHERE Id_all = @Id_all";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Placas", Reg.Placas);
                cmd2.Parameters.AddWithValue("@Caja", Reg.Caja);
                cmd2.Parameters.AddWithValue("@NombreOperador", Reg.NombreOperador);
                cmd2.Parameters.AddWithValue("@Telefono", Reg.Telefono);
                cmd2.Parameters.AddWithValue("@Id_all", Reg.Id_all);
                cmd2.ExecuteNonQuery();
            }
            return Reg;
        }
        public static void EliminarRegistro(int Id_all)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //Eliminar registros de categorias
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
                {
                    conn.Open();
                    string query = @"DELETE FROM Shipreg WHERE Id_all = @Id_all;  DELETE FROM Shipdet WHERE Id_all = @Id_all; ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id_all", Id_all);
                    cmd.ExecuteNonQuery();
                }
                scope.Complete();
            }
        }
        public static Registro ObtenerById(int Id_all)
        {
            Registro Reg = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all, H.Id_cliente, H.Id_carrier, H.Entrada, H.Salida, H.Tarjeta, D.Placas,  D.Caja, D.NombreOperador, D.Telefono, 
                            C.description AS 'Cliente', L.description AS 'Carrier',  H.Id_planta
                            FROM [dbo].[Shipreg] H
                            INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier WHERE  H.Id_all=@Id_all";
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
        //SALIDA
        public static Registro ObtenerByTarget(int Tarjeta)
        {
            Registro Reg = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all, H.Id_cliente, H.Id_carrier, H.Entrada, H.Salida, H.Tarjeta, D.Placas,  D.Caja, D.NombreOperador, D.Telefono, 
                            C.description AS 'Cliente', L.description AS 'Carrier',  H.Id_planta
                            FROM [dbo].[Shipreg] H
                            INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier WHERE Tarjeta";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Tarjeta", Tarjeta);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Reg = ConvertirRegistro(reader);
                }
            }
            return Reg;
        }
        public static Registro ActualizarSalida(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"UPDATE Shipreg SET  Salida = GETDATE(), shipStatus=@shipStatus WHERE Id_all = @Id_all";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_all", Reg.Id_all);
                cmd.Parameters.AddWithValue("@shipStatus", Reg.Status);
                cmd.ExecuteNonQuery();
            }
            return Reg;
        }
        //LLENAR EL GRIDVIEW X shipStatus Y PLANTA
        public static List<Registro> ListadoRegistros(int Status, int Id_planta)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all, H.Id_cliente, H.Id_carrier, H.Entrada, H.Salida, H.Tarjeta, D.Placas, D.Caja, D.NombreOperador, D.Telefono, 
                            C.description AS 'Cliente', L.description AS 'Carrier',  H.Id_planta
                            FROM [dbo].[Shipreg] H
                            INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier WHERE H.shipStatus=@shipStatus AND H.Id_planta=@Id_planta ORDER BY H.Entrada desc ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@shipStatus", Status);
                cmd.Parameters.AddWithValue("@Id_planta", Id_planta);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirRegistro(reader));
                }
            }
            return lista;
        }
        private static Registro ConvertirRegistro(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all = Convert.ToInt32(reader["Id_all"]);
            Reg.Id_cliente = Convert.ToInt32(reader["Id_cliente"]);
            Reg.Id_carrier = Convert.ToInt32(reader["Id_carrier"]);
            //Reg.Entrada = Convert.ToDateTime(reader["Entrada"]);
            //Reg.Salida = Convert.ToString(reader["Salida"]);
            //Reg.Tarjeta = Convert.ToInt32(reader["Tarjeta"]);
            //Reg.Placas = Convert.ToString(reader["Placas"]);
            //Reg.Caja = Convert.ToString(reader["Caja"]);
            Reg.NombreOperador = Convert.ToString(reader["NombreOperador"]);
            Reg.Telefono = Convert.ToString(reader["Telefono"]);
            Reg.ClienteName = Convert.ToString(reader["Cliente"]);
            Reg.CarrierName = Convert.ToString(reader["Carrier"]);
            Reg.Id_planta = Convert.ToInt32(reader["Id_planta"]);
            return Reg;
        }
        //GRIDVIEW X PLANTA
        public static List<Registro> ListadoDashboard (int Id_planta)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_all,H.Date,H.From_time,H.To_time AS TimeRange,H.Part_number, H.Id_cliente, H.Id_planta,H.Id_carrier,H.Bill_of_Lading, H.Quantity,H.Dock,H.shipStatus, H.shipReason,H.shipComment, C.description AS 'Cliente',L.description AS 'Carrier', D.Caja, R.description AS 'Ruta',
                        CASE WHEN shipStatus = 3 THEN 'SENT' WHEN shipStatus = 1 THEN 'EARRING'
	                         --WHEN shipStatus = 2 AND ( (LEFT(CONVERT(TIME,H.Entrada,108),8)>R.input) AND (LEFT(CONVERT(TIME,H.Entrada,108),8) < R.output) ) THEN 'ONTIME'
	                         WHEN shipStatus = 2 AND ( (LEFT(CONVERT(TIME,H.Entrada,108),8)<R.input)) THEN 'ONTIME'
	                         WHEN shipStatus = 2 AND ( (LEFT(CONVERT(TIME,H.Entrada,108),8)>R.input)) THEN 'DELAYED' END AS ESTADO 
                        FROM [dbo].[the_whole_table] H
                        INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
                        INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                        INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier 
                        LEFT JOIN [dbo].[Ruta] R ON H.Id_ruta = R.Id_ruta
                        WHERE  H.Id_planta=@Id_planta AND CONVERT(Date,H.Entrada) = CONVERT(DATE,GETDATE()) ORDER BY H.Entrada desc ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_planta", Id_planta);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirDash(reader));
                }
            }
            return lista;
        }
        private static Registro ConvertirDash(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all = Convert.ToInt32(reader["Id_all"]);
            Reg.Id_cliente = Convert.ToInt32(reader["Id_cliente"]);
            Reg.Id_carrier = Convert.ToInt32(reader["Id_carrier"]);
            Reg.Entrada = Convert.ToDateTime(reader["Entrada"]);
            Reg.Salida = Convert.ToString(reader["Salida"]);
            Reg.ClienteName = Convert.ToString(reader["Cliente"]);
            Reg.CarrierName = Convert.ToString(reader["Carrier"]);
            Reg.Caja = Convert.ToString(reader["Caja"]);
            Reg.RutaName = Convert.ToString(reader["Ruta"]);
            Reg.Shipper = Convert.ToInt32(reader["Shipper"]);
            Reg.Estado = Convert.ToString(reader["Estado"]);
            Reg.Id_planta = Convert.ToInt32(reader["Id_planta"]);
            return Reg;
        }
        //ACTUALIZAR SHIPPER 
        public static Registro ActualizarShipper(Registro Reg)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"UPDATE Shipreg SET  Shipper = @Shipper, Id_ruta=@Id_ruta, shipStatus=@shipStatus WHERE Id_all = @Id_all";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_all", Reg.Id_all);
                cmd.Parameters.AddWithValue("@Shipper", Reg.Shipper);
                cmd.Parameters.AddWithValue("@Id_ruta", Reg.Id_ruta);
                cmd.Parameters.AddWithValue("@shipStatus", Reg.Status);
                cmd.ExecuteNonQuery();
            }
            return Reg;
        }
        //OBTENER WIDGETS
        public static Registro ObtenerRegistros(int Id_planta)
        {
            Registro list = null;
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                con.Open();
                string query = @"
                        SELECT DISTINCT  
                        (SELECT ISNULL((Count(H.shipStatus)),0) FROM Shipreg H  WHERE H.shipStatus = 3  AND CONVERT(Date,H.Entrada) = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta ) AS 'SENT',

                        (SELECT ISNULL((Count(H.shipStatus)),0) FROM Shipreg H  WHERE H.shipStatus = 1  AND CONVERT(Date,H.Entrada) = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta) AS 'EARRING',

                        (SELECT ISNULL((Count(H.shipStatus)),0) FROM the_whole_table H  
                        LEFT JOIN [dbo].[Ruta] R ON H.Id_ruta = R.Id_ruta
                        WHERE shipStatus = 2 AND CONVERT(Date,H.Entrada) = CONVERT(DATE,GETDATE()) AND ( (LEFT(CONVERT(TIME,H.Entrada,108),8)<R.input)) AND H.Id_planta=@Id_planta)AS 'ONTIME' ,

                        (SELECT ISNULL((Count(H.shipStatus)),0) AS 'DELAYED' FROM Shipreg H 
                        LEFT JOIN [dbo].[Ruta] R ON H.Id_ruta = R.Id_ruta
                        WHERE H.shipStatus = 2  AND CONVERT(Date,H.Entrada) = CONVERT(DATE,GETDATE()) AND ((LEFT(CONVERT(TIME,H.Entrada,108),8)>R.input)) AND H.Id_planta=@Id_planta) AS 'DELAYED' 
                        FROM Shipreg ";
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
        private static Registro Convertir(IDataReader reader)
        {
            Registro list = new Registro();
            list.Completed = Convert.ToString(reader["SENT"]);
            list.Pendiente = Convert.ToString(reader["EARRING"]);
            list.Ontime =    Convert.ToString(reader["ONTIME"]);
            list.DELAYED =   Convert.ToString(reader["DELAYED"]);
            return list;
        }


        public static List<Registro> FiltroReporte(int Id_planta, string FechaIni, string FechaFin)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"
                        SELECT H.Id_all, H.Id_cliente, H.Id_carrier, H.Entrada, CASE WHEN H.Salida='1900-01-01 00:00:00.000' THEN '-' WHEN H.Salida<>'1900-01-01 00:00:00.000' THEN Convert(nvarchar,H.Salida,21) END AS 'Salida',
                        C.description AS 'Cliente', L.description AS 'Carrier', D.Caja, R.description as 'Ruta',
                        R.input, R.output,
                        H.Shipper, H.Id_ruta, H.Id_planta, 
                        CASE WHEN shipStatus = 1 THEN 'NO SHIPPER ASSIGNMENT'
                        WHEN H.Salida ='1900-01-01 00:00:00.000' THEN 'DO NOT REGISTER OUT'
                        WHEN ( (LEFT(CONVERT(TIME,H.Entrada,108),8)<R.input)) THEN 'ONTIME'
                        WHEN ( (LEFT(CONVERT(TIME,H.Entrada,108),8)>R.input)) THEN 'DELAYED' END AS ESTADO 
                        FROM [dbo].[Shipreg] H
                        INNER JOIN [dbo].[Shipdet] D ON H.Id_all=D.Id_all 
                        INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                        INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier 
                        LEFT JOIN  [dbo].[Ruta] R ON H.Id_ruta = R.Id_ruta
                        WHERE  H.Id_planta=@Id_planta AND CONVERT(Date, H.Entrada) BETWEEN '" + FechaIni + "' AND ' " + @FechaFin + "'  ORDER BY H.Entrada desc ";
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
        private static Registro ConvertirFiltro(IDataReader reader)
        {
            Registro Reg = new Registro();
            Reg.Id_all = Convert.ToInt32(reader["Id_all"]);
            Reg.Id_cliente = Convert.ToInt32(reader["Id_cliente"]);
            Reg.Id_carrier = Convert.ToInt32(reader["Id_carrier"]);
            Reg.Entrada = Convert.ToDateTime(reader["Entrada"]);
            Reg.Salida = Convert.ToString(reader["Salida"]);
            Reg.ClienteName = Convert.ToString(reader["Cliente"]);
            Reg.CarrierName = Convert.ToString(reader["Carrier"]);
            Reg.Caja = Convert.ToString(reader["Caja"]);
            Reg.RutaName = Convert.ToString(reader["Ruta"]);
            Reg.Input = Convert.ToString(reader["input"]);
            Reg.Output = Convert.ToString(reader["output"]);
            Reg.Shipper = Convert.ToInt32(reader["Shipper"]);
            Reg.Estado = Convert.ToString(reader["Estado"]);
            Reg.Id_planta = Convert.ToInt32(reader["Id_planta"]);
            return Reg;
        }
    }
}