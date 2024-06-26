﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reportes.aspx.cs" Inherits="ShippingDisplay.ShippingDisplay.Reportes" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link rel="icon" type="image/png" href="Template/img/martinrea_logo.png"/>
    <title>Shipping Display</title>
    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <link rel="stylesheet" href="template/plugins/fontawesome-free/css/all.min.css" />
    <!-- daterange picker -->
    <link rel="stylesheet" href="template/plugins/daterangepicker/daterangepicker.css" />
    <!-- iCheck for checkboxes and radio inputs -->
    <link rel="stylesheet" href="template/plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <!-- Bootstrap Color Picker -->
    <link rel="stylesheet" href="template/plugins/bootstrap-colorpicker/css/bootstrap-colorpicker.min.css" />
    <!-- Tempusdominus Bootstrap 4 -->
    <link rel="stylesheet" href="template/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css" />
    <!-- Select2 -->
    <link rel="stylesheet" href="template/plugins/select2/css/select2.min.css" />
    <link rel="stylesheet" href="template/plugins/select2-bootstrap4-theme/select2-bootstrap4.min.css"/>
    <!-- Bootstrap4 Duallistbox -->
    <link rel="stylesheet" href="template/plugins/bootstrap4-duallistbox/bootstrap-duallistbox.min.css" />
    <!-- BS Stepper -->
    <link rel="stylesheet" href="template/plugins/bs-stepper/css/bs-stepper.min.css" />
    <!-- dropzonejs -->
    <link rel="stylesheet" href="template/plugins/dropzone/min/dropzone.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="template/dist/css/adminlte.min.css" />

    <script type = "text/javascript">
        function DisableButton()
        {
            document.getElementById("<%=btnFiltrar.ClientID %>").disabled = true;
        }
        window.onbeforeunload = DisableButton;
        function isDelete() {
            var r = confirm("En realidad deseas eliminarlo?");
            return r;
        }
    </script>
</head>
<body class="hold-transition sidebar-mini layout-fixed">

<div class="wrapper">
    <!-- Preloader -->
    <div class="preloader flex-column justify-content-center align-items-center">
        <img class="animation__shake" src="template/img/martinrea_logo.png" alt="Martinrea" height="150" width="150" />
    </div>
    <!-- Navbar -->
    <nav class="main-header navbar navbar-expand navbar-white navbar-light">
        <!-- Left navbar links -->
        <ul class="navbar-nav">
            <li class="nav-item">
                <a class="nav-link" data-widget="pushmenu" href="#" role="button"><i class="fas fa-bars"></i></a>
            </li>
            <li class="nav-item d-none d-sm-inline-block">
                <a href="Dashboard.aspx" class="nav-link">Inicio</a>
            </li>
        </ul>
        <!-- Right navbar links -->
        <ul class="navbar-nav ml-auto">
            <li class="nav-item">
                <a class="nav-link" data-widget="fullscreen" href="#" role="button">
                    <i class="fas fa-expand-arrows-alt"></i>
                </a>
            </li>
        </ul>
    </nav>
    <!-- /.navbar -->
    <!-- Main Sidebar Container -->
    <form class="form-horizontal" runat="server">
    <aside class="main-sidebar sidebar-dark-primary elevation-4">
        <!-- Brand Logo -->
        <a href="dashboard.aspx" class="brand-link">
            <img src="template/img/martinrea_logo.png" alt="Martinrea" class="brand-image img-circle elevation-3" style="opacity: .8" />
            <span class="brand-text font-weight-light">Martinrea</span>
        </a>
        <!-- Sidebar -->
        <div class="sidebar">
            <div class="user-panel mt-3 pb-3 mb-3 d-flex">
                <div class="image">
                    <img src="Template/dist/img/user_icon.png" class="img-circle elevation-2" alt="User Image" />
                </div>
                <div class="info">
                    <span class="brand-text font-weight-light"><asp:Label ID="lblNombre" runat="server" Text="Usuario" ForeColor ="White"></asp:Label></span>
                </div>
            </div>
            <nav class="mt-2">
                <ul class="nav nav-pills nav-sidebar flex-column" data-widget="treeview" role="menu" data-accordion="false">
                    <li class="nav-item">
                        <asp:HyperLink ID="LinkDash" NavigateUrl="Dashboard.aspx" runat="server" Visible="true"  class="nav-link"> 
                            <i class="nav-icon fas fa-tachometer-alt"></i><p>Dashboard</p>
                        </asp:HyperLink>
                    </li>
                    <li class="nav-item">
                        <asp:HyperLink ID="LinkDashEmb" NavigateUrl="DashboardV1.aspx" runat="server" Visible="true"  class="nav-link"> 
                            <i class="nav-icon fas fa-ellipsis-h"></i><p>Embarques</p>
                        </asp:HyperLink>
                    </li>
                    <li class="nav-item menu-open">
                         <asp:HyperLink ID="LinkConfig" NavigateUrl="#" runat="server" Visible="true"  class="nav-link"> 
                            <i class="nav-icon fas fa-edit"></i><p>Registro<i class="fas fa-angle-left right"></i></p>
                        </asp:HyperLink>
                        <ul class="nav nav-treeview">
                            <li class="nav-item">
                                <asp:HyperLink ID="LinkRegEntry" NavigateUrl="RegistroEntrada.aspx" runat="server" Visible="true"  class="nav-link"> 
                                    <i class="far fa-circle nav-icon"></i><p>Entrada</p>
                                </asp:HyperLink>
                            </li>
                            <li class="nav-item">
                                <asp:HyperLink ID="LinkRegOut" NavigateUrl="RegistroSalida.aspx" runat="server" Visible="true"  class="nav-link"> 
                                    <i class="far fa-circle nav-icon"></i><p>Salida</p>
                                </asp:HyperLink>
                            </li>
                        </ul>
                    </li>
                    <li class="nav-item">
                        <asp:HyperLink ID="LinkShipper" NavigateUrl="Shipper.aspx" runat="server" Visible="true"  class="nav-link"> 
                            <i class="nav-icon fas fa-book"></i><p>Shipper</p>
                        </asp:HyperLink>
                    </li>
                    <li class="nav-item">
                        <asp:HyperLink ID="LinkReport" NavigateUrl="Reportes.aspx" runat="server" Visible="true"  class="nav-link active"> 
                            <i class="nav-icon far fa-calendar-alt"></i><p>Reportes</p>
                        </asp:HyperLink>
                    </li>
                    <li class="nav-item">
                        <a href="#" class="nav-link"><i class="nav-icon fa fa-cog"></i><p>Configuración<i class="fas fa-angle-left right"></i></p></a>
                        <ul class="nav nav-treeview">
                            <li class="nav-item">
                                <asp:HyperLink ID="LinkRegister" NavigateUrl="RegistroUsuarios.aspx" runat="server" Visible="true" class="nav-link">
                                    <i class="nav-icon fa fa-user-plus"></i><p>Perfil de usuarios</p>
                                </asp:HyperLink>
                            </li>
                            <li class="nav-item">
                                <asp:HyperLink ID="LinkPerfil" NavigateUrl="#" runat="server" Visible="true" class="nav-link">
                                    <i class="nav-icon fa fa-user"></i><p>Mi perfil</p>
                                </asp:HyperLink>
                            </li>
                        </ul>
                    </li>
                     <li class="nav-item">
                        <asp:LinkButton ID="LinkSalir" runat="server" OnClick="LinkSalir_Click"  class="nav-link">
                            <i class="nav-icon ion-log-out"></i><p>Salir</p>
                        </asp:LinkButton>
                    </li>
                </ul>
            </nav>
        </div>
        <!-- /.sidebar -->
    </aside>

    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <div class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h1 class="m-0"></h1>
                    </div><!-- /.col -->
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item active">Reportes</li>
                        </ol>
                    </div><!-- /.col -->
                </div><!-- /.row -->
            </div><!-- /.container-fluid -->
        </div>
        <!-- /.content-header -->

        <!-- Main content -->
        <section class="content">
            <!-- container-fluid -->
            <div class="container-fluid">
               
                <!-- Small boxes (Stat box) -->
                <div class="card card-default">
                    <!-- Horizontal Form -->
                    <div class="card card-info">
                        <div class="card-header">
                            <%--<h3 class="card-title">Horizontal Form</h3>--%>
                        </div>
                        <!-- /.card-header -->
                        <!-- form start -->
                        <div class="card-body">
                            <!-- Date range -->
                            <div class="form-group">
                                <label>Fecha:</label>
                                <div class="input-group">
                                    <div class="input-group-prepend">
                                        <span class="input-group-text">
                                            <i class="far fa-calendar-alt"></i>
                                        </span>
                                    </div>
                                    <asp:TextBox ID="reservation" class="form-control float-right" runat="server"></asp:TextBox>
                                </div>
                            </div>
                            <div class="form-group">
                                <label>Planta:</label>
                                <div class="input-group"> 
                                    <asp:DropDownList ID="dblPlanta" runat="server" class="select2" style="width: 100%;"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <!-- /.card-body -->
                        <div class="card-footer">
                            <asp:Button ID="btnFiltrar" runat="server" Text="Filtar"  class="btn btn-block btn-info btn-lg" OnClick="btnFiltrar_Click" />
                        </div>
                            <!-- /.card-footer -->                  
                    </div>
                    <!-- /.card -->
                </div>
                <!-- Main row -->
                <div class="row">
                    <div class="col-12">
                        <!-- /.card -->
                        <div class="card">
                        <div class="card-header">
                            <h3 class="card-title">Registro</h3>
                            <div class="card-tools">
                                <%--<span title="3 New Messages" class="badge badge-primary">--%>
                                <asp:Button ID="btnExporta" class="btn btn-box-tool" runat="server" Text="Exportar" OnClick="btnExporta_Click" />
                                <button type="button" class="btn btn-tool" data-card-widget="collapse">
                                    <i class="fas fa-minus"></i>
                                </button>
                                <button type="button" class="btn btn-tool" data-card-widget="remove">
                                    <i class="fas fa-times"></i>
                                </button>
                             
                            </div>
                        </div>
                        <!-- /.card-header -->
                        <div class="card-body">
                            <asp:GridView ID="gvRegistros" runat="server"  AutoGenerateColumns="false" DataKeyNames="Id_reg" class="table table-bordered table-striped" >
                                <Columns>
                                    <asp:BoundField HeaderText="ID" DataField="Id_reg" />
                                    <asp:BoundField HeaderText="Cliente" DataField="ClienteName" />
                                    <asp:BoundField HeaderText="Carrier" DataField="CarrierName" />
                                    <asp:BoundField HeaderText="Entrada" DataField="Entrada" />
                                    <asp:BoundField HeaderText="Salida" DataField="Salida" />
                                    <asp:BoundField HeaderText="Caja" DataField="Caja" />
                                    <asp:BoundField HeaderText="Ruta" DataField="RutaName" />
                                    <asp:BoundField HeaderText="Entrada Ruta" DataField="Input" />
                                    <asp:BoundField HeaderText="Salida Ruta" DataField="Output" />
                                    <asp:BoundField HeaderText="Shipper" DataField="Shipper" />
                                    <asp:BoundField HeaderText="Estado" DataField="Estado" />
                                    <%--<asp:BoundField HeaderText="ASN Sent" DataField="ASN_Sent" />
                                    <asp:BoundField HeaderText="ASN Ack" DataField="ASN_Ack" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    <!-- /.card-body -->
                    </div>
                        <!-- /.card -->
                    </div>
                </div>
                <!-- /.row (main row) -->
            </div>
            <!-- /.container-fluid -->
        </section>
        <!-- /.content -->
    </div>
    <!-- /.content-wrapper -->
    <footer class="main-footer">
        <strong>Copyright &copy; 2021 Martinrea.</strong>
        All rights reserved.
        <div class="float-right d-none d-sm-inline-block">
            <b>Version</b> 1.0
        </div>
    </footer>
    <!-- Control Sidebar -->
    <aside class="control-sidebar control-sidebar-dark">
        <!-- Control sidebar content goes here -->
    </aside>
    <!-- /.control-sidebar -->
    </form>
</div>
<!-- ./wrapper -->

<!-- jQuery -->
<script src="template/plugins/jquery/jquery.min.js"></script>
<!-- Bootstrap 4 -->
<script src="template/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- Select2 -->
<script src="template/plugins/select2/js/select2.full.min.js"></script>
<!-- Bootstrap4 Duallistbox -->
<script src="template/plugins/bootstrap4-duallistbox/jquery.bootstrap-duallistbox.min.js"></script>
<!-- InputMask -->
<script src="template/plugins/moment/moment.min.js"></script>
<script src="template/plugins/inputmask/jquery.inputmask.min.js"></script>
<!-- date-range-picker -->
<script src="template/plugins/daterangepicker/daterangepicker.js"></script>
<!-- bootstrap color picker -->
<script src="template/plugins/bootstrap-colorpicker/js/bootstrap-colorpicker.min.js"></script>
<!-- Tempusdominus Bootstrap 4 -->
<script src="template/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
<!-- Bootstrap Switch -->
<script src="template/plugins/bootstrap-switch/js/bootstrap-switch.min.js"></script>
<!-- BS-Stepper -->
<script src="template/plugins/bs-stepper/js/bs-stepper.min.js"></script>
<!-- dropzonejs -->
<script src="template/plugins/dropzone/min/dropzone.min.js"></script>
<!-- AdminLTE App -->
<script src="template/dist/js/adminlte.min.js"></script>
<!-- AdminLTE for demo purposes -->
<script src="template/dist/js/demo.js"></script>
<!-- Page specific script -->
<script>
  $(function () {
    //Initialize Select2 Elements
    $('.select2').select2()

    //Initialize Select2 Elements
    $('.select2bs4').select2({
      theme: 'bootstrap4'
    })

    //Datemask dd/mm/yyyy
    $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
    //Datemask2 mm/dd/yyyy
    $('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
    //Money Euro
    $('[data-mask]').inputmask()

    //Date picker
    $('#reservationdate').datetimepicker({
        format: 'L'
    });

    //Date and time picker
    $('#reservationdatetime').datetimepicker({ icons: { time: 'far fa-clock' } });

    //Date range picker
    $('#reservation').daterangepicker()
    //Date range picker with time picker
    $('#reservationtime').daterangepicker({
      timePicker: true,
      timePickerIncrement: 30,
      locale: {
        format: 'MM/DD/YYYY hh:mm A'
      }
    })
    //Date range as a button
    $('#daterange-btn').daterangepicker(
      {
        ranges   : {
          'Today'       : [moment(), moment()],
          'Yesterday'   : [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
          'Last 7 Days' : [moment().subtract(6, 'days'), moment()],
          'Last 30 Days': [moment().subtract(29, 'days'), moment()],
          'This Month'  : [moment().startOf('month'), moment().endOf('month')],
          'Last Month'  : [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        },
        startDate: moment().subtract(29, 'days'),
        endDate  : moment()
      },
      function (start, end) {
        $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'))
      }
    )

    //Timepicker
    $('#timepicker').datetimepicker({
      format: 'LT'
    })

    //Bootstrap Duallistbox
    $('.duallistbox').bootstrapDualListbox()

    //Colorpicker
    $('.my-colorpicker1').colorpicker()
    //color picker with addon
    $('.my-colorpicker2').colorpicker()

    $('.my-colorpicker2').on('colorpickerChange', function(event) {
      $('.my-colorpicker2 .fa-square').css('color', event.color.toString());
    })

    $("input[data-bootstrap-switch]").each(function(){
      $(this).bootstrapSwitch('state', $(this).prop('checked'));
    })

  })
  // BS-Stepper Init
  document.addEventListener('DOMContentLoaded', function () {
    window.stepper = new Stepper(document.querySelector('.bs-stepper'))
  })

  // DropzoneJS Demo Code Start
  Dropzone.autoDiscover = false

  // Get the template HTML and remove it from the doumenthe template HTML and remove it from the doument
  var previewNode = document.querySelector("#template")
  previewNode.id = ""
  var previewTemplate = previewNode.parentNode.innerHTML
  previewNode.parentNode.removeChild(previewNode)

  var myDropzone = new Dropzone(document.body, { // Make the whole body a dropzone
    url: "/target-url", // Set the url
    thumbnailWidth: 80,
    thumbnailHeight: 80,
    parallelUploads: 20,
    previewTemplate: previewTemplate,
    autoQueue: false, // Make sure the files aren't queued until manually added
    previewsContainer: "#previews", // Define the container to display the previews
    clickable: ".fileinput-button" // Define the element that should be used as click trigger to select files.
  })

  myDropzone.on("addedfile", function(file) {
    // Hookup the start button
    file.previewElement.querySelector(".start").onclick = function() { myDropzone.enqueueFile(file) }
  })

  // Update the total progress bar
  myDropzone.on("totaluploadprogress", function(progress) {
    document.querySelector("#total-progress .progress-bar").style.width = progress + "%"
  })

  myDropzone.on("sending", function(file) {
    // Show the total progress bar when upload starts
    document.querySelector("#total-progress").style.opacity = "1"
    // And disable the start button
    file.previewElement.querySelector(".start").setAttribute("disabled", "disabled")
  })

  // Hide the total progress bar when nothing's uploading anymore
  myDropzone.on("queuecomplete", function(progress) {
    document.querySelector("#total-progress").style.opacity = "0"
  })

  // Setup the buttons for all transfers
  // The "add files" button doesn't need to be setup because the config
  // `clickable` has already been specified.
  document.querySelector("#actions .start").onclick = function() {
    myDropzone.enqueueFiles(myDropzone.getFilesWithStatus(Dropzone.ADDED))
  }
  document.querySelector("#actions .cancel").onclick = function() {
    myDropzone.removeAllFiles(true)
  }
  // DropzoneJS Demo Code End
</script>
</body>
</html>