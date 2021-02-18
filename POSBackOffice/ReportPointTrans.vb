Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Public Class ReportPointTrans
    Dim cardno As String
    Dim C_Nr As String
    Dim C_Reg, C_Shift As String
    Dim ds, dsReg, dsShift, dsCard As New DataSet
    Dim prg As Decimal
    Dim t_load As Boolean = False
    Dim C_Point As Integer
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        prg = 0
        'Dim cc As Integer
        'cc = IIf(Weekday(Now()) = 1, 7, Weekday(Now()) - 1)
        'If TextBox1.Text <> "" Then
        '    cardno = " And dbo.Cust_Point_Trans.Card_Nr = '" & TextBox1.Text & "' "
        'End If
        'cmb(ComboBox1, "select distinct SUBSTRING(b.Transaction_Number,5,3) As Reg from pos_server_history.dbo.paid a Inner Join dbo.Cust_Point_Trans b on a.Transaction_Number = b.Transaction_Number Where  b.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
        '               "And b.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' Order By SUBSTRING(b.Transaction_Number,5,3)", "Reg", "Reg", 0)
        'cmb(ComboBox2, "select distinct a.Shift from pos_server_history.dbo.paid a inner join Cust_Point_Trans b on a.Transaction_Number = b.Transaction_Number  Where b.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
        '               "And  b.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' Order By a.Shift", "Shift", "Shift", 0)
        'cmb(ComboBox3, "select distinct b.Card_Nr from pos_server_history.dbo.paid a inner join Cust_Point_Trans b on a.Transaction_Number = b.Transaction_Number  Where b.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
        '               "And  b.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' Order By b.Card_Nr", "Card_Nr", "Card_Nr", 0)
        ''dsReg = getSqldb2("select distinct SUBSTRING(b.Transaction_Number,5,3) from pos_server_history.dbo.paid a Inner Join dbo.Cust_Point_Trans b on a.Transaction_Number = b.Transaction_Number Where  b.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
        '               "And b.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' Order By SUBSTRING(b.Transaction_Number,5,3)")
        'dsShift = getSqldb2("select distinct a.Shift from pos_server_history.dbo.paid a inner join Cust_Point_Trans b on a.Transaction_Number = b.Transaction_Number order by a.Shift Where a.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
        '               "And  a.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' Order By a.Shift")
        'dsCard = getSqldb2("select distinct b.Card_Nr from pos_server_history.dbo.paid a inner join Cust_Point_Trans b on a.Transaction_Number = b.Transaction_Number order by a.Shift Where a.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
        '               "And  a.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' Order By b.Card_Nr")
        'ds = getSqldb2("SELECT * from V_PointReward " & _
        '               " Where  Date_Trans >= '" & Format(DateTimePicker1.Value.Date, "yyyy-MM-dd") & "' " & _
        '               "And  Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' " & _
        '               "Order By Shift,Card_Nr ")
        ds = getSqldb2("SELECT * from V_PointReward Where CONVERT(VARCHAR(10), Date_Trans, 101)  between  " & _
                       "CONVERT(VARCHAR(10), Convert(datetime,'" & DateTimePicker1.Value & "'), 101) And " & _
                       "CONVERT(VARCHAR(10), Convert(datetime,'" & DateTimePicker2.Value & "'), 101) Order By Shift,Card_Nr ")
        lvView(ds)
        t_load = True
        
    End Sub

    Sub lvView(ByVal dsLv As DataSet)
        'C_Nr = "XXXXXX"
        'C_Point = 0
        Dim TotlPoint, TotlRp, GrandTotlPoint, GrandTotlRp As Decimal
        C_Reg = "XXXXXX"
        C_Shift = "000000"
        ListView1.Items.Clear()
        Dim day As Integer = 0
        Dim mth As Integer = 0
        If ds.Tables(0).Rows.Count > 0 Then
            TotlPoint = 0
            TotlRp = 0
            GrandTotlPoint = 0
            GrandTotlRp = 0
            For Each ro As DataRow In ds.Tables(0).Rows
                If C_Reg <> ro(8) Then
                    If C_Reg <> "XXXXXX" Then
                        Dim stra2(7) As String
                        Dim itma2 As ListViewItem
                        stra2(0) = "================"
                        stra2(1) = "======================"
                        stra2(2) = "================"
                        stra2(3) = "================"
                        stra2(4) = "======"
                        stra2(5) = "======="
                        stra2(6) = "======="
                        stra2(7) = "==============="
                        itma2 = New ListViewItem(stra2)
                        ListView1.Items.Add(itma2)

                        Dim strb2(7) As String
                        Dim itmb2 As ListViewItem
                        strb2(0) = "Reg Total : "
                        strb2(1) = ""
                        strb2(2) = ""
                        strb2(3) = ""
                        strb2(4) = CDec(TotlPoint).ToString("N0")
                        strb2(5) = ""
                        strb2(6) = CDec(TotlRp).ToString("N0")
                        itmb2 = New ListViewItem(strb2)
                        itmb2.Font = New System.Drawing.Font _
            ("Tahoma", 9.75, System.Drawing.FontStyle.Bold)
                        ListView1.Items.Add(itmb2)
                        TotlPoint = 0
                        TotlRp = 0

                        Dim strc8(7) As String
                        Dim itmc8 As ListViewItem
                        strc8(0) = ""
                        itmc8 = New ListViewItem(strc8)
                        ListView1.Items.Add(itmc8)
                    End If

                    'Dim strc2(7) As String
                    'Dim itmc2 As ListViewItem
                    'strc2(0) = ""
                    'itmc2 = New ListViewItem(strc2)
                    'ListView1.Items.Add(itmc2)


                    Dim strb(7) As String
                    Dim itmb As ListViewItem
                    strb(0) = "Register : "
                    strb(1) = ro(8)
                    strb(2) = ""
                    strb(3) = ""
                    strb(4) = ""
                    itmb = New ListViewItem(strb)
                    itmb.Font = New System.Drawing.Font _
    ("Tahoma", 9.75, System.Drawing.FontStyle.Bold)
                    ListView1.Items.Add(itmb)

                    'Dim strc(7) As String
                    'Dim itmc As ListViewItem
                    'strc(0) = ""
                    'itmc = New ListViewItem(strc)
                    'ListView1.Items.Add(itmc)

                    Dim strc(7) As String
                    Dim itmc As ListViewItem
                    strc(0) = "Shift : "
                    strc(1) = ro(7)
                    strc(2) = ""
                    strc(3) = ""
                    strc(4) = ""
                    itmc = New ListViewItem(strc)
                    ListView1.Items.Add(itmc)
                Else
                    If C_Shift <> ro(7) Then
                        Dim strc(7) As String
                        Dim itmc As ListViewItem
                        strc(0) = "Shift : "
                        strc(1) = ro(7)
                        strc(2) = ""
                        strc(3) = ""
                        strc(4) = ""
                        itmc = New ListViewItem(strc)
                        ListView1.Items.Add(itmc)
                    End If
                End If

                prg += 100 / ds.Tables(0).Rows.Count
                Dim str(7) As String
                Dim itm As ListViewItem
                str(0) = ro(0)
                str(1) = ro(1)
                str(2) = ro(2)
                str(3) = ro(9)
                str(4) = CDec(ro(3)).ToString("N0")
                str(5) = CDec(ro(4)).ToString("N0")
                If IsDBNull(ro(5)) Then
                    str(6) = 0
                    TotlRp += 0
                    GrandTotlRp += 0
                Else
                    str(6) = CDec(ro(5)).ToString("N0")
                    TotlRp += CDec(ro(5))
                    GrandTotlRp += CDec(ro(5))
                End If

                str(7) = ro(6)
                itm = New ListViewItem(str)
                ListView1.Items.Add(itm)
                'C_Nr = ro(0)
                'C_Point = ro(5)
                C_Reg = ro(8)
                C_Shift = ro(7)
                TotlPoint += CDec(ro(3))
                GrandTotlPoint += CDec(ro(3))
                'BackgroundWorker1.ReportProgress(Int(Prg))
            Next
            Dim stra4(7) As String
            Dim itma4 As ListViewItem
            stra4(0) = "================"
            stra4(1) = "======================"
            stra4(2) = "================"
            stra4(3) = "================"
            stra4(4) = "======"
            stra4(5) = "======="
            stra4(6) = "======="
            stra4(7) = "==============="
            itma4 = New ListViewItem(stra4)
            ListView1.Items.Add(itma4)

            Dim strb5(7) As String
            Dim itmb5 As ListViewItem
            strb5(0) = "Reg Total : "
            strb5(1) = ""
            strb5(2) = ""
            strb5(3) = ""
            strb5(4) = CDec(TotlPoint).ToString("N0")
            strb5(5) = ""
            strb5(6) = CDec(TotlRp).ToString("N0")
            itmb5 = New ListViewItem(strb5)
            itmb5.Font = New System.Drawing.Font _
("Tahoma", 9.75, System.Drawing.FontStyle.Bold)
            ListView1.Items.Add(itmb5)

            Dim strc6(7) As String
            Dim itmc6 As ListViewItem
            strc6(0) = ""
            itmc6 = New ListViewItem(strc6)
            ListView1.Items.Add(itmc6)

            Dim stra10(7) As String
            Dim itma10 As ListViewItem
            stra10(0) = "================"
            stra10(1) = "======================"
            stra10(2) = "================"
            stra10(3) = "================"
            stra10(4) = "======"
            stra10(5) = "======="
            stra10(6) = "======="
            stra10(7) = "==============="
            itma10 = New ListViewItem(stra10)
            ListView1.Items.Add(itma10)

            Dim stra11(7) As String
            Dim itma11 As ListViewItem
            stra11(0) = "Grand Total : "
            stra11(1) = ""
            stra11(2) = ""
            stra11(3) = ""
            stra11(4) = CDec(GrandTotlPoint).ToString("N0")
            stra11(5) = ""
            stra11(6) = CDec(GrandTotlRp).ToString("N0")
            itma11 = New ListViewItem(stra11)
            itma11.Font = New System.Drawing.Font _
("Tahoma", 9.75, System.Drawing.FontStyle.Bold)
            ListView1.Items.Add(itma11)

            Dim strc12(7) As String
            Dim itma12 As ListViewItem
            strc12(0) = ""
            itma12 = New ListViewItem(strc12)
            ListView1.Items.Add(itma12)
        Else
            Button3.Enabled = True
            GroupBox1.Enabled = True
            ProgressBar1.Visible = False
            MessageBox.Show("No Result!!!", "Information", MessageBoxButtons.OK)
            'Exit Sub
        End If
    End Sub

    'Sub ChangeView1(ByVal cmb1 As String)
    '    ds.Clear()
    '    cmb(ComboBox2, "select distinct a.Shift from pos_server_history.dbo.paid a inner join Cust_Point_Trans b on a.Transaction_Number = b.Transaction_Number Where b.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
    '                   "And  b.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' " & cmb1 & " Order By a.Shift", "Shift", "Shift", 0)
    '    ds = getSqldb2("SELECT DISTINCT  b.Card_Nr, a.Cust_Name,b.Transaction_Number, b.Claim_Point,b.Date_Trans, c.Card_Point,d.Shift, " & _
    '                  "SUBSTRING(b.Transaction_Number,5,3) As Req  FROM dbo.Cust_Point_Trans b INNER JOIN dbo.Card c  " & _
    '                  "ON b.Card_Nr = c.Card_Nr INNER JOIN dbo.Customer_Master_Member a ON c.Cust_Nr = a.Cust_Nr " & _
    '                  "inner join pos_server_history.dbo.paid d On b.Transaction_Number = d.Transaction_Number " & _
    '                  " Where  b.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
    '                  "And  b.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' " & _
    '                  " " & cmb1 & " Order By b.Card_Nr ")

    '    lvView(ds)
    'End Sub

    'Sub ChangeView2(ByVal cmb1 As String, ByVal cmb2 As String)
    '    ds.Clear()
    '    cmb(ComboBox3, "select distinct b.Card_Nr from pos_server_history.dbo.paid a inner join Cust_Point_Trans b on a.Transaction_Number = b.Transaction_Number Where b.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
    '                    "And  b.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' " & cmb1 & cmb2 & " Order By b.Card_Nr", "Card_Nr", "Card_Nr", 0)
    '    ds = getSqldb2("SELECT DISTINCT  b.Card_Nr, d.Cust_Name,b.Transaction_Number, b.Claim_Point,b.Date_Trans, c.Card_Point,a.Shift, " & _
    '                  "SUBSTRING(b.Transaction_Number,5,3) As Req  FROM dbo.Cust_Point_Trans b INNER JOIN dbo.Card c  " & _
    '                  "ON b.Card_Nr = c.Card_Nr INNER JOIN dbo.Customer_Master_Member d ON c.Cust_Nr = d.Cust_Nr " & _
    '                  "inner join pos_server_history.dbo.paid a On b.Transaction_Number = a.Transaction_Number " & _
    '                  " Where  b.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
    '                  "And  b.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' " & _
    '                  " " & cmb1 & cmb2 & " Order By b.Card_Nr ")

    '    lvView(ds)
    'End Sub

    Sub ChangeView3(ByVal cmb1 As String, ByVal cmb2 As String, ByVal cmb3 As String)
        ds.Clear()
        ds = getSqldb2("SELECT DISTINCT  b.Card_Nr, d.Cust_Name,b.Transaction_Number, b.Claim_Point,b.Date_Trans, c.Card_Point,a.Shift, " & _
                      "SUBSTRING(b.Transaction_Number,5,3) As Req  FROM dbo.Cust_Point_Trans b INNER JOIN dbo.Card c  " & _
                      "ON b.Card_Nr = c.Card_Nr INNER JOIN dbo.Customer_Master_Member d ON c.Cust_Nr = d.Cust_Nr " & _
                      "inner join pos_server_history.dbo.paid a On b.Transaction_Number = a.Transaction_Number " & _
                      " Where  b.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
                      "And  b.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' " & _
                      " " & cmb1 & cmb2 & cmb3 & " Order By b.Card_Nr ")
        lvView(ds)

    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            Button3_Click(sender, e)
        End If
    End Sub

    Private Sub ReportPointTrans_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConnectServer()
        m_Sqlconn = "Data Source=" & m_ServerName & ";" & "Initial Catalog=" & m_DBName & ";" & "User ID=" & m_UserName & ";" & "Password=" & m_Password & ";"
        m_Sqlconn2 = "Data Source=" & m_ServerName2 & ";" & "Initial Catalog=" & m_DBName2 & ";" & "User ID=" & m_UserName2 & ";" & "Password=" & m_Password2 & ";"
        DateTimePicker1.Value = Format(CDate(DateTimePicker1.Value.Month & "/1/" & DateTimePicker1.Value.Year), "dd  MMM  yyyy")
        lv()
    End Sub

    Sub lv()
        ListView1.Columns.Add("Card No", 155, HorizontalAlignment.Left)
        ListView1.Columns.Add("Card Name", 220, HorizontalAlignment.Left)
        ListView1.Columns.Add("Trans No", 160, HorizontalAlignment.Left)
        ListView1.Columns.Add("Trans Point No", 160, HorizontalAlignment.Left)
        ListView1.Columns.Add("Claim Point", 80, HorizontalAlignment.Left)
        ListView1.Columns.Add("Curent Point", 80, HorizontalAlignment.Left)
        ListView1.Columns.Add("Claim Rp", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Trans Date", 160, HorizontalAlignment.Left)
    End Sub

    'Private Sub ComboBox1_SelectionChangeCommitted(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If t_load = False Then
    '        Exit Sub
    '    End If
    '    If ComboBox1.SelectedValue = "*" Then
    '        ChangeView1("")
    '    Else
    '        ChangeView1(" And SUBSTRING(b.Transaction_Number,5,3) = " & ComboBox1.SelectedValue & " ")
    '    End If
    'End Sub

    'Private Sub ComboBox2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If t_load = False Then
    '        Exit Sub
    '    End If
    '    Dim cmb1 As String
    '    If ComboBox1.SelectedValue = "*" Then
    '        cmb1 = ""
    '    Else
    '        cmb1 = " And SUBSTRING(b.Transaction_Number,5,3) = " & ComboBox1.SelectedValue & " " & ""
    '    End If

    '    If ComboBox2.SelectedValue = "*" Then
    '        ChangeView2(cmb1, "")
    '    Else
    '        ChangeView2(cmb1, " And a.Shift = " & ComboBox2.SelectedValue & " " & "")
    '    End If

    'End Sub

    'Private Sub ComboBox3_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
    '    If t_load = False Then
    '        Exit Sub
    '    End If
    '    Dim cmb1, cmb2 As String
    '    If ComboBox1.SelectedValue = "*" Then
    '        cmb1 = ""
    '    Else
    '        cmb1 = " And SUBSTRING(b.Transaction_Number,5,3) = '" & ComboBox1.SelectedValue & "' " & ""
    '    End If

    '    If ComboBox2.SelectedValue = "*" Then
    '        cmb2 = ""
    '    Else
    '        cmb2 = " And a.Shift = '" & ComboBox2.SelectedValue & "' "
    '    End If

    '    If ComboBox3.SelectedValue = "*" Then
    '        ChangeView3(cmb1, cmb2, "")
    '    Else
    '        ChangeView3(cmb1, cmb2, " And  b.Card_Nr = '" & ComboBox3.SelectedValue & "' " & "")
    '    End If

    'End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Dim cmb1, cmb2, cmb3 As String
        'If ComboBox1.SelectedValue = "*" Then
        '    cmb1 = ""
        'Else
        '    cmb1 = " And Req = '" & ComboBox1.SelectedValue & "' " & ""
        'End If

        'If ComboBox2.SelectedValue = "*" Then
        '    cmb2 = ""
        'Else
        '    cmb2 = " And Shift = '" & ComboBox2.SelectedValue & "' "
        'End If

        'If ComboBox3.SelectedValue = "*" Then
        '    cmb3 = ""
        'Else
        '    cmb3 = "Card_Nr = '" & ComboBox3.SelectedValue & "' " & ""
        'End If

        Try
            ds.Clear()
            ds = getSqldb2("SELECT * from V_PointReward Where CONVERT(VARCHAR(10), Date_Trans, 101)  between  " & _
                       "CONVERT(VARCHAR(10), Convert(datetime,'" & DateTimePicker1.Value & "'), 101) And " & _
                       "CONVERT(VARCHAR(10), Convert(datetime,'" & DateTimePicker2.Value & "'), 101) Order By Shift,Card_Nr ")
            If ds.Tables(0).Rows.Count > 0 Then
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New CrystalReport1
                cryRpt.SetDataSource(ds.Tables(0))
                cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
                'If MsgBox("Print this Report ??", MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                'PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
                'End If
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

    Private Sub PrintReport(ByVal printerName As String, ByVal ReportDoc As ReportDocument)
        ReportDoc.PrintOptions.PrinterName = printerName
        ReportDoc.PrintToPrinter(1, False, 0, 0)

    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

    End Sub
End Class