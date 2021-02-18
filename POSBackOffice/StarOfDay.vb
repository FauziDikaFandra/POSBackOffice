Public Class StarOfDay
    Dim ds As New DataSet
    Dim dsCtrl As New DataSet
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If MsgBox("Process SOD ?", MsgBoxStyle.YesNo, "Attention") = MsgBoxResult.No Then
            Exit Sub
        End If
        ProgressBar1.Visible = True
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub StarOfDay_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'GroupBox1.Enabled = True
        'DSBranch = getSqldb2("Select * from Branches")
        'If DSBranch.Tables(0).Rows(0).Item("Flag_SOD") <> "1" Then
        '    GroupBox1.Enabled = False
        'End If

        dsCtrl = getSqldb2("Select Status,description,execution_time from Transfer_Control where eod_flag = 0 Order By process_order")
        If dsCtrl.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = dsCtrl.Tables(0)
            DataGridView1.Refresh()
        End If
        Button1.Focus()
        DateTimePicker2.Value = Now
        DateTimePicker1.Value = DSBranch.Tables(0).Rows(0).Item("Date_Current")
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim Prg As Decimal
        Prg = 0
        ProgressBar1.Value = 0

        Prg += 25
        BackgroundWorker1.ReportProgress(Int(Prg))
        Value(1) = DSBranch.Tables(0).Rows(0).Item("Branch_ID")
        Value(2) = DateTimePicker1.Value.Date
        Param(1) = "@branch_id"
        Param(2) = "@EOD_Date"
        ds = SelProcSer("spp_SODbranch_SetDate", 2)

        Prg += 25
        BackgroundWorker1.ReportProgress(Int(Prg))
        Value(1) = DateTimePicker2.Value.Date
        Value(2) = usrID
        Param(1) = "@last_sod_date"
        Param(2) = "@last_sod_by"
        ds = SelProcSer("spp_SODBRANCH_UpdateLastInfo", 2)

        Prg += 25
        BackgroundWorker1.ReportProgress(Int(Prg))
        Value(1) = DateTimePicker1.Value.Date
        Value(2) = DSBranch.Tables(0).Rows(0).Item("Branch_ID")
        Param(1) = "@eod_date"
        Param(2) = "@branch_id"
        ds = SelProcSer("spp_SODBRANCH_UpdateItemMaster", 2)

        Prg += 25
        BackgroundWorker1.ReportProgress(Int(Prg))
        Try
            ds = SelProcSer("spp_SOD_Bali", 0)
        Catch ex As Exception
            MsgBox("Sync to HO Failed !!!")
        End Try


        getSqldb2("Update Transfer_Control set execution_time = '" & FormatDateTime(Now, DateFormat.ShortTime) & " - " & FormatDateTime(Now, DateFormat.ShortTime) & "',status = 1")
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        ProgressBar1.Visible = False
        dsCtrl = getSqldb2("Select Status,description,execution_time from Transfer_Control where eod_flag = 0 Order By process_order")
        If dsCtrl.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = dsCtrl.Tables(0)
            DataGridView1.Refresh()
        End If
        getSqldb2("Update Transfer_Control set execution_time = NULL,status = 0")
        DSBranch = getSqldb2("Select * from Branches")
        DateTimePicker1.Value = DSBranch.Tables(0).Rows(0).Item("Date_Current")
        MsgBox("SOD Done !!!")
        Button1.Focus()
    End Sub
End Class