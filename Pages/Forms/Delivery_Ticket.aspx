<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Delivery_Ticket.aspx.cs" Inherits="Delivery_Ticket" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Delivery Ticket</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link href="../../CSS/Print.css" rel="stylesheet" media="print" type="text/css">
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
            <h2 class="h2 no-print">Delivery Ticket</h2>
            <h3 class="no-print">Use this page to print out the delivery tickets for public and private schools. You can also print out the Kits Only ticket from here.
            </h3>
            <p class="no-print">Visit Date:</p>
            <asp:TextBox ID="tbVisitDate" runat="server" TextMode="Date" CssClass="textbox no-print" AutoPostBack="true" OnTextChanged="tbVisitDate_TextChanged"></asp:TextBox>

            <%--School Name DDL--%>
            <div id="divSchoolName" runat="server" visible="false">
                <p class="no-print">School Name:</p>
                <asp:DropDownList ID="ddlSchoolName" runat="server" CssClass="ddl no-print" AutoPostBack="true" OnSelectedIndexChanged="ddlSchoolName_SelectedIndexChanged"></asp:DropDownList>
            </div>

            <%--Teacher Name DDL--%>
            <div id="divTeacherName" runat="server" visible="false">
                <p class="no-print">Teacher Name:</p>
                <asp:DropDownList ID="ddlTeacherName" runat="server" CssClass="ddl no-print" AutoPostBack="true" OnSelectedIndexChanged="ddlTeacherName_SelectedIndexChanged"></asp:DropDownList>
            </div>

            <%--Letter Type DDL--%>
            <div id="divLetterType" runat="server" visible="false">
                <p class="no-print">Letter Type:</p>
                <asp:DropDownList ID="ddlLetterType" runat="server" AutoPostBack="true" CssClass="ddl no-print" OnSelectedIndexChanged="ddlLetterType_SelectedIndexChanged">
                    <asp:ListItem>Public</asp:ListItem>
                    <asp:ListItem>Private</asp:ListItem>
                    <asp:ListItem>Kits Only</asp:ListItem>
                </asp:DropDownList><a class="no-print">&ensp;</a><asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
                <br class="no-print" />
                <br class="no-print" />
                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="button no-print" OnClick="btnPrint_Click" />
            </div>

            <%--Letter--%>
            <div id="divLetter" runat="server" visible="false" class="letter">
                <h4 class="letter_title">Delivery Ticket</h4>
                <p class="letter_p">
                    School Name:
                    <asp:Label ID="lblSchoolName" runat="server"></asp:Label><a class="letter_r">Contact Person:
                        <asp:Label ID="lblContact" runat="server"></asp:Label></a>
                </p>
                <p class="letter_p">
                    Workbooks Delivered:
                    <asp:Label ID="lblBooks" runat="server"></asp:Label><a class="letter_r">Kit Number(s):
                        <asp:Label ID="lblKits" runat="server"></asp:Label></a>
                </p>
                <p class="letter_p">
                    Stavros Times Delivered:
                    <asp:Label ID="lblTimes" runat="server"></asp:Label><a class="letter_r">Teacher Name:
                        <asp:Label ID="lblTeacherName" runat="server"></asp:Label></a>
                </p>
                <br />

                <%--Public / Private--%>
                <div id="divPub" runat="server" visible="false">
                    <p>Signature of Person receiving Finance Park Materials:<a class="letter_r">_______________________________________</a></p>
                    <br />
                    <br />
                    <p>Date you are recieving Finance Park Materials:<a class="letter_r">_______________________________________</a></p>
                    <br />
                    <h4 class="letter_title">Please return this form to the Front Office to be filled.</h4>
                </div>

                <%--Kits Only--%>
                <div id="divKit" runat="server" visible="false">
                    <p>Kits are going to:</p>
                    <table>
                        <tr>
                            <td class="letter_p" style="text-align: center;">Teacher Name</td>
                            <td class="letter_p" style="text-align: center;">Kit Number</td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTeacherNameKit1" runat="server"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblKits1" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTeacherNameKit2" runat="server"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblKits2" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTeacherNameKit3" runat="server"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblKits3" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblTeacherNameKit4" runat="server"></asp:Label></td>
                            <td>
                                <asp:Label ID="lblKits4" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                    <br />
                    <p>Name of person receiving the Finance Park Kits:<a class="letter_r">_______________________________________</a></p>
                    <br />
                    <br />
                    <p>Position:<a class="letter_r">_______________________________________</a></p>
                    <br />
                    <br />
                    <p>Finance Park Kits:<a class="letter_r">_______________________________________</a></p>
                </div>

                <br />
                <asp:Image ID="imgStavrosLogo" runat="server" ImageUrl="~/Media/FP_SI_Logo.png" CssClass="letter_logo_bottom" />
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
