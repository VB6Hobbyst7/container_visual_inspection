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
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.llSetToTop = New System.Windows.Forms.LinkLabel()
        Me.chkShowCaptured = New System.Windows.Forms.CheckBox()
        Me.btnCapture1 = New System.Windows.Forms.Button()
        Me.lblCapture = New System.Windows.Forms.Label()
        Me.btnAuto1 = New System.Windows.Forms.Button()
        Me.AxVLCPlugin21 = New AxAXVLC.AxVLCPlugin2()
        Me.btnLive1 = New System.Windows.Forms.Button()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnCenter = New System.Windows.Forms.Button()
        Me.FlowLayoutPanel2 = New System.Windows.Forms.FlowLayoutPanel()
        Me.btnCapture2 = New System.Windows.Forms.Button()
        Me.lblCount2 = New System.Windows.Forms.Label()
        Me.AxVLCPlugin22 = New AxAXVLC.AxVLCPlugin2()
        Me.btnLive2 = New System.Windows.Forms.Button()
        Me.lblSensor = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        CType(Me.AxVLCPlugin21, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        CType(Me.AxVLCPlugin22, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.FlowLayoutPanel1.AutoScroll = True
        Me.FlowLayoutPanel1.ImeMode = System.Windows.Forms.ImeMode.Hiragana
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(7, 559)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(873, 120)
        Me.FlowLayoutPanel1.TabIndex = 7
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.ErrorImage = Nothing
        Me.PictureBox1.Location = New System.Drawing.Point(317, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(563, 556)
        Me.PictureBox1.TabIndex = 8
        Me.PictureBox1.TabStop = False
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.lblSensor)
        Me.GroupBox1.Controls.Add(Me.llSetToTop)
        Me.GroupBox1.Controls.Add(Me.chkShowCaptured)
        Me.GroupBox1.Controls.Add(Me.btnCapture1)
        Me.GroupBox1.Controls.Add(Me.lblCapture)
        Me.GroupBox1.Controls.Add(Me.btnAuto1)
        Me.GroupBox1.Controls.Add(Me.AxVLCPlugin21)
        Me.GroupBox1.Controls.Add(Me.btnLive1)
        Me.GroupBox1.Location = New System.Drawing.Point(7, 0)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(304, 257)
        Me.GroupBox1.TabIndex = 10
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "GroupBox1"
        '
        'llSetToTop
        '
        Me.llSetToTop.AutoSize = True
        Me.llSetToTop.Location = New System.Drawing.Point(191, 239)
        Me.llSetToTop.Name = "llSetToTop"
        Me.llSetToTop.Size = New System.Drawing.Size(102, 13)
        Me.llSetToTop.TabIndex = 16
        Me.llSetToTop.TabStop = True
        Me.llSetToTop.Text = "set all CTCS to TOP"
        '
        'chkShowCaptured
        '
        Me.chkShowCaptured.AutoSize = True
        Me.chkShowCaptured.Location = New System.Drawing.Point(6, 238)
        Me.chkShowCaptured.Name = "chkShowCaptured"
        Me.chkShowCaptured.Size = New System.Drawing.Size(98, 17)
        Me.chkShowCaptured.TabIndex = 15
        Me.chkShowCaptured.Text = "Show captured"
        Me.chkShowCaptured.UseVisualStyleBackColor = True
        '
        'btnCapture1
        '
        Me.btnCapture1.Location = New System.Drawing.Point(202, 206)
        Me.btnCapture1.Name = "btnCapture1"
        Me.btnCapture1.Size = New System.Drawing.Size(92, 32)
        Me.btnCapture1.TabIndex = 14
        Me.btnCapture1.Text = "Capture 1 pic"
        Me.btnCapture1.UseVisualStyleBackColor = True
        '
        'lblCapture
        '
        Me.lblCapture.AutoSize = True
        Me.lblCapture.Location = New System.Drawing.Point(17, 193)
        Me.lblCapture.Name = "lblCapture"
        Me.lblCapture.Size = New System.Drawing.Size(39, 13)
        Me.lblCapture.TabIndex = 13
        Me.lblCapture.Text = "Label1"
        '
        'btnAuto1
        '
        Me.btnAuto1.Location = New System.Drawing.Point(104, 206)
        Me.btnAuto1.Name = "btnAuto1"
        Me.btnAuto1.Size = New System.Drawing.Size(92, 32)
        Me.btnAuto1.TabIndex = 12
        Me.btnAuto1.Text = "Capture"
        Me.btnAuto1.UseVisualStyleBackColor = True
        '
        'AxVLCPlugin21
        '
        Me.AxVLCPlugin21.Enabled = True
        Me.AxVLCPlugin21.Location = New System.Drawing.Point(6, 19)
        Me.AxVLCPlugin21.Name = "AxVLCPlugin21"
        Me.AxVLCPlugin21.OcxState = CType(resources.GetObject("AxVLCPlugin21.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxVLCPlugin21.Size = New System.Drawing.Size(288, 181)
        Me.AxVLCPlugin21.TabIndex = 11
        '
        'btnLive1
        '
        Me.btnLive1.Location = New System.Drawing.Point(6, 206)
        Me.btnLive1.Name = "btnLive1"
        Me.btnLive1.Size = New System.Drawing.Size(92, 32)
        Me.btnLive1.TabIndex = 10
        Me.btnLive1.Text = "Live View"
        Me.btnLive1.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnCenter)
        Me.GroupBox2.Controls.Add(Me.FlowLayoutPanel2)
        Me.GroupBox2.Controls.Add(Me.btnCapture2)
        Me.GroupBox2.Controls.Add(Me.lblCount2)
        Me.GroupBox2.Controls.Add(Me.AxVLCPlugin22)
        Me.GroupBox2.Controls.Add(Me.btnLive2)
        Me.GroupBox2.Location = New System.Drawing.Point(7, 263)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(304, 307)
        Me.GroupBox2.TabIndex = 11
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "GroupBox2"
        '
        'btnCenter
        '
        Me.btnCenter.Location = New System.Drawing.Point(104, 206)
        Me.btnCenter.Name = "btnCenter"
        Me.btnCenter.Size = New System.Drawing.Size(92, 32)
        Me.btnCenter.TabIndex = 16
        Me.btnCenter.Text = "Center"
        Me.btnCenter.UseVisualStyleBackColor = True
        '
        'FlowLayoutPanel2
        '
        Me.FlowLayoutPanel2.Location = New System.Drawing.Point(9, 241)
        Me.FlowLayoutPanel2.Name = "FlowLayoutPanel2"
        Me.FlowLayoutPanel2.Size = New System.Drawing.Size(284, 65)
        Me.FlowLayoutPanel2.TabIndex = 15
        '
        'btnCapture2
        '
        Me.btnCapture2.Location = New System.Drawing.Point(202, 206)
        Me.btnCapture2.Name = "btnCapture2"
        Me.btnCapture2.Size = New System.Drawing.Size(92, 32)
        Me.btnCapture2.TabIndex = 14
        Me.btnCapture2.Text = "Capture 1 pic"
        Me.btnCapture2.UseVisualStyleBackColor = True
        '
        'lblCount2
        '
        Me.lblCount2.AutoSize = True
        Me.lblCount2.Location = New System.Drawing.Point(17, 193)
        Me.lblCount2.Name = "lblCount2"
        Me.lblCount2.Size = New System.Drawing.Size(39, 13)
        Me.lblCount2.TabIndex = 13
        Me.lblCount2.Text = "Label1"
        '
        'AxVLCPlugin22
        '
        Me.AxVLCPlugin22.Enabled = True
        Me.AxVLCPlugin22.Location = New System.Drawing.Point(6, 19)
        Me.AxVLCPlugin22.Name = "AxVLCPlugin22"
        Me.AxVLCPlugin22.OcxState = CType(resources.GetObject("AxVLCPlugin22.OcxState"), System.Windows.Forms.AxHost.State)
        Me.AxVLCPlugin22.Size = New System.Drawing.Size(288, 181)
        Me.AxVLCPlugin22.TabIndex = 11
        '
        'btnLive2
        '
        Me.btnLive2.Location = New System.Drawing.Point(6, 206)
        Me.btnLive2.Name = "btnLive2"
        Me.btnLive2.Size = New System.Drawing.Size(92, 32)
        Me.btnLive2.TabIndex = 10
        Me.btnLive2.Text = "Live View"
        Me.btnLive2.UseVisualStyleBackColor = True
        '
        'lblSensor
        '
        Me.lblSensor.AutoSize = True
        Me.lblSensor.Location = New System.Drawing.Point(101, 239)
        Me.lblSensor.Name = "lblSensor"
        Me.lblSensor.Size = New System.Drawing.Size(40, 13)
        Me.lblSensor.TabIndex = 17
        Me.lblSensor.Text = "Sensor"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 688)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.FlowLayoutPanel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.KeyPreview = True
        Me.Name = "Form1"
        Me.Text = "Container Visual Inspection"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.AxVLCPlugin21, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.AxVLCPlugin22, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents btnCapture1 As Button
    Friend WithEvents lblCapture As Label
    Friend WithEvents btnAuto1 As Button
    Friend WithEvents AxVLCPlugin21 As AxAXVLC.AxVLCPlugin2
    Friend WithEvents btnLive1 As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents btnCapture2 As Button
    Friend WithEvents lblCount2 As Label
    Friend WithEvents AxVLCPlugin22 As AxAXVLC.AxVLCPlugin2
    Friend WithEvents btnLive2 As Button
    Friend WithEvents FlowLayoutPanel2 As FlowLayoutPanel
    Friend WithEvents chkShowCaptured As CheckBox
    Friend WithEvents btnCenter As Button
    Friend WithEvents llSetToTop As LinkLabel
    Friend WithEvents lblSensor As Label
End Class
