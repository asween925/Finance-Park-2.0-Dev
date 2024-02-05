Imports System.Data.SqlClient
Public Class Class_TransactionData
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

    Function GetTotalTransactions(empID As String, visitID As String)
        Dim returnTotalTransactions As String = ""

        'Returns total purchase amount
        cmd = New SqlCommand
        cmd.Connection = con
        con.ConnectionString = connection_string
        con.Open()
        cmd.CommandText = "SELECT CASE WHEN SUM(saleamount + saleAmount2 + saleAmount3 + saleAmount4) IS NULL THEN '0.00' 
                                ELSE SUM(saleamount + saleAmount2 + saleAmount3 + saleAmount4) END as Transactions 
                                FROM transactions 
                                WHERE visitdate = '" & visitID & "' AND employeeNumber ='" & empID & "'"
        dr = cmd.ExecuteReader

        While dr.Read
            returnTotalTransactions = dr("Transactions").ToString()
        End While

        cmd.Dispose()
        con.Close()
        'dr.Close()

        Return returnTotalTransactions
    End Function

    Function LoadTransactionTable(empID As String, visitID As String)

        'Load transaction info
        cmd = New SqlCommand
        cmd.Connection = con
        con.ConnectionString = connection_string
        con.Open()
        cmd.CommandText = "SELECT t.ID,t.transactiontimestamp,b.businessName,CONCAT('$',t.saleamount + t.saleAmount2 + t.saleAmount3 + t.saleAmount4) AS saleamount FROM Transactions t 
                            INNER JOIN businessinfo b ON b.ID = t.business
                            WHERE visitdate ='" & visitID & "' AND employeeNumber ='" & empID & "'"

        Dim da As New SqlDataAdapter
        da.SelectCommand = cmd
        Dim dt As New DataTable
        da.Fill(dt)

        cmd.Dispose()
        con.Close()

        Return dt

    End Function

    Function GetDepositInfo(visitID As String, empID As String) As (NetDeposit1 As Double, NetDeposit2 As Double, NetDeposit3 As Double, NetDeposit4 As Double, CBW1 As Double, CBW2 As Double, CBW3 As Double, CBW4 As Double, InitialDeposit1 As Double, InitialDeposit2 As Double, InitialDeposit3 As Double, InitialDeposit4 As Double, Savings As Double)
        Dim N1 As Double
        Dim N2 As Double
        Dim N3 As Double
        Dim N4 As Double
        Dim C1 As Double
        Dim C2 As Double
        Dim C3 As Double
        Dim C4 As Double
        Dim S As Double
        Dim I1 As Double
        Dim I2 As Double
        Dim I3 As Double
        Dim I4 As Double

        cmd = New SqlCommand
        cmd.Connection = con
        con.ConnectionString = connection_string
        con.Open()
        cmd.CommandText = "SELECT netDeposit1, netDeposit2, netDeposit3, netDeposit4, cbw1, cbw2, cbw3, cbw4, savings, initialDeposit1, initialDeposit2, initialDeposit3, initialDeposit4 FROM studentInfo WHERE visit='" & visitID & "' AND employeeNumber ='" & empID & "'"
        dr = cmd.ExecuteReader

        While dr.Read()

            If dr("netDeposit1").ToString = Nothing Then
                N1 = 0.00
            Else
                N1 = dr("netDeposit1")
            End If
            If dr("NetDeposit2").ToString = Nothing Then
                N2 = 0.00
            Else
                N2 = dr("netDeposit2")
            End If
            If dr("NetDeposit3").ToString = Nothing Then
                N3 = 0.00
            Else
                N3 = dr("netDeposit3")
            End If
            If dr("NetDeposit4").ToString = Nothing Then
                N4 = 0.00
            Else
                N4 = dr("netDeposit4")
            End If

            If dr("cbw1").ToString = Nothing Then
                C1 = 0.00
            Else
                C1 = dr("cbw1")
            End If
            If dr("cbw2").ToString = Nothing Then
                C2 = 0.00
            Else
                C2 = dr("cbw2")
            End If
            If dr("cbw3").ToString = Nothing Then
                C3 = 0.00
            Else
                C3 = dr("cbw3")
            End If
            If dr("cbw4").ToString = Nothing Then
                C4 = 0.00
            Else
                C4 = dr("cbw4")
            End If

            If dr("initialDeposit1").ToString = Nothing Then
                I1 = 0.00
            Else
                I1 = dr("initialDeposit1")
            End If
            If dr("initialDeposit2").ToString = Nothing Then
                I2 = 0.00
            Else
                I2 = dr("initialDeposit2")
            End If
            If dr("initialDeposit3").ToString = Nothing Then
                I3 = 0.00
            Else
                I3 = dr("initialDeposit3")
            End If
            If dr("initialDeposit4").ToString = Nothing Then
                I4 = 0.00
            Else
                I4 = dr("initialDeposit4")
            End If

            If dr("savings").ToString = Nothing Then
                S = 0.00
            Else
                S = dr("savings")
            End If
        End While

        cmd.Dispose()
        con.Close()

        Return (N1, N2, N3, N4, C1, C2, C3, C4, I1, I2, I3, I4, S)

    End Function

    Function LoadStudentTotalPurchasesTable(VisitID As String)
        con.ConnectionString = connection_string
        con.Open()
        cmd = New SqlCommand
        cmd.Connection = con
        cmd.CommandText = "IF (OBJECT_ID('tempdb..#netdeposits') IS NOT NULL) DROP TABLE #netdeposits   
                                   SELECT 
                                          s.employeeNumber
                                          ,s.firstName
                                          ,s.lastName
                                          ,SUM(ISNULL(s.netdeposit1,0) + ISNULL(s.netdeposit2,0) + ISNULL(s.netdeposit3,0) + ISNULL(s.netdeposit4,0) - ISNULL(s.savings,0)) totalDeposits
                                   INTO #netdeposits
                                   FROM dbo.studentInfo s
                                   WHERE visit = '" & VisitID & "'
                                   GROUP BY s.employeeNumber, s.firstName, s.lastName

                                   SELECT 
                                           t.employeeNumber, CONCAT (MAX(firstname), ' ',MAX(lastName)) as studentname
                                          ,MAX(s.totalDeposits) TotalDeposits
                                          ,SUM(ISNULL(saleamount + saleamount2 + saleamount3 + saleamount4,0)) as TotalPurchases
                                          ,MAX(s.totalDeposits) - sum(ISNULL(saleamount + saleamount2 + saleamount3 + saleamount4,0)) as Balance
                                   FROM transactions t
                                   INNER JOIN #netdeposits s ON t.employeeNumber = s.employeeNumber
                                   WHERE visitdate = '" & VisitID & "'
                                   GROUP BY t.employeeNumber
                                   ORDER BY TotalPurchases DESC"

        Dim da As New SqlDataAdapter
        da.SelectCommand = cmd
        Dim dt As New DataTable
        da.Fill(dt)

        Return dt
    End Function

    Function LoadTransactionTimestampsTable(VisitID As String, AccountNumber As String)

        'Load transaction info
        cmd = New SqlCommand
        cmd.Connection = con
        con.ConnectionString = connection_string
        con.Open()
        cmd.CommandText = "SELECT t.ID, b.BusinessName as Business, t.transactiontimestamp, t.saleamount, t.transactiontimestamp2, t.saleamount2, t.transactiontimestamp3, t.saleamount3, t.transactiontimestamp4, t.saleamount4 
                                FROM transactions t
                                INNER JOIN BusinessInfo b ON b.id = t.business
                                WHERE t.visitdate ='" & VisitID & "' AND t.employeeNumber ='" & AccountNumber & "'
                                ORDER BY transactiontimestamp"

        Dim da As New SqlDataAdapter
        da.SelectCommand = cmd
        Dim dt As New DataTable
        da.Fill(dt)

        da.Dispose()
        cmd.Dispose()
        con.Close()

        Return dt

    End Function

    Function InsertSale(VisitID As String, AccountNumber As String, BusinessID As String, SaleAmount As String, Item1 As String, Optional Item2 As String = "0.00", Optional Item3 As String = "0.00", Optional Item4 As String = "0.00")
        Dim ReturnStatement As String
        Dim Timestamp As String = DateTime.Now
        Dim SQLStatement As String = "INSERT INTO transactions (employeeNumber, business, transactionTimestamp, saleAmount, visitdate, item1, item2, item3, item4, saleAmount2, saleAmount3, saleAmount4) 
                                VALUES ('" & AccountNumber & "', '" & BusinessID & "', '" & Timestamp & "', '" & SaleAmount & "', '" & VisitID & "', '" & Item1 & "'
                                , '" & Item2 & "', '" & Item3 & "', '" & Item4 & "', 0.00, 0.00, 0.00)"

        'First check if a sale for that account number has been inserted into the transactions table already
        cmd.Connection = con
        con.ConnectionString = connection_string
        con.Open()
        cmd.CommandText = "SELECT saleAmount FROM transactions WHERE visitDate='" & VisitID & "' AND employeeNumber='" & AccountNumber & "' AND business='" & BusinessID & "'"
        dr = cmd.ExecuteReader

        If dr.HasRows Then
            cmd.Dispose()
            ReturnStatement = "Has Rows"
        Else
            dr.Close()
            cmd.CommandText = SQLStatement
            cmd.ExecuteNonQuery()
            cmd.Dispose()
            con.Close()

            ReturnStatement = "Sale inserted"
        End If

        cmd.Dispose()
        con.Close()
        Return ReturnStatement

    End Function

    Function Sale(SaleNumber As String, VisitID As String, AccountNumber As String, BusinessID As String, SaleAmount As String, Item1 As String, Optional Item2 As String = "0.00", Optional Item3 As String = "0.00", Optional Item4 As String = "0.00")
        Dim ReturnStatement As String
        Dim Timestamp As String = DateTime.Now
        Dim SQLMainSelectStatment As String = "FROM transactions WHERE visitDate='" & VisitID & "' AND employeeNumber='" & AccountNumber & "' AND business='" & BusinessID & "'"
        Dim SQLMainUpdateStatement As String = "item1='" & Item1 & "', item2='" & Item2 & "', item3='" & Item3 & "', item4='" & Item4 & "' WHERE visitDate='" & VisitID & "' AND employeeNumber='" & AccountNumber & "' AND business='" & BusinessID & "'"
        Dim SQLUpdateString As String
        Dim SQLSelectString As String

        'Check which sale amount is being updated
        Select Case SaleNumber
            Case "2"
                SQLUpdateString = "UPDATE transactions SET saleAmount2='" & SaleAmount & "', transactionTimeStamp2='" & Timestamp & "', "
                SQLSelectString = "SELECT saleAmount2, transactionTimeStamp2 "
            Case "3"
                SQLUpdateString = "UPDATE transactions SET saleAmount3='" & SaleAmount & "', transactionTimeStamp3='" & Timestamp & "', "
                SQLSelectString = "SELECT saleAmount3, transactionTimeStamp3 "
            Case "4"
                SQLUpdateString = "UPDATE transactions SET saleAmount4='" & SaleAmount & "', transactionTimeStamp4='" & Timestamp & "', "
                SQLSelectString = "SELECT saleAmount4, transactionTimeStamp4 "
        End Select

        'Add the update string to the main SQL statement
        SQLMainUpdateStatement = SQLUpdateString & SQLMainUpdateStatement
        SQLMainSelectStatment = SQLSelectString & SQLMainSelectStatment

        'First check if a sale for that account number has been inserted into the transactions table already
        cmd = New SqlCommand
        cmd.Connection = con
        con.ConnectionString = connection_string
        con.Open()
        cmd.CommandText = SQLMainSelectStatment
        dr = cmd.ExecuteReader

        While dr.Read()
            If dr(0).ToString <> "0.00" Then
                ReturnStatement = "Has Rows"
                Exit While
            End If
        End While

        If ReturnStatement <> "Has Rows" Then
            dr.Close()
            cmd.Dispose()
            cmd.CommandText = SQLMainUpdateStatement
            cmd.ExecuteNonQuery()

            ReturnStatement = "Sale successful"
        End If


        dr.Close()
        cmd.Dispose()
        con.Close()
        Return ReturnStatement

    End Function

End Class
