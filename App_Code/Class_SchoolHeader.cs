using System.Data.SqlClient;

public partial class Class_SchoolHeader
{
    private Class_VisitData VisitID = new Class_VisitData();
    private int Visit;
    private string schoolHeader;
    private string schoolHeader2;
    private string schoolHeader3;
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"];
    string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"];
    string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"];
    string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"];
    private string connection_string;
    private string returnSchool1;
    private string returnSchool2;
    private string returnSchool3;
    private string returnSchool = "No School Scheduled";

    public Class_SchoolHeader()
    {
        Visit = VisitID.GetVisitID();
        schoolHeader = "SELECT s.SchoolName FROM schoolinfoFP s INNER JOIN visitInfoFP v on s.ID = v.School WHERE v.id='" + Visit + "'";
        schoolHeader2 = "SELECT s.SchoolName FROM schoolinfoFP s INNER JOIN visitInfoFP v on s.ID = v.School2 WHERE v.id='" + Visit + "'";
        schoolHeader3 = "SELECT s.SchoolName FROM schoolinfoFP s INNER JOIN visitInfoFP v on s.ID = v.School3 WHERE v.id='" + Visit + "'";
        connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    public object GetSchoolHeader()
    {
        // Populating header school and school name label
        try
        {
            con.ConnectionString = connection_string;
            con.Open();
            cmd.CommandText = schoolHeader;
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                returnSchool1 = dr["schoolName"].ToString();

            cmd.Dispose();
            con.Close();
        }
        catch
        {
        }
        finally
        {
            cmd.Dispose();
            con.Close();

        }

        // School 2
        try
        {
            con.ConnectionString = connection_string;
            con.Open();
            cmd.CommandText = schoolHeader2;
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                returnSchool2 = " And " + dr["schoolName"].ToString();

                if ((returnSchool2 ?? "") == " And " + " ")
                {
                    returnSchool2 = "";
                }
            }

            cmd.Dispose();
            con.Close();
        }
        catch
        {
        }
        finally
        {
            cmd.Dispose();
            con.Close();

        }

        // School 3
        try
        {
            con.ConnectionString = connection_string;
            con.Open();
            cmd.CommandText = schoolHeader3;
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                returnSchool3 = " And " + dr["schoolName"].ToString();

                if ((returnSchool3 ?? "") == " And " + " ")
                {
                    returnSchool3 = "";
                }
            }

            cmd.Dispose();
            con.Close();
        }
        catch
        {
        }
        finally
        {
            cmd.Dispose();
            con.Close();

        }

        returnSchool = returnSchool1 + returnSchool2 + returnSchool3;

        if (Visit == 0)
        {
            returnSchool = "No School Scheduled";
        }

        return returnSchool;

    }

}