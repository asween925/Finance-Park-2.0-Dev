<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Login.aspx.cs" Inherits="Sim_Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - Start</title>

    <link href="../../CSS/StudentPages.css" rel="stylesheet" media="screen" type="text/css" />
    <link rel="shortcut icon" href="../../Media/faviconFP.png" type="image/ico" />

</head>
<body>
    <form id="EMS_Form" runat="server">
        <div id="Site_Wrap_Fullscreen">

            <%--Blur area--%>
            <div id="blur" class="blurArea">

                <%--Header--%>
                <div id="divHeader" runat="server" class="headerStart">
                    <asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_SI_Logo.png" CssClass="Sim_Start_Logo" />
                </div>

                <%--Login Div--%>
                <div id="divLogin" runat="server" class="Sim_Login_Div">
                    <h4 class="Sim_Login_Top">LOGIN</h4>
                    <p>Account Number:
                        <asp:TextBox ID="tbAcctNum" runat="server" CssClass="textbox Sim_Login_Fields" Width="80px" TextMode="Number"></asp:TextBox></p>
                    <p>First Name:
                        <asp:TextBox ID="tbFirstName" runat="server" CssClass="textbox Sim_Login_Fields"></asp:TextBox></p>
                    <p>Last Name:
                        <asp:TextBox ID="tbLastName" runat="server" CssClass="textbox Sim_Login_Fields"></asp:TextBox></p>
                    <p>
                        Gender:
                        <asp:DropDownList ID="ddlGender" runat="server" CssClass="ddl Sim_Login_Fields">
                            <asp:ListItem>Male</asp:ListItem>
                            <asp:ListItem>Female</asp:ListItem>
                            <asp:ListItem>Other</asp:ListItem>
                        </asp:DropDownList>
                    </p>
                    <p>School:
                        <asp:DropDownList ID="ddlSchoolName" runat="server" CssClass="ddl Sim_Login_Fields" AutoPostBack="true" OnSelectedIndexChanged="ddlSchoolName_SelectedIndexChanged"></asp:DropDownList></p>
                    <p>Teacher:
                        <asp:DropDownList ID="ddlTeacher" runat="server" CssClass="ddl Sim_Login_Fields"></asp:DropDownList></p>
                    <p>Grade:
                        <asp:TextBox ID="tbGrade" runat="server" CssClass="textbox Sim_Login_Fields" TextMode="Number" Width="40px" Text="8"></asp:TextBox></p>
                    <p>
                        <asp:Button ID="btnLogin" runat="server" CssClass="button Sim_Login_Fields" Text="Login" OnClick="btnLogin_Click" />&ensp;<asp:Button ID="btnReset" runat="server" CssClass="buttonReset Sim_Login_Fields" Text="Reset" OnClick="btnReset_Click" /></p>
                </div>
                <br />
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="X-Large"></asp:Label>
            </div>

            <%--Popup--%>
            <div id="popup">
                <p class="popup_header"><asp:Label ID="lblPopupText" runat="server" Text="Please enter your PIN:"></asp:Label></p>
                <asp:TextBox ID="tbPin" runat="server" CssClass="popup_textbox" Width="70px" TextMode="Number"></asp:TextBox>
                <br /><br />                
                <asp:Button ID="btnEnter" runat="server" Text="Enter" CssClass="button" OnClick="btnEnter_Click" /><asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="buttonReset" Text="Cancel"></asp:Button>
            </div>
        </div>
        <script type="text/javascript" src="../../Scripts/Scripts.js"></script>
    </form>
</body>
</html>
