using Microsoft.SqlServer.Server;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.Remoting.Lifetime;
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
        int VisitID = int.Parse(VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString());
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
        string SQLStatementBusinesses = "SELECT * FROM openStatusFP WHERE visitID=" + VisitID + "";

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

        //Load open / closed businesses
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = SQLStatementBusinesses;
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                Checkbox1.Checked = bool.Parse(dr["open1"].ToString());
                Checkbox6.Checked = bool.Parse(dr["open6"].ToString());
                Checkbox7.Checked = bool.Parse(dr["open7"].ToString());
                Checkbox8.Checked = bool.Parse(dr["open8"].ToString());
                Checkbox9.Checked = bool.Parse(dr["open9"].ToString());
                Checkbox10.Checked = bool.Parse(dr["open10"].ToString());
                Checkbox11.Checked = bool.Parse(dr["open11"].ToString());
                Checkbox12.Checked = bool.Parse(dr["open12"].ToString());
                Checkbox13.Checked = bool.Parse(dr["open13"].ToString());
                Checkbox14.Checked = bool.Parse(dr["open14"].ToString());
                Checkbox15.Checked = bool.Parse(dr["open15"].ToString());
                Checkbox16.Checked = bool.Parse(dr["open16"].ToString());
                Checkbox17.Checked = bool.Parse(dr["open17"].ToString());
                Checkbox18.Checked = bool.Parse(dr["open18"].ToString());
                Checkbox19.Checked = bool.Parse(dr["open19"].ToString());
                Checkbox20.Checked = bool.Parse(dr["open20"].ToString());
                Checkbox21.Checked = bool.Parse(dr["open21"].ToString());
                Checkbox22.Checked = bool.Parse(dr["open22"].ToString());
                Checkbox23.Checked = bool.Parse(dr["open23"].ToString());
                Checkbox24.Checked = bool.Parse(dr["open24"].ToString());
                Checkbox25.Checked = bool.Parse(dr["open25"].ToString());
                Checkbox26.Checked = bool.Parse(dr["open26"].ToString());
                Checkbox27.Checked = bool.Parse(dr["open27"].ToString());
                Checkbox28.Checked = bool.Parse(dr["open28"].ToString());
                Checkbox29.Checked = bool.Parse(dr["open29"].ToString());
                Checkbox30.Checked = bool.Parse(dr["open30"].ToString());
                Checkbox31.Checked = bool.Parse(dr["open31"].ToString());
                Checkbox32.Checked = bool.Parse(dr["open32"].ToString());
            }
        }
        catch
        {

        }
        



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
            //Check if visit date exists
            if (VisitData.ShowVisitConfirmation(tbVisitDate.Text).ToString() != "")
            {
                lblError.Text = VisitData.ShowVisitConfirmation(tbVisitDate.Text).ToString();
                return;
            }

            //Clear error label
            lblError.Text = "";

            //Make divs visible
            divOpen.Visible = true;
            divGridview.Visible = true;

            //Load data
            LoadData();
        }
    }



    protected void btnOpenAll_Click(object sender, EventArgs e)
    {
        if (btnOpenAll.Text == "Open All Businesses")
        {
            //Check off all checkboxes
            Checkbox1.Checked = true;
            Checkbox6.Checked = true;
            Checkbox7.Checked = true;
            Checkbox8.Checked = true;
            Checkbox9.Checked = true;
            Checkbox10.Checked = true;
            Checkbox11.Checked = true;
            Checkbox12.Checked = true;
            Checkbox13.Checked = true;
            Checkbox14.Checked = true;
            Checkbox15.Checked = true;
            Checkbox16.Checked = true;
            Checkbox17.Checked = true;
            Checkbox18.Checked = true;
            Checkbox19.Checked = true;
            Checkbox20.Checked = true;
            Checkbox21.Checked = true;
            Checkbox22.Checked = true;
            Checkbox23.Checked = true;
            Checkbox24.Checked = true;
            Checkbox25.Checked = true;
            Checkbox26.Checked = true;
            Checkbox27.Checked = true;
            Checkbox28.Checked = true;
            Checkbox29.Checked = true;
            Checkbox30.Checked = true;
            Checkbox31.Checked = true;
            Checkbox32.Checked = true;

            //Change text on button to close
            btnOpenAll.Text = "Close All Businesses";
        }
        else
        {
            //Check off all checkboxes
            Checkbox1.Checked = false;
            Checkbox6.Checked = false;
            Checkbox7.Checked = false;
            Checkbox8.Checked = false;
            Checkbox9.Checked = false;
            Checkbox10.Checked = false;
            Checkbox11.Checked = false;
            Checkbox12.Checked = false;
            Checkbox13.Checked = false;
            Checkbox14.Checked = false;
            Checkbox15.Checked = false;
            Checkbox16.Checked = false;
            Checkbox17.Checked = false;
            Checkbox18.Checked = false;
            Checkbox19.Checked = false;
            Checkbox20.Checked = false;
            Checkbox21.Checked = false;
            Checkbox22.Checked = false;
            Checkbox23.Checked = false;
            Checkbox24.Checked = false;
            Checkbox25.Checked = false;
            Checkbox26.Checked = false;
            Checkbox27.Checked = false;
            Checkbox28.Checked = false;
            Checkbox29.Checked = false;
            Checkbox30.Checked = false;
            Checkbox31.Checked = false;
            Checkbox32.Checked = false;

            //Change text on button to open
            btnOpenAll.Text = "Open All Businesses";
        }
    }

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        int VisitID = int.Parse(VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString());

        //Update openStatusFP
        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"UPDATE openStatusFP SET open1=@open1, open6=@open6, open7=@open7, open8=@open8, open9=@open9, open10=@open10, open11=@open11, open12=@open12, open13=@open13, open14=@open14, open15=@open15, open16=@open16, open17=@open17, open18=@open18, open19=@open19, open20=@open20, open21=@open21, open22=@open22, open23=@open23, open24=@open24, open25=@open25, open26=@open26, open27=@open27, open28=@open28, open29=@open29, open30=@open30, open31=@open31, open32=@open32 WHERE visitID=@visitID"))

                {
                    // Date that is inputed in the textbox
                    cmd.Parameters.Add("@visitID", SqlDbType.Int).Value = VisitID;
                    cmd.Parameters.Add("@open1", SqlDbType.Bit).Value = Checkbox1.Checked;
                    cmd.Parameters.Add("@open6", SqlDbType.Bit).Value = Checkbox6.Checked;
                    cmd.Parameters.Add("@open7", SqlDbType.Bit).Value = Checkbox7.Checked;
                    cmd.Parameters.Add("@open8", SqlDbType.Bit).Value = Checkbox8.Checked;
                    cmd.Parameters.Add("@open9", SqlDbType.Bit).Value = Checkbox9.Checked;
                    cmd.Parameters.Add("@open10", SqlDbType.Bit).Value = Checkbox10.Checked;
                    cmd.Parameters.Add("@open11", SqlDbType.Bit).Value = Checkbox11.Checked;
                    cmd.Parameters.Add("@open12", SqlDbType.Bit).Value = Checkbox12.Checked;
                    cmd.Parameters.Add("@open13", SqlDbType.Bit).Value = Checkbox13.Checked;
                    cmd.Parameters.Add("@open14", SqlDbType.Bit).Value = Checkbox14.Checked;
                    cmd.Parameters.Add("@open15", SqlDbType.Bit).Value = Checkbox15.Checked;
                    cmd.Parameters.Add("@open16", SqlDbType.Bit).Value = Checkbox16.Checked;
                    cmd.Parameters.Add("@open17", SqlDbType.Bit).Value = Checkbox17.Checked;
                    cmd.Parameters.Add("@open18", SqlDbType.Bit).Value = Checkbox18.Checked;
                    cmd.Parameters.Add("@open19", SqlDbType.Bit).Value = Checkbox19.Checked;
                    cmd.Parameters.Add("@open20", SqlDbType.Bit).Value = Checkbox20.Checked;
                    cmd.Parameters.Add("@open21", SqlDbType.Bit).Value = Checkbox21.Checked;
                    cmd.Parameters.Add("@open22", SqlDbType.Bit).Value = Checkbox22.Checked;
                    cmd.Parameters.Add("@open23", SqlDbType.Bit).Value = Checkbox23.Checked;
                    cmd.Parameters.Add("@open24", SqlDbType.Bit).Value = Checkbox24.Checked;
                    cmd.Parameters.Add("@open25", SqlDbType.Bit).Value = Checkbox25.Checked;
                    cmd.Parameters.Add("@open26", SqlDbType.Bit).Value = Checkbox26.Checked;
                    cmd.Parameters.Add("@open27", SqlDbType.Bit).Value = Checkbox27.Checked;
                    cmd.Parameters.Add("@open28", SqlDbType.Bit).Value = Checkbox28.Checked;
                    cmd.Parameters.Add("@open29", SqlDbType.Bit).Value = Checkbox29.Checked;
                    cmd.Parameters.Add("@open30", SqlDbType.Bit).Value = Checkbox30.Checked;
                    cmd.Parameters.Add("@open31", SqlDbType.Bit).Value = Checkbox31.Checked;
                    cmd.Parameters.Add("@open32", SqlDbType.Bit).Value = Checkbox32.Checked;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch
        {
            lblError.Text = "Error in Submit(). Could not edit opened/closed businesses.";
            return;
        }
    }
}