using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Sim_Research : System.Web.UI.Page
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
    private int VisitID;

    public Sim_Research()
    {
        ConnectionString = "Server=" + SQLServer + ";database=" + SQLDatabase + ";uid=" + SQLUser + ";pwd=" + SQLPassword + ";Connection Timeout=20;";
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        
    }



    protected void enter_btn_Click(object sender, EventArgs e)
    {
        string ID;

        //Get ID from textbox if not blank
        if (businessID_tb.Text != "")
        {
            ID = businessID_tb.Text;

            //Check if ID matches the kiosk ID of a business in the DB
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT kioskID FROM businessInfoFP WHERE kioskID = '" + ID + "'";
                dr = cmd.ExecuteReader();

                if (dr.HasRows == true) {
                    //redirect to Sim_Business
                    Response.Redirect("Sim_Business.aspx?b=" + ID);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
                    popupText_lbl.ForeColor = System.Drawing.Color.Red;
                    popupText_lbl.Text = "ID is not tied to a business. Check the number again before entering.";
                }
        }
            catch
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
                popupText_lbl.ForeColor = System.Drawing.Color.Red;
                popupText_lbl.Text = "Could not detect business ID in database. Please find a staff member.";
                return;
            }
    }
        else
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "TogglePopup", "toggle();", true);
            popupText_lbl.ForeColor = System.Drawing.Color.Red;
            popupText_lbl.Text = "No ID entered. Please enter a business ID before submitting.";
        }
        
    }

    protected void cancel_btn_Click(object sender, EventArgs e)
    {
        //Refresh page
        Response.Redirect("Sim_Research.aspx");
    }
}