Option Strict On
Option Infer Off
Option Explicit On

Imports System.IO
Imports System.Reflection
Imports Autodesk.Windows
Imports cadwiki.DllReloader.AutoCAD.UiRibbon.Buttons

Namespace UiRibbon.Panels
    Public Class Info
        Public Shared Function CreateInfoPanel(blankButton As RibbonButton) As RibbonPanel
            Dim row1 As RibbonRowPanel = New RibbonRowPanel()
            Dim row2 As RibbonRowPanel = New RibbonRowPanel()
            Dim row3 As RibbonRowPanel = New RibbonRowPanel()
            Dim row4 As RibbonRowPanel = New RibbonRowPanel()
            Dim currentIExtensionAppAssembly As Assembly = Assembly.GetExecutingAssembly
            Dim dllName As String = App.AcadAppDomainDllReloader.GetReloadedAssemblyNameSafely(currentIExtensionAppAssembly)
            Dim versionNumberStr As String = ""
            Dim exeName As String = ""
            If (App.AcadAppDomainDllReloader.GetReloadCount() >= 1) Then
                exeName = Path.GetFileName(App.AcadAppDomainDllReloader.GetDllPath())
                versionNumberStr = GenericClickCommandHandler.GetAssemblyVersionFromFullName(dllName)
            Else
                Dim filePath As String = Assembly.GetExecutingAssembly().Location
                exeName = Path.GetFileName(filePath)
                versionNumberStr = GenericClickCommandHandler.GetAssemblyVersionFromFullName(currentIExtensionAppAssembly.FullName)
            End If
            Dim versionNumber As RibbonButton = CreateVersionNumberButton(versionNumberStr)
            Dim assemblyName As RibbonButton = CreateAssemblyNameButton(exeName)
            Dim reloadCount As RibbonButton = CreateReloadCountButton(exeName)
            Dim ribbonPanelSource As RibbonPanelSource = New RibbonPanelSource()
            ribbonPanelSource.Title = "Info"
            ribbonPanelSource.Items.Add(row1)
            ribbonPanelSource.Items.Add(New RibbonRowBreak())
            ribbonPanelSource.Items.Add(row2)
            ribbonPanelSource.Items.Add(New RibbonRowBreak())
            ribbonPanelSource.Items.Add(row3)
            ribbonPanelSource.Items.Add(New RibbonRowBreak())
            ribbonPanelSource.Items.Add(row4)
            Dim ribbonPanel As RibbonPanel = New RibbonPanel()
            ribbonPanel.Source = ribbonPanelSource
            row1.Items.Add(versionNumber)
            row2.Items.Add(assemblyName)
            row3.Items.Add(reloadCount)
            row4.Items.Add(blankButton)
            row4.Items.Add(blankButton)
            Return ribbonPanel
        End Function

        Private Shared Function CreateVersionNumberButton(versionNumberStr As String) As RibbonButton
            Dim versionNumber As RibbonButton = New RibbonButton()
            versionNumber.Name = "Version"
            versionNumber.ShowText = True
            versionNumber.Text = " v" + versionNumberStr + " "
            versionNumber.Size = RibbonItemSize.Standard
            versionNumber.IsEnabled = False
            Return versionNumber
        End Function

        Private Shared Function CreateReloadCountButton(exeName As String) As RibbonButton
            Dim button As RibbonButton = New RibbonButton()
            button.Name = "ReloadCount"
            button.ShowText = True
            button.Text = " Reload Count: " + App.AcadAppDomainDllReloader.GetReloadCount().ToString()
            button.Size = RibbonItemSize.Standard
            button.CommandHandler = New DllReloadClickCommandHandler()
            button.ToolTip = "Reload the " + exeName + " dll into AutoCAD"

            Dim uiRouter As UiRouter = New UiRouter(
                "BusinessLogic",
                "BusinessLogic.Commands.HelloFromCadWiki",
                "Run",
                Nothing,
                App.AcadAppDomainDllReloader,
                Assembly.GetExecutingAssembly())
            button.CommandParameter = uiRouter
            Return button
        End Function

        Private Shared Function CreateAssemblyNameButton(exeName As String) As RibbonButton
            Dim assemblyName As RibbonButton = New RibbonButton()
            assemblyName.Name = "ReleaseStatus"
            assemblyName.ShowText = True
            assemblyName.Text = " Dll: " + exeName
            assemblyName.Size = RibbonItemSize.Standard
            assemblyName.IsEnabled = False
            Return assemblyName
        End Function
    End Class
End Namespace
