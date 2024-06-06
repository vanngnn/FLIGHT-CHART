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
        public int Id_reg { get; set; }
        public int Id_cliente { get; set; }
        public int Id_carrier { get; set; }
        public DateTime Entrada { get; set; }
        public string Salida { get; set; }
        public int Shipper { get; set; }
        public int Id_ruta { get; set; }
        public int Id_planta { get; set; }
        public int Tarjeta { get; set; }
        public int Status { get; set; }
        public string shipStatus { get; set; }
        public string shipComment { get; set; }
        public string partNumber { get; set; }
        public DateTime timeAssigned { get; set; }
        public string sBL { get; set; }
        public int partQuantity { get; set; }
        public string shipReason { get; set; }



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