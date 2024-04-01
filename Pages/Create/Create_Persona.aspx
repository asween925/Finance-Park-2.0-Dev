<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Create_Persona.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="Create_Persona" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Create a Persona</title>

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
            <h2 class="h2">Create a Persona</h2>
            <h3>This page allows you to create a new persona.
            </h3>
            <p>Job Title:</p>
            <asp:DropDownList ID="ddlJobTitle" runat="server" CssClass="ddl"></asp:DropDownList>&ensp;<asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <p>Job Type:</p>
            <asp:DropDownList ID="ddlJobType" runat="server" CssClass="ddl">
                <asp:ListItem>Part Time</asp:ListItem>
                <asp:ListItem>Full Time</asp:ListItem>
            </asp:DropDownList>
            <p>GAI:</p>
            <asp:TextBox ID="tbGAI" runat="server" CssClass="textbox" TextMode="Number"></asp:TextBox>
            <p>Age:</p>
            <asp:TextBox ID="tbAge" runat="server" CssClass="textbox" TextMode="Number" Width="50px"></asp:TextBox>
            <p>Martial Status:</p>
            <asp:DropDownList ID="ddlMartialStatus" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlMartialStatus_SelectedIndexChanged">
                <asp:ListItem>Single</asp:ListItem>
                <asp:ListItem>Married</asp:ListItem>
            </asp:DropDownList>
            <p id="pSpouseAge" runat="server" visible="false">Spouse Age:</p>
            <asp:Textbox ID="tbSpouseAge" runat="server" TextMode="Number" Width="50px" CssClass="textbox" Visible="false"></asp:Textbox>
            <p>Number of Children:</p>
            <asp:DropDownList ID="ddlNumOfChild" runat="server" TextMode="Number" AutoPostBack="true" CssClass="ddl" OnSelectedIndexChanged="ddlNumOfChild_SelectedIndexChanged" >
                <asp:ListItem>0</asp:ListItem>
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
            </asp:DropDownList>
            <p id="pChild1" runat="server" visible="false">Child #1 Age</p>
            <asp:TextBox ID="tbChild1" runat="server" Visible="false" CssClass="textbox" TextMode="Number" Text="0" ></asp:TextBox>
            <p id="pChild2" runat="server" visible="false">Child #2 Age</p>
            <asp:TextBox ID="tbChild2" runat="server" Visible="false" CssClass="textbox" TextMode="Number" Text="0" ></asp:TextBox>
            <p>Credit Score:</p>
            <asp:Textbox ID="tbCreditScore" runat="server" TextMode="Number" Width="50px" CssClass="textbox"></asp:Textbox>
            <p>NMI:</p>
            <asp:Textbox ID="tbNMI" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>CC Debt:</p>
            <asp:Textbox ID="tbCCDebt" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Furniture Limit:</p>
            <asp:Textbox ID="tbFurnLimit" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Home Improvment Limit:</p>
            <asp:Textbox ID="tbHomeImp" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Longterm Savings:</p>
            <asp:Textbox ID="tbLongSave" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Emergency Funds:</p>
            <asp:Textbox ID="tbEmerFunds" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Other Savings:</p>
            <asp:Textbox ID="tbOtherSave" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Auto Loan Limit:</p>
            <asp:Textbox ID="tbAutoLoan" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Mortgage Amount:</p>
            <asp:Textbox ID="tbMortAmnt" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>That's Life Amount:</p>
            <asp:Textbox ID="tbThatsAmnt" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Description:</p>
            <asp:Textbox ID="tbDesc" runat="server" CssClass="textbox" TextMode="MultiLine" Height="102px" Width="288px"></asp:Textbox>
            <br /><br />
            <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="Submit" OnClick="btnSubmit_Click" />&ensp;<asp:Label ID="lblSuccess" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <br /><br />
          
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
