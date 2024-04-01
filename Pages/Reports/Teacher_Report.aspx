<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Teacher_Report.aspx.cs" Inherits="Teacher_Report" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Teacher Report</title>

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
            <h2 class="h2 no-print">Teacher Report</h2>
            <h3 class="no-print">This page displays all teachers in the database. Select the school the teacher belongs to from the drop down menu below.
            </h3>
            <p>School Name:</p>
            <asp:DropDownList ID="ddlSchoolName" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlSchoolName_SelectedIndexChanged"></asp:DropDownList>&ensp;<asp:Label ID="lblError" runat="server" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <p>Type in First or Last Name:</p>
            <asp:TextBox ID="tbSearch" runat="server" CssClass="textbox"></asp:TextBox>&nbsp;&nbsp;<asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Search" OnClick="btnSearch_Click" />           
            <br /><br />          
            <asp:Button ID="btnRefresh" runat="server" CssClass="button" Text="Show All Teachers" OnClick="btnRefresh_Click" />
            <br /><br />       
            <%--Print Out Table--%>
            <div>
                <asp:GridView ID="dgvTeachers" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="false" DataKeyNames="ID" CellPadding="5" Height="50" AllowPaging="True" OnPageIndexChanging="dgvTeachers_PageIndexChanging" ShowHeaderWhenEmpty="True" Font-Size="Medium" Visible="true" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" Visible="true" />                        
                        <asp:BoundField DataField="firstName" HeaderText="First Name" Visible="true" />
                        <asp:BoundField DataField="lastName" HeaderText="Last Name" Visible="true" />
                        <asp:BoundField DataField="email" HeaderText="Email" Visible="true" />                        
                        <asp:BoundField DataField="password" HeaderText="Password" Visible="true" />
                        <asp:BoundField DataField="schoolName" HeaderText="School Name" Visible="true" />
                        <asp:BoundField DataField="contact" HeaderText="Contact Teacher?" Visible="true" />
                        <asp:BoundField DataField="currVisitDate" HeaderText="Current<br />Visit Date" DataFormatString="{0: MM/dd/yyyy } " HtmlEncode="false" />
                        <asp:BoundField DataField="prevVisitDate" HeaderText="Previous<br />Visit Date" DataFormatString="{0: MM/dd/yyyy } " HtmlEncode="false" />
                    </Columns>
                </asp:GridView>
                <br />
            </div>
        </div>

        <asp:HiddenField ID="hfVisitDate" runat="server" />
        <asp:HiddenField ID="hfTeachers" runat="server" />
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
