Imports RACReaderAPI
Imports RACReaderAPI.MyInterface
Imports RACReaderAPI.Models
'Imports RACReaderAPI.MyConnect
Imports System
Imports System.Collections.Generic
Imports System.IO
Public Class Form1
    Implements IAsynchronousMessage
    Implements ISearchDevice
    Shared param_Set As Param_Set = New Param_Set()
    Shared rfid_Option As RFID_Option = New RFID_Option()
    Dim example As Form1
    Dim epcstr As String
    Dim ipPort As String
    Dim hitung As Integer = 0
    Private rowIndex As Integer = 0
    Dim klik As Boolean = False
    Private Sub RFIDSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        example = New Form1()
        If File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt") Then
            Using sr As New StreamReader(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt")
                Dim line As String
                line = sr.ReadToEnd
                txtIP.Text = line
            End Using
        End If
        m_TagTableRapid = New Hashtable
        ButtonEnable(False)
        Read.Enabled = False
    End Sub

    Private Sub OutPutTags(ByVal tag As Tag_Model) Implements IAsynchronousMessage.OutPutTags
        Dim isFound As Boolean = False
        SyncLock m_TagTableRapid.SyncRoot
            isFound = m_TagTableRapid.ContainsKey(tag.TID.ToString)
        End SyncLock
        If isFound Then

        Else
            EPC(CntRFID) = tag.EPC.ToString
            TID(CntRFID) = tag.TID.ToString
            UserData(CntRFID) = tag.UserData.ToString
            TagType(CntRFID) = tag.TagType.ToString
            ReaderName(CntRFID) = tag.ReaderName.ToString
            CntRFID += 1
            My.Computer.Audio.Play(Application.StartupPath & "/Beep.wav", AudioPlayMode.Background)
            SyncLock m_TagTableRapid.SyncRoot
                m_TagTableRapid.Add(tag.TID.ToString, tag.TID.ToString)
            End SyncLock
        End If


    End Sub

    Private Sub OutPutTagsOver() Implements IAsynchronousMessage.OutPutTagsOver
        RFIDOKE = True

    End Sub

    Private Sub PortClosing(ByVal connID As String) Implements IAsynchronousMessage.PortClosing

    End Sub

    Private Sub PortConnecting(ByVal connID As String) Implements IAsynchronousMessage.PortConnecting

    End Sub

    Private Sub WriteLog(ByVal msg As String) Implements IAsynchronousMessage.WriteLog
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Read.Click
        m_TagTableRapid.Clear()

        If Read.Text = "Stop" Then
            If MyReader._Config.Stop(ipPort) = 0 Then
                Read.Text = "Read"
                'ButtonEnable(False)
            End If
        Else
            Try
                hitung = 0
                DataGridView1.ColumnCount = 9
                DataGridView1.Columns(0).Name = "RFID Code"
                DataGridView1.Columns(1).Name = "Antenna"
                DataGridView1.Columns(2).Name = "PLU"
                DataGridView1.Columns(3).Name = "Description"
                DataGridView1.Columns(4).Name = "Status"
                DataGridView1.Columns(5).Name = "Bank"
                DataGridView1.Columns(6).Name = "Bank Name"
                DataGridView1.Columns(7).Name = "Real EPC"
                DataGridView1.Columns(8).Name = "EPC2"

                DataGridView1.Columns(5).Visible = False
                DataGridView1.Columns(6).Visible = False
                DataGridView1.Columns(7).Visible = False
                DataGridView1.Columns(8).Visible = False

                DataGridView1.Columns(0).Width = "150"
                DataGridView1.Columns(1).Width = "80"
                DataGridView1.Columns(2).Width = "150"
                DataGridView1.Columns(3).Width = "290"
                DataGridView1.Columns(4).Width = "90"


                If DataGridView1.Rows.Count > 0 Then
                    DataGridView1.Rows.Clear()
                End If
                Dim chkBoxCol As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
                DataGridView1.Columns.Add(chkBoxCol)
                DataGridView1.Columns(9).Width = "40"
                DataGridView1.Columns(9).ReadOnly = False
                DataGridView1.Columns(0).ReadOnly = True
                DataGridView1.Columns(1).ReadOnly = True
                DataGridView1.Columns(2).ReadOnly = True
                DataGridView1.Columns(3).ReadOnly = True
                DataGridView1.Columns(4).ReadOnly = True
                DataGridView1.Columns(5).ReadOnly = True
                DataGridView1.Columns(6).ReadOnly = True
                DataGridView1.Columns(7).ReadOnly = True
                DataGridView1.Columns(8).ReadOnly = True

                CntRFID = 0
                RFIDOKE = False
                Dim antNum As New eAntennaNo()
                antNum = eAntennaNo._1
                Dim readType As New eReadType()
                readType = eReadType.Single
                Timer1.Enabled = True
                If MyReader._Tag6C.GetEPC_TID_UserData(ipPort, antNum, readType, 0, 2) = 0 Then
                    Read.Text = "Stop"
                End If
                ButtonEnable(True)
                'biar otomatis stop tambahan
                'If MyReader._Config.Stop(ipPort) = 0 Then
                '    Read.Text = "Read"
                '    ButtonEnable(True)
                'End If
                'SAVEAllEPC.Enabled = False
            Catch ex As Exception
                MsgBox("Gagal " + ex.ToString())
            End Try
        End If
    End Sub

    Function HexToString(ByVal hex As String) As String
        Dim text As New System.Text.StringBuilder(hex.Length \ 2)
        For i As Integer = 0 To hex.Length - 2 Step 2
            text.Append(Chr(Convert.ToByte(hex.Substring(i, 2), 16)))
        Next
        Return text.ToString
    End Function

    Sub ButtonEnable(ByVal status As Boolean)
        WEPC.Enabled = status
        'WUSER.Enabled = status
        SETIP.Enabled = status
        GETIP.Enabled = status
        'WAEPC.Enabled = status
        'SAVEAllEPC.Enabled = status
        SaveDB.Enabled = status
    End Sub

    Sub CheckCon()

        ipPort = txtIP.Text

        If (MyReader.CreateTcpConn(ipPort, example)) = True Then
            MsgBox("Connected!!!")
            'Connect.Text = "Connected"
            Connect.Enabled = False
            Read.Enabled = True
            Connect.ForeColor = Color.Green
            If File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt") Then
                File.Delete(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt")
            End If
            Dim sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt")
            sw.Write(txtIP.Text.Trim)
            sw.Close()
            sw.Dispose()
            ButtonEnable(True)
        Else
            MsgBox("Gagal")
        End If
    End Sub

    Private Sub GPIControlMsg(ByVal gpiIndex As Integer, ByVal gpiState As Integer, ByVal startOrStop As Integer) Implements IAsynchronousMessage.GPIControlMsg

    End Sub
    Private Sub WriteDebugMsg(ByVal msg As String) Implements IAsynchronousMessage.WriteDebugMsg

    End Sub

    Private Sub DeviceInfo(ByVal Model As Device_Mode) Implements ISearchDevice.DeviceInfo

    End Sub

    Private Sub DebugMsg(ByVal Msg As String) Implements ISearchDevice.DebugMsg

    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        'If hitung < 1 Then
        '    hitung += 1
        '    Exit Sub
        'End If
        If RFIDOKE = True Then
            Dim dsCek As New DataSet
            For y As Integer = 0 To CntRFID - 1
                dsCek.Clear()
                dsCek = getSqldb3("select a.PLU,b.long_Description as Description,a.TID,a.flag from item_master_rfid a inner join item_master b on a.article_code = b.article_code " &
                                          " where EPC = '" & HexToString(Microsoft.VisualBasic.Left(EPC(y), 26)) & "' And Status = '0'")
                Dim stt As String = "Available"
                Dim PLU, Description As String
                If dsCek.Tables(0).Rows.Count > 0 Then
                    PLU = dsCek.Tables(0).Rows(0).Item("PLU").ToString.Trim
                    Description = dsCek.Tables(0).Rows(0).Item("Description").ToString.Trim

                Else
                    PLU = ""
                    Description = ""

                End If
                DataGridView1.Rows.Add(1)
                DataGridView1.Item(0, y).Value = HexToString(Microsoft.VisualBasic.Left(EPC(y), 26))
                DataGridView1.Item(1, y).Value = "1"
                DataGridView1.Item(2, y).Value = PLU
                DataGridView1.Item(3, y).Value = Description
                DataGridView1.Item(7, y).Value = EPC(y)
                DataGridView1.Item(6, y).Value = UserData(y)
                DataGridView1.Item(5, y).Value = TID(y)
                DataGridView1.Columns(5).Visible = False
                DataGridView1.Columns(6).Visible = False
                DataGridView1.Columns(7).Visible = False
                DataGridView1.Columns(8).Visible = False
                DataGridView1.Item(8, y).Value = HexToString(Microsoft.VisualBasic.Left(EPC(y), 26))
                DataGridView1.Rows(y).Cells(4).Style.Font = New Font(DataGridView1.Font.Name, DataGridView1.Font.Size, FontStyle.Bold)
                If dsCek.Tables(0).Rows.Count > 0 Then
                    If dsCek.Tables(0).Rows(0).Item("flag").ToString.Trim = "0" Then
                        DataGridView1.Rows(y).Cells(4).Style.BackColor = Color.PaleGreen
                        stt = "Activated"
                    Else
                        DataGridView1.Rows(y).Cells(4).Style.BackColor = Color.Khaki
                        stt = "Sold"
                    End If
                    If dsCek.Tables(0).Rows(0).Item("TID").ToString.Trim = "" Then
                        DataGridView1.Rows(y).Cells(4).Style.BackColor = Color.White
                        stt = "Available"
                    Else
                        If dsCek.Tables(0).Rows(0).Item("TID").ToString.Trim <> TID(y) Then
                            DataGridView1.Rows(y).Cells(4).Style.BackColor = Color.Salmon
                            stt = "Duplicate"
                        End If
                    End If
                    DataGridView1.Item(2, y).ReadOnly = True
                Else
                    DataGridView1.Item(2, y).ReadOnly = False
                End If
                DataGridView1.Item(4, y).Value = stt
                CheckBox1.Visible = True
            Next

            RFIDOKE = False
            Timer1.Enabled = False
        End If
    End Sub

    Private Sub Connect_Click(sender As Object, e As EventArgs) Handles Connect.Click
        CheckCon()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles WEPC.Click
        Dim myValue As String = InputBox("Input Here", "New EPC", DataGridView1(0, DataGridView1.CurrentRow.Index).Value)
        If myValue <> "" Then
            epcstr = ""
            AddChar(myValue)
            DataGridView1.Columns(5).Visible = False
            Dim antNum As New eAntennaNo()
            antNum = eAntennaNo._1
            If MyReader._Tag6C.WriteEPC_MatchTID(ipPort, antNum, epcstr, DataGridView1(5, DataGridView1.CurrentRow.Index).Value, 0) = 0 Then
                MsgBox("Success!!!")
                getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Write RFID Rapid','" & DataGridView1(5, DataGridView1.CurrentRow.Index).Value & "','Success','','" & Now & "')")
                Button1_Click(sender, e)
                Button1_Click(sender, e)
            End If
        End If
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs)
        Dim myValue As String = InputBox("Input Here", "New UserData", DataGridView1(0, DataGridView1.CurrentRow.Index).Value)
        If myValue <> "" Then
            DataGridView1.Columns(4).Visible = False
            Dim antNum As New eAntennaNo()
            antNum = eAntennaNo._1
            If MyReader._Tag6C.WriteUserData_MatchTID(ipPort, antNum, myValue, 0, DataGridView1(5, DataGridView1.CurrentRow.Index).Value, 0) = 0 Then
                MsgBox("Success!!!")
                Button1_Click(sender, e)
                If MyReader._Config.Stop(ipPort) = 0 Then
                    Read.Text = "Read"
                    ButtonEnable(True)
                    'SAVEAllEPC.Enabled = False
                End If
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles GETIP.Click
        MsgBox(MyReader._Config.GetReaderNetworkPortParam(ipPort))
        MsgBox(MyReader._Config.GetReaderServerOrClient(ipPort))
    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles SETIP.Click
        Try
            Dim ip, Mask, GateWay As String
            ip = InputBox("IP Address", "Input Here", "192.168.1.116")
            Mask = InputBox("Mask", "Input Here", "255.255.255.0")
            GateWay = InputBox("GateWay", "Input Here", "192.168.1.1")
            If MyReader._Config.SetReaderNetworkPortParam(ipPort, ip, Mask, GateWay) = 0 Then
                MsgBox("Success!!!")
                If File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt") Then
                    File.Delete(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt")
                End If
                Dim sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt")
                sw.Write(ip & ":9090")
                sw.Close()
                sw.Dispose()
            Else
                MsgBox("Failed")
            End If
            Application.Restart()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        MyReader.CloseConn(txtIP.Text)
    End Sub

    'Private Sub WAEPC_Click(sender As Object, e As EventArgs) Handles WAEPC.Click
    '    SAVEAllEPC.Enabled = True
    '    DataGridView1.Columns(5).Visible = True
    '    DataGridView1.Columns(5).Name = "New EPC"
    '    DataGridView1.CurrentCell = DataGridView1.Item(5, 0)
    '    DataGridView1.BeginEdit(True)
    '    SAVEAllEPC.Enabled = True
    'End Sub

    'Private Sub WAUSER_Click(sender As Object, e As EventArgs) Handles SAVEAllEPC.Click
    '    If DataGridView1.RowCount > 0 Then
    '        For Each row As DataGridViewRow In DataGridView1.Rows
    '            If row.Cells(5).Value <> "" Then
    '                epcstr = ""
    '                AddChar(row.Cells(5).Value)
    '                Dim antNum As New eAntennaNo()
    '                antNum = eAntennaNo._1
    '                If MyReader._Tag6C.WriteEPC_MatchTID(ipPort, antNum, epcstr, row.Cells(1).Value, 0) = 0 Then

    '                End If
    '            End If
    '        Next
    '        Button1_Click(sender, e)
    '        Button1_Click(sender, e)
    '        DataGridView1.Columns(5).Visible = False
    '        MsgBox("Updated")
    '    End If
    'End Sub

    Private Sub SaveDB_Click(sender As Object, e As EventArgs) Handles SaveDB.Click
        If DataGridView1.RowCount > 0 Then
            Dim dsCek As New DataSet
            For Each row As DataGridViewRow In DataGridView1.Rows
                If row.Cells(9).Value = True Then
                    If row.Cells(0).Value.ToString.Trim = "1000000000000" And row.Cells(2).Value.ToString.Trim = "" Then
                        getSqldb3("update Item_Master_RFID set status = 1 where EPC = '" & row.Cells(8).Value.ToString.Trim & "' And TID = '" & row.Cells(5).Value.ToString.Trim & "'")
                        epcstr = ""
                        AddChar("1000000000000")
                        Dim antNum As New eAntennaNo()
                        antNum = eAntennaNo._1
                        If MyReader._Tag6C.WriteEPC_MatchTID(ipPort, antNum, epcstr, DataGridView1(5, DataGridView1.CurrentRow.Index).Value, 0) = 0 Then
                            getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Clear RFID Rapid','" & DataGridView1(5, DataGridView1.CurrentRow.Index).Value & "','Success','','" & Now & "')")
                        End If
                    Else
                        dsCek.Clear()
                        dsCek = getSqldb3("select * from Item_Master where PLU = '" & row.Cells(2).Value.ToString.Trim & "'")
                        If dsCek.Tables(0).Rows.Count = 0 Then
                            GoTo 2
                        End If
                        dsCek.Clear()
                        dsCek = getSqldb3("Select * from Item_Master_RFID where EPC = '" & row.Cells(0).Value.ToString.Trim & "' ")
                        If dsCek.Tables(0).Rows.Count > 0 Then
                            Try
                                'getSqldb3("update a set a.plu = b.plu, a.article_code = '" & Replace(row.Cells(0).Value.ToString.Trim, "F", "") & "', a.EPC = '" & row.Cells(0).Value.ToString.Trim & "', " &
                                '                              "a.userdata = '" & row.Cells(2).Value.ToString.Trim & "' from Item_Master_RFID a inner join " &
                                '                              "item_master b on a.article_code = b.article_code where a.EPC = '" & row.Cells(0).Value.ToString.Trim & "'")
                                If dsCek.Tables(0).Rows(0).Item("TID").ToString.Trim = "" Then
                                Else
                                    If dsCek.Tables(0).Rows(0).Item("TID").ToString.Trim <> row.Cells(5).Value.ToString.Trim Then
                                        GoTo 1
                                    End If
                                End If
                                'getSqldb3("update a set a.TID = '" & row.Cells(5).Value.ToString.Trim & "',a.article_code = b.article_code,a.plu = b.plu from Item_Master_RFID a inner join " &
                                '                              "item_master b on '" & row.Cells(2).Value.ToString.Trim & "' = b.PLU where a.EPC = '" & row.Cells(0).Value.ToString.Trim & "' And b.plu = '" & row.Cells(2).Value.ToString.Trim & "'")
                                getSqldb3("update Item_Master_RFID set status = 1 where EPC = '" & row.Cells(0).Value.ToString.Trim & "' And TID = '" & row.Cells(5).Value.ToString.Trim & "'")
                                Dim NewEPC As String = ""
                                Dim DSCek2 As New DataSet
                                DSCek2 = getSqldb3("select dp2 from item_master where plu = '" & row.Cells(2).Value.ToString.Trim & "'")
                                If DSCek2.Tables(0).Rows.Count > 0 Then
                                    NewEPC = Cek(DSCek2.Tables(0).Rows(0).Item("dp2").ToString.Trim)
                                    getSqldb3("Insert  into Item_Master_RFID select distinct article_code,PLU,'" & NewEPC & "','" & row.Cells(5).Value.ToString.Trim & "','','','',0,0,getdate() from item_master where plu = '" & row.Cells(2).Value.ToString.Trim & "'")
                                    NewEPC = NewEPC.ToString.Trim
                                    epcstr = ""
                                    AddChar(NewEPC)
                                    Dim antNum As New eAntennaNo()
                                    antNum = eAntennaNo._1
                                    If MyReader._Tag6C.WriteEPC_MatchTID(ipPort, antNum, epcstr, DataGridView1(5, DataGridView1.CurrentRow.Index).Value, 0) = 0 Then
                                        getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Write Update RFID Rapid','" & DataGridView1(5, DataGridView1.CurrentRow.Index).Value & "','Success','','" & Now & "')")
                                    End If
                                End If

1:
                            Catch ex As Exception
                                MsgBox(ex.Message & " " & row.Cells(0).Value.ToString.Trim)
                            End Try
                            'Else
                            '    'getSqldb3("Insert Into Item_Master_RFID select '" & row.Cells(0).Value.ToString.Trim.Substring(0, row.Cells(0).Value.ToString.Trim.IndexOf("F")) & "',plu,'" & row.Cells(0).Value.ToString.Trim & "'," &
                            '    '    "'" & row.Cells(1).Value.ToString.Trim & "','" & row.Cells(2).Value.ToString.Trim & "','','',0,0 from  Item_Master " &
                            '    '    " where article_code = '" & row.Cells(0).Value.ToString.Trim.Substring(0, row.Cells(0).Value.ToString.Trim.IndexOf("F")) & "'")
                            '    getSqldb3("Insert Into Item_Master_RFID select top 1 '" & row.Cells(0).Value.ToString.Trim.Substring(0, row.Cells(0).Value.ToString.Trim.IndexOf("F")) & "',plu,'" & row.Cells(0).Value.ToString.Trim & "'," &
                            '       "'" & row.Cells(1).Value.ToString.Trim & "','" & row.Cells(2).Value.ToString.Trim & "','','',0,0 from  Item_Master " &
                            '       " where article_code = '" & row.Cells(0).Value.ToString.Trim.Substring(0, row.Cells(0).Value.ToString.Trim.IndexOf("F")) & "'")
                        Else
                            Dim DSCek2 As New DataSet
                            Dim NewEPC As String = ""
                            DSCek2 = getSqldb3("select dp2 from item_master where plu = '" & row.Cells(2).Value.ToString.Trim & "'")
                            If DSCek2.Tables(0).Rows.Count > 0 Then
                                NewEPC = Cek(DSCek2.Tables(0).Rows(0).Item("dp2").ToString.Trim)
                                getSqldb3("Insert  into Item_Master_RFID select distinct article_code,PLU,'" & NewEPC & "','" & row.Cells(5).Value.ToString.Trim & "','','','',0,0,getdate() from item_master where plu = '" & row.Cells(2).Value.ToString.Trim & "'")
                                NewEPC = NewEPC.ToString.Trim
                                epcstr = ""
                                AddChar(NewEPC)
                                Dim antNum As New eAntennaNo()
                                antNum = eAntennaNo._1
                                If MyReader._Tag6C.WriteEPC_MatchTID(ipPort, antNum, epcstr, DataGridView1(5, DataGridView1.CurrentRow.Index).Value, 0) = 0 Then
                                    getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Write NEW RFID Rapid','" & DataGridView1(5, DataGridView1.CurrentRow.Index).Value & "','Success','','" & Now & "')")
                                End If
                            End If
                        End If
                    End If


                End If
2:
            Next
            Button1_Click(sender, e)
            Button1_Click(sender, e)
            DataGridView1.Columns(5).Visible = False
            MsgBox("Success!!!")
        End If
    End Sub

    Sub AddChar(ByVal epc As String)
        For x As Integer = 1 To 13
            Select Case Microsoft.VisualBasic.Mid(epc, x, 1)
                Case "0"
                    epcstr &= "30"
                Case "1"
                    epcstr &= "31"
                Case "2"
                    epcstr &= "32"
                Case "3"
                    epcstr &= "33"
                Case "4"
                    epcstr &= "34"
                Case "5"
                    epcstr &= "35"
                Case "6"
                    epcstr &= "36"
                Case "7"
                    epcstr &= "37"
                Case "8"
                    epcstr &= "38"
                Case "9"
                    epcstr &= "39"
                Case "A"
                    epcstr &= "41"
                Case "B"
                    epcstr &= "42"
                Case "C"
                    epcstr &= "43"
                Case "D"
                    epcstr &= "44"
                Case "E"
                    epcstr &= "45"
                Case "F"
                    epcstr &= "46"
            End Select
        Next

    End Sub

    Private Function Cek(ByVal SBU As String) As String
        Dim ds As New DataSet

        If SBU = "CH" Then
            Cek = "A" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        ElseIf SBU = "HH" Then
            Cek = "B" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        ElseIf SBU = "LA" Then
            Cek = "C" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        ElseIf SBU = "LD" Then
            Cek = "D" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        ElseIf SBU = "MD" Then
            Cek = "E" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        Else
            Cek = "F" & Microsoft.VisualBasic.Right(Year(Now.Date), 2) & "00".Substring(0, 2 - Len(Month(Now.Date).ToString)) & Month(Now.Date)
        End If
        ds = getSqldb3("Select Convert(INT,SUBSTRING(EPC,6,8)) + 1 As Urut from Item_Master_RFID where SUBSTRING(EPC,1,5) = '" & Cek & "' and Status = '0' order by Convert(INT,SUBSTRING(EPC,6,8)) desc")

        If ds.Tables(0).Rows.Count > 0 Then
            Cek = Cek & "00000000".Substring(0, 8 - Len(ds.Tables(0).Rows(0).Item("urut").ToString)) & ds.Tables(0).Rows(0).Item("urut")
        Else
            Cek = Cek & "00000001"
        End If
        Return Cek
    End Function
    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        If CheckBox1.Checked = True Then
            For x As Integer = 0 To DataGridView1.RowCount - 1
                DataGridView1.Item(9, x).Value = True
            Next
        Else
            For x As Integer = 0 To DataGridView1.RowCount - 1
                DataGridView1.Item(9, x).Value = False
            Next
        End If
    End Sub

    Private Sub ContextMenuStrip1_Click(sender As Object, e As EventArgs) Handles ContextMenuStrip1.Click
        'If DataGridView1.RowCount > 0 Then
        '    For Each ro As DataGridViewRow In DataGridView1.SelectedRows
        '        If DataGridView1.Item(2, ro.Index).ReadOnly = True Then
        '            DataGridView1.Item(2, ro.Index).ReadOnly = False
        '            DataGridView1.Item(2, ro.Index).Selected = True
        '            klik = True
        '        End If
        '    Next
        'End If
    End Sub

    Private Sub DataGridView1_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridView1.CellMouseUp
        If e.Button = MouseButtons.Right Then
            'Me.DataGridView1.Rows(e.RowIndex).Selected = True
            Me.rowIndex = e.RowIndex
            'Me.DataGridView1.CurrentCell = Me.DataGridView1.Rows(e.RowIndex).Cells(2)
            Me.ContextMenuStrip1.Show(Me.DataGridView1, e.Location)
            ContextMenuStrip1.Show(Cursor.Position)

        End If
    End Sub

    'Private Sub DataGridView1_CellMouseLeave(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellMouseLeave
    '    If klik = True Then
    '        If DataGridView1(3, e.RowIndex).Value <> "" Then
    '            DataGridView1(e.RowIndex, e.ColumnIndex).ReadOnly = True
    '        End If
    '        klik = False
    '    End If

    'End Sub

    Private Sub DataGridView1_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        Try
            If e.RowIndex <> rowIndex And DataGridView1.Item(3, rowIndex).Value.ToString.Trim <> "" Then
                DataGridView1(2, rowIndex).ReadOnly = True
            End If
            rowIndex = e.RowIndex
        Catch ex As Exception

        End Try

    End Sub

    Private Sub CLEARALLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CLEARALLToolStripMenuItem.Click
        If DataGridView1.RowCount > 0 Then
            For Each ro As DataGridViewRow In DataGridView1.SelectedRows
                If DataGridView1.Item(2, ro.Index).ReadOnly = True Then
                    DataGridView1.Item(0, ro.Index).Value = "1000000000000"
                    DataGridView1.Item(2, ro.Index).Value = ""
                    DataGridView1.Item(3, ro.Index).Value = ""
                    klik = True
                End If
            Next
        End If
    End Sub

    Private Sub EDITToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EDITToolStripMenuItem.Click
        If DataGridView1.RowCount > 0 Then
            For Each ro As DataGridViewRow In DataGridView1.SelectedRows
                If DataGridView1.Item(2, ro.Index).ReadOnly = True Then
                    DataGridView1.Item(2, ro.Index).ReadOnly = False
                    DataGridView1.Item(2, ro.Index).Selected = True
                    klik = True
                End If
            Next
        End If
    End Sub
End Class
