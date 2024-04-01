<%@ Page Language="C#" AutoEventWireup="true" CodeFile="School_Visit_Checklist.aspx.cs" Inherits="School_Visit_Checklist" %>

<!doctype html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0; maximum-scale=2.0; user-scalable=0;">

    <title>FP 2.0 - School Visit Checklist</title>

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
            <h2 class="h2 no-print">School Visit Checklist</h2>
            <h3 class="no-print">This page is used to start the school visit checklist process.
                <br />
                <br />
                Enter a visit date and school name to view the checklist. Each section is designed for a specific staff member to use before they send it off to another staff member.
            </h3>

            <%--Visit Date Section (This is in a div because of the no-print class)--%>
            <div class="no-print">
                <p>Visit Date:</p>
                <asp:TextBox ID="tbVisitDate" runat="server" TextMode="Date" CssClass="textbox no-print" AutoPostBack="true" OnTextChanged="tbVisitDate_TextChanged"></asp:TextBox>&ensp;<asp:Label ID="lblError" runat="server" Font-Bold="true" Font-Size="X-Large" ForeColor="Red"></asp:Label>
                
                <%--School Name Section--%>
                <div id="divSchoolName" runat="server" visible="false">
                    <p>School Name:</p>
                    <asp:DropDownList ID="ddlSchoolName" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlSchoolName_SelectedIndexChanged"></asp:DropDownList>
                    <br /><br />
                </div>
                
                <%--Buttons--%>
                <div id="divButtons" runat="server" visible="false">
                    <asp:Button ID="btnPrintFull" runat="server" Text="Print Full Checklist" CssClass="button no-print" OnClick="btnPrintFull_Click" />&ensp;<asp:Button ID="btnPrintTicket" runat="server" Text="Print Delivery Ticket" CssClass="button no-print" OnClick="btnPrintTicket_Click" />&ensp;<asp:Button ID="btnRefresh" runat="server" Text="Refresh" CssClass="button no-print" OnClick="btnRefresh_Click" />
                    <br /><br />
                </div>                
            </div>
            
            <%--Checklist--%>
            <div id="divChecklist" runat="server" visible="false" class="SVC_Letter_Width">

                <%--Print Only Title--%>
                <div id="divTitlePrint" runat="server" visible="false">
                    <h4 class="letter_title">Finance Park School Visit Checklist</h4>
                </div>

                <%--Step 1 (Teacher Use Only)--%>
                <div id="divS1" runat="server" visible="true">
                    <h4 class="SVC_Section_Title" >STEP 1: TEACHER ONLY
                        <asp:Label ID="lblS1Comp" runat="server" Visible="false" Font-Bold="true" Font-Italic="true" Text=" - COMPLETED"></asp:Label></h4>
                    <p class="SVC_p no-print">Last Edited By: 
                        <asp:Label ID="lblS1LastEdit" runat="server" Font-Bold="false" Text="None"></asp:Label></p>
                    <p class="SVC_p">School Type:</p>
                    <asp:DropDownList ID="ddlS1SchoolType" runat="server" CssClass="ddl">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>PCSB / Other</asp:ListItem>
                        <asp:ListItem>Other</asp:ListItem>
                    </asp:DropDownList>
                    <p class="SVC_p">Student Count Form Received:</p>
                    <asp:TextBox ID="tbS1StudentForm" runat="server" CssClass="textbox" TextMode="Date"></asp:TextBox>
                    <p class="SVC_p">School Name: 
                        <asp:Label ID="lblS1SchoolName" runat="server" Font-Bold="false"></asp:Label>&ensp;<a class="letter_r">Visit Date: 
                        <asp:Label ID="lblS1VisitDate" runat="server" Font-Bold="false"></asp:Label></a></p>
                    <p class="SVC_p">Contact Teacher: 
                        <asp:Label ID="lblS1ContactTeacher" runat="server" Font-Bold="false"></asp:Label>&ensp;<a class="letter_r">Number of Students: 
                        <asp:Label ID="lblS1NumStudents" runat="server" Font-Bold="false"></asp:Label></a></p>
                    <p class="SVC_p">Admin Email: 
                        <asp:Label ID="lblS1AdminEmail" runat="server" Font-Bold="false"></asp:Label></p>
                    <asp:Button ID="btnS1Submit" runat="server" Text="Submit" CssClass="button no-print" OnClick="btnS1Submit_Click" />
                    <br class="no-print" />
                    <br class="no-print" />
                </div>

                <%--Step 2 (Bookkeeper Use Only)--%>
                <div id="divS2" runat="server" visible="true" class="SVC_Section_Line">
                    <h4 class="SVC_Section_Title">STEP 2: BOOKKEEPER ONLY
                        <asp:Label ID="lblS2Comp" runat="server" Visible="false" Font-Bold="true" Font-Italic="true" Text=" - COMPLETED"></asp:Label></h4>
                    <p class="SVC_p no-print">Last Edited By: 
                        <asp:Label ID="lblS2LastEdit" runat="server" Font-Bold="false" Text="None"></asp:Label></p>
                    <asp:CheckBox ID="chkS2Invoice" runat="server" Text="Invoice # Issued" />&ensp;<asp:CheckBox ID="chkS2Director" runat="server" Text="Director's Signature" />
                    <br /><br />
                    <asp:Button ID="btnS2Submit" runat="server" Text="Submit" CssClass="button no-print" OnClick="btnS2Submit_Click" />
                    <br class="no-print" />
                    <br class="no-print" />
                </div>

                <%--Step 3 (Front Office Use Only)--%>
                <div id="divS3" runat="server" visible="true" class="SVC_Section_Line">
                    <h4 class="SVC_Section_Title">STEP 3: FRONT OFFICE ONLY
                        <asp:Label ID="lblS3Comp" runat="server" Visible="false" Font-Bold="true" Font-Italic="true" Text=" - COMPLETED"></asp:Label></h4>
                    <p class="SVC_p no-print">Last Edited By: 
                        <asp:Label ID="lblS3LastEdit" runat="server" Font-Bold="false" Text="None"></asp:Label></p>                    
                    <p class="SVC_p">Contract Received On:</p> 
                    <asp:Textbox ID="tbS3Contract" runat="server" CssClass="textbox" TextMode="Date"></asp:Textbox>
                    <p class="SVC_p">Invoice #:</p>
                    <asp:Textbox ID="tbS3Invoice" runat="server" CssClass="textbox" Width="70px" TextMode="Number"></asp:Textbox>
                    <p class="SVC_p">Delivery Method:</p>
                    <asp:DropDownList ID="ddlS3Delivery" runat="server" CssClass="ddl">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem>Pick Up</asp:ListItem>
                        <asp:ListItem>Pony</asp:ListItem>
                        <asp:ListItem>Delivered</asp:ListItem>
                        <asp:ListItem>Mailed</asp:ListItem>
                    </asp:DropDownList>
                    <p class="SVC_p">Notes: </p>
                    <asp:TextBox ID="tbS3Notes" runat="server" CssClass="textbox" TextMode="MultiLine" Width="300px"></asp:TextBox>
                    <br />
                    <br />
                    <asp:Button ID="btnS3Submit" runat="server" Text="Submit" CssClass="button no-print" OnClick="btnS3Submit_Click" />
                    <br class="no-print" />
                    <br class="no-print" />
                </div>

                <%--Step 4 (TA Use Only)--%>
                <div id="divS4" runat="server" visible="true" class="SVC_Section_Line">
                    <h4 class="SVC_Section_Title">STEP 4: TA ONLY<asp:Label ID="lblS4Comp" runat="server" Visible="false" Font-Bold="true" Font-Italic="true" Text=" - COMPLETED"></asp:Label></h4>
                    <p class="SVC_p no-print">Last Edited By: 
                        <asp:Label ID="lblS4LastEdit" runat="server" Font-Bold="false" Text="None"></asp:Label></p>
                    <p class="SVC_p">Number of Kits: </p>
                    <asp:DropDownList ID="ddlS4NumKits" runat="server" CssClass="ddl" AutoPostBack="true" OnSelectedIndexChanged="ddlS4NumKits_SelectedIndexChanged">
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
                    </asp:DropDownList>
                    <p class="SVC_p">Kit Numbers:</p>
                    <asp:TextBox ID="tbS4Kit1" runat="server" CssClass="textbox" TextMode="Number" Width="60px"></asp:TextBox>&ensp;<asp:TextBox ID="tbS4Kit2" runat="server" CssClass="textbox" TextMode="Number" Visible="False" Width="60px"></asp:TextBox>&ensp;<asp:TextBox ID="tbS4Kit3" runat="server" CssClass="textbox" TextMode="Number" Visible="False" Width="60px"></asp:TextBox>&ensp;<asp:TextBox ID="tbS4Kit4" runat="server" CssClass="textbox" TextMode="Number" Visible="False" Width="60px"></asp:TextBox>&ensp;<asp:TextBox ID="tbS4Kit5" runat="server" CssClass="textbox" TextMode="Number" Visible="False" Width="60px"></asp:TextBox>&ensp;<asp:TextBox ID="tbS4Kit6" runat="server" CssClass="textbox" TextMode="Number" Visible="False" Width="60px"></asp:TextBox>&ensp;<asp:TextBox ID="tbS4Kit7" runat="server" CssClass="textbox" TextMode="Number" Visible="False" Width="60px"></asp:TextBox>&ensp;<asp:TextBox ID="tbS4Kit8" runat="server" CssClass="textbox" TextMode="Number" Visible="False" Width="60px"></asp:TextBox>&ensp;<asp:TextBox ID="tbS4Kit9" runat="server" CssClass="textbox" TextMode="Number" Visible="False" Width="60px"></asp:TextBox>&ensp;<asp:TextBox ID="tbS4Kit10" runat="server" CssClass="textbox" TextMode="Number" Visible="False" Width="60px"></asp:TextBox>&ensp;
                    <p class="SVC_p">Materials included for <asp:TextBox ID="tbS4Workbooks" runat="server" CssClass="textbox" Width="40px" TextMode="Number"></asp:TextBox> &nbsp;students</p>
                    <asp:Button ID="btnS4Submit" runat="server" Text="Submit" CssClass="button no-print" OnClick="btnS4Submit_Click" />
                    <br class="no-print" /><br class="no-print" />
                </div>

                <%--Step 5 (Front Office Use Only)--%>
                <div id="divS5" runat="server" visible="true" class="SVC_Section_Line">
                    <h4 class="SVC_Section_Title">STEP 5: FRONT OFFICE ONLY
                        <asp:Label ID="lblS5Comp" runat="server" Visible="false" Font-Bold="true" Font-Italic="true" Text=" - COMPLETED"></asp:Label></h4>
                    <p class="SVC_p no-print">Last Edited By: 
                        <asp:Label ID="lblS5LastEdit" runat="server" Font-Bold="false" Text="None"></asp:Label></p>
                    <p class="SVC_p">Delivery Accepted By:</p>
                    <asp:TextBox ID="tbS5DelAcc" runat="server" CssClass="textbox"></asp:TextBox>
                    <p class="SVC_p">Position:</p>
                    <asp:TextBox ID="tbS5Position" runat="server" CssClass="textbox"></asp:TextBox>
                    <p class="SVC_p">Date Accepted:</p>
                    <asp:TextBox ID="tbS5DateAcc" runat="server" CssClass="textbox" TextMode="Date"></asp:TextBox>
                    <br class="no-print" /><br class="no-print" />
                    <asp:Button ID="btnS5Submit" runat="server" Text="Submit" CssClass="button no-print" OnClick="btnS5Submit_Click" />
                    <br class="no-print" />
                    <br class="no-print" />
                </div>

                <%--FP Logo Print Only--%>
                <asp:Image ID="imgStavrosLogo" runat="server" ImageUrl="~/Media/FP_SI_Logo.png" CssClass="letter_logo_bottom" Visible="false" />

            </div>
         
        </div>

        <asp:HiddenField ID="hfCurrentVID" runat="server" />

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
