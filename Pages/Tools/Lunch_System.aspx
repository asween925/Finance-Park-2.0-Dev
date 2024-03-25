<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Lunch_System.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Lunch_System" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Lunch System</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link href="../../CSS/Print.css" rel="stylesheet" media="print" type="text/css">
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
            <h2 class="h2 no-print">Lunch System</h2>
            <h3 class="no-print">This page allows you to control the lunch system for Finance Park.
                <br />
                <br />
                Enter a date of the simulation to see the students account numbers and pin numbers and check of if a student has received their lunch.
            </h3>
            <p>Visit Date:</p>
            <asp:TextBox ID="visitDate_tb" runat="server" TextMode="Date" CssClass="textbox" AutoPostBack="true" OnTextChanged="visitDate_tb_TextChanged"></asp:TextBox>&ensp;<asp:Label ID="error_lbl" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <br /><br />

            <%--Optional Gridview--%>
            <div id="lunches_div" runat="server" visible="true">
                <asp:GridView ID="lunches_dgv" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="true" DataKeyNames="ID" CellPadding="5" Height="50" AllowPaging="True" ShowHeaderWhenEmpty="True" Font-Size="Medium"  OnRowEditing="lunches_dgv_RowEditing" OnRowCancelingEdit="lunches_dgv_RowCancelingEdit" OnRowUpdating="lunches_dgv_RowUpdating" OnPageIndexChanging="lunches_dgv_PageIndexChanging" OnRowDataBound="lunches_dgv_RowDataBound" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Visible="true"> 
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                        <asp:BoundField DataField="pin" HeaderText="Pin #" Visible="true" readonly="true"/>
                        <asp:TemplateField HeaderText="Lunch<br />Served">
                            <ItemTemplate>
                               <asp:CheckBox ID="lunchServedDGV_chk" runat="server" Checked='<%#Bind("lunchServed") %>'></asp:CheckBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lunch<br />Ticket">
                            <ItemTemplate>
                                <asp:Label ID="lunchTicketDGV_lbl" runat="server" Text='<%#Bind("lunchTicket") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="lunchTicketDGV_ddl" runat="server" AutoPostBack="true" readonly="false">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>A</asp:ListItem>
                                    <asp:ListItem>B</asp:ListItem>
                                    <asp:ListItem>C</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
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
