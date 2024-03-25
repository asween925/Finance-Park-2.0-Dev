using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public class Class_SVC
{
    private string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"].ToString();
    private string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"].ToString();
    private SqlDataReader dr;
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private string connection_string;

    public object LoadTable(int VisitID, int SchoolID, string Column)
    {
        string ReturnData = "";
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";

        // Get school info from school name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT " + Column + " FROM schoolVisitChecklistFP WHERE visitID = '" + VisitID + "' AND schoolID = '" + SchoolID + "'";
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

    public object GetKitNumbers(int VisitID, int SchoolID)
    {
        string ReturnData = "";
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";

        // Get school info from school name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT kitTotal FROM kitsFP WHERE visitID = '" + VisitID + "' AND schoolID = '" + SchoolID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            ReturnData = dr["kitTotal"].ToString();
            cmd.Dispose();
            con.Close();
            return ReturnData;
        }

        cmd.Dispose();
        con.Close();

        return ReturnData;
    }

    public object GetWorkbooks(int VisitID, int SchoolID)
    {
        string ReturnData = "";
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";

        // Get school info from school name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT workbooks FROM kitsFP WHERE visitID = '" + VisitID + "' AND schoolID = '" + SchoolID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            ReturnData = dr["workbooks"].ToString();
            cmd.Dispose();
            con.Close();
            return ReturnData;
        }

        cmd.Dispose();
        con.Close();

        return ReturnData;
    }

    public object GetKitNumber(int VisitID, int SchoolID, int KitNum)
    {
        string ReturnData = "";
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";

        // Get school info from school name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT kit" + KitNum + " as kit FROM kitsFP WHERE visitID = '" + VisitID + "' AND schoolID = '" + SchoolID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            ReturnData = dr["kit"].ToString();
            cmd.Dispose();
            con.Close();
            return ReturnData;
        }

        cmd.Dispose();
        con.Close();

        return ReturnData;
    }

}