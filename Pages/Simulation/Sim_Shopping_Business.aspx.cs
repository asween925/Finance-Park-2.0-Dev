using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sim_Shopping_Business : System.Web.UI.Page
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
    private Class_BusinessData Businesses = new Class_BusinessData();
    private Class_SponsorData Sponsors = new Class_SponsorData();
    private int VisitID;
    private int StudentID;
    private int AcctNum;
    private int BusinessID;

    public Sim_Shopping_Business()
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
            var Student = Students.StudentLookup(25, StudentID);
            AcctNum = Student.AccountNumber;

            //Check if business id is passed through
            if (Request["c"] != null)
            {
                BusinessID = int.Parse(Request["c"]);
            }

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

                //Load Data
                LoadData(StudentID);   

            }              
        }
    }

    protected void LoadData(int StudentID)
    {
        var Scripts = Businesses.GetBusinessScripts(BusinessID);
        var Actions = Businesses.GetBusinessActions(BusinessID);
        var Student = Students.StudentLookup(25, StudentID);
        var Persona = Students.PersonaLookup(Student.PersonaID);
        var Buttons = Businesses.GetActionButtons(BusinessID);
        var Tables = Businesses.GetBusinessTableData(BusinessID);
        List<int> SIDs = Sponsors.GetSponsorIDs(BusinessID);
        int[] SIDsA = SIDs.ToArray();
        int ArrayCount = SIDsA.Length;
        int ActionBtnTotal = Actions.Action;
        string SQLStatement = "SELECT id, itemName, cost, category, photoPath FROM shoppingItemsFP WHERE businessID='" + BusinessID + "'";

        //Load business name
        lblBusinessName.Text = Businesses.GetBusinessName(BusinessID);

        //Load student spending data
        lblNMI.Text = Student.NMI.ToString("c");
        lblSpent.Text = Student.Spent.ToString("c");
        lblRemaining.Text = (Student.NMI - Student.Spent).ToString("c");

        //Load sponsor logos
        switch (ArrayCount)
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

        //Load scripts
        lblKioskScript.Text = Scripts.Shopping.ToString();
        lblKioskScript2.Text = Scripts.Shopping2.ToString();

        //Load actions buttons
        ActionButtons();

        //Load table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvShoppingItems.DataSource = Review_sds;
            dgvShoppingItems.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load item table.";
            return;
        }

        //Load budget in progress bar

        //Load label spent, calculate remaining

        //Load selected items 
    }

    protected void ActionButtons()
    {
        //Check for action buttons
        var Actions = Businesses.GetActionButtons(BusinessID);

        if (Actions.Btn1SText != "")
        {
            btnAction.Text = Actions.Btn1SText;
            btnAction.Visible = true;
        }
        if (Actions.BtnSText2 != "")
        {
            btnAction2.Text = Actions.BtnSText2;
            btnAction2.Visible = true;
        }
        if (Actions.BtnSText3 != "")
        {
            btnAction3.Text = Actions.BtnSText3;
            btnAction3.Visible = true;
        }
    }


    protected void btnNext_Click(object sender, EventArgs e)
    {

    }

    protected void btnEnter_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Automatically closes the popup
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Sim_Shopping.aspx?b=" + StudentID);
    }

    protected void btnAction_Click(object sender, EventArgs e)
    {

    }

    protected void btnAction2_Click(object sender, EventArgs e)
    {

    }

    protected void btnAction3_Click(object sender, EventArgs e)
    {

    }



    protected void chkBuy_CheckedChanged(object sender, EventArgs e)
    {
        CheckBox chk = (CheckBox)sender;
        GridViewRow dgv = (GridViewRow)chk.NamingContainer;
        int ItemID;
        
        //If checked
        if (chk.Checked == true)
        {
            //Get item ID
            ItemID = Convert.ToInt16(dgv.Cells[0].Text);

            //Insert item into studentShopping
            Sim.InsertShoppingItem(25, StudentID, BusinessID, ItemID);

            //Load data
        }

        //If unchecked
        else
        {
            //Get item ID
            ItemID = Convert.ToInt16(dgv.Cells[0].Text);

            //Remove item from studentShopping
            Sim.DeleteShoppingItem(25, StudentID, BusinessID, ItemID);

            //Load data
        }

    }
}