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
    public class PlantaDAL
    {
        public static List<Planta> ObtenerPlantas()
        {
            List<Planta> lista = new List<Planta>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                con.Open();
                string query = @"SELECT id_planta, description FROM Planta WHERE status=1";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirPlant(reader));
                }
            }
            return lista;
        }
        private static Planta ConvertirPlant(IDataReader reader)
        {
            Planta plant = new Planta();
            plant.Id_planta = Convert.ToInt32(reader["id_planta"]);
            plant.Description = Convert.ToString(reader["description"]);
            return plant;
        }
    }
}