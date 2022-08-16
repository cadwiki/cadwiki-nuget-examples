Option Strict On
Option Infer Off
Option Explicit On

Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.EditorInput
Imports cadwiki.NUnitTestRunner


Namespace Workflows
    Public Class NUnitTestRunner
        Public Sub Run(num As Integer, regressionTestTypes As Type())
            Dim doc As Document = Application.DocumentManager.MdiActiveDocument
            Dim ed As Editor = doc.Editor
            Try
                Dim results As New Results.ObservableTestSuiteResults()
                Dim driver As New Ui.Driver(results, regressionTestTypes)
                Dim window As Ui.WindowTestRunner = driver.GetWindow()
                'https://forums.autodesk.com/t5/net/how-to-set-a-focus-to-autocad-main-window-from-my-form-of-c-net/td-p/4680059
                Application.ShowModelessWindow(window)
                driver.ExecuteTests()
            Catch ex As Exception
                ed.WriteMessage(vbLf + "Exception: " + ex.Message)
            End Try
        End Sub
    End Class
End Namespace