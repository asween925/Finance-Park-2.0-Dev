﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit_Question.aspx.cs" Inherits="Edit_Question" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - Edit Question</title>

    <link href="../../CSS/StaffPages.css" rel="stylesheet" media="screen" type="text/css">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/font-awesome/4.4.0/css/font-awesome.min.css">
    <link rel="shortcut icon" type="image/png" href="../../Media/faviconFP.png" />

    <script src="https://code.jquery.com/jquery-1.10.2.js"></script>
    <script type="text/javascript">
        window.onload = function () {

            var table = document.getElementById("<%=questions_dgv.ClientID %>");
            var dropdownlist = table.getElementsByTagName('select');
            for (var i = 0; i < dropdownlist.length; i++) {
                if (dropdownlist[i].value == "Written Response") {
                    var parent = dropdownlist[i].parentNode.parentNode;
                    var control = parent.getElementsByTagName('input');
                    for (var j = 0; j < control.length; j++) {
                        if (control[j].getElementById("<%=questions_dgv.FindControl("option1DGV_tb") %>") == true) {
                            control[j].disabled = "disabled";
                        }
                    }
                }
                else {
                }
            }

        }
    </script>
    <script type="text/javascript">
        function EnableDisableTextBox(ddl) {
            var parent = ddl.parentNode.parentNode;
            var control = parent.getElementsByTagName('input');
            for (var i = 0; i < control.length; i++) {
                if (control[i].type == "text" && ddl.value == "Written Response") {
                    control[i].disabled = "disabled";
                }
                if (control[i].type == "text" && ddl.value != "Written Response") {
                    control[i].disabled = "";
                }
            }
        }
    </script>
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
            <h2 class="h2">Edit Question</h2>
            <h3>This page allows you to edit or reorder the lifestyle questions shown during the simulation.
            </h3>
            <asp:Label ID="error_lbl" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
            <br /><br />

            <%--Optional Gridview--%>
            <div id="questions_div" runat="server" visible="true">
                <%--   --%>
                <asp:GridView ID="questions_dgv" runat="server" AutoGenerateColumns="False" AutoGenerateEditButton="true" AutoGenerateDeleteButton="true" DataKeyNames="ID" CellPadding="5" Height="50" AllowPaging="True" ShowHeaderWhenEmpty="True" Font-Size="Medium" OnRowEditing="questions_dgv_RowEditing" OnRowCancelingEdit="questions_dgv_RowCancelingEdit" OnRowUpdating="questions_dgv_RowUpdating" OnRowDataBound="questions_dgv_RowDataBound" OnPageIndexChanging="questions_dgv_PageIndexChanging" OnRowDeleting="questions_dgv_RowDeleting" PagerStyle-CssClass="pager" HeaderStyle-CssClass="header" RowStyle-CssClass="rows" Visible="true"> 
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="false" />
                        <asp:TemplateField HeaderText="Order">
                            <ItemTemplate>
                                <asp:Label ID="questionOrderDGV_lbl" runat="server" Text='<%#Bind("questionOrder") %>' visible="false"></asp:Label>
                                <asp:DropDownList ID="questionOrderDGV_ddl" runat="server" CssClass="ddl" AutoPostBack="true" ></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Text">
                            <ItemTemplate>
                                <asp:Textbox ID="questionTextDGV_tb" runat="server" Text='<%#Bind("questionText") %>' CssClass="textbox" visible="true" Width="300px"></asp:Textbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Answer Type">
                            <ItemTemplate>
                                <asp:Label ID="answerTypeDGV_lbl" runat="server" Text='<%#Bind("answerType") %>' visible="false"></asp:Label>
                                <asp:DropDownList ID="answerTypeDGV_ddl" runat="server" CssClass="ddl" AutoPostBack="true" ></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Category">
                            <ItemTemplate>
                                <asp:Label ID="questionCategoryDGV_lbl" runat="server" Text='<%#Bind("questionCategory") %>' visible="false"></asp:Label>
                                <asp:DropDownList ID="questionCategoryDGV_ddl" runat="server" CssClass="ddl" AutoPostBack="true" onchange="EnableDisableTextBox(this);" ></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="End<br />of Sim">
                            <ItemTemplate>
                                <asp:Checkbox ID="endOfSimDGV_chk" runat="server" Checked='<%#Bind("endOfSim") %>' visible="true"></asp:Checkbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Show">
                            <ItemTemplate>
                                <asp:Label ID="questionShortDGV_lbl" runat="server" Text='<%#Bind("questionShort") %>' CssClass="textbox" visible="false"></asp:Label>
                                <asp:DropDownList ID="questionShortDGV_ddl" runat="server" CssClass="ddl" AutoPostBack="true" ></asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Active">
                            <ItemTemplate>
                                <asp:Checkbox ID="activeDGV_chk" runat="server" Checked='<%#Bind("active") %>' visible="true"></asp:Checkbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Answer #1">
                            <ItemTemplate>
                                <asp:Textbox ID="option1DGV_tb" runat="server" Text='<%#Bind("option1") %>' CssClass="textbox" visible="true" Width="200px"></asp:Textbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Answer #2">
                            <ItemTemplate>
                                <asp:Textbox ID="option2DGV_tb" runat="server" Text='<%#Bind("option2") %>' CssClass="textbox" visible="true" Width="200px"></asp:Textbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Answer #3">
                            <ItemTemplate>
                                <asp:Textbox ID="option3DGV_tb" runat="server" Text='<%#Bind("option3") %>' CssClass="textbox" visible="true" Width="200px"></asp:Textbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Answer #4">
                            <ItemTemplate>
                                <asp:Textbox ID="option4DGV_tb" runat="server" Text='<%#Bind("option4") %>' CssClass="textbox" visible="true" Width="200px"></asp:Textbox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Answer #5">
                            <ItemTemplate>
                                <asp:Textbox ID="option5DGV_tb" runat="server" Text='<%#Bind("option5") %>' CssClass="textbox" visible="true" Width="200px"></asp:Textbox>
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
