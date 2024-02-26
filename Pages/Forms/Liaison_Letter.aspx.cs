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
           headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }

    }

    public void LoadData()
    {
        var VisitDate = DateTime.Parse(visitDate_tb.Text);
        var VArrivalTime = DateTime.Parse(Schedule.GetVolArrivalTime(VisitData.LoadVisitInfoFromDate(VisitDate.ToString(), "vTrainingTime").ToString()).ToString());
        string SchoolName = schoolName_ddl.SelectedValue;
        string LiaisonName;
        var DismissalTime = DateTime.Parse(Schedule.GetDismissalTime(VArrivalTime.ToString()).ToString());

        //Reveal divs
        print_btn.Visible = true;
        letter_div.Visible = true;

        // Get volunteer count, training time, and reply by
        //VMin = SchoolData.GetVolunteerRange(VisitDate, SchoolID).VMin;
        //VMax = SchoolData.GetVolunteerRange(VisitDate, SchoolID).VMax;

        // Get liaison information
        LiaisonName = SchoolData.LoadSchoolInfoFromSchool(SchoolName, "liaisonName").ToString();
        
        // Assign labels
        schoolName_lbl.Text = SchoolName;
        visitDate_lbl.Text = VisitDate.ToString("D");
        liaison_lbl.Text = LiaisonName;
        arrivalTime_lbl.Text = VArrivalTime.ToString("t");
        volunteerDismisal_lbl.Text = DismissalTime.ToString("t");

    }



    protected void visitDate_tb_TextChanged(object sender, EventArgs e)
    {
        if (visitDate_tb.Text != "")
        {
            //Reveal school DDL
            schoolName_ddl.Visible = true;
            school_p.Visible = true;

            //Load school name DDL
            SchoolData.LoadVisitingSchoolsDDL(visitDate_tb.Text, schoolName_ddl);
        }
        else
        {
            info_div.Visible = false;
            print_btn.Visible = false;
        }
    }

    protected void schoolName_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (schoolName_ddl.SelectedIndex != 0) 
        {
            LoadData();        
        }
    }

    protected void print_btn_Click(object sender, EventArgs e)
    {
        FPLogo_img.Visible = true;
        Page.ClientScript.RegisterStartupScript(GetType(), "Print", "window.print();", true);
    }
}