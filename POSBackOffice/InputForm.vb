Public Class InputForm
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles cmdOKBtn.Click
        Me.Close()
    End Sub

    Private Sub InputForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'AddHandler cmdOKBtn.Click, AddressOf cmdOK2_Click
        'AddHandler FormClosing, AddressOf inputForm_closing
        Me.TopMost = True
        TextBox1.Clear()
        TextBox1.Focus()
    End Sub

    Sub cmdOK2_Click(ByVal sender As Object, ByVal e As EventArgs)
        'If TextBox1.Text = "" Then
        '    Dim vbanser As String = MsgBox("Submit empty field-?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Input Empty Field-?")
        '    If vbanser = 7 Then
        '        InputForm.ActiveControl = inptxtbox  'Promt the user if they leave the input box field blank and they click OK
        '        Exit Sub
        '    End If
        '    inptxtbox.Text = " "  'Return empty space so it will not be = Nothing
        'End If
        'InputForm.Close()
    End Sub

    Private Sub TextBox1_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Me.Close()
        End If
    End Sub
End Class