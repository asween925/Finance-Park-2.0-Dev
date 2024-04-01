<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Business_Assignments.aspx.cs" Inherits="Business_Assignments" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Business Assignments</title>

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
            <h2 class="h2 no-print">Business Assignments</h2>
            <h3 class="no-print">Use this page to print out the business assignment sheets for each business in FP.
                <br class="no-print" />
                <br class="no-print" />
                Enter a visit date and select a business to view the list of students assigned to that business. Click the Print button to print out the list.
            </h3>
            <p class="no-print">Visit Date:</p>
            <asp:TextBox ID="tbVisitDate" runat="server" TextMode="Date" CssClass="textbox no-print" OnTextChanged="tbVisitDate_TextChanged" AutoPostBack="true" ></asp:TextBox><a class=no-print>&ensp;</a><asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red" CssClass="no-print"></asp:Label>

            <%--School Name Section--%>
            <div id="divBusinessName" runat="server" visible="false" class="no-print">
                <p>Business Name:</p>
                <asp:DropDownList ID="ddlBusinessName" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlBusinessName_SelectedIndexChanged"></asp:DropDownList>
                <br />
                <br />
                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="button no-print" OnClick="btnPrint_Click" />&ensp;<asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="button no-print" OnClick="btnRefresh_Click" />
            </div>
            <br class="no-print" />

            <%--Print Only Header--%>
            <div id="divPrintHeader" runat="server" visible="true">
                <h3 class="letter_title">Business Assignment Sheet (<asp:Label ID="lblPrintVisitDate" runat="server"></asp:Label><asp:Label ID="lblPrintSchool" runat="server" ></asp:Label>)</h3>
                <br /><br />
            </div>

            <%--Business Logo--%>
            <div id="divBusinessLogo" runat="server" visible="true">
                <img id="imgBusinessLogo" runat="server" class="Biz_Assign_Logo_Top" src="~/Media/FP_Logo.png" />
            </div>

            <%--Optional Gridview--%>
            <div id="divStudents" runat="server" visible="true">
                <asp:GridView ID="dgvStudents" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" CellPadding="5" Height="50" AllowPaging="True" ShowHeaderWhenEmpty="True" Font-Size="Medium" OnRowDataBound="dgvStudents_OnRowDataBound" OnPageIndexChanging="dgvStudents_PageIndexChanging" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Visible="true"> 
                    <Columns>
                        <asp:TemplateField HeaderText="Student Count">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                        <asp:BoundField DataField="accountNum" HeaderText="Account Number" Visible="true" />
                        <asp:BoundField DataField="pin" HeaderText="Pin #" Visible="true" />
                        <asp:BoundField DataField="studentName" HeaderText="Student Name" Visible="true" />
                    </Columns>
                </asp:GridView>
            </div>

            <br />
            <asp:Image ID="imgStavrosLogo" runat="server" ImageUrl="~/Media/FP_SI_Logo.png" CssClass="letter_logo_bottom" />
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
