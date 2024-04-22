<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Calculations.aspx.cs" Inherits="Sim_Calculations" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - Calculations</title>

    <link href="../../CSS/StudentPages.css" rel="stylesheet" media="screen" type="text/css" />
    <link rel="shortcut icon" href="../../Media/faviconFP.png" type="image/ico" />
    <script src="https://code.jquery.com/jquery-1.10.2.js"></script>
</head>
<body>
    <form id="EMS_Form" runat="server">
        <div id="Site_Wrap_Fullscreen">
            
            <%--Header--%>
            <div id="divHeader" runat="server" class="header">
                <div id="nav-placeholder"></div>
                <a class="headerTitle" >Calculations</a>
                <a><asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>               
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">
               
                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <a class="directions">Select the taxes on the left side of the screen to calculate your net monthly income after tax.</a>&ensp;
                    <a class="unlocked"></a>
                </div>

                <%--Content--%>
                <div class="Sim_Content_With_Sponsor">

                    <%--Content - Right Block--%>
                    <div class="Sim_Content_Right">

                    </div>

                    <%--Content - Left Block--%>
                    <div class="Sim_Sponsor">
                        <div class="Sim_Sponsor_Header">
                            <p>Taxes</p>
                        </div>
                        <br />
                        <div class="Sim_Sponsor_Content">
                            <asp:Button ID="btnTaxesFed" runat="server" Text="Federal Withholding" />
                            <asp:Button ID="btnTaxesMedicare" runat="server" Text="Medicare" />
                            <asp:Button ID="btnTaxesSS" runat="server" Text="Social Security" />
                        </div>                        
                    </div>
                </div>

                    
            </div>

            <%--Popup--%>
            <div id="popup">
                <p class="Sim_Research_Popup_Header">
                    <asp:Label ID="lblPopupText" runat="server" Text="Popup Text"></asp:Label></p>
                <asp:TextBox ID="tbBusinessID" runat="server" CssClass="popup_textbox" TextMode="Number"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="btnEnter" runat="server" Text="Enter" CssClass="button" OnClick="btnEnter_Click" /><asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="buttonReset" Text="Cancel"></asp:Button>
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
