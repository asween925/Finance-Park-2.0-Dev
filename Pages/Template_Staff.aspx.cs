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
    private Class_SQLCommands SQL = new Class_SQLCommands();
    private Class_VisitData VisitData = new Class_VisitData();
    private Class_SchoolData SchoolData = new Class_SchoolData();
    private Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    private Class_GridviewFunctions Gridviews = new Class_GridviewFunctions();
    private int VisitID;

    public Template_Staff()
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
        // Highlight row being edited
        //foreach (GridViewRow row in dgvNotes.Rows)
        //{
        //    if (row.RowIndex == dgvNotes.EditIndex) 
        //    {
        //        row.BackColor = ColorTranslator.FromHtml("#ebe534");
        //        row.BorderWidth = 2;
        //    }
        //}
    }

    //protected void dgvNotes_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //    int ID = Convert.ToInt32(dgvNotes.DataKeys[e.RowIndex].Values[0]); // Gets id number
    //    string SchoolName = ((DropDownList)dgvNotes.Rows[e.RowIndex].FindControl("ddlSchoolNameDGV")).SelectedValue;
    //    string Note = ((TextBox)dgvNotes.Rows[e.RowIndex].FindControl("tbNoteDGV")).Text;
    //    string NoteUser = ((Label)dgvNotes.Rows[e.RowIndex].FindControl("lblNoteUserDGV")).Text;
    //    string NoteTimestamp = ((Label)dgvNotes.Rows[e.RowIndex].FindControl("lblNoteTimestampDGV")).Text;


    //    try
    //    {
    //        SQL.UpdateRow(ID, "questionOrder", QuestionOrder, "questionsFP");

    //        dgvNotes.EditIndex = -1;       // reset the grid after editing
    //        LoadData();
    //    }
    //    catch
    //    {
    //        lblError.Text = "Error in rowUpdating. Cannot update row.";
    //        return;
    //    }
    //}

    //protected void dgvNotes_RowDeleting(object sender, GridViewDeleteEventArgs e)
    //{
    //    int ID = Convert.ToInt32(dgvNotes.DataKeys[e.RowIndex].Values[0]); // Gets id number

    //    try
    //    {
    //        SQL.DeleteRow(ID, "dgvNotes");

    //        dgvNotes.EditIndex = -1;       // reset the grid after editing

    //        LoadData();
    //    }
    //    catch
    //    {
    //        lblError.Text = "Error in rowDeleting. Cannot delete row.";
    //        return;
    //    }
    //}

    //protected void dgvNotes_RowEditing(object sender, GridViewEditEventArgs e)
    //{
    //    dgvNotes.EditIndex = e.NewEditIndex;
    //    LoadData();
    //}

    //protected void dgvNotes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    //{
    //    dgvNotes.EditIndex = -1;
    //    LoadData();
    //}

    //protected void dgvNotes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    dgvNotes.PageIndex = e.NewPageIndex;
    //    LoadData();
    //}

    //protected void dgvNotes_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if ((e.Row.RowType == DataControlRowType.DataRow))
    //    {
    //        string lblSchool1 = (e.Row.FindControl("schoolName1DGV_lbl") as Label).Text;
    //        DropDownList ddlSchool1 = e.Row.FindControl("school1DGV_ddl") as DropDownList;

    //        //Load gridview school DDLs with school names
    //        Gridviews.SchoolNames(ddlSchool1, lblSchool1);
    //    }
    //}

}