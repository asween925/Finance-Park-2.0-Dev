using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Daily_Totals : Page
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

    public Daily_Totals()
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
        }
    }

    public void LoadData()
    {
        string VisitDate = visitDate_tb.Text;
        string SchoolName = schoolName_ddl.SelectedValue;
        string TeacherName = teacherName_ddl.SelectedValue;       
        string StudentCount = VisitData.LoadVisitInfoFromDate(VisitDate, "studentCount").ToString();
        string Email = "";

        //If teacher name is not blank, assign first and last name and get email
        if (teacherName_ddl.SelectedValue != "") 
        {
            string TeacherFirst = TeacherName.Split(' ')[0];
            string TeacherLast = TeacherName.Split(' ')[1];
            Email = TeacherData.GetTeacherEmail(Int16.Parse(TeacherData.GetTeacherIDFromName(TeacherFirst, TeacherLast).ToString())).ToString();
        }

        //Check if private or public
        if (letterType_ddl.SelectedValue == "Public")
        {
            pri1_p.Visible = false;
            pri2_p.Visible = false;
            pri_div.Visible = false;
        }
        else
        {
            pri1_p.Visible = true;
            pri2_p.Visible = true;
            pri_div.Visible = true;
        }

        //Assign labels
        visitDate_lbl.Text = DateTime.Parse(VisitDate).ToString("d");
        studentCount_lbl.Text = StudentCount;
        schoolName_lbl.Text = SchoolName;
        schoolNamePri1_lbl.Text = SchoolName;
        email_lbl.Text = Email;
        
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