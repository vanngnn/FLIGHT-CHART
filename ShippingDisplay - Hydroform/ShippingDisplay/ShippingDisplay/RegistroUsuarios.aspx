<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RegistroUsuarios.aspx.cs" Inherits="ShippingDisplay.ShippingDisplay.RegistroUsuarios" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="refresh" content="30" />
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
                        <asp:HyperLink ID="LinkDash" NavigateUrl="Dashboard.aspx" runat="server" Visible="true"  class="nav-link active"> 
                            <i class="nav-icon fas fa-tachometer-alt"></i><p>Dashboard</p>
                        </asp:HyperLink>
                    </li>
                    <li class="nav-item">
                        <asp:HyperLink ID="HyperLink4" NavigateUrl="#" runat="server" Visible="true"  class="nav-link"> 
                            <i class="nav-icon fas fa-ellipsis-h"></i><p>Shipments<i class="fas fa-angle-left right"></i></p>
                        </asp:HyperLink>

                        <ul class="nav nav-treeview">
                            <li class="nav-item">
                                <asp:HyperLink ID="LinkShipIn" NavigateUrl="#" runat="server" Visible="true"  class="nav-link"> 
                                    <i class="far fa-circle nav-icon"></i><p>Inputs<i class="fas fa-angle-left right"></i></p>
                                </asp:HyperLink>

                                <ul class="nav nav-treeview">
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyinput_dock1" NavigateUrl="~/ShippingDisplay/DAILYINPUT_DOCK1.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 1</p>
                                        </asp:HyperLink>
                                    </li>
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyinput_dock2" NavigateUrl="~/ShippingDisplay/DAILYINPUT_DOCK2.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 2</p>
                                        </asp:HyperLink>
                                    </li>
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyinput_dock3" NavigateUrl="~/ShippingDisplay/DAILYINPUT_DOCK3.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 3</p>
                                        </asp:HyperLink>
                                    </li>
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyinput_dock4" NavigateUrl="~/ShippingDisplay/DAILYINPUT_DOCK4.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 4</p>
                                        </asp:HyperLink>
                                    </li>
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyinput_dock5" NavigateUrl="~/ShippingDisplay/DAILYINPUT_DOCK5.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 5</p>
                                        </asp:HyperLink>
                                    </li>
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyinput_dock6" NavigateUrl="~/ShippingDisplay/DAILYINPUT_DOCK6.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 6</p>
                                        </asp:HyperLink>
                                    </li>
                                </ul>
                            </li>

                            <li class="nav-item">
                                <asp:HyperLink ID="LinkShipOut" NavigateUrl="#" runat="server" Visible="true"  class="nav-link" > 
                                    <i class="far fa-circle nav-icon"></i><p>Outputs<i class="fas fa-angle-left right"></i></p>
                                </asp:HyperLink>
                                
                                <ul class="nav nav-treeview">
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyoutput_dock1" NavigateUrl="~/ShippingDisplay/DAILYOUTPUT_DOCK1.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 1</p>
                                        </asp:HyperLink>
                                    </li>
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyoutput_dock2" NavigateUrl="~/ShippingDisplay/DAILYOUTPUT_DOCK2.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 2</p>
                                        </asp:HyperLink>
                                    </li>
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyoutput_dock3" NavigateUrl="~/ShippingDisplay/DAILYOUTPUT_DOCK3.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 3</p>
                                        </asp:HyperLink>
                                    </li>
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyoutput_dock4" NavigateUrl="~/ShippingDisplay/DAILYOUTPUT_DOCK4.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 4</p>
                                        </asp:HyperLink>
                                    </li>
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyoutput_dock5" NavigateUrl="~/ShippingDisplay/DAILYOUTPUT_DOCK5.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 5</p>
                                        </asp:HyperLink>
                                    </li>
                                    <li class="nav-item">
                                        <asp:HyperLink ID="dailyoutput_dock6" NavigateUrl="~/ShippingDisplay/DAILYOUTPUT_DOCK5.aspx" runat="server" Visible="true" class="nav-link">
                                            <p>DOCK 6</p>
                                        </asp:HyperLink>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </li>

                    <li class="nav-item">
                        <asp:HyperLink ID="LinkConfig" NavigateUrl="#" runat="server" Visible="true"  class="nav-link"> 
                            <i class="nav-icon fas fa-edit"></i><p>Daily Log<i class="fas fa-angle-left right"></i></p>
                        </asp:HyperLink>
                        <ul class="nav nav-treeview">
                            <li class="nav-item">
                                <asp:HyperLink ID="LinkRegEntry" NavigateUrl="DAILYLOG_INPUT.aspx" runat="server" Visible="true"  class="nav-link"> 
                                    <i class="far fa-circle nav-icon"></i><p>Inputs</p>
                                </asp:HyperLink>
                            </li>
                            <li class="nav-item">
                                <asp:HyperLink ID="LinkRegOut" NavigateUrl="DAILYLOG_OUTPUT.aspx.aspx" runat="server" Visible="true"  class="nav-link" > 
                                    <i class="far fa-circle nav-icon"></i><p>Outputs</p>
                                </asp:HyperLink>
                            </li>
                        </ul>
                    </li>
                    <li class="nav-item">
                        <asp:HyperLink ID="LinkShipper" NavigateUrl="SHIPPER.aspx" runat="server" Visible="true"  class="nav-link"> 
                            <i class="nav-icon fas fa-book"></i><p>Shipper</p>
                        </asp:HyperLink>
                    </li>
                    <li class="nav-item">
                        <asp:HyperLink ID="LinkReport" NavigateUrl="REPORTS.aspx" runat="server" Visible="true"  class="nav-link"> 
                            <i class="nav-icon far fa-calendar-alt"></i><p>Reports</p>
                        </asp:HyperLink>
                    </li>
                    <li class="nav-item">
                        <a href="#" class="nav-link"><i class="nav-icon fa fa-cog"></i><p>Configuration<i class="fas fa-angle-left right"></i></p></a>
                        <ul class="nav nav-treeview">
                            <li class="nav-item">
                                <asp:HyperLink ID="LinkRegister" NavigateUrl="REGISTERUSER.aspx" runat="server" Visible="true" class="nav-link">
                                    <i class="nav-icon fa fa-user-plus"></i><p>Users profiles</p>
                                </asp:HyperLink>
                            </li>
                        </ul>
                    </li>
                    <li class="nav-item">
                        <asp:LinkButton ID="LinkSalir" runat="server" OnClick="LinkSalir_Click"  class="nav-link">
                            <i class="nav-icon fa ion-log-out"></i><p>Log out</p>
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
        <section class="content-header">
            <div class="container-fluid">
                <div class="row mb-2">
                    <div class="col-sm-6">
                        <h5>Perfiles</h5>
                    </div>
                    <div class="col-sm-6">
                        <ol class="breadcrumb float-sm-right">
                            <li class="breadcrumb-item"><a href="#">Home</a></li>
                            <li class="breadcrumb-item active">Users</li>
                        </ol>
                    </div>
                </div>
            </div><!-- /.container-fluid -->
        </section>
        <section class="content">
            <div class="container-fluid">
                <div class="row">
                    <div class="col-md-3">
                        <!-- Profile Image -->
				        <div class="card card-outline card-info">
				            <div class="card-body box-profile">
					            <div class="text-center">
					                <img class="profile-user-img img-fluid img-circle" src="Template/dist/img/UserAdd.png" alt="User profile picture" />
					            </div>
					            <h3 class="profile-username text-center"><asp:Label ID="txtNameUsr" runat="server" placeholder="Name"/></h3>
					            <p class="text-muted text-center"><asp:Label ID="txtUser" runat="server" placeholder="Username"/></p>
					            <ul class="list-group list-group-unbordered mb-3">
					                <li class="list-group-item">
						                <b></b> <a class="float-right"></a>
					                </li>
					                <li class="list-group-item">
						                <b></b> <a class="float-right"></a>
					                </li>
					                <li class="list-group-item">
						                <b></b> <a class="float-right"></a>
					                </li>
					            </ul>
				            </div>
				            <!-- /.card-body -->
				        </div>
				        <!-- /.card -->
                    </div>
                    <div class="col-md-9">
                        <div class="card">
                            <div class="card-header p-2">
                                <ul class="nav nav-pills">
                                    <li class="nav-item"><a class="nav-link" href="#activity" data-toggle="tab">Users</a></li>
                                </ul>
                            </div>
                            <div class="card-body">
                            <div class="tab-content">
                                <div class="active tab-pane" id="activity">
                                    <div class="form-group row">
                                        <label for="inputName" class="col-sm-2 col-form-label">Name</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtNombre" class="form-control" runat="server" placeholder="Name"/>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputEmail" class="col-sm-2 col-form-label">User</label>
                                        <div class="col-sm-10">
                                             <asp:TextBox ID="txtUsuario" class="form-control" runat="server" placeholder="Username"/>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputName2" class="col-sm-2 col-form-label">Password</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtPassword" class="form-control" runat="server" type="password" />
                                            <asp:TextBox runat="server" Visible="false" ID="txtSalt" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputExperience" class="col-sm-2 col-form-label">E-mail:</label>
                                        <div class="col-sm-10">
                                            <asp:TextBox ID="txtEmail" class="form-control" runat="server" placeholder="Ejm: juan.perez@martinrea.com" TextMode="Email" />
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputSkills" class="col-sm-2 col-form-label">Plant</label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList ID="dblPlanta" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <label for="inputSkills" class="col-sm-2 col-form-label">Department</label>
                                        <div class="col-sm-10">
                                            <asp:DropDownList ID="dblDepartamento" class="form-control" runat="server"></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="offset-sm-2 col-sm-10">
                                            <div class="checkbox">
                                                <label>Status: </label>
                                                    <asp:RadioButtonList ID="rbActivo" runat="server" RepeatDirection="Horizontal">
                                                             <asp:ListItem text="YES" Value=1></asp:ListItem>
                                                             <asp:ListItem text="NOT" Value=0></asp:ListItem>
                                                    </asp:RadioButtonList>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="form-group row">
                                        <div class="offset-sm-2 col-sm-10">
                                            <asp:Button ID="btnGuardar" Text="Save" CssClass="btn btn-success"  runat="server" OnClick="btnGuardar_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="card card-outline card-info">
                        <div class="card-header">
                            <h3 class="card-title">User list</h3>
                        </div>
                        <div class="card-body">
                            <asp:GridView ID="gvUsuarios" runat="server"  AutoGenerateColumns="false" DataKeyNames="Id_user" class="table table-bordered table-striped" OnRowCommand="gvUsuarios_RowCommand" >
                                <Columns>
                                    <asp:BoundField HeaderText="ID" DataField="Id_user" />
                                    <asp:BoundField HeaderText="Name" DataField="Nombre" />
                                    <asp:BoundField HeaderText="Username" DataField="User" />    
                                    <asp:BoundField HeaderText="Email" DataField="Email" />
                                    <asp:BoundField HeaderText="Plant" DataField="DesPlanta" />
                                    <asp:BoundField HeaderText="Department" DataField="DesDepto" />
                                    <asp:BoundField HeaderText="Updated" DataField="Fec_mod" />
                                    <asp:BoundField HeaderText="Status" DataField="Status" />
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button text="Edit" CommandName="Editar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" CssClass="btn btn-primary" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button text="Delete" CommandName="Eliminar" CommandArgument="<%# ((GridViewRow)Container).RowIndex %>" runat="server" CssClass="btn btn-danger" OnClientClick="return isDelete();" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </section>
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