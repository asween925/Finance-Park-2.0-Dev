<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Volunteer_Database.aspx.cs" Inherits="Volunteer_Database" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Volunteer Database</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link href="../../CSS/Print.css" rel="stylesheet" media="print" type="text/css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <link rel="shortcut icon" type="image/png" href="../../Media/faviconFP.png" />

    <script src="https://code.jquery.com/jquery-1.10.2.js"></script>

    <style>
        .tg-hfk9 {
            font-size: 14px;
        }

        .td-float {
            float: right;
        }
    </style>
</head>

<body>
    <form autocomplete="off" id="EMS_Form" runat="server">

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
            <h2 class="h2">Volunteer Database</h2>
            <h3>This is the volunteer database where you can add a new volunteer to the list and view all the volunteers in the database. Enter a visit date or select a school name to get started.
            </h3>
            <asp:Button ID="btnAddVol" runat="server" CssClass="button" Text="Add New Volunteer" OnClick="addVol_btn_Click" />&ensp;
            <asp:Button ID="btnScheduleVol" runat="server" CssClass="button" Text="Schedule Volunteers" OnClick="scheduleVol_btn_Click" />&ensp;
            <asp:Button ID="btnCheckIn" runat="server" CssClass="button" Text="Check In Volunteers" OnClick="checkIn_btn_Click" />&ensp;
            <asp:Button ID="btnViewVol" runat="server" CssClass="button" Text="View Volunteers" OnClick="viewVol_btn_Click" />
            &ensp;|&ensp;
            <asp:Button ID="btnBusinessAssignments" runat="server" CssClass="button" Text="View Assignments (Opens New Tab)" OnClick="businessAssignments_btn_Click"></asp:Button>&ensp;|&nbsp;
            <asp:Button ID="btnRefresh" runat="server" CssClass="button" Text="Refresh" OnClick="refresh_btn_Click" />&ensp;<asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>

            <%--Add New Volunteer--%>
            <div id="divAddVol" runat="server" visible="false" style="border-bottom: 1px solid gray; padding-bottom: 10px;">
                <h3>Add New Volunteer:</h3>
                <p>First Name:</p>
                <asp:TextBox ID="tbFirstName" runat="server" CssClass="textbox"></asp:TextBox>
                <p>Last Name:</p>
                <asp:TextBox ID="tbLastName" runat="server" CssClass="textbox"></asp:TextBox>
                <p>School Name:</p>
                <asp:DropDownList ID="ddlSchoolNameAdd" runat="server" CssClass="ddl"></asp:DropDownList>
                <p>PR:</p>
                <asp:DropDownList ID="ddlPR" runat="server" CssClass="ddl">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem>Not Registered</asp:ListItem>
                    <asp:ListItem>Active</asp:ListItem>
                    <asp:ListItem>Inactive</asp:ListItem>
                    <asp:ListItem>Hold</asp:ListItem>
                    <asp:ListItem>Pending</asp:ListItem>
                    <asp:ListItem>Staff</asp:ListItem>
                </asp:DropDownList>
                <p>SV Hours:</p>
                <asp:TextBox ID="tbSVHours" runat="server" CssClass="textbox" TextMode="Number" Text="6" Width="70px"></asp:TextBox>
                <p>Regular?</p>
                <asp:DropDownList ID="ddlRegularVol" runat="server" CssClass="ddl" Width="70px">
                    <asp:ListItem></asp:ListItem>
                    <asp:ListItem>No</asp:ListItem>
                    <asp:ListItem>Yes</asp:ListItem>
                    <asp:ListItem>Corporate</asp:ListItem>
                </asp:DropDownList>
                <p>Notes:</p>
                <asp:TextBox ID="tbNotes" runat="server" CssClass="textbox"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="btnSubmit" runat="server" CssClass="button" Text="Submit" OnClick="submit_btn_Click" />
            </div>

            <%--Schedule Volunteers--%>
            <div id="divScheduleVol" runat="server" visible="false" style="border-bottom: 1px solid gray; padding-bottom: 10px;">
                <h3>Schedule Volunteers:</h3>
                <p>Filter by School Name (Optional):</p>
                <asp:DropDownList ID="ddlSchoolNameSchedule" runat="server" AutoPostBack="true" CssClass="ddl" OnSelectedIndexChanged="schoolNameSchedule_ddl_SelectedIndexChanged"></asp:DropDownList>
                <p>Volunteer Name:</p>
                <asp:DropDownList ID="ddlVolNameSchedule" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="volNameSchedule_ddl_SelectedIndexChanged"></asp:DropDownList>
                <br />
                <br />

                <%--Table of Volunteers--%>
                <div id="divScheduledVol" runat="server" visible="false">
                    <asp:GridView ID="dgvScheduledVol" runat="server" Font-Size="Medium" Visible="true" PageSize="20" CellPadding="5" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows"></asp:GridView>

                    <%--Visit Date Schedule--%>
                    <p>Visit Date:</p>
                    <asp:TextBox ID="tbVisitDateSchedule" runat="server" CssClass="textbox" TextMode="Date"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="btnSubmitSchedule" runat="server" CssClass="button" Text="Submit" OnClick="submitSchedule_btn_Click" />
                </div>


            </div>

            <%--Check In Volunteer--%>
            <div id="divCheckIn" runat="server" visible="true" style="border-bottom: 1px solid gray; padding-bottom: 10px;">
                <h3>Check In Volunteers:</h3>
                <p>Visit Date: &emsp;&emsp;&emsp;&emsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <a id="aSchoolNameCheckin" runat="server" visible="false">Scheduled Schools for Selected Visit Date:</a></p>
                <asp:TextBox ID="tbVisitDateCheckin" runat="server" TextMode="Date" CssClass="textbox" AutoPostBack="true" OnTextChanged="tbVisitDateCheckin_TextChanged"></asp:TextBox>&emsp;&emsp;<asp:DropDownList ID="ddlSchoolNameCheckin" runat="server" AutoPostBack="true" CssClass="ddl" Visible="false" OnSelectedIndexChanged="ddlSchoolNameCheckin_SelectedIndexChanged"></asp:DropDownList>
                <br />
                <br />
                <table>
                    <tbody>
                        <tr>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS1" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS1N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS1N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS1" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS2" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS2N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS2N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS2" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS3" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS3N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS3N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS3" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS4" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS4N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS4N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS4" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS5" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS5N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS5N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS5" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS6" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS6N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS6N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS6" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS7" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS7N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS7N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS7" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS8" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS8N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS8N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS8" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS9" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS9N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS9N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS9" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS10" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS10N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS10N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS10" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS11" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS11N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS11N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS11" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS12" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS12N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS12N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS12" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS13" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS13N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS13N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS13" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS14" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS14N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS14N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS14" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS15" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS15N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS15N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS15" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS16" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS16N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS16N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS16" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS17" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS17N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS17N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS17" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS18" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS18N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS18N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS18" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS19" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS19N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS19N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS19" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS20" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS20N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS20N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS20" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS21" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS21N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS21N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS21" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS22" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS22N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS22N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS22" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS23" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS23N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS23N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS23" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS24" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS24N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS24N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS24" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS25" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS25N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS25N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS25" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS26" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS26N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS26N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS26" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS27" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS27N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS27N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS27" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS28" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS28N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS28N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS28" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                        </tr>
                        <tr>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS29" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS29N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS29N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS29" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS30" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS30N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS30N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS30" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS31" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS31N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS31N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS31" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                            <td class="tg-hfk9">
                                <asp:Label ID="lblS32" runat="server" Text="Sponsor Name"></asp:Label>:
                                <asp:CheckBox ID="chkS32N1" runat="server" AutoPostBack="true" Enabled="false" />
                                <asp:CheckBox ID="chkS32N2" runat="server" AutoPostBack="true" Enabled="false" />&ensp;<asp:DropDownList ID="ddlRegVolS32" runat="server" CssClass="ddl td-float" Enabled="false"></asp:DropDownList></td>
                        </tr>                        
                    </tbody>
                </table>
                <br />
                <asp:Button ID="btnSubmitCheckIn" runat="server" CssClass="button" Text="Submit" OnClick="checkIn_btn_Click" />
            </div>

            <%--View Volunteers Control--%>
            <div id="divViewVolControls" runat="server" visible="false" style="border-bottom: 1px solid gray; padding-bottom: 10px;">
                <h3>View Volunteers:</h3>
                <p>Visit Date:</p>
                <asp:TextBox ID="tbVisitDateViewVolCtrl" runat="server" CssClass="textbox" TextMode="Date" AutoPostBack="true" OnTextChanged="tbVisitDateViewVolCtrl_TextChanged"></asp:TextBox>
                <p>or Volunteer Name:</p>
                <asp:DropDownList ID="ddlVolNameViewVolCtrl" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlVolNameViewVolCtrl_SelectedIndexChanged"></asp:DropDownList>
                <br />
                <br />

                <p>
                    Total SV Hours:
                    <asp:Label ID="lblTotalSVHoursCtrl" runat="server"></asp:Label>
                </p>
                <div id="divViewVolCtrlDGV" runat="server" visible="false">
                    <asp:GridView ID="dgvViewVolCtrl" runat="server" AutoGenerateDeleteButton="true" AutoGenerateColumns="False" DataKeyNames="ID" CellPadding="5" Height="50" OnRowDeleting="dgvViewVolCtrl_RowDeleting" AllowPaging="True" ShowHeaderWhenEmpty="True" Font-Size="Medium" Visible="true" PageSize="20" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" Visible="true" />
                            <asp:BoundField DataField="volunteerID" HeaderText="Volunteer ID" ReadOnly="true" Visible="true" />
                            <asp:BoundField DataField="firstName" HeaderText="First Name" ReadOnly="true" Visible="true" />
                            <asp:BoundField DataField="lastName" HeaderText="Last Name" ReadOnly="true" Visible="true" />
                            <asp:BoundField DataField="visitDate" HeaderText="Visit Date" ReadOnly="true" Visible="true" />
                            <asp:BoundField DataField="schoolName" HeaderText="School Name" ReadOnly="true" Visible="true" />
                            <asp:BoundField DataField="pr" HeaderText="PR" ReadOnly="true" Visible="true" />
                            <asp:BoundField DataField="svHours" HeaderText="SV Hours" ReadOnly="true" Visible="true" />
                            <asp:BoundField DataField="notes" HeaderText="Notes" ReadOnly="true" Visible="true" />
                            <asp:BoundField DataField="regular" HeaderText="Regular Volunteer?" ReadOnly="true" Visible="true" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <%--Volunteers Gridview--%>
            <div id="divViewVol" runat="server" visible="true">
                <p>
                    Search by Last Name: 
                    <asp:TextBox ID="tbSearch" runat="server" CssClass="textbox"></asp:TextBox>&ensp;<asp:Button ID="btnSearch" runat="server" CssClass="button" Text="Search" OnClick="search_btn_Click" />&ensp;|&ensp;
                    Sort By:
                    <asp:DropDownList ID="ddlSortBy" runat="server" CssClass="ddl">
                        <asp:ListItem>Recently Added</asp:ListItem>
                        <asp:ListItem>First Name</asp:ListItem>
                        <asp:ListItem>Last Name</asp:ListItem>
                        <asp:ListItem>Sponsor Name</asp:ListItem>
                        <asp:ListItem>School Name</asp:ListItem>
                        <asp:ListItem>Visit Date</asp:ListItem>
                        <asp:ListItem>PR</asp:ListItem>
                        <asp:ListItem>SV Hours</asp:ListItem>
                        <asp:ListItem>Regular Volunteer</asp:ListItem>
                    </asp:DropDownList>
                    &ensp;
                    <asp:DropDownList ID="ddlAscDesc" runat="server" CssClass="ddl">
                        <asp:ListItem>Descending</asp:ListItem>
                        <asp:ListItem>Ascending</asp:ListItem>
                    </asp:DropDownList>&ensp;<asp:Button ID="btnSortBy" runat="server" CssClass="button" Text="Sort" OnClick="sortBy_btn_Click" />
                    &ensp;
                    <a>Total SV Hours:
                        <asp:Label ID="lblTotalSVHours" runat="server" Text="-"></asp:Label></a>
                </p>
                <div>
                    <asp:GridView ID="dgvVolunteers" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="true" AutoGenerateDeleteButton="true" DataKeyNames="ID" OnRowDataBound="dgvVolunteers_RowDataBound" OnRowDeleting="dgvVolunteers_RowDeleting" OnRowEditing="dgvVolunteers_RowEditing" OnRowUpdating="dgvVolunteers_RowUpdating" OnRowCancelingEdit="dgvVolunteers_RowCancelingEdit" OnPageIndexChanging="dgvVolunteers_PageIndexChanging" CellPadding="5" Height="50" AllowPaging="True" ShowHeaderWhenEmpty="True" Font-Size="Medium" Visible="true" PageSize="20" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="true" Visible="true" />
                            <%--<asp:BoundField DataField="visitDate" HeaderText="Visit Date" Visible="true" DataFormatString="{0: MM/dd/yyyy }" />--%>
                            <asp:TemplateField HeaderText="First Name">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbFirstNameDGV" runat="server" Width="80px" Text='<%#Bind("firstName") %>' CssClass="textbox"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Last Name">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbLastNameDGV" runat="server" Width="80px" Text='<%#Bind("lastName") %>' CssClass="textbox"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Visit Date">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbVisitDateDGV" runat="server" TextMode="Date" Text='<%#Bind("visitDate", "{0:yyyy-MM-dd}") %>' CssClass="textbox"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Assigned<br/>Sponsor">
                                <ItemTemplate>
                                    <asp:Label ID="lblSponsorNameDGV" runat="server" Text='<%#Bind("sponsorID") %>' Visible="false"></asp:Label>
                                    <asp:DropDownList CssClass="ddl" ID="ddlSponsorNameDGV" runat="server" Width="200px" ReadOnly="false"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="School Name">
                                <ItemTemplate>
                                    <asp:Label ID="lblSchoolNameDGV" runat="server" Text='<%#Bind("schoolID") %>' Visible="false"></asp:Label>
                                    <asp:DropDownList CssClass="ddl" ID="ddlSchoolNameDGV" runat="server" Width="200px" ReadOnly="false"></asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PR">
                                <ItemTemplate>
                                    <asp:Label ID="lblPRDGV" runat="server" Text='<%#Bind("pr")%>' Visible="false"></asp:Label>
                                    <asp:DropDownList CssClass="ddl" ID="ddlPRDGV" runat="server" ReadOnly="false">
                                        <asp:ListItem>PR</asp:ListItem>
                                        <asp:ListItem>Not Registered</asp:ListItem>
                                        <asp:ListItem>Active</asp:ListItem>
                                        <asp:ListItem>Inactive</asp:ListItem>
                                        <asp:ListItem>Hold</asp:ListItem>
                                        <asp:ListItem>Pending</asp:ListItem>
                                        <asp:ListItem>Staff</asp:ListItem>
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="SV Hours">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbSVHoursDGV" runat="server" Width="50px" Text='<%#Bind("svHours") %>' TextMode="Number" CssClass="textbox"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Notes">
                                <ItemTemplate>
                                    <asp:TextBox ID="tbNotesDGV" runat="server" Width="150px" Text='<%#Bind("notes") %>' CssClass="textbox" TextMode="MultiLine"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Regular<br/>Volunteer?">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkRegularDGV" runat="server" Checked='<%#Bind("regular") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <br />
                    <br />
                </div>
            </div>




            <asp:DropDownList CssClass="ddl" ID="ddlBusinessCount" runat="server" Visible="false">
                <asp:ListItem>1</asp:ListItem>
                <asp:ListItem>2</asp:ListItem>
                <asp:ListItem>3</asp:ListItem>
                <asp:ListItem>4</asp:ListItem>
                <asp:ListItem>5</asp:ListItem>
                <asp:ListItem>6</asp:ListItem>
                <asp:ListItem>7</asp:ListItem>
                <asp:ListItem>8</asp:ListItem>
                <asp:ListItem>9</asp:ListItem>
                <asp:ListItem>10</asp:ListItem>
                <asp:ListItem>11</asp:ListItem>
                <asp:ListItem>12</asp:ListItem>
                <asp:ListItem>13</asp:ListItem>
                <asp:ListItem>14</asp:ListItem>
                <asp:ListItem>15</asp:ListItem>
                <asp:ListItem>16</asp:ListItem>
                <asp:ListItem>17</asp:ListItem>
                <asp:ListItem>18</asp:ListItem>
                <asp:ListItem>19</asp:ListItem>
                <asp:ListItem>20</asp:ListItem>
                <asp:ListItem>21</asp:ListItem>
                <asp:ListItem>22</asp:ListItem>
                <asp:ListItem>23</asp:ListItem>
                <asp:ListItem>24</asp:ListItem>
                <asp:ListItem>25</asp:ListItem>
                <asp:ListItem>26</asp:ListItem>
                <asp:ListItem>27</asp:ListItem>
                <asp:ListItem>28</asp:ListItem>
                <asp:ListItem>29</asp:ListItem>
                <asp:ListItem>30</asp:ListItem>
                <asp:ListItem>31</asp:ListItem>
                <asp:ListItem>32</asp:ListItem>
            </asp:DropDownList>
        </div>

        <asp:HiddenField ID="hfCurrentVisitID" runat="server" />

        <script src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.1.1/jquery.min.js"></script>
        <script src="../../Scripts.js"></script>
        <script>
            /* Loop through all dropdown buttons to toggle between hiding and showing its dropdown content - This allows the user to have multiple dropdowns without any conflict */
            $(".sub-menu ul").hide();
            $(".sub-menu a").click(function () {
                $(this).parent(".sub-menu").children("ul").slideToggle("100");
                $(this).find(".right").toggleClass("fa-caret-up fa-caret-down");
            });
        </script>

        <%--Delete confirmation--%>
        <script>
            jQuery("a").filter(function () {
                return this.innerHTML.indexOf("Delete") == 0;
            }).click(function () {
                return confirm("Are you sure you want to delete this volunteer?");
            });
        </script>

        <asp:SqlDataSource ID="Review_sds" runat="server"></asp:SqlDataSource>
    </form>
</body>
</html>
