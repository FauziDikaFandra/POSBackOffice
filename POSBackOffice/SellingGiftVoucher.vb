Public Class SellingGiftVoucher
    Dim TotDg As Decimal

    Private Sub txtAmnt_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtAmnt.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtAmnt.Text <> "" Then
                txtDisc.Focus()
            End If
        End If
    End Sub

    Private Sub txtAmnt_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAmnt.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub SellingGiftVoucher_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RadioButton1.Checked = True
        GroupBox3.Enabled = False
        DateTimePicker2.Value = Now.Month & "/" & Now.Day & "/2050"
        cmb3(cmbDesc, "Select * from Voucher_Desc Order By Description", "No.", "Description", 1)
        cmbDesc.SelectedIndex = 0
        txtDescDtl.Focus()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If cmbDesc.SelectedValue = "" Then
            MsgBox("Invalid Entry !!")
            cmbDesc.Focus()
            Exit Sub
        End If
        If txtDescDtl.Text.Trim = "" Then
            MsgBox("Invalid Entry !!")
            txtDescDtl.Focus()
            Exit Sub
        End If
        If txtAmnt.Text.Trim = "" Then
            MsgBox("Invalid Entry !!")
            txtAmnt.Focus()
            Exit Sub
        End If
        If txtDisc.Text.Trim = "" Then
            MsgBox("Invalid Entry !!")
            txtDisc.Focus()
            Exit Sub
        End If
        If txtDics2.Text.Trim = "" Then
            MsgBox("Invalid Entry !!")
            txtDics2.Focus()
            Exit Sub
        End If
        Label5.Text = (CDec(txtAmnt.Text) - CDec(Label4.Text)).ToString("N0")
        GroupBox3.Enabled = True
        ForTotDg()
    End Sub

    Private Sub txtAmnt_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAmnt.TextChanged
        If txtAmnt.Text.Length = 0 Then
            txtAmnt.Text = 0
        End If
        If txtAmnt.Text.Length > 3 Then
            txtAmnt.Text = CDec(txtAmnt.Text).ToString("N0")
            txtAmnt.SelectionStart = txtAmnt.TextLength
        End If
    End Sub

    Private Sub txtDics2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDics2.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtDics2.Text.Length = 0 Then
                txtDics2.Text = 0
            End If
            Button1_Click(sender, e)
            Button2.Focus()
        End If
    End Sub

    Private Sub txtDics2_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDics2.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub txtDisc_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDisc.KeyDown
        If e.KeyCode = Keys.Enter Then
            txtDics2.Focus()
        End If
    End Sub

    Private Sub txtDisc_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtDisc.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub txtDics2_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDics2.TextChanged
        If txtDics2.Text.Length = 0 Then
            txtDics2.Text = 0
        End If
        If txtDics2.Text.Length > 3 Then
            txtDics2.Text = CDec(txtDics2.Text).ToString("N0")
            txtDics2.SelectionStart = txtDics2.TextLength
        End If
    End Sub

    Private Sub txtDisc_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDisc.Leave
        If txtDisc.Text.Length = 0 Then
            txtDisc.Text = 0
        End If
    End Sub

    Private Sub txtDics2_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDics2.Leave
        If txtDics2.Text.Length = 0 Then
            txtDics2.Text = 0
        End If
    End Sub

    Private Sub txtDisc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDisc.TextChanged
        If txtDisc.Text.Length = 0 Then
            txtDisc.Text = 0
        End If
        If txtDisc.Text.Length > 3 Then
            txtDisc.Text = CDec(txtDisc.Text).ToString("N0")
            txtDisc.SelectionStart = txtDisc.TextLength
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        AddVoucher.Text = "Add Voucher"
        AddVoucher.TextBox1.Enabled = True
        AddVoucher.TextBox2.Enabled = True
        AddVoucher.TextBox1.Focus()
        AddVoucher.Show()
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        AddVoucher.Text = "Edit Voucher"
        AddVoucher.Label4.Text = V_Code
        AddVoucher.Label5.Text = DataGridView1.CurrentRow.Index
        AddVoucher.TextBox1.Enabled = False
        AddVoucher.TextBox2.Enabled = False
        AddVoucher.TextBox1.Text = DataGridView1.Item(1, DataGridView1.CurrentRow.Index).Value
        AddVoucher.TextBox2.Text = DataGridView1.Item(2, DataGridView1.CurrentRow.Index).Value
        AddVoucher.TextBox3.Text = DataGridView1.Item(3, DataGridView1.CurrentRow.Index).Value
        AddVoucher.TextBox1.Focus()
        AddVoucher.Show()
    End Sub

    Sub ForTotDg()
        Dim I As Integer
        TotDg = 0
        Dim b As Integer = 0
        For I = 0 To DataGridView1.Rows.Count - 1
            'If I > 0 Then
            '    b = 1
            'Else
            '    b = 0
            'End If
            For x = b To CInt(Microsoft.VisualBasic.Right(DataGridView1.Item(2, I).Value, 8)) - CInt(Microsoft.VisualBasic.Right(DataGridView1.Item(1, I).Value, 8))
                TotDg += CDec(DataGridView1.Item(3, I).Value)
                'Dim c As Integer = CInt(Microsoft.VisualBasic.Right(DataGridView1.Item(2, I).Value, 8))
                'c = CInt(Microsoft.VisualBasic.Right(DataGridView1.Item(1, I).Value, 8))
                'c = CInt(Microsoft.VisualBasic.Right(DataGridView1.Item(2, I).Value, 8)) - CInt(Microsoft.VisualBasic.Right(DataGridView1.Item(1, I).Value, 8))
            Next
            'MsgBox(ListView1.SelectedItems(I).SubItems(2).Text)
        Next
        Label4.Text = CDec(TotDg).ToString("N0")
        Label5.Text = (CDec(txtAmnt.Text) - CDec(Label4.Text)).ToString("N0")
        If CDec(Label5.Text) = 0 Then
            Label5.ForeColor = Color.Black
        ElseIf CDec(Label5.Text) > 0 Then
            Label5.ForeColor = Color.Green
        Else
            Label5.Text = Replace((CDec(txtAmnt.Text) - CDec(Label4.Text)).ToString("N0"), "-", "") & "-"
            Label5.ForeColor = Color.Red
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        'getSqldb("DELETE [vc_pay]  WHERE [trans_no]='" & Trans_No_v & "'")
        If MsgBox("Save Data ??", MsgBoxStyle.YesNo, "Information") = MsgBoxResult.No Then
            Exit Sub
        End If
        If CDec(Label5.Text) <> 0 Then
            MsgBox("Out Of Balance Must Be 0 !!!", MsgBoxStyle.Exclamation)
            Exit Sub
        End If
        Dim dsPar, dsCek As New DataSet
        Dim int_ref As Integer
        Dim v_ref As String = ""
        For s As Integer = 0 To DataGridView1.Rows.Count - 1
            dsCek = getSqldb("Select * from NewVoc Where v_no between '" & DataGridView1.Item(1, s).Value & "' and '" & DataGridView1.Item(2, s).Value & "'")
            If dsCek.Tables(0).Rows.Count = 0 Then
                MsgBox("Voucher '" & DataGridView1.Item(1, s).Value & "' DOES NOT EXIST!!!", MsgBoxStyle.Exclamation)
                'Exit Sub
            End If
            'biar bisa selling jika ada yg sudahkejual dibawah di remarks
            'If dsCek.Tables(0).Rows.Count > 0 Then
            '    For Each ro As DataRow In dsCek.Tables(0).Rows
            '        If ro("V_SELL").ToString <> "" And ro("V_FLAG").ToString = "" Then
            '            MsgBox("Voucher '" & DataGridView1.Item(1, s).Value & "' HAS BEEN SELLING!!!", MsgBoxStyle.Exclamation)
            '            Exit Sub
            '        End If
            '        If dsCek.Tables(0).Rows(0).Item("V_SELL").ToString <> "" And dsCek.Tables(0).Rows(0).Item("V_FLAG").ToString <> "" Then
            '            MsgBox("Voucher '" & DataGridView1.Item(1, s).Value & "' HAS BEEN SOLD!!!", MsgBoxStyle.Exclamation)
            '            Exit Sub
            '        End If
            '    Next
            'End If
            'Dim cc As String = dsCek.Tables(0).Rows(0).Item("V_SELL").ToString
            'cc = DataGridView1.Item(1, s).Value
            'cc = dsCek.Tables(0).Rows(0).Item("V_FLAG").ToString   
        Next

        dsPar = getSqldb("SELECT [par_valn] FROM [par_appl] WHERE [par_id]='008'")
        int_ref = CInt(dsPar.Tables(0).Rows(0).Item("par_valn").ToString.Trim) + 1
        Select Case int_ref.ToString.Length
            Case Is = 1
                v_ref = "000000" & int_ref
            Case Is = 2
                v_ref = "00000" & int_ref
            Case Is = 3
                v_ref = "0000" & int_ref
            Case Is = 4
                v_ref = "000" & int_ref
            Case Is = 5
                v_ref = "00" & int_ref
            Case Is = 6
                v_ref = "0" & int_ref
            Case Is = 7
                v_ref = int_ref
        End Select

        ''Server
        'For s As Integer = 0 To DataGridView1.Rows.Count - 1
        '    getSqldb("Update NewVoc Set  V_SELL = '" & DateTimePicker1.Value.Date & "',  V_KRE = '" & DateTimePicker1.Value.Date & "',  V_DESC = '" & txtDescription.Text & "', V_REF = '" & v_ref & "',V_TYPE = 'C' Where  V_Code = '" & V_Code & "' And  ( V_NO between '" & DataGridView1.Item(1, s).Value & "' And  '" & DataGridView1.Item(2, s).Value & "')  And V_AMT = '" & DataGridView1.Item(3, s).Value & "'")
        'Next
        getSqldb("UPDATE par_appl SET par_valn='" & v_ref & "' WHERE par_valn ='" & dsPar.Tables(0).Rows(0).Item("par_valn").ToString.Trim & "' AND par_id ='008'")
        getSqldb("Insert Into lain values ('" & V_Code & "','" & v_ref & "','" & DateTimePicker1.Value.Date & "','C','" & CDec(txtAmnt.Text) & "','" & CDec(txtDisc.Text) & "','" & cmbDesc.Text & "','" & DateTimePicker1.Value.Date & "','" & usrID & "')")
        ''History
        'For s As Integer = 0 To DataGridView1.Rows.Count - 1
        '    getSqldb2("Update NewVoc Set  V_SELL = '" & DateTimePicker1.Value.Date & "',  V_KRE = '" & DateTimePicker1.Value.Date & "',  V_DESC = '" & txtDescription.Text & "', V_REF = '" & v_ref & "',V_TYPE = 'C' Where  V_Code = '" & V_Code & "' And  ( V_NO between '" & DataGridView1.Item(1, s).Value & "' And  '" & DataGridView1.Item(2, s).Value & "')  And V_AMT = '" & DataGridView1.Item(3, s).Value & "'")
        'Next
        ''Me.Close()

        'menggunakan server Voucher
        For s As Integer = 0 To DataGridView1.Rows.Count - 1
            dsCek.Clear()
            dsCek = getSqldb("select * from NewVoc where V_NO between '" & DataGridView1.Item(1, s).Value & "' And  '" & DataGridView1.Item(2, s).Value & "'")
            If dsCek.Tables(0).Rows.Count > 0 Then
                For Each ro As DataRow In dsCek.Tables(0).Rows
                    'If IsDBNull(ro("V_FLAG")) Then
                    getSqldb3("Update NewVoc Set V_DEPO = '" & Format(DateTimePicker2.Value.Date, "yyMMdd") & "',V_SELL = '" & DateTimePicker1.Value.Date & "',  V_KRE = '" & DateTimePicker1.Value.Date & "',  V_DESC = '" & cmbDesc.Text & "', V_REF = '" & v_ref & "',V_TYPE = 'C' Where  V_Code = '" & V_Code & "' And   V_NO = '" & ro("V_NO") & "' And V_AMT = '" & CDec(DataGridView1.Item(3, s).Value) & "'")
                    getSqldb3("Update NewVocDtl Set V_DESC_DTL = '" & txtDescDtl.Text & "' Where  V_NO = '" & ro("V_NO") & "'")
                    getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Selling Voucher','" & DataGridView1.Item(1, s).Value & "','Success','" & DataGridView1.Item(2, s).Value & "','" & Now & "')")
                    'End If
                Next
            End If


        Next

        GroupBox3.Enabled = False
        For a = 1 To DataGridView1.Rows.Count
            DataGridView1.Rows.RemoveAt(0)
        Next
        cmbDesc.SelectedValue = ""
        txtDics2.Clear()
        txtDisc.Clear()
        txtAmnt.Clear()
        txtDescDtl.Clear()
        cmbDesc.Focus()
        MsgBox("Has Been Saved !!!", MsgBoxStyle.Information, "Information")
        cmbDesc.Text = ""
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If MsgBox("Delete Data ??", MsgBoxStyle.YesNo, "Attention") = MsgBoxResult.Yes Then
            DataGridView1.Rows.RemoveAt(DataGridView1.CurrentRow.Index)
            'getSqldb("DELETE [vc_pay]  WHERE [trans_no]='" & TextBox1.Text & "' And v_no = '" & DataGridView1.Item(1, DataGridView1.CurrentRow.Index).Value & "'")
            'setDG()
            ForTotDg()
        End If
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        GroupBox3.Enabled = False
        For a = 1 To DataGridView1.Rows.Count
            DataGridView1.Rows.RemoveAt(0)
        Next
        cmbDesc.SelectedValue = ""
        cmbDesc.Text = ""
        txtDics2.Clear()
        txtDisc.Clear()
        txtAmnt.Clear()
        cmbDesc.Focus()
        txtDescDtl.Clear()
    End Sub

    Private Sub txtDescription_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)
        If e.KeyCode = Keys.Enter Then
            If cmbDesc.SelectedValue <> "" Then
                txtAmnt.Focus()
            End If
        End If
    End Sub

    Private Sub cmbDesc_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbDesc.SelectedValueChanged
        txtDescDtl.Focus()
    End Sub

    Private Sub txtDescDtl_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDescDtl.KeyDown
        If e.KeyCode = Keys.Enter Then
            If txtDescDtl.Text <> "" Then
                txtAmnt.Focus()
            End If
        End If
    End Sub

End Class