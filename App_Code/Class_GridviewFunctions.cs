using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;

public class Class_GridviewFunctions
{
    private string SQLServer = ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string SQLDatabase = ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string SQLUser = ConfigurationManager.AppSettings["db_user"].ToString();
    private string SQLPassword = ConfigurationManager.AppSettings["db_password"].ToString();
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string ConnectionString;

    public Class_GridviewFunctions()
    {
        ConnectionString = "Server=" + SQLServer + ";database=" + SQLDatabase + ";uid=" + SQLUser + ";pwd=" + SQLPassword + ";Connection Timeout=20;";
    }

    //Gets all the school names in the schoolInfoFP and inserts them into a DDL
    public void SchoolNames(DropDownList ddlSchool, string lblSchool)
    {
        ddlSchool.DataSource = GetData("SELECT ID, schoolName FROM schoolInfoFP ORDER BY schoolName");
        ddlSchool.DataTextField = "schoolName";
        ddlSchool.DataValueField = "id";
        ddlSchool.DataBind();
        ddlSchool.Items.Insert(0, "");

        if (lblSchool == "")
        {
            ddlSchool.Items.FindByText("").Selected = true;
        }
        else
        {
            ddlSchool.Items.FindByValue(lblSchool).Selected = true;
        }
    }


    //Gets all the business names in the businessInfoFP and inserts them into a DDL
    public void BusinessNames(DropDownList ddlBusiness, string lblBusiness)
    {
        ddlBusiness.DataSource = GetData("SELECT ID, businessName FROM businessInfoFP ORDER BY businessName");
        ddlBusiness.DataTextField = "businessName";
        ddlBusiness.DataValueField = "id";
        ddlBusiness.DataBind();
        ddlBusiness.Items.Insert(0, "");

        if (lblBusiness == "")
        {
            ddlBusiness.Items.FindByText("").Selected = true;
        }
        else if (lblBusiness == "0")
        {
            ddlBusiness.SelectedIndex = 0;
        }
        else
        {
            ddlBusiness.Items.FindByValue(lblBusiness).Selected = true;
        }
    }


    //Gets all numbers of the order in the questionsFP and inserts them into a DDL
    public void QuestionOrder(DropDownList ddlQOrder, string lblQOrder)
    {
        ddlQOrder.DataSource = GetData("SELECT DISTINCT questionOrder FROM questionsFP ORDER BY questionOrder ASC");
        ddlQOrder.DataTextField = "questionOrder";
        ddlQOrder.DataBind();
        ddlQOrder.Items.Insert(0, "");

        if (lblQOrder == "")
        {
            ddlQOrder.Items.FindByText("").Selected = true;
        }
        else
        {
            ddlQOrder.Items.FindByValue(lblQOrder).Selected = true;
        }
    }


    //Gets all answer types in the questionsFP and inserts them into a DDL
    public void AnswerTypes(DropDownList ddlType, string lblType)
    {
        ddlType.DataSource = GetData("SELECT DISTINCT answerType FROM questionsFP ORDER BY answerType ASC");
        ddlType.DataTextField = "answerType";
        ddlType.DataBind();
        ddlType.Items.Insert(0, "");

        if (lblType == "")
        {
            ddlType.Items.FindByText("").Selected = true;
        }
        else
        {
            ddlType.Items.FindByValue(lblType).Selected = true;
        }
    }


    //Gets all question category in the questionsFP and inserts them into a DDL
    public void QuestionCategory(DropDownList ddlCat, string lblCat)
    {
        ddlCat.DataSource = GetData("SELECT DISTINCT questionCategory FROM questionsFP ORDER BY questionCategory ASC");
        ddlCat.DataTextField = "questionCategory";
        ddlCat.DataBind();
        ddlCat.Items.Insert(0, "");

        if (lblCat == "")
        {
            ddlCat.Items.FindByText("").Selected = true;
        }
        else
        {
            ddlCat.Items.FindByValue(lblCat).Selected = true;
        }
    }


    //Gets all questions short in the questionsFP and inserts them into a DDL
    public void QuestionShort(DropDownList ddlShort, string lblShort)
    {
        ddlShort.DataSource = GetData("SELECT DISTINCT questionShort FROM questionsFP ORDER BY questionShort ASC");
        ddlShort.DataTextField = "questionShort";
        ddlShort.DataBind();
        ddlShort.Items.Insert(0, "");

        if (lblShort == "")
        {
            ddlShort.Items.FindByText("").Selected = true;
        }
        else
        {
            ddlShort.Items.FindByValue(lblShort).Selected = true;
        }
    }

    //Gets all questions short in the questionsFP and inserts them into a DDL
    public void JobTitle(DropDownList ddlJobTitle, string lblJobTitle)
    {
        ddlJobTitle.DataSource = GetData("SELECT DISTINCT id, jobTitle FROM jobsFP ORDER BY jobTitle ASC");
        ddlJobTitle.DataTextField = "jobTitle";
        ddlJobTitle.DataValueField = "id";
        ddlJobTitle.DataBind();
        ddlJobTitle.Items.Insert(0, "");

        if (lblJobTitle == "0")
        {
            ddlJobTitle.Items.FindByText("").Selected = true;
        }
        else
        {
            ddlJobTitle.Items.FindByValue(lblJobTitle).Selected = true;
        }
    }

    private DataSet GetData(string query)
    {
        var cmd = new SqlCommand(query);
        using (var con = new SqlConnection(ConnectionString))
        {
            using (var sda = new SqlDataAdapter())
            {
                cmd.Connection = con;
                sda.SelectCommand = cmd;
                using (var ds = new DataSet())
                {
                    sda.Fill(ds);
                    return ds;
                }
            }
        }
    }
}