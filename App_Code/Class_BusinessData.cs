using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Class_BusinessData
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

    public Class_BusinessData()
    {
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    public object GetBusinessID(string businessName)
    {
        string returnBusinessID = "";

        // Get school info from school name
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT ID FROM businessInfoFP WHERE businessName = '" + businessName + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            returnBusinessID = dr["ID"].ToString();
            cmd.Dispose();
            con.Close();
            return returnBusinessID;
        }

        cmd.Dispose();
        con.Close();

        return returnBusinessID;
    }



    public object LoadBusinessNamesDDL(DropDownList businessNameDDL)
    {
        string errorString;
        // Dim businessNameDDL As DropDownList

        // Clear out teacher and school DDLs
        businessNameDDL.Items.Clear();

        // Populate school DDL from entered visit date
        try
        {
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT businessName FROM businessInfoFP ORDER BY businessName";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                businessNameDDL.Items.Add(dr[0].ToString());
            businessNameDDL.Items.Insert(0, "");

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            errorString = "Error in visitDate. Could not get school names.";
            return errorString;
        }

        return businessNameDDL.Items;
    }



    // Update image and title based on business ID
    public (string ImagePath, string BColor, string BusinessName) GetBusinessLogos(string BusinessID)
    {
        string I = "";
        string C = "";
        string B = "";
        string logoRoot = "~/media/Logos/";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT logoPath, businessColor, businessName FROM businessinfo WHERE ID='" + BusinessID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            I = logoRoot + dr[0].ToString();
            C = dr[1].ToString();
            B = dr[2].ToString();
        }

        cmd.Dispose();
        con.Close();

        return (I, C, B);

    }



    // Get business address
    public object GetBusinessAddress(string BusinessName)
    {
        string returnAddress = "";

        // Get school info from school name
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT address FROM businessInfo WHERE businessName = '" + BusinessName + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            returnAddress = dr["address"].ToString();
            cmd.Dispose();
            con.Close();
            return returnAddress;
        }

        cmd.Dispose();
        con.Close();

        return returnAddress;
    }



    // Get job ID from job title
    public object GetJobID(string JobTitle)
    {
        var ReturnID = default(string);

        // Get job ID from job title
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT id FROM jobs WHERE jobTitle='" + JobTitle + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            ReturnID = dr["id"].ToString();

        cmd.Dispose();
        con.Close();

        return ReturnID;

    }



    // Gets a string of the closed businesses in EV
    public object GetClosedBusinesses(string VisitDate)
    {
        var ClosedBusinesses = default(string);

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = @"Declare @val varchar(MAX)
                               SELECT @val = COALESCE(@val + ', ' + b.businessName, b.businessName)
	                                FROM businessInfo b
                                INNER JOIN onlineBanking o
                                ON b.id = o.businessID
                                WHERE o.openstatus=0 AND o.visitDate='" + VisitDate + @"'
                                ORDER BY b.businessName
	                                SELECT @val as names";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            ClosedBusinesses = dr["names"].ToString();

        cmd.Dispose();
        con.Close();

        return ClosedBusinesses;
    }


    //Get the business name from the ID
    public object GetBusinessNameFromID(int ID)
    {
        string ReturnID = "";

        // Get job ID from job title
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT businessName FROM jobsFP WHERE id='" + ID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            ReturnID = dr["businessName"].ToString();
        }
            
        cmd.Dispose();
        con.Close();

        return ReturnID;
    }



    //Get total number of businesses
    public object GetTotalBusinesses()
    {
        string ReturnCount = "";

        // Get job ID from job title
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT COUNT(businessName) FROM businessInfoFP";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            ReturnCount = dr["businessName"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return ReturnCount;
    }



    //Load sponsor DDL
    public object LoadSponsorDDL(DropDownList ddlSponsor)
    {
        // Clear out teacher and school DDLs
        ddlSponsor.Items.Clear();

        // Populate school DDL from entered visit date
            con.ConnectionString = ConnectionString;
            con.Open();
            cmd.CommandText = "SELECT sponsorName FROM sponsorsFP ORDER BY sponsorName ASC";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                ddlSponsor.Items.Add(dr[0].ToString());
            ddlSponsor.Items.Insert(0, "");

            cmd.Dispose();
            con.Close();

        return ddlSponsor.Items;
    }



    //Get sponsor ID from name
    public int GetSponsorID(string SponsorName)
    {
        int SponsorID = 0;

        // Get job ID from job title
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT id FROM sponsorsFP WHERE sponsorName='" + SponsorName + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            SponsorID = int.Parse(dr["id"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return SponsorID;
    }
}