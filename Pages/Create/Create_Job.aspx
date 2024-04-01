<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Create_Job.aspx.cs" Inherits="Create_Job" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Create a Job</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <link rel="shortcut icon" type="image/png" href="../../Media/faviconFP.png" />

    <script src="https://code.jquery.com/jquery-1.10.2.js"></script>
</head>

<body>
    <form id="EMS_Form" runat="server">

        <%--Header information--%>
        <header class="headerTop no-print"><a style="float: left; padding-top: 2px;">Finance Park 2.0</a><a style="float: right; padding-right: 30px; padding-top: 2px;"><asp:Label ID="lblHeaderSchoolName" Text="School Name Here" runat="server"></asp:Label></a></header>

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
            <h2 class="h2">Create a Job</h2>
            <h3>This page allows you to create a new job position.
                <br />
                <br />
                Fill out the fields below and click the submit button to finish.
            </h3>
            <p>Job Title:</p>
            <asp:TextBox ID="tbJobTitle" runat="server" CssClass="textbox"></asp:TextBox>&ensp;<asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <p>Business:</p>
            <asp:DropDownList ID="ddlBusinessName" runat="server" CssClass="ddl"></asp:DropDownList>
            <p>Education Background:</p>
            <asp:DropDownList ID="ddlEducation" runat="server" CssClass="ddl">
                <asp:ListItem>Associate's Degree</asp:ListItem>
                <asp:ListItem>Bachelor's Degree</asp:ListItem>
                <asp:ListItem>Master's Degree</asp:ListItem>
                <asp:ListItem>High School Diploma</asp:ListItem>
                <asp:ListItem>Trade/Vocational School</asp:ListItem>
            </asp:DropDownList>
            <p>Job Duties:</p>
            <textarea id="tbDuties" runat="server" class="textbox"></textarea>
            <p>Educational Debt:</p>
            <asp:TextBox ID="tbEdDebt" runat="server" TextMode="Number" CssClass="textbox"></asp:TextBox>
            <p>Advancement:</p>
            <textarea id="tbAdvance" runat="server" class="textbox"></textarea>
            <br />
            <br />
            <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="Submit" OnClick="btnSubmit_Click" />
            <br />
        </div>

        <asp:HiddenField ID="hfCurrentVisitID" runat="server" />

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
