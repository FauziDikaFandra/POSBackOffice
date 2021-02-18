Public Class UserPOSFrontOffice
    Dim ds, dscmb, dscek As New DataSet
    Dim u_Edit, t_Load As Boolean
    Private Sub UserPOSFrontOffice_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        t_Load = False
        ds = getSqldb("Select * From Users")
        If ds.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = ds.Tables(0)
            DataGridView1.Columns("Security_Level").Visible = False
            DataGridView1.Columns("Password").Visible = False
            DataGridView1.Refresh()
        End If
        TextBox4.Text = DSBranch.Tables(0).Rows(0).Item("Branch_ID")
        txtBranchName.Text = DSBranch.Tables(0).Rows(0).Item("Branch_Name")
        Dim c As New ArrayList
        c.Add(New CCombo("0", "Administrator"))
        c.Add(New CCombo("1", "IT Officer"))
        c.Add(New CCombo("2", "Supervisor"))
        c.Add(New CCombo("3", "Cashier"))
        With cmbgroup
            .DataSource = c
            .DisplayMember = "Number_Name"
            .ValueMember = "ID"
        End With
        u_Edit = False
        t_Load = True
    End Sub


    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            Dim cc As String = DataGridView1.Item("Security_Level", e.RowIndex).Value
            cmbgroup.SelectedValue = DataGridView1.Item("Security_Level", e.RowIndex).Value
            TextBox1.Text = DataGridView1.Item("User_ID", e.RowIndex).Value
            TextBox3.Text = DataGridView1.Item("User_Name", e.RowIndex).Value
            TextBox2.Text = DataGridView1.Item("Password", e.RowIndex).Value
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
        TextBox1.Focus()
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        If cmbgroup.SelectedValue = "" Or TextBox1.Text = "" Or TextBox2.Text = "" Or TextBox3.Text = "" Then
            MsgBox("Data Is Incomplete !!!")
            Exit Sub
        End If
        Dim Spv As String = ""
        If u_Edit = True Then
            getSqldb("Update Users Set Security_Level = '" & cmbgroup.SelectedValue & "',User_Name = '" & TextBox3.Text & "',Password = '" & TextBox2.Text & "' Where User_ID = '" & TextBox1.Text & "' And Branch_ID = '" & TextBox4.Text & "' ")
            MsgBox("Update Successfull !!!")
        Else
            dscek = getSqldb("Select * from Users Where User_ID = '" & TextBox1.Text & "' And Branch_ID = '" & TextBox4.Text & "'")
            If dscek.Tables(0).Rows.Count > 0 Then
                MsgBox("User ID Already Exist.. !!")
                Exit Sub
            End If
            getSqldb("Insert Into Users (User_ID,Branch_ID,User_Name,Password,Security_Level) Values ('" & TextBox1.Text & "','" & TextBox4.Text & "','" & TextBox3.Text & "','" & TextBox2.Text & "','" & cmbgroup.SelectedValue & "')")
            MsgBox("Add Data Successfull !!!")
        End If
        ds = getSqldb("Select * From Users")
        DataGridView1.DataSource = Nothing
        If ds.Tables(0).Rows.Count > 0 Then
            DataGridView1.DataSource = ds.Tables(0)
            DataGridView1.Columns("Security_Level").Visible = False
            DataGridView1.Columns("Password").Visible = False
            DataGridView1.Refresh()
        End If
        u_Edit = False
        UClear()
        TextBox1.Enabled = True
    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        If MsgBox("Delete This Data ??", MsgBoxStyle.YesNo, "Information") Then
            getSqldb("Delete From Users Where User_ID = '" & TextBox1.Text & "' And Branch_ID = '" & TextBox4.Text & "' ")
            UClear()
            MsgBox("Delete Data Successfull !!!")
            ds = getSqldb("Select * From Users")
            DataGridView1.DataSource = Nothing
            If ds.Tables(0).Rows.Count > 0 Then
                DataGridView1.DataSource = ds.Tables(0)
                DataGridView1.Columns("Security_Level").Visible = False
                DataGridView1.Columns("Password").Visible = False
                DataGridView1.Refresh()
            End If
            u_Edit = False
            TextBox1.Enabled = True
        End If
    End Sub
End Class