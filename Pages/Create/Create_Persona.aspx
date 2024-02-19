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
            <h2 class="h2">Create a Persona</h2>
            <h3>This page allows you to create a new persona.
            </h3>
            <p>Job Title:</p>
            <asp:DropDownList ID="jobTitle_ddl" runat="server" CssClass="ddl"></asp:DropDownList>&ensp;<asp:Label ID="error_lbl" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <p>Job Type:</p>
            <asp:DropDownList ID="jobType_ddl" runat="server" CssClass="ddl">
                <asp:ListItem>Part Time</asp:ListItem>
                <asp:ListItem>Full Time</asp:ListItem>
            </asp:DropDownList>
            <p>GAI:</p>
            <asp:TextBox ID="gai_tb" runat="server" CssClass="textbox" TextMode="Number"></asp:TextBox>
            <p>Age:</p>
            <asp:TextBox ID="age_tb" runat="server" CssClass="textbox" TextMode="Number" Width="50px"></asp:TextBox>
            <p>Martial Status:</p>
            <asp:DropDownList ID="martialStatus_ddl" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="martialStatus_ddl_SelectedIndexChanged">
                <asp:ListItem>Single</asp:ListItem>
                <asp:ListItem>Married</asp:ListItem>
            </asp:DropDownList>
            <p id="spouseAge_p" runat="server" visible="false">Spouse Age:</p>
            <asp:Textbox ID="spouseAge_tb" runat="server" TextMode="Number" Width="50px" CssClass="textbox" Visible="false"></asp:Textbox>
            <p>Number of Children:</p>
            <asp:DropDownList ID="numOfChild_ddl" runat="server" TextMode="Number" AutoPostBack="true" CssClass="ddl" OnSelectedIndexChanged="numOfChild_ddl_SelectedIndexChanged" >
                <asp:ListItem>0</asp:ListItem>
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
            </asp:DropDownList>
            <p id="child1_p" runat="server" visible="false">Child #1 Age</p>
            <asp:TextBox ID="child1_tb" runat="server" Visible="false" CssClass="textbox" TextMode="Number" Text="0" ></asp:TextBox>
            <p id="child2_p" runat="server" visible="false">Child #2 Age</p>
            <asp:TextBox ID="child2_tb" runat="server" Visible="false" CssClass="textbox" TextMode="Number" Text="0" ></asp:TextBox>
            <p>Credit Score:</p>
            <asp:Textbox ID="creditScore_tb" runat="server" TextMode="Number" Width="50px" CssClass="textbox"></asp:Textbox>
            <p>NMI:</p>
            <asp:Textbox ID="nmi_tb" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>CC Debt:</p>
            <asp:Textbox ID="ccdebt_tb" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Furniture Limit:</p>
            <asp:Textbox ID="furnLimit_tb" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Home Improvment Limit:</p>
            <asp:Textbox ID="homeImp_tb" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Longterm Savings:</p>
            <asp:Textbox ID="longSave_tb" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Emergency Funds:</p>
            <asp:Textbox ID="emerFunds_tb" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Other Savings:</p>
            <asp:Textbox ID="otherSave_tb" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Auto Loan Limit:</p>
            <asp:Textbox ID="autoLoan_tb" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Mortgage Amount:</p>
            <asp:Textbox ID="mortAmnt_tb" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>That's Life Amount:</p>
            <asp:Textbox ID="thatsAmnt_tb" runat="server" TextMode="Number" Width="100px" CssClass="textbox"></asp:Textbox>
            <p>Description:</p>
            <asp:Textbox ID="desc_tb" runat="server" CssClass="textbox" TextMode="MultiLine" Height="102px" Width="288px"></asp:Textbox>
            <br /><br />
            <asp:Button ID="submit_btn" runat="server" CssClass="button" Text="Submit" OnClick="submit_btn_Click" />&ensp;<asp:Label ID="success_lbl" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
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
