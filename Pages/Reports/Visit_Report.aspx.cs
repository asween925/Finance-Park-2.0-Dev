using Antlr.Runtime;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Visit_Report : Page
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

    public Visit_Report()
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
            //Populate School DDL
            SchoolData.LoadSchoolsDDL(schoolName_ddl);

            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load data
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLWhereVisitDate = "";
        string SQLWhereSchoolName = "";
        string SQLWhereMonth = "";
        string SQLWhereNot = "";
        int CurrentYear = DateTime.Today.Year;
        int SelectedMonth = 0;

        //Clear visit table
        visit_dgv.DataSource = null;
        visit_dgv.DataBind();

        //Check if visit date 
        if (visitDate_tb.Text != "")
        {
            SQLWhereVisitDate = " WHERE v.visitDate='" + visitDate_tb.Text + "' AND NOT v.school=1 ORDER BY v.visitDate DESC";
        }

        //Check if school name is selected
        if (schoolName_ddl.SelectedIndex != 0)
        {
            SQLWhereSchoolName = " WHERE s.schoolName='" + schoolName_ddl.SelectedValue + "' OR s2.schoolName='" + schoolName_ddl.SelectedValue + "' OR s3.schoolName='" + schoolName_ddl.SelectedValue + "' OR s4.schoolName='" + schoolName_ddl.SelectedValue + "' OR s5.schoolName='" + schoolName_ddl.SelectedValue + "' AND NOT v.school=1 ORDER BY v.visitDate DESC";
        }

        //Check if month is selected
        if (month_ddl.SelectedIndex != 0) 
        {
            switch(month_ddl.SelectedValue) 
            {
                case "January":
                    SelectedMonth = 1;
                    break;
                case "February":
                    SelectedMonth = 2;
                    break;
                case "March":
                    SelectedMonth = 3;
                    break;
                case "April":
                    SelectedMonth = 4;
                    break;
                case "May":
                    SelectedMonth = 5;
                    break;
                case "June":
                    SelectedMonth = 6;
                    break;
                case "July":
                    SelectedMonth = 7;
                    break;
                case "August":
                    SelectedMonth = 8;
                    break;
                case "September":
                    SelectedMonth = 9;
                    break;
                case "October":
                    SelectedMonth = 10;
                    break;
                case "November":
                    SelectedMonth = 11;
                    break;
                case "December":
                    SelectedMonth = 12;
                    break;
            }

            SQLWhereMonth = " WHERE DATEPART(MONTH, v.visitDate) = '" + SelectedMonth + "' AND DATEPART(YEAR, v.visitDate) = '" + CurrentYear + "' ORDER BY v.visitDate";

            visit_dgv.PageSize = 31;
        }

        //Check if school name or visit date or month are blank
        if ((schoolName_ddl.SelectedIndex == 0) && (visitDate_tb.Text == "") && (month_ddl.SelectedIndex == 0)) 
        {
            SQLWhereNot = " WHERE NOT v.school=1 ORDER BY v.visitDate DESC";
        }

        //Load visit table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            visit_dgv.DataSource = VisitData.LoadVisitInfoTable(SQLWhereVisitDate, SQLWhereSchoolName, SQLWhereMonth, SQLWhereNot);
            visit_dgv.DataBind();
        }
        catch
        {
            error_lbl.Text = "Error in LoadData. Cannot load data.";
            return;
        }

    }



    protected void visit_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        visit_dgv.PageIndex = e.NewPageIndex;
        LoadData();
    }



    protected void refresh_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("visit_report.aspx");
    }

    protected void month_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (month_ddl.SelectedIndex != 0)
        {
            visitDate_tb.Text = "";
            schoolName_ddl.SelectedIndex = schoolName_ddl.Items.IndexOf(schoolName_ddl.Items.FindByValue(""));
            LoadData();
        }
    }

    protected void schoolName_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (schoolName_ddl.SelectedIndex != 0)
        {
            visitDate_tb.Text = "";
            month_ddl.SelectedIndex = month_ddl.Items.IndexOf(month_ddl.Items.FindByValue(""));
            LoadData();
        }
    }

    protected void visitDate_tb_TextChanged(object sender, EventArgs e)
    {
        if (visitDate_tb.Text != "")
        {
            schoolName_ddl.SelectedIndex = schoolName_ddl.Items.IndexOf(schoolName_ddl.Items.FindByValue(""));
            month_ddl.SelectedIndex = month_ddl.Items.IndexOf(month_ddl.Items.FindByValue(""));
            LoadData();
        }
    }
}