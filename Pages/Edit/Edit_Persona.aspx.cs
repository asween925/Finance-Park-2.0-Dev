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
            //Load job title ddl
            Jobs.LoadJobsDDL(jobTitle_ddl);

            // Populating school header
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load data
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLStatement = "SELECT * FROM personasFP";

        //Clear error
        error_lbl.Text = "";

        //Clear table
        persona_dgv.DataSource = null;
        persona_dgv.DataBind();

        //If loading by the DDL, add school name to search query
        if (jobTitle_ddl.SelectedIndex != 0)
        {
            SQLStatement = SQLStatement + " WHERE jobID='" + Jobs.GetJobIDFromTitle(jobTitle_ddl.SelectedValue).ToString() + "'";
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
            persona_dgv.DataSource = Review_sds;
            persona_dgv.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            error_lbl.Text = "Error in LoadData(). Cannot load personas table.";
            return;
        }

        // Highlight row being edited
        foreach (GridViewRow row in persona_dgv.Rows)
        {
            if (row.RowIndex == persona_dgv.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }

    protected void persona_dgv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(persona_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string JobID = ((DropDownList)persona_dgv.Rows[e.RowIndex].FindControl("jobTitleDGV_ddl")).SelectedValue;
        string JobType = ((DropDownList)persona_dgv.Rows[e.RowIndex].FindControl("jobTypeDGV_ddl")).SelectedValue;
        string GAI = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("gaiDGV_tb")).Text;
        string Age = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("ageDGV_tb")).Text;
        string MaritalStatus = ((DropDownList)persona_dgv.Rows[e.RowIndex].FindControl("maritalStatusDGV_ddl")).SelectedValue;
        string SpouseAge = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("spouseAgeDGV_tb")).Text;
        string NumOfChildren = ((DropDownList)persona_dgv.Rows[e.RowIndex].FindControl("numOfChildrenDGV_ddl")).SelectedValue;
        string Child1Age = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("child1AgeDGV_tb")).Text;
        string Child2Age = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("child2AgeDGV_tb")).Text;
        string CreditScore = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("creditScoreDGV_tb")).Text;
        string NMI = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("nmiDGV_tb")).Text;
        string CCDebt = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("ccDebtDGV_tb")).Text;
        string FurnitureLimit = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("furnitureLimitDGV_tb")).Text;
        string HomeImpLimit = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("homeImpLimitDGV_tb")).Text;
        string LongSavings = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("longSavingsDGV_tb")).Text;
        string EmergFunds = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("emergFundsDGV_tb")).Text;
        string OtherSavings = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("otherSavingsDGV_tb")).Text;
        string AutoLoanAmnt = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("autoLoanAmntDGV_tb")).Text;
        string MortAmnt = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("mortAmntDGV_tb")).Text;
        string ThatsLifeAmnt  = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("thatsLifeAmntDGV_tb")).Text;
        string Description = ((TextBox)persona_dgv.Rows[e.RowIndex].FindControl("descriptionDGV_tb")).Text;

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

            persona_dgv.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void persona_dgv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(persona_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number

        try
        {
            SQL.DeleteRow(ID, "persona_dgv");

            persona_dgv.EditIndex = -1;       // reset the grid after editing

            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }
    }

    protected void persona_dgv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        persona_dgv.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void persona_dgv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        persona_dgv.EditIndex = -1;
        LoadData();
    }

    protected void persona_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        persona_dgv.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void persona_dgv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblJobTitle = (e.Row.FindControl("jobTitleDGV_lbl") as Label).Text;
            string lblJobType = (e.Row.FindControl("jobTypeDGV_lbl") as Label).Text;
            string lblMaritalStatus = (e.Row.FindControl("maritalStatusDGV_lbl") as Label).Text;
            string lblNumOfChild = (e.Row.FindControl("numOfChildrenDGV_lbl") as Label).Text;

            DropDownList ddlJobTitle = e.Row.FindControl("jobTitleDGV_ddl") as DropDownList;
            DropDownList ddlJobType = e.Row.FindControl("jobTypeDGV_ddl") as DropDownList;
            DropDownList ddlMaritalStatus = e.Row.FindControl("maritalStatusDGV_ddl") as DropDownList;
            DropDownList ddlNumOfChild = e.Row.FindControl("numOfChildrenDGV_ddl") as DropDownList;

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


    protected void refresh_btn_Click(object sender, EventArgs e)
    {
        Response.Redirect("edit_persona.aspx");
    }

    protected void jobTitle_ddl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (jobTitle_ddl.SelectedIndex != 0)
        {
            LoadData();
        }        
    }
}