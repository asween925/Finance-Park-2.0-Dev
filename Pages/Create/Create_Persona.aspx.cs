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
        VisitID = VisitData.GetVisitID();
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
            Jobs.LoadJobsDDL(jobTitle_ddl);

            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void Submit()
    {
        string JobID;
        string Child1 = "0";
        string Child2 = "0";
        string SpouseAge = "0";

        //Check if fields are empty
        if (jobTitle_ddl.SelectedIndex == 0)
        {
            success_lbl.Text = "Please select a job title before submitting.";
            return;
        }
        else if (gai_tb.Text == "")
        {
            success_lbl.Text = "Please enter a GAI amount before submitting.";
            return;
        }
        else if (age_tb.Text == "")
        {
            success_lbl.Text = "Please enter an age before submitting.";
            return;
        }
        else if (creditScore_tb.Text == "")
        {
            success_lbl.Text = "Please enter a credit score before submitting.";
            return;
        }
        else if (creditScore_tb.Text == "")
        {
            success_lbl.Text = "Please enter a credit score before submitting.";
            return;
        }
        else if (nmi_tb.Text == "")
        {
            success_lbl.Text = "Please enter a NMI amount before submitting.";
            return;
        }
        else if (ccdebt_tb.Text == "")
        {
            success_lbl.Text = "Please enter a CC debt amount before submitting.";
            return;
        }
        else if (furnLimit_tb.Text == "")
        {
            success_lbl.Text = "Please enter a furniture limit before submitting.";
            return;
        }
        else if (homeImp_tb.Text == "")
        {
            success_lbl.Text = "Please enter a home improvement limit before submitting.";
            return;
        }
        else if (longSave_tb.Text == "")
        {
            success_lbl.Text = "Please enter a longterm savings before submitting.";
            return;
        }
        else if (emerFunds_tb.Text == "")
        {
            success_lbl.Text = "Please enter an emergencies funds before submitting.";
            return;
        }
        else if (otherSave_tb.Text == "")
        {
            success_lbl.Text = "Please enter an other savings amount before submitting.";
            return;
        }
        else if (autoLoan_tb.Text == "")
        {
            success_lbl.Text = "Please enter an auto loan amount before submitting.";
            return;
        }
        else if (mortAmnt_tb.Text == "")
        {
            success_lbl.Text = "Please enter a mortgage amount before submitting.";
            return;
        }
        else if (thatsAmnt_tb.Text == "")
        {
            success_lbl.Text = "Please enter a 'That's Life' amount before submitting.";
            return;
        }
        else if (desc_tb.Text == "")
        {
            success_lbl.Text = "Please enter a description before submitting.";
            return;
        }

        //Get job ID from name
        JobID = Jobs.GetJobIDFromTitle(jobTitle_ddl.SelectedValue).ToString();

        //Check if married
        if (martialStatus_ddl.SelectedIndex == 1) 
        {
            SpouseAge = spouseAge_tb.Text;
        }

        //Check if child 1 or child 2 is selected
        if (numOfChild_ddl.SelectedIndex == 1)
        {
            if (child1_tb.Text == "")
            {
                Child1 = child1_tb.Text;
            }          
        }
        else if (numOfChild_ddl.SelectedIndex == 2)
        {
            if (child2_tb.Text == "")
            {
                Child2 = child2_tb.Text;
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
                    cmd.Parameters.Add("@jobType", SqlDbType.VarChar).Value = jobType_ddl.SelectedValue;
                    cmd.Parameters.Add("@gai", SqlDbType.Money).Value = gai_tb.Text;
                    cmd.Parameters.Add("@age", SqlDbType.Int).Value = age_tb.Text;
                    cmd.Parameters.Add("@maritalStatus", SqlDbType.VarChar).Value = martialStatus_ddl.SelectedValue;
                    cmd.Parameters.Add("@spouseAge", SqlDbType.Int).Value = SpouseAge;
                    cmd.Parameters.Add("@numOfChildren", SqlDbType.Int).Value = numOfChild_ddl.SelectedValue;
                    cmd.Parameters.Add("@child1Age", SqlDbType.Int).Value = Child1;
                    cmd.Parameters.Add("@child2Age", SqlDbType.Int).Value = Child2;
                cmd.Parameters.Add("@creditScore", SqlDbType.Int).Value = creditScore_tb.Text;
                cmd.Parameters.Add("@nmi", SqlDbType.Money).Value = nmi_tb.Text;
                    cmd.Parameters.Add("@ccDebt", SqlDbType.Money).Value = ccdebt_tb.Text;
                    cmd.Parameters.Add("@furnitureLimit", SqlDbType.Money).Value = furnLimit_tb.Text;
                    cmd.Parameters.Add("@homeImpLimit", SqlDbType.Money).Value = homeImp_tb.Text;
                    cmd.Parameters.Add("@longSavings", SqlDbType.Money).Value = longSave_tb.Text;
                    cmd.Parameters.Add("@emergFunds", SqlDbType.Money).Value = emerFunds_tb.Text;
                    cmd.Parameters.Add("@otherSavings", SqlDbType.Money).Value = otherSave_tb.Text;
                    cmd.Parameters.Add("@autoLoanAmnt", SqlDbType.Money).Value = autoLoan_tb.Text;
                    cmd.Parameters.Add("@mortAmnt", SqlDbType.Money).Value = mortAmnt_tb.Text;
                    cmd.Parameters.Add("@thatsLifeAmnt", SqlDbType.Money).Value = thatsAmnt_tb.Text;
                    cmd.Parameters.Add("@description", SqlDbType.VarChar).Value = desc_tb.Text;

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
                success_lbl.Text = "Submission successful! Refreshing page...";
        }
            catch
            {
            error_lbl.Text = "Error in Submit(). Cannot create new persona.";
            return;
        }
    }
    }

    protected void submit_btn_Click(object sender, EventArgs e)
    {
        Submit();
    }

    protected void numOfChild_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
            if (numOfChild_ddl.SelectedIndex == 0)
            {
                child2_p.Visible = false;
                child2_tb.Visible = false;
                child1_p.Visible = false;
                child1_tb.Visible = false;
        }
            else if (numOfChild_ddl.SelectedIndex == 1)
            {
                child2_p.Visible = false;
                child2_tb.Visible = false;
                child1_p.Visible = true;
                child1_tb.Visible = true;
            }
            else if (numOfChild_ddl.SelectedIndex == 2)
            {
                child2_p.Visible = true;
                child2_tb.Visible = true;
                child1_p.Visible = true;
                child1_tb.Visible = true;
            }
    }

    protected void martialStatus_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (martialStatus_ddl.SelectedIndex == 0)
        {
            spouseAge_p.Visible = false;
            spouseAge_tb.Visible = false;
        }
        else if (martialStatus_ddl.SelectedIndex == 1)
        {
            spouseAge_p.Visible= true;
            spouseAge_tb.Visible = true;
        }
    }
}