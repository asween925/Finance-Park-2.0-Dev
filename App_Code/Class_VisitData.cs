using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Class_VisitData
{
    private string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"].ToString();
    private string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"].ToString();    
    private SqlDataReader dr;
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private string connection_string;

    public int GetVisitID()
    {
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
        SqlDataReader dr;
        string dateSQL = "SELECT ID FROM visitInfoFP WHERE visitDate = '" + DateTime.Now.ToShortDateString() + "'";
        int returnValue = 0;
        var con = new SqlConnection();
        var cmd = new SqlCommand();

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = dateSQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();


        if (dr.HasRows)
        {
            while (dr.Read())
            {
                returnValue = Convert.ToInt16(dr["ID"]);
            }
                
        }
        else
        {
            // No visit on current date
            returnValue = 0;

        }
        cmd.Dispose();
        con.Close();


        return returnValue;
    }

    public object GetVisitIDFromDate(string visitDate)
    {
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
        SqlDataReader dr;
        string dateSQL = "SELECT ID FROM visitInfoFP WHERE visitDate = '" + visitDate + "'";
        int returnValue = 0;
        var con = new SqlConnection();
        var cmd = new SqlCommand();

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = dateSQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            while (dr.Read())
                returnValue = Convert.ToInt16(dr["ID"]);
        }
        else
        {
            // No visit on current date
            returnValue = 0;

        }
        cmd.Dispose();
        con.Close();


        return returnValue;
    }

    public object GetVisitDateFromID(string VisitID)
    {
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
        string dateSQL = "SELECT visitDate FROM visitInfoFP WHERE id = '" + VisitID + "'";
        var returnValue = default(string);

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = dateSQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            while (dr.Read())
                returnValue = dr["visitDate"].ToString();
        }
        else
        {
            // No visit on current date
            returnValue = "";

        }
        cmd.Dispose();
        con.Close();


        return returnValue;
    }

    public object LoadVisitInfoFromDate(string VisitDate, string Column)
    {
        string ReturnData = "";
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";

        // Get school info from school name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT " + Column + " FROM visitInfoFP WHERE visitDate = '" + VisitDate + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            ReturnData = dr[Column].ToString();
            cmd.Dispose();
            con.Close();
            return ReturnData;
        }

        cmd.Dispose();
        con.Close();

        return ReturnData;
    }

    public object LoadVisitInfoTable(string SQLWhereVisitDate = null, string SQLWhereSchool = null, string SQLWhereMonth = null, string SQLWhereNot = null)
    {
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
        string SQLStatement = @"SELECT v.id, IIF(s.id IS NULL, '', CONCAT(s.schoolName, ' (', s.id, ')')) as 'School #1',
		                                    IIF(s2.id IS NULL, '', CONCAT(s2.schoolName, ' (', s2.id, ')')) as 'School #2', 
                                             IIF(s3.id IS NULL, '', CONCAT(s3.schoolName, ' (', s3.id, ')')) as 'School #3', 
                                             IIF(s4.id IS NULL, '', CONCAT(s4.schoolName, ' (', s4.id, ')')) as 'School #4', 
                                             IIF(s5.id IS NULL, '', CONCAT(s5.schoolName, ' (', s5.id, ')')) as 'School #5',
                                            v.vTrainingTime, v.vMinCount, v.vMaxCount, v.visitDate, v.studentCount, v.visitTime
                                            FROM visitInfoFP v 
                                            LEFT JOIN schoolInfoFP s ON s.ID = v.school
                                            LEFT JOIN schoolInfoFP s2 ON s2.ID = v.school2
                                            LEFT JOIN schoolInfoFP s3 ON s3.ID = v.school3
                                            LEFT JOIN schoolInfoFP s4 ON s4.ID = v.school4
                                            LEFT JOIN schoolInfoFP s5 ON s5.ID = v.school5";

        if (!string.IsNullOrEmpty(SQLWhereVisitDate))
        {
            SQLStatement += SQLWhereVisitDate;
        }

        if (!string.IsNullOrEmpty(SQLWhereSchool))
        {
            SQLStatement += SQLWhereSchool;
        }

        if (!string.IsNullOrEmpty(SQLWhereMonth))
        {
            SQLStatement += SQLWhereMonth;
        }

        if (string.IsNullOrEmpty(SQLWhereSchool) & string.IsNullOrEmpty(SQLWhereVisitDate))
        {
            SQLStatement += SQLWhereNot;
        }

        con.ConnectionString = connection_string;
        con.Open();
        cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = SQLStatement;

        var da = new SqlDataAdapter();
        da.SelectCommand = cmd;
        var dt = new DataTable();
        da.Fill(dt);

        return dt;
    }

    public object LoadVisitDatesFromSchool(string SchoolID, DropDownList VisitDateDDL)
    {
        string ReturnData = "";

        // Get school info from school name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT FORMAT (visitDate, 'MM/dd/yyyy') FROM visitInfoFP WHERE school = '" + SchoolID + "' Or school2 = '" + SchoolID + "' Or school3 = '" + SchoolID + "' Or school4 = '" + SchoolID + "' Or school5 = '" + SchoolID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            while (dr.Read())
                VisitDateDDL.Items.Add(dr[0].ToString());

            VisitDateDDL.Items.Insert(0, "");
        }

        cmd.Dispose();
        con.Close();

        return ReturnData;
    }

}