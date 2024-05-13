<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Shopping.aspx.cs" Inherits="Sim_Shopping" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - Shopping</title>

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
                <a class="headerTitle" >Shopping</a>
                <a><asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>               
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">
               
                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <a class="directions">You may now go shopping. Select a business below and start your shopping!</a>&ensp;
                    <a class="unlocked">
                        <asp:Label ID="lblError" runat="server" CssClass="error_label" ></asp:Label>
                        <table class="Sim_Shopping_Student_Budget_Table">
                            <tr>
                                <th>NMI</th>
                                <th>Spent</th>
                                <th>Remaining</th>
                            </tr>
                            <tr>
                                <td><asp:Label ID="lblNMI" runat="server" Text="$0"></asp:Label></td>
                                <td><asp:Label ID="lblSpent" runat="server" Text="$0"></asp:Label></td>
                                <td><asp:Label ID="lblRemaining" runat="server" Text="$0"></asp:Label></td>
                            </tr>
                        </table>
                    </a>
                </div>
                <br />
                <div class="Sim_Shopping_Budget">
                  <div class="Sim_Shopping_Budget_Wrapper">
                    <div class="Sim_Shopping_Budget_Wrapper_Progress" />
                    <div class="Sim_Shopping_Budget_Wrapper_Progress_Bar"> </div>
                  </div>
                </div>
                <br />
                <%--Content--%>
                <div class="Sim_Content_With_Sponsor">

                     <%--Research Table--%>
                     <div class="Sim_Content">
                         <table class="Sim_Research_Table">
                                 <tr>
                                     <th style="width: 33%;">
                                         <asp:Button ID="btnAutoInsurance" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Auto Insurance" OnClick="btnAutoInsurance_Click" ></asp:Button></th>
                                     <th style="width: 33%;">
                                         <asp:Button ID="btnBankSave" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Bank Savings" OnClick="btnBankSave_Click" ></asp:Button></th>
                                    <th style="width: 33%;">
                                         <asp:Button ID="btnChildcare" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Childcare" OnClick="btnChildcare_Click" ></asp:Button></th>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Button ID="btnClothing" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Clothing" OnClick="btnClothing_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnCredit" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Credit Card" OnClick="btnCredit_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnDining" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Dining Out" OnClick="btnDining_Click" ></asp:Button></td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Button ID="btnEducation" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Education" OnClick="btnEducation_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnEntertainment" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Entertainment" OnClick="btnEntertainment_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnGas" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Gas and Maintenance" OnClick="btnGas_Click" ></asp:Button></td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Button ID="btnGrocery" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Grocery" OnClick="btnGrocery_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnHomeimp" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Home Improvement" OnClick="btnHomeimp_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnHomeins" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Home Insurance" OnClick="btnHomeins_Click" ></asp:Button></td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Button ID="btnHousing" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Housing" OnClick="btnHousing_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnInternet" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Internet" OnClick="btnInternet_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnInvestment" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Investment" OnClick="btnInvestment_Click" ></asp:Button></td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Button ID="btnPhila" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Philanthropy" OnClick="btnPhila_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnPhone" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Phone" OnClick="btnPhone_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnThatslife" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="That's Life" OnClick="btnThatslife_Click" ></asp:Button></td>
                                 </tr>
                                 <tr>
                                     <td>
                                         <asp:Button ID="btnTransport" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Transportation" OnClick="btnTransport_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnUtiwater" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Utilities: Water, Trash" OnClick="btnUtiwater_Click" ></asp:Button></td>
                                     <td>
                                         <asp:Button ID="btnUtielec" runat="server" CssClass="Sim_Research_Button" Style="background-image: url('../../Media/Business Icons/cart.png')" Text="Utilities: Electricity" OnClick="btnUtielec_Click" ></asp:Button></td>
                                 </tr>
                         </table>
                         <br />

                         <%--Next button--%>
                         <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="button" OnClick="btnNext_Click" Visible="true" />
                     </div>               
                    <br />

                    
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
            <asp:HiddenField ID="hfBID" runat="server" />
            <br /><br />          
        </div>

        <script type="text/javascript" src="../../Scripts/Scripts.js"></script>
        <script>
            $(function () {
                 $("#nav-placeholder").load("../../navsim.html");
            });
        </script>

        <%--Progress Bar Script--%>
        <script>
            var budgetAllocated = 400;

            // Listen to the change, keyup & paste events.
            $(function () {

                // Figure out the length of the input value
                var budgetSpent = 100;

                // Calculate the percentage
                var percent = (budgetSpent / budgetAllocated) * 100;

                // Limit the percentage to 100.
                if (percent > 100) {
                    percent = 100;
                }

                // Set the width of the bar based on the percentage.
                //$('.progress-bar').width(percent + '%');

                // Animate the width of the bar based on the percentage.
                $('.Sim_Shopping_Budget_Wrapper_Progress_Bar').animate({
                    width: percent + '%'
                }, 200)
            });
        </script>
    </form>
</body>
</html>
