Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Public Class ReportDiscByStar
    Dim cardno As String
    Dim C_Nr As String
    Dim C_Reg As String
    Dim ds, dsReg, dsShift, dsCard As New DataSet
    Dim prg As Decimal
    Dim t_load As Boolean = False
    Dim C_Point As Integer
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        prg = 0
        'ds = getSqldb2("SELECT * from v_DiscByStar " & _
        '              " Where  Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
        '              "And  Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' " & _
        '              "Order By Reg,Transaction_Number ")
        ds = getSqldb2("SELECT * from v_DiscByStar  Where  CONVERT(VARCHAR(10),Date_Trans,110) between  " & _
                      "CONVERT(VARCHAR(10),Cast('" & DateTimePicker1.Value.Date & "' as Datetime),110) And " & _
                      "CONVERT(VARCHAR(10),Cast('" & DateTimePicker2.Value.Date & "' as Datetime),110) Order By Reg,Transaction_Number ")
        lvView(ds)
        t_load = True
    End Sub

    Sub lvView(ByVal dsLv As DataSet)
        'C_Nr = "XXXXXX"
        'C_Point = 0
        Dim TotlPoint, TotlRp, GrandTotlRp As Decimal
        C_Reg = "XXXXXX"
        ListView1.Items.Clear()
        Dim day As Integer = 0
        Dim mth As Integer = 0
        If ds.Tables(0).Rows.Count > 0 Then
            TotlPoint = 0
            TotlRp = 0
            GrandTotlRp = 0
            For Each ro As DataRow In ds.Tables(0).Rows
                If C_Reg <> ro(1) Then
                    If C_Reg <> "XXXXXX" Then
                        Dim stra2(7) As String
                        Dim itma2 As ListViewItem
                        stra2(0) = "================"
                        stra2(1) = "==========="
                        itma2 = New ListViewItem(stra2)
                        ListView1.Items.Add(itma2)

                        Dim strb2(7) As String
                        Dim itmb2 As ListViewItem
                        strb2(0) = "Total : "
                        strb2(1) = CDec(TotlRp).ToString("N0")
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

                    Dim strb(7) As String
                    Dim itmb As ListViewItem
                    strb(0) = "Register : "
                    strb(1) = ro(1)
                    itmb = New ListViewItem(strb)
                    itmb.Font = New System.Drawing.Font _
    ("Tahoma", 9.75, System.Drawing.FontStyle.Bold)
                    ListView1.Items.Add(itmb)

                    Dim strc(7) As String
                    Dim itmc As ListViewItem
                    strc(0) = ""
                    itmc = New ListViewItem(strc)
                    ListView1.Items.Add(itmc)
                End If
                prg += 100 / ds.Tables(0).Rows.Count
                Dim str(7) As String
                Dim itm As ListViewItem
                str(0) = ro(0)
                If IsDBNull(ro(2)) Then
                    str(1) = 0
                    TotlRp += 0
                    GrandTotlRp += 0
                Else
                    str(1) = CDec(ro(2)).ToString("N0")
                    TotlRp += CDec(ro(2))
                    GrandTotlRp += CDec(ro(2))
                End If
                itm = New ListViewItem(str)
                ListView1.Items.Add(itm)
                'C_Nr = ro(0)
                'C_Point = ro(5)
                C_Reg = ro(1)

                'BackgroundWorker1.ReportProgress(Int(Prg))
            Next
            Dim stra4(7) As String
            Dim itma4 As ListViewItem
            stra4(0) = "================"
            stra4(1) = "==========="
            itma4 = New ListViewItem(stra4)
            ListView1.Items.Add(itma4)

            Dim strb5(7) As String
            Dim itmb5 As ListViewItem
            strb5(0) = "Total : "
            strb5(1) = CDec(TotlRp).ToString("N0")
            itmb5 = New ListViewItem(strb5)
            itmb5.Font = New System.Drawing.Font _
("Tahoma", 9.75, System.Drawing.FontStyle.Bold)
            ListView1.Items.Add(itmb5)

            Dim stra10(7) As String
            Dim itma10 As ListViewItem
            stra10(0) = "================"
            stra10(1) = "==========="
            itma10 = New ListViewItem(stra10)
            ListView1.Items.Add(itma10)

            Dim stra11(7) As String
            Dim itmb11 As ListViewItem
            stra11(0) = "Grand Total : "
            stra11(1) = CDec(GrandTotlRp).ToString("N0")
            itmb11 = New ListViewItem(stra11)
            itmb11.Font = New System.Drawing.Font _
("Tahoma", 9.75, System.Drawing.FontStyle.Bold)
            ListView1.Items.Add(itmb11)

            Dim stra12(7) As String
            Dim itmc12 As ListViewItem
            stra12(0) = ""
            itmc12 = New ListViewItem(stra12)
            ListView1.Items.Add(itmc12)
        Else
            Button3.Enabled = True
            GroupBox1.Enabled = True
            ProgressBar1.Visible = False
            MessageBox.Show("No Result!!!", "Information", MessageBoxButtons.OK)
            'Exit Sub
        End If
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            Button3_Click(sender, e)
        End If
    End Sub

    Sub lv()
        ListView1.Columns.Add("Trans No", 160, HorizontalAlignment.Left)
        ListView1.Columns.Add("Disc Rp", 160, HorizontalAlignment.Left)
    End Sub



    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        Try
            ds.Clear()
            ds = getSqldb2("Select * from v_DiscByStar Where  Date_Trans >= '" & Format(DateTimePicker1.Value, "yyyy-MM-dd") & "' " & _
                      "And Date_Trans < '" & Format(DateTimePicker2.Value.AddDays(1), "yyyy-MM-dd") & "' " & _
                      " Order By Reg,Transaction_Number ")
            If ds.Tables(0).Rows.Count > 0 Then
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New CrystalReport2
                cryRpt.SetDataSource(ds.Tables(0))
                cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
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

    Private Sub PrintReport(ByVal printerName As String, ByVal ReportDoc As ReportDocument)
        ReportDoc.PrintOptions.PrinterName = printerName
        ReportDoc.PrintToPrinter(1, False, 0, 0)

    End Sub

    Private Sub ReportDiscByStar_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConnectServer()
        m_Sqlconn = "Data Source=" & m_ServerName & ";" & "Initial Catalog=" & m_DBName & ";" & "User ID=" & m_UserName & ";" & "Password=" & m_Password & ";"
        m_Sqlconn2 = "Data Source=" & m_ServerName2 & ";" & "Initial Catalog=" & m_DBName2 & ";" & "User ID=" & m_UserName2 & ";" & "Password=" & m_Password2 & ";"
        'DateTimePicker1.Value = Format(CDate(DateTimePicker1.Value.Month & "/1/" & DateTimePicker1.Value.Year), "dd  MMM  yyyy")
        lv()
    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork

    End Sub
End Class