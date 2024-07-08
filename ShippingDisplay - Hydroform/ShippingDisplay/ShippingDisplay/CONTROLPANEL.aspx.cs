using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ShippingDisplay.ShippingDisplay.DataAccess;
using ShippingDisplay.ShippingDisplay.DataAccess.Entidades;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Web.UI.WebControls.WebParts;

namespace ShippingDisplay.ShippingDisplay
{
    public partial class CONTROLPANEL : System.Web.UI.Page
    {
        int Id_Planta;
        protected List<Registro> Dock1Parts { get; set; }
        protected List<Registro> Dock2Parts { get; set; }
        protected List<Registro> Dock3Parts { get; set; }
        protected List<Registro> Dock4Parts { get; set; }
        protected List<Registro> Dock5Parts { get; set; }
        protected List<Registro> Dock6Parts { get; set; }

        protected List<Registro> Dock1Parts_coatings { get; set; }
        protected List<Registro> Dock2Parts_coatings { get; set; }
        protected List<Registro> Dock3Parts_coatings { get; set; }
        protected List<Registro> Dock4Parts_coatings { get; set; }


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string dockName1 = "Dock 1";  // Specify the dock name here
                Dock1Parts = RegistroDAL.control_panel_dock(dockName1);

                string dockName2 = "Dock 2";  // Specify the dock name here
                Dock2Parts = RegistroDAL.control_panel_dock(dockName2);

                string dockName3 = "Dock 3";  // Specify the dock name here
                Dock3Parts = RegistroDAL.control_panel_dock(dockName3);

                string dockName4 = "Dock 4";  // Specify the dock name here
                Dock4Parts = RegistroDAL.control_panel_dock(dockName4);

                string dockName5 = "Dock 5";  // Specify the dock name here
                Dock5Parts = RegistroDAL.control_panel_dock(dockName5);

                string dockName6 = "Dock 6";  // Specify the dock name here
                Dock6Parts = RegistroDAL.control_panel_dock(dockName6);

                string dockName1_coatings = "Dock 1";  // Specify the dock name here
                Dock1Parts_coatings = RegistroDAL.control_panel_dock_coatings(dockName1_coatings);

                string dockName2_coatings = "Dock 2";  // Specify the dock name here
                Dock2Parts_coatings = RegistroDAL.control_panel_dock_coatings(dockName2_coatings);

                string dockName3_coatings = "Dock 3";  // Specify the dock name here
                Dock3Parts_coatings = RegistroDAL.control_panel_dock_coatings(dockName3_coatings);

                string dockName4_coatings = "Dock 4";  // Specify the dock name here
                Dock4Parts_coatings = RegistroDAL.control_panel_dock_coatings(dockName4_coatings);


                if (Context.User.Identity.IsAuthenticated)
                {
                    string Username = HttpContext.Current.User.Identity.Name;
                    //CargarPerfil(Username);
                    //ObtenerWidgets();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "AlertLogin", "window.onload = function(){ alert('Authentication is required. You need to sign into your account'); };", true);
                    Response.Redirect("login.aspx");
                }
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
    }
}

        