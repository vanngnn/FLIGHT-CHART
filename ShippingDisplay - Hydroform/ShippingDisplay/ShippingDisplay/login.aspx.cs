using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ShippingDisplay.ShippingDisplay.DataAccess;
using ShippingDisplay.ShippingDisplay.DataAccess.Entidades;
using SimpleCrypto;

namespace ShippingDisplay.ShippingDisplay
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //RECUPERAR LOS VALORES INGRESADOS POR EL USUARIO
            string usuario = txtUsuario.Text.Trim();
            string password = txtPassword.Text.Trim();
            if (usuario != "" && password != "")
            {
                Usuario usr = UsuarioDAL.ObtenerUser(usuario);
                if (usr != null)
                {
                    //ALMACENAMOS INFORMACION RECUPERADA 
                    string Username = usr.User;
                    string Contrasenia = usr.Password;
                    string salt = usr.Salt;
                    //GENERAMOS ALGORITMO DE ENCRIPTACION A COMPARAR 
                    ICryptoService cryptoService = new PBKDF2();
                    string contraseniaEncriptada = cryptoService.Compute(password, salt);
                    if (cryptoService.Compare(Contrasenia, contraseniaEncriptada))
                    {
                        try
                        {
                            DateTime ult_fec = DateTime.Now;
                            Usuario UsrSession = new Usuario();
                            {
                                usr.User = Username;
                                usr.Ult_sesion = ult_fec;
                            }
                            UsuarioDAL.ActualizarInicio(usr);
                            FormsAuthentication.RedirectFromLoginPage(Username, true);
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "AlertLogin", "window.onload = function(){ alert('Successful login .'); };", true);
                        }
                        catch
                        {
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Error", "window.onload = function(){ alert('A problem occurred, try again'); };", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "AlertLogin", "window.onload = function(){ alert('Incorrect password.'); };", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "AlertLogin", "window.onload = function(){ alert('User does not exist .'); };", true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Alert", "window.onload = function(){ alert('Username or password must be filled in.'); };", true);
            }
        }
    }
}