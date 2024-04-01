using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Kit_Report : Page
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

    public Kit_Report()
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

            //Load school name ddl
            SchoolData.LoadSchoolsDDL(ddlSchoolName);
        }

        //Load data
        LoadData();
    }

    public void LoadData()
    {
        string SQLStatement = @"SELECT k.id, v.visitDate, s.schoolName, k.workbooks, k.kitTotal,
                                 CASE
                                    WHEN k.kitTotal = 3 THEN CONCAT(k.kit1, ', ', k.kit2, ', ',k.kit3)
	                                WHEN k.kitTotal = 4 THEN CONCAT(k.kit1, ', ', k.kit2, ', ',k.kit3, ', ',k.kit4)
	                                WHEN k.kitTotal = 5 THEN CONCAT(k.kit1, ', ', k.kit2, ', ',k.kit3, ', ',k.kit4, ', ',k.kit5)
	                                WHEN k.kitTotal = 6 THEN CONCAT(k.kit1, ', ', k.kit2, ', ',k.kit3, ', ',k.kit4, ', ',k.kit5, ', ',k.kit6)
	                                WHEN k.kitTotal = 7 THEN CONCAT(k.kit1, ', ', k.kit2, ', ',k.kit3, ', ',k.kit4, ', ',k.kit5, ', ',k.kit6, ', ',k.kit7)
	                                WHEN k.kitTotal = 8 THEN CONCAT(k.kit1, ', ', k.kit2, ', ',k.kit3, ', ',k.kit4, ', ',k.kit5, ', ',k.kit6, ', ',k.kit7, ', ',k.kit8)
	                                WHEN k.kitTotal = 9 THEN CONCAT(k.kit1, ', ', k.kit2, ', ',k.kit3, ', ',k.kit4, ', ',k.kit5, ', ',k.kit6, ', ',k.kit7, ', ',k.kit8, ', ',k.kit9)
	                                WHEN k.kitTotal = 10 THEN CONCAT(k.kit1, ', ', k.kit2, ', ',k.kit3, ', ',k.kit4, ', ',k.kit5, ', ',k.kit6, ', ',k.kit7, ', ',k.kit8, ', ',k.kit9, ', ',k.kit10)
                                    ELSE '0'
                                END as kits
                                  FROM kitsFP k
                                  JOIN visitInfoFP v ON v.id = k.visitID
                                  JOIN schoolInfoFP s ON s.id = k.schoolID";

        //Clear error
        lblError.Text = "";

        //Clear table
        dgvKits.DataSource = null;
        dgvKits.DataBind();

        //If loading by the DDL, add school name to search query
        if (ddlSchoolName.SelectedIndex != 0)
        {
            SQLStatement = SQLStatement + " WHERE s.schoolName='" + ddlSchoolName.SelectedValue + "'";
        }
        else
        {
            SQLStatement = SQLStatement + " ORDER BY v.visitDate ASC";
        }

        //Load kits table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvKits.DataSource = Review_sds;
            dgvKits.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load kits table.";
            return;
        }
    }

    protected void dgvKits_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvKits.PageIndex = e.NewPageIndex;
        LoadData();
    }



    protected void ddlSchoolName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchoolName.SelectedIndex != 0)
        {
            LoadData();
        }
    }



    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //Print
        Page.ClientScript.RegisterStartupScript(GetType(), "Print", "print();", true);
    }

}