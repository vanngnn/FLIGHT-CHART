<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LocationHandler.aspx.cs" Inherits="ShippingDisplay.ShippingDisplay.LocationHandler" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Location Handler</title>
    <script type="text/javascript">
        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError);
            } else {
                console.log("Geolocation is not supported by this browser.");
            }
        }

        function showPosition(position) {
            var latitude = position.coords.latitude;
            var longitude = position.coords.longitude;

            document.getElementById('<%= hdnLatitude.ClientID %>').value = latitude;
            document.getElementById('<%= hdnLongitude.ClientID %>').value = longitude;

            __doPostBack('LocationUpdate', '');
        }

        function showError(error) {
            switch (error.code) {
                case error.PERMISSION_DENIED:
                    console.log("User denied the request for Geolocation.");
                    break;
                case error.POSITION_UNAVAILABLE:
                    console.log("Location information is unavailable.");
                    break;
                case error.TIMEOUT:
                    console.log("The request to get user location timed out.");
                    break;
                case error.UNKNOWN_ERROR:
                    console.log("An unknown error occurred.");
                    break;
            }
        }

        function confirmAction() {
            getLocation();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnLatitude" runat="server" />
        <asp:HiddenField ID="hdnLongitude" runat="server" />
        
        <button type="button" onclick="confirmAction()">Confirm</button>
    </form>
</body>
</html>
