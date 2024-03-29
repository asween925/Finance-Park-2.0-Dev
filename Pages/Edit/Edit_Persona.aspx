﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit_Persona.aspx.cs" Inherits="Edit_Persona" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Edit Persona</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
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
            <h2 class="h2">Edit Persona</h2>
            <h3>This page allows you to edit existing personas.
            </h3>
            <p>Job Title:</p>
            <asp:DropDownList ID="jobTitle_ddl" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="jobTitle_ddl_SelectedIndexChanged"></asp:DropDownList>&ensp;<asp:Label ID="error_lbl" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <br /><br />
            <asp:Button ID="refresh_btn" runat="server" CssClass="button" Text="Show All Personas" OnClick="refresh_btn_Click" />
            <br /><br />

            <%--Optional Gridview--%>
            <div id="personas_div" runat="server" visible="true">
                <asp:GridView ID="persona_dgv" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="true" AutoGenerateDeleteButton="true" DataKeyNames="ID" CellPadding="5" Height="50" AllowPaging="True" ShowHeaderWhenEmpty="True" Font-Size="Medium" OnRowEditing="persona_dgv_RowEditing" OnRowCancelingEdit="persona_dgv_RowCancelingEdit" OnRowUpdating="persona_dgv_RowUpdating" OnRowDataBound="persona_dgv_RowDataBound" OnPageIndexChanging="persona_dgv_PageIndexChanging" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Visible="true"> 
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="Persona Number" Visible="false" />
                        <asp:TemplateField HeaderText="Job Title">
                            <ItemTemplate>
                                <asp:Label ID="jobTitleDGV_lbl" runat="server" Text='<%#Bind("jobID") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="jobTitleDGV_ddl" runat="server" readonly="false"></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Job Type">
                            <ItemTemplate>
                                <asp:Label ID="jobTypeDGV_lbl" runat="server" Text='<%#Bind("jobType") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="jobTypeDGV_ddl" runat="server" readonly="false">
                                    <asp:ListItem>Part Time</asp:ListItem>
                                    <asp:ListItem>Full Time</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="GAI">
                            <ItemTemplate>
                               <asp:TextBox ID="gaiDGV_tb" runat="server" ReadOnly="false" Text='<%#Bind("gai") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Age">
                            <ItemTemplate>
                               <asp:TextBox ID="ageDGV_tb" runat="server" Width="50px" ReadOnly="false" Text='<%#Bind("age") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Marital Status">
                            <ItemTemplate>
                                <asp:Label ID="maritalStatusDGV_lbl" runat="server" Text='<%#Bind("maritalStatus") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="maritalStatusDGV_ddl" runat="server" readonly="false" AutoPostBack="true">
                                    <asp:ListItem>Single</asp:ListItem>
                                    <asp:ListItem>Married</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Spouse Age">
                            <ItemTemplate>
                               <asp:TextBox ID="spouseAgeDGV_tb" runat="server" Width="50px" ReadOnly="false" Text='<%#Bind("spouseAge") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Number<br />of Children">
                            <ItemTemplate>
                                <asp:Label ID="numOfChildrenDGV_lbl" runat="server" Text='<%#Bind("numOfChildren") %>' Visible="false"></asp:Label>
                                <asp:DropDownList CssClass="ddl" ID="numOfChildrenDGV_ddl" runat="server" readonly="false" AutoPostBack="true">
                                    <asp:ListItem>0</asp:ListItem>
                                    <asp:ListItem>1</asp:ListItem>
                                    <asp:ListItem>2</asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Child #1 Age">
                            <ItemTemplate>
                               <asp:TextBox ID="child1AgeDGV_tb" runat="server" Width="50px" ReadOnly="false" Text='<%#Bind("child1Age") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Child #2 Age">
                            <ItemTemplate>
                               <asp:TextBox ID="child2AgeDGV_tb" runat="server" Width="50px" ReadOnly="false" Text='<%#Bind("child2Age") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Credit Score">
                            <ItemTemplate>
                               <asp:TextBox ID="creditScoreDGV_tb" runat="server" Width="60px" ReadOnly="false" Text='<%#Bind("creditScore") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="NMI">
                            <ItemTemplate>
                               <asp:TextBox ID="nmiDGV_tb" runat="server" Width="80px" ReadOnly="false" Text='<%#Bind("nmi") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="CC Debt">
                            <ItemTemplate>
                               <asp:TextBox ID="ccDebtDGV_tb" runat="server" Width="60px" ReadOnly="false" Text='<%#Bind("ccDebt") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Furniture Limit">
                            <ItemTemplate>
                               <asp:TextBox ID="furnitureLimitDGV_tb" runat="server" Width="60px" ReadOnly="false" Text='<%#Bind("furnitureLimit") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Home<br />Improvement Limit">
                            <ItemTemplate>
                               <asp:TextBox ID="homeImpLimitDGV_tb" runat="server" Width="60px" ReadOnly="false" Text='<%#Bind("homeImpLimit") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Longterm<br />Savings">
                            <ItemTemplate>
                               <asp:TextBox ID="longSavingsDGV_tb" runat="server" Width="60px" ReadOnly="false" Text='<%#Bind("longSavings") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Emergency<br />Funds">
                            <ItemTemplate>
                               <asp:TextBox ID="emergFundsDGV_tb" runat="server" Width="60px" ReadOnly="false" Text='<%#Bind("emergFunds") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Other<br />Savings">
                            <ItemTemplate>
                               <asp:TextBox ID="otherSavingsDGV_tb" runat="server" Width="60px" ReadOnly="false" Text='<%#Bind("otherSavings") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>                       
                        <asp:TemplateField HeaderText="Auto Loan<br />Amount">
                            <ItemTemplate>
                               <asp:TextBox ID="autoLoanAmntDGV_tb" runat="server" Width="60px" ReadOnly="false" Text='<%#Bind("autoLoanAmnt") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mortgage<br />Amount">
                            <ItemTemplate>
                               <asp:TextBox ID="mortAmntDGV_tb" runat="server" Width="60px" ReadOnly="false" Text='<%#Bind("mortAmnt") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="That's Life<br />Amount">
                            <ItemTemplate>
                               <asp:TextBox ID="thatsLifeAmntDGV_tb" runat="server" Width="60px" ReadOnly="false" Text='<%#Bind("thatsLifeAmnt") %>' CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                               <asp:TextBox ID="descriptionDGV_tb" runat="server" ReadOnly="false" Text='<%#Bind("description") %>' TextMode="MultiLine" CssClass="textbox"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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
