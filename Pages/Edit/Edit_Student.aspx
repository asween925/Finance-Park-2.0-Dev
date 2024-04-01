﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit_Student.aspx.cs" Inherits="Edit_Student" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Edit Student</title>

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
            <h2 class="h2 no-print">Edit Student</h2>
            <h3 class="no-print">This page allows you to edit existing student's information in the database.
                <br />
                <br />
                Enter a visit date to see all students for that date. Use the Edit / Update buttons to make and save changes to the database.
            </h3>
            <p>Visit Date:</p>
            <asp:TextBox ID="tbVisitDate" runat="server" TextMode="Date" CssClass="textbox" AutoPostBack="true" OnTextChanged="tbVisitDate_TextChanged"></asp:TextBox>&ensp;<asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            
            <%--School Name Section--%>
            <div id="divSchoolName" runat="server" visible="false">
                <p>School Name:</p>
                <asp:DropDownList ID="ddlSchoolName" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlSchoolName_SelectedIndexChanged"></asp:DropDownList>
                <br />
                <br />
                <asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="button no-print" OnClick="btnRefresh_Click" />
            </div>
            <br />

            <%--Optional Gridview--%>
            <div id="divStudents" runat="server" visible="true">
                <%--   --%>
                <asp:GridView ID="dgvStudents" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="true" AutoGenerateDeleteButton="true" DataKeyNames="ID" CellPadding="5" Height="50" AllowPaging="True" ShowHeaderWhenEmpty="True" Font-Size="Medium" OnRowEditing="dgvStudents_RowEditing" OnRowCancelingEdit="dgvStudents_RowCancelingEdit" OnRowUpdating="dgvStudents_RowUpdating" OnRowDataBound="dgvStudents_RowDataBound" OnPageIndexChanging="dgvStudents_PageIndexChanging" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Visible="true"> 
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />                        
                        <asp:TemplateField HeaderText="Account Number">
                            <ItemTemplate>
                               <asp:TextBox ID="tbAccountNumDGV" runat="server" TextMode="Number" Width="100px" Text='<%#Bind("accountNum") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="pin" HeaderText="Pin #" ReadOnly="true" />
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
                        <asp:TemplateField HeaderText="School Name">
                            <ItemTemplate>
                                <asp:Label ID="lblSchoolIDDGV" runat="server" Text='<%#Bind("schoolID") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="ddlSchoolNameDGV" runat="server" AutoPostBack="true" readonly="false"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Business Name">
                            <ItemTemplate>
                                <asp:Label ID="lblBusinessIDDGV" runat="server" Text='<%#Bind("businessID") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="ddlBusinessNameDGV" runat="server" AutoPostBack="true" readonly="false"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job">
                            <ItemTemplate>
                                <asp:Label ID="lblJobIDDGV" runat="server" Text='<%#Bind("jobID") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="ddlJobNameDGV" runat="server" AutoPostBack="true" readonly="false"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Teacher Name">
                            <ItemTemplate>
                                <asp:Label ID="lblTeacherIDDGV" runat="server" Text='<%#Bind("teacherID") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="ddlTeacherNameDGV" runat="server" AutoPostBack="true" readonly="false"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Persona">
                            <ItemTemplate>
                                <asp:Label ID="lblPersonaIDDGV" runat="server" Text='<%#Bind("personaID") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="ddlPersonaNameDGV" runat="server" AutoPostBack="true" readonly="false"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Lunch Served?">
                            <ItemTemplate>
                                <asp:Checkbox ID="chkLunchServedDGV" runat="server" Checked='<%#Bind("lunchServed") %>' Visible="true"></asp:Checkbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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