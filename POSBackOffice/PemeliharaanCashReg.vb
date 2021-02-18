Public Class PemeliharaanCashReg
    Dim ds, ds1, dscek As New DataSet
    Dim regid, rfrom, rto As String
    Dim edit As Boolean
    Private Sub PemeliharaanCashReg_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        lv()
        lv2()
        ds = getSqldb("select * from reg_loc order by reg_id")
        If ds.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In ds.Tables(0).Rows
                Dim str(0) As String
                Dim itm As ListViewItem
                str(0) = ro(0) & "  " & ro(1)
                itm = New ListViewItem(str)
                ListView1.Items.Add(itm)
            Next
            regid = ds.Tables(0).Rows(0).Item("reg_id")
            ds1 = getSqldb("SELECT * FROM [reg_loc2] WHERE [reg_id]='" & ds.Tables(0).Rows(0).Item("reg_id") & "'")
            If ds1.Tables(0).Rows.Count > 0 Then
                For Each ro As DataRow In ds1.Tables(0).Rows
                    Dim str(0) As String
                    Dim itm As ListViewItem
                    str(0) = ro(1) & " - " & ro(2)
                    itm = New ListViewItem(str)
                    ListView2.Items.Add(itm)
                Next
            End If
            ListView1.Items(0).Selected = True
        End If
    End Sub

    Sub lv()
        ListView1.Columns.Add("", 200, HorizontalAlignment.Left)
    End Sub
    Sub lv2()
        ListView2.Columns.Add("", 100, HorizontalAlignment.Left)
    End Sub

    Private Sub ListView2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView2.Click
        Dim I As Integer
        For I = 0 To ListView2.SelectedItems.Count - 1
            rfrom = Microsoft.VisualBasic.Left(ListView2.SelectedItems(I).SubItems(0).Text, 3)
            rto = Microsoft.VisualBasic.Left(ListView2.SelectedItems(I).SubItems(0).Text, 3)
            TextBox1.Text = Microsoft.VisualBasic.Left(ListView2.SelectedItems(I).SubItems(0).Text, 3)
            TextBox2.Text = Microsoft.VisualBasic.Right(ListView2.SelectedItems(I).SubItems(0).Text, 3)
            Exit For
        Next
    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            TextBox2.Focus()
        End If
    End Sub

    Private Sub TextBox1_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox1.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button3_Click(sender, e)
        End If
    End Sub


    Private Sub TextBox2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox2.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        clearT()
    End Sub

    Sub clearT()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox1.Focus()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If MsgBox("Save Setting ?", MsgBoxStyle.YesNo, "Information") = MsgBoxResult.Yes Then
            dscek = getSqldb("Select * From [reg_loc2] WHERE [reg_id] ='" & regid & "' And regFrom ='" & rfrom & "'  And regTo ='" & rto & "'")
            getSqldb("Insert Into [reg_loc2] values ('" & regid & "','" & TextBox1.Text & "','" & TextBox2.Text & "')")
            reload()
            clearT()
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If MsgBox("Delete ?", MsgBoxStyle.YesNo, "Information") = MsgBoxResult.Yes Then
            Dim I As Integer
            For I = 0 To ListView2.SelectedItems.Count - 1
                getSqldb("Delete From [reg_loc2] WHERE [reg_id] ='" & regid & "' And regFrom ='" & Microsoft.VisualBasic.Left(ListView2.SelectedItems(I).SubItems(0).Text, 3) & "'  And regTo ='" & Microsoft.VisualBasic.Right(ListView2.SelectedItems(I).SubItems(0).Text, 3) & "'")
                reload()
                Exit For
            Next
            clearT()
        End If
    End Sub

    Sub reload()
        ds1 = getSqldb("SELECT * FROM [reg_loc2] WHERE [reg_id]='" & regid & "'")
        If ds1.Tables(0).Rows.Count > 0 Then
            ListView2.Items.Clear()
            For Each ro As DataRow In ds1.Tables(0).Rows
                Dim str(0) As String
                Dim itm As ListViewItem
                str(0) = ro(1) & " - " & ro(2)
                itm = New ListViewItem(str)
                ListView2.Items.Add(itm)
            Next
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim I As Integer
        For I = 0 To ListView2.SelectedItems.Count - 1
            TextBox1.Text = Microsoft.VisualBasic.Left(ListView2.SelectedItems(I).SubItems(0).Text, 3)
            TextBox2.Text = Microsoft.VisualBasic.Right(ListView2.SelectedItems(I).SubItems(0).Text, 3)
            Exit For
        Next
    End Sub

    Private Sub ListView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.Click
        reload()
    End Sub

    Private Sub ListView2_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView2.SelectedIndexChanged
        Dim I As Integer
        For I = 0 To ListView2.SelectedItems.Count - 1
            rfrom = Microsoft.VisualBasic.Left(ListView2.SelectedItems(I).SubItems(0).Text, 3)
            rto = Microsoft.VisualBasic.Left(ListView2.SelectedItems(I).SubItems(0).Text, 3)
            TextBox1.Text = Microsoft.VisualBasic.Left(ListView2.SelectedItems(I).SubItems(0).Text, 3)
            TextBox2.Text = Microsoft.VisualBasic.Right(ListView2.SelectedItems(I).SubItems(0).Text, 3)
            Exit For
        Next
    End Sub
End Class