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
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();           
        }
    }

    public void LoadData()
    {
        string TaxName = ddlTaxName.SelectedValue.ToString();

        //Reset all divs
        divFederal.Visible = false;
        divFICA.Visible = false;
        divMedicare.Visible = false;

        //Make taxes div visible
        divTaxes.Visible = true;

        if (TaxName == "Federal")
        {
            //Make div visible
            divFederal.Visible = true;

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
                    tbGAIRange1MinS.Text = dr["gaiRange1Min"].ToString();
                    tbGAIRange1MaxS.Text = dr["gaiRange1Max"].ToString();
                    tbGAIRange2MinS.Text = dr["gaiRange2Min"].ToString();
                    tbGAIRange2MaxS.Text = dr["gaiRange2Max"].ToString();
                    tbGAIRange3MinS.Text = dr["gaiRange3Min"].ToString();
                    tbGAIRange3MaxS.Text = dr["gaiRange3Max"].ToString();
                    tbTaxEqual1LeftS.Text = dr["taxEqual1Left"].ToString();
                    tbTaxEqual1RightS.Text = dr["taxEqual1Right"].ToString();
                    tbTaxEqual2LeftS.Text = dr["taxEqual2Left"].ToString();
                    tbTaxEqual2RightS.Text = dr["taxEqual2Right"].ToString();
                    tbTaxEqual3LeftS.Text = dr["taxEqual3Left"].ToString();
                    tbTaxEqual3RightS.Text = dr["taxEqual3Right"].ToString();
                    tbGMI1S.Text = dr["gmi1"].ToString();
                    tbGMI2S.Text = dr["gmi2"].ToString();
                    tbGMI3S.Text = dr["gmi3"].ToString();
                }

                cmd.Dispose();
                con.Close();
            }
            catch
            {
                lblError.Text = "Error in LoadData(). Cannot get data for single federal taxes.";
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
                    tbGAIRange1MinM.Text = dr["gaiRange1Min"].ToString();
                    tbGAIRange1MaxM.Text = dr["gaiRange1Max"].ToString();
                    tbGAIRange2MinM.Text = dr["gaiRange2Min"].ToString();
                    tbGAIRange2MaxM.Text = dr["gaiRange2Max"].ToString();
                    tbGAIRange3MinM.Text = dr["gaiRange3Min"].ToString();
                    tbGAIRange3MaxM.Text = dr["gaiRange3Max"].ToString();
                    tbTaxEqual1LeftM.Text = dr["taxEqual1Left"].ToString();
                    tbTaxEqual1RightM.Text = dr["taxEqual1Right"].ToString();
                    tbTaxEqual2LeftM.Text = dr["taxEqual2Left"].ToString();
                    tbTaxEqual2RightM.Text = dr["taxEqual2Right"].ToString();
                    tbTaxEqual3LeftM.Text = dr["taxEqual3Left"].ToString();
                    tbTaxEqual3RightM.Text = dr["taxEqual3Right"].ToString();
                    tbGMI1M.Text = dr["gmi1"].ToString();
                    tbGMI2M.Text = dr["gmi2"].ToString();
                    tbGMI3M.Text = dr["gmi3"].ToString();
                }

                cmd.Dispose();
                con.Close();
            }
            catch
            {
                lblError.Text = "Error in LoadData(). Cannot get data for single federal taxes.";
                return;
            }

        }
        else if (TaxName == "FICA")
        {
            //Make div visible
            divFICA.Visible = true;

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
                    tbCalcF.Text = dr["taxCalc"].ToString();
                }

                cmd.Dispose();
                con.Close();

            }
            catch
            {
                lblError.Text = "Error in LoadData(). Cannot get data for FICA taxes.";
                return;
            }
        }
        else if (TaxName == "Medicare")
        {
            //Make div visible
            divMedicare.Visible = true;

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
                    tbCalcM.Text = dr["taxCalc"].ToString();
                }

                cmd.Dispose();
                con.Close();

            }
            catch
            {
                lblError.Text = "Error in LoadData(). Cannot get data for medicare taxes.";
                return;
            }
        }
    }

    public void Submit()
    {
        string TaxName = ddlTaxName.SelectedValue.ToString();

        //Check which tax to update
        if (TaxName == "Federal")
        {
            //Update for single federal
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = "UPDATE taxesFP SET gaiRange1Min='" + tbGAIRange1MinS.Text + "', gaiRange1Max='" + tbGAIRange1MaxS.Text + "', gaiRange2Min='" + tbGAIRange2MinS.Text + "', gaiRange2Max='" + tbGAIRange2MaxS.Text + "', gaiRange3Min='" + tbGAIRange3MinS.Text + "', gaiRange3Max='" + tbGAIRange3MaxS.Text + "', taxEqual1Left='" + tbTaxEqual1LeftS.Text + "', taxEqual1Right='" + tbTaxEqual1RightS.Text + "', taxEqual2Left='" + tbTaxEqual2LeftS.Text + "', taxEqual2Right='" + tbTaxEqual2RightS.Text + "', taxEqual3Left='" + tbTaxEqual3LeftS.Text + "', taxEqual3Right='" + tbTaxEqual3RightS.Text + "', gmi1='" + tbGMI1S.Text + "' , gmi2='" + tbGMI2S.Text + "' , gmi3='" + tbGMI3S.Text + "' WHERE taxName='Single Federal'";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

            }
            catch
            {
                lblError.Text = "Error in Submit(). Cannot update taxes for Single Federal.";
                return;
            }

            //Update for married federal
            try
            {
                con.ConnectionString = ConnectionString;
                con.Open();
                cmd.CommandText = "UPDATE taxesFP SET gaiRange1Min='" + tbGAIRange1MinM.Text + "', gaiRange1Max='" + tbGAIRange1MaxM.Text + "', gaiRange2Min='" + tbGAIRange2MinM.Text + "', gaiRange2Max='" + tbGAIRange2MaxM.Text + "', gaiRange3Min='" + tbGAIRange3MinM.Text + "', gaiRange3Max='" + tbGAIRange3MaxM.Text + "', taxEqual1Left='" + tbTaxEqual1LeftM.Text + "', taxEqual1Right='" + tbTaxEqual1RightM.Text + "', taxEqual2Left='" + tbTaxEqual2LeftM.Text + "', taxEqual2Right='" + tbTaxEqual2RightM.Text + "', taxEqual3Left='" + tbTaxEqual3LeftM.Text + "', taxEqual3Right='" + tbTaxEqual3RightM.Text + "', gmi1='" + tbGMI1M.Text + "' , gmi2='" + tbGMI2M.Text + "' , gmi3='" + tbGMI3M.Text + "' WHERE taxName='Married Federal'";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

            }
            catch
            {
                lblError.Text = "Error in Submit(). Cannot update taxes for Married Federal.";
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
                cmd.CommandText = "UPDATE taxesFP SET taxCalc='" + tbCalcF.Text + "' WHERE taxName='Social Security'";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

            }
            catch
            {
                lblError.Text = "Error in Submit(). Cannot update taxes for FICA.";
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
                cmd.CommandText = "UPDATE taxesFP SET taxCalc='" + tbCalcM.Text + "' WHERE taxName='Medicare'";
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                con.Close();

            }
            catch
            {
                lblError.Text = "Error in Submit(). Cannot update taxes for Medicare.";
                return;
            }
        }

        //Show success message and refresh page               
        HtmlMeta meta = new HtmlMeta();
        meta.HttpEquiv = "Refresh";
        meta.Content = "3;url=edit_taxes.aspx";
        this.Page.Controls.Add(meta);
        lblError.Text = "Submission successful! Refreshing page...";
    }

    protected void ddlTaxName_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlTaxName.SelectedIndex != 0)
        {
            LoadData();
        }
    }

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        Submit();
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("edit_taxes.aspx");
    }
}