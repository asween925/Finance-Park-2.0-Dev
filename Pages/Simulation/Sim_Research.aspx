<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Research.aspx.cs" Inherits="Sim_Research" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - Research</title>

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
                <a class="headerTitle" >Research</a>
                <a><asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>               
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">
               
                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <a class="directions">Go to each business below, tap on the associated icon and enter the unique Business ID to unlock each expense category.</a>&ensp;
                    <a class="unlocked">
                        <asp:Label ID="lblBusinessUnlocked" runat="server" Font-Bold="true" Text="0"></asp:Label>
                        of
                        <asp:Label ID="lblTotalBusiness" runat="server" Font-Bold="true" Text="22"></asp:Label>
                        businesses unlocked.</a>
                </div>

                <%--Research Table--%>
                <div class="Sim_Content">
                    <table class="Sim_Research_Table">
                        <tbody>
                            <tr>
                                <td>
                                    <asp:Button ID="btnAutoInsurance" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Auto Insurance" OnClick="btnAutoInsurance_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnBankMort" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Bank Mortgage" OnClick="btnBankMort_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnBankSave" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Bank Savings" OnClick="btnBankSave_Click" ></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnChildcare" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Childcare" OnClick="btnChildcare_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnClothing" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Clothing" OnClick="btnClothing_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnCredit" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Credit Card" OnClick="btnCredit_Click" ></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnDining" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Dining Out" OnClick="btnDining_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnEducation" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Education" OnClick="btnEducation_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnEntertainment" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Entertainment" OnClick="btnEntertainment_Click" ></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnGas" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Gas and Maintenance" OnClick="btnGas_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnGrocery" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Grocery" OnClick="btnGrocery_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnHomeimp" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Home Improvement" OnClick="btnHomeimp_Click" ></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnHomeins" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Home Insurance" OnClick="btnHomeins_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnHousing" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Housing" OnClick="btnHousing_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnInternet" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Internet" OnClick="btnInternet_Click" ></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnInvestment" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Investment" OnClick="btnInvestment_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnPhila" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Philanthropy" OnClick="btnPhila_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnPhone" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Phone" OnClick="btnPhone_Click" ></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnThatslife" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="That's Life" OnClick="btnThatslife_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnTransport" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Transportation" OnClick="btnTransport_Click" ></asp:Button></td>
                                <td>
                                    <asp:Button ID="btnUtiwater" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Utilities: Water, Trash" OnClick="btnUtiwater_Click" ></asp:Button></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Button ID="btnUtielec" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/Locked.png')" Text="Utilities: Electricity" OnClick="btnUtielec_Click" ></asp:Button></td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>

            <%--Popup--%>
            <div id="popup">
                <p class="popup_header">
                    <asp:Label ID="lblPopupText" runat="server" Text="Please enter the Business ID to proceed:"></asp:Label></p>
                <asp:TextBox ID="tbBusinessID" runat="server" CssClass="popup_textbox" TextMode="Number"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="btnEnter" runat="server" Text="Enter" CssClass="button" OnClick="btnEnter_Click" /><asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="buttonReset" Text="Cancel"></asp:Button>
            </div>

            <br /><br />    
            <asp:HiddenField ID="hfBID" runat="server" />
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
