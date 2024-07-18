using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using ShippingDisplay.ShippingDisplay.DataAccess;
using ShippingDisplay.ShippingDisplay.DataAccess.Entidades;
using ZXing;

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
                    setShipperPlantDropDown();
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "AlertLogin", "window.onload = function(){ alert('Authentication is required. You need to sign into your account'); };", true);
                    Response.Redirect("login.aspx");
                }
            }
        }

        private void setShipperPlantDropDown()
        {
            ShipperPlantDropDown.Items.Insert(0, "-- Select Plant --");
            ShipperPlantDropDown.Items.Insert(1, "Hydroform");
            ShipperPlantDropDown.Items.Insert(2, "Coatings");
        }

        protected void btnRegistrar_Click(object sender, EventArgs e)
        {
            string plantName = Convert.ToString(ShipperPlantDropDown.SelectedValue);
            gvRegistros.DataSource = RegistroDAL.ListadoRegistros_Shipper(plantName);
            gvRegistros.DataBind();
        }

        private void CargarPerfil(string username)
        {
            Usuario perfil = UsuarioDAL.ObtenerUser(username);
            lblNombre.Text = perfil.Nombre;
            Id_Planta = perfil.Id_planta;
            //ACTIVATE TABS ACCORDING TO THE USER LEVEL
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

        protected void LinkSalir_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }

        protected void gvRegistros_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string shipStatus = e.Row.Cells[10].Text;
                if (shipStatus == "On Time")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#28a745");
                }
                else if (shipStatus == "Shipped")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#17a2b8");
                }
                else if (shipStatus == "Delayed")
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#dc3545");
                    e.Row.CssClass = "blink";
                }
                else
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#ffc107");
                }
            }
        }

        protected void gvRegistros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "GenerateQRCode")
            {
                int rowIndex = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvRegistros.Rows[rowIndex];
                string data = row.Cells[3].Text; // Adjust index based on your data

                // Generate QR Code and update modal content
                var qrWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new ZXing.Common.EncodingOptions
                    {
                        Width = 200,
                        Height = 200
                    }
                };
                var qrBitmap = qrWriter.Write(data);

                using (MemoryStream ms = new MemoryStream())
                {
                    qrBitmap.Save(ms, ImageFormat.Png);
                    string base64String = Convert.ToBase64String(ms.ToArray());
                    Image1.ImageUrl = "data:image/png;base64," + base64String;
                    Image1.Visible = true;

                    // Enable download button
                    btnDownloadQRCode.Visible = true;
                    lblMessage.Text = "QR Code generated successfully.";
                    lblMessage.Visible = true;

                    // Save base64 string in hidden field for download
                    hiddenQRCode.Value = base64String;

                    // Show modal popup
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowQRModal", "showModal();", true);
                }
            }
        }

        protected void btnDownloadQRCode_Click(object sender, EventArgs e)
        {
            string base64String = hiddenQRCode.Value;
            byte[] qrCodeBytes = Convert.FromBase64String(base64String);

            Response.Clear();
            Response.ContentType = "image/png";
            Response.AddHeader("Content-Disposition", "attachment; filename=QRCode.png");
            Response.BinaryWrite(qrCodeBytes);
            Response.End();
        }
    }
}
