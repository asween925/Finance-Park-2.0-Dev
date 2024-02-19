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
            //Load business names ddl
            Sponsors.LoadSponsorNamesDDL(sponsorName_ddl);

            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load data
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLStatement = "SELECT * FROM sponsorsFP";

        //Clear table
        sponsors_dgv.DataSource = null;
        sponsors_dgv.DataBind();

        //Clear error label
        error_lbl.Text = "";

        //Check if sponsor name is loaded
        if (sponsorName_ddl.SelectedIndex != 0)
        {
            SQLStatement = SQLStatement + " WHERE sponsorName='" + sponsorName_ddl.SelectedValue + "'";
        }

        //Load sponsor table
        //try
        //{
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            sponsors_dgv.DataSource = Review_sds;
            sponsors_dgv.DataBind();

        //}
        //catch
        //{
        //    error_lbl.Text = "Error in LoadData). Cannot load sponsor table.";
        //    return;
        //}

        //Close connection
        cmd.Dispose();
        con.Close();

        // Highlight row being edited
        foreach (GridViewRow row in sponsors_dgv.Rows)
        {
            if (row.RowIndex == sponsors_dgv.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }

    protected void sponsors_dgv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(sponsors_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string SponsorName = ((TextBox)sponsors_dgv.Rows[e.RowIndex].FindControl("sponsorNameDGV_tb")).Text;
        string BusinessName1 = ((DropDownList)sponsors_dgv.Rows[e.RowIndex].FindControl("businessNameDGV_ddl")).SelectedValue;
        string BusinessName2 = ((DropDownList)sponsors_dgv.Rows[e.RowIndex].FindControl("businessName2DGV_ddl")).SelectedValue;
        string BusinessName3 = ((DropDownList)sponsors_dgv.Rows[e.RowIndex].FindControl("businessName3DGV_ddl")).SelectedValue;
        string BusinessName4 = ((DropDownList)sponsors_dgv.Rows[e.RowIndex].FindControl("businessName4DGV_ddl")).SelectedValue;       

        try
        {
            SQL.UpdateRow(ID, "sponsorName", SponsorName, "sponsorsFP");
            SQL.UpdateRow(ID, "businessID", BusinessName1, "sponsorsFP");
            SQL.UpdateRow(ID, "businessID2", BusinessName2, "sponsorsFP");
            SQL.UpdateRow(ID, "businessID3", BusinessName3, "sponsorsFP");
            SQL.UpdateRow(ID, "businessID4", BusinessName4, "sponsorsFP");

            sponsors_dgv.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void sponsors_dgv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(sponsors_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number

        try
        {
            SQL.DeleteRow(ID, "sponsorsFP");

            sponsors_dgv.EditIndex = -1;       // reset the grid after editing

            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }
    }

    protected void sponsors_dgv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        sponsors_dgv.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void sponsors_dgv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        sponsors_dgv.EditIndex = -1;
        LoadData();
    }

    protected void sponsors_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        sponsors_dgv.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void sponsors_dgv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblBusiness1 = (e.Row.FindControl("businessName1DGV_lbl") as Label).Text;
            DropDownList ddlBusiness1 = e.Row.FindControl("businessName1DGV_ddl") as DropDownList;
            string lblBusiness2 = (e.Row.FindControl("businessName2DGV_lbl") as Label).Text;
            DropDownList ddlBusiness2 = e.Row.FindControl("businessName2DGV_ddl") as DropDownList;
            string lblBusiness3 = (e.Row.FindControl("businessName3DGV_lbl") as Label).Text;
            DropDownList ddlBusiness3 = e.Row.FindControl("businessName3DGV_ddl") as DropDownList;
            string lblBusiness4 = (e.Row.FindControl("businessName4DGV_lbl") as Label).Text;
            DropDownList ddlBusiness4 = e.Row.FindControl("businessName4DGV_ddl") as DropDownList;

            //Load gridview school DDLs with school names
            Gridviews.BusinessNames(ddlBusiness1, lblBusiness1);
            Gridviews.BusinessNames(ddlBusiness2, lblBusiness2);
            Gridviews.BusinessNames(ddlBusiness3, lblBusiness3);
            Gridviews.BusinessNames(ddlBusiness4, lblBusiness4);
        }
    }


    protected void sponsorName_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadData();
    }
}