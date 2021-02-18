Imports Symbol.RFID3
Imports Symbol.RFID3.Events
Imports Symbol.RFID3.TagAccess
Imports Symbol.RFID3.TagAccess.Sequence
Public Class inventoryMovement
    Dim ip, Port As String
    Friend m_ReaderAPI As RFIDReader
    Dim m_IsConnected As Boolean
    Private m_TagTable As Hashtable
    Private m_UpdateStatusHandler As UpdateStatus = Nothing
    Private m_ReadTag As TagData = Nothing
    Private m_UpdateReadHandler As UpdateRead = Nothing
    Friend m_AccessOpResult As AccessOperationResult
    Friend m_ReadParams As ReadAccessParams = Nothing
    Friend m_WriteParams As WriteAccessParams
    Friend m_WriteSpecParams As WriteSpecificFieldAccessParams
    Friend m_AntennaInfoForm As AntennaInfo
    Dim cnt As Integer = 0
    Dim epcstr As String
    Private rowIndex As Integer = 0
    Dim antID As UInt16()
    Dim rxValues As Integer()
    Dim txValues As Integer()
    Dim tLoad As Boolean = False
    Dim klik As Boolean = False

    Dim ReadStatus As Boolean = False
    Private Sub InventoryTransfer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.m_ReadTag = New TagData
        Me.m_UpdateStatusHandler = New UpdateStatus(AddressOf Me.myUpdateStatus)
        Me.m_UpdateReadHandler = New UpdateRead(AddressOf Me.myUpdateRead)
        Me.m_TagTable = New Hashtable
        Me.m_AccessOpResult = New AccessOperationResult
        Me.m_IsConnected = False

        Me.m_ReadParams = New ReadAccessParams
        Me.m_ReadParams.MemoryBank = MEMORY_BANK.MEMORY_BANK_EPC
        Me.m_ReadParams.AccessPassword = 0
        Me.m_ReadParams.ByteOffset = 0
        Me.m_ReadParams.ByteCount = 0


        Me.m_WriteParams = New WriteAccessParams
        Me.m_WriteParams.MemoryBank = MEMORY_BANK.MEMORY_BANK_USER
        Me.m_WriteParams.AccessPassword = 0
        Me.m_WriteParams.ByteOffset = 0
        Me.m_WriteParams.WriteDataLength = 0


        If m_IsConnected = False Then
            CheckCon()
            OneReadAll = False
            If m_IsConnected = False Then

            Else
                SetReader()
                RefreshRFID()
            End If
        End If
        'Me.DataGridView1.Font = New Font("Tahoma", 12, FontStyle.Regular)
    End Sub

    Private Sub SetReader()
        Try

            Dim antID As UInt16() = m_ReaderAPI.Config.Antennas.AvailableAntennas
            Dim antConfig As Antennas.Config = m_ReaderAPI.Config.Antennas.Item(antID(0)).GetConfig
            antConfig.TransmitPowerIndex = transmitPower

            m_ReaderAPI.Config.Antennas.Item(antID(0)).SetConfig(antConfig)

        Catch iue As InvalidUsageException

        Catch ofe As OperationFailureException

        Catch ex As Exception

        End Try
    End Sub

    Sub CheckCon()

        m_ReaderAPI = New RFIDReader(IPReader, UInt32.Parse("5084"), 0)
        Try
            m_IsConnected = False
            m_ReaderAPI.Connect()
            'MsgBox("Connect Succeed")

            m_IsConnected = True

            AddHandler Me.m_ReaderAPI.Events.ReadNotify, New ReadNotifyHandler(AddressOf Me.Events_ReadNotify)
            Me.m_ReaderAPI.Events.AttachTagDataWithReadEvent = False
            AddHandler Me.m_ReaderAPI.Events.StatusNotify, New StatusNotifyHandler(AddressOf Me.Events_StatusNotify)
            Me.m_ReaderAPI.Events.NotifyGPIEvent = True
            Me.m_ReaderAPI.Events.NotifyReaderDisconnectEvent = True
            Me.m_ReaderAPI.Events.NotifyAccessStartEvent = True
            Me.m_ReaderAPI.Events.NotifyAccessStopEvent = True
            Me.m_ReaderAPI.Events.NotifyInventoryStartEvent = True
            Me.m_ReaderAPI.Events.NotifyInventoryStopEvent = True

        Catch operationException As OperationFailureException
            MsgBox(operationException.Result)
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Sub RefreshRFID()
        If ReadStatus = False Then

            ReadStatus = True
            m_TagTable.Clear()
            cnt = 0
            DataGridView1.ColumnCount = 9
            DataGridView1.Columns(0).Name = "RFID Code"
            DataGridView1.Columns(1).Name = "Antenna"
            DataGridView1.Columns(2).Name = "PLU"
            DataGridView1.Columns(3).Name = "Description"
            DataGridView1.Columns(4).Name = "Status"
            DataGridView1.Columns(5).Name = "Bank"
            DataGridView1.Columns(6).Name = "Location"
            DataGridView1.Columns(7).Name = "Real EPC"
            DataGridView1.Columns(8).Name = "EPC2"

            DataGridView1.Columns(5).Visible = False
            DataGridView1.Columns(7).Visible = False
            DataGridView1.Columns(8).Visible = False
            DataGridView1.Columns(1).Visible = False

            DataGridView1.Columns(0).Width = "160"
            DataGridView1.Columns(1).Width = "30"
            DataGridView1.Columns(2).Width = "160"
            DataGridView1.Columns(3).Width = "380"
            DataGridView1.Columns(4).Width = "120"
            DataGridView1.Columns(6).Width = "130"
            If DataGridView1.Rows.Count > 0 Then
                DataGridView1.Rows.Clear()
            End If

            Dim chkBoxCol As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
            DataGridView1.Columns.Add(chkBoxCol)
            DataGridView1.Columns(9).Width = "40"




            Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.DeleteAll()
            If OneReadAll = True Then
                'Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.StopSequence()
            End If

            Dim op As New Operation
            op.AccessOperationCode = ACCESS_OPERATION_CODE.ACCESS_OPERATION_READ
            op.ReadAccessParams.MemoryBank = MEMORY_BANK.MEMORY_BANK_TID
            op.ReadAccessParams.ByteCount = 0
            op.ReadAccessParams.ByteOffset = 0
            op.ReadAccessParams.AccessPassword = 0
            Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.Add(op)


            OneReadAll = True
            Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.PerformSequence(Nothing, Nothing, Nothing)
        Else
            Try
                If (Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.Length > 0) Then
                    Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.StopSequence()
                Else
                    Me.m_ReaderAPI.Actions.Inventory.Stop()
                End If
            Catch ex As Exception

            End Try

            ReadStatus = False

            ReadStatus = True
            m_TagTable.Clear()
            cnt = 0
            DataGridView1.ColumnCount = 9
            DataGridView1.Columns(0).Name = "RFID Code"
            DataGridView1.Columns(1).Name = "Antenna"
            DataGridView1.Columns(2).Name = "PLU"
            DataGridView1.Columns(3).Name = "Description"
            DataGridView1.Columns(4).Name = "Status"
            DataGridView1.Columns(5).Name = "Bank"
            DataGridView1.Columns(6).Name = "Location"
            DataGridView1.Columns(7).Name = "Real EPC"
            DataGridView1.Columns(8).Name = "EPC2"

            DataGridView1.Columns(5).Visible = False
            DataGridView1.Columns(7).Visible = False
            DataGridView1.Columns(8).Visible = False
            DataGridView1.Columns(1).Visible = False

            DataGridView1.Columns(0).Width = "160"
            DataGridView1.Columns(1).Width = "30"
            DataGridView1.Columns(2).Width = "160"
            DataGridView1.Columns(3).Width = "380"
            DataGridView1.Columns(4).Width = "120"
            DataGridView1.Columns(6).Width = "130"
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



            Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.DeleteAll()
            If OneReadAll = True Then
                'Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.StopSequence()
            End If

            Dim op As New Operation
            op.AccessOperationCode = ACCESS_OPERATION_CODE.ACCESS_OPERATION_READ
            op.ReadAccessParams.MemoryBank = MEMORY_BANK.MEMORY_BANK_TID
            op.ReadAccessParams.ByteCount = 0
            op.ReadAccessParams.ByteOffset = 0
            op.ReadAccessParams.AccessPassword = 0
            Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.Add(op)


            OneReadAll = True
            Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.PerformSequence(Nothing, Nothing, Nothing)
        End If
    End Sub

    Private Delegate Sub UpdateStatus(ByVal eventData As StatusEventData)
    Private Delegate Sub UpdateRead(ByVal eventData As ReadEventData)

    Public Sub Events_StatusNotify(ByVal sender As Object, ByVal statusEventArgs As StatusEventArgs)
        Try
            MyBase.Invoke(Me.m_UpdateStatusHandler, New Object() {statusEventArgs.StatusEventData})
        Catch exception1 As Exception
        End Try
    End Sub

    Private Sub Events_ReadNotify(ByVal sender As Object, ByVal readEventArgs As ReadEventArgs)
        Try
            MyBase.Invoke(Me.m_UpdateReadHandler, New Object() {Nothing})
        Catch exception1 As Exception
        End Try
    End Sub

    Friend Class AccessOperationResult
        ' Fields
        Public m_OpCode As ACCESS_OPERATION_CODE = ACCESS_OPERATION_CODE.ACCESS_OPERATION_READ
        Public m_Result As RFIDResults = RFIDResults.RFID_NO_ACCESS_IN_PROGRESS
    End Class

    Private Sub myUpdateStatus(ByVal eventData As Events.StatusEventData)
        Select Case eventData.StatusEventType
            Case Symbol.RFID3.Events.STATUS_EVENT_TYPE.INVENTORY_START_EVENT
                functionCallStatusLabel.Text = "Inventory started"
                Exit Select
            Case Symbol.RFID3.Events.STATUS_EVENT_TYPE.INVENTORY_STOP_EVENT
                functionCallStatusLabel.Text = "Inventory stopped"
                'ReadOtherBank()
                Exit Select
            Case Symbol.RFID3.Events.STATUS_EVENT_TYPE.ACCESS_START_EVENT
                functionCallStatusLabel.Text = "Access Operation started"
                Exit Select
            Case Symbol.RFID3.Events.STATUS_EVENT_TYPE.ACCESS_STOP_EVENT
                functionCallStatusLabel.Text = "Access Operation stopped"
                Exit Select
            Case Symbol.RFID3.Events.STATUS_EVENT_TYPE.BUFFER_FULL_WARNING_EVENT
                functionCallStatusLabel.Text = " Buffer full warning"
                myUpdateRead(Nothing)
                Exit Select
            Case Symbol.RFID3.Events.STATUS_EVENT_TYPE.BUFFER_FULL_EVENT
                functionCallStatusLabel.Text = "Buffer full"
                myUpdateRead(Nothing)
                Exit Select
            Case Symbol.RFID3.Events.STATUS_EVENT_TYPE.DISCONNECTION_EVENT
                functionCallStatusLabel.Text = "Disconnection Event " & eventData.DisconnectionEventData.DisconnectEventInfo.ToString()
                Exit Select
            Case Symbol.RFID3.Events.STATUS_EVENT_TYPE.ANTENNA_EVENT
                functionCallStatusLabel.Text = "Antenna Status Update"
                Exit Select
            Case Symbol.RFID3.Events.STATUS_EVENT_TYPE.READER_EXCEPTION_EVENT
                functionCallStatusLabel.Text = "Reader ExceptionEvent " & eventData.ReaderExceptionEventData.ReaderExceptionEventInfo
                Exit Select
            Case Else
                Exit Select
        End Select
    End Sub

    Private Sub myUpdateRead(ByVal eventData As ReadEventData)
        Dim index As Integer = 0
        Dim isFoundIndex As Integer = 0
        Dim tagData As Symbol.RFID3.TagData() = m_ReaderAPI.Actions.GetReadTags(1000)
        Dim dsCek As New DataSet
        If tagData IsNot Nothing Then
            For nIndex As Integer = 0 To tagData.Length - 1
                If tagData(nIndex).OpCode = ACCESS_OPERATION_CODE.ACCESS_OPERATION_NONE OrElse (tagData(nIndex).OpCode = ACCESS_OPERATION_CODE.ACCESS_OPERATION_READ AndAlso tagData(nIndex).OpStatus = ACCESS_OPERATION_STATUS.ACCESS_SUCCESS) Then
                    Dim tag As Symbol.RFID3.TagData = tagData(nIndex)
                    Dim tagID As String = tag.TagID
                    Dim isFound As Boolean = False
                    Dim TIDTag As String = Microsoft.VisualBasic.Left(tag.MemoryBankData, 35)
                    SyncLock m_TagTable.SyncRoot
                        isFound = m_TagTable.ContainsKey(TIDTag)
                        If Not isFound Then
                            isFound = m_TagTable.ContainsKey(TIDTag)
                        End If
                    End SyncLock
                    If isFound Then
                        Me.m_ReaderAPI.Actions.Inventory.Stop()
                    Else
                        'If OnlyOne = True Then
                        '    MsgBox("There's Another RFID on The Scan !!!")
                        '    Exit Sub
                        'End If
                        dsCek.Clear()
                        dsCek = getSqldb3("select a.PLU,b.long_Description as Description,a.TID,a.flag,c.location_name from item_master_rfid a inner join item_master b on a.article_code = b.article_code " &
                                          " inner join location c on a.location = c.location where EPC = '" & HexToString(Microsoft.VisualBasic.Left(tag.TagID, 26)) & "' And Status = '1'")
                        Dim stt As String = "Available"
                        Dim PLU, Description, Loc As String
                        If dsCek.Tables(0).Rows.Count > 0 Then
                            PLU = dsCek.Tables(0).Rows(0).Item("PLU").ToString.Trim
                            Description = dsCek.Tables(0).Rows(0).Item("Description").ToString.Trim
                            Loc = dsCek.Tables(0).Rows(0).Item("location_name").ToString.Trim
                        Else
                            PLU = ""
                            Description = ""
                            Loc = ""
                        End If
                        DataGridView1.Rows.Add(1)
                        DataGridView1.Item(0, cnt).Value = HexToString(Microsoft.VisualBasic.Left(tag.TagID, 26))
                        DataGridView1.Item(1, cnt).Value = tag.AntennaID.ToString()
                        DataGridView1.Item(2, cnt).Value = PLU
                        DataGridView1.Item(3, cnt).Value = Description
                        DataGridView1.Item(7, cnt).Value = tag.TagID
                        DataGridView1.Columns(7).Visible = False
                        DataGridView1.Item(8, cnt).Value = HexToString(Microsoft.VisualBasic.Left(tag.TagID, 26))
                        Dim memoryBank As String = tag.MemoryBank.ToString()
                        index = memoryBank.LastIndexOf("_"c)
                        If index <> -1 Then
                            memoryBank = memoryBank.Substring(index + 1)
                        End If
                        If tag.MemoryBankData.Length > 0 Then
                            DataGridView1.Item(5, cnt).Value = tag.MemoryBankData
                            'DataGridView1.Item(6, cnt).Value = memoryBank
                        Else
                            DataGridView1.Item(5, cnt).Value = ""
                            'DataGridView1.Item(6, cnt).Value = ""
                        End If
                        DataGridView1.Item(6, cnt).Value = Loc
                        DataGridView1.Columns(5).Visible = False
                        DataGridView1.Columns(8).Visible = False
                        DataGridView1.Rows(cnt).Cells(4).Style.Font = New Font(DataGridView1.Font.Name, DataGridView1.Font.Size, FontStyle.Bold)
                        If dsCek.Tables(0).Rows.Count > 0 Then
                            If dsCek.Tables(0).Rows(0).Item("flag").ToString.Trim = "0" Then
                                DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.PaleGreen
                                stt = "Activated"
                            Else
                                DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.Khaki
                                stt = "Sold"
                            End If
                            If dsCek.Tables(0).Rows(0).Item("TID").ToString.Trim = "" Then
                                DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.White
                                stt = "Available"
                            Else
                                If Microsoft.VisualBasic.Left(dsCek.Tables(0).Rows(0).Item("TID").ToString.Trim, 35) <> Microsoft.VisualBasic.Left(tag.MemoryBankData, 35) Then
                                    DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.Salmon
                                    stt = "Duplicate"
                                End If
                            End If
                            DataGridView1.Item(2, cnt).ReadOnly = True
                        Else
                            DataGridView1.Item(2, cnt).ReadOnly = False

                        End If
                        My.Computer.Audio.Play(Application.StartupPath & "/Beep.wav", AudioPlayMode.Background)

                        If stt = "Available" Then
                            DataGridView1.Item(9, cnt).Value = False
                        Else
                            DataGridView1.Item(9, cnt).Value = True
                        End If
                        DataGridView1.Item(4, cnt).Value = stt

                        SyncLock m_TagTable.SyncRoot
                            m_TagTable.Add(TIDTag, DataGridView1.Item(5, cnt).Value)
                        End SyncLock
                        DataGridView1.Columns(1).Visible = False
                        cnt += 1
                        lblTotal.Text = "Total : " & CDec(cnt).ToString("N0")
                        Dim row As DataGridViewRow
                        Dim i As Integer = 0
                        For Each row In DataGridView1.Rows
                            DataGridView1.Rows(i).HeaderCell.Value = (1 + i).ToString
                            i += 1
                        Next
                    End If
                End If
            Next

        End If

    End Sub

    Function HexToString(ByVal hex As String) As String
        Dim text As New System.Text.StringBuilder(hex.Length \ 2)
        For i As Integer = 0 To hex.Length - 2 Step 2
            text.Append(Chr(Convert.ToByte(hex.Substring(i, 2), 16)))
        Next
        Return text.ToString
    End Function

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RefreshRFID()
    End Sub



    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Try
            If (Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.Length > 0) Then
                Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.StopSequence()
            Else
                Me.m_ReaderAPI.Actions.Inventory.Stop()
            End If
            Me.m_ReaderAPI.Disconnect()
            'Me.m_IsConnected = False
            'workEventArgs.Result = "Disconnect Succeed"
        Catch ofe As OperationFailureException
            'workEventArgs.Result = ofe.Result
        End Try
    End Sub

    Private Sub InventoryTransfer_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        If DataGridView1.RowCount > 0 Then
            'If MsgBox("Transfer Items To WAREHOUSE ??", MsgBoxStyle.YesNo, "Attention") = vbYes Then
            Dim dsCek As New DataSet
            For Each row As DataGridViewRow In DataGridView1.Rows
                'If row.Cells(9).Value = True Then
                getSqldb3("update Item_Master_RFID set Location = 0 where EPC = '" & row.Cells(0).Value.ToString.Trim & "' And TID = '" & row.Cells(5).Value.ToString.Trim & "'")
                getSqldb("Insert into Back_Office_Log values ('" & UserName & "','UPDATE LOCATION to WAREHOUSE','" & row.Cells(0).Value.ToString.Trim & "','Success','','" & Now & "')")
                'End If
            Next
            RefreshRFID()
            'MsgBox("Moved to Warehouse Successfully !!!", MsgBoxStyle.Information, "Information")
            'End If

        End If

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If DataGridView1.RowCount > 0 Then
            'If MsgBox("Transfer Items To Floor ??", MsgBoxStyle.YesNo, "Attention") = vbYes Then
            Dim dsCek As New DataSet
            For Each row As DataGridViewRow In DataGridView1.Rows
                ''If row.Cells(9).Value = True Then
                getSqldb3("update Item_Master_RFID set Location = 1 where EPC = '" & row.Cells(0).Value.ToString.Trim & "' And TID = '" & row.Cells(5).Value.ToString.Trim & "'")
                getSqldb("Insert into Back_Office_Log values ('" & UserName & "','UPDATE LOCATION to FLOOR','" & row.Cells(0).Value.ToString.Trim & "','Success','','" & Now & "')")
                'End If
            Next
            RefreshRFID()
            'MsgBox("Moved to Floor Successfully !!!", MsgBoxStyle.Information, "Information")
            'End If

        End If
    End Sub

    'Private Sub Button4_Click(sender As Object, e As EventArgs)
    '    Dim dsCek As New DataSet
    '    Dim index As Integer = 0
    '    Dim isFoundIndex As Integer = 0
    '    dsCek.Clear()
    '    dsCek = getSqldb3("select a.PLU,b.long_Description as Description,a.TID,a.flag,c.location_name from item_master_rfid a inner join item_master b on a.article_code = b.article_code " &
    '                      " inner join location c on a.location = c.location where EPC = 'D181100000179' And Status = '1'")
    '    Dim stt As String = "Available"
    '    Dim PLU, Description, Loc As String
    '    If dsCek.Tables(0).Rows.Count > 0 Then
    '        PLU = dsCek.Tables(0).Rows(0).Item("PLU").ToString.Trim
    '        Description = dsCek.Tables(0).Rows(0).Item("Description").ToString.Trim
    '        Loc = dsCek.Tables(0).Rows(0).Item("location_name").ToString.Trim
    '    Else
    '        PLU = ""
    '        Description = ""
    '        Loc = ""
    '    End If
    '    DataGridView1.Rows.Add(1)
    '    DataGridView1.Item(0, cnt).Value = "D181100000179"
    '    DataGridView1.Item(1, cnt).Value = "1"
    '    DataGridView1.Item(2, cnt).Value = PLU
    '    DataGridView1.Item(3, cnt).Value = Description
    '    DataGridView1.Item(7, cnt).Value = "D181100000179"
    '    DataGridView1.Columns(7).Visible = False
    '    DataGridView1.Item(8, cnt).Value = "D181100000179"


    '    DataGridView1.Item(5, cnt).Value = ""
    '    'DataGridView1.Item(6, cnt).Value = ""
    '    DataGridView1.Item(6, cnt).Value = Loc
    '    DataGridView1.Columns(5).Visible = False
    '    DataGridView1.Columns(8).Visible = False
    '    'DataGridView1.Rows(cnt).Cells(4).Style.Font = New Font(DataGridView1.Font.Name, DataGridView1.Font.Size, FontStyle.Bold)
    '    If dsCek.Tables(0).Rows.Count > 0 Then
    '        If dsCek.Tables(0).Rows(0).Item("flag").ToString.Trim = "0" Then
    '            DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.PaleGreen
    '            stt = "Activated"
    '        Else
    '            DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.Khaki
    '            stt = "Sold"
    '        End If
    '        If dsCek.Tables(0).Rows(0).Item("TID").ToString.Trim = "" Then
    '            DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.White
    '            stt = "Available"
    '        Else

    '        End If
    '        DataGridView1.Item(2, cnt).ReadOnly = True
    '    Else
    '        DataGridView1.Item(2, cnt).ReadOnly = False

    '    End If
    '    My.Computer.Audio.Play(Application.StartupPath & "/Beep.wav", AudioPlayMode.Background)

    '    If stt = "Available" Then
    '        DataGridView1.Item(9, cnt).Value = False
    '    Else
    '        DataGridView1.Item(9, cnt).Value = True
    '    End If
    '    DataGridView1.Item(4, cnt).Value = stt


    '    DataGridView1.Columns(1).Visible = False
    '    cnt += 1
    '    lblTotal.Text = "Total : " & CDec(cnt).ToString("N0")
    '    Dim row As DataGridViewRow
    '    Dim i As Integer = 0
    '    For Each row In DataGridView1.Rows
    '        DataGridView1.Rows(i).HeaderCell.Value = (1 + i).ToString
    '        i += 1
    '    Next
    'End Sub

    Private Sub InventoryTransfer_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        If e.KeyCode = Keys.F1 Then
            Button1_Click(sender, e)
        ElseIf e.KeyCode = Keys.F2 Then
            Button3_Click(sender, e)
        ElseIf e.KeyCode = Keys.F3 Then
            Button2_Click(sender, e)
        End If
    End Sub
End Class