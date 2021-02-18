Imports System.ComponentModel
Imports System.Data.SqlClient
Imports Microsoft.Office.Interop
Imports System.IO
Public Class Set_Price
    Dim dsListing As New DataSet
    Dim t_Load As Boolean
    Dim openFileDialog1 As New OpenFileDialog()
    Dim myStream As Stream = Nothing
    Dim Ecount As Integer = 0
    Private Sub Set_Price_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        t_Load = False
        Dim c As New ArrayList
        If m_con.State = ConnectionState.Closed Then m_con.Open()
        Dim cmd2 As New SqlCommand("select distinct class,brand As Brand from Item_Master where class = 'LKN' Order by Brand ", m_con)

        Dim objreader2 As SqlDataReader = cmd2.ExecuteReader()
        'c.Add(New CCombo("*", "*** ALL BRAND ***"))
        Do While objreader2.Read()
            c.Add(New CCombo(Trim(objreader2("class")), Trim(objreader2("Brand").ToString)))
        Loop
        m_con.Close()
        With CmbBrand
            .DataSource = c
            .DisplayMember = "Number_Name"
            .ValueMember = "ID"
        End With

        ComboBox1.Items.Add("50")
        ComboBox1.Items.Add("100")
        ComboBox1.Items.Add("ALL")
        ComboBox1.SelectedIndex = 0

        dsListing.Clear()
        DataGridView1.DataSource = Nothing
        dsListing = getSqldb3("select  distinct Top 50 e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                             "where b.Qty = 0 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                             "c where c.Qty = 2 And c.PLU = a.PLU) as [2],(select d.Price from Item_Master_listing " &
                             "d where d.Qty = 3 And d.PLU = a.PLU) as [3] from Item_Master_listing a right join " &
                             "Item_Master e on a.PLU = e.plu where e.description <> 'TIDAK AKTIF' and e.class = 'LKN'  order by e.PLU")
        If dsListing.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = dsListing.Tables(0)
            DataGridView1.Columns(2).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(3).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(4).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(0).ReadOnly = True
            DataGridView1.Columns(1).ReadOnly = True
            DataGridView1.Columns(2).HeaderText = "Purch Price 1"
            DataGridView1.Columns(3).HeaderText = "Purch Price 2"
            DataGridView1.Columns(4).HeaderText = "Purch Price 3"

            DataGridView1.Refresh()
        End If
        CheckForIllegalCrossThreadCalls = False
        t_Load = True
    End Sub

    Sub ViewList(ByVal brand As String, ByVal plu As String)
        dsListing.Clear()
        DataGridView1.DataSource = Nothing
        Dim topview As String
        If ComboBox1.Text = "50" Then
            topview = " Top 50 "
        ElseIf ComboBox1.Text = "100" Then
            topview = " Top 100 "
        Else
            topview = "  "
        End If
        If brand = "*" And plu = "" Then
            dsListing = getSqldb3("select distinct " & topview & " e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                             "where b.Qty = 0 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                             "c where c.Qty = 2 And c.PLU = a.PLU) as [2],(select d.Price from Item_Master_listing " &
                             "d where d.Qty = 3 And d.PLU = a.PLU) as [3] from Item_Master_listing a right join " &
                             "Item_Master e on a.PLU = e.plu where e.description <> 'TIDAK AKTIF' order by e.PLU")
        ElseIf brand <> "*" And plu = "" Then
            dsListing = getSqldb3("select distinct " & topview & " e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                            "where b.Qty = 0 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                            "c where c.Qty = 2 And c.PLU = a.PLU) as [2],(select d.Price from Item_Master_listing " &
                            "d where d.Qty = 3 And d.PLU = a.PLU) as [3] from Item_Master_listing a right join " &
                            "Item_Master e on a.PLU = e.plu where e.description <> 'TIDAK AKTIF'  and e.class = '" & brand & "' order by e.PLU")
        Else
            dsListing = getSqldb3("select distinct " & topview & " e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                           "where b.Qty = 0 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                           "c where c.Qty = 2 And c.PLU = a.PLU) as [2],(select d.Price from Item_Master_listing " &
                           "d where d.Qty = 3 And d.PLU = a.PLU) as [63] from Item_Master_listing a right join " &
                           "Item_Master e on a.PLU = e.plu  where e.description <> 'TIDAK AKTIF' and  e.class = '" & brand & "' and e.plu like '" & plu & "%' order by e.PLU")
        End If
        If dsListing.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = dsListing.Tables(0)
            DataGridView1.Columns(2).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(3).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(4).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(0).ReadOnly = True
            DataGridView1.Columns(1).ReadOnly = True
            DataGridView1.Columns(2).HeaderText = "Purch Price 1"
            DataGridView1.Columns(3).HeaderText = "Purch Price 2"
            DataGridView1.Columns(4).HeaderText = "Purch Price 3"
            DataGridView1.Refresh()
        End If
    End Sub

    Private Sub txtPLU_TextChanged(sender As Object, e As EventArgs) Handles txtPLU.TextChanged
        ViewList(CmbBrand.SelectedValue, txtPLU.Text.ToString.Trim)
    End Sub

    Private Sub SaveDB_Click(sender As Object, e As EventArgs) Handles SaveDB.Click
        If MessageBox.Show("You'll Update data with Effective Date '" & Format(DateTimePicker1.Value, "dd MMM yyyy") & "'  ?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
            ProgressBar1.Value = 0
            ProgressBar1.Visible = True
            Label2.Visible = True
            SaveDB.Enabled = False
            GroupBox1.Enabled = False
            BackgroundWorker1.WorkerReportsProgress = True
            BackgroundWorker1.WorkerSupportsCancellation = True
            BackgroundWorker1.RunWorkerAsync()
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        If DataGridView1.RowCount > 0 Then
            Dim dsCek As New DataSet
            Dim Prg As Decimal
            Dim Cnt As Integer = DataGridView1.Rows.Count
            Dim Nol As Boolean
            Prg = 0
            ProgressBar1.Value = 0
            For Each row As DataGridViewRow In DataGridView1.Rows

                Prg += 100 / Cnt
                If row.Cells(2).Value.ToString.Trim = "" Or row.Cells(3).Value.ToString.Trim = "" Or row.Cells(4).Value.ToString.Trim = "" Then
                    GoTo 1
                End If
                Nol = False 
                If row.Cells(2).Value.ToString.Trim = "0" Or row.Cells(3).Value.ToString.Trim = "0" Or row.Cells(4).Value.ToString.Trim = "0" Then
                    Nol = True
                End If
                dsCek = getSqldb3("select * from Item_Master_Listing where PLU = '" & row.Cells(0).Value.ToString.Trim & "'")

                If dsCek.Tables(0).Rows.Count > 0 Then
                    If Nol = True Then
                        Try
                            setLabelTxt("Delete : " & row.Cells(0).Value.ToString.Trim & " - '" & row.Cells(1).Value.ToString.Trim & "('" & row.Cells(2).Value.ToString & "','" & row.Cells(3).Value.ToString & "','" & row.Cells(4).Value.ToString & "')", Label2)
                            getSqldb3("Delete From Item_Master_Listing  where Branch_ID = 'S012' And PLU = '" & row.Cells(0).Value.ToString.Trim & "'")
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try
                    Else
                        Try
                            setLabelTxt("Update : " & row.Cells(0).Value.ToString.Trim & " - '" & row.Cells(1).Value.ToString.Trim & "('" & row.Cells(2).Value.ToString & "','" & row.Cells(3).Value.ToString & "','" & row.Cells(4).Value.ToString & "')", Label2)
                            getSqldb3("update Item_Master_Listing set Price = '" & row.Cells(2).Value.ToString.Trim & "',Start_Date = '" & DateTimePicker1.Value.Date & "' where Branch_ID = 'S012' And PLU = '" & row.Cells(0).Value.ToString.Trim & "' And Qty in (0)")
                            getSqldb3("update Item_Master_Listing set Price = '" & row.Cells(3).Value.ToString.Trim & "',Start_Date = '" & DateTimePicker1.Value.Date & "' where Branch_ID = 'S012' And PLU = '" & row.Cells(0).Value.ToString.Trim & "' And Qty in (2)")
                            getSqldb3("update Item_Master_Listing set Price = '" & row.Cells(4).Value.ToString.Trim & "',Start_Date = '" & DateTimePicker1.Value.Date & "' where Branch_ID = 'S012' And PLU = '" & row.Cells(0).Value.ToString.Trim & "' And Qty in (3)")
                            getSqldb3("update Item_Master_Listing set Price = '" & row.Cells(4).Value.ToString.Trim & "',Start_Date = '" & DateTimePicker1.Value.Date & "' where Branch_ID = 'S012' And PLU = '" & row.Cells(0).Value.ToString.Trim & "' And Qty in (4)")
                            getSqldb3("update Item_Master_Listing set Price = '" & row.Cells(4).Value.ToString.Trim & "',Start_Date = '" & DateTimePicker1.Value.Date & "' where Branch_ID = 'S012' And PLU = '" & row.Cells(0).Value.ToString.Trim & "' And Qty in (5)")
                            getSqldb3("update Item_Master_Listing set Price = '" & row.Cells(4).Value.ToString.Trim & "',Start_Date = '" & DateTimePicker1.Value.Date & "' where Branch_ID = 'S012' And PLU = '" & row.Cells(0).Value.ToString.Trim & "' And Qty in (6)")
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try

                    End If

                Else
                    If Nol = True Then
                        Try
                            setLabelTxt("Delete : " & row.Cells(0).Value.ToString.Trim & " - '" & row.Cells(1).Value.ToString.Trim & "('" & row.Cells(2).Value.ToString & "','" & row.Cells(3).Value.ToString & "','" & row.Cells(4).Value.ToString & "')", Label2)
                            getSqldb3("Delete From Item_Master_Listing  where Branch_ID = 'S012' And PLU = '" & row.Cells(0).Value.ToString.Trim & "'")
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try
                    Else
                        Try
                            setLabelTxt("Update : " & row.Cells(0).Value.ToString.Trim & " - '" & row.Cells(1).Value.ToString.Trim & "('" & row.Cells(2).Value.ToString & "','" & row.Cells(3).Value.ToString & "','" & row.Cells(4).Value.ToString & "')", Label2)
                            getSqldb3("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',0,'" & row.Cells(2).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                            getSqldb3("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',2,'" & row.Cells(3).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                            getSqldb3("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',3,'" & row.Cells(4).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                            getSqldb3("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',4,'" & row.Cells(4).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                            getSqldb3("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',5,'" & row.Cells(4).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                            getSqldb3("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',6,'" & row.Cells(4).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                        Catch ex As Exception
                            MsgBox(ex.Message)
                        End Try
                    End If


                End If
1:
                If Prg <= 100 Then
                    BackgroundWorker1.ReportProgress(Int(Prg))
                End If
            Next
        End If
    End Sub

    Private Sub setLabelTxt(ByVal text As String, ByVal lbl As Label)
        If lbl.InvokeRequired Then
            lbl.Invoke(New setLabelTxtInvoker(AddressOf setLabelTxt), text, lbl)
        Else
            lbl.Text = text
        End If
    End Sub

    Private Delegate Sub setLabelTxtInvoker(ByVal text As String, ByVal lbl As Label)
    Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        MsgBox("Success!!")
        SaveDB.Enabled = True
        GroupBox1.Enabled = True
        ProgressBar1.Visible = False
        Label2.Visible = False
        dsListing.Clear()
        txtPLU.Clear()
        txtPLU.Focus()
        DataGridView1.DataSource = Nothing
        dsListing = getSqldb3("select  distinct Top 50 e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                             "where b.Qty = 0 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                             "c where c.Qty = 2 And c.PLU = a.PLU) as [2],(select d.Price from Item_Master_listing " &
                             "d where d.Qty = 3 And d.PLU = a.PLU) as [3] from Item_Master_listing a right join " &
                             "Item_Master e on a.PLU = e.plu order by e.PLU")
        ComboBox1.SelectedIndex = 0
        If dsListing.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = dsListing.Tables(0)
            DataGridView1.Columns(2).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(3).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(4).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(0).ReadOnly = True
            DataGridView1.Columns(1).ReadOnly = True
            DataGridView1.Columns(2).HeaderText = "Purch Price 1"
            DataGridView1.Columns(3).HeaderText = "Purch Price 2"
            DataGridView1.Columns(4).HeaderText = "Purch Price 3"
            DataGridView1.Refresh()
        End If
    End Sub

    Private Sub CmbBrand_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CmbBrand.SelectedIndexChanged
        If t_Load = True Then
            ViewList(CmbBrand.SelectedValue, txtPLU.Text.ToString.Trim)
            txtPLU.Focus()
            txtPLU.SelectionStart = 0
            txtPLU.SelectionLength = txtPLU.TextLength
        End If

    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        openFileDialog1.DefaultExt = "xlsx"
        openFileDialog1.FileName = ""
        openFileDialog1.InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
        openFileDialog1.Filter = "Excel 97-2003 Template (*.xls)|*.xls|Excel Workbook (*.xlsx*)|*.xlsx*"
        openFileDialog1.FilterIndex = 2
        openFileDialog1.RestoreDirectory = True

        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

            myStream = openFileDialog1.OpenFile()
            DataGridView1.DataSource = Nothing
            If (myStream IsNot Nothing) Then
                Dim xlApp As Excel.Application
                Dim xlWorkBook As Excel.Workbook
                Dim xlWorkSheet As Excel.Worksheet
                Dim range As Excel.Range
                Dim rCnt As Integer
                xlApp = New Excel.ApplicationClass
                xlWorkBook = xlApp.Workbooks.Open(openFileDialog1.FileName)
                xlWorkSheet = xlWorkBook.Worksheets("sheet1")
                range = xlWorkSheet.UsedRange

                Ecount = 4
                While (xlWorkSheet.Cells(Ecount, 1).Value IsNot Nothing)
                    Ecount = Ecount + 1
                End While
                Ecount = Ecount - 4
                getSqldb3("delete from set_price_temp")
                For rCnt = 4 To Ecount + 3
                    Try
                        getSqldb3("insert into set_price_temp values ('" & range.Cells(rCnt, 1).value.ToString.Trim & "','" & range.Cells(rCnt, 2).value.ToString.Trim & "','" & range.Cells(rCnt, 3).value.ToString.Trim & "','" & range.Cells(rCnt, 4).value.ToString.Trim & "','" & range.Cells(rCnt, 5).value.ToString.Trim & "')")
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try
                Next
                xlWorkBook.Close()
                xlApp.Quit()

                releaseObject(xlApp)
                releaseObject(xlWorkBook)
                releaseObject(xlWorkSheet)
            End If
            MsgBox("Success!!!")
            dsListing.Clear()
            DataGridView1.DataSource = Nothing

            dsListing = getSqldb3("select * from set_price_temp")

            If dsListing.Tables(0).Rows.Count > 0 Then
                DataGridView1.DataSource = dsListing.Tables(0)
                DataGridView1.Columns(2).DefaultCellStyle.Format = "N0"
                DataGridView1.Columns(3).DefaultCellStyle.Format = "N0"
                DataGridView1.Columns(4).DefaultCellStyle.Format = "N0"
                DataGridView1.Columns(0).ReadOnly = True
                DataGridView1.Columns(1).ReadOnly = True
                DataGridView1.Columns(2).HeaderText = "Purch Price 1"
                DataGridView1.Columns(3).HeaderText = "Purch Price 2"
                DataGridView1.Columns(4).HeaderText = "Purch Price 3"
                DataGridView1.Refresh()
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Set_Price.xlsx") Then
            File.Delete(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Set_Price.xlsx")
        End If
        If File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Set_Pricex.xlsx") Then
            File.Delete(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Set_Pricex.xlsx")
        End If
        Dim sFileName As String
        Dim txtFileNew As String = Now.Year & Now.Month
        Dim line As String = ""
        Dim dsFile As DataSet
        Dim PLUStr As String = ""
        Dim loopgb1 As Integer = 0
        sFileName = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\Set_Price.xlsx"

        If File.Exists(sFileName) Then
            Try
                File.Delete(sFileName)
            Catch ex As Exception
                MsgBox("File is Being Opened !!!")
                Exit Sub
            End Try
        End If
        If CmbBrand.SelectedValue = "*" And txtPLU.Text = "" Then
            dsFile = getSqldb3("select distinct Top 100 e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                             "where b.Qty = 0 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                             "c where c.Qty = 2 And c.PLU = a.PLU) as [2],(select d.Price from Item_Master_listing " &
                             "d where d.Qty = 3 And d.PLU = a.PLU) as [3] from Item_Master_listing a right join " &
                             "Item_Master e on a.PLU = e.plu where e.description <> 'TIDAK AKTIF' order by e.PLU")
        ElseIf CmbBrand.SelectedValue <> "*" And txtPLU.Text = "" Then
            dsFile = getSqldb3("select distinct Top 100 e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                            "where b.Qty = 0 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                            "c where c.Qty = 2 And c.PLU = a.PLU) as [2],(select d.Price from Item_Master_listing " &
                            "d where d.Qty = 3 And d.PLU = a.PLU) as [3] from Item_Master_listing a right join " &
                            "Item_Master e on a.PLU = e.plu where e.description <> 'TIDAK AKTIF'  and e.class = '" & CmbBrand.SelectedValue & "' order by e.PLU")
        Else
            dsFile = getSqldb3("select distinct Top 100 e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                           "where b.Qty = 0 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                           "c where c.Qty = 2 And c.PLU = a.PLU) as [2],(select d.Price from Item_Master_listing " &
                           "d where d.Qty = 3 And d.PLU = a.PLU) as [3] from Item_Master_listing a right join " &
                           "Item_Master e on a.PLU = e.plu  where e.description <> 'TIDAK AKTIF' and  e.class = '" & CmbBrand.SelectedValue & "' and e.plu like '" & txtPLU.Text & "%' order by e.PLU")
        End If


        If dsFile.Tables(0).Rows.Count > 0 Then

            Dim chartRange As Excel.Range
            Dim xl As Object
            Dim xlWorkBook As Object
            Dim xlWorksheet As Object
            Dim dsExcel As New DataSet
            Dim Opt As String = ""
            xl = CreateObject("Excel.Application")
            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            If Dir(sFileName) = "" Then
                xlWorkBook = xl.Workbooks.Add  'File doesnt exist - add a new workbook
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Else
                xlWorkBook = xl.Workbooks.Open(sFileName)  'File exists - load it
            End If
            xlWorksheet = xlWorkBook.Worksheets(1)  'Work with the first worksheet
            'xl.Visible = True
            xlWorksheet.UsedRange.Clear()
            chartRange = xlWorksheet.Range("a3", "e3")
            chartRange.BorderAround(Excel.XlLineStyle.xlDouble,
            Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.
            xlColorIndexAutomatic)
            xl.cells(1, 1) = ("Set Price")
            'xlWorksheet.Range("A3:F3").EntireColumn.AutoFit()
            'xlWorksheet.Range("a1", "a1").Font.Name = "Arial"
            xlWorksheet.Range("a1", "a1").Font.Size = 12
            xlWorksheet.Range("a1", "a1").Font.Bold = True
            xlWorksheet.Range("a3", "e3").Font.Size = 12
            xlWorksheet.Range("a3", "e3").Font.Bold = True
            xlWorksheet.Range("d4", "d" & dsFile.Tables(0).Rows.Count + 3).numberformat = "00"
            xl.cells(3, 1) = "PLU"
            xl.cells(3, 2) = "Description"
            xl.cells(3, 3) = "Price 1"
            xl.cells(3, 4) = "Price 2"
            xl.cells(3, 5) = "Price 3"

            Dim x As Integer = 0
            For Each ro As DataRow In dsFile.Tables(0).Rows
                For y As Integer = 1 To 5
                    If y = 1 Then
                        xl.cells(x + 4, y) = "'" & ro(y - 1).ToString
                    Else
                        xl.cells(x + 4, y) = ro(y - 1).ToString
                    End If

                Next
                x += 1
            Next

            xlWorksheet.Columns("A:AD").AutoFit()
            Try
                With SaveFileDialog1
                    .Title = " Set Price"
                    .InitialDirectory = My.Computer.FileSystem.SpecialDirectories.MyDocuments
                    .DefaultExt = "xlsx"
                    .FileName = ""
                    .Filter = "Excel 97-2003 Template (*.xls)|*.xls|Excel Workbook (*.xlsx*)|*.xlsx*"
                    .FilterIndex = 2
                    If .ShowDialog() = DialogResult.OK Then
                        xlWorkBook.SaveAs(.FileName)
                        sFileName = .FileName
                    Else
                        xlWorkBook.SaveAs(sFileName)
                    End If
                End With
                'xlWorkBook.SaveAs(sFileName)
            Catch ex As Exception

            End Try
            'Save (and disconnect from) the Workbook
            xlWorkBook.Close(SaveChanges:=True)

            xlWorkBook = Nothing
            xl.Quit()                                'Close (and disconnect from) Excel
            xl = Nothing
            'make sure you clean up Excel, and SQL objects

            Process.Start(sFileName)
        End If
    End Sub

    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        ViewList(CmbBrand.SelectedValue, txtPLU.Text.ToString.Trim)
    End Sub
End Class