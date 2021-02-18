Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Public Class DailySales
    Dim ds As New DataSet
    Dim C_Reg As String
    Private Sub Button1_Click(sender As Object, e As EventArgs)

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
                        strb8(0) = "Reg Total " & C_Reg & " :"
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
            strb5(0) = "Reg Total " & C_Reg & " :"
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

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        ds = getSqldb("select * from v_MOP  Where Transaction_Date =  '" & DateTimePicker1.Value.Date & "'  union all select * from [POS_SERVER].dbo.v_mop" &
                    " Where Transaction_Date =  '" & DateTimePicker1.Value.Date & "'  Order By Transaction_Date,transaction_number ")
        lvView(ds)
    End Sub

    Private Sub DailySales_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'DateTimePicker1.Value = Format(CDate(DateTimePicker1.Value.Month & "/1/" & DateTimePicker1.Value.Year), "dd  MMM  yyyy")
        lv()
    End Sub

    Sub lv()
        ListView1.Columns.Add("Trans No", 165, HorizontalAlignment.Left)
        ListView1.Columns.Add("Trans Name", 200, HorizontalAlignment.Left)
        ListView1.Columns.Add("Amount", 120, HorizontalAlignment.Left)
    End Sub
End Class