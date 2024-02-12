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
            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Populate school name DDL
            SchoolData.LoadSchoolsDDL(schoolName_ddl);

            //Load teacher table
            LoadData();
        }
    }

    public void LoadData()
    {
        string SchoolID;
        string SQLStatement = "SELECT t.id, t.firstName, t.lastName, t.email, t.contact, t.password, t.schoolID, v.visitDate as currVisitDate, v2.visitDate as prevVisitDate FROM teacherInfoFP t LEFT JOIN visitInfoFP v ON v.id = t.currVisitID LEFT JOIN visitInfoFP v2 ON v2.id = t.prevVisitID";

        //Clear teacher table
        teachers_dgv.DataSource = null;
        teachers_dgv.DataBind();

        //Clear error label
        error_lbl.Text = "";

        //Check if school name DDL or search field is entered
        if (schoolName_ddl.SelectedIndex != 0)
        {
            //get school id from name
            SchoolID = SchoolData.GetSchoolID(schoolName_ddl.SelectedValue).ToString();
            SQLStatement = SQLStatement + " WHERE schoolID='" + SchoolID + "'";
        }
        else if (search_tb.Text != "")
        {
            SQLStatement = SQLStatement + " WHERE firstName LIKE '%" + search_tb.Text + "%' OR lastName LIKE '%" + search_tb.Text + "%'";
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
            teachers_dgv.DataSource = Review_sds;
            teachers_dgv.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            error_lbl.Text = "Error in LoadData(). Cannot load teacherInfo table.";
            return;
        }

        // Highlight row being edited
        foreach (GridViewRow row in teachers_dgv.Rows)
        {
            if (row.RowIndex == teachers_dgv.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }

    protected void teachers_dgv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(teachers_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string FirstName = ((TextBox)teachers_dgv.Rows[e.RowIndex].FindControl("firstNameDGV_tb")).Text;
        string LastName = ((TextBox)teachers_dgv.Rows[e.RowIndex].FindControl("lastNameDGV_tb")).Text;
        string Email = ((TextBox)teachers_dgv.Rows[e.RowIndex].FindControl("emailDGV_tb")).Text;
        bool Contact = ((CheckBox)teachers_dgv.Rows[e.RowIndex].FindControl("contactDGV_chk")).Checked;
        string Password = ((TextBox)teachers_dgv.Rows[e.RowIndex].FindControl("passwordDGV_tb")).Text;
        string SchoolID = ((DropDownList)teachers_dgv.Rows[e.RowIndex].FindControl("schoolNameDGV_ddl")).SelectedValue;

        //Check if email is blank
        if (Email == "")
        {
            error_lbl.Text = "Please enter a valid email address.";
            return;
        }

        //Check if email is a valid address
        if ((!(Email.Contains("@")) && (!(Email.Contains("."))))) 
        {
            error_lbl.Text = "Not a valid email address.";
            return;
        }

        //Check if last name field id blank
        if (LastName == "")
        {
            error_lbl.Text = "Please enter a last name.";
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
            teachers_dgv.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void teachers_dgv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(teachers_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number

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
            teachers_dgv.EditIndex = -1;       // reset the grid after editing

            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }
    }

    protected void teachers_dgv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        teachers_dgv.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void teachers_dgv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        teachers_dgv.EditIndex = -1;
        LoadData();
    }

    protected void teachers_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        teachers_dgv.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void teachers_dgv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblSchool1 = (e.Row.FindControl("schoolNameDGV_lbl") as Label).Text;
            DropDownList ddlSchool1 = e.Row.FindControl("schoolNameDGV_ddl") as DropDownList;

            //Load gridview school DDLs with school names
            Gridviews.SchoolNames(ddlSchool1, lblSchool1);
        }
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
            error_lbl.Text = "Please enter a teacher name to search.";
        }
    }

    protected void refresh_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("Edit_Teacher.aspx");
    }
}