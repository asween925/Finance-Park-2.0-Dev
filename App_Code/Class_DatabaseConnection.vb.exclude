Imports System.Data.SqlClient
Imports Microsoft.VisualBasic


Public Class DatabaseConection
    Public Function dbFunctions(ByVal sql As String, ByVal rValue As Integer) As SqlDataReader
        Dim sqlserver As String = System.Configuration.ConfigurationManager.AppSettings("EV_sfp").ToString
        Dim sqldatabase As String = System.Configuration.ConfigurationManager.AppSettings("EV_DB").ToString
        Dim sqluser As String = System.Configuration.ConfigurationManager.AppSettings("db_user").ToString
        Dim sqlpassword As String = System.Configuration.ConfigurationManager.AppSettings("db_password").ToString

        Dim connection_string As String = "Server=" & sqlserver & ";database=" & sqldatabase & ";uid=" & sqluser & ";pwd=" & sqlpassword & ";Connection Timeout=20;"

        Dim con As New SqlConnection
        Dim cmd As New SqlCommand
        Dim dr As SqlDataReader

        con.ConnectionString = connection_string
        con.Open()
        cmd.Connection = con
        cmd.CommandText = sql

        Try
            If rValue = 0 Then
                dr = cmd.ExecuteReader
                Return dr
            Else
                cmd.ExecuteNonQuery()
            End If

        Catch ex As Exception

            con.Close()
            con.Dispose()
            cmd.Dispose()

        Finally

        End Try

        dr.Close()
        con.Dispose()
        con.Close()
        cmd.Dispose()
    End Function
End Class