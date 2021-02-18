Public Class VoucherReceiveDtl
    Dim dslv1, dscek As New DataSet
    Dim TotDg As Decimal
    Private Sub VoucherReceive_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'lv()
        TextBox1.Text = Trans_No_v
        TextBox2.Text = CDec(Tot_v).ToString("N0")
        'SetLv1()
        setDG()
        ForTotDg()
        Label5.Text = CDec(Tot_v).ToString("N0")
    End Sub

    Sub setDG()
        Dim rw As Integer = -1
        DataGridView1.DataSource = Nothing
        dscek = getSqldb("Select V_code,v_no as Voucher#,v_amt as Amount  from vc_pay where trans_no = '" & Trans_No_v & "'")
        If dscek.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In dscek.Tables(0).Rows
                rw += 1
                DataGridView1.Rows.Add(1)
                DataGridView1.Item(0, rw).Value = ro("V_code")
                DataGridView1.Item(1, rw).Value = ro("Voucher#").ToString.Trim
                DataGridView1.Item(2, rw).Value = ro("Amount")
            Next
            DataGridView1.Refresh()
        Else
            dslv1 = getSqldb("select V_code,Credit_Card_No as Voucher#,Paid_Amount as Amount from paid a inner join payment_types b on a.payment_types=b.payment_types inner join NewVoc c on Credit_Card_No=V_NO and Paid_Amount=V_AMT where LEFT(transaction_number,16)= '" & Microsoft.VisualBasic.Mid(Trans_No_v, 1, 16) & "' and types='SV' and Shift='" & Microsoft.VisualBasic.Right(Trans_No_v, 1) & "'")
            If dslv1.Tables(0).Rows.Count > 0 Then
                For Each ro As DataRow In dslv1.Tables(0).Rows
                    rw += 1
                    DataGridView1.Rows.Add(1)
                    DataGridView1.Item(0, rw).Value = ro("V_code")
                    DataGridView1.Item(1, rw).Value = ro("Voucher#").ToString.Trim
                    DataGridView1.Item(2, rw).Value = ro("Amount")
                Next
                DataGridView1.Refresh()
            End If
        End If

    End Sub

    Sub ForTotDg()
        Dim I As Integer
        TotDg = 0
        For I = 0 To DataGridView1.Rows.Count - 1
            TotDg += CDec(DataGridView1.Item(2, I).Value)
            'MsgBox(ListView1.SelectedItems(I).SubItems(2).Text)
        Next
        Label6.Text = CDec(TextBox2.Text - TotDg).ToString("N0")
        If CDec(TextBox2.Text - TotDg) = 0 Then
            Label6.ForeColor = Color.Black
        ElseIf CDec(TextBox2.Text - TotDg) > 0 Then
            Label6.ForeColor = Color.Green
        Else
            Label6.ForeColor = Color.Red
        End If
    End Sub

    'Sub lv()
    '    ListView1.Columns.Add("V Code", 90, HorizontalAlignment.Left)
    '    ListView1.Columns.Add("Voucher # ", 120, HorizontalAlignment.Left)
    '    ListView1.Columns.Add("Amount", 120, HorizontalAlignment.Left)
    'End Sub

    'Sub SetLv1()
    '    ListView1.Items.Clear()
    '    dslv1 = getSqldb("	select a.V_CODE as V_Code,a.V_NO as Voucher#,b.Paid_Amount as Amount  from NewVoc a inner join paid b " & _
    '                     "on a.V_NO = b.Credit_Card_No where SUBSTRING(b.Transaction_Number,1,16) = '" & Microsoft.VisualBasic.Mid(Trans_No_v, 1, 16) & "' and b.Shift = '" & Microsoft.VisualBasic.Right(Trans_No_v, 1) & "' order by a.V_NO")
    '    If DsLv1.Tables(0).Rows.Count > 0 Then
    '        For Each ro As DataRow In dslv1.Tables(0).Rows
    '            Dim str(2) As String
    '            Dim itm As ListViewItem
    '            str(0) = ro(0)
    '            str(1) = ro(1)
    '            str(2) = CDec(ro(2)).ToString("N0")
    '            itm = New ListViewItem(str)
    '            ListView1.Items.Add(itm)
    '        Next
    '    End If
    'End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If MsgBox("Cancel This Voucher Payment ??", MsgBoxStyle.YesNo, "Attention") = MsgBoxResult.Yes Then
            getSqldb("DELETE [vc_pay]  WHERE [trans_no]='" & TextBox1.Text & "'")
            SetLv1()
            Me.Close()
        End If

    End Sub

    Sub SetLv1()

        Pemeliharaan_Voucher.ListView1.Items.Clear()
        dslv1 = getSqldb("select * from slip_pay where left(trans_no,4) = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and substring(trans_no,9,8) = '" & Format(Pemeliharaan_Voucher.DateTimePicker1.Value, "ddMMyyyy") & "' and types = 'SV' and paid_amount > 0 order by trans_no")
        If dslv1.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In dslv1.Tables(0).Rows
                Dim str(3) As String
                Dim itm As ListViewItem
                str(0) = ro(0)
                str(1) = ro(2)
                str(2) = CDec(ro(4)).ToString("N0")
                dscek = getSqldb("SELECT [trans_no] FROM [vc_pay] WHERE [trans_no]='" & ro("trans_no") & "'")
                If dscek.Tables(0).Rows.Count > 0 Then
                    str(3) = "X"
                Else
                    str(3) = ""
                End If
                itm = New ListViewItem(str)
                Pemeliharaan_Voucher.ListView1.Items.Add(itm)
            Next
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        'diremarks karena pengecekan yang direceived ga usah
        'For s As Integer = 0 To DataGridView1.Rows.Count - 1
        '    dscek = getSqldb("SELECT [v_rec],[v_kunci],[v_flag],[v_type],[v_sell] FROM [newvoc] a inner join vc_pay b on a.[v_no] = b.[v_no] WHERE a.[v_code]='" & V_Code & "' AND a.[v_no]='" & DataGridView1.Item(1, s).Value & "' AND a.[v_amt]='" & CDec(DataGridView1.Item(2, s).Value) & "' And v_Flag = 'R'")
        '    If dscek.Tables(0).Rows.Count > 0 Then
        '        MsgBox("Received [" & V_Code & "-" & DataGridView1.Item(1, s).Value.ToString & "] " & vbNewLine & "Date : " & Format(dscek.Tables(0).Rows(0).Item("v_rec"), "dd/MM/yyyy") & vbNewLine & "Ref : " & dscek.Tables(0).Rows(0).Item("v_kunci"), MsgBoxStyle.Critical)
        '        Exit Sub
        '    End If
        'Next

        getSqldb("DELETE [vc_pay]  WHERE [trans_no]='" & Trans_No_v & "'")
        For s As Integer = 0 To DataGridView1.Rows.Count - 1
            'penambahan kasus double voucher
            getSqldb3("update newvoc set v_flag = 'R' where v_no = '" & DataGridView1.Item(1, s).Value & "' ")
            getSqldb("Insert Into vc_pay Values ('" & Trans_No_v & "','" & DataGridView1.Item(0, s).Value & "','" & DataGridView1.Item(1, s).Value & "','" & CDec(DataGridView1.Item(2, s).Value) & "')")
            getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Receipt','" & DataGridView1.Item(1, s).Value & "','Success','','" & Now & "')")
        Next
        'getSqldb("Insert Into vc_pay select '" & Trans_No_v & "',V_code,Substring(Credit_Card_No,1,11) as Voucher#,Paid_Amount as Amount from paid a inner join payment_types b on a.payment_types=b.payment_types inner join NewVoc c on Credit_Card_No=V_NO and Paid_Amount=V_AMT where LEFT(transaction_number,16)= '" & Microsoft.VisualBasic.Mid(Trans_No_v, 1, 16) & "' and types='SV' and Shift='" & Microsoft.VisualBasic.Right(Trans_No_v, 1) & "'")
        SetLv1()
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        AddData.Text = "Add Data"
        AddData.Label4.Text = V_Code
        AddData.Label5.Text = DataGridView1.CurrentRow.Index
        AddData.TextBox1.Enabled = True
        AddData.TextBox2.Enabled = True
        AddData.TextBox1.Focus()
        AddData.Show()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If DataGridView1.RowCount > 0 Then
            AddData.Text = "Edit Data"
            AddData.TextBox1.Enabled = False
            AddData.TextBox2.Enabled = False
            AddData.Label4.Text = V_Code
            AddData.Label5.Text = DataGridView1.CurrentRow.Index
            AddData.TextBox1.Text = DataGridView1.Item(1, DataGridView1.CurrentRow.Index).Value
            AddData.TextBox2.Text = DataGridView1.Item(1, DataGridView1.CurrentRow.Index).Value
            AddData.TextBox3.Text = CDec(DataGridView1.Item(2, DataGridView1.CurrentRow.Index).Value).ToString("N0")
            AddData.TextBox3.Focus()
            AddData.Show()
        End If

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If MsgBox("Delete Data ??", MsgBoxStyle.YesNo, "Attention") = MsgBoxResult.Yes Then
            DataGridView1.Rows.RemoveAt(DataGridView1.CurrentRow.Index)
            'getSqldb("DELETE [vc_pay]  WHERE [trans_no]='" & TextBox1.Text & "' And v_no = '" & DataGridView1.Item(1, DataGridView1.CurrentRow.Index).Value & "'")
            'setDG()
            ForTotDg()
        End If
    End Sub
End Class