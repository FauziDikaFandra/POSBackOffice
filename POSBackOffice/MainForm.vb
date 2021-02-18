Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Public Class MainForm
    Dim Cls As Boolean
    Dim dsAcc As DataSet
    Private Sub LogMailToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LogMailToolStripMenuItem.Click
        Cls = False
        LoginForm.Show()
        LoginForm.TextBox2.Focus()
        Me.Close()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        LoginForm.Close()
        Me.Close()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ToolStripStatusLabel5.Text = Now.ToString
    End Sub

    Private Sub MainForm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        If Cls = True Then
            LoginForm.Close()
        End If

    End Sub

    Private Sub MainForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        'If e.KeyCode = Keys.Escape Then
        '    Me.Close()
        'End If
    End Sub

    Private Sub MainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Cls = True
        ToolStripStatusLabel2.Spring = True
        ToolStripStatusLabel4.Spring = True
        ToolStripStatusLabel1.Text = usrID & " - " & UserName
        ToolStripStatusLabel3.Text = m_ServerName
        Timer1.Enabled = True
        dsAcc = getSqldb("Select * from User_Groups_Form where Group_ID = '" & Group_ID & "'")
        If dsAcc.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In dsAcc.Tables(0).Rows
                For Each mnuitem As ToolStripMenuItem In MenuStrip1.Items
                    If mnuitem.Name = ro("Group_Form").ToString Then
                        mnuitem.Visible = False
                        GoTo 1
                    End If
                    For Each mnuitemdetail As ToolStripMenuItem In mnuitem.DropDownItems
                        If mnuitemdetail.Name = ro("Group_Form").ToString Then
                            mnuitemdetail.Visible = False
                            GoTo 2
                        End If
                        For Each mnuitemdetail2 As ToolStripMenuItem In mnuitemdetail.DropDownItems
                            If mnuitemdetail2.Name = ro("Group_Form").ToString Then
                                mnuitemdetail2.Visible = False
                                GoTo 3
                            End If
                            For Each mnuitemdetail3 As ToolStripMenuItem In mnuitemdetail2.DropDownItems
                                If mnuitemdetail3.Name = ro("Group_Form").ToString Then
                                    mnuitemdetail3.Visible = False
                                    GoTo 4
                                End If
                            Next
4:
                        Next
3:
                    Next
2:
                Next
1:
            Next
        End If

        'Try
        '    Dim ds As New DataSet
        '    'ds = getSqldb("select * from v_Voucher_Receive where Store = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID").ToString.Trim & "' and Tgl = '" & Format(DateTimePicker1.Value, "ddMMyyy") & "' order by VNUMBER")
        '    ds = getSqldb("select top 1 * from v_Voucher_Receive order by VNUMBER")
        '    If ds.Tables(0).Rows.Count > 0 Then
        '        Dim cryRpt As New ReportDocument
        '        cryRpt = New Voucher_ReceiveReport
        '        cryRpt.SetDataSource(ds.Tables(0))
        '        cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
        '        cryRpt.SetParameterValue("ParTgl", Format(Now.Date, "dd / MMM / yyyy") & "   -   " & Format(Now.Date, "dd / MMM / yyyy"))
        '        Reports.CrystalReportViewer2.ReportSource = cryRpt
        '    End If
        'Catch ex As Exception

        'End Try
    End Sub

    Private Sub SendMailToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendMailToolStripMenuItem.Click
        ChangePassword.MdiParent = Me
        ChangePassword.Show()
    End Sub

    Private Sub TransPointReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TransPointReportToolStripMenuItem.Click
        ReportPointTrans.MdiParent = Me
        ReportPointTrans.Show()
    End Sub

    Private Sub StarOfDayToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StarOfDayToolStripMenuItem.Click
        StarOfDay.MdiParent = Me
        StarOfDay.Show()
    End Sub

    Private Sub EndOfDayToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EndOfDayToolStripMenuItem.Click
        EndOfDay.MdiParent = Me
        EndOfDay.Show()
    End Sub

    Private Sub ImportDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImportDataToolStripMenuItem.Click
        ImportData.MdiParent = Me
        ImportData.Show()
    End Sub

    Private Sub MaintenanceDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MaintenanceDataToolStripMenuItem.Click
        MaintenanceData.MdiParent = Me
        MaintenanceData.Show()
    End Sub

    Private Sub DiscrepancySalesDataToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiscrepancySalesDataToolStripMenuItem.Click
        Filtering_Discrepancy_Sales_Data.MdiParent = Me
        Filtering_Discrepancy_Sales_Data.Show()
    End Sub

    Private Sub CashRegisterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CashRegisterToolStripMenuItem.Click
        Cash_Register_Report.MdiParent = Me
        Cash_Register_Report.Show()
    End Sub

    Private Sub SalesMaintenanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SalesMaintenanceToolStripMenuItem.Click
        FloorOrHistory.MdiParent = Me
        FloorOrHistory.Show()
    End Sub

    Private Sub TransDiscBySTARReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TransDiscBySTARReportToolStripMenuItem.Click
        ReportDiscByStar.MdiParent = Me
        ReportDiscByStar.Show()
    End Sub

    Private Sub PemeliharaanVoucherToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PemeliharaanVoucherToolStripMenuItem.Click
        Pemeliharaan_Voucher.MdiParent = Me
        Pemeliharaan_Voucher.Show()
    End Sub

    Private Sub ShortOverCashierReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShortOverCashierReportToolStripMenuItem.Click
        ShortOverCashierReport.MdiParent = Me
        ShortOverCashierReport.Show()
    End Sub

    Private Sub VoucherReceiveToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VoucherReceiveToolStripMenuItem.Click
        Voucher_Receive.MdiParent = Me
        Voucher_Receive.Show()
    End Sub

    Private Sub ViewVoucherSoldToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewVoucherSoldToolStripMenuItem.Click
        ViewVoucherSold.MdiParent = Me
        ViewVoucherSold.Show()
    End Sub

    Private Sub SellingGiftVoucherToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SellingGiftVoucherToolStripMenuItem.Click
        SellingGiftVoucher.MdiParent = Me
        SellingGiftVoucher.Show()
    End Sub

    Private Sub CancelSellingGiftVoucherToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelSellingGiftVoucherToolStripMenuItem.Click
        CancelSellingGiftVoucher.MdiParent = Me
        CancelSellingGiftVoucher.Show()
    End Sub

    Private Sub VoucherSellingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VoucherSellingToolStripMenuItem.Click
        VoucherSellingReport.MdiParent = Me
        VoucherSellingReport.Show()
    End Sub

    Private Sub PemeliharToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PemeliharToolStripMenuItem.Click
        PemeliharaanCashReg.MdiParent = Me
        PemeliharaanCashReg.Show()
    End Sub

    Private Sub UserMaintenanceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UserMaintenanceToolStripMenuItem.Click
        UserMaintenance.MdiParent = Me
        UserMaintenance.Show()
    End Sub

    Private Sub UserPOSMaintananceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UserPOSMaintananceToolStripMenuItem.Click
        UserPOSFrontOffice.MdiParent = Me
        UserPOSFrontOffice.Show()
    End Sub

    Private Sub GenerateSalesToSAPToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GenerateSalesToSAPToolStripMenuItem.Click
        GenerateSalesToSAP.MdiParent = Me
        GenerateSalesToSAP.Show()
    End Sub

    Private Sub ViewTransacToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewTransacToolStripMenuItem.Click
        ViewTransaction.MdiParent = Me
        ViewTransaction.Show()
    End Sub

    Private Sub SettingVoucherToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingVoucherToolStripMenuItem.Click
        Setting_Voucher.MdiParent = Me
        Setting_Voucher.Show()
    End Sub

    Private Sub GenerateSalesToSAPToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GenerateSalesToSAPToolStripMenuItem1.Click
        GenerateSalesToSAP.MdiParent = Me
        GenerateSalesToSAP.Show()
    End Sub

    Private Sub UpdateVoucherDescToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Voucher_Desc.MdiParent = Me
        Voucher_Desc.Show()
    End Sub

    Private Sub XReadingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles XReadingToolStripMenuItem.Click
        X_ZReset.MdiParent = Me
        X_ZReset.Show()
    End Sub

    Private Sub TransRoundReportToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TransRoundReportToolStripMenuItem.Click
        ReportRoundingTrans.MdiParent = Me
        ReportRoundingTrans.Show()
    End Sub

    Private Sub ZXAppToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZXAppToolStripMenuItem.Click
        X_ZResetApproval.MdiParent = Me
        X_ZResetApproval.Show()
    End Sub



    Private Sub RapidReaderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RapidReaderToolStripMenuItem.Click
        'Form1.MdiParent = Me
        'Form1.Show()
    End Sub

    Private Sub ZebraReaderToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ZebraReaderToolStripMenuItem.Click
        ZebraSetting.MdiParent = Me
        ZebraSetting.Show()
    End Sub

    Private Sub PriceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles PriceToolStripMenuItem.Click
        Set_Price.MdiParent = Me
        Set_Price.Show()
    End Sub

    Private Sub DownloadItemToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DownloadItemToolStripMenuItem.Click
        Print_Barcode_RFID.MdiParent = Me
        Print_Barcode_RFID.Show()
    End Sub

    Private Sub RFIDActivationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RFIDActivationToolStripMenuItem.Click
        Activation_Zebra.MdiParent = Me
        Activation_Zebra.Show()
    End Sub

    Private Sub Activate2ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles Activate2ToolStripMenuItem.Click
        'Activation_Rapid.MdiParent = Me
        'Activation_Rapid.Show()
        System.Diagnostics.Process.Start("RapidRFID.exe")
    End Sub

    Private Sub InventoryTransferToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles InventoryTransferToolStripMenuItem.Click
        inventoryMovement.MdiParent = Me
        inventoryMovement.Show()
    End Sub

    Private Sub UserFingerToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles UserFingerToolStripMenuItem.Click
        FingerRegistration.MdiParent = Me
        FingerRegistration.Show()
    End Sub

    Private Sub DailySalesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DailySalesToolStripMenuItem.Click
        DailySales.MdiParent = Me
        DailySales.Show()
    End Sub

    Private Sub SyncDataToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SyncDataToolStripMenuItem.Click
        SyncData.MdiParent = Me
        SyncData.Show()
    End Sub

    Private Sub SalesReportsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SalesReportsToolStripMenuItem.Click
        System.Diagnostics.Process.Start("WindowsApplication1.exe")
    End Sub

    Private Sub SOToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SOToolStripMenuItem.Click
        Stock_Opname.MdiParent = Me
        Stock_Opname.Show()
    End Sub
End Class