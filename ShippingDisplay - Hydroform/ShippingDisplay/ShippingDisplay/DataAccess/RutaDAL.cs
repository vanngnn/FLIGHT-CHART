using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ShippingDisplay.ShippingDisplay.DataAccess.Entidades; 

namespace ShippingDisplay.ShippingDisplay.DataAccess
{
    public class RutaDAL
    {
        public static List<Ruta> ObtenerRutas(int Id_planta)
        {
            List<Ruta> lista = new List<Ruta>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                con.Open();
                string query = @"SELECT id_ruta, description FROM Ruta WHERE status=1 AND dia =DATEPART(dw, GETDATE()) AND id_planta=@Id_planta";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Id_planta", Id_planta);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirRuta(reader));
                }
            }
            return lista;
        }
        private static Ruta ConvertirRuta(IDataReader reader)
        {
            Ruta RT = new Ruta();
            RT.Id_ruta = Convert.ToInt32(reader["id_ruta"]);
            RT.Description = Convert.ToString(reader["description"]);
            return RT;
        }
    }
}