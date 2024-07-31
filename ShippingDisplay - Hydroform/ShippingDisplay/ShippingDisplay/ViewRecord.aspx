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
            display: flex;
            align-items: center;
            margin: 40px 0;
            position: relative;
            justify-content: space-between; /* Space out items */
        }
        .logo-header {
            display: flex;
            align-items: center;
        }
        .logo-header img {
            width: 150px; /* Adjust the width as needed */
            height: auto;
        }
        .shipment-info {
            text-align: center;
            flex: 1;
        }
        .status-label {
            display: none;
            background-color: #17a2b8; /* Blue background for status */
            color: white;
            padding: 10px 20px;
            border-radius: 5px;
            font-size: 16px;
            text-align: center;
            margin-left: 20px; /* Space between the label and the shipment info */
        }
        .location-display {
            display: flex;
            align-items: center;
            background-color: white;
            padding: 8px 12px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
            margin-left: 20px; /* Space between the label and the shipment info */
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

        #map { height: 400px; width: 100%; }
        <script src="https://maps.googleapis.com/maps/api/js?key=AIzaSyBxyg6peFAo07Q5iosf958WzKewd-3VYnU&libraries=geometry"></script>

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
                    <div id="locationDisplay" class="location-display">
                        Latitude: <span id="latitudeDisplay"></span> | Longitude: <span id="longitudeDisplay"></span>
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

    <!-- Status Label -->
    <div id="statusLabel" class="status-label">In Progress</div>

    <script>
        function showPopup() {
            // Get values from hidden fields
            var partNumber = document.getElementById('<%= hdnPartNumber.ClientID %>').value;
            var plantName = document.getElementById('<%= hdnPlantName.ClientID %>').value;

            // Set the text for the popup message
            document.getElementById('partNumber').textContent = partNumber;
            document.getElementById('plantName').textContent = plantName;

            // Show popup
            document.getElementById('popup').style.display = 'flex';
        }

        function sendLocationToServer(latitude, longitude) {
            var xhr = new XMLHttpRequest();
            xhr.open("POST", "ViewRecord.aspx/UpdateLocation", true);
            xhr.setRequestHeader("Content-Type", "application/json; charset=utf-8");
            xhr.setRequestHeader("Accept", "application/json");

            var data = JSON.stringify({
                latitude: latitude,
                longitude: longitude
            });

            xhr.send(data);
        }



        function confirmAction() {
            document.getElementById('popup').style.display = 'none';
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(
                    function (position) {
                        var latitude = position.coords.latitude;
                        var longitude = position.coords.longitude;

                        //Display latitude and longitude
                        document.getElementById('latitudeDisplay').textContent = latitude;
                        document.getElementById('longitudeDisplay').textContent = longitude;

                        // Set latitude and longitude in hidden fields
                        document.getElementById('<%= hdnLatitude.ClientID %>').value = latitude;
                        document.getElementById('<%= hdnLongitude.ClientID %>').value = longitude;

                        // Hide the "Pick up" button
                        var pickupButton = document.getElementById('<%= btnPickup.ClientID %>');
                        pickupButton.style.display = 'none';

                        // Show the status label
                        var statusLabel = document.getElementById('statusLabel');
                        statusLabel.style.display = 'block';

                        // Call the web method to update the shipment status
                        var shipmentId = parseInt(document.getElementById('<%= hdnPartNumber.ClientID%>').value); //Make sure this is the one that determines the shipment that we want to update
                        updateShipmentStatus(shipmentId);
                    },
                    function (error) {
                        console.error('Error getting location:', error.message);
                    }
                );

            } else {
                console.log('Geolocation is not supported by this browser.');
            }
        }

        function updateShipmentStatus(shipmentId) {
                $.ajax({
                type: 'POST',
                url: 'ViewRecord.aspx/UpdateShipmentStatus',
                data: JSON.stringify({ shipmentId: shipmentId }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    console.log('Status updated successfully.');
                    window.location.href = "Shipper.aspx";
                },
                error: function (error) {
                    console.error('Error updating shipment status:', error);
                }
            });
        
        }


        function declineAction() {
            document.getElementById('popup').style.display = 'none';
            // You can add additional actions here if needed
        }

        let map, userMarker, fromMarker, toMarker;
        const fromCoordinates = {
            LogOutput: { lat: 43.751838, lng: -79.7112873 },
            LogOutput_coatings: { lat: 43.6930758, lng: -79.6016076 }
        };

        const toCoordinates = {
            Hydroform: { lat: 43.751838, lng: -79.7112873 },
            Coatings: { lat: 43.6930758, lng: -79.6016076 }
        };

        const RADIUS = 5000; // 5 km

        function initMap()
        {
            map = new google.maps.Map(document.getElementById("map"), {
                center: { lat: 43.751838, lng: -79.7112873 }, //Hydroform
                zoom: 10,
            });

            //Get source and plant name from hidden fields
            const dataSource = '<%= hdnPartNumber.Value %>'; //LogOutput or LogOutput_coatings - depend on the part number dragged from which plant
            const plantName = '<%= hdnPlantName.Value %>'; //Hydroform or Coatings

            fromMarker = new google.maps.Marker({
                position: fromCoordinates[dataSource],
                map: map,
                title: 'From'
            });

            toMarker = new google.maps.Marker({
                position: toCoordinates[plantName],
                map: map,
                title: 'To'
            });

            const geofenceCircle = new google.maps.Circle({
                strokeColor: "#FF0000",
                strokeOpacity: 0.8,
                strokeWeight: 2,
                fillColor: "#FF0000",
                fillOpacity: 0.35,
                map: map,
                center: fromCoordinates[dataSource],
                radius: RADIUS,
            });

            if (navigator.geolocation) {
                navigator.geolocation.watchPosition(updateUserLocation);
            } else {
                console.log('Geolocation is not supported by this browser.');
            }
        }

        function UpdateLocation(position) {
            const userLatLng = {
                lat: position.coords.latitude,
                lng: position.coords.longitude
            };

            if (!userMar) {
                userMarker = new google.maps.Marker({
                    position: userLatLng,
                    map: map,
                    title: 'You are here'
                });
            } else {
                userMarker.setPosition(userLatLng);
            }

            const plantName = '<%= hdnPlantName.Value %>';
            const plantLatlng = toCoordinates[plantName];

            const distance = google.maps.geometry.spherical.computeDistanceBetween(
                new google.maps.LatLng(userLatLng),
                new google.maps.LatLng(plantLatlng)
            );

            if (distance <= RADIUS) {
               // User is within the geofence
                console.log('User is within the geofence');
                sendEmailNotification('within');
            } else {
                // User is outside the geofence
                console.log('User is outside the geofence');
                sendEmailNotification('outside');
            }
        }

        function sendEmailNotification(status) {
            $.ajax({
                type: 'POST',
                url: 'ViewRecord.aspx/SendEmailNotification',
                data: JSON.stringify({ status: status }),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (response) {
                    console.log('Email sent response:', response.d);
                },
                error: function (error) {
                    console.error('Error sending email:', error);
                }
            });
        }

    </script>
</body>
</html>

