Imports System.Data.SqlClient

Public Class Class_BadgesData
    Dim con As New SqlConnection
    Dim cmd As New SqlCommand
    Dim dr As SqlDataReader
    Dim sqlserver As String = System.Configuration.ConfigurationManager.AppSettings("EV_sfp").ToString
    Dim sqldatabase As String = System.Configuration.ConfigurationManager.AppSettings("EV_DB").ToString
    Dim sqluser As String = System.Configuration.ConfigurationManager.AppSettings("db_user").ToString
    Dim sqlpassword As String = System.Configuration.ConfigurationManager.AppSettings("db_password").ToString
    Dim connection_string As String = "Server=" & sqlserver & ";database=" & sqldatabase & ";uid=" & sqluser & ";pwd=" & sqlpassword & ";Connection Timeout=20;"
    Dim studentCount As String


    'Loads a table of saved badges from a passed through visit ID. Optional where and order by clauses
    Function LoadExistingBadgesTable(VisitID As String, Optional SQLWhere As String = "", Optional SQLOrderBy As String = "")
        Dim SQLStatement As String = "SELECT id, employeeNumber, employeeName, businessName, position, businessID, date, CONCAT('~/media/', photoPath) as photoPath, visitID
                                        FROM badges
                                        WHERE visitID = '" & VisitID & "'"

        'Check if where clause and order by clause is not blank
        If SQLWhere <> "" Then
            SQLStatement &= SQLWhere & SQLOrderBy
        Else
            SQLStatement &= SQLOrderBy
        End If

        'Populate existing badges table
        con.ConnectionString = connection_string
        con.Open()
        cmd.Connection = con
        cmd.CommandText = SQLStatement

        Dim da As New SqlDataAdapter
        da.SelectCommand = cmd
        Dim dt As New DataTable
        da.Fill(dt)

        cmd.Dispose()
        con.Close()

        Return dt

    End Function


    'Populates a gridview with student names that have a badge saved in the DB
    Function LoadExistingBadgesNamesDDL(VisitID As String, StudentNamesDDL As DropDownList)
        Dim SQLStatement As String = "SELECT CONCAT(employeeNumber, '.   ', + employeeName)
                                        FROM badges
                                        WHERE visitID='" & VisitID & "'
                                        ORDER BY id DESC"

        'Add student names who have a saved badge to DDL
        con.ConnectionString = connection_string
        con.Open()
        cmd.CommandText = SQLStatement
        cmd.Connection = con
        dr = cmd.ExecuteReader

        While dr.Read()
            StudentNamesDDL.Items.Add(dr(0).ToString)
        End While

        StudentNamesDDL.Items.Insert(0, "")

        cmd.Dispose()
        con.Close()

        Return StudentNamesDDL
    End Function


    'Checks if a badge exists
    Function CheckIfBadgeExists(VisitID As String, AccountNumber As String)
        Dim NumberCheck As Boolean = False

        cmd.Connection = con
        con.ConnectionString = connection_string
        con.Open()
        cmd.CommandText = "SELECT employeeNumber FROM badges WHERE visitID='" & VisitID & "' AND employeeNumber='" & AccountNumber & "'"
        dr = cmd.ExecuteReader

        While dr.Read()
            If dr.HasRows = True Then
                NumberCheck = True
            End If
        End While

        dr.Close()
        cmd.Dispose()
        con.Close()

        Return NumberCheck
    End Function
End Class
