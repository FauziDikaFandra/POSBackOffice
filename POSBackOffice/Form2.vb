Imports System.ComponentModel
Imports System.Data.SqlClient
Public Class Set_Price
    Dim dsListing As New DataSet
    Dim t_Load As Boolean
    Private Sub Set_Price_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        t_Load = False
        Dim c As New ArrayList
        If m_con.State = ConnectionState.Closed Then m_con.Open()
        Dim cmd2 As New SqlCommand("select distinct class,brand As Brand from Item_Master Order by Brand", m_con)

        Dim objreader2 As SqlDataReader = cmd2.ExecuteReader()
        c.Add(New CCombo("*", "*** ALL BRAND ***"))
        Do While objreader2.Read()
            c.Add(New CCombo(Trim(objreader2("class")), Trim(objreader2("Brand").ToString)))
        Loop
        m_con.Close()
        With CmbBrand
            .DataSource = c
            .DisplayMember = "Number_Name"
            .ValueMember = "ID"
        End With

        dsListing.Clear()
        DataGridView1.DataSource = Nothing
        dsListing = getSqldb("select  distinct Top 30 e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                             "where b.Qty = 1 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                             "c where c.Qty = 3 And c.PLU = a.PLU) as [3],(select d.Price from Item_Master_listing " &
                             "d where d.Qty = 6 And d.PLU = a.PLU) as [6] from Item_Master_listing a right join " &
                             "Item_Master e on a.PLU = e.plu where e.description <> 'TIDAK AKTIF'  order by e.PLU")
        If dsListing.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = dsListing.Tables(0)
            DataGridView1.Columns(2).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(3).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(4).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(0).ReadOnly = True
            DataGridView1.Columns(1).ReadOnly = True
            DataGridView1.Columns(2).HeaderText = "Purch Price 1"
            DataGridView1.Columns(3).HeaderText = "Purch Price 3"
            DataGridView1.Columns(4).HeaderText = "Purch Price 6"

            DataGridView1.Refresh()
        End If
        CheckForIllegalCrossThreadCalls = False
        t_Load = True
    End Sub

    Sub ViewList(ByVal brand As String, ByVal plu As String)
        dsListing.Clear()
        DataGridView1.DataSource = Nothing
        If brand = "*" And plu = "" Then
            dsListing = getSqldb("select distinct Top 100 e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                             "where b.Qty = 1 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                             "c where c.Qty = 3 And c.PLU = a.PLU) as [3],(select d.Price from Item_Master_listing " &
                             "d where d.Qty = 6 And d.PLU = a.PLU) as [6] from Item_Master_listing a right join " &
                             "Item_Master e on a.PLU = e.plu where e.description <> 'TIDAK AKTIF' order by e.PLU")
        ElseIf brand <> "*" And plu = "" Then
            dsListing = getSqldb("select distinct Top 100 e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                            "where b.Qty = 1 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                            "c where c.Qty = 3 And c.PLU = a.PLU) as [3],(select d.Price from Item_Master_listing " &
                            "d where d.Qty = 6 And d.PLU = a.PLU) as [6] from Item_Master_listing a right join " &
                            "Item_Master e on a.PLU = e.plu where e.description <> 'TIDAK AKTIF'  and e.class = '" & brand & "' order by e.PLU")
        Else
            dsListing = getSqldb("select distinct Top 100 e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                           "where b.Qty = 1 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                           "c where c.Qty = 3 And c.PLU = a.PLU) as [3],(select d.Price from Item_Master_listing " &
                           "d where d.Qty = 6 And d.PLU = a.PLU) as [6] from Item_Master_listing a right join " &
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
            DataGridView1.Columns(3).HeaderText = "Purch Price 3"
            DataGridView1.Columns(4).HeaderText = "Purch Price 6"
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
            Prg = 0
            ProgressBar1.Value = 0
            For Each row As DataGridViewRow In DataGridView1.Rows

                Prg += 100 / Cnt
                If row.Cells(2).Value.ToString.Trim = "" Or row.Cells(3).Value.ToString.Trim = "" Or row.Cells(4).Value.ToString.Trim = "" Then
                    GoTo 1
                End If
                dsCek = getSqldb("select * from Item_Master_Listing where PLU = '" & row.Cells(0).Value.ToString.Trim & "'")
                Try

                Catch ex As Exception

                End Try
                If dsCek.Tables(0).Rows.Count > 0 Then
                    Try
                        setLabelTxt("Update : " & row.Cells(0).Value.ToString.Trim & " - '" & row.Cells(1).Value.ToString.Trim & "('" & row.Cells(2).Value.ToString & "','" & row.Cells(3).Value.ToString & "','" & row.Cells(4).Value.ToString & "')", Label2)
                        getSqldb("update Item_Master_Listing set Price = '" & row.Cells(2).Value.ToString.Trim & "',Start_Date = '" & DateTimePicker1.Value.Date & "' where Branch_ID = 'S012' And PLU = '" & row.Cells(0).Value.ToString.Trim & "' And Qty in (1,2)")
                        getSqldb("update Item_Master_Listing set Price = '" & row.Cells(3).Value.ToString.Trim & "',Start_Date = '" & DateTimePicker1.Value.Date & "' where Branch_ID = 'S012' And PLU = '" & row.Cells(0).Value.ToString.Trim & "' And Qty in (3,4,5)")
                        getSqldb("update Item_Master_Listing set Price = '" & row.Cells(4).Value.ToString.Trim & "',Start_Date = '" & DateTimePicker1.Value.Date & "' where Branch_ID = 'S012' And PLU = '" & row.Cells(0).Value.ToString.Trim & "' And Qty in (6)")
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try

                Else
                    Try
                        setLabelTxt("Update : " & row.Cells(0).Value.ToString.Trim & " - '" & row.Cells(1).Value.ToString.Trim & "('" & row.Cells(2).Value.ToString & "','" & row.Cells(3).Value.ToString & "','" & row.Cells(4).Value.ToString & "')", Label2)
                        getSqldb("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',1,'" & row.Cells(2).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                        getSqldb("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',2,'" & row.Cells(2).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                        getSqldb("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',3,'" & row.Cells(3).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                        getSqldb("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',4,'" & row.Cells(3).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                        getSqldb("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',5,'" & row.Cells(3).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                        getSqldb("insert into Item_Master_Listing values('S012','1','" & row.Cells(0).Value.ToString.Trim & "',6,'" & row.Cells(4).Value.ToString.Trim & "',1,'" & DateTimePicker1.Value.Date & "','2050-12-31 00:00:00')")
                    Catch ex As Exception
                        MsgBox(ex.Message)
                    End Try

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
        DataGridView1.DataSource = Nothing
        dsListing = getSqldb("select  distinct Top 30 e.PLU,e.description,(select b.Price from Item_Master_listing b " &
                             "where b.Qty = 1 And b.PLU = a.PLU) as [1] ,(select c.Price from Item_Master_listing " &
                             "c where c.Qty = 3 And c.PLU = a.PLU) as [3],(select d.Price from Item_Master_listing " &
                             "d where d.Qty = 6 And d.PLU = a.PLU) as [6] from Item_Master_listing a right join " &
                             "Item_Master e on a.PLU = e.plu order by e.PLU")
        If dsListing.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = dsListing.Tables(0)
            DataGridView1.Columns(2).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(3).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(4).DefaultCellStyle.Format = "N0"
            DataGridView1.Columns(0).ReadOnly = True
            DataGridView1.Columns(1).ReadOnly = True
            DataGridView1.Columns(2).HeaderText = "Purch Price 1"
            DataGridView1.Columns(3).HeaderText = "Purch Price 3"
            DataGridView1.Columns(4).HeaderText = "Purch Price 6"
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
End Class