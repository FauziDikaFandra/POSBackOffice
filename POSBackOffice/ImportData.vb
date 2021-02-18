Public Class ImportData
    Dim dsSlip, dsPayType, dsPaidAmount As DataSet
    Dim Ulg As Boolean
    Dim decPaidAmount As Decimal
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Button1.Enabled = False
    '    DateTimePicker1.Enabled = False
    '    ProgressBar1.Visible = True
    '    BackgroundWorker1.WorkerReportsProgress = True
    '    BackgroundWorker1.WorkerSupportsCancellation = True
    '    BackgroundWorker1.RunWorkerAsync()
    'End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim Prg As Decimal
        Prg = 0
        ProgressBar1.Value = 0

        dsSlip = getSqldb("Select * from slip where substring(trans_no,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'")
        If dsSlip.Tables(0).Rows.Count > 0 Then
            If MsgBox("Data Pada Tanggal '" & Format(DateTimePicker1.Value, "dd-MM-yyyy") & "' Sudah Ada !!!", MsgBoxStyle.YesNo, "Proses Ulang ??") = MsgBoxResult.Yes Then
                Ulg = True
                getSqldb("Delete From slip where substring(trans_no,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'")
                getSqldb("Delete From slip_pay where substring(trans_no,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'")
            Else
                GoTo 1
                Ulg = False
            End If
            getSqldb("insert into slip select SUBSTRING(a.Transaction_Number,1,17) + a.shift as trans_no, sum(a.Paid_Amount) as xread,sum(a.Paid_Amount) as realcash,0 as r_short,0 as r_over,max(b.Cashier_ID) as cashier_id,0 as cashier_pay,0 as cashier_bal,0 as r_depo,0 as r_cs  from paid a inner join Sales_Transactions b on a.Transaction_Number = b.Transaction_Number  where substring(a.Transaction_Number,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' Group By a.Shift,SUBSTRING(a.Transaction_Number,5,3), SUBSTRING(a.Transaction_Number,1,17) + a.shift")
        Else
            getSqldb("insert into slip select SUBSTRING(a.Transaction_Number,1,17) + a.shift as trans_no, sum(a.Paid_Amount) as xread,sum(a.Paid_Amount) as realcash,0 as r_short,0 as r_over,max(b.Cashier_ID) as cashier_id,0 as cashier_pay,0 as cashier_bal,0 as r_depo,0 as r_cs  from paid a inner join Sales_Transactions b on a.Transaction_Number = b.Transaction_Number  where substring(a.Transaction_Number,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' Group By a.Shift,SUBSTRING(a.Transaction_Number,5,3), SUBSTRING(a.Transaction_Number,1,17) + a.shift")
        End If
        dsSlip.Clear()
        dsSlip = getSqldb("Select Distinct trans_no from slip where substring(trans_no,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'")
        If dsSlip.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In dsSlip.Tables(0).Rows
                Prg += 100 / dsSlip.Tables(0).Rows.Count
                dsPayType = getSqldb("Select * from Payment_Types")
                BackgroundWorker1.ReportProgress(Int(Prg))
                If dsPayType.Tables(0).Rows.Count > 0 Then
                    For Each re As DataRow In dsPayType.Tables(0).Rows
                        dsPaidAmount = getSqldb("select SUBSTRING(a.Transaction_Number,1,17) + a.shift as trans_no,a.Payment_Types as payment_types,b.Description as description,b.Types as types,sum(a.Paid_Amount) as paid_amount from paid a inner join Payment_Types b on a.Payment_Types = b.Payment_Types where SUBSTRING(a.Transaction_Number,1,17) + a.shift  = '" & ro("trans_no") & "' And  b.Payment_Types = '" & re("Payment_Types") & "' Group By a.Shift,SUBSTRING(a.Transaction_Number,5,3), SUBSTRING(a.Transaction_Number,1,17) + a.shift,a.Payment_Types,b.Description,b.Types")
                        Dim cc As String = "select SUBSTRING(a.Transaction_Number,1,17) + a.shift as trans_no,a.Payment_Types as payment_types,b.Description as description,b.Types as types,sum(a.Paid_Amount) as paid_amount from paid a inner join Payment_Types b on a.Payment_Types = b.Payment_Types where SUBSTRING(a.Transaction_Number,1,17) + a.shift  = '" & ro("trans_no") & "' And  b.Payment_Types = '" & re("Payment_Types") & "' Group By a.Shift,SUBSTRING(a.Transaction_Number,5,3), SUBSTRING(a.Transaction_Number,1,17) + a.shift,a.Payment_Types,b.Description,b.Types"
                        cc = ""
                        If dsPaidAmount.Tables(0).Rows.Count > 0 Then
                            decPaidAmount = dsPaidAmount.Tables(0).Rows(0).Item("paid_amount")
                        Else
                            decPaidAmount = 0
                        End If
                        getSqldb("Insert Into slip_pay values ('" & ro("trans_no") & "','" & re("Payment_Types") & "','" & re("Description") & "','" & re("Types") & "','" & decPaidAmount & "')")
                    Next
                End If
            Next
        End If
1:
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Button1.Enabled = True
        DateTimePicker1.Enabled = True
        ProgressBar1.Visible = False
        'If Ulg = True Then
        getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Import Data','','Success','','" & Now & "')")
        MsgBox("Import Successfull !!")
        'End If

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Button1.Enabled = False
        DateTimePicker1.Enabled = False
        ProgressBar1.Visible = True
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub ImportData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Ulg = False
        CheckForIllegalCrossThreadCalls = False
    End Sub
End Class