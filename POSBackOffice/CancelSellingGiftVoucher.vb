Public Class CancelSellingGiftVoucher
    Dim dslv1, dsCek As New DataSet
    Dim CekAll As Boolean
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        SetLv1()
        Dim I As Integer
        For I = 0 To ListView1.Items.Count - 1
            If ListView1.Items(I).SubItems(0).Text <> "" Then
                ListView1.Items(I).Selected = True
                SetLv2(ListView1.Items(I).SubItems(0).Text, ListView1.Items(I).SubItems(1).Text)
                Exit For
            End If
        Next
    End Sub

    Sub SetLv1()
        ListView1.Items.Clear()
        'dslv1 = getSqldb("select lokasi,l_ref,l_amt,l_ket from lain where day(l_tgl) = '" & DateTimePicker1.Value.Day & "' and month(l_tgl) = '" & DateTimePicker1.Value.Month & "' and year(l_tgl) = '" & DateTimePicker1.Value.Year & "' order by l_ref,l_lupdate")
        dslv1 = getSqldb("select a.lokasi,a.l_ref,a.l_amt,MAX(a.l_ket + ' ' + c.V_DESC_DTL) As l_ket,CONVERT(DateTime,'20'+LEFT(V_DEPO,2)+'-'+SUBSTRING(V_DEPO,3,2)+'-'+RIGHT(V_DEPO,2)+ ' 00:00:00') AS Expired from lain a inner join newvoc b on a.l_ref = b.v_ref  " & _
                         "inner join NewVocDtl c on b.V_NO = c.V_NO where day(l_tgl) = '" & DateTimePicker1.Value.Day & "' and month(l_tgl) = '" & DateTimePicker1.Value.Month & "' and year(l_tgl) = '" & DateTimePicker1.Value.Year & "' and v_depo is NOT NULL " & _
                         "group by a.lokasi,a.l_ref,a.l_amt,a.l_ket ,CONVERT(DateTime,'20'+LEFT(V_DEPO,2)+'-'+SUBSTRING(V_DEPO,3,2)+'-'+RIGHT(V_DEPO,2)+ ' 00:00:00') ")
        If dslv1.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In dslv1.Tables(0).Rows
                Dim str(4) As String
                Dim itm As ListViewItem
                str(0) = ro(0)
                str(1) = ro(1)
                str(2) = CDec(ro(2)).ToString("N0")
                str(3) = ro(3)
                str(4) = Format(CDate(ro(4)), "dd MMMM yyyy")
                itm = New ListViewItem(str)
                ListView1.Items.Add(itm)
            Next
        End If
    End Sub

    Sub SetLv2(ByVal Lo As String, ByVal ref As String)
        ListView2.Items.Clear()
        dslv1 = getSqldb("select a.V_NO,V_AMT,V_DESC + ' ' + b.V_DESC_DTL As V_DESC,V_FLAG from NewVoc a inner join NewVocDtl b on a.V_NO = b.V_NO where a.V_CODE = '" & Lo & "' and a.V_REF = '" & ref & "'")
        If dslv1.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In dslv1.Tables(0).Rows
                Dim str(4) As String
                Dim itm As ListViewItem
                str(0) = ""
                str(1) = ro(0)
                str(2) = CDec(ro(1)).ToString("N0")
                str(3) = ro(2)
                If IsDBNull(ro(3)) Then
                    str(4) = ".."
                Else
                    str(4) = ro(3).ToString()
                End If
                itm = New ListViewItem(str)
                ListView2.Items.Add(itm)
            Next
        End If
    End Sub

    Private Sub CancelSellingGiftVoucher_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        CekAll = False
        lv()
        lv2()
    End Sub

    Sub lv()
        ListView1.Columns.Add("Lo", 30, HorizontalAlignment.Left)
        ListView1.Columns.Add("v Ref", 110, HorizontalAlignment.Left)
        ListView1.Columns.Add("Amount", 90, HorizontalAlignment.Left)
        ListView1.Columns.Add("Desc", 180, HorizontalAlignment.Left)
        ListView1.Columns.Add("Expired", 100, HorizontalAlignment.Left)
    End Sub

    Sub lv2()
        ListView2.Columns.Add("", 20, HorizontalAlignment.Left)
        ListView2.Columns.Add("V_No", 90, HorizontalAlignment.Left)
        ListView2.Columns.Add("V_Amt", 90, HorizontalAlignment.Left)
        ListView2.Columns.Add("V_Desc", 180, HorizontalAlignment.Left)
        ListView2.Columns.Add("", 40, HorizontalAlignment.Left)
    End Sub

    Private Sub ListView1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.Click
        Dim I As Integer
        CheckBox1.Checked = False
        For I = 0 To ListView1.SelectedItems.Count - 1
            SetLv2(ListView1.SelectedItems(I).SubItems(0).Text, ListView1.SelectedItems(I).SubItems(1).Text)
            Exit For
        Next
    End Sub

    Private Sub ListView2_ItemChecked(ByVal sender As Object, ByVal e As System.Windows.Forms.ItemCheckedEventArgs) Handles ListView2.ItemChecked
        If CekAll = True Then
            Exit Sub
        End If
        Dim nom As Decimal = 0
        For aa As Integer = 0 To ListView2.CheckedItems.Count - 1
            nom += ListView2.CheckedItems(aa).SubItems(2).Text
        Next
        Label1.Text = "Selected Amount : " & nom.ToString("N0")
    End Sub

    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Dim I As Integer
        Dim dsCek As New DataSet
        Dim DelAll As Boolean = True
        For I = 0 To ListView1.SelectedItems.Count - 1
            If MsgBox("Cancel This Voucher ??", MsgBoxStyle.YesNo, "Information") = MsgBoxResult.No Then
                Exit Sub
            End If
            'dsCek = getSqldb("select * from NewVoc where V_CODE = '" & ListView1.SelectedItems(I).SubItems(0).Text & "' and V_REF = '" & ListView1.SelectedItems(I).SubItems(1).Text & "' And V_KUNCI IS NOT NULL")
            'If dsCek.Tables(0).Rows.Count > 0 Then
            '    MsgBox("Voucher " & V_Code & " -" & dsCek.Tables(0).Rows(0).Item("V_REF") & " Already Received," & vbNewLine & " Reference Number : " & dsCek.Tables(0).Rows(0).Item("V_KUNCI"))
            '    Exit Sub
            'End If
            
            If ListView2.CheckedItems.Count = 0 Then
                MsgBox("Pleace Checked Voucher List !!!!", MsgBoxStyle.Exclamation, "Information")
                Exit Sub
            End If
            For D = 0 To ListView2.Items.Count - 1
                If ListView2.Items(D).Checked = False Then
                    DelAll = False
                    Exit For
                End If
            Next
            'dsCek = getSqldb("select * from NewVoc where V_CODE = '" & ListView1.SelectedItems(I).SubItems(0).Text & "' and V_REF = '" & ListView1.SelectedItems(I).SubItems(1).Text & "'")
            'If dsCek.Tables(0).Rows.Count > 0 Then
            'MsgBox("Voucher " & V_Code & " -" & dsCek.Tables(0).Rows(0).Item("V_REF") & " Already Received," & vbNewLine & " Reference Number : " & dsCek.Tables(0).Rows(0).Item("V_KUNCI"))
            'Exit Sub
            'DelAll = False
            'End If
            Try

                For I2 = 0 To ListView2.CheckedItems.Count - 1
                    If DelAll = False Then
                        Try
                            dsCek.Clear()
                            dsCek = getSqldb("select * from NewVoc where V_NO = '" & ListView2.CheckedItems(I2).SubItems(1).Text & "'")
                            If dsCek.Tables(0).Rows.Count > 0 Then
                                getSqldb("Update lain set L_AMT = L_AMT - " & CDec(ListView2.CheckedItems(I2).SubItems(2).Text) & " where day(l_tgl) = '" & DateTimePicker1.Value.Day & "' and month(l_tgl) = '" & DateTimePicker1.Value.Month & "' and year(l_tgl) = '" & DateTimePicker1.Value.Year & "' and " & _
                                                                                 "lokasi = '" & ListView1.SelectedItems(I).SubItems(0).Text & "' and l_ref = '" & ListView1.SelectedItems(I).SubItems(1).Text & "'  and l_amt = " & _
                                                                                 "'" & CDec(ListView1.SelectedItems(I).SubItems(2).Text) & "'")
                                getSqldb3("Update NewVoc Set V_DEPO = NULL, V_SELL = NULL,  V_KRE = NULL,  V_DESC = NULL, V_REF = NULL,V_TYPE = NULL Where  V_Code = '" & V_Code & "' And   V_NO = '" & ListView2.CheckedItems(I2).SubItems(1).Text & "' And V_AMT = '" & CDec(ListView2.CheckedItems(I2).SubItems(2).Text) & "'")
                                getSqldb3("Update NewVocDtl Set V_DESC_DTL = '' Where   V_NO = '" & ListView2.CheckedItems(I2).SubItems(1).Text & "' ")
                                getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Cancel Voucher','" & ListView2.CheckedItems(I2).SubItems(1).Text & "','Success','" & ListView1.SelectedItems(I).SubItems(1).Text & "','" & Now & "')")
                            End If

                        Catch ex As Exception
                            MsgBox("Pengurangan Total Cancel Selling Tidak Berhasil !!")
                        End Try

                    End If
                    'getSqldb("Update NewVoc Set  V_SELL = NULL,  V_KRE = NULL,  V_DESC = NULL, V_REF = NULL,V_TYPE = NULL Where  V_Code = '" & V_Code & "' And   V_NO = '" & ListView2.CheckedItems(I2).SubItems(1).Text & "' And V_AMT = '" & ListView2.CheckedItems(I2).SubItems(2).Text & "'")
                    'getSqldb2("Update NewVoc Set  V_SELL = NULL,  V_KRE = NULL,  V_DESC = NULL, V_REF = NULL,V_TYPE = NULL Where  V_Code = '" & V_Code & "' And   V_NO = '" & ListView2.CheckedItems(I2).SubItems(1).Text & "' And V_AMT = '" & ListView2.CheckedItems(I2).SubItems(2).Text & "'")
                    'menggunakan server voucher
                    'dsCek = getSqldb3("select * from NewVoc where V_FLAG IS NOT NULL and V_Code = '" & V_Code & "' And   V_NO = '" & ListView2.CheckedItems(I2).SubItems(1).Text & "' And V_AMT = '" & CDec(ListView2.CheckedItems(I2).SubItems(2).Text) & "' ")
                    'If dsCek.Tables(0).Rows.Count > 0 Then
                    '    DelAll = False
                    'End If

                Next
                If DelAll = True Then
                    For I2 = 0 To ListView2.CheckedItems.Count - 1
                        CheckBox1.Checked = True
                        getSqldb3("Update NewVoc Set V_DEPO = NULL, V_SELL = NULL,  V_KRE = NULL,  V_DESC = NULL, V_REF = NULL,V_TYPE = NULL Where  V_Code = '" & V_Code & "' And   V_NO = '" & ListView2.CheckedItems(I2).SubItems(1).Text & "' And V_AMT = '" & CDec(ListView2.CheckedItems(I2).SubItems(2).Text) & "'")
                        getSqldb3("Update NewVocDtl Set V_DESC_DTL = '' Where   V_NO = '" & ListView2.CheckedItems(I2).SubItems(1).Text & "' ")
                        getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Cancel Voucher','" & ListView2.CheckedItems(I2).SubItems(1).Text & "','Success','" & ListView1.SelectedItems(I).SubItems(1).Text & "','" & Now & "')")
                        getSqldb("delete from lain where day(l_tgl) = '" & DateTimePicker1.Value.Day & "' and month(l_tgl) = '" & DateTimePicker1.Value.Month & "' and year(l_tgl) = '" & DateTimePicker1.Value.Year & "' and " & _
                                                             "lokasi = '" & ListView1.SelectedItems(I).SubItems(0).Text & "' and l_ref = '" & ListView1.SelectedItems(I).SubItems(1).Text & "'  and l_amt = " & _
                                                             "'" & CDec(ListView1.SelectedItems(I).SubItems(2).Text) & "' and l_ket = '" & ListView1.SelectedItems(I).SubItems(3).Text & "'")
                    Next
                End If

            Catch ex As Exception
                MsgBox("Cancel Voucher Failed !!", MsgBoxStyle.Exclamation)
                Exit Sub
            End Try
            MsgBox("Has Been Canceled !!!", MsgBoxStyle.Information, "Information")
            ListView1.Items.Clear()
            ListView2.Items.Clear()
            CheckBox1.Checked = False
            Button1_Click(sender, e)
            Exit Sub
        Next
        MsgBox("Click List Voucher First  !!!", MsgBoxStyle.Exclamation)
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        CekAll = True
        If CheckBox1.Checked = True Then
            For I = 0 To ListView2.Items.Count - 1
                ListView2.Items(I).Checked = True
            Next
        Else
            For I = 0 To ListView2.Items.Count - 1
                ListView2.Items(I).Checked = False
            Next
        End If
        CekAll = False
        Dim nom As Decimal = 0
        For aa As Integer = 0 To ListView2.CheckedItems.Count - 1
            nom += ListView2.CheckedItems(aa).SubItems(2).Text
        Next
        Label1.Text = "Selected Amount : " & nom.ToString("N0")
    End Sub

    Private Sub ListView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Dim I As Integer
        CheckBox1.Checked = False
        For I = 0 To ListView1.SelectedItems.Count - 1
            SetLv2(ListView1.SelectedItems(I).SubItems(0).Text, ListView1.SelectedItems(I).SubItems(1).Text)
            Exit For
        Next
    End Sub
End Class