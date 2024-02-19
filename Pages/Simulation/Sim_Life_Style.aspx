<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Life_Style.aspx.cs" Inherits="Sim_Life_Style" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;"/>

    <title>Finance Park - Life Style Questions</title>

    <link href="../../CSS/StudentPages.css" rel="stylesheet" media="screen" type="text/css"/>
    <link rel="shortcut icon" href="../../Media/faviconFP.png" type="image/ico" />
    
</head>
<body>
    <form id="EMS_Form" runat="server">
        <div id="Site_Wrap_Fullscreen">

            <%--Logo--%>
            <div id="startLogo_div" runat="server" class="header">
                <asp:Image ID="startLogo_img" runat="server" ImageUrl="~/Media/FP_SI_Logo.png" />
            </div>
            <br />
                
            <%--Question 1--%>
            <div class="content_div">
                <h3 class="underline"><asp:Label ID="q1_lbl" runat="server"></asp:Label></h3>
                
                <%--Questions--%>
                <%--Going to do radio buttons so the kids only select 1 answer--%>
                <div>
                    <label>
                        <input type="radio" name="q1a1" class="" />
                    </label>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
