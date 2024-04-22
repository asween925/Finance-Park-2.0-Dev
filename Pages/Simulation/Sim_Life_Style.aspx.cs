using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sim_Life_Style : System.Web.UI.Page
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
    private Class_Simulation Sim = new Class_Simulation();
    private int VisitID;
    private int StudentID;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Get visit ID and student ID
        VisitID = VisitData.GetVisitID();

        //Check if student id is passed through
        if (Request["b"] != null)
        {
            StudentID = int.Parse(Request["b"]);
        }

        //Get number of questions and question order
        QButtons(Sim.GetActiveQuestions());

        //Load question 1
        LoadQuestion(int.Parse(hfQuestion.Value));
    }

    protected void LoadQuestion(int Q)
    {
        //Assign varriable for multiple choice options
        var MultiOptions = Sim.GetMultiOptions(Q);

        //Assign question number to hidden field
        hfQuestion.Value = Q.ToString();

        //Clear error
        lblError.Text = "";

        //Make all answers invisible
        divA1.Visible = false;
        divA2.Visible = false;
        divA3.Visible = false;
        divA4.Visible = false;
        divA5.Visible = false;
        divWrite.Visible = false;

        //Check if final question, if so, change next text to Continue
        if (Q == Sim.GetActiveQuestions())
        {
            btnNext.Text = "Continue to Life Scenario";
        }
        else
        {
            btnNext.Text = "Next Question";
        }

        //Highlight question number button
        switch (Q)
        {
            case 1:
                ClearAttributes();
                btnQ1.Attributes.Add("style", "background-color: darkgreen; text-decoration: underline; font-weight: bold;");
                break;
            case 2:
                ClearAttributes();
                btnQ2.Attributes.Add("style", "background-color: darkgreen; text-decoration: underline; font-weight: bold;");
                break;
            case 3:
                ClearAttributes();
                btnQ3.Attributes.Add("style", "background-color: darkgreen; text-decoration: underline; font-weight: bold;");
                break;
            case 4:
                ClearAttributes();
                btnQ4.Attributes.Add("style", "background-color: darkgreen; text-decoration: underline; font-weight: bold;");
                break;
            case 5:
                ClearAttributes();
                btnQ5.Attributes.Add("style", "background-color: darkgreen; text-decoration: underline; font-weight: bold;");
                break;
            case 6:
                ClearAttributes();
                btnQ6.Attributes.Add("style", "background-color: darkgreen; text-decoration: underline; font-weight: bold;");
                break;
            case 7:
                ClearAttributes();
                btnQ7.Attributes.Add("style", "background-color: darkgreen; text-decoration: underline; font-weight: bold;");
                break;
            case 8:
                ClearAttributes();
                btnQ8.Attributes.Add("style", "background-color: darkgreen; text-decoration: underline; font-weight: bold;");
                break;
            case 9:
                ClearAttributes();
                btnQ9.Attributes.Add("style", "background-color: darkgreen; text-decoration: underline; font-weight: bold;");
                break;
            case 10:
                ClearAttributes();
                btnQ10.Attributes.Add("style", "background-color: darkgreen; text-decoration: underline; font-weight: bold;");
                break;
        }

        //Get question name
        lblQ.Text = "Question " + Q + ": " + Sim.GetQuestionName(Q);

        //Get answer type (multi choice or written)
        if (Sim.GetAnswerType(Q).ToString() == "Multiple Choice")
        {
            //Get options and text for answers
            switch (MultiOptions.Options)
            {
                case 1:
                    //Assign labels for answer text
                    lblA1.Text = MultiOptions.O1.ToString();

                    //Make number of answers visible
                    divA1.Visible = true;
                    break;
                case 2:
                    //Assign labels for answer text
                    lblA1.Text = MultiOptions.O1.ToString();
                    lblA2.Text = MultiOptions.O2.ToString();

                    //Make number of answers visible
                    divA1.Visible = true;
                    divA2.Visible = true;
                    break;
                case 3:
                    //Assign labels for answer text
                    lblA1.Text = MultiOptions.O1.ToString();
                    lblA2.Text = MultiOptions.O2.ToString();
                    lblA3.Text = MultiOptions.O3.ToString();

                    //Make number of answers visible
                    divA1.Visible = true;
                    divA2.Visible = true;
                    divA3.Visible = true;
                    break;
                case 4:
                    //Assign labels for answer text
                    lblA1.Text = MultiOptions.O1.ToString();
                    lblA2.Text = MultiOptions.O2.ToString();
                    lblA3.Text = MultiOptions.O3.ToString();
                    lblA4.Text = MultiOptions.O4.ToString();

                    //Make number of answers visible
                    divA1.Visible = true;
                    divA2.Visible = true;
                    divA3.Visible = true;
                    divA4.Visible = true;
                    break;
                case 5:
                    //Assign labels for answer text
                    lblA1.Text = MultiOptions.O1.ToString();
                    lblA2.Text = MultiOptions.O2.ToString();
                    lblA3.Text = MultiOptions.O3.ToString();
                    lblA4.Text = MultiOptions.O4.ToString();
                    lblA5.Text = MultiOptions.O5.ToString();

                    //Make number of answers visible
                    divA1.Visible = true;
                    divA2.Visible = true;
                    divA3.Visible = true;
                    divA4.Visible = true;
                    divA5.Visible = true;
                    break;
            }
        }
        else
        {
            //Make written response field visible
            divWrite.Visible = true;
        } 
    }

    protected void QButtons(int Q)
    {
        //Make buttons invisible
        btnQ2.Visible = false;
        btnQ3.Visible = false;
        btnQ4.Visible = false;
        btnQ5.Visible = false;
        btnQ6.Visible = false;
        btnQ7.Visible = false;
        btnQ8.Visible = false;
        btnQ9.Visible = false;
        btnQ10.Visible = false;

        switch (Q)
        {
            case 1:
                btnQ1.Visible = true;
                break;
            case 2:
                btnQ1.Visible = true;
                btnQ2.Visible = true;
                break;
            case 3:
                btnQ1.Visible = true;
                btnQ2.Visible = true;
                btnQ3.Visible = true;
                break;
            case 4:
                btnQ1.Visible = true;
                btnQ2.Visible = true;
                btnQ3.Visible = true;
                btnQ4.Visible = true;
                break;
            case 5:
                btnQ1.Visible = true;
                btnQ2.Visible = true;
                btnQ3.Visible = true;
                btnQ4.Visible = true;
                btnQ5.Visible = true;
                break;
            case 6:
                btnQ1.Visible = true;
                btnQ2.Visible = true;
                btnQ3.Visible = true;
                btnQ4.Visible = true;
                btnQ5.Visible = true;
                btnQ6.Visible = true;
                break;
            case 7:
                btnQ1.Visible = true;
                btnQ2.Visible = true;
                btnQ3.Visible = true;
                btnQ4.Visible = true;
                btnQ5.Visible = true;
                btnQ6.Visible = true;
                btnQ7.Visible = true;
                break;
            case 8:
                btnQ1.Visible = true;
                btnQ2.Visible = true;
                btnQ3.Visible = true;
                btnQ4.Visible = true;
                btnQ5.Visible = true;
                btnQ6.Visible = true;
                btnQ7.Visible = true;
                btnQ8.Visible = true;
                break;
            case 9:
                btnQ1.Visible = true;
                btnQ2.Visible = true;
                btnQ3.Visible = true;
                btnQ4.Visible = true;
                btnQ5.Visible = true;
                btnQ6.Visible = true;
                btnQ7.Visible = true;
                btnQ8.Visible = true;
                btnQ9.Visible = true;
                break;
            case 10:
                btnQ1.Visible = true;
                btnQ2.Visible = true;
                btnQ3.Visible = true;
                btnQ4.Visible = true;
                btnQ5.Visible = true;
                btnQ6.Visible = true;
                btnQ7.Visible = true;
                btnQ8.Visible = true;
                btnQ9.Visible = true;
                btnQ10.Visible = true;
                break;
        }
    }

    protected void ClearAttributes()
    {
        btnQ1.Attributes.Clear();
        btnQ2.Attributes.Clear();
        btnQ3.Attributes.Clear();
        btnQ4.Attributes.Clear();
        btnQ5.Attributes.Clear();
        btnQ6.Attributes.Clear();
        btnQ7.Attributes.Clear();
        btnQ8.Attributes.Clear();
        btnQ9.Attributes.Clear();
        btnQ10.Attributes.Clear();
    }

    protected void CheckAnswers(int Q)
    {
        //Check answer type
        //If multi choice, check if a rdo is checked
        if (Sim.GetAnswerType(Q).ToString() == "Multiple Choice")
        {
            if (rdoA1.Checked == false && rdoA2.Checked == false && rdoA3.Checked == false && rdoA4.Checked == false && rdoA5.Checked == false)
            {
                lblError.Text = "Please select an answer before continuing.";
                return;
            }
        }

        //If written response, check if blank or character count is higher than 200
        else
        {
            if (tbWrittenResponse.Text == "")
            {
                lblError.Text = "Please enter an answer before continuing.";
                return;
            }
            else if (tbWrittenResponse.Text.Length > 200)
            {
                lblError.Text = "Word count cannot be more than 200.";
                return;
            }
        }
    }

    protected void Next(int Q)
    {
        string Answer = "";
        
        //Check if answered
        CheckAnswers(Q);

        //Check if multiple choice
        if (Sim.GetAnswerType(Q).ToString() == "Multiple Choice")
        {
            if (rdoA1.Checked == true)
            {
                Answer = "1";
            }
            else if (rdoA2.Checked == true)
            {
                Answer = "2";
            }
            else if (rdoA3.Checked == true)
            {
                Answer = "3";
            }
            else if (rdoA4.Checked == true)
            {
                Answer = "4";
            }
            else
            {
                Answer = "5";
            }
        }
        else
        {
            Answer = tbWrittenResponse.Text;
        }

        //First check if row is in database
        if (Sim.CheckAnswersDB(VisitID, StudentID) == true)
        {
            //Update answer row
            Sim.UpdateLifestyleAnswer(VisitID, StudentID, Answer, Q);
        }
        else
        {
            //Update answer row
            Sim.InsertLifestyleAnswer(VisitID, StudentID, Answer);
        }

        //Load next question OR go to the life scenario page
        if (Q == Sim.GetActiveQuestions())
        {
            Response.Redirect("Sim_Life_Scenario.aspx?b=" + StudentID);
        }
        else
        {
            LoadQuestion(Q + 1);    
        }

        //Clear radio button and textbox selections
        rdoA1.Checked = false;
        rdoA2.Checked = false;
        rdoA3.Checked = false;
        rdoA4.Checked = false;
        rdoA5.Checked = false;
        tbWrittenResponse.Text = "";
    }



    protected void btnNext_Click(object sender, EventArgs e)
    {
        Next(int.Parse(hfQuestion.Value));
    }

    protected void btnQ1_Click(object sender, EventArgs e)
    {
        LoadQuestion(1);
    }

    protected void btnQ2_Click(object sender, EventArgs e)
    {
        LoadQuestion(2);
    }

    protected void btnQ3_Click(object sender, EventArgs e)
    {
        LoadQuestion(3);
    }

    protected void btnQ4_Click(object sender, EventArgs e)
    {
        LoadQuestion(4);
    }

    protected void btnQ5_Click(object sender, EventArgs e)
    {
        LoadQuestion(5);
    }

    protected void btnQ6_Click(object sender, EventArgs e)
    {
        LoadQuestion(6);
    }

    protected void btnQ7_Click(object sender, EventArgs e)
    {
        LoadQuestion(7);
    }

    protected void btnQ8_Click(object sender, EventArgs e)
    {
        LoadQuestion(8);
    }

    protected void btnQ9_Click(object sender, EventArgs e)
    {
        LoadQuestion(9);
    }

    protected void btnQ10_Click(object sender, EventArgs e)
    {
        LoadQuestion(10);
    }
}