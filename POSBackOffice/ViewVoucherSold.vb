Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Public Class ViewVoucherSold

    Dim Rootnode As TreeNode = Nothing
    Dim Mainnode As TreeNode = Nothing
    Dim Childnode As TreeNode = Nothing
    Dim MainName As String = String.Empty

    Private Sub TreeView1_AfterCollapse(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterCollapse
        Rootnode.ImageIndex = 0
        Rootnode.SelectedImageIndex = 2
    End Sub

    Private Sub TreeView1_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterExpand
        Rootnode.ImageIndex = 0
        Rootnode.SelectedImageIndex = 0
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim ds As New DataSet
        ds = getSqldb("update x set x.l_amt = y.Amount from lain x inner join " & _
                      " (SELECT a.v_ref,a.v_depo,a.v_sell,sum(a.v_amt) as Amount FROM newvoc a inner join NewVocDtl b on a.v_no = b.v_no group by  a.v_sell,a.v_depo,a.v_ref) y " & _
                      " on x.l_ref = y.v_ref and day(x.l_tgl) = day(y.v_sell) and month(x.l_tgl) = month(y.v_sell) and year(x.l_tgl) = year(y.v_sell) " & _
                      " where  day(x.l_tgl) = '" & DateTimePicker1.Value.Day & "' and month(x.l_tgl) = '" & DateTimePicker1.Value.Month & "' " & _
                      " and year(x.l_tgl) = '" & DateTimePicker1.Value.Year & "' and y.v_depo is NOT NULL ")
        'ds = getSqldb("update lain set l_amt = (SELECT sum(v_amt) as Amount FROM newvoc a inner join NewVocDtl b on a.v_no = b.v_no where " & _
        '              "(a.v_code = 'A') and v_ref = lain.l_ref and day(v_sell) = '4' and month(v_sell) = '11' and year(v_sell) = '2017')")
        ds = getSqldb("select a.lokasi,a.l_ref,a.l_amt,MAX(a.l_ket + ' ' + c.V_DESC_DTL) As l_ket from lain a inner join newvoc b on a.l_ref = b.v_ref " & _
                      "inner join NewVocDtl c on b.V_NO = c.V_NO where  day(l_tgl) = '" & DateTimePicker1.Value.Day & "' and month(l_tgl) = '" & DateTimePicker1.Value.Month & "' and year(l_tgl) = '" & DateTimePicker1.Value.Year & "'  and v_depo is NOT NULL " & _
                      "group by a.lokasi,a.l_ref,a.l_amt,a.l_ket")
        If ds.Tables(0).Rows.Count > 0 Then
            TreeView1.Nodes.Clear()
            Dim myImageList As New ImageList()
            Dim cc As String = Application.StartupPath
            Try
                myImageList.Images.Add(Image.FromFile("2.ico"))
                myImageList.Images.Add(Image.FromFile("1.ico"))
                myImageList.Images.Add(Image.FromFile("3.ico"))
            Catch ex As Exception
                MsgBox("Image 1,2,3.ico Trouble!!")
                myImageList.Images.Add(Image.FromFile(Application.StartupPath & "\2.ico"))
                myImageList.Images.Add(Image.FromFile(Application.StartupPath & "\2.ico"))
                myImageList.Images.Add(Image.FromFile(Application.StartupPath & "\2.ico"))
            End Try


            TreeView1.ImageList = myImageList

            TreeView1.ImageIndex = 0
            TreeView1.SelectedImageIndex = 2

            Rootnode = TreeView1.Nodes.Add(key:="Root", text:="Voucher Transactions", _
            imageIndex:=0, selectedImageIndex:=0)
            For Each row As DataRow In ds.Tables(0).Rows
                'If MainName <> row(0).ToString Then
                Mainnode = Rootnode.Nodes.Add(key:=row(1).ToString, text:=row(0).ToString & "  " & row(1).ToString & "  " & row(3).ToString & Space(40 - Microsoft.VisualBasic.Left(row(3), 39).ToString.Length) & CDec(row(2)).ToString("N0"), _
                imageIndex:=1, selectedImageIndex:=1)
                MainName = row(0).ToString
                'End If
            Next
            TreeView1.ExpandAll()
        Else
            MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
        End If
    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

        Dim dsDg As New DataSet
        dsDg = getSqldb("SELECT a.v_no as V_No,v_desc + ' ' + b.V_DESC_DTL as Description,v_amt as Amount,CONVERT(DateTime,'20'+LEFT(V_DEPO,2)+'-'+SUBSTRING(V_DEPO,3,2)+'-'+RIGHT(V_DEPO,2)+ ' 00:00:00') AS Expired,v_flag as Status  FROM newvoc a inner join NewVocDtl b on a.v_no = b.v_no where (a.v_code = '" & Microsoft.VisualBasic.Left(e.Node.Text, 1) & "') and v_ref = '" & e.Node.Name & "' and day(v_sell) = '" & DateTimePicker1.Value.Day & "' and month(v_sell) = '" & DateTimePicker1.Value.Month & "' and year(v_sell) = '" & DateTimePicker1.Value.Year & "' order by a.v_no")
        If dsDg.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = dsDg.Tables(0)
            DataGridView1.Columns("Amount").DefaultCellStyle.Format = "N0"
            DataGridView1.Columns("Expired").DefaultCellStyle.Format = "dd MMM yyyy"
            DataGridView1.Refresh()
        Else
            DataGridView1.DataSource = Nothing
        End If
       
    End Sub

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        Dim ds As New DataSet
    '        ds.Clear()
    '        ds = getSqldb("Select * from DailyVoucherSelling Where  day(L_LUPDATE) = '" & DateTimePicker1.Value.Day & "' and month(L_LUPDATE) = '" & DateTimePicker1.Value.Month & "' and year(L_LUPDATE) = '" & DateTimePicker1.Value.Year & "' and V_CODE = '" & V_Code & "' order by v_no")
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            Dim cryRpt As New ReportDocument
    '            Dim printDoc As New PrintDocument
    '            cryRpt = New RptDailyVoucherSelling
    '            cryRpt.SetDataSource(ds.Tables(0))
    '            cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
    '            Reports.CrystalReportViewer2.ReportSource = cryRpt
    '            Reports.ShowDialog()
    '            Reports.TopMost = True
    '        Else
    '            MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
    '        End If

    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        Dim ds As New DataSet
    '        ds.Clear()
    '        ds = getSqldb("Select * from DailyVoucherSelling Where  day(L_LUPDATE) = '" & DateTimePicker1.Value.Day & "' and month(L_LUPDATE) = '" & DateTimePicker1.Value.Month & "' and year(L_LUPDATE) = '" & DateTimePicker1.Value.Year & "' and V_CODE = '" & V_Code & "' order by v_no")
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            Dim cryRpt As New ReportDocument
    '            Dim printDoc As New PrintDocument
    '            cryRpt = New RptDailyVoucherSelling
    '            cryRpt.SetDataSource(ds.Tables(0))
    '            cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
    '            Reports.CrystalReportViewer2.ReportSource = cryRpt
    '            Reports.ShowDialog()
    '            Reports.TopMost = True
    '        Else
    '            MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
    '        End If

    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    'End Sub

    
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Try
            Dim ds, ds2 As New DataSet
            ds = getSqldb("update x set x.l_amt = y.Amount from lain x inner join " & _
                      " (SELECT a.v_ref,a.v_depo,a.v_sell,sum(a.v_amt) as Amount FROM newvoc a inner join NewVocDtl b on a.v_no = b.v_no group by  a.v_sell,a.v_depo,a.v_ref) y " & _
                      " on x.l_ref = y.v_ref and day(x.l_tgl) = day(y.v_sell) and month(x.l_tgl) = month(y.v_sell) and year(x.l_tgl) = year(y.v_sell) " & _
                      " where  day(x.l_tgl) = '" & DateTimePicker1.Value.Day & "' and month(x.l_tgl) = '" & DateTimePicker1.Value.Month & "' " & _
                      " and year(x.l_tgl) = '" & DateTimePicker1.Value.Year & "' and y.v_depo is NOT NULL ")
            ds.Clear()
            ds = getSqldb("exec sp_DailyVoucherSelling  '" & DateTimePicker1.Value.Day & "' , '" & DateTimePicker1.Value.Month & "' , '" & DateTimePicker1.Value.Year & "' , '" & V_Code & "'")
            If ds.Tables(0).Rows.Count > 0 Then
                ds2.Clear()
                ds2 = getSqldb("Select sum(l_amt) as Total from (select distinct l_amt,v_ref from DailyVoucherSelling Where  day(V_SELL) = '" & DateTimePicker1.Value.Day & "' and month(V_SELL) = '" & DateTimePicker1.Value.Month & "' and year(V_SELL) = '" & DateTimePicker1.Value.Year & "' and V_CODE = '" & V_Code & "') a ")
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New RptDailyVoucherSelling
                cryRpt.SetDataSource(ds.Tables(0))
                If ds2.Tables(0).Rows.Count > 0 Then
                    cryRpt.SetParameterValue("Total", ds2.Tables(0).Rows(0).Item("Total"))
                Else
                    cryRpt.SetParameterValue("Total", "")
                End If
                cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
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

End Class