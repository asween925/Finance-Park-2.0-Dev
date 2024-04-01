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
            lblHeaderSchoolName.Text = (SchoolHeader.GetSchoolHeader()).ToString();

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
            dgvQuestions.DataSource = Review_sds;
            dgvQuestions.DataBind();

            cmd.Dispose();
            con.Close();

        }
        catch
        {
            lblError.Text = "Error in LoadData(). Cannot load questions table.";
            return;
        }

        // Highlight row being edited
        foreach (GridViewRow row in dgvQuestions.Rows)
        {
            if (row.RowIndex == dgvQuestions.EditIndex)
            {
                row.BackColor = ColorTranslator.FromHtml("#ebe534");
                row.BorderWidth = 2;
            }
        }
    }



    protected void dgvQuestions_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int ID = Convert.ToInt32(dgvQuestions.DataKeys[e.RowIndex].Values[0]); // Gets id number
        string QuestionOrder = ((DropDownList)dgvQuestions.Rows[e.RowIndex].FindControl("ddlQuestionOrderDGV")).SelectedValue;
        string QuestionText = ((TextBox)dgvQuestions.Rows[e.RowIndex].FindControl("tbQuestionTextDGV")).Text;
        string AnswerType = ((DropDownList)dgvQuestions.Rows[e.RowIndex].FindControl("ddlAnswerTypeDGV")).SelectedValue;
        string QuestionCategory = ((DropDownList)dgvQuestions.Rows[e.RowIndex].FindControl("ddlQuestionCategoryDGV")).SelectedValue;
        bool EndOfSim = ((CheckBox)dgvQuestions.Rows[e.RowIndex].FindControl("chkEndOfSimDGV")).Checked;
        string QuestionShort = ((DropDownList)dgvQuestions.Rows[e.RowIndex].FindControl("ddlQuestionShortDGV")).SelectedValue;
        bool Active = ((CheckBox)dgvQuestions.Rows[e.RowIndex].FindControl("chkActiveDGV")).Checked;
        string Option1 = ((TextBox)dgvQuestions.Rows[e.RowIndex].FindControl("tbOption1DGV")).Text;
        string Option2 = ((TextBox)dgvQuestions.Rows[e.RowIndex].FindControl("tbOption2DGV")).Text;
        string Option3 = ((TextBox)dgvQuestions.Rows[e.RowIndex].FindControl("tbOption3DGV")).Text;
        string Option4 = ((TextBox)dgvQuestions.Rows[e.RowIndex].FindControl("tbOption4DGV")).Text;
        string Option5 = ((TextBox)dgvQuestions.Rows[e.RowIndex].FindControl("tbOption5DGV")).Text;

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

            dgvQuestions.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowUpdating. Cannot update row.";
            return;
        }
    }

    protected void dgvQuestions_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int ID = Convert.ToInt32(dgvQuestions.DataKeys[e.RowIndex].Values[0]); // Gets id number

        try
        {
            SQL.DeleteRow(ID, "questionsInfoFP");

            dgvQuestions.EditIndex = -1;       // reset the grid after editing
            LoadData();
        }
        catch
        {
            lblError.Text = "Error in rowDeleting. Cannot delete row.";
            return;
        }
    }

    protected void dgvQuestions_RowEditing(object sender, GridViewEditEventArgs e)
    {
        dgvQuestions.EditIndex = e.NewEditIndex;
        LoadData();
    }

    protected void dgvQuestions_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        dgvQuestions.EditIndex = -1;
        LoadData();
    }

    protected void dgvQuestions_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        dgvQuestions.PageIndex = e.NewPageIndex;
        LoadData();
    }

    protected void dgvQuestions_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if ((e.Row.RowType == DataControlRowType.DataRow))
        {
            string lblQOrder = (e.Row.FindControl("lblQuestionOrderDGV") as Label).Text;
            string lblType = (e.Row.FindControl("lblAnswerTypeDGV") as Label).Text;
            string lblCat = (e.Row.FindControl("lblQuestionCategoryDGV") as Label).Text;
            string lblShort = (e.Row.FindControl("lblQuestionShortDGV") as Label).Text;

            DropDownList ddlQOrder = e.Row.FindControl("ddlQuestionOrderDGV") as DropDownList;                    
            DropDownList ddlType = e.Row.FindControl("ddlAnswerTypeDGV") as DropDownList;
            DropDownList ddlCat = e.Row.FindControl("ddlQuestionCategoryDGV") as DropDownList;
            DropDownList ddlShort = e.Row.FindControl("ddlQuestionShortDGV") as DropDownList;


            Gridviews.AnswerTypes(ddlType, lblType);
            Gridviews.QuestionOrder(ddlQOrder, lblQOrder);
            Gridviews.QuestionCategory(ddlCat, lblCat);
            Gridviews.QuestionShort(ddlShort, lblShort);
        }
    }

}