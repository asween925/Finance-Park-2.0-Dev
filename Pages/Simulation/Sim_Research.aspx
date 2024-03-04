<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Research.aspx.cs" Inherits="Sim_Research" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - Research</title>

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
                <a class="headerTitle" >Research</a>
                <a><asp:Image ID="startLogo_img" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>               
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">
               
                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <a class="directions">Go to each business below, tap on the associated icon and enter the unique Business ID to unlock each expense category.</a>&ensp;
                    <a class="unlocked">
                        <asp:Label ID="businessUnlocked_lbl" runat="server" Font-Bold="true"></asp:Label>
                        of
                        <asp:Label ID="totalBusiness_lbl" runat="server" Font-Bold="true"></asp:Label>
                        businesses unlocked.</a>
                </div>

                <%--Research Table--%>
                <div class="content_div">
                    <table class="Sim_Research_Table">
                        <tbody>
                            <tr>
                                <td>
                                    <asp:Button ID="autoInsurance_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Auto.png')" Text="Auto Insurance" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="bankMort_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/BankMort.png')" Text="Bank Mortgage" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="bankSave_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/BankSave.png')" Text="Bank Savings" OnClientClick="toggle(); return false;"></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="childcare_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Childcare.png')" Text="Childcare" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="clothing_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Clothing.png')" Text="Clothing" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="credit_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/.png')" Text="Credit Card" OnClientClick="toggle(); return false;"></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="dining_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/DiningOut.png')" Text="Dining Out" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="education_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Education.png')" Text="Education" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="entertainment_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Entertainment.png')" Text="Entertainment" OnClientClick="toggle(); return false;"></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="gas_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Gas.png')" Text="Gas and Maintenance" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="grocery_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Grocery.png')" Text="Grocery" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="homeimp_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/HomeImprov.png')" Text="Home Improvement" OnClientClick="toggle(); return false;"></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="homeins_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/HomeIns.png')" Text="Home Insurance" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="housing_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Housing.png')" Text="Housing" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="internet_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Internet.png')" Text="Internet" OnClientClick="toggle(); return false;"></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="investment" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/.png')" Text="Investment" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="phila_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Philan.png')" Text="Philanthropy" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="phone_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/.png')" Text="Phone" OnClientClick="toggle(); return false;"></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="thatslife_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/ThatsLife.png')" Text="That's Life" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="transport_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/.png')" Text="Transportation" OnClientClick="toggle(); return false;"></asp:Button></td>
                                <td>
                                    <asp:Button ID="utiwater_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/UtilityWater.png')" Text="Utilities: Water, Trash" OnClientClick="toggle(); return false;"></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="utielec_btn" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/UtilityElc.png')" Text="Utilities: Electricity" OnClientClick="toggle(); return false;"></asp:Button></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <%--Popup--%>
            <div id="popup">
                <p class="popup_header">
                    <asp:Label ID="popupText_lbl" runat="server" Text="Please enter the Business ID to proceed:"></asp:Label></p>
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
