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
    private Class_VisitData VisitData = new Class_VisitData();
    private Class_SchoolData SchoolData = new Class_SchoolData();
    private Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    private Class_GridviewFunctions Gridviews = new Class_GridviewFunctions();
    private Class_BusinessData BusinessData = new Class_BusinessData();
    private int VisitID;

    public Edit_Job()
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
            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void LoadData()
    {
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
            string lblSchool1 = (e.Row.FindControl("schoolName1DGV_lbl") as Label).Text;
            DropDownList ddlSchool1 = e.Row.FindControl("school1DGV_ddl") as DropDownList;

            //Load gridview school DDLs with school names
            Gridviews.SchoolNames(ddlSchool1, lblSchool1);
        }
    }

    protected void submit_btn_Click(object sender, EventArgs e)
    {

    }

}