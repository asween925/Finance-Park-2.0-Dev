using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Template_Staff : Page
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
    private int VisitID;

    public Template_Staff()
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
        // Highlight row being edited
        //foreach (GridViewRow row in notes_dgv.Rows)
        //{
        //    if (row.RowIndex == notes_dgv.EditIndex) 
        //    {
        //        row.BackColor = ColorTranslator.FromHtml("#ebe534");
        //        row.BorderWidth = 2;
        //    }
        //}
    }

    //private void notes_dgv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    int ID = Convert.ToInt32(notes_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number
    //    string SchoolName = ((DropDownList)notes_dgv.Rows[e.RowIndex].FindControl("schoolNameDGV_ddl")).SelectedValue;
    //    string Note = ((TextBox)notes_dgv.Rows[e.RowIndex].FindControl("noteDGV_tb")).Text;
    //    string NoteUser = ((Label)notes_dgv.Rows[e.RowIndex].FindControl("noteUserDGV_lbl")).Text;
    //    string NoteTimestamp = ((Label)notes_dgv.Rows[e.RowIndex].FindControl("noteTimestampDGV_lbl")).Text;


    //    try
    //    {
    //        using (SqlConnection con = new SqlConnection(ConnectionString))
    //        {
    //            using (SqlCommand cmd = new SqlCommand("UPDATE schoolNotesFP SET schoolName=@schoolName, note=@note, noteUser=@noteUser, noteTimestamp=@noteTimestamp WHERE ID=@Id"))
    //            {
    //                cmd.Parameters.AddWithValue("@ID", ID);
    //                cmd.Parameters.AddWithValue("@schoolName", SchoolName);
    //                cmd.Parameters.AddWithValue("@note", Note);
    //                cmd.Parameters.AddWithValue("@noteUser", NoteUser);
    //                cmd.Parameters.AddWithValue("@noteTimestamp", NoteTimestamp);
    //                cmd.Connection = con;
    //                con.Open();
    //                cmd.ExecuteNonQuery();
    //                con.Close();
    //            }
    //        }
    //        notes_dgv.EditIndex = -1;       // reset the grid after editing
    //        LoadData();
    //    }
    //    catch
    //    {
    //        error_lbl.Text = "Error in rowUpdating. Cannot update row.";
    //        return;
    //    }
    //}

    //private void notes_dgv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    int ID = Convert.ToInt32(notes_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number

    //    try
    //    {
    //        using (SqlConnection con = new SqlConnection(ConnectionString))
    //        {
    //            using (SqlCommand cmd = new SqlCommand("DELETE FROM schoolNotesFP WHERE id=@ID"))
    //            {
    //                cmd.Parameters.AddWithValue("@ID", ID);
    //                cmd.Connection = con;
    //                con.Open();
    //                cmd.ExecuteNonQuery();
    //                con.Close();
    //            }
    //        }
    //        notes_dgv.EditIndex = -1;       // reset the grid after editing

    //        LoadData();
    //    }
    //    catch
    //    {
    //        error_lbl.Text = "Error in rowDeleting. Cannot delete row.";
    //        return;
    //    }
    //}

    //private void notes_dgv_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    notes_dgv.EditIndex = e.NewEditIndex;
    //    LoadData();
    //}

    //private void notes_dgv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    notes_dgv.EditIndex = -1;
    //    LoadData();
    //}

    //private void notes_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    notes_dgv.PageIndex = e.NewPageIndex;
    //    LoadData();
    //}

    //private void notes_dgv_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if ((e.Row.RowType == DataControlRowType.DataRow))
    //    {
    //        string lblSchool1 = (e.Row.FindControl("schoolName1DGV_lbl") as Label).Text;
    //        DropDownList ddlSchool1 = e.Row.FindControl("school1DGV_ddl") as DropDownList;

    //        //Load gridview school DDLs with school names
    //        Gridviews.SchoolNames(ddlSchool1, lblSchool1);
    //    }
    //}

    //private DataSet GetData(string query)
    //{
    //    var cmd = new SqlCommand(query);
    //    using (var con = new SqlConnection(ConnectionString))
    //    {
    //        using (var sda = new SqlDataAdapter())
    //        {
    //            cmd.Connection = con;
    //            sda.SelectCommand = cmd;
    //            using (var ds = new DataSet())
    //            {
    //                sda.Fill(ds);
    //                return ds;
    //            }
    //        }
    //    }
    //}

}