Imports System.IO
Public Class SalesMaintenance
    Dim dsH, dsD, dsP As New DataSet
    Dim H_NetAmount As Decimal = 0
    Dim seq As Integer = 0
    Dim com As Boolean = False
    Dim RowDg1, RowDg2 As Integer
    Dim TransNo, TransStr, DetailStr, PaidStr, OldStr As String
    Private Sub SalesMaintenance_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Button6.Enabled = False
        rdNormal.Checked = True
        MaskedTextBox1.Focus()
        MaskedTextBox1.Select()
        cmb(cmbStrore, "select Store_Type,Store_Name  from Store", "Store_Type", "Store_Name", 1)
        cmb(cmbPayType, "select Payment_Types,Description  from Payment_Types", "Payment_Types", "Description", 1)
        cmbStrore.Text = ""
        DateTimePicker1.Value = Now.Date
        H_EnableFalse()
        EnbleFalseDtl()
        buttonenableF()
    End Sub

    Sub buttonenableF()
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        Button4.Enabled = False
    End Sub
    Sub buttonenable()
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        Button4.Enabled = True
    End Sub

    Private Sub MaskedTextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MaskedTextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            TransStr = "Header : "
            DetailStr = "Detail : "
            PaidStr = "Paid : "
            'Button6.Enabled = True
            'H_Enable()
            If FloorHistory = 0 Then
                dsH = getSqldb2("Select a.*,b.Store_Type from sales_transactions a inner join Cash_Register b on b.Cash_Register_ID = substring(a.transaction_number,5,3)  where a.transaction_number = '" & MaskedTextBox1.Text & "'")
                dsD = getSqldb2("Select * from sales_transaction_details where transaction_number = '" & MaskedTextBox1.Text & "' Order By Seq ")
                dsP = getSqldb2("Select * from paid where transaction_number = '" & MaskedTextBox1.Text & "' Order By Seq")
            Else
                dsH = getSqldb("Select a.*,b.Store_Type from sales_transactions a inner join Cash_Register b on b.Cash_Register_ID = substring(a.transaction_number,5,3)  where a.transaction_number = '" & MaskedTextBox1.Text & "'")
                dsD = getSqldb("Select * from sales_transaction_details where transaction_number = '" & MaskedTextBox1.Text & "' Order By Seq ")
                dsP = getSqldb("Select * from paid where transaction_number = '" & MaskedTextBox1.Text & "' Order By Seq")
            End If

            If dsH.Tables(0).Rows.Count > 0 Then
                Button5.Text = "Add New"
                H_EnableFalse()
                Button6.Enabled = True
                Button6.Text = "Edit"
                txtCustID.Text = dsH.Tables(0).Rows(0).Item("Customer_ID")
                If dsH.Tables(0).Rows(0).Item("Flag_Return") = 0 Then
                    rdNormal.Checked = True
                Else
                    rdReturn.Checked = True
                End If
                txtCashier.Text = dsH.Tables(0).Rows(0).Item("Cashier_ID").ToString.Trim
                cmbStrore.SelectedValue = dsH.Tables(0).Rows(0).Item("Store_Type")
                DateTimePicker1.Value = Format(dsH.Tables(0).Rows(0).Item("Transaction_Date"), "MM/dd/yyyy") & " " & Format(CDate(dsH.Tables(0).Rows(0).Item("Transaction_Time")), "HH:mm:ss")
                txtTotPaid.Text = CDec(dsH.Tables(0).Rows(0).Item("Total_Paid")).ToString("N0")
                txtNetPrice.Text = CDec(dsH.Tables(0).Rows(0).Item("Net_Price")).ToString("N0")
                txtTax.Text = CDec(dsH.Tables(0).Rows(0).Item("Tax")).ToString("N0")
                txtNetAmount.Text = CDec(dsH.Tables(0).Rows(0).Item("Net_Amount")).ToString("N0")
                txtChangeAmount.Text = CDec(dsH.Tables(0).Rows(0).Item("Change_Amount")).ToString("N0")
                H_NetAmount = CDec(dsH.Tables(0).Rows(0).Item("Net_Amount"))
                TransNo = dsH.Tables(0).Rows(0).Item("Transaction_Number")
                TransStr &= txtCashier.Text & "," & cmbStrore.SelectedValue & "," & DateTimePicker1.Value _
                & "," & txtTotPaid.Text & "," & cmbStrore.SelectedValue & "," & cmbStrore.SelectedValue _
                & "," & cmbStrore.SelectedValue & "," & txtNetPrice.Text & "," & txtChangeAmount.Text _
                & "," & H_NetAmount & "," & TransNo
            End If
            If dsD.Tables(0).Rows.Count > 0 Then
                'DataGridView1.AllowUserToAddRows = True
                For a = 1 To DataGridView1.Rows.Count
                    DataGridView1.Rows.RemoveAt(0)
                Next
                Dim Linexx As Integer = 0
                For Each ro As DataRow In dsD.Tables(0).Rows
                    DataGridView1.Rows.Add(1)
                    DataGridView1.Item("Column2", DataGridView1.Rows.Count - 1).Value = ro("PLU").ToString.Trim
                    DataGridView1.Item("Column3", DataGridView1.Rows.Count - 1).Value = ro("Item_Description").ToString.Trim
                    DataGridView1.Item("Column4", DataGridView1.Rows.Count - 1).Value = CDec(ro("Price")).ToString("N0")
                    DataGridView1.Item("Column5", DataGridView1.Rows.Count - 1).Value = CDec(ro("Qty")).ToString("N0")
                    DataGridView1.Item("Column6", DataGridView1.Rows.Count - 1).Value = CDec(ro("Amount")).ToString("N0")
                    DataGridView1.Item("Column7", DataGridView1.Rows.Count - 1).Value = CDec(ro("Discount_Percentage")).ToString("N0")
                    DataGridView1.Item("Column8", DataGridView1.Rows.Count - 1).Value = CDec(ro("Discount_Amount")).ToString("N0")
                    DataGridView1.Item("Column9", DataGridView1.Rows.Count - 1).Value = CDec(ro("ExtraDisc_Pct")).ToString("N0")
                    DataGridView1.Item("Column10", DataGridView1.Rows.Count - 1).Value = CDec(ro("ExtraDisc_Amt")).ToString("N0")
                    DataGridView1.Item("Column11", DataGridView1.Rows.Count - 1).Value = CDec(ro("Net_Price")).ToString("N0")
                    DataGridView1.Item("Column1", DataGridView1.Rows.Count - 1).Value = CDec(ro("Seq")).ToString("N0")
                    'getSqldb("Insert Into Sales_Transaction_Details Values ('" & MaskedTextBox1.Text & "','" & seq & "','" & txtPLU.Text & "','" & txtDesc.Text & "','" & txtPrice.Text & "','" & txtQty.Text & "','" & txtAmount.Text & "','" & txtDisc.Text & "','" & txtDiscAmount.Text & "','" & txtExDisc.Text & "','" & txtExDiscAmount.Text & "','" & txtNetPrice2.Text & "','0','0','0','0','0')")
                    DetailStr &= ro("PLU").ToString.Trim & "," & CDec(ro("Price")).ToString("N0") & "," & _
                    CDec(ro("Qty")).ToString("N0") & "," & CDec(ro("Net_Price")).ToString("N0")
                Next
                'DataGridView1.AllowUserToAddRows = False
                'DataGridView1.DataSource = dsD.Tables(0)
                'DataGridView1.Columns("Price").DefaultCellStyle.Format = "N0"
                'DataGridView1.Columns("Qty").DefaultCellStyle.Format = "N0"
                'DataGridView1.Columns("Amount").DefaultCellStyle.Format = "N0"
                'DataGridView1.Columns("Discount_Percentage").DefaultCellStyle.Format = "N0"
                'DataGridView1.Columns("Discount_Percentage").HeaderText = "Disc%"
                'DataGridView1.Columns("Discount_Amount").DefaultCellStyle.Format = "N0"
                'DataGridView1.Columns("Discount_Amount").HeaderText = "Disc_Amt"
                'DataGridView1.Columns("ExtraDisc_Pct").DefaultCellStyle.Format = "N0"
                'DataGridView1.Columns("ExtraDisc_Pct").HeaderText = "Ex_Disc%"
                'DataGridView1.Columns("ExtraDisc_Amt").DefaultCellStyle.Format = "N0"
                'DataGridView1.Columns("ExtraDisc_Amt").HeaderText = "Ex_Disc_Amt"
                'DataGridView1.Columns("Net_Price").DefaultCellStyle.Format = "N0"
                'DataGridView1.Columns("Transaction_Number").Visible = False
                'DataGridView1.Columns("Seq").Visible = False
                'DataGridView1.Columns("Points_Received").Visible = False
                'DataGridView1.Columns("Flag_Void").Visible = False
                'DataGridView1.Columns("Flag_Status").Visible = False
                'DataGridView1.Columns("Flag_Paket_Discount").Visible = False
                'DataGridView1.Columns("Share_Percentage").Visible = False
                'DataGridView1.Refresh()
                cekTotDtl()
            End If
            If dsP.Tables(0).Rows.Count > 0 Then
                For a = 1 To DataGridView2.Rows.Count
                    DataGridView2.Rows.RemoveAt(0)
                Next
                Dim dsPT As New DataSet
                Dim Linexx As Integer = 0
                For Each ro As DataRow In dsP.Tables(0).Rows
                    DataGridView2.Rows.Add(1)
                    DataGridView2.Item("Column13", DataGridView2.Rows.Count - 1).Value = ro("Payment_Types")
                    If FloorHistory = 0 Then
                        dsPT = getSqldb2("Select Description From Payment_Types Where Payment_Types = '" & ro("Payment_Types") & "'")
                    Else
                        dsPT = getSqldb("Select Description From Payment_Types Where Payment_Types = '" & ro("Payment_Types") & "'")
                    End If

                    If dsPT.Tables(0).Rows.Count > 0 Then
                        DataGridView2.Item("Column14", DataGridView2.Rows.Count - 1).Value = dsPT.Tables(0).Rows(0).Item("Description")
                    Else
                        DataGridView2.Item("Column14", DataGridView2.Rows.Count - 1).Value = ro("Payment_Types")
                    End If

                    DataGridView2.Item("Column15", DataGridView2.Rows.Count - 1).Value = ro("Credit_Card_No").ToString.Trim
                    DataGridView2.Item("Column16", DataGridView2.Rows.Count - 1).Value = ro("Credit_Card_Name").ToString.Trim
                    DataGridView2.Item("Column17", DataGridView2.Rows.Count - 1).Value = CDec(ro("Paid_Amount")).ToString("N0")
                    DataGridView2.Item("Column18", DataGridView2.Rows.Count - 1).Value = ro("Shift")
                    DataGridView2.Item("Column12", DataGridView2.Rows.Count - 1).Value = ro("Seq").ToString.Trim
                    'getSqldb("Insert Into Sales_Transaction_Details Values ('" & MaskedTextBox1.Text & "','" & seq & "','" & txtPLU.Text & "','" & txtDesc.Text & "','" & txtPrice.Text & "','" & txtQty.Text & "','" & txtAmount.Text & "','" & txtDisc.Text & "','" & txtDiscAmount.Text & "','" & txtExDisc.Text & "','" & txtExDiscAmount.Text & "','" & txtNetPrice2.Text & "','0','0','0','0','0')")
                    PaidStr &= ro("Payment_Types") & "," & ro("Credit_Card_No").ToString.Trim & "," & _
                    CDec(ro("Paid_Amount")).ToString("N0") & "," & ro("Shift") & "," & ro("Seq").ToString.Trim
                Next
                cmbPayType.Text = ""
                cekTotPaid()
            End If
        End If
    End Sub

    Sub cekTotDtl()
        Dim TotDtl As Decimal = 0
        For x As Integer = 0 To DataGridView1.Rows.Count - 1
            TotDtl += DataGridView1.Item(10, x).Value
        Next
        Label27.Text = TotDtl.ToString("N0")
        If TotDtl <> H_NetAmount Then
            Label28.Text = CDec(TotDtl - H_NetAmount).ToString("N0")
        Else
            Label28.Text = ""
        End If
    End Sub

    Sub cekTotPaid()
        Dim TotPaid As Decimal = 0
        For x As Integer = 0 To DataGridView2.Rows.Count - 1
            TotPaid += DataGridView2.Item(5, x).Value
        Next
        Label29.Text = TotPaid.ToString("N0")
        If TotPaid <> H_NetAmount Then
            Label12.Text = CDec(TotPaid - H_NetAmount).ToString("N0")
        Else
            Label12.Text = ""
        End If
    End Sub

    Sub H_Enable()
        txtCashier.Enabled = True
        cmbStrore.Enabled = True
        DateTimePicker1.Enabled = True
        txtTotPaid.Enabled = True
        txtNetPrice.Enabled = True
        txtTax.Enabled = True
        txtNetAmount.Enabled = True
        txtChangeAmount.Enabled = True
        GroupBox1.Enabled = True
    End Sub

    Sub H_EnableFalse()
        txtCashier.Enabled = False
        cmbStrore.Enabled = False
        DateTimePicker1.Enabled = False
        txtTotPaid.Enabled = False
        txtNetPrice.Enabled = False
        txtTax.Enabled = False
        txtNetAmount.Enabled = False
        txtChangeAmount.Enabled = False
        GroupBox1.Enabled = False
    End Sub

    Private Sub DataGridView1_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            EnbleFalseDtl()
            RowDg1 = e.RowIndex
            seq = DataGridView1.Item("Column1", DataGridView1.CurrentRow.Index).Value
            txtPLU.Text = DataGridView1.Item("Column2", DataGridView1.CurrentRow.Index).Value
            txtDesc.Text = DataGridView1.Item("Column3", DataGridView1.CurrentRow.Index).Value
            txtPrice.Text = CDec(DataGridView1.Item("Column4", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtQty.Text = CDec(DataGridView1.Item("Column5", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtAmount.Text = CDec(DataGridView1.Item("Column6", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtDisc.Text = CDec(DataGridView1.Item("Column7", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtDiscAmount.Text = CDec(DataGridView1.Item("Column8", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtExDisc.Text = CDec(DataGridView1.Item("Column9", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtExDiscAmount.Text = CDec(DataGridView1.Item("Column10", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtNetPrice2.Text = CDec(DataGridView1.Item("Column11", DataGridView1.CurrentRow.Index).Value).ToString("N0")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DataGridView2_CellClick(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles DataGridView2.CellClick
        Try
            RowDg2 = e.RowIndex
            cmbPayType.SelectedValue = Trim(DataGridView2.Item("Column13", DataGridView2.CurrentRow.Index).Value)
            txtCCNo.Text = DataGridView2.Item("Column15", DataGridView2.CurrentRow.Index).Value
            txtCCName.Text = DataGridView2.Item("Column16", DataGridView2.CurrentRow.Index).Value
            txtPaidAmount.Text = CDec(DataGridView2.Item("Column17", DataGridView2.CurrentRow.Index).Value).ToString("N0")
            txtShift.Text = DataGridView2.Item("Column18", DataGridView2.CurrentRow.Index).Value
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        Dim dsCekPaid, dsCekVoucher As New DataSet
        
        H_Enable()
        buttonenable()
        txtCustID.Focus()
        Dim dsDetail, DsPaid As New DataSet
        Dim TypeTrans As String = ""
        If rdNormal.Checked = True Then
            TypeTrans = "0"
        Else
            TypeTrans = "1"
        End If

        If Replace(Button6.Text, "&", "") = "Edit" Then
            Button6.Text = "Update"
            MaskedTextBox1.Enabled = False
            Button5.Enabled = False
        Else
            Button5.Enabled = True
            If txtNetPrice.Text <> Label27.Text Then
                MsgBox("Detail total are not The Same as Header Total")
                Exit Sub
            End If

            If txtNetPrice.Text > Label29.Text Then
                MsgBox("Paid total is less than the details total")
                Exit Sub
            End If

            MaskedTextBox1.Enabled = True
            Button6.Text = "Edit"
            If FloorHistory = 0 Then
                getSqldb2("Update Sales_Transactions Set Customer_ID = '" & txtCustID.Text.Trim & "',Flag_Return = '" & TypeTrans & "',Cashier_ID = '" & txtCashier.Text & "',Transaction_Time = '" & Format(DateTimePicker1.Value, "HH:mm") & "',Total_Paid = '" & txtTotPaid.Text & "',Net_Price = '" & txtNetPrice.Text & "',Tax = '" & txtTax.Text & "',Net_Amount = '" & txtNetAmount.Text & "',Change_Amount = '" & txtChangeAmount.Text & "' Where Transaction_Number = '" & MaskedTextBox1.Text & "' ")
            Else
                getSqldb("Update Sales_Transactions Set Customer_ID = '" & txtCustID.Text.Trim & "',Flag_Return = '" & TypeTrans & "',Cashier_ID = '" & txtCashier.Text & "',Transaction_Time = '" & Format(DateTimePicker1.Value, "HH:mm") & "',Total_Paid = '" & txtTotPaid.Text & "',Net_Price = '" & txtNetPrice.Text & "',Tax = '" & txtTax.Text & "',Net_Amount = '" & txtNetAmount.Text & "',Change_Amount = '" & txtChangeAmount.Text & "' Where Transaction_Number = '" & MaskedTextBox1.Text & "' ")
            End If

            For x As Integer = 0 To DataGridView1.Rows.Count - 1

                If FloorHistory = 0 Then

                    dsDetail = getSqldb2("Select * from Sales_Transaction_Details Where Transaction_Number = '" & MaskedTextBox1.Text & "' And Seq = '" & DataGridView1.Item("Column1", x).Value & "'")
                    If dsDetail.Tables(0).Rows.Count > 0 Then
                        getSqldb2("Update Sales_Transaction_Details Set PLU = '" & DataGridView1.Item("Column2", x).Value & "',Item_Description = '" & DataGridView1.Item("Column3", x).Value & "',Price = '" & DataGridView1.Item("Column4", x).Value & "',Qty = '" & DataGridView1.Item("Column5", x).Value & "',Amount = '" & DataGridView1.Item("Column6", x).Value & "',Discount_Percentage = '" & DataGridView1.Item("Column7", x).Value & "',Discount_Amount = '" & DataGridView1.Item("Column8", x).Value & "',ExtraDisc_Pct = '" & DataGridView1.Item("Column9", x).Value & "',ExtraDisc_Amt = '" & DataGridView1.Item("Column10", x).Value & "',Net_Price = '" & DataGridView1.Item("Column11", x).Value & "' Where Transaction_Number = '" & MaskedTextBox1.Text & "' And Seq = '" & DataGridView1.Item("Column1", x).Value & "'")
                    Else
                        getSqldb2("Insert Into Sales_Transaction_Details Values ('" & MaskedTextBox1.Text & "','" & DataGridView1.Item("Column1", x).Value & "','" & DataGridView1.Item("Column2", x).Value & "','" & DataGridView1.Item("Column3", x).Value & "','" & DataGridView1.Item("Column4", x).Value & "','" & DataGridView1.Item("Column5", x).Value & "','" & DataGridView1.Item("Column6", x).Value & "','" & DataGridView1.Item("Column7", x).Value & "','" & DataGridView1.Item("Column8", x).Value & "','" & DataGridView1.Item("Column9", x).Value & "','" & DataGridView1.Item("Column10", x).Value & "','" & DataGridView1.Item("Column11", x).Value & "',0,0,0,0,0)")
                    End If
                Else
                    dsDetail = getSqldb("Select * from Sales_Transaction_Details Where Transaction_Number = '" & MaskedTextBox1.Text & "' And Seq = '" & DataGridView1.Item("Column1", x).Value & "'")
                    If dsDetail.Tables(0).Rows.Count > 0 Then
                        getSqldb("Update Sales_Transaction_Details Set PLU = '" & DataGridView1.Item("Column2", x).Value & "',Item_Description = '" & DataGridView1.Item("Column3", x).Value & "',Price = '" & DataGridView1.Item("Column4", x).Value & "',Qty = '" & DataGridView1.Item("Column5", x).Value & "',Amount = '" & DataGridView1.Item("Column6", x).Value & "',Discount_Percentage = '" & DataGridView1.Item("Column7", x).Value & "',Discount_Amount = '" & DataGridView1.Item("Column8", x).Value & "',ExtraDisc_Pct = '" & DataGridView1.Item("Column9", x).Value & "',ExtraDisc_Amt = '" & DataGridView1.Item("Column10", x).Value & "',Net_Price = '" & DataGridView1.Item("Column11", x).Value & "' Where Transaction_Number = '" & MaskedTextBox1.Text & "' And Seq = '" & DataGridView1.Item("Column1", x).Value & "'")
                    Else
                        getSqldb("Insert Into Sales_Transaction_Details Values ('" & MaskedTextBox1.Text & "','" & DataGridView1.Item("Column1", x).Value & "','" & DataGridView1.Item("Column2", x).Value & "','" & DataGridView1.Item("Column3", x).Value & "','" & DataGridView1.Item("Column4", x).Value & "','" & DataGridView1.Item("Column5", x).Value & "','" & DataGridView1.Item("Column6", x).Value & "','" & DataGridView1.Item("Column7", x).Value & "','" & DataGridView1.Item("Column8", x).Value & "','" & DataGridView1.Item("Column9", x).Value & "','" & DataGridView1.Item("Column10", x).Value & "','" & DataGridView1.Item("Column11", x).Value & "',0,0,0,0,0)")
                    End If
                End If

            Next
            For x As Integer = 0 To DataGridView2.Rows.Count - 1
                If FloorHistory = 0 Then
                    If Trim(cmbPayType.SelectedValue) = "8" Then
                        dsCekPaid = getSqldb2("Select * from Paid where Payment_Types in ('8') And Credit_Card_No = '" & Trim(DataGridView2.Item("Column15", x).Value) & "'")
                        If dsCekPaid.Tables(0).Rows.Count = 0 Then
                            dsCekVoucher = getSqldb2("select * from newvoc where v_no = '" & Trim(DataGridView2.Item("Column15", x).Value) & "' And V_Flag = 'R'")
                            If dsCekVoucher.Tables(0).Rows.Count > 0 Then
                                MsgBox("Transaction '" & MaskedTextBox1.Text & "' can not be processed due to duplicate in 'Other Store' Voucher Number !!")
                                Exit Sub
                            End If
                        End If
                    End If

                    DsPaid = getSqldb2("Select * from Paid Where Transaction_Number = '" & MaskedTextBox1.Text & "' And Seq = '" & DataGridView2.Item("Column12", x).Value & "'")
                    If DsPaid.Tables(0).Rows.Count > 0 Then
                        getSqldb2("Update Paid Set Payment_Types = '" & DataGridView2.Item("Column13", x).Value & "',Credit_Card_No = '" & DataGridView2.Item("Column15", x).Value & "',Credit_Card_Name = '" & DataGridView2.Item("Column16", x).Value & "',Paid_Amount = '" & DataGridView2.Item("Column17", x).Value & "',Shift = '" & DataGridView2.Item("Column18", x).Value & "' Where Transaction_Number = '" & MaskedTextBox1.Text & "' And Seq = '" & DataGridView2.Item("Column12", x).Value & "'")
                    Else
                        getSqldb2("Insert Into Paid Values ('" & MaskedTextBox1.Text & "','" & DataGridView2.Item("Column13", x).Value & "','" & DataGridView2.Item("Column12", x).Value & "','IDR','1','" & DataGridView2.Item("Column15", x).Value & "','" & DataGridView2.Item("Column16", x).Value & "','" & DataGridView2.Item("Column17", x).Value & "','" & DataGridView2.Item("Column18", x).Value & "')")
                    End If
                Else
                    If Trim(cmbPayType.SelectedValue) = "8" Then
                        dsCekPaid = getSqldb("Select * from Paid where Payment_Types in ('8') And Credit_Card_No = '" & Trim(DataGridView2.Item("Column15", x).Value) & "'")
                        If dsCekPaid.Tables(0).Rows.Count = 0 Then
                            dsCekVoucher = getSqldb("select * from newvoc where v_no = '" & Trim(DataGridView2.Item("Column15", x).Value) & "' And V_Flag = 'R'")
                            If dsCekVoucher.Tables(0).Rows.Count > 0 Then
                                MsgBox("Transaction '" & MaskedTextBox1.Text & "' can not be processed due to duplicate in 'Other Store' Voucher Number !!")
                                Exit Sub
                            End If
                        End If
                    End If


                    DsPaid = getSqldb("Select * from Paid Where Transaction_Number = '" & MaskedTextBox1.Text & "' And Seq = '" & DataGridView2.Item("Column12", x).Value & "'")
                    If DsPaid.Tables(0).Rows.Count > 0 Then
                        getSqldb("Update Paid Set Payment_Types = '" & DataGridView2.Item("Column13", x).Value & "',Credit_Card_No = '" & DataGridView2.Item("Column15", x).Value & "',Credit_Card_Name = '" & DataGridView2.Item("Column16", x).Value & "',Paid_Amount = '" & DataGridView2.Item("Column17", x).Value & "',Shift = '" & DataGridView2.Item("Column18", x).Value & "' Where Transaction_Number = '" & MaskedTextBox1.Text & "' And Seq = '" & DataGridView2.Item("Column12", x).Value & "'")
                    Else
                        getSqldb("Insert Into Paid Values ('" & MaskedTextBox1.Text & "','" & DataGridView2.Item("Column13", x).Value & "','" & DataGridView2.Item("Column12", x).Value & "','IDR','1','" & DataGridView2.Item("Column15", x).Value & "','" & DataGridView2.Item("Column16", x).Value & "','" & DataGridView2.Item("Column17", x).Value & "','" & DataGridView2.Item("Column18", x).Value & "')")
                    End If
                End If

            Next
            ClearScreen()
            buttonenableF()
            If Not Directory.Exists(Application.StartupPath & "\LogBack") Then
                Directory.CreateDirectory(Application.StartupPath & "\LogBack")
            End If

            If File.Exists(Application.StartupPath & "\LogBack\" & Format(Now, "ddMMyyyy") & "_Log.txt") Then
                Using sr As New StreamReader(Application.StartupPath & "\LogBack\" & Format(Now, "ddMMyyyy") & "_Log.txt")
                    Dim line As String
                    line = sr.ReadToEnd
                    OldStr = line
                End Using
            End If

            If File.Exists(Application.StartupPath & "\LogBack\" & Format(Now, "ddMMyyyy") & "_Log.txt") Then
                File.Delete(Application.StartupPath & "\LogBack\" & Format(Now, "ddMMyyyy") & "_Log.txt")
            End If

            Dim sw As New IO.StreamWriter(Application.StartupPath & "\LogBack\" & Format(Now, "ddMMyyyy") & "_Log.txt")

            sw.Write(TransStr)
            sw.Write(vbNewLine)
            sw.Write(DetailStr)
            sw.Write(vbNewLine)
            sw.Write(PaidStr)
            sw.Write(vbNewLine)
            sw.Close()
            sw.Dispose()
        End If

    End Sub

    Private Sub Button7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button7.Click
        Button5.Text = "Add New"
        ClearScreen()
        buttonenableF()
        H_EnableFalse()
    End Sub

    Sub ClearScreen()
        'Header
        MaskedTextBox1.Clear()
        MaskedTextBox1.Enabled = True
        MaskedTextBox1.Focus()
        txtCustID.Clear()
        txtCashier.Clear()
        cmbStrore.SelectedValue = ""
        DateTimePicker1.Value = Now.Date
        txtTotPaid.Clear()
        txtNetPrice.Clear()
        txtNetPrice2.Clear()
        txtTax.Clear()
        txtNetAmount.Clear()
        txtChangeAmount.Clear()
        Button6.Enabled = False
        Button5.Enabled = True
        'Detail
        txtPLU.Clear()
        txtDesc.Clear()
        txtPrice.Clear()
        txtQty.Clear()
        txtAmount.Clear()
        txtDisc.Clear()
        txtDiscAmount.Clear()
        txtExDisc.Clear()
        txtExDiscAmount.Clear()
        txtNetPrice.Clear()
        For a = 1 To DataGridView1.Rows.Count
            DataGridView1.Rows.RemoveAt(0)
        Next
        Label28.Text = ""
        Label27.Text = 0
        'paid
        cmbPayType.SelectedValue = ""
        txtCCNo.Clear()
        txtCCName.Clear()
        txtPaidAmount.Clear()
        txtShift.Clear()
        For a = 1 To DataGridView2.Rows.Count
            DataGridView2.Rows.RemoveAt(0)
        Next
        Label12.Text = ""
        Label29.Text = 0
    End Sub

    Sub EnbleFalseDtl()
        txtDesc.Enabled = False
        txtAmount.Enabled = False
        txtNetPrice2.Enabled = False
    End Sub

    Sub EnbleDtl()
        txtDesc.Enabled = False
        txtAmount.Enabled = False
        txtNetPrice2.Enabled = False
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        If Replace(Button6.Text, "&", "") = "Save" Then
            com = True
            For Each c As Control In TabControl1.TabPages(0).Controls
                If TypeOf (c) Is TextBox Or TypeOf (c) Is ComboBox Then
                    If c.Text = "" Then
                        MsgBox("Data Not Valid, Please Re-Check !!!")
                        Exit Sub
                    End If
                End If
            Next
            If DataGridView1.Rows.Count > 0 And DataGridView2.Rows.Count > 0 Then
                Button5.Text = "Add New"
                MaskedTextBox1.Focus()
            Else
                MsgBox("Data Not Valid, Please Re-Check !!!")
            End If

        Else
            Button5.Text = "Save"
            buttonenable()
            ClearScreen()
            H_Enable()
            MaskedTextBox1.Focus()
        End If

    End Sub

    Public Sub ClearTextBox(ByVal root As Control)
        For Each ctrl As Control In root.Controls
            ClearTextBox(ctrl)
            If TypeOf ctrl Is TextBox Then
                CType(ctrl, TextBox).Text = String.Empty
            End If
        Next ctrl
    End Sub

    Public Sub CheckTextBox(ByVal root As Control)
        For Each ctrl As Control In root.Controls
            CheckTextBox(ctrl)
            If TypeOf ctrl Is TextBox Then
                If CType(ctrl, TextBox).Text = String.Empty Then
                    com = False
                    Exit Sub
                End If
            End If
        Next ctrl
    End Sub


    Private Sub Button8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button8.Click
        Me.Close()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        DataGridView1.Item("Column2", RowDg1).Value = txtPLU.Text
        DataGridView1.Item("Column3", RowDg1).Value = txtDesc.Text
        DataGridView1.Item("Column4", RowDg1).Value = txtPrice.Text
        DataGridView1.Item("Column5", RowDg1).Value = txtQty.Text
        DataGridView1.Item("Column6", RowDg1).Value = txtAmount.Text
        DataGridView1.Item("Column7", RowDg1).Value = txtDisc.Text
        DataGridView1.Item("Column8", RowDg1).Value = txtDiscAmount.Text
        DataGridView1.Item("Column9", RowDg1).Value = txtExDisc.Text
        DataGridView1.Item("Column10", RowDg1).Value = txtExDiscAmount.Text
        DataGridView1.Item("Column11", RowDg1).Value = txtNetPrice2.Text
        'getSqldb("Update Sales_Transaction_Details Set PLU = '" & txtPLU.Text & "',Item_Description = '" & txtDesc.Text & "',Price = '" & txtPrice.Text & "',Qty = '" & txtQty.Text & "',Amount = '" & txtAmount.Text & "',Discount_Percentage = '" & txtDisc.Text & "',Discount_Amount = '" & txtDiscAmount.Text & "',ExtraDisc_Pct = '" & txtExDisc.Text & "',ExtraDisc_Amt = '" & txtExDiscAmount.Text & "',Net_Price = '" & txtNetPrice2.Text & "' Where Transaction_Number = '" & MaskedTextBox1.Text & "' And Seq = '" & seq & "'")
        cekTotDtl()
        ClearD()
    End Sub

    Sub ClearD()
        txtQty.Clear()
        txtPLU.Clear()
        txtDesc.Clear()
        txtPrice.Clear()
        txtAmount.Clear()
        txtDisc.Clear()
        txtDiscAmount.Clear()
        txtExDisc.Clear()
        txtExDiscAmount.Clear()
        txtNetPrice2.Clear()
        txtQty.Focus()
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim seq As Integer = 0
        Dim dsSeq As New DataSet
        If FloorHistory = 0 Then
            dsSeq = getSqldb2("Select * from Sales_Transaction_Details where Transaction_Number = '" & MaskedTextBox1.Text & "' Order By Seq Desc")
        Else
            dsSeq = getSqldb("Select * from Sales_Transaction_Details where Transaction_Number = '" & MaskedTextBox1.Text & "' Order By Seq Desc")
        End If
        If dsSeq.Tables(0).Rows.Count > 0 Then
            seq = dsSeq.Tables(0).Rows(0).Item("Seq") + 1
        Else
            seq = 1
        End If
        DataGridView1.Rows.Add(1)
        DataGridView1.Item("Column2", DataGridView1.Rows.Count - 1).Value = txtPLU.Text
        DataGridView1.Item("Column3", DataGridView1.Rows.Count - 1).Value = txtDesc.Text
        DataGridView1.Item("Column4", DataGridView1.Rows.Count - 1).Value = txtPrice.Text
        DataGridView1.Item("Column5", DataGridView1.Rows.Count - 1).Value = txtQty.Text
        DataGridView1.Item("Column6", DataGridView1.Rows.Count - 1).Value = txtAmount.Text
        DataGridView1.Item("Column7", DataGridView1.Rows.Count - 1).Value = txtDisc.Text
        DataGridView1.Item("Column8", DataGridView1.Rows.Count - 1).Value = txtDiscAmount.Text
        DataGridView1.Item("Column9", DataGridView1.Rows.Count - 1).Value = txtExDisc.Text
        DataGridView1.Item("Column10", DataGridView1.Rows.Count - 1).Value = txtExDiscAmount.Text
        DataGridView1.Item("Column11", DataGridView1.Rows.Count - 1).Value = txtNetPrice2.Text
        DataGridView1.Item("Column1", DataGridView1.Rows.Count - 1).Value = seq
        'getSqldb("Insert Into Sales_Transaction_Details Values ('" & MaskedTextBox1.Text & "','" & seq & "','" & txtPLU.Text & "','" & txtDesc.Text & "','" & txtPrice.Text & "','" & txtQty.Text & "','" & txtAmount.Text & "','" & txtDisc.Text & "','" & txtDiscAmount.Text & "','" & txtExDisc.Text & "','" & txtExDiscAmount.Text & "','" & txtNetPrice2.Text & "','0','0','0','0','0')")
        cekTotDtl()
        ClearD()
    End Sub

    Private Sub txtPLU_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPLU.KeyDown
        Dim dsItemMaster As New DataSet
        If e.KeyCode = Keys.Enter Then
            dsItemMaster = getSqldb("Select * from Item_Master Where PLU = '" & txtPLU.Text & "'")
            If dsItemMaster.Tables(0).Rows.Count > 0 Then
                txtDesc.Text = dsItemMaster.Tables(0).Rows(0).Item("Description")
                txtPrice.Focus()
            Else
                MsgBox("PLU Can not Found !!!")
                txtDesc.Text = ""
                txtPLU.Focus()
            End If
        End If
    End Sub




    Private Sub txtNetPrice_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtNetPrice.KeyPress, txtTotPaid.KeyPress, txtCashier.KeyPress, txtTax.KeyPress, txtNetAmount.KeyPress, txtChangeAmount.KeyPress, txtQty.KeyPress, txtAmount.KeyPress, txtDisc.KeyPress, txtDiscAmount.KeyPress, txtExDisc.KeyPress, txtExDiscAmount.KeyPress, txtNetPrice2.KeyPress
        If Asc(e.KeyChar) <> 8 Then
            If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
                e.Handled = True
            End If
        End If
    End Sub

    Private Sub txtTotPaid_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTotPaid.KeyUp
        If txtTotPaid.Text.Length > 0 And txtNetPrice.Text.Length And txtTax.Text.Length Then
            txtChangeAmount.Text = txtTotPaid.Text - txtNetPrice.Text - txtTax.Text
        End If

    End Sub

    Private Sub txtTotPaid_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTotPaid.TextChanged
        If txtTotPaid.Text.Length > 3 Then
            txtTotPaid.Text = CDec(txtTotPaid.Text).ToString("N0")
            txtTotPaid.SelectionStart = txtTotPaid.TextLength
        End If
    End Sub

    Private Sub txtNetPrice_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtNetPrice.KeyUp
        If txtTotPaid.Text.Length > 0 And txtNetPrice.Text.Length And txtTax.Text.Length Then
            txtChangeAmount.Text = txtTotPaid.Text - txtNetPrice.Text - txtTax.Text
            txtNetAmount.Text = txtNetPrice.Text
        End If

    End Sub

    Private Sub txtNetPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetPrice.TextChanged
        If txtNetPrice.Text.Length > 3 Then
            txtNetPrice.Text = CDec(txtNetPrice.Text).ToString("N0")
            txtNetPrice.SelectionStart = txtNetPrice.TextLength
        End If
    End Sub

    Private Sub txtTax_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtTax.KeyUp
        If txtTotPaid.Text.Length > 0 And txtNetPrice.Text.Length And txtTax.Text.Length Then
            txtChangeAmount.Text = txtTotPaid.Text - txtNetPrice.Text - txtTax.Text
            txtNetAmount.Text = txtNetPrice.Text
        End If
    End Sub

    Private Sub txtTax_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtTax.TextChanged
        If txtTax.Text.Length > 3 Then
            txtTax.Text = CDec(txtTax.Text).ToString("N0")
            txtTax.SelectionStart = txtTax.TextLength
        End If
    End Sub

    Private Sub txtNetAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtNetAmount.TextChanged
        If txtNetAmount.Text.Length > 3 Then
            txtNetAmount.Text = CDec(txtNetAmount.Text).ToString("N0")
            txtNetAmount.SelectionStart = txtNetAmount.TextLength
        End If
    End Sub

    Private Sub txtChangeAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtChangeAmount.TextChanged
        If txtChangeAmount.Text.Length > 3 Then
            txtChangeAmount.Text = CDec(txtChangeAmount.Text).ToString("N0")
            txtChangeAmount.SelectionStart = txtChangeAmount.TextLength
        End If


    End Sub

    Private Sub TextBox_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MaskedTextBox1.Validating, txtCustID.Validating, cmbStrore.Validating, txtCashier.Validating, txtTotPaid.Validating, txtNetPrice.Validating, txtTax.Validating, txtNetAmount.Validating, txtChangeAmount.Validating
        Dim ctl As Control = CType(sender, Control)
        If ctl.Text = "" Then
            e.Cancel = True
        End If
    End Sub

    Private Sub txtPrice_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtPrice.KeyUp
        If txtQty.Text.Length > 0 And txtPrice.Text.Length > 0 Then
            txtAmount.Text = CDec(txtQty.Text * txtPrice.Text).ToString("N0")
            txtDiscAmount.Text = CDec((txtDisc.Text / 100) * txtAmount.Text).ToString("N0")
            txtExDiscAmount.Text = CDec((txtExDisc.Text / 100) * txtAmount.Text).ToString("N0")
            txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
        End If
    End Sub

    Private Sub txtQty_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtQty.KeyUp
        If txtQty.Text.Length > 0 And txtPrice.Text.Length > 0 Then
            txtAmount.Text = CDec(txtQty.Text * txtPrice.Text).ToString("N0")
            txtDiscAmount.Text = CDec((txtDisc.Text / 100) * txtAmount.Text).ToString("N0")
            txtExDiscAmount.Text = CDec((txtExDisc.Text / 100) * txtAmount.Text).ToString("N0")
            txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
        End If
    End Sub

    Private Sub txtDisc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDisc.KeyUp
        If txtDisc.Text.Length > 0 Then
            txtDiscAmount.Text = CDec((txtDisc.Text / 100) * txtAmount.Text).ToString("N0")
            txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
        End If
    End Sub

    Private Sub txtExDisc_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtExDisc.KeyUp
        If txtExDisc.Text.Length > 0 Then
            txtExDiscAmount.Text = CDec((txtExDisc.Text / 100) * txtAmount.Text).ToString("N0")
            txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Dim seq As Integer = 0
        Dim dsSeq, dsCekVoucher As New DataSet
        If txtShift.Text = "" Then
            MsgBox("Please Insert Shift First !!!")
            Exit Sub
        End If
        If FloorHistory = 0 Then
            If Trim(cmbPayType.SelectedValue) = "8" Then
                dsCekVoucher = getSqldb2("Select * from Paid where Payment_Types in ('8') And Credit_Card_No = '" & Trim(txtCCNo.Text) & "' And Transaction_Number <> '" & MaskedTextBox1.Text & "'")
                If dsCekVoucher.Tables(0).Rows.Count > 0 Then
                    MsgBox("Transaction '" & MaskedTextBox1.Text & "' can not be processed due to duplicate Voucher Number !!")
                    Exit Sub
                End If
            End If
        Else
            If Trim(cmbPayType.SelectedValue) = "8" Then
                dsCekVoucher = getSqldb("Select * from Paid where Payment_Types in ('8') And Credit_Card_No = '" & Trim(txtCCNo.Text) & "' And Transaction_Number <> '" & MaskedTextBox1.Text & "'")
                If dsCekVoucher.Tables(0).Rows.Count > 0 Then
                    MsgBox("Transaction '" & MaskedTextBox1.Text & "' can not be processed due to duplicate Voucher Number !!")
                    Exit Sub
                End If
            End If
        End If
        
        If FloorHistory = 0 Then
            dsSeq = getSqldb2("Select * from paid where Transaction_Number = '" & MaskedTextBox1.Text & "' Order By Seq Desc")
        Else
            dsSeq = getSqldb("Select * from paid where Transaction_Number = '" & MaskedTextBox1.Text & "' Order By Seq Desc")
        End If

        'If dsSeq.Tables(0).Rows.Count > 0 Then
        '    seq = dsSeq.Tables(0).Rows(0).Item("Seq") + 1
        'Else
        '    seq = 1
        'End If

        seq = DataGridView2.RowCount + 1

        DataGridView2.Rows.Add(1)
        DataGridView2.Item("Column13", DataGridView2.Rows.Count - 1).Value = cmbPayType.SelectedValue
        DataGridView2.Item("Column14", DataGridView2.Rows.Count - 1).Value = cmbPayType.Text
        DataGridView2.Item("Column15", DataGridView2.Rows.Count - 1).Value = txtCCNo.Text
        DataGridView2.Item("Column16", DataGridView2.Rows.Count - 1).Value = txtCCName.Text
        DataGridView2.Item("Column17", DataGridView2.Rows.Count - 1).Value = txtPaidAmount.Text
        DataGridView2.Item("Column18", DataGridView2.Rows.Count - 1).Value = txtShift.Text
        DataGridView2.Item("Column12", DataGridView2.Rows.Count - 1).Value = seq
        'getSqldb("Insert Into Sales_Transaction_Details Values ('" & MaskedTextBox1.Text & "','" & seq & "','" & txtPLU.Text & "','" & txtDesc.Text & "','" & txtPrice.Text & "','" & txtQty.Text & "','" & txtAmount.Text & "','" & txtDisc.Text & "','" & txtDiscAmount.Text & "','" & txtExDisc.Text & "','" & txtExDiscAmount.Text & "','" & txtNetPrice2.Text & "','0','0','0','0','0')")
        cekTotPaid()
        ClearP()
    End Sub

    Sub ClearP()
        cmbPayType.Text = ""
        txtCCNo.Clear()
        txtCCName.Clear()
        txtShift.Clear()
        txtPaidAmount.Clear()
        cmbPayType.Focus()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If txtShift.Text = "" Then
            MsgBox("Please Insert Shift First !!!")
            Exit Sub
        End If
        If txtPaidAmount.Text <> "" And (cmbPayType.Text <> "" Or DataGridView1.RowCount > 0) Then
            Try
                Dim dsCekVoucher As New DataSet
                If FloorHistory = 0 Then
                    If Trim(cmbPayType.SelectedValue) = "8" Then
                        dsCekVoucher = getSqldb2("Select * from Paid where Payment_Types in ('8') And Credit_Card_No = '" & Trim(txtCCNo.Text) & "' And Transaction_Number <> '" & MaskedTextBox1.Text & "'")
                        If dsCekVoucher.Tables(0).Rows.Count > 0 Then
                            MsgBox("Transaction '" & MaskedTextBox1.Text & "' can not be processed due to duplicate Voucher Number !!")
                            Exit Sub
                        End If
                    End If
                Else
                    If Trim(cmbPayType.SelectedValue) = "8" Then
                        dsCekVoucher = getSqldb("Select * from Paid where Payment_Types in ('8') And Credit_Card_No = '" & Trim(txtCCNo.Text) & "' And Transaction_Number <> '" & MaskedTextBox1.Text & "'")
                        If dsCekVoucher.Tables(0).Rows.Count > 0 Then
                            MsgBox("Transaction '" & MaskedTextBox1.Text & "' can not be processed due to duplicate Voucher Number !!")
                            Exit Sub
                        End If
                    End If
                End If
                DataGridView2.Item("Column13", RowDg2).Value = cmbPayType.SelectedValue
                DataGridView2.Item("Column14", RowDg2).Value = cmbPayType.Text
                DataGridView2.Item("Column15", RowDg2).Value = txtCCNo.Text
                DataGridView2.Item("Column16", RowDg2).Value = txtCCName.Text
                DataGridView2.Item("Column17", RowDg2).Value = txtPaidAmount.Text
                DataGridView2.Item("Column18", RowDg2).Value = txtShift.Text
                'getSqldb("Update Sales_Transaction_Details Set PLU = '" & txtPLU.Text & "',Item_Description = '" & txtDesc.Text & "',Price = '" & txtPrice.Text & "',Qty = '" & txtQty.Text & "',Amount = '" & txtAmount.Text & "',Discount_Percentage = '" & txtDisc.Text & "',Discount_Amount = '" & txtDiscAmount.Text & "',ExtraDisc_Pct = '" & txtExDisc.Text & "',ExtraDisc_Amt = '" & txtExDiscAmount.Text & "',Net_Price = '" & txtNetPrice2.Text & "' Where Transaction_Number = '" & MaskedTextBox1.Text & "' And Seq = '" & seq & "'")
                cekTotPaid()
                ClearD()
            Catch ex As Exception
                MsgBox("Please click on the payment to be changed !!!", MsgBoxStyle.Critical)
            End Try
        End If

    End Sub

    Private Sub txtPaidAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPaidAmount.TextChanged
        Try
            If txtPaidAmount.Text.Length > 3 Then
                txtPaidAmount.Text = CDec(txtPaidAmount.Text).ToString("N0")
                txtPaidAmount.SelectionStart = txtPaidAmount.TextLength
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtPrice_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPrice.Leave

        If txtPrice.Text.Length = 0 Then
            If txtDisc.Text = "" Then
                txtDisc.Text = 0
            End If
            If txtExDisc.Text = "" Then
                txtExDisc.Text = 0
            End If
            txtPrice.Text = 0
            txtAmount.Text = CDec(txtQty.Text * txtPrice.Text).ToString("N0")
            txtDiscAmount.Text = CDec((txtDisc.Text / 100) * txtAmount.Text).ToString("N0")
            txtExDiscAmount.Text = CDec((txtExDisc.Text / 100) * txtAmount.Text).ToString("N0")
            txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
        End If
    End Sub

    Private Sub txtPrice_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPrice.TextChanged
        Try
            If txtPrice.Text.Length > 3 Then
                txtPrice.Text = CDec(txtPrice.Text).ToString("N0")
                txtPrice.SelectionStart = txtPrice.TextLength
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtExDisc_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExDisc.Leave
        Try
            If txtExDisc.Text.Length = 0 Then
                txtExDisc.Text = 0
                txtExDiscAmount.Text = CDec((txtExDisc.Text / 100) * txtAmount.Text).ToString("N0")
                txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtDisc_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDisc.Leave
        Try
            If txtDisc.Text.Length = 0 Then
                txtDisc.Text = 0
                txtDiscAmount.Text = CDec((txtDisc.Text / 100) * txtAmount.Text).ToString("N0")
                txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtDiscAmount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtDiscAmount.KeyUp
        Try
            If txtDiscAmount.Text.Length > 0 Then
                txtDisc.Text = CDec((txtDiscAmount.Text / txtAmount.Text) * 100).ToString("N0")
                txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtExDiscAmount_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtExDiscAmount.KeyUp
        Try
            If txtExDiscAmount.Text.Length > 0 Then
                txtExDisc.Text = CDec((txtExDiscAmount.Text / txtAmount.Text) * 100).ToString("N0")
                txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtDiscAmount_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscAmount.Leave
        Try
            If txtDiscAmount.Text.Length = 0 Then
                txtDiscAmount.Text = 0
                txtDisc.Text = CDec((txtDiscAmount.Text / txtAmount.Text) * 100).ToString("N0")
                txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtExDiscAmount_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExDiscAmount.Leave
        Try
            If txtExDiscAmount.Text.Length = 0 Then
                txtExDiscAmount.Text = 0
                txtExDisc.Text = CDec((txtExDiscAmount.Text / txtAmount.Text) * 100).ToString("N0")
                txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtDiscAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDiscAmount.TextChanged
        Try
            If txtDiscAmount.Text.Length > 3 Then
                txtDiscAmount.Text = CDec(txtDiscAmount.Text).ToString("N0")
                txtDiscAmount.SelectionStart = txtDiscAmount.TextLength
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtExDiscAmount_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExDiscAmount.TextChanged
        Try
            If txtExDiscAmount.Text.Length > 3 Then
                txtExDiscAmount.Text = CDec(txtExDiscAmount.Text).ToString("N0")
                txtExDiscAmount.SelectionStart = txtExDiscAmount.TextLength
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtDisc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtDisc.TextChanged
        Try
            If txtDisc.Text.Length > 3 Then
                txtDisc.Text = CDec(txtDisc.Text).ToString("N0")
                txtDisc.SelectionStart = txtDisc.TextLength
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtExDisc_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtExDisc.TextChanged
        Try
            If txtExDisc.Text.Length > 3 Then
                txtExDisc.Text = CDec(txtExDisc.Text).ToString("N0")
                txtExDisc.SelectionStart = txtExDisc.TextLength
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub txtQty_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtQty.Leave
        If txtQty.Text.Length = 0 Then
            txtQty.Text = 1
            If txtPrice.Text.Length = 0 Then
            Else
                txtAmount.Text = CDec(txtQty.Text * txtPrice.Text).ToString("N0")
                txtDiscAmount.Text = CDec((txtDisc.Text / 100) * txtAmount.Text).ToString("N0")
                txtExDiscAmount.Text = CDec((txtExDisc.Text / 100) * txtAmount.Text).ToString("N0")
                txtNetPrice2.Text = CDec(txtQty.Text * txtPrice.Text - txtDiscAmount.Text - txtExDiscAmount.Text).ToString("N0")
            End If

        End If

    End Sub

    Private Sub DataGridView1_TabIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView1.TabIndexChanged
        Try
            EnbleFalseDtl()
            RowDg1 = DataGridView1.CurrentRow.Index
            seq = DataGridView1.Item("Column1", DataGridView1.CurrentRow.Index).Value
            txtPLU.Text = DataGridView1.Item("Column2", DataGridView1.CurrentRow.Index).Value
            txtDesc.Text = DataGridView1.Item("Column3", DataGridView1.CurrentRow.Index).Value
            txtPrice.Text = CDec(DataGridView1.Item("Column4", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtQty.Text = CDec(DataGridView1.Item("Column5", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtAmount.Text = CDec(DataGridView1.Item("Column6", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtDisc.Text = CDec(DataGridView1.Item("Column7", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtDiscAmount.Text = CDec(DataGridView1.Item("Column8", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtExDisc.Text = CDec(DataGridView1.Item("Column9", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtExDiscAmount.Text = CDec(DataGridView1.Item("Column10", DataGridView1.CurrentRow.Index).Value).ToString("N0")
            txtNetPrice2.Text = CDec(DataGridView1.Item("Column11", DataGridView1.CurrentRow.Index).Value).ToString("N0")
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DataGridView2_TabIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DataGridView2.TabIndexChanged
        Try
            RowDg2 = DataGridView2.CurrentRow.Index
            cmbPayType.SelectedValue = DataGridView2.Item("Column13", DataGridView2.CurrentRow.Index).Value
            txtCCNo.Text = DataGridView2.Item("Column15", DataGridView2.CurrentRow.Index).Value
            txtCCName.Text = DataGridView2.Item("Column16", DataGridView2.CurrentRow.Index).Value
            txtPaidAmount.Text = CDec(DataGridView2.Item("Column17", DataGridView2.CurrentRow.Index).Value).ToString("N0")
            txtShift.Text = DataGridView2.Item("Column18", DataGridView2.CurrentRow.Index).Value
        Catch ex As Exception

        End Try
    End Sub
End Class