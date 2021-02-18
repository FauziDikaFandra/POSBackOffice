Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Imports System.Data.SqlClient
Imports System.IO
Imports System.Text
Public Class X_ZReset
    Dim c, c2 As New ArrayList
    Dim ds, dsShift, dsCash, dsVouc, dsRetur, dsx As New DataSet
    Dim DB, Path, Shift, Cashier_ID, Cashier_Name, Modalstr As String
    Dim Jual, jumlah, diskon, retur, batal As Decimal

    Private Sub X_ZReset_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        c.Add(New CCombo("1", "X-Reading"))
        c.Add(New CCombo("2", "Z-Reset"))
        With cmbxTyp
            .DataSource = c
            .DisplayMember = "Number_Name"
            .ValueMember = "ID"
        End With
        cmbxTyp.SelectedValue = "1"
        cmb(cmbReg, "Select Cash_Register_ID,Cash_Register_ID From Cash_Register Where Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' ", "Cash_Register_ID", "Cash_Register_ID", 1)
        cmbReg.SelectedIndex = 0
        c2.Add(New CCombo("1", "1"))
        c2.Add(New CCombo("2", "2"))
        With ComboBox1
            .DataSource = c2
            .DisplayMember = "Number_Name"
            .ValueMember = "ID"
        End With
        ComboBox1.SelectedValue = "1"
    End Sub

    Sub CloseShiftDB()
        Path = "D:\X-Reading_" & Format(DateTimePicker1.Value, "ddMMyyyy") & ".txt"
        If File.Exists(Path) Then
            File.Delete(Path)
        End If
        If Not File.Exists(Path) Then
            ' Create a file to write to. 
            Using sw As StreamWriter = File.CreateText(Path)
                dsShift = getSqldb("Select * from  [POS_SERVER].dbo.Cash_Register Where Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and Cash_register_Id = '" & _
                    cmbReg.SelectedValue & "'")
                If dsShift.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("X-Reading Shift: " & ComboBox1.SelectedValue & " On-Line")
                Else
                    MsgBox("(1) Register " & cmbReg.SelectedValue & " Tidak Ada Ditable [Cash_Register]")
                    Exit Sub
                End If
                sw.WriteLine("Branch     : " & DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
                dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from  [POS_SERVER].dbo.Cash a left join  [POS_SERVER].dbo.Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
                cmbReg.SelectedValue & "' And Shift = '" & ComboBox1.SelectedValue & "' And Convert(Varchar(10),Datetime,110) = Convert(Varchar(10),Convert(Datetime,'" & DateTimePicker1.Value & "'),110)")
                If dsCash.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("Cashier ID : " & dsCash.Tables(0).Rows(0).Item("User_ID") & " - " & cmbReg.SelectedValue)
                Else
                    MsgBox("(2) Register " & cmbReg.SelectedValue & " dan tanggal " & DateTimePicker1.Value.Date & " tidak ada di table Cash")
                    Exit Sub
                End If
                sw.WriteLine("Date       : " & Format(DateTimePicker1.Value, "dd/MMM/yyyy"))
                sw.WriteLine("------------------------------")
                sw.WriteLine("Modal         : Rp." & CDec(dsCash.Tables(0).Rows(0).Item("Modal")).ToString("N0").PadLeft(11, " "c))
                ds = getSqldb("SELECT Payment_Types.Description, SUM(Paid.Paid_Amount) AS Nilai " & _
                    "FROM  [POS_SERVER].dbo.Paid INNER JOIN  [POS_SERVER].dbo.Payment_Types ON Paid.Payment_Types = Payment_Types.Payment_Types " & _
                    "WHERE (Paid.Shift = '" & ComboBox1.SelectedValue & "') and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "' And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "' GROUP BY Payment_Types.seq, Payment_Types.Description order by Payment_Types.seq")
                sw.WriteLine("")
                Dim total, discx, retur As Decimal
                total = 0
                discx = 0
                retur = 0
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each ro As DataRow In ds.Tables(0).Rows
                        sw.WriteLine(Microsoft.VisualBasic.Left(Trim(ro("Description")), 13).PadRight(14, " "c) & ": Rp." & CDec(ro("Nilai")).ToString("N0").PadLeft(11, " "c))
                        total += ro("Nilai")
                    Next
                Else
                    MsgBox("(3) Tidak ada Transaksi di shift " & Shift & " ")
                    Exit Sub
                End If
                sw.WriteLine("------------------------------")
                sw.WriteLine("TOTAL        : Rp." & CDec(total).ToString("N0").PadLeft(12, " "c))
                dsVouc = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Nilai, isnull(SUM(Total_discount),0) AS Potong " & _
                    "FROM  [POS_SERVER].dbo.Sales_Transactions WHERE Status = '00' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "'  And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "'  and Transaction_Number in (select transaction_number from  [POS_SERVER].dbo.paid where Paid.Shift = '" & ComboBox1.SelectedValue & "')")
                If dsVouc.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("Over Voucher : Rp." & CDec(total - dsVouc.Tables(0).Rows(0).Item("Nilai")).ToString("N0").PadLeft(12, " "c))
                    sw.WriteLine("------------------------------")
                    sw.WriteLine("X-Reading    : Rp." & CDec(dsVouc.Tables(0).Rows(0).Item("Nilai")).ToString("N0").PadLeft(12, " "c))
                    discx = dsVouc.Tables(0).Rows(0).Item("Potong")
                Else
                    sw.WriteLine("Over Voucher : Rp." & CDec(0).ToString("N0").PadLeft(12, " "c))
                    sw.WriteLine("------------------------------")
                    sw.WriteLine("X-Reading    : Rp." & CDec(0).ToString("N0").PadLeft(12, " "c))
                    discx = 0
                End If
                sw.WriteLine("")
                sw.WriteLine("Discount     : Rp." & CDec(discx).ToString("N0").PadLeft(12, " "c))
                dsRetur = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Balik " & _
                    "FROM  [POS_SERVER].dbo.Sales_Transactions WHERE Flag_Return  = '1' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "'  And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "'  and Transaction_Number in (select transaction_number from  [POS_SERVER].dbo.paid where Paid.Shift = '" & ComboBox1.SelectedValue & "'  And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "' ) ")
                If dsRetur.Tables(0).Rows.Count > 0 Then
                    retur = dsRetur.Tables(0).Rows(0).Item("Balik")
                Else
                    retur = 0
                End If
                sw.WriteLine("Return       : Rp." & CDec(retur).ToString("N0").PadLeft(12, " "c))
            End Using
            Process.Start(Path)
        End If
    End Sub

    Sub CloseRegisterDB()
        Path = "D:\Z-Reset_" & Format(DateTimePicker1.Value, "ddMMyyyy") & ".txt"
        If File.Exists(Path) Then
            File.Delete(Path)
        End If
        If Not File.Exists(Path) Then
            ' Create a file to write to. 
            Using sw As StreamWriter = File.CreateText(Path)
                dsShift = getSqldb("Select * from  [POS_SERVER].dbo.Cash_Register Where Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and Cash_register_Id = '" & _
                    cmbReg.SelectedValue & "'")
                If dsShift.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("Z-Reset On-Line")
                Else
                    MsgBox("(1) Register " & cmbReg.SelectedValue & " Tidak Ada Ditable [Cash_Register]")
                    Exit Sub
                End If
                sw.WriteLine("Branch     : " & DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
                dsCash = getSqldb("Select b.User_ID,b.User_Name,SUM(a.Modal) As Modal from  [POS_SERVER].dbo.Cash a left join  [POS_SERVER].dbo.Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
                cmbReg.SelectedValue & "' And Convert(Varchar(10),Datetime,110) = Convert(Varchar(10),Convert(Datetime,'" & DateTimePicker1.Value & "'),110)  Group By b.User_ID,b.User_Name")
                If dsCash.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("Cashier ID : " & dsCash.Tables(0).Rows(0).Item("User_ID") & " - " & cmbReg.SelectedValue)
                Else
                    MsgBox("(2) Register " & cmbReg.SelectedValue & " dan tanggal " & DateTimePicker1.Value.Date & " tidak ada di table Cash")
                    Exit Sub
                End If
                sw.WriteLine("Date       : " & Format(DateTimePicker1.Value, "dd/MMM/yyyy"))
                sw.WriteLine("------------------------------")
                sw.WriteLine("Modal         : Rp." & CDec(dsCash.Tables(0).Rows(0).Item("Modal")).ToString("N0").PadLeft(11, " "c))
                ds = getSqldb("SELECT Payment_Types.Description, SUM(Paid.Paid_Amount) AS Nilai " & _
                    "FROM  [POS_SERVER].dbo.Paid INNER JOIN  [POS_SERVER].dbo.Payment_Types ON Paid.Payment_Types = Payment_Types.Payment_Types " & _
                    "WHERE substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "'   And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "'  GROUP BY Payment_Types.seq, Payment_Types.Description order by Payment_Types.seq")
                sw.WriteLine("")
                Dim total, discx, retur As Decimal
                total = 0
                discx = 0
                retur = 0
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each ro As DataRow In ds.Tables(0).Rows
                        sw.WriteLine(Microsoft.VisualBasic.Left(Trim(ro("Description")), 13).PadRight(14, " "c) & ": Rp." & CDec(ro("Nilai")).ToString("N0").PadLeft(11, " "c))
                        total += ro("Nilai")
                    Next
                Else
                    MsgBox("(3) Tidak ada Transaksi")
                    Exit Sub
                End If
                sw.WriteLine("------------------------------")
                sw.WriteLine("TOTAL        : Rp." & CDec(total).ToString("N0").PadLeft(12, " "c))
                dsVouc = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Nilai, isnull(SUM(Total_discount),0) AS Potong " & _
                    "FROM  [POS_SERVER].dbo.Sales_Transactions WHERE Status = '00' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "'  And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "' ")
                If dsVouc.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("Over Voucher : Rp." & CDec(total - dsVouc.Tables(0).Rows(0).Item("Nilai")).ToString("N0").PadLeft(12, " "c))
                    sw.WriteLine("------------------------------")
                    sw.WriteLine("Z-Reset      : Rp." & CDec(dsVouc.Tables(0).Rows(0).Item("Nilai")).ToString("N0").PadLeft(12, " "c))
                    discx = dsVouc.Tables(0).Rows(0).Item("Potong")
                Else
                    sw.WriteLine("Over Voucher : Rp." & CDec(0).ToString("N0").PadLeft(12, " "c))
                    sw.WriteLine("------------------------------")
                    sw.WriteLine("Z-Reset      : Rp." & CDec(0).ToString("N0").PadLeft(12, " "c))
                    discx = 0
                End If
                sw.WriteLine("")
                sw.WriteLine("Discount     : Rp." & CDec(discx).ToString("N0").PadLeft(12, " "c))
                dsRetur = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Balik " & _
                    "FROM  [POS_SERVER].dbo.Sales_Transactions WHERE Flag_Return  = '1' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "'  And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "' ")
                If dsRetur.Tables(0).Rows.Count > 0 Then
                    retur = dsRetur.Tables(0).Rows(0).Item("Balik")
                Else
                    retur = 0
                End If
                sw.WriteLine("Return       : Rp." & CDec(retur).ToString("N0").PadLeft(12, " "c))
            End Using
            Process.Start(Path)
        End If
    End Sub

    Sub CloseShift()
        Path = "D:\X-Reading_" & Format(DateTimePicker1.Value, "ddMMyyyy") & ".txt"
        If File.Exists(Path) Then
            File.Delete(Path)
        End If
        If Not File.Exists(Path) Then
            ' Create a file to write to. 
            Using sw As StreamWriter = File.CreateText(Path)
                dsShift = getSqldb("Select * from  Cash_Register Where Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and Cash_register_Id = '" & _
                    cmbReg.SelectedValue & "'")
                If dsShift.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("X-Reading Shift: " & ComboBox1.SelectedValue & " On-Line")
                Else
                    MsgBox("(1) Register " & cmbReg.SelectedValue & " Tidak Ada Ditable [Cash_Register]")
                    Exit Sub
                End If
                sw.WriteLine("Branch     : " & DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
                dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from  Cash a left join  Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
                cmbReg.SelectedValue & "' And Shift = '" & ComboBox1.SelectedValue & "' And Convert(Varchar(10),Datetime,110) = Convert(Varchar(10),Convert(Datetime,'" & DateTimePicker1.Value & "'),110)")
                If dsCash.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("Cashier ID : " & dsCash.Tables(0).Rows(0).Item("User_ID") & " - " & cmbReg.SelectedValue)
                Else
                    MsgBox("(2) Register " & cmbReg.SelectedValue & " dan tanggal " & DateTimePicker1.Value.Date & " tidak ada di table Cash")
                    Exit Sub
                End If
                sw.WriteLine("Date       : " & Format(DateTimePicker1.Value, "dd/MMM/yyyy"))
                sw.WriteLine("------------------------------")
                sw.WriteLine("Modal         : Rp." & CDec(dsCash.Tables(0).Rows(0).Item("Modal")).ToString("N0").PadLeft(11, " "c))
                ds = getSqldb("SELECT Payment_Types.Description, SUM(Paid.Paid_Amount) AS Nilai " & _
                    "FROM  Paid INNER JOIN  Payment_Types ON Paid.Payment_Types = Payment_Types.Payment_Types " & _
                    "WHERE (Paid.Shift = '" & ComboBox1.SelectedValue & "') and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "'  And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "'  GROUP BY Payment_Types.seq, Payment_Types.Description order by Payment_Types.seq")
                sw.WriteLine("")
                Dim total, discx, retur As Decimal
                total = 0
                discx = 0
                retur = 0
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each ro As DataRow In ds.Tables(0).Rows
                        sw.WriteLine(Microsoft.VisualBasic.Left(Trim(ro("Description")), 13).PadRight(14, " "c) & ": Rp." & CDec(ro("Nilai")).ToString("N0").PadLeft(11, " "c))
                        total += ro("Nilai")
                    Next
                Else
                    MsgBox("(3) Tidak ada Transaksi di shift " & Shift & " ")
                    Exit Sub
                End If
                sw.WriteLine("------------------------------")
                sw.WriteLine("TOTAL        : Rp." & CDec(total).ToString("N0").PadLeft(12, " "c))
                dsVouc = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Nilai, isnull(SUM(Total_discount),0) AS Potong " & _
                    "FROM  Sales_Transactions WHERE Status = '00' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "'  And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "'  and Transaction_Number in (select transaction_number from  paid where Paid.Shift = '" & ComboBox1.SelectedValue & "')")
                If dsVouc.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("Over Voucher : Rp." & CDec(total - dsVouc.Tables(0).Rows(0).Item("Nilai")).ToString("N0").PadLeft(12, " "c))
                    sw.WriteLine("------------------------------")
                    sw.WriteLine("X-Reading    : Rp." & CDec(dsVouc.Tables(0).Rows(0).Item("Nilai")).ToString("N0").PadLeft(12, " "c))
                    discx = dsVouc.Tables(0).Rows(0).Item("Potong")
                Else
                    sw.WriteLine("Over Voucher : Rp." & CDec(0).ToString("N0").PadLeft(12, " "c))
                    sw.WriteLine("------------------------------")
                    sw.WriteLine("X-Reading    : Rp." & CDec(0).ToString("N0").PadLeft(12, " "c))
                    discx = 0
                End If
                sw.WriteLine("")
                sw.WriteLine("Discount     : Rp." & CDec(discx).ToString("N0").PadLeft(12, " "c))
                dsRetur = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Balik " & _
                    "FROM Sales_Transactions WHERE Flag_Return  = '1' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "'  And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "'  and Transaction_Number in (select transaction_number from  paid where Paid.Shift = '" & ComboBox1.SelectedValue & "') ")
                If dsRetur.Tables(0).Rows.Count > 0 Then
                    retur = dsRetur.Tables(0).Rows(0).Item("Balik")
                Else
                    retur = 0
                End If
                sw.WriteLine("Return       : Rp." & CDec(retur).ToString("N0").PadLeft(12, " "c))
            End Using
            Process.Start(Path)
        End If
    End Sub

    Sub CloseRegister()
        Path = "D:\Z-Reset_" & Format(DateTimePicker1.Value, "ddMMyyyy") & ".txt"
        If File.Exists(Path) Then
            File.Delete(Path)
        End If
        If Not File.Exists(Path) Then
            ' Create a file to write to. 
            Using sw As StreamWriter = File.CreateText(Path)
                dsShift = getSqldb("Select * from Cash_Register Where Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and Cash_register_Id = '" & _
                    cmbReg.SelectedValue & "'")
                If dsShift.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("Z-Reset On-Line")
                Else
                    MsgBox("(1) Register " & cmbReg.SelectedValue & " Tidak Ada Ditable [Cash_Register]")
                    Exit Sub
                End If
                sw.WriteLine("Branch     : " & DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
                dsCash = getSqldb("Select b.User_ID,b.User_Name,SUM(a.Modal) As Modal from Cash a left join  Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
                cmbReg.SelectedValue & "' And Convert(Varchar(10),Datetime,110) = Convert(Varchar(10),Convert(Datetime,'" & DateTimePicker1.Value & "'),110) Group By b.User_ID,b.User_Name")
                If dsCash.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("Cashier ID : " & dsCash.Tables(0).Rows(0).Item("User_ID") & " - " & cmbReg.SelectedValue)
                Else
                    MsgBox("(2) Register " & cmbReg.SelectedValue & " dan tanggal " & DateTimePicker1.Value.Date & " tidak ada di table Cash")
                    Exit Sub
                End If
                sw.WriteLine("Date       : " & Format(DateTimePicker1.Value, "dd/MMM/yyyy"))
                sw.WriteLine("------------------------------")
                sw.WriteLine("Modal         : Rp." & CDec(dsCash.Tables(0).Rows(0).Item("Modal")).ToString("N0").PadLeft(11, " "c))
                ds = getSqldb("SELECT Payment_Types.Description, SUM(Paid.Paid_Amount) AS Nilai " & _
                    "FROM  Paid INNER JOIN  Payment_Types ON Paid.Payment_Types = Payment_Types.Payment_Types " & _
                    "WHERE substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "'  And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "'  GROUP BY Payment_Types.seq, Payment_Types.Description order by Payment_Types.seq")
                sw.WriteLine("")
                Dim total, discx, retur As Decimal
                total = 0
                discx = 0
                retur = 0
                If ds.Tables(0).Rows.Count > 0 Then
                    For Each ro As DataRow In ds.Tables(0).Rows
                        sw.WriteLine(Microsoft.VisualBasic.Left(Trim(ro("Description")), 13).PadRight(14, " "c) & ": Rp." & CDec(ro("Nilai")).ToString("N0").PadLeft(11, " "c))
                        total += ro("Nilai")
                    Next
                Else
                    MsgBox("(3) Tidak ada Transaksi")
                    Exit Sub
                End If
                sw.WriteLine("------------------------------")
                sw.WriteLine("TOTAL        : Rp." & CDec(total).ToString("N0").PadLeft(12, " "c))
                dsVouc = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Nilai, isnull(SUM(Total_discount),0) AS Potong " & _
                    "FROM Sales_Transactions WHERE Status = '00' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "'  And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "' ")
                If dsVouc.Tables(0).Rows.Count > 0 Then
                    sw.WriteLine("Over Voucher : Rp." & CDec(total - dsVouc.Tables(0).Rows(0).Item("Nilai")).ToString("N0").PadLeft(12, " "c))
                    sw.WriteLine("------------------------------")
                    sw.WriteLine("Z-Reset      : Rp." & CDec(dsVouc.Tables(0).Rows(0).Item("Nilai")).ToString("N0").PadLeft(12, " "c))
                    discx = dsVouc.Tables(0).Rows(0).Item("Potong")
                Else
                    sw.WriteLine("Over Voucher : Rp." & CDec(0).ToString("N0").PadLeft(12, " "c))
                    sw.WriteLine("------------------------------")
                    sw.WriteLine("Z-Reset      : Rp." & CDec(0).ToString("N0").PadLeft(12, " "c))
                    discx = 0
                End If
                sw.WriteLine("")
                sw.WriteLine("Discount     : Rp." & CDec(discx).ToString("N0").PadLeft(12, " "c))
                dsRetur = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Balik " & _
                    "FROM Sales_Transactions WHERE Flag_Return  = '1' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
                    "'  And substring(transaction_number, 5,3)= '" & cmbReg.SelectedValue & "' ")
                If dsRetur.Tables(0).Rows.Count > 0 Then
                    retur = dsRetur.Tables(0).Rows(0).Item("Balik")
                Else
                    retur = 0
                End If
                sw.WriteLine("Return       : Rp." & CDec(retur).ToString("N0").PadLeft(12, " "c))
            End Using
            Process.Start(Path)
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            If cmbxTyp.SelectedValue = "1" And DateTimePicker1.Value.Date = Now.Date Then
                CloseShiftDB()
            ElseIf cmbxTyp.SelectedValue = "1" And DateTimePicker1.Value.Date <> Now.Date Then
                CloseShift()
            ElseIf cmbxTyp.SelectedValue = "2" And DateTimePicker1.Value.Date = Now.Date Then
                CloseRegisterDB()
            Else
                CloseRegister()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Sub CloseShiftPrint()
        dsShift = getSqldb("Select * from Cash_Register Where Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and Cash_register_Id = '" & _
                    cmbReg.SelectedValue & "'")
        If dsShift.Tables(0).Rows.Count > 0 Then
            Shift = ComboBox1.SelectedValue
        Else
            MsgBox("(1) Register " & cmbReg.SelectedValue & " Tidak Ada Ditable [Cash_Register]")
            Exit Sub
        End If
        dsx = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Nilai, isnull(SUM(Total_discount),0) AS Potong " & _
             "FROM Sales_Transactions WHERE Status = '00' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
             "' and Transaction_Number in (select transaction_number from paid where Paid.Shift = '" & Shift & "')  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            Jual = dsx.Tables(0).Rows(0).Item("Nilai")
            diskon = dsx.Tables(0).Rows(0).Item("Potong")
        Else
            MsgBox("(2) Tidak ada record di table Sales_Transactions !!!")
            Exit Sub
        End If
        dsx.Clear()
        dsx = getSqldb("select sum(a.Paid_Amount) as Sales from  Paid a left join " & _
                            " payment_types b on a.Payment_Types = b.Payment_Types where " & _
                            "substring(transaction_number,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' and shift = '" & Shift & "'  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            If IsDBNull(dsx.Tables(0).Rows(0).Item("Sales")) Then
                MsgBox("(3) Tidak ada Nilai transaksi di shift " & Shift & " ")
                Exit Sub
            Else
            End If
            jumlah = dsx.Tables(0).Rows(0).Item("Sales")
        Else
            MsgBox("(4) Tidak ada Transaksi di shift " & Shift & " ")
            Exit Sub
        End If
        dsx.Clear()
        dsx = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Balik " & _
             "FROM  Sales_Transactions WHERE Flag_Return  = '1' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
             "' and Transaction_Number in (select transaction_number from paid where Paid.Shift = '" & Shift & "')  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            retur = dsx.Tables(0).Rows(0).Item("Balik")
        Else
            retur = 0
        End If
        dsx.Clear()
        dsx = getSqldb("SELECT isnull(SUM(Net_Price),0) AS Nilai " & _
             "FROM Sales_Transaction_Details WHERE substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
             "' and Transaction_Number in (select transaction_number from paid where Paid.Shift =  '" & Shift & "' and flag_void='1')  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            batal = dsx.Tables(0).Rows(0).Item("Nilai")
        Else
            batal = 0
        End If

        'dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from  Cash a left join Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
        '            cmbReg.SelectedValue & "'")
        dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from  Cash a left join  Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
                cmbReg.SelectedValue & "' And Shift = '" & Shift & "' And Convert(Varchar(10),Datetime,110) = Convert(Varchar(10),Convert(Datetime,'" & DateTimePicker1.Value & "'),110)")
        If dsCash.Tables(0).Rows.Count > 0 Then
            Cashier_ID = dsCash.Tables(0).Rows(0).Item("User_ID")
            Cashier_Name = dsCash.Tables(0).Rows(0).Item("User_Name")
            Modalstr = dsCash.Tables(0).Rows(0).Item("Modal")
        Else
            MsgBox("(5) Register " & cmbReg.SelectedValue & " dan tanggal " & DateTimePicker1.Value.Date & " tidak ada di table Cash")
            Exit Sub
        End If

        Try
            ds.Clear()
            ds = getSqldb("select * from v_xreading where " & _
                            "periode = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' and shift = '" & Shift & "' And Reg = '" & cmbReg.SelectedValue & "'")
            If ds.Tables(0).Rows.Count > 0 Then
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New XRead
                cryRpt.SetDataSource(ds.Tables(0))

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
                cryRpt.SetParameterValue("Date", Format(DateTimePicker1.Value, "dd/MMM/yyyy"))

                'PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
                Reports.CrystalReportViewer2.ReportSource = cryRpt
                Reports.ShowDialog()
                Reports.TopMost = True
            Else
                MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
            End If
            'MsgBox("Printed Success!!!")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub CloseShiftPrintDB()
        dsShift = getSqldb("Select * from [POS_SERVER].dbo.Cash_Register Where Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and Cash_register_Id = '" & _
                    cmbReg.SelectedValue & "'")
        If dsShift.Tables(0).Rows.Count > 0 Then
            Shift = ComboBox1.SelectedValue
        Else
            MsgBox("(1) Register " & cmbReg.SelectedValue & " Tidak Ada Ditable [Cash_Register]")
            Exit Sub
        End If
        dsx = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Nilai, isnull(SUM(Total_discount),0) AS Potong " & _
             "FROM [POS_SERVER].dbo.Sales_Transactions WHERE Status = '00' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
             "' and Transaction_Number in (select transaction_number from  [POS_SERVER].dbo.paid where Paid.Shift = '" & Shift & "')  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            Jual = dsx.Tables(0).Rows(0).Item("Nilai")
            diskon = dsx.Tables(0).Rows(0).Item("Potong")
        Else
            MsgBox("(2) Tidak ada record di table Sales_Transactions !!!")
            Exit Sub
        End If
        dsx.Clear()
        dsx = getSqldb("select sum(a.Paid_Amount) as Sales from [POS_SERVER].dbo.Paid a left join " & _
                            " [POS_SERVER].dbo.payment_types b on a.Payment_Types = b.Payment_Types where " & _
                            "substring(transaction_number,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' and shift = '" & Shift & "'  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            If IsDBNull(dsx.Tables(0).Rows(0).Item("Sales")) Then
                MsgBox("(3) Tidak ada Nilai transaksi di shift " & Shift & " ")
                Exit Sub
            Else
            End If
            jumlah = dsx.Tables(0).Rows(0).Item("Sales")
        Else
            MsgBox("(4) Tidak ada Transaksi di shift " & Shift & " ")
            Exit Sub
        End If
        dsx.Clear()
        dsx = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Balik " & _
             "FROM  [POS_SERVER].dbo.Sales_Transactions WHERE Flag_Return  = '1' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
             "' and Transaction_Number in (select transaction_number from [POS_SERVER].dbo.paid where Paid.Shift = '" & Shift & "')  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            retur = dsx.Tables(0).Rows(0).Item("Balik")
        Else
            retur = 0
        End If
        dsx.Clear()
        dsx = getSqldb("SELECT isnull(SUM(Net_Price),0) AS Nilai " & _
             "FROM [POS_SERVER].dbo.Sales_Transaction_Details WHERE substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & _
             "' and Transaction_Number in (select transaction_number from [POS_SERVER].dbo.paid where Paid.Shift =  '" & Shift & "' and flag_void='1')  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            batal = dsx.Tables(0).Rows(0).Item("Nilai")
        Else
            batal = 0
        End If

        'dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from  [POS_SERVER].dbo.Cash a left join [POS_SERVER].dbo.Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
        '            cmbReg.SelectedValue & "'")
        dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from  [POS_SERVER].dbo.Cash a left join  [POS_SERVER].dbo.Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
                cmbReg.SelectedValue & "' And Shift = '" & Shift & "' And Convert(Varchar(10),Datetime,110) = Convert(Varchar(10),Convert(Datetime,'" & DateTimePicker1.Value & "'),110)")
        If dsCash.Tables(0).Rows.Count > 0 Then
            Cashier_ID = dsCash.Tables(0).Rows(0).Item("User_ID")
            Cashier_Name = dsCash.Tables(0).Rows(0).Item("User_Name")
            Modalstr = dsCash.Tables(0).Rows(0).Item("Modal")
        Else
            MsgBox("(5) Register " & cmbReg.SelectedValue & " dan tanggal " & DateTimePicker1.Value.Date & " tidak ada di table Cash")
            Exit Sub
        End If

        Try
            ds.Clear()
            ds = getSqldb("select * from [POS_SERVER].dbo.v_xreading where " & _
                            "periode = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' and shift = '" & Shift & "'  And Reg = '" & cmbReg.SelectedValue & "'")
            If ds.Tables(0).Rows.Count > 0 Then
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New XRead
                cryRpt.SetDataSource(ds.Tables(0))

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
                cryRpt.SetParameterValue("Date", Format(DateTimePicker1.Value, "dd/MMM/yyyy"))

                'PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
                Reports.CrystalReportViewer2.ReportSource = cryRpt
                Reports.ShowDialog()
                Reports.TopMost = True
            Else
                MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
            End If
            'MsgBox("Printed Success!!!")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub CloseRegisterPrint()
        dsx = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Nilai, isnull(SUM(Total_discount),0) AS Potong " & _
             "FROM Sales_Transactions WHERE Status = '00' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            Jual = dsx.Tables(0).Rows(0).Item("Nilai")
            diskon = dsx.Tables(0).Rows(0).Item("Potong")
        Else
            MsgBox("(2) Tidak ada record di table Sales_Transactions !!!")
            Exit Sub
        End If
        dsx.Clear()
        dsx = getSqldb("select sum(a.Paid_Amount) as Sales from Paid a left join " & _
                            " payment_types b on a.Payment_Types = b.Payment_Types where " & _
                            " substring(transaction_number,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            If IsDBNull(dsx.Tables(0).Rows(0).Item("Sales")) Then
                MsgBox("(3) Tidak ada Nilai transaksi")
                Exit Sub
            Else
            End If
            jumlah = dsx.Tables(0).Rows(0).Item("Sales")
        Else
            MsgBox("(4) Tidak ada Transaksi")
            Exit Sub
        End If
        dsx.Clear()
        dsx = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Balik " & _
             "FROM  Sales_Transactions WHERE Flag_Return  = '1' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            retur = dsx.Tables(0).Rows(0).Item("Balik")
        Else
           retur = 0
        End If
        dsx.Clear()
        dsx = getSqldb("SELECT isnull(SUM(Net_Price),0) AS Nilai " & _
             "FROM Sales_Transaction_Details WHERE substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' and flag_void='1'  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            batal = dsx.Tables(0).Rows(0).Item("Nilai")
        Else
            batal = 0
        End If

        'dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from Cash a left join  Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
        '            cmbReg.SelectedValue & "'")
        dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from  Cash a left join  Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
                cmbReg.SelectedValue & "' And Convert(Varchar(10),Datetime,110) = Convert(Varchar(10),Convert(Datetime,'" & DateTimePicker1.Value & "'),110)")
        If dsCash.Tables(0).Rows.Count > 0 Then
            Cashier_ID = dsCash.Tables(0).Rows(0).Item("User_ID")
            Cashier_Name = dsCash.Tables(0).Rows(0).Item("User_Name")
            Modalstr = dsCash.Tables(0).Rows(0).Item("Modal")
        Else
            MsgBox("(5) Register " & cmbReg.SelectedValue & " dan tanggal " & DateTimePicker1.Value.Date & " tidak ada di table Cash")
            Exit Sub
        End If

        Try
            ds.Clear()
            ds = getSqldb("select Periode,Reg,Description,Seq,sum(Sales) As Sales from v_xreading where " & _
                            "periode = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'  And Reg = '" & cmbReg.SelectedValue & "' " & _
                            "group by Periode,Reg,Description,Seq order by Seq")
            If ds.Tables(0).Rows.Count > 0 Then
                'For x As Integer = 0 To 2
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New XRead
                cryRpt.SetDataSource(ds.Tables(0))

                cryRpt.SetParameterValue("Ztype", cmbxTyp.Text)
                cryRpt.SetParameterValue("ShiftStat", "ONLINE")
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
                cryRpt.SetParameterValue("Date", Format(DateTimePicker1.Value, "dd/MMM/yyyy"))
                'PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
                Reports.CrystalReportViewer2.ReportSource = cryRpt
                Reports.ShowDialog()
                Reports.TopMost = True
            Else
                MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
            End If
            'MsgBox("Printed Success!!!")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub CloseRegisterPrintDB()
        dsx = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Nilai, isnull(SUM(Total_discount),0) AS Potong " & _
             "FROM [POS_SERVER].dbo.Sales_Transactions WHERE Status = '00' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            Jual = dsx.Tables(0).Rows(0).Item("Nilai")
            diskon = dsx.Tables(0).Rows(0).Item("Potong")
        Else
            MsgBox("(2) Tidak ada record di table Sales_Transactions !!!")
            Exit Sub
        End If
        dsx.Clear()
        dsx = getSqldb("select sum(a.Paid_Amount) as Sales from [POS_SERVER].dbo.Paid a left join " & _
                            " [POS_SERVER].dbo.payment_types b on a.Payment_Types = b.Payment_Types where " & _
                            " substring(transaction_number,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            If IsDBNull(dsx.Tables(0).Rows(0).Item("Sales")) Then
                MsgBox("(3) Tidak ada Nilai transaksi")
                Exit Sub
            Else
            End If
            jumlah = dsx.Tables(0).Rows(0).Item("Sales")
        Else
            MsgBox("(4) Tidak ada Transaksi")
            Exit Sub
        End If
        dsx.Clear()
        dsx = getSqldb("SELECT isnull(SUM(Net_amount),0) AS Balik " & _
             "FROM  [POS_SERVER].dbo.Sales_Transactions WHERE Flag_Return  = '1' and substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            retur = dsx.Tables(0).Rows(0).Item("Balik")
        Else
            retur = 0
        End If
        dsx.Clear()
        dsx = getSqldb("SELECT isnull(SUM(Net_Price),0) AS Nilai " & _
             "FROM [POS_SERVER].dbo.Sales_Transaction_Details WHERE substring(transaction_number, 9,8)='" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' and flag_void='1'  And substring(transaction_number, 5,3) = '" & cmbReg.SelectedValue & "'")
        If dsx.Tables(0).Rows.Count > 0 Then
            batal = dsx.Tables(0).Rows(0).Item("Nilai")
        Else
            batal = 0
        End If

        'dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from [POS_SERVER].dbo.Cash a left join  [POS_SERVER].dbo.Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
        '            cmbReg.SelectedValue & "'")
        dsCash = getSqldb("Select b.User_ID,b.User_Name,a.Modal from  [POS_SERVER].dbo.Cash a left join  [POS_SERVER].dbo.Users b on a.User_ID = b.User_ID Where a.Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and a.Cash_register_Id = '" & _
                cmbReg.SelectedValue & "' And Convert(Varchar(10),Datetime,110) = Convert(Varchar(10),Convert(Datetime,'" & DateTimePicker1.Value & "'),110)")
        If dsCash.Tables(0).Rows.Count > 0 Then
            Cashier_ID = dsCash.Tables(0).Rows(0).Item("User_ID")
            Cashier_Name = dsCash.Tables(0).Rows(0).Item("User_Name")
            Modalstr = dsCash.Tables(0).Rows(0).Item("Modal")
        Else
            MsgBox("(5) Register " & cmbReg.SelectedValue & " dan tanggal " & DateTimePicker1.Value.Date & " tidak ada di table Cash")
            Exit Sub
        End If

        Try
            ds.Clear()
            ds = getSqldb("select Periode,Reg,Description,seq,sum(Sales) As Sales from [POS_SERVER].dbo.v_xreading where " & _
                            "periode = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "'  And Reg = '" & cmbReg.SelectedValue & "' " & _
                            "group by Periode,Reg,Description,Seq order by Seq")
            If ds.Tables(0).Rows.Count > 0 Then
                'For x As Integer = 0 To 2
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New XRead
                cryRpt.SetDataSource(ds.Tables(0))

                cryRpt.SetParameterValue("Ztype", cmbxTyp.Text)
                cryRpt.SetParameterValue("ShiftStat", "ONLINE")
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
                cryRpt.SetParameterValue("Date", Format(DateTimePicker1.Value, "dd/MMM/yyyy"))

                'PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
                Reports.CrystalReportViewer2.ReportSource = cryRpt
                Reports.ShowDialog()
                Reports.TopMost = True
            Else
                MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
            End If
            'MsgBox("Printed Success!!!")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub PrintReport(ByVal printerName As String, ByVal ReportDoc As ReportDocument)
        ReportDoc.PrintOptions.PrinterName = printerName
        ReportDoc.PrintToPrinter(1, False, 0, 0)

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            If cmbxTyp.SelectedValue = "1" And DateTimePicker1.Value.Date = Now.Date Then
                CloseShiftPrintDB()
            ElseIf cmbxTyp.SelectedValue = "1" And DateTimePicker1.Value.Date <> Now.Date Then
                CloseShiftPrint()
            ElseIf cmbxTyp.SelectedValue = "2" And DateTimePicker1.Value.Date = Now.Date Then
                CloseRegisterPrintDB()
            Else
                CloseRegisterPrint()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

End Class