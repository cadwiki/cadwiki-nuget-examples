Option Strict On
Option Infer Off
Option Explicit On

Imports Autodesk.Windows

Namespace UiRibbon.Panels
    Public Class PanelCreator
        Public Shared Sub AddAllPanels(ribbonTab As RibbonTab)
            Dim blankButton As RibbonButton = New RibbonButton()
            blankButton.Name = "BlankButton1"
            blankButton.Size = RibbonItemSize.Standard
            blankButton.IsEnabled = False
            Dim infoRibbonPanel As RibbonPanel = Info.CreateInfoPanel(blankButton)
            ribbonTab.Panels.Add(infoRibbonPanel)
            Dim examplePanel As RibbonPanel = Example.CreateExamplePanel(blankButton)
            ribbonTab.Panels.Add(examplePanel)
        End Sub
    End Class
End Namespace
