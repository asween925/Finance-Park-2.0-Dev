Imports System.Data.SqlClient

Public Class Class_BusinessData
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

	Function GetBusinessID(businessName As String)
		Dim returnBusinessID As String = ""

		'Get school info from school name
		con.ConnectionString = connection_string
		con.Open()
		cmd.CommandText = "SELECT ID FROM businessInfo WHERE businessName = '" & businessName & "'"
		cmd.Connection = con
		dr = cmd.ExecuteReader

		While dr.Read()
			returnBusinessID = dr("ID").ToString()
			cmd.Dispose()
			con.Close()
			Return returnBusinessID
		End While

		cmd.Dispose()
		con.Close()

		Return returnBusinessID
	End Function

	Function LoadBusinessNamesDDL(businessNameDDL As DropDownList)
		Dim errorString As String
		'Dim businessNameDDL As DropDownList

		'Clear out teacher and school DDLs
		businessNameDDL.Items.Clear()

		'Populate school DDL from entered visit date
		Try
			con.ConnectionString = connection_string
			con.Open()
			cmd.CommandText = "SELECT businessName FROM businessInfo WHERE NOT id=28 AND NOT id=29 ORDER BY businessName"
			cmd.Connection = con
			dr = cmd.ExecuteReader

			While dr.Read()
				businessNameDDL.Items.Add(dr(0).ToString)
			End While
			businessNameDDL.Items.Insert(0, "")

			cmd.Dispose()
			con.Close()

		Catch
			errorString = "Error in visitDate. Could not get school names."
			Return errorString
			Exit Function
		End Try

		Return businessNameDDL.Items
	End Function

	'Update image and title based on business ID
	Function GetBusinessLogos(BusinessID As String) As (ImagePath As String, BColor As String, BusinessName As String)
		Dim I As String = ""
		Dim C As String = ""
		Dim B As String = ""
		Dim logoRoot As String = "~/media/Logos/"

		con.ConnectionString = connection_string
		con.Open()
		cmd.CommandText = "SELECT logoPath, businessColor, businessName FROM businessinfo WHERE ID='" & BusinessID & "'"
		cmd.Connection = con
		dr = cmd.ExecuteReader

		While dr.Read()
			I = logoRoot & dr(0).ToString
			C = dr(1).ToString
			B = dr(2).ToString
		End While

		cmd.Dispose()
		con.Close()

		Return (I, C, B)

	End Function

	'Get business address
	Function GetBusinessAddress(BusinessName As String)
		Dim returnAddress As String = ""

		'Get school info from school name
		con.ConnectionString = connection_string
		con.Open()
		cmd.CommandText = "SELECT address FROM businessInfo WHERE businessName = '" & BusinessName & "'"
		cmd.Connection = con
		dr = cmd.ExecuteReader

		While dr.Read()
			returnAddress = dr("address").ToString()
			cmd.Dispose()
			con.Close()
			Return returnAddress
		End While

		cmd.Dispose()
		con.Close()

		Return returnAddress
	End Function

	'Get job ID from job title
	Function GetJobID(JobTitle As String)
		Dim ReturnID As String

		'Get job ID from job title
		con.ConnectionString = connection_string
		con.Open()
		cmd.CommandText = "SELECT id FROM jobs WHERE jobTitle='" & JobTitle & "'"
		cmd.Connection = con
		dr = cmd.ExecuteReader

		While dr.Read()
			ReturnID = dr("id").ToString
		End While

		cmd.Dispose()
		con.Close()

		Return ReturnID

	End Function

	'Gets a string of the closed businesses in EV
	Function GetClosedBusinesses(VisitDate As String)
		Dim ClosedBusinesses As String

		con.ConnectionString = connection_string
		con.Open()
		cmd.CommandText = "Declare @val varchar(MAX)
                               SELECT @val = COALESCE(@val + ', ' + b.businessName, b.businessName)
	                                FROM businessInfo b
                                INNER JOIN onlineBanking o
                                ON b.id = o.businessID
                                WHERE o.openstatus=0 AND o.visitDate='" & VisitDate & "'
                                ORDER BY b.businessName
	                                SELECT @val as names"
		cmd.Connection = con
		dr = cmd.ExecuteReader

		While dr.Read()
			ClosedBusinesses = dr("names").ToString
		End While

		cmd.Dispose()
		con.Close()

		Return ClosedBusinesses
	End Function

End Class
