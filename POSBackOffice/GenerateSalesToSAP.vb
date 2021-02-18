Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.ReportSource
Imports CrystalDecisions.Windows.Forms
Imports CrystalDecisions.Shared
Imports System.Drawing.Printing
Imports Excel = Microsoft.Office.Interop.Excel
Imports System.IO
Public Class GenerateSalesToSAP
    Dim ds As New DataSet
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        'Dim xlApp As Excel.Application = New Microsoft.Office.Interop.Excel.Application()
        'ds = getSqldb("SELECT '100000' AS Cust_Id, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        '              " AS Periode, Sales_Transactions.Branch_ID, Sales_Transaction_Details.Article_code AS Artikel, " & _
        '              " Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price AS Harga, SUM(Sales_Transaction_Details.Qty) " & _
        '              " AS Qty, SUM(Sales_Transaction_Details.Qty * Sales_Transaction_Details.Price) AS total, " & _
        '              " SUM(Sales_Transaction_Details.Discount_Amount) AS disc_amt, SUM(Sales_Transaction_Details.Discount_Percentage) " & _
        '              " AS disc_pct, SUM(Sales_Transaction_Details.ExtraDisc_Amt) AS exdisc_amt, SUM(Sales_Transaction_Details.Net_Price) " & _
        '              " AS net FROM Sales_Transactions INNER JOIN Sales_Transaction_Details ON Sales_Transactions.Transaction_Number = " & _
        '              " Sales_Transaction_Details.Transaction_Number WHERE  (Sales_Transactions.Status = '00') AND " & _
        '              " (Sales_Transaction_Details.Qty > 0) GROUP BY Sales_Transaction_Details.PLU, CONVERT(varchar(10), " & _
        '              " Sales_Transactions.Transaction_Date, 112),  Sales_Transaction_Details.Price,  Sales_Transactions.Branch_ID,  " & _
        '              " Sales_Transaction_Details.Article_code) HAVING (CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        '              " = '" & Format(DateTimePicker1.Value, "yyyyMMdd") & "') Union ALL SELECT '100000' AS Cust_Id, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        '              " AS Periode, Sales_Transactions.Branch_ID, Sales_Transaction_Details.Article_code AS Artikel, " & _
        '              " Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price AS Harga, SUM(Sales_Transaction_Details.Qty) AS Qty, " & _
        '              " SUM(Sales_Transaction_Details.Qty * Sales_Transaction_Details.Price) AS total, SUM(Sales_Transaction_Details.Discount_Amount) " & _
        '              " AS disc_amt, SUM(Sales_Transaction_Details.Discount_Percentage) AS disc_pct, SUM(Sales_Transaction_Details.ExtraDisc_Amt) " & _
        '              " AS exdisc_amt, SUM(Sales_Transaction_Details.Net_Price) AS net FROM Sales_Transactions INNER JOIN Sales_Transaction_Details " & _
        '              " ON Sales_Transactions.Transaction_Number = Sales_Transaction_Details.Transaction_Number WHERE  (Sales_Transactions.Status = '00') " & _
        '              " AND (Sales_Transaction_Details.Qty < 0) GROUP BY Sales_Transaction_Details.PLU, CONVERT(varchar(10), " & _
        '              " Sales_Transactions.Transaction_Date, 112),  Sales_Transaction_Details.Price,  Sales_Transactions.Branch_ID,  " & _
        '              " Sales_Transaction_Details.Article_code HAVING (CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) = '" & Format(DateTimePicker1.Value, "yyyyMMdd") & "') " & _
        '              " ORDER BY Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price")
        'If ds.Tables(0).Rows.Count = 0 Then
        '    Exit Sub
        'End If

        'If xlApp Is Nothing Then
        '    MessageBox.Show("Excel is not properly installed!!")
        '    Return
        'End If

        'Dim x As Integer = 1
        'Dim str As String = ""
        'Dim clm As Integer = ds.Tables(0).Columns.Count
        'Dim xlWorkBook As Excel.Workbook
        'Dim xlWorkSheet As Excel.Worksheet
        'Dim misValue As Object = System.Reflection.Missing.Value

        'xlWorkBook = xlApp.Workbooks.Add(misValue)
        'xlWorkSheet = xlWorkBook.Sheets("Sheet1")
        'For Each ro As DataRow In ds.Tables(0).Rows
        '    For d As Integer = 0 To clm - 1
        '        If IsNumeric(ro(d)) Then
        '            str += CDec(ro(d)).ToString("N0")
        '        Else
        '            str += ro(d).ToString.Trim
        '        End If
        '    Next
        '    str = Replace(str, ",", "")
        '    xlWorkSheet.Cells(x, 1).Value = str
        '    str = ""
        '    x += 1
        'Next


        'xlWorkBook.SaveAs(m_PathSAP & "SCSSsales" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv", Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, _
        ' Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue)
        'xlWorkBook.Close(True, misValue, misValue)
        'xlApp.Quit()

        'releaseObject(xlWorkSheet)
        'releaseObject(xlWorkBook)
        'releaseObject(xlApp)

        'MessageBox.Show("Excel file created , you can find the file " & m_PathSAP & "SCSSsales" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv")
        'If Directory.Exists(m_PathSAP & "GenerateSAP\") Then
        'Else
        '    Directory.CreateDirectory(m_PathSAP & "\GenerateSAP\")
        'End If
        Try
            SaveSales()
            SaveMOP()
            MsgBox("Success , Please Check " & m_PathSales & " !!")
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'SaveNonMD()
        'Dim Proc As String = "Explorer.exe"
        ''Dim Args As String = ControlChars.Quote & IO.Path.Combine(m_PathSAP, "GenerateSAP") & ControlChars.Quote
        'Dim Args As String = ControlChars.Quote & IO.Path.Combine(m_PathSAP, "") & ControlChars.Quote
        'Process.Start(Proc, Args)
    End Sub

    Sub SaveNonMD()
        Dim txtFileNew As String = Now.Year & Now.Month
        Dim line As String = ""
        Dim PatchSAP As String = m_PathSales & "\sales" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & "2.csv"
        If File.Exists(m_PathSales & "\sales" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & "2.csv") Then
            File.Delete(m_PathSales & "\sales" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & "2.csv")
        End If
        Dim sw As New IO.StreamWriter(PatchSAP)
        sw.Write(line)
        ds.Clear()
        'Query Oke
        ds = getSqldb2("SELECT '100000' AS Cust_Id, CONVERT(varchar(10), T_TransGwp_Header.Date, 112) " & _
        "AS Periode, Substring(T_TransGwp_Header.transaction_number,1,4) as Branch_ID,   " & _
        "im.Article_Code AS Artikel, " & _
        "T_TransGwp_Details.PLU, 0 AS Harga,  " & _
        "SUM(T_TransGwp_Details.Qty) AS Qty,  " & _
        "0 AS total, " & _
        "0 AS disc_amt, 0 " & _
        "AS disc_pct, 0 AS exdisc_amt,  " & _
        "0 as ExtraDisc_Pct, 0 " & _
        "AS net  " & _
        "        FROM T_TransGwp_Header " & _
        "INNER JOIN T_TransGwp_Details ON T_TransGwp_Header.Transaction_Number=T_TransGwp_Details.Transaction_Number  " & _
        "INNER JOIN item_master_gwp im on T_TransGwp_Details.PLU=im.PLU " & _
        "WHERE (T_TransGwp_Header.Status = '1') AND (T_TransGwp_Details.Qty > 0) and  " & _
        "(CONVERT(varchar(10), T_TransGwp_Header.Date, 112) " & _
                       " = '" & Format(DateTimePicker1.Value, "yyyyMMdd") & "') " & _
        "GROUP BY T_TransGwp_Details.PLU, CONVERT(varchar(10), T_TransGwp_Header.Date, 112),   " & _
        "        Substring(T_TransGwp_Header.transaction_number,1,4), " & _
        "        im.Article_Code")


        
        If ds.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In ds.Tables(0).Rows
                sw.Write(ro(0).ToString.Trim & vbTab & ro(1).ToString.Trim & vbTab & ro(2).ToString.Trim & vbTab & ro(3).ToString.Trim & vbTab & ro(4).ToString.Trim & vbTab & CInt(ro(5)) & vbTab & CInt(ro(6)) _
                         & vbTab & CInt(ro(7)) & vbTab & CInt(ro(8)) & vbTab & CInt(ro(9)) & vbTab & CInt(ro(10)) & vbTab & CInt(ro(11)) & vbNewLine)
            Next
        End If

        sw.Close()
        sw.Dispose()
    End Sub

    Private Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

    Private Sub GenerateSalesToSAP_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Sub SaveSales()
        Dim txtFileNew As String = Now.Year & Now.Month
        Dim line As String = ""
        Dim PatchSAP As String = m_PathSales & "\sales" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv"
        If File.Exists(m_PathSales & "\sales" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv") Then
            File.Delete(m_PathSales & "\sales" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv")
        End If
        Dim sw As New IO.StreamWriter(PatchSAP)
        sw.Write(line)
        ds.Clear()
        'Query Oke
        ds = getSqldb("SELECT '100000' AS Cust_Id, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        "AS Periode, Sales_Transactions.Branch_ID as Branch_ID,   " & _
        "im.Article_Code AS Artikel, " & _
        "Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price AS Harga,  " & _
        "SUM(Sales_Transaction_Details.Qty) AS Qty,  " & _
        "SUM(Sales_Transaction_Details.Qty * Sales_Transaction_Details.Price) AS total, " & _
        "SUM(Sales_Transaction_Details.Discount_Amount) AS disc_amt, SUM(Sales_Transaction_Details.Discount_Percentage) " & _
        "AS disc_pct, SUM(Sales_Transaction_Details.ExtraDisc_Amt) AS exdisc_amt,  " & _
        "sum(ExtraDisc_Pct) as ExtraDisc_Pct, SUM(Sales_Transaction_Details.Net_Price) " & _
        "AS net  " & _
        "        FROM Sales_Transactions " & _
        "INNER JOIN Sales_Transaction_Details ON Sales_Transactions.Transaction_Number=Sales_Transaction_Details.Transaction_Number  " & _
        "LEFT JOIN Item_Master im on Sales_Transaction_Details.PLU=im.PLU " & _
        "WHERE (Sales_Transactions.Status = '00') AND (Sales_Transaction_Details.Qty > 0) and  " & _
        "(CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
                       " = '" & Format(DateTimePicker1.Value, "yyyyMMdd") & "') " & _
        "GROUP BY Sales_Transaction_Details.PLU, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112),   " & _
        "        Sales_Transaction_Details.Price, Sales_Transactions.Branch_ID, " & _
        "        im.Article_Code" & _
        "        Union ALL " & _
        "SELECT '100000' AS Cust_Id, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        "AS Periode, Sales_Transactions.Branch_ID as Branch_ID,   " & _
        "im.Article_Code AS Artikel, " & _
        "Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price AS Harga, SUM(Sales_Transaction_Details.Qty) AS Qty, " & _
        "SUM(Sales_Transaction_Details.Qty * Sales_Transaction_Details.Price)*(-1) AS total, SUM(Sales_Transaction_Details.Discount_Amount) " & _
        "AS disc_amt, SUM(Sales_Transaction_Details.Discount_Percentage) AS disc_pct, SUM(Sales_Transaction_Details.ExtraDisc_Amt) " & _
        "AS exdisc_amt, sum(ExtraDisc_Pct) as ExtraDisc_Pct, SUM(Sales_Transaction_Details.Net_Price)*(-1) AS net  " & _
        "FROM Sales_Transactions  " & _
        "INNER JOIN Sales_Transaction_Details ON Sales_Transactions.Transaction_Number = Sales_Transaction_Details.Transaction_Number  " & _
        "LEFT JOIN item_master im on Sales_Transaction_Details.PLU=im.PLU " & _
        "WHERE (Sales_Transactions.Status = '00') AND (Sales_Transaction_Details.Qty < 0) and  " & _
        "(CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
                       " = '" & Format(DateTimePicker1.Value, "yyyyMMdd") & "') " & _
        "GROUP BY Sales_Transaction_Details.PLU, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112),   " & _
        "        Sales_Transaction_Details.Price, Sales_Transactions.Branch_ID, " & _
        "				 im.Article_Code ")


        'ds = getSqldb("SELECT '100000' AS Cust_Id, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        '               " AS Periode, Sales_Transactions.Branch_ID, Sales_Transaction_Details.Article_code AS Artikel, " & _
        '               " Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price AS Harga, SUM(Sales_Transaction_Details.Qty) " & _
        '               " AS Qty, SUM(Sales_Transaction_Details.Qty * Sales_Transaction_Details.Price) AS total, " & _
        '               " SUM(Sales_Transaction_Details.Discount_Amount) AS disc_amt, SUM(Sales_Transaction_Details.Discount_Percentage) " & _
        '               " AS disc_pct, SUM(Sales_Transaction_Details.ExtraDisc_Amt) AS exdisc_amt, SUM(Sales_Transaction_Details.Net_Price) " & _
        '               " AS net FROM Sales_Transactions INNER JOIN Sales_Transaction_Details ON Sales_Transactions.Transaction_Number = " & _
        '               " Sales_Transaction_Details.Transaction_Number WHERE  (Sales_Transactions.Status = '00') AND " & _
        '               " (Sales_Transaction_Details.Qty > 0) GROUP BY Sales_Transaction_Details.PLU, CONVERT(varchar(10), " & _
        '               " Sales_Transactions.Transaction_Date, 112),  Sales_Transaction_Details.Price,  Sales_Transactions.Branch_ID,  " & _
        '               " Sales_Transaction_Details.Article_code HAVING (CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        '               " = '" & Format(DateTimePicker1.Value, "yyyyMMdd") & "') Union ALL SELECT '100000' AS Cust_Id, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        '               " AS Periode, Sales_Transactions.Branch_ID, Sales_Transaction_Details.Article_code AS Artikel, " & _
        '               " Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price AS Harga, SUM(Sales_Transaction_Details.Qty) AS Qty, " & _
        '               " SUM(Sales_Transaction_Details.Qty * Sales_Transaction_Details.Price) AS total, SUM(Sales_Transaction_Details.Discount_Amount) " & _
        '               " AS disc_amt, SUM(Sales_Transaction_Details.Discount_Percentage) AS disc_pct, SUM(Sales_Transaction_Details.ExtraDisc_Amt) " & _
        '               " AS exdisc_amt, SUM(Sales_Transaction_Details.Net_Price) AS net FROM Sales_Transactions INNER JOIN Sales_Transaction_Details " & _
        '               " ON Sales_Transactions.Transaction_Number = Sales_Transaction_Details.Transaction_Number WHERE  (Sales_Transactions.Status = '00') " & _
        '               " AND (Sales_Transaction_Details.Qty < 0) GROUP BY Sales_Transaction_Details.PLU, CONVERT(varchar(10), " & _
        '               " Sales_Transactions.Transaction_Date, 112),  Sales_Transaction_Details.Price,  Sales_Transactions.Branch_ID,  " & _
        '               " Sales_Transaction_Details.Article_code HAVING (CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) = '" & Format(DateTimePicker1.Value, "yyyyMMdd") & "') " & _
        '               " ORDER BY Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price")
        If ds.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In ds.Tables(0).Rows
                sw.Write(ro(0).ToString.Trim & vbTab & ro(1).ToString.Trim & vbTab & ro(2).ToString.Trim & vbTab & ro(3).ToString.Trim & vbTab & ro(4).ToString.Trim & vbTab & CInt(ro(5)) & vbTab & CInt(ro(6)) _
                         & vbTab & CInt(ro(7)) & vbTab & CInt(ro(8)) & vbTab & CInt(ro(9)) & vbTab & CInt(ro(10)) & vbTab & CInt(ro(11)) & vbTab & CInt(ro(12)) & vbNewLine)
            Next
        End If

        sw.Close()
        sw.Dispose()
    End Sub

    Sub SaveSalesBali()
        Dim txtFileNew As String = Now.Year & Now.Month
        Dim line As String = ""
        Dim PatchSAP As String = m_PathSales & "\sales" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv"
        If File.Exists(m_PathSales & "\sales" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv") Then
            File.Delete(m_PathSales & "\sales" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv")
        End If
        Dim sw As New IO.StreamWriter(PatchSAP)
        sw.Write(line)
        ds.Clear()
        'Query Oke
        ds = getSqldb("SELECT '100000' AS Cust_Id, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        "AS Periode, Sales_Transactions.Branch_ID as Branch_ID,   " & _
        "im.Article_Code AS Artikel, " & _
        "Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price AS Harga,  " & _
        "SUM(Sales_Transaction_Details.Qty) AS Qty,  " & _
        "SUM(Sales_Transaction_Details.Qty * Sales_Transaction_Details.Price) AS total, " & _
        "SUM(Sales_Transaction_Details.Discount_Amount) AS disc_amt, SUM(Sales_Transaction_Details.Discount_Percentage) " & _
        "AS disc_pct, SUM(Sales_Transaction_Details.ExtraDisc_Amt) AS exdisc_amt,  " & _
        "sum(ExtraDisc_Pct) as ExtraDisc_Pct, SUM(Sales_Transaction_Details.Net_Price) " & _
        "AS net  " & _
        "        FROM Sales_Transactions " & _
        "INNER JOIN Sales_Transaction_Details ON Sales_Transactions.Transaction_Number=Sales_Transaction_Details.Transaction_Number  " & _
        "LEFT JOIN Item_Master im on Sales_Transaction_Details.PLU=im.PLU " & _
        "WHERE (Sales_Transactions.Status = '00') AND (Sales_Transaction_Details.Qty > 0) and  " & _
        "SUBSTRING(Sales_Transactions.Transaction_number,11,6) " & _
                       " = '" & Format(DateTimePicker1.Value, "MMyyyy") & "' " & _
        "GROUP BY Sales_Transaction_Details.PLU, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112),   " & _
        "        Sales_Transaction_Details.Price, Sales_Transactions.Branch_ID, " & _
        "        im.Article_Code" & _
        "        Union ALL " & _
        "SELECT '100000' AS Cust_Id, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        "AS Periode, Sales_Transactions.Branch_ID as Branch_ID,   " & _
        "im.Article_Code AS Artikel, " & _
        "Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price AS Harga, SUM(Sales_Transaction_Details.Qty) AS Qty, " & _
        "SUM(Sales_Transaction_Details.Qty * Sales_Transaction_Details.Price)*(-1) AS total, SUM(Sales_Transaction_Details.Discount_Amount) " & _
        "AS disc_amt, SUM(Sales_Transaction_Details.Discount_Percentage) AS disc_pct, SUM(Sales_Transaction_Details.ExtraDisc_Amt) " & _
        "AS exdisc_amt, sum(ExtraDisc_Pct) as ExtraDisc_Pct, SUM(Sales_Transaction_Details.Net_Price)*(-1) AS net  " & _
        "FROM Sales_Transactions  " & _
        "INNER JOIN Sales_Transaction_Details ON Sales_Transactions.Transaction_Number = Sales_Transaction_Details.Transaction_Number  " & _
        "LEFT JOIN item_master im on Sales_Transaction_Details.PLU=im.PLU " & _
        "WHERE (Sales_Transactions.Status = '00') AND (Sales_Transaction_Details.Qty < 0) and  " & _
        "SUBSTRING(Sales_Transactions.Transaction_number,11,6) " & _
                       " = '" & Format(DateTimePicker1.Value, "MMyyyy") & "' " & _
        "GROUP BY Sales_Transaction_Details.PLU, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112),   " & _
        "        Sales_Transaction_Details.Price, Sales_Transactions.Branch_ID, " & _
        "				 im.Article_Code ")


        'ds = getSqldb("SELECT '100000' AS Cust_Id, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        '               " AS Periode, Sales_Transactions.Branch_ID, Sales_Transaction_Details.Article_code AS Artikel, " & _
        '               " Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price AS Harga, SUM(Sales_Transaction_Details.Qty) " & _
        '               " AS Qty, SUM(Sales_Transaction_Details.Qty * Sales_Transaction_Details.Price) AS total, " & _
        '               " SUM(Sales_Transaction_Details.Discount_Amount) AS disc_amt, SUM(Sales_Transaction_Details.Discount_Percentage) " & _
        '               " AS disc_pct, SUM(Sales_Transaction_Details.ExtraDisc_Amt) AS exdisc_amt, SUM(Sales_Transaction_Details.Net_Price) " & _
        '               " AS net FROM Sales_Transactions INNER JOIN Sales_Transaction_Details ON Sales_Transactions.Transaction_Number = " & _
        '               " Sales_Transaction_Details.Transaction_Number WHERE  (Sales_Transactions.Status = '00') AND " & _
        '               " (Sales_Transaction_Details.Qty > 0) GROUP BY Sales_Transaction_Details.PLU, CONVERT(varchar(10), " & _
        '               " Sales_Transactions.Transaction_Date, 112),  Sales_Transaction_Details.Price,  Sales_Transactions.Branch_ID,  " & _
        '               " Sales_Transaction_Details.Article_code HAVING (CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        '               " = '" & Format(DateTimePicker1.Value, "yyyyMMdd") & "') Union ALL SELECT '100000' AS Cust_Id, CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) " & _
        '               " AS Periode, Sales_Transactions.Branch_ID, Sales_Transaction_Details.Article_code AS Artikel, " & _
        '               " Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price AS Harga, SUM(Sales_Transaction_Details.Qty) AS Qty, " & _
        '               " SUM(Sales_Transaction_Details.Qty * Sales_Transaction_Details.Price) AS total, SUM(Sales_Transaction_Details.Discount_Amount) " & _
        '               " AS disc_amt, SUM(Sales_Transaction_Details.Discount_Percentage) AS disc_pct, SUM(Sales_Transaction_Details.ExtraDisc_Amt) " & _
        '               " AS exdisc_amt, SUM(Sales_Transaction_Details.Net_Price) AS net FROM Sales_Transactions INNER JOIN Sales_Transaction_Details " & _
        '               " ON Sales_Transactions.Transaction_Number = Sales_Transaction_Details.Transaction_Number WHERE  (Sales_Transactions.Status = '00') " & _
        '               " AND (Sales_Transaction_Details.Qty < 0) GROUP BY Sales_Transaction_Details.PLU, CONVERT(varchar(10), " & _
        '               " Sales_Transactions.Transaction_Date, 112),  Sales_Transaction_Details.Price,  Sales_Transactions.Branch_ID,  " & _
        '               " Sales_Transaction_Details.Article_code HAVING (CONVERT(varchar(10), Sales_Transactions.Transaction_Date, 112) = '" & Format(DateTimePicker1.Value, "yyyyMMdd") & "') " & _
        '               " ORDER BY Sales_Transaction_Details.PLU, Sales_Transaction_Details.Price")
        If ds.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In ds.Tables(0).Rows
                sw.Write(ro(0).ToString.Trim & vbTab & ro(1).ToString.Trim & vbTab & ro(2).ToString.Trim & vbTab & ro(3).ToString.Trim & vbTab & ro(4).ToString.Trim & vbTab & CInt(ro(5)) & vbTab & CInt(ro(6)) _
                         & vbTab & CInt(ro(7)) & vbTab & CInt(ro(8)) & vbTab & CInt(ro(9)) & vbTab & CInt(ro(10)) & vbTab & CInt(ro(11)) & vbTab & CInt(ro(12)) & vbNewLine)
            Next
        End If

        sw.Close()
        sw.Dispose()
    End Sub

    Sub SaveMOP()
        Dim txtFileNew As String = Now.Year & Now.Month
        Dim line As String = ""
        Dim PatchSAP As String = m_PathMOP & "\sales_mop" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv"
        If File.Exists(m_PathMOP & "\sales_mop" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv") Then
            File.Delete(m_PathMOP & "\sales_mop" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv")
        End If
        Dim sw As New IO.StreamWriter(PatchSAP)
        sw.Write(line)
        ds.Clear()
        ds = getSqldb("select '" & Format(DateTimePicker1.Value, "yyyyMMdd") & "' As Tgl,substring(a.Transaction_number,1,4) As Store,c.sboPT, SUM(a.Paid_Amount)  As Amount from paid a inner join payment_types b on a.Payment_Types = b.Payment_Types inner join Payment_Types_CP c on  b.Payment_Types = c.POSPT  where substring(a.Transaction_number,9,8) = '" & Format(DateTimePicker1.Value, "ddMMyyyy") & "' group by substring(a.Transaction_number,9,8),substring(a.Transaction_number,1,4),c.sboPT order by substring(a.Transaction_number,9,8) ")
        If ds.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In ds.Tables(0).Rows
                sw.Write(ro(0).ToString.Trim & vbTab & ro(1).ToString.Trim & vbTab & ro(2).ToString.Trim & vbTab & CInt(ro(3)).ToString.Trim & vbNewLine)
            Next
        End If

        sw.Close()
        sw.Dispose()
    End Sub

    Sub SaveMOPBali()
        Dim txtFileNew As String = Now.Year & Now.Month
        Dim line As String = ""
        Dim PatchSAP As String = m_PathMOP & "\sales_mop" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv"
        If File.Exists(m_PathMOP & "\sales_mop" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv") Then
            File.Delete(m_PathMOP & "\sales_mop" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & Format(DateTimePicker1.Value, "yyyyMMdd") & ".csv")
        End If
        Dim sw As New IO.StreamWriter(PatchSAP)
        sw.Write(line)
        ds.Clear()
        ds = getSqldb("select '" & Format(DateTimePicker1.Value, "yyyyMMdd") & "' As Tgl,substring(a.Transaction_number,1,4) As Store,c.sboPT, SUM(a.Paid_Amount)  As Amount from paid a inner join payment_types b on a.Payment_Types = b.Payment_Types inner join Payment_Types_CP c on  b.Payment_Types = c.POSPT  where substring(a.Transaction_number,11,6) = '" & Format(DateTimePicker1.Value, "MMyyyy") & "' group by substring(a.Transaction_number,11,6),substring(a.Transaction_number,1,4),c.sboPT order by substring(a.Transaction_number,11,6) ")
        If ds.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In ds.Tables(0).Rows
                sw.Write(ro(0).ToString.Trim & vbTab & ro(1).ToString.Trim & vbTab & ro(2).ToString.Trim & vbTab & CInt(ro(3)).ToString.Trim & vbNewLine)
            Next
        End If

        sw.Close()
        sw.Dispose()
    End Sub

End Class