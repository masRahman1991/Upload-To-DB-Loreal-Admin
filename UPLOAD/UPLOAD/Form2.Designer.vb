<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form2
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
        OpenFileDialog1 = New OpenFileDialog()
        btnBrowse = New Button()
        txtFilePath = New TextBox()
        btnUpload = New Button()
        ProgressBar1 = New ProgressBar()
        lblProgressPercentage = New Label()
        TabControl1 = New TabControl()
        TabPage1 = New TabPage()
        TabPage2 = New TabPage()
        Button1 = New Button()
        TabControl1.SuspendLayout()
        TabPage1.SuspendLayout()
        TabPage2.SuspendLayout()
        SuspendLayout()
        ' 
        ' OpenFileDialog1
        ' 
        OpenFileDialog1.FileName = "OpenFileDialog1"
        ' 
        ' btnBrowse
        ' 
        btnBrowse.Location = New Point(0, 3)
        btnBrowse.Name = "btnBrowse"
        btnBrowse.Size = New Size(112, 125)
        btnBrowse.TabIndex = 0
        btnBrowse.Text = "Browse"
        btnBrowse.UseVisualStyleBackColor = True
        ' 
        ' txtFilePath
        ' 
        txtFilePath.BorderStyle = BorderStyle.FixedSingle
        txtFilePath.Enabled = False
        txtFilePath.Location = New Point(117, 3)
        txtFilePath.Multiline = True
        txtFilePath.Name = "txtFilePath"
        txtFilePath.Size = New Size(844, 125)
        txtFilePath.TabIndex = 1
        ' 
        ' btnUpload
        ' 
        btnUpload.Location = New Point(967, 5)
        btnUpload.Name = "btnUpload"
        btnUpload.Size = New Size(193, 122)
        btnUpload.TabIndex = 2
        btnUpload.Text = "Upload"
        btnUpload.UseVisualStyleBackColor = True
        ' 
        ' ProgressBar1
        ' 
        ProgressBar1.Location = New Point(2, 134)
        ProgressBar1.Name = "ProgressBar1"
        ProgressBar1.Size = New Size(1158, 132)
        ProgressBar1.TabIndex = 3
        ' 
        ' lblProgressPercentage
        ' 
        lblProgressPercentage.AutoSize = True
        lblProgressPercentage.Location = New Point(17, 186)
        lblProgressPercentage.Name = "lblProgressPercentage"
        lblProgressPercentage.Size = New Size(95, 25)
        lblProgressPercentage.TabIndex = 4
        lblProgressPercentage.Text = "Percentase"
        ' 
        ' TabControl1
        ' 
        TabControl1.Controls.Add(TabPage1)
        TabControl1.Controls.Add(TabPage2)
        TabControl1.Location = New Point(12, 12)
        TabControl1.Name = "TabControl1"
        TabControl1.SelectedIndex = 0
        TabControl1.Size = New Size(1172, 327)
        TabControl1.TabIndex = 5
        ' 
        ' TabPage1
        ' 
        TabPage1.Controls.Add(btnBrowse)
        TabPage1.Controls.Add(lblProgressPercentage)
        TabPage1.Controls.Add(txtFilePath)
        TabPage1.Controls.Add(ProgressBar1)
        TabPage1.Controls.Add(btnUpload)
        TabPage1.Location = New Point(4, 34)
        TabPage1.Name = "TabPage1"
        TabPage1.Padding = New Padding(3)
        TabPage1.Size = New Size(1164, 289)
        TabPage1.TabIndex = 0
        TabPage1.Text = "UPLOAD"
        TabPage1.UseVisualStyleBackColor = True
        ' 
        ' TabPage2
        ' 
        TabPage2.Controls.Add(Button1)
        TabPage2.Location = New Point(4, 34)
        TabPage2.Name = "TabPage2"
        TabPage2.Size = New Size(1164, 289)
        TabPage2.TabIndex = 1
        TabPage2.Text = "CLEAN DATA"
        TabPage2.UseVisualStyleBackColor = True
        ' 
        ' Button1
        ' 
        Button1.Font = New Font("Segoe UI", 14F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        Button1.ForeColor = SystemColors.ActiveCaptionText
        Button1.ImageKey = "(none)"
        Button1.Location = New Point(19, 79)
        Button1.Name = "Button1"
        Button1.Size = New Size(1126, 112)
        Button1.TabIndex = 3
        Button1.Text = "CLEAN DATA STOCK COUNT "
        Button1.UseVisualStyleBackColor = True
        ' 
        ' Form2
        ' 
        AutoScaleDimensions = New SizeF(10F, 25F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(1196, 351)
        Controls.Add(TabControl1)
        MaximizeBox = False
        MinimizeBox = False
        Name = "Form2"
        StartPosition = FormStartPosition.CenterScreen
        Text = "Upload To DB L'oreal Version 1.4 - Admin"
        TabControl1.ResumeLayout(False)
        TabPage1.ResumeLayout(False)
        TabPage1.PerformLayout()
        TabPage2.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents OpenFileDialog1 As OpenFileDialog
    Friend WithEvents btnBrowse As Button
    Friend WithEvents txtFilePath As TextBox
    Friend WithEvents btnUpload As Button
    Friend WithEvents ProgressBar1 As ProgressBar
    Friend WithEvents lblProgressPercentage As Label
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents Button1 As Button
End Class
