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
        VisitID = VisitData.GetVisitID();
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
                currentVisitID_hf.Value = VisitID.ToString();
            }

            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }
    }

    public void Submit()
    {
        //Check for empty fields, update the text
        if (schoolName_tb.Text == "" || phoneNum_tb.Text == "")
        {
            error_lbl.Text = "Please enter a name for the school and a phone number.";
            return;
        }

        if (address_tb.Text == "")
        {
            address_tb.Text = "N/A";
        }

        if (city_tb.Text == "")
        {
            city_tb.Text = "N/A";
        }

        if (zip_tb.Text == "")
        {
            zip_tb.Text = "N/A";
        }

        if (principalFirst_tb.Text == "")
        {
            principalFirst_tb.Text = "N/A";
        }

        if (principalLast_tb.Text == "")
        {
            principalLast_tb.Text = "N/A";
        }

        if (schoolNum_tb.Text == "")
        {
            schoolNum_tb.Text = "0000";
        }

        if (schoolHours_tb.Text == "")
        {
            schoolHours_tb.Text = "N/A";
        }

        if (adminEmail_tb.Text == "")
        {
            adminEmail_tb.Text = "N/A";
        }

        if (notes_tb.Text == "")
        {
            notes_tb.Text = "N/A";
        }

        if (county_tb.Text == "")
        {
            county_tb.Text = "N/A";
        }

        if (liaison_tb.Text == "")
        {
            liaison_tb.Text = "N/A";
        }

        using (SqlConnection con = new SqlConnection(ConnectionString))
        {
            //Check if school name is already in schoolInfoFP
            try
            {
                using (SqlCommand cmd = new SqlCommand(@"SELECT schoolName FROM schoolInfoFP WHERE schoolName = '" + schoolName_tb.Text + "'"))
                {
                    cmd.Connection = con;
                    con.Open();
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        if (dr.HasRows)
                        {
                            error_lbl.Text = "A school with that name is already in the database. Please view the 'Edit School' page to make changes to the school.";

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
                error_lbl.Text = "Error in Submit(). Cannot detect if there is an identical school name in the database.";
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
                    cmd.Parameters.Add("@schoolName", SqlDbType.VarChar).Value = schoolName_tb.Text;
                    cmd.Parameters.Add("@address", SqlDbType.VarChar).Value = address_tb.Text;
                    cmd.Parameters.Add("@city", SqlDbType.VarChar).Value = city_tb.Text;
                    cmd.Parameters.Add("@zip", SqlDbType.VarChar).Value = zip_tb.Text;
                    cmd.Parameters.Add("@principalFirst", SqlDbType.VarChar).Value = principalFirst_tb.Text;
                    cmd.Parameters.Add("@principalLast", SqlDbType.VarChar).Value = principalLast_tb.Text;
                    cmd.Parameters.Add("@phone", SqlDbType.VarChar).Value = phoneNum_tb.Text;
                    cmd.Parameters.Add("@schoolNum", SqlDbType.Int).Value = schoolNum_tb.Text;
                    cmd.Parameters.Add("@schoolHours", SqlDbType.VarChar).Value = schoolHours_tb.Text;
                    cmd.Parameters.Add("@schoolType", SqlDbType.VarChar).Value = schoolType_ddl.SelectedValue;
                    cmd.Parameters.Add("@administratorEmail", SqlDbType.VarChar).Value = adminEmail_tb.Text;
                    cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = notes_tb.Text;
                    cmd.Parameters.Add("@county", SqlDbType.Text).Value = county_tb.Text;
                    cmd.Parameters.Add("@liaisonName", SqlDbType.Text).Value = liaison_tb.Text;

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
                error_lbl.Text = "Submission successful! Refreshing page...";
            }
            catch
            {
                error_lbl.Text = "Error in Submit(). Cannot create new school.";
                return;
            }
        }
    }

    protected void Submit_btn_Click(object sender, EventArgs e)
    {
        Submit();
    }
}