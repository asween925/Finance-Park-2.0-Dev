<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Create_Teacher.aspx.cs" Inherits="Create_Teacher" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Create a Teacher</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <link rel="shortcut icon" type="image/png" href="../../Media/faviconFP.png" />

    <script src="https://code.jquery.com/jquery-1.10.2.js"></script>
</head>

<body>
    <form id="EMS_Form" runat="server">

        <%--Header information--%>
        <header class="headerTop no-print"><a style="float: left; padding-top: 2px;">Finance Park 2.0</a><a style="float: right; padding-right: 30px; padding-top: 2px;"><asp:Label ID="headerSchoolName_lbl" Text="School Name Here" runat="server"></asp:Label></a></header>

        <%--Navigation bar--%>
        <div id="nav-placeholder">
        </div>

        <script>
            $(function () {
                $("#nav-placeholder").load("../../nav.html");
            });
        </script>

        <%--Content--%>
        <div class="content">
            <h2 class="h2">Create a Teacher</h2>
            <h3>This page will allow you to add a new teacher to the database.
                <br /><br />
                Enter the teacher's first name, last name, school, school email, school county, and the approximate student count. Click 'Submit' when you are finished.
            </h3>
            <p>First Name:</p>
            <asp:TextBox ID="firstName_tb" runat="server" CssClass="textbox"></asp:TextBox>&ensp;<asp:Label ID="error_lbl" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <p>Last Name (Required): </p>
            <asp:TextBox ID="lastName_tb" runat="server" CssClass="textbox"></asp:TextBox>
            <p>School Name (Required):</p>
            <asp:DropDownList ID="schoolName_ddl" runat="server" CssClass="ddl"></asp:DropDownList>
            <p>Teacher Email (Required):</p>
            <asp:TextBox ID="email_tb" runat="server" CssClass="textbox"></asp:TextBox>
            <p>Contact Teacher? (Checked is Yes)</p>
            <asp:CheckBox ID="contact_chk" runat="server"/>
            <br />
            <br />
            <asp:Button ID="Submit_btn" runat="server" Text="Submit" CssClass="button" OnClick="Submit_btn_Click" />
            <br />
            <br />
        </div>

        <asp:HiddenField ID="currentVisitID_hf" runat="server" />
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
        <script src="Scripts.js"></script>
        <script>
            /* Loop through all dropdown buttons to toggle between hiding and showing its dropdown content - This allows the user to have multiple dropdowns without any conflict */
            $(".sub-menu ul").hide();
            $(".sub-menu a").click(function () {
                $(this).parent(".sub-menu").children("ul").slideToggle("100");
                $(this).find(".right").toggleClass("fa-caret-up fa-caret-down");
            });
        </script>

        <asp:SqlDataSource ID="Review_sds" runat="server"></asp:SqlDataSource>
    </form>
</body>
</html>
