Public Class ViewTransaction
    Dim Rootnode As TreeNode = Nothing
    Dim Mainnode As TreeNode = Nothing
    Dim Childnode As TreeNode = Nothing
    Dim Childnode2 As TreeNode = Nothing
    Dim Childnode3 As TreeNode = Nothing
    Dim MainName As String = String.Empty
    'Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
    '    'Get a list of drives
    '    Dim drives As System.Collections.ObjectModel.ReadOnlyCollection(Of IO.DriveInfo) = My.Computer.FileSystem.Drives
    '    Dim rootDir As String = String.Empty
    '    'Now loop thru each drive and populate the treeview
    '    For i As Integer = 0 To drives.Count - 1
    '        rootDir = drives(i).Name
    '        'Add this drive as a root node
    '        TreeView1.Nodes.Add(rootDir)
    '        'Populate this root node
    '        PopulateTreeView(rootDir, TreeView1.Nodes(i))
    '    Next
    'End Sub

    'Private Sub PopulateTreeView(ByVal dir As String, ByVal parentNode As TreeNode)
    '    Dim folder As String = String.Empty
    '    Try
    '        Dim folders() As String = IO.Directory.GetDirectories(dir)
    '        If folders.Length <> 0 Then
    '            Dim childNode As TreeNode = Nothing
    '            For Each folder In folders
    '                childNode = New TreeNode(folder)
    '                parentNode.Nodes.Add(childNode)
    '                PopulateTreeView(folder, childNode)
    '            Next
    '        End If
    '    Catch ex As UnauthorizedAccessException
    '        parentNode.Nodes.Add(folder & ": Access Denied")
    '    End Try
    'End Sub

    

    Private Delegate Sub setLabelTxtInvoker(ByVal text As String, ByVal lbl As TreeView)

    Private Sub GroupBox1_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox1.Enter

    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Button1.Enabled = False
        ''TreeView1.Enabled = False
        'ProgressBar1.Visible = True
        'BackgroundWorker1.WorkerReportsProgress = True
        'BackgroundWorker1.WorkerSupportsCancellation = True
        'BackgroundWorker1.RunWorkerAsync()

        Dim ds, ds2, ds3, ds4 As New DataSet
        ds = getSqldb("select distinct substring(trans_no,5,3) as regNo from slip where left(trans_no,4) = 'S001' and substring(trans_no,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' order by regNo")
        If ds.Tables(0).Rows.Count > 0 Then
            TreeView1.Nodes.Clear()
            Dim myImageList As New ImageList()
            myImageList.Images.Add(Image.FromFile("2.ico"))
            myImageList.Images.Add(Image.FromFile("1.ico"))
            myImageList.Images.Add(Image.FromFile("3.ico"))

            TreeView1.ImageList = myImageList

            TreeView1.ImageIndex = 0
            TreeView1.SelectedImageIndex = 2

            Rootnode = TreeView1.Nodes.Add(key:="Root", text:="Cash Register", _
            imageIndex:=0, selectedImageIndex:=0)
            Rootnode.Tag = "Rootnode"
            For Each row As DataRow In ds.Tables(0).Rows
                'Prg += (100 / ds.Tables(0).Rows.Count)
                'BackgroundWorker1.ReportProgress(Int(Prg))
                'If MainName <> row(0).ToString Then
                Mainnode = Rootnode.Nodes.Add(key:=row(0).ToString, text:=row(0).ToString, _
                imageIndex:=0, selectedImageIndex:=0)
                Mainnode.Tag = "Mainnode"
                ds2 = getSqldb("select trans_no,cashier_id,xread from slip where left(trans_no,16) = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "" & row(0).ToString & "-" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' order by trans_no")
                For Each row2 As DataRow In ds2.Tables(0).Rows
                    Childnode = Mainnode.Nodes.Add(key:=row2(0).ToString, text:=row2(0).ToString & Space(21 - row2(0).ToString.Length) & row2(1).ToString & Space(14 - row2(1).ToString.Length) & CDec(row2(2)).ToString("N0"), _
                imageIndex:=0, selectedImageIndex:=0)
                    Childnode.Tag = "Childnode"
                    'ds3 = getSqldb("select trans_no,payment_types,description,Paid_Amount from slip_pay where trans_no = '" & row2(0).ToString & "' order by trans_no")
                    'For Each row3 As DataRow In ds3.Tables(0).Rows
                    '    Childnode2 = Childnode.Nodes.Add(key:=row3(0).ToString, text:=row3(1).ToString & Space(4 - row3(1).ToString.Length) & row3(2).ToString + Space(30 - row3(2).ToString.Length) + CDec(row3(3)).ToString("N0"), _
                    'imageIndex:=0, selectedImageIndex:=0)
                    '    ds4 = getSqldb("select transaction_number,paid_amount from paid where left(transaction_number,16)='" & Microsoft.VisualBasic.Left(row3(0), 16) & "' and shift = '" & Microsoft.VisualBasic.Right(row3(0), 1) & "' and payment_types = '" & row3(1).ToString & "' order by transaction_number")
                    '    For Each row4 As DataRow In ds4.Tables(0).Rows
                    '        Childnode3 = Childnode2.Nodes.Add(key:=row4(0).ToString, text:=row4(0).ToString + Space(32 - row4(0).ToString.Length) + CDec(row4(1)).ToString("N0"), _
                    '    imageIndex:=1, selectedImageIndex:=1)
                    '    Next
                    'Next
                Next
                MainName = row(0).ToString
                'End If
            Next
            'TreeView1.ExpandAll()
        Else
            MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
        End If
        Button1.Enabled = True
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim Prg As Decimal
        Prg = 0
        ProgressBar1.Value = 0
      
       
        Dim ds, ds2, ds3, ds4 As New DataSet
        ds = getSqldb("select distinct substring(trans_no,5,3) as regNo from slip where left(trans_no,4) = 'S001' and substring(trans_no,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' order by regNo")
        If ds.Tables(0).Rows.Count > 0 Then
            TreeView1.Nodes.Clear()
            Dim myImageList As New ImageList()
            myImageList.Images.Add(Image.FromFile("2.ico"))
            myImageList.Images.Add(Image.FromFile("1.ico"))
            myImageList.Images.Add(Image.FromFile("3.ico"))

            TreeView1.ImageList = myImageList

            TreeView1.ImageIndex = 0
            TreeView1.SelectedImageIndex = 2

            Rootnode = TreeView1.Nodes.Add(key:="Root", text:="Cash Register", _
            imageIndex:=0, selectedImageIndex:=0)
            Rootnode.Tag = "Rootnode"
            For Each row As DataRow In ds.Tables(0).Rows
                If row(0).ToString = "210" Then
                    MsgBox("")
                End If

                'Label2.Text = row(0).ToString
                Dim c As String = row(0).ToString
                Prg += (100 / ds.Tables(0).Rows.Count)
                If Prg < 100 Then
                    BackgroundWorker1.ReportProgress(Int(Prg))
                End If
                ProgressBar1.Refresh()
                'If MainName <> row(0).ToString Then
                Mainnode = Rootnode.Nodes.Add(key:=row(0).ToString, text:=row(0).ToString, _
                imageIndex:=0, selectedImageIndex:=0)
                Mainnode.Tag = "Mainnode"
                ds2 = getSqldb("select trans_no,cashier_id,xread from slip where left(trans_no,16) = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "" & row(0).ToString & "-" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' order by trans_no")
                For Each row2 As DataRow In ds2.Tables(0).Rows
                    Childnode = Mainnode.Nodes.Add(key:=row2(0).ToString, text:=row2(0).ToString & Space(21 - row2(0).ToString.Length) & row2(1).ToString & Space(14 - row2(1).ToString.Length) & CDec(row2(2)).ToString("N0"), _
                imageIndex:=0, selectedImageIndex:=0)
                    ds3 = getSqldb("select trans_no,payment_types,description,Paid_Amount from slip_pay where trans_no = '" & row2(0).ToString & "' order by trans_no")
                    For Each row3 As DataRow In ds3.Tables(0).Rows
                        Childnode2 = Childnode.Nodes.Add(key:=row3(0).ToString, text:=row3(1).ToString & Space(4 - row3(1).ToString.Length) & row3(2).ToString + Space(30 - row3(2).ToString.Length) + CDec(row3(3)).ToString("N0"), _
                    imageIndex:=0, selectedImageIndex:=0)
                        ds4 = getSqldb("select transaction_number,paid_amount from paid where left(transaction_number,16)='" & Microsoft.VisualBasic.Left(row3(0), 16) & "' and shift = '" & Microsoft.VisualBasic.Right(row3(0), 1) & "' and payment_types = '" & row3(1).ToString & "' order by transaction_number")
                        For Each row4 As DataRow In ds4.Tables(0).Rows
                            Childnode3 = Childnode2.Nodes.Add(key:=row4(0).ToString, text:=row4(0).ToString + Space(32 - row4(0).ToString.Length) + CDec(row4(1)).ToString("N0"), _
                        imageIndex:=1, selectedImageIndex:=1)
                        Next
                    Next
                Next
                MainName = row(0).ToString
                'End If
            Next
            'TreeView1.ExpandAll()
        Else
            MsgBox("No Result!!!", MsgBoxStyle.Information, "Information")
        End If
    End Sub

    Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        ProgressBar1.Visible = False
        Button1.Enabled = True

        Try
        Catch ex As Exception
        End Try
        'TreeView1.Enabled = True
        'BackgroundWorker1.CancelAsync()
        BackgroundWorker1.Dispose()
        'Me.Refresh()
    End Sub

    Private Sub ViewTransaction_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CheckForIllegalCrossThreadCalls = False
        txtCustID.Text = DSBranch.Tables(0).Rows(0).Item("Branch_ID")
    End Sub

    Private Sub TreeView1_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseClick
        If e.Node.Tag = "Childnode3" Then
            Dim ds4 As New DataSet
            ds4 = getSqldb("select Seq,PLU,Item_Description,Price,Qty,Amount,Net_Price from Sales_Transaction_Details where transaction_number ='" & e.Node.Name.ToString & "'  order by Seq")
            If ds4.Tables(0).Rows.Count > 0 Then
                DataGridView1.DataSource = ds4.Tables(0)
                DataGridView1.Columns("Seq").DefaultCellStyle.Format = "N0"
                DataGridView1.Columns("Price").DefaultCellStyle.Format = "N0"
                DataGridView1.Columns("Qty").DefaultCellStyle.Format = "N0"
                DataGridView1.Columns("Amount").DefaultCellStyle.Format = "N0"
                DataGridView1.Columns("Net_Price").DefaultCellStyle.Format = "N0"
                DataGridView1.Refresh()
            End If
        End If
    End Sub


    Private Sub TreeView1_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView1.NodeMouseDoubleClick
        If e.Node.Tag = "Childnode" Then
            Try
                If e.Node.Nodes(0).Text <> "" Then
                    Exit Sub
                End If
                Exit Sub
            Catch ex As Exception

            End Try

            Dim ds3 As New DataSet
            ds3 = getSqldb("select trans_no,payment_types,description,Paid_Amount from slip_pay where trans_no = '" & e.Node.Name.ToString & "' order by payment_types")
            For Each row3 As DataRow In ds3.Tables(0).Rows
                Childnode2 = TreeView1.SelectedNode.Nodes.Add(key:=row3(0).ToString, text:=row3(1).ToString & Space(4 - row3(1).ToString.Length) & row3(2).ToString + Space(30 - row3(2).ToString.Length) + CDec(row3(3)).ToString("N0"), _
            imageIndex:=0, selectedImageIndex:=0)
                Childnode2.Tag = "Childnode2"
            Next
        ElseIf e.Node.Tag = "Childnode2" Then
            Try
                If e.Node.Nodes(0).Text <> "" Then
                    Exit Sub
                End If
                Exit Sub
            Catch ex As Exception

            End Try
            Dim ds4 As New DataSet
            ds4 = getSqldb("select transaction_number,paid_amount from paid where left(transaction_number,16)='" & Microsoft.VisualBasic.Left(e.Node.Name.ToString, 16) & "' and shift = '" & Microsoft.VisualBasic.Right(e.Node.Name.ToString, 1) & "' and payment_types = '" & CInt(Microsoft.VisualBasic.Left(e.Node.Text, 2)) & "' order by transaction_number")
            For Each row4 As DataRow In ds4.Tables(0).Rows
                Childnode3 = TreeView1.SelectedNode.Nodes.Add(key:=row4(0).ToString, text:=row4(0).ToString + Space(33 - row4(0).ToString.Length) + CDec(row4(1)).ToString("N0"), _
            imageIndex:=1, selectedImageIndex:=1)
                Childnode3.Tag = "Childnode3"
            Next
        End If
    End Sub
End Class