<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Kit_Report.aspx.cs" Inherits="Kit_Report" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Kit Report</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link href="../../CSS/Print.css" rel="stylesheet" media="print" type="text/css">
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
            <h2 class="h2 no-print">Kit Report</h2>
            <h3 class="no-print">This page allows you to view all kit numbers assigned to a school.
                <br />
                <br />
                Select a school name from the drop down menu to view kits from that school. Click Print to print out a list. Kit numbers can be assigned via the School Visit Checklist found under Tools.
            </h3>
            <p>School Name:</p>
            <asp:DropDownList ID="ddlSchoolName" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlSchoolName_SelectedIndexChanged"></asp:DropDownList>&ensp;<asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <br />
            <br />
            <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="button no-print" OnClick="btnPrint_Click" />
            <br /><br />

            <%--Optional Gridview--%>
            <div id="divKits" runat="server" visible="true">                
                <asp:GridView ID="dgvKits" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" CellPadding="5" Height="50" PageSize="15" AllowPaging="True" ShowHeaderWhenEmpty="True" Font-Size="Medium" OnPageIndexChanging="dgvKits_PageIndexChanging" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Visible="true"> 
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                        <asp:BoundField DataField="visitDate" HeaderText="Visit Date" Visible="true" DataFormatString="{0: MM/dd/yyyy }" />
                        <asp:BoundField DataField="schoolName" HeaderText="School Name" Visible="true" />
                        <asp:BoundField DataField="workbooks" HeaderText="Workbooks" Visible="true" />
                        <asp:BoundField DataField="kitTotal" HeaderText="Number of Kits Sent" Visible="true" />
                        <asp:BoundField DataField="kits" HeaderText="Kit Numbers" Visible="true" />
                    </Columns>
                </asp:GridView>
            </div>
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
