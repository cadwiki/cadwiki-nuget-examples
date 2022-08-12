Option Strict On
Option Infer Off
Option Explicit On

Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.Windows
Imports MainApp.UiRibbon.Panels

Namespace UiRibbon.Tabs
    Public Class TabCreator
        Public Shared Sub AddTabs(doc As Document)
            Dim appRibbonTab As RibbonTab = AppTab.CreateAppTab()
            PanelCreator.AddAllPanels(appRibbonTab)
            Dim allRibbonTabs As New List(Of RibbonTab)(New RibbonTab() {appRibbonTab})
            AddTabs(doc, allRibbonTabs)
        End Sub

        Private Shared Sub AddTabs(doc As Document, ribbonTabs As List(Of RibbonTab))
            doc.Editor.WriteMessage(vbLf & "Adding tabs...")
            For Each ribbonTab As RibbonTab In ribbonTabs
                AddTab(doc, ribbonTab)
            Next
        End Sub

        Private Shared Sub AddTab(doc As Document, ribbonTab As RibbonTab)
            doc.Editor.WriteMessage(vbLf & "Add tab...")
            Dim ribbonControl As RibbonControl = ComponentManager.Ribbon
            If (ribbonControl IsNot Nothing And
                ribbonTab IsNot Nothing) Then
                If (ribbonTab.Name Is Nothing) Then
                    Throw New Exception("Ribbon Tab does not have a name. Please add a name to the ribbon tab.")
                End If
                Dim tabName As String = ribbonTab.Name
                doc.Editor.WriteMessage(vbLf & tabName)
                Dim doesTabAlreadyExist As RibbonTab = ribbonControl.FindTab(tabName)
                If (doesTabAlreadyExist IsNot Nothing) Then
                    ribbonControl.Tabs.Remove(doesTabAlreadyExist)
                End If
                ribbonControl.Tabs.Add(ribbonTab)
                ribbonTab.IsActive = True
            End If
        End Sub
    End Class
End Namespace
