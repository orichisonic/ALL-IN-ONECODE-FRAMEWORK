<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class RecipePreviewControl
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.pctRecipe = New System.Windows.Forms.PictureBox()
        Me.lbTitle = New System.Windows.Forms.Label()
        Me.tbComments = New System.Windows.Forms.TextBox()
        CType(Me.pctRecipe, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pctRecipe
        '
        Me.pctRecipe.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pctRecipe.Location = New System.Drawing.Point(13, 27)
        Me.pctRecipe.Name = "pctRecipe"
        Me.pctRecipe.Size = New System.Drawing.Size(269, 142)
        Me.pctRecipe.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom
        Me.pctRecipe.TabIndex = 8
        Me.pctRecipe.TabStop = False
        '
        'lbTitle
        '
        Me.lbTitle.AutoSize = True
        Me.lbTitle.Location = New System.Drawing.Point(10, 11)
        Me.lbTitle.Name = "lbTitle"
        Me.lbTitle.Size = New System.Drawing.Size(23, 13)
        Me.lbTitle.TabIndex = 7
        Me.lbTitle.Text = "title"
        '
        'tbComments
        '
        Me.tbComments.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbComments.Location = New System.Drawing.Point(13, 175)
        Me.tbComments.Multiline = True
        Me.tbComments.Name = "tbComments"
        Me.tbComments.ReadOnly = True
        Me.tbComments.ScrollBars = System.Windows.Forms.ScrollBars.Both
        Me.tbComments.Size = New System.Drawing.Size(269, 157)
        Me.tbComments.TabIndex = 6
        '
        'RecipePreviewControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.Controls.Add(Me.pctRecipe)
        Me.Controls.Add(Me.lbTitle)
        Me.Controls.Add(Me.tbComments)
        Me.Name = "RecipePreviewControl"
        Me.Size = New System.Drawing.Size(293, 342)
        CType(Me.pctRecipe, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents pctRecipe As System.Windows.Forms.PictureBox
    Private WithEvents lbTitle As System.Windows.Forms.Label
    Private WithEvents tbComments As System.Windows.Forms.TextBox

End Class
