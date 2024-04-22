using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class School_Report : Page
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
    private int VisitID;

    public School_Report()
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
            //Load schools DDL
            SchoolData.LoadSchoolsDDL(ddlSchoolName, false);

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load data
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLStatement = "SELECT * FROM schoolInfoFP";

        //Clear error
        lblError.Text = "";

        //Clear table
        dgvSchools.DataSource = null;
        dgvSchools.DataBind();

        //If loading by the DDL, add school name to search query
        if (ddlSchoolName.SelectedIndex != 0)
        {
            SQLStatement = SQLStatement + " WHERE schoolName='" + ddlSchoolName.SelectedValue + "'";
        }
        else if (tbSearch.Text != "")
        {
            SQLStatement = SQLStatement + " WHERE schoolName LIKE '%" + tbSearch.Text + "%'";
        }
        else
        {
            SQLStatement = SQLStatement + " WHERE NOT id='1' ORDER BY schoolName ASC";
        }

        //Load schoolInfoFP table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvSchools.DataSource = Review_sds;
            dgvSchools.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load schoolInfo table.";
            return;
        }
    }



    protected void dgvSchools_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvSchools.PageIndex = e.NewPageIndex;
        LoadData();
    }



    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (tbSearch.Text != "")
        {
            LoadData();
            ddlSchoolName.SelectedIndex = 0;
        }
        else
        {
            lblError.Text = "Please enter a search keyword and press 'Search'.";
            return;
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("school_report.aspx");
    }

    protected void ddlSchoolName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchoolName.SelectedIndex != 0)
        {
            LoadData();
        }
    }
}