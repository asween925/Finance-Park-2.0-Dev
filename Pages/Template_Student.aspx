<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Template_Student.aspx.cs" Inherits="Template_Student" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - </title>

    <link href="../../CSS/StudentPages.css" rel="stylesheet" media="screen" type="text/css" />
    <link rel="shortcut icon" href="../../Media/FP_favicon_2.png" type="image/ico" />
    <script src="https://code.jquery.com/jquery-1.10.2.js"></script>
</head>
<body>
    <form id="EMS_Form" runat="server">
        <div id="Site_Wrap_Fullscreen">
            
            <%--Header--%>
            <div id="divHeader" runat="server" class="header">
                <div id="nav-placeholder"></div>
                <a class="headerTitle" >Title</a>
                <a><asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>               
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">
               
                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <a class="directions">Directions.</a>&ensp;
                    <a class="unlocked">
                        <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label>
                        <asp:Label ID="lblBusinessUnlocked" runat="server" Font-Bold="true"></asp:Label>
                        of
                        <asp:Label ID="lblTotalBusiness" runat="server" Font-Bold="true"></asp:Label>
                        businesses unlocked.</a>
                </div>

                <%--Content--%>
                <div class="Sim_Content_With_Sponsor">

                    <%--Content - Right Block--%>
                    <div class="Sim_Content_Right">

                    </div>

                    <%--Content - Left Block--%>
                    <div class="Sim_Sponsor">
                        <div class="Sim_Sponsor_Header">
                            <p>Title</p>
                        </div>
                        <div class="Sim_Sponsor_Content">
                            
                        </div>
                    </div>
                    <br />

                    <%--Next button--%>
                    <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="button" OnClick="btnNext_Click" Visible="false" />
                </div>
            </div>

            <%--Popup--%>
            <div id="popup">
                <p class="popup_header">
                    <asp:Label ID="lblPopupText" runat="server" Text="Popup Text"></asp:Label></p>
                <asp:TextBox ID="tbPopupTB" runat="server" CssClass="popup_textbox" TextMode="Number"></asp:TextBox>
                <br />
                <asp:Label ID="lblErrorPopup" runat="server" CssClass="error_label"></asp:Label>
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
