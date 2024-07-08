using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShippingDisplay.ShippingDisplay.DataAccess.Entidades
{
    public class Registro
    {
        public Registro() { }
        //HYDROFROM INPUT
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

        public string ClienteName { get; set; } //PROJECT NAME
        public string CarrierName { get; set; } //CARRIER NAME
        public string PlantName { get; set; } //PLANT NAME



        //DONT NEED THESE

        public int Shipper { get; set; } //ID SHIPPER
        public int Status { get; set; } //STATUS

        //
        public string Ontime { get; set; }
        public string DELAYED { get; set; }
        public string Completed { get; set; }
        public string Pendiente { get; set; }
        public string TimePeriodAssigned //CONCATENATE FROM TIME AND TO TIME
        {
            get
            {
                return $"{assignedFromtime} - {assignedTotime}";
            }
        }
        
        //HYDROFROM OUTPUT
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

        //                                                               COATINGS STUFF
        //COATINGS INPUTS
        public int Id_all_coatings { get; set; } //MAIN KEY
        public DateTime assignedDate_coatings { get; set; } //ASSIGNED DATE
        public string assignedFromtime_coatings { get; set; } //ASSIGNED FROM TIME
        public string assignedTotime_coatings { get; set; } //ASSIGNED TO TIME
        public string partNumber_coatings { get; set; } //PART NUMBER
        public int Id_cliente_coatings { get; set; } //PROJECT
        public int Id_planta_coatings { get; set; } //ID PLANTS
        public int Id_carrier_coatings { get; set; } //CARRIER
        public int assignedBOL_coatings { get; set; } //BILL OF LADING
        public int assignedQTY_coatings { get; set; } //QUANTITY
        public string assignedDock_coatings { get; set; } //DOCK
        public string shipStatus_coatings { get; set; } //SHIP REASON
        public string shipReason_coatings { get; set; } //SHIP REASON
        public string shipComment_coatings { get; set; } //SHIP COMMENTS
        public string ClienteName_coatings { get; set; } //PROJECT NAME
        public string CarrierName_coatings { get; set; } //CARRIER NAME
        public string PlantName_coatings { get; set; } //PLANT NAME

        public string TimePeriodAssigned_coatings //CONCATENATE FROM TIME AND TO TIME
        {
            get
            {
                return $"{assignedFromtime_coatings} - {assignedTotime_coatings}";
            }
        }

        //COATINGS OUTPUT
        public int Id_all_output_coatings { get; set; }
        public DateTime assignedDate_output_coatings { get; set; }
        public string assignedFromtime_output_coatings { get; set; }
        public string assignedTotime_output_coatings { get; set; }
        public string partNumber_output_coatings { get; set; }
        public int Id_cliente_output_coatings { get; set; }
        public int Id_planta_output_coatings { get; set; }
        public int Id_carrier_output_coatings { get; set; }
        public int assignedBOL_output_coatings { get; set; }
        public int assignedQTY_output_coatings { get; set; }
        public string assignedDock_output_coatings { get; set; }
        public string shipStatus_output_coatings { get; set; }
        public string shipReason_output_coatings { get; set; }
        public string shipComment_output_coatings { get; set; }
        public string ClienteName_output_coatings { get; set; }
        public string CarrierName_output_coatings { get; set; }
        public string RutaName_output_coatings { get; set; }
        public string PlantName_output_coatings { get; set; }

        public string TimePeriodAssigned_output_coatings //CONCATENATE FROM TIME AND TO TIME
        {
            get
            {
                return $"{assignedFromtime_output_coatings} - {assignedTotime_output_coatings}";
            }
        }

        public string Dashboard_dock_plant_input_coatings
        {
            get
            {
                return $"From: {PlantName_coatings}";
            }
        }

        public string Dashboard_dock_plant_output_coatings
        {
            get
            {
                return $"To: {PlantName_coatings}";
            }
        }

        public bool IsInput_coatings { get; set; }






























    }
}