using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

public partial class Class_StudentData
{
    private Class_VisitData VisitID = new Class_VisitData();
    private int Visit;
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string sqlserver = System.Configuration.ConfigurationManager.AppSettings["EV_sfp"].ToString();
    private string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["EV_DB"].ToString();
    private string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"].ToString();
    private string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"].ToString();
    private string connection_string;
    private string studentCount;
    private string errorStr;

    public Class_StudentData()
    {
        Visit = VisitID.GetVisitID();
        connection_string = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    // Gets the number of students in EV 2.0 from a passed through visit date (this is the correct number of students, not the student count number that the staff enters when the visit is created)
    public object GetStudentCount(string visitDate)
    {
        string studentCountSQL = @"SELECT COUNT(lastName) as studentCount FROM (SELECT s.id, s.employeeNumber, s.firstName, s.lastName, j.jobTitle, b.businessName, sc.schoolName
                                FROM studentInfo s
                                INNER JOIN jobs j ON j.id=s.job
                                INNER JOIN businessInfo b ON b.id=s.business
                                INNER JOIN visitInfoFP v ON v.id=s.visit
                                INNER JOIN schoolInfo sc ON s.school = sc.id
                                WHERE v.visitDate='" + visitDate + "' AND NOT businessName='Training Business' AND NOT firstName='NULL' AND NOT lastName='NULL' AND NOT firstName = ' ' AND NOT lastName=' ' ) t";

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = studentCountSQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            studentCount = dr["studentCount"].ToString();

        cmd.Dispose();
        con.Close();

        return studentCount;
    }


    // Gets the number of students of ONE school in EV 2.0 from a passed through visit date and school name (this is the correct number of students, not the student count number that the staff enters when the visit is created)
    public object GetStudentCountOfSchool(string VisitDate, string SchoolName)
    {
        string studentCountSQL = @"SELECT COUNT(lastName) as studentCount FROM (SELECT s.id, s.employeeNumber, s.firstName, s.lastName, j.jobTitle, b.businessName, sc.schoolName
                                FROM studentInfo s
                                INNER JOIN jobs j ON j.id=s.job
                                INNER JOIN businessInfo b ON b.id=s.business
                                INNER JOIN visitInfoFP v ON v.id=s.visit
                                INNER JOIN schoolInfo sc ON s.school = sc.id
                                WHERE v.visitDate='" + VisitDate + "' AND sc.schoolName = '" + SchoolName + "' AND NOT businessName='Training Business' AND NOT firstName='NULL' AND NOT lastName='NULL' AND NOT firstName = ' ' AND NOT lastName=' ' ) t";

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = studentCountSQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            studentCount = dr["studentCount"].ToString();

        cmd.Dispose();
        con.Close();

        return studentCount;
    }

    public object GetStudentCountOfBusiness(string VisitDate, string BusinessName)
    {
        string studentCountSQL = @"SELECT COUNT(lastName) as studentCount FROM (SELECT s.id, s.employeeNumber, s.firstName, s.lastName, j.jobTitle, b.businessName, sc.schoolName
                                FROM studentInfo s
                                INNER JOIN jobs j ON j.id=s.job
                                INNER JOIN businessInfo b ON b.id=s.business
                                INNER JOIN visitInfoFP v ON v.id=s.visit
                                INNER JOIN schoolInfo sc ON s.school = sc.id
                                WHERE v.visitDate='" + VisitDate + "' AND b.businessName = '" + BusinessName + "' AND NOT businessName='Training Business' AND NOT firstName='NULL' AND NOT lastName='NULL' AND NOT firstName = ' ' AND NOT lastName=' ' ) t";

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = studentCountSQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            studentCount = dr["studentCount"].ToString();

        cmd.Dispose();
        con.Close();

        return studentCount;
    }


    // Gets the manually entered student count (from step 1, the teachers only section) from the school visit checklist
    public object GetSVCStudentCount(string VisitDate, string SchoolName)
    {
        string studentCountSQL = "SELECT schoolStudentCount FROM schoolVisitChecklist WHERE visitDate='" + VisitDate + "' AND schoolName = '" + SchoolName + "'";

        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = studentCountSQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            studentCount = dr["schoolStudentCount"].ToString();

        cmd.Dispose();
        con.Close();

        return studentCount;
    }


    // Populates a DDL with the account number and name of a student in a passed through visit ID
    public object LoadStudentNameWithNumDDL(DropDownList studentName_ddl, string visitID)
    {

        // Populates a DDL with student names and their account numbers at the beginning of the name
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = "SELECT CONCAT(employeeNumber, '.     ', firstName, ' ', lastName) as 'Account # and Name' FROM studentInfo WHERE visit='" + visitID + "'  AND NOT lastName IS NULL";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
            studentName_ddl.Items.Add(dr[0].ToString());
        studentName_ddl.Items.Insert(0, "");

        cmd.Dispose();
        con.Close();

        return studentName_ddl.Items;
    }


    // Loads a table with total transactions and balance but only for students with a negative balance
    public object LoadTransactionsWithNegativeBalanceTable(string VisitID)
    {
        con.ConnectionString = connection_string;
        con.Open();
        cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = @"IF (OBJECT_ID('tempdb..#netdeposits') IS NOT NULL) DROP TABLE #netdeposits
       -- Total Deposits
       SELECT 
              s.employeeNumber
              ,s.firstName
              ,s.lastName
              ,SUM(ISNULL(s.netdeposit1,0) + ISNULL(s.netdeposit2,0) + ISNULL(s.netdeposit3,0) + ISNULL(s.netdeposit4,0) - ISNULL(s.savings,0)) totalDeposits
       INTO #netdeposits
       FROM dbo.studentInfo s
       WHERE s.visit = '" + VisitID + @"'
       GROUP BY s.employeeNumber, s.firstName, s.lastName

       -- Total Purchases and with JOIN to #netdeposits temp table
       SELECT 
               t.employeeNumber, CONCAT (MAX(firstname), ' ',MAX(lastName)) as studentname
              ,MAX(s.totalDeposits) TotalDeposits
              ,SUM(ISNULL(saleamount,0)) as TotalPurchases
              ,MAX(s.totalDeposits) - sum(ISNULL(saleamount,0)) as Balance
       FROM transactions t
       INNER JOIN #netdeposits s ON t.employeeNumber = s.employeeNumber
       WHERE t.visitdate = '" + VisitID + @"'
       GROUP BY t.employeeNumber
       HAVING MAX(s.totalDeposits) - sum(ISNULL(saleamount,0)) < 0
       ORDER BY Balance";


        var da = new SqlDataAdapter();
        da.SelectCommand = cmd;
        var dt = new DataTable();
        da.Fill(dt);

        cmd.Dispose();
        con.Close();

        return dt;
    }


    // Gets students info of various columns
    public (string AccountNumber, string StudentID, string FirstName, string LastName, string VisitID, string SchoolName, string VisitDate, string BusinessName, string JobTitle, string Salary) StudentLookup(string VID, string AccNum)
    {
        string A = "";
        string S = "";
        string F = "";
        string L = "";
        string V = "";
        string Sc = "";
        string Vi = "";
        string B = "";
        string J = "";
        string Sa = "";
        string SQLStatement = @"SELECT s.employeeNumber, s.id as studentID, s.firstName, s.lastName, v.id as visitID, sc.schoolName as schoolName, v.VisitDate,
                    b.businessName, j.jobTitle, sa.tierSalary
                    FROM studentInfo s
                    INNER JOIN visitInfoFP v
	                    ON v.id = s.visit
                    FULL JOIN schoolinfo sc
	                    ON sc.ID = s.school
                    FULL JOIN teacherinfo t
	                    ON t.Id = s.teacher
                    INNER JOIN businessInfo b
	                    ON b.ID = s.business
                    INNER JOIN jobs j
	                    ON j.ID = s.job
                    INNER JOIN salary sa
	                    ON sa.payTier = j.jobSalary
                    WHERE s.employeeNumber ='" + AccNum + "' AND v.id = '" + VID + "'";

        cmd.Connection = con;
        con.ConnectionString = connection_string;
        con.Open();
        cmd.CommandText = SQLStatement;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            A = dr["employeeNumber"].ToString();
            S = dr["studentID"].ToString();
            F = dr["firstName"].ToString();
            L = dr["lastName"].ToString();
            V = dr["visitID"].ToString();
            Sc = dr["schoolName"].ToString();
            Vi = dr["visitDate"].ToString();
            B = dr["businessName"].ToString();
            J = dr["jobTitle"].ToString();
            Sa = dr["tierSalary"].ToString();
        }

        dr.Close();
        cmd.Dispose();
        con.Close();

        return (A, S, F, L, V, Sc, Vi, B, J, Sa);
    }


    // Loads a gridview with deposits and cash back (for magic computer)
    public object LoadDepositsTable(string VisitID, string AccountNumber)
    {
        con.ConnectionString = connection_string;
        con.Open();
        cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = @"SELECT id, cbw1, cbw2, cbw3, cbw4, initialDeposit1, initialDeposit2, initialDeposit3, initialDeposit4 
                              FROM studentInfo 
                              WHERE visit='" + VisitID + "' AND employeeNumber ='" + AccountNumber + "'";

        var da = new SqlDataAdapter();
        da.SelectCommand = cmd;
        var dt = new DataTable();
        da.Fill(dt);

        cmd.Dispose();
        con.Close();

        return dt;
    }


}