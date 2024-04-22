using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.EnterpriseServices;
using System.Text.RegularExpressions;
using System.Data.Common;
using System.Activities.Expressions;
using System.Activities.Statements;
using System.IO;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.ServiceModel.Activities;

public partial class _Default : Page
{    
    string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"];
    string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"];
    string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"];
    string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"];
    string ConnectionString;
    string logoRoot = "~/media/Logos/";
    string strProfit;
    string schoolName;
    string schoolID;

    protected void Page_Load(object sender, EventArgs e)
    {
        ValidationSettings.UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;
    }

    protected void Login()
    {
        string email = tbEmail.Text;
        string[] username = email.Split('@');
        string password = password_tb.Text;
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
        Regex emailCheck = new Regex("[@]");
        Regex domainCheck = new Regex(@"[\\]");
        Match emailMatch = emailCheck.Match(email);
        Match domainMatch = domainCheck.Match(email);
        SqlConnection con = new SqlConnection(ConnectionString);
        SqlCommand cmd = new SqlCommand(ConnectionString, con);
        SqlDataReader dr;

        //Checks if tbEmail is a proper email address (contains both a '@' and a '.'
        if (!(email.Contains("@")) && (!(email.Contains('.'))))
        {
            //email is not a valid address, show error, exit method
            lblError.Text = "Not a valid email address.";
            return;
        }

        //cmd.Dispose();
        //con.Close();

        //Checks if user is PCSB, if email contains '@pcsb.org'
        if (email.Contains("@pcsb.org"))
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.Connection = con;

            try
            {
                //Checking PCSB credentials
                if (ValidateActiveDirectoryLogin("pinellas.local", username[0], password) == true)
                {
                    // Valid user. Set Session variables and take to menu

                    Session.Add("LoggedIn", "1");
                    Session.Add("username", username[0]);

                    cmd.CommandText = "SELECT * FROM adminInfoFP WHERE email='" + email + "'";
                    dr = cmd.ExecuteReader();

                    //Checks if email and password entered is GSI Staff
                    if (dr.HasRows == true)
                    {
                        Session.Add("isAdmin", "True");

                        //Check if GSI staff member is bus driver or not; sends them to the inventory page
                        //Might not need to include this
                        while (dr.Read()) 
                        { 
                            if (dr["job"].ToString() == "Bus Driver")
                            {
                                Response.Redirect("Inventory_Home.aspx");
                            }
                            else
                            {
                                Response.Redirect("Pages/home_page.aspx");
                            }
                        }
                    }
                    //If email and password entered is NOT GSI then it will check if the email entered is in the teacherInfo DB
                    else
                    {
                        cmd.Dispose();
                        con.Close();

                        //Check if entered email is in the database, by checking if there is a school name associated with email
                        try
                        {
                            con.ConnectionString = ConnectionString;
                            con.Open();
                            cmd.CommandText = "SELECT DISTINCT t.schoolName FROM teacherInfoFP t LEFT JOIN schoolInfo s ON s.schoolName = t.schoolName WHERE t.futureRequestsEmail = '" + email + "' OR s.futureRequestsEmail = '" + email + "'";
                            cmd.Connection = con;
                            dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                lblSchoolName.Text = dr["schoolName"].ToString();

                                if (dr.HasRows == false || lblSchoolName.Text == "")
                                {
                                    lblError.Text = "We do not have a school associated with your email. Please use the link above to email us about this issue.";
                                    return;
                                }
                            }

                            cmd.Dispose();
                            con.Close();
                        }
                        catch
                        {
                            lblError.Text = "Error code 1. Please use the link above to email us about this issue.";
                            return;
                        }

                        try
                        {
                            con.ConnectionString = ConnectionString;
                            con.Open();
                            cmd.CommandText = "SELECT DISTINCT id FROM schoolInfoFP WHERE schoolName = '" + lblSchoolName.Text + "' AND NOT id=505";
                            cmd.Connection = con;
                            dr = cmd.ExecuteReader();

                            while (dr.Read())
                            {
                                hfSchoolID.Value = dr["id"].ToString();

                                if (hfSchoolID.Value == "" || hfSchoolID.Value == null)
                                {
                                    lblError.Text = "Error code 22. Please use the link above to email us about this issue.";
                                    cmd.Dispose();
                                    con.Close();
                                    return;
                                }
                            }
                        }
                        catch
                        {
                            lblError.Text = "Error code 2. Please use the link above to email us about this issue.";
                            cmd.Dispose( );
                            con.Close( );
                            return;
                        }

                        //Check if school has a visit created for 22-23
                        try
                        {
                            con.ConnectionString = ConnectionString;
                            con.Open();

                            //CHANGE THIS CODE HERE AFTER EACH SCHOOL YEAR:     CHANGE THE YEAR BETWEEN AT THE END OF THIS LINE TO 8-10-(Current Year) AND 6-10(Next Year)
                            cmd.CommandText = "SELECT id FROM (SELECT id, visitDate FROM visitInfoFP WHERE school = '" + hfSchoolID.Value + "' OR school2 = '" + hfSchoolID.Value + "' OR school3 = '" + hfSchoolID.Value + "' OR school4 = '" + hfSchoolID.Value + "' OR school5 = '" + hfSchoolID.Value + "') as x WHERE visitDate BETWEEN '07-01-2023' AND '07-01-2024' AND NOT id = 505";
                            cmd.Connection = con;
                            dr = cmd.ExecuteReader();

                            if (dr.HasRows == true)
                            {
                                var URLEnd = hfSchoolID.Value;

                                Session.Add("isAdmin", "False");
                                Response.Redirect("input_student_information.aspx?b=" + URLEnd);
                            }
                            else
                            {
                                lblError.Text = "We do not have a record of an upcoming visit for you. Please use the link above to email us about this issue.";
                                return;
                            }

                            cmd.Dispose();
                            con.Close();
                        }
                        catch
                        {
                            lblError.Text = "Error code 3. Please use the link above to email us about this issue.";
                            return;
                        }
                    }
                }
                else
                {
                    //Warn about invalid credentials. 
                    lblError.Text = "Invalid PCSB credentials. Email or password is incorrect.";
                    return;
                }

            }
            catch
            {
                lblError.Text = "Error code 5. Please use the link above to email us about this issue.";
                cmd.Dispose();
                con.Close();
                return;
            }

        }
        else
        {
            //Checks if password is valid
            try
            {
                Session.Add("LoggedIn", "1");

                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = "SELECT DISTINCT password FROM teacherInfoFP WHERE futureRequestsEmail = '" + email + "'";
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    if (dr.HasRows == true)
                    {
                        if (password_tb.Text == null || password_tb.Text == "")
                        {
                            lblError.Text = "Please enter your password.";
                            return;
                        }
                        else if (password_tb.Text == dr["password"].ToString())
                        {
                            lblError.Text = dr["password"].ToString();
                            return;
                        }   
                        else
                        {
                            lblError.Text = "Invalid password. Please use the password provided in your email.";
                            return;
                        }                           
                    }
                    else
                    {
                        lblError.Text = "We do not have a school associated with your email. Please use the link above to email us about this issue.";
                        return;
                    }                        
                }

                cmd.Dispose();
                con.Close();

                //Gets schoolName and ID from teacherInfo using email
                try
                {
                    con.ConnectionString = ConnectionString;
                    con.Open();
                    cmd.CommandText = "SELECT DISTINCT id, schoolName FROM teacherInfoFP WHERE futureRequestsEmail = '" + email + "'";
                    cmd.Connection = con;
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lblSchoolName.Text = dr["schoolName"].ToString();
                        teacherID_hf.Value = dr["id"].ToString();

                        if (lblSchoolName.Text == "")
                        {
                            lblError.Text = "We do not have a school associated with your email. Please use the link above to email us about this issue.";
                            return;
                        }

                        if (dr.HasRows != true)
                        {
                            lblError.Text = "We do not have a school associated with your email. Please use the link above to email us about this issue.";
                        }                           
                    }

                    cmd.Dispose();
                    con.Close();
                }
                catch
                {
                    lblError.Text = "Error code 6. Please use the link above to email us about this issue.";
                    //error2_lbl.Text = hfSchoolID.Value & "/" & lblSchoolName.Text;
                    return;
                } 
                
                //Get school ID
                try
                {
                    con.ConnectionString = ConnectionString;
                    con.Open();
                    cmd.CommandText = "SELECT DISTINCT id FROM schoolInfoFP WHERE schoolName = '" + lblSchoolName.Text + "' AND NOT id='505'";
                    cmd.Connection = con;
                    dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        hfSchoolID.Value = dr["id"].ToString();

                        if (hfSchoolID.Value == "" || hfSchoolID.Value == null)
                        {
                            lblError.Text = "Error code 22. Please use the link above to email us about this issue.";
                            cmd.Dispose();
                            con.Close();
                        }                            
                    }

                    cmd.Dispose();
                    con.Close();
                }
                catch
                {
                    lblError.Text = "Error code 7. Please use the link above to email us about this issue.";
                    //error2_lbl.Text = hfSchoolID.Value & "/" & lblSchoolName.Text;
                    return;
                }                   

                //Get visit ID from visitInfoFP and check if there is a visit in the system
                try
                {
                    con.ConnectionString = ConnectionString;
                    con.Open();

                    //CHANGE THE YEAR BETWEEN AT THE END OF THIS LINE TO 8-10-(Current Year) AND 6-10(Next Year)
                    cmd.CommandText = "SELECT id FROM (SELECT id, visitDate FROM visitInfoFP WHERE school = '" + hfSchoolID.Value + "' OR school2 = '" + hfSchoolID.Value + "' OR school3 = '" + hfSchoolID.Value + "' OR school4 = '" + hfSchoolID.Value + "' OR school5 = '" + hfSchoolID.Value + "') as x WHERE visitDate BETWEEN '07-01-2023' AND '07-01-2024'";
                    cmd.Connection = con;
                    dr = cmd.ExecuteReader();

                    if (dr.HasRows == true)
                    {
                        string schoolIDURL = hfSchoolID.Value;
                        string teacherIDURL = teacherID_hf.Value;

                        Session.Add("isAdmin", "False");

                        //Redirect to teacher home page
                        Response.Redirect("teacher_home.aspx?b=" + schoolIDURL + "&c=" + teacherIDURL);
                    }
                    else
                    {
                        lblError.Text = "We do not have a record of a visit with your school (non-PCSB) in our system. Please use the link above to email us about this issue.";
                        return;
                    }

                    cmd.Dispose();
                    con.Close();
                }   
                catch
                {
                    lblError.Text = "Error code 8. Please use the link above to email us about this issue.";
                    //error2_lbl.Text = hfSchoolID.Value & "/" & lblSchoolName.Text
                    return;
                }                    
            }
            catch
            {
                lblError.Text = "Error code 9. Please use the link above to email us about this issue.";
                return;
            }
        }
    }

    private bool ValidateActiveDirectoryLogin(string domain, string username, string password)
    {
        bool Success = false;
        System.DirectoryServices.DirectoryEntry entry = new System.DirectoryServices.DirectoryEntry("LDAP://" + domain, username, password);
        System.DirectoryServices.DirectorySearcher searcher = new System.DirectoryServices.DirectorySearcher(entry);
        searcher.SearchScope = System.DirectoryServices.SearchScope.OneLevel;

        try
        {
            System.DirectoryServices.SearchResult Results = searcher.FindOne();
            Success = !(Results == null);
        }
        catch
        {
            Success = false;
            error2_lbl.Text = "Error code 10. Please use the link above to email us about this issue.";
        }
        return (Success);
    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        Login();
    }

    protected void tbEmail_TextChanged(object sender, EventArgs e)
    {

    }
}