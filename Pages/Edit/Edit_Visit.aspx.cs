using Microsoft.SqlServer.Server;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Runtime.Remoting.Lifetime;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_Visit : Page
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
    private Class_SQLCommands SQLC = new Class_SQLCommands();
    private Class_SponsorData Sponsors = new Class_SponsorData();
    private int VisitID;

    public Edit_Visit()
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
            // Assign current visit ID to hidden field
            if (VisitID != 0)
            {
                hfCurrentVisitID.Value = VisitID.ToString();
            }

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();
        }

        //Assign sponsor names to checkboxes
        LoadSponsorNamesOnCheckboxes();
    }

    public void LoadData()
    {
        string VisitDate = tbVisitDate.Text;
        int VIDOfDate = int.Parse(VisitData.GetVisitIDFromDate(VisitDate).ToString());
        int count = 1;
        string SQLStatement = @"SELECT v.id, s.id as 'schoolid1', s2.id as 'schoolid2', s3.id as 'schoolid3', s4.id as 'schoolid4', s5.id as 'schoolid5'
                                , s.schoolName as 'schoolname1', s2.schoolName as 'schoolname2', s3.schoolName as 'schoolname3', s4.schoolName as 'schoolname4', s5.schoolName as 'schoolname5'
                                , v.vTrainingTime, v.vMinCount, v.vMaxCount, FORMAT(v.visitDate, 'yyyy-MM-dd') as visitDate, v.studentCount, v.visitTime, FORMAT(v.dueBy, 'yyyy-MM-dd') as dueBy 
                                FROM visitInfoFP v 
                                LEFT JOIN schoolInfoFP s ON s.ID = v.school 
                                LEFT JOIN schoolInfoFP s2 ON s2.ID = v.school2 
                                LEFT JOIN schoolInfoFP s3 ON s3.ID = v.school3 
                                LEFT JOIN schoolInfoFP s4 ON s4.ID = v.school4 
                                LEFT JOIN schoolInfoFP s5 ON s5.ID = v.school5 
                                WHERE v.visitDate = '" + VisitDate + "'";

        //Clear table
        dgvVisit.DataSource = null;
        dgvVisit.DataBind();

        //Clear error label
        lblError.Text = "";

        //Load visit table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvVisit.DataSource = Review_sds;
            dgvVisit.DataBind();
        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load visit table.";
            return;
        }

        //Close connection
        cmd.Dispose();
        con.Close();

        //Load schools into open business
        LoadSchoolsIntoOpenDDL(VisitDate);

        //Load open or closed businesses
        while (count < 33)
        {
            //Check if count is 2, 3, 4, or 5. Those businesses are not needed to open
            //if (count == 2 || count == 3 || count == 4 || count == 5)
            //{
            //    count = count + 1;
            //}
            //else
            //{
                //Assign variable for function to get the school name and the open status of the count number (businessID)
                var OpenStatus = LoadOpenBusinesses(VIDOfDate, count);

                //Check off checkboxes and select the school name in the DDL
                switch (count)
                {
                    case 1:
                        ddlSchoolOpen1.SelectedIndex = ddlSchoolOpen1.Items.IndexOf(ddlSchoolOpen1.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox1.Checked = true;
                        }
                        break;
                case 2:
                    ddlSchoolOpen2.SelectedIndex = ddlSchoolOpen2.Items.IndexOf(ddlSchoolOpen2.Items.FindByValue(OpenStatus.SchoolName));
                    if (OpenStatus.Open == true)
                    {
                        Checkbox2.Checked = true;
                    }
                    break;
                case 3:
                    ddlSchoolOpen3.SelectedIndex = ddlSchoolOpen3.Items.IndexOf(ddlSchoolOpen3.Items.FindByValue(OpenStatus.SchoolName));
                    if (OpenStatus.Open == true)
                    {
                        Checkbox3.Checked = true;
                    }
                    break;
                case 4:
                    ddlSchoolOpen4.SelectedIndex = ddlSchoolOpen4.Items.IndexOf(ddlSchoolOpen4.Items.FindByValue(OpenStatus.SchoolName));
                    if (OpenStatus.Open == true)
                    {
                        Checkbox4.Checked = true;
                    }
                    break;
                case 5:
                    ddlSchoolOpen5.SelectedIndex = ddlSchoolOpen5.Items.IndexOf(ddlSchoolOpen5.Items.FindByValue(OpenStatus.SchoolName));
                    if (OpenStatus.Open == true)
                    {
                        Checkbox5.Checked = true;
                    }
                    break;
                case 6:
                        ddlSchoolOpen6.SelectedIndex = ddlSchoolOpen6.Items.IndexOf(ddlSchoolOpen6.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox6.Checked = true;
                        }
                        break;
                    case 7:
                        ddlSchoolOpen7.SelectedIndex = ddlSchoolOpen7.Items.IndexOf(ddlSchoolOpen7.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox7.Checked = true;
                        }
                        break;
                    case 8:
                        ddlSchoolOpen8.SelectedIndex = ddlSchoolOpen8.Items.IndexOf(ddlSchoolOpen8.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox8.Checked = true;
                        }
                        break;
                    case 9:
                        ddlSchoolOpen9.SelectedIndex = ddlSchoolOpen9.Items.IndexOf(ddlSchoolOpen9.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox9.Checked = true;
                        }
                        break;
                    case 10:
                        ddlSchoolOpen10.SelectedIndex = ddlSchoolOpen10.Items.IndexOf(ddlSchoolOpen11.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox10.Checked = true;
                        }
                        break;
                    case 11:
                        ddlSchoolOpen11.SelectedIndex = ddlSchoolOpen11.Items.IndexOf(ddlSchoolOpen11.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox11.Checked = true;
                        }
                        break;
                    case 12:
                        ddlSchoolOpen12.SelectedIndex = ddlSchoolOpen12.Items.IndexOf(ddlSchoolOpen12.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox12.Checked = true;
                        }
                        break;
                    case 13:
                        ddlSchoolOpen13.SelectedIndex = ddlSchoolOpen13.Items.IndexOf(ddlSchoolOpen13.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox13.Checked = true;
                        }
                        break;
                    case 14:
                        ddlSchoolOpen14.SelectedIndex = ddlSchoolOpen14.Items.IndexOf(ddlSchoolOpen14.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox14.Checked = true;
                        }
                        break;
                    case 15:
                        ddlSchoolOpen15.SelectedIndex = ddlSchoolOpen15.Items.IndexOf(ddlSchoolOpen15.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox15.Checked = true;
                        }
                        break;
                    case 16:
                        ddlSchoolOpen16.SelectedIndex = ddlSchoolOpen16.Items.IndexOf(ddlSchoolOpen16.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox16.Checked = true;
                        }
                        break;
                    case 17:
                        ddlSchoolOpen17.SelectedIndex = ddlSchoolOpen17.Items.IndexOf(ddlSchoolOpen17.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox17.Checked = true;
                        }
                        break;
                    case 18:
                        ddlSchoolOpen18.SelectedIndex = ddlSchoolOpen18.Items.IndexOf(ddlSchoolOpen18.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox18.Checked = true;
                        }
                        break;
                    case 19:
                        ddlSchoolOpen19.SelectedIndex = ddlSchoolOpen19.Items.IndexOf(ddlSchoolOpen19.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox19.Checked = true;
                        }
                        break;
                    case 20:
                        ddlSchoolOpen20.SelectedIndex = ddlSchoolOpen20.Items.IndexOf(ddlSchoolOpen20.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox20.Checked = true;
                        }
                        break;
                    case 21:
                        ddlSchoolOpen21.SelectedIndex = ddlSchoolOpen21.Items.IndexOf(ddlSchoolOpen21.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox21.Checked = true;
                        }
                        break;
                    case 22:
                        ddlSchoolOpen22.SelectedIndex = ddlSchoolOpen22.Items.IndexOf(ddlSchoolOpen22.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox22.Checked = true;
                        }
                        break;
                    case 23:
                        ddlSchoolOpen23.SelectedIndex = ddlSchoolOpen23.Items.IndexOf(ddlSchoolOpen23.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox23.Checked = true;
                        }
                        break;
                    case 24:
                        ddlSchoolOpen24.SelectedIndex = ddlSchoolOpen24.Items.IndexOf(ddlSchoolOpen24.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox24.Checked = true;
                        }
                        break;
                    case 25:
                        ddlSchoolOpen25.SelectedIndex = ddlSchoolOpen25.Items.IndexOf(ddlSchoolOpen25.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox25.Checked = true;
                        }
                        break;
                    case 26:
                        ddlSchoolOpen26.SelectedIndex = ddlSchoolOpen26.Items.IndexOf(ddlSchoolOpen26.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox26.Checked = true;
                        }
                        break;
                    case 27:
                        ddlSchoolOpen27.SelectedIndex = ddlSchoolOpen27.Items.IndexOf(ddlSchoolOpen27.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox27.Checked = true;
                        }
                        break;
                    case 28:
                        ddlSchoolOpen28.SelectedIndex = ddlSchoolOpen28.Items.IndexOf(ddlSchoolOpen28.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox28.Checked = true;
                        }
                        break;
                    case 29:
                        ddlSchoolOpen29.SelectedIndex = ddlSchoolOpen29.Items.IndexOf(ddlSchoolOpen29.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox29.Checked = true;
                        }
                        break;
                    case 30:
                        ddlSchoolOpen30.SelectedIndex = ddlSchoolOpen30.Items.IndexOf(ddlSchoolOpen30.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox30.Checked = true;
                        }
                        break;
                    case 31:
                        ddlSchoolOpen31.SelectedIndex = ddlSchoolOpen31.Items.IndexOf(ddlSchoolOpen31.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox31.Checked = true;
                        }
                        break;
                    case 32:
                        ddlSchoolOpen32.SelectedIndex = ddlSchoolOpen32.Items.IndexOf(ddlSchoolOpen32.Items.FindByValue(OpenStatus.SchoolName));
                        if (OpenStatus.Open == true)
                        {
                            Checkbox32.Checked = true;
                        }
                        break;
                }

                //Add one to count
                count++;
                               
        }
               
        // Highlight row being edited
        foreach (GridViewRow row in dgvVisit.Rows)
        {
            if (row.RowIndex == dgvVisit.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }

    public (bool Open, string SchoolName) LoadOpenBusinesses(int VisitID, int BusinessID)
    {
        string SQLStatementBusinesses = "SELECT * FROM openStatusFP WHERE visitID=" + VisitID + " AND sponsorID='" + BusinessID + "'";
        string SchoolName = "";
        bool Open = false;

        //Load values
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = SQLStatementBusinesses;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Open = bool.Parse(dr["openStatus"].ToString());
            SchoolName = SchoolData.GetSchoolNameFromID(dr["schoolID"].ToString()).ToString();
        }

        cmd.Dispose();
        con.Close();

        return (Open, SchoolName);
    }

    public void LoadSchoolsIntoOpenDDL(string VisitDate)
    {
        //Add the passed through school name to all open status ddls
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen1);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen2);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen3);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen4);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen5);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen6);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen7);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen8);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen9);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen10);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen11);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen12);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen13);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen14);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen15);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen16);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen17);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen18);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen19);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen20);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen21);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen22);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen23);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen24);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen25);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen26);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen27);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen28);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen29);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen30);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen31);
        SchoolData.LoadVisitDateSchoolsDDL(VisitDate, ddlSchoolOpen32);

        ddlSchoolOpen1.Items.RemoveAt(0);
        ddlSchoolOpen2.Items.RemoveAt(0);
        ddlSchoolOpen3.Items.RemoveAt(0);
        ddlSchoolOpen4.Items.RemoveAt(0);
        ddlSchoolOpen5.Items.RemoveAt(0);
        ddlSchoolOpen6.Items.RemoveAt(0);
        ddlSchoolOpen7.Items.RemoveAt(0);
        ddlSchoolOpen8.Items.RemoveAt(0);
        ddlSchoolOpen9.Items.RemoveAt(0);
        ddlSchoolOpen10.Items.RemoveAt(0);
        ddlSchoolOpen11.Items.RemoveAt(0);
        ddlSchoolOpen12.Items.RemoveAt(0);
        ddlSchoolOpen13.Items.RemoveAt(0);
        ddlSchoolOpen14.Items.RemoveAt(0);
        ddlSchoolOpen15.Items.RemoveAt(0);
        ddlSchoolOpen16.Items.RemoveAt(0);
        ddlSchoolOpen17.Items.RemoveAt(0);
        ddlSchoolOpen18.Items.RemoveAt(0);
        ddlSchoolOpen19.Items.RemoveAt(0);
        ddlSchoolOpen20.Items.RemoveAt(0);
        ddlSchoolOpen21.Items.RemoveAt(0);
        ddlSchoolOpen22.Items.RemoveAt(0);
        ddlSchoolOpen23.Items.RemoveAt(0);
        ddlSchoolOpen24.Items.RemoveAt(0);
        ddlSchoolOpen25.Items.RemoveAt(0);
        ddlSchoolOpen26.Items.RemoveAt(0);
        ddlSchoolOpen27.Items.RemoveAt(0);
        ddlSchoolOpen28.Items.RemoveAt(0);
        ddlSchoolOpen29.Items.RemoveAt(0);
        ddlSchoolOpen30.Items.RemoveAt(0);
        ddlSchoolOpen31.Items.RemoveAt(0);
        ddlSchoolOpen32.Items.RemoveAt(0);
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

    public void UpdateCurrentVisitDate(string VisitDate, string SchoolID)
    {
        string CurrentVisitDate = SchoolData.GetCurrentVisitDate(SchoolID).ToString();

        if (CurrentVisitDate != VisitDate)
        {
            try
            {
                SchoolData.UpdatePreviousVisitDate(SchoolID);
                SchoolData.UpdateCurrentVisitDate(SchoolID, VisitDate);

                dgvVisit.EditIndex = -1;
                LoadData();
            }
            catch
            {
                lblError.Text = "Error in UpdateCurrentVisitDate. Cannot update either the previous or current visit date.";
                return;
            }
        }
    }

    public void UpdateOpenClosedStatus()
    {
        int VisitID = int.Parse(VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString());
        int schoolID;
        int count = 1;
        string openStatus;

        //Load open or closed businesses
        while (count < 33)
        {
            openStatus = "0";

            //Check if count is 2, 3, 4, or 5. Those businesses are not needed to open
            //if (count == 2 || count == 3 || count == 4 || count == 5)
            //{
            //    count = count + 1;
            //}
            //else
            //{
                //Check off checkboxes and select the school name in the DDL
                switch (count)
                {
                    case 1:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen1.SelectedValue);
                        if (Checkbox1.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 2:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen2.SelectedValue);
                        if (Checkbox2.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 3:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen3.SelectedValue);
                        if (Checkbox3.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 4:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen4.SelectedValue);
                        if (Checkbox4.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 5:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen5.SelectedValue);
                        if (Checkbox5.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 6:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen6.SelectedValue);
                        if (Checkbox6.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 7:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen7.SelectedValue);
                        if (Checkbox7.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 8:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen8.SelectedValue);
                        if (Checkbox8.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 9:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen9.SelectedValue);
                        if (Checkbox9.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 10:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen10.SelectedValue);
                        if (Checkbox10.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 11:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen11.SelectedValue);
                        if (Checkbox11.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 12:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen12.SelectedValue);
                        if (Checkbox12.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 13:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen13.SelectedValue);
                        if (Checkbox13.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 14:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen14.SelectedValue);
                        if (Checkbox14.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 15:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen15.SelectedValue);
                        if (Checkbox15.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 16:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen16.SelectedValue);
                        if (Checkbox16.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 17:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen17.SelectedValue);
                        if (Checkbox17.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 18:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen18.SelectedValue);
                        if (Checkbox18.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 19:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen19.SelectedValue);
                        if (Checkbox19.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 20:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen20.SelectedValue);
                        if (Checkbox20.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 21:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen21.SelectedValue);
                        if (Checkbox21.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 22:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen22.SelectedValue);
                        if (Checkbox22.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 23:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen23.SelectedValue);
                        if (Checkbox23.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 24:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen24.SelectedValue);
                        if (Checkbox24.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 25:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen25.SelectedValue);
                        if (Checkbox25.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 26:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen26.SelectedValue);
                        if (Checkbox26.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 27:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen27.SelectedValue);
                        if (Checkbox27.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 28:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen28.SelectedValue);
                        if (Checkbox28.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 29:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen29.SelectedValue);
                        if (Checkbox29.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 30:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen30.SelectedValue);
                        if (Checkbox30.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 31:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen31.SelectedValue);
                        if (Checkbox31.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                    case 32:
                        schoolID = SchoolData.GetSchoolID(ddlSchoolOpen32.SelectedValue);
                        if (Checkbox32.Checked == true)
                        {
                            openStatus = "1";
                        }
                        SQLC.ExecuteSQL("UPDATE openStatusFP SET schoolID='" + schoolID + "', openStatus='" + openStatus + "' WHERE visitID='" + VisitID + "' AND sponsorID='" + count + "'");
                        break;
                        
                }

                //Add one to count
                count++;
       
        }

        //Load data again
        LoadData();
    }



    protected void dgvVisit_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(dgvVisit.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string VisitDate = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbVisitDateDGV")).Text;
        string VisitTime = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbVisitTimeDGV")).Text;
        string School1 = ((DropDownList)dgvVisit.Rows[e.RowIndex].FindControl("ddlSchool1DGV")).SelectedValue;
        string School2 = ((DropDownList)dgvVisit.Rows[e.RowIndex].FindControl("ddlSchool2DGV")).SelectedValue;
        string School3 = ((DropDownList)dgvVisit.Rows[e.RowIndex].FindControl("ddlSchool3DGV")).SelectedValue;
        string School4 = ((DropDownList)dgvVisit.Rows[e.RowIndex].FindControl("ddlSchool4DGV")).SelectedValue;
        string School5 = ((DropDownList)dgvVisit.Rows[e.RowIndex].FindControl("ddlSchool5DGV")).SelectedValue;
        string VTrainingTime = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbVTrainingTimeDGV")).Text;
        string VMin = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbVMinCountDGV")).Text;
        string VMax = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbVMaxCountDGV")).Text;
        string DueBy = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbDueByDGV")).Text;
        string StudentCount = ((TextBox)dgvVisit.Rows[e.RowIndex].FindControl("tbStudentCountDGV")).Text;

        //Update the row
        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("UPDATE visitInfoFP SET school=@school1, vTrainingTime=@vTrainingTime, vMinCount=@vMinCount, vMaxCount=@vMaxCount, dueBy=@dueBy, visitDate=@visitDate, studentCount=@studentCount, school2=@school2, school3=@school3, school4=@school4, visitTime=@visitTime, school5=@school5 WHERE ID=@Id"))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@visitDate", VisitDate);
                    cmd.Parameters.AddWithValue("@visitTime", VisitTime);
                    cmd.Parameters.AddWithValue("@school1", School1);
                    cmd.Parameters.AddWithValue("@school2", School2);
                    cmd.Parameters.AddWithValue("@school3", School3);
                    cmd.Parameters.AddWithValue("@school4", School4);
                    cmd.Parameters.AddWithValue("@school5", School5);
                    cmd.Parameters.AddWithValue("@vTrainingTime", VTrainingTime);
                    cmd.Parameters.AddWithValue("@vMinCount", VMin);
                    cmd.Parameters.AddWithValue("@vMaxCount", VMax);
                    cmd.Parameters.AddWithValue("@dueBy", DueBy);
                    cmd.Parameters.AddWithValue("@studentCount", StudentCount);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            dgvVisit.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowUpdating(). Cannot update row.";
            return;
        }

        //Update previous visit date with new visit date if visit date has changed
        UpdateCurrentVisitDate(VisitDate, School1);
        UpdateCurrentVisitDate(VisitDate, School2);
        UpdateCurrentVisitDate(VisitDate, School3);
        UpdateCurrentVisitDate(VisitDate, School4);
        UpdateCurrentVisitDate(VisitDate, School5);

    }

    protected void dgvVisit_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvVisit.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void dgvVisit_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvVisit.EditIndex = -1;
        LoadData();
    }

    protected void dgvVisit_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvVisit.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void dgvVisit_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblSchool1 = (e.Row.FindControl("lblSchoolName1DGV") as Label).Text;
            string lblSchool2 = (e.Row.FindControl("lblSchoolName2DGV") as Label).Text;
            string lblSchool3 = (e.Row.FindControl("lblSchoolName3DGV") as Label).Text;
            string lblSchool4 = (e.Row.FindControl("lblSchoolName4DGV") as Label).Text;
            string lblSchool5 = (e.Row.FindControl("lblSchoolName5DGV") as Label).Text;
            DropDownList ddlSchool1 = e.Row.FindControl("ddlSchool1DGV") as DropDownList;
            DropDownList ddlSchool2 = e.Row.FindControl("ddlSchool2DGV") as DropDownList;
            DropDownList ddlSchool3 = e.Row.FindControl("ddlSchool3DGV") as DropDownList;
            DropDownList ddlSchool4 = e.Row.FindControl("ddlSchool4DGV") as DropDownList;
            DropDownList ddlSchool5 = e.Row.FindControl("ddlSchool5DGV") as DropDownList;

            //Load gridview school DDLs with school names
            Gridviews.SchoolNames(ddlSchool1, lblSchool1);
            Gridviews.SchoolNames(ddlSchool2, lblSchool2);
            Gridviews.SchoolNames(ddlSchool3, lblSchool3);
            Gridviews.SchoolNames(ddlSchool4, lblSchool4);
            Gridviews.SchoolNames(ddlSchool5, lblSchool5);

        }
    }



    protected void tbVisitDate_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDate.Text != "")
        {
            //Check if visit date exists
            if (VisitData.ShowVisitConfirmation(tbVisitDate.Text).ToString() != "")
            {
                lblError.Text = VisitData.ShowVisitConfirmation(tbVisitDate.Text).ToString();
                return;
            }

            //Clear error label
            lblError.Text = "";

            //Make divs visible
            divOpen.Visible = true;
            divGridview.Visible = true;

            //Load data
            LoadData();
        }
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

    protected void btnUpdate_Click(object sender, EventArgs e)
    {
        UpdateOpenClosedStatus();
    }
}