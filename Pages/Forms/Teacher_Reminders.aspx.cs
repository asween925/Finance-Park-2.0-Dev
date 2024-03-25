using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Teacher_Reminders : Page
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
    private Class_TeacherData TeacherData = new Class_TeacherData();
    private Class_SchoolSchedule SchoolSchedule = new Class_SchoolSchedule();
    private int VisitID;

    public Teacher_Reminders()
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
            
            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void LoadData()
    {
        string VisitDate = visitDate_tb.Text;
        string SchoolName = schoolName_ddl.SelectedValue;
        string TeacherName = teacherName_ddl.SelectedValue;
        string VisitTime;
        string DueBy;

        //Clear error
        error_lbl.Text = "";

        //Check if visit date exists
        if (VisitData.GetVisitIDFromDate(VisitDate).ToString() == "0")
        {
            error_lbl.Text = "No visit scheduled for selected date.";
            return;
        }

        //Assign VisitTime and DueBy dates
        VisitTime = VisitData.LoadVisitInfoFromDate(VisitDate, "visitTime").ToString();
        DueBy = VisitData.LoadVisitInfoFromDate(VisitDate, "dueBy").ToString();

        //Make all inivisible
        genPub_ul.Visible = false;
        genPri_ul.Visible = false;
        genDay_ul.Visible = false;
        volPub_ul.Visible = false;
        volPri_ul.Visible = false;
        volDay_ul.Visible = false;
        tran_div.Visible = false;
        transportPub_ul.Visible = false;
        transportPri_ul.Visible = false;
        lunchPub_ul.Visible = false;
        lunchPri_ul.Visible = false;
        lunchHome_ul.Visible = false;

        //Check if letter type is public or private and make it visible
        if (letterType_ddl.SelectedValue == "Public")
        {
            genPub_ul.Visible = true;
            volPub_ul.Visible = true;
            lunchPub_ul.Visible = true;
            tran_div.Visible = true;
            transportPub_ul.Visible = true;
        }
        else if (letterType_ddl.SelectedValue == "Private")
        {
            genPri_ul.Visible = true;
            volPri_ul.Visible = true;
            lunchPri_ul.Visible = true;
            tran_div.Visible = true;
            transportPri_ul.Visible = true;
            paymentPri_div.Visible = true;     
        }
        else if (letterType_ddl.SelectedValue == "Home Schooled")
        {
            genPri_ul.Visible = true;
            volPri_ul.Visible = true;
            paymentPri_div.Visible = true;
            lunchPri_ul.Visible = true;
        }
        else
        {
            genDay_ul.Visible = true;
            volDay_ul.Visible = true;
            tran_div.Visible = true;
            transportPub_ul.Visible = true;
            lunchPub_ul.Visible = true;
        }

        //Load visit info
        schoolName_lbl.Text = SchoolName;
        teacherName_lbl.Text = TeacherName;
        visitDate_lbl.Text = DateTime.Parse(VisitDate).ToString("d");
        numOfStud_lbl.Text = VisitData.LoadVisitInfoFromDate(VisitDate, "studentCount").ToString();
        numOfVol_lbl.Text = VisitData.LoadVisitInfoFromDate(VisitDate, "vMaxCount").ToString();
        dueBy_lbl.Text = DateTime.Parse(DueBy).ToString("d");
        dueByLetter_lbl.Text = DateTime.Parse(DueBy).ToString("d");
        dueByLetterPri_lbl.Text = DateTime.Parse(DueBy).ToString("d");
        dueByLetter2Pub_lbl.Text = DateTime.Parse(DueBy).ToString("d");
        dueByLetterDay_lbl.Text = DateTime.Parse(DueBy).ToString("d");
        volArrive_lbl.Text = DateTime.Parse(SchoolSchedule.GetVolArrivalTime(VisitTime).ToString()).ToString("t");
        stuArrive_lbl.Text = DateTime.Parse(SchoolSchedule.GetArrivalTime(VisitTime).ToString()).ToString("t") + " /";
        stuDismiss_lbl.Text = DateTime.Parse(SchoolSchedule.GetDismissalTime(VisitTime).ToString()).ToString("t");
    }   



    protected void visitDate_tb_TextChanged(object sender, EventArgs e)
    {
        if (visitDate_tb.Text != "")
        {
            //Check if date is a scheduled visit
            if (VisitData.GetVisitIDFromDate(visitDate_tb.Text).ToString() != "")
            {
                //Make school name div visible
                schoolName_div.Visible = true;

                //Load School name DDL
                SchoolData.LoadVisitingSchoolsDDL(visitDate_tb.Text, schoolName_ddl);

                //Load Data
                LoadData();
            }
            else
            {
                error_lbl.Text = "Date entered is not scheduled.";
                return;
            }
            
        }
    }

    protected void schoolName_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (schoolName_ddl.SelectedIndex != 0)
        {
            //Make teacher name div visible
            teacherName_div.Visible = true;

            //Clear teacher ddl
            teacherName_ddl.Items.Clear();

            //Load teacher name ddl
            TeacherData.LoadTeacherNamesFromVID(Int16.Parse(VisitData.GetVisitIDFromDate(visitDate_tb.Text).ToString()), Int16.Parse(SchoolData.GetSchoolID(schoolName_ddl.SelectedValue).ToString()), teacherName_ddl);
            teacherName_ddl.Items.Insert(0, "");
            
            //Load Data
            LoadData();
        }
    }

    protected void teacherName_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (teacherName_ddl.SelectedIndex != 0)
        {
            //make letter and letter div visible
            letterType_div.Visible = true;
            letter_div.Visible = true;

            //Load Data
            LoadData();
        }
        
    }

    protected void letterType_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Load data
        LoadData();
    }

    protected void print_btn_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "Print", "print();", true);
    }
}