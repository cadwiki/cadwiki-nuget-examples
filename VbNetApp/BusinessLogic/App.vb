Imports Autodesk.AutoCAD.ApplicationServices

Public Class App
    Public Shared Sub Initialize()
        Dim doc As Document = Application.DocumentManager.CurrentDocument
        doc.Editor.WriteMessage(vbLf & "Hello from the BusinessLogic.App.Initialize() Method." + vbLf)
    End Sub
End Class
