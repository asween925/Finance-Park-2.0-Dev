using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;


public class Class_Schedule
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

    public Class_Schedule()
    {
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    public void LoadSchoolSchedule()
    {
        string SQLStatement = "SELECT * FROM schoolScheduleFP";

        //Load school schedule table
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = SQLStatement;
        SqlDataAdapter da = new SqlDataAdapter();
        da.SelectCommand = cmd;
        DataTable dt = new DataTable();
        da.Fill(dt);

        cmd.Dispose();
        con.Close();
    }

    public object GetVolArrivalTime(string Time)
    {
        string ArrivalTime = "00:00";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "SELECT CONVERT(VARCHAR(5), timeVolArrive, 108) as timeVolArrive FROM schoolScheduleFP WHERE timeVolArrive = '" + Time + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            ArrivalTime = dr["timeVolArrive"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return ArrivalTime;
    }

    public object GetDismissalTime(string ArrivalTime)
    {
        string DismissalTime = "00:00";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "SELECT CONVERT(VARCHAR(5), leave, 108) as leave FROM schoolScheduleFP WHERE timeVolArrive = '" + ArrivalTime + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read()) {
            DismissalTime = dr["leave"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return DismissalTime;
    }
}