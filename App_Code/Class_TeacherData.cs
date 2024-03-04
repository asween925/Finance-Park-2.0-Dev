using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using static Antlr.Runtime.Tree.TreeWizard;

public partial class Class_TeacherData
{
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"].ToString();
    private string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"].ToString();
    private string connection_string;

    public Class_TeacherData()
    {
        connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    // Gets the contact teacher name
    public object GetContactTeacher(string schoolID)
    {
        string returnData = "";

        // Get school info from school name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT TRIM(firstName) + ' ' + TRIM(lastName) as teacherName FROM teacherInfoFP WHERE schoolID = '" + schoolID + "' AND contact=1";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            returnData = dr["teacherName"].ToString();
            cmd.Dispose();
            con.Close();
            return returnData;
        }

        cmd.Dispose();
        con.Close();

        return returnData;
    }


    // Gets first and last name of teacher from a school name
    public object LoadTeacherNameDDLFromSchoolName(string SchoolName, DropDownList DDL)
    {
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT t.firstName, t.lastName FROM teacherInfoFP t INNER JOIN schoolInfoFP s ON s.id = t.schoolID WHERE s.schoolName='" + SchoolName + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            DDL.Items.Add(dr[0].ToString() + " " + dr[1].ToString());

        DDL.Items.Insert(0, "");

        cmd.Dispose();
        con.Close();

        return DDL;
    }


    //Gets first and last name of teacher from a school ID
    public object LoadTeacherNameDDLFromSchoolID(string SchoolID, DropDownList DDL)
    {
      
            con.ConnectionString = connection_string;
            con.Open();
            cmd.CommandText = "SELECT firstName, lastName FROM teacherInfoFP WHERE schoolID='" + SchoolID + "'";
            cmd.Connection = con;
            dr = cmd.ExecuteReader();

            while (dr.Read())
                DDL.Items.Add(dr[0].ToString() + " " + dr[1].ToString());

            DDL.Items.Insert(0, "");

            cmd.Dispose();
            con.Close();

            return DDL;
     
    }


    //Gets ID of teacher from first and last name
    public object GetTeacherIDFromName(string First, string Last)
    {
        string ID = "";

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT ID FROM teacherInfoFP WHERE firstName='" + First + "' AND lastName='" + Last + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            ID = dr[0].ToString();

        cmd.Dispose ();
        con.Close();

        return ID;
    }


    //Updates the current visit ID in teacherInfoFP
    public void UpdateCurrentVisit(string TID, string VID)
    {
        string UpdateSQL = "UPDATE teacherInfoFP SET ";
        
        //Check if currVisitID is not null
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT currVisitID FROM teacherInfoFP WHERE id='" + TID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        if (dr.HasRows == false)
        {
            UpdateSQL += "currVisitID='" + VID + "' WHERE id='" + TID + "'";
        }
        else
        {
            UpdateSQL += "prevVisitID=currVisitID, currVisitID='" + VID + "' WHERE id='" + TID + "'";
        }
        cmd.Dispose ( );
        con.Close();

        con.Open ();
        cmd.CommandText = UpdateSQL;
        cmd.Connection = con;
        cmd.ExecuteNonQuery();
        cmd.Dispose();
        con.Close();
    }


    //Loads a DDL with the teachers of a visit ID
    public object LoadTeacherNamesFromVID(int VisitID, int SchoolID, DropDownList DDL)
    {
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT firstName, lastName FROM teacherInfoFP WHERE schoolID='" + SchoolID + "' AND currVisitID='" + VisitID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            DDL.Items.Add(dr[0].ToString() + " " + dr[1].ToString());

        //DDL.Items.Insert(0, "");

        cmd.Dispose();
        con.Close();

        return DDL;
    }


    public object GetTeacherEmail(int TID)
    {
        string email = "";
        
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT email FROM teacherInfoFP WHERE id='" + TID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            email = dr[0].ToString();

        cmd.Dispose();
        con.Close();

        return email;
    }
}