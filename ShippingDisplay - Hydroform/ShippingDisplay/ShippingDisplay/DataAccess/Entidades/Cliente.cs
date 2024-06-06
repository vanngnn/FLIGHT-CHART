using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShippingDisplay.ShippingDisplay.DataAccess.Entidades
{
    public class Cliente
    {
        public Cliente() { }
        public int Id_cliente { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime Date { get; set; }
        public DateTime Last_mod { get; set; }
        public string Usr_mod { get; set; }
    }
}