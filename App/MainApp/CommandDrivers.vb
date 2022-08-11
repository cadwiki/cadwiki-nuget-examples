Option Strict On
Option Infer Off
Option Explicit On

Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.Runtime

' This decorator tells the AutoCAD runtime to load the entire CommandDrivers class and look for <CommandMethods> to load as callable from the AutoCAD command line
<Assembly: CommandClass(GetType(CommandDrivers))>
Public Class CommandDrivers
    Public Class Constants
        Public Const GroupName As String = "MainAppCommands"
    End Class


    <CommandMethod(Constants.GroupName, "MyCommand", CommandFlags.Session)>
    Public Shared Sub CommandMethod()
        Dim doc As Document = Application.DocumentManager.MdiActiveDocument
        doc.Editor.WriteMessage("Hello from the command drivers class" + vbLf)
        BusinessLogic.Commands.Example.Run()
    End Sub

End Class
