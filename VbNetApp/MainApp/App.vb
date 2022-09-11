Option Strict On
Option Infer Off
Option Explicit On

Imports System.Reflection
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Runtime
Imports Application = Autodesk.AutoCAD.ApplicationServices.Application
Imports cadwiki.DllReloader.AutoCAD

Public Class App
    Implements IExtensionApplication

    Public Shared AcadAppDomainDllReloader As New AutoCADAppDomainDllReloader

    Public Sub Initialize() Implements IExtensionApplication.Initialize
        'This Event Handler allows the IExtensionApplication to Resolve any Assemblies
        'The AssemblyResolve method finds the correct assembly in the AppDomain when there are multiple assemblies
        'with the same name and differing version number
        AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf AutoCADAppDomainDllReloader.AssemblyResolve
        Dim iExtensionAppAssembly As Assembly = Assembly.GetExecutingAssembly
        Dim iExtensionAppVersion As Version = cadwiki.NetUtils.AssemblyUtils.GetVersion(iExtensionAppAssembly)
        AcadAppDomainDllReloader.Configure(iExtensionAppAssembly)
        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        doc.Editor.WriteMessage(vbLf & "App " & iExtensionAppVersion.ToString & " initialized...")
        doc.Editor.WriteMessage(vbLf)
        UiRibbon.Tabs.TabCreator.AddTabs(doc)
        BusinessLogic.App.Initialize()
    End Sub


    Public Sub Terminate() Implements IExtensionApplication.Terminate
        AcadAppDomainDllReloader.Terminate()
    End Sub

End Class




