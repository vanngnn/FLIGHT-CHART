using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Windows.Forms;
using System.Configuration;
using System.Net;

namespace ShippingDisplay.ShippingDisplay.DataAccess
{
    public class Correo
    {
        public void enviarCorreo(string destinatario, string copia, string Msj, string Msj2, string PlantaC, string CarrierName, string Caja)
        {
            DateTime date = DateTime.Today;
            string Hora = DateTime.Now.ToString("HH:mm:ss");
            string Fecha = DateTime.Now.ToLongDateString();
            string CorreoDestino = destinatario;
            string CorreoDestinoCC = copia;
            if (string.IsNullOrEmpty(CorreoDestinoCC))
            {
                CorreoDestinoCC = "";
            }
            else
            {
                CorreoDestinoCC = "," + CorreoDestinoCC;
            }
            CorreoDestino = CorreoDestino + CorreoDestinoCC;
            SmtpClient smtp = new SmtpClient();
            MailMessage correo = new MailMessage();
            correo.From = new MailAddress(ConfigurationManager.AppSettings["CorreoEmisor"].ToString(), ConfigurationManager.AppSettings["Asunto"].ToString());
            correo.To.Add(CorreoDestino);
            correo.SubjectEncoding = System.Text.Encoding.UTF8;
            correo.Subject = Msj2;
            correo.Body = "<h1><b>[*************UNIT NOTIFICATION************]</b><p></p></span></h1>" +
                "<p><b><h3>" + Msj + "</h3></b><p> " +
                "<p><b><h3>" + PlantaC + "</h3></b><p> " +
                "<p><b>Carrier Line: </b>" + CarrierName + "<p> " +
                "<p><b>Box number: </b>" + Caja + "<p> " +
                 "<p><b>Truk schedule registration: </b>" + Fecha + " " + Hora + "<p> " +
                "<br/>" +
                "<br/>" +
                "<p><b>NOTIFICATION</b></p>" +
                "<p><b><span>" +
                "NOTE:" +
                "This email is just automatic response, please do not reply this email" +
                "</span></b></p>" +
                "<span><p></p></span>";
            correo.BodyEncoding = System.Text.Encoding.UTF8;
            correo.IsBodyHtml = true;
            correo.Priority = MailPriority.High;            
            smtp.Host = ConfigurationManager.AppSettings["Servidor"].ToString();
            smtp.Port = Convert.ToInt16(ConfigurationManager.AppSettings["PuertoCorreo"].ToString());
            smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["CorreoEmisor"].ToString(), ConfigurationManager.AppSettings["PasswordCorreo"].ToString(), ConfigurationManager.AppSettings["DominioCorreo"].ToString());
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(correo);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Oops! Something went wrong.", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}