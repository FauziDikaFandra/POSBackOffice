Imports System.Data.SqlClient
Imports System.Xml
Imports System.Math
Imports System.Text
Module Module1
    Public m_con, m_con2, m_con3 As SqlConnection
    Public m_Sqlconn, m_ServerName, m_DBName, m_UserName, m_Password, m_Sqlconn2, m_Sqlconn3,
    m_ServerName2, m_DBName2, m_UserName2, m_Password2, m_ServerName3, m_DBName3,
    m_UserName3, m_Password3, m_Path, m_PathMOP, m_PathSales, usrID, PrintTyp, UserName, Pass, Trans_No_v, V_Code, Group_ID, FloorHistory, IPReader As String
    Public ConSer As Integer
    Public Tot_v As Decimal
    Public Value(100), Value2(100), OuPut(100) As String
    Public Param(100), Param2(100) As String
    Public TypeP(100) As SqlDbType
    Public DSBranch As New DataSet
    Public RFIDOKE As Boolean
    Public EPC(50) As String
    Public TID(50) As String
    Public UserData(50) As String
    Public TagType(50) As String
    Public ReaderName(50) As String
    Public OneReadAll As Boolean
    Public CntRFID As Integer
    Public m_TagTableRapid As Hashtable
    Public Function getSqldb(ByVal scmd As String) As DataSet
        m_con = New SqlConnection(m_Sqlconn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim cmd As New SqlCommand

        cmd = m_con.CreateCommand
        cmd.CommandText = scmd
        cmd.CommandTimeout = 120
        da.SelectCommand = cmd
        If m_con.State = ConnectionState.Open Then
            m_con.Close()
        End If
        m_con.Open()
1:
        Try
            da.Fill(ds)


        Catch ex As Exception
            'GoTo 1
            MsgBox(ex.Message)
        End Try

        m_con.Close()
        Return ds
    End Function

    Public Function cmb(ByVal ccmb As ComboBox, ByVal sql As String, ByVal UsrID As String, ByVal mName As String, ByVal cek As Integer)
        Dim c As New ArrayList
        m_con = New SqlConnection(m_Sqlconn)
        Try
            Dim strsql As String
            strsql = sql

            If m_con.State = ConnectionState.Closed Then m_con.Open()
            Dim cmd2 As New SqlCommand(strsql, m_con)

            Dim objreader2 As SqlDataReader = cmd2.ExecuteReader()
            If cek = 1 Then
            Else
                c.Add(New CCombo("", ""))
            End If
            Do While objreader2.Read()
                c.Add(New CCombo(Trim(objreader2(UsrID)), Trim(objreader2(mName).ToString)))
                'cmbPTKPID.Items.Add(objreader2("ID"))
            Loop
            m_con.Close()
            With ccmb
                .DataSource = c
                .DisplayMember = "Number_Name"
                .ValueMember = "ID"
            End With

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
        Return ccmb
    End Function

    Public Function cmb2(ByVal ccmb As ComboBox, ByVal sql As String, ByVal UsrID As String, ByVal mName As String, ByVal cek As Integer)
        Dim c As New ArrayList
        m_con = New SqlConnection(m_Sqlconn2)
        Try
            Dim strsql As String
            strsql = sql

            If m_con.State = ConnectionState.Closed Then m_con.Open()
            Dim cmd2 As New SqlCommand(strsql, m_con)

            Dim objreader2 As SqlDataReader = cmd2.ExecuteReader()
            If cek = 1 Then
            Else
                c.Add(New CCombo("", ""))
            End If
            Do While objreader2.Read()
                c.Add(New CCombo(Trim(objreader2(UsrID)), Trim(objreader2(mName).ToString)))
                'cmbPTKPID.Items.Add(objreader2("ID"))
            Loop
            m_con.Close()
            With ccmb
                .DataSource = c
                .DisplayMember = "Number_Name"
                .ValueMember = "ID"
            End With

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
        Return ccmb
    End Function
    Public Function cmb3(ByVal ccmb As ComboBox, ByVal sql As String, ByVal UsrID As String, ByVal mName As String, ByVal cek As Integer)
        Dim c As New ArrayList
        m_con3 = New SqlConnection(m_Sqlconn3)
        Try
            Dim strsql As String
            strsql = sql

            If m_con3.State = ConnectionState.Closed Then m_con3.Open()
            Dim cmd2 As New SqlCommand(strsql, m_con3)

            Dim objreader2 As SqlDataReader = cmd2.ExecuteReader()
            If cek = 1 Then
            Else
                c.Add(New CCombo("", ""))
            End If
            Do While objreader2.Read()
                c.Add(New CCombo(objreader2(UsrID), objreader2(mName).ToString))
                'cmbPTKPID.Items.Add(objreader2("ID"))
            Loop
            m_con3.Close()
            With ccmb
                .DataSource = c
                .DisplayMember = "Number_Name"
                .ValueMember = "ID"
            End With

        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try
        Return ccmb
    End Function

    Public Function SelProc(ByVal StoreName As String, ByVal Count As Integer) As DataSet
        m_con = New SqlConnection(m_Sqlconn)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim cmd As New SqlCommand
        cmd = m_con.CreateCommand
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = StoreName
        If Count <> 0 Then
            For x As Integer = 1 To Count
                'cmd.Parameters.Add(Param(x), CType(SetSqlType(x), SqlDbType)).Value = Value(x)
                cmd.Parameters.AddWithValue(Param(x), Value(x))
            Next
        End If


        da.SelectCommand = cmd
        If m_con.State = ConnectionState.Open Then
            m_con.Close()
        End If
        m_con.Open()
        cmd.CommandTimeout = 120
        da.Fill(ds)
        m_con.Close()
        Return ds
    End Function

    Public Function SelProcSer(ByVal StoreName As String, ByVal Count As Integer) As DataSet
        m_con = New SqlConnection(m_Sqlconn2)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim cmd As New SqlCommand
        cmd = m_con.CreateCommand
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = StoreName
        If Count <> 0 Then
            For x As Integer = 1 To Count
                'cmd.Parameters.Add(Param(x), CType(SetSqlType(x), SqlDbType)).Value = Value(x)
                cmd.Parameters.AddWithValue(Param(x), Value(x))
            Next
        End If


        da.SelectCommand = cmd
        If m_con.State = ConnectionState.Open Then
            m_con.Close()
        End If
        m_con.Open()
        cmd.CommandTimeout = 120
        da.Fill(ds)
        m_con.Close()
        Return ds
    End Function

    Public Function SelProcOut(ByVal StoreName As String, ByVal Count As Integer, ByVal Count2 As Integer, ByVal size As Integer) As DataSet
        m_con = New SqlConnection(m_Sqlconn2)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim cmd As New SqlCommand
        cmd = m_con.CreateCommand
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = StoreName
        If Count <> 0 Then
            For x As Integer = 1 To Count
                cmd.Parameters.AddWithValue(Param(x), Value(x))
            Next
        End If
        If Count2 <> 0 Then
            For y As Integer = 1 To Count2
                cmd.Parameters.Add(Param2(y), TypeP(y), size)
                cmd.Parameters(Param2(y)).Direction = ParameterDirection.Output
            Next
        End If

        da.SelectCommand = cmd
        If m_con.State = ConnectionState.Open Then
            m_con.Close()
        End If
        m_con.Open()
        cmd.CommandTimeout = 120
        da.Fill(ds)


        m_con.Close()
        If Count2 <> 0 Then
            For z As Integer = 1 To Count2
                Value2(z) = cmd.Parameters(Param2(z)).Value.ToString()
            Next
        End If

        Return ds
    End Function

    Public Function getSqldb2(ByVal scmd As String) As DataSet
        m_con2 = New SqlConnection(m_Sqlconn2)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim cmd As New SqlCommand

        cmd = m_con2.CreateCommand
        cmd.CommandText = scmd
        cmd.CommandTimeout = 120
        da.SelectCommand = cmd
        If m_con2.State = ConnectionState.Open Then
            m_con2.Close()
        End If
        m_con2.Open()
1:
        Try
            da.Fill(ds)
        Catch ex As Exception
            'GoTo 1
            MsgBox(ex.Message)
        End Try
        m_con2.Close()
        Return ds

    End Function

    Public Function getSqldb3(ByVal scmd As String) As DataSet
        m_con3 = New SqlConnection(m_Sqlconn3)
        Dim da As New SqlDataAdapter
        Dim ds As New DataSet
        Dim cmd As New SqlCommand

        cmd = m_con3.CreateCommand
        cmd.CommandText = scmd
        cmd.CommandTimeout = 120
        da.SelectCommand = cmd
        If m_con3.State = ConnectionState.Open Then
            m_con3.Close()
        End If

1:
        Try
            m_con3.Open()
            da.Fill(ds)
        Catch ex As Exception
            GoTo 1
            MsgBox(ex.Message)
        End Try
        m_con3.Close()
        Return ds

    End Function

    Public Sub ConnectServer()
        m_ServerName = ReadIni("ServerH", "ServerName")
        m_DBName = ReadIni("ServerH", "DatabaseName")
        m_UserName = ReadIni("ServerH", "LoginID")
        m_Password = decrypt(ReadIni("ServerH", "Password"))
        m_ServerName2 = ReadIni("Server", "ServerName")
        m_DBName2 = ReadIni("Server", "DatabaseName")
        m_UserName2 = ReadIni("Server", "LoginID")
        m_Password2 = decrypt(ReadIni("Server", "Password"))
        m_ServerName3 = ReadIni("UPLOADINFO", "ServerName")
        m_DBName3 = ReadIni("UPLOADINFO", "DatabaseName")
        m_UserName3 = ReadIni("UPLOADINFO", "LoginID")
        m_Password3 = decrypt(ReadIni("UPLOADINFO", "Password"))
        m_Path = ReadIni("Device", "Log_Path")
        PrintTyp = ReadIni("Device", "PrinterPort")
        m_PathMOP = ReadIni("RegisterInfo", "patchMOP")
        m_PathSales = ReadIni("RegisterInfo", "patchSales")
    End Sub

    Public Function StrC(ByVal c As String) As String
        Dim result$
        Dim i%
        result = ""
        i = 1
        Do While i < (Len(Trim(c)) + 1)
            result = result & Chr(Asc(Mid(Trim(c), i, 1)) / 2)
            i = i + 1
        Loop
        StrC = result
        Exit Function
    End Function

    Public Function ReadIni(ByVal xTipe As String, ByVal xName As String) As String
        Dim res As Integer
        Dim sb As StringBuilder
        sb = New StringBuilder(500)
        res = GetPrivateProfileString(xTipe, xName, "", sb, sb.Capacity, Application.StartupPath & "\Config.ini")
        ReadIni = sb.ToString()
    End Function

    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpAppName As String, _
            ByVal lpKeyName As String, _
            ByVal lpDefault As String, _
            ByVal lpReturnedString As StringBuilder, _
            ByVal nSize As Integer, _
            ByVal lpFileName As String) As Integer

    Public Function decrypt(ByVal unpass As String) As String
        Dim x As Integer
        Dim awal As String, kembali As String
        x = 1
        awal = ""
        Do While x <= Len(Trim(unpass))
            kembali = Mid(unpass, x, 3)
            x = x + 3
            awal = awal + Chr((Val(kembali) + 11) / 3 - 5)
        Loop
        decrypt = awal
    End Function

    Function StringToHexPrint(ByVal text As String) As String
        Dim hex As String = ""
        For i As Integer = 0 To text.Length - 1
            hex &= Asc(text.Substring(i, 1)).ToString("x").ToUpper
        Next
        Return hex
    End Function
    Public Sub releaseObject(ByVal obj As Object)
        Try
            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing
        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
        End Try
    End Sub

End Module
