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
    public partial class RegistroSalida_coatings : System.Web.UI.Page
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
                    CargarCarrier();
                    CargarCliente();
                    CargarGrid();
                    setStatusDropdown();
                    setReasonDropDown();
                    setDockDropDown();
                    setPlantsDropDown();
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
        private void CargarCarrier()
        {
            dblCarrier.DataTextField = "description";
            dblCarrier.DataValueField = "id_carrier";
            dblCarrier.DataSource = CarrierDAL.ObtenerCarrier();
            dblCarrier.DataBind();
            dblCarrier.Items.Insert(0, " - Carrier - ");
        }

        private void setPlantsDropDown()
        {
            dblPlant.DataTextField = "description";
            dblPlant.DataValueField = "id_planta";
            dblPlant.DataSource = PlantaDAL.ObtenerPlantas();
            dblPlant.DataBind();
            dblPlant.Items.Insert(0, " - Plant - ");
        }
        private void CargarCliente()
        {
            dblCliente.DataTextField = "description";
            dblCliente.DataValueField = "id_cliente";
            dblCliente.DataSource = ClienteDAL.ObtenerClientes();
            dblCliente.DataBind();
            dblCliente.Items.Insert(0, " - Customer - ");
        }
        private void setStatusDropdown()
        {
            StatusDropDown.Items.Insert(0, "On Time");
            StatusDropDown.Items.Insert(1, "Delayed");
            StatusDropDown.Items.Insert(2, "Without Shipper");
            StatusDropDown.Items.Insert(3, "Shipped");
        }
        private void setReasonDropDown()
        {
            ReasonDropDown.Items.Insert(0, "On the Way");
            ReasonDropDown.Items.Insert(1, "Ready to Ship");
            ReasonDropDown.Items.Insert(2, "Arrived on Time");
            ReasonDropDown.Items.Insert(3, "Waiting for Loading");
            ReasonDropDown.Items.Insert(4, "Waiting for Production");
            ReasonDropDown.Items.Insert(5, "Waiting for Carrier");
            ReasonDropDown.Items.Insert(6, "Delayed to Arrive");
            ReasonDropDown.Items.Insert(7, "Other (Please comment):");
        }
        private void setDockDropDown()
        {
            DockDropDown.Items.Insert(0, "Dock 1");
            DockDropDown.Items.Insert(1, "Dock 2");
            DockDropDown.Items.Insert(2, "Dock 3");
            DockDropDown.Items.Insert(3, "Dock 4");

        }

        // DONE
        protected void btnRegistrar_Click(object sender, EventArgs e) //onclick for register button
        {
            try
            {
            string Username = HttpContext.Current.User.Identity.Name;
            CargarPerfil(Username);
            string ID;
            ID = txtId_all.Text;
            Id_Planta = dblPlant.SelectedIndex;
            if (ID == "")
            {
                Registro Reg = new Registro();
                {
                    Reg.Status = 1;
                    Reg.assignedDate_output_coatings = Convert.ToDateTime(EntryDate.Text);
                    Reg.assignedFromtime_output_coatings = Convert.ToString(fromTime.Text);
                    Reg.assignedTotime_output_coatings = Convert.ToString(toTime.Text);
                    Reg.partNumber_output_coatings = txtPN.Text;
                    Reg.Id_cliente_output_coatings = Convert.ToInt32(dblCliente.SelectedValue);
                    Reg.Id_planta_output_coatings = Id_Planta; //FROM/TO: PLANT
                    Reg.Id_carrier_output_coatings = Convert.ToInt32(dblCarrier.SelectedValue);
                    Reg.assignedBOL_output_coatings = Convert.ToInt32(txtBL.Text);
                    Reg.assignedQTY_output_coatings = Convert.ToInt32(txtQTY.Text);
                    Reg.assignedDock_output_coatings = DockDropDown.SelectedItem.Text;
                    Reg.shipStatus_output_coatings = StatusDropDown.SelectedItem.Text;
                    Reg.shipReason_output_coatings = ReasonDropDown.SelectedItem.Text;
                    Reg.shipComment_output_coatings = txtComment.Text;

                }
                RegistroDAL.AgregarNuevo_output_coatings(Reg);
                try
                {
                    string PlantaCorrepondiente = "Plant " + Id_Planta;
                    string Msj = "SHIPPING DISPLAY INPUT";
                    //c.enviarCorreo("van.nguyen@martinrea.com", "Ali.Akhoondzadeh@martinrea.com", "Transport input", Msj, PlantaCorrepondiente, Carrier, Caja);

                    string script = @"<script type='text/javascript'> alert('Successfully sent data'); </script>";
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
            else
            {
                try
                {
                    //DEFINICIÓN DE VARIABLES LOCALES
                    Registro Reg = new Registro();
                    {
                        Reg.assignedDate_output_coatings = Convert.ToDateTime(EntryDate.Text);
                        Reg.assignedFromtime_output_coatings = Convert.ToString(fromTime.Text);
                        Reg.assignedTotime_output_coatings = Convert.ToString(toTime.Text);
                        Reg.partNumber_output_coatings = txtPN.Text;
                        Reg.Id_cliente_output_coatings = Convert.ToInt32(dblCliente.SelectedValue);
                        Reg.Id_planta_output_coatings = Convert.ToInt32(Id_Planta); //FROM/TO: PLANT
                        Reg.Id_carrier_output_coatings = Convert.ToInt32(dblCarrier.SelectedValue);
                        Reg.assignedBOL_output_coatings = Convert.ToInt32(txtBL.Text);
                        Reg.assignedQTY_output_coatings = Convert.ToInt32(txtQTY.Text);
                        Reg.assignedDock_output_coatings = DockDropDown.SelectedItem.Text;
                        Reg.shipStatus_output_coatings = StatusDropDown.SelectedItem.Text;
                        Reg.shipReason_output_coatings = ReasonDropDown.SelectedItem.Text;
                        Reg.shipComment_output_coatings = txtComment.Text;
                        Reg.Id_all_output_coatings = Convert.ToInt32(txtId_all.Text);
                    }
                    RegistroDAL.ActualizarRegistro_output_coatings(Reg);
                    CleanControl(this.Controls);
                    CargarGrid();
                    string script = @"<script type='text/javascript'> alert('Updated successfully'); </script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", script, false);
                }
                catch
                {
                    string script = @"<script type='text/javascript'> alert('Error sending email notification'); </script>";
                    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", script, false);
                }
            }
            }
            catch
            {
               string script = @"<script type='text/javascript'> alert('Oops! Something went wrong.'); </script>";
                ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", script, false);
            }
        }
        private void CargarGrid()
        {
            int shipStatus = 1;
            gvRegistros.DataSource = RegistroDAL.ListadoRegistros_output_coatings(shipStatus);
            gvRegistros.DataBind();
        }

        protected void gvRegistros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar" || e.CommandName == "Eliminar")
            {
                // Get the row index from CommandArgument
                int index = Convert.ToInt32(e.CommandArgument);

                // Get the Id_all value from DataKeys
                int cod = Convert.ToInt32(gvRegistros.DataKeys[index].Value);

                if (e.CommandName == "Editar")
                {
                    CargarRegistro_output_coatings(cod);
                }
                else if (e.CommandName == "Eliminar")
                {
                    try
                    {
                        RegistroDAL.EliminarRegistro_output_coatings(cod);
                        CleanControl(this.Controls);
                        CargarGrid();
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception (e.g., log it, show a message to the user, etc.)
                        throw;
                    }
                }
            }
        }

        //DONE
        private void CargarRegistro_output_coatings(int Id_all_output_coatings)
        {
            Registro Reg = RegistroDAL.ObtenerById_output_coatings(Id_all_output_coatings);
            txtId_all.Text = Convert.ToString(Reg.Id_all_output_coatings);
            EntryDate.Text = Reg.assignedDate_output_coatings.ToString("yyyy-MM-dd");
            fromTime.Text = Convert.ToString(Reg.assignedFromtime_output_coatings);
            toTime.Text = Convert.ToString(Reg.assignedTotime_output_coatings);
            txtPN.Text = Convert.ToString(Reg.partNumber_output_coatings);
            dblCliente.SelectedValue = Convert.ToString(Reg.Id_cliente_output_coatings); //PROJECT - DROPDOWN
            dblPlant.SelectedValue = Convert.ToString(Reg.Id_planta_output_coatings); //ID PLANTS - DROPDOWN
            dblCarrier.SelectedValue = Convert.ToString(Reg.Id_carrier_output_coatings); //CARRIER - DROPDOWN
            txtBL.Text = Convert.ToString(Reg.assignedBOL_output_coatings);
            txtQTY.Text = Convert.ToString(Reg.assignedQTY_output_coatings);
            DockDropDown.SelectedValue = Convert.ToString(Reg.assignedDock_output_coatings);
            StatusDropDown.SelectedValue = Convert.ToString(Reg.shipStatus_output_coatings);
            ReasonDropDown.SelectedValue = Convert.ToString(Reg.shipReason_output_coatings);
            txtComment.Text = Convert.ToString(Reg.shipComment_output_coatings);
        }

        //DONT KNOW BUT SEEMS DONE
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
        protected void LinkSalir_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            FormsAuthentication.RedirectToLoginPage();
        }
    }
}