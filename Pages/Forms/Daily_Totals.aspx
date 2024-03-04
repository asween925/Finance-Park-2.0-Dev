<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Daily_Totals.aspx.cs" Inherits="Daily_Totals" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Daily Totals</title>

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
            <h2 class="h2 no-print">Daily Totals</h2>
            <h3 class="no-print">Use this page to print out the daily totals for the public and prive schools.
            </h3>
            <p class="no-print">Visit Date:</p>
            <asp:TextBox ID="visitDate_tb" runat="server" TextMode="Date" CssClass="textbox no-print" AutoPostBack="true" OnTextChanged="visitDate_tb_TextChanged"></asp:TextBox>

            <%--School Name DDL--%>
            <div id="schoolName_div" runat="server" visible="false">
                <p class="no-print">School Name:</p>
                <asp:DropDownList ID="schoolName_ddl" runat="server" CssClass="ddl no-print" AutoPostBack="true" OnSelectedIndexChanged="schoolName_ddl_SelectedIndexChanged"></asp:DropDownList>
            </div>

            <%--Teacher Name DDL--%>
            <div id="teacherName_div" runat="server" visible="false">
                <p class="no-print">Teacher Name:</p>
                <asp:DropDownList ID="teacherName_ddl" runat="server" CssClass="ddl no-print" AutoPostBack="true" OnSelectedIndexChanged="teacherName_ddl_SelectedIndexChanged"></asp:DropDownList>
            </div>

            <%--Letter Type DDL--%>
            <div id="letterType_div" runat="server" visible="false">
                <p class="no-print">Letter Type:</p>
                <asp:DropDownList ID="letterType_ddl" runat="server" AutoPostBack="true" CssClass="ddl no-print" OnSelectedIndexChanged="letterType_ddl_SelectedIndexChanged">
                    <asp:ListItem>Public</asp:ListItem>
                    <asp:ListItem>Private</asp:ListItem>
                </asp:DropDownList><a class="no-print">&ensp;</a><asp:Label ID="error_lbl" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
                <br class="no-print" /><br class="no-print" />
                <asp:Button ID="print_btn" runat="server" Text="Print" CssClass="button no-print" OnClick="print_btn_Click" />         
            </div>

            <%--Letter--%>
            <div id="letter_div" runat="server" visible="false" class="letter">
                <h4 class="letter_title">Pinellas County Schools Form</h4>
                <p class="letter_p">Visit Date: <asp:Label ID="visitDate_lbl" runat="server"></asp:Label></p>
                <p class="letter_p">Total Number of <asp:Label ID="schoolName_lbl" runat="server"></asp:Label> students attending: <asp:Label ID="studentCount_lbl" runat="server"></asp:Label></p>

                <%--Private--%>
                <p id="pri1_p" runat="server" visible="false" class="letter_p">Number of workbooks recieved by <asp:Label ID="schoolNamePri1_lbl" runat="server"></asp:Label>: <asp:Label ID="booksRec_lbl" runat="server"></asp:Label></p>
                <p id="pri2_p" runat="server" visible="false" class="letter_p">Number of unused workbooks returned: <asp:Label ID="booksRet_lbl" runat="server"></asp:Label></p>

                <br />
                <p class="letter_r" style="font-style: italic;">Please circle your answer:</p>
                <br /><br />
                <p><a class="letter_l">Have you turned in a list of students attending today to our front office?</a>&ensp;<a class="letter_r">Yes&emsp;&emsp;&emsp;No</a></p>
                
                <%--Private--%>
                <div id="pri_div" runat="server" visible="false">
                    <p><a class="letter_l">Did you return your Finance Park kit(s)?</a>&ensp;<a class="letter_r">Yes&emsp;&emsp;&emsp;No</a></p>
                    <p><a class="letter_l">Did we transport your students today?</a>&ensp;<a class="letter_r">Yes&emsp;&emsp;&emsp;No</a></p>
                </div>

                <br />
                <p><a class="letter_l">Teacher Signature:_______________________________________</a>&ensp;<a class="letter_r">Date:____________________</a></p>
                <br />
                <p>Correct email address: <asp:Label ID="email_lbl" runat="server"></asp:Label></p>
                
                <%--For FP Staff Only--%>
                <div id="fpOnly_lbl" runat="server" style="border: 2px solid black;">
                    <p class="letter_p" style="text-align: center;">For Finance Park Staff Only</p>
                    <p style="text-align: center;">Lunch Information</p>
                    <p style="margin-left: 10px;">Number of students <i style="font-weight: bold;">NOT</i> eating a McDonald's lunch:<a class="letter_r" style="margin-right: 10px;">________</a></p>
                    <p style="margin-left: 10px;">Number of staff purchasing lunch:<a class="letter_r" style="margin-right: 10px;">________</a></p>
                </div>
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
