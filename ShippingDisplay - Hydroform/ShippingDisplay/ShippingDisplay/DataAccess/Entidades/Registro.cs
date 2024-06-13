using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShippingDisplay.ShippingDisplay.DataAccess.Entidades
{
    public class Registro
    {
        public Registro() { }
        //HEADER
        public int Id_reg { get; set; } //MAIN KEY
        public int Id_cliente { get; set; } //PROJECT
        public int Id_carrier { get; set; } //CARRIER
        public DateTime Entrada { get; set; } //THE DATE WHEN U ENTER THE SHIPMENT (not needed)
        public string Salida { get; set; } //THE DATE WHEN U OUTPUT THE SHIPMENT (not needed)
        public int Shipper { get; set; } //ID SHIPPER
        public int Id_ruta { get; set; } //ID ROUTE
        public int Id_planta { get; set; } //ID PLANTS
        public int Tarjeta { get; set; } //SHIPPER ACCESS CARD
        public int Status { get; set; } //STATUS
        public int 

        //DETALLE
        public int Id_det { get; set; }
        public string Placas { get; set; }
        public string Caja { get; set; }
        public string NombreOperador { get; set; }
        public string Telefono { get; set; }

        //ADICIONALES 

        public string ClienteName { get; set; }
        public string CarrierName { get; set; }
        public string RutaName { get; set; }
        //
        public string Ontime { get; set; }
        public string DELAYED { get; set; }
        public string Completed { get; set; }
        public string Pendiente { get; set; }
        public string Estado { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
    }
}