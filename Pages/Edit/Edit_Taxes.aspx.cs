using System;
using System.Activities.Statements;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Edit_Taxes : Page
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

    public Edit_Taxes()
    {
        ConnectionString = "Server=" + SQLServer + ";database=" + SQLDatabase + ";uid=" + SQLUser + ";pwd=" + SQLPassword + ";Connection Timeout=20;";
        VisitID = VisitData.GetVisitID();
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
            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();           
        }
    }

    public void LoadData()
    {
        string TaxName = taxName_ddl.SelectedValue.ToString();

        //Reset all divs
        federal_div.Visible = false;
        fica_div.Visible = false;
        medicare_div.Visible = false;

        //Make taxes div visible
        taxes_div.Visible = true;

        if (TaxName == "Federal")
        {
            //Make div visible
            federal_div.Visible = true;

            //Load data for single
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = "SELECT * FROM taxesFP WHERE taxName='Single Federal'";
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    gaiRange1MinS_tb.Text = dr["gaiRange1Min"].ToString();
                    gaiRange1MaxS_tb.Text = dr["gaiRange1Max"].ToString();
                    gaiRange2MinS_tb.Text = dr["gaiRange2Min"].ToString();
                    gaiRange2MaxS_tb.Text = dr["gaiRange2Max"].ToString();
                    gaiRange3MinS_tb.Text = dr["gaiRange3Min"].ToString();
                    gaiRange3MaxS_tb.Text = dr["gaiRange3Max"].ToString();
                    taxEqual1LeftS_tb.Text = dr["taxEqual1Left"].ToString();
                    taxEqual1RightS_tb.Text = dr["taxEqual1Right"].ToString();
                    taxEqual2LeftS_tb.Text = dr["taxEqual2Left"].ToString();
                    taxEqual2RightS_tb.Text = dr["taxEqual2Right"].ToString();
                    taxEqual3LeftS_tb.Text = dr["taxEqual3Left"].ToString();
                    taxEqual3RightS_tb.Text = dr["taxEqual3Right"].ToString();
                    gmi1S_tb.Text = dr["gmi1"].ToString();
                    gmi2S_tb.Text = dr["gmi2"].ToString();
                    gmi3S_tb.Text = dr["gmi3"].ToString();
                }

                cmd.Dispose();
                con.Close();
            }
            catch
            {
                error_lbl.Text = "Error in LoadData(). Cannot get data for single federal taxes.";
                return;
            }

            //Load data for married
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = "SELECT * FROM taxesFP WHERE taxName='Married Federal'";
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    gaiRange1MinM_tb.Text = dr["gaiRange1Min"].ToString();
                    gaiRange1MaxM_tb.Text = dr["gaiRange1Max"].ToString();
                    gaiRange2MinM_tb.Text = dr["gaiRange2Min"].ToString();
                    gaiRange2MaxM_tb.Text = dr["gaiRange2Max"].ToString();
                    gaiRange3MinM_tb.Text = dr["gaiRange3Min"].ToString();
                    gaiRange3MaxM_tb.Text = dr["gaiRange3Max"].ToString();
                    taxEqual1LeftM_tb.Text = dr["taxEqual1Left"].ToString();
                    taxEqual1RightM_tb.Text = dr["taxEqual1Right"].ToString();
                    taxEqual2LeftM_tb.Text = dr["taxEqual2Left"].ToString();
                    taxEqual2RightM_tb.Text = dr["taxEqual2Right"].ToString();
                    taxEqual3LeftM_tb.Text = dr["taxEqual3Left"].ToString();
                    taxEqual3RightM_tb.Text = dr["taxEqual3Right"].ToString();
                    gmi1M_tb.Text = dr["gmi1"].ToString();
                    gmi2M_tb.Text = dr["gmi2"].ToString();
                    gmi3M_tb.Text = dr["gmi3"].ToString();
                }

                cmd.Dispose();
                con.Close();
            }
            catch
            {
                error_lbl.Text = "Error in LoadData(). Cannot get data for single federal taxes.";
                return;
            }

        }
        else if (TaxName == "FICA")
        {
            //Make div visible
            fica_div.Visible = true;

            //Load data for single
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = "SELECT * FROM taxesFP WHERE taxName='Social Security'";
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    calcF_tb.Text = dr["taxCalc"].ToString();
                }

                cmd.Dispose();
                con.Close();

            }
            catch
            {
                error_lbl.Text = "Error in LoadData(). Cannot get data for FICA taxes.";
                return;
            }
        }
        else if (TaxName == "Medicare")
        {
            //Make div visible
            medicare_div.Visible = true;

            //Load data for single
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = "SELECT * FROM taxesFP WHERE taxName='Medicare'";
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    calcM_tb.Text = dr["taxCalc"].ToString();
                }

                cmd.Dispose();
                con.Close();

            }
            catch
            {
                error_lbl.Text = "Error in LoadData(). Cannot get data for medicare taxes.";
                return;
            }
        }
    }

    public void Submit()
    {
        string TaxName = taxName_ddl.SelectedValue.ToString();

        //Check which tax to update
        if (TaxName == "Federal")
        {     
            //Update for single federal
            //try
            //{
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = "UPDATE taxesFP SET gaiRange1Min='" + gaiRange1MinS_tb.Text + "', gaiRange1Max='" + gaiRange1MaxS_tb.Text + "', gaiRange2Min='" + gaiRange2MinS_tb.Text + "', gaiRange2Max='" + gaiRange2MaxS_tb.Text + "', gaiRange3Min='" + gaiRange3MinS_tb.Text + "', gaiRange3Max='" + gaiRange3MaxS_tb.Text + "', taxEqual1Left='" + taxEqual1LeftS_tb.Text + "', taxEqual1Right='" + taxEqual1RightS_tb.Text + "', taxEqual2Left='" + taxEqual2LeftS_tb.Text + "', taxEqual2Right='" + taxEqual2RightS_tb.Text + "', taxEqual3Left='" + taxEqual3LeftS_tb.Text + "', taxEqual3Right='" + taxEqual3RightS_tb.Text + "', gmi1='" + gmi1S_tb.Text + "' , gmi2='" + gmi2S_tb.Text + "' , gmi3='" + gmi3S_tb.Text + "' WHERE taxName='Single Federal'";
            cmd.Connection = con;
            cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();
           
            //}
            //catch
            //{
            //    error_lbl.Text = "Error in Submit(). Cannot update taxes for Single Federal.";
            //    return;
            //}

            //Update for married federal
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = "UPDATE taxesFP SET gaiRange1Min='" + gaiRange1MinM_tb.Text + "', gaiRange1Max='" + gaiRange1MaxM_tb.Text + "', gaiRange2Min='" + gaiRange2MinM_tb.Text + "', gaiRange2Max='" + gaiRange2MaxM_tb.Text + "', gaiRange3Min='" + gaiRange3MinM_tb.Text + "', gaiRange3Max='" + gaiRange3MaxM_tb.Text + "', taxEqual1Left='" + taxEqual1LeftM_tb.Text + "', taxEqual1Right='" + taxEqual1RightM_tb.Text + "', taxEqual2Left='" + taxEqual2LeftM_tb.Text + "', taxEqual2Right='" + taxEqual2RightM_tb.Text + "', taxEqual3Left='" + taxEqual3LeftM_tb.Text + "', taxEqual3Right='" + taxEqual3RightM_tb.Text + "', gmi1='" + gmi1M_tb.Text + "' , gmi2='" + gmi2M_tb.Text + "' , gmi3='" + gmi3M_tb.Text + "' WHERE taxName='Married Federal'";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

            }
            catch
            {
                error_lbl.Text = "Error in Submit(). Cannot update taxes for Married Federal.";
                return;
            }
        }
        else if (TaxName == "FICA")
        {
            //Update for FICA
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = "UPDATE taxesFP SET taxCalc='" + calcF_tb.Text + "' WHERE taxName='Social Security'";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

            }
            catch
            {
                error_lbl.Text = "Error in Submit(). Cannot update taxes for FICA.";
                return;
            }
        }
        else if (TaxName == "Medicare")
        {
            //Update for single federal
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = "UPDATE taxesFP SET taxCalc='" + calcM_tb.Text + "' WHERE taxName='Medicare'";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

            }
            catch
            {
                error_lbl.Text = "Error in Submit(). Cannot update taxes for Medicare.";
                return;
            }
        }

        //Show success message and refresh page               
        HtmlMeta meta = new HtmlMeta();
        meta.HttpEquiv = "Refresh";
        meta.Content = "3;url=edit_taxes.aspx";
        this.Page.Controls.Add(meta);
        error_lbl.Text = "Submission successful! Refreshing page...";
    }

    protected void taxName_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (taxName_ddl.SelectedIndex != 0)
        {
            LoadData();
        }
    }

    protected void submit_btn_Click(object sender, EventArgs e)
    {
        Submit();
    }

    protected void reset_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("edit_taxes.aspx");
    }
}