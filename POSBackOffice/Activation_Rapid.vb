Imports RACReaderAPI
Imports RACReaderAPI.MyInterface
Imports RACReaderAPI.Models
'Imports RACReaderAPI.MyConnect
Imports System
Imports System.Collections.Generic
Imports System.IO
Public Class Activation_Rapid
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
    Dim ReadStatus As Boolean = False
    Dim OnlyOne As Boolean = False
    Private Sub Connect_Click(sender As Object, e As EventArgs) Handles Connect.Click
        CheckCon()
        Button1_Click(sender, e)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        m_TagTableRapid.Clear()
        CheckBox1.Checked = False
        If ReadStatus = True Then
            If MyReader._Config.Stop(ipPort) = 0 Then
                ReadStatus = False
            End If
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
                    ReadStatus = True
                End If
            Catch ex As Exception
                MsgBox("Gagal " + ex.ToString())
            End Try
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
                    ReadStatus = True
                End If
            Catch ex As Exception
                MsgBox("Gagal " + ex.ToString())
            End Try
        End If
    End Sub

    Sub CheckCon()

        ipPort = txtIP.Text

        If (MyReader.CreateTcpConn(ipPort, example)) = True Then
            MsgBox("Connected!!!")
            'Connect.Text = "Connected"
            Connect.Enabled = False
            Button1.Enabled = True
            Connect.ForeColor = Color.Green
            If File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt") Then
                File.Delete(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt")
            End If
            Dim sw As New IO.StreamWriter(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt")
            sw.Write(txtIP.Text.Trim)
            sw.Close()
            sw.Dispose()
        Else
            MsgBox("Gagal")
        End If
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

    Private Sub GPIControlMsg(ByVal gpiIndex As Integer, ByVal gpiState As Integer, ByVal startOrStop As Integer) Implements IAsynchronousMessage.GPIControlMsg

    End Sub
    Private Sub WriteDebugMsg(ByVal msg As String) Implements IAsynchronousMessage.WriteDebugMsg

    End Sub

    Private Sub DeviceInfo(ByVal Model As Device_Mode) Implements ISearchDevice.DeviceInfo

    End Sub

    Private Sub DebugMsg(ByVal Msg As String) Implements ISearchDevice.DebugMsg

    End Sub

    Private Sub Activation_Rapid_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ReadStatus = False
        example = New Form1()
        If File.Exists(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt") Then
            Using sr As New StreamReader(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\IPReader.txt")
                Dim line As String
                line = sr.ReadToEnd
                txtIP.Text = line
            End Using
        End If
        m_TagTableRapid = New Hashtable
    End Sub

    Function HexToString(ByVal hex As String) As String
        Dim text As New System.Text.StringBuilder(hex.Length \ 2)
        For i As Integer = 0 To hex.Length - 2 Step 2
            text.Append(Chr(Convert.ToByte(hex.Substring(i, 2), 16)))
        Next
        Return text.ToString
    End Function

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        If RFIDOKE = True Then
            Dim dsCek As New DataSet
            For y As Integer = 0 To CntRFID - 1
                Timer1.Stop()
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
                    If OnlyOne = False Then
                        OnlyOne = True
                    End If
                End If

                If stt = "Available" Then
                    DataGridView1.Item(9, y).Value = True
                Else
                    DataGridView1.Item(9, y).Value = False
                End If
                DataGridView1.Item(4, y).Value = stt
                CheckBox1.Visible = True
                If OnlyOne = True Then
                    'Dim NewData As String = InputBox("New PLU  " & HexToString(Microsoft.VisualBasic.Left(tag.TagID, 26)) & " ", "Input Here", "")

                    Dim NewData As String = MyInputBox2(HexToString(Microsoft.VisualBasic.Left(EPC(y), 26)), "Enter name", txtIP.ToString, True)
                    If NewData <> "" Then
                        dsCek.Clear()
                        dsCek = getSqldb3("select * from Item_Master where PLU = '" & NewData.Trim & "'")
                        If dsCek.Tables(0).Rows.Count = 0 Then
                            MsgBox("PLU is Not Found !!!")
                            DataGridView1.Item(9, y).Value = False
                            OnlyOne = False
                            GoTo 1
                        End If
                        DataGridView1.Item(2, y).Value = NewData
                    Else
                        DataGridView1.Item(9, y).Value = False
                        OnlyOne = False
                        'GoTo 1
                    End If
                    'Timer1.Start()

                End If
1:
            Next

            RFIDOKE = False
            Timer1.Enabled = False
            If OnlyOne = True Then
                Simpan()
            End If
        End If
    End Sub

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

    Private Sub EDITToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EDITToolStripMenuItem.Click
        If DataGridView1.RowCount > 0 Then
            For Each ro As DataGridViewRow In DataGridView1.SelectedRows
                Dim dsCek As New DataSet

                'Dim NewData As String = InputBox("New PLU  " & DataGridView1.Item(0, ro.Index).Value & " ", "Input Here", "")
                Dim NewData As String = MyInputBox2(DataGridView1.Item(0, ro.Index).Value, "Enter name", txtIP.ToString, True)
                If NewData <> "" Then
                    dsCek.Clear()
                    dsCek = getSqldb3("select * from Item_Master where PLU = '" & NewData.Trim & "'")
                    If dsCek.Tables(0).Rows.Count = 0 Then
                        MsgBox("PLU is Not Found !!!")
                        DataGridView1.Item(9, ro.Index).Value = False
                        Exit Sub
                    End If
                    DataGridView1.Item(2, ro.Index).Value = NewData
                    DataGridView1.Item(9, ro.Index).Value = True
                    Simpan()
                    klik = True
                End If

            Next
        End If
    End Sub

    Sub Simpan()
        Dim BaruStatus As Boolean = False
        If ReadStatus = True Then
            'MsgBox("Please STOP Reading first !!!")
            'Exit Sub
            Try
                If MyReader._Config.Stop(ipPort) = 0 Then
                    ReadStatus = False
                End If
            Catch ex As Exception

            End Try

        End If
        If DataGridView1.RowCount > 0 Then
            Dim dsCek As New DataSet
            For Each row As DataGridViewRow In DataGridView1.Rows
                If row.Cells(9).Value = True Then
                    dsCek.Clear()
                    dsCek = getSqldb3("Select * from Item_Master_RFID where EPC = '" & row.Cells(0).Value.ToString.Trim & "' And status = 0")
                    If dsCek.Tables(0).Rows.Count > 0 Then
                        If row.Cells(3).Value.ToString.Trim = "" Then
                            getSqldb3("update Item_Master_RFID set status = 1 where EPC = '" & row.Cells(0).Value.ToString.Trim & "' And TID = '" & row.Cells(5).Value.ToString.Trim & "'")
                            getSqldb("Insert into Back_Office_Log values ('" & UserName & "','RESET RFID ZEBRA','" & row.Cells(0).Value.ToString.Trim & "','Success','','" & Now & "')")
                        Else
                            getSqldb3("update Item_Master_RFID set status = 1 where EPC = '" & row.Cells(0).Value.ToString.Trim & "' And TID = '" & row.Cells(5).Value.ToString.Trim & "'")
                            getSqldb3("Insert  into Item_Master_RFID select distinct article_code,PLU,'" & row.Cells(0).Value.ToString.Trim & "','" & row.Cells(5).Value.ToString.Trim & "','','','',0,0,0,getdate() from item_master where plu = '" & row.Cells(2).Value.ToString.Trim & "'")
                            getSqldb("Insert into Back_Office_Log values ('" & UserName & "','UPDATE RFID ZEBRA','" & row.Cells(0).Value.ToString.Trim & "','Success','','" & Now & "')")
                        End If
                    Else
                        getSqldb3("Insert into Item_Master_RFID select distinct article_code,PLU,'" & row.Cells(0).Value.ToString.Trim & "','" & row.Cells(5).Value.ToString.Trim & "','','','',0,0,0,getdate() from item_master where plu = '" & row.Cells(2).Value.ToString.Trim & "'")
                        getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Write NEW RFID ZEBRA','" & row.Cells(0).Value.ToString.Trim & "','Success','','" & Now & "')")
                    End If
                End If
            Next
            DataGridView1.Columns(5).Visible = False
            RefreshRFID()
            'MsgBox("Success!!!")
        End If
    End Sub

    Sub RefreshRFID()
        CheckBox1.Checked = False
        m_TagTableRapid.Clear()
        OnlyOne = False
        If ReadStatus = True Then
            If MyReader._Config.Stop(ipPort) = 0 Then
                ReadStatus = False
            End If
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
                    ReadStatus = True
                End If
            Catch ex As Exception
                MsgBox("Gagal " + ex.ToString())
            End Try
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
                    ReadStatus = True
                End If
            Catch ex As Exception
                MsgBox("Gagal " + ex.ToString())
            End Try
        End If
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

    Private Sub CLEARALLToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CLEARALLToolStripMenuItem.Click
        If DataGridView1.RowCount > 0 Then
            If MsgBox("Are You Sure Clear This Data??", MsgBoxStyle.YesNo, "Information") = MsgBoxResult.Yes Then
                For Each ro As DataGridViewRow In DataGridView1.SelectedRows
                    If DataGridView1.Item(2, ro.Index).ReadOnly = True Then
                        'DataGridView1.Item(0, ro.Index).Value = "1000000000000"
                        DataGridView1.Item(2, ro.Index).Value = ""
                        DataGridView1.Item(3, ro.Index).Value = ""
                        DataGridView1.Item(9, ro.Index).Value = True
                        Simpan()
                        klik = True
                    End If
                Next
            End If

        End If
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridView1.RowCount > 0 Then
            If MsgBox("Are You Sure Clear This Data??", MsgBoxStyle.YesNo, "Information") = MsgBoxResult.Yes Then

                Dim dsCek As New DataSet
                For Each row As DataGridViewRow In DataGridView1.Rows
                    If row.Cells(9).Value = True Then
                        row.Cells(2).Value = ""
                        row.Cells(3).Value = ""
                    End If
                Next
                Simpan()
                klik = True
            End If
        End If
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Simpan()
    End Sub

    Private Sub Activation_Rapid_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F1 Then
            Connect_Click(sender, e)
        ElseIf e.KeyCode = Keys.F2 Then
            Button1_Click(sender, e)
        ElseIf e.KeyCode = Keys.F4 Then
            Button4_Click(sender, e)
        ElseIf e.KeyCode = Keys.F5 Then
            Button2_Click(sender, e)
        End If
    End Sub
End Class