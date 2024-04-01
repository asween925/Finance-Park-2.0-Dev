using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Student_Report : Page
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

    public Student_Report()
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
        int SchoolID;
        int VisitID = int.Parse(VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString());
        DateTime VisitDate = DateTime.Parse(tbVisitDate.Text);
        string SQLStatement = @"SELECT s.id, s.accountNum, a.pin, s.firstName, s.lastName, p.maritalStatus, p.numOfChildren, j.educationBG, b.businessName, j.jobTitle
                                FROM studentInfoFP s 
                                JOIN accountNumsFP a ON s.accountNum = a.accountNum 
								JOIN schoolInfoFP sc ON sc.id = s.schoolID
								JOIN businessInfoFP b ON b.id = s.businessID
								JOIN jobsFP j ON j.id = s.jobID
								JOIN personasFP p ON p.id = s.personaID
                                WHERE s.visitID='" + VisitID + "'";

        //Clear error label
        lblError.Text = "";

        //Clear teacher table
        dgvStudents.DataSource = null;
        dgvStudents.DataBind();

        //Check if school name DDL or search field is entered
        if (ddlSchoolName.SelectedIndex != 0)
        {
            //get school id from name
            SchoolID = int.Parse(SchoolData.GetSchoolID(ddlSchoolName.SelectedValue).ToString());
            SQLStatement = SQLStatement + " AND s.schoolID='" + SchoolID + "'";

            //Assign school name to print only label
            lblPrintSchool.Text = " for " + ddlSchoolName.SelectedValue;
        }
        else
        {
            SQLStatement = SQLStatement + " ORDER BY s.accountNum ASC";
        }

        //Load print only visit date label
        lblPrintVisitDate.Text = VisitDate.ToShortDateString();

        //Load teacherInfoFP table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvStudents.DataSource = Review_sds;
            dgvStudents.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load studentInfo table.";
            return;
        }
    }



    protected void dgvStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvStudents.PageIndex = e.NewPageIndex;
        LoadData();
    }


    protected void tbVisitDate_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDate.Text != "")
        {
            //Check if visit date exists
            if (VisitData.ShowVisitConfirmation(tbVisitDate.Text).ToString() != "")
            {
                lblError.Text = VisitData.ShowVisitConfirmation(tbVisitDate.Text).ToString();
                return;
            }

            //Load schools of visit date
            SchoolData.LoadVisitDateSchoolsDDL(tbVisitDate.Text, ddlSchoolName);

            //Show schools div
            divSchoolName.Visible = true;

            LoadData();
        }
    }



    protected void ddlSchoolName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }



    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("Student_Report.aspx");
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //Make print only div visible
        divPrintHeader.Visible = true;
        
        //Print
        Page.ClientScript.RegisterStartupScript(GetType(), "Print", "print();", true);       
    }
}