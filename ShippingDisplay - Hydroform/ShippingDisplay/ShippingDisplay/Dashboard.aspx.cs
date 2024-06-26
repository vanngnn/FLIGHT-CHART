﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ShippingDisplay.ShippingDisplay.DataAccess;
using ShippingDisplay.ShippingDisplay.DataAccess.Entidades;
using System.Drawing;

namespace ShippingDisplay.ShippingDisplay
{
    public partial class Dashboard : System.Web.UI.Page
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
                    ObtenerWidgets();
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
            gvRegistros.DataSource = RegistroDAL.ListadoDashboard(Id_Planta);
            gvRegistros.DataBind();
        }
        public void ObtenerWidgets()
        {
            try
            {
                string username = HttpContext.Current.User.Identity.Name;
                Usuario perfil = UsuarioDAL.ObtenerUser(username);
                Id_Planta = perfil.Id_planta;

                Registro Reg = RegistroDAL.ObtenerRegistros(Id_Planta);

                if(Reg ==null)
                {
                    lblEntiempo.Text = "0";
                    lblAtrasado.Text = "0";
                    lblSinShipper.Text = "0";
                    lblEnviado.Text = "0";
                }
                else
                {
                    lblEntiempo.Text = Convert.ToString(Reg.Ontime);
                    lblAtrasado.Text = Convert.ToString(Reg.DELAYED);
                    lblSinShipper.Text = Convert.ToString(Reg.Pendiente);
                    lblEnviado.Text = Convert.ToString(Reg.Completed);
                }
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "Alert", "window.onload = function(){ alert('Oops! Something went wrong.'); };", true);
            }
        }

        private bool IsNull(Registro Ontime)
        {
            throw new NotImplementedException();
        }

        protected void LinkSalir_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
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
                LinkDashEmb.Visible = false;
            }
            else if (Dept == 2)
            {
                LinkShipper.Visible = false;
                LinkRegister.Visible = false;
                LinkDashEmb.Visible = false;
            }
        }

        protected void gvRegistros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string Estado = e.Row.Cells[8].Text;
                if (Estado == "ONTIME")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#28a745");
                }
                else if (Estado == "SENT")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#17a2b8");
                }
                else if (Estado == "DELAYED")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#dc3545");
                }
                else
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffc107");
                }
            }
        }
    }
}