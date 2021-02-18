Public Class UserMaintenance
    Dim ds, dscmb, dscek As New DataSet
    Dim u_Edit, t_Load As Boolean
    Private Sub UserMaintenance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        t_Load = False
        ds = getSqldb("Select * From Users_Pos")
        If ds.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = ds.Tables(0)
            DataGridView1.Columns("User_Name").Visible = False
            DataGridView1.Columns("Password").Visible = False
            DataGridView1.Columns("IsSupervisor").HeaderText = "Is Spv"
            DataGridView1.Refresh()
        End If
        cmb(cmbgroup, "Select Group_ID,Group_Name From User_Groups", "Group_ID", "Group_ID", 1)
        TextBox4.Text = DSBranch.Tables(0).Rows(0).Item("Branch_ID")
        txtBranchName.Text = DSBranch.Tables(0).Rows(0).Item("Branch_Name")
        u_Edit = False
        t_Load = True
    End Sub

    Private Sub cmbgroup_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbgroup.SelectedIndexChanged
        If t_Load = False Then
            Exit Sub
        End If
        dscmb = getSqldb("Select Group_Name from User_Groups where Group_ID = '" & cmbgroup.SelectedValue & "'")
        If dscmb.Tables(0).Rows.Count > 0 Then
            txtUserGroup.Text = dscmb.Tables(0).Rows(0).Item("Group_Name")
        Else
            txtUserGroup.Text = ""
        End If
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            cmbgroup.SelectedValue = DataGridView1.Item("Group_ID", e.RowIndex).Value
            TextBox1.Text = DataGridView1.Item("User_ID", e.RowIndex).Value
            TextBox3.Text = DataGridView1.Item("User_Name", e.RowIndex).Value
            TextBox2.Text = DataGridView1.Item("Password", e.RowIndex).Value
            If DataGridView1.Item("IsSupervisor", e.RowIndex).Value = "Y" Then
                CheckBox1.Checked = True
            Else
                CheckBox1.Checked = False
            End If
            u_Edit = True
            TextBox1.Enabled = False
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        UClear()
        u_Edit = False
        TextBox1.Enabled = True
    End Sub

    Sub UClear()
        cmbgroup.SelectedValue = ""
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox3.Clear()
        CheckBox1.Checked = False
        TextBox1.Focus()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If cmbgroup.SelectedValue = "" Or TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
            MsgBox("Data Is Incomplete !!!")
            Exit Sub
        End If
        Dim Spv As String = ""
        If CheckBox1.Checked = True Then
            Spv = "Y"
        Else
            Spv = "N"
        End If

        If u_Edit = True Then
            getSqldb("Update Users_POS Set Group_ID = '" & cmbgroup.SelectedValue & "',User_Name = '" & TextBox3.Text & "',Password = '" & TextBox2.Text & "',IsSupervisor = '" & Spv & "',Last_Update = '" & Now & "' Where User_ID = '" & TextBox1.Text & "' And Branch_ID = '" & TextBox4.Text & "' ")
            MsgBox("Update Successfull !!!")
        Else
            dscek = getSqldb("Select * from Users_POS Where User_ID = '" & TextBox1.Text & "' And Branch_ID = '" & TextBox4.Text & "'")
            If dscek.Tables(0).Rows.Count > 0 Then
                MsgBox("User ID Already Exist.. !!")
                Exit Sub
            End If
            getSqldb("Insert Into Users_POS (User_ID,Branch_ID,Group_ID,User_Name,Password,Last_Update,IsSupervisor) Values ('" & TextBox1.Text & "','" & TextBox4.Text & "','" & cmbgroup.SelectedValue & "','" & TextBox3.Text & "','" & TextBox2.Text & "','" & Now & "','" & Spv & "')")
            MsgBox("Add Data Successfull !!!")
        End If
        ds = getSqldb("Select * From Users_Pos")
        DataGridView1.DataSource = Nothing
        If ds.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = ds.Tables(0)
            DataGridView1.Columns("User_Name").Visible = False
            DataGridView1.Columns("Password").Visible = False
            DataGridView1.Columns("IsSupervisor").HeaderText = "Is Spv"
            DataGridView1.Refresh()
        End If
        u_Edit = False
        UClear()
        TextBox1.Enabled = True
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If MsgBox("Delete This Data ??", MsgBoxStyle.YesNo, "Information") Then
            getSqldb("Delete From Users_POS Where User_ID = '" & TextBox1.Text & "' And Branch_ID = '" & TextBox4.Text & "' ")
            UClear()
            MsgBox("Delete Data Successfull !!!")
            ds = getSqldb("Select * From Users_Pos")
            DataGridView1.DataSource = Nothing
            If ds.Tables(0).Rows.Count > 0 Then
                DataGridView1.DataSource = ds.Tables(0)
                DataGridView1.Columns("User_Name").Visible = False
                DataGridView1.Columns("Password").Visible = False
                DataGridView1.Columns("IsSupervisor").HeaderText = "Is Spv"
                DataGridView1.Refresh()
            End If
            u_Edit = False
            TextBox1.Enabled = True
        End If
    End Sub
End Class