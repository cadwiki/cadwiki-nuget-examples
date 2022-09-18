Class MainWindow
    Inherits Window
    Public Sub New()
        ' This call is required by the designer.

        Me.Hide()
        InitializeComponent()
        Dim window As Window = New cadwiki.CadDevTools.MainWindow()
        window.ShowDialog()
        Me.Close()
    End Sub
End Class
