using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_Teacher : Page
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

    public Edit_Teacher()
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
            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Populate school name DDL
            SchoolData.LoadSchoolsDDL(ddlSchoolName, false);

            //Load teacher table
            LoadData();
        }
    }

    public void LoadData()
    {
        string SchoolID;
        string SQLStatement = "SELECT t.id, t.firstName, t.lastName, t.email, t.contact, t.password, t.schoolID, v.visitDate as currVisitDate, v2.visitDate as prevVisitDate FROM teacherInfoFP t LEFT JOIN visitInfoFP v ON v.id = t.currVisitID LEFT JOIN visitInfoFP v2 ON v2.id = t.prevVisitID";

        //Clear teacher table
        dgvTeachers.DataSource = null;
        dgvTeachers.DataBind();

        //Clear error label
        lblError.Text = "";

        //Check if school name DDL or search field is entered
        if (ddlSchoolName.SelectedIndex != 0)
        {
            //get school id from name
            SchoolID = SchoolData.GetSchoolID(ddlSchoolName.SelectedValue).ToString();
            SQLStatement = SQLStatement + " WHERE schoolID='" + SchoolID + "'";
        }
        else if (tbSearch.Text != "")
        {
            SQLStatement = SQLStatement + " WHERE firstName LIKE '%" + tbSearch.Text + "%' OR lastName LIKE '%" + tbSearch.Text + "%'";
        }
        else
        {
            SQLStatement = SQLStatement + " ORDER BY schoolID ASC";
        }

        //Load teacherInfoFP table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvTeachers.DataSource = Review_sds;
            dgvTeachers.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load teacherInfo table.";
            return;
        }

        // Highlight row being edited
        foreach (GridViewRow row in dgvTeachers.Rows)
        {
            if (row.RowIndex == dgvTeachers.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }

    protected void dgvTeachers_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(dgvTeachers.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string FirstName = ((TextBox)dgvTeachers.Rows[e.RowIndex].FindControl("tbFirstNameDGV")).Text;
        string LastName = ((TextBox)dgvTeachers.Rows[e.RowIndex].FindControl("tbLastNameDGV")).Text;
        string Email = ((TextBox)dgvTeachers.Rows[e.RowIndex].FindControl("tbEmailDGV")).Text;
        bool Contact = ((CheckBox)dgvTeachers.Rows[e.RowIndex].FindControl("chkContactDGV")).Checked;
        string Password = ((TextBox)dgvTeachers.Rows[e.RowIndex].FindControl("tbPasswordDGV")).Text;
        string SchoolID = ((DropDownList)dgvTeachers.Rows[e.RowIndex].FindControl("ddlSchoolNameDGV")).SelectedValue;

        //Check if email is blank
        if (Email == "")
        {
            lblError.Text = "Please enter a valid email address.";
            return;
        }

        //Check if email is a valid address
        if ((!(Email.Contains("@")) && (!(Email.Contains("."))))) 
        {
            lblError.Text = "Not a valid email address.";
            return;
        }

        //Check if last name field id blank
        if (LastName == "")
        {
            lblError.Text = "Please enter a last name.";
            return;
        }

        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE teacherInfoFP SET firstName=@firstName, lastName=@lastName, email=@email, contact=@contact, password=@password, schoolID=@schoolID WHERE ID=@Id"))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@password", Password);
                    cmd.Parameters.AddWithValue("@schoolID", SchoolID);
                    cmd.Parameters.AddWithValue("@firstName", FirstName);
                    cmd.Parameters.AddWithValue("@lastName", LastName);
                    cmd.Parameters.AddWithValue("@email", Email);
                    cmd.Parameters.AddWithValue("@contact", Contact);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dgvTeachers.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void dgvTeachers_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(dgvTeachers.DataKeys[e.RowIndex].Values[0]); // Gets id number

        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM teacherInfoFP WHERE id=@ID"))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dgvTeachers.EditIndex = -1;       // reset the grid after editing

            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }
    }

    protected void dgvTeachers_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvTeachers.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void dgvTeachers_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvTeachers.EditIndex = -1;
        LoadData();
    }

    protected void dgvTeachers_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvTeachers.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void dgvTeachers_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblSchool1 = (e.Row.FindControl("lblSchoolNameDGV") as Label).Text;
            DropDownList ddlSchool1 = e.Row.FindControl("ddlSchoolNameDGV") as DropDownList;

            //Load gridview school DDLs with school names
            Gridviews.SchoolNames(ddlSchool1, lblSchool1);
        }
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
            lblError.Text = "Please enter a teacher name to search.";
        }
    }

    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("Edit_Teacher.aspx");
    }
}