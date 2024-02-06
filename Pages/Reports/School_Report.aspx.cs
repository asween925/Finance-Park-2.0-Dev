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
            //Load schools DDL
            SchoolData.LoadSchoolsDDL(schoolName_ddl);

            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load data
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLStatement = "SELECT * FROM schoolInfoFP";

        //Clear error
        error_lbl.Text = "";

        //Clear table
        schools_dgv.DataSource = null;
        schools_dgv.DataBind();

        //If loading by the DDL, add school name to search query
        if (schoolName_ddl.SelectedIndex != 0)
        {
            SQLStatement = SQLStatement + " WHERE schoolName='" + schoolName_ddl.SelectedValue + "'";
        }
        else if (search_tb.Text != "")
        {
            SQLStatement = SQLStatement + " WHERE schoolName LIKE '%" + search_tb.Text + "%'";
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
            schools_dgv.DataSource = Review_sds;
            schools_dgv.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            error_lbl.Text = "Error in LoadData(). Cannot load schoolInfo table.";
            return;
        }
    }



    protected void schools_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        schools_dgv.PageIndex = e.NewPageIndex;
        LoadData();
    }



    protected void search_btn_Click(object sender, EventArgs e)
    {
        if (search_tb.Text != "")
        {
            LoadData();
            schoolName_ddl.SelectedIndex = 0;
        }
        else
        {
            error_lbl.Text = "Please enter a search keyword and press 'Search'.";
            return;
        }
    }

    protected void refresh_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("school_report.aspx");
    }

    protected void schoolName_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (schoolName_ddl.SelectedIndex != 0)
        {
            LoadData();
        }
    }
}