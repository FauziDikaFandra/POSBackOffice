Public Class Setting_Voucher
    Dim ds, dsUrut, DsCek As New DataSet
    Dim no As Integer
    Dim desc As String
    Dim edit As Boolean
    Private Sub Setting_Voucher_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ds = getSqldb("Select * from Voucher_Desc Order By No")
        If ds.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = ds.Tables(0)
            DataGridView1.Columns(0).Width = 40
            DataGridView1.Columns(1).Width = 180
            DataGridView1.Refresh()
        End If
        dsUrut = getSqldb("Select * from Voucher_Desc Order By No Desc")
        If dsUrut.Tables(0).Rows.Count > 0 Then
            txtNo.Text = dsUrut.Tables(0).Rows(0).Item("No") + 1
        Else
            txtNo.Text = 1
        End If
        txtDescription.Clear()
        txtDescription.Focus()
        edit = False
        desc = ""
        Button1.Enabled = False
        Button6.Enabled = False
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            edit = True
            no = DataGridView1.Item(0, e.RowIndex).Value
            desc = DataGridView1.Item(1, e.RowIndex).Value
            txtNo.Text = DataGridView1.Item(0, e.RowIndex).Value
            txtDescription.Text = DataGridView1.Item(1, e.RowIndex).Value
            Button1.Enabled = True
            Button6.Enabled = True
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        dsUrut = getSqldb("Select * from Voucher_Desc Order By [No.] Desc")
        If dsUrut.Tables(0).Rows.Count > 0 Then
            txtNo.Text = dsUrut.Tables(0).Rows(0).Item("No.") + 1
        Else
            txtNo.Text = 1
        End If
        txtDescription.Clear()
        txtDescription.Focus()
        edit = False
        desc = ""
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If MsgBox("Hapus Data Berikut ??", MsgBoxStyle.YesNo, "Attention") = MsgBoxResult.Yes Then
            getSqldb("Delete from Voucher_Desc where [No.] = '" & no & "'")
            Setting_Voucher_Load(sender, e)
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If edit = True Then
            getSqldb("Update  Voucher_Desc Set Description = '" & txtDescription.Text & "' where [No.] = '" & txtNo.Text & "' And Description = '" & desc & "'")
        Else
            DsCek = getSqldb("Select * from Voucher_Desc where Description = '" & txtDescription.Text & "'")
            If DsCek.Tables(0).Rows.Count > 0 Then
                MsgBox("Description '" & txtDescription.Text & "' Sudah Ada !!")
                Exit Sub
            Else
                getSqldb("Insert into Voucher_Desc ([No.],Description) Values ('" & txtNo.Text & "','" & txtDescription.Text & "')")
                MsgBox("Berhasil")
            End If
        End If
        Setting_Voucher_Load(sender, e)
    End Sub
End Class