using Microsoft.Ajax.Utilities;
using System;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using static Antlr.Runtime.Tree.TreeWizard;

public partial class Volunteer_Database : Page
{
    private string SQLServer = ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string SQLDatabase = ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string SQLUser = ConfigurationManager.AppSettings["db_user"].ToString();
    private string SQLPassword = ConfigurationManager.AppSettings["db_password"].ToString();
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string ConnectionString;
    private Class_VisitData VisitData = new Class_VisitData();
    private Class_BusinessData BusinessData = new Class_BusinessData();
    private Class_SchoolData SchoolData = new Class_SchoolData();
    private Class_SchoolHeader SH = new Class_SchoolHeader();
    private Class_GridviewFunctions Gridviews = new Class_GridviewFunctions();
    private Class_SponsorData Sponsors = new Class_SponsorData();
    private int VisitID;
    private DataTable ScheduleVolTable;

    public Volunteer_Database()
    {
        ConnectionString = "Server=" + SQLServer + ";database=" + SQLDatabase + ";uid=" + SQLUser + ";pwd=" + SQLPassword + ";Connection Timeout=20;";
        VisitID = VisitData.GetVisitID();
        Load += Page_Load;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Check if user is logged in
        if (HttpContext.Current.Session["LoggedIn"] == null)
        {
            Response.Redirect("../../Default.aspx");
        }

        if (!IsPostBack)
        {

            // Set session for scheduled vol gridviews
            ScheduleVolTable = new DataTable();
            ScheduleVolTable.Columns.Add("Volunteer ID");
            ScheduleVolTable.Columns.Add("Volunteer Name");
            Session["dt"] = ScheduleVolTable;

            // Populating school header
            lblHeaderSchoolName.Text = SH.GetSchoolHeader().ToString();

            // Populate school name ddl in Add New Volunteer
            SchoolData.LoadSchoolsDDL(ddlSchoolNameAdd, true);
            SchoolData.LoadSchoolsDDL(ddlSchoolNameSchedule, true);

            // Populate volunteer names in ddl in Schedule Volunteers
            LoadVolunteerNameDDL(ddlVolNameSchedule);

            // Populate ddlRegular volunteer DDLs
            PopulateVolDDL();

            //Load sponsor names into labels on check in screen
            LoadSponsorNames();

            // Load data
            LoadData();
        }
        else
        {
            ScheduleVolTable = (DataTable)Session["dt"];
        }
    }

    // Loads the volunteer gridview based on what button was clicked at the top of the page / what section is currently visible 
    public void LoadData()
    {
        int VIDOfDate;
        int SchoolID;
        DateTime VisitDateFromID;
        string SQLStatement = @"SELECT vo.id, vo.firstName, vo.lastName, vo.sponsorID, FORMAT(v.visitDate, 'yyyy-MM-dd') as visitDate, vo.schoolID, vo.pr, vo.svHours, vo.notes, vo.regular
										  FROM volunteersFP vo
										  JOIN volunteersScheduleFP vs ON vo.id = vs.volunteerID
										  JOIN visitInfoFP v ON v.id = vs.visitID";

        // Clear error and reset all fields
        lblError.Text = "";
        lblTotalSVHours.Text = "-";
        divViewVol.Visible = false;
        ddlSchoolNameCheckin.Visible = false;

        // Check which section is visible
        // Add Volunteer
        if (divAddVol.Visible == true)
        {
            divViewVol.Visible = false;
        }

        // Schedule Volunteer
        else if (divScheduleVol.Visible == true)
        {

            // Check if visit date has been entered
            if (ddlVolNameSchedule.SelectedIndex == 0)
            {
                SQLStatement = SQLStatement;
            }
        }

        // Check In Volunteer
        else if (divCheckIn.Visible == true)
        {

            // Check if current date is a visit date
            if (tbVisitDateCheckin.Text != "")
            {
                
                // Assign SQL statement
                VIDOfDate = int.Parse(VisitData.GetVisitIDFromDate(tbVisitDateCheckin.Text).ToString());
                SQLStatement = SQLStatement + " WHERE vs.visitID = '" + VIDOfDate + "'";

                // Load total SV hours
                lblTotalSVHours.Text = TotalSVHours(tbVisitDateCheckin.Text).ToString();

                // Enable checkboxes
                Checkboxes(VIDOfDate);

                // Load volunteer check in information
                LoadVolCheckIn(tbVisitDateCheckin.Text);

                // Make volunteers div visible
                divViewVol.Visible = true;

                // Make school name ddl visible
                ddlSchoolNameCheckin.Visible = true;
                aSchoolNameCheckin.Visible = true;
            }

            else if (VisitID != 0)
            {

                // Load visit date into text box
                VisitDateFromID = DateTime.Parse(VisitData.GetVisitDateFromID(VisitID).ToString());
                tbVisitDateCheckin.Text = VisitDateFromID.ToString("yyyy-MM-dd");

                // Assign SQL statement
                SQLStatement = SQLStatement + " WHERE vs.visitID = '" + VisitID + "'";

                // Load visiting schools
                SchoolData.LoadVisitDateSchoolsDDL(VisitDateFromID.ToString(), ddlSchoolNameCheckin);
                ddlSchoolNameCheckin.Items.RemoveAt(0);
                ddlSchoolNameCheckin.Items.Insert(0, "Show All Schools");

                // Load total SV hours
                lblTotalSVHours.Text = TotalSVHours(tbVisitDateCheckin.Text).ToString();

                // Enable checkboxes
                Checkboxes(VisitID);

                // Load volunteer check in information
                LoadVolCheckIn(tbVisitDateCheckin.Text);

                // Make volunteers div visible
                divViewVol.Visible = true;

                // Make school name ddl visible
                ddlSchoolNameCheckin.Visible = true;
                aSchoolNameCheckin.Visible = true;

                // Assign colors to school DDL text
                switch (ddlSchoolNameCheckin.Items.Count)
                {
                    case 1:
                        {
                            break;
                        }
                    // The first item in the DDL is the Show All Schools item, does not need to change color
                    case 2:
                        {
                            ddlSchoolNameCheckin.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                            break;
                        }
                    case 3:
                        {
                            ddlSchoolNameCheckin.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                            ddlSchoolNameCheckin.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                            break;
                        }

                    case 4:
                        {
                            ddlSchoolNameCheckin.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                            ddlSchoolNameCheckin.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                            ddlSchoolNameCheckin.Items[3].Attributes.CssStyle.Add("background-color", "#bfffaf");
                            break;
                        }
                    case 5:
                        {
                            ddlSchoolNameCheckin.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                            ddlSchoolNameCheckin.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                            ddlSchoolNameCheckin.Items[3].Attributes.CssStyle.Add("background-color", "#bfffaf");
                            ddlSchoolNameCheckin.Items[4].Attributes.CssStyle.Add("background-color", "#afc3ff");
                            break;
                        }
                    case 6:
                        {
                            ddlSchoolNameCheckin.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                            ddlSchoolNameCheckin.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                            ddlSchoolNameCheckin.Items[3].Attributes.CssStyle.Add("background-color", "#bfffaf");
                            ddlSchoolNameCheckin.Items[4].Attributes.CssStyle.Add("background-color", "#afc3ff");
                            ddlSchoolNameCheckin.Items[5].Attributes.CssStyle.Add("background-color", "#ffd8af");
                            break;
                        }
                }
            }

            else
            {

                // Make volunteers div invisible
                lblError.Text = "No visit date today.";
                divViewVol.Visible = false;

            }
        }

        //View Volunteers
        else
        {
            divViewVol.Visible = true;
        }

        // Check if search bar is filled
        if (tbSearch.Text != "")
        {
            SQLStatement = SQLStatement + " AND vo.lastName LIKE '%" + tbSearch.Text + "%'";
        }

        // Add order by to SQLStatement
        SQLStatement = SQLStatement + " ORDER BY ";

        // Check sorting DDLs
        if (ddlSortBy.SelectedValue == "Recently Added")
        {
            SQLStatement = SQLStatement + "vo.id ";
        }
        else if (ddlSortBy.SelectedValue == "First Name")
        {
            SQLStatement = SQLStatement + "vo.firstName ";
        }
        else if (ddlSortBy.SelectedValue == "Last Name")
        {
            SQLStatement = SQLStatement + "vo.lastName ";
        }
        else if (ddlSortBy.SelectedValue == "Sponsor Name")
        {
            SQLStatement = SQLStatement + "vo.sponsorID ";
        }
        else if (ddlSortBy.SelectedValue == "School Name")
        {
            SQLStatement = SQLStatement + "vo.schoolName ";
        }
        else if (ddlSortBy.SelectedValue == "Visit Date")
        {
            SQLStatement = SQLStatement + "v.visitDate ";
        }
        else if (ddlSortBy.SelectedValue == "PR")
        {
            SQLStatement = SQLStatement + "vo.pr ";
        }
        else if (ddlSortBy.SelectedValue == "SV Hours")
        {
            SQLStatement = SQLStatement + "vo.svHours ";
        }
        else if (ddlSortBy.SelectedValue == "Regular Volunteer")
        {
            SQLStatement = SQLStatement + "vo.regular ";
        }

        if (ddlAscDesc.SelectedValue == "Ascending")
        {
            SQLStatement = SQLStatement + "ASC";
        }
        else if (ddlAscDesc.SelectedValue == "Descending")
        {
            SQLStatement = SQLStatement + "DESC";
        }

        //lblError.Text = SQLStatement;
        //return;

        // Load data from volunteers table
        try
        {
            con.Close();
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = SQLStatement;
            cmd.Connection = con;

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            var dt = new DataTable();
            da.Fill(dt);
            dgvVolunteers.DataSource = dt;
            dgvVolunteers.DataBind();

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            lblError.Text = "Error in LoadData(). Could not load volunteer data into table.";
            return;
        }

        // Highlight row being edited
        foreach (GridViewRow row in dgvVolunteers.Rows)
        {
            if (row.RowIndex == dgvVolunteers.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                // row.BorderColor = ColorTranslator.FromHtml("#ffffff")
                row.BorderWidth = 2;
            }
        }

    }

    // Loads a ddl with all volunteer names, first and last, with their ID in parentheses and returns it
    public object LoadVolunteerNameDDL(DropDownList VolNamesDDL, string SchoolID = null)
    {
        string SQLStatement = "SELECT CONCAT(lastName, ', ', firstName, ' (', id, ')') as volName FROM volunteersFP";

        // Clear volunteer names ddl
        VolNamesDDL.Items.Clear();

        // Check if schoolID is not null
        if (!string.IsNullOrEmpty(SchoolID))
        {
            SQLStatement = SQLStatement + " WHERE schoolID='" + SchoolID + "'";
        }

        // Add ORDER BY clause
        SQLStatement = SQLStatement + " ORDER BY lastName ASC";

        // Populates a DDL with student names and their account numbers at the beginning of the name
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = SQLStatement;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            VolNamesDDL.Items.Add(dr[0].ToString());
        VolNamesDDL.Items.Insert(0, "");

        cmd.Dispose();
        con.Close();

        return VolNamesDDL.Items;
    }

    // Gets an updated list of ddlRegular volunteers and assigns them by first name to a passed through ddl
    public object LoadRegVolDDL(DropDownList DDL)
    {
        // Get all first names of ddlRegular volunteers from volunteers table and load them into the check in DDLs
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT firstName FROM volunteersFP WHERE regular=1 ORDER BY firstName ASC";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                DDL.Items.Add(dr[0].ToString());
            DDL.Items.Insert(0, "None");

            cmd.Dispose();
            con.Close();

            return DDL.Items;
        }

        catch
        {
            lblError.Text = "Error in LoadRegVolDDL(). Could not load ddlRegular volunteer DDLs.";
            return null;
        }
    }

    // Load data from volunteers Check in table and checks off the checkboxes based on amount of volunteers assigned to that business. Also loads the selected regular volunteer into the DDL for the business
    public void LoadVolCheckIn(string VisitDate)
    {
        int VIDOfDate = int.Parse(VisitData.GetVisitIDFromDate(VisitDate).ToString());
        string CheckInSQL = "SELECT * FROM volunteersCheckInFP WHERE visitID = '" + VIDOfDate + "'";
        string Vol1 = "",Reg1 = "",Vol2 = "",Reg2 = "",Vol3 = "",Reg3 = "",Vol4 = "",Reg4 = "",Vol5 = "",Reg5 = "",Vol6 = "",Reg6 = "",Vol7 = "",Reg7 = "",Vol8 = "",Reg8 = "",Vol9 = "",Reg9 = "",Vol10 = "",Reg10 = "",Vol11 = "",Reg11 = "",Vol12 = "",Reg12 = "",Vol13 = "",Reg13 = "",Vol14 = "",Reg14 = "",Vol15 = "",Reg15 = "",Vol16 = "",Reg16 = "",Vol17 = "",Reg17 = "",Vol18 = "",Reg18 = "",Vol19 = "",Reg19 = "",Vol20 = "",Reg20 = "",Vol21 = "",Reg21 = "",Vol22 = "",Reg22 = "",Vol23 = "",Reg23 = "",Vol24 = "",Reg24 = "",Vol25 = "",Reg25 = "",Vol26 = "",Reg26 = "",Vol27 = "",Reg27 = "",Vol28 = "",Reg28 = "",Vol29 = "",Reg29 = "",Vol30 = "",Reg30 = "",Vol31 = "",Reg31 = "",Vol32 = "",Reg32 = "";

        // Clear checkboxes
        foreach (Control cBox in divCheckIn.Controls)
        {
            if (cBox is CheckBox)
            {
                ((CheckBox)cBox).Checked = false;
            }
        }

        // Reset DDLs
        ddlRegVolS1.SelectedIndex = 0;
        ddlRegVolS2.SelectedIndex = 0;
        ddlRegVolS3.SelectedIndex = 0;
        ddlRegVolS4.SelectedIndex = 0;
        ddlRegVolS5.SelectedIndex = 0;
        ddlRegVolS6.SelectedIndex = 0;
        ddlRegVolS7.SelectedIndex = 0;
        ddlRegVolS8.SelectedIndex = 0;
        ddlRegVolS9.SelectedIndex = 0;
        ddlRegVolS10.SelectedIndex = 0;
        ddlRegVolS11.SelectedIndex = 0;
        ddlRegVolS12.SelectedIndex = 0;
        ddlRegVolS13.SelectedIndex = 0;
        ddlRegVolS14.SelectedIndex = 0;
        ddlRegVolS15.SelectedIndex = 0;
        ddlRegVolS16.SelectedIndex = 0;
        ddlRegVolS17.SelectedIndex = 0;
        ddlRegVolS18.SelectedIndex = 0;
        ddlRegVolS19.SelectedIndex = 0;
        ddlRegVolS20.SelectedIndex = 0;
        ddlRegVolS21.SelectedIndex = 0;
        ddlRegVolS22.SelectedIndex = 0;
        ddlRegVolS23.SelectedIndex = 0;
        ddlRegVolS24.SelectedIndex = 0;
        ddlRegVolS25.SelectedIndex = 0;
        ddlRegVolS26.SelectedIndex = 0;
        ddlRegVolS27.SelectedIndex = 0;
        ddlRegVolS28.SelectedIndex = 0;
        ddlRegVolS29.SelectedIndex = 0;
        ddlRegVolS30.SelectedIndex = 0;
        ddlRegVolS31.SelectedIndex = 0;
        ddlRegVolS32.SelectedIndex = 0;

        // Load data from volunteers Check in table and assign it to the variables and DDLs
        //try
        //{
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = CheckInSQL;
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Vol1 = dr["vol1"].ToString();
                    ddlRegVolS1.SelectedValue = dr["reg1"].ToString();
                    Vol2 = dr["vol2"].ToString();
                    ddlRegVolS2.SelectedValue = dr["reg2"].ToString();
                    Vol3 = dr["vol3"].ToString();
                    ddlRegVolS3.SelectedValue = dr["reg3"].ToString();
                    Vol4 = dr["vol4"].ToString();
                    ddlRegVolS4.SelectedValue = dr["reg4"].ToString();
                    Vol5 = dr["vol5"].ToString();
                    ddlRegVolS5.SelectedValue = dr["reg5"].ToString();
                    Vol6 = dr["vol6"].ToString();
                    ddlRegVolS6.SelectedValue = dr["reg6"].ToString();
                    Vol7 = dr["vol7"].ToString();
                    ddlRegVolS7.SelectedValue = dr["reg7"].ToString();
                    Vol8 = dr["vol8"].ToString();
                    ddlRegVolS8.SelectedValue = dr["reg8"].ToString();
                    Vol9 = dr["vol9"].ToString();
                    ddlRegVolS9.SelectedValue = dr["reg9"].ToString();
                    Vol10 = dr["vol10"].ToString();
                    ddlRegVolS10.SelectedValue = dr["reg10"].ToString();
                    Vol11 = dr["vol11"].ToString();
                    ddlRegVolS11.SelectedValue = dr["reg11"].ToString();
                    Vol12 = dr["vol12"].ToString();
                    ddlRegVolS12.SelectedValue = dr["reg12"].ToString();
                    Vol13 = dr["vol13"].ToString();
                    ddlRegVolS13.SelectedValue = dr["reg13"].ToString();
                    Vol14 = dr["vol14"].ToString();
                    ddlRegVolS14.SelectedValue = dr["reg14"].ToString();
                    Vol15 = dr["vol15"].ToString();
                    ddlRegVolS15.SelectedValue = dr["reg15"].ToString();
                    Vol16 = dr["vol16"].ToString();
                    ddlRegVolS16.SelectedValue = dr["reg16"].ToString();
                    Vol17 = dr["vol17"].ToString();
                    ddlRegVolS17.SelectedValue = dr["reg17"].ToString();
                    Vol18 = dr["vol18"].ToString();
                    ddlRegVolS18.SelectedValue = dr["reg18"].ToString();
                    Vol19 = dr["vol19"].ToString();
                    ddlRegVolS19.SelectedValue = dr["reg19"].ToString();
                    Vol20 = dr["vol20"].ToString();
                    ddlRegVolS20.SelectedValue = dr["reg20"].ToString();
                    Vol21 = dr["vol21"].ToString();
                    ddlRegVolS21.SelectedValue = dr["reg21"].ToString();
                    Vol22 = dr["vol22"].ToString();
                    ddlRegVolS22.SelectedValue = dr["reg22"].ToString();
                    Vol23 = dr["vol23"].ToString();
                    ddlRegVolS23.SelectedValue = dr["reg23"].ToString();
                    Vol24 = dr["vol24"].ToString();
                    ddlRegVolS24.SelectedValue = dr["reg24"].ToString();
                    Vol25 = dr["vol25"].ToString();
                    ddlRegVolS25.SelectedValue = dr["reg25"].ToString();
                    Vol26 = dr["vol26"].ToString();
                    ddlRegVolS26.SelectedValue = dr["reg26"].ToString();
                    Vol27 = dr["vol27"].ToString();
                    ddlRegVolS27.SelectedValue = dr["reg27"].ToString();
                    Vol28 = dr["vol28"].ToString();
                    ddlRegVolS28.SelectedValue = dr["reg28"].ToString();
                    Vol29 = dr["vol29"].ToString();
                    ddlRegVolS29.SelectedValue = dr["reg29"].ToString();
                    Vol30 = dr["vol30"].ToString();
                    ddlRegVolS30.SelectedValue = dr["reg30"].ToString();
                    Vol31 = dr["vol31"].ToString();
                    ddlRegVolS31.SelectedValue = dr["reg31"].ToString();
                    Vol32 = dr["vol32"].ToString();
                    ddlRegVolS32.SelectedValue = dr["reg32"].ToString();
                }

                // Change submit button to update
                btnSubmitCheckIn.Text = "Update";
            }

            else
            {
                // Change submit button to update
                btnSubmitCheckIn.Text = "Submit";
            }

            cmd.Dispose();
            con.Close();
        //}
        //catch
        //{
        //    lblError.Text = "Error in LoadVolCheckin(). Could not load volunteer check in information.";
        //    return;
        //}

        cmd.Dispose();
        con.Close();

        // Assign values to checkboxes
        switch (Vol1)
        {
            case "": 
                {
                    break;
                }
            case "1":
                {
                    chkS1N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS1N1.Checked = true;
                    chkS1N2.Checked = true;
                    break;
                }     
        }

        switch (Vol2)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS2N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS2N1.Checked = true;
                    chkS2N2.Checked = true;
                    break;
                }
        }

        switch (Vol3)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS3N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS3N1.Checked = true;
                    chkS3N2.Checked = true;
                    break;
                }
        }

        switch (Vol4)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS4N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS4N1.Checked = true;
                    chkS4N2.Checked = true;
                    break;
                }
        }

        switch (Vol5)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS5N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS5N1.Checked = true;
                    chkS5N2.Checked = true;
                    break;
                }
        }

        switch (Vol6)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS6N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS6N1.Checked = true;
                    chkS6N2.Checked = true;
                    break;
                }
        }

        switch (Vol7)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS7N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS7N1.Checked = true;
                    chkS7N2.Checked = true;
                    break;
                }
        }

        switch (Vol8)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS8N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS8N1.Checked = true;
                    chkS8N2.Checked = true;
                    break;
                }
        }

        switch (Vol9)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS9N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS9N1.Checked = true;
                    chkS9N2.Checked = true;
                    break;
                }
        }

        switch (Vol10)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS10N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS10N1.Checked = true;
                    chkS10N2.Checked = true;
                    break;
                }
        }

        switch (Vol11)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS11N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS11N1.Checked = true;
                    chkS11N2.Checked = true;
                    break;
                }
        }

        switch (Vol12)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS12N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS12N1.Checked = true;
                    chkS12N2.Checked = true;
                    break;
                }
        }

        switch (Vol13)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS13N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS13N1.Checked = true;
                    chkS13N2.Checked = true;
                    break;
                }
        }

        switch (Vol14)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS14N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS14N1.Checked = true;
                    chkS14N2.Checked = true;
                    break;
                }
        }

        switch (Vol15)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS15N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS15N1.Checked = true;
                    chkS15N2.Checked = true;
                    break;
                }
        }

        switch (Vol16)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS16N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS16N1.Checked = true;
                    chkS16N2.Checked = true;
                    break;
                }
        }

        switch (Vol17)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS17N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS17N1.Checked = true;
                    chkS17N2.Checked = true;
                    break;
                }
        }

        switch (Vol18)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS18N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS18N1.Checked = true;
                    chkS18N2.Checked = true;
                    break;
                }
        }

        switch (Vol19)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS19N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS19N1.Checked = true;
                    chkS19N2.Checked = true;
                    break;
                }
        }

        switch (Vol20)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS20N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS20N1.Checked = true;
                    chkS20N2.Checked = true;
                    break;
                }
        }

        switch (Vol21)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS21N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS21N1.Checked = true;
                    chkS21N2.Checked = true;
                    break;
                }
        }

        switch (Vol22)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS22N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS22N1.Checked = true;
                    chkS22N2.Checked = true;
                    break;
                }
        }

        switch (Vol23)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS23N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS23N1.Checked = true;
                    chkS23N2.Checked = true;
                    break;
                }
        }

        switch (Vol24)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS24N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS24N1.Checked = true;
                    chkS24N2.Checked = true;
                    break;
                }
        }

        switch (Vol25)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS25N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS25N1.Checked = true;
                    chkS25N2.Checked = true;
                    break;
                }
        }

        switch (Vol26)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS26N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS26N1.Checked = true;
                    chkS26N2.Checked = true;
                    break;
                }
        }

        switch (Vol27)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS27N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS27N1.Checked = true;
                    chkS27N2.Checked = true;
                    break;
                }
        }

        switch (Vol28)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS28N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS28N1.Checked = true;
                    chkS28N2.Checked = true;
                    break;
                }
        }

        switch (Vol29)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS29N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS29N1.Checked = true;
                    chkS29N2.Checked = true;
                    break;
                }
        }

        switch (Vol30)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS30N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS30N1.Checked = true;
                    chkS30N2.Checked = true;
                    break;
                }
        }

        switch (Vol31)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS31N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS31N1.Checked = true;
                    chkS31N2.Checked = true;
                    break;
                }
        }

        switch (Vol32)
        {
            case "":
                {
                    break;
                }
            case "1":
                {
                    chkS32N1.Checked = true;
                    break;
                }
            case "2":
                {
                    chkS32N1.Checked = true;
                    chkS32N2.Checked = true;
                    break;
                }
        }   

    }

    // Loads the seperate viewVolCtrl gridview used for view volunteers from a specific visit date or viewing volunteer's muiltiple visits
    public void LoadViewVolCtrlGridview()
    {
        int VIDOfDate;
        string VisitDate;
        var TotalHours = "";
        string[] VolNameWithID;
        string VolName;
        string[] VolIDWithParentheses;
        string VolID;
        string SQLStatement = @"SELECT vo.id, vo.id as volunteerID, vo.firstName, vo.lastName, FORMAT(v.visitDate, 'MM/dd/yyyy') as visitDate, s.schoolName, vo.pr, vo.svHours, vo.notes, vo.regular
										FROM volunteersFP vo
										LEFT JOIN volunteersScheduleFP vs ON vo.id = vs.volunteerID
										LEFT JOIN visitInfoFp v ON v.id = vs.visitID
										LEFT JOIN schoolInfoFP s ON s.id = vo.schoolID";

        // Clear error and reset all fields
        lblError.Text = "";
        lblTotalSVHours.Text = "-";
        divViewVol.Visible = false;
        divViewVolControls.Visible = false;
        ddlSchoolNameCheckin.Visible = false;

        // Check if school name or visit date is loaded
        if (tbVisitDateViewVolCtrl.Text != "")
        {

            // Assign visit date
            VisitDate = tbVisitDateViewVolCtrl.Text;

            // Get VID of date
            VIDOfDate = int.Parse(VisitData.GetVisitIDFromDate(VisitDate).ToString());

            // Update SQL statement
            SQLStatement = SQLStatement + " WHERE vs.visitID='" + VIDOfDate + "'";

            // Assign total hours
            TotalHours = TotalSVHours(VisitDate).ToString();
        }

        else if (ddlVolNameViewVolCtrl.SelectedIndex != 0)
        {
            VolNameWithID = ddlVolNameViewVolCtrl.SelectedValue.Split('(');
            VolName = VolNameWithID[0];
            VolIDWithParentheses = VolNameWithID[1].Split(')');
            VolID = VolIDWithParentheses[0];

            // Update SQL statement
            SQLStatement = SQLStatement + " WHERE vo.id='" + VolID + "'";

        }

        // Load view control gridview
        try
        {
            con.Close();
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = SQLStatement;
            cmd.Connection = con;

            var da = new SqlDataAdapter();
            da.SelectCommand = cmd;
            var dt = new DataTable();
            da.Fill(dt);
            dgvViewVolCtrl.DataSource = dt;
            dgvViewVolCtrl.DataBind();

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            lblError.Text = "Error in LoadViewVolCtrl(). Could not load volunteer data into table.";
            return;
        }

        // assign total SV hours to label
        lblTotalSVHoursCtrl.Text = TotalHours;

        // Make view vol ctrl div visible
        divViewVolCtrlDGV.Visible = true;
        divViewVolControls.Visible = true;

    }

    //Gets updated sponsor names and loads them into the labels on the check in screen
    public void LoadSponsorNames()
    {
        lblS1.Text = Sponsors.GetSponsorName(1);
        lblS2.Text = Sponsors.GetSponsorName(2);
        lblS3.Text = Sponsors.GetSponsorName(3);
        lblS4.Text = Sponsors.GetSponsorName(4);
        lblS5.Text = Sponsors.GetSponsorName(5);
        lblS6.Text = Sponsors.GetSponsorName(6);
        lblS7.Text = Sponsors.GetSponsorName(7);
        lblS8.Text = Sponsors.GetSponsorName(8);
        lblS9.Text = Sponsors.GetSponsorName(9);
        lblS10.Text = Sponsors.GetSponsorName(10);
        lblS11.Text = Sponsors.GetSponsorName(11);
        lblS12.Text = Sponsors.GetSponsorName(12);
        lblS13.Text = Sponsors.GetSponsorName(13);
        lblS14.Text = Sponsors.GetSponsorName(14);
        lblS15.Text = Sponsors.GetSponsorName(15);
        lblS16.Text = Sponsors.GetSponsorName(16);
        lblS17.Text = Sponsors.GetSponsorName(17);
        lblS18.Text = Sponsors.GetSponsorName(18);
        lblS19.Text = Sponsors.GetSponsorName(19);
        lblS20.Text = Sponsors.GetSponsorName(20);
        lblS21.Text = Sponsors.GetSponsorName(21);
        lblS22.Text = Sponsors.GetSponsorName(22);
        lblS23.Text = Sponsors.GetSponsorName(23);
        lblS24.Text = Sponsors.GetSponsorName(24);
        lblS25.Text = Sponsors.GetSponsorName(25);
        lblS26.Text = Sponsors.GetSponsorName(26);
        lblS27.Text = Sponsors.GetSponsorName(27);
        lblS28.Text = Sponsors.GetSponsorName(28);
        lblS29.Text = Sponsors.GetSponsorName(29);
        lblS30.Text = Sponsors.GetSponsorName(30);
        lblS31.Text = Sponsors.GetSponsorName(31);
        lblS32.Text = Sponsors.GetSponsorName(32);
    }

    // Uses the LoadRegVolDDL function to populate all ddlRegular volunteer DDLs found in the check in section
    public void PopulateVolDDL()
    {
        LoadRegVolDDL(ddlRegVolS1);
        LoadRegVolDDL(ddlRegVolS2);
        LoadRegVolDDL(ddlRegVolS3);
        LoadRegVolDDL(ddlRegVolS4);
        LoadRegVolDDL(ddlRegVolS5);
        LoadRegVolDDL(ddlRegVolS6);
        LoadRegVolDDL(ddlRegVolS7);
        LoadRegVolDDL(ddlRegVolS8);
        LoadRegVolDDL(ddlRegVolS9);
        LoadRegVolDDL(ddlRegVolS10);
        LoadRegVolDDL(ddlRegVolS11);
        LoadRegVolDDL(ddlRegVolS12);
        LoadRegVolDDL(ddlRegVolS13);
        LoadRegVolDDL(ddlRegVolS14);
        LoadRegVolDDL(ddlRegVolS15);
        LoadRegVolDDL(ddlRegVolS16);
        LoadRegVolDDL(ddlRegVolS17);
        LoadRegVolDDL(ddlRegVolS18);
        LoadRegVolDDL(ddlRegVolS19);
        LoadRegVolDDL(ddlRegVolS20);
        LoadRegVolDDL(ddlRegVolS21);
        LoadRegVolDDL(ddlRegVolS22);
        LoadRegVolDDL(ddlRegVolS23);
        LoadRegVolDDL(ddlRegVolS24);
        LoadRegVolDDL(ddlRegVolS25);
        LoadRegVolDDL(ddlRegVolS26);
        LoadRegVolDDL(ddlRegVolS27);
        LoadRegVolDDL(ddlRegVolS28);
        LoadRegVolDDL(ddlRegVolS29);
        LoadRegVolDDL(ddlRegVolS30);
        LoadRegVolDDL(ddlRegVolS31);
        LoadRegVolDDL(ddlRegVolS32);
    }

    // Gets the total amount of SV hours of a visit, school, or volunteer (currently volunteer isn't working - 8/29/2024)
    public object TotalSVHours(string VisitDate = null, string SchoolName = null, string VolID = null)
    {
        string SQLStatement = "SELECT SUM(svHours) as svHours FROM volunteersFP WHERE ";
        int VIDOfDate = int.Parse(VisitData.GetVisitIDFromDate(VisitDate).ToString());
        int SchoolID = SchoolData.GetSchoolID(SchoolName);
        string Total = "";

        // Check if load by visit date or load by school name is active
        if (!string.IsNullOrEmpty(VisitDate))
        {
            SQLStatement += "visitID= '" + VIDOfDate + "' ";

            if (!string.IsNullOrEmpty(SchoolName))
            {
                SQLStatement += " AND schoolID= '" + SchoolID + "' ";
            }
            else if (!string.IsNullOrEmpty(VolID))
            {
                SQLStatement += " AND id= '" + VolID + "' ";
            }
        }
        else if (!string.IsNullOrEmpty(SchoolName))
        {
            SQLStatement += "schoolID = '" + SchoolID + "' ";

            if (!string.IsNullOrEmpty(VisitDate))
            {
                SQLStatement += " AND visitID = '" + VIDOfDate + "' ";
            }
            else if (!string.IsNullOrEmpty(VolID))
            {
                SQLStatement += " AND id='" + VolID + "'";
            }
        }
        else if (!string.IsNullOrEmpty(VolID))
        {
            SQLStatement += "id = '" + VolID + "' ";

            if (VisitID != 0)
            {
                SQLStatement += " AND visitID = '" + VIDOfDate + "' ";
            }
            else if (SchoolID != 0)
            {
                SQLStatement += " AND schoolID = '" + SchoolID + "' ";
            }
        }

        // Get total hours and apply it to label
        try
        {
            con.Close();
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = SQLStatement;
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                Total = dr["svHours"].ToString();
        }

        catch
        {
            lblError.Text = "Error in TotalSVHours(). Could not get total sv hours.";
            return "";
        }

        cmd.Dispose();
        con.Close();

        return Total;
    }

    // Enables the ddlRegular vol ddls and assigns the colors for the checkboxes based on what business is open for the entered visit date in the check in section
    public void Checkboxes(int VIDOfDate)
    {
        int count = 1;
        string SQLStatement = "";
        // Dim VIDOfDate As Integer = VisitData.GetVisitIDFromDate(tbVisitDateCheckin.Text)

        // Clear checkboxes
        foreach (Control cBox in divCheckIn.Controls)
        {
            if (cBox is CheckBox)
            {
                ((CheckBox)cBox).Checked = false;
            }
        }

        // Reset DDLs
        ddlRegVolS1.SelectedIndex = 0;
        ddlRegVolS2.SelectedIndex = 0;
        ddlRegVolS3.SelectedIndex = 0;
        ddlRegVolS4.SelectedIndex = 0;
        ddlRegVolS5.SelectedIndex = 0;
        ddlRegVolS6.SelectedIndex = 0;
        ddlRegVolS7.SelectedIndex = 0;
        ddlRegVolS8.SelectedIndex = 0;
        ddlRegVolS9.SelectedIndex = 0;
        ddlRegVolS10.SelectedIndex = 0;
        ddlRegVolS11.SelectedIndex = 0;
        ddlRegVolS12.SelectedIndex = 0;
        ddlRegVolS13.SelectedIndex = 0;
        ddlRegVolS14.SelectedIndex = 0;
        ddlRegVolS15.SelectedIndex = 0;
        ddlRegVolS16.SelectedIndex = 0;
        ddlRegVolS17.SelectedIndex = 0;
        ddlRegVolS18.SelectedIndex = 0;
        ddlRegVolS19.SelectedIndex = 0;
        ddlRegVolS20.SelectedIndex = 0;
        ddlRegVolS21.SelectedIndex = 0;
        ddlRegVolS22.SelectedIndex = 0;
        ddlRegVolS23.SelectedIndex = 0;
        ddlRegVolS24.SelectedIndex = 0;
        ddlRegVolS25.SelectedIndex = 0;
        ddlRegVolS26.SelectedIndex = 0;
        ddlRegVolS27.SelectedIndex = 0;
        ddlRegVolS28.SelectedIndex = 0;
        ddlRegVolS29.SelectedIndex = 0;
        ddlRegVolS30.SelectedIndex = 0;
        ddlRegVolS31.SelectedIndex = 0;
        ddlRegVolS32.SelectedIndex = 0;

        // Get opened and closed businesses for selected visit date and enable checkboxes for opened businesses
        while (count < 33)
        {
                //Update SQLstatment, assign count to the open column name
                SQLStatement = "SELECT sp.id, sp.sponsorName, o.businessID, s.schoolName, o.openStatus FROM openStatusFP o JOIN schoolInfoFP s ON s.id = o.schoolID LEFT JOIN sponsorsFP sp ON sp.businessID = o.businessID OR sp.businessID2 = o.businessID OR sp.businessID3 = o.businessID OR sp.businessID4 = o.businessID WHERE o.visitID = '" + VIDOfDate + "' AND o.openStatus = '1' AND o.businessID = '" + count + "'";

                //try
                //{
                    con.ConnectionString = ConnectionString;
                    con.Open();
                    cmd.CommandText = SQLStatement;
                    cmd.Connection = con;
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        // Check for rows
                        if (dr.HasRows == true)
                        {
                            // If sponsor ID equals the count number then check the boxes
                            if (dr["businessID"].ToString() == count.ToString())
                            {
                                // Set color of text based on school assigned and enable vol reg ddl
                                switch (count.ToString())
                                {
                                    case "1":
                                        {
                                            ddlRegVolS1.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS1.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS1.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS1.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS1.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS1.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "2":
                                    {
                                        ddlRegVolS2.Enabled = true;

                                        // Set color of text based on school assigned
                                        if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                        {
                                            lblS2.Attributes.CssStyle.Add("color", "blue");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                        {
                                            lblS2.Attributes.CssStyle.Add("color", "red");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                        {
                                            lblS2.Attributes.CssStyle.Add("color", "green");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                        {
                                            lblS2.Attributes.CssStyle.Add("color", "magenta");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                        {
                                            lblS2.Attributes.CssStyle.Add("color", "orange");
                                        }

                                        break;
                                    }

                                    case "3":
                                    {
                                        ddlRegVolS3.Enabled = true;

                                        // Set color of text based on school assigned
                                        if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                        {
                                            lblS3.Attributes.CssStyle.Add("color", "blue");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                        {
                                            lblS3.Attributes.CssStyle.Add("color", "red");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                        {
                                            lblS3.Attributes.CssStyle.Add("color", "green");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                        {
                                            lblS3.Attributes.CssStyle.Add("color", "magenta");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                        {
                                            lblS3.Attributes.CssStyle.Add("color", "orange");
                                        }

                                        break;
                                    }

                                    case "4":
                                    {
                                        ddlRegVolS4.Enabled = true;

                                        // Set color of text based on school assigned
                                        if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                        {
                                            lblS4.Attributes.CssStyle.Add("color", "blue");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                        {
                                            lblS4.Attributes.CssStyle.Add("color", "red");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                        {
                                            lblS4.Attributes.CssStyle.Add("color", "green");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                        {
                                            lblS4.Attributes.CssStyle.Add("color", "magenta");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                        {
                                            lblS4.Attributes.CssStyle.Add("color", "orange");
                                        }

                                        break;
                                    }

                                    case "5":
                                    {
                                        ddlRegVolS5.Enabled = true;

                                        // Set color of text based on school assigned
                                        if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                        {
                                            lblS5.Attributes.CssStyle.Add("color", "blue");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                        {
                                            lblS5.Attributes.CssStyle.Add("color", "red");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                        {
                                            lblS5.Attributes.CssStyle.Add("color", "green");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                        {
                                            lblS5.Attributes.CssStyle.Add("color", "magenta");
                                        }
                                        else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                        {
                                            lblS5.Attributes.CssStyle.Add("color", "orange");
                                        }

                                        break;
                                    }

                                    case "6":
                                        {
                                            ddlRegVolS6.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS6.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS6.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS6.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS6.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS6.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "7":
                                        {
                                            ddlRegVolS7.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS7.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS7.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS7.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS7.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS7.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "8":
                                        {
                                            ddlRegVolS8.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS8.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS8.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS8.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS8.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS8.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "9":
                                        {
                                            ddlRegVolS9.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS9.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS9.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS9.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS9.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS9.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "10":
                                        {
                                            ddlRegVolS10.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS10.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS10.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS10.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS10.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS10.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "11":
                                        {
                                            ddlRegVolS11.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS11.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS11.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS11.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS11.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS11.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "12":
                                        {
                                            ddlRegVolS12.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS12.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS12.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS12.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS12.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS12.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "13":
                                        {
                                            ddlRegVolS13.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS13.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS13.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS13.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS13.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS13.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "14":
                                        {
                                            ddlRegVolS14.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS14.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS14.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS14.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS14.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS14.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "15":
                                        {
                                            ddlRegVolS15.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS15.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS15.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS15.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS15.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS15.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "16":
                                        {
                                            ddlRegVolS16.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS16.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS16.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS16.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS16.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS16.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "17":
                                        {
                                            ddlRegVolS17.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS17.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS17.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS17.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS17.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS17.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "18":
                                        {
                                            ddlRegVolS18.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS18.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS18.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS18.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS18.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS18.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "19":
                                        {
                                            ddlRegVolS19.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS19.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS19.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS19.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS19.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS19.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "20":
                                        {
                                            ddlRegVolS20.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS20.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS20.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS20.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS20.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS20.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "21":
                                        {
                                            ddlRegVolS21.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS21.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS21.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS21.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS21.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS21.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "22":
                                        {
                                            ddlRegVolS22.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS22.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS22.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS22.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS22.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS22.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "23":
                                        {
                                            ddlRegVolS23.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS23.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS23.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS23.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS23.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS23.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "24":
                                        {
                                            ddlRegVolS24.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS24.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS24.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS24.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS24.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS24.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "25":
                                        {
                                            ddlRegVolS25.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS25.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS25.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS25.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS25.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS25.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "26":
                                        {
                                            ddlRegVolS26.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS26.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS26.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS26.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS26.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS26.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "27":
                                        {
                                            ddlRegVolS27.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS27.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS27.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS27.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS27.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS27.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "28":
                                        {
                                            ddlRegVolS28.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS28.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS28.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS28.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS28.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS28.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "29":
                                        {
                                            ddlRegVolS29.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS29.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS29.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS29.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS29.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS29.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "30":
                                        {
                                            ddlRegVolS30.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS30.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS30.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS30.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS30.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS30.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "31":
                                        {
                                            ddlRegVolS31.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS31.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS31.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS31.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS31.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS31.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }

                                    case "32":
                                        {
                                            ddlRegVolS32.Enabled = true;

                                            // Set color of text based on school assigned
                                            if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[1].Value)
                                            {
                                                lblS32.Attributes.CssStyle.Add("color", "blue");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[2].Value)
                                            {
                                                lblS32.Attributes.CssStyle.Add("color", "red");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[3].Value)
                                            {
                                                lblS32.Attributes.CssStyle.Add("color", "green");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[4].Value)
                                            {
                                                lblS32.Attributes.CssStyle.Add("color", "magenta");
                                            }
                                            else if (dr["schoolName"].ToString() == ddlSchoolNameCheckin.Items[5].Value)
                                            {
                                                lblS32.Attributes.CssStyle.Add("color", "orange");
                                            }

                                            break;
                                        }
                                }

                            }
                        }

                        //Business has not been opened yet
                        else
                        {
                            break;
                        }

                    }

                    cmd.Dispose();
                    con.Close();
                //}
                //catch
                //{
                //    lblError.Text = "Error in Checkboxes(). Could not get business open information.";
                //    cmd.Dispose();
                //    con.Close();
                //    return;
                //}

                cmd.Dispose();
                con.Close();

                // Increase count number
                count = count + 1;
            
        }

        // LoadData()

    }

    // Returns a true or false value for if a passed through volunteer ID is already in the volunteersSchedule table in the database assigned to the passed through visit ID
    public bool CheckVolunteerSchedule(int VolunteerID, int VisitID)
    {
        bool Check = false;

        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT id FROM volunteersScheduleFP WHERE volunteerID='" + VolunteerID + "' AND visitID='" + VisitID + "'";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            if (dr.HasRows == true)
            {
                Check = true;
            }
            else
            {
                Check = false;
            }

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            lblError.Text = "Error in LoadRegVolDDL(). Could not load regular volunteer DDLs.";
            return Check;
        }

        return Check;
    }

    // Adds a new volunteer into the volunteers table in the database
    public void SubmitAddNewVolunteer()
    {
        string FirstName;
        string LastName;
        string SchoolName;
        int SchoolID;
        string PR;
        string SVHours = "6";
        bool Regular = false;
        string Notes;

        // Check for empty fields    Or businessName_ddl.SelectedIndex = 0 Or regularVol_ddl.SelectedIndex = 0 
        if (tbFirstName.Text == "" | tbLastName.Text == "")
        {
            lblError.Text = "Please enter a first name, last name, business, visit date, school name and regular volunteer status before submitting.";
            return;
        }

        if (ddlSchoolNameAdd.SelectedIndex == 0)
        {
            lblError.Text = "Please select a school name before submitting.";
            return;
        }

        // Assign fields to variables
        SchoolName = ddlSchoolNameAdd.SelectedValue;
        SchoolID = SchoolData.GetSchoolID(SchoolName);
        FirstName = tbFirstName.Text;
        LastName = tbLastName.Text;
        PR = ddlPR.SelectedValue;
        Notes = tbNotes.Text;

        //lblError.Text = SchoolID.ToString();
        //return;

        // Check if volunteer first and last name are already apart of the selected school
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "SELECT firstName, lastName, schoolID FROM volunteersFP WHERE firstName LIKE '%" + FirstName + "%' AND lastName LIKE '%" + LastName + "%' AND schoolID='" + SchoolID + "'";
            dr = cmd.ExecuteReader();

            if (dr.HasRows)
            {
                lblError.Text = "A volunteer by that name has already been added to the database. To schedule them, click on the Schedule Volunteers button and find their name in the drop down list.";
                return;
            }

            cmd.Dispose();
            con.Close();
        }
        catch
        {

        }

        // Insert data into DB
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = @"INSERT INTO volunteersFP (schoolID, sponsorID, firstName, lastName, pr, svHours, notes, regular)
                                VALUES ('" + SchoolID + "', '0','" + FirstName + "', '" + LastName + "', '" + PR + "', '" + SVHours + "', '" + Notes + "', '" + Regular + "')";

            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
        catch
        {
            lblError.Text = "Error in SubmitVolunteer(). Could not add the volunteer.";
            return;
        }

        // Clear out text boxes and set cursor to first name
        tbFirstName.Text = "";
        tbLastName.Text = "";
        tbFirstName.Focus();
     
        // Load table
        LoadData();

        // Show success message
        lblError.Text = "Volunteer successfully added.";

    }

    // Adds all volunteer IDs from the schedule volunteers table, to the volunteersSchedule table, found in the database. Used for loading in the volunteers in the check in section
    public void SubmitScheduleVolunteers()
    {
        string VisitDate;
        string VisitID;
        int CountOfVolunteers;
        int VolunteerID;
        string SQLInsertQuery = "INSERT INTO volunteersScheduleFP (volunteerID, visitID) VALUES";
        string SQLRemoveBusiness = "UPDATE volunteersFP SET sponsorID=0 WHERE ";

        // Check if the visit date is not blank
        if (tbVisitDateSchedule.Text != "")
        {
            VisitDate = tbVisitDateSchedule.Text;
        }
        else
        {
            lblError.Text = "Please enter a visit date before submitting.";
            return;
        }

        // Get visit ID (if found)
        VisitID = VisitData.GetVisitIDFromDate(VisitDate).ToString();

        if (VisitID == "0" | string.IsNullOrEmpty(VisitID))
        {
            lblError.Text = "Visit date entered has not been created. Please check with an EV teacher to confirm that the visit date you have entered has been created.";
            return;
        }

        // Get the total number of volunteers in the table
        CountOfVolunteers = dgvScheduledVol.Rows.Count;

        // Start a for loop to add volunteer IDs and visit IDs to the insert query
        for (int i = 0; i < CountOfVolunteers; i++)
        {

            // Assign volunteer ID
            VolunteerID = int.Parse(dgvScheduledVol.Rows[i].Cells[0].Text);

            // Check if volunteer ID is already assigned to visitID
            if (CheckVolunteerSchedule(VolunteerID, int.Parse(VisitID)) == true)
            {
                //Go back to start of the loop
                continue;
            }

            // If adding data for the first row, do not add a comma to the beginning of the query
            if (i == 0 || !SQLInsertQuery.Contains("VALUES ("))
            {
                SQLInsertQuery = SQLInsertQuery + " (" + VolunteerID + ", " + VisitID + ")";
            }
            else
            {
                SQLInsertQuery = SQLInsertQuery + ", (" + VolunteerID + ", " + VisitID + ")";
            }

            // Remove business ID from exsiting volunteers (if they are being added into the insert 
            if (i == 0 || !SQLRemoveBusiness.Contains("id="))
            {
                SQLRemoveBusiness = SQLRemoveBusiness + " id=" + VolunteerID + " ";
            }
            else
            {
                SQLRemoveBusiness = SQLRemoveBusiness + " AND id=" + VolunteerID + "";
            }
        }

        //lblError.Text = SQLRemoveBusiness + CountOfVolunteers;
        //return;

        //Check if there are any values for insertion
        if (!SQLInsertQuery.Contains("VALUES ("))
        {
            lblError.Text = "All volunteers have already been scheduled.";
            return;
        }

        // Insert into volunteerSchedule table in the database
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = SQLInsertQuery;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
        catch
        {           
            lblError.Text = "Error in submission for Schedule Volunteers. Cannot insert volunteers into database.";
            return;
        }

        // Update volunteers database to remove old business from table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = SQLRemoveBusiness;
            cmd.ExecuteNonQuery();
            cmd.Dispose();
            con.Close();
        }
        catch
        {
            //lblError.Text = "Error in submission for Schedule Volunteers. Cannot remove old assigned business into database.";
            lblError.Text = SQLRemoveBusiness;
            return;
        }

        // Show success message and refresh
        lblError.Text = "Volunteers scheduled for " + VisitDate;
        var meta = new HtmlMeta();
        meta.HttpEquiv = "Refresh";
        meta.Content = "3;url=volunteer_database.aspx";
        Page.Controls.Add(meta);

    }

    // Assigns the regular volunteer that was selected from the DDL on the check in section, found next to the checkboxes
    public void SubmitCheckIn(int VIDOfDate)
    {
        string SQLStatement;
        int BusinessCount = 1;
        string Vol1 = "", Reg1 = "", Vol2 = "", Reg2 = "", Vol3 = "", Reg3 = "", Vol4 = "", Reg4 = "", Vol5 = "", Reg5 = "", Vol6 = "", Reg6 = "", Vol7 = "", Reg7 = "", Vol8 = "", Reg8 = "", Vol9 = "", Reg9 = "", Vol10 = "", Reg10 = "", Vol11 = "", Reg11 = "", Vol12 = "", Reg12 = "", Vol13 = "", Reg13 = "", Vol14 = "", Reg14 = "", Vol15 = "", Reg15 = "", Vol16 = "", Reg16 = "", Vol17 = "", Reg17 = "", Vol18 = "", Reg18 = "", Vol19 = "", Reg19 = "", Vol20 = "", Reg20 = "", Vol21 = "", Reg21 = "", Vol22 = "", Reg22 = "", Vol23 = "", Reg23 = "", Vol24 = "", Reg24 = "", Vol25 = "", Reg25 = "", Vol26 = "", Reg26 = "", Vol27 = "", Reg27 = "", Vol28 = "", Reg28 = "", Vol29 = "", Reg29 = "", Vol30 = "", Reg30 = "", Vol31 = "", Reg31 = "", Vol32 = "", Reg32 = "";

        // Check for bodies and assign variables

        // Check for sposnor 1 bodies
        if (ddlRegVolS1.SelectedIndex != 0)
        {
            Reg1 = ddlRegVolS1.SelectedValue;
        }
        else
        {
            Reg1 = "None";
        }

        // Check for sposnor 2 bodies
        if (ddlRegVolS2.SelectedIndex != 0)
        {
            Reg2 = ddlRegVolS2.SelectedValue;
        }
        else
        {
            Reg2 = "None";
        }

        // Check for sposnor 3 bodies
        if (ddlRegVolS3.SelectedIndex != 0)
        {
            Reg3 = ddlRegVolS3.SelectedValue;
        }
        else
        {
            Reg3 = "None";
        }

        // Check for sposnor 4 bodies
        if (ddlRegVolS4.SelectedIndex != 0)
        {
            Reg4 = ddlRegVolS4.SelectedValue;
        }
        else
        {
            Reg4 = "None";
        }

        // Check for sposnor 5 bodies
        if (ddlRegVolS5.SelectedIndex != 0)
        {
            Reg5 = ddlRegVolS5.SelectedValue;
        }
        else
        {
            Reg5 = "None";
        }

        // Check for sposnor 6 bodies
        if (ddlRegVolS6.SelectedIndex != 0)
        {
            Reg6 = ddlRegVolS6.SelectedValue;
        }
        else
        {
            Reg6 = "None";
        }

        // Check for sposnor 7 bodies
        if (ddlRegVolS7.SelectedIndex != 0)
        {
            Reg7 = ddlRegVolS7.SelectedValue;
        }
        else
        {
            Reg7 = "None";
        }

        // Check for sposnor 8 bodies
        if (ddlRegVolS8.SelectedIndex != 0)
        {
            Reg8 = ddlRegVolS8.SelectedValue;
        }
        else
        {
            Reg8 = "None";
        }

        // Check for sposnor 9 bodies
        if (ddlRegVolS9.SelectedIndex != 0)
        {
            Reg9 = ddlRegVolS9.SelectedValue;
        }
        else
        {
            Reg9 = "None";
        }

        // Check for sposnor 10 bodies
        if (ddlRegVolS10.SelectedIndex != 0)
        {
            Reg10 = ddlRegVolS10.SelectedValue;
        }
        else
        {
            Reg10 = "None";
        }

        // Check for sposnor 11 bodies
        if (ddlRegVolS11.SelectedIndex != 0)
        {
            Reg11 = ddlRegVolS11.SelectedValue;
        }
        else
        {
            Reg11 = "None";
        }

        // Check for sposnor 12 bodies
        if (ddlRegVolS12.SelectedIndex != 0)
        {
            Reg12 = ddlRegVolS12.SelectedValue;
        }
        else
        {
            Reg12 = "None";
        }

        // Check for sposnor 13 bodies
        if (ddlRegVolS13.SelectedIndex != 0)
        {
            Reg13 = ddlRegVolS13.SelectedValue;
        }
        else
        {
            Reg13 = "None";
        }

        // Check for sposnor 14 bodies
        if (ddlRegVolS14.SelectedIndex != 0)
        {
            Reg14 = ddlRegVolS14.SelectedValue;
        }
        else
        {
            Reg14 = "None";
        }

        // Check for sposnor 15 bodies
        if (ddlRegVolS15.SelectedIndex != 0)
        {
            Reg15 = ddlRegVolS15.SelectedValue;
        }
        else
        {
            Reg15 = "None";
        }

        // Check for sposnor 16 bodies
        if (ddlRegVolS16.SelectedIndex != 0)
        {
            Reg16 = ddlRegVolS16.SelectedValue;
        }
        else
        {
            Reg16 = "None";
        }

        // Check for sposnor 17 bodies
        if (ddlRegVolS17.SelectedIndex != 0)
        {
            Reg17 = ddlRegVolS17.SelectedValue;
        }
        else
        {
            Reg17 = "None";
        }

        // Check for sposnor 18 bodies
        if (ddlRegVolS18.SelectedIndex != 0)
        {
            Reg18 = ddlRegVolS18.SelectedValue;
        }
        else
        {
            Reg18 = "None";
        }

        // Check for sposnor 19 bodies
        if (ddlRegVolS19.SelectedIndex != 0)
        {
            Reg19 = ddlRegVolS19.SelectedValue;
        }
        else
        {
            Reg19 = "None";
        }

        // Check for sposnor 20 bodies
        if (ddlRegVolS20.SelectedIndex != 0)
        {
            Reg20 = ddlRegVolS20.SelectedValue;
        }
        else
        {
            Reg20 = "None";
        }
        // Check for sposnor 21 bodies
        if (ddlRegVolS21.SelectedIndex != 0)
        {
            Reg21 = ddlRegVolS21.SelectedValue;
        }
        else
        {
            Reg21 = "None";
        }

        // Check for sposnor 22 bodies
        if (ddlRegVolS22.SelectedIndex != 0)
        {
            Reg22 = ddlRegVolS22.SelectedValue;
        }
        else
        {
            Reg22 = "None";
        }

        // Check for sposnor 23 bodies
        if (ddlRegVolS23.SelectedIndex != 0)
        {
            Reg23 = ddlRegVolS23.SelectedValue;
        }
        else
        {
            Reg23 = "None";
        }

        // Check for sposnor 24 bodies
        if (ddlRegVolS24.SelectedIndex != 0)
        {
            Reg24 = ddlRegVolS24.SelectedValue;
        }
        else
        {
            Reg24 = "None";
        }

        // Check for sposnor 25 bodies
        if (ddlRegVolS25.SelectedIndex != 0)
        {
            Reg25 = ddlRegVolS25.SelectedValue;
        }
        else
        {
            Reg25 = "None";
        }

        // Check for sposnor 26 bodies
        if (ddlRegVolS26.SelectedIndex != 0)
        {
            Reg26 = ddlRegVolS26.SelectedValue;
        }
        else
        {
            Reg26 = "None";
        }

        // Check for sposnor 27 bodies
        if (ddlRegVolS27.SelectedIndex != 0)
        {
            Reg27 = ddlRegVolS27.SelectedValue;
        }
        else
        {
            Reg27 = "None";
        }

        // Check for sposnor 28 bodies
        if (ddlRegVolS28.SelectedIndex != 0)
        {
            Reg28 = ddlRegVolS28.SelectedValue;
        }
        else
        {
            Reg28 = "None";
        }

        // Check for sposnor 29 bodies
        if (ddlRegVolS29.SelectedIndex != 0)
        {
            Reg29 = ddlRegVolS29.SelectedValue;
        }
        else
        {
            Reg29 = "None";
        }

        // Check for sposnor 30 bodies
        if (ddlRegVolS30.SelectedIndex != 0)
        {
            Reg30 = ddlRegVolS30.SelectedValue;
        }
        else
        {
            Reg30 = "None";
        }

        // Check for sposnor 31 bodies
        if (ddlRegVolS31.SelectedIndex != 0)
        {
            Reg31 = ddlRegVolS31.SelectedValue;
        }
        else
        {
            Reg31 = "None";
        }

        // Check for sposnor 32 bodies
        if (ddlRegVolS32.SelectedIndex != 0)
        {
            Reg32 = ddlRegVolS32.SelectedValue;
        }
        else
        {
            Reg32 = "None";
        }


        // Calculuate amount of volunteers in each business
        while (BusinessCount < 32)
        {

            // Check if business ID is not 4 (no business for ID 4)
            //if (BusinessCount == 2 || BusinessCount == 3)
            //{
            //    BusinessCount += 1;
            //}

            // Get the count 
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = @"SELECT COUNT(*) as occur FROM volunteersFP v
									  WHERE v.visitID='" + VIDOfDate + "' AND v.sponsorID='" + BusinessCount + @"'
									  GROUP BY v.sponsorID";
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {

                    // Checking if there are volunteers in that business
                    if (dr.HasRows)
                    {

                        // Assign count to variable based on business
                        switch (BusinessCount)
                        {
                            case 1:
                                {
                                    Vol1 = dr["occur"].ToString();
                                    break;
                                }
                            case 2:
                                {
                                    Vol2 = dr["occur"].ToString();
                                    break;
                                }
                            case 3:
                                {
                                    Vol3 = dr["occur"].ToString();
                                    break;
                                }
                            case 4:
                                {
                                    Vol4 = dr["occur"].ToString();
                                    break;
                                }
                            case 5:
                                {
                                    Vol5 = dr["occur"].ToString();
                                    break;
                                }
                            case 6:
                                {
                                    Vol6 = dr["occur"].ToString();
                                    break;
                                }
                            case 7:
                                {
                                    Vol7 = dr["occur"].ToString();
                                    break;
                                }
                            case 8:
                                {
                                    Vol8 = dr["occur"].ToString();
                                    break;
                                }
                            case 9:
                                {
                                    Vol9 = dr["occur"].ToString();
                                    break;
                                }
                            case 10:
                                {
                                    Vol10 = dr["occur"].ToString();
                                    break;
                                }
                            case 11:
                                {
                                    Vol11 = dr["occur"].ToString();
                                    break;
                                }
                            case 12:
                                {
                                    Vol12 = dr["occur"].ToString();
                                    break;
                                }
                            case 13:
                                {
                                    Vol13 = dr["occur"].ToString();
                                    break;
                                }
                            case 14:
                                {
                                    Vol14 = dr["occur"].ToString();
                                    break;
                                }
                            case 15:
                                {
                                    Vol15 = dr["occur"].ToString();
                                    break;
                                }
                            case 16:
                                {
                                    Vol16 = dr["occur"].ToString();
                                    break;
                                }
                            case 17:
                                {
                                    Vol17 = dr["occur"].ToString();
                                    break;
                                }
                            case 18:
                                {
                                    Vol18 = dr["occur"].ToString();
                                    break;
                                }
                            case 19:
                                {
                                    Vol19 = dr["occur"].ToString();
                                    break;
                                }
                            case 20:
                                {
                                    Vol20 = dr["occur"].ToString();
                                    break;
                                }
                            case 21:
                                {
                                    Vol21 = dr["occur"].ToString();
                                    break;
                                }
                            case 22:
                                {
                                    Vol22 = dr["occur"].ToString();
                                    break;
                                }
                            case 23:
                                {
                                    Vol23 = dr["occur"].ToString();
                                    break;
                                }
                            case 24:
                                {
                                    Vol24 = dr["occur"].ToString();
                                    break;
                                }
                            case 25:
                                {
                                    Vol25 = dr["occur"].ToString();
                                    break;
                                }
                            case 26:
                                {
                                    Vol26 = dr["occur"].ToString();
                                    break;
                                }
                            case 27:
                                {
                                    Vol27 = dr["occur"].ToString();
                                    break;
                                }
                            case 28:
                                {
                                    Vol28 = dr["occur"].ToString();
                                    break;
                                }
                            case 29:
                                {
                                    Vol29 = dr["occur"].ToString();
                                    break;
                                }
                            case 30:
                                {
                                    Vol30 = dr["occur"].ToString();
                                    break;
                                }
                            case 31:
                                {
                                    Vol31 = dr["occur"].ToString();
                                    break;
                                }
                            case 32:
                                {
                                    Vol32 = dr["occur"].ToString();
                                    break;
                                }
                        }

                    }

                }

                cmd.Dispose();
                con.Close();
            }

            catch
            {
                lblError.Text = "Error in SubmitCheckIn(). Could not assign volunteer count.";
                return;
            }

            // add 1 to count
            BusinessCount += 1;

        }

        cmd.Dispose();
        con.Close();

        // Check if visit date exists, if not, insert new entry, otherwise update existing entry
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT visitID FROM volunteersCheckInFP WHERE visitID='" + VIDOfDate + "'";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            //Check if check in for visit id is already added, otherwise, do insert query
            if (dr.HasRows == true)
            {
                SQLStatement = @"UPDATE volunteersCheckInFP 
								SET vol1='" + Vol1 + "', reg1='" + Reg1 + "', vol2='" + Vol2 + "', reg2='" + Reg2 + "', vol3='" + Vol3 + @"'
								, reg3='" + Reg3 + "', vol4='" + Vol4 + "', reg4 ='" + Reg4 + "', vol5='" + Vol5 + "', reg5='" + Reg5 + @"'
								, vol6='" + Vol6 + "', reg6='" + Reg6+ "', vol7='" + Vol7 + "', reg7='" + Reg7 + "', vol8='" + Vol8 + @"'
								, reg8='" + Reg8 + "', vol9='" + Vol9 + "', reg9='" + Reg9 + "', vol10='" + Vol10 + "', reg10='" + Reg10 + @"'
								, vol11='" + Vol11 + "', reg11='" + Reg11 + "', vol12='" + Vol12 + "', reg12='" + Reg12 + "', vol13='" + Vol13 + @"'
								, reg13='" + Reg13 + "', vol14='" + Vol14 + "', reg14 ='" + Reg14+ "', vol15='" + Vol15+ "', reg15='" + Reg15 + @"'
								, vol16='" + Vol16 + "', reg16='" + Reg16+ "', vol17='" + Vol17 + "', reg17='" + Reg17 + "', vol18='" + Vol18+ @"'
								, reg18='" + Reg18+ "', vol19='" + Vol19 + "', reg19='" + Reg19+ "', vol20='" + Vol20+ "', reg20='" + Reg20 + @"'
								, vol21='" + Vol21+ "', reg21='" + Reg21 + "', vol22='" + Vol22 + "', reg22='" + Reg22 + "', vol23='" + Vol23 + "', reg23='" + Reg23 + @"'
                                , vol24='" + Vol24+ "', reg24='" + Reg24 + "', vol25='" + Vol25 + "', reg25='" + Reg25 + "', vol26='" + Vol26 + "', reg26='" + Reg26 + @"'
                                , vol27='" + Vol27 + "', reg27='" + Reg27 + "', vol28='" + Vol28 + "', reg28='" + Reg28 + "', vol29='" + Vol29 + "', reg29='" + Reg29 + @"'
                                , vol30='" + Vol30+ "', reg30='" + Reg30 + "', vol31='" + Vol31 + "', reg31='" + Reg31 + "', vol32='" + Vol32 + "', reg32='" + Reg32 + @"'
								WHERE visitID='" + VIDOfDate + "'";
            }
            else
            {
                SQLStatement = @"INSERT INTO volunteersCheckInFP
								(visitID, vol1, reg1, vol2, reg2, vol3, reg3, vol4, reg4, vol5, reg5, vol6, reg6, vol7, reg7, vol8, reg8, vol9, reg9
                                , vol10, reg10, vol11, reg11, vol12, reg12, vol13, reg13, vol14, reg14, vol15, reg15, vol16, reg16, vol17, reg17, vol18, reg18, vol19, reg19
                                , vol20, reg20, vol21, reg21, vol22, reg22, vol23, reg23, vol24, reg24, vol25, reg25, vol26, reg26, vol27, reg27, vol28, reg28, vol29, reg29
                                , vol30, reg30, vol31, reg31, vol32, reg32)
                                VALUES ('" + VIDOfDate + "', '" + Vol1 + "', '" + Reg1 + "', '" + Vol2 + "', '" + Reg2 + "', '" + Vol3 + "', '" + Reg3 + "', '" + Vol4 + "', '" + Reg4 + @"'
								, '" + Vol5 + "', '" + Reg5 + "', '" + Vol6 + "', '" + Reg6 + "', '" + Vol7 + "', '" + Reg7 + "', '" + Vol8 + "', '" + Reg8 + "', '" + Vol9 + @"'
								, '" + Reg9 + "', '" + Vol10 + "', '" + Reg10 + "', '" + Vol11 + "', '" + Reg11 + "', '" + Vol12 + "', '" + Reg12 + "', '" + Vol13 + "', '" + Reg13 + @"'
								, '" + Vol14 + "', '" + Reg14 + "', '" + Vol15 + "', '" + Reg15 + "', '" + Vol16 + "', '" + Reg16  + "', '" + Vol17 + "', '" + Reg17 + "', '" + Vol18 + @"'
								, '" + Reg18 + "', '" + Vol19 + "', '" + Reg19 + "', '" + Vol20 + "', '" + Reg20 + "', '" + Vol21 + "', '" + Reg21 + "', '" + Vol22 + "', '" + Reg22 + "', '" + Vol23 + "', '" + Reg23 + @"'
                                , '" + Vol24 + "', '" + Reg24 + "', '" + Vol25 + "', '" + Reg25 + "', '" + Vol26 + "', '" + Reg26  + "', '" + Vol27 + "', '" + Reg27 + "', '" + Vol28 + "','" + Reg28 + @"'
                                , '" + Vol29 + "', '" + Reg29 + "', '" + Vol30 + "', '" + Reg30 + "', '" + Vol31 + "', '" + Reg31  + "', '" + Vol32 + "', '" + Reg32 + "')";
            }

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            lblError.Text = "Error in SubmitCheckIn(). Could not detect selected visit date in database.";
            return;
        }

        cmd.Dispose();
        con.Close();

        // Submit data into SQL server
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = SQLStatement;

            cmd.ExecuteNonQuery();

            lblError.Text = "Successfully updated volunteer check in.";

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            //"Error in SubmitCheckIn(). Could not complete the check in process."
            lblError.Text = SQLStatement;
            return;
        }

        // Refresh page
        var meta = new HtmlMeta();
        meta.HttpEquiv = "Refresh";
        meta.Content = "3;url=volunteer_database.aspx";
        Page.Controls.Add(meta);

    }

    // Loads a volunteer from a vol name DDL into a gridview found in the schedule volunteers section
    public void AddVolIntoTable()
    {
        string[] VolNameWithID = ddlVolNameSchedule.SelectedValue.Split('(');
        string VolName = VolNameWithID[0];
        string[] VolIDWithParentheses = VolNameWithID[1].Split(')');
        string VolID = VolIDWithParentheses[0];
        var ScheduledVolRow = ScheduleVolTable.NewRow();

        ScheduledVolRow["Volunteer ID"] = VolID;
        ScheduledVolRow["Volunteer Name"] = VolName;

        ScheduleVolTable.Rows.Add(ScheduledVolRow);
        dgvScheduledVol.DataSource = ScheduleVolTable;
        dgvScheduledVol.DataBind();

        Session["dt"] = ScheduleVolTable;

    }



    // All of the volunteers gridview modules
    protected void dgvVolunteers_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        GridViewRow row = dgvVolunteers.Rows[0];                           // Code is used to enable the editing prodecure
        int ID = Convert.ToInt32(dgvVolunteers.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string firstName = ((TextBox)dgvVolunteers.Rows[e.RowIndex].FindControl("tbFirstNameDGV")).Text;
        string lastName = ((TextBox)dgvVolunteers.Rows[e.RowIndex].FindControl("tbLastNameDGV")).Text;
        string sponsorID = ((DropDownList)dgvVolunteers.Rows[e.RowIndex].FindControl("ddlSponsorNameDGV")).SelectedValue;
        string schoolID = ((DropDownList)dgvVolunteers.Rows[e.RowIndex].FindControl("ddlSchoolNameDGV")).SelectedValue;
        string visitDate = ((TextBox)dgvVolunteers.Rows[e.RowIndex].FindControl("tbVisitDateDGV")).Text;
        string pr = ((DropDownList)dgvVolunteers.Rows[e.RowIndex].FindControl("ddlPRDGV")).SelectedValue;
        string svHours = ((TextBox)dgvVolunteers.Rows[e.RowIndex].FindControl("tbSVHoursDGV")).Text;
        string notes = ((TextBox)dgvVolunteers.Rows[e.RowIndex].FindControl("tbNotesDGV")).Text;
        bool regular = ((CheckBox)dgvVolunteers.Rows[e.RowIndex].FindControl("chkRegularDGV")).Checked;
        string VIDOfDate;

        // Get visit ID of date
        try
        {
            VIDOfDate = (VisitData.GetVisitIDFromDate(visitDate)).ToString();
        }
        catch
        {
            lblError.Text = "Error in Updating(). Could not get visit ID of date entered.";
            return;
        }

        // Check if visit date entered is an existing visit date
        if (VIDOfDate == "" | VIDOfDate == "0")
        {
            lblError.Text = "No visit scheduled for entered visit date. Please create a visit for that date or enter a different date that has already been created.";
            return;
        }

        // Update volunteers table
        try
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand("UPDATE volunteersFP SET firstName=@firstName, lastName=@lastName, sponsorID=@sponsorID, schoolID=@schoolID, pr=@pr, svHours=@svHours, notes=@notes, regular=@regular WHERE ID=@Id"))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);
                    cmd.Parameters.AddWithValue("@sponsorID", sponsorID);
                    cmd.Parameters.AddWithValue("@schoolID", schoolID);
                    cmd.Parameters.AddWithValue("@pr", pr);
                    cmd.Parameters.AddWithValue("@svHours", svHours);
                    cmd.Parameters.AddWithValue("@notes", notes);
                    cmd.Parameters.AddWithValue("@regular", regular);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dgvVolunteers.EditIndex = -1; // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error with updating table.";
            return;
        }

        // Update checkboxes if check in is visible
        if (divCheckIn.Visible == true)
        {
            // Update checkboxes
            SubmitCheckIn(int.Parse(VIDOfDate));
        }

        //Update vol schedule
        try
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand("UPDATE volunteersScheduleFP SET visitID=@visitID WHERE volunteerID=@volunteerID AND visitID=@visitID"))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@firstName", firstName);
                    cmd.Parameters.AddWithValue("@lastName", lastName);
                    cmd.Parameters.AddWithValue("@sponsorID", sponsorID);
                    cmd.Parameters.AddWithValue("@schoolID", schoolID);
                    cmd.Parameters.AddWithValue("@pr", pr);
                    cmd.Parameters.AddWithValue("@svHours", svHours);
                    cmd.Parameters.AddWithValue("@notes", notes);
                    cmd.Parameters.AddWithValue("@regular", regular);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dgvVolunteers.EditIndex = -1; // reset the grid after editing
            LoadData();
        }
        catch
        {

        }

    }

    protected void dgvVolunteers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            string lblSchool = ((Label)e.Row.FindControl("lblSchoolNameDGV")).Text;
            string lblSponsor = ((Label)e.Row.FindControl("lblSponsorNameDGV")).Text;
            string lblPR = ((Label)e.Row.FindControl("lblPRDGV")).Text;

            DropDownList ddlSchool = (DropDownList)e.Row.FindControl("ddlSchoolNameDGV");
            DropDownList ddlSponsor = (DropDownList)e.Row.FindControl("ddlSponsorNameDGV");
            DropDownList ddlPR = (DropDownList)e.Row.FindControl("ddlPRDGV");


            // School Dropdown
            Gridviews.SchoolNames(ddlSchool, lblSchool);

            //Business drop down
            Gridviews.Sponsors(ddlSponsor, lblSponsor);

            // PR Dropdown           
            ddlPR.Items.FindByValue(lblPR).Selected = true;

            // Assign different colors for visit dates           
            if (ddlSchoolNameCheckin.Visible == true) // And schoolNameSchedule.Visible = False
            {
                switch (ddlSchoolNameCheckin.Items.Count)
                {
                    //1 school
                    case 2:
                        {
                            if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[1].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#afd8ff");
                            }

                            ddlSchool.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                            break;
                        }
                    
                    //2 schools
                    case 3:
                        {
                            if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[1].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#afd8ff");
                            }
                            else if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[2].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#ffafaf");
                            }

                            ddlSchool.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                            ddlSchool.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                            break;
                        }
                    
                    //3 schools
                    case 4:
                        {
                            if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[1].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#afd8ff");
                            }
                            else if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[2].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#ffafaf");
                            }
                            else if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[3].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#bfffaf");
                            }

                            ddlSchool.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                            ddlSchool.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                            ddlSchool.Items[3].Attributes.CssStyle.Add("background-color", "#bfffaf");
                            break;
                        }
                    
                    //4 schools
                    case 5:
                        {
                            if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[1].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#afd8ff");
                            }
                            else if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[2].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#ffafaf");
                            }
                            else if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[3].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#bfffaf");
                            }
                            else if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[4].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#afc3ff");
                            }

                            ddlSchool.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                            ddlSchool.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                            ddlSchool.Items[3].Attributes.CssStyle.Add("background-color", "#bfffaf");
                            ddlSchool.Items[4].Attributes.CssStyle.Add("background-color", "#afc3ff");
                            break;
                        }
                    
                    //5 schools
                    case 6:
                        {
                            if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[1].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#afd8ff");
                            }
                            else if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[2].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#ffafaf");
                            }
                            else if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[3].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#bfffaf");
                            }
                            else if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[4].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#afc3ff");
                            }
                            else if (int.Parse(lblSchool) == int.Parse(SchoolData.GetSchoolID(ddlSchoolNameCheckin.Items[5].Value).ToString()))
                            {
                                e.Row.BackColor = ColorTranslator.FromHtml("#ffd8af");
                            }

                            ddlSchool.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                            ddlSchool.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                            ddlSchool.Items[3].Attributes.CssStyle.Add("background-color", "#bfffaf");
                            ddlSchool.Items[4].Attributes.CssStyle.Add("background-color", "#afc3ff");
                            ddlSchool.Items[5].Attributes.CssStyle.Add("background-color", "#ffd8af");
                            break;
                        }
                }
            }
                    
        }
    }

    protected void dgvVolunteers_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvVolunteers.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void dgvVolunteers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = dgvVolunteers.Rows[0];                           // Code is used to enable the editing prodecure
        int ID = Convert.ToInt32(dgvVolunteers.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string SQLStatement;

        //If check in screen is visible, only delete the volunteer's entry on the volunteerScheduleFP table
        if (divCheckIn.Visible == true)
        {
            string VisitID = VisitData.GetVisitIDFromDate(tbVisitDateCheckin.Text).ToString();
            SQLStatement = "DELETE FROM volunteersScheduleFP WHERE volunteerID=@ID AND visitID='" + VisitID + "'";
        }
        else //deletes the volunteer entirely from the volunteersFP table
        {
            SQLStatement = "DELETE FROM volunteersFP WHERE id=@ID";
        }

        try
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(SQLStatement))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dgvVolunteers.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }

        catch
        {
            lblError.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }

    }

    protected void dgvVolunteers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvVolunteers.EditIndex = -1;
        LoadData();
    }

    protected void dgvVolunteers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvVolunteers.PageIndex = e.NewPageIndex;
        LoadData();
    }


    //Deletion handler for the view volunteers gridview
    protected void dgvViewVolCtrl_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        GridViewRow row = dgvViewVolCtrl.Rows[0];                           // Code is used to enable the editing prodecure
        int ID = Convert.ToInt32(dgvViewVolCtrl.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string SQLStatement;

        SQLStatement = "DELETE FROM volunteersFP WHERE id=@ID";

        try
        {
            using (var con = new SqlConnection(ConnectionString))
            {
                using (var cmd = new SqlCommand(SQLStatement))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dgvViewVolCtrl.EditIndex = -1;       // reset the grid after editing
            LoadViewVolCtrlGridview();
        }

        catch
        {
            lblError.Text = "Error in rowDeleting in View Volunteers. Cannot delete row.";
            return;
        }
    }



    // All button controls (most go to another sub or function)
    protected void submit_btn_Click(object sender, EventArgs e)
    {
        SubmitAddNewVolunteer();
    }

    protected void submitSchedule_btn_Click(object sender, EventArgs e)
    {
        SubmitScheduleVolunteers();
    }

    protected void submitCheckIn_btn_Click(object sender, EventArgs e)
    {
        if (tbVisitDateCheckin.Text != "")
        {
            int VIDOfDate = int.Parse(VisitData.GetVisitIDFromDate(tbVisitDateCheckin.Text).ToString());
            SubmitCheckIn(VIDOfDate);
        }
    }

    protected void search_btn_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void sortBy_btn_Click(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void refresh_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("volunteer_database.aspx");
    }

    protected void businessAssignments_btn_Click(object sender, EventArgs e)
    {
        string URL = "/pages/edit/Open_Closed_Status.aspx";
        string VisitID = "";
        string VisitDate = "";

        if (tbVisitDateCheckin.Text != "")
        {
            VisitDate = tbVisitDateCheckin.Text;
        }
        else
        {
            VisitDate = "";
        }

        // Check if visit date has been selected or entered
        if (!string.IsNullOrEmpty(VisitDate))
        {

            // get visit id of selected visit date
            if (!string.IsNullOrEmpty(VisitDate))
            {
                VisitID = VisitData.GetVisitIDFromDate(VisitDate).ToString();
            }

            // Add visit ID to URL
            URL += "?b=" + VisitID;

            Page.ClientScript.RegisterStartupScript(GetType(), "OpenInNewWindow", "OpenLinkInNewTab('" + URL + "');", true);
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "OpenInNewWindow", "OpenLinkInNewTab('" + URL + "');", true);
        }
    }

    protected void addVol_btn_Click(object sender, EventArgs e)
    {

        // Reveal add volunteer section if not revealed
        if (divAddVol.Visible == false)
        {
            divAddVol.Visible = true;
            divScheduleVol.Visible = false;
            divCheckIn.Visible = false;
            divViewVolControls.Visible = false;

            // Clear error label and visit date
            lblError.Text = "";
            tbVisitDateCheckin.Text = "";
            ddlVolNameSchedule.SelectedIndex = 0;

            LoadData();

        }

    }

    protected void scheduleVol_btn_Click(object sender, EventArgs e)
    {

        // Reveal add volunteer section if not revealed
        if (divScheduleVol.Visible == false)
        {
            divAddVol.Visible = false;
            divScheduleVol.Visible = true;
            divCheckIn.Visible = false;
            divViewVolControls.Visible = false;

            // Clear error label and visit date
            lblError.Text = "";
            tbVisitDateCheckin.Text = "";

            //Clear and reload volunteer names
            ddlVolNameSchedule.Items.Clear();
            LoadVolunteerNameDDL(ddlVolNameSchedule);

            LoadData();
        }

    }

    protected void checkIn_btn_Click(object sender, EventArgs e)
    {

        // Reveal check in section
        if (divCheckIn.Visible == false)
        {
            divCheckIn.Visible = true;
            divAddVol.Visible = false;
            divScheduleVol.Visible = false;
            divViewVolControls.Visible = false;

            // Clear error label and visit date
            lblError.Text = "";
            tbVisitDateCheckin.Text = "";
            ddlVolNameSchedule.SelectedIndex = 0;

            LoadData();
        }

    }

    protected void viewVol_btn_Click(object sender, EventArgs e)
    {
        divAddVol.Visible = false;
        divScheduleVol.Visible = false;
        divCheckIn.Visible = false;
        divViewVol.Visible = false;
        divViewVolControls.Visible = true;

        LoadVolunteerNameDDL(ddlVolNameViewVolCtrl);
    }



    // All textbox functions
    protected void tbVisitDateCheckin_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDateCheckin.Text != "")
        {
            ddlSchoolNameCheckin.Visible = true;
            aSchoolNameCheckin.Visible = true;

            // Load schools associated with selected visit date
            SchoolData.LoadVisitDateSchoolsDDL(tbVisitDateCheckin.Text, ddlSchoolNameCheckin);

            // Add an option to show all the volunteers for a visit date
            ddlSchoolNameCheckin.Items.RemoveAt(0);
            ddlSchoolNameCheckin.Items.Insert(0, "Show All Schools");

            // Assign colors to school DDL text
            switch (ddlSchoolNameCheckin.Items.Count)
            {
                case 1:
                    {
                        break;
                    }
                // The first item in the DDL is the Show All Schools item, does not need to change color
                case 2:
                    {
                        ddlSchoolNameCheckin.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                        break;
                    }
                case 3:
                    {
                        ddlSchoolNameCheckin.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                        ddlSchoolNameCheckin.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                        break;
                    }

                case 4:
                    {
                        ddlSchoolNameCheckin.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                        ddlSchoolNameCheckin.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                        ddlSchoolNameCheckin.Items[3].Attributes.CssStyle.Add("background-color", "#bfffaf");
                        break;
                    }
                case 5:
                    {
                        ddlSchoolNameCheckin.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                        ddlSchoolNameCheckin.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                        ddlSchoolNameCheckin.Items[3].Attributes.CssStyle.Add("background-color", "#bfffaf");
                        ddlSchoolNameCheckin.Items[4].Attributes.CssStyle.Add("background-color", "#afc3ff");
                        break;
                    }
                case 6:
                    {
                        ddlSchoolNameCheckin.Items[1].Attributes.CssStyle.Add("background-color", "#afd8ff");
                        ddlSchoolNameCheckin.Items[2].Attributes.CssStyle.Add("background-color", "#ffafaf");
                        ddlSchoolNameCheckin.Items[3].Attributes.CssStyle.Add("background-color", "#bfffaf");
                        ddlSchoolNameCheckin.Items[4].Attributes.CssStyle.Add("background-color", "#afc3ff");
                        ddlSchoolNameCheckin.Items[5].Attributes.CssStyle.Add("background-color", "#ffd8af");
                        break;
                    }
            }

            LoadData();
        }
    }

    protected void tbVisitDateViewVolCtrl_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDateViewVolCtrl.Text != "")
        {

            // Make vol name ddl selected at 0
            ddlVolNameViewVolCtrl.SelectedIndex = 0;

            // Load gridview
            LoadViewVolCtrlGridview();
        }
    }



    // All ddl functions
    protected void schoolNameSchedule_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchoolNameSchedule.SelectedIndex != 0)
        {

            // Get school id and load volunteer names from school
            string SID = SchoolData.GetSchoolID(ddlSchoolNameSchedule.SelectedValue).ToString();
            LoadVolunteerNameDDL(ddlVolNameSchedule, SID);
        }
        else
        {
            LoadVolunteerNameDDL(ddlVolNameSchedule);
        }
    }

    protected void ddlSchoolNameCheckin_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void volNameSchedule_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlVolNameSchedule.SelectedIndex != 0)
        {
            AddVolIntoTable();

            // Reveal table div
            divScheduledVol.Visible = true;
        }

    }

    protected void ddlVolNameViewVolCtrl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlVolNameViewVolCtrl.SelectedIndex != 0)
        {

            // Make visit date clear
            tbVisitDateViewVolCtrl.Text = "";

            // Load gridview
            LoadViewVolCtrlGridview();
        }
    }

    
}