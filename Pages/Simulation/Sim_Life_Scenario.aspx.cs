using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sim_Life_Scenario: System.Web.UI.Page
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
    private Class_StudentData Students = new Class_StudentData();
    private Class_JobData Jobs = new Class_JobData();
    private int VisitID;
    private int StudentID;
    private int AcctNum;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Get visit ID and student ID
        VisitID = VisitData.GetVisitID();
        
        //Check if student id is passed through
        if (Request["b"] != null)
        {
            StudentID = int.Parse(Request["b"]);

            //Get account number
            var Student = Students.StudentLookup(VisitID, StudentID);
            AcctNum = Student.AccountNumber;

            //Load Data
            LoadData(StudentID);
        }       
    }

    public void LoadData(int StudentID)
    {
        var Student = Students.StudentLookup(VisitID, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);
        var Job = Jobs.JobLookup(Persona.JobID);
        int PersonaID = Student.PersonaID;
        
        //Load top section (name, job title, degree, age, status, children, account number, and PIN)
        lblStudentName.Text = Student.FirstName.ToString() + " " + Student.LastName.ToString();
        lblJobTitle.Text = Persona.JobTitle.ToString();
        lblDegree.Text = Job.EducationBG.ToString();
        lblAge.Text = Persona.Age.ToString() + " years old";
        lblMaritalStatus.Text = Persona.MarriageStatus.ToString();
        lblChildren.Text = "Children: " + Persona.NumOfChild.ToString();
        lblAccountNumber.Text = "Account #: " + Student.AccountNumber.ToString();
        lblPin.Text = "PIN: " + Students.GetPIN(AcctNum).ToString();

        //Load financials
        lblCreditScore.Text = Persona.CreditScore.ToString();
        lblGAI.Text = Persona.GAI.ToString("C");
        lblGMI.Text = Persona.NMI.ToString("C");
        lblEdDebt.Text = Job.EdDebt.ToString("C");
        lblCCDebt.Text = Persona.CCDebt.ToString("C");
        lblRetire.Text = Persona.LongSavings.ToString("C");
        lblEmerFunds.Text = Persona.EmergFunds.ToString("C");
        lblOther.Text = Persona.OtherSavings.ToString("C");

        //Load bottom section
        lblResponse.Text = Job.JobDuties.ToString();
        lblAdvancement.Text = Job.Advancement.ToString();
    }

    protected void btnEnter_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }

}