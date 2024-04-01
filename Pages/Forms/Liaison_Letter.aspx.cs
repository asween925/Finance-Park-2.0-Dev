using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;

public partial class Liaison_Letter : Page
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
    private Class_Schedule Schedule = new Class_Schedule();

    public Liaison_Letter()
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
        var VisitDate = DateTime.Parse(tbVisitDate.Text);
        var VArrivalTime = DateTime.Parse(Schedule.GetVolArrivalTime(VisitData.LoadVisitInfoFromDate(VisitDate.ToString(), "vTrainingTime").ToString()).ToString());
        string SchoolName = ddlSchoolName.SelectedValue;
        string LiaisonName;
        var DismissalTime = DateTime.Parse(Schedule.GetDismissalTime(VArrivalTime.ToString()).ToString());

        //Reveal divs
        btnPrint.Visible = true;
        divLetter.Visible = true;

        // Get volunteer count, training time, and reply by
        //VMin = SchoolData.GetVolunteerRange(VisitDate, SchoolID).VMin;
        //VMax = SchoolData.GetVolunteerRange(VisitDate, SchoolID).VMax;

        // Get liaison information
        LiaisonName = SchoolData.LoadSchoolInfoFromSchool(SchoolName, "liaisonName").ToString();
        
        // Assign labels
        lblSchoolName.Text = SchoolName;
        lblVisitDate.Text = VisitDate.ToString("D");
        lblLiaison.Text = LiaisonName;
        lblArrivalTime.Text = VArrivalTime.ToString("t");
        lblVolunteerDismissal.Text = DismissalTime.ToString("t");

    }



    protected void tbVisitDate_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDate.Text != "")
        {
            //Reveal school DDL
            ddlSchoolName.Visible = true;
            school_p.Visible = true;

            //Load school name DDL
            SchoolData.LoadVisitingSchoolsDDL(tbVisitDate.Text, ddlSchoolName);
        }
        else
        {
            divInfo.Visible = false;
            btnPrint.Visible = false;
        }
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
        imgFPLogo.Visible = true;
        Page.ClientScript.RegisterStartupScript(GetType(), "Print", "window.print();", true);
    }
}