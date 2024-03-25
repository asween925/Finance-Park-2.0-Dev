<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Simulation_Report.aspx.cs" Inherits="Simulation_Report" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Simulation Report</title>

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
            <h2 class="h2">Simulation Report</h2>
            <h3 class="no-print">This page will allow you to view all existing simulations in the database.</h3>
            <p class="no-print">Visit Date:</p>
            <asp:TextBox ID="visitDate_tb" runat="server" TextMode="Date" AutoPostBack="true" CssClass="textbox no-print" OnTextChanged="visitDate_tb_TextChanged"></asp:TextBox><a class="no-print">&emsp;</a><asp:Label ID="error_lbl" runat="server" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <p class="no-print">School Name:</p>
            <asp:DropDownList CssClass="ddl no-print" ID="schoolName_ddl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="schoolName_ddl_SelectedIndexChanged"></asp:DropDownList>
            <p class="no-print">Month:</p>
            <asp:DropDownList ID="month_ddl" runat="server" CssClass="ddl no-print" AutoPostBack="true" OnSelectedIndexChanged="month_ddl_SelectedIndexChanged">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>January</asp:ListItem>
                <asp:ListItem>February</asp:ListItem>
                <asp:ListItem>March</asp:ListItem>
                <asp:ListItem>April</asp:ListItem>
                <asp:ListItem>May</asp:ListItem>
                <asp:ListItem>June</asp:ListItem>
                <asp:ListItem>July</asp:ListItem>
                <asp:ListItem>August</asp:ListItem>
                <asp:ListItem>September</asp:ListItem>
                <asp:ListItem>October</asp:ListItem>
                <asp:ListItem>November</asp:ListItem>
                <asp:ListItem>December</asp:ListItem>
            </asp:DropDownList>
            <br class="no-print" /><br class="no-print" />
            <asp:Button ID="refresh_btn" runat="server" CssClass="button no-print" Text="Show All Visits" OnClick="refresh_btn_Click" /><a class="no-print">&ensp;|&ensp;</a><asp:Button ID="print_btn" runat="server" CssClass="button no-print" Text="Print" />
            <br class="no-print" /><br class="no-print" />
            
            <%--Table--%>
            <div>
                <asp:GridView ID="visit_dgv" runat="server" AutoGenerateColumns="False" AllowPaging="True" CellPadding="3" Font-Size="Medium" Height="50px" PageSize="10" OnPageIndexChanging="visit_dgv_PageIndexChanging" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="ID" ReadOnly="true" Visible="true" />
                        <asp:BoundField DataField="visitDate" HeaderText="Visit Date" ReadOnly="true" Visible="true" DataFormatString="{0: MM-dd-yyyy }" />
                        <asp:BoundField DataField="visitTime" HeaderText="Visit Time" ReadOnly="true" Visible="true" />
                        <asp:BoundField DataField="School #1" HeaderText="School" ReadOnly="true" Visible="true" />
                        <asp:BoundField DataField="School #2" HeaderText="School #2" ReadOnly="true" Visible="true" />
                        <asp:BoundField DataField="School #3" HeaderText="School #3" ReadOnly="true" Visible="true" />
                        <asp:BoundField DataField="School #4" HeaderText="School #4" ReadOnly="true" Visible="true" />
                        <asp:BoundField DataField="School #5" HeaderText="School #5" ReadOnly="true" Visible="true" />
                        <asp:BoundField DataField="studentCount" HeaderText="Student Count" ReadOnly="true" Visible="true" />
                        <asp:BoundField DataField="vTrainingTime" HeaderText="Volunteer Training Start" ReadOnly="true" Visible="true" />
                        <asp:BoundField DataField="vMinCount" HeaderText="Volunteer Min Count" ReadOnly="true" Visible="true" />
                        <asp:BoundField DataField="vMaxCount" HeaderText="Volunteer Max Count" ReadOnly="true" Visible="true" />                    
                    </Columns>
                </asp:GridView>
            </div>
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
        <asp:SqlDataSource ID="print_sds" runat="server"></asp:SqlDataSource>
    </form>
</body>
</html>
