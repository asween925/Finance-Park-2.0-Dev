using Antlr.Runtime;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Simulation_Report : Page
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

    public Simulation_Report()
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
            //Populate School DDL
            SchoolData.LoadSchoolsDDL(ddlSchoolName);

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();

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
        dgvVisit.DataSource = null;
        dgvVisit.DataBind();

        //Check if visit date 
        if (tbVisitDate.Text != "")
        {
            SQLWhereVisitDate = " WHERE v.visitDate='" + tbVisitDate.Text + "' AND NOT v.school=1 ORDER BY v.visitDate DESC";
        }

        //Check if school name is selected
        if (ddlSchoolName.SelectedIndex != 0)
        {
            SQLWhereSchoolName = " WHERE s.schoolName='" + ddlSchoolName.SelectedValue + "' OR s2.schoolName='" + ddlSchoolName.SelectedValue + "' OR s3.schoolName='" + ddlSchoolName.SelectedValue + "' OR s4.schoolName='" + ddlSchoolName.SelectedValue + "' OR s5.schoolName='" + ddlSchoolName.SelectedValue + "' AND NOT v.school=1 ORDER BY v.visitDate DESC";
        }

        //Check if month is selected
        if (ddlMonth.SelectedIndex != 0) 
        {
            switch(ddlMonth.SelectedValue) 
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

            dgvVisit.PageSize = 31;
        }

        //Check if school name or visit date or month are blank
        if ((ddlSchoolName.SelectedIndex == 0) && (tbVisitDate.Text == "") && (ddlMonth.SelectedIndex == 0)) 
        {
            SQLWhereNot = " WHERE NOT v.school=1 ORDER BY v.visitDate DESC";
        }

        //Load visit table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            dgvVisit.DataSource = VisitData.LoadVisitInfoTable(SQLWhereVisitDate, SQLWhereSchoolName, SQLWhereMonth, SQLWhereNot);
            dgvVisit.DataBind();
        }
        catch
        {
            lblError.Text = "Error in LoadData. Cannot load data.";
            return;
        }

    }



    protected void dgvVisit_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvVisit.PageIndex = e.NewPageIndex;
        LoadData();
    }



    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("visit_report.aspx");
    }

    protected void ddlMonth_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMonth.SelectedIndex != 0)
        {
            tbVisitDate.Text = "";
            ddlSchoolName.SelectedIndex = ddlSchoolName.Items.IndexOf(ddlSchoolName.Items.FindByValue(""));
            LoadData();
        }
    }

    protected void ddlSchoolName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchoolName.SelectedIndex != 0)
        {
            tbVisitDate.Text = "";
            ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(""));
            LoadData();
        }
    }

    protected void tbVisitDate_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDate.Text != "")
        {
            ddlSchoolName.SelectedIndex = ddlSchoolName.Items.IndexOf(ddlSchoolName.Items.FindByValue(""));
            ddlMonth.SelectedIndex = ddlMonth.Items.IndexOf(ddlMonth.Items.FindByValue(""));
            LoadData();
        }
    }
}