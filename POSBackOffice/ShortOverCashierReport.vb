Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Public Class ShortOverCashierReport
    Dim ds, ds2 As New DataSet
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Try
    '        getSqldb("IF  NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'ShortOverCashReport' AND type = 'U') CREATE TABLE [dbo].[ShortOverCashReport] " & _
    '                 "([Reg] [nvarchar](3) NULL,[Cashier1] [nvarchar](50) NULL,[XRD1] [decimal](18, 0) NULL,[Fisik1] [decimal](18, 0) NULL,[Sel1] [int] NULL," & _
    '                 "[Cashier2] [nvarchar](50) NULL,[XRD2] [decimal](18, 0) NULL,[Fisik2] [decimal](18, 0) NULL,[Sel2] [int] NULL,[Tgl] [nvarchar](10) NULL) ON [PRIMARY]")
    '        getSqldb("delete from ShortOverCashReport")
    '        getSqldb("insert into ShortOverCashReport select DISTINCT SUBSTRING(trans_no, 5, 3) AS Reg,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'" & Format(DateTimePicker1.Value, "ddMMyyy") & "' " & _
    '                 "from slip where  SUBSTRING(trans_no, 9, 8) = '" & Format(DateTimePicker1.Value, "ddMMyyy") & "' order By SUBSTRING(trans_no, 5, 3)")
    '        ds.Clear()
    '        ds = getSqldb("SELECT     SUBSTRING(a.trans_no, 18, 1) AS Shift, SUBSTRING(a.trans_no, 9, 8) AS tgl, SUBSTRING(a.trans_no, 5, 3) AS Reg, b.User_Name, a.xread, a.realcash, a.r_over FROM " & _
    '                       "slip AS a LEFT OUTER JOIN   Users AS b ON a.cashier_id = b.User_ID where SUBSTRING(a.trans_no, 9, 8) " & _
    '                       "= '" & Format(DateTimePicker1.Value, "ddMMyyy") & "' order by  SUBSTRING(a.trans_no, 18, 1) ")
    '        If ds.Tables(0).Rows.Count > 0 Then
    '            For Each ro As DataRow In ds.Tables(0).Rows
    '                If ro("Shift") = "1" Then
    '                    getSqldb("Update ShortOverCashReport set Cashier1 = '" & ro("User_Name") & "',XRD1 = '" & ro("xread") & "',Fisik1 = '" & ro("realcash") & "',Sel1 = '" & ro("r_over") & "' where Reg = '" & ro("reg") & "'")
    '                Else
    '                    getSqldb("Update ShortOverCashReport set Cashier2 = '" & ro("User_Name") & "',XRD2 = '" & ro("xread") & "',Fisik2 = '" & ro("realcash") & "',Sel2 = '" & ro("r_over") & "' where Reg = '" & ro("reg") & "'")
    '                End If
    '            Next
    '            ds2 = getSqldb("select * from ShortOverCashReport")
    '            Dim cryRpt As New ReportDocument
    '            Dim printDoc As New PrintDocument
    '            cryRpt = New ShortOverCashier
    '            cryRpt.SetDataSource(ds2.Tables(0))
    '            cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
    '            'PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
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
            getSqldb("IF  NOT EXISTS (SELECT * FROM sys.tables WHERE name = N'ShortOverCashReport' AND type = 'U') CREATE TABLE [dbo].[ShortOverCashReport] " & _
                     "([Reg] [nvarchar](3) NULL,[Cashier1] [nvarchar](50) NULL,[XRD1] [decimal](18, 0) NULL,[Fisik1] [decimal](18, 0) NULL,[Sel1] [int] NULL," & _
                     "[Cashier2] [nvarchar](50) NULL,[XRD2] [decimal](18, 0) NULL,[Fisik2] [decimal](18, 0) NULL,[Sel2] [int] NULL,[Tgl] [nvarchar](10) NULL) ON [PRIMARY]")
            getSqldb("delete from ShortOverCashReport")
            getSqldb("insert into ShortOverCashReport select DISTINCT SUBSTRING(trans_no, 5, 3) AS Reg,NULL,NULL,NULL,NULL,NULL,NULL,NULL,NULL,'" & Format(DateTimePicker1.Value, "ddMMyyy") & "' " & _
                     "from slip where  SUBSTRING(trans_no, 9, 8) = '" & Format(DateTimePicker1.Value, "ddMMyyy") & "' order By SUBSTRING(trans_no, 5, 3)")
            ds.Clear()
            ds = getSqldb("SELECT     SUBSTRING(a.trans_no, 18, 1) AS Shift, SUBSTRING(a.trans_no, 9, 8) AS tgl, SUBSTRING(a.trans_no, 5, 3) AS Reg, b.User_Name, a.xread, a.realcash, a.r_over FROM " & _
                           "slip AS a LEFT OUTER JOIN   Users AS b ON a.cashier_id = b.User_ID where SUBSTRING(a.trans_no, 9, 8) " & _
                           "= '" & Format(DateTimePicker1.Value, "ddMMyyy") & "' order by  SUBSTRING(a.trans_no, 18, 1) ")
            If ds.Tables(0).Rows.Count > 0 Then
                For Each ro As DataRow In ds.Tables(0).Rows
                    If ro("Shift") = "1" Then
                        getSqldb("Update ShortOverCashReport set Cashier1 = '" & ro("User_Name") & "',XRD1 = '" & ro("xread") & "',Fisik1 = '" & ro("realcash") & "',Sel1 = '" & ro("r_over") & "' where Reg = '" & ro("reg") & "'")
                    Else
                        getSqldb("Update ShortOverCashReport set Cashier2 = '" & ro("User_Name") & "',XRD2 = '" & ro("xread") & "',Fisik2 = '" & ro("realcash") & "',Sel2 = '" & ro("r_over") & "' where Reg = '" & ro("reg") & "'")
                    End If
                Next
                ds2 = getSqldb("select * from ShortOverCashReport")
                Dim cryRpt As New ReportDocument
                Dim printDoc As New PrintDocument
                cryRpt = New ShortOverCashier
                cryRpt.SetDataSource(ds2.Tables(0))
                cryRpt.SetParameterValue("Store", DSBranch.Tables(0).Rows(0).Item("Branch_Name"))
                'PrintReport(printDoc.PrinterSettings.DefaultPageSettings.PrinterSettings.PrinterName.ToString, cryRpt)
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

    Private Sub ShortOverCashierReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub DateTimePicker1_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DateTimePicker1.ValueChanged

    End Sub

    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click

    End Sub

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub
End Class