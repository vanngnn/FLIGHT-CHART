using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShippingDisplay.ShippingDisplay.DataAccess.Entidades
{
    public class Usuario
    {
        public Usuario() { }
        public int Id_user { get; set; }
        public string Nombre { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public string Email { get; set; }
        public DateTime Fec_alta { get; set; }
        public DateTime Fec_mod { get; set; }
        public DateTime Ult_sesion { get; set; }
        public int Id_planta { get; set; }
        public int Id_depto { get; set; }
        public bool Activo { get; set; }

        //DATOS ADICIONALES
        public string DesPlanta { get; set; }
        public string DesDepto { get; set; }
        public string Status { get; set; }
    }
}