Imports System.Data.SqlClient
Public Class EndOfDay
    Dim ds, dsReg, dsCtrl As New DataSet
    Dim Process_Date, Server_Date As Date
    Dim eod_can_go, ProcessStatus As Integer

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If MsgBox("Process EOD ?", MsgBoxStyle.YesNo, "Attention") = MsgBoxResult.No Then
            Exit Sub
        End If
        ProgressBar1.Visible = True
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()

    End Sub

    Private Sub EndOfDay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Label1.Text = ""
        DSBranch = getSqldb2("Select * from Branches")
        'If DSBranch.Tables(0).Rows(0).Item("Flag_SOD") <> "0" Then
        '    Button1.Enabled = False
        'End If
        DateTimePicker2.Value = Now
        DateTimePicker1.Value = DSBranch.Tables(0).Rows(0).Item("Date_Current")
        Value(1) = DSBranch.Tables(0).Rows(0).Item("Branch_ID")
        Param(1) = "@branch_id"
        Param2(1) = "@Process_Date"
        Param2(2) = "@Server_Date"
        TypeP(1) = SqlDbType.SmallDateTime
        TypeP(2) = SqlDbType.SmallDateTime
        ds = SelProcOut("spp_GetProcessDate", 1, 2, 100)
        Process_Date = Value2(1)
        Server_Date = Value2(2)
        CheckForIllegalCrossThreadCalls = False
        Param2(1) = "@Synt"
        Param2(2) = "@Synt1"
        Param2(3) = "@Synt2"
        Param2(4) = "@Synt3"
        Param2(5) = "@Synt4"
        TypeP(1) = SqlDbType.VarChar
        TypeP(2) = SqlDbType.VarChar
        TypeP(3) = SqlDbType.VarChar
        TypeP(4) = SqlDbType.VarChar
        TypeP(5) = SqlDbType.VarChar
        ds = SelProcOut("spp_GetCashRegistersPivot", 0, 5, 8000)



        getSqldb2(Value2(2))
        getSqldb2(Value2(3))
        If Value2(4).ToString <> "" Then
            getSqldb2(Value2(4))
        End If
        If Value2(5).ToString <> "" Then
            getSqldb2(Value2(5))
        End If
        getSqldb2(Value2(1))
        dsReg = getSqldb2("Select * from CREG")
        If dsReg.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = dsReg.Tables(0)
            DataGridView1.Refresh()
        End If
        dsCtrl = getSqldb2("Select Status,description,execution_time from Transfer_Control where eod_flag = 1 Order By process_order")
        If dsCtrl.Tables(0).Rows.Count > 0 Then
            DataGridView2.DataSource = dsCtrl.Tables(0)
            DataGridView2.Refresh()
        End If
        Button1.Focus()
    End Sub

    Private Sub setLabelTxt(ByVal text As String, ByVal lbl As Label)
        If lbl.InvokeRequired Then
            lbl.Invoke(New setLabelTxtInvoker(AddressOf setLabelTxt), text, lbl)
        Else
            lbl.Text = text
        End If
    End Sub

    Private Delegate Sub setLabelTxtInvoker(ByVal text As String, ByVal lbl As Label)

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim Prg As Decimal
        Prg = 0
        ProgressBar1.Value = 0

        Prg += 10
        BackgroundWorker1.ReportProgress(Int(Prg))
        Try
            setLabelTxt("GetProcessStatus", Label1)
        Catch ex As Exception

        End Try

        Value(1) = "EXTRACT_ARRANGEMENT"
        Param(1) = "@ProcessName"
        Param2(1) = "@status"
        TypeP(1) = SqlDbType.TinyInt
        Try
            ds = SelProcOut("spp_EODbranch_GetProcessStatus", 1, 1, 100)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        ProcessStatus = Value2(1)


        Prg += 10
        BackgroundWorker1.ReportProgress(Int(Prg))
        Try
            setLabelTxt("GetZResetStatus", Label1)
        Catch ex As Exception

        End Try
        Param2(1) = "@msg"
        Param2(2) = "@eod_can_go"
        TypeP(1) = SqlDbType.VarChar
        TypeP(2) = SqlDbType.TinyInt
        Try
            ds = SelProcOut("spp_GetZResetStatus", 0, 2, 100)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        If Value2(1).ToString <> "" Then
            MsgBox(Value2(1), MsgBoxStyle.Information, "Information")
        End If
        eod_can_go = Value2(2)

        Prg += 10
        BackgroundWorker1.ReportProgress(Int(Prg))
        Try
            setLabelTxt("SaveHistory_SALES_TRANSACTION_DETAILS", Label1)
        Catch ex As Exception

        End Try

        Try
            ds = SelProcOut("spp_EODBranch_SaveHistory_SALES_TRANSACTION_DETAILS", 0, 0, 100)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Prg += 10
        BackgroundWorker1.ReportProgress(Int(Prg))
        Try
            setLabelTxt("SetDate", Label1)
        Catch ex As Exception

        End Try
        Value(1) = DSBranch.Tables(0).Rows(0).Item("Branch_ID")
        Value(2) = Process_Date
        Param(1) = "@branch_id"
        Param(2) = "@EOD_Date"

        Try
            ds = SelProcOut("spp_EODbranch_SetDate", 2, 0, 100)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Prg += 10
        BackgroundWorker1.ReportProgress(Int(Prg))
        Try
            setLabelTxt("SaveHistory_CASH", Label1)
        Catch ex As Exception

        End Try

        Try
            ds = SelProcOut("spp_EODBranch_SaveHistory_CASH", 0, 0, 100)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Prg += 10
        BackgroundWorker1.ReportProgress(Int(Prg))
        Try
            setLabelTxt("SaveHistory_SALES_TRANSACTIONS", Label1)
        Catch ex As Exception

        End Try

        Try
            ds = SelProcOut("spp_EODBranch_SaveHistory_SALES_TRANSACTIONS", 0, 0, 100)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try


        Prg += 10
        BackgroundWorker1.ReportProgress(Int(Prg))
        Try
            setLabelTxt("SaveHistory_PAID", Label1)
        Catch ex As Exception

        End Try

        Try
            ds = SelProcOut("spp_EODBranch_SaveHistory_PAID", 0, 0, 100)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
        Prg += 10
        BackgroundWorker1.ReportProgress(Int(Prg))
        'Balikin zReset Jadi 0 lagi
        Try
            setLabelTxt("SetEODComplete", Label1)
        Catch ex As Exception

        End Try
        Value(1) = DSBranch.Tables(0).Rows(0).Item("Branch_ID")
        Value(2) = Process_Date
        Param(1) = "@branch_id"
        Param(2) = "@eod_date"

        Try
            ds = SelProcOut("spp_EODBranch_SetEODComplete", 2, 0, 100)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Try
            ds = SelProcSer("spp_EOD_Bali", 0)
        Catch ex As Exception
            MsgBox("Sync to HO Failed !!!")
        End Try


        Prg += 10
        BackgroundWorker1.ReportProgress(Int(Prg))
        getSqldb2("Update Transfer_Control set execution_time = '" & FormatDateTime(Now, DateFormat.ShortTime) & " - " & FormatDateTime(Now, DateFormat.ShortTime) & "',status = 1")
        Prg += 10
        BackgroundWorker1.ReportProgress(Int(Prg))
        Try
            setLabelTxt("Done !!!", Label1)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        'MsgBox("EOD Successfull !!!!!")
        ProgressBar1.Visible = False
        dsReg = getSqldb2("Select * from CREG")
        If dsReg.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = dsReg.Tables(0)
            DataGridView1.Refresh()
        End If
        dsCtrl = getSqldb2("Select Status,description,execution_time from Transfer_Control where eod_flag = 1 Order By process_order")
        If dsCtrl.Tables(0).Rows.Count > 0 Then
            DataGridView2.DataSource = dsCtrl.Tables(0)
            DataGridView2.Refresh()
        End If
        getSqldb2("Update Transfer_Control set execution_time = NULL,status = 0")
        getSqldb("Insert into Back_Office_Log values ('" & UserName & "','EOD','','Success','','" & Now & "')")
        MsgBox("EOD Done !!!")
        Button1.Focus()
    End Sub
End Class