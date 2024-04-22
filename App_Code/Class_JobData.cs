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

    public object LoadJobsDDL(DropDownList jobsDDL)
    {
        // Clear out teacher and school DDLs
        jobsDDL.Items.Clear();

        // Populate school DDL from entered visit date
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT jobTitle FROM jobsFP ORDER BY jobTitle ASC";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                jobsDDL.Items.Add(dr[0].ToString());
            }

            jobsDDL.Items.Insert(0, "");

            cmd.Dispose();
            con.Close();

        return jobsDDL.Items;
    }

    public object GetJobIDFromTitle(string Title)
    {
        string JobID = "0";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT id FROM jobsFP WHERE jobTitle='" + Title + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            JobID = dr["id"].ToString();
        }
      
        cmd.Dispose();
        con.Close();

        return JobID;
    }

    public (string JobTitle, int BusinessID, string EducationBG, string JobDuties, decimal EdDebt, string Advancement) JobLookup (int JobID)
    {
        string JT = "";
        int BID = 0;
        string EBG = "";
        string JD = "";
        decimal ED = 0;
        string A = "";
        string SQL = "SELECT * FROM jobsFP WHERE id='" + JobID + "'";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = SQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            JT = dr["jobTitle"].ToString();
            BID = int.Parse(dr["businessID"].ToString());
            EBG = dr["educationBG"].ToString();
            JD = dr["jobduties"].ToString();
            ED = decimal.Parse(dr["edDebt"].ToString());
            A = dr["advancement"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return (JT, BID, EBG, JD, ED, A);
    }

}