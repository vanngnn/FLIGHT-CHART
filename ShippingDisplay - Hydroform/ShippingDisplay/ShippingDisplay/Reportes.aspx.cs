using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ShippingDisplay.ShippingDisplay.DataAccess;
using ShippingDisplay.ShippingDisplay.DataAccess.Entidades;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace ShippingDisplay.ShippingDisplay
{
    public partial class Reportes : System.Web.UI.Page
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
                    CargarPlanta();
                    ReportShipmentDropDown();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "AlertLogin", "window.onload = function(){ alert('Authentication is required. You need to sign into your account'); };", true);
                    Response.Redirect("login.aspx");
                }
            }
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
                LinkDashEmb.Visible = false;
            }
            else if (Dept == 2)
            {
                LinkShipper.Visible = false;
                LinkRegister.Visible = false;
                LinkDashEmb.Visible = false;
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
        protected void LinkSalir_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            //EXTRAER FILTROS SELECCIONADOS 
            //EXTRAER VARIABLE DE INICIO DE SESION 
            string ReservFecha = reservation.Text;
            string Fec_ini = ReservFecha.Substring(6, 4) + "-" + ReservFecha.Substring(0, 2) + "-" + ReservFecha.Substring(3, 2);
            string Fec_fin = ReservFecha.Substring(19, 4) + "-" + ReservFecha.Substring(13, 2) + "-" + ReservFecha.Substring(16, 2);
            int Id_plant = Convert.ToInt32(dblPlanta.SelectedValue);
            gvRegistros.DataSource = RegistroDAL.FiltroReporte(Id_plant, Fec_ini, Fec_fin);
            gvRegistros.DataBind();
        }
        private void ExportToExcel(string nameReport, GridView wControl)
        {
            HttpResponse response = Response;
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            Page pageToRender = new Page();
            HtmlForm form = new HtmlForm();
            form.Controls.Add(wControl);
            pageToRender.Controls.Add(form);
            response.Clear();
            response.Buffer = true;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", "attachment;filename=" + nameReport);
            response.Charset = "UTF-8";
            response.ContentEncoding = Encoding.Default;
            pageToRender.RenderControl(htw);
            response.Write(sw.ToString());
            response.End();
        }

        private void ReportShipmentDropDown()
        {
            ReportFilterDropDown.Items.Insert(0,"All");
            ReportFilterDropDown.Items.Insert(1,"Inputs");
            ReportFilterDropDown.Items.Insert(2,"Outputs");
        }

        protected void btnExporta_Click(object sender, EventArgs e)
        {
            ExportToExcel("Informe.xls", gvRegistros);
        }
    }
}
