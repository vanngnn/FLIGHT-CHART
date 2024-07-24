using ShippingDisplay.ShippingDisplay.DataAccess;
using System;
using System.Diagnostics;

namespace ShippingDisplay.ShippingDisplay
{
    public partial class ViewRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string partNumber = Request.QueryString["partNumber"];
            string plantName = Request.QueryString["plantName"];

            Debug.WriteLine($"partNumber from QueryString: {partNumber}");
            Debug.WriteLine($"plantName from QueryString: {plantName}");

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(partNumber))
                {
                    var record = RegistroDAL.GetRecordByPartNumber(partNumber, plantName);
                    if (record != null)
                    {
                        lblRecordDetails.Text = $@"
                            <table>
                                <tr>
                                    <th>Field</th>
                                    <th>Details</th>
                                </tr>
                                <tr>
                                    <td>DATE</td>
                                    <td>{record.assignedDate_output}</td>
                                </tr>
                                <tr>
                                    <td>ASSIGNED TIME</td>
                                    <td>{record.assignedFromtime_output} to {record.assignedTotime_output}</td>
                                </tr>
                                <tr>
                                    <td>P/N</td>
                                    <td>{record.partNumber_output}</td>
                                </tr>
                                <tr>
                                    <td>PROJECT</td>
                                    <td>{record.ClienteName_output}</td>
                                </tr>
                                <tr>
                                    <td>TO</td>
                                    <td>{record.PlantName_output}</td>
                                </tr>
                                <tr>
                                    <td>CARRIER</td>
                                    <td>{record.CarrierName_output}</td>
                                </tr>
                                <tr>
                                    <td>B/L</td>
                                    <td>{record.assignedBOL_output}</td>
                                </tr>
                                <tr>
                                    <td>QTY</td>
                                    <td>{record.assignedQTY_output}</td>
                                </tr>
                                <tr>
                                    <td>DOCK</td>
                                    <td>{record.assignedDock_output}</td>
                                </tr>
                                <tr>
                                    <td>STATUS</td>
                                    <td>{record.shipStatus_output}</td>
                                </tr>
                                <tr>
                                    <td>REASON</td>
                                    <td>{record.shipReason_output}</td>
                                </tr>
                                <tr>
                                    <td>COMMENT</td>
                                    <td>{record.shipComment_output}</td>
                                </tr>
                            </table>";

                        hdnPartNumber.Value = record.partNumber_output;
                        hdnPlantName.Value = record.PlantName_output;
                    }
                    else
                    {
                        lblRecordDetails.Text = "Record not found.";
                    }
                }
                else
                {
                    lblRecordDetails.Text = "No part number provided.";
                }
            }
            else if (IsPostBack && Request["__EVENTTARGET"] == "LocationUpdate")
            {
                string latitude = hdnLatitude.Value;
                string longitude = hdnLongitude.Value;

                Debug.WriteLine("Latitude: " + latitude);
                Debug.WriteLine("Longitude: " + longitude);

                // Optionally, you can process the latitude and longitude further here
            }
        }
    }
}