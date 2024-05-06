using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_Sponsor : Page
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
    private Class_SponsorData Sponsors = new Class_SponsorData();
    private Class_BusinessData BusinessData = new Class_BusinessData();
    private int VisitID;

    public Edit_Sponsor()
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
            //Load business names ddl
            Sponsors.LoadSponsorNamesDDL(ddlSponsorName);

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load data
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLStatement = "SELECT id, sponsorName, CONCAT('~/Media/', logoPath) as logoPath, businessID, businessID2, businessID3, businessID4 FROM sponsorsFP";

        //Clear table
        dgvSponsors.DataSource = null;
        dgvSponsors.DataBind();

        //Clear error label
        lblError.Text = "";

        //Check if sponsor name is loaded
        if (ddlSponsorName.SelectedIndex != 0)
        {
            SQLStatement = SQLStatement + " WHERE sponsorName='" + ddlSponsorName.SelectedValue + "'";
        }

        //Load sponsor table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvSponsors.DataSource = Review_sds;
            dgvSponsors.DataBind();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load sponsor table.";
            return;
        }

        //Close connection
        cmd.Dispose();
        con.Close();

        // Highlight row being edited
        foreach (GridViewRow row in dgvSponsors.Rows)
        {
            if (row.RowIndex == dgvSponsors.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }

    protected void dgvSponsors_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(dgvSponsors.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string SponsorName = ((TextBox)dgvSponsors.Rows[e.RowIndex].FindControl("tbSponsorNameDGV")).Text;
        string BusinessName1 = ((DropDownList)dgvSponsors.Rows[e.RowIndex].FindControl("ddlBusinessName1DGV")).SelectedValue;
        string BusinessName2 = ((DropDownList)dgvSponsors.Rows[e.RowIndex].FindControl("ddlBusinessName2DGV")).SelectedValue;
        string BusinessName3 = ((DropDownList)dgvSponsors.Rows[e.RowIndex].FindControl("ddlBusinessName3DGV")).SelectedValue;
        string BusinessName4 = ((DropDownList)dgvSponsors.Rows[e.RowIndex].FindControl("ddlBusinessName4DGV")).SelectedValue;       

        try
        {
            SQL.UpdateRow(ID, "sponsorName", SponsorName, "sponsorsFP");
            SQL.UpdateRow(ID, "businessID", BusinessName1, "sponsorsFP");
            SQL.UpdateRow(ID, "businessID2", BusinessName2, "sponsorsFP");
            SQL.UpdateRow(ID, "businessID3", BusinessName3, "sponsorsFP");
            SQL.UpdateRow(ID, "businessID4", BusinessName4, "sponsorsFP");

            dgvSponsors.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void dgvSponsors_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(dgvSponsors.DataKeys[e.RowIndex].Values[0]); // Gets id number

        try
        {
            SQL.DeleteRow(ID, "sponsorsFP");

            dgvSponsors.EditIndex = -1;       // reset the grid after editing

            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }
    }

    protected void dgvSponsors_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvSponsors.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void dgvSponsors_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvSponsors.EditIndex = -1;
        LoadData();
    }

    protected void dgvSponsors_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvSponsors.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void dgvSponsors_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblBusiness1 = (e.Row.FindControl("lblBusinessName1DGV") as Label).Text;
            DropDownList ddlBusiness1 = e.Row.FindControl("ddlBusinessName1DGV") as DropDownList;
            string lblBusiness2 = (e.Row.FindControl("lblBusinessName2DGV") as Label).Text;
            DropDownList ddlBusiness2 = e.Row.FindControl("ddlBusinessName2DGV") as DropDownList;
            string lblBusiness3 = (e.Row.FindControl("lblBusinessName3DGV") as Label).Text;
            DropDownList ddlBusiness3 = e.Row.FindControl("ddlBusinessName3DGV") as DropDownList;
            string lblBusiness4 = (e.Row.FindControl("lblBusinessName4DGV") as Label).Text;
            DropDownList ddlBusiness4 = e.Row.FindControl("ddlBusinessName4DGV") as DropDownList;

            //Load gridview school DDLs with school names
            Gridviews.BusinessNames(ddlBusiness1, lblBusiness1);
            Gridviews.BusinessNames(ddlBusiness2, lblBusiness2);
            Gridviews.BusinessNames(ddlBusiness3, lblBusiness3);
            Gridviews.BusinessNames(ddlBusiness4, lblBusiness4);
        }
    }


    protected void ddlSponsorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }
}