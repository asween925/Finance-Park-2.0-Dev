<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Business.aspx.cs" Inherits="Template_Student" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;"/>

    <title>Finance Park - Business</title>

    <link href="../../CSS/StudentPages.css" rel="stylesheet" media="screen" type="text/css"/>
    <link rel="shortcut icon" href="../../Media/faviconFP.png" type="image/ico" />
    
</head>
<body>
    <form id="EMS_Form" runat="server">
        <div id="Site_Wrap_Fullscreen">

            <%--Header--%>
            <div id="divHeader" runat="server" class="header">
                <div id="nav-placeholder"></div>
                <a class="headerTitle" >Business Information</a>
                <a><asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>               
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">

                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <asp:Image ID="imgBusinessLogo" runat="server" /><asp:Label ID="lblBusinessName" runat="server"></asp:Label>
                    <a class="unlocked">
                        <asp:Label ID="lblBusinessUnlocked" runat="server"></asp:Label>
                        of
                        <asp:Label ID="lblTotalBusiness" runat="server" Font-Bold="true"></asp:Label>
                        businesses unlocked.&ensp;<asp:Button ID="rebtnSearch" runat="server" Text="Unlock More" CssClass="button" /></a>                   
                </div>          

                <%--Content--%>
                <div class="Sim_Business_Content">

                    <%--Sponsor--%>
                    <div class="Sim_Business_Sponsor">
                        <div class="Sim_Business_Sponsor_Header">
                            <p>Sponsored By:</p>
                        </div>
                        <asp:Image ID="imgSponsorLogo" runat="server" />
                    </div>

                    <%--Script--%>
                    <div class="Sim_Business_Script">
                        <asp:Label ID="lblKioskScript" runat="server" Text="The kiosk script for the business will go here." ></asp:Label>
                    </div>
                </div>
                
            </div>
          
            <%--Popup--%>
            <div id="popup">
                <p class="popup_header"></p>
                <p><asp:Label ID="lblPopupText" runat="server"></asp:Label></p>
                <br /><br />
                <button onclick="toggle(); return false;" class="button">Okay</button>
            </div>
                  

        </div>
        <script type="text/javascript" src="../../Scripts/Scripts.js"></script>
    </form>
</body>
</html>
