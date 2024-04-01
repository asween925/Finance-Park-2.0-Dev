<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Start.aspx.cs" Inherits="Sim_Start" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;"/>

    <title>Finance Park - Start</title>

    <link href="../../CSS/StudentPages.css" rel="stylesheet" media="screen" type="text/css"/>
    <link rel="shortcut icon" href="../../Media/faviconFP.png" type="image/ico" />
    
</head>
<body>
    <form id="EMS_Form" runat="server">
        <div id="Site_Wrap_Fullscreen">

            <%--Logo--%>
            <div id="divStartLogo" runat="server" class="Sim_Start_Logo_Back">
                <asp:Image ID="imgStartLogo" runat="server" ImageUrl="../../Media/FP_SI_Logo.png" CssClass="Sim_Start_Logo" AlternateText="Finance Park Logo" />
            </div>
            <br />

            <%--Start Button--%>
            <div id="divStartSim" runat="server" class="Sim_Start_Btn_Div">
                <asp:Button ID="btnStartSim" runat="server" CssClass="buttonLarge Sim_Start_Btn" Text="Start!" OnClick="btnStartSim_Click" />
            </div>          

        </div>
    </form>
</body>
</html>
