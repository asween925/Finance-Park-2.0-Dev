<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Create_Sponsor.aspx.cs" Inherits="Create_Sponsor" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Create a Sponsor</title>

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
            <h2 class="h2">Create a Sponsor</h2>
            <h3>This page allows you to add a new sponsor to Finance Park. Fill out the required fields and press submit to create a sponsor.
            </h3>
            <p>Sponsor Name (Required):</p>
            <asp:TextBox ID="sponsorName_tb" runat="server" CssClass="textbox"></asp:TextBox>&ensp;<asp:Label ID="error_lbl" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <p>Business Name #1 (Required):</p>
            <asp:DropDownList ID="businessName1_ddl" runat="server" CssClass="ddl"></asp:DropDownList>
            <p>Business Name #2:</p>
            <asp:DropDownList ID="businessName2_ddl" runat="server" CssClass="ddl"></asp:DropDownList>
            <p>Business Name #3:</p>
            <asp:DropDownList ID="businessName3_ddl" runat="server" CssClass="ddl"></asp:DropDownList>
            <p>Business name #4:</p>
            <asp:DropDownList ID="businessName4_ddl" runat="server" CssClass="ddl"></asp:DropDownList>
            <p>Upload Logo:</p>
            <asp:FileUpload ID="logo_fu" runat="server" />
            <br /><br />
            <asp:Button ID="submit_btn" runat="server" CssClass="button" Text="Submit" OnClick="submit_btn_Click" />
            <br /><br />
            
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
