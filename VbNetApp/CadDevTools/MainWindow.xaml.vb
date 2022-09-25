Imports System.IO
Imports cadwiki.NetUtils

Class MainWindow
    Inherits Window
    Public Sub New()
        ' This call is required by the designer.

        Me.Hide()
        InitializeComponent()
        Dim solutionDir As String = Paths.TryGetSolutionDirectoryPath()
        Dim wildCardFileName As String = "*" + "MainApp.dll"
        Dim mainAppDll As String = Paths.GetNewestDllInVsubfoldersOfSolutionDirectory(solutionDir, wildCardFileName)
        Dim dependencies As New cadwiki.CadDevTools.MainWindow.Dependencies()
        dependencies.DllFilePathToNetload = mainAppDll
        Dim window As Window = New cadwiki.CadDevTools.MainWindow(dependencies)
        window.ShowDialog()
        Me.Close()
    End Sub


End Class
