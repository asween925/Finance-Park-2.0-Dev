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
    private int VisitID;

    public Create_Job()
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
            //Load businesses ddl from businessInfoFP
            JobData.LoadBusinessDDL(businessName_ddl);

            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void Submit()
    {
        //Check if fields are empty
        if (jobTitle_tb.Text == "")
        {
            error_lbl.Text = "Please enter a job title before submitting.";
            return;
        }
        else if (businessName_ddl.SelectedIndex == 0)
        {
            error_lbl.Text = "Please select a business before submitting.";
            return;
        }
        else if (duties_tb.InnerText == "")
        {
            error_lbl.Text = "Please enter in a job duty before submitting.";
            return;
        }
        else if (edDebt_tb.Text == "")
        {
            error_lbl.Text = "Please enter in a debt amount before submitting.";
            return;
        }
        else if (advance_tb.InnerText == "")
        {
            error_lbl.Text = "Please enter in an advancement text before submitting.";
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
                    cmd.Parameters.Add("@jobTitle", SqlDbType.VarChar).Value = jobTitle_tb.Text;
                    cmd.Parameters.Add("@business", SqlDbType.VarChar).Value = businessName_ddl.SelectedValue;
                    cmd.Parameters.Add("@educationBG", SqlDbType.VarChar).Value = education_ddl.SelectedValue;
                    cmd.Parameters.Add("@jobDuties", SqlDbType.VarChar).Value = duties_tb.InnerText;
                    cmd.Parameters.Add("@edDebt", SqlDbType.VarChar).Value = edDebt_tb.Text;
                    cmd.Parameters.Add("@advancement", SqlDbType.VarChar).Value = advance_tb.InnerText;

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
                error_lbl.Text = "Submission successful! Refreshing page...";
            }
                catch
                {
                error_lbl.Text = "Error in Submit(). Cannot create new school.";
                return;
            }
        }

    }



    protected void submit_btn_Click(object sender, EventArgs e)
    {
        Submit();
    }
}