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
    public partial class Shipper : System.Web.UI.Page
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
                    CargarRuta();
                    //CargarGrid();
                    TruckLocation();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "AlertLogin", "window.onload = function(){ alert('Authentication is required. You need to sign into your account'); };", true);
                    Response.Redirect("login.aspx");
                }
            }
        }
        //private void CargarGrid()
        //{
          //  int Estatus = 1;
           // gvRegistros.DataSource = RegistroDAL.ListadoRegistros(Estatus, Id_Planta);
           // gvRegistros.DataBind();
        //}
        private void CargarRuta()
        {
            dblRuta.DataTextField = "description";
            dblRuta.DataSource = RutaDAL.ObtenerRutas(Id_Planta);
            dblRuta.DataBind();
            dblRuta.Items.Insert(0, " - Carrier - ");
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
        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            Registro Reg = new Registro();
            {
                Reg.Id_all = Convert.ToInt32(txtId_all.Text);
                Reg.Shipper = Convert.ToInt32(txtShipper.Text);
                Reg.Status = 2;
            }
            RegistroDAL.ActualizarShipper(Reg);
            CleanControl(this.Controls);
            //CargarGrid();
        }
        private void CargarRegistro(int Id_all)
        {
            Registro Reg = RegistroDAL.ObtenerById(Id_all);
            txtId_all.Text = Convert.ToString(Reg.Id_all);

        }

        private void TruckLocation()
        {
            //txtTrackedLocation.DataTextField="location";
        }



        protected void gvRegistros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int cod = int.Parse(gvRegistros.Rows[index].Cells[0].Text);
                CargarRegistro(cod);
            }
        }
        protected void LinkSalir_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
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
            txtId_all.Text = "";
        }
    }
}