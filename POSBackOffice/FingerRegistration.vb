Imports System.Data.SqlClient
Imports libzkfpcsharp
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.IO

Public Class FingerRegistration
    Inherits Form
    Private mDevHandle As IntPtr = IntPtr.Zero
    Private mDBHandle As IntPtr = IntPtr.Zero
    Private FormHandle As IntPtr = IntPtr.Zero
    Private stringconeksi As String = ""
    Private bIsTimeToDie As Boolean = False
    Private IsRegister As Boolean = False
    Private bIdentify As Boolean = True
    Private FPBuffer As Byte()
    Private RegisterCount As Integer = 0
    Const REGISTER_FINGER_COUNT As Integer = 3

    Private RegTmps As Byte()() = New Byte(2)() {}
    Private RegTmp As Byte() = New Byte(2047) {}
    Private CapTmp As Byte() = New Byte(2047) {}
    Private cbCapTmp As Integer = 2048
    Private cbRegTmp As Integer = 0
    Private iFid As Integer = 1

    Private mfpWidth As Integer = 0
    Private mfpHeight As Integer = 0

    Const MESSAGE_CAPTURED_OK As Integer = &H400 + 6
    <DllImport("user32.dll", EntryPoint:="SendMessageA")>
    Public Shared Function SendMessage(ByVal hwnd As IntPtr, ByVal wMsg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As Integer

    End Function
    Private Sub FingerRegistration_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        FormHandle = Me.Handle
        btnReg.Enabled = False
        txtuser.Enabled = False
        btnDisc.Enabled = False
        btnVerify.Enabled = False
        btnCon.Enabled = True
    End Sub

    Sub CheckFinger()
        Dim ret As Integer = zkfperrdef.ZKFP_ERR_OK
        If (zkfp2.Init()) = zkfperrdef.ZKFP_ERR_OK Then
            Dim nCount As Integer = zkfp2.GetDeviceCount()
            If nCount > 0 Then
                Dim ret2 As Integer = zkfp.ZKFP_ERR_OK
                mDevHandle = zkfp2.OpenDevice(0)
                If IntPtr.Zero = (zkfp2.OpenDevice(0)) Then
                    Exit Sub
                End If
                mDBHandle = zkfp2.DBInit()
                If IntPtr.Zero = (mDBHandle = zkfp2.DBInit()) Then
                    zkfp2.CloseDevice(mDevHandle)
                    mDevHandle = IntPtr.Zero
                    Exit Sub
                End If

                For i As Integer = 0 To 3 - 1
                    RegTmps(i) = New Byte(2048) {}
                Next

                Dim paramValue As Byte() = New Byte(4) {}
                Dim size As Integer = 4
                zkfp2.GetParameters(mDevHandle, 1, paramValue, size)
                zkfp2.ByteArray2Int(paramValue, mfpWidth)
                size = 4
                zkfp2.GetParameters(mDevHandle, 2, paramValue, size)
                zkfp2.ByteArray2Int(paramValue, mfpHeight)
                FPBuffer = New Byte(mfpWidth * mfpHeight) {}
                Dim captureThread As Thread = New Thread(AddressOf DoCapture)
                captureThread.IsBackground = True
                captureThread.Start()
                bIsTimeToDie = False
                btnReg.Enabled = True
                txtuser.Enabled = True
                btnCon.Enabled = False
                btnDisc.Enabled = True
                btnVerify.Enabled = True
                txtInfo.Text = "Connected!!"
                MsgBox("Connected!!")
                txtuser.Focus()
            Else
                zkfp2.Terminate()
            End If
        Else
        End If
    End Sub

    Private Sub DoCapture()
        While Not bIsTimeToDie
            cbCapTmp = 2048
            Dim ret As Integer = zkfp2.AcquireFingerprint(mDevHandle, FPBuffer, CapTmp, cbCapTmp)

            If ret = zkfp.ZKFP_ERR_OK Then
                SendMessage(FormHandle, MESSAGE_CAPTURED_OK, IntPtr.Zero, IntPtr.Zero)
            End If

            Thread.Sleep(200)
        End While
    End Sub

    Protected Overrides Sub DefWndProc(ByRef m As Message)
        Select Case m.Msg
            Case MESSAGE_CAPTURED_OK
                Dim ms As MemoryStream = New MemoryStream()
                Sample.BitmapFormat.GetBitmap(FPBuffer, mfpWidth, mfpHeight, ms)
                Dim bmp As Bitmap = New Bitmap(ms)
                Me.picFPImg.Image = bmp

                If (IsRegister) Then
                    Dim ret As Integer = zkfp.ZKFP_ERR_OK
                    Dim fid As Integer = 0, score As Integer = 0
                    ret = zkfp2.DBIdentify(mDBHandle, CapTmp, fid, score)

                    If zkfp.ZKFP_ERR_OK = ret Then
                        txtInfo.Text = "This finger was already register by " & fid & "!"
                        MsgBox("This finger was already register by " & fid & "!")
                        btnReg.Enabled = True
                        Exit Sub
                    End If

                    If RegisterCount > 0 AndAlso zkfp2.DBMatch(mDBHandle, CapTmp, RegTmps(RegisterCount - 1)) <= 0 Then
                        txtInfo.Text = "Please press the same finger 3 times for the enrollment"
                        MsgBox("Please press the same finger 3 times for the enrollment")
                        btnReg.Enabled = True
                        Exit Sub
                    End If
                    My.Computer.Audio.Play(Application.StartupPath & "/Bleep.wav", AudioPlayMode.Background)
                    Array.Copy(CapTmp, RegTmps(RegisterCount), cbCapTmp)
                    Dim strBase64 As String = zkfp2.BlobToBase64(CapTmp, cbCapTmp)
                    Dim blob As Byte() = zkfp2.Base64ToBlob(strBase64)
                    RegisterCount += 1

                    If RegisterCount >= REGISTER_FINGER_COUNT Then
                        RegisterCount = 0

                        If zkfp.ZKFP_ERR_OK = (ret = zkfp2.DBMerge(mDBHandle, RegTmps(0), RegTmps(1), RegTmps(2), RegTmp, cbRegTmp)) AndAlso zkfp.ZKFP_ERR_OK = (ret = zkfp2.DBAdd(mDBHandle, txtuser.Text, RegTmp)) Then
                            m_con = New SqlConnection(m_Sqlconn2)
                            Dim da As New SqlDataAdapter
                            Dim ds As New DataSet
                            Dim cmd As New SqlCommand

                            cmd = m_con.CreateCommand
                            cmd.CommandText = "insert into users_finger values ('" & txtuser.Text & "','" & DSBranch.Tables(0).Rows(0).Item("Branch_ID") & "' ,@binaryValue,GETDATE()) "
                            cmd.CommandTimeout = 120
                            da.SelectCommand = cmd
                            If m_con.State = ConnectionState.Open Then
                                m_con.Close()
                            End If
                            m_con.Open()
                            Dim sqlParam As SqlParameter = cmd.Parameters.AddWithValue("@binaryValue", RegTmp)
                            sqlParam.DbType = DbType.Binary
                            cmd.ExecuteNonQuery()
                            m_con.Close()
                            MsgBox("Enroll Success!!")
                            txtInfo.Text = "Enroll Success!!"
                            txtuser.Clear()
                            btnReg.Enabled = True
                        Else
                            txtInfo.Text = "Enroll Fail, error code=" & ret
                            MsgBox("Enroll Fail, error code=" & ret)
                            btnReg.Enabled = True
                        End If

                        IsRegister = False
                        Return
                    Else
                        txtInfo.Text = "You need to press the " & (REGISTER_FINGER_COUNT - RegisterCount) & " times fingerprint"
                        MsgBox("Press the " & (REGISTER_FINGER_COUNT - RegisterCount) & " times fingerprint")
                        btnReg.Enabled = True
                    End If
                End If
                If (bIdentify) Then
                    Dim dsCek As New DataSet
                    dsCek = getSqldb2("select * from users_finger")
                    If dsCek.Tables(0).Rows.Count > 0 Then
                        For Each ro As DataRow In dsCek.Tables(0).Rows
                            Dim ret As Integer = zkfp2.DBMatch(mDBHandle, CapTmp, CType(ro("RegTmp"), Byte()))
                            If ret > 0 Then
                                My.Computer.Audio.Play(Application.StartupPath & "/Bleep.wav", AudioPlayMode.Background)
                                txtInfo.Text = "Match finger success, User ID =" & ro("User_ID") & "!"
                                MsgBox("Match finger success, User ID =" & ro("User_ID") & "!")
                                'btnVerify.Enabled = True
                                Exit Sub
                            Else
                            End If
                        Next
                    End If
                    txtInfo.Text = "Match finger fail!!"
                    MsgBox("Match finger fail!!", MsgBoxStyle.Critical, "Attentions")
                End If
            Case Else
                MyBase.DefWndProc(m)
        End Select
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles btnReg.Click
        If txtuser.Text = "" Then
            MsgBox("Please insert user id!!")
            txtuser.Focus()
            Exit Sub
        End If
        btnVerify.Enabled = True
        bIdentify = False
        btnReg.Enabled = False
        IsRegister = True
        RegisterCount = 0
        cbRegTmp = 0
        txtInfo.Text = "Please press your finger 3 times!"
    End Sub

    Private Sub btnCon_Click(sender As Object, e As EventArgs) Handles btnCon.Click
        CheckFinger()
    End Sub

    Private Sub btnDisc_Click(sender As Object, e As EventArgs) Handles btnDisc.Click
        bIsTimeToDie = True
        RegisterCount = 0
        Thread.Sleep(1000)
        zkfp2.CloseDevice(mDevHandle)
        zkfp2.Terminate()
        cbRegTmp = 0
        btnReg.Enabled = False
        txtuser.Enabled = False
        btnCon.Enabled = True
        btnDisc.Enabled = False
        btnVerify.Enabled = False
    End Sub

    Private Sub btnVerify_Click(sender As Object, e As EventArgs) Handles btnVerify.Click
        btnVerify.Enabled = False
        btnReg.Enabled = True
        bIdentify = True
        txtInfo.Text = "Please press your finger!"
    End Sub

    Private Sub FingerRegistration_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        zkfp2.CloseDevice(mDevHandle)
        zkfp2.Terminate()
    End Sub
End Class