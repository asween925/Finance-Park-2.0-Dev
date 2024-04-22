using Microsoft.Owin.BuilderProperties;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Security.Policy;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_School : Page
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

    public Edit_School()
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
            //Populate school name ddl
            SchoolData.LoadSchoolsDDL(ddlSchoolName, false);

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load schoolInfoFP table
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLStatement = "SELECT * FROM schoolInfoFP";

        //Clear error
        lblError.Text = "";

        //Clear table
        dgvSchool.DataSource = null;
        dgvSchool.DataBind();

        //If loading by the DDL, add school name to search query
        if (ddlSchoolName.SelectedIndex != 0)
        {
            SQLStatement = SQLStatement + " WHERE schoolName='" + ddlSchoolName.SelectedValue + "'";
        }
        else if (tbSearch.Text != "")
        {
            SQLStatement = SQLStatement + " WHERE schoolName LIKE '%" + tbSearch.Text + "%'";
        }
        else
        {
            SQLStatement = SQLStatement + " WHERE NOT id='1' ORDER BY schoolName ASC";
        }

        //Load schoolInfoFP table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvSchool.DataSource = Review_sds;
            dgvSchool.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load schoolInfo table.";
            return;
        }

        // Highlight row being edited
        foreach (GridViewRow row in dgvSchool.Rows)
        {
            if (row.RowIndex == dgvSchool.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }



    protected void dgvSchool_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(dgvSchool.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string SchoolName = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbSchoolNameDGV")).Text;
        string Address = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbAddressDGV")).Text;
        string City = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbCityDGV")).Text;
        string Zip = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbZipDGV")).Text;
        string County = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbCountyDGV")).Text;
        string PrincipalFirst = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbPrincipalFirstDGV")).Text;
        string PrincipalLast = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbPrincipalLastDGV")).Text;
        string AdminEmail = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbAdministratorEmailDGV")).Text;
        string Phone = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbPhoneDGV")).Text;
        string Hours = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbHoursDGV")).Text;
        string SchoolType = ((DropDownList)dgvSchool.Rows[e.RowIndex].FindControl("ddlSchoolTypeDGV")).SelectedValue;
        string Notes = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbNotesDGV")).Text;
        string LiaisonName = ((TextBox)dgvSchool.Rows[e.RowIndex].FindControl("tbLiaisonNameDGV")).Text;

        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE schoolInfoFP SET schoolName=@schoolName, principalFirst=@principalFirst, principalLast=@principalLast, phone=@phone, schoolHours=@schoolHours, schoolType=@schoolType, administratorEmail=@adminEmail, notes=@notes, address=@address, city=@city, zip=@zip, county=@county, liaisonName=@liaisonName WHERE ID=@Id"))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@schoolName", SchoolName);
                    cmd.Parameters.AddWithValue("@principalFirst", PrincipalFirst);
                    cmd.Parameters.AddWithValue("@principalLast", PrincipalLast);
                    cmd.Parameters.AddWithValue("@phone", Phone);
                    cmd.Parameters.AddWithValue("@schoolHours", Hours);
                    cmd.Parameters.AddWithValue("@schoolType", SchoolType);
                    cmd.Parameters.AddWithValue("@adminEmail", AdminEmail);
                    cmd.Parameters.AddWithValue("@notes", Notes);
                    cmd.Parameters.AddWithValue("@address", Address);
                    cmd.Parameters.AddWithValue("@city", City);
                    cmd.Parameters.AddWithValue("@zip", Zip);
                    cmd.Parameters.AddWithValue("@county", County);
                    cmd.Parameters.AddWithValue("@liaisonName", LiaisonName);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dgvSchool.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void dgvSchool_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvSchool.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void dgvSchool_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvSchool.EditIndex = -1;
        LoadData();
    }

    protected void dgvSchool_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvSchool.PageIndex = e.NewPageIndex;
        LoadData();
    }



    protected void ddlSchoolName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSchoolName.SelectedIndex != 0)
        {
            tbSearch.Text = "";
            LoadData();
        }    
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        if (tbSearch.Text != "")
        {
            ddlSchoolName.SelectedIndex = 0;
            LoadData();
        }
        else
        {
            lblError.Text = "Please enter a school name to search.";
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("Edit_school.aspx");
    }
}