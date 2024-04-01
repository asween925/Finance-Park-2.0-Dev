using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_Student : Page
{
    private string SQLServer = ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string SQLDatabase = ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string SQLUser = ConfigurationManager.AppSettings["db_user"].ToString();
    private string SQLPassword = ConfigurationManager.AppSettings["db_password"].ToString();
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string ConnectionString;
    private Class_SQLCommands SQL = new Class_SQLCommands();
    private Class_VisitData VisitData = new Class_VisitData();
    private Class_SchoolData SchoolData = new Class_SchoolData();
    private Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    private Class_GridviewFunctions Gridviews = new Class_GridviewFunctions();
    private int VisitID;

    public Edit_Student()
    {
        ConnectionString = "Server=" + SQLServer + ";database=" + SQLDatabase + ";uid=" + SQLUser + ";pwd=" + SQLPassword + ";Connection Timeout=20;";        
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
            // Assign current visit ID to hidden field
            if (VisitID != 0)
            {
                hfCurrentVisitID.Value = VisitID.ToString();
            }

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void LoadData()
    {
        int VisitID = int.Parse(VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString());
        string SQLStatement = @"SELECT s.id, s.accountNum, a.pin, s.firstName, s.lastName, s.schoolID, s.businessID, s.jobID, s.teacherID, s.personaID, s.lunchServed 
                                FROM studentInfoFP s 
                                JOIN accountNumsFP a ON s.accountNum = a.accountNum 
                                WHERE s.visitID='" + VisitID + "'";
        int SchoolID;

        //Clear error
        lblError.Text = "";

        //Clear table
        dgvStudents.DataSource = null;
        dgvStudents.DataBind();

        //If loading by the DDL, add school name to search query
        if (ddlSchoolName.SelectedIndex != 0)
        {
            //Get ID from school name and add WHERE clause to SQL statement
            SchoolID = int.Parse(SchoolData.GetSchoolID(ddlSchoolName.SelectedValue).ToString());
            SQLStatement = SQLStatement + " AND schoolID='" + SchoolID + "'";
        }
        else
        {
            SQLStatement = SQLStatement + " ORDER BY accountNum ASC";
        }

        //Load schoolInfoFP table
        //try
        //{
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvStudents.DataSource = Review_sds;
            dgvStudents.DataBind();

            cmd.Dispose();
            con.Close();

        //}
        //catch
        //{
        //    lblError.Text = "Error in LoadData(). Cannot load studentInfo table.";
        //    return;
        //}

        // Highlight row being edited
        foreach (GridViewRow row in dgvStudents.Rows)
        {
            if (row.RowIndex == dgvStudents.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }

    protected void dgvStudents_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(dgvStudents.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string AccountNum = ((TextBox)dgvStudents.Rows[e.RowIndex].FindControl("tbAccountNumDGV")).Text;
        string FirstName = ((TextBox)dgvStudents.Rows[e.RowIndex].FindControl("tbFirstNameDGV")).Text;
        string LastName = ((TextBox)dgvStudents.Rows[e.RowIndex].FindControl("tbLastNameDGV")).Text;
        string SchoolID = ((DropDownList)dgvStudents.Rows[e.RowIndex].FindControl("ddlSchoolNameDGV")).SelectedValue;
        string BusinessID = ((DropDownList)dgvStudents.Rows[e.RowIndex].FindControl("ddlBusinessNameDGV")).SelectedValue;
        string JobID = ((DropDownList)dgvStudents.Rows[e.RowIndex].FindControl("ddlJobNameDGV")).SelectedValue;
        string TeacherID = ((DropDownList)dgvStudents.Rows[e.RowIndex].FindControl("ddlTeacherNameDGV")).SelectedValue;
        string PersonaID = ((DropDownList)dgvStudents.Rows[e.RowIndex].FindControl("ddlPersonaNameDGV")).SelectedValue;
        bool LunchServed = ((CheckBox)dgvStudents.Rows[e.RowIndex].FindControl("chkLunchServedDGV")).Checked;

        //Update table
        try
        {
            SQL.UpdateRow(ID, "accountNum", AccountNum, "studentInfoFP");
            SQL.UpdateRow(ID, "firstName", FirstName, "studentInfoFP");
            SQL.UpdateRow(ID, "lastName", LastName, "studentInfoFP");
            SQL.UpdateRow(ID, "schoolID", SchoolID, "studentInfoFP");
            SQL.UpdateRow(ID, "businessID", BusinessID, "studentInfoFP");
            SQL.UpdateRow(ID, "jobID", JobID, "studentInfoFP");
            SQL.UpdateRow(ID, "teacherID", TeacherID, "studentInfoFP");
            SQL.UpdateRow(ID, "personaID", PersonaID, "studentInfoFP");
            SQL.UpdateRow(ID, "lunchServed", LunchServed.ToString(), "studentInfoFP");

            dgvStudents.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void dgvStudents_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(dgvStudents.DataKeys[e.RowIndex].Values[0]); // Gets id number

        try
        {
            SQL.DeleteRow(ID, "dgvStudents");

            dgvStudents.EditIndex = -1;       // reset the grid after editing

            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }
    }

    protected void dgvStudents_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvStudents.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void dgvStudents_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvStudents.EditIndex = -1;
        LoadData();
    }

    protected void dgvStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvStudents.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void dgvStudents_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            int VisitID = int.Parse(VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString());
            string lblSchool = (e.Row.FindControl("lblSchoolIDDGV") as Label).Text;
            string lblBusiness = (e.Row.FindControl("lblBusinessIDDGV") as Label).Text;
            string lblJob = (e.Row.FindControl("lblJobIDDGV") as Label).Text;
            string lblTeacher = (e.Row.FindControl("lblTeacherIDDGV") as Label).Text;
            string lblPersona = (e.Row.FindControl("lblPersonaIDDGV") as Label).Text;
            DropDownList ddlSchool = e.Row.FindControl("ddlSchoolNameDGV") as DropDownList;
            DropDownList ddlBusiness = e.Row.FindControl("ddlBusinessNameDGV") as DropDownList;
            DropDownList ddlJob = e.Row.FindControl("ddlJobNameDGV") as DropDownList;
            DropDownList ddlTeacher = e.Row.FindControl("ddlTeacherNameDGV") as DropDownList;
            DropDownList ddlPersona = e.Row.FindControl("ddlPersonaNameDGV") as DropDownList;

            //Load gridview school DDLs with school names, business names, job names, persona names, and teacher names
            Gridviews.VisitingSchoolNames(ddlSchool, lblSchool, VisitID);
            Gridviews.BusinessNames(ddlBusiness, lblBusiness);
            Gridviews.JobTitle(ddlJob, lblJob);
            //Gridviews.TeacherName(ddlTeacher, lblTeacher);
            Gridviews.SchoolOnlyTeacherName(ddlTeacher, lblTeacher, 66); //int.Parse(lblSchool)
            Gridviews.Personas(ddlPersona, lblPersona);
        }
    }


    protected void tbVisitDate_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDate.Text != "")
        {
            //Show school name div
            divSchoolName.Visible = true;

            //Load schools in DDL
            SchoolData.LoadVisitDateSchoolsDDL(tbVisitDate.Text, ddlSchoolName);

            //Load data
            LoadData();
        }
    }

    protected void ddlSchoolName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("Edit_Student.aspx");
    }
}