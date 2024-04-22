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

}