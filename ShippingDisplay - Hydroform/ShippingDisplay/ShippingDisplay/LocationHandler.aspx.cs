using System;
using System.Diagnostics;
using System.Web.UI.WebControls;

namespace ShippingDisplay.ShippingDisplay
{
    public partial class LocationHandler : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack && Request["__EVENTTARGET"] == "LocationUpdate")
            {
                // Find controls dynamically
                var hdnLatitude = (HiddenField)FindControl("hdnLatitude");
                var hdnLongitude = (HiddenField)FindControl("hdnLongitude");

                if (hdnLatitude != null && hdnLongitude != null)
                {
                    string latitude = hdnLatitude.Value;
                    string longitude = hdnLongitude.Value;

                    Debug.WriteLine("Latitude: " + latitude);
                    Debug.WriteLine("Longitude: " + longitude);

                    // Handle latitude and longitude as needed
                }
                else
                {
                    Debug.WriteLine("Hidden fields not found.");
                }
            }
        }
    }
}
