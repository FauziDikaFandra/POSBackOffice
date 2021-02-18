
Public Class SalesReports
    Dim ds As New DataSet
    Private Sub SalesReports_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadioButton1.Checked = True
    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        If RadioButton1.Checked = True Then
            Label5.Text = "From : "
            Label1.Visible = True
            DateTimePicker2.Visible = True
        Else
            Label5.Text = "Date : "
            Label1.Visible = False
            DateTimePicker2.Visible = False
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        If RadioButton1.Checked = True Then
            Label5.Text = "From : "
            Label1.Visible = True
            DateTimePicker2.Visible = True
        Else
            Label5.Text = "Date : "
            Label1.Visible = False
            DateTimePicker2.Visible = False
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If RadioButton1.Checked Then
                ds.Clear()
                ds = getSqldb2("select * from v_Sales where Transaction_Date between '" & DateTimePicker1.Value.Date & "' And '" & DateTimePicker2.Value.Date & "'")
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim cryRpt2 As New ReportDocument
                    cryRpt2 = New SalesReport
                    cryRpt2.SetDataSource(ds.Tables(0))
                    'cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
                    Reports.CrystalReportViewer2.ReportSource = cryRpt2
                    Reports.ShowDialog()
                    Reports.TopMost = True
                Else
                    MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
                End If
            Else
                ds.Clear()
                ds = getSqldb2("exec uspSalesByBrand '" & DateTimePicker1.Value.Date & "'")
                If ds.Tables(0).Rows.Count > 0 Then
                    Dim cryRpt2 As New ReportDocument
                    cryRpt2 = New SalesBrand
                    cryRpt2.SetDataSource(ds.Tables(0))
                    cryRpt2.SetParameterValue("Date", DateTimePicker1.Value.Date)
                    Reports.CrystalReportViewer2.ReportSource = cryRpt2
                    Reports.ShowDialog()
                    Reports.TopMost = True
                Else
                    MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
                End If
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class