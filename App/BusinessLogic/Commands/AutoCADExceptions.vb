Option Strict On
Option Infer Off
Option Explicit On

Imports Autodesk.AutoCAD.ApplicationServices

Namespace Commands
    Public Class AutoCADExceptions

        Public Shared Sub Handle(ex As Exception)

            Dim doc As Document = Application.DocumentManager.MdiActiveDocument
            Dim stackTraceStringList As List(Of String) = cadwiki.NetUtils.Exceptions.GetStackTraceLines(ex)
            Dim stackTraceString As String = cadwiki.NetUtils.Lists.StringListToString(stackTraceStringList, vbLf)
            For Each str As String In stackTraceStringList
                doc.Editor.WriteMessage(vbLf & str)
            Next
            doc.Editor.WriteMessage(vbLf & "Exception :" & ex.Message)
            doc.Editor.WriteMessage(vbLf & vbLf)
        End Sub

    End Class

End Namespace
