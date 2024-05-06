using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Edit_Job : Page
{
    private string SQLServer = ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string SQLDatabase = ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string SQLUser = ConfigurationManager.AppSettings["db_user"].ToString();
    private string SQLPassword = ConfigurationManager.AppSettings["db_password"].ToString();
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string ConnectionString;
    private Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    private Class_GridviewFunctions Gridviews = new Class_GridviewFunctions();
    private Class_BusinessData BusinessData = new Class_BusinessData();
    private Class_JobData JobData = new Class_JobData(); 

    public Edit_Job()
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
            //Load jobs ddl
            JobData.LoadJobsDDL(ddlJobTitle);

            // Populating school header
            lblHeaderSchoolName.Text = SchoolHeader.GetSchoolHeader().ToString();

            //Load table
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLStatement = "SELECT * FROM jobsFP";

        //Clear error
        lblError.Text = "";

        //Clear table
        dgvJobs.DataSource = null;
        dgvJobs.DataBind();

        //If loading by the DDL, add school name to search query
        if (ddlJobTitle.SelectedIndex != 0)
        {
            SQLStatement = SQLStatement + " WHERE jobTitle='" + ddlJobTitle.SelectedValue + "'";
        }
        else if (tbSearch.Text != "")
        {
            SQLStatement = SQLStatement + " WHERE jobTitle LIKE '%" + tbSearch.Text + "%'";
        }
        else
        {
            SQLStatement = SQLStatement + " ORDER BY jobTitle ASC";
        }

        //Load schoolInfoFP table
        //try
        //{
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvJobs.DataSource = Review_sds;
            dgvJobs.DataBind();

            cmd.Dispose();
            con.Close();

        //}
        //catch
        //{
        //    lblError.Text = "Error in LoadData(). Cannot load jobs table.";
        //    return;
        //}

        // Highlight row being edited
        foreach (GridViewRow row in dgvJobs.Rows)
        {
            if (row.RowIndex == dgvJobs.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }

    protected void dgvJobs_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(dgvJobs.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string JobTitle = ((TextBox)dgvJobs.Rows[e.RowIndex].FindControl("tbJobTitleDGV")).Text;
        string BusinessName = ((DropDownList)dgvJobs.Rows[e.RowIndex].FindControl("ddlBusinessNameDGV")).SelectedValue;
        string EducationBG = ((DropDownList)dgvJobs.Rows[e.RowIndex].FindControl("ddlEducationBGDGV")).SelectedValue;
        string JobDuties = ((TextBox)dgvJobs.Rows[e.RowIndex].FindControl("tbJobDutiesDGV")).Text;
        string EdDebt = ((TextBox)dgvJobs.Rows[e.RowIndex].FindControl("tbEdDebtDGV")).Text;
        string Advancement = ((TextBox)dgvJobs.Rows[e.RowIndex].FindControl("tbAdvancementDGV")).Text;
        string BusinessID;

        //Get business ID from name
        if (BusinessName != "All")
        {
            BusinessID = BusinessData.GetBusinessID(BusinessName).ToString();
        }
        else
        {
            BusinessID = "0";
        }
        
        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE jobsFP SET jobTitle=@jobTitle, business=@business, educationBG=@educationBG, jobDuties=@jobDuties, edDebt=@edDebt, advancement=@advancement WHERE ID=@Id"))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@jobTitle", JobTitle);
                    cmd.Parameters.AddWithValue("@business", BusinessID);
                    cmd.Parameters.AddWithValue("@educationBG", EducationBG);
                    cmd.Parameters.AddWithValue("@jobDuties", JobDuties);
                    cmd.Parameters.AddWithValue("@edDebt", EdDebt);
                    cmd.Parameters.AddWithValue("@advancement", Advancement);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dgvJobs.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void dgvJobs_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(dgvJobs.DataKeys[e.RowIndex].Values[0]); // Gets id number

        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM jobsFP WHERE id=@ID"))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dgvJobs.EditIndex = -1;       // reset the grid after editing

            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }
    }

    protected void dgvJobs_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvJobs.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void dgvJobs_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvJobs.EditIndex = -1;
        LoadData();
    }

    protected void dgvJobs_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvJobs.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void dgvJobs_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblBusiness = (e.Row.FindControl("lblBusinessNameDGV") as Label).Text;
            DropDownList ddlBusiness = e.Row.FindControl("ddlBusinessNameDGV") as DropDownList;

            //Load gridview school DDLs with school names
            Gridviews.BusinessNames(ddlBusiness, lblBusiness);

            string lblEdBG = (e.Row.FindControl("lblEducationBGDGV") as Label).Text;
            DropDownList ddlEdBG = e.Row.FindControl("ddlEducationBGDGV") as DropDownList;

            //Select business name from label
            ddlEdBG.SelectedValue = lblEdBG;

        }
    }



    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (tbSearch.Text != "")
        {
            LoadData();
        }
    }

    protected void ddlJobTitle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlJobTitle.SelectedIndex != 0) 
        {
            LoadData();
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("edit_job.aspx");
    }
}