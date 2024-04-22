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
    private string ConnectionString;

    public Class_SVC()
    {
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    public object LoadTable(int VisitID, int SchoolID, string Column)
    {
        string ReturnData = "";

        // Get school info from school name
        con.ConnectionString = ConnectionString;
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
 

        // Get school info from school name
        con.ConnectionString = ConnectionString;
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

    public int GetWorkbooks(int VisitID, int SchoolID)
    {
        int Workbooks = 0;

        // Get school info from school name
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT workbooks FROM kitsFP WHERE visitID = '" + VisitID + "' AND schoolID = '" + SchoolID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Workbooks = int.Parse(dr["workbooks"].ToString());
            cmd.Dispose();
            con.Close();
            return Workbooks;
        }

        cmd.Dispose();
        con.Close();

        return Workbooks;
    }

    public object GetKitNumber(int VisitID, int SchoolID, int KitNum)
    {
        string ReturnData = "";

        // Get school info from school name
        con.ConnectionString = ConnectionString;
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

    public string GetKitNumbersString(int VisitID, int SchoolID)
    {
        string Kits = "0";
        string SQL = @"SELECT CONCAT(
		                IIF(kit1<>0, kit1, '')
		                , IIF(kit2<>0, CONCAT(', ', kit2), '')
		                , IIF(kit3<>0, CONCAT(', ', kit3), '')
		                , IIF(kit4<>0, CONCAT(', ', kit4), '')
		                , IIF(kit5<>0, CONCAT(', ', kit5), '')
		                , IIF(kit6<>0, CONCAT(', ', kit6), '')
		                , IIF(kit7<>0, CONCAT(', ', kit7), '')
		                , IIF(kit8<>0, CONCAT(', ', kit8), '')
		                , IIF(kit9<>0, CONCAT(', ', kit9), '')
		                , IIF(kit10<>0, CONCAT(', ', kit10), '')) as kits 
                      FROM kitsFP 
                      WHERE visitID='" + VisitID + "' AND schoolID='" + SchoolID + "'";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = SQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Kits = dr["kits"].ToString();
            return Kits;
        }

        cmd.Dispose();
        con.Close();

        return Kits;
    }
}