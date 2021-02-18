Public Class Voucher_Desc
    Dim ds, ds2 As New DataSet
    Dim NewData As Boolean
    Private Sub Voucher_Desc_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadData()
        cek()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        NewData = True
        Clear()
    End Sub

    Sub Clear()
        TxtNo.Clear()
        txtDesc.Clear()
        txtDesc.Focus()
        cek()
    End Sub

    Sub LoadData()
        ds = getSqldb3("Select * from Voucher_Desc order by Description")
        NewData = True
        If ds.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = ds.Tables(0)
            DataGridView1.Columns(0).Width = 40
            DataGridView1.Columns(1).Width = 300
            DataGridView1.Refresh()
        End If
    End Sub

    Sub cek()
        ds2 = getSqldb("Select * from Voucher_Desc order by No Desc")
        If ds2.Tables(0).Rows.Count > 0 Then
            TxtNo.Text = ds2.Tables(0).Rows(0).Item("No") + 1
        Else
            TxtNo.Text = 1
        End If
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        getSqldb3("Delete from Voucher_Desc Where No = '" & DataGridView1(0, DataGridView1.CurrentRow.Index).Value & "'")
        MsgBox("Deleted!!!")
        Clear()
        LoadData()
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            NewData = False
            TxtNo.Text = DataGridView1(0, DataGridView1.CurrentRow.Index).Value
            txtDesc.Text = DataGridView1(1, DataGridView1.CurrentRow.Index).Value
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If NewData = True Then
            getSqldb3("Insert Into Voucher_Desc Values ('" & TxtNo.Text & "','" & txtDesc.Text & "')")
            MsgBox("Successfull!!!")
        Else
            getSqldb3("update Voucher_Desc Set Description = '" & txtDesc.Text & "' where No = '" & TxtNo.Text & "'")
            MsgBox("Updated!!!")
        End If
        LoadData()
        Clear()
    End Sub
End Class