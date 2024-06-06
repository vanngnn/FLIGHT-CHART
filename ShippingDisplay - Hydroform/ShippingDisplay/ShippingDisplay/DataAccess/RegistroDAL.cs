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
                //OBTAIN THE UNIQUE ID OF THE ISSUE
                int ID;

                string query = @"INSERT INTO Shipreg (Id_cliente, Id_carrier, Entrada, Salida, Shipper, Id_ruta, Id_planta, Tarjeta, Estatus)
                                 VALUES (@Id_cliente, @Id_carrier, GETDATE(), '', '', '',@Id_planta, @Tarjeta, @Estatus); SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_cliente", Reg.Id_cliente);
                cmd.Parameters.AddWithValue("@Id_carrier", Reg.Id_carrier);
                cmd.Parameters.AddWithValue("@Id_planta", Reg.Id_planta);
                cmd.Parameters.AddWithValue("@Tarjeta", Reg.Tarjeta);
                cmd.Parameters.AddWithValue("@Estatus", Reg.Status);
                //RECUPERAR ID GENERADO POR LA TAB
                Reg.Id_reg = Convert.ToInt32(cmd.ExecuteScalar());
                ID = Reg.Id_reg;

                //--------------------------DETALLE DEL HEADER
                string query2 = @"INSERT INTO Shipdet (Id_reg, Placas, Caja, NombreOperador, Telefono) 
                                 VALUES (@Id_reg, @Placas, @Caja, @NombreOperador, @Telefono); SELECT SCOPE_IDENTITY()";
                SqlCommand cmdd = new SqlCommand(query2, conn);
                cmdd.Parameters.AddWithValue("@Id_reg", ID);
                cmdd.Parameters.AddWithValue("@Placas", Reg.Placas);
                cmdd.Parameters.AddWithValue("@Caja", Reg.Caja);
                cmdd.Parameters.AddWithValue("@NombreOperador", Reg.NombreOperador);
                cmdd.Parameters.AddWithValue("@Telefono", Reg.Telefono);
                Reg.Id_det = Convert.ToInt32(cmdd.ExecuteScalar());
            }
            return Reg;
        }
        public static Registro ActualizarRegistro(Registro Reg) //UpdateRegistry
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"UPDATE Shipreg SET Id_cliente = @Id_cliente, Id_carrier = @Id_carrier, Tarjeta = @Tarjeta WHERE Id_reg = @id_reg";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_cliente", Reg.Id_cliente);
                cmd.Parameters.AddWithValue("@Id_carrier", Reg.Id_carrier);
                cmd.Parameters.AddWithValue("@Tarjeta", Reg.Tarjeta);
                cmd.Parameters.AddWithValue("@Id_reg", Reg.Id_reg);
                cmd.ExecuteNonQuery();

                string query2 = @"UPDATE Shipdet SET Placas = @Placas, Caja=@Caja, NombreOperador = @NombreOperador, Telefono = @Telefono WHERE Id_reg = @id_reg";
                SqlCommand cmd2 = new SqlCommand(query2, conn);
                cmd2.Parameters.AddWithValue("@Placas", Reg.Placas);
                cmd2.Parameters.AddWithValue("@Caja", Reg.Caja);
                cmd2.Parameters.AddWithValue("@NombreOperador", Reg.NombreOperador);
                cmd2.Parameters.AddWithValue("@Telefono", Reg.Telefono);
                cmd2.Parameters.AddWithValue("@Id_reg", Reg.Id_reg);
                cmd2.ExecuteNonQuery();
            }
            return Reg;
        }
        public static void EliminarRegistro(int Id_reg)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //Eliminar registros de categorias
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
                {
                    conn.Open();
                    string query = @"DELETE FROM Shipreg WHERE Id_reg = @Id_reg;  DELETE FROM Shipdet WHERE Id_reg = @Id_reg; ";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id_reg", Id_reg);
                    cmd.ExecuteNonQuery();
                }
                scope.Complete();
            }
        }
        public static Registro ObtenerById(int Id_reg)
        {
            Registro Reg = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_reg, H.Id_cliente, H.Id_carrier, H.Entrada, H.Salida, H.Tarjeta, D.Placas,  D.Caja, D.NombreOperador, D.Telefono, 
                            C.description AS 'Cliente', L.description AS 'Carrier',  H.Id_planta
                            FROM [dbo].[Shipreg] H
                            INNER JOIN [dbo].[Shipdet] D ON H.Id_reg=D.Id_reg 
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier WHERE  H.Id_reg=@Id_reg";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_reg", Id_reg);
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
                string query = @"SELECT H.Id_reg, H.Id_cliente, H.Id_carrier, H.Entrada, H.Salida, H.Tarjeta, D.Placas,  D.Caja, D.NombreOperador, D.Telefono, 
                            C.description AS 'Cliente', L.description AS 'Carrier',  H.Id_planta
                            FROM [dbo].[Shipreg] H
                            INNER JOIN [dbo].[Shipdet] D ON H.Id_reg=D.Id_reg 
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
                string query = @"UPDATE Shipreg SET  Salida = GETDATE(), Estatus=@Estatus WHERE Id_reg = @id_reg";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_reg", Reg.Id_reg);
                cmd.Parameters.AddWithValue("@Estatus", Reg.Status);
                cmd.ExecuteNonQuery();
            }
            return Reg;
        }
        //LLENAR EL GRIDVIEW X ESTATUS Y PLANTA
        public static List<Registro> ListadoRegistros(int Status, int Id_planta)
        {
            List<Registro> lista = new List<Registro>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT H.Id_reg, H.Id_cliente, H.Id_carrier, H.Entrada, H.Salida, H.Tarjeta, D.Placas, D.Caja, D.NombreOperador, D.Telefono, 
                            C.description AS 'Cliente', L.description AS 'Carrier',  H.Id_planta
                            FROM [dbo].[Shipreg] H
                            INNER JOIN [dbo].[Shipdet] D ON H.Id_reg=D.Id_reg 
                            INNER JOIN [dbo].[Cliente] C ON H.Id_cliente = C.id_cliente
                            INNER JOIN [dbo].[Carrier] L ON H.Id_carrier = L.id_carrier WHERE H.Estatus=@Estatus AND H.Id_planta=@Id_planta ORDER BY H.Entrada desc ";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Estatus", Status);
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
            Reg.Id_reg = Convert.ToInt32(reader["Id_reg"]);
            Reg.Id_cliente = Convert.ToInt32(reader["Id_cliente"]);
            Reg.Id_carrier = Convert.ToInt32(reader["Id_carrier"]);
            Reg.Entrada = Convert.ToDateTime(reader["Entrada"]);
            Reg.Salida = Convert.ToString(reader["Salida"]);
            Reg.Tarjeta = Convert.ToInt32(reader["Tarjeta"]);
            Reg.Placas = Convert.ToString(reader["Placas"]);
            Reg.Caja = Convert.ToString(reader["Caja"]);
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
                string query = @"
                        SELECT H.Id_reg, H.Id_cliente, H.Id_carrier, H.Entrada, CASE WHEN H.Salida='1900-01-01 00:00:00.000' THEN '-' WHEN H.Salida<>'1900-01-01 00:00:00.000' THEN Convert(nvarchar,H.Salida,21) END AS 'Salida', C.description AS 'Cliente', L.description AS 'Carrier', D.Caja, R.description as 'Ruta',
                        H.Shipper, H.Id_ruta, H.Id_planta, 
                        CASE WHEN Estatus = 3 THEN 'SENT' 
                             WHEN Estatus = 1 THEN 'EARRING'
	                         --WHEN Estatus = 2 AND ( (LEFT(CONVERT(TIME,H.Entrada,108),8)>R.input) AND (LEFT(CONVERT(TIME,H.Entrada,108),8) < R.output) ) THEN 'ONTIME'
	                         WHEN Estatus = 2 AND ( (LEFT(CONVERT(TIME,H.Entrada,108),8)<R.input)) THEN 'ONTIME'
	                         WHEN Estatus = 2 AND ( (LEFT(CONVERT(TIME,H.Entrada,108),8)>R.input)) THEN 'DELAYED' END AS ESTADO 
                        FROM [dbo].[Shipreg] H
                        INNER JOIN [dbo].[Shipdet] D ON H.Id_reg=D.Id_reg 
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
            Reg.Id_reg = Convert.ToInt32(reader["Id_reg"]);
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
                string query = @"UPDATE Shipreg SET  Shipper = @Shipper, Id_ruta=@Id_ruta, Estatus=@Estatus WHERE Id_reg = @id_reg";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id_reg", Reg.Id_reg);
                cmd.Parameters.AddWithValue("@Shipper", Reg.Shipper);
                cmd.Parameters.AddWithValue("@Id_ruta", Reg.Id_ruta);
                cmd.Parameters.AddWithValue("@Estatus", Reg.Status);
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
                        (SELECT ISNULL((Count(H.Estatus)),0) FROM Shipreg H  WHERE H.Estatus = 3  AND CONVERT(Date,H.Entrada) = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta ) AS 'SENT',

                        (SELECT ISNULL((Count(H.Estatus)),0) FROM Shipreg H  WHERE H.Estatus = 1  AND CONVERT(Date,H.Entrada) = CONVERT(DATE,GETDATE()) AND H.Id_planta=@Id_planta) AS 'EARRING',

                        (SELECT ISNULL((Count(H.Estatus)),0) FROM Shipreg H  
                        LEFT JOIN [dbo].[Ruta] R ON H.Id_ruta = R.Id_ruta
                        WHERE Estatus = 2 AND CONVERT(Date,H.Entrada) = CONVERT(DATE,GETDATE()) AND ( (LEFT(CONVERT(TIME,H.Entrada,108),8)<R.input)) AND H.Id_planta=@Id_planta)AS 'ONTIME' ,

                        (SELECT ISNULL((Count(H.Estatus)),0) AS 'DELAYED' FROM Shipreg H 
                        LEFT JOIN [dbo].[Ruta] R ON H.Id_ruta = R.Id_ruta
                        WHERE H.Estatus = 2  AND CONVERT(Date,H.Entrada) = CONVERT(DATE,GETDATE()) AND ((LEFT(CONVERT(TIME,H.Entrada,108),8)>R.input)) AND H.Id_planta=@Id_planta) AS 'DELAYED' 
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
                        SELECT H.Id_reg, H.Id_cliente, H.Id_carrier, H.Entrada, CASE WHEN H.Salida='1900-01-01 00:00:00.000' THEN '-' WHEN H.Salida<>'1900-01-01 00:00:00.000' THEN Convert(nvarchar,H.Salida,21) END AS 'Salida',
                        C.description AS 'Cliente', L.description AS 'Carrier', D.Caja, R.description as 'Ruta',
                        R.input, R.output,
                        H.Shipper, H.Id_ruta, H.Id_planta, 
                        CASE WHEN Estatus = 1 THEN 'NO SHIPPER ASSIGNMENT'
                        WHEN H.Salida ='1900-01-01 00:00:00.000' THEN 'DO NOT REGISTER OUT'
                        WHEN ( (LEFT(CONVERT(TIME,H.Entrada,108),8)<R.input)) THEN 'ONTIME'
                        WHEN ( (LEFT(CONVERT(TIME,H.Entrada,108),8)>R.input)) THEN 'DELAYED' END AS ESTADO 
                        FROM [dbo].[Shipreg] H
                        INNER JOIN [dbo].[Shipdet] D ON H.Id_reg=D.Id_reg 
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
            Reg.Id_reg = Convert.ToInt32(reader["Id_reg"]);
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