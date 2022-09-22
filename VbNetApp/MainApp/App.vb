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

    'start here 1 - AutoCADAppDomainDllReloader
    'this variable handles routing the Ui clicks on a AutoCAD ribbon button to your methods found in an Assembly
    Public Shared AcadAppDomainDllReloader As New AutoCADAppDomainDllReloader

    'start here 2 - IExtensionApplication.Initialize
    'once the AcadAppDomainDllReloader is configured with the current Assembly, it will be able to route Ui clicks
    'to the correct method
    Public Sub Initialize() Implements IExtensionApplication.Initialize
        'This Event Handler allows the IExtensionApplication to Resolve any Assemblies
        'The AssemblyResolve method finds the correct assembly in the AppDomain when there are multiple assemblies
        'with the same name and differing version number
        AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf AutoCADAppDomainDllReloader.AssemblyResolve
        Dim iExtensionAppAssembly As Assembly = Assembly.GetExecutingAssembly
        Dim iExtensionAppVersion As Version = cadwiki.NetUtils.AssemblyUtils.GetVersion(iExtensionAppAssembly)
        AcadAppDomainDllReloader.Configure(iExtensionAppAssembly, True)
        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        doc.Editor.WriteMessage(vbLf & "App " & iExtensionAppVersion.ToString & " initialized...")
        doc.Editor.WriteMessage(vbLf)
        UiRibbon.Tabs.TabCreator.AddTabs(doc)
        BusinessLogic.App.Initialize()
    End Sub


    'start here 3 - IExtensionApplication.Terminate
    'add a call to terminate the AcadAppDomainDllReloader
    Public Sub Terminate() Implements IExtensionApplication.Terminate
        AcadAppDomainDllReloader.Terminate()
    End Sub

End Class




