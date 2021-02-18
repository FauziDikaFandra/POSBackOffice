Public Class FloorOrHistory

    Private Sub FloorOrHistory_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Sub FloorOrHistory_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        rbHistory.Checked = True
        FloorHistory = 1
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If rbFloor.Checked = True Then
            FloorHistory = 0
        Else
            FloorHistory = 1
        End If
        SalesMaintenance.MdiParent = MainForm
        SalesMaintenance.Show()
        Me.Close()
    End Sub
End Class