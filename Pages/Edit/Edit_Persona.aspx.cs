using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_Persona : Page
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
    private Class_JobData Jobs = new Class_JobData();
    private int VisitID;

    public Edit_Persona()
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
            //Load job title ddl
            Jobs.LoadJobsDDL(ddlJobTitle);

            // Populating school header
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load data
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLStatement = "SELECT * FROM personasFP";

        //Clear error
        lblError.Text = "";

        //Clear table
        dgvPersona.DataSource = null;
        dgvPersona.DataBind();

        //If loading by the DDL, add school name to search query
        if (ddlJobTitle.SelectedIndex != 0)
        {
            SQLStatement = SQLStatement + " WHERE jobID='" + Jobs.GetJobIDFromTitle(ddlJobTitle.SelectedValue).ToString() + "'";
        }
        else
        {
            SQLStatement = SQLStatement + " ORDER BY id ASC";
        }

        //Load personasFP table
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            dgvPersona.DataSource = Review_sds;
            dgvPersona.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load personas table.";
            return;
        }

        // Highlight row being edited
        foreach (GridViewRow row in dgvPersona.Rows)
        {
            if (row.RowIndex == dgvPersona.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }

    protected void dgvPersona_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(dgvPersona.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string JobID = ((DropDownList)dgvPersona.Rows[e.RowIndex].FindControl("ddlJobTitleDGV")).SelectedValue;
        string JobType = ((DropDownList)dgvPersona.Rows[e.RowIndex].FindControl("ddlJobTypeDGV")).SelectedValue;
        string GAI = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbGAIDGV")).Text;
        string Age = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbAgeDGV")).Text;
        string MaritalStatus = ((DropDownList)dgvPersona.Rows[e.RowIndex].FindControl("ddlMaritalStatusDGV")).SelectedValue;
        string SpouseAge = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbSpouseAgeDGV")).Text;
        string NumOfChildren = ((DropDownList)dgvPersona.Rows[e.RowIndex].FindControl("ddlNumOfChildren")).SelectedValue;
        string Child1Age = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbChild1AgeDGV")).Text;
        string Child2Age = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbChild2AgeDGV")).Text;
        string CreditScore = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbCreditScoreDGV")).Text;
        string NMI = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbNMIDGV")).Text;
        string CCDebt = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbCCDebtDGV")).Text;
        string FurnitureLimit = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbFurnitureLimitDGV")).Text;
        string HomeImpLimit = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbHomeImpLimitDGV")).Text;
        string LongSavings = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbLongSavingsDGV")).Text;
        string EmergFunds = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbEmergFundsDGV")).Text;
        string OtherSavings = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbOtherSavingsDGV")).Text;
        string AutoLoanAmnt = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbAutoLoanAmntDGV")).Text;
        string MortAmnt = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbMortAmntDGV")).Text;
        string ThatsLifeAmnt  = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbThatsLifeAmntDGV")).Text;
        string Description = ((TextBox)dgvPersona.Rows[e.RowIndex].FindControl("tbDescriptionDGV")).Text;

        //Update row
        try
        {
            SQL.UpdateRow(ID, "jobID", JobID, "personasFP");
            SQL.UpdateRow(ID, "jobType", JobType, "personasFP");
            SQL.UpdateRow(ID, "gai", GAI, "personasFP");
            SQL.UpdateRow(ID, "age", Age, "personasFP");
            SQL.UpdateRow(ID, "maritalStatus", MaritalStatus, "personasFP");
            SQL.UpdateRow(ID, "numOfChildren", NumOfChildren, "personasFP");
            SQL.UpdateRow(ID, "creditScore", CreditScore, "personasFP");
            SQL.UpdateRow(ID, "nmi", NMI, "personasFP");
            SQL.UpdateRow(ID, "ccDebt", CCDebt, "personasFP");
            SQL.UpdateRow(ID, "furnitureLimit", FurnitureLimit, "personasFP");
            SQL.UpdateRow(ID, "homeImpLimit", HomeImpLimit, "personasFP");
            SQL.UpdateRow(ID, "longSavings", LongSavings, "personasFP");
            SQL.UpdateRow(ID, "emergFunds", EmergFunds, "personasFP");
            SQL.UpdateRow(ID, "otherSavings", OtherSavings, "personasFP");
            SQL.UpdateRow(ID, "autoLoanAmnt", AutoLoanAmnt, "personasFP");
            SQL.UpdateRow(ID, "mortAmnt", MortAmnt, "personasFP");
            SQL.UpdateRow(ID, "thatsLifeAmnt", ThatsLifeAmnt, "personasFP");
            SQL.UpdateRow(ID, "description", Description, "personasFP");

            //Update marital status if married
            if (MaritalStatus == "Married")
            {
                SQL.UpdateRow(ID, "spouseAge", SpouseAge, "personasFP");
            }

            //Update child 1 and/or 2 age
            if (NumOfChildren == "1")
            {
                SQL.UpdateRow(ID, "child1Age", Child1Age, "personasFP");
            }
            else if (NumOfChildren == "2")
            {
                SQL.UpdateRow(ID, "child1Age", Child1Age, "personasFP");
                SQL.UpdateRow(ID, "child2Age", Child2Age, "personasFP");
            }

            dgvPersona.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void dgvPersona_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(dgvPersona.DataKeys[e.RowIndex].Values[0]); // Gets id number

        try
        {
            SQL.DeleteRow(ID, "dgvPersona");

            dgvPersona.EditIndex = -1;       // reset the grid after editing

            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }
    }

    protected void dgvPersona_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvPersona.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void dgvPersona_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvPersona.EditIndex = -1;
        LoadData();
    }

    protected void dgvPersona_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvPersona.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void dgvPersona_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblJobTitle = (e.Row.FindControl("lblJobTitleDGV") as Label).Text;
            string lblJobType = (e.Row.FindControl("lblJobTypeDGV") as Label).Text;
            string lblMaritalStatus = (e.Row.FindControl("lblMaritalStatusDGV") as Label).Text;
            string lblNumOfChild = (e.Row.FindControl("lblNumOfChildren") as Label).Text;

            DropDownList ddlJobTitle = e.Row.FindControl("ddlJobTitleDGV") as DropDownList;
            DropDownList ddlJobType = e.Row.FindControl("ddlJobTypeDGV") as DropDownList;
            DropDownList ddlMaritalStatus = e.Row.FindControl("ddlMaritalStatusDGV") as DropDownList;
            DropDownList ddlNumOfChild = e.Row.FindControl("ddlNumOfChildren") as DropDownList;

            //Load gridview job DDLs with job title
            Gridviews.JobTitle(ddlJobTitle, lblJobTitle);

            //Find job type
            if (lblJobType != "")
            {
                ddlJobType.Items.FindByValue(lblJobType).Selected = true;
            }

            //Find job type
            if (lblMaritalStatus != "")
            {
                ddlMaritalStatus.Items.FindByValue(lblMaritalStatus).Selected = true;
            }

            //Find job type
            if (lblNumOfChild!= "")
            {
                ddlNumOfChild.Items.FindByValue(lblNumOfChild).Selected = true;
            }

        }
    }


    protected void btnRefresh_Click(object sender, EventArgs e)
    {
        Response.Redirect("edit_persona.aspx");
    }

    protected void ddlJobTitle_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlJobTitle.SelectedIndex != 0)
        {
            LoadData();
        }        
    }
}