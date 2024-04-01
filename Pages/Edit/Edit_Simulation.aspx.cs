using Microsoft.SqlServer.Server;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_Simulation : Page
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
    private int VisitID;

    public Edit_Simulation()
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
            // Assign current visit ID to hidden field
            if (VisitID != 0)
            {
                hfCurrentVisitID.Value = VisitID.ToString();
            }

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void LoadData()
    {
        string VisitDate = tbVisitDate.Text;
        string SQLStatement = @"SELECT v.id, s.id as 'schoolid1', s2.id as 'schoolid2', s3.id as 'schoolid3', s4.id as 'schoolid4', s5.id as 'schoolid5'
                                , s.schoolName as 'schoolname1', s2.schoolName as 'schoolname2', s3.schoolName as 'schoolname3', s4.schoolName as 'schoolname4', s5.schoolName as 'schoolname5'
                                , v.vTrainingTime, v.vMinCount, v.vMaxCount, FORMAT(v.visitDate, 'yyyy-MM-dd') as visitDate, v.studentCount, v.visitTime, FORMAT(v.dueBy, 'yyyy-MM-dd') as dueBy 
                                FROM visitInfoFP v 
                                LEFT JOIN schoolInfoFP s ON s.ID = v.school 
                                LEFT JOIN schoolInfoFP s2 ON s2.ID = v.school2 
                                LEFT JOIN schoolInfoFP s3 ON s3.ID = v.school3 
                                LEFT JOIN schoolInfoFP s4 ON s4.ID = v.school4 
                                LEFT JOIN schoolInfoFP s5 ON s5.ID = v.school5 
                                WHERE v.visitDate = '" + VisitDate + "'";

        //Clear table
        dgvVisit.DataSource = null;
        dgvVisit.DataBind();

        //Clear error label
        lblError.Text = "";

        //Load visit table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvVisit.DataSource = Review_sds;
            dgvVisit.DataBind();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load visit table.";
            return;
        }

        //Close connection
        cmd.Dispose();
            con.Close();
      
        // Highlight row being edited
        foreach (GridViewRow row in dgvVisit.Rows)
        {
            if (row.RowIndex == dgvVisit.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }

    public void UpdateCurrentVisitDate(string VisitDate, string SchoolID)
    {
        string CurrentVisitDate = SchoolData.GetCurrentVisitDate(SchoolID).ToString();

        if (CurrentVisitDate != VisitDate)
        {
            try
            {
                SchoolData.UpdatePreviousVisitDate(SchoolID);
                SchoolData.UpdateCurrentVisitDate(SchoolID, VisitDate);

                dgvVisit.EditIndex = -1;
                LoadData();
            }
            catch
            {
                lblError.Text = "Error in UpdateCurrentVisitDate. Cannot update either the previous or current visit date.";
                return;
            }
        }
    }



    protected void dgvVisit_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(dgvVisit.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string VisitDate = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbVisitDateDGV")).Text;
        string VisitTime = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbVisitTimeDGV")).Text;
        string School1 = ((DropDownList)dgvVisit.Rows[e.RowIndex].FindControl("ddlSchool1DGV")).SelectedValue;
        string School2 = ((DropDownList)dgvVisit.Rows[e.RowIndex].FindControl("ddlSchool2DGV")).SelectedValue;
        string School3 = ((DropDownList)dgvVisit.Rows[e.RowIndex].FindControl("ddlSchool3DGV")).SelectedValue;
        string School4 = ((DropDownList)dgvVisit.Rows[e.RowIndex].FindControl("ddlSchool4DGV")).SelectedValue;
        string School5 = ((DropDownList)dgvVisit.Rows[e.RowIndex].FindControl("ddlSchool5DGV")).SelectedValue;
        string VTrainingTime = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbVTrainingTimeDGV")).Text;
        string VMin = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbVMinCountDGV")).Text;
        string VMax = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbVMaxCountDGV")).Text;
        string DueBy = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbDueByDGV")).Text;
        string StudentCount = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbStudentCountDGV")).Text;

        //Update the row
        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE visitInfoFP SET school=@school1, vTrainingTime=@vTrainingTime, vMinCount=@vMinCount, vMaxCount=@vMaxCount, dueBy=@dueBy, visitDate=@visitDate, studentCount=@studentCount, school2=@school2, school3=@school3, school4=@school4, visitTime=@visitTime, school5=@school5 WHERE ID=@Id"))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@visitDate", VisitDate);
                    cmd.Parameters.AddWithValue("@visitTime", VisitTime);
                    cmd.Parameters.AddWithValue("@school1", School1);
                    cmd.Parameters.AddWithValue("@school2", School2);
                    cmd.Parameters.AddWithValue("@school3", School3);
                    cmd.Parameters.AddWithValue("@school4", School4);
                    cmd.Parameters.AddWithValue("@school5", School5);
                    cmd.Parameters.AddWithValue("@vTrainingTime", VTrainingTime);
                    cmd.Parameters.AddWithValue("@vMinCount", VMin);
                    cmd.Parameters.AddWithValue("@vMaxCount", VMax);
                    cmd.Parameters.AddWithValue("@dueBy", DueBy);
                    cmd.Parameters.AddWithValue("@studentCount", StudentCount);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dgvVisit.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowUpdating(). Cannot update row.";
            return;
        }

        //Update previous visit date with new visit date if visit date has changed
        UpdateCurrentVisitDate(VisitDate, School1);
        UpdateCurrentVisitDate(VisitDate, School2);
        UpdateCurrentVisitDate(VisitDate, School3);
        UpdateCurrentVisitDate(VisitDate, School4);
        UpdateCurrentVisitDate(VisitDate, School5);

    }

    protected void dgvVisit_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvVisit.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void dgvVisit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvVisit.EditIndex = -1;
        LoadData();
    }

    protected void dgvVisit_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvVisit.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void dgvVisit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblSchool1 = (e.Row.FindControl("lblSchoolName1DGV") as Label).Text;
            string lblSchool2 = (e.Row.FindControl("lblSchoolName2DGV") as Label).Text;
            string lblSchool3 = (e.Row.FindControl("lblSchoolName3DGV") as Label).Text;
            string lblSchool4 = (e.Row.FindControl("lblSchoolName4DGV") as Label).Text;
            string lblSchool5 = (e.Row.FindControl("lblSchoolName5DGV") as Label).Text;
            DropDownList ddlSchool1 = e.Row.FindControl("ddlSchool1DGV") as DropDownList;
            DropDownList ddlSchool2 = e.Row.FindControl("ddlSchool2DGV") as DropDownList;
            DropDownList ddlSchool3 = e.Row.FindControl("ddlSchool3DGV") as DropDownList;
            DropDownList ddlSchool4 = e.Row.FindControl("ddlSchool4DGV") as DropDownList;
            DropDownList ddlSchool5 = e.Row.FindControl("ddlSchool5DGV") as DropDownList;

            //Load gridview school DDLs with school names
            Gridviews.SchoolNames(ddlSchool1, lblSchool1);
            Gridviews.SchoolNames(ddlSchool2, lblSchool2);
            Gridviews.SchoolNames(ddlSchool3, lblSchool3);
            Gridviews.SchoolNames(ddlSchool4, lblSchool4);
            Gridviews.SchoolNames(ddlSchool5, lblSchool5);

        }
    }



    protected void tbVisitDate_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDate.Text != "")
        {
            divGridview.Visible = true;
            LoadData();
        }
    }
}