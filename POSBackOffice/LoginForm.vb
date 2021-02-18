Imports System.IO
Public Class LoginForm
    Dim ds As New DataSet

    Private Sub LoginForm_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub
    Private Sub LoginForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConnectServer()
        m_Sqlconn = "Data Source=" & m_ServerName & ";" & "Initial Catalog=" & m_DBName & ";" & "User ID=" & m_UserName & ";" & "Password=" & m_Password & ";"
        m_Sqlconn2 = "Data Source=" & m_ServerName2 & ";" & "Initial Catalog=" & m_DBName2 & ";" & "User ID=" & m_UserName2 & ";" & "Password=" & m_Password2 & ";"
        m_Sqlconn3 = "Data Source=" & m_ServerName3 & ";" & "Initial Catalog=" & m_DBName3 & ";" & "User ID=" & m_UserName3 & ";" & "Password=" & m_Password3 & ";"
        If File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IDPOS.txt") Then
            Using sr As New StreamReader(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IDPOS.txt")
                Dim line As String
                line = sr.ReadToEnd
                TextBox1.Text = line
            End Using
            TextBox2.Select()
            TextBox2.Focus()
        End If
    End Sub

    Sub TestDataAdapter()
        m_con2 = New System.Data.SqlClient.SqlConnection(m_Sqlconn2)
        Dim da As New System.Data.SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim cmd As New System.Data.SqlClient.SqlCommand
        Dim tblTemp As DataTable
        Dim editrecord As DataRow

        cmd = m_con2.CreateCommand
        cmd.CommandText = "Select * from Users_POS where user_ID = '" & TextBox1.Text.Trim & "' And Password = '" & TextBox2.Text.Trim & "'"
        cmd.CommandTimeout = 120
        da.SelectCommand = cmd
        If m_con2.State = ConnectionState.Open Then
            m_con2.Close()
        End If
        m_con2.Open()
1:
        Try
            da.Fill(ds)
            tblTemp = ds.Tables(0)
            editrecord = tblTemp.Rows(0)
            editrecord.BeginEdit()
            editrecord("Password") = "XXX"
            editrecord.EndEdit()
            Dim ObjCmdBuil As New System.Data.SqlClient.SqlCommandBuilder(da)
            da.Update(tblTemp)
        Catch ex As Exception
            'GoTo 1
            MsgBox(ex.Message)
        End Try
        m_con2.Close()
    End Sub

    Sub TestDataAdapter2()
        m_con2 = New System.Data.SqlClient.SqlConnection(m_Sqlconn2)
        Dim da As New System.Data.SqlClient.SqlDataAdapter
        Dim ds As New DataSet
        Dim cmd As New System.Data.SqlClient.SqlCommand

        cmd = m_con2.CreateCommand
        cmd.CommandText = "Select * from Users_POS where user_ID = '" & TextBox1.Text.Trim & "' And Password = '" & TextBox2.Text.Trim & "'"
        cmd.CommandTimeout = 120
        da.SelectCommand = cmd
        If m_con2.State = ConnectionState.Open Then
            m_con2.Close()
        End If
        m_con2.Open()
1:
        Try
            Dim ObjCmdBuil As New System.Data.SqlClient.SqlCommandBuilder(da)
            da.Fill(ds)

            For Each ro As DataRow In ds.Tables(0).Rows
                ro("password") = "1"
            Next
            da.Update(ds)
        Catch ex As Exception
            'GoTo 1
            MsgBox(ex.Message)
        End Try
        m_con2.Close()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'TestDataAdapter2()
        'Exit Sub
        ds = getSqldb2("Select * from Users_POS where user_ID = '" & TextBox1.Text.Trim & "' And Password = '" & TextBox2.Text.Trim & "'")
        DSBranch = getSqldb2("Select * from Branches")
        If ds.Tables(0).Rows.Count > 0 Then
            TextBox2.Clear()
            usrID = TextBox1.Text.Trim
            UserName = ds.Tables(0).Rows(0).Item("User_Name").ToString.Trim
            Pass = ds.Tables(0).Rows(0).Item("Password").ToString.Trim
            V_Code = DSBranch.Tables(0).Rows(0).Item("v_code").ToString.Trim
            Group_ID = ds.Tables(0).Rows(0).Item("Group_ID").ToString.Trim
            If File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IDPOS.txt") Then
                File.Delete(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IDPOS.txt")
            End If
            Dim sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IDPOS.txt")
            sw.Write(TextBox1.Text.Trim)
            sw.Close()
            sw.Dispose()
            getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Login','','Success','','" & Now & "')")
            Me.Hide()
            MainForm.Show()
        Else
            MsgBox("ID Tidak TerDaftar!!!")
            getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Login','','Failed','','" & Now & "')")
            Clear()
            Exit Sub
        End If
    End Sub


    Sub Clear()
        TextBox1.Clear()
        TextBox2.Clear()
        TextBox1.Focus()
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
            Button1_Click(sender, e)
        End If
    End Sub




End Class
