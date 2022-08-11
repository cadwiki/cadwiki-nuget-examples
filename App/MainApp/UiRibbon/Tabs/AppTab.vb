Option Strict On
Option Infer Off
Option Explicit On

Imports Autodesk.Windows

Namespace UiRibbon.Tabs
    Public Class AppTab
        Private Shared ReadOnly tabName As String = "AppTab"

        Public Shared Function CreateAppTab() As RibbonTab
            Dim ribbonTab As RibbonTab = New RibbonTab()
            ribbonTab.Title = tabName
            ribbonTab.Id = tabName
            ribbonTab.Name = tabName
            Return ribbonTab
        End Function
    End Class
End Namespace
