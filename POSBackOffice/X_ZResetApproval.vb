Public Class X_ZResetApproval
    Dim ds As New DataSet
    Private Sub X_ZResetApproval_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Reload()
        cmb(cmbReg, "Select Cash_Register_ID,Cash_Register_ID From Cash_Register Where Branch_ID = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' ", "Cash_Register_ID", "Cash_Register_ID", 1)
        cmbReg.SelectedIndex = 0
    End Sub

    Sub Reload()
        DataGridView1.DataSource = Nothing
        ds = getSqldb2("Select Cash_Register_ID As Register_ID,Case When Spending_Program_ID = " & _
                       " Shift Then 'Yes' Else 'No' End as Approved,Shift from Cash_Register where Active_Status = 1")
        If ds.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = ds.Tables(0)
            DataGridView1.Refresh()
            DataGridView1_CellClick(DataGridView1, New DataGridViewCellEventArgs(0, 0))
        End If

    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            cmbReg.SelectedValue = DataGridView1.Item(0, e.RowIndex).Value
            If DataGridView1.Item(1, e.RowIndex).Value = "Yes" Then
                RadioButton2.Checked = True
            Else
                RadioButton1.Checked = True
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Timer1.Stop()
        If RadioButton2.Checked = True Then
            getSqldb2("Update Cash_Register Set Spending_Program_ID = Shift Where Cash_Register_ID = '" & cmbReg.SelectedValue & "'")
        Else
            getSqldb2("Update Cash_Register Set Spending_Program_ID = 0 Where Cash_Register_ID = '" & cmbReg.SelectedValue & "'")
        End If
        Reload()
        MsgBox("Updated !!!")
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Reload()
    End Sub
End Class