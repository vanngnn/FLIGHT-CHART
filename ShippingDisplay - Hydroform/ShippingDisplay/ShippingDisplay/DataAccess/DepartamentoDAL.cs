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
    public class DepartamentoDAL
    {
        public static List<Departamento> ObtenerDept()
        {
            List<Departamento> lista = new List<Departamento>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                con.Open();
                string query = @"SELECT id_dept, description FROM Departamento where status=1 ";
                SqlCommand cmd = new SqlCommand(query, con);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirDept(reader));
                }
            }
            return lista;
        }
        private static Departamento ConvertirDept(IDataReader reader)
        {
            Departamento dept = new Departamento();
            dept.Id_dept = Convert.ToInt32(reader["id_dept"]);
            dept.Description = Convert.ToString(reader["description"]);
            return dept;
        }

    }
}