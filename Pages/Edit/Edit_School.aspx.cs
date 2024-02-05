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
            //Populate school name ddl
            SchoolData.LoadSchoolsDDL(schoolName_ddl);

            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load schoolInfoFP table
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLStatement = "SELECT * FROM schoolInfoFP";

        //Clear error
        error_lbl.Text = "";

        //Clear table
        school_dgv.DataSource = null;
        school_dgv.DataBind();

        //If loading by the DDL, add school name to search query
        if (schoolName_ddl.SelectedIndex != 0)
        {
            SQLStatement = SQLStatement + " WHERE schoolName='" + schoolName_ddl.SelectedValue + "'";
        }
        else if (search_tb.Text != "")
        {
            SQLStatement = SQLStatement + " WHERE schoolName LIKE '%" + search_tb.Text + "%'";
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
            school_dgv.DataSource = Review_sds;
            school_dgv.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            error_lbl.Text = "Error in LoadData(). Cannot load schoolInfo table.";
            return;
        }

        // Highlight row being edited
        foreach (GridViewRow row in school_dgv.Rows)
        {
            if (row.RowIndex == school_dgv.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }



    protected void school_dgv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(school_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string SchoolName = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("schoolNameDGV_tb")).Text;
        string Address = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("addressDGV_tb")).Text;
        string City = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("cityDGV_tb")).Text;
        string Zip = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("zipDGV_tb")).Text;
        string County = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("countyDGV_tb")).Text;
        string PrincipalFirst = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("principalFirstDGV_tb")).Text;
        string PrincipalLast = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("principalLastDGV_tb")).Text;
        string AdminEmail = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("administratorEmailDGV_tb")).Text;
        string Phone = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("phoneDGV_tb")).Text;
        string Hours = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("hoursDGV_tb")).Text;
        string SchoolType = ((DropDownList)school_dgv.Rows[e.RowIndex].FindControl("schoolTypeDGV_ddl")).SelectedValue;
        string Notes = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("notesDGV_tb")).Text;
        string LiaisonName = ((TextBox)school_dgv.Rows[e.RowIndex].FindControl("liaisonNameDGV_tb")).Text;

        //try
        //{
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
            school_dgv.EditIndex = -1;       // reset the grid after editing
            LoadData();
        //}
        //catch
        //{
        //    error_lbl.Text = "Error in rowUpdating. Cannot update row.";
        //    return;
        //}
    }

    protected void school_dgv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        school_dgv.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void school_dgv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        school_dgv.EditIndex = -1;
        LoadData();
    }

    protected void school_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        school_dgv.PageIndex = e.NewPageIndex;
        LoadData();
    }



    protected void schoolName_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (schoolName_ddl.SelectedIndex != 0)
        {
            search_tb.Text = "";
            LoadData();
        }    
    }

    protected void search_btn_Click(object sender, EventArgs e)
    {
        if (search_tb.Text != "")
        {
            schoolName_ddl.SelectedIndex = 0;
            LoadData();
        }
        else
        {
            error_lbl.Text = "Please enter a school name to search.";
        }
    }

    protected void refresh_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("Edit_school.aspx");
    }
}