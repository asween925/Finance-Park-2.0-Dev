using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Create_Teacher : Page
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

    public Create_Teacher()
    {
        ConnectionString = "Server=" + SQLServer + ";database=" + SQLDatabase + ";uid=" + SQLUser + ";pwd=" + SQLPassword + ";Connection Timeout=20;";
        
        Load += Page_Load;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Check if user is logged in
        if (HttpContext.Current.Session["LoggedIn"] == null)
        {
            Response.Redirect("../../default.aspx");
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

            //Populate school name DDL
            SchoolData.LoadSchoolsDDL(ddlSchoolName);
        }
    }

    public void Submit()
    {
        string FirstName = tbFirstName.Text;
        string LastName = tbLastName.Text;
        string SchoolName = ddlSchoolName.SelectedValue;
        string SchoolID;
        string Email = tbEmail.Text;
        bool Contact = chkContact.Checked;

        // Checks what spots are empty and replaces select ones
        if (FirstName == "" & LastName == "" & ddlSchoolName.SelectedIndex == 0 & Email == "")
        {
            lblError.Text = "Please select a school name and enter a last name and an email before submiting.";
            return;
        }
        else if (FirstName == "")
        {
            FirstName = " ";
        }
        else if (LastName == "")
        {
            lblError.Text = "Please enter a last name for the teacher.";
            return;
        }
        else if (ddlSchoolName.SelectedIndex == 0)
        {
            lblError.Text = "Please select a school name from the drop down menu.";
            return;
        }
        else if (Email == "")
        {
            lblError.Text = "Please enter a valid email.";
            return;
        }

        // Checks if tbEmail is an address
        if (!(Email.Contains("@")) & !(Email.Contains(".")))
        {
            // Not an email. Show message
            lblError.Text = "Not a valid email address.";
            return;
        }

        //Get the school ID of the selected school
        SchoolID = SchoolData.GetSchoolID(SchoolName).ToString();

        using (var con = new SqlConnection(ConnectionString))
        {
            // Checks if there is already a name, email, and school ID with the entered information in the DB
            using (var cmd = new SqlCommand("SELECT email FROM teacherInfoFP WHERE email = '" + Email + "'"))
            {
                cmd.Connection = con;
                con.Open();
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (dr.HasRows == true)
                    {
                        lblError.Text = "A teacher with that email address is already in the database. Please view the 'Edit Teacher' page to make changes to the teacher.";
                        return;
                    }
                }

                con.Close();
                cmd.Dispose();
            }

            using (var cmd = new SqlCommand(@"INSERT INTO teacherInfoFP (firstName, lastName, email, contact, schoolID)
										      VALUES (@firstName, @lastName, @email, @contact, @schoolID);"))
            {

                cmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = FirstName;
                cmd.Parameters.Add("@lastName", SqlDbType.VarChar).Value = LastName;
                cmd.Parameters.Add("@schoolID", SqlDbType.Int).Value = SchoolID;
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@contact", SqlDbType.Bit).Value = Contact;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();

                //Show success message and refresh page               
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "3;url=create_teacher.aspx";
                this.Page.Controls.Add(meta);
                lblError.Text = "Submission successful! Refreshing page...";

            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
    }
}