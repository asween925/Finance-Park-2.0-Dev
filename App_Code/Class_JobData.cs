using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for Class_JobData
/// </summary>
public class Class_JobData
{
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"];
    string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"];
    string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"];
    string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"];
    private string ConnectionString;

    public Class_JobData()
    {
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    public object LoadBusinessDDL(DropDownList businessNameDDL)
    {
        // Clear out teacher and school DDLs
        businessNameDDL.Items.Clear();

        // Populate school DDL from entered visit date
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT DISTINCT businessName FROM businessInfoFP ORDER BY businessName ASC";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                businessNameDDL.Items.Add(dr[0].ToString());
            }

            businessNameDDL.Items.Insert(0, "");

            cmd.Dispose();
            con.Close();

        return businessNameDDL.Items;
    }

}