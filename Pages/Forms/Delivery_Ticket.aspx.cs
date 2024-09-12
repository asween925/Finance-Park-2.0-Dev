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
    private Class_SVC SVC = new Class_SVC();
    private int VisitID;
    private string URL = HttpContext.Current.Request.Url.ToString();

    public Delivery_Ticket()
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
            // Assign current visit ID to hidden field
            if (VisitID != 0)
            {
                hfCurrentVisitID.Value = VisitID.ToString();
            }

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //if coming from SVC, automatically load the visit date and school name
            if (URL.Contains("b=")) {

                //get Date from visit id
                string VisitID = Request["b"];

                //Assign visit date to text box
                tbVisitDate.Text = DateTime.Parse(VisitData.GetVisitDateFromID(int.Parse(VisitID)).ToString()).ToString("yyyy-MM-dd");

                //Load data
                LoadData();

                //Get school name from id
                string SchoolID = Request["c"];

                //Assign school to ddl
                ddlSchoolName.SelectedValue = SchoolData.GetSchoolNameFromID(SchoolID).ToString();

                //Load data again
                LoadData();
            }
        }
    }

    public void LoadData()
    {      
        DateTime VisitDate = DateTime.Parse(tbVisitDate.Text);
        int VisitID = int.Parse(VisitData.GetVisitIDFromDate(VisitDate.ToString()).ToString());
        string SchoolName = ddlSchoolName.SelectedValue;
        string TeacherName = ddlTeacherName.SelectedValue;
        int SchoolID = SchoolData.GetSchoolID(SchoolName);
        string ContactTeacher = TeacherData.GetContactTeacher(SchoolData.GetSchoolID(SchoolName).ToString()).ToString();
        string StudentCount = VisitData.LoadVisitInfoFromDate(VisitDate.ToString(), "studentCount").ToString();
        string Workbooks = SVC.GetWorkbooks(VisitID, SchoolID).ToString();
        string Kits = SVC.GetKitNumbersString(VisitID, SchoolID).ToString();
        string Email = "";

        //Load public, private, or kits only
        if (ddlLetterType.SelectedValue == "Public" || ddlLetterType.SelectedValue == "Private")
        {
            divPub.Visible = true;
            divKit.Visible = false;
        }
        else if (ddlLetterType.SelectedValue == "Kits Only")
        {
            divPub.Visible = false;
            divKit.Visible = true;
        }

        //If teacher name is not blank, assign first and last name and get email
        if (ddlTeacherName.SelectedValue != "")
        {
            string TeacherFirst = TeacherName.Split(' ')[0];
            string TeacherLast = TeacherName.Split(' ')[1];
            Email = TeacherData.GetTeacherEmail(Int16.Parse(TeacherData.GetTeacherIDFromName(TeacherFirst, TeacherLast).ToString())).ToString();
        }
      
        //Assign labels
        lblSchoolName.Text = SchoolName;
        lblContact.Text = ContactTeacher;
        lblTeacherName.Text = TeacherName;
        lblBooks.Text = Workbooks;
        lblKits.Text = Kits;
    }



    protected void tbVisitDate_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDate.Text != "")
        {
            //Check if date is a scheduled visit
            if (VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString() != "")
            {
                //Make school name div visible
                divSchoolName.Visible = true;

                //Load School name DDL
                SchoolData.LoadVisitingSchoolsDDL(tbVisitDate.Text, ddlSchoolName);

                //Load Data
                LoadData();
            }
            else
            {
                lblError.Text = "Date entered is not scheduled.";
                return;
            }

        }
    }

    protected void ddlSchoolName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchoolName.SelectedIndex != 0)
        {
            //Make teacher name div visible
            divTeacherName.Visible = true;

            //Clear teacher ddl
            ddlTeacherName.Items.Clear();

            //Load teacher name ddl
            TeacherData.LoadTeacherNamesFromVID(int.Parse(SchoolData.GetSchoolID(ddlSchoolName.SelectedValue).ToString()), ddlTeacherName);
            ddlTeacherName.Items.Insert(0, "");

            //Load Data
            LoadData();
        }
    }

    protected void ddlTeacherName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTeacherName.SelectedIndex != 0)
        {
            //make letter and letter div visible
            divLetterType.Visible = true;
            divLetter.Visible = true;

            //Load Data
            LoadData();
        }

    }

    protected void ddlLetterType_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Load data
        LoadData();
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "Print", "print();", true);
    }
}