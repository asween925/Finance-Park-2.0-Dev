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
        }
    }

    public void LoadData()
    {
        string VisitDate = tbVisitDate.Text;
        string SchoolName = ddlSchoolName.SelectedValue;
        string TeacherName = ddlTeacherName.SelectedValue;       
        string StudentCount = VisitData.LoadVisitInfoFromDate(VisitDate, "studentCount").ToString();
        string Email = "";

        //If teacher name is not blank, assign first and last name and get email
        if (ddlTeacherName.SelectedValue != "") 
        {
            string TeacherFirst = TeacherName.Split(' ')[0];
            string TeacherLast = TeacherName.Split(' ')[1];
            Email = TeacherData.GetTeacherEmail(Int16.Parse(TeacherData.GetTeacherIDFromName(TeacherFirst, TeacherLast).ToString())).ToString();
        }

        //Check if private or public
        if (ddlLetterType.SelectedValue == "Public")
        {
            pPri1.Visible = false;
            pPri2.Visible = false;
            divPri.Visible = false;
        }
        else
        {
            pPri1.Visible = true;
            pPri2.Visible = true;
            divPri.Visible = true;
        }

        //Assign labels
        lblVisitDate.Text = DateTime.Parse(VisitDate).ToString("d");
        lblStudentCount.Text = StudentCount;
        lblSchoolName.Text = SchoolName;
        lblSchoolNamePri1.Text = SchoolName;
        lblEmail.Text = Email;
        
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
            TeacherData.LoadTeacherNamesFromVID(Int16.Parse(VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString()), Int16.Parse(SchoolData.GetSchoolID(ddlSchoolName.SelectedValue).ToString()), ddlTeacherName);
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