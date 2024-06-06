using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShippingDisplay.ShippingDisplay.DataAccess.Entidades
{
    public class Ruta
    {
        public Ruta() { }
        public int Id_ruta { get; set; }
        public string Description { get; set; }
        public string Dia { get; set; }
        public DateTime Input { get; set; }
        public DateTime Output { get; set;}
        public int  Id_planta { get; set; }
        public Boolean Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime Last_mod { get; set; }
        public string Usr_mod { get; set; }
    }
}