<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit_Taxes.aspx.cs" Inherits="Edit_Taxes" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Edit Taxes</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <link rel="shortcut icon" type="image/png" href="../../Media/faviconFP.png" />

    <script src="https://code.jquery.com/jquery-1.10.2.js"></script>
</head>

<body>
    <form id="EMS_Form" runat="server">

        <%--Header information--%>
        <header class="headerTop no-print"><a style="float: left; padding-top: 2px;">Finance Park 2.0</a><a style="float: right; padding-right: 30px; padding-top: 2px;"><asp:Label ID="lblHeaderSchoolName" Text="School Name Here" runat="server"></asp:Label></a></header>

        <%--Navigation bar--%>
        <div id="nav-placeholder">
        </div>

        <script>
            $(function () {
                $("#nav-placeholder").load("../../nav.html");
            });
        </script>

        <%--Content--%>
        <div class="content">
            <h2 class="h2">Edit Taxes</h2>
            <h3>This page allows you to edit the tax information for the simulation.
            </h3>
            <p>Tax Name:</p>
            <asp:DropDownList ID="ddlTaxName" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlTaxName_SelectedIndexChanged">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>Federal</asp:ListItem>
                <asp:ListItem>FICA</asp:ListItem>
                <asp:ListItem>Medicare</asp:ListItem>
            </asp:DropDownList>&ensp;<asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <br />
            <br />

            <%--Taxes Div--%>
            <div id="divTaxes" runat="server" visible="false">

                <%--Federal--%>
                <div id="divFederal" runat="server" visible="false">

                    <%--Single--%>
                    <p style="font-weight: bold;">Marital Status: Single</p>
                    <table>
                        <tbody>
                            <tr>
                                <td>GAI Between:</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange1MinS" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange1MaxS" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual1LeftS" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual1RightS" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="tbGMI1S" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
                                </td>
                            </tr>
                            <tr>
                                <td>GAI Between:</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange2MinS" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange2MaxS" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual2LeftS" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual2RightS" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="tbGMI2S" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
                                </td>
                            </tr>
                            <tr>
                                <td>GAI Between:</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange3MinS" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange3MaxS" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual3LeftS" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual3RightS" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="tbGMI3S" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
                                </td>
                            </tr>
                        </tbody>
                    </table>

                    <p style="font-weight: bold;">Marital Status: Married</p>
                    <table>
                        <tbody>
                            <tr>
                                <td>GAI Between:</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange1MinM" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange1MaxM" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual1LeftM" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual1RightM" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="tbGMI1M" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
                                </td>
                            </tr>
                            <tr>
                                <td>GAI Between:</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange2MinM" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange2MaxM" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual2LeftM" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual2RightM" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="tbGMI2M" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
                                </td>
                            </tr>
                            <tr>
                                <td>GAI Between:</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange3MinM" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="tbGAIRange3MaxM" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual3LeftM" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="tbTaxEqual3RightM" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="tbGMI3M" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <%--FICA--%>
                <div id="divFICA" runat="server" visible="false">
                    <p>Calculation:</p>
                    <asp:TextBox ID="tbCalcF" runat="server" CssClass="textbox"></asp:TextBox>
                    * GMI
                </div>

                <%--Medciare--%>
                <div id="divMedicare" runat="server" visible="false">
                    <p>Calculation:</p>
                    <asp:TextBox ID="tbCalcM" runat="server" CssClass="textbox"></asp:TextBox>
                    * GMI
                </div>

                <br />
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="Submit" OnClick="btnSubmit_Click" />&ensp;<asp:Button ID="btnReset" runat="server" CssClass="buttonReset" Text="Reset" OnClick="btnReset_Click" />
            </div>

        </div>

        <asp:HiddenField ID="hfCurrentVisitID" runat="server" />

        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
        <script src="Scripts.js"></script>
        <script>
            /* Loop through all dropdown buttons to toggle between hiding and showing its dropdown content - This allows the user to have multiple dropdowns without any conflict */
            $(".sub-menu ul").hide();
            $(".sub-menu a").click(function () {
                $(this).parent(".sub-menu").children("ul").slideToggle("100");
                $(this).find(".right").toggleClass("fa-caret-up fa-caret-down");
            });
        </script>

        <asp:SqlDataSource ID="Review_sds" runat="server"></asp:SqlDataSource>
    </form>
</body>
</html>
