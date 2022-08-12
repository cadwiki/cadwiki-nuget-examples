Option Strict On
Option Infer Off
Option Explicit On

Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.EditorInput
Imports NUnit.Framework
Imports System.Reflection

Namespace Commands
    Public Class NunitTestRunner

        Public Sub Run()
            Try


                Dim doc As Document = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument
                Dim ed As Editor = doc.Editor


                Dim suiteResult As New IntegrationTests.ObservableTestSuiteResults
                Dim window As IntegrationTests.WindowTestRunner = New IntegrationTests.WindowTestRunner(suiteResult)

                'https://forums.autodesk.com/t5/net/how-to-set-a-focus-to-autocad-main-window-from-my-form-of-c-net/td-p/4680059
                Application.ShowModelessWindow(window)
                window.AddResult()
                Dim stopWatch As Stopwatch = New Stopwatch()
                stopWatch.Start()
                Dim integrationTestsType As Type = GetType(IntegrationTests.Tests)
                Dim integrationTestTypes As Type() = {integrationTestsType}
                IntegrationTests.UiLogic.RunTestsFromType(window.ObservableResults, stopWatch, integrationTestTypes)
                window.UpdateResult()
            Catch ex As Exception
                Debug.WriteLine("Exception: " + ex.Message)
            End Try

        End Sub


    End Class

End Namespace
