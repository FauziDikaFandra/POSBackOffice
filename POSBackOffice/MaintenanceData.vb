Public Class MaintenanceData
    Dim DsLv1, DsLv1x, DsLv2 As DataSet
    Dim xread As Decimal = 0
    Dim trans_no As String
    Dim LvRow As Integer = 0
    Private Sub MaintenanceData_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If DSBranch.Tables(0).Rows.Count > 0 Then
            TextBox1.Text = DSBranch.Tables(0).Rows(0).Item("Branch_ID")
        End If
        lv()
        lv2()
    End Sub

    Sub lv()
        ListView1.Columns.Add("Trans No", 155, HorizontalAlignment.Left)
        ListView1.Columns.Add("Cashier ", 70, HorizontalAlignment.Left)
        ListView1.Columns.Add("Amount", 110, HorizontalAlignment.Left)
    End Sub

    Sub lv2()
        ListView2.Columns.Add("No", 30, HorizontalAlignment.Left)
        ListView2.Columns.Add("Pay Desc ", 220, HorizontalAlignment.Left)
        ListView2.Columns.Add("Amount", 160, HorizontalAlignment.Left)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        SetLv1()
        ListView2.Items.Clear()
        Dim I As Integer
        For I = 0 To ListView1.Items.Count - 1
            If ListView1.Items(I).SubItems(0).Text <> "" Then
                SetLv2(ListView1.Items(I).SubItems(0).Text)
                Exit For
            End If
            'MsgBox(ListView1.SelectedItems(I).SubItems(2).Text)
        Next

    End Sub

    Sub SetLv1()
        ListView1.Items.Clear()
        DsLv1 = getSqldb("SELECT trans_no,cashier_id,xread, realcash, r_short, r_over,  cashier_pay, cashier_bal, r_depo, r_cs FROM slip WHERE  substring(trans_no,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' Order By trans_no")
        If DsLv1.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In DsLv1.Tables(0).Rows
                Dim str(2) As String
                Dim itm As ListViewItem
                str(0) = ro(0)
                str(1) = ro(1)
                str(2) = CDec(ro(2)).ToString("N0")
                itm = New ListViewItem(str)
                ListView1.Items.Add(itm)
            Next
            txtFisik.Text = CDec(DsLv1.Tables(0).Rows(0).Item("realcash")).ToString("N0")
            txtShort.Text = DsLv1.Tables(0).Rows(0).Item("r_short")
            txtOver.Text = DsLv1.Tables(0).Rows(0).Item("r_over")

            txtDepo.Text = DsLv1.Tables(0).Rows(0).Item("r_depo")
            txtCC.Text = DsLv1.Tables(0).Rows(0).Item("r_cs")
            xread = DsLv1.Tables(0).Rows(0).Item("xread")
            trans_no = DsLv1.Tables(0).Rows(0).Item("trans_no")
        Else
            MsgBox("Data Not Found !!", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Sub SetLv2(ByVal transno As String)
        ListView2.Items.Clear()
        DsLv2 = getSqldb("SELECT payment_types, description, paid_amount FROM slip_pay WHERE trans_no = '" & transno & "' Order By payment_types")
        If DsLv2.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In DsLv2.Tables(0).Rows
                Dim str(2) As String
                Dim itm As ListViewItem
                str(0) = ro(0)
                str(1) = ro(1)
                str(2) = CDec(ro(2)).ToString("N0")
                itm = New ListViewItem(str)
                ListView2.Items.Add(itm)
            Next
        End If
    End Sub

    Private Sub ListView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.Click
        Dim I As Integer
        For I = 0 To ListView1.SelectedItems.Count - 1
            If ListView1.SelectedItems(I).SubItems(2).Text <> "" Then
                SetLv2(ListView1.SelectedItems(I).SubItems(0).Text)
                DsLv1x = getSqldb("SELECT trans_no,cashier_id,xread, realcash, r_short, r_over,  cashier_pay, cashier_bal, r_depo, r_cs FROM slip WHERE  trans_no = '" & ListView1.SelectedItems(I).SubItems(0).Text & "' Order By trans_no")
                If DsLv1x.Tables(0).Rows.Count > 0 Then
                    txtFisik.Text = CDec(DsLv1x.Tables(0).Rows(0).Item("realcash")).ToString("N0")
                    txtShort.Text = DsLv1x.Tables(0).Rows(0).Item("r_short")
                    txtOver.Text = DsLv1x.Tables(0).Rows(0).Item("r_over")

                    txtDepo.Text = DsLv1x.Tables(0).Rows(0).Item("r_depo")
                    txtCC.Text = DsLv1x.Tables(0).Rows(0).Item("r_cs")
                    xread = DsLv1x.Tables(0).Rows(0).Item("xread")
                    trans_no = DsLv1x.Tables(0).Rows(0).Item("trans_no")
                End If
                LvRow = I
            End If
            'MsgBox(ListView1.SelectedItems(I).SubItems(2).Text)
        Next
    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        If txtFisik.Text <> "" Then
            txtFisik.Enabled = True
            ListView1.Enabled = False
            ListView2.Enabled = False
            txtFisik.Focus()
            txtFisik.Text = Replace(txtFisik.Text, ",", "")
            txtFisik.SelectAll()
            'txtFisik.SelectionStart = txtFisik.TextLength
        End If

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        getSqldb("Update Slip set realcash = '" & CDec(txtFisik.Text) & "',r_short = '" & CDec(txtShort.Text) & "',r_over = '" & CDec(txtOver.Text) & "' Where trans_no = '" & trans_no & "'")
        txtFisik.Enabled = False
        ListView1.Enabled = True
        ListView2.Enabled = True
        'Button1_Click(sender, e)
        ListView1.Focus()
        If ListView1.Items.Count >= LvRow + 2 Then
            ListView1.Items(LvRow + 1).Focused = True
            ListView1.Items(LvRow + 1).Selected = True
        Else
            ListView1.Items(LvRow).Selected = True
        End If

    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        txtFisik.Enabled = False
        ListView1.Enabled = True
        ListView2.Enabled = True
        txtFisik.Text = DsLv1.Tables(0).Rows(0).Item("realcash")
        txtShort.Text = DsLv1.Tables(0).Rows(0).Item("r_short")
        txtOver.Text = DsLv1.Tables(0).Rows(0).Item("r_over")

        txtDepo.Text = DsLv1.Tables(0).Rows(0).Item("r_depo")
        txtCC.Text = DsLv1.Tables(0).Rows(0).Item("r_cs")
        xread = DsLv1.Tables(0).Rows(0).Item("xread")
        trans_no = DsLv1.Tables(0).Rows(0).Item("trans_no")
    End Sub

    Private Sub txtDepo_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDepo.TextChanged
        If txtDepo.Text.Length > 3 Then
            txtDepo.Text = CDec(txtDepo.Text).ToString("N0")
            txtDepo.SelectionStart = txtDepo.TextLength
        End If
    End Sub

    Private Sub txtFisik_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtFisik.KeyUp
        If e.KeyCode = Keys.Enter Then
            Button3_Click(sender, e)
            'If xread - txtFisik.Text < 0 Then
            '    txtOver.Text = (xread - txtFisik.Text) * -1
            '    txtShort.Text = 0
            'Else
            '    txtShort.Text = (xread - txtFisik.Text)
            '    txtOver.Text = 0
            'End If
            'txtFisik.Text = CDec(txtFisik.Text).ToString("N0")
            'txtFisik.SelectionStart = txtFisik.TextLength
        End If

    End Sub

    Private Sub txtFisik_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtFisik.TextChanged
        'If txtFisik.Text.Length > 3 Then
        '    txtFisik.Text = CDec(txtFisik.Text).ToString("N0")
        '    txtFisik.SelectionStart = txtFisik.TextLength
        'End If
        Try
            If txtFisik.Text <> "" Then
                If xread - txtFisik.Text < 0 Then
                    txtOver.Text = (xread - txtFisik.Text) * -1
                    txtShort.Text = 0
                Else
                    txtShort.Text = (xread - txtFisik.Text)
                    txtOver.Text = 0
                End If
                txtFisik.Text = CDec(txtFisik.Text).ToString("N0")
                txtFisik.SelectionStart = txtFisik.TextLength
            End If
        Catch ex As Exception
            txtFisik.Text = ""
        End Try


    End Sub

    Private Sub txtOver_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtOver.TextChanged
        If txtOver.Text.Length > 3 Then
            txtOver.Text = CDec(txtOver.Text).ToString("N0")
            txtOver.SelectionStart = txtOver.TextLength
        End If
    End Sub

    Private Sub txtShort_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtShort.TextChanged
        If txtShort.Text.Length > 3 Then
            txtShort.Text = CDec(txtShort.Text).ToString("N0")
            txtShort.SelectionStart = txtShort.TextLength
        End If
    End Sub

    Private Sub ListView1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ListView1.KeyUp
        If e.KeyCode = Keys.Enter Then
            If txtFisik.Text <> "" Then
                txtFisik.Enabled = True
                ListView1.Enabled = False
                ListView2.Enabled = False
                txtFisik.Focus()
                txtFisik.Text = Replace(txtFisik.Text, ",", "")
                txtFisik.SelectAll()
                'txtFisik.SelectionStart = txtFisik.TextLength
                LvRow = ListView1.FocusedItem.Index
            End If
            
        End If
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Dim I As Integer
        For I = 0 To ListView1.SelectedItems.Count - 1
            If ListView1.SelectedItems(I).SubItems(2).Text <> "" Then
                SetLv2(ListView1.SelectedItems(I).SubItems(0).Text)
                DsLv1x = getSqldb("SELECT trans_no,cashier_id,xread, realcash, r_short, r_over,  cashier_pay, cashier_bal, r_depo, r_cs FROM slip WHERE  trans_no = '" & ListView1.SelectedItems(I).SubItems(0).Text & "' Order By trans_no")
                If DsLv1x.Tables(0).Rows.Count > 0 Then
                    
                    txtFisik.Text = CDec(DsLv1x.Tables(0).Rows(0).Item("realcash")).ToString("N0")
                    txtShort.Text = DsLv1x.Tables(0).Rows(0).Item("r_short")
                    txtOver.Text = DsLv1x.Tables(0).Rows(0).Item("r_over")
                    txtDepo.Text = DsLv1x.Tables(0).Rows(0).Item("r_depo")
                    txtCC.Text = DsLv1x.Tables(0).Rows(0).Item("r_cs")
                    xread = DsLv1x.Tables(0).Rows(0).Item("xread")
                    trans_no = DsLv1x.Tables(0).Rows(0).Item("trans_no")
                End If
                LvRow = I
            End If
            'MsgBox(ListView1.SelectedItems(I).SubItems(2).Text)
        Next
    End Sub
End Class