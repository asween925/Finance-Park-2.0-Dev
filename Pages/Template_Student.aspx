<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Template_Student.aspx.cs" Inherits="Template_Student" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - </title>

    <link href="../../CSS/StudentPages.css" rel="stylesheet" media="screen" type="text/css" />
    <link rel="shortcut icon" href="../../Media/faviconFP.png" type="image/ico" />
    <script src="https://code.jquery.com/jquery-1.10.2.js"></script>
</head>
<body>
    <form id="EMS_Form" runat="server">
        <div id="Site_Wrap_Fullscreen">
            
            <%--Header--%>
            <div id="header_div" runat="server" class="header">
                <div id="nav-placeholder"></div>
                <a class="headerTitle" >Title</a>
                <a><asp:Image ID="startLogo_img" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>               
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">
               
                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <a class="directions">Directions.</a>&ensp;
                    <a class="unlocked">
                        <asp:Label ID="businessUnlocked_lbl" runat="server" Font-Bold="true"></asp:Label>
                        of
                        <asp:Label ID="totalBusiness_lbl" runat="server" Font-Bold="true"></asp:Label>
                        businesses unlocked.</a>
                </div>

                <%--Research Table--%>
                <div class="content_div">
                    
                </div>
            </div>

            <%--Popup--%>
            <div id="popup">
                <p class="Sim_Research_Popup_Header">
                    <asp:Label ID="popupText_lbl" runat="server" Text="Popup Text"></asp:Label></p>
                <asp:TextBox ID="businessID_tb" runat="server" CssClass="popup_textbox" TextMode="Number"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="enter_btn" runat="server" Text="Enter" CssClass="button" OnClick="enter_btn_Click" /><asp:Button ID="cancel_btn" runat="server" OnClick="cancel_btn_Click" CssClass="buttonReset" Text="Cancel"></asp:Button>
            </div>

            <br /><br />          
        </div>

        <script type="text/javascript" src="../../Scripts/Scripts.js"></script>
        <script>
            $(function () {
                 $("#nav-placeholder").load("../../navsim.html");
            });
        </script>
    </form>
</body>
</html>
