<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Savings.aspx.cs" Inherits="Sim_Savings" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - Savings</title>

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
                <a class="headerTitle" >Savings</a>
                <a><asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>               
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">
               
                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <a class="directions">Allocate your money to your savings.</a>&ensp;
                    <a class="unlocked">
                        <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label></a>
                </div>

                <%--Content--%>
                <div class="Sim_Content_With_Sponsor">

                    <%--Content - Right Block--%>
                    <div class="Sim_Content_Right">

                        <%--Starting Point--%>
                        <div>
                            <a class="Sim_Savings_Section">Your starting point</a>
                            <br />
                            <table class="Sim_Savings_Table">
                                <tr>
                                    <td class="Sim_Savings_Table_Cat">Existing retirement savings:</td>
                                    <td><asp:Label ID="lblExistingRetirement" runat="server" CssClass="Sim_Savings_Table_Data" Text="$0"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Savings_Table_Cat">Existing emergency fund savings:</td>
                                    <td><asp:Label ID="lblExistingEmer" runat="server" CssClass="Sim_Savings_Table_Data" Text="$0"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Savings_Table_Cat">Other existing savings:</td>
                                    <td><asp:Label ID="lblExistingOther" runat="server" CssClass="Sim_Savings_Table_Data" Text="$0"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Savings_Table_Cat">Household NMI:</td>
                                    <td><asp:Label ID="lblHouseNMI" runat="server" CssClass="Sim_Savings_Table_Data" Text="$0"></asp:Label></td>
                                </tr>
                            </table>
                        </div>

                        <%--Savings Goal--%>
                        <div>
                            <a class="Sim_Savings_Section">Your savings goal</a>
                            <p class="Sim_Savings_Text">First, set an overall savings goal. You can increase or decrease your savings if you would like.</p>
                            <table class="Sim_Savings_Table">
                                <tr>
                                    <td class="Sim_Savings_Table_Cat">To save 5% of my NMI each month, I would save:</td>
                                    <td>
                                        <asp:Button ID="btnSaveNMI5" runat="server" CssClass="buttonTable" Text="Click to Calculate!" OnClick="btnSaveNMI5_Click" /></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Savings_Table_Cat">To save 10% of my NMI each month, I would save:</td>
                                    <td>
                                        <asp:Button ID="btnSaveNMI10" runat="server" CssClass="buttonTable" Text="Click to Calculate!" OnClick="btnSaveNMI10_Click" /></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Savings_Table_Cat">To save 15% of my NMI each month, I would save:</td>
                                    <td>
                                        <asp:Button ID="btnSaveNMI15" runat="server" CssClass="buttonTable" Text="Click to Calculate!" OnClick="btnSaveNMI15_Click" /></td>
                                </tr>                               
                            </table>
                        </div>

                        <%--Total savings plan--%>
                        <div>
                            <a class="Sim_Savings_Section">Total savings plan for this month:</a>                           
                            <p class="Sim_Savings_Text">Enter the total you will save this month: 
                                <asp:TextBox ID="tbSavingsTotal" runat="server" CssClass="textbox Sim_Savings_Textbox" PLaceholder="$0" TextMode="Number"></asp:TextBox>
                            </p>
                            <p class="Sim_Savings_Text">Determine how you will divide this amount into each savings type:</p>
                            <table class="Sim_Savings_Table">
                                <tr>
                                    <td class="Sim_Savings_Table_Header">Savings Type</td>
                                    <td class="Sim_Savings_Table_Header" style="float: right;">Allocate your total monthly savings</td>
                                </tr>
                                <tr>
                                    <td class="Sim_Savings_Table_Cat">Retirement Savings:</td>
                                    <td>
                                        <asp:TextBox ID="tbSaveRetire" runat="server" CssClass="textbox Sim_Savings_Textbox" Width="30%" PLaceholder="$0" TextMode="Number" OnTextChanged="tbSaveRetire_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Savings_Table_Cat">Emergency Fund Savings:</td>
                                    <td>
                                        <asp:TextBox ID="tbSaveEmerg" runat="server" CssClass="textbox Sim_Savings_Textbox" Width="30%" PLaceholder="$0" TextMode="Number" OnTextChanged="tbSaveEmerg_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Savings_Table_Cat">Other Savings:</td>
                                    <td>
                                        <asp:TextBox ID="tbSaveOther" runat="server" CssClass="textbox Sim_Savings_Textbox" Width="30%" PLaceholder="$0" TextMode="Number" OnTextChanged="tbSaveOther_TextChanged" AutoPostBack="true"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td>&ensp;</td>
                                    <td>&ensp;</td>
                                </tr>
                                <tr class="Sim_Savings_Table_Row">
                                    <td class="Sim_Savings_Table_Cat">Total:</td>
                                    <td class="Sim_Savings_Table_Data" style="float: right;"><asp:Label ID="lblSavingsTotal" runat="server" Text="$0"></asp:Label></td>
                                </tr>
                            </table>
                            <br />

                            <%--Next button--%>
                            <asp:Button ID="btnNext" runat="server" Text="Continue to Research" CssClass="button" OnClick="btnNext_Click" Visible="false" />
                        </div>

                    </div>

                    <%--Content - Left Block--%>
                    <div class="Sim_Sponsor">
                        <div class="Sim_Sponsor_Header">
                            <p>Sponsored By:</p>
                        </div>
                        <div class="Sim_Sponsor_Content">

                            <%--Sponsor logos here--%>
                            <asp:Image ID="imgSponsor1" runat="server" />
                            <br />
                            <asp:Image ID="imgSponsor2" runat="server" />
                            <br />
                            <asp:Image ID="imgSponsor3" runat="server" />
                        </div>
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
