using System;
using System.Diagnostics;
using ShippingDisplay.ShippingDisplay.DataAccess;

namespace ShippingDisplay.ShippingDisplay
{
    public partial class ViewRecord : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string partNumber = Request.QueryString["partNumber"];



            Debug.WriteLine($"partNumber from QueryString: {partNumber}");

            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(partNumber))
                {
                    // Fetch the data based on the part number
                    var record = RegistroDAL.GetRecordByPartNumber(partNumber);
                    if (record != null)
                    {
                        // Display record details including Part_number_output
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

                            //Set values for hidden fields
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
        }
    }
}
