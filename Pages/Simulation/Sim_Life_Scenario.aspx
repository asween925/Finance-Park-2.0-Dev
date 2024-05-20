<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Life_Scenario.aspx.cs" Inherits="Sim_Life_Scenario" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;" />

    <title>Finance Park - Life Scenario</title>

    <link href="../../CSS/StudentPages.css" rel="stylesheet" media="screen" type="text/css" />
    <link rel="shortcut icon" href="../../Media/FP_favicon_2.png" type="image/ico" />
    <script src="https://code.jquery.com/jquery-1.10.2.js"></script>
    <style type="text/css">
        .auto-style1 {
            width: 100%;
        }
    </style>
</head>
<body>
    <form id="EMS_Form" runat="server">
        <div id="Site_Wrap_Fullscreen">
            
            <%--Header--%>
            <div id="divHeader" runat="server" class="header">
                <div id="nav-placeholder"></div>
                <a class="headerTitle" >Life Scenario</a>
                <a><asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>               
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">
               
                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <a class="directions">This is your Life Scenario. It contains all information about your persona for Finance Park.</a>&ensp;
                    <a class="unlocked">
                        <asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label></a>
                </div>

                <%--Content--%>
                <div class="Sim_Content_With_Sponsor">

                    <%--Content - Right Block--%>
                    <div class="Sim_Content_Right">

                        <div class="Sim_Life_Scenario_Info">
                            <a class="Sim_Life_Scenario_Name">
                                <asp:Label ID="lblStudentName" runat="server" Text="Student Name"></asp:Label></a>
                            <br />
                            <a class="Sim_Life_Scenario_Job">
                                <asp:Label ID="lblJobTitle" runat="server" Text="Job Title"></asp:Label>&ensp;
                                <asp:Label ID="lblAccountNumber" runat="server" Text="Account #" Font-Bold="true" CssClass="Sim_Life_Scenario_Acct"></asp:Label>                               
                            </a>
                            <br />
                            <a class="Sim_Life_Scenario_Job">
                                <asp:Label ID="lblDegree" runat="server" Text="Degree"></asp:Label>
                                <asp:Label ID="lblPin" runat="server" Text="Pin #" Font-Bold="true" CssClass="Sim_Life_Scenario_Acct"></asp:Label>
                            </a>
                            <br />
                            <a class="Sim_Life_Scenario_ASC">
                                <asp:Label ID="lblAge" runat="server" Text="Age"></asp:Label>&nbsp;|&nbsp;<asp:Label ID="lblMaritalStatus" runat="server" Text="Status"></asp:Label>&nbsp;|&nbsp;<asp:Label ID="lblChildren" runat="server" Text="Children: 0"></asp:Label></a>
                            <br />
                        </div>
                        <br />

                        <%--Finances--%>
                        <div class="Sim_Life_Scenario_Finances">                           
                            <table class="Sim_Life_Scenario_Table">
                                <tr>
                                    <td class="Sim_Life_Scenario_Table_Cat">Credit Score</td>
                                    <td>
                                        <asp:Label ID="lblCreditScore" runat="server" Text="600" CssClass="Sim_Life_Scenario_Table_Value"></asp:Label></td>                                   
                                </tr>
                                <tr>
                                    <td class="Sim_Life_Scenario_Table_Cat">My Gross Annual Income</td>
                                    <td>
                                        <asp:Label ID="lblGAI" runat="server" Text="$40,000" CssClass="Sim_Life_Scenario_Table_Value"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Life_Scenario_Table_Cat">My Net Monthly Income</td>
                                    <td>
                                        <asp:Label ID="lblGMI" runat="server" Text="$2000" CssClass="Sim_Life_Scenario_Table_Value"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>&ensp;</td>
                                    <td>&ensp;</td>
                                </tr>
                                <tr>
                                    <td class="Sim_Life_Scenario_Table_Cat">Educational Debt</td>
                                    <td>
                                        <asp:Label ID="lblEdDebt" runat="server" Text="$50,000" CssClass="Sim_Life_Scenario_Table_Value"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Life_Scenario_Table_Cat">Credit Card Debt</td>
                                    <td>
                                        <asp:Label ID="lblCCDebt" runat="server" Text="$1,000" CssClass="Sim_Life_Scenario_Table_Value"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Life_Scenario_Table_Cat">Retirement Savings</td>
                                    <td>
                                        <asp:Label ID="lblRetire" runat="server" Text="$300" CssClass="Sim_Life_Scenario_Table_Value"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Life_Scenario_Table_Cat">Emergency Funds</td>
                                    <td>
                                        <asp:Label ID="lblEmerFunds" runat="server" Text="$0" CssClass="Sim_Life_Scenario_Table_Value"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td class="Sim_Life_Scenario_Table_Cat">Other Savings</td>
                                    <td>
                                        <asp:Label ID="lblOther" runat="server" Text="$0" CssClass="Sim_Life_Scenario_Table_Value"></asp:Label></td>
                                </tr>
                            </table>                           
                        </div>
                        <br />

                        <%--Career and Background--%>
                        <div class="Sim_Life_Scenario_BG">
                            <a class="Sim_Life_Scenario_Name">Job Responsibilities</a>
                            <br />
                            <asp:Label ID="lblResponse" runat="server" Text="Job Responsibilities" CssClass="Sim_Life_Scenario_Job"></asp:Label>
                            <br /><br />
                            <a class="Sim_Life_Scenario_Name">Career Advancement Posibilities</a>
                            <br />
                            <asp:Label ID="lblAdvancement" runat="server" Text="Advancement Posibilities" CssClass="Sim_Life_Scenario_Job"></asp:Label>
                        </div>
                        
                        <%--Avatars--%>
                        <div id="divAvatars" runat="server" class="Sim_Life_Scenario_BG">
                            <p>Avatars go here:</p>
                            <table>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td></td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <asp:Button ID="btnNext" runat="server" CssClass="button" Text="Continue to Calculations" OnClick="btnNext_Click" />
                    </div>

                    <%--Avatar--%>
                    <div class="Sim_Sponsor">
                        <div class="Sim_Sponsor_Header">
                            <p>Select Avatar:</p>
                        </div>
                        <br />
                        <div class="Sim_Sponsor_Content">
                            <br />                           
                        </div>
                        <br />
                        <asp:Button ID="btnAvatar" runat="server" Text="Choose Avatar" CssClass="Sim_Sponsor_Button" />
                    </div>
                    
                </div>
                <br /><br />               
            </div>

            <%--Popup--%>
            <div id="popup">
                <p class="Sim_Research_Popup_Header">
                    <asp:Label ID="lblPopupText" runat="server" Text="Popup Text"></asp:Label></p>
                <asp:TextBox ID="tbBusinessID" runat="server" CssClass="popup_textbox" TextMode="Number"></asp:TextBox>
                <br />
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
