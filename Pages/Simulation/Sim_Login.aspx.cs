using Antlr.Runtime;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sim_Login : System.Web.UI.Page
{
    private Class_VisitData VisitData = new Class_VisitData();
    private Class_SchoolData SchoolData = new Class_SchoolData();
    private Class_TeacherData TeacherData = new Class_TeacherData();
    private Class_StudentData StudentData = new Class_StudentData();
    private Class_BusinessData Businesses = new Class_BusinessData();
    public int VisitID;

    public Sim_Login()
    {
        VisitID = VisitData.GetVisitID();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //Check for visit ID
            if (VisitData.GetVisitID().ToString() != "")
            {
                //Load schools in DDL from today's date
                SchoolData.LoadVisitDateSchoolsDDL(DateTime.Now.ToShortDateString(), ddlSchoolName);

                //Load sponsors in DDL
                Businesses.LoadSponsorDDL(ddlSponsor);

            }
            else
            {
                lblError.Text = "No simulation active for today!";
                return;
            }
        }
        
    }

    protected void Enter()
    {
        int AcctNum;       

        //Check if all fields are entered
        if (tbAcctNum.Text == "")
        {
            lblError.Text = "Please enter an account number.";
            return;
        }
        else if (tbFirstName.Text == "")
        {
            lblError.Text = "Please enter your first name.";
            return;
        }
        else if (tbLastName.Text == "")
        {
            lblError.Text = "Please enter your last name.";
            return;
        }
        else if (ddlSchoolName.SelectedIndex == 0)
        {
            lblError.Text = "Please select your school.";
            return;
        }

        //Assign AcctNum variable
        AcctNum = int.Parse(tbAcctNum.Text);

        //Check if account number is between 10001-10172
        if (AcctNum < 10000 || AcctNum > 10173)
        {
            lblError.Text = "Account number is not a valid number. Please enter your account number found on your sheet.";
            return;
        }

        //Open popup
        Page.ClientScript.RegisterStartupScript(GetType(), "Popup", "toggle();", true);
    }

    protected void Login()
    {
        int AcctNum = int.Parse(tbAcctNum.Text);
        int PIN = StudentData.GetPIN(AcctNum);
        int SchoolID = SchoolData.GetSchoolID(ddlSchoolName.SelectedValue);
        string[] TeacherName = ddlTeacherName.SelectedValue.Split(' ');
        int TeacherID = TeacherData.GetTeacherIDFromName(TeacherName[0], TeacherName[1]);
        int Grade = int.Parse(ddlGrade.SelectedValue);
        int SponsorID = Businesses.GetSponsorID(ddlSponsor.SelectedValue);
        

        //Check if PIN matches account number
        if (int.Parse(tbPin.Text) != PIN)
        {
            lblError.Text = "PIN entered does not match account number's associated PIN.";
            return;
        }

        //Check if account number already exists for current visit
        if (StudentData.AcctNumExists(VisitID, AcctNum) == true)
        {
            //Check if first and last name matches account number
            if (StudentData.StudentExists(VisitID, AcctNum, tbFirstName.Text, tbLastName.Text) != true)
            {
                lblError.Text = "Please use a different account number to log in, or type the first and last name of the student with this account number to log in.";
                return;
            }      
        }

        //Student does NOT exist
        else
        {
            //Insert new student into DB
            //try
            //{
                StudentData.NewStudent(VisitID, AcctNum, tbFirstName.Text, tbLastName.Text, ddlGender.SelectedValue, SchoolID, TeacherID, Grade, SponsorID);
            //}
            //catch
            //{
            //    lblError.Text = "Error in Login. Could not insert new student into the database. Please see a teacher for help.";
            //    return;
            //}
        }

        //Redirect to lifestyle questions, add student ID to link
        Response.Redirect("Sim_Life_Style.aspx?b=" + StudentData.GetStudentID(VisitID, AcctNum));
    }           



    protected void btnReset_Click(object sender, EventArgs e)
    {
        //Refresh page
        Response.Redirect("Sim_Login.aspx");
    }

    protected void btnEnter_Click(object sender, EventArgs e)
    {
        Enter();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Refresh page
        Response.Redirect("Sim_Login.aspx");
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Login();
    }



    protected void ddlSchoolName_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Get school ID from selected value
        if (ddlSchoolName.SelectedIndex != 0)
        {
            int SchoolID = SchoolData.GetSchoolID(ddlSchoolName.SelectedValue);

            //Load teachers name
            TeacherData.LoadTeacherNameDDLFromSchoolID(SchoolID, ddlTeacherName);
        }

    }
}