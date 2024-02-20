<%@ Page Language="C#" AutoEventWireup="false" CodeFile="Home_Page.aspx.cs" Inherits="_Home_Page" %>

<!doctype html>
<html>
<head runat="server">

    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Home</title>

    <link href="../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link rel="shortcut icon" type="image/png" href="../Media/faviconFP.png" />
</head>

<body>
    <form id="EMS_Form" runat="server">
        <div id="site_wrap">

            <%--Header information--%>
            <div class="content_header">
                <div id="header-e1">
                    Finance Park 2.0
                </div>
                <div id="header-e3">
                    <asp:Label ID="headerSchoolName_lbl" Text="School Name Here" runat="server"></asp:Label><asp:Label ID="headerSchoolName2_lbl" runat="server"></asp:Label><asp:Label ID="headerSchoolName3_lbl" runat="server"></asp:Label>
                </div>
                <div id="header-e2">
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                </div>
            </div>

            <%--Content--%>
            <div class="content_home">
                <h2 class="h2" style="text-align: center;">Home Page</h2>

                <%--Today's Date, Student Count, and Schools--%>
                <div class="Home_Page_Todays_Info">
                    <p>Today's Date: <asp:Label ID="visitDate_lbl" runat="server" Font-Italic="true" Font-Bold="false"></asp:Label>&emsp;Student Count: <asp:Label ID="studentCount_lbl" runat="server" Font-Italic="true" Font-Bold="false"></asp:Label></p>
                    <p>Today's School(s): <asp:Label ID="Label1" runat="server" Text="No School Visit Created For Today!" Font-Bold="false" Font-Italic="true"></asp:Label></p>
                </div>                

                <%--Header Links--%>
                <asp:LinkButton ID="LinkButton9" runat="server" PostBackUrl="/Pages/Simulation/Sim_Start.aspx" Font-Size="Large" CssClass="button1">SIMULATION START</asp:LinkButton>
                <asp:LinkButton ID="LinkButton13" runat="server" PostBackUrl="/Pages/Simulation/Sim_Directory.aspx" Font-Size="Large" CssClass="button1">SIMULATION DIRECTORY</asp:LinkButton>
                <asp:LinkButton ID="LinkButton33" runat="server" PostBackUrl="/inventory_home.aspx" Font-Size="Large" CssClass="button1">INVENTORY</asp:LinkButton>
                <asp:LinkButton ID="LinkButton27" runat="server" PostBackUrl="/Help_page.aspx" Font-Size="Large" CssClass="button1">HELP</asp:LinkButton>
                <asp:LinkButton ID="LinkButton24" runat="server" PostBackUrl="/default.aspx" Font-Size="Large" CssClass="button1">LOG OUT</asp:LinkButton>
                <br />
                <asp:Label ID="error_lbl" runat="server" Font-Bold="True" Font-Size="X-Large" ForeColor="Red"></asp:Label>
                <br />
                <br />

                <%--Create Pages--%>
                <div class="Home_Page_Drop_Down_Menu">
                    <button class="Home_Page_Drop_Down_Btn">Create ▼</button>
                    <div class="Home_Page_Drop_Down_Content">
                        <a href="../Pages/Create/Create_Simulation.aspx">Create a Simulation</a>
                        <a href="../Pages/Create/Create_School.aspx">Create a School</a>
                        <a href="../Pages/Create/Create_Teacher.aspx">Create a Teacher</a>
                        <a href="../Pages/Create/Create_Job.aspx">Create a Job</a>
                        <a href="../Pages/Create/Create_Sponsor.aspx">Create a Sponsor</a>
                        <a href="../Pages/Create/Create_Persona.aspx">Create a Persona</a>
                    </div>
                </div>

                <%--Edit Pages--%>
                <div class="Home_Page_Drop_Down_Menu">
                    <button class="Home_Page_Drop_Down_Btn">Edit ▼</button>
                    <div class="Home_Page_Drop_Down_Content">
                        <a href="../Pages/Edit/edit_Simulation.aspx">Edit Simulation</a>
                        <a href="../Pages/Edit/edit_school.aspx">Edit School</a>
                        <a href="../Pages/Edit/edit_teacher.aspx">Edit Teacher</a>
                        <a href="../Pages/Edit/Edit_Job.aspx">Edit Jobs</a>
                        <a href="../Pages/Edit/Edit_Question.aspx">Edit Questions</a>
                        <a href="../Pages/Edit/Edit_Sponsor.aspx">Edit Sponsors</a>
                        <a href="../Pages/Edit/Edit_Persona.aspx">Edit Persona</a>
                        <a href="../Pages/Edit/Edit_Taxes.aspx">Edit Taxes</a>
                    </div>
                </div>

                <%--Tools--%>
                <div class="Home_Page_Drop_Down_Menu">
                    <button class="Home_Page_Drop_Down_Btn">Tools ▼</button>
                    <div class="Home_Page_Drop_Down_Content">
                        <a href="profit_display.aspx">Town Hall Powerpoint</a>
                        <a href="School_Visit_Checklist.aspx">School Visit Checklist</a>
                        <a href="Kit_Inventory.aspx">Kit Inventory</a>
                        <a href="Upload_Move_Articles.aspx">Upload / Move Articles</a>
                        <a href="School_Notes.aspx">School Notes</a>
                        <a href="Schedule_request_form_checklist.aspx">Schedule Request Form Checklist</a>
                        <a href="volunteer_database.aspx">Volunteer Database</a>
                        <a href="Requested_Features.aspx">Requested Features and Bug Reports</a>
                    </div>
                </div>

                <%--Reports--%>
                <div class="Home_Page_Drop_Down_Menu">
                    <button class="Home_Page_Drop_Down_Btn">Reports ▼</button>
                    <div class="Home_Page_Drop_Down_Content">
                        <a href="../Pages/Reports/Simulation_Report.aspx">Visit Report</a>
                        <a href="../Pages/Reports/School_Report.aspx">School Report</a>                      
                        <a href="../Pages/Reports/Teacher_Report.aspx">Teacher Report</a>
                    </div>
                </div>

                <%--Forms and Letters--%>
                <div class="Home_Page_Drop_Down_Menu">
                    <button class="Home_Page_Drop_Down_Btn">Forms and Letters ▼</button>
                    <div class="Home_Page_Drop_Down_Content">
                        <a href="Liason_Information.aspx">Family and Community Liason Information Forms</a>
                        <a href="Teacher_Letter.aspx">Teacher Letter</a>
                        <a href="Parent_Letter.aspx">Parent Letter</a>
                        <a href="FP_Daily_Forms.aspx">FP Daily Forms</a>
                        <a href="FP_Lunch_Forms.aspx">FP Lunch Forms</a>
                        <a href="Staff_List.aspx">Staff List</a>
                        <a href="Break_Schedules.aspx">Break Schedules</a>
                        <a href="Bus_transportation.aspx">Bus Transportation</a>
                        <a href="school_schedule.aspx">School Schedule</a>
                        <a href="closed_business_checks.aspx">Closed Business Checks</a>
                        <a href="visit_calendar.aspx">Visit Calendar</a>
                    </div>
                </div>
                <br />

                <%--Weekly Calendar--%>
                <div class="Home_Page_Calendar_Outer">
                    <h3>Weekly Visits</h3>
                    <div class="Home_Page_Calendar_Inner">
                        <div class="Home_Page_Calendar_Block_Start">
                            <a class="Home_Page_Calendar_Day">Monday</a>
                            <asp:Label ID="monday_lbl" runat="server" CssClass="Home_Page_Calendar_Number" Text="0"></asp:Label>
                            <br />
                            <br />
                            <asp:Button ID="mondaySchool1_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="mondaySchool2_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="mondaySchool3_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="mondaySchool4_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="mondaySchool5_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                        </div>
                        <div class="Home_Page_Calendar_Block">
                            <a class="Home_Page_Calendar_Day">Tuesday</a>
                            <asp:Label ID="tuesday_lbl" runat="server" CssClass="Home_Page_Calendar_Number" Text="0"></asp:Label>
                            <br />
                            <br />
                            <asp:Button ID="tuesdaySchool1_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="tuesdaySchool2_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="tuesdaySchool3_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="tuesdaySchool4_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="tuesdaySchool5_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                        </div>
                        <div class="Home_Page_Calendar_Block">
                            <a class="Home_Page_Calendar_Day">Wednesday</a>
                            <asp:Label ID="wednesday_lbl" runat="server" CssClass="Home_Page_Calendar_Number" Text="0"></asp:Label>
                            <br />
                            <br />
                            <asp:Button ID="wednesdaySchool1_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="wednesdaySchool2_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="wednesdaySchool3_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="wednesdaySchool4_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="wednesdaySchool5_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                        </div>
                        <div class="Home_Page_Calendar_Block">
                            <a class="Home_Page_Calendar_Day">Thursday</a>
                            <asp:Label ID="thursday_lbl" runat="server" CssClass="Home_Page_Calendar_Number" Text="0"></asp:Label>
                            <br />
                            <br />
                            <asp:Button ID="thursdaySchool1_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="thursdaySchool2_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="thursdaySchool3_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="thursdaySchool4_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="thursdaySchool5_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                        </div>
                        <div class="Home_Page_Calendar_Block_End">
                            <a class="Home_Page_Calendar_Day">Friday</a>
                            <asp:Label ID="friday_lbl" runat="server" CssClass="Home_Page_Calendar_Number" Text="0"></asp:Label>
                            <br />
                            <br />
                            <asp:Button ID="fridaySchool1_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="fridaySchool2_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="fridaySchool3_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="fridaySchool4_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                            <asp:Button ID="fridaySchool5_btn" runat="server" CssClass="Home_Page_Calendar_Button" Text="School Name Here" Visible="false"></asp:Button>
                        </div>
                    </div>
                </div>

                <br />

                <%--Changelog popup--%>
                <div class="content_home_onfloor">
                    <div class="popup content_home_changelog" onclick="myFunction()">
                        Latest Updates
                        <div class="popuptext" id="myPopup">
                            <span>Last Updated: 1/11/2023 (v2.2.20)</span>                           

                        </div>
                    </div>
                    |
                    <asp:LinkButton ID="LinkButton32" runat="server" PostBackUrl="/changelog.aspx" CssClass="content_home_changelog">Changelog</asp:LinkButton>
                    |
                    <div class="popup content_home_changelog" id="getJoke">
                        Get Joke
                        <div class="popuptext" id="jokePopup">
                            <a id="jokeSetup">Setup</a>
                            <br />
                            <a id="jokePunchline">Punchline</a>
                        </div>
                    </div>
                    <div class="content_home_bottom">
                        <p>Last Updated: 1/11/2024 @ 3:30pm&ensp;Version: 2.2.20&ensp;Current Visit ID:<asp:Label ID="visitID_lbl" runat="server"></asp:Label></p>
                    </div>

                    <br />
                    <br />
                </div>









            </div>
        </div>

        <asp:HiddenField ID="visitdate_hf" runat="server" />

        <script src="Scripts.js"></script>
        <asp:SqlDataSource ID="Review_sds" runat="server"></asp:SqlDataSource>
        <script>

            // Joke functions (just for fun)
            //
            // Starts when the user clicks on the "button" at the bottom of the page called "Get Joke"
            document.addEventListener("click", function (event) {

                // Checking if the button was clicked
                if (!event.target.matches("#getJoke")) return;

                // Here we send a request to the Joke API, then process the response into JSON, then pass the data to our renderJoke function.               
                fetch("https://official-joke-api.appspot.com/random_joke")
                    .then((response) => response.json())
                    .then((data) => renderJoke(data))
                    .catch(() => renderError());

                // Displays the popup window
                var popup = document.getElementById("jokePopup");
                popup.classList.toggle("show");
            });

            // Function to render the joke
            function renderJoke(data) {

                // Get text elements
                const setup = document.getElementById("jokeSetup");
                const punchline = document.getElementById("jokePunchline");
                const error = document.getElementById("error_lbl");

                // Hide error and render jokes
                error.innerHTML = "";
                setup.innerHTML = data.setup;
                punchline.innerHTML = data.punchline;
            }

            // Function to render the error message if there is one
            function renderError() {
                const error = document.getElementById("error_lbl");
                error.innerHTML = "Whoops, something went wrong. Please try again later!";
            }

            // Opens a popup window for the "Latest Updates" button
            function myFunction() {
                var popup = document.getElementById("myPopup");
                popup.classList.toggle("show");
            }

            // Opens a popup window for the "Weather" button
            function myFunction2() {
                var popup = document.getElementById("myPopup2");
                popup.classList.toggle("Home_Page_Weather_Popup_Show");
            }

        </script>
        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>

        <script>
            /* Loop through all dropdown buttons to toggle between hiding and showing its dropdown content - This allows the user to have multiple dropdowns without any conflict */
            $(".sub-menu ul").hide();
            $(".sub-menu a").click(function () {
                $(this).parent(".sub-menu").children("ul").slideToggle("100");
                $(this).find(".right").toggleClass("fa-caret-up fa-caret-down");
            });
        </script>

    </form>
</body>
</html>

