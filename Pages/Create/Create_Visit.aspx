<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Create_Visit.aspx.cs" Inherits="Create_Visit" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Create a Visit</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <link rel="shortcut icon" type="image/png" href="../../Media/FP_favicon_2.png" />
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

        <%-- Content--%>
        <div class="content">
            <h2 class="h2">Create a Visit</h2>
            <h3>This page is used to create a new school visit.
                <br />
                <br />
                Enter the information currently known below. You do not need to fill out all the information, the empty fields will be a default value in the database. You can change this later on the 'Edit Visit' page under 'Edit'.
            </h3>

            <asp:Button ID="Button1" runat="server" Text="Open / Close Businesses" PostBackUrl="~/Open_Closed_Status.aspx" CssClass="button" />&ensp;<asp:Button ID="Button2" runat="server" Text="School Visit Checklist" PostBackUrl="~/school_visit_checklist.aspx" CssClass="button" />&ensp;<asp:Label runat="server" ID="error_lbl" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>           
            <p>Enter Date Of Visit (Required)</p>
            <asp:TextBox placeholder="MM/DD/YYYY" ID="visit_tb" Width="100px" runat="server" TextMode="Date" CssClass="textbox"></asp:TextBox>
            <br />
            <p>Enter Reply By Date</p>
            <asp:TextBox placeholder="MM/DD/YYYY" ID="replyBy_tb" Width="100px" TextMode="Date" runat="server" CssClass="textbox"></asp:TextBox>
            <br />
            <p>Select Visit Time (School Schedule)</p>
            <asp:DropDownList CssClass="ddl" ID="visitTime_ddl" runat="server" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="visitTime_ddl_SelectedIndexChanged1"></asp:DropDownList>
            <br />
            <p>Enter Approximate Student Count</p>
            <asp:TextBox TextMode="Number" ID="studentCount_tb" Width="45px" runat="server" CssClass="textbox"></asp:TextBox>
            <br />
            <p>Volunteer Training Start Time</p>
            <asp:TextBox ID="volunteerTime_tb" Width="40px" runat="server" CssClass="textbox" Text="7:05" Placeholder="00:00"></asp:TextBox>
            <br />
            <p>School #1 (Required)</p>
            <asp:DropDownList CssClass="ddl" ID="schools_ddl" runat="server"></asp:DropDownList>
            <p>School #2</p>
            <asp:DropDownList CssClass="ddl" ID="schools2_ddl" runat="server"></asp:DropDownList>
            <p>School #3</p>
            <asp:DropDownList CssClass="ddl" ID="schools3_ddl" runat="server"></asp:DropDownList>
            <p>School #4</p>
            <asp:DropDownList CssClass="ddl" ID="schools4_ddl" runat="server"></asp:DropDownList>
            <p>School #5</p>
            <asp:DropDownList CssClass="ddl" ID="schools5_ddl" runat="server"></asp:DropDownList>
            <br />
            <br />
            <asp:Button ID="Submit_btn" runat="server" Text="Submit" CssClass="button" OnClick="Submit_btn_Click1" />
            <br />
            <br />
        </div>

        <asp:HiddenField ID="visitdate_hf" runat="server" />
        <asp:HiddenField ID="visitdateUpdate_hf" runat="server" />

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
