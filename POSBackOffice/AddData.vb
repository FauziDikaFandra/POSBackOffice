Public Class AddData

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Then
            MsgBox("invalid Entry")
            Exit Sub
        End If
        If TextBox2.Text = "" Then
            MsgBox("invalid Entry")
            Exit Sub
        End If
        If TextBox3.Text = "" Then
            MsgBox("invalid Entry")
            Exit Sub
        End If
        If Me.Text = "Add Data" Then
            Try
                VoucherReceiveDtl.DataGridView1.Rows.Add(1)
                VoucherReceiveDtl.DataGridView1.Item(0, VoucherReceiveDtl.DataGridView1.Rows.Count - 1).Value = Label4.Text
                VoucherReceiveDtl.DataGridView1.Item(1, VoucherReceiveDtl.DataGridView1.Rows.Count - 1).Value = TextBox1.Text
                VoucherReceiveDtl.DataGridView1.Item(2, VoucherReceiveDtl.DataGridView1.Rows.Count - 1).Value = TextBox3.Text
                'getSqldb("Insert Into vc_pay values('" & Trans_No_v & "','" & Label4.Text & "','" & TextBox1.Text & "','" & TextBox3.Text & "')")
            Catch ex As Exception

            End Try
        Else
            Try
                VoucherReceiveDtl.DataGridView1.Item(0, CInt(Label5.Text)).Value = Label4.Text
                VoucherReceiveDtl.DataGridView1.Item(1, CInt(Label5.Text)).Value = TextBox1.Text
                VoucherReceiveDtl.DataGridView1.Item(2, CInt(Label5.Text)).Value = TextBox3.Text
                'getSqldb("Update vc_pay Set v_code = '" & Label4.Text & "', v_amt = '" & CDec(TextBox3.Text) & "' where v_no = '" & TextBox1.Text & "' and trans_no = '" & Trans_No_v & "'")
            Catch ex As Exception

            End Try
        End If
        VoucherReceiveDtl.ForTotDg()
        Me.Close()
    End Sub

    Private Sub AddData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TextBox3_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles TextBox3.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub TextBox3_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox3.TextChanged
        If TextBox3.Text.Length = 0 Then
            TextBox3.Text = 0
        End If
        If TextBox3.Text.Length > 3 Then
            TextBox3.Text = CDec(TextBox3.Text).ToString("N0")
            TextBox3.SelectionStart = TextBox3.TextLength
        End If
    End Sub


    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            TextBox2.Focus()
        End If
    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            TextBox3.Focus()
        End If
    End Sub
End Class