﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DashboardV1.aspx.cs" Inherits="ShippingDisplay.ShippingDisplay.DashboardV1" %>

<!DOCTYPE html>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="refresh" content="10" />
    <link rel="icon" type="image/png" href="Template/img/martinrea_logo.png"/>
    <title>Shipping Display</title>
    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback" />
    <!-- Font Awesome -->
    <link rel="stylesheet" href="template/plugins/fontawesome-free/css/all.min.css" />
    <!-- Ionicons -->
    <link rel="stylesheet" href="https://code.ionicframework.com/ionicons/2.0.1/css/ionicons.min.css" />
    <!-- Tempusdominus Bootstrap 4 -->
    <link rel="stylesheet" href="template/plugins/tempusdominus-bootstrap-4/css/tempusdominus-bootstrap-4.min.css" />
    <!-- iCheck -->
    <link rel="stylesheet" href="template/plugins/icheck-bootstrap/icheck-bootstrap.min.css" />
    <!-- JQVMap -->
    <link rel="stylesheet" href="template/plugins/jqvmap/jqvmap.min.css" />
    <!-- Theme style -->
    <link rel="stylesheet" href="template/dist/css/adminlte.min.css" />
    <!-- overlayScrollbars -->
    <link rel="stylesheet" href="template/plugins/overlayScrollbars/css/OverlayScrollbars.min.css" />
    <!-- Daterange picker -->
    <link rel="stylesheet" href="template/plugins/daterangepicker/daterangepicker.css" />
    <!-- summernote -->
    <link rel="stylesheet" href="template/plugins/summernote/summernote-bs4.min.css" />
</head>
<body class="hold-transition sidebar-mini sidebar-collapse">
    
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
                <a href="Dashboard.aspx" class="nav-link">Dashboard</a>
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
    <form id="form1" runat="server">
    <aside class="main-sidebar sidebar-dark-primary elevation-4">
        
        <!-- Brand Logo -->
        <a href="dashboard.aspx" class="brand-link">
            <img src="template/img/martinrea_logo.png" alt="Martinrea" class="brand-image img-circle elevation-3" style="opacity: .8" />
            <span class="brand-text font-weight-light">Martinrea</span>
        </a>
        <!-- Sidebar -->
        <div class="sidebar">
             <!-- Sidebar user panel (optional) -->
            <div class="user-panel mt-3 pb-3 mb-3 d-flex">
                <div class="image">
                    <img src="Template/dist/img/user_icon.png" class="img-circle elevation-2" alt="User Image" />
                </div>
                <div class="info">
                    <span class="brand-text font-weight-light"><asp:Label ID="lblNombre" runat="server" Text="Username" ForeColor ="White"></asp:Label></span>
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
                        <asp:HyperLink ID="LinkDashEmb" NavigateUrl="DashboardV1.aspx" runat="server" Visible="true"  class="nav-link active"> 
                            <i class="nav-icon fas fa-ellipsis-h"></i><p>Shipments</p>
                        </asp:HyperLink>
                    </li>
                    <li class="nav-item">
                         <asp:HyperLink ID="LinkConfig" NavigateUrl="#" runat="server" Visible="true"  class="nav-link"> 
                            <i class="nav-icon fas fa-edit"></i><p>Daily Log<i class="fas fa-angle-left right"></i></p>
                        </asp:HyperLink>
                        <ul class="nav nav-treeview">
                            <li class="nav-item">
                                <asp:HyperLink ID="LinkRegEntry" NavigateUrl="RegistroEntrada.aspx" runat="server" Visible="true"  class="nav-link"> 
                                    <i class="far fa-circle nav-icon"></i><p>Inputs</p>
                                </asp:HyperLink>
                            </li>
                            <li class="nav-item">
                                <asp:HyperLink ID="LinkRegOut" NavigateUrl="RegistroSalida.aspx" runat="server" Visible="true"  class="nav-link" > 
                                    <i class="far fa-circle nav-icon"></i><p>Outputs</p>
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
                        <asp:HyperLink ID="LinkReport" NavigateUrl="Reportes.aspx" runat="server" Visible="true"  class="nav-link"> 
                            <i class="nav-icon far fa-calendar-alt"></i><p>Reports</p>
                        </asp:HyperLink>
                    </li>
                    <li class="nav-item">
                        <a href="#" class="nav-link"><i class="nav-icon fa fa-cog"></i><p>Configuration<i class="fas fa-angle-left right"></i></p></a>
                        <ul class="nav nav-treeview">
                            <li class="nav-item">
                                <asp:HyperLink ID="LinkRegister" NavigateUrl="RegistroUsuarios.aspx" runat="server" Visible="true" class="nav-link">
                                    <i class="nav-icon fa fa-user-plus"></i><p>Users profiles</p>
                                </asp:HyperLink>
                            </li>
                        </ul>
                    </li>
                    <li class="nav-item">
                        <asp:LinkButton ID="LinkSalir" runat="server" OnClick="LinkSalir_Click"  class="nav-link">
                            <i class="nav-icon ion-log-out"></i><p>Log out</p>
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
                <%--<div class="row mb-2">
                    <div class="col-sm-6">
                        <h1 class="m-0">Dashboard</h1>
                    </div><!-- /.col -->
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item active">Dashboard</li>
                        </ol>
                    </div><!-- /.col -->
                </div><!-- /.row -->--%>
            </div><!-- /.container-fluid -->
        </div>
        <!-- /.content-header -->

        <!-- Main content -->
        <section class="content">
            <div class="container-fluid">
                <!-- Small boxes (Stat box) -->
                <div class="row">
                    <!-- ./col -->
                    <div class="col-lg-3 col-6">
                    <!-- small box -->
                        <div class="small-box bg-success">
                            <div class="inner">
                                <h3><asp:Label ID="lblEntiempo" runat="server" Text="0"></asp:Label></h3>
                                <p><h3>ON TIME</h3></p>
                            </div>
                            <div class="icon"> 
                                <i class="ion ion-ios-clock"></i>                 
                            </div>
                        </div>
                    </div>
                    <!-- ./col -->
                    <div class="col-lg-3 col-6">
                        <!-- small box -->
                        <div class="small-box bg-danger">
                            <div class="inner">
                                <h3><asp:Label ID="lblAtrasado" runat="server" Text="0"></asp:Label></h3>
                                <p><h3>DELAYED</h3></p>
                            </div>
                            <div class="icon">
                                <i class="ion ion-android-warning"></i>
                            </div>
                        </div>
                    </div>
                    <!-- ./col -->
                    <div class="col-lg-3 col-6">
                        <!-- small box -->
                        <div class="small-box bg-warning">
                            <div class="inner">
                                <h3><asp:Label ID="lblSinShipper" runat="server" Text="0"></asp:Label></h3>
                                <p><h3>WITHOUT SHIPPER</h3></p>
                            </div>
                            <div class="icon">
                                <i class="ion ion-android-checkmark-circle"></i>
                            </div>
                        </div>
                    </div>     
                    <div class="col-lg-3 col-6">
                        <!-- small box -->
                        <div class="small-box bg-info">
                            <div class="inner">
                                <h3><asp:Label ID="lblEnviado" runat="server" Text="0"></asp:Label></h3>
                                <p><h3>SHIPPED</h3></p>
                            </div>
                            <div class="icon">
                                <i class="ion ion-checkmark-round"></i>
                            </div>
                        </div>
                    </div>   
                </div>
                <!-- /.row -->

                <!-- Main row -->
                <div class="row">
                    <div class="col-12">
                        <!-- /.card -->

            <div class="card">
              <div class="card-header">
                <h3 class="card-title">Daily entry</h3>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
                <asp:GridView ID="gvRegistros" runat="server"  AutoGenerateColumns="false" DataKeyNames="Id_reg" class="table table-bordered table-striped" OnRowDataBound="gvRegistros_RowDataBound" >
                                <Columns>
                                    <asp:BoundField HeaderText="ID" DataField="Id_reg" />
                                    <asp:BoundField HeaderText="Customer" DataField="ClienteName" />
                                    <asp:BoundField HeaderText="Carrier" DataField="CarrierName" />
                                    <asp:BoundField HeaderText="Input" DataField="Entrada" />
                                    <asp:BoundField HeaderText="Output" DataField="Salida" />
                                    <asp:BoundField HeaderText="Box" DataField="Caja" />
                                    <asp:BoundField HeaderText="Route" DataField="RutaName" />
                                    <asp:BoundField HeaderText="Shipper" DataField="Shipper" />
                                    <asp:BoundField HeaderText="Status" DataField="Estado" />
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
            </div><!-- /.container-fluid -->
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
<!-- jQuery UI 1.11.4 -->
<script src="template/plugins/jquery-ui/jquery-ui.min.js"></script>
<!-- Resolve conflict in jQuery UI tooltip with Bootstrap tooltip -->
<script>
    $.widget.bridge('uibutton', $.ui.button)
</script>
<!-- Bootstrap 4 -->
<script src="template/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
<!-- ChartJS -->
<script src="template/plugins/chart.js/Chart.min.js"></script>
<!-- Sparkline -->
<script src="template/plugins/sparklines/sparkline.js"></script>
<!-- JQVMap -->
<script src="template/plugins/jqvmap/jquery.vmap.min.js"></script>
<script src="template/plugins/jqvmap/maps/jquery.vmap.usa.js"></script>
<!-- jQuery Knob Chart -->
<script src="template/plugins/jquery-knob/jquery.knob.min.js"></script>
<!-- daterangepicker -->
<script src="template/plugins/moment/moment.min.js"></script>
<script src="template/plugins/daterangepicker/daterangepicker.js"></script>
<!-- Tempusdominus Bootstrap 4 -->
<script src="template/plugins/tempusdominus-bootstrap-4/js/tempusdominus-bootstrap-4.min.js"></script>
<!-- Summernote -->
<script src="template/plugins/summernote/summernote-bs4.min.js"></script>
<!-- overlayScrollbars -->
<script src="template/plugins/overlayScrollbars/js/jquery.overlayScrollbars.min.js"></script>
<!-- AdminLTE App -->
<script src="template/dist/js/adminlte.js"></script>
<!-- AdminLTE for demo purposes -->
<script src="template/dist/js/demo.js"></script>
<!-- AdminLTE dashboard demo (This is only for demo purposes) -->
<script src="template/dist/js/pages/dashboard.js"></script>

</body>
</html>