<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class aspWebSite
    Inherits System.Windows.Forms.UserControl

    'UserControl reemplaza a Dispose para limpiar la lista de componentes.
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

    'Requerido por el Diseñador de Windows Forms
    Private components As System.ComponentModel.IContainer

    'NOTA: el Diseñador de Windows Forms necesita el siguiente procedimiento
    'Se puede modificar usando el Diseñador de Windows Forms.  
    'No lo modifique con el editor de código.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.WebKitBrowser1 = New WebKit.WebKitBrowser()
        Me.SuspendLayout()
        '
        'WebKitBrowser1
        '
        Me.WebKitBrowser1.BackColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(39, Byte), Integer))
        Me.WebKitBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.WebKitBrowser1.Location = New System.Drawing.Point(0, 0)
        Me.WebKitBrowser1.Name = "WebKitBrowser1"
        Me.WebKitBrowser1.Size = New System.Drawing.Size(647, 446)
        Me.WebKitBrowser1.TabIndex = 0
        Me.WebKitBrowser1.Url = New System.Uri("http://ziononline.azurewebsites.net/", System.UriKind.Absolute)
        '
        'aspWebSite
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(36, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(39, Byte), Integer))
        Me.Controls.Add(Me.WebKitBrowser1)
        Me.Margin = New System.Windows.Forms.Padding(2, 2, 2, 2)
        Me.Name = "aspWebSite"
        Me.Size = New System.Drawing.Size(647, 446)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents WebKitBrowser1 As WebKit.WebKitBrowser
End Class
