﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Business_Assignments : Page
{
    private string SQLServer = ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string SQLDatabase = ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string SQLUser = ConfigurationManager.AppSettings["db_user"].ToString();
    private string SQLPassword = ConfigurationManager.AppSettings["db_password"].ToString();
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string ConnectionString;
    private Class_SQLCommands SQL = new Class_SQLCommands();
    private Class_VisitData VisitData = new Class_VisitData();
    private Class_SchoolData SchoolData = new Class_SchoolData();
    private Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    private Class_GridviewFunctions Gridviews = new Class_GridviewFunctions();
    private Class_SponsorData Sponsors = new Class_SponsorData();
    private int VisitID;

    public Business_Assignments()
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
    }

    public void LoadData()
    {
        string SponsorName = ddlSponsorName.SelectedValue;
        int SponsorID = Sponsors.GetSponsorID(SponsorName);
        int VisitID = int.Parse(VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString());
        DateTime VisitDate = DateTime.Parse(tbVisitDate.Text);
        string LogoPath = Sponsors.GetSponsorLogoFromID(SponsorID);
        string SQLStatement = @"SELECT DISTINCT s.id, s.accountNum, a.pin, CONCAT(s.firstName, ' ', s.lastName) as studentName
                                FROM studentInfoFP s 
                                INNER JOIN accountNumsFP a ON s.accountNum = a.accountNum 
								INNER JOIN sponsorsFP b ON s.id = s.sponsorID
                                WHERE s.visitID='" + VisitID + "' AND s.sponsorID='" + SponsorID + "' ORDER BY s.accountNum ASC";

        //Clear error label
        lblError.Text = "";

        //Make fields visible
        divBusinessLogo.Visible = true;
        divBusinessName.Visible = true;
        divPrintHeader.Visible = true;
        divStudents.Visible = true;
        imgStavrosLogo.Visible = true;

        //Clear teacher table
        dgvStudents.DataSource = null;
        dgvStudents.DataBind();

        //Load print only visit date label
        lblPrintVisitDate.Text = VisitDate.ToShortDateString();

        //Load teacherInfoFP table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvStudents.DataSource = Review_sds;
            dgvStudents.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load studentInfo table.";
            return;
        }

        //Load business logo
        imgBusinessLogo.Attributes["src"] = ResolveUrl(LogoPath);
    }



    protected void dgvStudents_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvStudents.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void dgvStudents_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            (e.Row.FindControl("lblRowNumber") as Label).Text = (e.Row.RowIndex + 1).ToString();
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

            //Load businesses of visit date
            Sponsors.LoadSponsorNamesDDL(ddlSponsorName);

            //Show schools div
            divBusinessName.Visible = true;
        }
    }



    protected void ddlSponsorName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlSponsorName.SelectedIndex != 0)
        { 
            LoadData();
        }      
    }



    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("Business_Assignments.aspx");
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        //Make print only div visible
        divPrintHeader.Visible = true;
        
        //Print
        Page.ClientScript.RegisterStartupScript(GetType(), "Print", "print();", true);       
    }
}