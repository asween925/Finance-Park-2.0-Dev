<%@ Page Language="C#" AutoEventWireup="false" CodeFile="Liaison_Letter.aspx.cs" Inherits="Liaison_Letter" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Family & Community Liaison Information</title>

    <link href="../../CSS/Print.css" rel="stylesheet" media="print" type="text/css" />
    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <link rel="shortcut icon" type="image/png" href="../../Media/FP_favicon_2.png" />
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
            <h2 class="h2 no-print">Family & Community Liaison Information</h2>
            <h3 class="no-print">This page is to print out a PDF of visit information for the volunteer liaison for the school.</h3>          
            <p class="no-print">Visit Date:</p>
            <asp:TextBox ID="visitDate_tb" runat="server" TextMode="Date" AutoPostBack="true" CssClass="textbox no-print" OnTextChanged="visitDate_tb_TextChanged"></asp:TextBox>&emsp;<asp:Label runat="server" ID="error_lbl" Font-Size="X-Large" ForeColor="Red" CssClass="no-print"></asp:Label>
            <p id="school_p" runat="server" visible="false" class="no-print">School Name:</p>
            <asp:DropDownList ID="schoolName_ddl" runat="server" AutoPostBack="true" Visible="false" CssClass="ddl no-print" OnSelectedIndexChanged="schoolName_ddl_SelectedIndexChanged"></asp:DropDownList>
            <br class="no-print" /><br class="no-print" />

            <%--Letter--%>
            <div id="letter_div" runat="server" visible="false">
                <asp:Button ID="print_btn" runat="server" Text="Print" CssClass="button no-print" OnClick="print_btn_Click" />
                <br />
                <div id="info_div" runat="server" visible="true">
                    <h3 style="text-align: center;">Enterprise Village Family & Community Liaison Information</h3>
                    <br />
                    <p>School: <asp:Label ID="schoolName_lbl" runat="server" Font-Bold="true"></asp:Label>&emsp; Liaison: <asp:Label ID="liaison_lbl" runat="server" Font-Bold="true"></asp:Label></p>
                    <p>From: <a style="font-weight: bold;">Karen Brighton, Community Liaison, Stavros Institute</a></p>
                    <p>&nbsp;</p>
                    <p>Your school is scheduled to visit Finance Park on: <asp:Label ID="visitDate_lbl" runat="server" Font-Bold="true"></asp:Label>.</p>
                    <p>Volunteer training for your school will be held on that day.</p>
                    <p style="font-weight: bold;">Volunteers need to only be Level I approved. They will be asked to present a driver's license or legal government ID on arrival.</p>
                    <p>Volunteers should arrive at Finance Park by <asp:Label ID="arrivalTime_lbl" runat="server" Font-Bold="true"></asp:Label>. Dismissal will be at approximately <asp:Label ID="volunteerDismisal_lbl" runat="server" Font-Bold="true"></asp:Label>.</p>
                    <p>The number of volunteers needed each day is calculated based on the number of students attending. Please reference the teacher schedule beflow for this information.</p>
                    <p>Each participating Finance Park teacher has been sent a Finance Park Parent Volunteer Letter to be sent home with each student. This letter includes the Finance Park Response Form which the parents are to complete and return. Once the students have returned the forms, the teachers will forward them to you for registration in the Volunteer system.</p>
                    <p>a complete list of all volunteers for each day should be sent to Karen Brighton by <asp:Label ID="replyBy_lbl" runat="server" Font-Bold="true"></asp:Label>.</p>
                    <p>Also, please let your staff and parents know that they can learn more about Finance Park by accessing our website, <a style="font-weight: bold;">www.stavrosinstitute.org.</a></p>
                    <p>If you have any questions, please call at 727-588-3746.</p>
                </div>
            </div>
            
            <%--FP Logo for Printing--%>        
            <asp:Image ID="FPLogo_img" runat="server" ImageUrl="../../Media/FP_SI_Logo.png" AlternateText="Finance Park Logo" ImageAlign="bottom" CssClass="FP_logo_print" Visible="true" />
        </div>

        <asp:HiddenField ID="visitdate_hf" runat="server" />
        <asp:HiddenField ID="businessID_hf" runat="server" />

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


