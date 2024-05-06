using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sim_Savings : System.Web.UI.Page
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
    private Class_StudentData Students = new Class_StudentData();
    private Class_Simulation Sim = new Class_Simulation();
    private int VisitID;
    private int StudentID;
    private int AcctNum;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Get current visit ID and student ID
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

    protected void LoadData(int StudentID)
    {
        var Student = Students.StudentLookup(VisitID, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);
        double NMI = Student.NMI;
        double HouseNMI = NMI;

        //Clear error
        lblError.Text = "";

        //Check if married
        if (Persona.MarriageStatus == "Married")
        {
            HouseNMI = NMI * 2;
        }

        //Check for percent buttons, update text
        if (btnSaveNMI5.Enabled == false)
        {
            btnSaveNMI5.Text = (NMI * 0.05).ToString("c");
        }
        if (btnSaveNMI10.Enabled == false)
        {
            btnSaveNMI10.Text = (NMI * 0.10).ToString("c");
        }
        if (btnSaveNMI15.Enabled == false)
        {
            btnSaveNMI15.Text = (NMI * 0.15).ToString("c");
        }

        //Load data in labels
        lblExistingRetirement.Text = Persona.LongSavings.ToString("c");
        lblExistingEmer.Text = Persona.EmergFunds.ToString("c");
        lblExistingOther.Text = Persona.OtherSavings.ToString("c");
        lblHouseNMI.Text = HouseNMI.ToString("c");

    }

    protected void TotalSavingCalc()
    {
        double TSavings = double.Parse(tbSavingsTotal.Text);
        double TotalSavings = 0;
        double Retire = 0;
        double Emerg = 0;
        double Other = 0;

        //Check if textboxes are blank
        if (tbSaveRetire.Text != "")
        {
            Retire = double.Parse(tbSaveRetire.Text);
        }
        if (tbSaveEmerg.Text != "")
        {
            Emerg = double.Parse(tbSaveEmerg.Text);
        }
        if (tbSaveOther.Text != "")
        {
            Other = double.Parse(tbSaveOther.Text);
        }

        //Calculate total savings
        TotalSavings = Retire + Emerg + Other;

        //Assign total savings to label
        lblSavingsTotal.Text = TotalSavings.ToString("c");

        //Check if total savings matches total savings entered in textbox
        if (TotalSavings == int.Parse(tbSavingsTotal.Text))
        {
            //Change label to green
            lblSavingsTotal.ForeColor = Color.Green;

            //Make next button visible
            btnNext.Visible = true;
        }
        else if (TotalSavings >  int.Parse(tbSavingsTotal.Text))
        {
            //Change label to red
            lblSavingsTotal.ForeColor = Color.Red;

            //Make next button invisible
            btnNext.Visible = false;
        }
        else if (TotalSavings < int.Parse(tbSavingsTotal.Text))
        {
            //Change label to black
            lblSavingsTotal.ForeColor = Color.Black;

            //Make next button invisible
            btnNext.Visible = false;
        }
    }



    protected void btnNext_Click(object sender, EventArgs e)
    {
        //Redirect to research
        Response.Redirect("Sim_Research.aspx?b=" + StudentID);
    }

    protected void btnEnter_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Automatically closes the popup
    }

    protected void btnSaveNMI5_Click(object sender, EventArgs e)
    {
        //Make button disabled
        btnSaveNMI5.Enabled = false;

        //Load data
        LoadData(StudentID);
    }

    protected void btnSaveNMI10_Click(object sender, EventArgs e)
    {
        //Make button disabled
        btnSaveNMI10.Enabled = false;

        //Load data
        LoadData(StudentID);
    }

    protected void btnSaveNMI15_Click(object sender, EventArgs e)
    {
        //Make button disabled
        btnSaveNMI15.Enabled = false;

        //Load data
        LoadData(StudentID);
    }



    protected void tbSaveRetire_TextChanged(object sender, EventArgs e)
    {
        TotalSavingCalc();
    }

    protected void tbSaveEmerg_TextChanged(object sender, EventArgs e)
    {
        TotalSavingCalc();
    }

    protected void tbSaveOther_TextChanged(object sender, EventArgs e)
    {
        TotalSavingCalc();
    }
}