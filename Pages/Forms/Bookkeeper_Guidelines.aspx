<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Bookkeeper_Guidelines.aspx.cs" Inherits="Bookkeeper_Guidelines" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Bookkeeper Guidelines</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link href="../../CSS/Print.css" rel="stylesheet" media="print" type="text/css">
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
            <h2 class="h2 no-print">Bookkeeper Guidelines</h2>
            <h3 class="no-print">Use this form to print out the bookkeeper guidelines.
            </h3>
            <p class="no-print">Letter Type</p>
            <asp:DropDownList ID="letterType_ddl" runat="server" CssClass="ddl no-print" OnSelectedIndexChanged="letterType_ddl_SelectedIndexChanged" AutoPostBack="true">
                <asp:ListItem>Public / Private</asp:ListItem>
                <asp:ListItem>Home Schooled</asp:ListItem>
            </asp:DropDownList>
            <br class="no-print"/><br class="no-print"/>
            <asp:Button ID="print_btn" runat="server" Text="Print" CssClass="button no-print" OnClick="print_btn_Click" />

            <%--Letter--%> 
            <div id="letter_div" runat="server" class="letter">
                <h4 class="letter_title" style="font-size: 23px;">Bookkeeper Guidelines for Finance Park</h4>

                <%--Top of Letter For Public / Private--%>
                <p id="letterTopPub_p" runat="server" visible="true">The fee for students attending Finance Park is $18.00 per child. <a style="font-weight: bold;">Please note that Teacher and
                    volunteer lunches are not included in the $18.00.</a> This fee <a style="font-weight: bold;">does not</a> include transportation costs.
                    <a style="color: red;">If your school needs transportation, please log onto our website-www.stavrosinstitute.org. Click on
                    Educators, Finance Park, Other School Resources and then Transportation Information.</a> To assist
                    you in preparing for your school’s visit to Finance Park, please use the following guidelines.</p>

                <%--Home Schooled--%>
                <p id="letterTopHome_p" runat="server" visible="false">The fee for students attending Finance Park is $10.00 per child. As we attempt to maintain the $10 student fee, 
                    we had to make some changes regarding our payment guidelines. <a style="font-weight: bold;">Please note that 
                    volunteer lunches will no longer be included.</a></p>
                <br />

                <%--Guidelines For Public / Private--%>
                <table id="tablePub_div" runat="server" visible="true">                    
                    <tr>
                        <td class="letter_p" style="text-align: center;">Curriculum Use</td>
                        <td class="letter_p" style="text-align: center;">Transportation</td>
                    </tr>
                    <tr>
                        <td>Curriculum fees listed below:</td>
                        <td>
                            The fees include the following:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li>The $18.00 fee includes a student lunch, a student workbook, and the program fee.</li>
                                <li>Teachers, volunteers, bus drivers and visiting parents are welcome to purchase a lunch when they arrive at Finance Park.</li>
                                <li>The lunch is ordered from McDonald’s and consists of a double burger, chicken nuggets, soda and a snack.</li>
                                <li> The cost of the lunch is $3.00. (Cash or Credit)</li>
                            </ul>     
                        </td>
                        <td>
                            <ul>
                                <li>Our site to yours to pick up students</li>
                                <li>Your site to ours to drop them off</li>
                                <li>Our site to yours to return the students</li>
                                <li>Your site to ours to return our bus</li>
                                <li>If more than one bus is used, the fee is doubled. We use Google Maps to determine mileage.</li>
                            </ul>
                        </td>
                    </tr>
                </table>

                <%--Home Schooled--%>
                <table id="tableHome_div" runat="server" visible="false">
                    <tr>
                        <td class="letter_p" style="text-align: center;">Curriculum Use</td>
                    </tr>
                    <tr>
                        <td>Curriculum fees listed below:</td>
                    </tr>
                    <tr>
                        <td>
                            <ul>
                                <li>The $10.00 fee includes a student lunch, a student workbook, and the program fee.</li>
                                <li>Teachers, volunteers, bus drivers and visiting parents are welcome to purchase a lunch when they arrive at Finance Park.</li>
                                <li>Pizza: $2.50, Salad: $2.50 (Limited Number)</li>
                            </ul>     
                        </td>
                    </tr>
                </table>

                <p class="letter_p" style="text-align: center;">Payment</p>

                <%--Payment for Public / Private--%>
                <p id="letterPayPub_p" runat="server" visible="true">You will be invoiced <a style="font-weight: bold;">after</a> your visit date for the number of students who participate and shipping
                    fees, if applicable. If your school contracted and received materials for more students than actually
                    attended, you may return all unused workbooks on the day of the visit or you will be invoiced $3.00
                    for each workbook not returned. All checks must be made payable to <a style="font-weight: bold;">The Gus A. Stavros Institute,</a>
                    not to Finance Park. Payment is expected within 30 days of receiving your invoice.</p>

                <%--Home Schooled--%>
                <p id="letterPayHome_p" runat="server" visible="false">You will be invoiced <a style="font-weight: bold;">after</a> 
                    your visit date for the number of students who participated. If your school contracted and recived materials for more students
                    than actually attended, you may return all unused workbooks on the day of the visit or you will be invoiced $3.00 for each workbook not
                    returned. You must also return your Teacher Kit(s) on the day of your visit or you will be billed $350.</p>
                <br />
                <asp:Image ID="stavrosLogo_img" runat="server" ImageUrl="~/Media/FP_SI_Logo.png" CssClass="letter_logo_bottom" />
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
