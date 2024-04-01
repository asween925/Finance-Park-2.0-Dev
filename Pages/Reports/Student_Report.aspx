<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Student_Report.aspx.cs" Inherits="Student_Report" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Student Report</title>

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
            <h2 class="h2 no-print">Student Report</h2>
            <h3 class="no-print">Use this page to view all students on a visit date.
                <br class="no-print" />
                <br class="no-print" />
                Enter a visit date to view all students assigned to that date. Filter the students even more by selecting a school name from the drop down list.
            </h3>
            <p class="no-print">Visit Date:</p>
            <asp:TextBox ID="tbVisitDate" runat="server" TextMode="Date" CssClass="textbox no-print" OnTextChanged="tbVisitDate_TextChanged" AutoPostBack="true" ></asp:TextBox><a class=no-print>&ensp;</a><asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red" CssClass="no-print"></asp:Label>

            <%--School Name Section--%>
            <div id="divSchoolName" runat="server" visible="false" class="no-print">
                <p>School Name:</p>
                <asp:DropDownList ID="ddlSchoolName" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlSchoolName_SelectedIndexChanged"></asp:DropDownList>
                <br />
                <br />
                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="button no-print" OnClick="btnPrint_Click" />&ensp;<asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="button no-print" OnClick="btnRefresh_Click" />
            </div>
            <br class="no-print" />

            <%--Print Only Header--%>
            <div id="divPrintHeader" runat="server" visible="false">
                <h3 class="letter_title">FP Attendance Form (<asp:Label ID="lblPrintVisitDate" runat="server"></asp:Label><asp:Label ID="lblPrintSchool" runat="server" ></asp:Label>)</h3>
                <br /><br />
            </div>

            <%--Optional Gridview--%>
            <div id="divStudents" runat="server" visible="true">
                <asp:GridView ID="dgvStudents" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" CellPadding="5" Height="50" AllowPaging="True" ShowHeaderWhenEmpty="True" Font-Size="Medium" OnPageIndexChanging="dgvStudents_PageIndexChanging" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Visible="true"> 
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                        <asp:BoundField DataField="accountNum" HeaderText="Account Number" Visible="true" />
                        <asp:BoundField DataField="pin" HeaderText="Pin #" Visible="true" />
                        <asp:BoundField DataField="firstName" HeaderText="First Name" Visible="true" />
                        <asp:BoundField DataField="lastName" HeaderText="Last Name" Visible="true" />
                        <asp:BoundField DataField="maritalStatus" HeaderText="Marital Status" Visible="true" />
                        <asp:BoundField DataField="numOfChildren" HeaderText="Number of Children" Visible="true" />
                        <asp:BoundField DataField="educationBG" HeaderText="Education BG" Visible="true" />
                        <asp:BoundField DataField="businessName" HeaderText="Business Name" Visible="true" />
                        <asp:BoundField DataField="jobTitle" HeaderText="Job" Visible="true" />
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
