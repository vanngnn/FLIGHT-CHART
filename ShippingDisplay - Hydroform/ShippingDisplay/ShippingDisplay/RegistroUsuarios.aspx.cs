using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShippingDisplay.ShippingDisplay.DataAccess;
using ShippingDisplay.ShippingDisplay.DataAccess.Entidades;
using SimpleCrypto;
using System.Web.Security;

namespace ShippingDisplay.ShippingDisplay
{
    public partial class RegistroUsuarios : System.Web.UI.Page
    {
        int Id_Planta;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Context.User.Identity.IsAuthenticated)
                {
                    string Username = HttpContext.Current.User.Identity.Name;
                    CargarPerfil(Username);
                    CargarDept();
                    CargarPlanta();
                    CargarGrid();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "AlertLogin", "window.onload = function(){ alert('Authentication is required. You need to sign into your account'); };", true);
                    Response.Redirect("login.aspx");
                }
            }
        }
        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                /*-----------OBTENER VALOR DE LAS VARIABLES A INGRESAR--------------*/
                string nombre = txtNombre.Text.Trim();
                string user = txtUsuario.Text.Trim();
                string password = txtPassword.Text.Trim();
                string email = txtEmail.Text.Trim();
                int Id_plant = Convert.ToInt32(dblPlanta.SelectedValue);
                int Id_dept = Convert.ToInt32(dblDepartamento.SelectedValue);
                int status;
                status = Convert.ToInt32(rbActivo.SelectedValue);

                if (UsuarioDAL.Existe(user))
                {
                    /*---------------MODIFICAR UN NUEVO USUARIO-------------------------*/
                    //SE INICIA LA ENTIDAD
                    Usuario Usr = new Usuario();
                    {
                        Usr.Nombre = nombre;
                        Usr.User = user;
                        Usr.Password = password;
                        Usr.Salt = txtSalt.Text;
                        Usr.Email = email;
                        Usr.Id_planta = Id_plant;
                        Usr.Id_depto = Id_dept;
                        Usr.Activo = Convert.ToBoolean(status);
                    }
                    UsuarioDAL.Save(Usr);
                    string script = @"<script type='text/javascript'> alert('Updated successfully'); </script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Notice", script, false);
                    CleanControl(this.Controls);
                    CargarGrid();
                }
                else
                {
                    /*---------------AGREGAR UN NUEVO USUARIO---------------------------*/

                    /*------------GENERAR ALGORITMO DE ENCRIPTACION---------------------*/
                    ICryptoService cryptoService = new PBKDF2();
                    string salt = cryptoService.GenerateSalt();
                    string contraseniaEncriptada = cryptoService.Compute(password);

                    //SE INICIA LA ENTIDAD
                    Usuario Usr = new Usuario();
                    {
                        Usr.Nombre = nombre;
                        Usr.User = user;
                        Usr.Password = contraseniaEncriptada;
                        Usr.Salt = salt;
                        Usr.Email = email;
                        Usr.Id_planta = Id_plant;
                        Usr.Id_depto = Id_dept;
                        Usr.Activo = Convert.ToBoolean(status);
                    }
                    UsuarioDAL.Save(Usr);
                    string script = @"<script type='text/javascript'> alert('Saved successfully'); </script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Notice", script, false);
                    CleanControl(this.Controls);
                    CargarGrid();

                }   
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('Oops! Something went wrong.'); </script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", script, false);
            }
        }
        private void CargarDept()
        {
            dblDepartamento.DataTextField = "description";
            dblDepartamento.DataValueField = "id_dept";
            dblDepartamento.DataSource = DepartamentoDAL.ObtenerDept();
            dblDepartamento.DataBind();
            dblDepartamento.Items.Insert(0, " - Select - ");
        }
        private void CargarPerfil(string username)
        {
            Usuario perfil = UsuarioDAL.ObtenerUser(username);
            lblNombre.Text = perfil.Nombre;
            Id_Planta = perfil.Id_planta;
            //ACTIVAR PESTAÑAS DE ACUERDO AL NIVEL DE USUARIO
            int Dept = Convert.ToInt32(perfil.Id_depto);
            if (Dept == 1)
            {
                LinkConfig.Visible = false;
                LinkRegEntry.Visible = false;
                LinkRegOut.Visible = false;
                LinkRegister.Visible = false;
                //LinkDashEmb.Visible = false;
            }
            else if (Dept == 2)
            {
                LinkShipper.Visible = false;
                LinkRegister.Visible = false;
                //LinkDashEmb.Visible = false;
            }
        }
        private void CargarPlanta()
        {
            dblPlanta.DataTextField = "description";
            dblPlanta.DataValueField = "id_planta";
            dblPlanta.DataSource = PlantaDAL.ObtenerPlantas();
            dblPlanta.DataBind();
            dblPlanta.Items.Insert(0, " - Select - ");
        }
        private void CargarGrid()
        {
            gvUsuarios.DataSource = UsuarioDAL.ObtenerUsuarios();
            gvUsuarios.DataBind();
        }
        //LIMPIAR CONTROLES
        public void CleanControl(ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is TextBox)
                    ((TextBox)control).Text = string.Empty;
                else if (control is DropDownList)
                    ((DropDownList)control).ClearSelection();
                else if (control is RadioButtonList)
                    ((RadioButtonList)control).ClearSelection();
                else if (control is CheckBoxList)
                    ((CheckBoxList)control).ClearSelection();
                else if (control is RadioButton)
                    ((RadioButton)control).Checked = false;
                else if (control is CheckBox)
                    ((CheckBox)control).Checked = false;
                else if (control.HasControls())
                    //Esta linea detécta un Control que contenga otros Controles
                    //Así ningún control se quedará sin ser limpiado.
                    CleanControl(control.Controls);
            }
        }
        protected void gvUsuarios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int cod = int.Parse(gvUsuarios.Rows[index].Cells[0].Text);
                CargarUsuario(cod);
            }
            else if (e.CommandName == "Eliminar")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int cod = int.Parse(gvUsuarios.Rows[index].Cells[0].Text);
                try
                {
                    UsuarioDAL.EliminarRegistro(cod);
                    CargarGrid();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        //CARGAR INFORMACION DEL PERFIL DE USUARIO
        private void CargarUsuario(int id_user)
        {
            Usuario usr = UsuarioDAL.ObtenerById(id_user);
            txtNombre.Text = usr.Nombre;
            txtUsuario.Text = usr.User;
            txtPassword.Text = usr.Password;
            txtSalt.Text = usr.Salt;
            txtEmail.Text = usr.Email;
            dblPlanta.SelectedValue = Convert.ToString(usr.Id_planta);
            dblDepartamento.SelectedValue = Convert.ToString(usr.Id_depto);
            int Estado = Convert.ToInt32(usr.Status);
            if (Estado == 1) { rbActivo.SelectedIndex = 1; } else { rbActivo.SelectedIndex = 0; }
        }
        protected void LinkSalir_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}