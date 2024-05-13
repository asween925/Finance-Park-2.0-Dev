<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Budget.aspx.cs" Inherits="Sim_Budget" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - Budget</title>

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
                <a class="headerTitle" >Budgeting</a>
                <a><asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>               
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">
               
                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <a class="directions">Determine your budget for each expense category.</a>&ensp;
                    <a class="unlocked">
                        <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label></a>
                </div>

                <%--Content--%>
                <div class="Sim_Content_With_Sponsor">

                    <%--Content - Right Block--%>
                    <div class="Sim_Content_Right">
                         
                        <%--Student Finances--%>
                         <table class="Sim_Budget_Top_Table">
                             <tr>
                                 <td>NMI: <asp:Label ID="lblNMI" runat="server" Font-Bold="true" ForeColor="Black" Text="$0"></asp:Label></td>
                                 <td>Total Budgeted: <asp:Label ID="lblTotal" runat="server" Font-Bold="true" ForeColor="Black" Text="$0"></asp:Label></td>
                                 <td>Left to Allocate: <asp:Label ID="lblAllocate" runat="server" Font-Bold="true" ForeColor="Black" Text="$0"></asp:Label></td>
                             </tr>
                         </table>
                        <br />

                        <%--Budgets--%>
                        <table class="Sim_Budget_Table">
                            <tr class="Sim_Budget_Table_Cat">
                                <td>Business<br /> Name</td>
                                <td>Minimum Recommended</td>
                                <td>Maximum Recommended</td>
                                <td>Your<br /> Budget</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Auto.png" class="Sim_Budget_Icon"></image>&ensp;Auto Insurance</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar1" runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer1" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar1" runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer1" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget1" runat="server" Textmode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget1_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer1" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/BankSave.png" class="Sim_Budget_Icon"></image>&ensp;Bank Savings</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar7"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer7" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar7"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer7" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget7"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget7_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer7" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Clothing.png" class="Sim_Budget_Icon"></image>&ensp;Clothing</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar11"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer11" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar11"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer11" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget11"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget11_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer11" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Credit.png" class="Sim_Budget_Icon"></image>&ensp;Credit Cards</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar12"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer12" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar12"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer12" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget12"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget12_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer12" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/DiningOut.png" class="Sim_Budget_Icon"></image>&ensp;Dining Out</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar13"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer13" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar13"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer13" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget13"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget13_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer13" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Education.png" class="Sim_Budget_Icon"></image>&ensp;Education</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar15"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer15" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar15"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer15" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget15"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget15_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer15" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Entertainment.png" class="Sim_Budget_Icon"></image>&ensp;Entertainment</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar16"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer16" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar16"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer16" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget16"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget16_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer16" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Gas.png" class="Sim_Budget_Icon"></image>&ensp;Gas and Maintenance</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar18"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer18" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar18"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer18" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget18"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget18_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer18" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Grocery.png" class="Sim_Budget_Icon"></image>&ensp;Grocery</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar19"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer19" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar19"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer19" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget19"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget19_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer19" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/HomeImprov.png" class="Sim_Budget_Icon"></image>&ensp;Home Improvement</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar21"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer21" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar21"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer21" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget21"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget21_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer21" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/HomeIns.png" class="Sim_Budget_Icon"></image>&ensp;Home Insurance</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar22"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer22" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar22"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer22" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget22"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget22_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer22" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Housing.png" class="Sim_Budget_Icon"></image>&ensp;Housing</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar23"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer23" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar23"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer23" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget23"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget23_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer23" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Internet.png" class="Sim_Budget_Icon"></image>&ensp;Internet</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar24"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer24" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar24"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer24" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget24"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget24_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer24" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Philan.png" class="Sim_Budget_Icon"></image>&ensp;Philanthropy</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar26"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer26" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar26"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer26" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget26"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget26_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer26" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Phone.png" class="Sim_Budget_Icon"></image>&ensp;Phone</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar27"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer27" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar27"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer27" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget27"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget27_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer27" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/ThatsLife.png" class="Sim_Budget_Icon"></image>&ensp;That's Life</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar28"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer28" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar28"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer28" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget28"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget28_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer28" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/Trans.png" class="Sim_Budget_Icon"></image>&ensp;Transportation</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar29"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer29" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar29"  runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer29" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget29"  runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget29_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer29" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/UtilitiesWater.png" class="Sim_Budget_Icon"></image>&ensp;Utilities: Water / Sewage</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar30" runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer30" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar30" runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer30" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget30" runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget30_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer30" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                            <tr class="Sim_Budget_Table_Row">
                                <td class="Sim_Budget_Table_Row_L"><image src="../../Media/Business Icons/UtilityElc.png" class="Sim_Budget_Icon"></image>&ensp;Utilities: Electricty</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMinDollar31" runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMinPer31" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_C"><asp:Label ID="lblMaxDollar31" runat="server" Text="$0"></asp:Label>&ensp;(<asp:Label ID="lblMaxPer31" runat="server" Text="0%"></asp:Label>)</td>
                                <td class="Sim_Budget_Table_Row_R">$ <asp:TextBox ID="tbBudget31" runat="server" TextMode="Number" CssClass="textbox Sim_Budget_Table_TB" AutoPostBack="true" OnTextChanged="tbBudget31_TextChanged"></asp:TextBox>&ensp;(<asp:Label ID="lblBudgetPer31" runat="server" Text="0%"></asp:Label>)</td>
                            </tr>
                        </table>
                        <br />
                        <asp:Button ID="btnNext" runat="server" CssClass="button" Text="Continue to Shopping" OnClick="btnNext_Click" />
                    </div>

                    <%--Content - Left Block--%>
                    <div class="Sim_Sponsor">
                        <div class="Sim_Sponsor_Header">
                            <p>Sponsored By:</p>
                        </div>
                        <div class="Sim_Sponsor_Content">
                            <a id="pImg1" runat="server" visible="true"><asp:Image ID="imgSponsorLogo1" runat="server" CssClass="Sim_Sponsor_Img" AlternateText="SponsorLogo1" ImageUrl="~/Media/Sponsor Logos/BBB/bbb_logo.png" /></a>
                            <a id="pImg2" runat="server" visible="true"><asp:Image ID="imgSponsorLogo2" runat="server" CssClass="Sim_Sponsor_Img" AlternateText="SponsorLogo2" ImageUrl="~/Media/Sponsor Logos/Nielson/nielson_logo.png" /></a>
                        </div>
                    </div>

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
