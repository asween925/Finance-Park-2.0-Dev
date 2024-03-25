using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Delivery_Ticket : Page
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
    private int VisitID;
    private string URL = HttpContext.Current.Request.Url.ToString();

    public Delivery_Ticket()
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
            // Assign current visit ID to hidden field
            if (VisitID != 0)
            {
                currentVisitID_hf.Value = VisitID.ToString();
            }

            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //if coming from SVC, automatically load the visit date and school name
            if (URL.Contains("b=")) {

                //get Date from visit id
                string VisitID = Request["b"];

                //Assign visit date to text box
                visitDate_tb.Text = DateTime.Parse(VisitData.GetVisitDateFromID(VisitID).ToString()).ToString("yyyy-MM-dd");

                //Load data
                LoadData();

                //Get school name from id
                string SchoolID = Request["c"];

                //Assign school to ddl
                schoolName_ddl.SelectedValue = SchoolData.GetSchoolNameFromID(SchoolID).ToString();

                //Load data again
                LoadData();
            }
        }
    }

    public void LoadData()
    {
        string VisitDate = visitDate_tb.Text;
        string SchoolName = schoolName_ddl.SelectedValue;
        string TeacherName = teacherName_ddl.SelectedValue;
        string ContactTeacher = TeacherData.GetContactTeacher(SchoolData.GetSchoolID(SchoolName).ToString()).ToString();
        string StudentCount = VisitData.LoadVisitInfoFromDate(VisitDate, "studentCount").ToString();
        string Email = "";

        //Load public, private, or kits only
        if (letterType_ddl.SelectedValue == "Public" || letterType_ddl.SelectedValue == "Private")
        {
            pub_div.Visible = true;
            kit_div.Visible = false;
        }
        else if (letterType_ddl.SelectedValue == "Kits Only")
        {
            pub_div.Visible = false;
            kit_div.Visible = true;
        }

        //If teacher name is not blank, assign first and last name and get email
        if (teacherName_ddl.SelectedValue != "")
        {
            string TeacherFirst = TeacherName.Split(' ')[0];
            string TeacherLast = TeacherName.Split(' ')[1];
            Email = TeacherData.GetTeacherEmail(Int16.Parse(TeacherData.GetTeacherIDFromName(TeacherFirst, TeacherLast).ToString())).ToString();
        }
      
        //Assign labels
        schoolName_lbl.Text = SchoolName;
        contact_lbl.Text = ContactTeacher;
        teacherName_lbl.Text = TeacherName;
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