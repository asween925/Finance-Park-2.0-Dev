<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Calculations.aspx.cs" Inherits="Sim_Calculations" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - Calculations</title>

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
                <a class="headerTitle" >Calculations</a>
                <a><asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>               
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">
               
                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <a class="directions">Select the taxes on the left side of the screen to calculate your net monthly income after tax.</a>&ensp;
                    <a class="unlocked"><asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label></a>
                </div>

                <%--Content--%>
                <div class="Sim_Content_With_Sponsor">

                    <%--Content - Right Block--%>
                    <div class="Sim_Content_Right">

                        <%--Top Section--%>
                        <div class="Sim_Calc_Top_Income">
                            <a>My Gross Annual Income: 
                                <asp:Label ID="lblGAI" runat="server" CssClass="Sim_Calc_Top_Income_Data" Text="$0"></asp:Label></a>
                            <br />
                            <a>My Gross Monthly Income:
                                <asp:Label ID="lblGMI" runat="server" CssClass="Sim_Calc_Top_Income_Data" Text="$0"></asp:Label></a>
                            <br />

                            <%--Taxes Section--%>
                            <div class="Sim_Calc_Taxes">
                                <a id="aPlaceholder" runat="server">Taxes will show up here!</a>

                                <%--Taxes Table--%>
                                <table class="Sim_Calc_Taxes_Table">
                                    <tr id="tdFed" runat="server" visible="false">
                                        <td class="Sim_Calc_Taxes_Table_Category">Federal Withholding:</td>
                                        <td>
                                            <asp:Label ID="lblTaxesFed" runat="server" CssClass="Sim_Calc_Taxes_Data" Text="$0"></asp:Label></td>
                                    </tr>
                                    <tr id="tdMed" runat="server" visible="false">
                                        <td class="Sim_Calc_Taxes_Table_Category">Medicare:</td>
                                        <td>
                                            <asp:Label ID="lblTaxesMedi" runat="server" CssClass="Sim_Calc_Taxes_Data" Text="$0"></asp:Label></td>
                                    </tr>
                                    <tr id="tdSS" runat="server" visible="false">
                                        <td class="Sim_Calc_Taxes_Table_Category">Social Security:</td>
                                        <td>
                                            <asp:Label ID="lblTaxesSS" runat="server" CssClass="Sim_Calc_Taxes_Data" Text="$0"></asp:Label></td>
                                    </tr>
                                </table>
                                
                                <%--Total--%>
                                <div class="Sim_Calc_Taxes_Total">
                                    <a>Total Monthly Taxes: <asp:Label ID="lblTotalTaxes" runat="server" CssClass="Sim_Calc_Taxes_Total_Data" BorderStyle="None" Text="$0"></asp:Label></a>
                                </div>
                            </div>

                            <%--Net income--%>
                            <a>My Net Monthly Income After Tax: 
                                <asp:Button ID="btnNMI" runat="server" Text="Click to Calulate!" CssClass="Sim_Calc_Income_Button" OnClick="btnNMI_Click" Enabled="false" /></a>
                        </div>
                        <br />

                        <%--Bottom Section--%>
                        <div class="Sim_Calc_Income_Bottom">
                            <a>Household Net Monthly Income: 
                                <asp:Label ID="lblHouseNMI" runat="server" CssClass="Sim_Calc_Income_Bottom_Data" Text="$0"></asp:Label></a>
                            <br />
                            <a style="font-size: calc(5px + .8vw);">Your life scenario information can be accessed at anytime by clicking the "Life Scenario" link in the menu located in the top right corner of the screen.</a>
                        </div>
                        
                        <br />
                        <asp:Button ID="btnNext" runat="server" Text="Next" CssClass="button" OnClick="btnNext_Click" Visible="false" />
                    </div>

                    <%--Content - Left Block--%>
                    <div class="Sim_Sponsor">
                        <div class="Sim_Sponsor_Header">
                            <p>Taxes</p>
                        </div>
                        <div class="Sim_Sponsor_Content">
                            <asp:Button ID="btnTaxesFed" runat="server" Text="Federal Withholding &#187;" CssClass="Sim_Calc_Taxes_Button" Style="margin-top: 5px;" OnClick="btnTaxesFed_Click"/>
                            <asp:Button ID="btnTaxesMedicare" runat="server" Text="Medicare &#187;" CssClass="Sim_Calc_Taxes_Button" Style="margin-top: 5px; margin-bottom: 5px;" OnClick="btnTaxesMedicare_Click" />
                            <asp:Button ID="btnTaxesSS" runat="server" Text="Social Security &#187;" CssClass="Sim_Calc_Taxes_Button" Style="margin-bottom: 5px;" OnClick="btnTaxesSS_Click" />
                        </div>
                                    
                    </div>
                </div>                   
            </div>

            <%--Popup--%>
            <div id="popup">
                <div id="divNMIPopup" runat="server" visible="true">
                    <p class="popup_header">
                        <a>Enter your Net Monthly Income!</a>
                        <br />
                        <table class="Sim_Calc_Taxes_Table">
                            <tr>
                                <td class="Sim_Calc_Taxes_Table_Category" style="color: black;">Gross Monthly Income:</td>
                                <td>
                                    <asp:Label ID="lblGMIPopup" runat="server" CssClass="Sim_Calc_Popup_Data" Text="$0"></asp:Label></td>
                            </tr>
                            <tr>
                                <td class="Sim_Calc_Taxes_Table_Category" style="color: black;">Taxes:</td>
                                <td>
                                    <asp:Label ID="lblTotalTaxesPopup" runat="server" CssClass="Sim_Calc_Popup_Data" ForeColor="Red" Text="$0"></asp:Label></td>
                            </tr>
                        </table>
                    </p>
                    <asp:TextBox ID="tbNMI" runat="server" CssClass="Sim_Calc_NMI_TB" TextMode="Number"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblErrorPopup" runat="server" CssClass="error_label"></asp:Label>
                    <br />
                    <asp:Button ID="btnEnter" runat="server" Text="Enter" CssClass="button" OnClick="btnEnter_Click"/><asp:Button ID="btnCancel" runat="server" OnClick="btnCancel_Click" CssClass="buttonReset" Text="Cancel"></asp:Button>
                </div>

                <%--To Savings--%>
                <div id="divSavingsPopup" runat="server" visible="false">
                    <p class="popup_header">
                        <a>
                            <asp:Label ID="lblSavingsHeader" runat="server" Text="Header Here"></asp:Label></a>
                    </p>
                    <p class="popup_header_2" style="font-size: calc(6px + .8vw);">
                        <a>
                            <asp:Label ID="lblSavingsText" runat="server" Text="Transition Text"></asp:Label></a>
                    </p>
                    <asp:TextBox ID="tbUnlock" runat="server" CssClass="popup_textbox" TextMode="Number"></asp:TextBox>
                    <br />
                    <asp:Label ID="lblErrorPopup2" runat="server" CssClass="error_label"></asp:Label>
                    <br />
                    <asp:Button ID="btnEnter2" runat="server" Text="Enter" CssClass="button" OnClick="btnEnter2_Click" /><asp:Button ID="btnCancel2" runat="server" OnClick="btnCancel2_Click" CssClass="buttonReset" Text="Cancel"></asp:Button>
                </div>
                
            </div>

            <asp:HiddenField ID="hfNMI" runat="server" Visible="true" />          
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
