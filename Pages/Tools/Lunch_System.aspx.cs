using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Lunch_System : Page
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
    private Class_StudentData StudentData = new Class_StudentData();
    private int VisitID;

    public Lunch_System()
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
        int VisitID;

        //Clear error
        error_lbl.Text = "";
        
        //Check if visit date is an ID
        if (VisitData.GetVisitIDFromDate(VisitDate).ToString() == "0")
        {
            error_lbl.Text = "Visit date entered is not scheduled.";
            return;
        }
        else
        {
            VisitID = int.Parse(VisitData.GetVisitIDFromDate(VisitDate).ToString());
        }

        //Load lunches table
        lunches_dgv.DataSource = StudentData.LoadLunchesTable(VisitID);
        lunches_dgv.DataBind();

        //Make table visible
        lunches_div.Visible = true;

        // Highlight row being edited
        foreach (GridViewRow row in lunches_dgv.Rows)
        {
            if (row.RowIndex == lunches_dgv.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }



    protected void lunches_dgv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(lunches_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number
        bool LunchServed = ((CheckBox)lunches_dgv.Rows[e.RowIndex].FindControl("lunchServedDGV_chk")).Checked;
        string LunchTicket = ((DropDownList)lunches_dgv.Rows[e.RowIndex].FindControl("lunchTicketDGV_ddl")).SelectedValue;

        try
        {
            SQL.UpdateRow(ID, "lunchServed", LunchServed.ToString(), "studentInfoFP");
            SQL.UpdateRow(ID, "lunchTicket", LunchTicket, "studentInfoFP");

            lunches_dgv.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void lunches_dgv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        lunches_dgv.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void lunches_dgv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        lunches_dgv.EditIndex = -1;
        LoadData();
    }

    protected void lunches_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        lunches_dgv.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void lunches_dgv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblLunch = (e.Row.FindControl("lunchTicketDGV_lbl") as Label).Text;
            DropDownList ddlLunch = e.Row.FindControl("lunchTicketDGV_ddl") as DropDownList;

            //Load gridview school DDLs with school names
            Gridviews.SchoolNames(ddlLunch, lblLunch);
        }
    }


    protected void visitDate_tb_TextChanged(object sender, EventArgs e)
    {
        if (visitDate_tb.Text != "")
        {
            LoadData();
        }
    }
}