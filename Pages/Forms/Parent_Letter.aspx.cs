using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Parent_Letter : Page
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
    private int VisitID;

    public Parent_Letter()
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
        string VisitDate = tbVisitDate.Text;
        DateTime vTrainingTime = DateTime.Parse(VisitData.LoadVisitInfoFromDate(VisitDate, "vTrainingTime").ToString());

        // Reveal divs
        divLetter.Visible = true;
        divReturnSlip.Visible = true;

        // Assign visit date labels
        lblVisitDate.Text = DateTime.Parse(VisitDate).ToString("d");
        lblVisitDateLetter.Text = DateTime.Parse(VisitDate).ToString("d");
        lblVisitDateSlip1.Text = DateTime.Parse(VisitDate).ToString("d");

        // Assign training time
        lblTrainingTimeSlip.Text = vTrainingTime.ToString("t");
    }



    protected void tbVisitDate_TextChanged(object sender, EventArgs e)
    {
        if (tbVisitDate.Text != "")
        {
            divLetter.Visible = true;
            divReturnSlip.Visible = true;
        }
    }

    protected void btnPrint_Click(object sender, EventArgs e)
    {
        Page.ClientScript.RegisterStartupScript(this.GetType(), "Print", "print();", true);
    }
}