Imports Symbol.RFID3
Imports Symbol.RFID3.Events
Imports Symbol.RFID3.TagAccess
Imports Symbol.RFID3.TagAccess.Sequence
Imports System.Text
Imports System.IO
Public Class Stock_Opname
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
    Dim ds As New DataSet
    Dim ReadStatus As Boolean = False
    Dim Rwind As Integer
    Private Sub Stock_Opname_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
                RefreshHasil()
            End If
        End If

        With CheckedListBox1
            ds = getSqldb("Select distinct Brand from item_master order by brand")
            For Each ro As DataRow In ds.Tables(0).Rows
                .Items.Add(ro("Brand"))
            Next
            .SelectedIndex = 0

        End With

        ComboBox2.SelectedIndex = 0
        RadioButton1.Checked = True
        TextBox1.Focus()
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
        Dim dsCek, dsCek2 As New DataSet
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
                        dsCek = getSqldb3("select a.PLU,b.long_Description as Description,a.TID,a.flag,c.location_name,b.brand from item_master_rfid a inner join item_master b on a.article_code = b.article_code " &
                                          " inner join location c on a.location = c.location where EPC = '" & HexToString(Microsoft.VisualBasic.Left(tag.TagID, 26)) & "' And Status = '1'")
                        Dim stt As String = "Available"
                        Dim PLU, Description, Loc, brand As String
                        If dsCek.Tables(0).Rows.Count > 0 Then

                            PLU = dsCek.Tables(0).Rows(0).Item("PLU").ToString.Trim
                            Description = dsCek.Tables(0).Rows(0).Item("Description").ToString.Trim
                            Loc = dsCek.Tables(0).Rows(0).Item("location_name").ToString.Trim
                            brand = dsCek.Tables(0).Rows(0).Item("brand").ToString.Trim
                        Else
                            PLU = ""
                            Description = ""
                            Loc = ""
                            brand = "Not Found"
                        End If
                        DataGridView1.Rows.Add(1)
                        DataGridView1.Item(0, cnt).Value = HexToString(Microsoft.VisualBasic.Left(tag.TagID, 26))
                        DataGridView1.Item(1, cnt).Value = tag.AntennaID.ToString()
                        DataGridView1.Item(2, cnt).Value = PLU
                        DataGridView1.Item(3, cnt).Value = Description
                        DataGridView1.Item(7, cnt).Value = tag.TagID
                        DataGridView1.Columns(7).Visible = False
                        DataGridView1.Item(8, cnt).Value = HexToString(Microsoft.VisualBasic.Left(tag.TagID, 26))
                        DataGridView1.Item(9, cnt).Value = brand
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
                                'DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.WhiteSmoke
                                stt = "Activated"
                            Else
                                'DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.Khaki
                                stt = "Sold"
                            End If
                            If dsCek.Tables(0).Rows(0).Item("TID").ToString.Trim = "" Then
                                'DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.White
                                stt = "Available"
                            Else
                                If Microsoft.VisualBasic.Left(dsCek.Tables(0).Rows(0).Item("TID").ToString.Trim, 35) <> Microsoft.VisualBasic.Left(tag.MemoryBankData, 35) Then
                                    'DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.Salmon
                                    stt = "Duplicate"
                                End If
                            End If
                            DataGridView1.Item(2, cnt).ReadOnly = True

                            Dim beda As Boolean = True
                            For j As Integer = 0 To CheckedListBox1.CheckedItems.Count - 1
                                If CheckedListBox1.CheckedItems(0).ToString.Trim = dsCek.Tables(0).Rows(0).Item("brand").ToString.Trim Then
                                    beda = False
                                End If
                            Next
                            If beda = True Then
                                DataGridView1.Rows(cnt).DefaultCellStyle.BackColor = Color.Salmon
                            End If

                        Else
                            DataGridView1.Item(2, cnt).ReadOnly = False

                        End If
                        My.Computer.Audio.Play(Application.StartupPath & "/Beep.wav", AudioPlayMode.Background)

                        'If stt = "Available" Then
                        '    DataGridView1.Item(9, cnt).Value = False
                        'Else
                        '    DataGridView1.Item(9, cnt).Value = True
                        'End If
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



    Sub RefreshRFID()
        If ReadStatus = False Then

            ReadStatus = True
            m_TagTable.Clear()
            cnt = 0
            DataGridView1.ColumnCount = 10
            DataGridView1.Columns(0).Name = "RFID Code"
            DataGridView1.Columns(1).Name = "Antenna"
            DataGridView1.Columns(2).Name = "PLU"
            DataGridView1.Columns(3).Name = "Description"
            DataGridView1.Columns(4).Name = "Status"
            DataGridView1.Columns(5).Name = "Bank"
            DataGridView1.Columns(6).Name = "Location"
            DataGridView1.Columns(7).Name = "Real EPC"
            DataGridView1.Columns(8).Name = "EPC2"
            DataGridView1.Columns(9).Name = "Brand"

            DataGridView1.Columns(5).Visible = False
            DataGridView1.Columns(7).Visible = False
            DataGridView1.Columns(8).Visible = False
            DataGridView1.Columns(1).Visible = False

            DataGridView1.Columns(0).Width = "120"
            DataGridView1.Columns(1).Width = "30"
            DataGridView1.Columns(2).Width = "130"
            DataGridView1.Columns(3).Width = "270"
            DataGridView1.Columns(4).Width = "100"
            DataGridView1.Columns(6).Width = "100"
            DataGridView1.Columns(9).Width = "120"
            If DataGridView1.Rows.Count > 0 Then
                DataGridView1.Rows.Clear()
            End If

            Dim chkBoxCol As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
            'DataGridView1.Columns.Add(chkBoxCol)
            'DataGridView1.Columns(9).Width = "40"




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
            DataGridView1.ColumnCount = 10
            DataGridView1.Columns(0).Name = "RFID Code"
            DataGridView1.Columns(1).Name = "Antenna"
            DataGridView1.Columns(2).Name = "PLU"
            DataGridView1.Columns(3).Name = "Description"
            DataGridView1.Columns(4).Name = "Status"
            DataGridView1.Columns(5).Name = "Bank"
            DataGridView1.Columns(6).Name = "Location"
            DataGridView1.Columns(7).Name = "Real EPC"
            DataGridView1.Columns(8).Name = "EPC2"
            DataGridView1.Columns(9).Name = "Brand"

            DataGridView1.Columns(5).Visible = False
            DataGridView1.Columns(7).Visible = False
            DataGridView1.Columns(8).Visible = False
            DataGridView1.Columns(1).Visible = False

            DataGridView1.Columns(0).Width = "120"
            DataGridView1.Columns(1).Width = "30"
            DataGridView1.Columns(2).Width = "130"
            DataGridView1.Columns(3).Width = "270"
            DataGridView1.Columns(4).Width = "100"
            DataGridView1.Columns(6).Width = "100"
            DataGridView1.Columns(9).Width = "120"
            If DataGridView1.Rows.Count > 0 Then
                DataGridView1.Rows.Clear()
            End If

            Dim chkBoxCol As DataGridViewCheckBoxColumn = New DataGridViewCheckBoxColumn()
            'DataGridView1.Columns.Add(chkBoxCol)
            'DataGridView1.Columns(9).Width = "40"
            'DataGridView1.Columns(9).ReadOnly = False
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

    'Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
    '    If TextBox1.Text = "" Then
    '        MsgBox("Isi Sub Location Terlebih Dahulu !!!")
    '        TextBox1.Focus()
    '        Exit Sub
    '    End If
    '    Dim dsHasil As New DataSet
    '    For Each row As DataGridViewRow In DataGridView1.Rows
    '        dsHasil = getSqldb("select * from SO_HasilScan where EPC = '" & row.Cells.Item("EPC").Value & "'")
    '        If dsHasil.Tables(0).Rows.Count > 0 Then
    '            MsgBox("EPC '" & row.Cells.Item("EPC").Value & "' Sudah Ada di Loc " & dsHasil.Tables(0).Rows(0).Item("Location") & " and Sub " & dsHasil.Tables(0).Rows(0).Item("Sub_Location") & "!!")
    '            Exit Sub
    '        End If
    '    Next
    '    For Each row As DataGridViewRow In DataGridView1.Rows
    '        getSqldb("Insert into SO_HasilScan values ('" & ComboBox2.Text & "','" & TextBox1.Text & "','" & row.Cells.Item("EPC").Value & "','" & row.Cells.Item("PLU").Value & "','" & row.Cells.Item("Description").Value & "','" & row.Cells.Item("Status").Value & "','" & row.Cells.Item("Location").Value & "','" & row.Cells.Item("Brand").Value & "')")
    '    Next

    '    MsgBox("Successfull !!!")
    '    RefreshRFID()
    '    RefreshHasil()
    '    ComboBox1.Enabled = False
    '    ComboBox2.Enabled = False
    '    TextBox1.Enabled = False
    '    Button4.Enabled = False
    'End Sub

    Sub RefreshHasil()
        Dim dsHasil As New DataSet
        DataGridView2.DataSource = Nothing
        If RadioButton1.Checked = True Then
            dsHasil = getSqldb2("select Location,Sub_Location,Count(EPC) Total from SO_HasilScan group by Location,Sub_Location Order By Location,Sub_Location")
            If dsHasil.Tables(0).Rows.Count > 0 Then
                DataGridView2.DataSource = dsHasil.Tables(0)
                DataGridView2.Refresh()
            End If
        Else
            dsHasil = getSqldb2("select Class,a.Brand,Count(EPC) Total from SO_HasilScan a inner join item_master b on a.plu = b.plu " &
                                " group by Class,a.Brand Order By Class,a.Brand")
            If dsHasil.Tables(0).Rows.Count > 0 Then
                DataGridView2.DataSource = dsHasil.Tables(0)
                DataGridView2.Refresh()
            End If
        End If

    End Sub

    Private Sub Button5_Click(sender As Object, e As EventArgs)
        ComboBox2.Enabled = True
        TextBox1.Enabled = True
        Button3.Enabled = True
    End Sub



    Private Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Dim checked As Boolean
        If CheckBox1.Checked = True Then
            checked = True
        Else
            checked = False
        End If

        For i As Integer = 0 To CheckedListBox1.Items.Count - 1
            CheckedListBox1.SetItemChecked(i, checked)
        Next
    End Sub

    Private Sub InventoryTransfer_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.WorkerSupportsCancellation = True
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        RefreshRFID()
    End Sub

    Private Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            DataGridView1.DataSource = Nothing
            DataGridView1.Refresh()
            Dim dsHasil As New DataSet
            If RadioButton1.Checked Then
                dsHasil = getSqldb2("select * from SO_HasilScan where Location = '" & DataGridView2.Item(0, DataGridView2.CurrentRow.Index).Value & "' and Sub_Location = '" & DataGridView2.Item(1, DataGridView2.CurrentRow.Index).Value & "'")
            Else
                dsHasil = getSqldb2("select * from SO_HasilScan where Brand = '" & DataGridView2.Item(2, DataGridView2.CurrentRow.Index).Value & "'")
            End If

            If dsHasil.Tables(0).Rows.Count > 0 Then
                cnt = 0
                For Each ro As DataRow In dsHasil.Tables(0).Rows
                    DataGridView1.Rows.Add(1)
                    DataGridView1.Item(0, cnt).Value = ro("EPC")
                    DataGridView1.Item(1, cnt).Value = 1
                    DataGridView1.Item(2, cnt).Value = ro("PLU")
                    DataGridView1.Item(3, cnt).Value = ro("Description")
                    DataGridView1.Item(4, cnt).Value = ro("Status")
                    DataGridView1.Item(7, cnt).Value = ro("EPC")
                    DataGridView1.Columns(7).Visible = False
                    DataGridView1.Item(8, cnt).Value = ro("EPC")
                    DataGridView1.Item(9, cnt).Value = ro("Brand")
                    DataGridView1.Item(5, cnt).Value = ""
                    DataGridView1.Item(6, cnt).Value = ro("RFID_Location")
                    DataGridView1.Columns(5).Visible = False
                    DataGridView1.Columns(8).Visible = False
                    DataGridView1.Rows(cnt).Cells(4).Style.Font = New Font(DataGridView1.Font.Name, DataGridView1.Font.Size, FontStyle.Bold)

                    'If ro("Status") = "Activated" Then
                    '    DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.WhiteSmoke
                    'End If
                    'If ro("Status") = "Sold" Then
                    '    DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.Khaki
                    'End If
                    'If ro("Status") = "Available" Then
                    '    DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.White
                    'End If

                    'If ro("Status") = "Duplicate" Then
                    '    DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.Salmon
                    'End If
                    cnt += 1
                Next
                lblTotal.Text = "Total : " & CDec(cnt).ToString("N0")
                Dim row As DataGridViewRow
                Dim i As Integer = 0
                For Each row In DataGridView1.Rows
                    DataGridView1.Rows(i).HeaderCell.Value = (1 + i).ToString
                    i += 1
                Next
            End If
        Catch ex As Exception

        End Try


    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        If MsgBox("Apakah anda Yakin ??", MsgBoxStyle.YesNo, "Information") = MsgBoxResult.Yes Then
            getSqldb2("Delete from SO_HasilScan where Location = '" & DataGridView2.Item(0, DataGridView2.CurrentRow.Index).Value & "' and Sub_Location = '" & DataGridView2.Item(1, DataGridView2.CurrentRow.Index).Value & "'")
            MsgBox("Successfull !!!")
        End If
        RefreshHasil()
    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim dsCek As New DataSet
        If ComboBox2.Text = "" Then
            MsgBox("Pilih Location !!!")
            Exit Sub
        End If
        'If TextBox1.Text = "" Then
        '    TextBox1.Focus()
        '    MsgBox("Ketik Sub Location !!!")
        '    Exit Sub
        'End If
        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            dsCek.Clear()
            dsCek = getSqldb2("select * from SO_HasilScan where EPC = '" & DataGridView1.Rows(i).Cells(0).Value.ToString & "'")
            If dsCek.Tables(0).Rows.Count > 0 Then
                MsgBox("EPC dengan Kode " & DataGridView1.Rows(i).Cells(0).Value.ToString & " Sudah ada diLokasi " & dsCek.Tables(0).Rows(0).Item("Location") & " / " & dsCek.Tables(0).Rows(0).Item("Sub_Location"))
                Exit Sub
            End If
        Next

        For i As Integer = 0 To DataGridView1.Rows.Count - 1
            getSqldb2("Insert Into SO_HasilScan Values ('" & ComboBox2.Text & "','" & TextBox1.Text & "','" & DataGridView1.Rows(i).Cells(0).Value.ToString & "','" & DataGridView1.Rows(i).Cells(2).Value.ToString & "'," &
                "'" & DataGridView1.Rows(i).Cells(3).Value.ToString & "','" & DataGridView1.Rows(i).Cells(4).Value.ToString & "','" & DataGridView1.Rows(i).Cells(6).Value.ToString & "','" & DataGridView1.Rows(i).Cells(9).Value.ToString & "')")
        Next

        MsgBox("Successfull !!!")
        RefreshRFID()
        RefreshHasil()

    End Sub

    Private Sub Button5_Click_1(sender As Object, e As EventArgs) Handles Button5.Click
        Dim dsCek As New DataSet
        dsCek = getSqldb2("select * from SO_HasilScan")
        If dsCek.Tables(0).Rows.Count > 0 Then
            Savetxt()
        Else
            MsgBox("Data tidak ditemukan !!!")
        End If
    End Sub

    Private Sub ComboBox2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox2.SelectedIndexChanged
        TextBox1.Focus()
    End Sub

    Sub Savetxt()
        Dim line As String = ""
        Dim path As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\test.txt"
        If File.Exists(path) Then
            File.Delete(path)
        End If

        Dim sw As New IO.StreamWriter(path)
        SaveFileDialog1.Filter = "csv Files (*.csv*)|*.csv"
        Dim dsHasil As New DataSet
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            dsHasil = getSqldb2("Select Location + '_' + Sub_Location as Loc,PLU,Count(EPC) Total from SO_HasilScan group by Location + '_' + Sub_Location,PLU Order By Location + '_' + Sub_Location")
            If dsHasil.Tables(0).Rows.Count > 0 Then
                For Each ro As DataRow In dsHasil.Tables(0).Rows
                    sw.Write(ro("Loc") & "," & ro("PLU") & "," & ro("Total"))
                    sw.Write(vbNewLine)
                Next
            End If
            sw.Close()
            sw.Dispose()
            If File.Exists(SaveFileDialog1.FileName) Then
                File.Delete(SaveFileDialog1.FileName)
            End If
            File.Copy(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\test.txt", SaveFileDialog1.FileName)
            MsgBox("Successfull !!!")
            Process.Start(SaveFileDialog1.FileName)
        End If

    End Sub

    Sub Savetxt2()
        Dim line As String = ""
        Dim path As String = My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\test.txt"
        If File.Exists(path) Then
            File.Delete(path)
        End If

        Dim sw As New IO.StreamWriter(path)
        SaveFileDialog1.Filter = "txt Files (*.txt*)|*.txt"
        SaveFileDialog1.Title = "Save Data"
        Dim dsHasil As New DataSet
        If SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then
            If (SaveFileDialog1.FileName = "") Then
                Exit Sub
            End If

            If RadioButton1.Checked = True Then

                    dsHasil = getSqldb2("select Location,Sub_Location,Count(EPC) Total from SO_HasilScan where Location = '" & DataGridView2.Item(0, Rwind).Value & "' and Sub_Location = '" & DataGridView2.Item(1, Rwind).Value & "'  group by Location,Sub_Location Order By Location,Sub_Location")
                    If dsHasil.Tables(0).Rows.Count > 0 Then
                        For Each ro As DataRow In dsHasil.Tables(0).Rows
                            sw.Write(ro("Location") & "," & ro("Sub_Location") & "," & ro("Total"))
                            sw.Write(vbNewLine)
                        Next
                    End If
                Else
                    dsHasil = getSqldb2("select Class,a.Brand,Count(EPC) Total from SO_HasilScan a inner join item_master b on a.plu = b.plu " &
                                    "  where Brand = '" & DataGridView2.Item(2, Rwind).Value & "' group by Class,a.Brand Order By Class,a.Brand")
                    If dsHasil.Tables(0).Rows.Count > 0 Then
                        For Each ro As DataRow In dsHasil.Tables(0).Rows
                            sw.Write(ro("Class") & "," & ro("Brand") & "," & ro("Total"))
                            sw.Write(vbNewLine)
                        Next
                    End If
                End If

                sw.Close()
                sw.Dispose()
                If File.Exists(SaveFileDialog1.FileName) Then
                    File.Delete(SaveFileDialog1.FileName)
                End If
                File.Copy(My.Computer.FileSystem.SpecialDirectories.MyDocuments & "\test.txt", SaveFileDialog1.FileName)
                MsgBox("Successfull !!!")
                Process.Start(SaveFileDialog1.FileName)
            End If

    End Sub

    Private Sub RadioButton1_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton1.CheckedChanged
        RefreshHasil()
    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim dsCek As New DataSet

        If IsDBNull(DataGridView2.CurrentRow.Index) Then
            Rwind = 0
        Else
            Rwind = DataGridView2.CurrentRow.Index
        End If
        If RadioButton1.Checked = False Then
            dsCek = getSqldb2("select * from SO_HasilScan where brand = '" & DataGridView2.Rows(Rwind).Cells(0).Value.ToString & "'")
        Else
            dsCek = getSqldb2("select * from SO_HasilScan where Location = '" & DataGridView2.Rows(Rwind).Cells(0).Value.ToString & "' and Sub_Location  = '" & DataGridView2.Rows(Rwind).Cells(1).Value.ToString & "'")
        End If

        If dsCek.Tables(0).Rows.Count > 0 Then
            Savetxt2()
        Else
            MsgBox("Data tidak ditemukan !!!")
        End If
    End Sub

    Private Sub RadioButton2_CheckedChanged(sender As Object, e As EventArgs) Handles RadioButton2.CheckedChanged
        RefreshHasil()
    End Sub

    Private Sub DataGridView2_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView2.CellDoubleClick
        Try
            DataGridView1.DataSource = Nothing
            DataGridView1.Refresh()
            Dim dsHasil As New DataSet
            If RadioButton1.Checked Then
                dsHasil = getSqldb2("select * from SO_HasilScan where Location = '" & DataGridView2.Item(0, e.RowIndex).Value & "' and Sub_Location = '" & DataGridView2.Item(1, e.RowIndex).Value & "'")
            Else
                dsHasil = getSqldb2("select * from SO_HasilScan where Brand = '" & DataGridView2.Item(2, e.RowIndex).Value & "'")
            End If

            If dsHasil.Tables(0).Rows.Count > 0 Then
                cnt = 0
                For Each ro As DataRow In dsHasil.Tables(0).Rows
                    DataGridView1.Rows.Add(1)
                    DataGridView1.Item(0, cnt).Value = ro("EPC")
                    DataGridView1.Item(1, cnt).Value = 1
                    DataGridView1.Item(2, cnt).Value = ro("PLU")
                    DataGridView1.Item(3, cnt).Value = ro("Description")
                    DataGridView1.Item(4, cnt).Value = ro("Status")
                    DataGridView1.Item(7, cnt).Value = ro("EPC")
                    DataGridView1.Columns(7).Visible = False
                    DataGridView1.Item(8, cnt).Value = ro("EPC")
                    DataGridView1.Item(9, cnt).Value = ro("Brand")
                    DataGridView1.Item(5, cnt).Value = ""
                    DataGridView1.Item(6, cnt).Value = ro("RFID_Location")
                    DataGridView1.Columns(5).Visible = False
                    DataGridView1.Columns(8).Visible = False
                    DataGridView1.Rows(cnt).Cells(4).Style.Font = New Font(DataGridView1.Font.Name, DataGridView1.Font.Size, FontStyle.Bold)

                    'If ro("Status") = "Activated" Then
                    '    DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.WhiteSmoke
                    'End If
                    'If ro("Status") = "Sold" Then
                    '    DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.Khaki
                    'End If
                    'If ro("Status") = "Available" Then
                    '    DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.White
                    'End If

                    'If ro("Status") = "Duplicate" Then
                    '    DataGridView1.Rows(cnt).Cells(4).Style.BackColor = Color.Salmon
                    'End If
                    cnt += 1
                Next
                lblTotal.Text = "Total : " & CDec(cnt).ToString("N0")
                Dim row As DataGridViewRow
                Dim i As Integer = 0
                For Each row In DataGridView1.Rows
                    DataGridView1.Rows(i).HeaderCell.Value = (1 + i).ToString
                    i += 1
                Next
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class