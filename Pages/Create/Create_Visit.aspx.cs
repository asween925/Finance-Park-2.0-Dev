using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using Microsoft.AspNet.Identity;
using System.Activities.Statements;

public partial class Create_Visit : System.Web.UI.Page
{
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    Class_VisitData VisitData = new Class_VisitData();
    Class_SchoolData SchoolData = new Class_SchoolData();
    Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    Class_SchoolSchedule SchoolSchedule = new Class_SchoolSchedule();
    string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"];
    string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"];
    string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"];
    string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"];
    string connection_string;
    int visit;

    protected void Page_Load(object sender, EventArgs e)
    {
        connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
        visit = VisitData.GetVisitID();
       
        //Check if user is logged in
        if (HttpContext.Current.Session["LoggedIn"] == null)
        {
            Response.Redirect("../../default.aspx");
        }

        if (!(IsPostBack)) 
        {
            //Check if visit date exists for today
            if (visit != 0) 
            {
                visitdateUpdate_hf.Value = visit.ToString();
            }

            //Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Populate schools 1-5 DDL
            SchoolData.LoadSchoolsDDL(schools_ddl);
            SchoolData.LoadSchoolsDDL(schools2_ddl);
            SchoolData.LoadSchoolsDDL(schools3_ddl);
            SchoolData.LoadSchoolsDDL(schools4_ddl);
            SchoolData.LoadSchoolsDDL(schools5_ddl);

            //Populate visit time DDL
            SchoolSchedule.LoadVisitTimeDDL(visitTime_ddl);

            //Insert no school scheduled into first school DDL
            schools_ddl.Items.Insert(1, "No School Scheduled");
        }
    }

    public void Submit()
    {
        string visitDate;
        string replyBy = replyBy_tb.Text;
        string visitTime = visitTime_ddl.SelectedValue;
        string studentCount = studentCount_tb.Text;
        string vTrainingStart = volunteerTime_tb.Text;
        string school1;
        string school2 = schools2_ddl.SelectedValue;
        string school3 = schools3_ddl.SelectedValue;
        string school4 = schools4_ddl.SelectedValue;
        string school5 = schools5_ddl.SelectedValue;

        // Check for empty fields
        if (visit_tb.Text == "" || schools_ddl.SelectedIndex == 0)
        {
            error_lbl.Text = "Please enter a visit date and select a school.";
            return;
        }
        else
        {
            visitDate = visit_tb.Text;
            school1 = schools_ddl.SelectedValue;
        }

        if (replyBy_tb.Text == ""/* TODO Change to default(_) if this is not a reference type */ )
        {
            replyBy = "01/01/1900";
        }
            
        if (studentCount_tb.Text == ""/* TODO Change to default(_) if this is not a reference type */ )
        {
            studentCount = "0";
        }            
        else if (int.Parse(studentCount) < 0)
        {
            error_lbl.Text = "Please enter a student count 0 or higher.";
            return;
        }

        if (volunteerTime_tb.Text == null/* TODO Change to default(_) if this is not a reference type */ )
        {
            vTrainingStart = "00:00";
        }

        // Check if visit date has already been created
        try
        {
            con.ConnectionString = connection_string;
            con.Open();
            cmd.CommandText = "SELECT visitDate FROM visitInfoFP WHERE visitDate = '" + visitDate + "'";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                if (dr.HasRows == true)
                {
                    error_lbl.Text = "A visit date has already been created for that day, please go to the 'Edit Visit' page to edit the visit for your inputted date.";
                    con.Close();
                    cmd.Dispose();
                    return;
                }
            }

            con.Close();
            cmd.Dispose();
        }
        catch
        {
            error_lbl.Text = "Error in Submit(). Could not check if visit date has been created.";
            return;
        }

        // Inserting new visit date into DB
        try
        {
            using (SqlConnection con = new SqlConnection(connection_string))
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO visitInfoFP(school, vTrainingTime, replyBy, visitDate, studentCount, school2, school3, school4, visitTime
                                                        , school5, teacherCompleted, deposit2Enable, deposit3Enable)
										            
                                                        SELECT (SELECT ID FROM schoolInfoFP WHERE schoolname = @schoolname), @vTrainingTime, @replyBy, @visitdate, @studentcount
                                                        , (SELECT ID FROM schoolInfoFP WHERE schoolname = @schoolname2), (SELECT ID FROM schoolInfoFP WHERE schoolname = @schoolname3)
                                                        , (SELECT ID FROM schoolInfoFP WHERE schoolname = @schoolname4), @visittime, (SELECT ID FROM schoolInfoFP WHERE schoolname = @schoolname5)
                                                        , teacherCompleted=0, deposit2Enable=0, deposit3Enable=0"))

                {

                    // Date that is inputed in the textbox
                    cmd.Parameters.Add("@visitdate", SqlDbType.Date).Value = visitDate;
                    cmd.Parameters.Add("@replyBy", SqlDbType.VarChar).Value = replyBy;
                    cmd.Parameters.Add("@schoolname", SqlDbType.VarChar).Value = school1;
                    cmd.Parameters.Add("@schoolname2", SqlDbType.VarChar).Value = school2;
                    cmd.Parameters.Add("@schoolname3", SqlDbType.VarChar).Value = school3;
                    cmd.Parameters.Add("@schoolname4", SqlDbType.VarChar).Value = school4;
                    cmd.Parameters.Add("@schoolname5", SqlDbType.VarChar).Value = school5;
                    cmd.Parameters.Add("@visittime", SqlDbType.Time).Value = visitTime;
                    cmd.Parameters.Add("@vTrainingTime", SqlDbType.Time).Value = vTrainingStart;
                    cmd.Parameters.Add("@studentcount", SqlDbType.Int).Value = studentCount;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch
        {
            error_lbl.Text = "Error in Submit(). Could not insert visit into table.";
            return;
        }

        // Move current visit date to previous visit date
        try
        {
            SchoolData.UpdateCurrentVisitDate(SchoolData.GetSchoolID(school1).ToString(), visitDate);
            SchoolData.UpdateCurrentVisitDate(SchoolData.GetSchoolID(school2).ToString(), visitDate);
            SchoolData.UpdateCurrentVisitDate(SchoolData.GetSchoolID(school3).ToString(), visitDate);
            SchoolData.UpdateCurrentVisitDate(SchoolData.GetSchoolID(school4).ToString(), visitDate);
            SchoolData.UpdateCurrentVisitDate(SchoolData.GetSchoolID(school5).ToString(), visitDate);
        }
        catch
        {

        }
        

        // Update schoolinfo current visit dates
        //using (SqlCommand cmd = new SqlCommand("UPDATE schoolInfo SET currentVisitDate=@currentVisitDate WHERE schoolName=@schoolName OR schoolName=@schoolName2 OR schoolName=@schoolName3 OR schoolName=@schoolName4 OR schoolName=@schoolName5"))
        //{
        //    cmd.Parameters.Add("@schoolname", SqlDbType.VarChar).Value = school1;
        //    cmd.Parameters.Add("@schoolname2", SqlDbType.VarChar).Value = school2;
        //    cmd.Parameters.Add("@schoolname3", SqlDbType.VarChar).Value = school3;
        //    cmd.Parameters.Add("@schoolname4", SqlDbType.VarChar).Value = school4;
        //    cmd.Parameters.Add("@schoolname5", SqlDbType.VarChar).Value = school5;
        //    cmd.Parameters.Add("@currentVisitDate", SqlDbType.Date).Value = visitDate;
        //    cmd.Connection = con;
        //    con.Open();
        //    cmd.ExecuteNonQuery();
        //    con.Close();

        // Refresh page
        HtmlMeta meta = new HtmlMeta();
        meta.HttpEquiv = "Refresh";
        meta.Content = "4;url=create_visit.aspx";
        this.Page.Controls.Add(meta);
        error_lbl.Text = "Submission Successful! Refreshing page...";
        //}
    }

    protected void visitTime_ddl_SelectedIndexChanged1(object sender, EventArgs e)
    {
        volunteerTime_tb.Text = SchoolSchedule.GetVolArrivalTime(visitTime_ddl.SelectedValue).ToString();
    }

    protected void Submit_btn_Click1(object sender, EventArgs e)
    {
        Submit();
    }
}