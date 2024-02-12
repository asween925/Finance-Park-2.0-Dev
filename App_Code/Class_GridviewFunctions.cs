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

    //public object BusinessNames(DropDownList ddlBusiness, string lblBusiness)
    //{
    //    return;
    //}

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