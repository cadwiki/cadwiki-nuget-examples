Option Strict On
Option Infer Off
Option Explicit On

Imports System.Reflection
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.Windows
Imports cadwiki.DllReloader.AutoCAD.UiRibbon.Buttons

Namespace UiRibbon.Panels
    Public Class Test

        Public Shared Function CreateTestsPanel(blankButton As RibbonButton) As RibbonPanel
            Dim integrationTestsButton As RibbonButton = CreateRegressionTestsButton()
            Dim ribbonPanelSource As RibbonPanelSource = New RibbonPanelSource()
            ribbonPanelSource.Title = "Tests"
            Dim row1 As RibbonRowPanel = New RibbonRowPanel()
            row1.IsTopJustified = True
            row1.Items.Add(integrationTestsButton)
            row1.Items.Add(New RibbonRowBreak())
            row1.Items.Add(blankButton)
            row1.Items.Add(New RibbonRowBreak())
            row1.Items.Add(blankButton)
            row1.Items.Add(New RibbonRowBreak())
            row1.Items.Add(blankButton)
            ribbonPanelSource.Items.Add(row1)
            Dim ribbonPanel As RibbonPanel = New RibbonPanel()
            ribbonPanel.Source = ribbonPanelSource
            Return ribbonPanel
        End Function

        Shared Function CreateRegressionTestsButton() As RibbonButton
            Dim ribbonButton As RibbonButton = New RibbonButton()
            ribbonButton.Name = "Regression Tests"
            ribbonButton.ShowText = True
            ribbonButton.Text = "Regression Tests"

            Dim allRegressionTests As Type = GetType(RegressionTests.RegressionTests)
            Dim allRegressionTestTypes As Type() = {allRegressionTests}
            'for testing workflow object method call
            'Dim worflow As Workflows.NUnitTestRunner = New Workflows.NUnitTestRunner()
            'worflow.Run(1, allRegressionTestTypes)
            ' bug in GenericClickCommandHandler when only a single parameter is passed
            ' using a temp integer value to work around the bug for now
            Dim uiRouter As UiRouter = New UiRouter(
                "MainApp.Workflows",
                "MainApp.Workflows.NUnitTestRunner",
                "Run",
                {1, allRegressionTestTypes},
                App.AcadAppDomainDllReloader,
                Assembly.GetExecutingAssembly())
            ribbonButton.CommandParameter = uiRouter
            ribbonButton.CommandHandler = New GenericClickCommandHandler(Application.DocumentManager.MdiActiveDocument)
            ribbonButton.ToolTip = "Runs regression tests from the current .dll"
            Return ribbonButton
        End Function

    End Class
End Namespace
