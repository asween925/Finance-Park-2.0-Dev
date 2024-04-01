<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit_Simulation.aspx.cs" Inherits="Edit_Simulation" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Edit Visit</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
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
            <h2 class="h2">Edit Simulation</h2>
            <h3>This page will allow you to edit the simulation of an existing simulation.
                <br /><br />
                Enter an existing simulation visit date in the textbox below, then click the 'Edit' button to edit a simulation.
            </h3>
            <p>Visit Date:</p>
            <asp:TextBox ID="tbVisitDate" runat="server" AutoPostBack="true" CssClass="textbox" TextMode="Date" OnTextChanged="tbVisitDate_TextChanged" ></asp:TextBox>&ensp;<asp:Label ID="lblError" runat="server" Font-Size="X-Large" ForeColor="Red" Font-Bold="true"></asp:Label>
            <br /><br />

            <%--Edit Visit Table--%>             
            <div class="gridviewDiv" id="divGridview" runat="server" visible="false">              
                <asp:GridView ID="dgvVisit" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="true" OnRowEditing="dgvVisit_RowEditing" OnRowCancelingEdit="dgvVisit_RowCancelingEdit" OnRowUpdating="dgvVisit_RowUpdating" OnRowDataBound="dgvVisit_RowDataBound" OnPageIndexChanging="dgvVisit_PageIndexChanging" DataKeyNames="ID" CellPadding="5" Font-Size="Medium" Visible="true" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                        <asp:TemplateField HeaderText="Visit Date">
                            <ItemTemplate>
                                <asp:TextBox ID="tbVisitDateDGV" runat="server" Text='<%#Bind("visitDate", "{0:d}") %>' TextMode="Date" CssClass="textbox" ></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Visit Time">
                            <ItemTemplate>
                                <asp:TextBox ID="tbVisitTimeDGV" runat="server" Text='<%#Bind("visitTime") %>' TextMode="Time" CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="School #1">
                            <ItemTemplate>
                                <asp:Label id="lblSchoolName1DGV" runat="server" Text='<%#Bind("schoolid1") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="ddlSchool1DGV" runat="server" AutoPostBack="true" Width="150px"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="School #2">
                            <ItemTemplate>
                                <asp:Label id="lblSchoolName2DGV" runat="server" Text='<%#Bind("schoolid2") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="ddlSchool2DGV" runat="server" AutoPostBack="true" Width="150px"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="School #3">
                            <ItemTemplate>
                                <asp:Label id="lblSchoolName3DGV" runat="server" Text='<%#Bind("schoolid3") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="ddlSchool3DGV" runat="server" AutoPostBack="true" Width="150px"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="School #4">
                            <ItemTemplate>
                                <asp:Label id="lblSchoolName4DGV" runat="server" Text='<%#Bind("schoolid4") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="ddlSchool4DGV" runat="server" AutoPostBack="true" Width="100px"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="School #5">
                            <ItemTemplate>
                                <asp:Label id="lblSchoolName5DGV" runat="server" Text='<%#Bind("schoolid5") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="ddlSchool5DGV" runat="server" AutoPostBack="true" Width="100px"></asp:DropDownList>                           
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Training </br> Time Start">
                            <ItemTemplate>
                                <asp:TextBox ID="tbVTrainingTimeDGV" runat="server" Text='<%#Bind("vTrainingTime") %>' Textmode="Time" CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Volunteer Min Count">
                            <ItemTemplate>
                                <asp:TextBox ID="tbVMinCountDGV" runat="server" Text='<%#Bind("vMinCount") %>' Width="55px" CssClass="textbox" TextMode="Number"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderText="Volunteer Max Count">
                            <ItemTemplate>
                                <asp:TextBox ID="tbVMaxCountDGV" runat="server" Text='<%#Bind("vMaxCount") %>' Width="55px" CssClass="textbox" TextMode="Number"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Student Count">
                            <ItemTemplate>
                                <asp:TextBox ID="tbStudentCountDGV" runat="server" Text='<%#Bind("studentCount") %>' Width="50px" CssClass="textbox" TextMode="Number"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vol Names </br> & Assign. Sheets </br> Due By Date">
                            <ItemTemplate>
                                <asp:TextBox ID="tbDueByDGV" runat="server" Text='<%#Bind("dueBy") %>' TextMode="Date" CssClass="textbox"></asp:TextBox>
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
