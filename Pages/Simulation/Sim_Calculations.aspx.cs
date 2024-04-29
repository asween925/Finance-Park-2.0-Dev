using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sim_Calculations : System.Web.UI.Page
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
    private Class_Simulation Sim = new Class_Simulation();
    private Class_SQLCommands SQL = new Class_SQLCommands();
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
            var Student = Students.StudentLookup(20, StudentID);
            AcctNum = Student.AccountNumber;

            //Load Data
            LoadData(StudentID);
        }
    }

    protected void LoadData(int StudentID)
    {
        var Student = Students.StudentLookup(20, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);
        var Trans = Sim.GetTransitionData(1); //the 1 is the step in the DB, im planning to make the steps through the simulation customizable at some point
        int GAI = Convert.ToInt32(Persona.GAI);
        int GMI = GAI / 12;
        double Fed = 0;
        double Med = 0;
        double SS = 0;
        double TotalTaxes = 0;
        int NMI = 0;

        //Reset NMI btn
        btnNMI.Enabled = false;

        //Load data into labels
        lblGAI.Text = GAI.ToString("c");
        lblGMI.Text = GMI.ToString("c");

        //Check for federal taxes invisiblity
        if (btnTaxesFed.Visible == false)
        {
            //invisible
            aPlaceholder.Visible = false;
            tdFed.Visible = true;

            //Check if single or married
            if (Persona.MarriageStatus == "Single")
            {
                Fed = Students.TaxesCalc(GMI, "Single Federal");
            }
            else if (Persona.MarriageStatus == "Married")
            {
                Fed = Students.TaxesCalc(GMI, "Married Federal");
            }           
        }

        //Check for medicare taxes invisiblity
        if (btnTaxesMedicare.Visible == false)
        {
            //invisible
            aPlaceholder.Visible = false;
            tdMed.Visible = true;

            Med = Students.TaxesCalc(GMI, "Medicare");
        }

        //Check for SS taxes invisiblity
        if (btnTaxesSS.Visible == false)
        {
            //invisible
            aPlaceholder.Visible = false;
            tdSS.Visible = true;

            SS = Students.TaxesCalc(GMI, "Social Security");
        }

        //Check if all taxes buttons are invisible
        if (btnTaxesFed.Visible == false && btnTaxesMedicare.Visible == false && btnTaxesSS.Visible == false)
        {
            //Make NMI button enabled
            btnNMI.Enabled = true;
        }

        //Load total taxes and NMI
        TotalTaxes = Fed + Med + SS;
        NMI = GMI - Convert.ToInt32(TotalTaxes);    

        //Assign labels
        lblTaxesFed.Text = "+ " + Fed.ToString("c");
        lblTaxesMedi.Text = "+ " + Med.ToString("c");
        lblTaxesSS.Text = "+ " + SS.ToString("c");
        lblTotalTaxes.Text = TotalTaxes.ToString("c");
        lblGMIPopup.Text = "+ " + GMI.ToString("c");
        lblTotalTaxesPopup.Text = "- " + TotalTaxes.ToString("c");        
        lblSavingsHeader.Text = Trans.HeaderText;
        lblSavingsText.Text = Trans.Text;
        hfNMI.Value = NMI.ToString();   //No 'c' here because its easier for the students to enter the NMI without the dollar sign and comma and decimal and stuff        

        //lblError.Text = (GMI - Convert.ToInt32(TotalTaxes)).ToString();
    }

    protected void CheckNMI()
    {
        var Student = Students.StudentLookup(20, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);
        int NMI = Convert.ToInt32(hfNMI.Value);

        //Check if NMI is correct
        if (tbNMI.Text == hfNMI.Value)
        {
            //Update NMI in studentInfoFP
            SQL.ExecuteSQL("UPDATE studentInfoFP SET nmi='" + NMI + "' WHERE id='" + StudentID + "'");
            
            //Close popup
            Page.ClientScript.RegisterStartupScript(GetType(), "Popup", "toggle();", false);

            //If married, double household NMI and assign to label
            if (Persona.MarriageStatus == "Married")
            {
                lblHouseNMI.Text = (NMI * 2).ToString("c");
            }
            else
            {
                lblHouseNMI.Text = NMI.ToString("c");
            }

            //Change text of button and disable it
            btnNMI.Text = NMI.ToString("c");
            btnNMI.Enabled = false;

            //Success message
            lblError.Text = "Correct! Please tap 'Next' to continue.";

            //Make next button visible
            btnNext.Visible = true;
        }
        else
        {
            //Keep popup open
            Page.ClientScript.RegisterStartupScript(GetType(), "Popup", "toggle();", true);            

            //Show failed message
            lblErrorPopup.Text = "NMI is incorrect. Please calculate the correct NMI amount.";
        }
    }



    protected void btnTaxesFed_Click(object sender, EventArgs e)
    {
        //Make fed taces button invisible
        btnTaxesFed.Visible = false;
        
        //Load Taxes
        LoadData(StudentID);

    }

    protected void btnTaxesMedicare_Click(object sender, EventArgs e)
    {
        //Make fed taces button invisible
        btnTaxesMedicare.Visible = false;

        //Load Taxes
        LoadData(StudentID);
    }

    protected void btnTaxesSS_Click(object sender, EventArgs e)
    {
        //Make fed taces button invisible
        btnTaxesSS.Visible = false;

        //Load Taxes
        LoadData(StudentID);
    }

    protected void btnNMI_Click(object sender, EventArgs e)
    {
        //Open popup
        Page.ClientScript.RegisterStartupScript(GetType(), "Popup", "toggle();", true);
    }

    protected void btnEnter_Click(object sender, EventArgs e)
    {
        CheckNMI();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Automatically goes back to the non-blured page
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        //Make savings section of popup visible
        divSavingsPopup.Visible = true;
        divNMIPopup.Visible = false;      

        //Open popup
        Page.ClientScript.RegisterStartupScript(GetType(), "Popup", "toggle();", true);
    }

    protected void btnEnter2_Click(object sender, EventArgs e)
    {
        var Trans = Sim.GetTransitionData(1); //the 1 is the step in the DB, im planning to make the steps through the simulation customizable at some point

        //Check if code is correct
        if (tbUnlock.Text == Trans.UnlockCode.ToString())
        {
            //Link to sim savings
            Response.Redirect("Sim_Savings.aspx?b=" + StudentID);
        }
        else
        {
            //Keep open second popup and divs
            Page.ClientScript.RegisterStartupScript(GetType(), "Popup", "toggle();", true);
            divNMIPopup.Visible = false;
            divSavingsPopup.Visible = true;

            //Error message
            lblErrorPopup2.Text = "Wrong code entered.";
        }        
    }

    protected void btnCancel2_Click(object sender, EventArgs e)
    {
        //Keep btnNMI disabled
        btnNMI.Enabled = false;
    }
}