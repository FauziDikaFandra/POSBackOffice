Imports Symbol.RFID3
Imports Symbol.RFID3.Events
Imports Symbol.RFID3.TagAccess
Imports Symbol.RFID3.TagAccess.Sequence
Imports System.Globalization
Public Class ZebraSetting
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

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub Connect_Click(sender As Object, e As EventArgs) Handles Connect.Click
        m_IsConnected = False
        CheckCon()
        OneReadAll = False
        Set_Antenna()
        If m_IsConnected = False Then
            Button1.Enabled = False
            Button5.Enabled = False
        Else
            Button1.Enabled = True
            Button5.Enabled = True
        End If
    End Sub

    Sub Set_Antenna()
        Try
            antID = m_ReaderAPI.Config.Antennas.AvailableAntennas
            rxValues = m_ReaderAPI.ReaderCapabilities.ReceiveSensitivityValues
            txValues = m_ReaderAPI.ReaderCapabilities.TransmitPowerLevelValues

            If (antID.Length > 0) Then
                Dim id As UInt16
                For Each id In antID
                    Me.antennaID_CB.Items.Add(id)
                Next
                Me.antennaID_CB.SelectedIndex = 0
                Dim antConfig As Antennas.Config = m_ReaderAPI.Config.Antennas.Item(antID(Me.antennaID_CB.SelectedIndex)).GetConfig
                Dim tx As Integer
                For Each tx In txValues
                    Me.transmitPower_CB.Items.Add(tx)
                Next
                Me.transmitPower_CB.SelectedIndex = antConfig.TransmitPowerIndex
                tLoad = True
            End If
        Catch ex As Exception
            functionCallStatusLabel.Text = ex.Message
        End Try
    End Sub



    Private Sub ZebraSetting_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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


        m_IsConnected = False
        If m_IsConnected = False Then
            Button1.Enabled = False
            Button5.Enabled = False
        End If
        txtIP.Text = IPReader
        CheckBox1.Visible = False
    End Sub


    Sub CheckCon()

        m_ReaderAPI = New RFIDReader(txtIP.Text, UInt32.Parse("5084"), 0)
        Try
            m_IsConnected = False
            m_ReaderAPI.Connect()
            MsgBox("Connect Succeed")
            Connect.Enabled = False
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



    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Button1.Text = "Read" Then
            CheckBox1.Visible = False
            CheckBox1.Checked = False
            Button1.Text = "Stop"
            m_TagTable.Clear()
            cnt = 0
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

            Button1.Text = "Stop"
            OneReadAll = True
            Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.PerformSequence(Nothing, Nothing, Nothing)
        Else
            If (Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.Length > 0) Then
                Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.StopSequence()
            Else
                Me.m_ReaderAPI.Actions.Inventory.Stop()
            End If
            Me.Button1.Text = "Read"
        End If

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) 
        Try
            If Button1.Text = "Read" Then
                MsgBox("Please Stop Reading First !!!")
                Exit Sub
            End If
            Dim len As String = InputBox("Lenght", "Input Here", "0")
            Dim NewData As String = InputBox("New User", "Input Here", "0")
            Dim length As UInt16 = UInt16.Parse(len)
            If (DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value.ToString.Trim.Length = 0) Then
                MsgBox("No TagID is defined")
            ElseIf ((length * 2) > DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value.ToString.Trim.Length) Then
                MsgBox("Data length has to be a word size (2X)")
            ElseIf ((length Mod 2) <> 0) Then
                MsgBox("Data length has to be a even number")
            Else
                Me.m_WriteParams.AccessPassword = 0
                Me.m_WriteParams.MemoryBank = MEMORY_BANK.MEMORY_BANK_USER
                Me.m_WriteParams.ByteOffset = UInt16.Parse(0)
                Me.m_WriteParams.WriteDataLength = length
                Dim writeData As Byte() = New Byte(Me.m_WriteParams.WriteDataLength - 1) {}
                Dim index As Integer = 0
                Do While (index < Me.m_WriteParams.WriteDataLength)
                    writeData(index) = Byte.Parse(NewData.Substring((index * 2), 2), NumberStyles.HexNumber)
                    writeData((index + 1)) = Byte.Parse(NewData.Substring(((index + 1) * 2), 2), NumberStyles.HexNumber)
                    index = (index + 2)
                Loop
                Me.m_WriteParams.WriteData = writeData
                Me.m_AccessOpResult.m_OpCode = ACCESS_OPERATION_CODE.ACCESS_OPERATION_WRITE
                Me.m_AccessOpResult.m_Result = RFIDResults.RFID_API_SUCCESS
                Me.m_ReaderAPI.Actions.TagAccess.WriteWait(DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value.ToString.Trim, m_WriteParams, Nothing)
                MsgBox("Write Succeed")
                Button1_Click(sender, e)

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) 
        Try
            If Button1.Text = "Read" Then
                MsgBox("Please Stop Reading First !!!")
                Exit Sub
            End If
            Dim len As String = InputBox("Lenght", "Input Here", "0")
            Dim NewData As String = InputBox("New Reserved", "Input Here", "0")
            Dim length As UInt16 = UInt16.Parse(len)
            If (DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value.ToString.Trim.Length = 0) Then
                MsgBox("No TagID is defined")
            ElseIf ((length * 2) > DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value.ToString.Trim.Length) Then
                MsgBox("Data length has to be a word size (2X)")
            ElseIf ((length Mod 2) <> 0) Then
                MsgBox("Data length has to be a even number")
            Else
                Me.m_WriteParams.AccessPassword = 0
                Me.m_WriteParams.MemoryBank = MEMORY_BANK.MEMORY_BANK_RESERVED
                Me.m_WriteParams.ByteOffset = UInt16.Parse(0)
                Me.m_WriteParams.WriteDataLength = length
                Dim writeData As Byte() = New Byte(Me.m_WriteParams.WriteDataLength - 1) {}
                Dim index As Integer = 0
                Do While (index < Me.m_WriteParams.WriteDataLength)
                    writeData(index) = Byte.Parse(NewData.Substring((index * 2), 2), NumberStyles.HexNumber)
                    writeData((index + 1)) = Byte.Parse(NewData.Substring(((index + 1) * 2), 2), NumberStyles.HexNumber)
                    index = (index + 2)
                Loop
                Me.m_WriteParams.WriteData = writeData
                Me.m_AccessOpResult.m_OpCode = ACCESS_OPERATION_CODE.ACCESS_OPERATION_WRITE
                Me.m_AccessOpResult.m_Result = RFIDResults.RFID_API_SUCCESS
                Me.m_ReaderAPI.Actions.TagAccess.WriteWait(DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value.ToString.Trim, m_WriteParams, Nothing)
                MsgBox("Write Succeed")
                Button1_Click(sender, e)

            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
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



    Private Sub antennaConfigButton_Click(sender As Object, e As EventArgs) Handles antennaConfigButton.Click
        Try

            Dim antID As UInt16() = m_ReaderAPI.Config.Antennas.AvailableAntennas
            Dim antConfig As Antennas.Config = m_ReaderAPI.Config.Antennas.Item(antID(Me.antennaID_CB.SelectedIndex)).GetConfig
            antConfig.TransmitPowerIndex = Me.transmitPower_CB.SelectedIndex

            m_ReaderAPI.Config.Antennas.Item(antID(Me.antennaID_CB.SelectedIndex)).SetConfig(antConfig)
            functionCallStatusLabel.Text = "Set Antenna Configuration Successfully"
            MsgBox("Success !!!")
        Catch iue As InvalidUsageException
            functionCallStatusLabel.Text = iue.VendorMessage
        Catch ofe As OperationFailureException
            functionCallStatusLabel.Text = ofe.StatusDescription
        Catch ex As Exception
            functionCallStatusLabel.Text = ex.Message
        End Try
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
                    'tag id zebra 
                    'Dim TIDTag As String = Microsoft.VisualBasic.Left(tag.MemoryBankData, 35)
                    'tag id AM+RFID
                    Dim TIDTag As String = Microsoft.VisualBasic.Left(tag.MemoryBankData, 28)
                    SyncLock m_TagTable.SyncRoot
                        isFound = m_TagTable.ContainsKey(TIDTag)
                        If Not isFound Then
                            'tagID += (CUInt(tag.MemoryBank) + tag.MemoryBankDataOffset)
                            isFound = m_TagTable.ContainsKey(TIDTag)

                        End If
                    End SyncLock
                    If isFound Then
                        'Dim count As UInteger = 0
                        'For x As Integer = 0 To DataGridView1.Rows.Count - 1
                        '    If DataGridView1.Item(0, x).Value = tagID Then
                        '        isFoundIndex = x
                        '        Exit For
                        '    End If
                        'Next
                        'Try
                        '    count = UInteger.Parse(DataGridView1.Item(2, isFoundIndex).Value) + tagData(nIndex).TagSeenCount
                        'Catch fe As FormatException
                        '    functionCallStatusLabel.Text = fe.Message
                        '    Exit Try
                        'End Try
                        ''DataGridView1.Item(1, cnt).Value = tag.AntennaID.ToString()
                        'DataGridView1.Item(2, cnt).Value = count.ToString()
                        ''DataGridView1.Item(3, cnt).Value = tag.PeakRSSI.ToString()

                        'Dim memoryBank As String = tag.MemoryBank.ToString()
                        'index = memoryBank.LastIndexOf("_"c)
                        'If index <> -1 Then
                        '    memoryBank = memoryBank.Substring(index + 1)
                        'End If
                        'If tag.MemoryBankData.Length > 0 AndAlso Not memoryBank.Equals(DataGridView1.Item(5, cnt).Value) Then
                        '    DataGridView1.Item(5, cnt).Value = tag.MemoryBankData
                        '    DataGridView1.Item(6, cnt).Value = memoryBank
                        '    DataGridView1.Item(7, cnt).Value = tag.MemoryBankDataOffset.ToString()

                        '    SyncLock m_TagTable.SyncRoot
                        '        m_TagTable.Remove(tagID)
                        '        m_TagTable.Add(tag.TagID + tag.MemoryBank.ToString() + tag.MemoryBankDataOffset.ToString(), DataGridView1.Item(0, cnt).Value)
                        '    End SyncLock
                        'End If
                        'If (Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.Length > 0) Then
                        '    Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.StopSequence()
                        'End If
                        Me.m_ReaderAPI.Actions.Inventory.Stop()

                        'Me.Button1.Text = "Start Reading"

                        'Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.StopSequence()

                    Else
                        dsCek.Clear()
                        dsCek = getSqldb3("select a.PLU,b.long_Description as Description,a.TID,a.flag from item_master_rfid a inner join item_master b on a.article_code = b.article_code " &
                                          " where EPC = '" & HexToString(Microsoft.VisualBasic.Left(tag.TagID, 26)) & "' And Status = '0'")
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
                        DataGridView1.Item(0, cnt).Value = HexToString(Microsoft.VisualBasic.Left(tag.TagID, 26))
                        DataGridView1.Item(1, cnt).Value = tag.AntennaID.ToString()
                        DataGridView1.Item(2, cnt).Value = PLU
                        'DataGridView1.Item(2, cnt).Value = tag.TagSeenCount.ToString()
                        DataGridView1.Item(3, cnt).Value = Description
                        'DataGridView1.Item(3, cnt).Value = tag.PeakRSSI.ToString()
                        'DataGridView1.Item(4, cnt).Value = tag.PC.ToString("X")
                        DataGridView1.Item(7, cnt).Value = tag.TagID
                        DataGridView1.Columns(7).Visible = False
                        DataGridView1.Item(8, cnt).Value = HexToString(Microsoft.VisualBasic.Left(tag.TagID, 26))
                        Dim memoryBank As String = tag.MemoryBank.ToString()
                        index = memoryBank.LastIndexOf("_"c)
                        If index <> -1 Then
                            memoryBank = memoryBank.Substring(index + 1)
                        End If
                        If tag.MemoryBankData.Length > 0 Then
                            DataGridView1.Item(5, cnt).Value = Microsoft.VisualBasic.Left(tag.MemoryBankData, 35)
                            DataGridView1.Item(6, cnt).Value = memoryBank
                        Else
                            DataGridView1.Item(5, cnt).Value = ""
                            DataGridView1.Item(6, cnt).Value = ""
                        End If
                        DataGridView1.Columns(5).Visible = False
                        DataGridView1.Columns(6).Visible = False
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
                                If dsCek.Tables(0).Rows(0).Item("TID").ToString.Trim <> tag.MemoryBankData Then
                                    DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.Salmon
                                    stt = "Duplicate"
                                End If
                            End If
                            DataGridView1.Item(2, cnt).ReadOnly = True
                        Else
                            DataGridView1.Item(2, cnt).ReadOnly = False
                        End If

                        DataGridView1.Item(4, cnt).Value = stt
                        CheckBox1.Visible = True
                        SyncLock m_TagTable.SyncRoot
                            m_TagTable.Add(TIDTag, DataGridView1.Item(5, cnt).Value)
                        End SyncLock
                        cnt += 1
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
        ds = getSqldb3("Select Convert(INT,SUBSTRING(EPC,6,8)) + 1 As Urut from Item_Master_RFID where SUBSTRING(EPC,1,5) = '" & Cek & "' and status = '0' order by Convert(INT,SUBSTRING(EPC,6,8)) desc")

        If ds.Tables(0).Rows.Count > 0 Then
            Cek = Cek & "00000000".Substring(0, 8 - Len(ds.Tables(0).Rows(0).Item("urut").ToString)) & ds.Tables(0).Rows(0).Item("urut")
        Else
            Cek = Cek & "00000001"
        End If
        Return Cek
    End Function

    Sub AddChar(ByVal epc As String)
        For x As Integer = 1 To 16
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

    Private Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            If Button1.Text <> "Read" Then
                MsgBox("Please Stop Reading First !!!")
                Exit Sub
            End If
            'Dim len As String = InputBox("Lenght", "Input Here", "0")
            Dim NewData As String = InputBox("New EPC", "Input Here", "0")
            'Dim length As UInt16 = UInt16.Parse(len)
            If NewData.Trim = "0" Or NewData.Trim = "" Then
                Exit Sub
            End If
            If (DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value.ToString.Trim.Length = 0) Then
                MsgBox("No TagID is defined")
                'ElseIf ((length * 2) > DataGridView1.Item(0, DataGridView1.CurrentRow.Index).Value.ToString.Trim.Length) Then
                '    MsgBox("Data length has to be a word size (2X)")
                'ElseIf ((length Mod 2) <> 0) Then
                '    MsgBox("Data length has to be a even number")
            Else
                Dim lp As Integer = 3
                If CheckBox2.Checked = False Then
                    lp = 1
                End If
                NewData = NewData.ToString.Trim & "000"
                For x As Integer = 1 To lp
                    Try
                        Me.m_AntennaInfoForm = m_AntennaInfoForm
                        Me.m_WriteParams.AccessPassword = 0
                        Me.m_WriteParams.MemoryBank = MEMORY_BANK.MEMORY_BANK_EPC
                        If x = 2 Then
                            Me.m_WriteParams.ByteOffset = 4
                        Else
                            Me.m_WriteParams.ByteOffset = 2
                        End If

                        Me.m_WriteParams.WriteDataLength = 16
                        Dim writeData As Byte() = New Byte(Me.m_WriteParams.WriteDataLength - 1) {}
                        Dim index As Integer = 0
                        epcstr = ""
                        AddChar(NewData)
                        Do While (index < Me.m_WriteParams.WriteDataLength)
                            writeData(index) = Byte.Parse(epcstr.Substring((index * 2), 2), NumberStyles.HexNumber)
                            writeData((index + 1)) = Byte.Parse(epcstr.Substring(((index + 1) * 2), 2), NumberStyles.HexNumber)
                            index = (index + 2)
                        Loop
                        Me.m_WriteParams.WriteData = writeData
                        Me.m_AccessOpResult.m_OpCode = ACCESS_OPERATION_CODE.ACCESS_OPERATION_WRITE
                        Me.m_AccessOpResult.m_Result = RFIDResults.RFID_API_SUCCESS
                        Me.m_ReaderAPI.Actions.TagAccess.WriteWait(DataGridView1.Item(7, DataGridView1.CurrentRow.Index).Value.ToString.Trim, m_WriteParams, Nothing)

                    Catch ex As Exception

                    End Try

                Next



                'Me.m_ReaderAPI.Actions.TagAccess.WriteWait(epcstr, m_WriteParams, Nothing)
                'Me.m_ReaderAPI.Actions.TagAccess.WriteEvent(m_WriteParams, Nothing, Nothing)
                getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Write RFID ZEBRA','" & DataGridView1(5, DataGridView1.CurrentRow.Index).Value & "','Success','','" & Now & "')")
                MsgBox("Write Succeed")
                Button1_Click(sender, e)
                'Masalah disini selalu kurang 1 angka setelah di write, solusinya, 
                'pertama ByteOffset 4 exec, lalu ByteOffset 2 exec, dan lanjutkan ByteOffset = 4 lagi 
                'diakali dengan cara looping di atas
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub ZebraSetting_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub antennaID_CB_SelectedIndexChanged(sender As Object, e As EventArgs) Handles antennaID_CB.SelectedIndexChanged
        If tLoad = True Then
            Dim antConfig As Antennas.Config = m_ReaderAPI.Config.Antennas.Item(antID(Me.antennaID_CB.SelectedIndex)).GetConfig
            Me.transmitPower_CB.SelectedIndex = antConfig.TransmitPowerIndex
        End If

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        If Button1.Text <> "Read" Then
            'MsgBox("Please STOP Reading first !!!")
            'Exit Sub
            If (Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.Length > 0) Then
                Me.m_ReaderAPI.Actions.TagAccess.OperationSequence.StopSequence()
            Else
                Me.m_ReaderAPI.Actions.Inventory.Stop()
            End If
            Me.Button1.Text = "Read"
        End If
        If DataGridView1.RowCount > 0 Then
            Dim dsCek As New DataSet
            For Each row As DataGridViewRow In DataGridView1.Rows
                If row.Cells(9).Value = True Then
                    If row.Cells(0).Value.ToString.Trim = "1000000000000" And row.Cells(2).Value.ToString.Trim = "" Then
                        getSqldb3("update Item_Master_RFID set status = 1 where EPC = '" & row.Cells(8).Value.ToString.Trim & "' And TID = '" & row.Cells(5).Value.ToString.Trim & "'")
                        epcstr = ""
                        AddChar("1000000000000000")


                        Me.m_AntennaInfoForm = m_AntennaInfoForm
                        Me.m_WriteParams.AccessPassword = 0
                        Me.m_WriteParams.MemoryBank = MEMORY_BANK.MEMORY_BANK_EPC
                        Me.m_WriteParams.ByteOffset = 4
                        Me.m_WriteParams.WriteDataLength = 16
                        Dim writeData As Byte() = New Byte(Me.m_WriteParams.WriteDataLength - 1) {}
                        Dim index As Integer = 0

                        Do While (index < Me.m_WriteParams.WriteDataLength)
                            writeData(index) = Byte.Parse(epcstr.Substring((index * 2), 2), NumberStyles.HexNumber)
                            writeData((index + 1)) = Byte.Parse(epcstr.Substring(((index + 1) * 2), 2), NumberStyles.HexNumber)
                            index = (index + 2)
                        Loop
                        Me.m_WriteParams.WriteData = writeData
                        Me.m_AccessOpResult.m_OpCode = ACCESS_OPERATION_CODE.ACCESS_OPERATION_WRITE
                        Me.m_AccessOpResult.m_Result = RFIDResults.RFID_API_SUCCESS
                        Me.m_ReaderAPI.Actions.TagAccess.WriteWait(DataGridView1.Item(7, DataGridView1.CurrentRow.Index).Value.ToString.Trim, m_WriteParams, Nothing)
                        getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Clear RFID ZEBRA','" & DataGridView1(5, DataGridView1.CurrentRow.Index).Value & "','Success','','" & Now & "')")
                        'Me.m_ReaderAPI.Actions.TagAccess.WriteEvent(m_WriteParams, Nothing, Nothing)
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
                                    getSqldb3("update Item_Master_RFID set TID = '" & row.Cells(5).Value.ToString.Trim & "' where EPC = '" & row.Cells(0).Value.ToString.Trim & "'")
                                    GoTo 1
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
                                    getSqldb3("Insert  into Item_Master_RFID select distinct article_code,PLU,'" & NewEPC & "','" & row.Cells(5).Value.ToString.Trim & "','','','',0,0,0,getdate() from item_master where plu = '" & row.Cells(2).Value.ToString.Trim & "'")
                                    NewEPC = NewEPC.ToString.Trim & "000"
                                    Me.m_AntennaInfoForm = m_AntennaInfoForm
                                    Me.m_WriteParams.AccessPassword = 0
                                    Me.m_WriteParams.MemoryBank = MEMORY_BANK.MEMORY_BANK_EPC
                                    Me.m_WriteParams.ByteOffset = 4
                                    Me.m_WriteParams.WriteDataLength = 16
                                    Dim writeData As Byte() = New Byte(Me.m_WriteParams.WriteDataLength - 1) {}
                                    Dim index As Integer = 0
                                    epcstr = ""
                                    AddChar(NewEPC)
                                    Do While (index < Me.m_WriteParams.WriteDataLength)
                                        writeData(index) = Byte.Parse(epcstr.Substring((index * 2), 2), NumberStyles.HexNumber)
                                        writeData((index + 1)) = Byte.Parse(epcstr.Substring(((index + 1) * 2), 2), NumberStyles.HexNumber)
                                        index = (index + 2)
                                    Loop
                                    Me.m_WriteParams.WriteData = writeData
                                    Me.m_AccessOpResult.m_OpCode = ACCESS_OPERATION_CODE.ACCESS_OPERATION_WRITE
                                    Me.m_AccessOpResult.m_Result = RFIDResults.RFID_API_SUCCESS
                                    Me.m_ReaderAPI.Actions.TagAccess.WriteWait(DataGridView1.Item(7, DataGridView1.CurrentRow.Index).Value.ToString.Trim, m_WriteParams, Nothing)
                                    getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Write Update RFID ZEBRA','" & DataGridView1(5, DataGridView1.CurrentRow.Index).Value & "','Success','','" & Now & "')")
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
                                getSqldb3("Insert  into Item_Master_RFID select distinct article_code,PLU,'" & NewEPC & "','" & row.Cells(5).Value.ToString.Trim & "','','','',0,0,0,getdate() from item_master where plu = '" & row.Cells(2).Value.ToString.Trim & "'")
                                NewEPC = NewEPC.ToString.Trim & "000"
                                Me.m_AntennaInfoForm = m_AntennaInfoForm
                                Me.m_WriteParams.AccessPassword = 0
                                Me.m_WriteParams.MemoryBank = MEMORY_BANK.MEMORY_BANK_EPC
                                Me.m_WriteParams.ByteOffset = 4
                                Me.m_WriteParams.WriteDataLength = 16
                                Dim writeData As Byte() = New Byte(Me.m_WriteParams.WriteDataLength - 1) {}
                                Dim index As Integer = 0
                                epcstr = ""
                                AddChar(NewEPC)
                                Do While (index < Me.m_WriteParams.WriteDataLength)
                                    writeData(index) = Byte.Parse(epcstr.Substring((index * 2), 2), NumberStyles.HexNumber)
                                    writeData((index + 1)) = Byte.Parse(epcstr.Substring(((index + 1) * 2), 2), NumberStyles.HexNumber)
                                    index = (index + 2)
                                Loop
                                Me.m_WriteParams.WriteData = writeData
                                Me.m_AccessOpResult.m_OpCode = ACCESS_OPERATION_CODE.ACCESS_OPERATION_WRITE
                                Me.m_AccessOpResult.m_Result = RFIDResults.RFID_API_SUCCESS
                                Me.m_ReaderAPI.Actions.TagAccess.WriteWait(DataGridView1.Item(7, DataGridView1.CurrentRow.Index).Value.ToString.Trim, m_WriteParams, Nothing)
                                'Me.m_ReaderAPI.Actions.TagAccess.WriteEvent(m_WriteParams, Nothing, Nothing)
                                getSqldb("Insert into Back_Office_Log values ('" & UserName & "','Write NEW RFID ZEBRA','" & DataGridView1(5, DataGridView1.CurrentRow.Index).Value & "','Success','','" & Now & "')")
                            End If


                        End If
                    End If

                End If
2:
            Next
            'Button1_Click(sender, e)
            Button1_Click(sender, e)
            DataGridView1.Columns(5).Visible = False
            MsgBox("Success!!!")
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

    Private Sub ContextMenuStrip1_Click(sender As Object, e As EventArgs)
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

    Private Sub ZebraSetting_ForeColorChanged(sender As Object, e As EventArgs) Handles Me.ForeColorChanged

    End Sub
End Class