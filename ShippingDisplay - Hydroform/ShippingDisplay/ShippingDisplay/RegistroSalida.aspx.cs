using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ShippingDisplay.ShippingDisplay.DataAccess;
using ShippingDisplay.ShippingDisplay.DataAccess.Entidades;

namespace ShippingDisplay.ShippingDisplay
{
    public partial class RegistroSalida : System.Web.UI.Page
    {
        Correo c = new Correo();
        int Id_Planta;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                if (Context.User.Identity.IsAuthenticated)
                {
                    string Username = HttpContext.Current.User.Identity.Name;
                    CargarPerfil(Username);
                    CargarGrid();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "AlertLogin", "window.onload = function(){ alert('Authentication is required. You need to sign into your account'); };", true);
                    Response.Redirect("login.aspx");
                }
            }
        }
        private void CargarGrid()
        {
            int Estatus = 2;
            gvRegistros.DataSource = RegistroDAL.ListadoRegistros(Estatus, Id_Planta);
            gvRegistros.DataBind();
        }
        private void CargarPerfil(string username)
        {
            Usuario perfil = UsuarioDAL.ObtenerUser(username);
            lblNombre.Text = perfil.Nombre;
            Id_Planta = perfil.Id_planta;
            //ACTIVAR PESTAÑAS DE ACUERDO AL NIVEL DE USUARIO
            int Dept = Convert.ToInt32(perfil.Id_depto);
            //if (Dept == 1)
            //{
            //    LinkConfig.Visible = false;
            //    LinkRegEntry.Visible = false;
            //    LinkRegOut.Visible = false;
            //    LinkRegister.Visible = false;
            //    LinkDashEmb.Visible = false;
            //}
            //else if (Dept == 2)
            //{
            //    LinkShipper.Visible = false;
            //    LinkRegister.Visible = false;
            //    LinkDashEmb.Visible = false;
            //}
        }
        protected void gvRegistros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Salida")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int cod = int.Parse(gvRegistros.Rows[index].Cells[0].Text);
                CargarRegistro(cod);
            }
        }
        private void CargarRegistro(int Id_all)
        {
            Registro Reg = RegistroDAL.ObtenerById(Id_all);
            //txtId_all.Text = Convert.ToString(Reg.Id_all);
            //txtAcceso.Text = Convert.ToString(Reg.Tarjeta);
        }
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            //if (txtAcceso.Text == "")
            //{
            //    string script = @"<script type='text/javascript'> alert('Selects the card for output'); </script>";
            //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", script, false);
            //    return;
            //}
            
            //int Id_all = Convert.ToInt32(txtId_all.Text);
            //Registro Info = RegistroDAL.ObtenerById(Id_all);
            //string Caja = Info.Caja;
            //string CarrierName = Info.CarrierName;

            Registro Reg = new Registro();
            {
                //Reg.Id_all = Id_all;
               // Reg.Tarjeta = Convert.ToInt32(txtAcceso.Text);
                Reg.Status = 3;
            }
            RegistroDAL.ActualizarSalida(Reg);
            try
            {
                string Username = HttpContext.Current.User.Identity.Name;
                CargarPerfil(Username);

                string PlantaCorrepondiente = "Plant " + Id_Planta;
                string Msj = "SHIPPING DISPLAY OUTPUT";
                //c.enviarCorreo("van.nguyen@martinrea.com", "Ali.Akhoondzadeh@martinrea.com", "Transport output", Msj, PlantaCorrepondiente, CarrierName, Caja);
                string script = @"<script type='text/javascript'> alert('Notification sent successfully'); </script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", script, false);
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('Error sending e-mail notification'); </script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", script, false);
            }
            CleanControl(this.Controls);
            CargarGrid();
        }
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
            //txtId_all.Text = "";
        }
        protected void LinkSalir_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
        protected void txtAcceso_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //int tarjeta = Convert.ToInt32(txtAcceso.Text);
                //CargarRegistro(tarjeta);
            }
            catch
            {
                string script = @"<script type='text/javascript'> alert('Access card not found'); </script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "alert", script, false);
            }
        }
    }
}