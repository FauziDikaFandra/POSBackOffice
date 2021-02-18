Public Class AddVoucher

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

        If Me.Text = "Add Voucher" Then
            Try
                SellingGiftVoucher.DataGridView1.Rows.Add(1)
                SellingGiftVoucher.DataGridView1.Item(0, SellingGiftVoucher.DataGridView1.Rows.Count - 1).Value = V_Code
                SellingGiftVoucher.DataGridView1.Item(1, SellingGiftVoucher.DataGridView1.Rows.Count - 1).Value = TextBox1.Text
                SellingGiftVoucher.DataGridView1.Item(2, SellingGiftVoucher.DataGridView1.Rows.Count - 1).Value = TextBox2.Text
                SellingGiftVoucher.DataGridView1.Item(3, SellingGiftVoucher.DataGridView1.Rows.Count - 1).Value = TextBox3.Text
            Catch ex As Exception

            End Try
        Else
            Try
                SellingGiftVoucher.DataGridView1.Item(0, CInt(Label5.Text)).Value = V_Code
                SellingGiftVoucher.DataGridView1.Item(1, CInt(Label5.Text)).Value = TextBox1.Text
                SellingGiftVoucher.DataGridView1.Item(2, CInt(Label5.Text)).Value = TextBox2.Text
                SellingGiftVoucher.DataGridView1.Item(3, CInt(Label5.Text)).Value = TextBox3.Text
            Catch ex As Exception

            End Try
        End If
        SellingGiftVoucher.ForTotDg()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        TextBox1.Focus()
        Me.Close()
    End Sub

    Private Sub AddVoucher_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub



    Private Sub TextBox3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox3.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button1_Click(sender, e)
        End If
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
            TextBox2.Text = TextBox1.Text
            TextBox2.SelectionStart = TextBox2.TextLength
        End If
    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            TextBox3.Focus()
        End If
    End Sub

End Class