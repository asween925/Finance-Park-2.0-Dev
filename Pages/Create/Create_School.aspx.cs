using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Create_School : Page
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
    private Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    private int VisitID;

    public Create_School()
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
        }
    }

    public void Submit()
    {
        //Check for empty fields, update the text
        if (tbSchoolName.Text == "" || tbPhoneNum.Text == "")
        {
            lblError.Text = "Please enter a name for the school and a phone number.";
            return;
        }

        if (tbAddress.Text == "")
        {
            tbAddress.Text = "N/A";
        }

        if (tbCity.Text == "")
        {
            tbCity.Text = "N/A";
        }

        if (tbZip.Text == "")
        {
            tbZip.Text = "N/A";
        }

        if (tbPrincipalFirst.Text == "")
        {
            tbPrincipalFirst.Text = "N/A";
        }

        if (tbPrincipalLast.Text == "")
        {
            tbPrincipalLast.Text = "N/A";
        }

        if (tbSchoolNum.Text == "")
        {
            tbSchoolNum.Text = "0000";
        }

        if (tbSchoolHours.Text == "")
        {
            tbSchoolHours.Text = "N/A";
        }

        if (tbAdminEmail.Text == "")
        {
            tbAdminEmail.Text = "N/A";
        }

        if (tbNotes.Text == "")
        {
            tbNotes.Text = "N/A";
        }

        if (tbCounty.Text == "")
        {
            tbCounty.Text = "N/A";
        }

        if (tbLiaison.Text == "")
        {
            tbLiaison.Text = "N/A";
        }

        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            //Check if school name is already in schoolInfoFP
            try
            {
                using (SqlCommand cmd = new SqlCommand(@"SELECT schoolName FROM schoolInfoFP WHERE schoolName = '" + tbSchoolName.Text + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        if (dr.HasRows)
                        {
                            lblError.Text = "A school with that name is already in the database. Please view the 'Edit School' page to make changes to the school.";

                            //Refresh page
                            Response.Redirect("../create_school");
                        }
                    }
                    con.Close();
                    cmd.Dispose();
                }
            }
            catch
            {
                lblError.Text = "Error in Submit(). Cannot detect if there is an identical school name in the database.";
                return;
            }

            //Insert data into new row in schoolinfoFP
            try
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO schoolInfoFP (schoolName, address, city, zip, county, principalFirst, principalLast, phone, schoolNum, schoolHours, schoolType
                                                     , administratorEmail, notes, liaisonName)

												    VALUES (@schoolName, @address, @city, @zip, @county, @principalFirst, @principalLast, @phone, @schoolNum, @schoolHours, @schoolType
                                                     , @administratorEmail, @notes, @liaisonName);"))
                {
                    cmd.Parameters.Add("@schoolName", SqlDbType.VarChar).Value = tbSchoolName.Text;
                    cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = tbAddress.Text;
                    cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = tbCity.Text;
                    cmd.Parameters.Add("@zip", SqlDbType.VarChar).Value = tbZip.Text;
                    cmd.Parameters.Add("@principalFirst", SqlDbType.VarChar).Value = tbPrincipalFirst.Text;
                    cmd.Parameters.Add("@principalLast", SqlDbType.VarChar).Value = tbPrincipalLast.Text;
                    cmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = tbPhoneNum.Text;
                    cmd.Parameters.Add("@schoolNum", SqlDbType.Int).Value = tbSchoolNum.Text;
                    cmd.Parameters.Add("@schoolHours", SqlDbType.VarChar).Value = tbSchoolHours.Text;
                    cmd.Parameters.Add("@schoolType", SqlDbType.VarChar).Value = tbSchoolType.SelectedValue;
                    cmd.Parameters.Add("@administratorEmail", SqlDbType.VarChar).Value = tbAdminEmail.Text;
                    cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = tbNotes.Text;
                    cmd.Parameters.Add("@county", SqlDbType.Text).Value = tbCounty.Text;
                    cmd.Parameters.Add("@liaisonName", SqlDbType.Text).Value = tbLiaison.Text;

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }

                //Show success message and refresh page               
                HtmlMeta meta = new HtmlMeta();
                meta.HttpEquiv = "Refresh";
                meta.Content = "3;url=create_school.aspx";
                this.Page.Controls.Add(meta);
                lblError.Text = "Submission successful! Refreshing page...";
            }
            catch
            {
                lblError.Text = "Error in Submit(). Cannot create new school.";
                return;
            }
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
    }
}