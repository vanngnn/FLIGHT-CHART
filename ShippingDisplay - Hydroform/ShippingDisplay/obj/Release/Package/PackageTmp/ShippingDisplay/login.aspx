<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ShippingDisplay.ShippingDisplay.login" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="icon" type="image/png" href="Template/img/martinrea_logo.png"/>
    <title>Shipping Display</title>
    <!-- Google Font: Source Sans Pro -->
    <link rel="stylesheet" href="https://fonts.googleapis.com/css?family=Source+Sans+Pro:300,400,400i,700&display=fallback"/>
    <!-- Font Awesome -->
    <link rel="stylesheet" href="Template/plugins/fontawesome-free/css/all.min.css"/>
    <!-- icheck bootstrap -->
    <link rel="stylesheet" href="Template/plugins/icheck-bootstrap/icheck-bootstrap.min.css"/>
    <!-- Theme style -->
    <link rel="stylesheet" href="Template/dist/css/adminlte.min.css"/>
</head>
<body class="hold-transition login-page">
    <div class="login-box">
        <!-- /.login-logo -->
        <div class="card card-outline card-primary">
            <div class="card-header text-center">
                <img src="Template/img/martinrea_logo.png" width="200px" height="200px" />
                <a href="login.aspx" class="h1"><b>MARTINREA</b></a>
                <br/><b>SHIPPING DISPLAY</b>
            </div>
            <div class="card-body">

                <form id="form1" runat="server">
                    <div class="input-group mb-3">
                        <asp:TextBox ID="txtUsuario" runat="server"  class="form-control" placeholder="Usuario"></asp:TextBox>
                        <%--<input type="email" class="form-control" placeholder="Email" />--%>
                        <div class="input-group-append">
                            <div class="input-group-text">
                                <span class="fas fa-user"></span>
                            </div>
                        </div>
                    </div>
                    <div class="input-group mb-3">
                        <asp:TextBox ID="txtPassword" runat="server"  class="form-control" placeholder="Contraseña"  type="password"></asp:TextBox>
                        <%--<input type="password" class="form-control" placeholder="Password" />--%>
                            <div class="input-group-append">
                                <div class="input-group-text">
                                    <span class="fas fa-lock"></span>
                                </div>
                            </div>
                    </div>
                    <div class="social-auth-links text-center mt-2 mb-3">
                        <a href="#" class="btn btn-block btn-primary">
                        <asp:Button ID="btnLogin" runat="server" Text="INICIAR"  class="btn btn-block btn-primary" OnClick="btnLogin_Click"/>
                        </a>
            </div>
            </form>
            
            <!-- /.social-auth-links -->
            <%--<p class="mb-1">
                <a href="forgot-password.html">I forgot my password</a>
            </p>
            <p class="mb-0">
                <a href="register.html" class="text-center">Register a new membership</a>
            </p>--%>
            </div>
            <!-- /.card-body -->
        </div>
        <!-- /.card -->
</div>
    <!-- jQuery -->
    <script src="Template/plugins/jquery/jquery.min.js"></script>
    <!-- Bootstrap 4 -->
    <script src="Template/plugins/bootstrap/js/bootstrap.bundle.min.js"></script>
    <!-- AdminLTE App -->
    <script src="Template/dist/js/adminlte.min.js"></script>
</body>
</html>
