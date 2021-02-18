Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Public Class Voucher_Receive
    Dim ds As New DataSet
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    Try
    '        ds = getSqldb("select * from v_Voucher_Receive where Store = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID").ToString.Trim & "' and Tgl = '" & Format(DateTimePicker1.Value, "ddMMyyy") & "' order by VNUMBER")
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            Dim cryRpt As New ReportDocument
    '            Dim printDoc As New PrintDocument
    '            cryRpt = New Voucher_ReceiveReport
    '            cryRpt.SetDataSource(ds.Tables(0))
    '            cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
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
        Try
            'ds = getSqldb("select * from v_Voucher_Receive where Store = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID").ToString.Trim & "' and Tgl = '" & Format(DateTimePicker1.Value, "ddMMyyy") & "' order by VNUMBER")
            ds = getSqldb("select * from v_Voucher_Receive where Tgl between '" & DateTimePicker1.Value.Date & "' And '" & DateTimePicker2.Value.Date & "' order by VNUMBER")
            If ds.Tables(0).Rows.Count > 0 Then
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New Voucher_ReceiveReport
                cryRpt.SetDataSource(ds.Tables(0))
                cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
                cryRpt.SetParameterValue("ParTgl", Format(DateTimePicker1.Value.Date, "dd / MMM / yyyy") & "   -   " & Format(DateTimePicker2.Value.Date, "dd / MMM / yyyy"))
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

    Private Sub Voucher_Receive_Report_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class