Imports System.Data.SqlClient
Imports System.Xml
Imports System.Math
Imports System.Text
Public Class Form1
    Dim ds As New DataSet
    Dim m_con As SqlConnection
    Dim m_Sqlconn, m_ServerName, m_DBName, m_UserName, m_Password As String
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ConnectServer()
        m_Sqlconn = "Data Source=" & m_ServerName & ";" & "Initial Catalog=" & m_DBName & ";" & "User ID=" & m_UserName & ";" & "Password=" & m_Password & ";"
        ds = getSqldb("select * from newvoc where V_DEPO = '" & Format(Now.Date, "yyMMdd") & "'")
        If ds.Tables(0).Rows.Count > 0 Then
            For Each ro As DataRow In ds.Tables(0).Rows
                getSqldb("update newvoc set V_FLAG = 'E' where V_NO = '" & ro("V_NO") & "'")
                'getSqldb("insert into Voucher_expired select * from newvoc where V_NO = '" & ro("V_NO") & "'")
                'getSqldb("delete from newvoc where V_NO = '" & ro("V_NO") & "'")
            Next
        End If
    End Sub

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

    Public Sub ConnectServer()
        m_ServerName = ReadIni("Local", "ServerName")
        m_DBName = ReadIni("Local", "DatabaseName")
        m_UserName = ReadIni("Local", "LoginID")
        m_Password = decrypt(ReadIni("Local", "Password"))
    End Sub

    Public Function ReadIni(ByVal xTipe As String, ByVal xName As String) As String
        Dim res As Integer
        Dim sb As StringBuilder
        sb = New StringBuilder(500)
        res = GetPrivateProfileString(xTipe, xName, "", sb, sb.Capacity, Application.StartupPath & "\Config.ini")
        'res = GetPrivateProfileString(xTipe, xName, "", sb, sb.Capacity, "C:\Program Files\Berca\Config.ini")
        ReadIni = sb.ToString()
    End Function

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

    Private Declare Auto Function GetPrivateProfileString Lib "kernel32" (ByVal lpAppName As String, _
           ByVal lpKeyName As String, _
           ByVal lpDefault As String, _
           ByVal lpReturnedString As StringBuilder, _
           ByVal nSize As Integer, _
           ByVal lpFileName As String) As Integer
End Class
