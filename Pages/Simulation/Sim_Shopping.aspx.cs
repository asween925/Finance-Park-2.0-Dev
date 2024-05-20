using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sim_Shopping : System.Web.UI.Page
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
    private Class_StudentData Students = new Class_StudentData();
    private Class_Simulation Sim = new Class_Simulation();
    private int VisitID;
    private int StudentID;
    private int AcctNum;

    public Sim_Shopping()
    {
        ConnectionString = "Server=" + SQLServer + ";database=" + SQLDatabase + ";uid=" + SQLUser + ";pwd=" + SQLPassword + ";Connection Timeout=20;";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //Get current visit ID and student ID
        VisitID = VisitData.GetVisitID();

        //Check if student id is passed through
        if (Request["b"] != null)
        {
            StudentID = int.Parse(Request["b"]);

            //Get account number
            var Student = Students.StudentLookup(25, StudentID);
            AcctNum = Student.AccountNumber;

            if (!IsPostBack)
            {
                //Load Data
                LoadData(StudentID);
            }         
        }
    }

    protected void LoadData(int StudentID)
    {
        var Student = Students.StudentLookup(25, StudentID);

        //Load NMI, spent amount, and remaining amount
        lblNMI.Text = Student.NMI.ToString("c");
        lblSpent.Text = Student.Spent.ToString("c");
        lblRemaining.Text = (Student.NMI - Student.Spent).ToString("c");

        //Check which businesses have been shopped at

    }

    protected void UnlockBiz(int StudentID)
    {
        bool U1 = false;
        bool U6 = false;
        bool U7 = false;
        bool U8 = false;
        bool U9 = false;
        bool U10 = false;
        bool U11 = false;
        bool U12 = false;
        bool U13 = false;
        bool U14 = false;
        bool U15 = false;
        bool U16 = false;
        bool U17 = false;
        bool U18 = false;
        bool U19 = false;
        bool U20 = false;
        bool U21 = false;
        bool U22 = false;
        bool UVisitID = false;
        bool U24 = false;
        bool U25 = false;
        bool U26 = false;
        bool U27 = false;
        bool U28 = false;
        bool U29 = false;
        bool U30 = false;
        bool U31 = false;
        bool U32 = false;
        string Attribute = "border: 2px solid green;";

        //Check which businesses have been unlocked
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM businessUnlockFP WHERE visitID='" + 25 + "' AND studentID='" + StudentID + "'";
            dr = cmd.ExecuteReader();

            //Prolly a better way to do this but fuck it
            while (dr.Read())
            {
                if (dr["u1"].ToString() == "True")
                {
                    U1 = true;
                }
                if (dr["u6"].ToString() == "True")
                {
                    U6 = true;
                }
                if (dr["u7"].ToString() == "True")
                {
                    U7 = true;
                }
                if (dr["u8"].ToString() == "True")
                {
                    U8 = true;
                }
                if (dr["u9"].ToString() == "True")
                {
                    U9 = true;
                }
                if (dr["u10"].ToString() == "True")
                {
                    U10 = true;
                }
                if (dr["u11"].ToString() == "True")
                {
                    U11 = true;
                }
                if (dr["u12"].ToString() == "True")
                {
                    U12 = true;
                }
                if (dr["u13"].ToString() == "True")
                {
                    U13 = true;
                }
                if (dr["u14"].ToString() == "True")
                {
                    U14 = true;
                }
                if (dr["u15"].ToString() == "True")
                {
                    U15 = true;
                }
                if (dr["u16"].ToString() == "True")
                {
                    U16 = true;
                }
                if (dr["u17"].ToString() == "True")
                {
                    U17 = true;
                }
                if (dr["u18"].ToString() == "True")
                {
                    U18 = true;
                }
                if (dr["u19"].ToString() == "True")
                {
                    U19 = true;
                }
                if (dr["u20"].ToString() == "True")
                {
                    U20 = true;
                }
                if (dr["u21"].ToString() == "True")
                {
                    U21 = true;
                }
                if (dr["u22"].ToString() == "True")
                {
                    U22 = true;
                }
                if (dr["uVisitID"].ToString() == "True")
                {
                    UVisitID = true;
                }
                if (dr["u24"].ToString() == "True")
                {
                    U24 = true;
                }
                if (dr["u25"].ToString() == "True")
                {
                    U25 = true;
                }
                if (dr["u26"].ToString() == "True")
                {
                    U26 = true;
                }
                if (dr["u27"].ToString() == "True")
                {
                    U27 = true;
                }
                if (dr["u28"].ToString() == "True")
                {
                    U28 = true;
                }
                if (dr["u29"].ToString() == "True")
                {
                    U29 = true;
                }
                if (dr["u30"].ToString() == "True")
                {
                    U30 = true;
                }
                if (dr["u31"].ToString() == "True")
                {
                    U31 = true;
                }
                if (dr["u32"].ToString() == "True")
                {
                    U32 = true;
                }
            }

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            lblError.Text = "Error.";
            return;
        }

        //Update CSS of unlocked businesses (again prolly a better way of doing this but f it)

        if (U1 == true)
        {
            btnAutoInsurance.Attributes.Add("style", Attribute);
        }
        if (U7 == true)
        {
            btnBankSave.Attributes.Add("style", Attribute);
        }
        if (U8 == true)
        {

        }
        if (U9 == true)
        {

        }
        if (U10 == true)
        {
            btnChildcare.Attributes.Add("style", Attribute);
        }
        if (U11 == true)
        {
            btnClothing.Attributes.Add("style", Attribute);
        }
        if (U12 == true)
        {
            btnCredit.Attributes.Add("style", Attribute);
        }
        if (U13 == true)
        {
            btnDining.Attributes.Add("style", Attribute);
        }
        if (U14 == true)
        {

        }
        if (U15 == true)
        {
            btnEducation.Attributes.Add("style", Attribute);
        }
        if (U16 == true)
        {
            btnEntertainment.Attributes.Add("style", Attribute);
        }
        if (U17 == true)
        {

        }
        if (U18 == true)
        {
            btnGas.Attributes.Add("style", Attribute);
        }
        if (U19 == true)
        {
            btnGrocery.Attributes.Add("style", Attribute);
        }
        if (U20 == true)
        {

        }
        if (U21 == true)
        {
            btnHomeimp.Attributes.Add("style", Attribute);
        }
        if (U22 == true)
        {
            btnHomeins.Attributes.Add("style", Attribute);
        }
        if (UVisitID == true)
        {
            btnHousing.Attributes.Add("style", Attribute);
        }
        if (U24 == true)
        {
            btnInternet.Attributes.Add("style", Attribute);
        }
        if (U25 == true)
        {
            btnInvestment.Attributes.Add("style", Attribute);
        }
        if (U26 == true)
        {
            btnPhila.Attributes.Add("style", Attribute);
        }
        if (U27 == true)
        {
            btnPhone.Attributes.Add("style", Attribute);
        }
        if (U28 == true)
        {
            btnThatslife.Attributes.Add("style", Attribute);
        }
        if (U29 == true)
        {
            btnTransport.Attributes.Add("style", Attribute);
        }
        if (U30 == true)
        {
            btnUtiwater.Attributes.Add("style", Attribute);
        }
        if (U31 == true)
        {
            btnUtielec.Attributes.Add("style", Attribute);
        }
        if (U32 == true)
        {

        }
    }

    protected void GoShopping()
    {
        string ID;
        int BusinessID = 0;
        int BIDHidden = int.Parse(hfBID.Value);


        //Get ID from textbox if not blank
        if (tbPopupTB.Text != "")
        {
            ID = tbPopupTB.Text;

            //Check if ID matches the kiosk ID of a business in the DB
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT id, kioskID FROM businessInfoFP WHERE kioskID = '" + ID + "'";
                dr = cmd.ExecuteReader();

                if (dr.HasRows == true)
                {
                    while (dr.Read())
                    {
                        //get business id
                        BusinessID = int.Parse(dr["id"].ToString());

                        //Check if Business ID matches the hidden value ID
                        if (BusinessID == BIDHidden)
                        {
                            //redirect to Sim_Business
                            Response.Redirect("Sim_Shopping_Business.aspx?b=" + StudentID + "&c=" + BusinessID);
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
                            lblPopupText.ForeColor = System.Drawing.Color.Red;
                            lblPopupText.Text = "ID entered is not for this business.";
                        }

                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
                    lblPopupText.ForeColor = System.Drawing.Color.Red;
                    lblPopupText.Text = "ID is not tied to a business. Check the number again before entering.";
                }
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
                lblPopupText.ForeColor = System.Drawing.Color.Red;
                lblPopupText.Text = "Could not detect business ID in database. Please find a staff member.";
                return;
            }
        }
        else
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
            lblPopupText.ForeColor = System.Drawing.Color.Red;
            lblPopupText.Text = "No ID entered. Please enter a business ID before submitting.";
        }
    }



    protected void btnEnter_Click(object sender, EventArgs e)
    {
        var Trans = Sim.GetTransitionData(6);

        if (lblPopupText.Text != "Please enter the Business ID to proceed:")
        {
            //Check if unlock code matches
            if (tbPopupTB.Text == Trans.UnlockCode.ToString())
            {
                Response.Redirect("Sim_Final.aspx?b=" + StudentID);
            }
        }
        else
        {
            GoShopping();
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        //Automatically closes the popup
    }

    protected void btnAutoInsurance_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "1";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnBankMort_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "6";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnBankSave_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "7";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnChildcare_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "10";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnClothing_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "11";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnCredit_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "12";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnDining_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "13";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnEducation_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "15";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnEntertainment_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "16";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnGas_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "18";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnGrocery_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "19";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnHomeimp_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "21";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnHomeins_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "22";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnHousing_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "23";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnInternet_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "24";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnInvestment_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "25";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnPhila_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "26";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnPhone_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "27";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnThatslife_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "28";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnTransport_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "29";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnUtiwater_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "30";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnUtielec_Click(object sender, EventArgs e)
    {
        //Assign business ID to hidden value
        hfBID.Value = "31";

        //Open pop up
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        var Trans = Sim.GetTransitionData(5);

        //Update popup text with transition data
        lblPopupText.Text = Trans.HeaderText.ToString();

        //Open popup
        Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);

    }
}