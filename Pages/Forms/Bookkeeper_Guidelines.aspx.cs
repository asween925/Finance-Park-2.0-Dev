using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Bookkeeper_Guidelines : Page
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

    public Bookkeeper_Guidelines()
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
            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void LoadData()
    {
        //Load public or home school
        if (ddlLetterType.SelectedIndex == 0) 
        {
            pLetterTopPub.Visible = true;
            divTablePub.Visible = true;
            pLetterPayPub.Visible = true;

            pLetterTopHome.Visible = false;
            divTableHome.Visible = false;
            pLetterPayHome.Visible = false;
        }
        else
        {
            pLetterTopPub.Visible = false;
            divTablePub.Visible = false;
            pLetterPayPub.Visible = false;

            pLetterTopHome.Visible = true;
            divTableHome.Visible = true;
            pLetterPayHome.Visible = true;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "Print", "print();", true);
    }

    protected void ddlLetterType_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }
}