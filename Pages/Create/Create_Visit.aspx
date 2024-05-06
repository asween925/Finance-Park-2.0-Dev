<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Create_Visit.aspx.cs" Inherits="Create_Visit" MaintainScrollPositionOnPostback="true" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Create a Visit</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css" />
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <link rel="shortcut icon" type="image/png" href="../../Media/FP_favicon_2.png" />
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

        <%-- Content--%>
        <div class="content">
            <h2 class="h2">Create a Visit</h2>
            <h3>This page is used to create a new school visit.
                <br />
                <br />
                Enter the information currently known below. You do not need to fill out all the information, the empty fields will be a default value in the database. You can change this later on the 'Edit Visit' page under 'Edit'.
            </h3>

            <asp:Button ID="Button2" runat="server" Text="School Visit Checklist" PostBackUrl="../Tools/school_visit_checklist.aspx" CssClass="button" />&ensp;<asp:Label runat="server" ID="lblError" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <p>Visit Date (Required):</p>
            <asp:TextBox ID="tbVisitDate" Width="110px" runat="server" TextMode="Date" CssClass="textbox" AutoPostBack="true" OnTextChanged="tbVisitDate_TextChanged"></asp:TextBox>
            <p>Visit Time (School Schedule):</p>
            <asp:DropDownList CssClass="ddl" ID="ddlVisitTime" runat="server" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="ddlVisitTime_SelectedIndexChanged1"></asp:DropDownList>
            <p>Approximate Student Count:</p>
            <asp:TextBox TextMode="Number" ID="tbStudentCount" Width="45px" runat="server" CssClass="textbox"></asp:TextBox>
            <p>Volunteer Training Start Time:</p>
            <asp:TextBox ID="tbVolunteerTime" runat="server" CssClass="textbox" TextMode="Time" ></asp:TextBox>
            <p>Volunteer Names / Business Assignment Sheets Due By:</p>
            <asp:TextBox ID="tbDueBy" runat="server" TextMode="Date" CssClass="textbox"></asp:TextBox>

            <%--Select Schools and teachers--%>
            <p>School #1 (Required): &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&ensp; <a id="aTeacher1" runat="server" visible="false">Teachers for School #1:</a></p>
            <asp:DropDownList CssClass="ddl" ID="ddlSchools" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSchools_SelectedIndexChanged"></asp:DropDownList>&emsp;
            
            <div id="divTeachers1" runat="server" visible="false" style="display: inline;">
                <asp:DropDownList ID="ddlTeacher1" runat="server" CssClass="ddl" ></asp:DropDownList>&ensp;
                <asp:DropDownList ID="ddlTeacher12" runat="server" CssClass="ddl"></asp:DropDownList>&ensp;
                <asp:DropDownList ID="ddlTeacher13" runat="server" CssClass="ddl"></asp:DropDownList>
            </div>
            
            <p>School #2: &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp; <a id="aTeacher2" runat="server" visible="false">Teachers for School #2:</a></p>
            <asp:DropDownList CssClass="ddl" ID="ddlSchools2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSchools2_SelectedIndexChanged"></asp:DropDownList>&emsp;
            
            <div id="divTeachers2" runat="server" visible="false" style="display: inline;">
                <asp:DropDownList ID="ddlTeacher2" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="ddlTeacher22" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="ddlTeacher23" runat="server" CssClass="ddl" ></asp:DropDownList>
            </div>

            <p>School #3: &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp; <a id="aTeacher3" runat="server" visible="false">Teachers for School #3:</a></p>
            <asp:DropDownList CssClass="ddl" ID="ddlSchools3" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSchools3_SelectedIndexChanged"></asp:DropDownList>&emsp;
            
            <div id="divTeachers3" runat="server" visible="false" style="display: inline;">
                <asp:DropDownList ID="ddlTeacher3" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="ddlTeacher32" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="ddlTeacher33" runat="server" CssClass="ddl" ></asp:DropDownList>
            </div>

            <p>School #4: &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp; <a id="aTeacher4" runat="server" visible="false">Teachers for School #4:</a></p>
            <asp:DropDownList CssClass="ddl" ID="ddlSchools4" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSchools4_SelectedIndexChanged"></asp:DropDownList>&emsp;
            
            <div id="divTeachers4" runat="server" visible="false" style="display: inline;">
                <asp:DropDownList ID="ddlTeacher4" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="ddlTeacher42" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="ddlTeacher43" runat="server" CssClass="ddl" ></asp:DropDownList>
            </div>

            <p>School #5: &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp; <a id="aTeacher5" runat="server" visible="false">Teachers for School #5:</a></p>
            <asp:DropDownList CssClass="ddl" ID="ddlSchools5" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSchools5_SelectedIndexChanged"></asp:DropDownList>&emsp;
            
            <div id="divTeachers5" runat="server" visible="false" style="display: inline;">
                <asp:DropDownList ID="ddlTeacher5" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="ddlTeacher52" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="ddlTeacher53" runat="server" CssClass="ddl" ></asp:DropDownList>
            </div>

            <%--Open closed business--%>
            <p>Select Businesses to Open:</p>
            <div>
                <table>
                    <tr>
                        <td>
                            <asp:CheckBox ID="Checkbox1" runat="server"  Text="Auto Insurance" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox6" runat="server"  Text="Bank Mortgage" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox7" runat="server"  Text="Bank Savings" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="Checkbox8" runat="server"  Text="Bank Auto Loan" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox9" runat="server"  Text="Cable" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox10" runat="server"  Text="Childcare" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="Checkbox11" runat="server"  Text="Clothing" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox12" runat="server"  Text="Credit Cards" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox13" runat="server"  Text="Dining Out" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="Checkbox14" runat="server"  Text="Disability/Life Insurance" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox15" runat="server"  Text="Education" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox16" runat="server"  Text="Entertainment" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="Checkbox17" runat="server"  Text="Furniture" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox18" runat="server"  Text="Gas and Maintenance" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox19" runat="server"  Text="Grocery" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="Checkbox20" runat="server"  Text="Health Insurance" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox21" runat="server"  Text="Home Improvement" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox22" runat="server"  Text="Home Insurance" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="Checkbox23" runat="server"  Text="Housing" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox24" runat="server"  Text="Internet" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox25" runat="server"  Text="Investments" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="Checkbox26" runat="server"  Text="Philanthropy" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox27" runat="server"  Text="Phone" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox28" runat="server"  Text="That's Life" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="Checkbox29" runat="server"  Text="Transportation" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox30" runat="server"  Text="Utilities - Water/Sewer/Trash" /></td>
                        <td>
                            <asp:CheckBox ID="Checkbox31" runat="server"  Text="Utilities - Electricity" /></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:CheckBox ID="Checkbox32" runat="server"  Text="Utilities - Gas" /></td>
                        <td>
                            </td>
                        <td>
                            </td>
                    </tr>
                </table>
            </div>
            <br />
            <br />
            <asp:Button ID="btnOpenAll" runat="server" CssClass="button" Text="Open All Businesses" OnClick="btnOpenAll_Click" />&ensp;<asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button" OnClick="btnSubmit_Click1" />&ensp;<asp:Label ID="lblSuccess" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="X-Large"></asp:Label>
            <br />
            <br />
        </div>

        <asp:HiddenField ID="hfVisitdate" runat="server" />
        <asp:HiddenField ID="hfVisitdateUpdate" runat="server" />

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
