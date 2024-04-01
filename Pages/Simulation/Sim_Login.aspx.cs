using Antlr.Runtime;
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
    private int VisitID;
    private int TeacherID;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Check for visit ID
        if (VisitData.GetVisitID().ToString() != "")
        {
            //Load schools in DDL from today's date
            SchoolData.LoadVisitDateSchoolsDDL(DateTime.Now.ToShortDateString(), ddlSchoolName);
        }
        else
        {
            lblError.Text = "No simulation active for today!";
            return;
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        //Refresh page
        Response.Redirect("Sim_Login.aspx");
    }

    protected void btnEnter_Click(object sender, EventArgs e)
    {       
        //Check if PIN matches account number


        //If not, call toggle and display error

        //If match, insert new student into student info

        //When new student has been added into the DB, redirect to lifestyle questions

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Refresh page
        Response.Redirect("Sim_Login.aspx");
    }

    protected void ddlSchoolName_SelectedIndexChanged(object sender, EventArgs e)
    {       
        //Get school ID from selected value
        if (ddlSchoolName.SelectedIndex != 0 )
        {
            int SchoolID = (Int16.Parse(SchoolData.GetSchoolID(ddlSchoolName.SelectedValue).ToString()));

            //Load teachers name
            TeacherData.LoadTeacherNamesFromVID(VisitID, SchoolID, ddlTeacher);
        }
        
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
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
        else if (tbGrade.Text == "")
        {
            lblError.Text = "Please enter your grade.";
            return;
        }

        //Open popup
        Page.ClientScript.RegisterStartupScript(GetType(), "Popup", "toggle();", false);
    }
}