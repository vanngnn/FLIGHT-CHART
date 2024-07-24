<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ViewRecord.aspx.cs" Inherits="ShippingDisplay.ShippingDisplay.ViewRecord" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="icon" type="image/png" href="Template/img/martinrea_logo.png"/>
    <title>Shipping Display</title>
    <style>
        table {
            width: 100%;
            border-collapse: collapse;
        }
        th, td {
            padding: 8px;
            text-align: left;
            border: 1px solid #ddd;
        }
        th {
            background-color: #d4d4d4; /* Grey color for header background */
            color: black; /* Black text color for header */
        }
        .pickup-button {
            margin-top: 20px; /* Space between buttons and table */
            display: flex;
            justify-content: center;
        }
        .header-container {
            position: relative;
            display: flex;
            align-items: center;
            margin: 40px 0;
        }
        .logo-header {
            display: flex;
            align-items: center;
            position: absolute;
            left: 0;
        }
        .logo-header img {
            width: 150px; /* Adjust the width as needed */
            height: auto;
        }
        .shipment-info {
            flex: 1;
            text-align: center;
            margin-left: 200px; /* Adjust margin to fit your design */
            margin-right: 20px; /* Adjust margin to fit your design */
        }
        .button-group {
            display: flex;
            justify-content: center;
            gap: 20px; /* Space between the buttons */
        }
        .button-group .btn {
            padding: 10px 20px;
            font-size: 16px;
            border: none;
            color: white; /* Button text color */
        }
        .btn-primary {
            background-color: #28a745; /* Green for Pick up button */
        }
        .btn-secondary {
            background-color: #dc3545; /* Red for Decline button (will be removed) */
        }
        .card-body {
            margin-top: 30px; /* Space between the bottom of the logo and the card */
        }
        /* Popup Styles */
        .popup {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            justify-content: center;
            align-items: center;
            z-index: 1000;
        }
        .popup-content {
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            text-align: center;
        }
        .confirm-btn {
            background-color: #28a745;
            color: white;
            padding: 10px 20px;
            border: none;
            cursor: pointer;
        }
        .decline-btn {
            background-color: #dc3545;
            color: white;
            padding: 10px 20px;
            border: none;
            cursor: pointer;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!-- Hidden field for JavaScript access -->
        <asp:HiddenField ID="hdnPartNumber" runat="server" />
        <asp:HiddenField ID="hdnPlantName" runat="server" />
        <asp:HiddenField ID="hdnLatitude" runat="server" />
        <asp:HiddenField ID="hdnLongitude" runat="server" />
        
        <section class="content">
            <div class="container-fluid">
                <!-- Header Container -->
                <div class="header-container">
                    <div class="logo-header">
                        <img src="Template/img/martinrea_logo.png" alt="Martinrea Logo" />
                    </div>
                    <div class="shipment-info">
                        <h1>Shipment Information</h1>
                    </div>
                </div>
                <div class="card card-default">
                    <div class="card card-info">
                        <div class="card-header"></div>
                        <div class="card-body">
                            <div class="form-group">
                                <asp:Label ID="lblRecordDetails" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="form-group pickup-button">
                                <div class="button-group">
                                    <asp:Button ID="btnPickup" runat="server" Text="Pick up" CssClass="btn btn-primary" OnClientClick="showPopup(); return false;" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </form>

    <!-- Pop up Notification -->
    <div id="popup" class="popup">
        <div class="popup-content">
            <p id="popupMessage">Pick up part#: <span id="partNumber"></span><br />Ship to: <span id="plantName"></span></p>
            <button class="confirm-btn" onclick="confirmAction()">Confirm</button>
            <button class="decline-btn" onclick="declineAction()">Decline</button>
        </div>
    </div>

    <script>
        function showPopup()
        {
            // Get values from hidden fields
            var partNumber = document.getElementById('<%= hdnPartNumber.ClientID %>').value;
            var plantName = document.getElementById('<%= hdnPlantName.ClientID %>').value;

            // Set the text for the popup message
            document.getElementById('partNumber').textContent = partNumber;
            document.getElementById('plantName').textContent = plantName;

            // Show popup
            document.getElementById('popup').style.display = 'flex';
        }

        function confirmAction()
        {
            document.getElementById('popup').style.display = 'none';
            if (navigator.geolocation)
            {
                navigator.geolocation.getCurrentPosition(
                    function (position)
                    {
                        var latitude = position.coords.latitude;
                        var longitude = position.coords.longitude;

                        // Set latitude and longitude in hidden fields
                        document.getElementById('<%= hdnLatitude.ClientID %>').value = latitude;
                        document.getElementById('<%= hdnLongitude.ClientID %>').value = longitude;
                    
                        // Trigger postback to server
                        __doPostBack('LocationUpdate', '');
                    },
                    function (error)
                    {
                        console.error('Error getting location:', error.message);
                    }
                );

            } else {
                console.log('Geolocation is not supported by this browser.');
            }
        }

        function declineAction() {
            document.getElementById('popup').style.display = 'none';
            // You can add additional actions here if needed
        }
    </script>
</body>
</html>
