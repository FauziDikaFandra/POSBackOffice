<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.SETIP = New System.Windows.Forms.Button()
        Me.GETIP = New System.Windows.Forms.Button()
        Me.txtIP = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.WEPC = New System.Windows.Forms.Button()
        Me.Connect = New System.Windows.Forms.Button()
        Me.Read = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.CheckBox1 = New System.Windows.Forms.CheckBox()
        Me.DataGridView1 = New System.Windows.Forms.DataGridView()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.SaveDB = New System.Windows.Forms.Button()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EDITToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CLEARALLToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
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
        Me.GroupBox1.Controls.Add(Me.GroupBox3)
        Me.GroupBox1.Controls.Add(Me.WEPC)
        Me.GroupBox1.Controls.Add(Me.Connect)
        Me.GroupBox1.Controls.Add(Me.Read)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(861, 73)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Set Up"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.SETIP)
        Me.GroupBox3.Controls.Add(Me.GETIP)
        Me.GroupBox3.Controls.Add(Me.txtIP)
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Location = New System.Drawing.Point(303, 10)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(538, 52)
        Me.GroupBox3.TabIndex = 37
        Me.GroupBox3.TabStop = False
        '
        'SETIP
        '
        Me.SETIP.Image = CType(resources.GetObject("SETIP.Image"), System.Drawing.Image)
        Me.SETIP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.SETIP.Location = New System.Drawing.Point(456, 15)
        Me.SETIP.Name = "SETIP"
        Me.SETIP.Size = New System.Drawing.Size(71, 31)
        Me.SETIP.TabIndex = 36
        Me.SETIP.Text = "Set IP"
        Me.SETIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.SETIP.UseVisualStyleBackColor = True
        '
        'GETIP
        '
        Me.GETIP.Image = CType(resources.GetObject("GETIP.Image"), System.Drawing.Image)
        Me.GETIP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.GETIP.Location = New System.Drawing.Point(379, 15)
        Me.GETIP.Name = "GETIP"
        Me.GETIP.Size = New System.Drawing.Size(71, 31)
        Me.GETIP.TabIndex = 35
        Me.GETIP.Text = "Get IP"
        Me.GETIP.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.GETIP.UseVisualStyleBackColor = True
        '
        'txtIP
        '
        Me.txtIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtIP.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtIP.Location = New System.Drawing.Point(50, 19)
        Me.txtIP.Name = "txtIP"
        Me.txtIP.Size = New System.Drawing.Size(126, 22)
        Me.txtIP.TabIndex = 30
        Me.txtIP.Text = "169.254.10.1:9090"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(29, 24)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(15, 14)
        Me.Label4.TabIndex = 31
        Me.Label4.Text = " :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(20, 14)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "IP"
        '
        'WEPC
        '
        Me.WEPC.Image = CType(resources.GetObject("WEPC.Image"), System.Drawing.Image)
        Me.WEPC.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.WEPC.Location = New System.Drawing.Point(182, 21)
        Me.WEPC.Name = "WEPC"
        Me.WEPC.Size = New System.Drawing.Size(94, 35)
        Me.WEPC.TabIndex = 33
        Me.WEPC.Text = "Write RFID"
        Me.WEPC.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.WEPC.UseVisualStyleBackColor = True
        '
        'Connect
        '
        Me.Connect.Image = CType(resources.GetObject("Connect.Image"), System.Drawing.Image)
        Me.Connect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Connect.Location = New System.Drawing.Point(15, 21)
        Me.Connect.Name = "Connect"
        Me.Connect.Size = New System.Drawing.Size(81, 35)
        Me.Connect.TabIndex = 35
        Me.Connect.Text = "Connect"
        Me.Connect.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Connect.UseVisualStyleBackColor = True
        '
        'Read
        '
        Me.Read.Image = CType(resources.GetObject("Read.Image"), System.Drawing.Image)
        Me.Read.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Read.Location = New System.Drawing.Point(102, 21)
        Me.Read.Name = "Read"
        Me.Read.Size = New System.Drawing.Size(74, 35)
        Me.Read.TabIndex = 28
        Me.Read.Text = "Read"
        Me.Read.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Read.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.CheckBox1)
        Me.GroupBox2.Controls.Add(Me.DataGridView1)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 90)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(851, 312)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "List"
        '
        'CheckBox1
        '
        Me.CheckBox1.AutoSize = True
        Me.CheckBox1.Location = New System.Drawing.Point(821, 26)
        Me.CheckBox1.Name = "CheckBox1"
        Me.CheckBox1.Size = New System.Drawing.Size(15, 14)
        Me.CheckBox1.TabIndex = 44
        Me.CheckBox1.UseVisualStyleBackColor = True
        '
        'DataGridView1
        '
        Me.DataGridView1.AllowUserToAddRows = False
        Me.DataGridView1.AllowUserToDeleteRows = False
        Me.DataGridView1.AllowUserToOrderColumns = True
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
        Me.DataGridView1.Size = New System.Drawing.Size(839, 285)
        Me.DataGridView1.TabIndex = 0
        '
        'Timer1
        '
        Me.Timer1.Interval = 1000
        '
        'SaveDB
        '
        Me.SaveDB.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SaveDB.Image = CType(resources.GetObject("SaveDB.Image"), System.Drawing.Image)
        Me.SaveDB.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.SaveDB.Location = New System.Drawing.Point(731, 408)
        Me.SaveDB.Name = "SaveDB"
        Me.SaveDB.Size = New System.Drawing.Size(132, 31)
        Me.SaveDB.TabIndex = 43
        Me.SaveDB.Text = "Save to Database"
        Me.SaveDB.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.SaveDB.UseVisualStyleBackColor = True
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
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ClientSize = New System.Drawing.Size(875, 449)
        Me.Controls.Add(Me.SaveDB)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Rapid Setting"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.DataGridView1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Read As Button
    Friend WithEvents DataGridView1 As DataGridView
    Friend WithEvents Timer1 As Timer
    Friend WithEvents Label4 As Label
    Friend WithEvents txtIP As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Connect As Button
    Friend WithEvents WUSER As Button
    Friend WithEvents WEPC As Button
    Private WithEvents SaveDB As Button
    Friend WithEvents SETIP As Button
    Friend WithEvents GETIP As Button
    Private WithEvents GroupBox3 As GroupBox
    Friend WithEvents CheckBox1 As CheckBox
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents EDITToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents CLEARALLToolStripMenuItem As ToolStripMenuItem
End Class
