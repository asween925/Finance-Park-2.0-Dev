using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

public class Class_Simulation
{
    private SqlConnection con = new SqlConnection();
    private SqlCommand cmd = new SqlCommand();
    private SqlDataReader dr;
    string sqlserver = System.Configuration.ConfigurationManager.AppSettings["FP_sfp"];
    string sqldatabase = System.Configuration.ConfigurationManager.AppSettings["FP_DB"];
    string sqluser = System.Configuration.ConfigurationManager.AppSettings["db_user"];
    string sqlpassword = System.Configuration.ConfigurationManager.AppSettings["db_password"];
    private string ConnectionString;

    public Class_Simulation()
    {
        ConnectionString = "Server=" + sqlserver + ";database=" + sqldatabase + ";uid=" + sqluser + ";pwd=" + sqlpassword + ";Connection Timeout=20;";
    }

    public int GetActiveQuestions()
    {
        int RowCount = 1;
        
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT COUNT(*) as count FROM questionsFP WHERE active=1";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            RowCount = int.Parse(dr["count"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return RowCount;
    }

    public string GetQuestionName(int Q)
    {
        string Question = "";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT questionText FROM questionsFP WHERE questionOrder=" + Q + ";";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Question = dr["questionText"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return Question;
    }

    public string GetAnswerType(int Q)
    {
        string Type = "";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT answerType FROM questionsFP WHERE questionOrder='" + Q + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Type = dr["answerType"].ToString();
        }

        cmd.Dispose();
        con.Close();

        return Type;
    }

    public (string O1, string O2, string O3, string O4, string O5, int Options) GetMultiOptions(int Q)
    {
        string O1 = "";
        string O2 = "";
        string O3 = "";
        string O4 = "";
        string O5 = "";
        int Options = 1;

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = @"SELECT q.option1, q.option2, q.option3, q.option4, q.option5, (
                                SELECT COUNT(*)
                                FROM(values(q.option1), (q.option2), (q.option3), (q.option4), (q.option5)) as v(col)
                                WHERE v.col IS NOT NULL
                            ) as nonNullCount
                            FROM questionsFP as q
                            WHERE q.questionOrder='" + Q + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            O1 = dr["option1"].ToString();
            O2 = dr["option2"].ToString();
            O3 = dr["option3"].ToString();
            O4 = dr["option4"].ToString();
            O5 = dr["option5"].ToString();
            Options = int.Parse(dr["nonNullCount"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return (O1, O2, O3, O4, O5, Options);
    }

    public bool CheckAnswersDB(int VisitID, int StudentID)
    {
        bool HasRows = false;

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT visitID, studentID FROM answersFP WHERE visitID='" + VisitID + "' AND studentID='" + StudentID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        if (dr.HasRows == true)
        {
            HasRows = true;
        }

        cmd.Dispose();
        con.Close();

        return HasRows;
    }

    public void InsertLifestyleAnswer(int VisitID, int StudentID, string Answer)
    {
        string SQL = @"INSERT INTO answersFP (visitID, studentID, q1A)
					   VALUES (@visitID, @studentID, @q1A)";

        using (var con = new SqlConnection(ConnectionString))
        {
            using (var cmd = new SqlCommand(SQL))
            {
                cmd.Parameters.Add("@visitID", SqlDbType.Int).Value = VisitID;
                cmd.Parameters.Add("@studentID", SqlDbType.VarChar).Value = StudentID;
                cmd.Parameters.Add("@q1A", SqlDbType.VarChar).Value = Answer;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public void UpdateLifestyleAnswer(int VisitID, int StudentID, string Answer, int Q)
    {
        string SQL = "UPDATE answersFP SET ";

        switch(Q) 
        {
            case 1:
                SQL = SQL + "q1A=@answer ";
                break;
            case 2:
                SQL = SQL + "q2A=@answer ";
                break;
            case 3:
                SQL = SQL + "q3A=@answer ";
                break;
            case 4:
                SQL = SQL + "q4A=@answer ";
                break;
            case 5:
                SQL = SQL + "q5A=@answer ";
                break;
            case 6:
                SQL = SQL + "q6A=@answer ";
                break;
            case 7:
                SQL = SQL + "q7A=@answer ";
                break;
            case 8:
                SQL = SQL + "q8A=@answer ";
                break;
            case 9:
                SQL = SQL + "q9A=@answer ";
                break;
            case 10:
                SQL = SQL + "q10A=@answer ";
                break;
        }

        //Add WHERE clause
        SQL = SQL + "WHERE visitID=@visitID AND studentID=@studentID";

        using (var con = new SqlConnection(ConnectionString))
        {
            using (var cmd = new SqlCommand(SQL))
            {
                cmd.Parameters.Add("@visitID", SqlDbType.Int).Value = VisitID;
                cmd.Parameters.Add("@studentID", SqlDbType.VarChar).Value = StudentID;
                cmd.Parameters.Add("@answer", SqlDbType.VarChar).Value = Answer;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public (string ScreenType, string HeaderText, string Text, string Location, int UnlockCode) GetTransitionData(int Step)
    {
        string ScreenType = "";
        string HeaderText = "";
        string Text = "";
        string Location = "";
        int UnlockCode = 0;

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "SELECT * FROM transitionsFP WHERE step=" + Step + "";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            ScreenType = dr["screenType"].ToString();
            HeaderText = dr["headerText"].ToString();
            Text = dr["text"].ToString();
            Location = dr["location"].ToString();        
            UnlockCode = int.Parse(dr["unlockCode"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return (ScreenType, HeaderText, Text, Location, UnlockCode);
    }

    public int GetTotalBizUnlocked(int VisitID, int StudentID)
    {
        int TotalBizUnlocked = 0;

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = @"SELECT
		  (CASE WHEN u1=1 THEN 1 ELSE 0 END +
		  CASE WHEN u6=1 THEN 1 ELSE 0 END +
		  CASE WHEN u7=1 THEN 1 ELSE 0 END +
		  CASE WHEN u8=1 THEN 1 ELSE 0 END +
		  CASE WHEN u9=1 THEN 1 ELSE 0 END +
		  CASE WHEN u10=1 THEN 1 ELSE 0 END +
		  CASE WHEN u11=1 THEN 1 ELSE 0 END +
		  CASE WHEN u12=1 THEN 1 ELSE 0 END +
		  CASE WHEN u13=1 THEN 1 ELSE 0 END +
		  CASE WHEN u14=1 THEN 1 ELSE 0 END +
		  CASE WHEN u15=1 THEN 1 ELSE 0 END +
		  CASE WHEN u16=1 THEN 1 ELSE 0 END +
		  CASE WHEN u17=1 THEN 1 ELSE 0 END +
		  CASE WHEN u18=1 THEN 1 ELSE 0 END +
		  CASE WHEN u19=1 THEN 1 ELSE 0 END +
		  CASE WHEN u20=1 THEN 1 ELSE 0 END +
		  CASE WHEN u21=1 THEN 1 ELSE 0 END +
		  CASE WHEN u22=1 THEN 1 ELSE 0 END +
		  CASE WHEN u23=1 THEN 1 ELSE 0 END +
		  CASE WHEN u24=1 THEN 1 ELSE 0 END +
		  CASE WHEN u25=1 THEN 1 ELSE 0 END +
		  CASE WHEN u26=1 THEN 1 ELSE 0 END +
		  CASE WHEN u27=1 THEN 1 ELSE 0 END +
		  CASE WHEN u28=1 THEN 1 ELSE 0 END +
		  CASE WHEN u29=1 THEN 1 ELSE 0 END +
		  CASE WHEN u30=1 THEN 1 ELSE 0 END +
		  CASE WHEN u31=1 THEN 1 ELSE 0 END +
		  CASE WHEN u32=1 THEN 1 ELSE 0 END) as TotalUnlock
	  FROM businessUnlockFP WHERE visitID='" + VisitID + "' AND studentID='" + StudentID + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            TotalBizUnlocked = int.Parse(dr["TotalUnlock"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return TotalBizUnlocked;
    }

    public void AddUnlockedBusiness (int VisitID, int StudentID, int BusinessID)
    {
        string SQL = "UPDATE businessUnlockFP SET u" + BusinessID + "=1 WHERE visitID='" + VisitID + "' AND studentID='" + StudentID + "'";

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = SQL;
        cmd.ExecuteNonQuery();

        cmd.Dispose();
        con.Close();
    }

    public bool CheckBudgets(int VisitID, int StudentID)
    {
        bool HasRows = false;

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.CommandText = "SELECT visitID, studentID FROM budgetsFP WHERE visitID='" + VisitID + "' AND studentID='" + StudentID + "'";
        cmd.Connection = con;
        dr = cmd.ExecuteReader();

        if (dr.HasRows == true)
        {
            HasRows = true;
        }

        cmd.Dispose();
        con.Close();

        return HasRows;
    }

    public void InsertBudget (int VisitID, int StudentID, int BusinessID, string Amount)
    {
        //string SQLBudget = "";
        string SQL = "INSERT INTO budgetsFP (visitID, studentID, budget" + BusinessID + ") VALUES (@visitID, @studentID, @amount)";

        //Check the business id and add it to the SQL string
        //switch (BusinessID)
        //{
        //    case 1:
        //        SQLBudget = "budget1";
        //        break;
        //    case 7:
        //        SQLBudget = "budget7";
        //        break;
        //    case 11:
        //        SQLBudget = "budget11";
        //        break;
        //    case 12:
        //        SQLBudget = "budget12";
        //        break;
        //    case 13:
        //        SQLBudget = "budget13";
        //        break;
        //    case 15:
        //        SQLBudget = "budget15";
        //        break;
        //    case 16:
        //        SQLBudget = "budget16";
        //        break;
        //    case 18:
        //        SQLBudget = "budget18";
        //        break;
        //    case 19:
        //        SQLBudget = "budget19";
        //        break;
        //    case 21:
        //        SQLBudget = "budget21";
        //        break;
        //    case 22:
        //        SQLBudget = "budget22";
        //        break;
        //    case 23:
        //        SQLBudget = "budget23";
        //        break;
        //    case 24:
        //        SQLBudget = "budget24";
        //        break;
        //    case 26:
        //        SQLBudget = "budget26";
        //        break;
        //    case 27:
        //        SQLBudget = "budget27";
        //        break;
        //    case 28:
        //        SQLBudget = "budget28";
        //        break;
        //    case 29:
        //        SQLBudget = "budget29";
        //        break;
        //    case 30:
        //        SQLBudget = "budget30";
        //        break;
        //    case 31:
        //        SQLBudget = "budget31";
        //        break;

        //}



        //Insert new student into budgets table
        using (var con = new SqlConnection(ConnectionString))
        {
            using (var cmd = new SqlCommand(SQL))
            {
                cmd.Parameters.Add("@visitID", SqlDbType.Int).Value = VisitID;
                cmd.Parameters.Add("@studentID", SqlDbType.VarChar).Value = StudentID;
                cmd.Parameters.Add("@amount", SqlDbType.VarChar).Value = Amount;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public void UpdateBudget (int VisitID, int StudentID, int BusinessID, string Amount)
    {
        string SQL = "UPDATE budgetsFP SET budget" + BusinessID + "=@amount WHERE visitID=@visitID AND studentID=@studentID";

        using (var con = new SqlConnection(ConnectionString))
        {
            using (var cmd = new SqlCommand(SQL))
            {
                cmd.Parameters.Add("@visitID", SqlDbType.Int).Value = VisitID;
                cmd.Parameters.Add("@studentID", SqlDbType.VarChar).Value = StudentID;
                cmd.Parameters.Add("@amount", SqlDbType.Int).Value = Amount;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public int GetBudgetData (int VisitID, int StudentID, int BusinessID)
    {
        int Budget = 0;
        
        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = "SELECT CASE WHEN budget" + BusinessID + " IS NOT NULL THEN budget" + BusinessID + " ELSE 0 END as budget FROM budgetsFP WHERE visitID='" + VisitID + "' AND studentID='" + StudentID + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Budget = Convert.ToInt16(dr["budget"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return Budget;
    }
    
    public int GetTotalBudget (int VisitID, int StudentID)
    {
        int Total = 0;

        con.ConnectionString = ConnectionString;
        con.Open();
        cmd.Connection = con;
        cmd.CommandText = @"SELECT 
                        (CASE WHEN budget1 IS NOT NULL THEN budget1 ELSE 0 END +
		                  CASE WHEN budget7 IS NOT NULL THEN budget7 ELSE 0 END +
		                  CASE WHEN budget11 IS NOT NULL THEN budget11 ELSE 0 END +
		                  CASE WHEN budget12 IS NOT NULL THEN budget12 ELSE 0 END +
		                  CASE WHEN budget13 IS NOT NULL THEN budget13 ELSE 0 END +
		                  CASE WHEN budget15 IS NOT NULL THEN budget15 ELSE 0 END +
		                  CASE WHEN budget16 IS NOT NULL THEN budget16 ELSE 0 END +
		                  CASE WHEN budget18 IS NOT NULL THEN budget18 ELSE 0 END +
		                  CASE WHEN budget19 IS NOT NULL THEN budget19 ELSE 0 END +
		                  CASE WHEN budget21 IS NOT NULL THEN budget21 ELSE 0 END +
		                  CASE WHEN budget22 IS NOT NULL THEN budget22 ELSE 0 END +
		                  CASE WHEN budget23 IS NOT NULL THEN budget23 ELSE 0 END +
		                  CASE WHEN budget24 IS NOT NULL THEN budget24 ELSE 0 END +
		                  CASE WHEN budget26 IS NOT NULL THEN budget26 ELSE 0 END +
		                  CASE WHEN budget27 IS NOT NULL THEN budget27 ELSE 0 END +
		                  CASE WHEN budget28 IS NOT NULL THEN budget28 ELSE 0 END +
		                  CASE WHEN budget29 IS NOT NULL THEN budget29 ELSE 0 END +
		                  CASE WHEN budget30 IS NOT NULL THEN budget30 ELSE 0 END +
		                  CASE WHEN budget31 IS NOT NULL THEN budget31 ELSE 0 END) as Total 
                        FROM budgetsFP WHERE visitID='" + VisitID + "' AND studentID='" + StudentID + "'";
        dr = cmd.ExecuteReader();

        while (dr.Read())
        {
            Total = int.Parse(dr["Total"].ToString());
        }

        cmd.Dispose();
        con.Close();

        return Total;
    }

    public void InsertShoppingItem (int VisitID, int StudentID, int BusinessID, int ItemID)
    {
        //string SQLBudget = "";
        string SQL = "INSERT INTO studentShoppingFP (visitID, studentID, businessID, itemID) VALUES (@visitID, @studentID, @businessID, @itemID)";

        //Insert new student into budgets table
        using (var con = new SqlConnection(ConnectionString))
        {
            using (var cmd = new SqlCommand(SQL))
            {
                cmd.Parameters.Add("@visitID", SqlDbType.Int).Value = VisitID;
                cmd.Parameters.Add("@studentID", SqlDbType.Int).Value = StudentID;
                cmd.Parameters.Add("@businessID", SqlDbType.Int).Value = BusinessID;
                cmd.Parameters.Add("@itemID", SqlDbType.Int).Value = ItemID;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    public void DeleteShoppingItem(int VisitID, int StudentID, int BusinessID, int ItemID)
    {
        string SQL = "DELETE FROM studentShoppingFP WHERE visitID=@visitID AND studentID=@studentID AND businessID=@businessID AND itemID=@itemID";

        //Insert new student into budgets table
        using (var con = new SqlConnection(ConnectionString))
        {
            using (var cmd = new SqlCommand(SQL))
            {
                cmd.Parameters.Add("@visitID", SqlDbType.Int).Value = VisitID;
                cmd.Parameters.Add("@studentID", SqlDbType.Int).Value = StudentID;
                cmd.Parameters.Add("@businessID", SqlDbType.Int).Value = BusinessID;
                cmd.Parameters.Add("@itemID", SqlDbType.Int).Value = ItemID;
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }

    //public 
}