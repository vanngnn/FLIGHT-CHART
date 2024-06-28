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
            DockDropDown.Items.Insert(4, "Dock 5");
            DockDropDown.Items.Insert(5, "Dock 6");
        }

        // DONE
        protected void btnRegistrar_Click(object sender, EventArgs e) //onclick for register button
        {
            //try
            //{
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
                    Reg.assignedDate_output = Convert.ToDateTime(EntryDate.Text);
                    Reg.assignedFromtime_output = Convert.ToString(fromTime.Text);
                    Reg.assignedTotime_output = Convert.ToString(toTime.Text);
                    Reg.partNumber_output = txtPN.Text;
                    Reg.Id_cliente_output = Convert.ToInt32(dblCliente.SelectedValue);
                    Reg.Id_planta_output = Id_Planta; //FROM/TO: PLANT
                    Reg.Id_carrier_output = Convert.ToInt32(dblCarrier.SelectedValue);
                    Reg.assignedBOL_output = Convert.ToInt32(txtBL.Text);
                    Reg.assignedQTY_output = Convert.ToInt32(txtQTY.Text);
                    Reg.assignedDock_output = DockDropDown.SelectedItem.Text;
                    Reg.shipStatus_output = StatusDropDown.SelectedItem.Text;
                    Reg.shipReason_output = ReasonDropDown.SelectedItem.Text;
                    Reg.shipComment_output = txtComment.Text;

                }
                RegistroDAL.AgregarNuevo_output(Reg);
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
                        Reg.assignedDate_output = Convert.ToDateTime(EntryDate.Text);
                        Reg.assignedFromtime_output = Convert.ToString(fromTime.Text);
                        Reg.assignedTotime_output = Convert.ToString(toTime.Text);
                        Reg.partNumber_output = txtPN.Text;
                        Reg.Id_cliente_output = Convert.ToInt32(dblCliente.SelectedValue);
                        Reg.Id_planta_output = Convert.ToInt32(Id_Planta); //FROM/TO: PLANT
                        Reg.Id_carrier_output = Convert.ToInt32(dblCarrier.SelectedValue);
                        Reg.assignedBOL_output = Convert.ToInt32(txtBL.Text);
                        Reg.assignedQTY_output = Convert.ToInt32(txtQTY.Text);
                        Reg.assignedDock_output = DockDropDown.SelectedItem.Text;
                        Reg.shipStatus_output = StatusDropDown.SelectedItem.Text;
                        Reg.shipReason_output = ReasonDropDown.SelectedItem.Text;
                        Reg.shipComment_output = txtComment.Text;
                    }
                    RegistroDAL.ActualizarRegistro(Reg);
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
            //}
            //catch
            //{
            //   string script = @"<script type='text/javascript'> alert('Oops! Something went wrong.'); </script>";
            //    ScriptManager.RegisterStartupScript(this, typeof(Page), "Alert", script, false);
            //}
        }
        private void CargarGrid()
        {
            int shipStatus = 1;
            gvRegistros.DataSource = RegistroDAL.ListadoRegistros_output(shipStatus);
            gvRegistros.DataBind();
        }

        protected void gvRegistros_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Editar")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int cod = int.Parse(gvRegistros.Rows[index].Cells[0].Text);
                CargarRegistro(cod);
            }
            else if (e.CommandName == "Eliminar")
            {
                int index = int.Parse(e.CommandArgument.ToString());
                int cod = int.Parse(gvRegistros.Rows[index].Cells[0].Text);
                try
                {
                    RegistroDAL.EliminarRegistro(cod);
                    CleanControl(this.Controls);
                    CargarGrid();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        //DONE
        private void CargarRegistro(int Id_all_output)
        {
            Registro Reg = RegistroDAL.ObtenerById(Id_all_output);
            txtId_all.Text = Convert.ToString(Reg.Id_all_output);
            EntryDate.Text = Convert.ToString(Reg.assignedDate_output);
            fromTime.Text = Convert.ToString(Reg.assignedFromtime_output);
            toTime.Text = Convert.ToString(Reg.assignedTotime_output);
            txtPN.Text = Convert.ToString(Reg.partNumber_output);
            dblCliente.SelectedValue = Convert.ToString(Reg.Id_cliente_output); //PROJECT - DROPDOWN
            dblPlant.SelectedValue = Convert.ToString(Reg.Id_planta_output); //ID PLANTS - DROPDOWN
            dblCarrier.SelectedValue = Convert.ToString(Reg.Id_carrier_output); //CARRIER - DROPDOWN
            txtBL.Text = Convert.ToString(Reg.assignedBOL_output);
            txtQTY.Text = Convert.ToString(Reg.assignedQTY_output);
            DockDropDown.SelectedValue = Convert.ToString(Reg.assignedDock_output);
            StatusDropDown.SelectedValue = Convert.ToString(Reg.shipStatus_output);
            ReasonDropDown.SelectedValue = Convert.ToString(Reg.shipReason_output);
            txtComment.Text = Convert.ToString(Reg.shipComment_output);
            // txtPlacas.Text = Convert.ToString(Reg.Placas);
            // txtCaja.Text = Convert.ToString(Reg.Caja);
            // txtOperador.Text = Convert.ToString(Reg.NombreOperador);
            // txtTelefono.Text = Convert.ToString(Reg.Telefono);
            // txtAcceso.Text = Convert.ToString(Reg.Tarjeta);
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