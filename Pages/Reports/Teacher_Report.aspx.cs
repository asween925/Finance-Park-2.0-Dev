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
            //Load school name DDL
            SchoolData.LoadSchoolsDDL(schoolName_ddl);

            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load data
            LoadData();
        }
    }

    public void LoadData()
    {
        string SchoolID;
        string SQLStatement = "SELECT t.id, s.schoolName, t.firstName, t.lastName, t.email, t.password, t.contact FROM teacherInfoFP t LEFT JOIN schoolInfoFP s ON s.id = t.schoolID";

        //Clear teacher table
        teachers_dgv.DataSource = null;
        teachers_dgv.DataBind();

        //Clear error label
        error_lbl.Text = "";

        //Check if school name DDL or search field is entered
        if (schoolName_ddl.SelectedIndex != 0)
        {
            //get school id from name
            //SchoolID = SchoolData.GetSchoolID(schoolName_ddl.SelectedValue).ToString();
            SQLStatement = SQLStatement + " WHERE s.schoolName='" + schoolName_ddl.SelectedValue + "'";
        }
        else if (search_tb.Text != "")
        {
            SQLStatement = SQLStatement + " WHERE t.firstName LIKE '%" + search_tb.Text + "%' OR t.lastName LIKE '%" + search_tb.Text + "%'";
        }
        else
        {
            SQLStatement = SQLStatement + " ORDER BY s.schoolName ASC";
        }

        //Load teacherInfoFP table
        //try
        //{
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            teachers_dgv.DataSource = Review_sds;
            teachers_dgv.DataBind();

            cmd.Dispose();
            con.Close();

        //}
        //catch
        //{
        //    error_lbl.Text = "Error in LoadData(). Cannot load teacherInfo table.";
        //    return;
        //}
    }



    protected void teachers_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        teachers_dgv.PageIndex = e.NewPageIndex;
        LoadData();
    }



    protected void schoolName_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (schoolName_ddl.SelectedIndex != 0)
        {
            search_tb.Text = "";
            LoadData();
        }
    }

    protected void search_btn_Click(object sender, EventArgs e)
    {
        if (search_tb.Text != "")
        {
            schoolName_ddl.SelectedIndex = 0;
            LoadData();
        }
        else
        {
            error_lbl.Text = "Please enter a teacher name to search.";
        }
    }

    protected void refresh_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("teacher_report.aspx");
    }
}