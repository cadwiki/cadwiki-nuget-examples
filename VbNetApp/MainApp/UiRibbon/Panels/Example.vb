Option Strict On
Option Infer Off
Option Explicit On

Imports System.Reflection
Imports Autodesk.Windows
Imports cadwiki.DllReloader.AutoCAD.UiRibbon.Buttons

Namespace UiRibbon.Panels
    Public Class Example
        Public Shared Function CreateExamplePanel(blankButton As RibbonButton) As RibbonPanel
            Dim ribbonPanelSource As RibbonPanelSource = New RibbonPanelSource()
            ribbonPanelSource.Title = "Example"
            Dim exampleButton As RibbonButton = ExampleButtons.CreateExampleButton()
            Dim helloButton As RibbonButton = ExampleButtons.CreatHelloButton()
            Dim row1 As RibbonRowPanel = New RibbonRowPanel()
            row1.IsTopJustified = True
            row1.Items.Add(exampleButton)
            row1.Items.Add(New RibbonRowBreak())
            row1.Items.Add(helloButton)
            row1.Items.Add(New RibbonRowBreak())
            row1.Items.Add(blankButton)
            row1.Items.Add(New RibbonRowBreak())
            row1.Items.Add(blankButton)
            ribbonPanelSource.Items.Add(row1)
            Dim exportersPanel As RibbonPanel = New RibbonPanel()
            exportersPanel.Source = ribbonPanelSource
            Return exportersPanel
        End Function
    End Class

    Partial Public Class ExampleButtons
        Shared Function CreateExampleButton() As RibbonButton
            Dim ribbonButton As RibbonButton = New RibbonButton()
            ribbonButton.Name = "Example - Run MyCommand"
            ribbonButton.ShowText = True
            ribbonButton.Text = "Example - Run MyCommand"
            ribbonButton.Size = RibbonItemSize.Standard
            Dim uiRouter As UiRouter = New UiRouter(
                "BusinessLogic.Commands.Example", "Run", Nothing,
                App.AcadAppDomainDllReloader,
                Assembly.GetExecutingAssembly())
            ribbonButton.CommandParameter = uiRouter
            ribbonButton.CommandHandler = New GenericClickCommandHandler()
            ribbonButton.ToolTip = "Click to run MyCommand using the CommandHandler And Command Parameter bound to this UiRibbon button"
            Return ribbonButton
        End Function

        Shared Function CreatHelloButton() As RibbonButton
            Dim ribbonButton As RibbonButton = New RibbonButton()
            ribbonButton.Name = "Hello"
            ribbonButton.ShowText = True
            ribbonButton.Text = "Hello"
            ribbonButton.Size = RibbonItemSize.Standard
            Dim uiRouter As UiRouter = New UiRouter("BusinessLogic.Commands.HelloFromCadWiki", "Run", Nothing,
                App.AcadAppDomainDllReloader,
                Assembly.GetExecutingAssembly())
            ribbonButton.CommandParameter = uiRouter
            ribbonButton.CommandHandler = New GenericClickCommandHandler()
            ribbonButton.ToolTip = "Click to run HelloFromCadWiki"
            Return ribbonButton
        End Function

    End Class
End Namespace


