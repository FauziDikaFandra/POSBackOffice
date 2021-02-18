<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Activation_Zebra
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Activation_Zebra))
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.txtIP = New System.Windows.Forms.TextBox()
        Me.transmitPower_CB = New System.Windows.Forms.ComboBox()
        Me.antennaConfigButton = New System.Windows.Forms.Button()
        Me.transmitPowerLabel = New System.Windows.Forms.Label()
        Me.antennaID_CB = New System.Windows.Forms.ComboBox()
        Me.antennaIDLlabel = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EDITToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CLEARALLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.functionCallStatusLabel = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Connect = New System.Windows.Forms.Button()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button4 = New System.Windows.Forms.Button()
        Me.lblTotal = New System.Windows.Forms.Label()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.GroupBox3.SuspendLayout()
        Me.ContextMenuStrip1.SuspendLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
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
        Me.GroupBox3.Location = New System.Drawing.Point(287, 11)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(564, 52)
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
        Me.txtIP.Text = "192.168.8.36"
        '
        'transmitPower_CB
        '
        Me.transmitPower_CB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.transmitPower_CB.FormattingEnabled = True
        Me.transmitPower_CB.Location = New System.Drawing.Point(342, 17)
        Me.transmitPower_CB.Name = "transmitPower_CB"
        Me.transmitPower_CB.Size = New System.Drawing.Size(60, 21)
        Me.transmitPower_CB.TabIndex = 3
        '
        'antennaConfigButton
        '
        Me.antennaConfigButton.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.antennaConfigButton.Image = CType(resources.GetObject("antennaConfigButton.Image"), System.Drawing.Image)
        Me.antennaConfigButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.antennaConfigButton.Location = New System.Drawing.Point(419, 11)
        Me.antennaConfigButton.Name = "antennaConfigButton"
        Me.antennaConfigButton.Size = New System.Drawing.Size(139, 35)
        Me.antennaConfigButton.TabIndex = 36
        Me.antennaConfigButton.Text = "Apply Reader  [F3]"
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
        Me.antennaID_CB.Size = New System.Drawing.Size(49, 21)
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
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EDITToolStripMenuItem, Me.CLEARALLToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(123, 48)
        '
        'EDITToolStripMenuItem
        '
        Me.EDITToolStripMenuItem.Image = CType(resources.GetObject("EDITToolStripMenuItem.Image"), System.Drawing.Image)
        Me.EDITToolStripMenuItem.Name = "EDITToolStripMenuItem"
        Me.EDITToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.EDITToolStripMenuItem.Text = "EDIT PLU"
        '
        'CLEARALLToolStripMenuItem
        '
        Me.CLEARALLToolStripMenuItem.Image = CType(resources.GetObject("CLEARALLToolStripMenuItem.Image"), System.Drawing.Image)
        Me.CLEARALLToolStripMenuItem.Name = "CLEARALLToolStripMenuItem"
        Me.CLEARALLToolStripMenuItem.Size = New System.Drawing.Size(122, 22)
        Me.CLEARALLToolStripMenuItem.Text = "RESET"
        '
        'BackgroundWorker1
        '
        '
        'functionCallStatusLabel
        '
        Me.functionCallStatusLabel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.functionCallStatusLabel.AutoSize = True
        Me.functionCallStatusLabel.Location = New System.Drawing.Point(91, 422)
        Me.functionCallStatusLabel.Name = "functionCallStatusLabel"
        Me.functionCallStatusLabel.Size = New System.Drawing.Size(68, 13)
        Me.functionCallStatusLabel.TabIndex = 45
        Me.functionCallStatusLabel.Text = "Connected!!!"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(844, 25)
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
        Me.DataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader
        Me.DataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView1.Size = New System.Drawing.Size(869, 296)
        Me.DataGridView1.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.CheckBox1)
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Location = New System.Drawing.Point(8, 85)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(881, 323)
        Me.GroupBox2.TabIndex = 44
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "List"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.Connect)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Location = New System.Drawing.Point(8, 6)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(881, 73)
        Me.GroupBox1.TabIndex = 43
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Set Up"
        '
        'Connect
        '
        Me.Connect.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Connect.Image = CType(resources.GetObject("Connect.Image"), System.Drawing.Image)
        Me.Connect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Connect.Location = New System.Drawing.Point(15, 21)
        Me.Connect.Name = "Connect"
        Me.Connect.Size = New System.Drawing.Size(112, 35)
        Me.Connect.TabIndex = 32
        Me.Connect.Text = "Connect  [F1]"
        Me.Connect.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Connect.UseVisualStyleBackColor = True
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Image = CType(resources.GetObject("Button1.Image"), System.Drawing.Image)
        Me.Button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button1.Location = New System.Drawing.Point(133, 22)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(107, 35)
        Me.Button1.TabIndex = 28
        Me.Button1.Text = "Refresh  [F2]"
        Me.Button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button2.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button2.Image = CType(resources.GetObject("Button2.Image"), System.Drawing.Image)
        Me.Button2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button2.Location = New System.Drawing.Point(684, 411)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(101, 35)
        Me.Button2.TabIndex = 47
        Me.Button2.Text = "Reset  [F5]"
        Me.Button2.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button4
        '
        Me.Button4.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button4.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button4.Image = CType(resources.GetObject("Button4.Image"), System.Drawing.Image)
        Me.Button4.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button4.Location = New System.Drawing.Point(791, 411)
        Me.Button4.Name = "Button4"
        Me.Button4.Size = New System.Drawing.Size(93, 35)
        Me.Button4.TabIndex = 46
        Me.Button4.Text = "Save  [F4]"
        Me.Button4.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button4.UseVisualStyleBackColor = True
        '
        'lblTotal
        '
        Me.lblTotal.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lblTotal.AutoSize = True
        Me.lblTotal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotal.Location = New System.Drawing.Point(12, 421)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(50, 14)
        Me.lblTotal.TabIndex = 55
        Me.lblTotal.Text = "Total : "
        '
        'Button3
        '
        Me.Button3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button3.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button3.Image = CType(resources.GetObject("Button3.Image"), System.Drawing.Image)
        Me.Button3.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Button3.Location = New System.Drawing.Point(519, 411)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(159, 35)
        Me.Button3.TabIndex = 56
        Me.Button3.Text = "Compare to Sales [F6]"
        Me.Button3.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button3.UseVisualStyleBackColor = True
        '
        'Activation_Zebra
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ClientSize = New System.Drawing.Size(895, 449)
        Me.Controls.Add(Me.Button3)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button4)
        Me.Controls.Add(Me.functionCallStatusLabel)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.KeyPreview = True
        Me.Name = "Activation_Zebra"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Activation_Zebra"
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ContextMenuStrip1.ResumeLayout(False)
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents CLEARALLToolStripMenuItem As ToolStripMenuItem
    Private WithEvents GroupBox3 As GroupBox
    Friend WithEvents txtIP As TextBox
    Friend WithEvents transmitPower_CB As ComboBox
    Private WithEvents antennaConfigButton As Button
    Private WithEvents transmitPowerLabel As Label
    Friend WithEvents antennaID_CB As ComboBox
    Private WithEvents antennaIDLlabel As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents EDITToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Private WithEvents Button4 As Button
    Friend WithEvents functionCallStatusLabel As Label
    Friend WithEvents Timer1 As Timer
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Connect As Button
    Friend WithEvents Button1 As Button
    Friend WithEvents GroupBox1 As GroupBox
    Private WithEvents Button2 As Button
    Friend WithEvents lblTotal As Label
    Private WithEvents Button3 As Button
End Class
