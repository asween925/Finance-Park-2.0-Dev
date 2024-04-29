using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Create_Sponsor : Page
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
    private Class_BusinessData BusinessData = new Class_BusinessData();
    private int VisitID;

    public Create_Sponsor()
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
            //Load business name DDLs
            BusinessData.LoadBusinessNamesDDL(ddlBusinessName1);
            BusinessData.LoadBusinessNamesDDL(ddlBusinessName2);
            BusinessData.LoadBusinessNamesDDL(ddlBusinessName3);
            BusinessData.LoadBusinessNamesDDL(ddlBusinessName4);

            // Populating school header
            lblHeaderSchoolName.Text = SchoolHeader.GetSchoolHeader().ToString();
        }
    }

    public void Submit()
    {
        string Logo = "";
        string BID1;
        string BID2 = "0";
        string BID3 = "0";
        string BID4 = "0";

        //Check if required fields are not blank
        if (tbSponsorName.Text == "")
        {
            lblError.Text = "Please enter a sponsor name before submitting.";
            return;
        }
        else if (ddlBusinessName1.SelectedIndex == 0)
        {
            lblError.Text = "Please select a business name before submitting.";
            return;
        }

        //Get business IDs from names
        BID1 = BusinessData.GetBusinessID(ddlBusinessName1.SelectedValue).ToString();

        if (ddlBusinessName2.SelectedIndex != 0)
        {
            BID2 = BusinessData.GetBusinessID(ddlBusinessName2.SelectedValue).ToString();
        }

        if (ddlBusinessName3.SelectedIndex != 0)
        {
            BID3 = BusinessData.GetBusinessID(ddlBusinessName3.SelectedValue).ToString();
        }

        if (ddlBusinessName4.SelectedIndex != 0)
        {
            BID4 = BusinessData.GetBusinessID(ddlBusinessName4.SelectedValue).ToString();
        }

        //If file is being uploaded, run uploadfile()
        if (fuLogo.HasFile == true)
        {
            Logo = fuLogo.FileName;
            UploadFile(tbSponsorName.Text);
        }

        //insert into sponsorsFP
        try
        {
            using (SqlConnection con = new SqlConnection(ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"INSERT INTO sponsorsFP (sponsorName, logoPath, businessID, businessID2, businessID3, businessID4)
										                 VALUES (@sponsorName, @logoPath, @businessID, @businessID2, @businessID3, @businessID4)"))
                {

                    cmd.Parameters.Add("@sponsorName", SqlDbType.VarChar).Value = tbSponsorName.Text;
                    cmd.Parameters.Add("@logoPath", SqlDbType.VarChar).Value = Logo;
                    cmd.Parameters.Add("@businessID", SqlDbType.Int).Value = BID1;
                    cmd.Parameters.Add("@businessID2", SqlDbType.Int).Value = BID2;
                    cmd.Parameters.Add("@businessID3", SqlDbType.Int).Value = BID3;
                    cmd.Parameters.Add("@businessID4", SqlDbType.Int).Value = BID4;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        catch
        {
            lblError.Text = "Error in Submit(). Cannot submit new sponsor.";
            return;
        }

        // Refresh page
        HtmlMeta meta = new HtmlMeta();
        meta.HttpEquiv = "Refresh";
        meta.Content = "4;url=create_sponsor.aspx";
        this.Page.Controls.Add(meta);
        lblError.Text = "Submission Successful! Refreshing page...";
    }

    public void UploadFile(string SponsorName)
    {
        string LogoFolderPath = Server.MapPath(@"~\Media\Sponsor Logos\");
        var fi = new FileInfo(fuLogo.FileName);
        string ext = fi.Extension;
        string FileName = fuLogo.FileName;
        int Count = 2;

        // Start uploading
        try
        {
            //Check if extension is a jpg or png
            if (ext == ".jpg" || ext == ".png")
                {

                //Check if file name in the directory is the same name
                while (Count != 0)
                    {
                        if (File.Exists(LogoFolderPath + FileName))
                        {
                            FileName = FileName + "(" + Count + ")" + ext;
                            Count += 1;
                        }
                        else
                        {
                            break;
                        }
                    }

                //Save the File to the Directory(Folder).
                fuLogo.SaveAs(LogoFolderPath + Path.GetFileName(FileName));
                }
                else
                {
                    lblError.Text = "File not uploaded. File must be a jpg or png.";
                    return;
                }
        }
        catch
        {
            lblError.Text = "Error in uploading. Please try again or click the link under the 'Log Out' button to ask for help.";
            return;
        }


    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
    }
}