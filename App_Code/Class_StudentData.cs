using Antlr.Runtime.Tree;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.ActiveDirectory;
using System.IdentityModel.Tokens;
using System.Runtime.Remoting.Lifetime;
using System.Security.Cryptography;
using System.Web.UI.WebControls;
using static Antlr.Runtime.Tree.TreeWizard;

public partial class Class_StudentData
{
    private Class_VisitData VisitData = new Class_VisitData();
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    private string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"].ToString();
    private string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"].ToString();
    private string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"].ToString();
    private string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"].ToString();
    private string ConnectionString;
    private string studentCount;
    private string errorStr;

    public Class_StudentData()
    {
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
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

        con.ConnectionString = ConnectionString;
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

        con.ConnectionString = ConnectionString;
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

        con.ConnectionString = ConnectionString;
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

        con.ConnectionString = ConnectionString;
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
        con.ConnectionString = ConnectionString;
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
        con.ConnectionString = ConnectionString;
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
    public (int AccountNumber, string FirstName, string LastName, int VisitID, string SchoolName, DateTime VisitDate, string TeacherName, string SponsorName, int PersonaID, double NMI, double Spent) StudentLookup(int VisitID, int SID)
    {
        int ANum = 0;
        string First = "";
        string Last = "";
        int VID = 0;
        string School = "";
        DateTime VDate = DateTime.Now;
        string Teacher = "";
        string Sponsor = "";
        int PID = 0;
        double NMI = 0;
        double Spent = 0;
        string SQLStatement = @"  SELECT s.accountNum, s.firstName, s.lastName, v.id as visitID, sc.schoolName as schoolName, v.visitDate, (t.firstName + ' ' + t.lastName) as teacherName, sp.sponsorName, s.personaID, s.nmi, s.spent
                    FROM studentInfoFP s
                    LEFT JOIN visitInfoFP v
	                    ON v.id = s.visitID
                    LEFT JOIN schoolInfoFP sc
	                    ON sc.ID = s.schoolID
                    LEFT JOIN teacherInfoFP t
	                    ON t.Id = s.teacherID
                    LEFT JOIN sponsorsFP sp
                        ON sp.id = s.sponsorID
                    WHERE s.id ='" + SID + "' AND v.id = '" + VisitID + "'";

        cmd.Connection = con;
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = SQLStatement;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            ANum = int.Parse(dr["accountNum"].ToString());
            First = dr["firstName"].ToString();
            Last = dr["lastName"].ToString();
            VID = int.Parse(dr["visitID"].ToString());
            School = dr["schoolName"].ToString();
            VDate = DateTime.Parse(dr["visitDate"].ToString());
            Teacher = dr["teacherName"].ToString();
            Sponsor = dr["sponsorName"].ToString();
            PID = int.Parse(dr["personaID"].ToString());
            NMI = double.Parse(dr["nmi"].ToString());
            Spent = double.Parse(dr["spent"].ToString());
        }

        dr.Close();
        cmd.Dispose();
        con.Close();    

        return (ANum, First, Last, VID, School, VDate, Teacher, Sponsor, PID, NMI, Spent);
    }


    // Loads a gridview with deposits and cash back (for magic computer)
    public object LoadDepositsTable(string VisitID, string AccountNumber)
    {
        con.ConnectionString = ConnectionString;
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


    //Loads a table with all lunch tickets for a visit
    public object LoadLunchesTable(int VisitID)
    {
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd = new SqlCommand();
        cmd.Connection = con;
        cmd.CommandText = @"SELECT s.id, a.pin, s.lunchServed
                              FROM studentInfoFP s
                                JOIN accountNumsFP a
                                ON a.accountNum = s.accountNum
                              WHERE s.visitID='" + VisitID + "'";

        var da = new SqlDataAdapter();
        da.SelectCommand = cmd;
        var dt = new DataTable();
        da.Fill(dt);

        cmd.Dispose();
        con.Close();

        return dt;
    }


    //Gets the PIN number associated with an account number
    public int GetPIN(int AcctNum)
    {
        string SQL = "SELECT pin FROM accountNumsFP WHERE accountNum = '" + AcctNum + "'";
        int PIN = 0;

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = SQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            PIN = int.Parse(dr["pin"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return PIN;
    }


    //Start new student insertion process
    public void NewStudent(int VisitID, int AcctNum, string FirstName, string LastName, string Gender, int SchoolID, int TeacherID, int Grade, int SponsorID)
    {
        int[] BusinessIDs = { 0, 0, 0, 0 };
        int RowCount = 0;
        int PID = 0;
        List<int> JobIDs = new List<int>();
        List<int> PersonaIDs = new List<int>();
        string JobsSQL = "SELECT id FROM jobsFP WHERE businessID=";
        string PersonasSQL = "SELECT id FROM personasFP ";
        string PersonasSQLWhere = "";
        Random random = new Random();

        //Get business IDs from sponsor ID
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT businessID, businessID2, businessID3, businessID4 FROM sponsorsFP WHERE id='" + SponsorID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            BusinessIDs[0] = int.Parse(dr["businessID"].ToString());

            if (dr["businessID2"].ToString() != "")
            {
                BusinessIDs[1] = int.Parse(dr["businessID2"].ToString());
            }
            if (dr["businessID3"].ToString() != "")
            {
                BusinessIDs[2] = int.Parse(dr["businessID3"].ToString());
            }
            if (dr["businessID4"].ToString() != "")
            {
                BusinessIDs[3] = int.Parse(dr["businessID4"].ToString());
            }

        }

        cmd.Dispose();
        con.Close();

        //Assign JobsSQL WHERE statement
        if (BusinessIDs[0] != 0)
        {
            JobsSQL = JobsSQL + "'" + BusinessIDs[0] + "' ";
        }
        if (BusinessIDs[1] != 0)
        {
            JobsSQL = JobsSQL + " OR businessID='" + BusinessIDs[1] + "' ";
        }
        if (BusinessIDs[2] != 0)
        {
            JobsSQL = JobsSQL + " OR businessID='" + BusinessIDs[2] + "' ";
        }
        if (BusinessIDs[3] != 0)
        {
            JobsSQL = JobsSQL + " OR businessID='" + BusinessIDs[3] + "' ";
        }

        //Add business ID 0 to the list, 0 means that any business can have that job
        JobsSQL = JobsSQL + " OR businessID=0";

        //Get all job IDs assigned to businesses including those assigned to any
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = JobsSQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            JobIDs.Add(int.Parse(dr["id"].ToString()));
        }

        cmd.Dispose();
        con.Close();

        //Convert finalized list to array
        int[] JobIDsArray = JobIDs.ToArray();

        //Assign WHERE clause for 1st job ID in array
        PersonasSQL = PersonasSQL + "WHERE jobID='" + JobIDsArray[0] + "' ";

        //Get persona SQL
        for (int t = 1; JobIDsArray.Length > t; t++)
        {
            PersonasSQLWhere = PersonasSQLWhere + "OR jobID='" + JobIDsArray[t] + "' ";
        }

        //Get all personas IDs assigned to job IDs
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = PersonasSQL + PersonasSQLWhere;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            PersonaIDs.Add(int.Parse(dr["id"].ToString()));
        }

        cmd.Dispose();
        con.Close();

        //Convert finalized persona list to array
        int[] PersonaIDsArray = PersonaIDs.ToArray();

        //Get a random persona ID from the array
        int PersonaIndex = random.Next(0, PersonaIDsArray.Length);
        PID = PersonaIDsArray[PersonaIndex];

        //Insert New Student
        InsertNewStudent(VisitID, AcctNum, FirstName, LastName, Gender, SchoolID, TeacherID, Grade, SponsorID, PID);

        //Insert Student into Business Unlock FP table
        InsertStudentInBizUnlock(VisitID, GetStudentID(VisitID, AcctNum));
    }


    //Insert new student into studentInfoFP
    public void InsertNewStudent(int VisitID, int AcctNum, string FirstName, string LastName, string Gender, int SchoolID, int TeacherID, int Grade, int SponsorID, int PersonaID)
    {
        string SQL = @"INSERT INTO studentInfoFP (accountNum, firstName, lastName, visitID, schoolID, teacherID, sponsorID, personaID, grade, gender, lunchServed, nmi, savingsTotal, savingsRetire, savingsEmergency, savingsOther, bizUnlocked)
					   VALUES(@accountNum, @firstName, @lastName, @visitID, @schoolID, @teacherID, @sponsorID, @personaID, @grade, @gender, 0, 0, 0, 0, 0, 0, 0);";
        int StudentID = 0;       

        using (var con = new SqlConnection(ConnectionString))
        {
            using (var cmd = new SqlCommand(SQL))
            {
                cmd.Parameters.Add("@accountNum", SqlDbType.Int).Value = AcctNum;
                cmd.Parameters.Add("@firstName", SqlDbType.VarChar).Value = FirstName;
                cmd.Parameters.Add("@lastName", SqlDbType.VarChar).Value = LastName;
                cmd.Parameters.Add("@visitID", SqlDbType.Int).Value = VisitID;
                cmd.Parameters.Add("@schoolID", SqlDbType.Int).Value = SchoolID;
                cmd.Parameters.Add("@teacherID", SqlDbType.Int).Value = TeacherID;
                cmd.Parameters.Add("@sponsorID", SqlDbType.Int).Value = SponsorID;
                cmd.Parameters.Add("@personaID", SqlDbType.Int).Value = PersonaID;
                cmd.Parameters.Add("@grade", SqlDbType.VarChar).Value = Grade;
                cmd.Parameters.Add("@gender", SqlDbType.VarChar).Value = Gender;

                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }            
        }
    }

    public void InsertStudentInBizUnlock(int VisitID, int StudentID)
    {
        //SQL business
        string SQLBiz = @"INSERT INTO businessUnlockFP (visitID, studentID, u1, u6, u7, u8, u9, u10, u11, u12, u13, u14, u15, u16, u17, u18, u19, u20, u21, u22, u23, u24, u25, u26, u27, u28, u29, u30, u31, u32)
	                      VALUES (" + VisitID + ", " + StudentID + ", 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0)";

        using (var con = new SqlConnection(ConnectionString))
        {
            using (var cmd = new SqlCommand(SQLBiz))
            {
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }


    //Checks if student already exists
    public bool StudentExists(int VisitID, int AcctNum, string FirstName, string LastName)
    {
        string SQL = "SELECT * FROM studentInfoFP WHERE visitID=" + VisitID + " AND accountNum=" + AcctNum + " AND firstName='" + FirstName + "' AND lastName='" + LastName + "'";
        bool Exists = false;

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = SQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            //Student with account number, name, and same visit id exists
            Exists = true;
        }
        else
        {
            //Student does not exist
            Exists = false;
        }

        cmd.Dispose();
        con.Close();

        return Exists;
    }


    //Gets student ID number
    public int GetStudentID(int VisitID, int AcctNum)
    {
        int StudentID = 0;

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT id FROM studentInfoFP WHERE visitID = " + VisitID + " AND accountNum = " + AcctNum + "";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            StudentID = int.Parse(dr["id"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return StudentID;
    }


    //Checks if an account number already exists for a student
    public bool AcctNumExists(int VisitID, int AcctNum)
    {
        string SQL = "SELECT accountNum FROM studentInfoFP WHERE visitID=" + VisitID + " AND accountNum=" + AcctNum + "";
        bool Exists = false;

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = SQL;
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        if (dr.HasRows)
        {
            //Student with account number already exists
            Exists = true;
        }
        else
        {
            //Account number does not exist
            Exists = false;
        }

        cmd.Dispose();
        con.Close();

        return Exists;
    }


    //Gets persona information
    public (int JobID, string JobTitle, string JobType, decimal GAI, int Age, string MarriageStatus, int SpouseAge, int NumOfChild, int Child1, int Child2, int CreditScore, decimal NMI, decimal CCDebt, decimal FurnitureLimit, decimal HomeImpLimit, decimal LongSavings, decimal EmergFunds, decimal OtherSavings, decimal AutoLoanAmount, decimal MortAmount, decimal ThatsLifeAmount, string Description) PersonaLookup(int PersonaID)
    {
        int JID = 0;
        string JT = "";
        string JTy = "";
        decimal GAI = 0;
        int Age = 0;
        string MarriageStatus = "";
        int SpouseAge = 0;
        int NumOfChild = 0;
        int Child1 = 0;
        int Child2 = 0;
        int CreditScore = 0;
        decimal NMI = 0;
        decimal CCDebt = 0;
        decimal FurnitureLimit = 0;
        decimal HomeImpLimit = 0;
        decimal LongSavings = 0;
        decimal EmergFunds = 0;
        decimal OtherSavings = 0;
        decimal AutoLoanAmount = 0;
        decimal MortAmount = 0;
        decimal ThatsLifeAmount = 0;
        string Desc = "";
        string SQL = @"SELECT p.id, p.jobID, j.jobTitle, p.jobType, p.gai, p.age, p.maritalStatus, p.spouseAge, p.numOfChildren, p.child1Age, p.child2Age, p.creditScore, p.nmi, p.ccDebt, p.furnitureLimit, p.homeImpLimit, p.longSavings, p.emergFunds, p.otherSavings, p.autoLoanAmnt, p.mortAmnt, p.thatsLifeAmnt, p.description
                          FROM personasFP p
                          LEFT JOIN jobsFP j
                          ON j.id = p.jobID
                          WHERE p.id='" + PersonaID + "' ORDER BY p.id ASC";

        cmd.Connection = con;
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = SQL;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            JID = int.Parse(dr["jobID"].ToString());
            JT = dr["jobTitle"].ToString();
            JTy = dr["jobType"].ToString();
            GAI = decimal.Parse(dr["gai"].ToString());
            Age = int.Parse(dr["age"].ToString());
            MarriageStatus = dr["maritalStatus"].ToString();

            //Check if single
            if (MarriageStatus == "Single")
            {
                SpouseAge = 0;
            }
            else
            {
                SpouseAge = int.Parse(dr["spouseAge"].ToString());
            }
            
            NumOfChild = int.Parse(dr["numOfChildren"].ToString());
            Child1 = int.Parse(dr["child1Age"].ToString());
            Child2 = int.Parse(dr["child2Age"].ToString());
            CreditScore = int.Parse(dr["creditScore"].ToString());
            NMI = decimal.Parse(dr["nmi"].ToString());
            CCDebt = decimal.Parse(dr["ccDebt"].ToString());
            FurnitureLimit = decimal.Parse(dr["furnitureLimit"].ToString());
            HomeImpLimit = decimal.Parse(dr["homeImpLimit"].ToString());
            LongSavings = decimal.Parse(dr["longSavings"].ToString());
            EmergFunds = decimal.Parse(dr["emergFunds"].ToString());
            OtherSavings = decimal.Parse(dr["otherSavings"].ToString());
            AutoLoanAmount = decimal.Parse(dr["autoLoanAmnt"].ToString());
            MortAmount = decimal.Parse(dr["mortAmnt"].ToString());
            ThatsLifeAmount = decimal.Parse(dr["thatsLifeAmnt"].ToString());
            Desc = dr["description"].ToString();
        }
        dr.Close();
        cmd.Dispose();
        con.Close();

        return (JID, JT, JTy, GAI, Age, MarriageStatus, SpouseAge, NumOfChild, Child1, Child2, CreditScore, NMI, CCDebt, FurnitureLimit, HomeImpLimit, LongSavings, EmergFunds, OtherSavings, AutoLoanAmount, MortAmount, ThatsLifeAmount, Desc);
    }


    //Gets taxes calculations
    public double TaxesCalc(int GMI, string TaxName)
    {
        double Tax = 0;
        string GAIRange1Min = "";
        string GAIRange1Max = "";
        string GAIRange2Min = "";
        string GAIRange2Max = "";
        string GAIRange3Min = "";
        string GAIRange3Max = "";
        string TaxEqual1Left = "";
        string TaxEqual1Right = "";
        string TaxEqual2Left = "";
        string TaxEqual2Right = "";
        string TaxEqual3Left = "";
        string TaxEqual3Right = "";
        string TaxGMI1 = "";
        string TaxGMI2 = "";
        string TaxGMI3 = "";
        string TaxCalc = "";
        int GAI = GMI * 12;
        string SQLStatement = "SELECT * FROM taxesFP WHERE taxName='" + TaxName + "'"; 

        //Get taxes info
        cmd.Connection = con;
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = SQLStatement;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            GAIRange1Min = dr["gaiRange1Min"].ToString();
            GAIRange1Max = dr["gaiRange1Max"].ToString();
            GAIRange2Min = dr["gaiRange2Min"].ToString();
            GAIRange2Max = dr["gaiRange2Max"].ToString();
            GAIRange3Min = dr["gaiRange3Min"].ToString();
            GAIRange3Max = dr["gaiRange3Max"].ToString();
            TaxEqual1Left = dr["taxEqual1Left"].ToString();
            TaxEqual1Right = dr["taxEqual1Right"].ToString();
            TaxEqual2Left = dr["taxEqual2Left"].ToString();
            TaxEqual2Right = dr["taxEqual2Right"].ToString();
            TaxEqual3Left = dr["taxEqual3Left"].ToString();
            TaxEqual3Right = dr["taxEqual3Right"].ToString();
            TaxGMI1 = dr["gmi1"].ToString();
            TaxGMI2 = dr["gmi2"].ToString();
            TaxGMI3 = dr["gmi3"].ToString();
            TaxCalc = dr["taxCalc"].ToString();
        }

        cmd.Dispose();
        con.Close();

        //Assign tax based on tax name entered
        if (TaxName == "Medicare" || TaxName == "Social Security")
        {
            Tax = double.Parse(TaxCalc) * GMI;
        }
        else if (TaxName == "Single Federal")
        {
            //Check for GAI range
            if (GAI > double.Parse(GAIRange1Min) && GAI < double.Parse(GAIRange1Max))
            {
                Tax = double.Parse(TaxEqual1Left) + double.Parse(TaxEqual1Right) * (GMI - double.Parse(TaxGMI1));
            }
            else if (GAI > double.Parse(GAIRange2Min) && GAI < double.Parse(GAIRange2Max))
            {
                Tax = double.Parse(TaxEqual2Left) + double.Parse(TaxEqual2Right) * (GMI - double.Parse(TaxGMI2));
            }
            else
            {
                Tax = double.Parse(TaxEqual3Left) + double.Parse(TaxEqual3Right) * (GMI - double.Parse(TaxGMI3));
            }
        }
        else if (TaxName == "Married Federal")
        {
            //Check for GAI range
            if (GAI > double.Parse(GAIRange1Min) && GAI < double.Parse(GAIRange1Max))
            {
                Tax = double.Parse(TaxEqual1Left) + double.Parse(TaxEqual1Right) * (GMI - double.Parse(TaxGMI1));
            }
            else if (GAI > double.Parse(GAIRange2Min) && GAI < double.Parse(GAIRange2Max))
            {
                Tax = double.Parse(TaxEqual2Left) + double.Parse(TaxEqual2Right) * (GMI - double.Parse(TaxGMI2));
            }
            else if (GAI > double.Parse(GAIRange3Min) && GAI < double.Parse(GAIRange3Max))
            {
                Tax = double.Parse(TaxEqual3Left) + double.Parse(TaxEqual3Right) * (GMI - double.Parse(TaxGMI3));
            }
        }

        //Round to nearest int
        Tax = Math.Round(Tax);

        //return tax
        return Tax;
    }


}