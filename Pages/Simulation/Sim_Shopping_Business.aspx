<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Shopping_Business.aspx.cs" EnableEventValidation="false" Inherits="Sim_Shopping_Business" %>

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
                <a class="headerTitle">Shopping</a>
                <a>
                    <asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">

                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock2">
                    <a class="directions">
                        <asp:Image ID="imgBusinessIcon" runat="server" /><asp:Label ID="lblBusinessName" runat="server" Text="Business Name"></asp:Label></a>&ensp;
                    <a>
                        <asp:Label ID="lblError" runat="server" CssClass="error_label" Text="test"></asp:Label>&ensp;<asp:Button ID="btnBack" runat="server" Text="Select Business" CssClass="button" OnClick="btnBack_Click" />
                    </a>
                </div>
                <br />

                <%--Student Spending--%>
                <div class="directionsBlock2">
                    <div class="Sim_Shopping_Budget">
                        <div class="Sim_Shopping_Budget_Wrapper">
                            <div class="Sim_Shopping_Budget_Wrapper_Progress">
                                <div class="Sim_Shopping_Budget_Wrapper_Progress_Bar">
                                    <asp:Label ID="lblSpentBar" runat="server" Text="$0" CssClass="Sim_Shopping_Budget_Wrapper_Progress_Bar_Text"></asp:Label>
                                </div>
                            </div>
                        </div>
                        <div>
                            $0<a style="padding-right: 25%; float: right;"><asp:Label ID="lblStudentBudget" runat="server"></asp:Label>
                                Your Budget</a>
                        </div>

                    </div>
                    <div class="Sim_Shopping_Spending">
                        <table class="Sim_Shopping_Student_Budget_Table">
                            <tr style="text-align: left; text-decoration: underline;">
                                <th>NMI</th>
                                <th>Spent</th>
                                <th>Remaining</th>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblNMI" runat="server" Text="$0"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblSpent" runat="server" Text="$0"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblRemaining" runat="server" Text="$0"></asp:Label></td>
                            </tr>
                        </table>
                    </div>
                </div>

                <%--Content--%>
                <div class="Sim_Content_With_Sponsor">

                    <%--Content - Right Block--%>
                    <div class="Sim_Content_Right">

                        <%--Scripts--%>
                        <div class="Sim_Shopping_Script_Container">
                            <asp:Label ID="lblKioskScript" runat="server" Text="The kiosk script for the business will go here." CssClass="Sim_Shopping_Script1"></asp:Label>
                            <br />
                            <asp:Label ID="lblKioskScript2" runat="server" Text="Tiny script here." CssClass="Sim_Shopping_Script2"></asp:Label>
                        </div>

                        <%--Price section--%>
                        <div class="Sim_Shopping_Price_Container">
                            <asp:Button ID="btnAction" runat="server" Text="Action" CssClass="button" Visible="false" OnClick="btnAction_Click" /><asp:Button ID="btnAction2" runat="server" Text="Action" CssClass="button" Visible="false" OnClick="btnAction2_Click" /><asp:Button ID="btnAction3" runat="server" Text="Action" CssClass="button" Visible="false" OnClick="btnAction3_Click" />

                            <%--Shopping Price Table--%>
                            <table class="Sim_Shopping_Price_Table">
                                <tr>
                                    <td>Your total cost: </td>
                                    <td>
                                        <asp:Label ID="lblTotalCost" runat="server" Text="$0"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblItemsSelected" runat="server" Text="0"></asp:Label>
                                        item(s) selected</td>
                                </tr>
                            </table>

                            <%--Credit Card Table--%>
                            <table class="Sim_Shopping_Price_Table">
                                <tr>
                                    <td>Existing Credit: </td>
                                    <td>
                                        <asp:Label ID="lblCCExisting" runat="server" Text="$0"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Minimum Monthly Payment: </td>
                                    <td>
                                        <asp:Textbox ID="tbCCMinPay" runat="server" textmode="Number" CssClass="textbox"></asp:Textbox></td>
                                </tr>
                            </table>
                        </div>

                        <%--Data gridview--%>
                        <div style="padding-top: 5px;">
                            <asp:GridView ID="dgvShoppingItems" HorizontalAlign="Center" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" CellPadding="5" Height="50" AllowPaging="True"
                                ShowHeaderWhenEmpty="True" Font-Size="Medium" PagerStyle-CssClass="gridviewPager" HeaderStyle-CssClass="gridviewHeader" RowStyle-CssClass="gridviewRows" Visible="true">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" />
                                    <asp:BoundField DataField="itemName" HeaderText="Item Name" ReadOnly="true" />
                                    <asp:BoundField DataField="cost" HeaderText="Cost" DataFormatString="{0:C}" ReadOnly="true" />
                                    <asp:BoundField DataField="category" HeaderText="Category" ReadOnly="true" />
                                    <asp:ImageField HeaderText="Photo" ReadOnly="true"></asp:ImageField>
                                    <asp:TemplateField HeaderText="Buy Item?">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkBuy" runat="server" AutoPostBack="true" OnCheckedChanged="chkBuy_CheckedChanged" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>

                        <%--Credit Card Calculator--%>
                        <div id="divRetire" runat="server" class="Sim_Business_Retire" visible="true">
                            <a class="Sim_Business_Retire_Header">Credit Debt Calculator</a>

                            <table style="padding-top: 6px;">
                                <tr>
                                    <td>Credit Card Debt: </td>
                                    <td>
                                        <asp:Label ID="lblCCDebt" runat="server" Text="$0"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Rate of Interest: </td>
                                    <td>
                                        <asp:Label ID="lblCCInterest" runat="server" Text="$0"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>Minimum Monthly Payment: </td>
                                    <td>
                                        <asp:Label ID="lblCCMinPay" runat="server" Text="$0"></asp:Label></td>
                                </tr>
                            </table>

                            <p>You will end up paying:</p>
                            <%--<asp:Button ID="btnRetireTotal" runat="server" Text="$0" CssClass="button" Enabled="false" OnClick="btnRetireTotal_Click" />--%>
                        </div>

                    </div>

                    <%--Content - Left Block--%>
                    <div class="Sim_Sponsor">
                        <div class="Sim_Sponsor_Header">
                            <p>Sponsored By:</p>
                        </div>
                        <div style="padding-left: 5px; padding-right: 5px;">
                            <a id="pImg1" runat="server" visible="false">
                                <asp:Image ID="imgSponsorLogo1" runat="server" CssClass="Sim_Sponsor_Img" AlternateText="SponsorLogo1" /></a>
                            <a id="pImg2" runat="server" visible="false">
                                <asp:Image ID="imgSponsorLogo2" runat="server" CssClass="Sim_Sponsor_Img" AlternateText="SponsorLogo2" /></a>
                            <a id="pImg3" runat="server" visible="false">
                                <asp:Image ID="imgSponsorLogo3" runat="server" CssClass="Sim_Sponsor_Img" AlternateText="SponsorLogo3" /></a>
                            <a id="pImg4" runat="server" visible="false">
                                <asp:Image ID="imgSponsorLogo4" runat="server" CssClass="Sim_Sponsor_Img" AlternateText="SponsorLogo4" /></a>
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
                    <asp:Label ID="lblPopupText" runat="server" Text="Popup Text"></asp:Label>
                </p>
                <asp:TextBox ID="tbPopupTB" runat="server" CssClass="popup_textbox" TextMode="Number"></asp:TextBox>
                <br />
                <asp:Label ID="lblErrorPopup" runat="server" CssClass="error_label"></asp:Label>
                <br />
                <asp:Button ID="btnEnter" runat="server" Text="Enter" CssClass="button" OnClick="btnEnter_Click" /><asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="buttonReset" Text="Cancel"></asp:Button>
            </div>

            <br />
            <br />
            <asp:HiddenField ID="hfMaxBudget" runat="server" />
            <asp:SqlDataSource ID="Review_sds" runat="server"></asp:SqlDataSource>
        </div>

        <script type="text/javascript" src="../../Scripts/Scripts.js"></script>
        <script>
            $(function () {
                $("#nav-placeholder").load("../../navsim.html");
            });
        </script>

        <%--Progress Bar Script--%>
        <script>
            var budgetAllocated = $(#hfMaxBudget);

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
