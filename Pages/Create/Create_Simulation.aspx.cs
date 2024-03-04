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

public partial class Create_Simulation : System.Web.UI.Page
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
        string visitTime = visitTime_ddl.SelectedValue;
        string studentCount = studentCount_tb.Text;
        string vTrainingStart = volunteerTime_tb.Text;
        string dueBy = dueBy_tb.Text;
        string school1;
        string school2 = schools2_ddl.SelectedValue;
        string school3 = schools3_ddl.SelectedValue;
        string school4 = schools4_ddl.SelectedValue;
        string school5 = schools5_ddl.SelectedValue;
        string newVisitID;

        // Check for empty fields
        if (visitDate_tb.Text == "" || schools_ddl.SelectedIndex == 0)
        {
            success_lbl.Text = "Please enter a visit date and select a school.";
            return;
        }
        else
        {
            visitDate = visitDate_tb.Text;
            school1 = schools_ddl.SelectedValue;
        }
            
        if (studentCount_tb.Text == "" )
        {
            studentCount = "0";
        }            
        else if (int.Parse(studentCount) < 0)
        {
            success_lbl.Text = "Please enter a student count 0 or higher.";
            return;
        }

        if (volunteerTime_tb.Text == "" )
        {
            vTrainingStart = "00:00";
        }

        if (dueBy_tb.Text == "")
        {
            dueBy = visitDate_tb.Text;
        }

        // Inserting new visit date into DB
        try
        {
            using (SqlConnection con = new SqlConnection(connection_string))
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
            success_lbl.Text = "Error in Submit(). Could not insert visit into table.";
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
            success_lbl.Text = "Error in Submit(). Could update current visit date on school table.";
            return;
        }

        //Get new visit ID
        newVisitID = VisitData.GetVisitIDFromDate(visitDate).ToString();

        //Insert openStatusFP
        try
        {
            using (SqlConnection con = new SqlConnection(connection_string))
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
            success_lbl.Text = "Error in Submit(). Could not open businesses.";
            return;
        }

        //Update current visit ID in teachersInfoFP
        UpdateTeachers(newVisitID);
        
        // Refresh page
        HtmlMeta meta = new HtmlMeta();
        meta.HttpEquiv = "Refresh";
        meta.Content = "4;url=create_simulation.aspx";
        this.Page.Controls.Add(meta);
        //ScriptManager.RegisterStartupScript(Page, this.GetType(), "ScrollPage", "window.scroll(0,0);", true);
        success_lbl.Text = "Submission Successful! Refreshing page...";      
    }

    public void UpdateTeachers(string VisitID)
    {
        //Check if school 1 has teachers
        if (schools_ddl.SelectedIndex != 0)
        {
            if (teacher1_ddl.SelectedIndex != 0)
            {
                string[] t11 = teacher1_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t11[0], t11[1]).ToString(), VisitID);
            }

            if (teacher12_ddl.SelectedIndex != 0)
            {
                string[] t12 = teacher12_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t12[0], t12[1]).ToString(), VisitID);
            }

            if (teacher13_ddl.SelectedIndex != 0)
            {
               string[] t13 = teacher13_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t13[0], t13[1]).ToString(), VisitID);
            }
        }

        //Check if school 2 has teachers
        if (schools2_ddl.SelectedIndex != 0)
        {
            if (teacher2_ddl.SelectedIndex != 0)
            {
                string[] t21 = teacher2_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t21[0], t21[1]).ToString(), VisitID);
            }
            if (teacher22_ddl.SelectedIndex != 0)
            {
                string[] t22 = teacher22_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t22[0], t22[1]).ToString(), VisitID);
            }
            if (teacher23_ddl.SelectedIndex != 0)
            {
                string[] t23 = teacher23_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t23[0], t23[1]).ToString(), VisitID);
            }
        }

        //Check if school 3 has teachers
        if (schools3_ddl.SelectedIndex != 0)
        {
            if (teacher3_ddl.SelectedIndex != 0)
            {
                string[] t31 = teacher3_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t31[0], t31[1]).ToString(), VisitID);
            }
            if (teacher32_ddl.SelectedIndex != 0)
            {
                string[] t32 = teacher32_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t32[0], t32[1]).ToString(), VisitID);
            }
            if (teacher33_ddl.SelectedIndex != 0)
            {
                string[] t33 = teacher33_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t33[0], t33[1]).ToString(), VisitID);
            }
        }

        //Check if school 4 has teachers
        if (schools4_ddl.SelectedIndex != 0)
        {
            if (teacher4_ddl.SelectedIndex != 0)
            {
                string[] t41 = teacher4_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t41[0], t41[1]).ToString(), VisitID);
            }
            if (teacher42_ddl.SelectedIndex != 0)
            {
                string[] t42 = teacher42_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t42[0], t42[1]).ToString(), VisitID);
            }
            if (teacher43_ddl.SelectedIndex != 0)
            {
                string[] t43 = teacher43_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t43[0], t43[1]).ToString(), VisitID);
            }
        }

        //Check if school 5 has teachers
        if (schools5_ddl.SelectedIndex != 0)
        {
            if (teacher5_ddl.SelectedIndex != 0)
            {
                string[] t51 = teacher5_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t51[0], t51[1]).ToString(), VisitID);
            }
            if (teacher52_ddl.SelectedIndex != 0)
            {
                string[] t52 = teacher52_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t52[0], t52[1]).ToString(), VisitID);
            }
            if (teacher53_ddl.SelectedIndex != 0)
            {
                string[] t53 = teacher53_ddl.SelectedValue.Split(' ');

                //Update the current visit ID in teacherInfoFP
                TeacherData.UpdateCurrentVisit(TeacherData.GetTeacherIDFromName(t53[0], t53[1]).ToString(), VisitID);
            }
        }
    }



    protected void visitTime_ddl_SelectedIndexChanged1(object sender, EventArgs e)
    {
        volunteerTime_tb.Text = SchoolSchedule.GetVolArrivalTime(visitTime_ddl.SelectedValue).ToString();
    }

    protected void Submit_btn_Click1(object sender, EventArgs e)
    {
        Submit();
    }

    protected void openAll_btn_Click(object sender, EventArgs e)
    {
        if (openAll_btn.Text == "Open All Businesses")
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
            openAll_btn.Text = "Close All Businesses";
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
            openAll_btn.Text = "Open All Businesses";
        }
    }

    protected void visitDate_tb_TextChanged(object sender, EventArgs e)
    {
        if (visitDate_tb.Text != "")
        {
            //Clear error label
            error_lbl.Text = "";

            // Check if visit date has already been created
            try
            {
                if (VisitData.LoadVisitInfoFromDate(visitDate_tb.Text, "visitDate").ToString() != "")
                {
                    //error_lbl.Text = VisitData.LoadVisitInfoFromDate(visitDate_tb.Text, "visitDate").ToString();
                    error_lbl.Text = "A visit date has already been created for that day, please go to the 'Edit Visit' page to edit the visit for your inputted date.";
                    return;
                }
            }
            catch
            {
                error_lbl.Text = "Error. Could not check if visit date has been created.";
                return;
            }
        }
    }

    protected void schools_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        teacher1_ddl.Items.Clear();
        teacher12_ddl.Items.Clear();
        teacher13_ddl.Items.Clear();

        if (schools_ddl.SelectedIndex != 0)
        {
            teacher1_a.Visible = true;
            teachers1_div.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools_ddl.SelectedValue, teacher1_ddl);
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools_ddl.SelectedValue, teacher12_ddl);
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools_ddl.SelectedValue, teacher13_ddl);
        }
        else
        {
            teacher1_a.Visible=false;
            teachers1_div.Visible=false;
        }
    }

    protected void schools2_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        teacher2_ddl.Items.Clear();
        teacher22_ddl.Items.Clear();
        teacher23_ddl.Items.Clear();

        if (schools2_ddl.SelectedIndex != 0)
        {
            teacher2_a.Visible = true;
            teachers2_div.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools2_ddl.SelectedValue, teacher2_ddl);
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools2_ddl.SelectedValue, teacher22_ddl);
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools2_ddl.SelectedValue, teacher23_ddl);
        }
        else
        {
            teacher2_a.Visible = false;
            teachers2_div.Visible = false;
        }
    }

    protected void schools3_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        teacher3_ddl.Items.Clear();
        teacher32_ddl.Items.Clear();
        teacher33_ddl.Items.Clear();

        if (schools3_ddl.SelectedIndex != 0)
        {
            teacher3_a.Visible = true;
            teachers3_div.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools3_ddl.SelectedValue, teacher3_ddl);
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools3_ddl.SelectedValue, teacher32_ddl);
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools3_ddl.SelectedValue, teacher33_ddl);
        }
        else
        {
            teacher3_a.Visible = false;
            teachers3_div.Visible = false;
        }
    }

    protected void schools4_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        teacher4_ddl.Items.Clear();
        teacher42_ddl.Items.Clear();
        teacher43_ddl.Items.Clear();

        if (schools4_ddl.SelectedIndex != 0)
        {
            teacher4_a.Visible = true;
            teachers4_div.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools4_ddl.SelectedValue, teacher4_ddl);
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools4_ddl.SelectedValue, teacher42_ddl);
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools4_ddl.SelectedValue, teacher43_ddl);
        }
        else
        {
            teacher4_a.Visible = false;
            teachers4_div.Visible = false;
        }
    }

    protected void schools5_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Clear DDLs
        teacher5_ddl.Items.Clear();
        teacher52_ddl.Items.Clear();
        teacher53_ddl.Items.Clear();

        if (schools5_ddl.SelectedIndex != 0)
        {
            teacher5_a.Visible = true;
            teachers5_div.Visible = true;

            //Load teacher DDLs
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools5_ddl.SelectedValue, teacher5_ddl);
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools5_ddl.SelectedValue, teacher52_ddl);
            TeacherData.LoadTeacherNameDDLFromSchoolName(schools5_ddl.SelectedValue, teacher53_ddl);
        }
        else
        {
            teacher5_a.Visible = false;
            teachers5_div.Visible = false;
        }
    }

}