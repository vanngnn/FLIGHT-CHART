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
    public class ClienteDAL
    {
        public static List<Cliente> ObtenerClientes()
        {
            List<Cliente> lista = new List<Cliente>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                con.Open();
                string query = @"SELECT id_cliente, description FROM Cliente WHERE status=1";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirCliente(reader));
                }
            }
            return lista;
        }
        private static Cliente ConvertirCliente(IDataReader reader)
        {
            Cliente linea = new Cliente();
            linea.Id_cliente = Convert.ToInt32(reader["id_cliente"]);
            linea.Description = Convert.ToString(reader["description"]);
            return linea;
        }
    }
}