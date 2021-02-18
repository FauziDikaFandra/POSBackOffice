Public Class Pemeliharaan_Voucher
    Dim dslv1 As New DataSet

    Private Sub Pemeliharaan_Voucher_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If DSBranch.Tables(0).Rows.Count > 0 Then
            TextBox1.Text = DSBranch.Tables(0).Rows(0).Item("Branch_ID") & " - " & DSBranch.Tables(0).Rows(0).Item("Branch_Name")
        End If
        lv()
    End Sub

    Sub lv()
        ListView1.Columns.Add("Trans No", 155, HorizontalAlignment.Left)
        ListView1.Columns.Add("Desc ", 130, HorizontalAlignment.Left)
        ListView1.Columns.Add("Amount", 90, HorizontalAlignment.Left)
        ListView1.Columns.Add("", 30, HorizontalAlignment.Left)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        SetLv1()
    End Sub

    Sub SetLv1()
        Dim dscek As New DataSet
        ListView1.Items.Clear()
        dslv1 = getSqldb("select * from slip_pay where left(trans_no,4) = '" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' and substring(trans_no,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' and types = 'SV' and paid_amount > 0 order by trans_no")
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
                ListView1.Items.Add(itm)
            Next
        Else
            MsgBox("Data Not Found !!", MsgBoxStyle.Exclamation)
        End If
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Dim I As Integer
        For I = 0 To ListView1.SelectedItems.Count - 1
            If ListView1.Items(I).SubItems(0).Text <> "" Then
                Trans_No_v = ListView1.SelectedItems(I).SubItems(0).Text
                Tot_v = ListView1.SelectedItems(I).SubItems(2).Text
                Exit For
            End If
            'MsgBox(ListView1.SelectedItems(I).SubItems(2).Text)
        Next
        VoucherReceiveDtl.MdiParent = MainForm
        VoucherReceiveDtl.Show()
        VoucherReceiveDtl.TopMost = True
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        Dim I As Integer
        For I = 0 To ListView1.SelectedItems.Count - 1
            If ListView1.Items(I).SubItems(0).Text <> "" Then
                Trans_No_v = ListView1.SelectedItems(I).SubItems(0).Text
                Tot_v = ListView1.SelectedItems(I).SubItems(2).Text
                Exit For
            End If
            'MsgBox(ListView1.SelectedItems(I).SubItems(2).Text)
        Next
        VoucherReceiveDtl.Show()
    End Sub


End Class