﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<!doctype html>

<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Login</title>

    <link href="CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <link rel="shortcut icon" type="image/png" href="Media/FP_favicon_2.png" />
</head>

<body>
    <form id="staffPages_form" runat="server">

        <%--Header information--%>
        <div id="header-e1">
            Finance Park 2.0
        </div>
        <div id="header-e3">
            <asp:Label ID="lblHeaderSchoolName" Text="Login" runat="server"></asp:Label>
        </div>
        <div id="header-e2">
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        </div>

        <%--Login--%>
        <div class="login">
            <h2 class="h2">FP 2.0 Login</h2>
            <br />
            <p>Email</p>
            <asp:TextBox ID="tbEmail" runat="server" TextMode="SingleLine" Width="200px" CssClass="textbox" OnTextChanged="tbEmail_TextChanged"></asp:TextBox>
            <p>Password</p>
            <asp:TextBox ID="password_tb" runat="server" TextMode="Password" Width="150px" CssClass="textbox"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="button" OnClick="btnLogin_Click" />
            <br />
            <br />
            <br />
            <p><a href="mailto:sweeneya@pcsb.org?subject=Login">Click here to ask for assistance</a> (or email sweeneya@pcsb.org)</p>
            <br />
            <asp:Label ID="lblError" runat="server" ForeColor="Red" />
            <asp:Label ID="error2_lbl" runat="server" />
        </div>

        <asp:Label ID="lblSchoolName" runat="server" Visible="false"></asp:Label><asp:Label ID="schoolID_lbl" runat="server" Visible="false"></asp:Label><asp:Label ID="lblVisitID" runat="server" Visible="false"></asp:Label>

        <asp:HiddenField ID="schoolName_hf" runat="server" />
        <asp:HiddenField ID="hfSchoolID" runat="server" />
        <asp:HiddenField ID="hfVisitID" runat="server" />
        <asp:HiddenField ID="clientName_hf" runat="server" />
        <asp:HiddenField ID="teacherID_hf" runat="server" />

        <script src="Scripts.js"></script>
    </form>

</body>
</html>
