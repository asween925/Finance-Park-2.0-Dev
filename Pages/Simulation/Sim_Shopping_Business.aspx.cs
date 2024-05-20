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
            }

            //Load Data
            LoadData(StudentID);       
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

        //Load table
        //try
        //{
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvShoppingItems.DataSource = Review_sds;
            dgvShoppingItems.DataBind();

            cmd.Dispose();
            con.Close();

        //}
        //catch
        //{
        //    lblError.Text = "Error in LoadData(). Cannot load item table.";
        //    return;
        //}
    }



    protected void dgvShoppingItems_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {

    }

    protected void dgvShoppingItems_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvShoppingItems.EditIndex = e.NewEditIndex;
        LoadData(StudentID);
    }

    protected void dgvShoppingItems_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvShoppingItems.EditIndex = -1;
        LoadData(StudentID);
    }

    protected void dgvShoppingItems_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvShoppingItems.PageIndex = e.NewPageIndex;
        LoadData(StudentID);
    }

    protected void dgvShoppingItems_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            //string lblJobTitle = (e.Row.FindControl("lblJobTitleDGV") as Label).Text;
            //string lblJobType = (e.Row.FindControl("lblJobTypeDGV") as Label).Text;
            //string lblMaritalStatus = (e.Row.FindControl("lblMaritalStatusDGV") as Label).Text;
            //string lblNumOfChild = (e.Row.FindControl("lblNumOfChildren") as Label).Text;

            //DropDownList ddlJobTitle = e.Row.FindControl("ddlJobTitleDGV") as DropDownList;
            //DropDownList ddlJobType = e.Row.FindControl("ddlJobTypeDGV") as DropDownList;
            //DropDownList ddlMaritalStatus = e.Row.FindControl("ddlMaritalStatusDGV") as DropDownList;
            //DropDownList ddlNumOfChild = e.Row.FindControl("ddlNumOfChildren") as DropDownList;

            ////Load gridview job DDLs with job title
            ////Gridviews.JobTitle(ddlJobTitle, lblJobTitle);

            ////Find job type
            //if (lblJobType != "")
            //{
            //    ddlJobType.Items.FindByValue(lblJobType).Selected = true;
            //}

            ////Find job type
            //if (lblMaritalStatus != "")
            //{
            //    ddlMaritalStatus.Items.FindByValue(lblMaritalStatus).Selected = true;
            //}

            ////Find job type
            //if (lblNumOfChild != "")
            //{
            //    ddlNumOfChild.Items.FindByValue(lblNumOfChild).Selected = true;
            //}

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
        //If checked

        //Insert item into studentShopping

        //Load data

        //If unchecked

        //Remove item from studentShopping

        //Load data
    }
}