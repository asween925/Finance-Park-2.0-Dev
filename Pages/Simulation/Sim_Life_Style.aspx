<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Sim_Life_Style.aspx.cs" Inherits="Sim_Life_Style" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;"/>

    <title>Finance Park - Life Style Questions</title>

    <link href="../../CSS/StudentPages.css" rel="stylesheet" media="screen" type="text/css"/>
    <link rel="shortcut icon" href="../../Media/FP_favicon_2.png" type="image/ico" />
    
</head>
<body>
    <form id="EMS_Form" runat="server">
        <div id="Site_Wrap_Fullscreen">

            <%--Header--%>
            <div id="divHeader" runat="server" class="header">
                <div id="nav-placeholder"></div>
                <a class="headerTitle">Lifestyle Questions</a>
                <a>
                    <asp:Image ID="imgStartLogo" runat="server" ImageUrl="~/Media/FP_Logo.png" CssClass="headerFPLogo" /></a>
            </div>

            <%--Blured Area--%>
            <div id="blur" class="blurArea">

                <%--Directions & Unlocked Biz--%>
                <div class="directionsBlock">
                    <a class="directions">Answer the questions below and click continue to move on.</a>&ensp;
                    <a class="unlocked"><asp:Button ID="btnQ1" runat="server" CssClass="buttonNav" Text="1" OnClick="btnQ1_Click" /><asp:Button ID="btnQ2" runat="server" CssClass="buttonNav" Text="2" OnClick="btnQ2_Click" /><asp:Button ID="btnQ3" runat="server" CssClass="buttonNav" Text="3" OnClick="btnQ3_Click" /><asp:Button ID="btnQ4" runat="server" CssClass="buttonNav" Text="4" OnClick="btnQ4_Click" /><asp:Button ID="btnQ5" runat="server" CssClass="buttonNav" Text="5" OnClick="btnQ5_Click" /><asp:Button ID="btnQ6" runat="server" CssClass="buttonNav" Text="6" OnClick="btnQ6_Click" /><asp:Button ID="btnQ7" runat="server" CssClass="buttonNav" Text="7" OnClick="btnQ7_Click" /><asp:Button ID="btnQ8" runat="server" CssClass="buttonNav" Text="8" OnClick="btnQ8_Click" /><asp:Button ID="btnQ9" runat="server" CssClass="buttonNav" Text="9" OnClick="btnQ9_Click" /><asp:Button ID="btnQ10" runat="server" CssClass="buttonNav" Text="10" OnClick="btnQ10_Click" /></a>
                </div>
                
                <%--Content--%>
                <div class="Sim_Content_With_Sponsor">

                    <%--Questions--%>
                    <div class="Sim_Content_Right">
                        <h3 class="Sim_Life_Style_Question"><asp:Label ID="lblQ" runat="server"></asp:Label></h3>
                
                        <%--Answer Fields--%>
                        <div>
                            <%--Multiple Choice--%>
                            <div id="divA1" runat="server" visible="false" class="Sim_Life_Style_QBox">
                                <label class="container">                                    
                                    <input id="rdoA1" runat="server" type="radio" name="q" />
                                    <asp:Label ID="lblA1" runat="server" Text="Answer 1" CssClass="checkmark"></asp:Label>
                                </label>
                            </div>
                            <div id="divA2" runat="server" visible="false" class="Sim_Life_Style_QBox">
                                <label class="container">                                   
                                    <input id="rdoA2" runat="server" type="radio" name="q" />
                                    <span class="checkmark"><asp:Label ID="lblA2" runat="server" Text="Answer 2"></asp:Label></span>
                                </label>
                            </div>
                            <div id="divA3" runat="server" visible="false" class="Sim_Life_Style_QBox">
                                <label class="container">                                   
                                    <input id="rdoA3" runat="server" type="radio" name="q" />
                                    <span class="checkmark"><asp:Label ID="lblA3" runat="server" Text="Answer 3"></asp:Label></span>
                                </label>
                            </div>
                            <div id="divA4" runat="server" visible="false" class="Sim_Life_Style_QBox">
                                <label class="container">                                    
                                    <input id="rdoA4" runat="server" type="radio" name="q" />
                                    <span class="checkmark"><asp:Label ID="lblA4" runat="server" Text="Answer 4"></asp:Label></span>
                                </label>
                            </div>
                            <div id="divA5" runat="server" visible="false" class="Sim_Life_Style_QBox">
                                <label class="container">                                   
                                    <input id="rdoA5" runat="server" type="radio" name="q" />
                                    <span class="checkmark"><asp:Label ID="lblA5" runat="server" Text="Answer 5"></asp:Label></span>
                                </label>
                            </div>

                            <%--Written Response--%>
                            <div id="divWrite" runat="server" visible="false" class="Sim_Life_Style_Written">
                                <asp:TextBox ID="tbWrittenResponse" runat="server" TextMode="MultiLine" CssClass="Sim_Life_Style_TB"></asp:TextBox>
                                <a class="Sim_Life_Style_Word_Count_A">Word Count: </a>
                            </div>

                            <br />
                            <asp:Button ID="btnNext" runat="server" Text="Next Question" CssClass="button" OnClick="btnNext_Click" />&ensp;<asp:Label ID="lblError" runat="server" ForeColor="Red" Font-Bold="true"></asp:Label>

                        </div>
                    </div>

                    <%--Sponsor--%>
                    <div class="Sim_Sponsor">
                        <div class="Sim_Sponsor_Header">
                            <p>Sponsored By:</p>
                        </div>
                        <asp:Image ID="imgSponsorLogo" runat="server" />
                    </div>
                </div>
                <asp:HiddenField ID="hfQuestion" runat="server" Value="1" />
            </div>
        </div>
    </form>
</body>
</html>
