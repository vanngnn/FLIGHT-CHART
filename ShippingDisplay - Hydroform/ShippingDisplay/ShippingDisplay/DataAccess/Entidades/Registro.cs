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
        public int Id_all { get; set; } //MAIN KEY
        public DateTime assignedDate { get; set; } //ASSIGNED DATE
        public string assignedFromtime { get; set; } //ASSIGNED FROM TIME
        public string assignedTotime { get; set; } //ASSIGNED TO TIME
        public string partNumber { get; set; } //PART NUMBER
        public int Id_cliente { get; set; } //PROJECT
        public int Id_planta { get; set; } //ID PLANTS
        public int Id_carrier { get; set; } //CARRIER
        public int assignedBOL { get; set; } //BILL OF LADING
        public int assignedQTY { get; set; } //QUANTITY
        public string assignedDock { get; set; } //DOCK
        public string shipStatus { get; set; } //SHIP REASON
        public string shipReason { get; set; } //SHIP REASON
        public string shipComment { get; set; } //SHIP COMMENTS

        //ADICIONALES 

        public string ClienteName { get; set; } //PROJECT NAME
        public string CarrierName { get; set; } //CARRIER NAME
        public string RutaName { get; set; } //ROUTE NAME (DONT NEED THIS)

        public string PlantName { get; set; } //PLANT NAME



        //DONT NEED THESE

        public DateTime Entrada { get; set; } //THE DATE WHEN U ENTER THE SHIPMENT (not needed)
        public string Salida { get; set; } //THE DATE WHEN U OUTPUT THE SHIPMENT (not needed)
        public int Shipper { get; set; } //ID SHIPPER

        public int Tarjeta { get; set; } //SHIPPER ACCESS CARD
        public int Status { get; set; } //STATUS


        //DETALLE
        public int Id_det { get; set; }
        public string Placas { get; set; }
        public string Caja { get; set; }
        public string NombreOperador { get; set; }
        public string Telefono { get; set; }

        //
        public string Ontime { get; set; }
        public string DELAYED { get; set; }
        public string Completed { get; set; }
        public string Pendiente { get; set; }
        public string Estado { get; set; }
        public string Input { get; set; }
        public string Output { get; set; }
        public string TimePeriodAssigned //CONCATENATE FROM TIME AND TO TIME
        {
            get
            {
                return $"{assignedFromtime} - {assignedTotime}";
            }
        }

        public int Id_all_output  { get; set; }
        public DateTime assignedDate_output { get; set; }
        public string assignedFromtime_output { get; set; }
        public string assignedTotime_output { get; set; }
        public string partNumber_output { get; set; }
        public int Id_cliente_output { get; set; }
        public int Id_planta_output { get; set; }
        public int Id_carrier_output { get; set; }
        public int assignedBOL_output { get; set; }
        public int assignedQTY_output { get; set; }
        public string assignedDock_output { get; set; }
        public string shipStatus_output { get; set; }
        public string shipReason_output { get; set; }
        public string shipComment_output { get; set; }
        public string ClienteName_output { get; set; }
        public string CarrierName_output { get; set; }
        public string RutaName_output { get; set; }
        public string PlantName_output { get; set; }

        public string TimePeriodAssigned_output //CONCATENATE FROM TIME AND TO TIME
        {
            get
            {
                return $"{assignedFromtime_output} - {assignedTotime_output}";
            }
        }

        public string Dashboard_dock_plant_input
        {
            get
            {
                return $"From: {PlantName}";
            }
        }

        public string Dashboard_dock_plant_output
        {
            get
            {
                return $"To: {PlantName}";
            }
        }

        public bool IsInput { get; set; }

    }
}