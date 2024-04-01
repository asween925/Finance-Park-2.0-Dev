using Microsoft.Ajax.Utilities;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Create_Job : Page
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
    private Class_JobData JobData = new Class_JobData();
    private Class_BusinessData BusinessData = new Class_BusinessData();
    private int VisitID;

    public Create_Job()
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
            //Load businesses ddl from businessInfoFP
            BusinessData.LoadBusinessNamesDDL(ddlBusinessName);

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void Submit()
    {
        //Check if fields are empty
        if (tbJobTitle.Text == "")
        {
            lblError.Text = "Please enter a job title before submitting.";
            return;
        }
        else if (ddlBusinessName.SelectedIndex == 0)
        {
            lblError.Text = "Please select a business before submitting.";
            return;
        }
        else if (tbDuties.InnerText == "")
        {
            lblError.Text = "Please enter in a job duty before submitting.";
            return;
        }
        else if (tbEdDebt.Text == "")
        {
            lblError.Text = "Please enter in a debt amount before submitting.";
            return;
        }
        else if (tbAdvance.InnerText == "")
        {
            lblError.Text = "Please enter in an advancement text before submitting.";
            return;
        }

        //Submit new entry into jobsFP
        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            try
            {           
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO jobsFP (jobTitle, business, educationBG, jobDuties, edDebt, advancement)
												        VALUES (@jobTitle, @business, @educationBG, @jobDuties, @edDebt, @advancement);"))
                {
                    cmd.Parameters.Add("@jobTitle", SqlDbType.VarChar).Value = tbJobTitle.Text;
                    cmd.Parameters.Add("@business", SqlDbType.VarChar).Value = ddlBusinessName.SelectedValue;
                    cmd.Parameters.Add("@educationBG", SqlDbType.VarChar).Value = ddlEducation.SelectedValue;
                    cmd.Parameters.Add("@jobDuties", SqlDbType.VarChar).Value = tbDuties.InnerText;
                    cmd.Parameters.Add("@edDebt", SqlDbType.VarChar).Value = tbEdDebt.Text;
                    cmd.Parameters.Add("@advancement", SqlDbType.VarChar).Value = tbAdvance.InnerText;

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                //Show success message and refresh page               
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "3;url=create_job.aspx";
                this.Page.Controls.Add(meta);
                lblError.Text = "Submission successful! Refreshing page...";
            }
                catch
                {
                lblError.Text = "Error in Submit(). Cannot create new school.";
                return;
            }
        }

    }



    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
    }
}