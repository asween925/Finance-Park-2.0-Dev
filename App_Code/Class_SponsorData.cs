using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public class Class_SponsorData
{
    private Class_VisitData VisitID = new Class_VisitData();
    private int Visit;
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"].ToString();
    private string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"].ToString();
    private string ConnectionString;

    public Class_SponsorData()
    {
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    public object LoadSponsorNamesDDL(DropDownList sponsorNameDDL)
    {
        // Clear out teacher and school DDLs
        sponsorNameDDL.Items.Clear();

        // Populate school DDL from entered visit date
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT sponsorName FROM sponsorsFP ORDER BY sponsorName ASC";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                sponsorNameDDL.Items.Add(dr[0].ToString());
            sponsorNameDDL.Items.Insert(0, "");

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            
        }

        return sponsorNameDDL.Items;
    }
}