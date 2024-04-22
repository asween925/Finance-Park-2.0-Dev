using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_Report : Page
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
    private Class_SchoolData SchoolData = new Class_SchoolData();
    private Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    private Class_GridviewFunctions Gridviews = new Class_GridviewFunctions();
    private int VisitID;

    public Teacher_Report()
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
            //Load school name DDL
            SchoolData.LoadSchoolsDDL(ddlSchoolName, false);

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load data
            LoadData();
        }
    }

    public void LoadData()
    {
        string SchoolID;
        string SQLStatement = "SELECT t.id, s.schoolName, t.firstName, t.lastName, t.email, t.password, t.contact, v.visitDate as currVisitDate, v2.visitDate as prevVisitDate FROM teacherInfoFP t LEFT JOIN schoolInfoFP s ON s.id = t.schoolID LEFT JOIN visitInfoFP v ON v.id = t.currVisitID LEFT JOIN visitInfoFP v2 ON v2.id = t.prevVisitID";

        //Clear teacher table
        dgvTeachers.DataSource = null;
        dgvTeachers.DataBind();

        //Clear error label
        lblError.Text = "";

        //Check if school name DDL or search field is entered
        if (ddlSchoolName.SelectedIndex != 0)
        {
            //get school id from name
            //SchoolID = SchoolData.GetSchoolID(ddlSchoolName.SelectedValue).ToString();
            SQLStatement = SQLStatement + " WHERE s.schoolName='" + ddlSchoolName.SelectedValue + "'";
        }
        else if (tbSearch.Text != "")
        {
            SQLStatement = SQLStatement + " WHERE t.firstName LIKE '%" + tbSearch.Text + "%' OR t.lastName LIKE '%" + tbSearch.Text + "%'";
        }
        else
        {
            SQLStatement = SQLStatement + " ORDER BY s.schoolName ASC";
        }

        //Load teacherInfoFP table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvTeachers.DataSource = Review_sds;
            dgvTeachers.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load teacherInfo table.";
            return;
        }
    }



    protected void dgvTeachers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvTeachers.PageIndex = e.NewPageIndex;
        LoadData();
    }



    protected void ddlSchoolName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchoolName.SelectedIndex != 0)
        {
            tbSearch.Text = "";
            LoadData();
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (tbSearch.Text != "")
        {
            ddlSchoolName.SelectedIndex = 0;
            LoadData();
        }
        else
        {
            lblError.Text = "Please enter a teacher name to search.";
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("teacher_report.aspx");
    }
}