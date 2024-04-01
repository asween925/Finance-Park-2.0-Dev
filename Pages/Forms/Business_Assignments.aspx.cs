using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
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
    private Class_BusinessData BusinessData = new Class_BusinessData();
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
        int BusinessID = int.Parse(BusinessData.GetBusinessID(ddlBusinessName.SelectedValue).ToString());
        int VisitID = int.Parse(VisitData.GetVisitIDFromDate(tbVisitDate.Text).ToString());
        DateTime VisitDate = DateTime.Parse(tbVisitDate.Text);
        string SQLStatement = @"SELECT s.id, s.accountNum, a.pin, CONCAT(s.firstName, ' ', s.lastName) as studentName
                                FROM studentInfoFP s 
                                JOIN accountNumsFP a ON s.accountNum = a.accountNum 
								JOIN businessInfoFP b ON b.id = s.businessID								
                                WHERE s.visitID='" + VisitID + "' AND s.businessID='" + BusinessID + "' ORDER BY s.accountNum ASC";

        //Clear error label
        lblError.Text = "";

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

        //Select and load business logo
        switch (BusinessID)
        {
            case 1:
                {
                    imgBusinessLogo.Attributes["src"] = ResolveUrl("~/Media/FP_SI_Logo.png");
                    break;
                }
            case 2:
                {
                    break;
                }
            case 3:
                {
                    break;
                }
            case 4:
                {
                    break;
                }
        }
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

            //Load businesses of visit date
            BusinessData.LoadBusinessNamesDDL(ddlBusinessName);

            //Show schools div
            divBusinessName.Visible = true;
        }
    }



    protected void ddlBusinessName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlBusinessName.SelectedIndex != 0)
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