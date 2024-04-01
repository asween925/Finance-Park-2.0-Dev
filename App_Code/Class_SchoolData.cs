using System;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Class_SchoolData
{
    private Class_VisitData VisitID = new Class_VisitData();
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"];
    string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"];
    string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"];
    string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"];
    private string connection_string;
    private string volRange;
    private string vMin;
    private string vMax;
    private string errorStr;

    public Class_SchoolData()
    {
        connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    // Populates a DDL with schools scheduled to come on an entered visit date
    public object LoadVisitDateSchoolsDDL(string visitDate, DropDownList schoolNameDDL)
    {
        string errorString;
        // Dim schoolNameDDL As DropDownList

        // Check if visit date has a date
        if (!string.IsNullOrEmpty(visitDate))
        {

            // Clear out teacher and school DDLs
            schoolNameDDL.Items.Clear();

            // Populate school DDL from entered visit date
            try
            {
                con.ConnectionString = connection_string;
                con.Open();
                cmd.CommandText = @"SELECT s.schoolName as 'schoolName'
											  FROM schoolInfoFP s 
											  JOIN visitInfoFP v ON v.school = s.id OR v.school2 = s.id OR v.school3 = s.id OR v.school4 = s.id OR v.school5 = s.id
											  WHERE v.visitDate='" + visitDate + @"' AND NOT s.id=505 
											  ORDER BY schoolName";
                cmd.Connection = con;
                dr = cmd.ExecuteReader();

                while (dr.Read())
                    schoolNameDDL.Items.Add(dr[0].ToString());

                schoolNameDDL.Items.Insert(0, "");

                cmd.Dispose();
                con.Close();
            }
            catch
            {
                errorString = "Error in visitDate. Could not get school names.";
                return errorString;
            }

        }

        return schoolNameDDL.Items;

    }

    // Populates a DDL with all schools in the DB
    public object LoadSchoolsDDL(DropDownList schoolNameDDL)
    {
        string errorString;
        // Dim schoolNameDDL As DropDownList

        // Clear out teacher and school DDLs
        schoolNameDDL.Items.Clear();

        // Populate school DDL from entered visit date
        try
        {
            con.ConnectionString = connection_string;
            con.Open();
            cmd.CommandText = "SELECT schoolname FROM schoolInfoFP  WHERE NOT schoolName = 'A1 No School Scheduled' AND NOT id='505' ORDER BY schoolName";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                schoolNameDDL.Items.Add(dr[0].ToString());

            schoolNameDDL.Items.Insert(0, "");

            cmd.Dispose();
            con.Close();
        }
        catch
        {
            errorString = "Error in visitDate. Could not get school names.";
            return errorString;
        }

        return schoolNameDDL.Items;
    }

    // Gets data from a passed through column name and school name.
    public object LoadSchoolInfoFromSchool(string schoolName, string column)
    {
        string returnData = "";

        // Get school info from school name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT " + column + " FROM schoolInfoFP WHERE schoolName = '" + schoolName + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            returnData = dr[column].ToString();
            cmd.Dispose();
            con.Close();
            return returnData;
        }

        cmd.Dispose();
        con.Close();

        return returnData;

    }

    // Gets the ID of a school name
    public object GetSchoolID(string schoolName)
    {
        string returnSchoolID = "";

        // Get school info from school name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT ID FROM schoolInfoFP WHERE schoolName = '" + schoolName + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            returnSchoolID = dr["ID"].ToString();
            cmd.Dispose();
            con.Close();
            return returnSchoolID;
        }

        cmd.Dispose();
        con.Close();

        return returnSchoolID;
    }

    // Populates a DDL with schools scheduled to come on a entered visit date
    public object LoadVisitingSchoolsDDL(string visitDate, DropDownList DDL)
    {
        string sqlStatement = @"SELECT s.schoolName as 'schoolName'
											  FROM schoolInfoFP s 
											  JOIN visitInfoFP v ON v.school = s.id OR v.school2 = s.id OR v.school3 = s.id OR v.school4 = s.id OR v.school5 = s.id
											  WHERE v.visitDate='" + visitDate + @"' AND NOT s.id=505 
											  ORDER BY schoolName";

        // Clear out business DDL
        DDL.Items.Clear();

        // Populate DDL
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = sqlStatement;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            DDL.Items.Add(dr["schoolName"].ToString());

        DDL.Items.Insert(0, "");

        cmd.Dispose();
        con.Close();

        return DDL.Items;

    }

    // Returns a list of all schools from a visit ID with commas seperating them and trims the end comma
    public object GetSchoolsString(string VisitDate)
    {
        string ReturnSchools = "";
        string School1 = "";
        string School2 = "";
        string School3 = "";
        string School4 = "";
        string School5 = "";
        string SQLStatement = @"SELECT s.schoolName as 'School #1', s2.schoolName as 'School #2', s3.schoolName as 'School #3'
											, s4.schoolName as 'School #4', s5.schoolName as 'School #5'
                                            FROM visitInfoFP v 
                                            LEFT JOIN schoolInfoFP s ON s.ID = v.school
                                            LEFT JOIN schoolInfoFP s2 ON s2.ID = v.school2
                                            LEFT JOIN schoolInfoFP s3 ON s3.ID = v.school3
                                            LEFT JOIN schoolInfoFP s4 ON s4.ID = v.school4
                                            LEFT JOIN schoolInfoFP s5 ON s5.ID = v.school5
											 WHERE v.visitDate='" + VisitDate + "' AND NOT v.school=1 ORDER BY v.visitDate DESC";

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = SQLStatement;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            School1 = dr[0].ToString();
            School2 = dr[1].ToString();
            School3 = dr[2].ToString();
            School4 = dr[3].ToString();
            School5 = dr[4].ToString();
        }

        if (School2 != " ")
        {
            School1 += ", ";
        }

        if (School3 != " ")
        {
            School2 += ", ";
        }

        if (School4 != " ")
        {
            School3 += ", ";
        }

        if (School5 != " ")
        {
            School4 += ", ";
        }

        cmd.Dispose();
        con.Close();

        ReturnSchools = School1 + School2 + School3 + School4 + School5;
        return ReturnSchools;

    }

    // Returns a list of sharing schools
    public object GetSharedSchoolsString(string VisitDate, string SchoolName)
    {
        string ReturnSchools;
        string School1 = "";
        string School2 = "";
        string School3 = "";
        string School4 = "";
        string School5 = "";
        string SQLStatement = @"SELECT s.schoolName as 'School #1', s2.schoolName as 'School #2', s3.schoolName as 'School #3'
											, s4.schoolName as 'School #4', s5.schoolName as 'School #5'
                                            FROM visitInfoFP v 
                                            LEFT JOIN schoolInfoFP s ON s.ID = v.school
                                            LEFT JOIN schoolInfoFP s2 ON s2.ID = v.school2
                                            LEFT JOIN schoolInfoFP s3 ON s3.ID = v.school3
                                            LEFT JOIN schoolInfoFP s4 ON s4.ID = v.school4
                                            LEFT JOIN schoolInfoFP s5 ON s5.ID = v.school5
											 WHERE v.visitDate='" + VisitDate + "' AND NOT v.school=1 ORDER BY v.visitDate DESC";

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = SQLStatement;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            School1 = dr[0].ToString();
            School2 = dr[1].ToString();
            School3 = dr[2].ToString();
            School4 = dr[3].ToString();
            School5 = dr[4].ToString();
        }

        if ((School1 ?? "") == (SchoolName ?? ""))
        {
            School1 = "";
        }
        else if ((School2 ?? "") == (SchoolName ?? ""))
        {
            School2 = "";
        }
        else if ((School3 ?? "") == (SchoolName ?? ""))
        {
            School3 = "";
        }
        else if ((School4 ?? "") == (SchoolName ?? ""))
        {
            School4 = "";
        }
        else if ((School5 ?? "") == (SchoolName ?? ""))
        {
            School5 = "";
        }

        if (!string.IsNullOrEmpty(School2) & !string.IsNullOrEmpty(School1))
        {
            School1 += ", ";
        }
        else if (School3 != " ")
        {
            School2 += ", ";
        }
        else if (School4 != " ")
        {
            School3 += ", ";
        }
        else if (School5 != " ")
        {
            School4 += ", ";
        }

        cmd.Dispose();
        con.Close();

        ReturnSchools = School1 + School2 + School3 + School4 + School5;

        return ReturnSchools;

    }

    // Returns all school names from a visit date
    public (string School1, string School2, string School3, string School4, string School5) GetSchoolsIndividual(string VisitDate)
    {
        string S1 = "";
        string S2 = "";
        string S3 = "";
        string S4 = "";
        string S5 = "";
        string SQLStatement = @"SELECT s.schoolName as 'School #1', s2.schoolName as 'School #2', s3.schoolName as 'School #3'
											, s4.schoolName as 'School #4', s5.schoolName as 'School #5'
                                            FROM visitInfoFP v 
                                            LEFT JOIN schoolInfoFP s ON s.ID = v.school
                                            LEFT JOIN schoolInfoFP s2 ON s2.ID = v.school2
                                            LEFT JOIN schoolInfoFP s3 ON s3.ID = v.school3
                                            LEFT JOIN schoolInfoFP s4 ON s4.ID = v.school4
                                            LEFT JOIN schoolInfoFP s5 ON s5.ID = v.school5
											 WHERE v.visitDate='" + VisitDate + "' AND NOT v.school=1 ORDER BY v.visitDate DESC";

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = SQLStatement;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            S1 = dr[0].ToString();
            S2 = dr[1].ToString();
            S3 = dr[2].ToString();
            S4 = dr[3].ToString();
            S5 = dr[4].ToString();
        }

        cmd.Dispose();
        con.Close();

        return (S1, S2, S3, S4, S5);
    }

    // Returns the minimum volunteer count and maximum volunteer count of a school
    public (string VMin, string VMax) GetVolunteerRange(string VisitDate, string SchoolID = null)
    {
        string Min = "0";
        string Max = "0";
        string SQLStatment = "SELECT SUM(o.businessVMinCount) as vMin, SUM(o.businessVMaxCount) as vMax FROM onlineBanking o";

        if (!string.IsNullOrEmpty(SchoolID))
        {
            SQLStatment += " WHERE o.visitDate='" + VisitDate + "' AND o.school='" + SchoolID + "' AND o.openstatus=1";
        }
        else
        {
            SQLStatment += " WHERE o.visitDate='" + VisitDate + "' AND o.openstatus=1";
        }

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = SQLStatment;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Min = dr["vMin"].ToString();
            Max = dr["vMax"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return (Min, Max);
    }

    // Gets the school name from a school ID
    public object GetSchoolNameFromID(string SchoolID)
    {
        string returnSchoolName = "";

        // Get school info from school name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT schoolName FROM schoolInfoFP WHERE id = '" + SchoolID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            returnSchoolName = dr["schoolName"].ToString();
            cmd.Dispose();
            con.Close();
            return returnSchoolName;
        }

        cmd.Dispose();
        con.Close();

        return returnSchoolName;
    }

    // Moves current visit date to previous visit date
    public void UpdatePreviousVisitDate(string SchoolID)
    {

        using (var con = new SqlConnection(connection_string))
        {
            using (var cmd = new SqlCommand("UPDATE schoolInfoFP SET previousVisitDate=currentVisitDate WHERE id=@school"))
            {
                cmd.Parameters.AddWithValue("@school", SchoolID);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

    }

    // Updates current visit date in school Info
    public void UpdateCurrentVisitDate(string SchoolID, string VisitDate)
    {
        if (SchoolID != "0" || SchoolID != "0")
        {
            using (var con = new SqlConnection(connection_string))
            {
                using (var cmd = new SqlCommand("UPDATE schoolInfoFP SET currentVisitDate=@VisitDate WHERE id=@school"))
                {
                    cmd.Parameters.AddWithValue("@school", SchoolID);
                    cmd.Parameters.AddWithValue("@VisitDate", VisitDate);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
        

    }

    //Gets the current visit date of a school ID and formats it to M/DD/YYYY
    public object GetCurrentVisitDate(string SchoolID)
    {
        string returnData = "";

        // Get school info from school name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT FORMAT(currentVisitDate, 'M/dd/yyyy') as currentVisitDate FROM schoolInfoFP WHERE id = '" + SchoolID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            returnData = dr["currentVisitDate"].ToString();
            cmd.Dispose();
            con.Close();
            return returnData;
        }

        cmd.Dispose();
        con.Close();

        return returnData;
    }

}