Option Strict On
Option Infer Off
Option Explicit On

Imports System.Reflection
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.Windows
Imports cadwiki.DllReloader.AutoCAD.UiRibbon.Buttons

Namespace UiRibbon.Panels
    Public Class Example
        Public Shared Title As String = "Example"
        Public Shared Id As String = "cadwiki.ExamplePanel"

        Public Shared Function CreateExamplePanel(blankButton As RibbonButton) As RibbonPanel
            Dim ribbonPanelSource As RibbonPanelSource = New RibbonPanelSource()
            ribbonPanelSource.Title = Title
            ribbonPanelSource.Id = Id
            Dim exampleButton As RibbonButton = ExampleButtons.CreateExampleButton()
            Dim helloButton As RibbonButton = ExampleButtons.CreatHelloButton()
            Dim pluginButton As RibbonButton = ExampleButtons.CreatePluginButton()
            Dim row1 As RibbonRowPanel = New RibbonRowPanel()
            row1.IsTopJustified = True
            row1.Items.Add(exampleButton)
            row1.Items.Add(New RibbonRowBreak())
            row1.Items.Add(helloButton)
            row1.Items.Add(New RibbonRowBreak())
            row1.Items.Add(pluginButton)
            row1.Items.Add(New RibbonRowBreak())
            row1.Items.Add(blankButton)
            ribbonPanelSource.Items.Add(row1)
            Dim exportersPanel As RibbonPanel = New RibbonPanel()
            exportersPanel.Source = ribbonPanelSource
            Return exportersPanel
        End Function
    End Class



    Partial Public Class ExampleButtons
        Public Shared HelloButtonId As String = "Hello"
        Shared Function CreateExampleButton() As RibbonButton
            Dim ribbonButton As RibbonButton = New RibbonButton()
            ribbonButton.Name = "Example - Run MyCommand"
            ribbonButton.ShowText = True
            ribbonButton.Text = "Example - Run MyCommand"
            ribbonButton.Size = RibbonItemSize.Standard
            Dim uiRouter As UiRouter = New UiRouter(
                "BusinessLogic",
                "BusinessLogic.Commands.Example",
                "Run",
                Nothing,
                App.AcadAppDomainDllReloader,
                Assembly.GetExecutingAssembly())
            ribbonButton.CommandParameter = uiRouter
            ribbonButton.CommandHandler = New GenericClickCommandHandler(Application.DocumentManager.MdiActiveDocument)
            ribbonButton.ToolTip = "Click to run MyCommand using the CommandHandler And Command Parameter bound to this UiRibbon button"
            Return ribbonButton
        End Function

        Shared Function CreatHelloButton() As RibbonButton
            Dim ribbonButton As RibbonButton = New RibbonButton()
            ribbonButton.Name = "Hello"
            ribbonButton.ShowText = True
            ribbonButton.Text = "Hello"
            ribbonButton.Size = RibbonItemSize.Standard
            ribbonButton.Id = HelloButtonId
            'start here 5 - UiRouter
            'The UiRouter contains all the information necessary for the AutoCADAppDomainDllReloader to
            'parse a dll in the current app domain, and call the method you want to call
            'the AutoCADAppDomainDllReloader will call your method from the most recently reloaded dll
            Dim uiRouter As UiRouter = New UiRouter(
                "BusinessLogic",
                "BusinessLogic.Commands.HelloFromCadWiki",
                "Run",
                Nothing,
                App.AcadAppDomainDllReloader,
                Assembly.GetExecutingAssembly())
            'start here 6 - RibbonButton CommandParameter = uiRouter
            'the UiRouter is stored on the ribbonButton.CommandParameter
            ribbonButton.CommandParameter = uiRouter
            'start here 7 - GenericClickCommandHandler
            'the GenericClickCommandHandler handles all Execute calls by utilizing the CommandParameter above
            ribbonButton.CommandHandler = New GenericClickCommandHandler(Application.DocumentManager.MdiActiveDocument)
            ribbonButton.ToolTip = "Click to run HelloFromCadWiki"
            Return ribbonButton
        End Function

        Public Shared Function CreatePluginButton() As RibbonButton
            Dim ribbonButton As RibbonButton = New RibbonButton()
            ribbonButton.Name = "Example - Run Plugin Method"
            ribbonButton.ShowText = True
            ribbonButton.Text = "Example - Run Plugin Method"
            ribbonButton.Size = RibbonItemSize.Standard
            Dim uiRouter As UiRouter = New UiRouter(
                "Plugin",
                "Plugin.Plugin.MyCommands",
                "MyCommand2",
                Nothing,
                App.AcadAppDomainDllReloader,
                Assembly.GetExecutingAssembly()
                )
            ribbonButton.CommandParameter = uiRouter
            ribbonButton.CommandHandler = New GenericClickCommandHandler()
            ribbonButton.ToolTip = "Click to run MyCommand using the CommandHandler And Command Parameter bound to this UiRibbon button"
            Return ribbonButton
        End Function

    End Class
End Namespace


