Imports System.Data.SqlClient

Public Class Class_SQLCommands
	Dim sqlserver As String = System.Configuration.ConfigurationManager.AppSettings("EV_sfp").ToString
	Dim sqldatabase As String = System.Configuration.ConfigurationManager.AppSettings("EV_DB").ToString
	Dim sqluser As String = System.Configuration.ConfigurationManager.AppSettings("db_user").ToString
	Dim sqlpassword As String = System.Configuration.ConfigurationManager.AppSettings("db_password").ToString
	Dim connection_string As String = "Server=" & sqlserver & ";database=" & sqldatabase & ";uid=" & sqluser & ";pwd=" & sqlpassword & ";Connection Timeout=20;"
	Dim con As New SqlConnection
	Dim cmd As New SqlCommand
	Dim dr As SqlDataReader

	Function InsertIntoKitInventory(kitNumber As String, schoolName As String, category As String, dateOut As String, notes As String)
		Dim connection_string As String = "Server=" & sqlserver & ";database=" & sqldatabase & ";uid=" & sqluser & ";pwd=" & sqlpassword & ";Connection Timeout=20;"
		Dim errorReturn As String
		Dim successReturn As String = "Submission successful!"
		Dim sqlStatement As String = "INSERT INTO kitInventory (
													 kitNumber
													,schoolName
													,category
													,dateOut
													,notes)
												VALUES(@kitNumber
													,@schoolName
													,@category
													,@dateOut
													,@notes);"
		'Check for blank sections
		If kitNumber = Nothing Or kitNumber = "" Then
			errorReturn = "Please enter a kit number before submitting."
			Return errorReturn
		End If

		If schoolName = Nothing Or schoolName = "" Then
			errorReturn = "Please select a school before submitting."
			Return errorReturn
		End If

		If category = Nothing Or category = "" Then
			errorReturn = "Please enter a category before submitting."
			Return errorReturn
		End If

		If dateOut = Nothing Or dateOut = "" Then
			errorReturn = "Please enter a date out before submitting."
			Return errorReturn
		End If

		'If gsiStaff = Nothing Or gsiStaff = "" Then
		'	errorReturn = "Please select a GSI Staff member before submitting."
		'	Return errorReturn
		'End If

		'Start INSERT command
		Try
			Using con As New SqlConnection(connection_string)
				Using cmd As New SqlCommand(sqlStatement)
					cmd.Parameters.Add("@kitNumber", SqlDbType.VarChar).Value = kitNumber
					cmd.Parameters.Add("@schoolName", SqlDbType.VarChar).Value = schoolName
					cmd.Parameters.Add("@category", SqlDbType.VarChar).Value = category
					'cmd.Parameters.Add("@teacherFirstName", SqlDbType.VarChar).Value = teacherFirstName
					'cmd.Parameters.Add("@teacherLastName", SqlDbType.VarChar).Value = teacherLastName
					cmd.Parameters.Add("@dateOut", SqlDbType.Date).Value = dateOut
					'cmd.Parameters.Add("@gsiStaff", SqlDbType.VarChar).Value = gsiStaff
					cmd.Parameters.Add("@notes", SqlDbType.VarChar).Value = notes
					cmd.Connection = con
					con.Open()
					cmd.ExecuteNonQuery()
					con.Close()
				End Using
			End Using

		Catch ex As Exception
			errorReturn = "Error in InsertIntoKitInventory. Could not submit new line into kits."
			Return errorReturn
		End Try

		Return successReturn
	End Function

	Function LoadKitInventory(Optional searchTerm As String = "", Optional searchBy As String = "id", Optional columnSort As String = "id", Optional orderSort As String = "ASC")
		Dim connection_string As String = "Server=" & sqlserver & ";database=" & sqldatabase & ";uid=" & sqluser & ";pwd=" & sqlpassword & ";Connection Timeout=20;"
		Dim con As New SqlConnection
		Dim cmd As New SqlCommand
		Dim errorReturn As String = ""
		Dim sqlStatement As String = "SELECT id, kitNumber, schoolName, category, FORMAT(dateIn, 'MM/dd/yyyy') as dateIn, FORMAT(dateOut, 'MM/dd/yyyy') as dateOut, gsiStaff, notes 
										FROM kitInventory"
		Dim sqlSearchStatement As String = " WHERE " & searchBy & " LIKE '%" & searchTerm & "%'"
		Dim sqlSortStatement As String = " ORDER BY " & columnSort & " " & orderSort & ""

		If searchTerm = "" And searchBy = "id" Then
			sqlStatement = sqlStatement & sqlSortStatement
		Else
			sqlStatement = sqlStatement & sqlSearchStatement & sqlSortStatement
		End If

		'Search and load kit inv table
		con.ConnectionString = connection_string
		con.Open()
		cmd = New SqlCommand()
		cmd.Connection = con
		cmd.CommandText = sqlStatement

		Dim da As New SqlDataAdapter
		da.SelectCommand = cmd
		Dim dt As New DataTable
		da.Fill(dt)

		Return dt

		da.Dispose()
		cmd.Dispose()
		con.Close()

	End Function

	Function LoadSchoolNotes(schoolName As String, Optional columnSort As String = "id", Optional orderSort As String = "DESC")
		Dim connection_string As String = "Server=" & sqlserver & ";database=" & sqldatabase & ";uid=" & sqluser & ";pwd=" & sqlpassword & ";Connection Timeout=20;"
		Dim con As New SqlConnection
		Dim cmd As New SqlCommand
		Dim errorReturn As String = ""
		Dim sqlStatement As String = "SELECT id, schoolName, note, noteUser, noteTimestamp
										FROM schoolNotes WHERE schoolName = '" & schoolName & "'"
		Dim sqlSortStatement As String = " ORDER BY " & columnSort & " " & orderSort & ""

		sqlStatement &= sqlSortStatement

		'Search and load kit inv table
		con.ConnectionString = connection_string
		con.Open()
		cmd = New SqlCommand()
		cmd.Connection = con
		cmd.CommandText = sqlStatement

		Dim da As New SqlDataAdapter
		da.SelectCommand = cmd
		Dim dt As New DataTable
		da.Fill(dt)

		cmd.Dispose()
		con.Close()

		Return dt

	End Function

	Function GetUserJob(Username As String)
		Dim returnJob As String = ""

		con.ConnectionString = connection_string
		con.Open()
		cmd.CommandText = "SELECT job FROM adminInfo WHERE username='" & Username & "'"
		cmd.Connection = con
		dr = cmd.ExecuteReader

		While dr.Read()
			returnJob = dr("job").ToString
		End While

		cmd.Dispose()
		con.Close()

		Return returnJob

	End Function

End Class
