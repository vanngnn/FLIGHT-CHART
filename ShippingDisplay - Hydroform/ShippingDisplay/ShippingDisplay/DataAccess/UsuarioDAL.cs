using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Transactions;
using SimpleCrypto;
using ShippingDisplay.ShippingDisplay.DataAccess.Entidades;

namespace ShippingDisplay.ShippingDisplay.DataAccess
{
    public class UsuarioDAL
    {
        //VALIDACION DE EXISTENCIA
        public static bool Existe(string user)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT COUNT(*) FROM Usuarios WHERE usuario= @user";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@user", user);
                int resultado = Convert.ToInt32(cmd.ExecuteScalar());
                if (resultado == 0)
                    return false;
                else
                    return true;
            }
        }
        //REGRESAR USUARIO Y CONTRASEÑA POR USERNAME
        public static Usuario ObtenerUser(string username)
        {
            Usuario usr = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT U.id_user, U.nombre, U.usuario, U.password, U.salt, U.email, U.id_depto, U.id_planta, U.activo FROM [dbo].[Usuarios] U
                                WHERE U.Usuario = @username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", username);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    usr = ConvertirUsername(reader);
                }
            }
            return usr;
        }
        private static Usuario ConvertirUsername(IDataReader reader)
        {
            Usuario usr = new Usuario();
            usr.Id_user = Convert.ToInt32(reader["id_user"]);
            usr.Nombre = Convert.ToString(reader["nombre"]);
            usr.User = Convert.ToString(reader["usuario"]);
            usr.Password = Convert.ToString(reader["password"]);
            usr.Salt = Convert.ToString(reader["salt"]);
            usr.Email = Convert.ToString(reader["email"]);
            usr.Id_depto = Convert.ToInt32(reader["id_depto"]);
            usr.Id_planta = Convert.ToInt32(reader["id_planta"]);
            usr.Activo = Convert.ToBoolean(reader["activo"]);
            return usr;
        }
        //ACTUALIZAR ULTIMO INICIO DE SESSION DE USUARIO
        public static Usuario ActualizarInicio(Usuario usr)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"UPDATE Usuarios SET ult_sesion=@ult_sesion
                                 WHERE usuario = @username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", usr.User);
                cmd.Parameters.AddWithValue("@ult_sesion", usr.Ult_sesion);
                cmd.ExecuteNonQuery();
            }
            return usr;
        }

        public static Usuario Save(Usuario Usr)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                if (Existe(Usr.User))
                    Actualizar(Usr);
                else
                    AgregarUsuario(Usr);
                scope.Complete();
            }
            return Usr;
        }

        public static Usuario AgregarUsuario(Usuario usr)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                //---------------ALTA DE USUARIO---------------------
                string query = @"INSERT INTO Usuarios(nombre, usuario, password, salt, email, fec_alt, fec_mod, ult_sesion, id_planta, id_depto, activo) 
                                 VALUES (@nombre, @usuario, @password, @salt, @email, GETDATE(), GETDATE(), NULL, @id_planta, @id_depto, @activo);";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@nombre", usr.Nombre);
                cmd.Parameters.AddWithValue("@usuario", usr.User);
                cmd.Parameters.AddWithValue("@password", usr.Password);
                cmd.Parameters.AddWithValue("@salt", usr.Salt);
                cmd.Parameters.AddWithValue("@email", usr.Email);
                cmd.Parameters.AddWithValue("@id_planta", usr.Id_planta);
                cmd.Parameters.AddWithValue("@id_depto", usr.Id_depto);
                cmd.Parameters.AddWithValue("@activo", usr.Activo);
                cmd.ExecuteNonQuery();
            }
            return usr;
        }
        //METODO ACTUALIZACION DE DATOS
        public static Usuario Actualizar(Usuario usr)
        {
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                //BUSCAR RELACIÓN CONTRA EL PASSWORD NO SE DESENCRIPTA EL PASSWORD POR SEGURIDAD
                SqlCommand pwd = new SqlCommand("SELECT password FROM Usuarios where usuario=@username ", conn);
                pwd.Parameters.AddWithValue("@username", usr.User);
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(pwd);
                da.Fill(dt);
                string pass, password, salt;
                DataRow row = dt.Rows[0];
                pass = Convert.ToString(row["password"]);
                if (pass == usr.Password)
                {
                    password = usr.Password;
                    salt = usr.Salt;
                }
                else
                {
                    //Generar el algoritmo de encriptación
                    ICryptoService cryptoService = new PBKDF2();
                    salt = cryptoService.GenerateSalt();
                    password = cryptoService.Compute(usr.Password);
                }
                string query = @"UPDATE Usuarios SET nombre = @nombre, password = @password, salt = @salt, 
                                 email = @email,  fec_mod = GETDATE(), id_planta= @id_planta, id_depto = @id_depto, activo= @activo 
                                 WHERE usuario = @username";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@username", usr.User);
                cmd.Parameters.AddWithValue("@nombre", usr.Nombre);
                cmd.Parameters.AddWithValue("@password", password);
                cmd.Parameters.AddWithValue("@salt", salt);
                cmd.Parameters.AddWithValue("@email", usr.Email);
                cmd.Parameters.AddWithValue("@id_planta", usr.Id_planta);
                cmd.Parameters.AddWithValue("@id_depto", usr.Id_depto);
                cmd.Parameters.AddWithValue("@activo", usr.Activo);
                cmd.ExecuteNonQuery();
            }
            return usr;
        }
        //OBTENER LISTADO DE USUARIOS
        public static List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> lista = new List<Usuario>();
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"select U.id_user, U.nombre, U.usuario, U.email, P.description as 'Planta', A.description as 'Area',
                            case when U.activo= 1 then 'Activo' 
                            when U.activo=0 then 'Inactivo' end status, U.fec_mod
                            from Usuarios U
                            INNER JOIN Planta P on U.id_planta = P.id_planta
                            INNER JOIN Departamento A on U.id_depto = A.id_dept";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(ConvertirUsuarios(reader));
                }
            }
            return lista;
        }
        private static Usuario ConvertirUsuarios(IDataReader reader)
        {
            Usuario usr = new Usuario();
            usr.Id_user = Convert.ToInt32(reader["id_user"]);
            usr.Nombre = Convert.ToString(reader["nombre"]);
            usr.User = Convert.ToString(reader["usuario"]);
            usr.Email = Convert.ToString(reader["email"]);
            usr.DesPlanta = Convert.ToString(reader["Planta"]);
            usr.DesDepto = Convert.ToString(reader["Area"]);
            usr.Status = Convert.ToString(reader["status"]);
            usr.Fec_mod = Convert.ToDateTime(reader["fec_mod"]);
            return usr;
        }
        //ELIMINAR REGISTROS DE USUARIOS
        public static void EliminarRegistro(int id_user)
        {
            using (TransactionScope scope = new TransactionScope())
            {
                //Eliminar la entidad usuario
                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
                {
                    conn.Open();
                    string query = @"DELETE FROM Usuarios WHERE id_user = @id_user";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id_user", id_user);
                    cmd.ExecuteNonQuery();
                }
                scope.Complete();
            }
        }
        //OBTENER POR ID
        public static Usuario ObtenerById(int id_user)
        {
            Usuario usr = null;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["SqlCon"].ToString()))
            {
                conn.Open();
                string query = @"SELECT U.id_user, U.nombre, U.usuario, U.password, U.salt, U.email, U.id_depto, U.id_planta, U.activo FROM [dbo].[Usuarios] U
                                WHERE U.id_user = @id_user";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id_user", id_user);
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    usr = ConvertirUsername(reader);
                }
            }
            return usr;
        }
    }
}