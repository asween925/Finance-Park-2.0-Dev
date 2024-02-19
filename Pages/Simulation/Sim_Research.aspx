<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Research.aspx.cs" Inherits="Sim_Research" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;"/>

    <title>Finance Park - Research</title>

    <link href="../../CSS/StudentPages.css" rel="stylesheet" media="screen" type="text/css"/>
    <link rel="shortcut icon" href="../../Media/faviconFP.png" type="image/ico" />
    
</head>
<body>
    <form id="EMS_Form" runat="server">
        <div id="Site_Wrap_Fullscreen">

            <%--Header--%>
            <div id="header_div" runat="server" class="header">
                <asp:Image ID="startLogo_img" runat="server" ImageUrl="~/Media/FP_SI_Logo.png" />
            </div>
            <br />
                  
            <%--Research Table--%>
            <div class="content_div">
                <table class="Sim_Research_Table">
                    <tbody>
                      <tr>
                        <td><button id="autoInsurance_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/Auto.png')">Auto Insurance</button></td>
                        <td><button id="bankMort_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/BankMort.png')">Bank Mortgage</button></td>
                        <td><button id="bankSave_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/BankSave.png')">Bank Savings</button></td>
                      </tr>
                      <tr>
                        <td ><button id="childcare_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/Childcare.png')">Childcare</button></td>
                        <td ><button id="clothing_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/Clothing.png')">Clothing</button></td>
                        <td ><button id="credit_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/.png')">Credit Card</button></td>
                      </tr>
                      <tr>
                        <td ><button id="dining_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/DiningOut.png')">Dining Out</button></td>
                        <td ><button id="education_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/Education.png')">Education</button></td>
                        <td ><button id="entertainment_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/Entertainment.png')">Entertainment</button></td>
                      </tr>
                      <tr>
                        <td ><button id="gas_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/Gas.png')">Gas and Maintenance</button></td>
                        <td ><button id="grocery_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/Grocery.png')">Grocery</button></td>
                        <td ><button id="homeimp_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/HomeImprov.png')">Home Improvement</button></td>
                      </tr>
                      <tr>
                        <td ><button id="homeins_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/HomeIns.png')">Home Insurance</button></td>
                        <td ><button id="housing_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/Housing.png')">Housing</button></td>
                        <td ><button id="internet_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/Internet.png')">Internet</button></td>
                      </tr>
                      <tr>
                        <td ><button id="investment" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/.png')">Investment</button></td>
                        <td ><button id="phila_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/Philan.png')">Philanthropy</button></td>
                        <td ><button id="phone_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/.png')">Phone</button></td>
                      </tr>
                        <tr>
                          <td ><button id="thatslife_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/ThatsLife.png')">That's Life</button></td>
                          <td ><button id="transport_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/.png')">Transportation</button></td>
                          <td ><button id="utiwater_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/UtilityWater.png')">Utilities: Water, Sewer, Trash</button></td>
                        </tr>
                        <tr>
                          <td ><button id="utielec_btn" runat="server" class="Sim_Research_Button" style="background-image: url('../../Media/Business Icons/UtilityElc.png')">Utilities: Electricity</button></td>
                        </tr>
                    </tbody>
                </table>

            </div>

        </div>
    </form>
</body>
</html>
