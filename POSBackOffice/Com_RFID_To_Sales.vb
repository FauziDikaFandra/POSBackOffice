Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Public Class Com_RFID_To_Sales
    Dim ds As New DataSet
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ds.Clear()
        ds = getSqldb("delete from Com_Sales")
        ds = getSqldb("Insert into Com_Sales select a.transaction_number,Cashier_ID,transaction_date, Cash_Register_ID,b.PLU,d.Long_Description,b.Price,b.Qty,c.EPC,d.Brand   " &
                          "From sales_transactions a inner Join sales_transaction_details b on a.transaction_number = b.transaction_number " &
                          "left join [POS_SERVER].dbo.Item_Master_RFID c on b.Epc_Code = c.TID inner join Item_Master d on b.PLU = d.plu where a.status = '00' " &
                          "And CONVERT(date,'" & DateTimePicker1.Value.Date & "') = Transaction_Date and Burui not in ('NMD31ZZZ9') " &
                          "order by a.transaction_number, b.PLU")

        ds.Clear()
        Try
            ds.Clear()
            ds = getSqldb("select * from v_ComRfidSales Order By Tag_PLU")
            If ds.Tables(0).Rows.Count > 0 Then
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New RptCom
                cryRpt.SetDataSource(ds.Tables(0))
                'PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
                Reports.CrystalReportViewer2.ReportSource = cryRpt
                Reports.ShowDialog()
                Reports.TopMost = True
            Else
                MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
            End If

        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Com_RFID_To_Sales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DateTimePicker1.Value = DateTimePicker1.Value.AddDays(-1)
    End Sub
End Class