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
            JobData.LoadJobsDDL(jobTitle_ddl);

            // Populating school header
            headerSchoolName_lbl.Text = SchoolHeader.GetSchoolHeader().ToString();

            //Load table
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLStatement = "SELECT * FROM jobsFP";

        //Clear error
        error_lbl.Text = "";

        //Clear table
        jobs_dgv.DataSource = null;
        jobs_dgv.DataBind();

        //If loading by the DDL, add school name to search query
        if (jobTitle_ddl.SelectedIndex != 0)
        {
            SQLStatement = SQLStatement + " WHERE jobTitle='" + jobTitle_ddl.SelectedValue + "'";
        }
        else if (search_tb.Text != "")
        {
            SQLStatement = SQLStatement + " WHERE jobTitle LIKE '%" + search_tb.Text + "%'";
        }
        else
        {
            SQLStatement = SQLStatement + " ORDER BY jobTitle ASC";
        }

        //Load schoolInfoFP table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            jobs_dgv.DataSource = Review_sds;
            jobs_dgv.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            error_lbl.Text = "Error in LoadData(). Cannot load jobs table.";
            return;
        }

        // Highlight row being edited
        foreach (GridViewRow row in jobs_dgv.Rows)
        {
            if (row.RowIndex == jobs_dgv.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }

    protected void jobs_dgv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(jobs_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string JobTitle = ((DropDownList)jobs_dgv.Rows[e.RowIndex].FindControl("jobTitleDGV_tb")).Text;
        string BusinessName = ((DropDownList)jobs_dgv.Rows[e.RowIndex].FindControl("businessNameDGV_ddl")).SelectedValue;
        string EducationBG = ((DropDownList)jobs_dgv.Rows[e.RowIndex].FindControl("educationBGDGV_lbl")).SelectedValue;
        string JobDuties = ((HtmlTextArea)jobs_dgv.Rows[e.RowIndex].FindControl("jobDutiesDGV_lbl")).InnerText;
        string EdDebt = ((TextBox)jobs_dgv.Rows[e.RowIndex].FindControl("edDebtDGV_tb")).Text;
        string Advancements = ((HtmlTextArea)jobs_dgv.Rows[e.RowIndex].FindControl("advancementsDGV_lbl")).InnerText;
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
                    cmd.Parameters.AddWithValue("@advancement", Advancements);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            jobs_dgv.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void jobs_dgv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(jobs_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number

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
            jobs_dgv.EditIndex = -1;       // reset the grid after editing

            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }
    }

    protected void jobs_dgv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        jobs_dgv.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void jobs_dgv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        jobs_dgv.EditIndex = -1;
        LoadData();
    }

    protected void jobs_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        jobs_dgv.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void jobs_dgv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblBusiness = (e.Row.FindControl("businessNameDGV_lbl") as Label).Text;
            DropDownList ddlBusiness = e.Row.FindControl("businessNameDGV_ddl") as DropDownList;

            //Load gridview school DDLs with school names
            Gridviews.BusinessNames(ddlBusiness, lblBusiness);

            string lblEdBG = (e.Row.FindControl("educationBGDGV_lbl") as Label).Text;
            DropDownList ddlEdBG = e.Row.FindControl("educationBGDGV_ddl") as DropDownList;

            //Select business name from label
            ddlEdBG.SelectedValue = lblEdBG;

        }
    }



    protected void submit_btn_Click(object sender, EventArgs e)
    {
        if (search_tb.Text != "")
        {
            LoadData();
        }
    }

    protected void jobTitle_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (jobTitle_ddl.SelectedIndex != 0) 
        {
            LoadData();
        }
    }

    protected void refresh_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("edit_job.aspx");
    }
}