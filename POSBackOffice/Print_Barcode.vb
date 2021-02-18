Imports System.ComponentModel
Imports System.IO
Imports Microsoft.Office.Interop
Public Class Print_Barcode_RFID
    Dim dsDg As New DataSet
    Dim t_load As Boolean = False
    Dim myStream As Stream = Nothing
    Private Sub Download_Item_List_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cmb2(ComboBox1, "select Vendor_code,name From m_vendor Order By Name", "Vendor_code", "name", 1)
        t_load = True

        ComboBox2.Items.Add("Vendor Name")
        ComboBox2.Items.Add("Good Reciept PO")
        ComboBox2.SelectedIndex = 0
        ViewDg()
        TextBox1.Visible = False
    End Sub

    Sub ViewDg()
        dsDg.Clear()
        If ComboBox2.SelectedIndex = 0 Then
            dsDg = getSqldb("select dp2 as SBU, Article_code As Article,PLU,long_description as Description,current_price as Price,'' as Qty from item_master where supplier_code  = '" & ComboBox1.SelectedValue & "' order by Article_code")
        Else
            dsDg = getSqldb2("select c.dp2 as SBU, c.Article_code As Article,c.PLU,c.long_description as Description,current_price as Price, b.Qty from t_rc a inner join t_rc_detail b on a.rc_code = b.rc_code inner join item_master c on b.plu = c.plu where a.rc_code = '" & TextBox1.Text & "' and a.status = 'POST' order by c.Article_code")
        End If

        DataGridView1.DataSource = Nothing
        If dsDg.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = dsDg.Tables(0)
            DataGridView1.Columns(0).Visible = False
            DataGridView1.Columns(1).ReadOnly = True
            DataGridView1.Columns(2).ReadOnly = True
            DataGridView1.Columns(3).ReadOnly = True
            DataGridView1.Columns(4).ReadOnly = True
            DataGridView1.Columns(4).DefaultCellStyle.Format = "N0"
        End If
    End Sub

    Private Sub ComboBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedValueChanged
        If t_load = True Then
            ViewDg()
        End If

    End Sub

    Private Sub SAVEAllEPC_Click(sender As Object, e As EventArgs) Handles SAVEAllEPC.Click
        getSqldb3("delete from Temp_ListItem")
        Dim EPC As String = ""
        Dim dsList As New DataSet
        Dim sFileName As String = ""
        For x As Integer = 0 To DataGridView1.RowCount - 1
            If DataGridView1.Item(5, x).Value.ToString <> "" Then
                If IsNumeric(DataGridView1.Item(5, x).Value.ToString) And DataGridView1.Item(5, x).Value.ToString <> "0" Then
                    Dim SBU As String = DataGridView1.Item(0, x).Value.ToString.Trim
                    For y As Integer = 1 To CInt(DataGridView1.Item(5, x).Value)
                        EPC = Cek(SBU)
                        getSqldb3("insert into Temp_ListItem values ('" & EPC & "','" & DataGridView1.Item(2, x).Value & "','" & Microsoft.VisualBasic.Left(DataGridView1.Item(3, x).Value, 20) & "','" & Microsoft.VisualBasic.Mid(DataGridView1.Item(3, x).Value, 21, 20) & "','Rp. " & CDec(DataGridView1.Item(4, x).Value).ToString("N0") & "')")
                        getSqldb3("insert into Item_Master_RFID_back values ('" & DataGridView1.Item(1, x).Value & "','" & DataGridView1.Item(2, x).Value & "','" & EPC & "','','','','','0','0','0',getdate())")
                    Next
                End If
            End If
        Next
        dsList = getSqldb3("select * from Temp_ListItem")
        If dsList.Tables(0).Rows.Count > 0 Then
            Dim xl As Object
            Dim xlWorkBook As Object
            Dim xlWorksheet As Object
            Dim dsExcel As New DataSet
            Dim Opt As String = ""
            xl = CreateObject("Excel.Application")
            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            xlWorkBook = xl.Workbooks.Add  'File doesnt exist - add a new workbook
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

            xlWorksheet = xlWorkBook.Worksheets(1)  'Work with the first worksheet

            xl.cells(1, 1) = "TCode"
            xl.cells(1, 2) = "PLU"
            xl.cells(1, 3) = "Desc 1"
            xl.cells(1, 4) = "Desc 2"
            xl.cells(1, 5) = "Price"

            Dim x As Integer = 2

            For Each ro As DataRow In dsList.Tables(0).Rows
                xl.cells(x, 1) = ro("TCode")
                xl.cells(x, 2) = "'" & ro("PLU")
                xl.cells(x, 3) = ro("Desc1")
                xl.cells(x, 4) = ro("Desc2")
                xl.cells(x, 5) = ro("Price")
                x += 1
            Next
            Try
                If (Not System.IO.Directory.Exists(m_Path & Format(Now, "ddMMyyyy"))) Then
                    System.IO.Directory.CreateDirectory(m_Path & Format(Now, "ddMMyyyy"))
                End If
                With SaveFileDialog1
                    .Title = " List Item"
                    .InitialDirectory = m_Path & Format(Now, "ddMMyyyy") & "\"
                    .DefaultExt = "xlsx"
                    .FileName = "List_Item"
                    .Filter = "Excel 97-2003 Template (*.xls)|*.xls|Excel Workbook (*.xlsx)|*.xlsx"
                    .FilterIndex = 2
                    If .ShowDialog() = DialogResult.OK Then
                        xlWorkBook.SaveAs(.FileName)
                        sFileName = .FileName
                    Else
                        xlWorkBook.SaveAs(sFileName)
                    End If
                    MsgBox("Success !!!")
                    Process.Start(Path.GetDirectoryName(sFileName))
                End With
                'xlWorkBook.SaveAs(sFileName)
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            'Save (and disconnect from) the Workbook
            xlWorkBook.Close(SaveChanges:=True)

            xlWorkBook = Nothing
            xl.Quit()                                'Close (and disconnect from) Excel
            xl = Nothing
        End If
    End Sub

    Private Function Cek(ByVal SBU As String) As String
        Dim ds As New DataSet

        If SBU = "CH" Then
            Cek = "A" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        ElseIf SBU = "HH" Then
            Cek = "B" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        ElseIf SBU = "LA" Then
            Cek = "C" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        ElseIf SBU = "LD" Then
            Cek = "D" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        ElseIf SBU = "MD" Then
            Cek = "E" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        Else
            Cek = "F" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        End If
        ds = getSqldb3("Select Convert(INT,SUBSTRING(EPC,6,8)) + 1 As Urut from Item_Master_RFID_back where SUBSTRING(EPC,1,5) = '" & Cek & "' order by Convert(INT,SUBSTRING(EPC,6,8)) desc")

        If ds.Tables(0).Rows.Count > 0 Then
            Cek = Cek & "00000000".Substring(0, 8 - Len(ds.Tables(0).Rows(0).Item("urut").ToString)) & ds.Tables(0).Rows(0).Item("urut")
        Else
            Cek = Cek & "00000001"
        End If
        Return Cek
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        OpenFileDialog1.DefaultExt = "xlsx"
        OpenFileDialog1.FileName = ""
        OpenFileDialog1.InitialDirectory = m_Path & Format(Now, "ddMMyyyy") & "\"
        OpenFileDialog1.Filter = "Excel 97-2003 Template (*.xls)|*.xls|Excel Workbook (*.xlsx*)|*.xlsx*"
        OpenFileDialog1.FilterIndex = 2
        OpenFileDialog1.RestoreDirectory = True

        If OpenFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            getSqldb3("delete from Temp_ListDownload")
            myStream = OpenFileDialog1.OpenFile()
            If (myStream IsNot Nothing) Then
                Dim xlApp As Excel.Application
                Dim xlWorkBook As Excel.Workbook
                Dim xlWorkSheet As Excel.Worksheet
                Dim range As Excel.Range
                Dim rCnt As Integer
                xlApp = New Excel.ApplicationClass
                xlWorkBook = xlApp.Workbooks.Open(OpenFileDialog1.FileName)
                xlWorkSheet = xlWorkBook.Worksheets("sheet1")
                range = xlWorkSheet.UsedRange
                Dim Ecount As Integer
                Ecount = 2
                While (xlWorkSheet.Cells(Ecount, 1).Value IsNot Nothing)
                    Ecount = Ecount + 1
                End While
                Ecount = Ecount - 2

                For rCnt = 2 To Ecount + 1
                    Try
                        getSqldb3("insert into Temp_ListDownload values ('" & range.Cells(rCnt, 1).value.ToString.Trim & "'," &
                                                     "'" & range.Cells(rCnt, 2).value.ToString.Trim & "','" & range.Cells(rCnt, 3).value.ToString.Trim & "'," &
                                                     "'" & range.Cells(rCnt, 4).value.ToString.Trim & "','" & range.Cells(rCnt, 5).value.ToString.Trim & "'," &
                                                     "'" & range.Cells(rCnt, 6).value.ToString.Trim & "')")
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
            dsDg.Clear()
            dsDg = getSqldb3("select * from Temp_ListDownload")
            DataGridView1.DataSource = Nothing
            If dsDg.Tables(0).Rows.Count > 0 Then
                DataGridView1.DataSource = dsDg.Tables(0)
                DataGridView1.Columns(0).Visible = False
                DataGridView1.Columns(1).ReadOnly = True
                DataGridView1.Columns(2).ReadOnly = True
                DataGridView1.Columns(3).ReadOnly = True
                DataGridView1.Columns(4).ReadOnly = True
                DataGridView1.Columns(4).DefaultCellStyle.Format = "N0"
            End If
        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim sFileName As String = ""
        Dim dsList As New DataSet
        dsList.Clear()
        dsList = getSqldb("select dp2 as SBU, Article_code As Article,PLU,long_description as Description,current_price as Price,'' as Qty from item_master where supplier_code  = '" & ComboBox1.SelectedValue & "' order by Article_code")
        If dsList.Tables(0).Rows.Count > 0 Then
            Dim xl As Object
            Dim xlWorkBook As Object
            Dim xlWorksheet As Object
            Dim dsExcel As New DataSet
            Dim Opt As String = ""
            xl = CreateObject("Excel.Application")
            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            xlWorkBook = xl.Workbooks.Add  'File doesnt exist - add a new workbook
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

            xlWorksheet = xlWorkBook.Worksheets(1)  'Work with the first worksheet

            xl.cells(1, 1) = "SBU"
            xl.cells(1, 2) = "Article"
            xl.cells(1, 3) = "PLU"
            xl.cells(1, 4) = "Description"
            xl.cells(1, 5) = "Price"
            xl.cells(1, 6) = "Qty"

            Dim x As Integer = 2

            For Each ro As DataRow In dsList.Tables(0).Rows
                xl.cells(x, 1) = ro("SBU")
                xl.cells(x, 2) = ro("Article")
                xl.cells(x, 3) = "'" & ro("PLU")
                xl.cells(x, 4) = ro("Description")
                xl.cells(x, 5) = ro("Price")
                xl.cells(x, 6) = ro("Qty")
                x += 1
            Next
            Try
                If (Not System.IO.Directory.Exists(m_Path & Format(Now, "ddMMyyyy"))) Then
                    System.IO.Directory.CreateDirectory(m_Path & Format(Now, "ddMMyyyy"))
                End If
                With SaveFileDialog1
                    .Title = " List Download"
                    .InitialDirectory = m_Path & Format(Now, "ddMMyyyy") & "\"
                    .DefaultExt = "xlsx"
                    .FileName = "List_Download"
                    .Filter = "Excel 97-2003 Template (*.xls)|*.xls|Excel Workbook (*.xlsx)|*.xlsx"
                    .FilterIndex = 2
                    If .ShowDialog() = DialogResult.OK Then
                        xlWorkBook.SaveAs(.FileName)
                        sFileName = .FileName
                    Else
                        xlWorkBook.SaveAs(sFileName)
                    End If
                    MsgBox("Success !!!")
                    Process.Start(Path.GetDirectoryName(sFileName))
                End With
                'xlWorkBook.SaveAs(sFileName)
            Catch ex As Exception

            End Try
            'Save (and disconnect from) the Workbook
            xlWorkBook.Close(SaveChanges:=True)

            xlWorkBook = Nothing
            xl.Quit()                                'Close (and disconnect from) Excel
            xl = Nothing
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        If ComboBox2.SelectedIndex = 0 Then
            TextBox1.Visible = False
            ComboBox1.Visible = True
        Else
            TextBox1.Visible = True
            ComboBox1.Visible = False
            TextBox1.Clear()
            TextBox1.Focus()
        End If
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            ViewDg()
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        'Button3.Enabled = False
        'getSqldb3("delete from Temp_ListItem")
        'ProgressBar1.Visible = True
        'BackgroundWorker1.WorkerReportsProgress = True
        'BackgroundWorker1.WorkerSupportsCancellation = True
        'BackgroundWorker1.RunWorkerAsync()

        Process.Start("C:\Program Files (x86)\Zebra Technologies\ZebraDesigner Pro 2\bin\Design.exe")
    End Sub

    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim Prg As Decimal
        Prg = 0
        ProgressBar1.Value = 0
        Process.Start("C:\Program Files (x86)\Zebra Technologies\ZebraDesigner Pro 2\bin\Design.exe")
        Dim EPC As String = ""
        Dim dsList As New DataSet
        Dim sFileName As String = ""
        For x As Integer = 0 To DataGridView1.RowCount - 1
            Prg += 100 / DataGridView1.RowCount
            BackgroundWorker1.ReportProgress(Int(Prg))
            If DataGridView1.Item(5, x).Value.ToString <> "" Then
                If IsNumeric(DataGridView1.Item(5, x).Value.ToString) And DataGridView1.Item(5, x).Value.ToString <> "0" Then
                    Dim SBU As String = DataGridView1.Item(0, x).Value.ToString.Trim
                    For y As Integer = 1 To CInt(DataGridView1.Item(5, x).Value)
                        EPC = Cek(SBU)
                        getSqldb3("insert into Temp_ListItem values ('" & EPC & "','" & DataGridView1.Item(2, x).Value & "','" & Microsoft.VisualBasic.Left(DataGridView1.Item(3, x).Value, 20) & "','" & Microsoft.VisualBasic.Mid(DataGridView1.Item(3, x).Value, 21, 20) & "','Rp. " & CDec(DataGridView1.Item(4, x).Value).ToString("N0") & "')")
                        getSqldb3("insert into Item_Master_RFID_back values ('" & DataGridView1.Item(1, x).Value & "','" & DataGridView1.Item(2, x).Value & "','" & EPC & "','','','','','0','0','0',getdate())")
                    Next
                End If
            End If
        Next
        Prg = 0
        'ProgressBar1.Value = 0
        dsList = getSqldb3("select * from Temp_ListItem")
        If dsList.Tables(0).Rows.Count > 0 Then

            Dim xl As Object
            Dim xlWorkBook As Object
            Dim xlWorksheet As Object
            Dim dsExcel As New DataSet
            Dim Opt As String = ""
            xl = CreateObject("Excel.Application")
            Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            xlWorkBook = xl.Workbooks.Add  'File doesnt exist - add a new workbook
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

            xlWorksheet = xlWorkBook.Worksheets(1)  'Work with the first worksheet

            xl.cells(1, 1) = "TCode"
            xl.cells(1, 2) = "PLU"
            xl.cells(1, 3) = "Desc 1"
            xl.cells(1, 4) = "Desc 2"
            xl.cells(1, 5) = "Price"

            Dim x As Integer = 2

            For Each ro As DataRow In dsList.Tables(0).Rows
                Prg += 100 / DataGridView1.RowCount
                BackgroundWorker1.ReportProgress(Int(Prg))
                xl.cells(x, 1) = ro("TCode")
                xl.cells(x, 2) = "'" & ro("PLU")
                xl.cells(x, 3) = ro("Desc1")
                xl.cells(x, 4) = ro("Desc2")
                xl.cells(x, 5) = ro("Price")
                x += 1
            Next
            Try
                If (Not System.IO.Directory.Exists(m_Path & Format(Now, "ddMMyyyy"))) Then
                    System.IO.Directory.CreateDirectory(m_Path & Format(Now, "ddMMyyyy"))
                End If
                'With SaveFileDialog1
                '    .Title = " List Item"
                '    .InitialDirectory = m_Path & Format(Now, "ddMMyyyy") & "\"
                '    .DefaultExt = "xlsx"
                '    .FileName = "List_Item"
                '    .Filter = "Excel 97-2003 Template (*.xls)|*.xls|Excel Workbook (*.xlsx)|*.xlsx"
                '    .FilterIndex = 2
                '    If .ShowDialog() = DialogResult.OK Then
                '        xlWorkBook.SaveAs(.FileName)
                '        sFileName = .FileName
                '    Else
                '        xlWorkBook.SaveAs(sFileName)
                '    End If
                '    'MsgBox("Success !!!")

                'End With
                If File.Exists(m_Path & "List_Item.xlsx") Then
                    File.Delete(m_Path & "List_Item.xlsx")
                End If
                xlWorkBook.SaveAs(m_Path & "List_Item.xlsx")

            Catch ex As Exception
                MsgBox(ex.Message)
            End Try
            'Save (and disconnect from) the Workbook
            xlWorkBook.Close(SaveChanges:=True)

            xlWorkBook = Nothing
            xl.Quit()                                'Close (and disconnect from) Excel
            xl = Nothing
        End If
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Button3.Enabled = True
        ProgressBar1.Visible = False
        getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Print Barcode','','Success','','" & Now & "')")
        'MsgBox("Import Successfull !!")
    End Sub
End Class