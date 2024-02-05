Imports System.Data.SqlClient

Public Class Class_TeacherData
	Dim VisitID As New Class_VisitData
	Dim Visit As Integer = VisitID.GetVisitID
	Dim con As New SqlConnection
	Dim cmd As New SqlCommand
	Dim dr As SqlDataReader
	Dim sqlserver As String = System.Configuration.ConfigurationManager.AppSettings("EV_sfp").ToString
	Dim sqldatabase As String = System.Configuration.ConfigurationManager.AppSettings("EV_DB").ToString
	Dim sqluser As String = System.Configuration.ConfigurationManager.AppSettings("db_user").ToString
	Dim sqlpassword As String = System.Configuration.ConfigurationManager.AppSettings("db_password").ToString
	Dim connection_string As String = "Server=" & sqlserver & ";database=" & sqldatabase & ";uid=" & sqluser & ";pwd=" & sqlpassword & ";Connection Timeout=20;"
	Dim volRange As String
	Dim vMin As String
	Dim vMax As String
	Dim errorStr As String

	'Gets the contact teacher name
	Function GetContactTeacher(schoolName As String)
		Dim returnData As String = ""

		'Get school info from school name
		con.ConnectionString = connection_string
		con.Open()
		cmd.CommandText = "SELECT TRIM(firstName) + ' ' + TRIM(lastName) as teacherName FROM teacherInfo WHERE schoolName = '" & schoolName & "' AND isContact=1"
		cmd.Connection = con
		dr = cmd.ExecuteReader

		While dr.Read()
			returnData = dr("teacherName").ToString()
			cmd.Dispose()
			con.Close()
			Return returnData
		End While

		cmd.Dispose()
		con.Close()

		Return returnData
	End Function


	'Gets first and last name of teacher from a school name
	Function LoadTeacherNameDDL(SchoolName As String, DDL As DropDownList)
		con.ConnectionString = connection_string
		con.Open()
		cmd.CommandText = "SELECT firstName, lastName FROM teacherInfo WHERE schoolName='" & SchoolName & "'"
		cmd.Connection = con
		dr = cmd.ExecuteReader

		While dr.Read()
			DDL.Items.Add(dr(0).ToString & " " & dr(1).ToString)
		End While

		DDL.Items.Insert(0, "")

		cmd.Dispose()
		con.Close()

		Return DDL
	End Function
End Class
