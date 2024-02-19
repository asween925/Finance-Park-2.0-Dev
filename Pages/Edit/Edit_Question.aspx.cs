using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IdentityModel.Tokens;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Edit_Question : Page
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
    private Class_SchoolHeader SchoolHeader = new Class_SchoolHeader();
    private Class_GridviewFunctions Gridviews = new Class_GridviewFunctions();
    private int VisitID;

    public Edit_Question()
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
            headerSchoolName_lbl.Text = (SchoolHeader.GetSchoolHeader()).ToString();

            //Load data
            LoadData();
        }
    }

    public void LoadData()
    {
        string SQLStatement = "SELECT * FROM questionsFP";

        //Load data
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            Review_sds.ConnectionString = ConnectionString;
            Review_sds.SelectCommand = SQLStatement;
            questions_dgv.DataSource = Review_sds;
            questions_dgv.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            error_lbl.Text = "Error in LoadData(). Cannot load questions table.";
            return;
        }

        // Highlight row being edited
        foreach (GridViewRow row in questions_dgv.Rows)
        {
            if (row.RowIndex == questions_dgv.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }



    protected void questions_dgv_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(questions_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string QuestionOrder = ((DropDownList)questions_dgv.Rows[e.RowIndex].FindControl("questionOrderDGV_ddl")).SelectedValue;
        string QuestionText = ((TextBox)questions_dgv.Rows[e.RowIndex].FindControl("questionTextDGV_tb")).Text;
        string AnswerType = ((DropDownList)questions_dgv.Rows[e.RowIndex].FindControl("answerTypeDGV_ddl")).SelectedValue;
        string QuestionCategory = ((DropDownList)questions_dgv.Rows[e.RowIndex].FindControl("questionCategoryDGV_ddl")).SelectedValue;
        bool EndOfSim = ((CheckBox)questions_dgv.Rows[e.RowIndex].FindControl("endOfSimDGV_chk")).Checked;
        string QuestionShort = ((DropDownList)questions_dgv.Rows[e.RowIndex].FindControl("questionShortDGV_ddl")).SelectedValue;
        bool Active = ((CheckBox)questions_dgv.Rows[e.RowIndex].FindControl("activeDGV_chk")).Checked;
        string Option1 = ((TextBox)questions_dgv.Rows[e.RowIndex].FindControl("option1DGV_tb")).Text;
        string Option2 = ((TextBox)questions_dgv.Rows[e.RowIndex].FindControl("option2DGV_tb")).Text;
        string Option3 = ((TextBox)questions_dgv.Rows[e.RowIndex].FindControl("option3DGV_tb")).Text;
        string Option4 = ((TextBox)questions_dgv.Rows[e.RowIndex].FindControl("option4DGV_tb")).Text;
        string Option5 = ((TextBox)questions_dgv.Rows[e.RowIndex].FindControl("option5DGV_tb")).Text;

        //Update questionsFP
        try
        {
            SQL.UpdateRow(ID, "questionOrder", QuestionOrder, "questionsFP");
            SQL.UpdateRow(ID, "questionText", QuestionText, "questionsFP");
            SQL.UpdateRow(ID, "answerType", AnswerType, "questionsFP");
            SQL.UpdateRow(ID, "questionCategory", QuestionCategory, "questionsFP");
            SQL.UpdateRow(ID, "endOfSim", EndOfSim.ToString(), "questionsFP");
            SQL.UpdateRow(ID, "questionShort", QuestionShort, "questionsFP");
            SQL.UpdateRow(ID, "active", Active.ToString(), "questionsFP");
            SQL.UpdateRow(ID, "option1", Option1, "questionsFP");
            SQL.UpdateRow(ID, "option2", Option2, "questionsFP");
            SQL.UpdateRow(ID, "option3", Option3, "questionsFP");
            SQL.UpdateRow(ID, "option4", Option4, "questionsFP");
            SQL.UpdateRow(ID, "option5", Option5, "questionsFP");

            questions_dgv.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void questions_dgv_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(questions_dgv.DataKeys[e.RowIndex].Values[0]); // Gets id number

        try
        {
            SQL.DeleteRow(ID, "questionsInfoFP");

            questions_dgv.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            error_lbl.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }
    }

    protected void questions_dgv_RowEditing(object sender, GridViewEditEventArgs e)
    {
        questions_dgv.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void questions_dgv_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        questions_dgv.EditIndex = -1;
        LoadData();
    }

    protected void questions_dgv_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        questions_dgv.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void questions_dgv_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblQOrder = (e.Row.FindControl("questionOrderDGV_lbl") as Label).Text;
            string lblType = (e.Row.FindControl("answerTypeDGV_lbl") as Label).Text;
            string lblCat = (e.Row.FindControl("questionCategoryDGV_lbl") as Label).Text;
            string lblShort = (e.Row.FindControl("questionShortDGV_lbl") as Label).Text;

            DropDownList ddlQOrder = e.Row.FindControl("questionOrderDGV_ddl") as DropDownList;                    
            DropDownList ddlType = e.Row.FindControl("answerTypeDGV_ddl") as DropDownList;
            DropDownList ddlCat = e.Row.FindControl("questionCategoryDGV_ddl") as DropDownList;
            DropDownList ddlShort = e.Row.FindControl("questionShortDGV_ddl") as DropDownList;


            Gridviews.AnswerTypes(ddlType, lblType);
            Gridviews.QuestionOrder(ddlQOrder, lblQOrder);
            Gridviews.QuestionCategory(ddlCat, lblCat);
            Gridviews.QuestionShort(ddlShort, lblShort);
        }
    }

}