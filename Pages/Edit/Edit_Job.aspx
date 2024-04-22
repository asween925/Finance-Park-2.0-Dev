﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit_Job.aspx.cs" Inherits="Edit_Job" MaintainScrollPositionOnPostback="true" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Edit Job</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
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

        <%--Content--%>
        <div class="content">
            <h2 class="h2">Edit Job</h2>
            <h3>This page allows you to edit existing jobs.
                <br />
                <br />
                Select a specific job from the drop down menu or search for it using the search textbox and button.
            </h3>
            <p>Job Title:</p>
            <asp:DropDownList ID="ddlJobTitle" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlJobTitle_SelectedIndexChanged" ></asp:DropDownList>&ensp;<asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <p>Type in Job Title:</p>
            <asp:TextBox ID="tbSearch" runat="server" CssClass="textbox"></asp:TextBox>&ensp;<asp:Button ID="btnSubmit" runat="server" Text="Search" CssClass="button" OnClick="btnSubmit_Click" />
            <br /><br />
            <asp:Button ID="btnRefresh" runat="server" CssClass="button" Text="Show All Jobs" OnClick="btnRefresh_Click"/>
            <br /><br />

            <%--Optional Gridview--%>
            <div id="divJobs" runat="server" visible="true">
                <asp:GridView ID="dgvJobs" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="true" AutoGenerateDeleteButton="true" DataKeyNames="ID" CellPadding="5" AllowPaging="True" ShowHeaderWhenEmpty="True" Font-Size="Medium" OnRowEditing="dgvJobs_RowEditing" OnRowCancelingEdit="dgvJobs_RowCancelingEdit" OnRowUpdating="dgvJobs_RowUpdating" OnRowDataBound="dgvJobs_RowDataBound" OnPageIndexChanging="dgvJobs_PageIndexChanging" OnRowDeleting="dgvJobs_RowDeleting" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows"> 
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="Job ID" Visible="false" />
                        <asp:TemplateField HeaderText="Job Title">
                            <ItemTemplate>
                                <asp:Textbox ID="tbJobTitleDGV" runat="server" Text='<%#Bind("jobTitle") %>' CssClass="textbox" Visible="true"></asp:Textbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Business">
                            <ItemTemplate>
                               <asp:Label ID="lblBusinessNameDGV" runat="server" Text='<%#Bind("business") %>' Visible="false"></asp:Label>
                               <asp:DropDownList ID="ddlBusinessNameDGV" runat="server" CssClass="ddl" AutoPostBack="true"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Education <br /> Background">
                            <ItemTemplate>
                                <asp:Label ID="lblEducationBGDGV" runat="server" Text='<%#Bind("educationBG") %>' Visible="false" ReadOnly="true"></asp:Label>
                                <asp:DropDownList ID="ddlEducationBGDGV" runat="server" CssClass="ddl" AutoPostBack="true">
                                    <asp:ListItem>Associate's Degree</asp:ListItem>
                                    <asp:ListItem>Bachelor's Degree</asp:ListItem>
                                    <asp:ListItem>Master's Degree</asp:ListItem>
                                    <asp:ListItem>High School Diploma</asp:ListItem>
                                    <asp:ListItem>Trade/Vocational School</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job Duties">
                            <ItemTemplate>
                                <asp:Textbox ID="tbJobDutiesDGV" runat="server" CssClass="textbox" Text='<%#Bind("jobDuties") %>' TextMode="MultiLine" Height="70px" Width="200px" ></asp:Textbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Educational <br /> Debt">
                            <ItemTemplate>
                                <asp:Textbox ID="tbEdDebtDGV" runat="server" Text='<%#Bind("edDebt") %>' CssClass="textbox" ></asp:Textbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Advancement">
                            <ItemTemplate>
                                <asp:Textbox ID="tbAdvancementDGV" runat="server" Text='<%#Bind("advancement") %>' CssClass="textbox" TextMode="MultiLine" Height="70px" Width="200px"></asp:Textbox>
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

        <%--Delete confirmation--%>
        <script>
            jQuery("a").filter(function () {
                return this.innerHTML.indexOf("Delete") == 0;
            }).click(function () {
                return confirm("Are you sure you want to delete this job?");
            });
        </script>

        <asp:SqlDataSource ID="Review_sds" runat="server"></asp:SqlDataSource>
    </form>
</body>
</html>
