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
            SchoolData.LoadVisitDateSchoolsDDL(DateTime.Now.ToShortDateString(), schoolName_ddl);
        }
        else
        {
            error_lbl.Text = "No simulation active for today!";
            return;
        }
    }

    protected void reset_btn_Click(object sender, EventArgs e)
    {
        //Refresh page
        Response.Redirect("Sim_Login.aspx");
    }

    protected void enter_btn_Click(object sender, EventArgs e)
    {       
        //Check if PIN matches account number


        //If not, call toggle and display error

        //If match, insert new student into student info

        //When new student has been added into the DB, redirect to lifestyle questions

    }

    protected void cancel_btn_Click(object sender, EventArgs e)
    {
        //Refresh page
        Response.Redirect("Sim_Login.aspx");
    }

    protected void schoolName_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {       
        //Get school ID from selected value
        if (schoolName_ddl.SelectedIndex != 0 )
        {
            int SchoolID = (Int16.Parse(SchoolData.GetSchoolID(schoolName_ddl.SelectedValue).ToString()));

            //Load teachers name
            TeacherData.LoadTeacherNamesFromVID(VisitID, SchoolID, teacher_ddl);
        }
        
    }

    protected void login_btn_Click(object sender, EventArgs e)
    {
        //Check if all fields are entered
        if (acctNum_tb.Text == "")
        {
            error_lbl.Text = "Please enter an account number.";
            return;
        }
        else if (firstName_tb.Text == "")
        {
            error_lbl.Text = "Please enter your first name.";
            return;
        }
        else if (lastName_tb.Text == "")
        {
            error_lbl.Text = "Please enter your last name.";
            return;
        }
        else if (schoolName_ddl.SelectedIndex == 0)
        {
            error_lbl.Text = "Please select your school.";
            return;
        }
        else if (grade_tb.Text == "")
        {
            error_lbl.Text = "Please enter your grade.";
            return;
        }

        //Open popup
        Page.ClientScript.RegisterStartupScript(GetType(), "Popup", "toggle();", false);
    }
}