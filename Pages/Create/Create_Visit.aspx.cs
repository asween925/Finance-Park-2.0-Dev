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
    private Class_VisitData VisitData = new Class_VisitData();
    Class_SchoolData SchoolData = new Class_SchoolData();
    Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    Class_SchoolSchedule SchoolSchedule = new Class_SchoolSchedule();
    Class_TeacherData TeacherData = new Class_TeacherData();
    string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"];
    string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"];
    string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"];
    string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"];
    string ConnectionString;
    int visit;

    protected void Page_Load(object sender, EventArgs e)
    {
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
       
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
                hfVisitdateUpdate.Value = visit.ToString();
            }

            //Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Populate schools 1-5 DDL
            SchoolData.LoadSchoolsDDL(ddlSchools, false);
            SchoolData.LoadSchoolsDDL(ddlSchools2, false);
            SchoolData.LoadSchoolsDDL(ddlSchools3, false);
            SchoolData.LoadSchoolsDDL(ddlSchools4, false);
            SchoolData.LoadSchoolsDDL(ddlSchools5, false);

            //Populate visit time DDL
            SchoolSchedule.LoadVisitTimeDDL(ddlVisitTime);           
        }
    }

    public void Submit()
    {
        string visitDate;
        string visitTime = ddlVisitTime.SelectedValue;
        string studentCount = tbStudentCount.Text;
        string vTrainingStart = tbVolunteerTime.Text;
        string dueBy = tbDueBy.Text;
        string school1;
        string school2 = ddlSchools2.SelectedValue;
        string school3 = ddlSchools3.SelectedValue;
        string school4 = ddlSchools4.SelectedValue;
        string school5 = ddlSchools5.SelectedValue;
        string newVisitID;

        // Check for empty fields
        if (tbVisitDate.Text == "" || ddlSchools.SelectedIndex == 0)
        {
            lblSuccess.Text = "Please enter a visit date and select a school.";
            return;
        }
        else
        {
            visitDate = tbVisitDate.Text;
            school1 = ddlSchools.SelectedValue;
        }
            
        if (tbStudentCount.Text == "" )
        {
            studentCount = "0";
        }            
        else if (int.Parse(studentCount) < 0)
        {
            lblSuccess.Text = "Please enter a student count 0 or higher.";
            return;
        }

        if (tbVolunteerTime.Text == "" )
        {
            vTrainingStart = "00:00";
        }

        if (tbDueBy.Text == "")
        {
            dueBy = tbVisitDate.Text;
        }

        // Inserting new visit date into DB
        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO visitInfoFP(school, vTrainingTime, visitDate, studentCount, school2, school3, school4, visitTime, school5, dueBy)
										            
                                                        SELECT (SELECT ID FROM schoolInfoFP WHERE schoolname = @schoolname), @vTrainingTime, @visitdate, @studentcount
                                                        , (SELECT ID FROM schoolInfoFP WHERE schoolname = @schoolname2), (SELECT ID FROM schoolInfoFP WHERE schoolname = @schoolname3)
                                                        , (SELECT ID FROM schoolInfoFP WHERE schoolname = @schoolname4), @visittime, (SELECT ID FROM schoolInfoFP WHERE schoolname = @schoolname5), @dueBy"))
                {

                    cmd.Parameters.Add("@visitdate", SqlDbType.Date).Value = visitDate;
                    cmd.Parameters.Add("@schoolname", SqlDbType.VarChar).Value = school1;
                    cmd.Parameters.Add("@schoolname2", SqlDbType.VarChar).Value = school2;
                    cmd.Parameters.Add("@schoolname3", SqlDbType.VarChar).Value = school3;
                    cmd.Parameters.Add("@schoolname4", SqlDbType.VarChar).Value = school4;
                    cmd.Parameters.Add("@schoolname5", SqlDbType.VarChar).Value = school5;
                    cmd.Parameters.Add("@visittime", SqlDbType.Time).Value = visitTime;
                    cmd.Parameters.Add("@vTrainingTime", SqlDbType.Time).Value = vTrainingStart;
                    cmd.Parameters.Add("@studentcount", SqlDbType.Int).Value = studentCount;
                    cmd.Parameters.Add("@dueBy", SqlDbType.Date).Value = dueBy;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch
        {
            lblSuccess.Text = "Error in Submit(). Could not insert visit into table.";
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
            lblSuccess.Text = "Error in Submit(). Could update current visit date on school table.";
            return;
        }

        //Get new visit ID
        newVisitID = VisitData.GetVisitIDFromDate(visitDate).ToString();

        //Insert openStatusFP
        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO openStatusFP(visitID, open1, open6, open7, open8, open9, open10, open11, open12, open13, open14, open15, open16, open17, open18, open19, open20, open21, open22, open23, open24, open25, open26, open27, open28, open29, open30, open31, open32 )
										            
                                                        VALUES (@visitID, @open1, @open6, @open7, @open8, @open9, @open10, @open11, @open12, @open13, @open14, @open15, @open16, @open17, @open18, @open19, @open20, @open21, @open22, @open23, @open24, @open25, @open26, @open27, @open28, @open29, @open30, @open31, @open32);"))

                {

                    // Date that is inputed in the textbox
                    cmd.Parameters.Add("@visitID", SqlDbType.Int).Value = newVisitID;
                    cmd.Parameters.Add("@open1", SqlDbType.Bit).Value = Checkbox1.Checked;
                    cmd.Parameters.Add("@open6", SqlDbType.Bit).Value = Checkbox6.Checked;
                    cmd.Parameters.Add("@open7", SqlDbType.Bit).Value = Checkbox7.Checked;
                    cmd.Parameters.Add("@open8", SqlDbType.Bit).Value = Checkbox8.Checked;
                    cmd.Parameters.Add("@open9", SqlDbType.Bit).Value = Checkbox9.Checked;
                    cmd.Parameters.Add("@open10", SqlDbType.Bit).Value = Checkbox10.Checked;
                    cmd.Parameters.Add("@open11", SqlDbType.Bit).Value = Checkbox11.Checked;
                    cmd.Parameters.Add("@open12", SqlDbType.Bit).Value = Checkbox12.Checked;
                    cmd.Parameters.Add("@open13", SqlDbType.Bit).Value = Checkbox13.Checked;
                    cmd.Parameters.Add("@open14", SqlDbType.Bit).Value = Checkbox14.Checked;
                    cmd.Parameters.Add("@open15", SqlDbType.Bit).Value = Checkbox15.Checked;
                    cmd.Parameters.Add("@open16", SqlDbType.Bit).Value = Checkbox16.Checked;
                    cmd.Parameters.Add("@open17", SqlDbType.Bit).Value = Checkbox17.Checked;
                    cmd.Parameters.Add("@open18", SqlDbType.Bit).Value = Checkbox18.Checked;
                    cmd.Parameters.Add("@open19", SqlDbType.Bit).Value = Checkbox19.Checked;
                    cmd.Parameters.Add("@open20", SqlDbType.Bit).Value = Checkbox20.Checked;
                    cmd.Parameters.Add("@open21", SqlDbType.Bit).Value = Checkbox21.Checked;
                    cmd.Parameters.Add("@open22", SqlDbType.Bit).Value = Checkbox22.Checked;
                    cmd.Parameters.Add("@open23", SqlDbType.Bit).Value = Checkbox23.Checked;
                    cmd.Parameters.Add("@open24", SqlDbType.Bit).Value = Checkbox24.Checked;
                    cmd.Parameters.Add("@open25", SqlDbType.Bit).Value = Checkbox25.Checked;
                    cmd.Parameters.Add("@open26", SqlDbType.Bit).Value = Checkbox26.Checked;
                    cmd.Parameters.Add("@open27", SqlDbType.Bit).Value = Checkbox27.Checked;
                    cmd.Parameters.Add("@open28", SqlDbType.Bit).Value = Checkbox28.Checked;
                    cmd.Parameters.Add("@open29", SqlDbType.Bit).Value = Checkbox29.Checked;
                    cmd.Parameters.Add("@open30", SqlDbType.Bit).Value = Checkbox30.Checked;
                    cmd.Parameters.Add("@open31", SqlDbType.Bit).Value = Checkbox31.Checked;
                    cmd.Parameters.Add("@open32", SqlDbType.Bit).Value = Checkbox32.Checked;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch
        {
            lblSuccess.Text = "Error in Submit(). Could not open businesses.";
            return;
        }

        //Update current visit ID in teachersInfoFP
        UpdateTeachers(newVisitID);
        
        // Refresh page
        HtmlMeta meta = new HtmlMeta();
        meta.HttpEquiv = "Refresh";
        meta.Content = "4;url=create_visit.aspx";
        this.Page.Controls.Add(meta);
        //ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
        lblSuccess.Text = "Submission Successful! Refreshing page...";      
    }

    public void UpdateTeachers(string VisitID)
    {
        //Check if school 1 has teachers
        if (ddlSchools.SelectedIndex != 0)
        {
            if (ddlTeacher1.SelectedIndex != 0)
            {
                string[] t11 = ddlTeacher1.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t11[0], t11[1]).ToString(), VisitID);
            }

            if (ddlTeacher12.SelectedIndex != 0)
            {
                string[] t12 = ddlTeacher12.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t12[0], t12[1]).ToString(), VisitID);
            }

            if (ddlTeacher13.SelectedIndex != 0)
            {
               string[] t13 = ddlTeacher13.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t13[0], t13[1]).ToString(), VisitID);
            }
        }

        //Check if school 2 has teachers
        if (ddlSchools2.SelectedIndex != 0)
        {
            if (ddlTeacher2.SelectedIndex != 0)
            {
                string[] t21 = ddlTeacher2.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t21[0], t21[1]).ToString(), VisitID);
            }
            if (ddlTeacher22.SelectedIndex != 0)
            {
                string[] t22 = ddlTeacher22.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t22[0], t22[1]).ToString(), VisitID);
            }
            if (ddlTeacher23.SelectedIndex != 0)
            {
                string[] t23 = ddlTeacher23.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t23[0], t23[1]).ToString(), VisitID);
            }
        }

        //Check if school 3 has teachers
        if (ddlSchools3.SelectedIndex != 0)
        {
            if (ddlTeacher3.SelectedIndex != 0)
            {
                string[] t31 = ddlTeacher3.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t31[0], t31[1]).ToString(), VisitID);
            }
            if (ddlTeacher32.SelectedIndex != 0)
            {
                string[] t32 = ddlTeacher32.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t32[0], t32[1]).ToString(), VisitID);
            }
            if (ddlTeacher33.SelectedIndex != 0)
            {
                string[] t33 = ddlTeacher33.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t33[0], t33[1]).ToString(), VisitID);
            }
        }

        //Check if school 4 has teachers
        if (ddlSchools4.SelectedIndex != 0)
        {
            if (ddlTeacher4.SelectedIndex != 0)
            {
                string[] t41 = ddlTeacher4.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t41[0], t41[1]).ToString(), VisitID);
            }
            if (ddlTeacher42.SelectedIndex != 0)
            {
                string[] t42 = ddlTeacher42.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t42[0], t42[1]).ToString(), VisitID);
            }
            if (ddlTeacher43.SelectedIndex != 0)
            {
                string[] t43 = ddlTeacher43.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t43[0], t43[1]).ToString(), VisitID);
            }
        }

        //Check if school 5 has teachers
        if (ddlSchools5.SelectedIndex != 0)
        {
            if (ddlTeacher5.SelectedIndex != 0)
            {
                string[] t51 = ddlTeacher5.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t51[0], t51[1]).ToString(), VisitID);
            }
            if (ddlTeacher52.SelectedIndex != 0)
            {
                string[] t52 = ddlTeacher52.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t52[0], t52[1]).ToString(), VisitID);
            }
            if (ddlTeacher53.SelectedIndex != 0)
            {
                string[] t53 = ddlTeacher53.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t53[0], t53[1]).ToString(), VisitID);
            }
        }
    }



    protected void ddlVisitTime_SelectedIndexChanged1(object sender, EventArgs e)
    {
        tbVolunteerTime.Text = SchoolSchedule.GetVolArrivalTime(ddlVisitTime.SelectedValue).ToString();
    }

    protected void btnSubmit_Click1(object sender, EventArgs e)
    {
        Submit();
    }

    protected void btnOpenAll_Click(object sender, EventArgs e)
    {
        if (btnOpenAll.Text == "Open All Businesses")
        {
            //Check off all checkboxes
            Checkbox1.Checked = true;
            Checkbox6.Checked = true;
            Checkbox7.Checked = true;
            Checkbox8.Checked = true;
            Checkbox9.Checked = true;
            Checkbox10.Checked = true;
            Checkbox11.Checked = true;
            Checkbox12.Checked = true;
            Checkbox13.Checked = true;
            Checkbox14.Checked = true;
            Checkbox15.Checked = true;
            Checkbox16.Checked = true;
            Checkbox17.Checked = true;
            Checkbox18.Checked = true;
            Checkbox19.Checked = true;
            Checkbox20.Checked = true;
            Checkbox21.Checked = true;
            Checkbox22.Checked = true;
            Checkbox23.Checked = true;
            Checkbox24.Checked = true;
            Checkbox25.Checked = true;
            Checkbox26.Checked = true;
            Checkbox27.Checked = true;
            Checkbox28.Checked = true;
            Checkbox29.Checked = true;
            Checkbox30.Checked = true;
            Checkbox31.Checked = true;
            Checkbox32.Checked = true;

            //Change text on button to close
            btnOpenAll.Text = "Close All Businesses";
        }
        else
        {
            //Check off all checkboxes
            Checkbox1.Checked = false;
            Checkbox6.Checked = false;
            Checkbox7.Checked = false;
            Checkbox8.Checked = false;
            Checkbox9.Checked = false;
            Checkbox10.Checked = false;
            Checkbox11.Checked = false;
            Checkbox12.Checked = false;
            Checkbox13.Checked = false;
            Checkbox14.Checked = false;
            Checkbox15.Checked = false;
            Checkbox16.Checked = false;
            Checkbox17.Checked = false;
            Checkbox18.Checked = false;
            Checkbox19.Checked = false;
            Checkbox20.Checked = false;
            Checkbox21.Checked = false;
            Checkbox22.Checked = false;
            Checkbox23.Checked = false;
            Checkbox24.Checked = false;
            Checkbox25.Checked = false;
            Checkbox26.Checked = false;
            Checkbox27.Checked = false;
            Checkbox28.Checked = false;
            Checkbox29.Checked = false;
            Checkbox30.Checked = false;
            Checkbox31.Checked = false;
            Checkbox32.Checked = false;

            //Change text on button to open
            btnOpenAll.Text = "Open All Businesses";
        }
    }

    protected void tbVisitDate_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDate.Text != "")
        {
            //Clear error label
            lblError.Text = "";

            // Check if visit date has already been created
            try
            {
                if (VisitData.LoadVisitInfoFromDate(tbVisitDate.Text, "visitDate").ToString() != "")
                {
                    //lblError.Text = VisitData.LoadVisitInfoFromDate(tbVisitDate.Text, "visitDate").ToString();
                    lblError.Text = "A visit date has already been created for that day, please go to the 'Edit Visit' page to edit the visit for your inputted date.";
                    return;
                }
            }
            catch
            {
                lblError.Text = "Error. Could not check if visit date has been created.";
                return;
            }
        }
    }



    protected void ddlSchools_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        ddlTeacher1.Items.Clear();
        ddlTeacher12.Items.Clear();
        ddlTeacher13.Items.Clear();

        if (ddlSchools.SelectedIndex != 0)
        {
            aTeacher1.Visible = true;
            divTeachers1.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools.SelectedValue, ddlTeacher1);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools.SelectedValue, ddlTeacher12);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools.SelectedValue, ddlTeacher13);
        }
        else
        {
            aTeacher1.Visible=false;
            divTeachers1.Visible=false;
        }
    }

    protected void ddlSchools2_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        ddlTeacher2.Items.Clear();
        ddlTeacher22.Items.Clear();
        ddlTeacher23.Items.Clear();

        if (ddlSchools2.SelectedIndex != 0)
        {
            aTeacher2.Visible = true;
            divTeachers2.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools2.SelectedValue, ddlTeacher2);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools2.SelectedValue, ddlTeacher22);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools2.SelectedValue, ddlTeacher23);
        }
        else
        {
            aTeacher2.Visible = false;
            divTeachers2.Visible = false;
        }
    }

    protected void ddlSchools3_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        ddlTeacher3.Items.Clear();
        ddlTeacher32.Items.Clear();
        ddlTeacher33.Items.Clear();

        if (ddlSchools3.SelectedIndex != 0)
        {
            aTeacher3.Visible = true;
            divTeachers3.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools3.SelectedValue, ddlTeacher3);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools3.SelectedValue, ddlTeacher32);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools3.SelectedValue, ddlTeacher33);
        }
        else
        {
            aTeacher3.Visible = false;
            divTeachers3.Visible = false;
        }
    }

    protected void ddlSchools4_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        ddlTeacher4.Items.Clear();
        ddlTeacher42.Items.Clear();
        ddlTeacher43.Items.Clear();

        if (ddlSchools4.SelectedIndex != 0)
        {
            aTeacher4.Visible = true;
            divTeachers4.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools4.SelectedValue, ddlTeacher4);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools4.SelectedValue, ddlTeacher42);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools4.SelectedValue, ddlTeacher43);
        }
        else
        {
            aTeacher4.Visible = false;
            divTeachers4.Visible = false;
        }
    }

    protected void ddlSchools5_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        ddlTeacher5.Items.Clear();
        ddlTeacher52.Items.Clear();
        ddlTeacher53.Items.Clear();

        if (ddlSchools5.SelectedIndex != 0)
        {
            aTeacher5.Visible = true;
            divTeachers5.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools5.SelectedValue, ddlTeacher5);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools5.SelectedValue, ddlTeacher52);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools5.SelectedValue, ddlTeacher53);
        }
        else
        {
            aTeacher5.Visible = false;
            divTeachers5.Visible = false;
        }
    }

}