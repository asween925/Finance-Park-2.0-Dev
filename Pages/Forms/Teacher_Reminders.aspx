<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Teacher_Reminders.aspx.cs" Inherits="Teacher_Reminders" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Teacher Reminders</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link href="../../CSS/Print.css" rel="stylesheet" media="print" type="text/css">
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
            <h2 class="h2 no-print">Teacher Reminders</h2>
            <h3 class="no-print">Use this page to print out the teacher reminders form for both the public and private schools.
                <br class="no-print" />
                <br class="no-print" />
                Select either public or private and click the print button.
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
                <br class="no-print"/>
                <asp:Button ID="print_btn" runat="server" Text="Print" CssClass="button no-print" OnClick="print_btn_Click" />
            </div>
            

            <%--Letter--%>
            <div id="letter_div" runat="server" visible="false" class="letter">
                <h4 class="letter_title">Finance Park Teacher Reminders</h4>
                <p class="letter_p">
                    <a class="letter_l">School: <asp:Label ID="schoolName_lbl" runat="server" Font-Bold="false"></asp:Label></a>
                    <a class="letter_r">Teacher: <asp:Label ID="teacherName_lbl" runat="server" Font-Bold="false"></asp:Label></a>
                </p>
                <br />
                <p class="letter_p">
                    <a class="letter_l">Visit Date: <asp:Label ID="visitDate_lbl" runat="server" Font-Bold="false"></asp:Label></a>
                    <a class="letter_r">Number of Students: <asp:Label ID="numOfStud_lbl" runat="server" Font-Bold="false"></asp:Label></a>
                </p>
                <br />
                <p class="letter_p">
                    <a class="letter_l">Number of Volunteers Needed: <asp:Label ID="numOfVol_lbl" runat="server" Text="111" Font-Bold="false"></asp:Label></a>
                    <a class="letter_r">Volunteer Names / Business Assignment Sheets Due By: <asp:Label ID="dueBy_lbl" runat="server" Text="12/28/2024" Font-Bold="false"></asp:Label></a>
                </p>
                <br />
                <p class="letter_p">
                    <a class="letter_l">Volunteer Arrival Time: <asp:Label ID="volArrive_lbl" runat="server" Font-Bold="false"></asp:Label></a>
                    <a class="letter_r">Student Arrival / Dismissal Time: <asp:Label ID="stuArrive_lbl" runat="server" Font-Bold="false"></asp:Label> <asp:Label ID="stuDismiss_lbl" runat="server" Font-Bold="false"></asp:Label></a>
                </p>
                <br />
                <p>If you have any questions or concerns, please call us at (727) 588-3746.</p>
                <br />

                <%--General Information--%>
                <div class="Teacher_Reminders_Section">
                    <p class="letter_p">General Information:</p>
                    <ul id="genPub_ul" runat="server" visible="false">
                        <li style="font-weight: bold;">The cost of the field trip is $3.00 per student.</li>
                        <li>Finance Park teaching materials and workbooks are available on line in Canvas. If you would like to request hard copies of workbooks ($3.00 per workbook) or teacher kits, please completeWorkbook Order Form and call Finance Park to arrange pick up or delivery.</li>
                        <li>Please return all kits on the date of your visit (the cost of not returned teacher kit is $350.00).</li>
                        <li>Send home the Finance Park Information Letter which introduces the program and basic procedures. (Found in your Instructor’s Guide on page 7.)</li>
                        <li>Please have students dress conducive to sitting on the floor during the day.</li>
                    </ul>
                    <ul id="genPri_ul" runat="server" visible="false">
                        <li>Finance Park teaching materials are on loan to you for the teaching of this curriculum. Remember, all materials other than the student workbooks, are non-comsumable.</li>
                        <li>Please return all kits on the date of your visit.</li>
                        <li>Send home the Finance Park Information Letter which introduces the program and basic procedures. (Found in your Instructor’s Guide on page 7.)</li>
                        <li>Please have students dress conducive to sitting on the floor during the day.</li>
                    </ul>
                </div>

                <%--Business Assignment Sheets--%>
                <div class="Teacher_Reminders_Section">
                    <p class="letter_p">Business Assignment Sheets</p>
                    <ul>
                        <li>We will email your Business Assignment Sheets along with instructions for completing them. Please return your completed sheets by <asp:Label ID="dueByLetter_lbl" runat="server"></asp:Label></li>
                        <li>Be certain that each student knows their business assignment BEFORE they arrive at Finance Park.</li>
                    </ul>
                </div>

                <%--Volunteers--%>
                <div class="Teacher_Reminders_Section">
                    <p class="letter_p">Volunteers:</p>
                    <ul id="volPub_ul" runat="server" visible="false">
                        <li>Volunteers are necessary for a successful visit day for your students. Volunteers MUST show a Photo ID.</li>
                        <li>Please note the number of volunteers needed for your visit day(s), found above.</li>
                        <li>Send home the Finance Park Parent Response Letter. (Found in your Instructor’s Guide on page 9.)</li>
                        <li>All volunteers must be registered, approved and active. Level 2 clearance is NOT required.</li>
                        <li>Send home the Finance Park Volunteer Information. (Found in your Instructor’s Guide on page 10.)</li>
                        <li>Please inform them of their arrival time of <asp:Label ID="stuArrivalLetterPub_lbl" runat="server"></asp:Label></li>
                        <li>Send a list of all approved volunteers to Finance Park (brightonk@pcsb.org) by <asp:Label ID="dueByLetter2Pub_lbl" runat="server"></asp:Label></li>                        
                        <li>ALL changes or additions must be forwarded to us prior to the start of training.</li>
                        <li>The volunteers will pay for their lunches upon arrival at the Stavros Institute. Please do not allow them to prepay at school.</li>
                    </ul>
                    <ul id="volPri_ul" runat="server" visible="false">
                        <li>Volunteers are necessary for a successful visit day for your students. Volunteers MUST show a Photo ID.</li>
                        <li>Please note the number of volunteers needed for your visit day(s), found above.</li>
                        <li>Send home the Finance Park Parent Response Letter. (Found in your Instructor’s Guide on page 9.)</li>
                        <li>Send home the Finance Park Volunteer Information. (Found in your Instructor’s Guide on page 10.)</li>
                        <li>Please inform them of their arrival time of <asp:Label ID="stuArrivalLetterPri_lbl" runat="server"></asp:Label></li>
                        <li>Send a list of all approved volunteers to Finance Park (brightonk@pcsb.org) by <asp:Label ID="dueByLetterPri_lbl" runat="server"></asp:Label></li>
                        <li>As we are unable to verify the Approval Status of your volunteers, ALL changes or additions must be forwarded to us.</li>
                        <li>The volunteers will pay for their lunches upon arrival at the Stavros Institute. Please do not allow them to prepay at school.</li>
                    </ul>
                </div>

                <%--Lunches--%>
                <div class="Teacher_Reminders_Section">
                    <p class="letter_p">Lunches</p>
                    <ul id="lunchPub_ul" runat="server" visible="false">
                        <li style="font-weight: bold;">McDonald’s burger and chicken nuggets, and soda are included in the $3.00 cost of the field trip.</li>
                        <li>Salads are no longer available.</li>
                        <li>Staff lunches are $3.00, cash or credit, paid upon arrival at the Stavros Institute.</li>
                        <li>Your school will be invoiced for the trip(s) AFTER all classes from your school have attended. DO NOT BRING A CHECK.</li>
                    </ul>
                    <ul id="lunchPri_ul" runat="server" visible="false">                        
                        <li>Each McDonald’s lunch includes a burger and chicken nuggets, a snack and soda.</li>
                        <li>Only student lunches are included in the $18.00 per student price of your school’s visit.</li>
                        <li>Salads are no longer available.</li>
                        <li>Staff lunches are $3.00, cash or credit, paid upon arrival at the Stavros Institute.</li>
                    </ul>
                </div>

                <%--Transportation--%>
                <div class="Teacher_Reminders_Section">
                    <p class="letter_p">Transportation</p>
                    <ul id="transportPub_ul" runat="server" visible="false">
                        <li>We provide transportation for students and staff. Please let us know if you need a wheelchair bus.</li>
                        <li>Our bus drivers will contact you with details about transportation the week prior to your visit.</li>
                        <li>Please provide the list of students attending the field trip. We need this list to be in compliance with our Crisis Plan.</li>
                        <li>Due to the limited space on our buses, students can ONLY attend Finance Park on their assigned day.</li>
                        <li>Remember, food, drinks and gum are not allowed on the buses or any time during the trip. Please remind students before they get on the bus.</li>
                    </ul>
                    <ul id="transportPri_ul" runat="server" visible="false">
                        <li>Transportation to and from Finance Park is NOT included in the student price but may be available upon approval of the Stavros Institute Director. Arrangements must be made each year by completing the Transportation Request Form found on the Stavros website: www.stavrosinstitute.org</li>
                    </ul>                  
                </div>

                <%--Payment--%>
                <div id="paymentPri_div" runat="server" visible="false" class="Teacher_Reminders_Section">
                    <p class="letter_p">Payment</p>
                    <ul>
                        <li>An invoice for your school’s visit will be issued on the day after your visit. Payment will be expected no later than 30 days following your visit date.</li>
                        <li>A $350.00 fee for any kit not returned the day of your visit, as well as a $3.00 fee for each unused workbook not returned, will also be included in this final invoice amount.</li>
                    </ul>
                </div>

            </div>
        </div>

        <asp:HiddenField ID="visitID_hf" runat="server" />
        <asp:HiddenField ID="schoolID_hf" runat="server" />

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
