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
    public class CarrierDAL
    {
        public static List<Carrier> ObtenerCarrier()
        {
            List<Carrier> lista = new List<Carrier>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                con.Open();
                string query = @"SELECT id_carrier, description FROM Carrier WHERE status=1";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirCarrier(reader));
                }
            }
            return lista;
        }
        private static Carrier ConvertirCarrier(IDataReader reader)
        {
            Carrier linea = new Carrier();
            linea.Id_carrier = Convert.ToInt32(reader["id_carrier"]);
            linea.Description = Convert.ToString(reader["description"]);
            return linea;
        }
    }
}