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
        <header class="headerTop no-print"><a style="float: left; padding-top: 2px;">Finance Park 2.0</a><a style="float: right; padding-right: 30px; padding-top: 2px;"><asp:Label ID="headerSchoolName_lbl" Text="School Name Here" runat="server"></asp:Label></a></header>

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
            <asp:DropDownList ID="taxName_ddl" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="taxName_ddl_SelectedIndexChanged">
                <asp:ListItem></asp:ListItem>
                <asp:ListItem>Federal</asp:ListItem>
                <asp:ListItem>FICA</asp:ListItem>
                <asp:ListItem>Medicare</asp:ListItem>
            </asp:DropDownList>&ensp;<asp:Label ID="error_lbl" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <br />
            <br />

            <%--Taxes Div--%>
            <div id="taxes_div" runat="server" visible="false">

                <%--Federal--%>
                <div id="federal_div" runat="server" visible="false">

                    <%--Single--%>
                    <p style="font-weight: bold;">Marital Status: Single</p>
                    <table>
                        <tbody>
                            <tr>
                                <td>GAI Between:</td>
                                <td>
                                    <asp:TextBox ID="gaiRange1MinS_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="gaiRange1MaxS_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="taxEqual1LeftS_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="taxEqual1RightS_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="gmi1S_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
                                </td>
                            </tr>
                            <tr>
                                <td>GAI Between:</td>
                                <td>
                                    <asp:TextBox ID="gaiRange2MinS_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="gaiRange2MaxS_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="taxEqual2LeftS_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="taxEqual2RightS_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="gmi2S_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
                                </td>
                            </tr>
                            <tr>
                                <td>GAI Between:</td>
                                <td>
                                    <asp:TextBox ID="gaiRange3MinS_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="gaiRange3MaxS_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="taxEqual3LeftS_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="taxEqual3RightS_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="gmi3S_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
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
                                    <asp:TextBox ID="gaiRange1MinM_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="gaiRange1MaxM_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="taxEqual1LeftM_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="taxEqual1RightM_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="gmi1M_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
                                </td>
                            </tr>
                            <tr>
                                <td>GAI Between:</td>
                                <td>
                                    <asp:TextBox ID="gaiRange2MinM_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="gaiRange2MaxM_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="taxEqual2LeftM_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="taxEqual2RightM_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="gmi2M_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
                                </td>
                            </tr>
                            <tr>
                                <td>GAI Between:</td>
                                <td>
                                    <asp:TextBox ID="gaiRange3MinM_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox>&</td>
                                <td>
                                    <asp:TextBox ID="gaiRange3MaxM_tb" runat="server" CssClass="textbox" Width="70px"></asp:TextBox></td>
                                <td>Tax is equal to:</td>
                                <td>
                                    <asp:TextBox ID="taxEqual3LeftM_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>+</td>
                                <td>
                                    <asp:TextBox ID="taxEqual3RightM_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>* (GMI - </td>
                                <td>
                                    <asp:TextBox ID="gmi3M_tb" runat="server" CssClass="textbox" Width="60px"></asp:TextBox>)
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>

                <%--FICA--%>
                <div id="fica_div" runat="server" visible="false">
                    <p>Calculation:</p>
                    <asp:TextBox ID="calcF_tb" runat="server" CssClass="textbox"></asp:TextBox>
                    * GMI
                </div>

                <%--Medciare--%>
                <div id="medicare_div" runat="server" visible="false">
                    <p>Calculation:</p>
                    <asp:TextBox ID="calcM_tb" runat="server" CssClass="textbox"></asp:TextBox>
                    * GMI
                </div>

                <br />
                <asp:Button ID="submit_btn" runat="server" CssClass="button" Text="Submit" OnClick="submit_btn_Click" />&ensp;<asp:Button ID="reset_btn" runat="server" CssClass="buttonReset" Text="Reset" OnClick="reset_btn_Click" />
            </div>

        </div>

        <asp:HiddenField ID="currentVisitID_hf" runat="server" />

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
