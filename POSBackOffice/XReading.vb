Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Imports System.Data.SqlClient
Imports System.IO
Public Class XReading
    Dim c As New ArrayList
    Dim ds1, dsShift, dsReg, dsCash, DsX, DsIP As New DataSet
    Dim Shift, Cashier_ID, Cashier_Name, Modalstr, IPReg As String
    Dim Jual, jumlah, diskon, retur, batal As Decimal
    Dim con As SqlConnection
    Dim cmd As SqlCommand
    Private Sub XReading_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        c.Add(New CCombo("1", "Close Shift"))
        c.Add(New CCombo("2", "Close Register"))
        With cmbxTyp
            .DataSource = c
            .DisplayMember = "Number_Name"
            .ValueMember = "ID"
        End With
        cmbReg.SelectedValue = "1"
        cmb(cmbReg, "Select Cash_Register_ID,Cash_Register_ID From Cash_Register Where Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' ", "Cash_Register_ID", "Cash_Register_ID", 1)
    End Sub

    Sub CloseShift()
        dsShift = getSqldb("Select * from  [" & IPReg & "].POS_LOCAL.dbo.Cash_Register Where Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and Cash_register_Id = '" & _
                    cmbReg.SelectedValue & "'")
        If dsShift.Tables(0).Rows.Count > 0 Then
            Shift = dsShift.Tables(0).Rows(0).Item("Shift")
        Else
            MsgBox("(1) Register tidak ditemukan / Offline")
            Exit Sub
        End If
        DsX = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Nilai, isnull(SUM(Total_discount),0) AS Potong " & _
             "FROM [" & IPReg & "].POS_LOCAL.dbo.Sales_Transactions WHERE Status = '00' and substring(transaction_number, 9,8)='" & Format(Now, "ddMMyyyy") & _
             "' and Transaction_Number in (select transaction_number from  [" & IPReg & "].POS_LOCAL.dbo.paid where Paid.Shift = '" & Shift & "') ")
        If DsX.Tables(0).Rows.Count > 0 Then
            Jual = DsX.Tables(0).Rows(0).Item("Nilai")
            diskon = DsX.Tables(0).Rows(0).Item("Potong")
        Else
            MsgBox("(2) Register tidak ditemukan / Offline")
            Exit Sub
        End If
        DsX.Clear()
        DsX = getSqldb("select sum(a.Paid_Amount) as Sales from  [" & IPReg & "].POS_LOCAL.dbo.Paid a left join " & _
                            " [" & IPReg & "].POS_LOCAL.dbo.payment_types b on a.Payment_Types = b.Payment_Types where " & _
                            "substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "' and shift = '" & Shift & "'")
        If DsX.Tables(0).Rows.Count > 0 Then
            If IsDBNull(DsX.Tables(0).Rows(0).Item("Sales")) Then
                MsgBox("(3) Tidak ada transaksi di shift " & Shift & " / Offline")
                Exit Sub
            Else
            End If
            jumlah = DsX.Tables(0).Rows(0).Item("Sales")
        Else
            MsgBox("(3) Register tidak ditemukan / Offline")
            Exit Sub
        End If
        DsX.Clear()
        DsX = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Balik " & _
             "FROM  [" & IPReg & "].POS_LOCAL.dbo.Sales_Transactions WHERE Flag_Return  = '1' and substring(transaction_number, 9,8)='" & Format(Now, "ddMMyyyy") & _
             "' and Transaction_Number in (select transaction_number from  [" & IPReg & "].POS_LOCAL.dbo.paid where Paid.Shift = '" & Shift & "') ")
        If DsX.Tables(0).Rows.Count > 0 Then
            retur = DsX.Tables(0).Rows(0).Item("Balik")
        Else
            MsgBox("(4) Register tidak ditemukan / Offline")
            Exit Sub
        End If
        DsX.Clear()
        DsX = getSqldb("SELECT isnull(SUM(Net_Price),0) AS Nilai " & _
             "FROM  [" & IPReg & "].POS_LOCAL.dbo.Sales_Transaction_Details WHERE substring(transaction_number, 9,8)='" & Format(Now, "ddMMyyyy") & _
             "' and Transaction_Number in (select transaction_number from  [" & IPReg & "].POS_LOCAL.dbo.paid where Paid.Shift =  '" & Shift & "' and flag_void='1') ")
        If DsX.Tables(0).Rows.Count > 0 Then
            batal = DsX.Tables(0).Rows(0).Item("Nilai")
        Else
            MsgBox("(5) Register tidak ditemukan / Offline")
            Exit Sub
        End If

        dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from  [" & IPReg & "].POS_LOCAL.dbo.Cash a left join  [" & IPReg & "].POS_LOCAL.dbo.Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
                    cmbReg.SelectedValue & "'")
        If dsCash.Tables(0).Rows.Count > 0 Then
            Cashier_ID = dsCash.Tables(0).Rows(0).Item("User_ID")
            Cashier_Name = dsCash.Tables(0).Rows(0).Item("User_Name")
            Modalstr = dsCash.Tables(0).Rows(0).Item("Modal")
        Else
            MsgBox("(6) Register tidak ditemukan / Offline")
            Exit Sub
        End If

        Try
            ds1.Clear()
            ds1 = getSqldb("select * from  [" & IPReg & "].POS_LOCAL.dbo.v_xreading where " & _
                            "periode = '" & Format(Now, "ddMMyyyy") & "' and shift = '" & Shift & "'")
            If ds1.Tables(0).Rows.Count > 0 Then
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New XRead
                cryRpt.SetDataSource(ds1.Tables(0))

                cryRpt.SetParameterValue("Ztype", cmbxTyp.Text)
                cryRpt.SetParameterValue("ShiftStat", Shift & " ONLINE")
                cryRpt.SetParameterValue("BranchName", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
                cryRpt.SetParameterValue("Reg", cmbReg.SelectedValue)

                cryRpt.SetParameterValue("Cashier", Trim(Cashier_ID) & " / " & Trim(Cashier_Name))
                cryRpt.SetParameterValue("Modal", Modalstr)
                cryRpt.SetParameterValue("Over", jumlah - Jual)
                cryRpt.SetParameterValue("XReading", Jual)

                cryRpt.SetParameterValue("Total", jumlah)
                cryRpt.SetParameterValue("Disc", diskon)
                cryRpt.SetParameterValue("Return", retur)
                cryRpt.SetParameterValue("Void", batal)

                If cmbxTyp.SelectedValue = "1" Then
                    getSqldb("update [" & IPReg & "].POS_LOCAL.dbo.cash_register set shift='2' WHERE Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & _
                      "' AND cash_register_id='" & cmbReg.SelectedValue & "'")
                    'Else
                    '    getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.Sales_transactions_backup where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    '    getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.Sales_transaction_details_backup where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    '    getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.PAID_backup where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")

                    '    getSqldb("Insert into  [" & IPReg & "].POS_LOCAL.dbo.Sales_transactions_backup select * from Sales_transactions where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    '    getSqldb("Insert into  [" & IPReg & "].POS_LOCAL.dbo.Sales_transaction_details_backup select * from Sales_transaction_details where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    '    getSqldb("Insert into  [" & IPReg & "].POS_LOCAL.dbo.PAID_backup select * from PAID where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")

                    '    If File.Exists("D:\POS_LOCAL_XREAD.bak") Then
                    '        File.Delete("D:\POS_LOCAL_XREAD.bak")
                    '    End If
                    '    con = New SqlConnection("Data Source=localhost;Integrated Security=SSPI;Initial Catalog=POS_LOCAL")
                    '    cmd = New SqlCommand("backup database POS_LOCAL to disk='D:\POS_LOCAL_XREAD.bak'", con)
                    '    con.Open()
                    '    cmd.ExecuteNonQuery()
                    '    con.Close()
                    '    'Label1.Text = "Back Up Is DONE !!!"
                    '    'MsgBox("Success !!!")

                    '    getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.Sales_transactions where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    '    getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.Sales_transaction_details where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    '    getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.PAID where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")

                End If


                PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
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

    Sub CloseRegister()
        DsX = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Nilai, isnull(SUM(Total_discount),0) AS Potong " & _
             "FROM [" & IPReg & "].POS_LOCAL.dbo.Sales_Transactions WHERE Status = '00' and substring(transaction_number, 9,8)='" & Format(Now, "ddMMyyyy") & "') ")
        If DsX.Tables(0).Rows.Count > 0 Then
            Jual = DsX.Tables(0).Rows(0).Item("Nilai")
            diskon = DsX.Tables(0).Rows(0).Item("Potong")
        Else
            MsgBox("(2) Register tidak ditemukan / Offline")
            Exit Sub
        End If
        DsX.Clear()
        DsX = getSqldb("select sum(a.Paid_Amount) as Sales from  [" & IPReg & "].POS_LOCAL.dbo.Paid a left join " & _
                            " [" & IPReg & "].POS_LOCAL.dbo.payment_types b on a.Payment_Types = b.Payment_Types where " & _
                            " substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
        If DsX.Tables(0).Rows.Count > 0 Then
            If IsDBNull(DsX.Tables(0).Rows(0).Item("Sales")) Then
                MsgBox("(3) Tidak ada transaksi di shift " & Shift & " / Offline")
                Exit Sub
            Else
            End If
            jumlah = DsX.Tables(0).Rows(0).Item("Sales")
        Else
            MsgBox("(3) Register tidak ditemukan / Offline")
            Exit Sub
        End If
        DsX.Clear()
        DsX = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Balik " & _
             "FROM  [" & IPReg & "].POS_LOCAL.dbo.Sales_Transactions WHERE Flag_Return  = '1' and substring(transaction_number, 9,8)='" & Format(Now, "ddMMyyyy") & "') ")
        If DsX.Tables(0).Rows.Count > 0 Then
            retur = DsX.Tables(0).Rows(0).Item("Balik")
        Else
            MsgBox("(4) Register tidak ditemukan / Offline")
            Exit Sub
        End If
        DsX.Clear()
        DsX = getSqldb("SELECT isnull(SUM(Net_Price),0) AS Nilai " & _
             "FROM  [" & IPReg & "].POS_LOCAL.dbo.Sales_Transaction_Details WHERE substring(transaction_number, 9,8)='" & Format(Now, "ddMMyyyy") & "' and flag_void='1') ")
        If DsX.Tables(0).Rows.Count > 0 Then
            batal = DsX.Tables(0).Rows(0).Item("Nilai")
        Else
            MsgBox("(5) Register tidak ditemukan / Offline")
            Exit Sub
        End If

        dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from  [" & IPReg & "].POS_LOCAL.dbo.Cash a left join  [" & IPReg & "].POS_LOCAL.dbo.Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
                    cmbReg.SelectedValue & "'")
        If dsCash.Tables(0).Rows.Count > 0 Then
            Cashier_ID = dsCash.Tables(0).Rows(0).Item("User_ID")
            Cashier_Name = dsCash.Tables(0).Rows(0).Item("User_Name")
            Modalstr = dsCash.Tables(0).Rows(0).Item("Modal")
        Else
            MsgBox("(6) Register tidak ditemukan / Offline")
            Exit Sub
        End If

        Try
            ds1.Clear()
            ds1 = getSqldb("select * from  [" & IPReg & "].POS_LOCAL.dbo.v_xreading where " & _
                            "periode = '" & Format(Now, "ddMMyyyy") & "'")
            If ds1.Tables(0).Rows.Count > 0 Then
                If cmbxTyp.SelectedValue <> "1" Then
                    '    getSqldb("update [" & IPReg & "].POS_LOCAL.dbo.cash_register set shift='1' WHERE Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & _
                    '      "' AND cash_register_id='" & cmbReg.SelectedValue & "'")
                    'Else
                    getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.Sales_transactions_backup where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.Sales_transaction_details_backup where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.PAID_backup where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")

                    getSqldb("Insert into  [" & IPReg & "].POS_LOCAL.dbo.Sales_transactions_backup select * from Sales_transactions where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    getSqldb("Insert into  [" & IPReg & "].POS_LOCAL.dbo.Sales_transaction_details_backup select * from Sales_transaction_details where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    getSqldb("Insert into  [" & IPReg & "].POS_LOCAL.dbo.PAID_backup select * from PAID where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")


                    'getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.Sales_transactions where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    'getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.Sales_transaction_details where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")
                    'getSqldb("Delete from  [" & IPReg & "].POS_LOCAL.dbo.PAID where substring(transaction_number,9,8) = '" & Format(Now, "ddMMyyyy") & "'")

                End If
                Try
                    If File.Exists("D:\POS_LOCAL_XREAD.bak") Then
                        File.Delete("D:\POS_LOCAL_XREAD.bak")
                    End If
                    con = New SqlConnection("Data Source=localhost;Integrated Security=SSPI;Initial Catalog=POS_LOCAL")
                    cmd = New SqlCommand("backup database POS_LOCAL to disk='D:\POS_LOCAL_XREAD.bak'", con)
                    con.Open()
                    cmd.ExecuteNonQuery()
                    con.Close()
                    'Label1.Text = "Back Up Is DONE !!!"
                    'MsgBox("Success !!!")
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try

                For x As Integer = 0 To 2
                    Dim cryRpt As New ReportDocument
                    Dim printDoc As New PrintDocument
                    cryRpt = New XRead
                    cryRpt.SetDataSource(ds1.Tables(0))

                    cryRpt.SetParameterValue("Ztype", cmbxTyp.Text)
                    cryRpt.SetParameterValue("ShiftStat", Shift & " ONLINE")
                    cryRpt.SetParameterValue("BranchName", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
                    cryRpt.SetParameterValue("Reg", cmbReg.SelectedValue)

                    cryRpt.SetParameterValue("Cashier", Trim(Cashier_ID) & " / " & Trim(Cashier_Name))
                    cryRpt.SetParameterValue("Modal", Modalstr)
                    cryRpt.SetParameterValue("Over", jumlah - Jual)
                    cryRpt.SetParameterValue("XReading", Jual)

                    cryRpt.SetParameterValue("Total", jumlah)
                    cryRpt.SetParameterValue("Disc", diskon)
                    cryRpt.SetParameterValue("Return", retur)
                    cryRpt.SetParameterValue("Void", batal)

                    PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
                    Reports.CrystalReportViewer2.ReportSource = cryRpt
                Next

                Reports.ShowDialog()
                Reports.TopMost = True
            Else
                MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
            End If
            getSqldb("exec [" & IPReg & "].POS_LOCAL.dbo.spp_ZresetLocal '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "', '" & cmbReg.SelectedValue & "', '" & Format(Now, "YYYY-MM-DD") & "'")
            getSqldb("exec [" & IPReg & "].POS_LOCAL.dbo.spp_ZresetServer '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "', '" & cmbReg.SelectedValue & "', '" & Format(Now, "YYYY-MM-DD") & "',''")
            getSqldb("exec [" & IPReg & "].POS_LOCAL.dbo.spp_DeleteTrans")

            getSqldb("exec spp_ZresetServer '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "', '" & cmbReg.SelectedValue & "', '" & Format(Now, "YYYY-MM-DD") & "',''")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        DsIP = getSqldb("Select IP_Register from IPRegister where  Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and Cash_Register_ID = '" & cmbReg.SelectedValue & "'")
        If DsIP.Tables(0).Rows.Count > 0 Then
            IPReg = Trim(DsIP.Tables(0).Rows(0).Item("IP_Register"))
        Else
            MsgBox("Data di Table IPRegister tidak lengkap / Register Offline!!!")
            Exit Sub
        End If

        If cmbxTyp.SelectedValue = "1" Then
            CloseShift()
        Else
            CloseRegister()
        End If


    End Sub

    Private Sub PrintReport(ByVal printerName As String, ByVal ReportDoc As ReportDocument)
        ReportDoc.PrintOptions.PrinterName = printerName
        ReportDoc.PrintToPrinter(1, False, 0, 0)

    End Sub
End Class