Public Class ChangePassword

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text <> Pass Then
            MsgBox("Current Password is Wrong !!!!")
            TextBox1.Focus()
            TextBox1.SelectionStart = 0
            TextBox1.SelectionLength = Len(TextBox1.Text)
            Exit Sub
        End If
        If TextBox2.Text <> TextBox3.Text Then
            MsgBox("Verify Password is Not Same !!!!")
            TextBox3.Focus()
            TextBox3.SelectionStart = 0
            TextBox3.SelectionLength = Len(TextBox3.Text)
            Exit Sub
        End If
        getSqldb("Update Users_POS Set Password = '" & TextBox2.Text.Trim & "' where User_ID = '" & usrID & "'")
        getSqldb2("Update Users_POS Set Password = '" & TextBox2.Text.Trim & "' where User_ID = '" & usrID & "'")
        Pass = TextBox2.Text.Trim
        getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Change Pass','" & Pass & "','Success','" & TextBox2.Text & "','" & Now & "')")
        MsgBox("Successfull")
        TextBox1.Clear() : TextBox2.Clear() : TextBox3.Clear() : TextBox1.Focus()
        Me.Close()
    End Sub

    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

    End Sub

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            TextBox2.Focus()
            TextBox2.SelectionStart = 0
            TextBox2.SelectionLength = Len(TextBox2.Text)
        End If
    End Sub

    Private Sub TextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            TextBox3.Focus()
            TextBox3.SelectionStart = 0
            TextBox3.SelectionLength = Len(TextBox3.Text)
        End If
    End Sub

    Private Sub TextBox3_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox3.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button1_Click(sender, e)
        End If
    End Sub

    Private Sub ChangePassword_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub ChangePassword_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            TextBox1.UseSystemPasswordChar = False
            TextBox2.UseSystemPasswordChar = False
            TextBox3.UseSystemPasswordChar = False
        Else
            TextBox1.UseSystemPasswordChar = True
            TextBox2.UseSystemPasswordChar = True
            TextBox3.UseSystemPasswordChar = True
        End If
    End Sub
End Class