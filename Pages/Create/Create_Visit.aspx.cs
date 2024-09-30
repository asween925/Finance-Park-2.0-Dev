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
using System.Collections.Specialized;
using Microsoft.Owin.Security;
using System.Runtime.Remoting.Lifetime;

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
    Class_SponsorData Sponsors = new Class_SponsorData();
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

            //Load sponsor names onto checkboxes
            LoadSponsorNamesOnCheckboxes();
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
        SubmitOpenStatus(newVisitID);
      
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

    public void SubmitOpenStatus(string newVisitID)
    {
        int count = 1;
        int schoolID;
        int open;     

        //Start while loop for business ID
        while (count < 33)
        {
            //Check if count is 2, 3, 4, or 5. Those businesses are not needed to open
            //if (count == 2 || count == 3 || count == 4 || count == 5)
            //{
            //    count = count + 1;
            //}
            //else
            //{
                var OpenStatus = GetSchoolIDAndOpenStatus(count);

                //Get schoolID from ddl
                schoolID = OpenStatus.schoolID;
                open = OpenStatus.openStatus;

                //Insert data into openStatusFP
                try
                {
                    using (SqlConnection con = new SqlConnection(ConnectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(@"INSERT INTO openStatusFP (visitID, schoolID, sponsorID, openStatus) VALUES (@visitID, @schoolID, @sponsorID, @openStatus);"))
                        {
                            // Date that is inputed in the textbox
                            cmd.Parameters.Add("@visitID", SqlDbType.Int).Value = newVisitID;
                            cmd.Parameters.Add("@schoolID", SqlDbType.Int).Value = schoolID;
                            cmd.Parameters.Add("@sponsorID", SqlDbType.Int).Value = count;
                            cmd.Parameters.Add("@openStatus", SqlDbType.Bit).Value = open;
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                catch
                {
                    lblSuccess.Text = "Error in SubmitOpenStatus(). Could not insert open status for business ID: " + count;
                    return;
                }
            

            //Add one to count, start loop over again
            count++;
        }
    }

    public (int schoolID, int openStatus) GetSchoolIDAndOpenStatus(int countNumber)
    {
        int schoolID = 0;
        int openStatus = 0;

        //Get school ID from the ddl tied to the checkboxes businessID number (count number) AND get the businessID if it is checked
        switch (countNumber) {
            case 1:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen1.SelectedValue);
                if (Checkbox1.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 6:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen6.SelectedValue);
                if (Checkbox6.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 7:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen7.SelectedValue);
                if (Checkbox7.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 8:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen8.SelectedValue);
                if (Checkbox8.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 9:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen9.SelectedValue);
                if (Checkbox9.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 10:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen10.SelectedValue);
                if (Checkbox10.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 11:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen11.SelectedValue);
                if (Checkbox11.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 12:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen12.SelectedValue);
                if (Checkbox12.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 13:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen13.SelectedValue);
                if (Checkbox13.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 14:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen14.SelectedValue);
                if (Checkbox14.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 15:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen15.SelectedValue);
                if (Checkbox15.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 16:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen16.SelectedValue);
                if (Checkbox16.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 17:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen17.SelectedValue);
                if (Checkbox17.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 18:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen18.SelectedValue);
                if (Checkbox18.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 19:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen19.SelectedValue);
                if (Checkbox19.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 20:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen20.SelectedValue);
                if (Checkbox20.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 21:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen21.SelectedValue);
                if (Checkbox21.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 22:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen22.SelectedValue);
                if (Checkbox22.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 23:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen23.SelectedValue);
                if (Checkbox23.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 24:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen24.SelectedValue);
                if (Checkbox24.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 25:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen25.SelectedValue);
                if (Checkbox25.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 26:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen26.SelectedValue);
                if (Checkbox26.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 27:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen27.SelectedValue);
                if (Checkbox27.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 28:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen28.SelectedValue);
                if (Checkbox28.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 29:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen29.SelectedValue);
                if (Checkbox29.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 30:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen30.SelectedValue);
                if (Checkbox30.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 31:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen31.SelectedValue);
                if (Checkbox31.Checked == true)
                {
                    openStatus = 1;
                }
                break;
            case 32:
                schoolID = SchoolData.GetSchoolID(ddlSchoolOpen32.SelectedValue);
                if (Checkbox32.Checked == true)
                {
                    openStatus = 1;
                }
                break;
        }

        return (schoolID, openStatus);
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

    public void AddSchoolToOpenDDL(string SchoolName)
    {
        //Add the passed through school name to all open status ddls
        ddlSchoolOpen1.Items.Add(SchoolName);
        ddlSchoolOpen2.Items.Add(SchoolName);
        ddlSchoolOpen3.Items.Add(SchoolName);
        ddlSchoolOpen4.Items.Add(SchoolName);
        ddlSchoolOpen5.Items.Add(SchoolName);
        ddlSchoolOpen6.Items.Add(SchoolName);
        ddlSchoolOpen7.Items.Add(SchoolName);
        ddlSchoolOpen8.Items.Add(SchoolName);
        ddlSchoolOpen9.Items.Add(SchoolName);
        ddlSchoolOpen10.Items.Add(SchoolName);
        ddlSchoolOpen11.Items.Add(SchoolName);
        ddlSchoolOpen12.Items.Add(SchoolName); 
        ddlSchoolOpen13.Items.Add(SchoolName);
        ddlSchoolOpen14.Items.Add(SchoolName);
        ddlSchoolOpen15.Items.Add(SchoolName);
        ddlSchoolOpen16.Items.Add(SchoolName);
        ddlSchoolOpen17.Items.Add(SchoolName);
        ddlSchoolOpen18.Items.Add(SchoolName);
        ddlSchoolOpen19.Items.Add(SchoolName);
        ddlSchoolOpen20.Items.Add(SchoolName);
        ddlSchoolOpen21.Items.Add(SchoolName);
        ddlSchoolOpen22.Items.Add(SchoolName);
        ddlSchoolOpen23.Items.Add(SchoolName);
        ddlSchoolOpen24.Items.Add(SchoolName);
        ddlSchoolOpen25.Items.Add(SchoolName);
        ddlSchoolOpen26.Items.Add(SchoolName);
        ddlSchoolOpen27.Items.Add(SchoolName);
        ddlSchoolOpen28.Items.Add(SchoolName);
        ddlSchoolOpen29.Items.Add(SchoolName);
        ddlSchoolOpen30.Items.Add(SchoolName);
        ddlSchoolOpen31.Items.Add(SchoolName);
        ddlSchoolOpen32.Items.Add(SchoolName);
    }

    public void LoadSponsorNamesOnCheckboxes()
    {
        int count = 1;

        //Get sponsor names and apply them to each checkbox
        while (count < 33)
        {

            //Check off checkboxes and select the school name in the DDL
            switch (count)
            {
                case 1:
                    Checkbox1.Text = Sponsors.GetSponsorName(count);
                    break;
                case 2:
                    Checkbox2.Text = Sponsors.GetSponsorName(count);
                    break;
                case 3:
                    Checkbox3.Text = Sponsors.GetSponsorName(count);
                    break;
                case 4:
                    Checkbox4.Text = Sponsors.GetSponsorName(count);
                    break;
                case 5:
                    Checkbox5.Text = Sponsors.GetSponsorName(count);
                    break;
                case 6:
                    Checkbox6.Text = Sponsors.GetSponsorName(count);
                    break;
                case 7:
                    Checkbox7.Text = Sponsors.GetSponsorName(count);
                    break;
                case 8:
                    Checkbox8.Text = Sponsors.GetSponsorName(count);
                    break;
                case 9:
                    Checkbox9.Text = Sponsors.GetSponsorName(count);
                    break;
                case 10:
                    Checkbox10.Text = Sponsors.GetSponsorName(count);
                    break;
                case 11:
                    Checkbox11.Text = Sponsors.GetSponsorName(count);
                    break;
                case 12:
                    Checkbox12.Text = Sponsors.GetSponsorName(count);
                    break;
                case 13:
                    Checkbox13.Text = Sponsors.GetSponsorName(count);
                    break;
                case 14:
                    Checkbox14.Text = Sponsors.GetSponsorName(count);
                    break;
                case 15:
                    Checkbox15.Text = Sponsors.GetSponsorName(count);
                    break;
                case 16:
                    Checkbox16.Text = Sponsors.GetSponsorName(count);
                    break;
                case 17:
                    Checkbox17.Text = Sponsors.GetSponsorName(count);
                    break;
                case 18:
                    Checkbox18.Text = Sponsors.GetSponsorName(count);
                    break;
                case 19:
                    Checkbox19.Text = Sponsors.GetSponsorName(count);
                    break;
                case 20:
                    Checkbox20.Text = Sponsors.GetSponsorName(count);
                    break;
                case 21:
                    Checkbox21.Text = Sponsors.GetSponsorName(count);
                    break;
                case 22:
                    Checkbox22.Text = Sponsors.GetSponsorName(count);
                    break;
                case 23:
                    Checkbox23.Text = Sponsors.GetSponsorName(count);
                    break;
                case 24:
                    Checkbox24.Text = Sponsors.GetSponsorName(count);
                    break;
                case 25:
                    Checkbox25.Text = Sponsors.GetSponsorName(count);
                    break;
                case 26:
                    Checkbox26.Text = Sponsors.GetSponsorName(count);
                    break;
                case 27:
                    Checkbox27.Text = Sponsors.GetSponsorName(count);
                    break;
                case 28:
                    Checkbox28.Text = Sponsors.GetSponsorName(count);
                    break;
                case 29:
                    Checkbox29.Text = Sponsors.GetSponsorName(count);
                    break;
                case 30:
                    Checkbox30.Text = Sponsors.GetSponsorName(count);
                    break;
                case 31:
                    Checkbox31.Text = Sponsors.GetSponsorName(count);
                    break;
                case 32:
                    Checkbox32.Text = Sponsors.GetSponsorName(count);
                    break;

            }

            //Add one to count
            count++;

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
            Checkbox2.Checked = true;
            Checkbox3.Checked = true;
            Checkbox4.Checked = true;
            Checkbox5.Checked = true;
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
            Checkbox2.Checked = false;
            Checkbox3.Checked = false;
            Checkbox4.Checked = false;
            Checkbox5.Checked = false;
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

        //If ddl is selected
        if (ddlSchools.SelectedIndex != 0)
        {
            //Make teacher div and header tag and school 2 ddl visible
            aTeacher1.Visible = true;
            divTeachers1.Visible = true;
            pSchool2.Visible = true;
            ddlSchools2.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools.SelectedValue, ddlTeacher1);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools.SelectedValue, ddlTeacher12);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools.SelectedValue, ddlTeacher13);

            //Add selected school name from ddlschools to all open status ddls
            AddSchoolToOpenDDL(ddlSchools.SelectedValue);
        }
        else
        {
            aTeacher1.Visible=false;
            divTeachers1.Visible=false;
            pSchool2.Visible = false;
            ddlSchools2.Visible = false;
        }
    }

    protected void ddlSchools2_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        ddlTeacher2.Items.Clear();
        ddlTeacher22.Items.Clear();
        ddlTeacher23.Items.Clear();

        //If schools 2 is selected
        if (ddlSchools2.SelectedIndex != 0)
        {
            //Make teacher div and header tag and school 3 ddl visible
            aTeacher2.Visible = true;
            divTeachers2.Visible = true;
            pSchool3.Visible = true;
            ddlSchools3.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools2.SelectedValue, ddlTeacher2);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools2.SelectedValue, ddlTeacher22);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools2.SelectedValue, ddlTeacher23);

            //Add selected school name from ddlschools to all open status ddls
            AddSchoolToOpenDDL(ddlSchools2.SelectedValue);
        }
        else
        {
            aTeacher2.Visible = false;
            divTeachers2.Visible = false;
            pSchool3.Visible = false;
            ddlSchools3.Visible = false;
        }
    }

    protected void ddlSchools3_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        ddlTeacher3.Items.Clear();
        ddlTeacher32.Items.Clear();
        ddlTeacher33.Items.Clear();

        //If schools 3 is selected
        if (ddlSchools3.SelectedIndex != 0)
        {
            //Make teacher div and header tag and school 4 ddl visible
            aTeacher3.Visible = true;
            divTeachers3.Visible = true;
            pSchool4.Visible = true;
            ddlSchools4.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools3.SelectedValue, ddlTeacher3);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools3.SelectedValue, ddlTeacher32);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools3.SelectedValue, ddlTeacher33);

            //Add selected school name from ddlschools to all open status ddls
            AddSchoolToOpenDDL(ddlSchools3.SelectedValue);
        }
        else
        {
            aTeacher3.Visible = false;
            divTeachers3.Visible = false;
            pSchool4.Visible = false;
            ddlSchools4.Visible = false;
        }
    }

    protected void ddlSchools4_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        ddlTeacher4.Items.Clear();
        ddlTeacher42.Items.Clear();
        ddlTeacher43.Items.Clear();

        //if schools 4 is selected
        if (ddlSchools4.SelectedIndex != 0)
        {
            //Make teacher div and header tag and school 5 ddl visible
            aTeacher4.Visible = true;
            divTeachers4.Visible = true;
            pSchool5.Visible = true;
            ddlSchools5.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools4.SelectedValue, ddlTeacher4);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools4.SelectedValue, ddlTeacher42);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools4.SelectedValue, ddlTeacher43);

            //Add selected school name from ddlschools to all open status ddls
            AddSchoolToOpenDDL(ddlSchools4.SelectedValue);
        }
        else
        {
            aTeacher4.Visible = false;
            divTeachers4.Visible = false;
            pSchool5.Visible = false;
            ddlSchools5.Visible = false;
        }
    }

    protected void ddlSchools5_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        ddlTeacher5.Items.Clear();
        ddlTeacher52.Items.Clear();
        ddlTeacher53.Items.Clear();

        //If schools 5 is selected
        if (ddlSchools5.SelectedIndex != 0)
        {
            //Make teacher div and header tag visible
            aTeacher5.Visible = true;
            divTeachers5.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools5.SelectedValue, ddlTeacher5);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools5.SelectedValue, ddlTeacher52);
            TeacherData.LoadTeacherNameDDLFromSchoolName(ddlSchools5.SelectedValue, ddlTeacher53);

            //Add selected school name from ddlschools to all open status ddls
            AddSchoolToOpenDDL(ddlSchools5.SelectedValue);
        }
        else
        {
            aTeacher5.Visible = false;
            divTeachers5.Visible = false;
        }
    }

}