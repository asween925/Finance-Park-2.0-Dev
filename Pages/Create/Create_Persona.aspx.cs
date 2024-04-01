using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Create_Persona : Page
{
    private string SQLServer = ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string SQLDatabase = ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string SQLUser = ConfigurationManager.AppSettings["db_user"].ToString();
    private string SQLPassword = ConfigurationManager.AppSettings["db_password"].ToString();
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string ConnectionString;
    private Class_SQLCommands SQL = new Class_SQLCommands();
    private Class_VisitData VisitData = new Class_VisitData();
    private Class_SchoolData SchoolData = new Class_SchoolData();
    private Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    private Class_GridviewFunctions Gridviews = new Class_GridviewFunctions();
    private Class_JobData Jobs = new Class_JobData();
    private int VisitID;

    public Create_Persona()
    {
        ConnectionString = "Server=" + SQLServer + ";database=" + SQLDatabase + ";uid=" + SQLUser + ";pwd=" + SQLPassword + ";Connection Timeout=20;";
        
        Load += Page_Load;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Check if user is logged in
        if (HttpContext.Current.Session["LoggedIn"] == null)
        {
            Response.Redirect("../../Default.aspx");
        }

        if (!IsPostBack)
        {
            //load job titles ddl
            Jobs.LoadJobsDDL(ddlJobTitle);

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void Submit()
    {
        string JobID;
        string Child1 = "0";
        string Child2 = "0";
        string SpouseAge = "0";

        //Check if fields are empty
        if (ddlJobTitle.SelectedIndex == 0)
        {
            lblSuccess.Text = "Please select a job title before submitting.";
            return;
        }
        else if (tbGAI.Text == "")
        {
            lblSuccess.Text = "Please enter a GAI amount before submitting.";
            return;
        }
        else if (tbAge.Text == "")
        {
            lblSuccess.Text = "Please enter an age before submitting.";
            return;
        }
        else if (tbCreditScore.Text == "")
        {
            lblSuccess.Text = "Please enter a credit score before submitting.";
            return;
        }
        else if (tbCreditScore.Text == "")
        {
            lblSuccess.Text = "Please enter a credit score before submitting.";
            return;
        }
        else if (tbNMI.Text == "")
        {
            lblSuccess.Text = "Please enter a NMI amount before submitting.";
            return;
        }
        else if (tbCCDebt.Text == "")
        {
            lblSuccess.Text = "Please enter a CC debt amount before submitting.";
            return;
        }
        else if (tbFurnLimit.Text == "")
        {
            lblSuccess.Text = "Please enter a furniture limit before submitting.";
            return;
        }
        else if (tbHomeImp.Text == "")
        {
            lblSuccess.Text = "Please enter a home improvement limit before submitting.";
            return;
        }
        else if (tbLongSave.Text == "")
        {
            lblSuccess.Text = "Please enter a longterm savings before submitting.";
            return;
        }
        else if (tbEmerFunds.Text == "")
        {
            lblSuccess.Text = "Please enter an emergencies funds before submitting.";
            return;
        }
        else if (tbOtherSave.Text == "")
        {
            lblSuccess.Text = "Please enter an other savings amount before submitting.";
            return;
        }
        else if (tbAutoLoan.Text == "")
        {
            lblSuccess.Text = "Please enter an auto loan amount before submitting.";
            return;
        }
        else if (tbMortAmnt.Text == "")
        {
            lblSuccess.Text = "Please enter a mortgage amount before submitting.";
            return;
        }
        else if (tbThatsAmnt.Text == "")
        {
            lblSuccess.Text = "Please enter a 'That's Life' amount before submitting.";
            return;
        }
        else if (tbDesc.Text == "")
        {
            lblSuccess.Text = "Please enter a description before submitting.";
            return;
        }

        //Get job ID from name
        JobID = Jobs.GetJobIDFromTitle(ddlJobTitle.SelectedValue).ToString();

        //Check if married
        if (ddlMartialStatus.SelectedIndex == 1) 
        {
            SpouseAge = tbSpouseAge.Text;
        }

        //Check if child 1 or child 2 is selected
        if (ddlNumOfChild.SelectedIndex == 1)
        {
            if (tbChild1.Text == "")
            {
                Child1 = tbChild1.Text;
            }          
        }
        else if (ddlNumOfChild.SelectedIndex == 2)
        {
            if (tbChild2.Text == "")
            {
                Child2 = tbChild2.Text;
            }
        }

        //Submit new entry into jobsFP
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO personasFP (jobID, jobType, gai, age, maritalStatus, spouseAge, numOfChildren, child1Age, child2Age, creditScore, nmi, ccDebt, furnitureLimit, homeImpLimit, longSavings, emergFunds, otherSavings, autoLoanAmnt, mortAmnt, thatsLifeAmnt, description)
												        VALUES (@jobID, @jobType, @gai, @age, @maritalStatus, @spouseAge, @numOfChildren, @child1Age, @child2Age, @creditScore, @nmi, @ccDebt, @furnitureLimit, @homeImpLimit, @longSavings, @emergFunds, @otherSavings, @autoLoanAmnt, @mortAmnt, @thatsLifeAmnt, @description);"))
                {
                    cmd.Parameters.Add("@jobID", SqlDbType.Int).Value = JobID;
                    cmd.Parameters.Add("@jobType", SqlDbType.VarChar).Value = ddlJobType.SelectedValue;
                    cmd.Parameters.Add("@gai", SqlDbType.Money).Value = tbGAI.Text;
                    cmd.Parameters.Add("@age", SqlDbType.Int).Value = tbAge.Text;
                    cmd.Parameters.Add("@maritalStatus", SqlDbType.VarChar).Value = ddlMartialStatus.SelectedValue;
                    cmd.Parameters.Add("@spouseAge", SqlDbType.Int).Value = SpouseAge;
                    cmd.Parameters.Add("@numOfChildren", SqlDbType.Int).Value = ddlNumOfChild.SelectedValue;
                    cmd.Parameters.Add("@child1Age", SqlDbType.Int).Value = Child1;
                    cmd.Parameters.Add("@child2Age", SqlDbType.Int).Value = Child2;
                cmd.Parameters.Add("@creditScore", SqlDbType.Int).Value = tbCreditScore.Text;
                cmd.Parameters.Add("@nmi", SqlDbType.Money).Value = tbNMI.Text;
                    cmd.Parameters.Add("@ccDebt", SqlDbType.Money).Value = tbCCDebt.Text;
                    cmd.Parameters.Add("@furnitureLimit", SqlDbType.Money).Value = tbFurnLimit.Text;
                    cmd.Parameters.Add("@homeImpLimit", SqlDbType.Money).Value = tbHomeImp.Text;
                    cmd.Parameters.Add("@longSavings", SqlDbType.Money).Value = tbLongSave.Text;
                    cmd.Parameters.Add("@emergFunds", SqlDbType.Money).Value = tbEmerFunds.Text;
                    cmd.Parameters.Add("@otherSavings", SqlDbType.Money).Value = tbOtherSave.Text;
                    cmd.Parameters.Add("@autoLoanAmnt", SqlDbType.Money).Value = tbAutoLoan.Text;
                    cmd.Parameters.Add("@mortAmnt", SqlDbType.Money).Value = tbMortAmnt.Text;
                    cmd.Parameters.Add("@thatsLifeAmnt", SqlDbType.Money).Value = tbThatsAmnt.Text;
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = tbDesc.Text;

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                //Show success message and refresh page               
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "3;url=create_persona.aspx";
                this.Page.Controls.Add(meta);
                lblSuccess.Text = "Submission successful! Refreshing page...";
        }
            catch
            {
            lblError.Text = "Error in Submit(). Cannot create new persona.";
            return;
        }
    }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
    }

    protected void ddlNumOfChild_SelectedIndexChanged(object sender, EventArgs e)
    {
            if (ddlNumOfChild.SelectedIndex == 0)
            {
                pChild2.Visible = false;
                tbChild2.Visible = false;
                pChild1.Visible = false;
                tbChild1.Visible = false;
        }
            else if (ddlNumOfChild.SelectedIndex == 1)
            {
                pChild2.Visible = false;
                tbChild2.Visible = false;
                pChild1.Visible = true;
                tbChild1.Visible = true;
            }
            else if (ddlNumOfChild.SelectedIndex == 2)
            {
                pChild2.Visible = true;
                tbChild2.Visible = true;
                pChild1.Visible = true;
                tbChild1.Visible = true;
            }
    }

    protected void ddlMartialStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlMartialStatus.SelectedIndex == 0)
        {
            pSpouseAge.Visible = false;
            tbSpouseAge.Visible = false;
        }
        else if (ddlMartialStatus.SelectedIndex == 1)
        {
            pSpouseAge.Visible= true;
            tbSpouseAge.Visible = true;
        }
    }
}