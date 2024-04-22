using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Class_SchoolSchedule
{
    string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"];
    string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"];
    string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"];
    string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"];
    private string ConnectionString;
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;

    public Class_SchoolSchedule()
    {
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    // Load school schedule table
    public object LoadSchoolSchedule()
    {
        string ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
        var con = new SqlConnection();
        var cmd = new SqlCommand();
        string sqlStatement = "SELECT * FROM schoolScheduleFP";

        // Search and load kit inv table
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = sqlStatement;

        var da = new SqlDataAdapter();
        da.SelectCommand = cmd;
        var dt = new DataTable();
        da.Fill(dt);

        cmd.Dispose();
        con.Close();

        return dt;

    }

    // Populates a DDL with all the available visit times
    public object LoadVisitTimeDDL(DropDownList VisitTimeDDL)
    {
        string errorString;
        // Dim schoolNameDDL As DropDownList

        // Clear out teacher and school DDLs
        VisitTimeDDL.Items.Clear();

        // Populate visit time DDL
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT CONVERT(VARCHAR(5), schoolSchedule, 108) as schoolSchedule FROM schoolScheduleFP";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                VisitTimeDDL.Items.Add(dr[0].ToString());

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            errorString = "Error in visitTime. Could not get school schedule times.";
            return errorString;
        }

        return VisitTimeDDL.Items;
    }

    // Get volunteer time based on the visit time
    public object GetVolArrivalTime(string VisitTime)
    {
        string errorString;
        var VolArrivalTime = default(string);

        // Populate visit time DDL
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT CONVERT(VARCHAR(5), volArrive, 108) as volArrive FROM schoolScheduleFP WHERE schoolSchedule = '" + VisitTime + "'";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                VolArrivalTime = dr["volArrive"].ToString();

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            errorString = "Error in visitTime. Could not get school schedule times.";
            return errorString;
        }

        return VolArrivalTime;
    }

    //Get student arrival time from visit time
    public object GetArrivalTime(string VisitTime)
    {
        string errorString;
        var VolArrivalTime = default(string);

        // Populate visit time DDL
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT CONVERT(VARCHAR(5), stuArrive, 108) as stuArrive FROM schoolScheduleFP WHERE schoolSchedule = '" + VisitTime + "'";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                VolArrivalTime = dr["stuArrive"].ToString();

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            errorString = "Error in visitTime. Could not get school schedule times.";
            return errorString;
        }

        return VolArrivalTime;
    }

    // Get volunteer dismissal time from visit time
    public object GetDismissalTime(string VisitTime)
    {
        string errorString;
        var DismissalTime = default(string);

        // Populate visit time DDL
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT CONVERT(VARCHAR(5), leave, 108) as leave FROM schoolScheduleFP WHERE schoolSchedule = '" + VisitTime + "'";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                DismissalTime = dr["leave"].ToString();

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            errorString = "Error in visitTime. Could not get school schedule times.";
            return errorString;
        }

        return DismissalTime;
    }
}