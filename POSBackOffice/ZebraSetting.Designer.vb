<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class ZebraSetting
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ZebraSetting))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button5 = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txtIP = New System.Windows.Forms.TextBox()
        Me.transmitPower_CB = New System.Windows.Forms.ComboBox()
        Me.antennaConfigButton = New System.Windows.Forms.Button()
        Me.transmitPowerLabel = New System.Windows.Forms.Label()
        Me.antennaID_CB = New System.Windows.Forms.ComboBox()
        Me.antennaIDLlabel = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Connect = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.functionCallStatusLabel = New System.Windows.Forms.Label()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EDITToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CLEARALLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckBox2 = New System.Windows.Forms.CheckBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.CheckBox2)
        Me.GroupBox1.Controls.Add(Me.Button5)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.Connect)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(861, 73)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Set Up"
        '
        'Button5
        '
        Me.Button5.Image = CType(resources.GetObject("Button5.Image"), System.Drawing.Image)
        Me.Button5.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button5.Location = New System.Drawing.Point(182, 21)
        Me.Button5.Name = "Button5"
        Me.Button5.Size = New System.Drawing.Size(94, 35)
        Me.Button5.TabIndex = 42
        Me.Button5.Text = "Write RFID"
        Me.Button5.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button5.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.txtIP)
        Me.GroupBox3.Controls.Add(Me.transmitPower_CB)
        Me.GroupBox3.Controls.Add(Me.antennaConfigButton)
        Me.GroupBox3.Controls.Add(Me.transmitPowerLabel)
        Me.GroupBox3.Controls.Add(Me.antennaID_CB)
        Me.GroupBox3.Controls.Add(Me.antennaIDLlabel)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Location = New System.Drawing.Point(313, 11)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(538, 52)
        Me.GroupBox3.TabIndex = 35
        Me.GroupBox3.TabStop = False
        '
        'txtIP
        '
        Me.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIP.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIP.Location = New System.Drawing.Point(46, 18)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(98, 22)
        Me.txtIP.TabIndex = 37
        '
        'transmitPower_CB
        '
        Me.transmitPower_CB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.transmitPower_CB.FormattingEnabled = True
        Me.transmitPower_CB.Location = New System.Drawing.Point(342, 16)
        Me.transmitPower_CB.Name = "transmitPower_CB"
        Me.transmitPower_CB.Size = New System.Drawing.Size(60, 22)
        Me.transmitPower_CB.TabIndex = 3
        '
        'antennaConfigButton
        '
        Me.antennaConfigButton.Image = CType(resources.GetObject("antennaConfigButton.Image"), System.Drawing.Image)
        Me.antennaConfigButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.antennaConfigButton.Location = New System.Drawing.Point(426, 11)
        Me.antennaConfigButton.Name = "antennaConfigButton"
        Me.antennaConfigButton.Size = New System.Drawing.Size(106, 35)
        Me.antennaConfigButton.TabIndex = 36
        Me.antennaConfigButton.Text = "Apply Reader"
        Me.antennaConfigButton.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.antennaConfigButton.UseVisualStyleBackColor = True
        '
        'transmitPowerLabel
        '
        Me.transmitPowerLabel.AutoSize = True
        Me.transmitPowerLabel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.transmitPowerLabel.Location = New System.Drawing.Point(290, 21)
        Me.transmitPowerLabel.Name = "transmitPowerLabel"
        Me.transmitPowerLabel.Size = New System.Drawing.Size(46, 14)
        Me.transmitPowerLabel.TabIndex = 4
        Me.transmitPowerLabel.Text = "Power"
        '
        'antennaID_CB
        '
        Me.antennaID_CB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.antennaID_CB.FormattingEnabled = True
        Me.antennaID_CB.Location = New System.Drawing.Point(224, 17)
        Me.antennaID_CB.Name = "antennaID_CB"
        Me.antennaID_CB.Size = New System.Drawing.Size(49, 22)
        Me.antennaID_CB.TabIndex = 34
        '
        'antennaIDLlabel
        '
        Me.antennaIDLlabel.AutoSize = True
        Me.antennaIDLlabel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.antennaIDLlabel.Location = New System.Drawing.Point(158, 21)
        Me.antennaIDLlabel.Name = "antennaIDLlabel"
        Me.antennaIDLlabel.Size = New System.Drawing.Size(60, 14)
        Me.antennaIDLlabel.TabIndex = 33
        Me.antennaIDLlabel.Text = "Antenna"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(13, 21)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(20, 14)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "IP"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(30, 21)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(15, 14)
        Me.Label4.TabIndex = 31
        Me.Label4.Text = " :"
        '
        'Connect
        '
        Me.Connect.Image = CType(resources.GetObject("Connect.Image"), System.Drawing.Image)
        Me.Connect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Connect.Location = New System.Drawing.Point(15, 21)
        Me.Connect.Name = "Connect"
        Me.Connect.Size = New System.Drawing.Size(81, 35)
        Me.Connect.TabIndex = 32
        Me.Connect.Text = "Connect"
        Me.Connect.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Connect.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(102, 21)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(74, 35)
        Me.Button1.TabIndex = 28
        Me.Button1.Text = "Read"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.CheckBox1)
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 91)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(861, 323)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "List"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(821, 26)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox1.TabIndex = 1
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowDrop = True
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToResizeColumns = False
        Me.DataGridView1.AllowUserToResizeRows = False
        Me.DataGridView1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView1.BackgroundColor = System.Drawing.Color.White
        Me.DataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.DataGridView1.Location = New System.Drawing.Point(6, 21)
        Me.DataGridView1.Name = "DataGridView1"
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(849, 296)
        Me.DataGridView1.TabIndex = 0
        '
        'functionCallStatusLabel
        '
        Me.functionCallStatusLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.functionCallStatusLabel.AutoSize = True
        Me.functionCallStatusLabel.Location = New System.Drawing.Point(9, 426)
        Me.functionCallStatusLabel.Name = "functionCallStatusLabel"
        Me.functionCallStatusLabel.Size = New System.Drawing.Size(79, 14)
        Me.functionCallStatusLabel.TabIndex = 40
        Me.functionCallStatusLabel.Text = "Connected!!!"
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Image = CType(resources.GetObject("Button4.Image"), System.Drawing.Image)
        Me.Button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button4.Location = New System.Drawing.Point(736, 417)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(132, 31)
        Me.Button4.TabIndex = 42
        Me.Button4.Text = "Save to Database"
        Me.Button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button4.UseVisualStyleBackColor = True
        '
        'BackgroundWorker1
        '
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EDITToolStripMenuItem, Me.CLEARALLToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(133, 48)
        '
        'EDITToolStripMenuItem
        '
        Me.EDITToolStripMenuItem.Image = CType(resources.GetObject("EDITToolStripMenuItem.Image"), System.Drawing.Image)
        Me.EDITToolStripMenuItem.Name = "EDITToolStripMenuItem"
        Me.EDITToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.EDITToolStripMenuItem.Text = "EDIT PLU"
        '
        'CLEARALLToolStripMenuItem
        '
        Me.CLEARALLToolStripMenuItem.Image = CType(resources.GetObject("CLEARALLToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CLEARALLToolStripMenuItem.Name = "CLEARALLToolStripMenuItem"
        Me.CLEARALLToolStripMenuItem.Size = New System.Drawing.Size(132, 22)
        Me.CLEARALLToolStripMenuItem.Text = "CLEAR ALL"
        '
        'CheckBox2
        '
        Me.CheckBox2.AutoSize = True
        Me.CheckBox2.Location = New System.Drawing.Point(286, 35)
        Me.CheckBox2.Name = "CheckBox2"
        Me.CheckBox2.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox2.TabIndex = 43
        Me.CheckBox2.UseVisualStyleBackColor = True
        '
        'ZebraSetting
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ClientSize = New System.Drawing.Size(875, 449)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.functionCallStatusLabel)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "ZebraSetting"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Zebra Setting"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Connect As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Timer1 As Timer
    Private WithEvents antennaConfigButton As Button
    Friend WithEvents antennaID_CB As ComboBox
    Private WithEvents antennaIDLlabel As Label
    Private WithEvents GroupBox3 As GroupBox
    Friend WithEvents transmitPower_CB As ComboBox
    Private WithEvents transmitPowerLabel As Label
    Friend WithEvents functionCallStatusLabel As Label
    Private WithEvents Button4 As Button
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents Button5 As Button
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents EDITToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CLEARALLToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents txtIP As TextBox
    Friend WithEvents CheckBox2 As CheckBox
End Class
