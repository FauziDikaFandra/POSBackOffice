<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FingerRegistration
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(FingerRegistration))
        Me.btnCon = New System.Windows.Forms.Button()
        Me.btnReg = New System.Windows.Forms.Button()
        Me.txtuser = New System.Windows.Forms.TextBox()
        Me.antennaIDLlabel = New System.Windows.Forms.Label()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnDisc = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.picFPImg = New System.Windows.Forms.PictureBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.btnVerify = New System.Windows.Forms.Button()
        Me.txtInfo = New System.Windows.Forms.TextBox()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.picFPImg, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox4.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCon
        '
        Me.btnCon.Image = CType(resources.GetObject("btnCon.Image"), System.Drawing.Image)
        Me.btnCon.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnCon.Location = New System.Drawing.Point(6, 16)
        Me.btnCon.Name = "btnCon"
        Me.btnCon.Size = New System.Drawing.Size(95, 35)
        Me.btnCon.TabIndex = 33
        Me.btnCon.Text = "Connect"
        Me.btnCon.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnCon.UseVisualStyleBackColor = True
        '
        'btnReg
        '
        Me.btnReg.Image = CType(resources.GetObject("btnReg.Image"), System.Drawing.Image)
        Me.btnReg.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnReg.Location = New System.Drawing.Point(6, 15)
        Me.btnReg.Name = "btnReg"
        Me.btnReg.Size = New System.Drawing.Size(95, 35)
        Me.btnReg.TabIndex = 34
        Me.btnReg.Text = "Registration"
        Me.btnReg.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnReg.UseVisualStyleBackColor = True
        '
        'txtuser
        '
        Me.txtuser.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtuser.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtuser.Location = New System.Drawing.Point(97, 16)
        Me.txtuser.Name = "txtuser"
        Me.txtuser.Size = New System.Drawing.Size(98, 22)
        Me.txtuser.TabIndex = 38
        '
        'antennaIDLlabel
        '
        Me.antennaIDLlabel.AutoSize = True
        Me.antennaIDLlabel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.antennaIDLlabel.Location = New System.Drawing.Point(6, 18)
        Me.antennaIDLlabel.Name = "antennaIDLlabel"
        Me.antennaIDLlabel.Size = New System.Drawing.Size(51, 14)
        Me.antennaIDLlabel.TabIndex = 39
        Me.antennaIDLlabel.Text = "User ID"
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.antennaIDLlabel)
        Me.GroupBox1.Controls.Add(Me.txtuser)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 128)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(214, 49)
        Me.GroupBox1.TabIndex = 40
        Me.GroupBox1.TabStop = False
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnDisc)
        Me.GroupBox2.Controls.Add(Me.btnCon)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 2)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(214, 59)
        Me.GroupBox2.TabIndex = 41
        Me.GroupBox2.TabStop = False
        '
        'btnDisc
        '
        Me.btnDisc.Image = CType(resources.GetObject("btnDisc.Image"), System.Drawing.Image)
        Me.btnDisc.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnDisc.Location = New System.Drawing.Point(107, 16)
        Me.btnDisc.Name = "btnDisc"
        Me.btnDisc.Size = New System.Drawing.Size(98, 35)
        Me.btnDisc.TabIndex = 35
        Me.btnDisc.Text = "Disconnect"
        Me.btnDisc.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnDisc.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.picFPImg)
        Me.GroupBox3.Location = New System.Drawing.Point(246, 2)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(205, 256)
        Me.GroupBox3.TabIndex = 43
        Me.GroupBox3.TabStop = False
        '
        'picFPImg
        '
        Me.picFPImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.picFPImg.Location = New System.Drawing.Point(19, 21)
        Me.picFPImg.Name = "picFPImg"
        Me.picFPImg.Size = New System.Drawing.Size(165, 215)
        Me.picFPImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.picFPImg.TabIndex = 9
        Me.picFPImg.TabStop = False
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.btnVerify)
        Me.GroupBox4.Controls.Add(Me.btnReg)
        Me.GroupBox4.Location = New System.Drawing.Point(12, 63)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(214, 59)
        Me.GroupBox4.TabIndex = 42
        Me.GroupBox4.TabStop = False
        '
        'btnVerify
        '
        Me.btnVerify.Image = CType(resources.GetObject("btnVerify.Image"), System.Drawing.Image)
        Me.btnVerify.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.btnVerify.Location = New System.Drawing.Point(107, 15)
        Me.btnVerify.Name = "btnVerify"
        Me.btnVerify.Size = New System.Drawing.Size(98, 35)
        Me.btnVerify.TabIndex = 35
        Me.btnVerify.Text = "Verify"
        Me.btnVerify.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.btnVerify.UseVisualStyleBackColor = True
        '
        'txtInfo
        '
        Me.txtInfo.BackColor = System.Drawing.Color.White
        Me.txtInfo.Location = New System.Drawing.Point(12, 183)
        Me.txtInfo.Multiline = True
        Me.txtInfo.Name = "txtInfo"
        Me.txtInfo.ReadOnly = True
        Me.txtInfo.Size = New System.Drawing.Size(214, 75)
        Me.txtInfo.TabIndex = 44
        '
        'FingerRegistration
        '
        Me.AcceptButton = Me.btnCon
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.LightSteelBlue
        Me.ClientSize = New System.Drawing.Size(465, 275)
        Me.Controls.Add(Me.txtInfo)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Tahoma", 9.0!)
        Me.MaximumSize = New System.Drawing.Size(481, 313)
        Me.MinimumSize = New System.Drawing.Size(481, 313)
        Me.Name = "FingerRegistration"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Finger Registration"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        CType(Me.picFPImg, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox4.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnCon As Button
    Friend WithEvents btnReg As Button
    Friend WithEvents txtuser As TextBox
    Private WithEvents antennaIDLlabel As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Private WithEvents picFPImg As PictureBox
    Friend WithEvents btnDisc As Button
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents btnVerify As Button
    Private WithEvents txtInfo As TextBox
End Class
