Public Class Filtering_Discrepancy_Sales_Data
    Dim ds As New DataSet
    Dim dspaid, dsheader, dsdetail As New DataSet
    Dim strDate As String
    Private Sub Filtering_Discrepancy_Sales_Data_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        DateTimePicker1.Value = DateTimePicker1.Value.AddDays(-1)
        CheckForIllegalCrossThreadCalls = False
        Label1.Text = ""
        lv()
        RadioButton1.Checked = True
    End Sub

    Sub lv()
        ListView1.Columns.Add("Register", 60, HorizontalAlignment.Left)
        ListView1.Columns.Add("Header ", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Detail", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("Paid", 100, HorizontalAlignment.Left)
        ListView1.Columns.Add("", 60, HorizontalAlignment.Left)

        ListView2.Columns.Add("Trans No", 150, HorizontalAlignment.Left)
        ListView2.Columns.Add("Header ", 80, HorizontalAlignment.Left)
        ListView2.Columns.Add("Detail", 80, HorizontalAlignment.Left)
        ListView2.Columns.Add("Paid", 80, HorizontalAlignment.Left)
        ListView2.Columns.Add("", 50, HorizontalAlignment.Left)
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If ListView2.Visible = True Then
            ListView2.Visible = False
        End If
        ListView1.Items.Clear()

        strDate = Format(DateTimePicker1.Value, "ddMMyyyy")
        Button1.Enabled = False
        Button2.Enabled = False
        Button3.Enabled = False
        DateTimePicker1.Enabled = False
        ProgressBar1.Visible = True
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()
        
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If ListView2.Visible = True Then
            ListView2.Visible = False
        Else
            Me.Close()
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim Prg As Decimal
        Prg = 0
        ProgressBar1.Value = 0
        If RadioButton1.Checked = True Then

            getSqldb("Delete From FilterSales")
            Prg += 10
            BackgroundWorker1.ReportProgress(Int(Prg))
            Try
                setLabelTxt("Sales ...", Label1)
            Catch ex As Exception

            End Try
            getSqldb("Insert FilterSales (Reg,Header,Detail,Paid) Select cash_register_id,0,0,0 from cash_register order by cash_register_id")
            Prg += 10
            BackgroundWorker1.ReportProgress(Int(Prg))
            Try
                setLabelTxt("aid ...", Label1)
            Catch ex As Exception

            End Try
            dspaid = getSqldb("select substring(transaction_number,5,3) as reg, sum(net_amount) as header from sales_transactions where status = '00' and substring(transaction_number,9,8) = '" & strDate & "' group by substring(transaction_number,5,3) order by substring(transaction_number,5,3)")
            If dspaid.Tables(0).Rows.Count > 0 Then
                getSqldb("update a set a.Paid = b.paid from FilterSales a inner join (select substring(a.transaction_number,5,3) as reg, sum(b.paid_amount) as paid from sales_transactions a inner join paid b on a.transaction_number = b.transaction_number where status = '00' and substring(a.transaction_number,9,8) = '" & strDate & "' group by substring(a.transaction_number,5,3)) b on a.Reg = b.reg where a.reg = b.reg ")
            End If
            Prg += 10
            BackgroundWorker1.ReportProgress(Int(Prg))
            Try
                setLabelTxt("Header ...", Label1)
            Catch ex As Exception

            End Try
            dsheader = getSqldb("select substring(a.transaction_number,5,3) as reg, sum(b.paid_amount) as paid from sales_transactions a inner join paid b on a.transaction_number = b.transaction_number where status = '00' and substring(a.transaction_number,9,8) = '" & strDate & "' group by substring(a.transaction_number,5,3) order by substring(a.transaction_number,5,3)")
            If dsheader.Tables(0).Rows.Count > 0 Then
                getSqldb("update a set a.Header = b.header from FilterSales a inner join (select substring(transaction_number,5,3) as reg, sum(net_amount) as header from sales_transactions where status = '00' and substring(transaction_number,9,8) = '" & strDate & "' group by substring(transaction_number,5,3) ) b on a.Reg = b.reg where a.reg = b.reg ")
            End If
            Prg += 10
            BackgroundWorker1.ReportProgress(Int(Prg))
            Try
                setLabelTxt("Detail ...", Label1)
            Catch ex As Exception

            End Try
            dsheader = getSqldb("select substring(a.transaction_number,5,3) as reg, (sum(b.amount)-sum(isnull(b.discount_amount,0))-sum(isnull(b.extradisc_amt,0))) as detail from sales_transactions a inner join sales_transaction_details b on a.transaction_number = b.transaction_number where status = '00' and substring(a.transaction_number,9,8) = '" & strDate & "' group by substring(a.transaction_number,5,3) order by substring(a.transaction_number,5,3)")
            If dsheader.Tables(0).Rows.Count > 0 Then
                getSqldb("update a set a.Detail = b.detail from FilterSales a inner join (select substring(a.transaction_number,5,3) as reg, (sum(b.amount)-sum(isnull(b.discount_amount,0))-sum(isnull(b.extradisc_amt,0))) as detail from sales_transactions a inner join sales_transaction_details b on a.transaction_number = b.transaction_number where status = '00' and substring(a.transaction_number,9,8) = '" & strDate & "' group by substring(a.transaction_number,5,3)) b on a.Reg = b.reg where a.reg = b.reg ")
            End If
            Prg += 10
            BackgroundWorker1.ReportProgress(Int(Prg))
            Try
                setLabelTxt("Sales...", Label1)
            Catch ex As Exception

            End Try
            ds = getSqldb("Select *,case when sum(Header) <> Sum(Detail) Then 'Over' When sum(Header) <> sum(Paid) Then 'Over' When sum(Detail) <> sum(Paid) Then 'Over' Else '' end as stat from FilterSales Group By reg,header,detail,paid")
            'ds = getSqldb("select Cash_Register_ID,sum(Header) as Header,Sum(Detail) as Detail,Sum(Paid) as Paid," & _
            '          "case when sum(Header) <> Sum(Detail) Then 'Over' When sum(Header) <> sum(Paid) Then 'Over' When sum(Detail) <> sum(Paid) Then 'Over' Else '' end as stat from " & _
            '          "(select a.Cash_Register_ID,sum(a.Net_Amount) as Header,(select sum(b.Net_Price) as Detail from " & _
            '          "Sales_Transaction_Details b inner join Sales_Transactions c on b.Transaction_Number = " & _
            '          "c.Transaction_Number where substring(b.Transaction_Number,5,12) = substring(a.Transaction_Number,5,12) " & _
            '          "and c.Status <> '02' group by substring(b.Transaction_Number,5,12)) as Detail,(select sum(d.Paid_Amount) " & _
            '          "from paid d where substring(d.Transaction_Number,5,12) = substring(a.Transaction_Number,5,12) " & _
            '          "group by substring(d.Transaction_Number,5,12)) as paid from Sales_Transactions a  " & _
            '          "where substring(a.Transaction_Number,9,8) = '" & strDate & "' and a.Status <> '02'  " & _
            '          "group by a.Cash_Register_ID,a.Status,substring(a.Transaction_Number,5,12) " & _
            '          "union Select Cash_Register_ID,0,0,0 from Cash_Register) a group by Cash_Register_ID order by Cash_Register_ID")
        Else
            getSqldb("Delete From FilterSales")
            Prg += 10
            BackgroundWorker1.ReportProgress(Int(Prg))
            Try
                setLabelTxt("Sales ...", Label1)
            Catch ex As Exception

            End Try
            getSqldb("Insert FilterSales (Reg,Header,Detail,Paid) Select cash_register_id,0,0,0 from cash_register order by cash_register_id")
            Prg += 10
            BackgroundWorker1.ReportProgress(Int(Prg))
            Try
                setLabelTxt("Paid ...", Label1)
            Catch ex As Exception

            End Try
            dspaid = getSqldb2("select substring(transaction_number,5,3) as reg, sum(net_amount) as header from sales_transactions where status = '00' and substring(transaction_number,9,8) = '" & strDate & "' group by substring(transaction_number,5,3) order by substring(transaction_number,5,3)")
            If dspaid.Tables(0).Rows.Count > 0 Then
                getSqldb2("update a set a.Paid = b.paid from [POS_SERVER_HISTORY].dbo.FilterSales a inner join (select substring(a.transaction_number,5,3) as reg, sum(b.paid_amount) as paid from sales_transactions a inner join paid b on a.transaction_number = b.transaction_number where status = '00' and substring(a.transaction_number,9,8) = '" & strDate & "' group by substring(a.transaction_number,5,3)) b on a.Reg = b.reg where a.reg = b.reg ")
            End If
            Prg += 10
            BackgroundWorker1.ReportProgress(Int(Prg))
            Try
                setLabelTxt("Header ...", Label1)
            Catch ex As Exception

            End Try
            dsheader = getSqldb2("select substring(a.transaction_number,5,3) as reg, sum(b.paid_amount) as paid from sales_transactions a inner join paid b on a.transaction_number = b.transaction_number where status = '00' and substring(a.transaction_number,9,8) = '" & strDate & "' group by substring(a.transaction_number,5,3) order by substring(a.transaction_number,5,3)")
            If dsheader.Tables(0).Rows.Count > 0 Then
                getSqldb2("update a set a.Header = b.header from [POS_SERVER_HISTORY].dbo.FilterSales a inner join (select substring(transaction_number,5,3) as reg, sum(net_amount) as header from sales_transactions where status = '00' and substring(transaction_number,9,8) = '" & strDate & "' group by substring(transaction_number,5,3) ) b on a.Reg = b.reg where a.reg = b.reg ")
            End If
            Prg += 10
            BackgroundWorker1.ReportProgress(Int(Prg))
            Try
                setLabelTxt("Detail ...", Label1)
            Catch ex As Exception

            End Try
            dsheader = getSqldb2("select substring(a.transaction_number,5,3) as reg, (sum(b.amount)-sum(isnull(b.discount_amount,0))-sum(isnull(b.extradisc_amt,0))) as detail from sales_transactions a inner join sales_transaction_details b on a.transaction_number = b.transaction_number where status = '00' and substring(a.transaction_number,9,8) = '" & strDate & "' group by substring(a.transaction_number,5,3) order by substring(a.transaction_number,5,3)")
            If dsheader.Tables(0).Rows.Count > 0 Then
                getSqldb2("update a set a.Detail = b.detail from [POS_SERVER_HISTORY].dbo.FilterSales a inner join (select substring(a.transaction_number,5,3) as reg, (sum(b.amount)-sum(isnull(b.discount_amount,0))-sum(isnull(b.extradisc_amt,0))) as detail from sales_transactions a inner join sales_transaction_details b on a.transaction_number = b.transaction_number where status = '00' and substring(a.transaction_number,9,8) = '" & strDate & "' group by substring(a.transaction_number,5,3)) b on a.Reg = b.reg where a.reg = b.reg ")
            End If
            Prg += 10
            BackgroundWorker1.ReportProgress(Int(Prg))
            Try
                setLabelTxt("Sales...", Label1)
            Catch ex As Exception

            End Try
            ds = getSqldb("Select *,case when sum(Header) <> Sum(Detail) Then 'Over' When sum(Header) <> sum(Paid) Then 'Over' When sum(Detail) <> sum(Paid) Then 'Over' Else '' end as stat from FilterSales Group By reg,header,detail,paid")

            'ds = getSqldb2("select Cash_Register_ID,sum(Header) as Header,Sum(Detail) as Detail,Sum(Paid) as Paid," & _
            '          "case when sum(Header) <> Sum(Detail) Then 'Over' When sum(Header) <> sum(Paid) Then 'Over' When sum(Detail) <> sum(Paid) Then 'Over' Else '' end as stat from " & _
            '          "(select a.Cash_Register_ID,sum(a.Net_Amount) as Header,(select sum(b.Net_Price) as Detail from " & _
            '          "Sales_Transaction_Details b inner join Sales_Transactions c on b.Transaction_Number = " & _
            '          "c.Transaction_Number where substring(b.Transaction_Number,5,12) = substring(a.Transaction_Number,5,12) " & _
            '          "and c.Status <> '02' group by substring(b.Transaction_Number,5,12)) as Detail,(select sum(d.Paid_Amount) " & _
            '          "from paid d where substring(d.Transaction_Number,5,12) = substring(a.Transaction_Number,5,12) " & _
            '          "group by substring(d.Transaction_Number,5,12)) as paid from Sales_Transactions a  " & _
            '          "where substring(a.Transaction_Number,9,8) = '" & strDate & "' and a.Status <> '02'  " & _
            '          "group by a.Cash_Register_ID,a.Status,substring(a.Transaction_Number,5,12) " & _
            '          "union Select Cash_Register_ID,0,0,0 from Cash_Register) a group by Cash_Register_ID order by Cash_Register_ID")
        End If

        If ds.Tables(0).Rows.Count > 0 Then
            Try
                setLabelTxt("List Data ...", Label1)
            Catch ex As Exception

            End Try
            If ds.Tables(0).Rows.Count > 0 Then
                For Each ro As DataRow In ds.Tables(0).Rows
                    Prg += 50 / ds.Tables(0).Rows.Count
                    BackgroundWorker1.ReportProgress(Int(Prg))
                    Dim str(4) As String
                    Dim itm As ListViewItem
                    str(0) = ro(0)
                    str(1) = CDec(ro(1)).ToString("N0")
                    str(2) = CDec(ro(2)).ToString("N0")
                    str(3) = CDec(ro(3)).ToString("N0")
                    str(4) = ro(4)
                    itm = New ListViewItem(str)
                    ListView1.Items.Add(itm)
                Next
            End If
        End If
    End Sub

    Private Sub setLabelTxt(ByVal text As String, ByVal lbl As Label)
        If lbl.InvokeRequired Then
            lbl.Invoke(New setLabelTxtInvoker(AddressOf setLabelTxt), text, lbl)
        Else
            lbl.Text = text
        End If
    End Sub

    Private Delegate Sub setLabelTxtInvoker(ByVal text As String, ByVal lbl As Label)

    Private Sub BackgroundWorker1_ProgressChanged(ByVal sender As Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ProgressBar1.Value = e.ProgressPercentage
    End Sub

    Private Sub BackgroundWorker1_RunWorkerCompleted(ByVal sender As Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Button1.Enabled = True
        Button2.Enabled = True
        Button3.Enabled = True
        DateTimePicker1.Enabled = True
        ProgressBar1.Visible = False
        Label1.Text = ""
    End Sub

    Private Sub ListView1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick
        Dim I As Integer
        For I = 0 To ListView1.SelectedItems.Count - 1
            SetLv2(ListView1.SelectedItems(I).SubItems(0).Text)
            Exit For
        Next
    End Sub

    Sub SetLv2(ByVal Reg As String)
        Dim dslv1 As New DataSet
        ListView2.Items.Clear()
        dslv1 = getSqldb("Select b.transaction_number,(Select sum(a.net_amount) from sales_transactions a where a.transaction_number = b.transaction_number) As header," & _
                      "(Select sum(c.net_price) from sales_transaction_details c where c.transaction_number = b.transaction_number) As detail," & _
                      "(Select sum(d.paid_amount) from paid d where d.transaction_number = b.transaction_number) As paid, " & _
                      " case when (Select sum(a.net_amount) from sales_transactions a where a.transaction_number = b.transaction_number) <> (Select sum(c.net_price) from sales_transaction_details c where c.transaction_number = b.transaction_number) Then 'Over' " & _
                      " When (Select sum(a.net_amount) from sales_transactions a where a.transaction_number = b.transaction_number) <> (Select sum(d.paid_amount) from paid d where d.transaction_number = b.transaction_number) Then 'Over' " & _
                      " When (Select sum(c.net_price) from sales_transaction_details c where c.transaction_number = b.transaction_number) <> (Select sum(d.paid_amount) from paid d where d.transaction_number = b.transaction_number) Then 'Over' Else '' end as stat " & _
                      " from sales_transactions b where b.Cash_Register_ID = '" & Reg & "' and substring(b.transaction_number,9,8) = '" & strDate & "' and b.status = '00'")
        If dslv1.Tables(0).Rows.Count > 0 Then
            ListView2.Visible = True
            For Each ro As DataRow In dslv1.Tables(0).Rows
                Dim str(4) As String
                Dim itm As ListViewItem
                str(0) = ro(0)
                str(1) = CDec(ro(1)).ToString("N0")
                str(2) = CDec(ro(2)).ToString("N0")
                str(3) = CDec(ro(3)).ToString("N0")
                str(4) = ro(4)
                itm = New ListViewItem(str)
                ListView2.Items.Add(itm)
            Next
        End If
    End Sub
End Class