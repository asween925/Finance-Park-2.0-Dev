<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Business.aspx.cs" Inherits="Sim_Business" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - Business</title>

    <link href="../../CSS/StudentPages.css" rel="stylesheet" media="screen" type="text/css" />
    <link rel="shortcut icon" href="../../Media/FP_favicon_2.png" type="image/ico" />

</head>
<body>
    <form id="EMS_Form" runat="server">
        <div id="Site_Wrap_Fullscreen">

            <%--Header--%>
            <div id="divHeader" runat="server" class="header">
                <div id="nav-placeholder"></div>
                <a class="headerTitle">Business Information</a>
                <a>
                    <asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">

                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <asp:Image ID="imgBusinessIcon" runat="server" /><asp:Label ID="lblBusinessName" runat="server" CssClass="directions Sim_Business_Name"></asp:Label>
                    <a class="unlocked">
                        <asp:Label ID="lblBusinessUnlocked" runat="server" Font-Bold="true" Text="0"></asp:Label>
                        of
                        <asp:Label ID="lblTotalBusiness" runat="server" Font-Bold="true" Text="22"></asp:Label>
                        businesses unlocked.&ensp;<asp:Button ID="btnResearch" runat="server" Text="Unlock More" CssClass="button" OnClick="btnResearch_Click" /></a>
                </div>

                <%--Content--%>
                <div class="Sim_Content_With_Sponsor">

                    <%--Right Side Content--%>
                    <div class="Sim_Content_Right" style="min-height: 100%;">
                        <asp:Button ID="btnAction" runat="server" Text="Action" CssClass="button" Visible="false" OnClick="btnAction_Click" /><asp:Button ID="btnAction2" runat="server" Text="Action" CssClass="button" Visible="false" OnClick="btnAction2_Click" /><asp:Button ID="btnAction3" runat="server" Text="Action" CssClass="button" Visible="false" OnClick="btnAction3_Click" />
                        <br />
                        <asp:Label ID="lblKioskScript" runat="server" Text="The kiosk script for the business will go here."></asp:Label>

                        <%--Table--%>
                        <table id="tblBusiness" runat="server" visible="false" class="Sim_Business_Table">
                            <tr class="Sim_Business_Table_Row">
                                <td class="Sim_Business_Table_Cat">
                                    <asp:Label ID="lblTblCat1" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Cat">
                                    <asp:Label ID="lblTblCat2" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Cat">
                                    <asp:Label ID="lblTblCat3" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Cat">
                                    <asp:Label ID="lblTblCat4" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Cat">
                                    <asp:Label ID="lblTblCat5" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Cat">
                                    <asp:Label ID="lblTblCat6" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="Sim_Business_Table_Row">
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat1Data1" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat2Data1" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat3Data1" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat4Data1" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat5Data1" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat6Data1" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="Sim_Business_Table_Row">
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat1Data2" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat2Data2" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat3Data2" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat4Data2" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat5Data2" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat6Data2" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="Sim_Business_Table_Row">
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat1Data3" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat2Data3" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat3Data3" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat4Data3" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat5Data3" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat6Data3" runat="server"></asp:Label></td>
                            </tr>
                            <tr class="Sim_Business_Table_Row">
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat1Data4" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat2Data4" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat3Data4" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat4Data4" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat5Data4" runat="server"></asp:Label></td>
                                <td class="Sim_Business_Table_Data">
                                    <asp:Label ID="lblTblCat6Data4" runat="server"></asp:Label></td>
                            </tr>                          
                        </table>

                        <%--Loan Configuration--%>
                        <div id="divLoan" runat="server" class="Sim_Business_Loan" visible="false">
                            <a class="Sim_Business_Loan_Header">Apply for Loan</a>

                            <p class="Sim_Business_Loan_Script">
                                <asp:Label ID="lblLoanAppScript" runat="server" Text="Script text goes here."></asp:Label></p>

                            <%--Left Side--%>
                            <div class="Sim_Business_Loan_Left">
                                <a class="Sim_Business_Loan_Btn_Text">Borrower</a>
                                <br />
                                <asp:Button ID="btnLoanBorrower" runat="server" Text="Tap to Fill" CssClass="Sim_Business_Loan_Left_Button" OnClick="btnLoanBorrower_Click" />
                                <br />
                                <br />
                                <a class="Sim_Business_Loan_Btn_Text">Co-Borrower</a>
                                <br />
                                <asp:Button ID="btnLoanCoBorrower" runat="server" Text="Tap to Fill" CssClass="Sim_Business_Loan_Left_Button" OnClick="btnLoanCoBorrower_Click" />
                                <br />
                                <br />
                                <a class="Sim_Business_Loan_Btn_Text">Purpose of Loan</a>
                                <br />
                                <asp:Button ID="btnLoanPurposePurchase" runat="server" Text="Purchase" CssClass="Sim_Business_Loan_Left_Button2" OnClick="btnLoanPurposePurchase_Click" />
                                <br />
                                <asp:Button ID="btnLoanPurposeRefinance" runat="server" Text="Refinance" CssClass="Sim_Business_Loan_Left_Button2" OnClick="btnLoanPurposeRefinance_Click" />
                                <br />
                                <asp:Button ID="btnLoanPurposeConstruction" runat="server" Text="Construction" CssClass="Sim_Business_Loan_Left_Button2" OnClick="btnLoanPurposeConstruction_Click" />
                            </div>

                            <%--Right Side--%>
                            <div class="Sim_Business_Loan_Right">
                                <p>Monthly Income and Assets / Liabilities Information</p>

                                <%--Account Financal Info Table--%>
                                <table>
                                    <tr>
                                        <td>
                                            <a class="Sim_Business_Loan_Btn_Text">GMI Borrower</a>
                                            <br />
                                            <asp:Button ID="btnLoanGMI" runat="server" Text="Tap to Fill" CssClass="Sim_Business_Loan_Right_Button" OnClick="btnLoanGMI_Click" />
                                        </td>
                                        <td>
                                            <a class="Sim_Business_Loan_Btn_Text">GMI Co-Borrower</a>
                                            <br />
                                            <asp:Button ID="btnLoanCoGMI" runat="server" Text="Tap to Fill" CssClass="Sim_Business_Loan_Right_Button" OnClick="btnLoanCoGMI_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a class="Sim_Business_Loan_Btn_Text">Credit Card Debt</a>
                                            <br />
                                            <asp:Button ID="btnLoanCC" runat="server" Text="Tap to Fill" CssClass="Sim_Business_Loan_Right_Button" OnClick="btnLoanCC_Click" />
                                        </td>
                                        <td>
                                            <a class="Sim_Business_Loan_Btn_Text">Bank Account Number</a>
                                            <br />
                                            <asp:Button ID="btnLoanBank" runat="server" Text="Tap to Fill" CssClass="Sim_Business_Loan_Right_Button" OnClick="btnLoanBank_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <a class="Sim_Business_Loan_Btn_Text">Current Balance</a>
                                            <br />
                                            <asp:Button ID="btnLoanBalance" runat="server" Text="Tap to Fill" CssClass="Sim_Business_Loan_Right_Button" OnClick="btnLoanBalance_Click" />
                                        </td>
                                        <td>
                                            <a class="Sim_Business_Loan_Btn_Text">Other Debt</a>
                                            <br />
                                            <asp:Button ID="btnLoanOther" runat="server" Text="Tap to Fill" CssClass="Sim_Business_Loan_Right_Button" OnClick="btnLoanOther_Click" />
                                        </td>
                                    </tr>
                                </table>

                            </div>


                            <a class="Sim_Business_Loan_Script_Bottom">I give permission for th assigned financial institution to verify or reverify any information contained in this application or obtain any information or data relating to the loan, through any source, including a consumer reporting agency.</a>
                            <asp:Button ID="btnLoanSubmit" runat="server" Text="Submit Application" CssClass="button" OnClick="btnLoanSubmit_Click" />
                        </div>

                        <%--Retirement Calculator--%>
                        <div id="divRetire" runat="server" class="Sim_Business_Retire" visible="false">
                            <a class="Sim_Business_Retire_Header">Retirement Calculator</a>

                            <table style="padding-top: 6px;">
                                <tr>
                                    <td>Rate of Interest: </td>
                                    <td>
                                        <asp:Button ID="btnRetire4" runat="server" Text="4%" CssClass="button" OnClick="btnRetire4_Click" /></td>
                                    <td>
                                        <asp:Button ID="btnRetire6" runat="server" Text="6%" CssClass="button" OnClick="btnRetire6_Click" /></td>
                                    <td>
                                        <asp:Button ID="btnRetire8" runat="server" Text="8%" CssClass="button" OnClick="btnRetire8_Click" /></td>
                                </tr>
                                <tr>
                                    <td>Number of Years: </td>
                                    <td>
                                        <asp:Button ID="btnRetire30" runat="server" Text="30" CssClass="button" OnClick="btnRetire30_Click" /></td>
                                    <td>
                                        <asp:Button ID="btnRetire40" runat="server" Text="40" CssClass="button" OnClick="btnRetire40_Click" /></td>
                                    <td>
                                        <asp:Button ID="btnRetire50" runat="server" Text="50" CssClass="button" OnClick="btnRetire50_Click"/></td>
                                </tr>
                            </table>

                            <p>If you save $100 each month for 30 years and earn 6% interest, your total retirement savings will be:</p>
                            <asp:Button ID="btnRetireTotal" runat="server" Text="$0" CssClass="button" Enabled="false" OnClick="btnRetireTotal_Click" />

                        </div>

                        <asp:Label ID="lblError" runat="server" CssClass="error_label"></asp:Label>

                    </div>

                    <%--Sponsor--%>
                    <div class="Sim_Sponsor">
                        <div class="Sim_Sponsor_Header">
                            <p>Sponsored By:</p>
                        </div>
                        <div style="padding-left: 5px; padding-right: 5px;">
                            <a id="pImg1" runat="server" visible="false"><asp:Image ID="imgSponsorLogo1" runat="server" CssClass="Sim_Sponsor_Img" AlternateText="SponsorLogo1" /></a>
                            <a id="pImg2" runat="server" visible="false"><asp:Image ID="imgSponsorLogo2" runat="server" CssClass="Sim_Sponsor_Img" AlternateText="SponsorLogo2" /></a>
                            <a id="pImg3" runat="server" visible="false"><asp:Image ID="imgSponsorLogo3" runat="server" CssClass="Sim_Sponsor_Img" AlternateText="SponsorLogo3" /></a>
                            <a id="pImg4" runat="server" visible="false"><asp:Image ID="imgSponsorLogo4" runat="server" CssClass="Sim_Sponsor_Img" AlternateText="SponsorLogo4" /></a>
                        </div>
                        
                    </div>

                </div>

            </div>

            <%--Popup--%>
            <div id="popup">
                <p class="popup_header"></p>
                <p>
                    <asp:Label ID="lblPopupText" runat="server"></asp:Label></p>
                <br />
                <br />
                <button onclick="toggle(); return false;" class="button">Okay</button>
            </div>


        </div>
        <script type="text/javascript" src="../../Scripts/Scripts.js"></script>
    </form>
</body>
</html>
