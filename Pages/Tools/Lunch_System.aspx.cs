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
        int VisitID;

        //Clear error
        lblError.Text = "";
        
        //Check if visit date is an ID
        if (VisitData.GetVisitIDFromDate(VisitDate).ToString() == "0")
        {
            lblError.Text = "Visit date entered is not scheduled.";
            return;
        }
        else
        {
            VisitID = int.Parse(VisitData.GetVisitIDFromDate(VisitDate).ToString());
        }

        //Load lunches table
        dgvLunches.DataSource = StudentData.LoadLunchesTable(VisitID);
        dgvLunches.DataBind();

        //Make table visible
        divLunches.Visible = true;

        // Highlight row being edited
        foreach (GridViewRow row in dgvLunches.Rows)
        {
            if (row.RowIndex == dgvLunches.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }



    protected void dgvLunches_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(dgvLunches.DataKeys[e.RowIndex].Values[0]); // Gets id number
        bool LunchServed = ((CheckBox)dgvLunches.Rows[e.RowIndex].FindControl("chkLunchServedDGV")).Checked;

        try
        {
            SQL.UpdateRow(ID, "lunchServed", LunchServed.ToString(), "studentInfoFP");

            dgvLunches.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void dgvLunches_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvLunches.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void dgvLunches_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvLunches.EditIndex = -1;
        LoadData();
    }

    protected void dgvLunches_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvLunches.PageIndex = e.NewPageIndex;
        LoadData();
    }


    protected void tbVisitDate_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDate.Text != "")
        {
            LoadData();
        }
    }
}