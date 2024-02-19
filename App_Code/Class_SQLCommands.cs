using System;
using System.Activities.Expressions;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens;

public partial class Class_SQLCommands
{
    private string sqlserver = System.Configuration.ConfigurationManager.AppSettings["Fp_sfp"].ToString();
    private string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"].ToString();
    private string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"].ToString();
    private string connection_string;
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;

    public Class_SQLCommands()
    {
        connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    public object InsertIntoKitInventory(string kitNumber, string schoolName, string category, string dateOut, string notes)
    {
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
        string errorReturn;
        string successReturn = "Submission successful!";
        string sqlStatement = @"INSERT INTO kitInventory (
													 kitNumber
													,schoolName
													,category
													,dateOut
													,notes)
												VALUES(@kitNumber
													,@schoolName
													,@category
													,@dateOut
													,@notes);";
        // Check for blank sections
        if (string.IsNullOrEmpty(kitNumber) | string.IsNullOrEmpty(kitNumber))
        {
            errorReturn = "Please enter a kit number before submitting.";
            return errorReturn;
        }

        if (string.IsNullOrEmpty(schoolName) | string.IsNullOrEmpty(schoolName))
        {
            errorReturn = "Please select a school before submitting.";
            return errorReturn;
        }

        if (string.IsNullOrEmpty(category) | string.IsNullOrEmpty(category))
        {
            errorReturn = "Please enter a category before submitting.";
            return errorReturn;
        }

        if (string.IsNullOrEmpty(dateOut) | string.IsNullOrEmpty(dateOut))
        {
            errorReturn = "Please enter a date out before submitting.";
            return errorReturn;
        }

        // If gsiStaff = Nothing Or gsiStaff = "" Then
        // errorReturn = "Please select a GSI Staff member before submitting."
        // Return errorReturn
        // End If

        // Start INSERT command
        try
        {
            using (var con = new SqlConnection(connection_string))
            {
                using (var cmd = new SqlCommand(sqlStatement))
                {
                    cmd.Parameters.Add("@kitNumber", SqlDbType.VarChar).Value = kitNumber;
                    cmd.Parameters.Add("@schoolName", SqlDbType.VarChar).Value = schoolName;
                    cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = category;
                    // cmd.Parameters.Add("@teacherFirstName", SqlDbType.VarChar).Value = teacherFirstName
                    // cmd.Parameters.Add("@teacherLastName", SqlDbType.VarChar).Value = teacherLastName
                    cmd.Parameters.Add("@dateOut", SqlDbType.Date).Value = dateOut;
                    // cmd.Parameters.Add("@gsiStaff", SqlDbType.VarChar).Value = gsiStaff
                    cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = notes;
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        catch (Exception ex)
        {
            errorReturn = "Error in InsertIntoKitInventory. Could not submit new line into kits.";
            return errorReturn;
        }

        return successReturn;
    }

    public object LoadKitInventory(string searchTerm = "", string searchBy = "id", string columnSort = "id", string orderSort = "ASC")
    {
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
        var con = new SqlConnection();
        var cmd = new SqlCommand();
        string errorReturn = "";
        string sqlStatement = @"SELECT id, kitNumber, schoolName, category, FORMAT(dateIn, 'MM/dd/yyyy') as dateIn, FORMAT(dateOut, 'MM/dd/yyyy') as dateOut, gsiStaff, notes 
										FROM kitInventory";
        string sqlSearchStatement = " WHERE " + searchBy + " LIKE '%" + searchTerm + "%'";
        string sqlSortStatement = " ORDER BY " + columnSort + " " + orderSort + "";

        if (string.IsNullOrEmpty(searchTerm) & searchBy == "id")
        {
            sqlStatement = sqlStatement + sqlSortStatement;
        }
        else
        {
            sqlStatement = sqlStatement + sqlSearchStatement + sqlSortStatement;
        }

        // Search and load kit inv table
        con.ConnectionString = connection_string;
        con.Open();
        cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = sqlStatement;

        var da = new SqlDataAdapter();
        da.SelectCommand = cmd;
        var dt = new DataTable();
        da.Fill(dt);

        return dt;

        da.Dispose();
        cmd.Dispose();
        con.Close();

    }

    public object LoadSchoolNotes(string schoolName, string columnSort = "id", string orderSort = "DESC")
    {
        string connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
        var con = new SqlConnection();
        var cmd = new SqlCommand();
        string errorReturn = "";
        string sqlStatement = @"SELECT id, schoolName, note, noteUser, noteTimestamp
										FROM schoolNotes WHERE schoolName = '" + schoolName + "'";
        string sqlSortStatement = " ORDER BY " + columnSort + " " + orderSort + "";

        sqlStatement += sqlSortStatement;

        // Search and load kit inv table
        con.ConnectionString = connection_string;
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

    public object GetUserJob(string Username)
    {
        string returnJob = "";

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT job FROM adminInfo WHERE username='" + Username + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            returnJob = dr["job"].ToString();

        cmd.Dispose();
        con.Close();

        return returnJob;

    }

    public void UpdateRow(int ID, string Field, string Value, string Table)
    {
        string SQLStatement = "UPDATE " + Table + " SET " + Field + "=@" + Field + " WHERE ID=@Id";  
        
        using (SqlConnection con = new SqlConnection(connection_string))
        {
            using (SqlCommand cmd = new SqlCommand(SQLStatement))
            {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@" + Field, Value);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
            }
        }
    }

    public void DeleteRow(int ID, string Table)
    {
        using (SqlConnection con = new SqlConnection(connection_string))
        {
            using (SqlCommand cmd = new SqlCommand("DELETE FROM " + Table + " WHERE id=@ID"))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

}