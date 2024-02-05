using Microsoft.SqlServer.Server;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_Visit : Page
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

    public Edit_Visit()
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
            // Assign current visit ID to hidden field
            if (VisitID != 0)
            {
                currentVisitID_hf.Value = VisitID.ToString();
            }

            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void LoadData()
    {
        string VisitDate = visitDate_tb.Text;
        string SQLStatement = "SELECT v.id, s.id as 'schoolid1', s2.id as 'schoolid2', s3.id as 'schoolid3', s4.id as 'schoolid4', s5.id as 'schoolid5', s.schoolName as 'schoolname1', s2.schoolName as 'schoolname2', s3.schoolName as 'schoolname3', s4.schoolName as 'schoolname4', s5.schoolName as 'schoolname5', v.vTrainingTime, v.vMinCount, v.vMaxCount, v.replyBy, v.visitDate, v.studentCount, v.visitTime FROM visitInfoFP v LEFT JOIN schoolInfoFP s ON s.ID = v.school LEFT JOIN schoolInfoFP s2 ON s2.ID = v.school2 LEFT JOIN schoolInfoFP s3 ON s3.ID = v.school3 LEFT JOIN schoolInfoFP s4 ON s4.ID = v.school4 LEFT JOIN schoolInfoFP s5 ON s5.ID = v.school5 WHERE v.visitDate = '" + VisitDate + "'";

        //Clear table
        visit_dgv.DataSource = null;
        visit_dgv.DataBind();

        //Clear error label
        error_lbl.Text = "";

        //Load visit table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            visit_dgv.DataSource = Review_sds;
            visit_dgv.DataBind();

        }
        catch
        {
            error_lbl.Text = "Error in LoadData). Cannot load visit table.";
            return;
        }

        //Close connection
        cmd.Dispose();
        con.Close();

        // Highlight row being edited
        foreach (GridViewRow row in visit_dgv.Rows)
        {
            if (row.RowIndex == visit_dgv.EditIndex)
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

                visit_dgv.EditIndex = -1;
                LoadData();
            }
            catch
            {
                error_lbl.Text = "Error in UpdateCurrentVisitDate. Cannot update either the previous or current visit date.";
                return;
            }
        }
    }



    protected void visit_dgv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(visit_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string VisitDate = ((TextBox)visit_dgv.Rows[e.RowIndex].FindControl("visitDateDGV_tb")).Text;
        string VisitTime = ((TextBox)visit_dgv.Rows[e.RowIndex].FindControl("visitTimeDGV_tb")).Text;
        string School1 = ((DropDownList)visit_dgv.Rows[e.RowIndex].FindControl("school1DGV_ddl")).SelectedValue;
        string School2 = ((DropDownList)visit_dgv.Rows[e.RowIndex].FindControl("school2DGV_ddl")).SelectedValue;
        string School3 = ((DropDownList)visit_dgv.Rows[e.RowIndex].FindControl("school3DGV_ddl")).SelectedValue;
        string School4 = ((DropDownList)visit_dgv.Rows[e.RowIndex].FindControl("school4DGV_ddl")).SelectedValue;
        string School5 = ((DropDownList)visit_dgv.Rows[e.RowIndex].FindControl("school5DGV_ddl")).SelectedValue;
        string VTrainingTime = ((TextBox)visit_dgv.Rows[e.RowIndex].FindControl("vTrainingTimeDGV_tb")).Text;
        string VMin = ((TextBox)visit_dgv.Rows[e.RowIndex].FindControl("vMinCountDGV_tb")).Text;
        string VMax = ((TextBox)visit_dgv.Rows[e.RowIndex].FindControl("vMaxCountDGV_tb")).Text;
        string ReplyBy = ((TextBox)visit_dgv.Rows[e.RowIndex].FindControl("replyByDGV_tb")).Text;
        string StudentCount = ((TextBox)visit_dgv.Rows[e.RowIndex].FindControl("studentCountDGV_tb")).Text;

        //Update the row
        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE visitInfoFP SET school=@school1, vTrainingTime=@vTrainingTime, vMinCount=@vMinCount, vMaxCount=@vMaxCount, replyBy=@replyBy, visitDate=@visitDate, studentCount=@studentCount, school2=@school2, school3=@school3, school4=@school4, visitTime=@visitTime, school5=@school5 WHERE ID=@Id"))
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
                    cmd.Parameters.AddWithValue("@replyBy", ReplyBy);
                    cmd.Parameters.AddWithValue("@studentCount", StudentCount);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            visit_dgv.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowUpdating(). Cannot update row.";
            return;
        }

        //Update previous visit date with new visit date if visit date has changed
        UpdateCurrentVisitDate(VisitDate, School1);
        UpdateCurrentVisitDate(VisitDate, School2);
        UpdateCurrentVisitDate(VisitDate, School3);
        UpdateCurrentVisitDate(VisitDate, School4);
        UpdateCurrentVisitDate(VisitDate, School5);
    }

    protected void visit_dgv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        visit_dgv.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void visit_dgv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        visit_dgv.EditIndex = -1;
        LoadData();
    }

    protected void visit_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        visit_dgv.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void visit_dgv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblSchool1 = (e.Row.FindControl("schoolName1DGV_lbl") as Label).Text;
            string lblSchool2 = (e.Row.FindControl("schoolName2DGV_lbl") as Label).Text;
            string lblSchool3 = (e.Row.FindControl("schoolName3DGV_lbl") as Label).Text;
            string lblSchool4 = (e.Row.FindControl("schoolName4DGV_lbl") as Label).Text;
            string lblSchool5 = (e.Row.FindControl("schoolName5DGV_lbl") as Label).Text;
            DropDownList ddlSchool1 = e.Row.FindControl("school1DGV_ddl") as DropDownList;
            DropDownList ddlSchool2 = e.Row.FindControl("school2DGV_ddl") as DropDownList;
            DropDownList ddlSchool3 = e.Row.FindControl("school3DGV_ddl") as DropDownList;
            DropDownList ddlSchool4 = e.Row.FindControl("school4DGV_ddl") as DropDownList;
            DropDownList ddlSchool5 = e.Row.FindControl("school5DGV_ddl") as DropDownList;

            //Load gridview school DDLs with school names
            Gridviews.SchoolNames(ddlSchool1, lblSchool1);
            Gridviews.SchoolNames(ddlSchool2, lblSchool2);
            Gridviews.SchoolNames(ddlSchool3, lblSchool3);
            Gridviews.SchoolNames(ddlSchool4, lblSchool4);
            Gridviews.SchoolNames(ddlSchool5, lblSchool5);

        }
    }



    protected void visitDate_tb_TextChanged(object sender, EventArgs e)
    {
        if (visitDate_tb.Text != "")
        {
            gridview_div.Visible = true;
            LoadData();
        }
    }
}