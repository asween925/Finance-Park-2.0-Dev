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
        string VisitDate = tbVisitDate.Text;
        string SchoolName = ddlSchoolName.SelectedValue;
        string TeacherName = ddlTeacherName.SelectedValue;
        string VisitTime;
        string DueBy;

        //Clear error
        lblError.Text = "";

        //Check if visit date exists
        if (VisitData.GetVisitIDFromDate(VisitDate).ToString() == "0")
        {
            lblError.Text = "No visit scheduled for selected date.";
            return;
        }

        //Assign VisitTime and DueBy dates
        VisitTime = VisitData.LoadVisitInfoFromDate(VisitDate, "visitTime").ToString();
        DueBy = VisitData.LoadVisitInfoFromDate(VisitDate, "dueBy").ToString();

        //Make all inivisible
        ulGenPub.Visible = false;
        ulGenPri.Visible = false;
        ulGenDay.Visible = false;
        ulVolPub.Visible = false;
        ulVolPri.Visible = false;
        ulVolDay.Visible = false;
        divTran.Visible = false;
        ulTransportPub.Visible = false;
        ulTransportPri.Visible = false;
        ulLunchPub.Visible = false;
        ulLunchPri.Visible = false;
        ulLunchHome.Visible = false;

        //Check if letter type is public or private and make it visible
        if (ddlLetterType.SelectedValue == "Public")
        {
            ulGenPub.Visible = true;
            ulVolPub.Visible = true;
            ulLunchPub.Visible = true;
            divTran.Visible = true;
            ulTransportPub.Visible = true;
        }
        else if (ddlLetterType.SelectedValue == "Private")
        {
            ulGenPri.Visible = true;
            ulVolPri.Visible = true;
            ulLunchPri.Visible = true;
            divTran.Visible = true;
            ulTransportPri.Visible = true;
            divPaymentPri.Visible = true;     
        }
        else if (ddlLetterType.SelectedValue == "Home Schooled")
        {
            ulGenPri.Visible = true;
            ulVolPri.Visible = true;
            divPaymentPri.Visible = true;
            ulLunchPri.Visible = true;
        }
        else
        {
            ulGenDay.Visible = true;
            ulVolDay.Visible = true;
            divTran.Visible = true;
            ulTransportPub.Visible = true;
            ulLunchPub.Visible = true;
        }

        //Load visit info
        lblSchoolName.Text = SchoolName;
        lblTeacherName.Text = TeacherName;
        lblVisitDate.Text = DateTime.Parse(VisitDate).ToString("d");
        lblNumOfStub.Text = VisitData.LoadVisitInfoFromDate(VisitDate, "studentCount").ToString();
        lblNumOfVol.Text = VisitData.LoadVisitInfoFromDate(VisitDate, "vMaxCount").ToString();
        lblDueBy.Text = DateTime.Parse(DueBy).ToString("d");
        lblDueByLetter.Text = DateTime.Parse(DueBy).ToString("d");
        lblDueByLetterPri.Text = DateTime.Parse(DueBy).ToString("d");
        lblDueByLetter2Pub.Text = DateTime.Parse(DueBy).ToString("d");
        lblDueByLetterDay.Text = DateTime.Parse(DueBy).ToString("d");
        lblVolArrive.Text = DateTime.Parse(SchoolSchedule.GetVolArrivalTime(VisitTime).ToString()).ToString("t");
        lblStuArrive.Text = DateTime.Parse(SchoolSchedule.GetArrivalTime(VisitTime).ToString()).ToString("t") + " /";
        lblStuDismiss.Text = DateTime.Parse(SchoolSchedule.GetDismissalTime(VisitTime).ToString()).ToString("t");
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
            TeacherData.LoadTeacherNamesFromVID(Int16.Parse(SchoolData.GetSchoolID(ddlSchoolName.SelectedValue).ToString()), ddlTeacherName);
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