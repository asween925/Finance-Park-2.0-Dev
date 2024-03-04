<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Create_Simulation.aspx.cs" Inherits="Create_Simulation" MaintainScrollPositionOnPostback="true" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Create a Simulation</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css" />
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

        <%-- Content--%>
        <div class="content">
            <h2 class="h2">Create a Simulation</h2>
            <h3>This page is used to create a new school visit.
                <br />
                <br />
                Enter the information currently known below. You do not need to fill out all the information, the empty fields will be a default value in the database. You can change this later on the 'Edit Visit' page under 'Edit'.
            </h3>

            <asp:Button ID="Button2" runat="server" Text="School Visit Checklist" PostBackUrl="~/school_visit_checklist.aspx" CssClass="button" />&ensp;<asp:Label runat="server" ID="error_lbl" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <p>Visit Date (Required):</p>
            <asp:TextBox ID="visitDate_tb" Width="110px" runat="server" TextMode="Date" CssClass="textbox" AutoPostBack="true" OnTextChanged="visitDate_tb_TextChanged"></asp:TextBox>
            <p>Visit Time (School Schedule):</p>
            <asp:DropDownList CssClass="ddl" ID="visitTime_ddl" runat="server" Visible="true" AutoPostBack="true" OnSelectedIndexChanged="visitTime_ddl_SelectedIndexChanged1"></asp:DropDownList>
            <p>Approximate Student Count:</p>
            <asp:TextBox TextMode="Number" ID="studentCount_tb" Width="45px" runat="server" CssClass="textbox"></asp:TextBox>
            <p>Volunteer Training Start Time:</p>
            <asp:TextBox ID="volunteerTime_tb" runat="server" CssClass="textbox" TextMode="Time" ></asp:TextBox>
            <p>Volunteer Names / Business Assignment Sheets Due By:</p>
            <asp:TextBox ID="dueBy_tb" runat="server" TextMode="Date" CssClass="textbox"></asp:TextBox>

            <%--Select Schools and teachers--%>
            <p>School #1 (Required): &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&ensp; <a id="teacher1_a" runat="server" visible="false">Teachers for School #1:</a></p>
            <asp:DropDownList CssClass="ddl" ID="schools_ddl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="schools_ddl_SelectedIndexChanged"></asp:DropDownList>&emsp;
            
            <div id="teachers1_div" runat="server" visible="false" style="display: inline;">
                <asp:DropDownList ID="teacher1_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>&ensp;
                <asp:DropDownList ID="teacher12_ddl" runat="server" CssClass="ddl"></asp:DropDownList>&ensp;
                <asp:DropDownList ID="teacher13_ddl" runat="server" CssClass="ddl"></asp:DropDownList>
            </div>
            
            <p>School #2: &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp; <a id="teacher2_a" runat="server" visible="false">Teachers for School #2:</a></p>
            <asp:DropDownList CssClass="ddl" ID="schools2_ddl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="schools2_ddl_SelectedIndexChanged"></asp:DropDownList>&emsp;
            
            <div id="teachers2_div" runat="server" visible="false" style="display: inline;">
                <asp:DropDownList ID="teacher2_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="teacher22_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="teacher23_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
            </div>

            <p>School #3: &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp; <a id="teacher3_a" runat="server" visible="false">Teachers for School #3:</a></p>
            <asp:DropDownList CssClass="ddl" ID="schools3_ddl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="schools3_ddl_SelectedIndexChanged"></asp:DropDownList>&emsp;
            
            <div id="teachers3_div" runat="server" visible="false" style="display: inline;">
                <asp:DropDownList ID="teacher3_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="teacher32_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="teacher33_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
            </div>

            <p>School #4: &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp; <a id="teacher4_a" runat="server" visible="false">Teachers for School #4:</a></p>
            <asp:DropDownList CssClass="ddl" ID="schools4_ddl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="schools4_ddl_SelectedIndexChanged"></asp:DropDownList>&emsp;
            
            <div id="teachers4_div" runat="server" visible="false" style="display: inline;">
                <asp:DropDownList ID="teacher4_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="teacher42_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="teacher43_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
            </div>

            <p>School #5: &emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&emsp;&nbsp; <a id="teacher5_a" runat="server" visible="false">Teachers for School #5:</a></p>
            <asp:DropDownList CssClass="ddl" ID="schools5_ddl" runat="server" AutoPostBack="true" OnSelectedIndexChanged="schools5_ddl_SelectedIndexChanged"></asp:DropDownList>&emsp;
            
            <div id="teachers5_div" runat="server" visible="false" style="display: inline;">
                <asp:DropDownList ID="teacher5_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="teacher52_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
                <asp:DropDownList ID="teacher53_ddl" runat="server" CssClass="ddl" ></asp:DropDownList>
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
            <asp:Button ID="openAll_btn" runat="server" CssClass="button" Text="Open All Businesses" OnClick="openAll_btn_Click" />&ensp;<asp:Button ID="Submit_btn" runat="server" Text="Submit" CssClass="button" OnClick="Submit_btn_Click1" />&ensp;<asp:Label ID="success_lbl" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="X-Large"></asp:Label>
            <br />
            <br />
        </div>

        <asp:HiddenField ID="visitdate_hf" runat="server" />
        <asp:HiddenField ID="visitdateUpdate_hf" runat="server" />

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
