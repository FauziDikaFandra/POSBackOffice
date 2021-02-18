Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Public Class VoucherSellingReport
    Dim dsCek, ds As New DataSet
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    getSqldb("IF  NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'SellingReport' AND type = 'U') " & _
    '             "CREATE TABLE [dbo].[SellingReport](Tgl Int,GV10 Int,Sepuluh Decimal (25,0),GV25 Int,DuaLima " & _
    '             "Decimal (25,0),GV50 Int,LimaPuluh Decimal (25,0),GV100 Int,Seratus Decimal (25,0),GV500 Int," & _
    '             "LimaRatus Decimal (25,0),GV1000 Int,Seribu Decimal (25,0),Total Decimal (25,0)) ON [PRIMARY]")
    '    getSqldb("delete from SellingReport")
    '    'Dim daysLeftInMonth As TimeSpan = DateTimePicker1.Value.Subtract(DateTimePicker1.Value)
    '    'daysLeftInMonth = daysLeftInMonth.Days.ToString 
    '    Dim TotDays As Integer = System.DateTime.DaysInMonth(DateTimePicker1.Value.Year, DateTimePicker1.Value.Month)
    '    For x2 As Integer = 1 To TotDays
    '        getSqldb("insert into SellingReport Values  ('" & x2 & "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL)")
    '    Next
    '    dsCek = getSqldb("select Day(a.V_SELL) As Tgl,Count(V_AMT) As Cnt,SUM(a.V_AMT) As Amount,a.V_AMT,(select SUM(b.V_AMT) As Total From [newvoc] b where Month(b.V_SELL) " & _
    '                     "= '" & DateTimePicker1.Value.Month & "' and Year(b.V_SELL) = '" & DateTimePicker1.Value.Year & "' And b.v_ref in (select L_REF  from lain where month(l_tgl) = '" & DateTimePicker1.Value.Month & "' and " & _
    '                     "year(l_tgl) = '" & DateTimePicker1.Value.Year & "')  And Day(b.V_SELL) = Day(a.V_SELL)) As Total From [newvoc] a where Month(a.V_SELL) = '" & DateTimePicker1.Value.Month & "' " & _
    '                     "and Year(a.V_SELL) = '" & DateTimePicker1.Value.Year & "' And a.v_ref in (select L_REF  from lain where month(l_tgl) = '" & DateTimePicker1.Value.Month & "' and " & _
    '                     "year(l_tgl) = '" & DateTimePicker1.Value.Year & "')  Group By Day(a.V_SELL),a.V_AMT,a.V_SELL Order By Day(a.V_SELL)")
    '    If dsCek.Tables(0).Rows.Count > 0 Then
    '        For Each ro As DataRow In dsCek.Tables(0).Rows
    '            Select Case ro("V_AMT")
    '                Case 10000
    '                    getSqldb("Update SellingReport Set GV10 = '" & ro("Cnt") & "',Sepuluh = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
    '                Case 25000
    '                    getSqldb("Update SellingReport Set GV25 = '" & ro("Cnt") & "',DuaLima = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
    '                Case 50000
    '                    getSqldb("Update SellingReport Set GV50 = '" & ro("Cnt") & "',LimaPuluh = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
    '                Case 100000
    '                    getSqldb("Update SellingReport Set GV100 = '" & ro("Cnt") & "',Seratus = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
    '                Case 500000
    '                    getSqldb("Update SellingReport Set GV500 = '" & ro("Cnt") & "',LimaRatus = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
    '                Case 1000000
    '                    getSqldb("Update SellingReport Set GV1000 = '" & ro("Cnt") & "',Seribu = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
    '            End Select
    '        Next
    '    End If

    '    Try
    '        ds = getSqldb("select * from SellingReport Order By tgl")
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            Dim cryRpt As New ReportDocument
    '            Dim printDoc As New PrintDocument
    '            cryRpt = New VoucherSellingRpt
    '            cryRpt.SetDataSource(ds.Tables(0))
    '            cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
    '            cryRpt.SetParameterValue("Tgl", Format(DateTimePicker1.Value, "MMMM yyyy"))
    '            'PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
    '            Reports.CrystalReportViewer2.ReportSource = cryRpt
    '            Reports.ShowDialog()
    '            Reports.TopMost = True
    '        Else
    '            MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
    '        End If
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        getSqldb("IF  NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'SellingReport' AND type = 'U') " & _
                 "CREATE TABLE [dbo].[SellingReport](Tgl Int,GV10 Int,Sepuluh Decimal (25,0),GV25 Int,DuaLima " & _
                 "Decimal (25,0),GV30 Int,TigaPuluh Decimal (25,0),GV50 Int,LimaPuluh Decimal (25,0),GV60 Int,EnamPuluh Decimal (25,0),GV75 Int," & _
                 "TujuhLima Decimal (25,0),GV100 Int,Seratus Decimal (25,0),Total Decimal (25,0)) ON [PRIMARY]")
        getSqldb("delete from SellingReport")
        'Dim daysLeftInMonth As TimeSpan = DateTimePicker1.Value.Subtract(DateTimePicker1.Value)
        'daysLeftInMonth = daysLeftInMonth.Days.ToString 
        Dim TotDays As Integer = System.DateTime.DaysInMonth(DateTimePicker1.Value.Year, DateTimePicker1.Value.Month)
        For x2 As Integer = 1 To TotDays
            getSqldb("insert into SellingReport Values  ('" & x2 & "',NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL)")
        Next
        dsCek = getSqldb("select Day(a.V_SELL) As Tgl,Count(V_AMT) As Cnt,SUM(a.V_AMT) As Amount,a.V_AMT,(select SUM(b.V_AMT) As Total From [newvoc] b where Month(b.V_SELL) " & _
                         "= '" & DateTimePicker1.Value.Month & "' and Year(b.V_SELL) = '" & DateTimePicker1.Value.Year & "' And b.v_ref in (select L_REF  from lain where month(l_tgl) = '" & DateTimePicker1.Value.Month & "' and " & _
                         "year(l_tgl) = '" & DateTimePicker1.Value.Year & "')  And Day(b.V_SELL) = Day(a.V_SELL)) As Total From [newvoc] a where Month(a.V_SELL) = '" & DateTimePicker1.Value.Month & "' " & _
                         "and Year(a.V_SELL) = '" & DateTimePicker1.Value.Year & "' And a.v_ref in (select L_REF  from lain where month(l_tgl) = '" & DateTimePicker1.Value.Month & "' and " & _
                         "year(l_tgl) = '" & DateTimePicker1.Value.Year & "')  Group By Day(a.V_SELL),a.V_AMT,a.V_SELL Order By Day(a.V_SELL)")
        If dsCek.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In dsCek.Tables(0).Rows
                Select Case ro("V_AMT")
                    Case 10000
                        getSqldb("Update SellingReport Set GV10 = '" & ro("Cnt") & "',Sepuluh = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
                    Case 25000
                        getSqldb("Update SellingReport Set GV25 = '" & ro("Cnt") & "',DuaLima = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
                    Case 30000
                        getSqldb("Update SellingReport Set GV30 = '" & ro("Cnt") & "',TigaPuluh = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
                    Case 50000
                        getSqldb("Update SellingReport Set GV50 = '" & ro("Cnt") & "',LimaPuluh = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
                    Case 60000
                        getSqldb("Update SellingReport Set GV60 = '" & ro("Cnt") & "',EnamPuluh = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
                    Case 75000
                        getSqldb("Update SellingReport Set GV75 = '" & ro("Cnt") & "',TujuhLima = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
                    Case 100000
                        getSqldb("Update SellingReport Set GV100 = '" & ro("Cnt") & "',Seratus = '" & ro("Amount") & "',Total = '" & ro("Total") & "' Where Tgl = '" & ro("Tgl") & "'")
                End Select
            Next
        End If

        Try
            ds = getSqldb("select * from SellingReport Order By tgl")
            If ds.Tables(0).Rows.Count > 0 Then
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New VoucherSellingRpt
                cryRpt.SetDataSource(ds.Tables(0))
                cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
                cryRpt.SetParameterValue("Tgl", Format(DateTimePicker1.Value, "MMMM yyyy"))
                'PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
                Reports.CrystalReportViewer2.ReportSource = cryRpt
                Reports.ShowDialog()
                Reports.TopMost = True
            Else
                MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class