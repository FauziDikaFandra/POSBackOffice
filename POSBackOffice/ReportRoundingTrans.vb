Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Public Class ReportRoundingTrans
    'Dim cardno As String
    'Dim C_Nr As String
    Dim C_Reg As String
    Dim ds As New DataSet
    ',dsReg, dsShift, dsCard As New DataSet
    'Dim prg As Decimal
    'Dim t_load As Boolean = False
    'Dim C_Point As Integer
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ds = getSqldb("SELECT * from V_Rounding " & _
                      " Where Transaction_Date between  '" & DateTimePicker1.Value.Date & "' And " & _
                       "'" & DateTimePicker2.Value.Date & "' Order By Transaction_Date,transaction_number ")
        lvView(ds)
        't_load = True
    End Sub

    Sub lvView(ByVal dsLv As DataSet)
        Dim TotlRp, GrandTotlRp As Decimal
        Dim awal As Boolean = True
        C_Reg = "XXXXXX"
        ListView1.Items.Clear()
        If ds.Tables(0).Rows.Count > 0 Then
            GrandTotlRp = 0
            TotlRp = 0
            For Each ro As DataRow In ds.Tables(0).Rows
                If C_Reg <> ro(1) Then
                    If awal = False Then
                        Dim stra7(3) As String
                        Dim itma7 As ListViewItem
                        stra7(0) = "================="
                        stra7(1) = "=========="
                        stra7(2) = "=========="
                        itma7 = New ListViewItem(stra7)
                        ListView1.Items.Add(itma7)

                        Dim strb8(7) As String
                        Dim itmb8 As ListViewItem
                        strb8(0) = "Reg Total : "
                        strb8(1) = ""
                        strb8(2) = CDec(TotlRp).ToString("N0")
                        itmb8 = New ListViewItem(strb8)
                        itmb8.Font = New System.Drawing.Font _
            ("Tahoma", 9.75, System.Drawing.FontStyle.Bold)
                        ListView1.Items.Add(itmb8)

                        Dim strc9(7) As String
                        Dim itmc9 As ListViewItem
                        strc9(0) = ""
                        itmc9 = New ListViewItem(strc9)
                        ListView1.Items.Add(itmc9)

                    End If

                    Dim strb(7) As String
                    Dim itmb As ListViewItem
                    strb(0) = "Register : " & Format(ro(1))
                    strb(1) = ""
                    strb(2) = ""
                    itmb = New ListViewItem(strb)
                    itmb.Font = New System.Drawing.Font _
    ("Tahoma", 9.75, System.Drawing.FontStyle.Bold)
                    ListView1.Items.Add(itmb)
                    TotlRp = 0
                End If

                Dim str(3) As String
                Dim itm As ListViewItem
                str(0) = ro(2)
                str(1) = ro(3).ToString.Trim
                str(2) = CDec(ro(4)).ToString("N0")
                TotlRp += CDec(ro(4))
                GrandTotlRp += CDec(ro(4))
                itm = New ListViewItem(str)
                ListView1.Items.Add(itm)
                C_Reg = ro(1)
                awal = False
            Next
            Dim stra4(3) As String
            Dim itma4 As ListViewItem
            stra4(0) = "================="
            stra4(1) = "=========="
            stra4(2) = "=========="
            itma4 = New ListViewItem(stra4)
            ListView1.Items.Add(itma4)

            Dim strb5(7) As String
            Dim itmb5 As ListViewItem
            strb5(0) = "Reg Total : "
            strb5(1) = ""
            strb5(2) = CDec(TotlRp).ToString("N0")
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
            stra10(0) = "================="
            stra10(1) = "=========="
            stra10(2) = "=========="
          
            itma10 = New ListViewItem(stra10)
            ListView1.Items.Add(itma10)

            Dim stra11(7) As String
            Dim itma11 As ListViewItem
            stra11(0) = "Grand Total : "
            stra11(1) = ""
            stra11(2) = CDec(GrandTotlRp).ToString("N0")
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
        End If
    End Sub

    
    'Sub ChangeView3(ByVal cmb1 As String, ByVal cmb2 As String, ByVal cmb3 As String)
    '    ds.Clear()
    '    ds = getSqldb2("SELECT DISTINCT  b.Card_Nr, d.Cust_Name,b.Transaction_Number, b.Claim_Point,b.Date_Trans, c.Card_Point,a.Shift, " & _
    '                  "SUBSTRING(b.Transaction_Number,5,3) As Req  FROM dbo.Cust_Point_Trans b INNER JOIN dbo.Card c  " & _
    '                  "ON b.Card_Nr = c.Card_Nr INNER JOIN dbo.Customer_Master_Member d ON c.Cust_Nr = d.Cust_Nr " & _
    '                  "inner join pos_server_history.dbo.paid a On b.Transaction_Number = a.Transaction_Number " & _
    '                  " Where  b.Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
    '                  "And  b.Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' " & _
    '                  " " & cmb1 & cmb2 & cmb3 & " Order By b.Card_Nr ")
    '    lvView(ds)

    'End Sub


    Private Sub ReportPointTrans_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConnectServer()
        m_Sqlconn = "Data Source=" & m_ServerName & ";" & "Initial Catalog=" & m_DBName & ";" & "User ID=" & m_UserName & ";" & "Password=" & m_Password & ";"
        m_Sqlconn2 = "Data Source=" & m_ServerName2 & ";" & "Initial Catalog=" & m_DBName2 & ";" & "User ID=" & m_UserName2 & ";" & "Password=" & m_Password2 & ";"
        DateTimePicker1.Value = Format(CDate(DateTimePicker1.Value.Month & "/1/" & DateTimePicker1.Value.Year), "dd  MMM  yyyy")
        lv()
    End Sub

    Sub lv()
        ListView1.Columns.Add("Trans No", 165, HorizontalAlignment.Left)
        ListView1.Columns.Add("Trans Name", 120, HorizontalAlignment.Left)
        ListView1.Columns.Add("Amount", 120, HorizontalAlignment.Left)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            ds.Clear()
            ds = getSqldb("SELECT * from V_Rounding " & _
                      " Where Transaction_Date between  '" & DateTimePicker1.Value.Date & "' And " & _
                       "'" & DateTimePicker2.Value.Date & "' Order By Transaction_Date,transaction_number ")
            If ds.Tables(0).Rows.Count > 0 Then
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New RoundingReport
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

End Class