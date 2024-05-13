using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Optimization;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sim_Business : System.Web.UI.Page
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
    private Class_StudentData Students = new Class_StudentData();
    private Class_Simulation Sim = new Class_Simulation();
    private Class_BusinessData Businesses = new Class_BusinessData();
    private Class_SponsorData Sponsors = new Class_SponsorData();
    private int VisitID;
    private int StudentID;
    private int BusinessID;
    private int AcctNum;

    public Sim_Business()
    {
        ConnectionString = "Server=" + SQLServer + ";database=" + SQLDatabase + ";uid=" + SQLUser + ";pwd=" + SQLPassword + ";Connection Timeout=20;";
    }

    protected void Page_Load(object sender, EventArgs e)
    {        
        //Get current visit ID and student ID
        VisitID = VisitData.GetVisitID();

        //Check if student id is passed through
        if (Request["b"] != null)
        {
            StudentID = int.Parse(Request["b"]);

            //Get account number
            var Student = Students.StudentLookup(23, StudentID);
            AcctNum = Student.AccountNumber;           
        }

        //Check if business ID is passed through
        if (Request["c"] != null)            
        {
            BusinessID = int.Parse(Request["c"]);

            //Check businesses unlocked
            lblBusinessUnlocked.Text = Sim.GetTotalBizUnlocked(23, StudentID).ToString();

            //Check for postback
            if (!IsPostBack)
            {
                var Scripts = Businesses.GetBusinessScripts(BusinessID);

                //Check for inital pop up
                if (Scripts.Popup != "")
                {
                    //Show popup
                    Page.ClientScript.RegisterStartupScript(GetType(), "Popup", "toggle();", true);

                    //Assign script to label in popup
                    lblPopupText.Text = Scripts.Popup;
                }
            }
            
            //Load Data
            LoadData(StudentID, BusinessID);
        }
    }

    protected void LoadData(int StudentID, int BusinessID)
    {
        var Scripts = Businesses.GetBusinessScripts(BusinessID);
        var Actions = Businesses.GetBusinessActions(BusinessID);
        var Student = Students.StudentLookup(23, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);
        var Buttons = Businesses.GetActionButtons(BusinessID);
        var Tables = Businesses.GetBusinessTableData(BusinessID);
        List<int> SIDs = Sponsors.GetSponsorIDs(BusinessID);
        int[] SIDsA = SIDs.ToArray();
        int ArrayCount = SIDsA.Length;
        int ActionBtnTotal = Actions.Action;
      
        //Assign business name to label
        lblBusinessName.Text = Businesses.GetBusinessName(BusinessID);      

        //Load sponsor logos
        switch(ArrayCount)
        {
            case 1:
                pImg1.Visible = true;
                imgSponsorLogo1.ImageUrl = "~/Media/" + Sponsors.GetSponsorLogoFromID(SIDsA[0]);
                break;
            case 2:
                pImg1.Visible = true;
                pImg2.Visible = true;
                imgSponsorLogo1.ImageUrl = "~/Media/" + Sponsors.GetSponsorLogoFromID(SIDsA[0]);
                imgSponsorLogo2.ImageUrl = "~/Media/" + Sponsors.GetSponsorLogoFromID(SIDsA[1]);
                break;
            case 3:
                pImg1.Visible = true;
                pImg2.Visible = true;
                pImg3.Visible = true;
                imgSponsorLogo1.ImageUrl = "~/Media/" + Sponsors.GetSponsorLogoFromID(SIDsA[0]);
                imgSponsorLogo2.ImageUrl = "~/Media/" + Sponsors.GetSponsorLogoFromID(SIDsA[1]);
                imgSponsorLogo3.ImageUrl = "~/Media/" + Sponsors.GetSponsorLogoFromID(SIDsA[2]);
                break;
            case 4:
                pImg1.Visible = true;
                pImg2.Visible = true;
                pImg3.Visible = true;
                pImg4.Visible = true;
                imgSponsorLogo1.ImageUrl = "~/Media/" + Sponsors.GetSponsorLogoFromID(SIDsA[0]);
                imgSponsorLogo2.ImageUrl = "~/Media/" + Sponsors.GetSponsorLogoFromID(SIDsA[1]);
                imgSponsorLogo3.ImageUrl = "~/Media/" + Sponsors.GetSponsorLogoFromID(SIDsA[2]);
                imgSponsorLogo3.ImageUrl = "~/Media/" + Sponsors.GetSponsorLogoFromID(SIDsA[3]);
                break;
        }          

        //Load main script
        if (Scripts.Main != null)
        {
            lblKioskScript.Text = Scripts.Main;
        }

        //Check for action buttons
        if (ActionBtnTotal != 0)
        {
            switch(ActionBtnTotal)
            {
                case 1:
                    btnAction.Visible = true;
                    btnAction.Text = Buttons.Btn1Text;
                    break;
                case 2:
                    btnAction.Visible = true;
                    btnAction.Text = Buttons.Btn1Text;
                    btnAction2.Visible = true;
                    btnAction2.Text = Buttons.BtnText2;
                    break;
                case 3:
                    btnAction.Visible = true;
                    btnAction.Text = Buttons.Btn1Text;
                    btnAction2.Visible = true;
                    btnAction2.Text = Buttons.BtnText2;
                    btnAction3.Visible = true;
                    btnAction3.Text = Buttons.BtnText3;
                    break;
            }
        }

        //Check for tables
        if (Actions.Table != false)
        {
            //Load table data
            //Load categories
            lblTblCat1.Text = Tables.Cat1;
            lblTblCat2.Text = Tables.Cat2;
            lblTblCat3.Text = Tables.Cat3;
            lblTblCat4.Text = Tables.Cat4;
            lblTblCat5.Text = Tables.Cat5;
            lblTblCat6.Text = Tables.Cat6;

            //Load data in categories
            lblTblCat1Data1.Text = Tables.Cat1D1;
            lblTblCat1Data2.Text = Tables.Cat1D2;
            lblTblCat1Data3.Text = Tables.Cat1D3;
            lblTblCat1Data4.Text = Tables.Cat1D4;
            lblTblCat2Data1.Text = Tables.Cat2D1;
            lblTblCat2Data2.Text = Tables.Cat2D2;
            lblTblCat2Data3.Text = Tables.Cat2D3;
            lblTblCat2Data4.Text = Tables.Cat2D4;
            lblTblCat3Data1.Text = Tables.Cat3D1;
            lblTblCat3Data2.Text = Tables.Cat3D2;
            lblTblCat3Data3.Text = Tables.Cat3D3;
            lblTblCat3Data4.Text = Tables.Cat3D4;
            lblTblCat4Data1.Text = Tables.Cat4D1;
            lblTblCat4Data2.Text = Tables.Cat4D2;
            lblTblCat4Data3.Text = Tables.Cat4D3;
            lblTblCat4Data4.Text = Tables.Cat4D4;
            lblTblCat5Data1.Text = Tables.Cat5D1;
            lblTblCat5Data2.Text = Tables.Cat5D2;
            lblTblCat5Data3.Text = Tables.Cat5D3;
            lblTblCat5Data4.Text = Tables.Cat5D4;
            lblTblCat6Data1.Text = Tables.Cat6D1;
            lblTblCat6Data2.Text = Tables.Cat6D2;
            lblTblCat6Data3.Text = Tables.Cat6D3;
            lblTblCat6Data4.Text = Tables.Cat6D4;

            //Check if credit cards if loaded
            if (lblBusinessName.Text == "Credit Cards")
            {
                lblTblCat1Data1.Text = Persona.CCDebt.ToString("c");
                lblTblCat2Data1.Text = "15.5%";
                lblTblCat3Data1.Text = (Persona.CCDebt * Convert.ToDecimal(0.04)).ToString("c");
            }
        }

        //Check for loan
        if (Actions.Loan != false)
        {
            //Load loan data
            lblLoanAppScript.Text = Scripts.Loan;
        }

        //Check for retirement
        if (Actions.Retire != false)
        {

        }
    }

    protected void Action(int BtnNum)
    {
        var Actions = Businesses.GetActionButtons(BusinessID);
        var Student = Students.StudentLookup(VisitID, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);
        string Action = "";

        //Close popup
        Page.ClientScript.RegisterStartupScript(GetType(), "Popup", "toggle();", false);

        //Check action for button number
        if (BtnNum == 1)
        {
            Action = Actions.Btn1A.ToString();
        }
        else if (BtnNum == 2)
        {
            Action = Actions.Btn2A.ToString();
        }
        else
        {
            Action = Actions.Btn3A.ToString();
        }

        //Do action
        if (Action == "Show Table")
        {
            tblBusiness.Visible = true;
        }
        else if (Action == "Show Retirement Calculator")
        {
            divRetire.Visible = true;

            //Load retirement savings
            btnRetireTotal.Text = Persona.LongSavings.ToString("c");
        }
        else if (Action == "Show Loan")
        {
            //Show loan div
            divLoan.Visible = true;

            //Check if married or single, if single, disable co-borrower button and co-GMI button
            if (Persona.MarriageStatus == "Married")
            {
                btnLoanCoBorrower.Enabled = false;
                btnLoanCoGMI.Enabled = false;

                //Change text
                btnLoanCoBorrower.Text = "Disabled";
                btnLoanCoGMI.Text = "Disabled";
            }
        }
    }

    protected void Retirement()
    {
        var Student = Students.StudentLookup(23, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);
        double Interest = 0;
        int Years = 0;
        string Attribute = "background-color: white; border: 1px solid green; color: black;";

        //Check enabled buttons, assign variables
        if (btnRetire4.HasAttributes == true)
        {
            Interest = 0.04;
        }
        else if (btnRetire6.HasAttributes == true)
        {
            Interest = 0.06;
        }
        else if (btnRetire8.HasAttributes == true)
        {
            Interest = 0.08;
        }

        if (btnRetire30.HasAttributes == true)
        {
            Years = 30;
        }
        else if (btnRetire40.HasAttributes == true)
        {
            Years = 40;
        }
        else if (btnRetire50.HasAttributes == true)
        {
            Years = 50;
        }

        //Calculate total savings over time
        int YearsTotal = 1200 * Years;
        btnRetireTotal.Text = ((YearsTotal * Interest) + YearsTotal + Convert.ToDouble(Persona.LongSavings)).ToString("c");
    }



    protected void btnResearch_Click(object sender, EventArgs e)
    {
        //Add business to unlocked table for Student ID and Visit ID
        Sim.AddUnlockedBusiness(23, StudentID, BusinessID);

        //Redirect to research page
        Response.Redirect("Sim_Research.aspx?b=" + StudentID);
    }

    protected void btnAction_Click(object sender, EventArgs e)
    {
        Action(1);
    }

    protected void btnAction2_Click(object sender, EventArgs e)
    {
        Action(2);
    }

    protected void btnAction3_Click(object sender, EventArgs e)
    {
        Action(3);
    }

    protected void btnLoanBorrower_Click(object sender, EventArgs e)
    {
        var Student = Students.StudentLookup(23, StudentID);

        //Disable button
        btnLoanBorrower.Enabled = false;

        //Change CSS
        btnLoanBorrower.CssClass = " Sim_Business_Loan_Button_Clicked";

        //Get student name and assign it to the button
        btnLoanBorrower.Text = Student.FirstName.ToString();
    }

    protected void btnLoanCoBorrower_Click(object sender, EventArgs e)
    {
        var Student = Students.StudentLookup(23, StudentID);

        //Disable button
        btnLoanCoBorrower.Enabled = false;

        //Change CSS
        btnLoanCoBorrower.CssClass = " Sim_Business_Loan_Button_Clicked";

        //Assign 'Spouse' to button text
        btnLoanCoBorrower.Text = "Spouse";
    }

    protected void btnLoanPurposePurchase_Click(object sender, EventArgs e)
    {
        //Assign CSS, clear other button attributes
        btnLoanPurposePurchase.Attributes.Add("style", "border: 2px solid green;");
        btnLoanPurposeConstruction.Attributes.Clear();
        btnLoanPurposeRefinance.Attributes.Clear();
    }

    protected void btnLoanPurposeRefinance_Click(object sender, EventArgs e)
    {
        //Assign CSS, clear other button attributes
        btnLoanPurposePurchase.Attributes.Clear();
        btnLoanPurposeConstruction.Attributes.Clear();
        btnLoanPurposeRefinance.Attributes.Add("style", "border: 2px solid green;");
    }

    protected void btnLoanPurposeConstruction_Click(object sender, EventArgs e)
    {
        //Assign CSS, clear other button attributes
        btnLoanPurposePurchase.Attributes.Clear();
        btnLoanPurposeConstruction.Attributes.Add("style", "border: 2px solid green;");
        btnLoanPurposeRefinance.Attributes.Clear();
    }

    protected void btnLoanGMI_Click(object sender, EventArgs e)
    {
        var Student = Students.StudentLookup(23, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);

        //Get GMI
        btnLoanGMI.Text = (Persona.GAI / 12).ToString("c");

        //Assign CSS
        btnLoanGMI.Attributes.Add("style", "background-color: white; color: black; border: 1px solid green;");
    }

    protected void btnLoanBank_Click(object sender, EventArgs e)
    {
        var Student = Students.StudentLookup(23, StudentID);

        //Get GMI
        btnLoanBank.Text = Student.AccountNumber.ToString();

        //Assign CSS
        btnLoanBank.Attributes.Add("style", "background-color: white; color: black; border: 1px solid green;");
    }

    protected void btnLoanCC_Click(object sender, EventArgs e)
    {
        var Student = Students.StudentLookup(23, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);

        //Get GMI
        btnLoanCC.Text = Persona.CCDebt.ToString("c");

        //Assign CSS
        btnLoanCC.Attributes.Add("style", "background-color: white; color: black; border: 1px solid green;");
    }

    protected void btnLoanCoGMI_Click(object sender, EventArgs e)
    {
        var Student = Students.StudentLookup(23, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);

        //Get GMI
        btnLoanCoGMI.Text = (Persona.GAI / 12).ToString("c");

        //Assign CSS
        btnLoanCoGMI.Attributes.Add("style", "background-color: white; color: black; border: 1px solid green;");
    }

    protected void btnLoanBalance_Click(object sender, EventArgs e)
    {
        var Student = Students.StudentLookup(23, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);

        //Get GMI
        btnLoanBalance.Text = Persona.GAI.ToString("c");

        //Assign CSS
        btnLoanBalance.Attributes.Add("style", "background-color: white; color: black; border: 1px solid green;");
    }

    protected void btnLoanOther_Click(object sender, EventArgs e)
    {
        var Student = Students.StudentLookup(23, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);

        //Get GMI
        btnLoanOther.Text = Persona.OtherSavings.ToString("c");

        //Assign CSS
        btnLoanOther.Attributes.Add("style", "background-color: white; color: black; border: 1px solid green;");
    }

    protected void btnLoanSubmit_Click(object sender, EventArgs e)
    {
        //Open popup
        Page.ClientScript.RegisterStartupScript(GetType(), "Popup", "toggle();", true);

        //Update pop up text
        lblPopupText.Text = "Application submitted! Close this window and tap on Unlock More to continue with the simulation.";
    }

    protected void btnRetire4_Click(object sender, EventArgs e)
    {
        //Update css
        btnRetire4.Attributes.Add("style", "background-color: white; border: 1px solid green; color: black;");

        //Reset CSS with other buttons
        btnRetire6.Attributes.Clear();
        btnRetire8.Attributes.Clear();

        //Update total retirement savings
        Retirement();
    }

    protected void btnRetire6_Click(object sender, EventArgs e)
    {
        //Update css
        btnRetire6.Attributes.Add("style", "background-color: white; border: 1px solid green; color: black;");

        //Reset CSS with other buttons
        btnRetire4.Attributes.Clear();
        btnRetire8.Attributes.Clear();

        //Update total retirement savings
        Retirement();
    }

    protected void btnRetire8_Click(object sender, EventArgs e)
    {
        //Update css
        btnRetire8.Attributes.Add("style", "background-color: white; border: 1px solid green; color: black;");

        //Reset CSS with other buttons
        btnRetire4.Attributes.Clear();
        btnRetire6.Attributes.Clear();

        //Update total retirement savings
        Retirement();
    }

    protected void btnRetire30_Click(object sender, EventArgs e)
    {
        //Update css
        btnRetire30.Attributes.Add("style", "background-color: white; border: 1px solid green; color: black;");

        //Reset CSS with other buttons
        btnRetire40.Attributes.Clear();
        btnRetire50.Attributes.Clear();

        //Update total retirement savings
        Retirement();
    }

    protected void btnRetire40_Click(object sender, EventArgs e)
    {
        //Update css
        btnRetire40.Attributes.Add("style", "background-color: white; border: 1px solid green; color: black;");

        //Reset CSS with other buttons
        btnRetire30.Attributes.Clear();
        btnRetire50.Attributes.Clear();

        //Update total retirement savings
        Retirement();
    }

    protected void btnRetire50_Click(object sender, EventArgs e)
    {
        //Update css
        btnRetire50.Attributes.Add("style", "background-color: white; border: 1px solid green; color: black;");

        //Reset CSS with other buttons
        btnRetire40.Attributes.Clear();
        btnRetire30.Attributes.Clear();

        //Update total retirement savings
        Retirement();
    }

    protected void btnRetireTotal_Click(object sender, EventArgs e)
    {

    }
}