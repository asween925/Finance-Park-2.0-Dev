using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI;
using Microsoft.VisualBasic.CompilerServices;

public partial class Liason_Letter : Page
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

    public Liason_Letter()
    {
        connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
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
        DateTime VisitDate = visitDate_tb.Text;
        DateTime VTrainingTime;
        DateTime ReplyBy;
        string SchoolName = schoolName_ddl.SelectedValue;
        string SchoolID = Conversions.ToString(SchoolData.GetSchoolID(SchoolName));
        string VMin;
        string VMax;
        var LiaisonName = default(string);
        DateTime DismissalTime;

        info_div.Visible = true;
        print_btn.Visible = true;

        // Get volunteer count, training time, and reply by
        VMin = SchoolData.GetVolunteerRange(Conversions.ToString(VisitDate), SchoolID).VMin;
        VMax = SchoolData.GetVolunteerRange(Conversions.ToString(VisitDate), SchoolID).VMax;
        // VMin = VisitData.LoadVisitInfoFromDate(VisitDate, "vMinCount")
        // VMax = VisitData.LoadVisitInfoFromDate(VisitDate, "vMaxCount")
        VTrainingTime = Conversions.ToDate(VisitData.LoadVisitInfoFromDate(Conversions.ToString(VisitDate), "vTrainingTime"));
        ReplyBy = Conversions.ToDate(VisitData.LoadVisitInfoFromDate(Conversions.ToString(VisitDate), "replyBy"));
        DismissalTime = Conversions.ToDate(SchoolScheduleData.GetDismissalTime(Conversions.ToString(VTrainingTime)));

        // Get liaison information
        try
        {
            con.ConnectionString = connection_string;
            con.Open();
            cmd.CommandText = @"SELECT liaisonName
								FROM schoolInfo
								WHERE schoolName='" + schoolName_ddl.SelectedValue + "'";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                LiaisonName = dr["liaisonName"].ToString();

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            error_lbl.Text = "Error in loaddata(). Could not get liaison information.";
            cmd.Dispose();
            con.Close();
            return;
        }

        // Assign labels
        schoolName_lbl.Text = SchoolName;
        visitDate_lbl.Text = VisitDate.ToShortDateString();
        visitDate2_lbl.Text = VisitDate.ToShortDateString();
        volunteerTime_lbl.Text = VTrainingTime.ToString("h:mm");
        replyBy_lbl.Text = ReplyBy.ToShortDateString();
        liaison_lbl.Text = LiaisonName;
        vMinCount_lbl.Text = VMin;
        vMaxCount_lbl.Text = VMax;
        volunteerDismisal_lbl.Text = DismissalTime.ToString("h:mm");

    }

    protected void visitDate_tb_TextChanged(object sender, EventArgs e)
    {
        if (visitDate_tb.Text != default)
        {
            schoolName_ddl.Visible = true;
            school_p.Visible = true;

            SchoolData.LoadVisitDateSchoolsDDL(visitDate_tb.Text, schoolName_ddl);

        }
    }

    protected void schoolName_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (schoolName_ddl.SelectedIndex != 0)
        {
            LoadData();
        }
        else
        {
            info_div.Visible = false;
            print_btn.Visible = false;
        }
    }

    protected void print_btn_Click(object sender, EventArgs e)
    {
        EVLogo_img.Visible = true;
        Page.ClientScript.RegisterStartupScript(GetType(), "Print", "PrintBadges();", true);
    }
}