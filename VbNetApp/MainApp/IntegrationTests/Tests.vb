Imports Autodesk.Windows

Imports NUnit.Framework
Imports cadwiki.NUnitTestRunner.TestEvidence
Imports cadwiki.NUnitTestRunner.Creators
Imports System.IO

Namespace MainApp.IntegrationTests
    <TestFixture>
    Public Class Tests

        <SetUp>
        Public Sub Init()
            Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute("(vla-startundomark (vla-get-ActiveDocument (vlax-get-acad-object)))" & vbLf, True, False, False)
        End Sub

        <TearDown>
        Public Sub TearDown()
            Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute("(vla-endundomark (vla-get-ActiveDocument (vlax-get-acad-object)))" & vbLf, True, False, False)
            Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument.SendStringToExecute("(command-s ""._undo"" ""back"" ""yes"")" & vbLf, True, False, False)
        End Sub

        <Test>
        Public Sub Test_Is1EqualTo1_ShouldPass()
            Assert.AreEqual(1, 1, "Test failed")
        End Sub




        <Test>
        Public Sub Test_ClickUiRibbonHelloWorld_ShouldAddScreenshotToPdf()
            Dim ribbonControl = ComponentManager.Ribbon
            Dim appTab As RibbonTab = ribbonControl.FindTab("AppTab")
            Dim examplePanel As RibbonPanel = appTab.FindPanel(UiRibbon.Panels.Example.Id)
            Dim item As RibbonItem = examplePanel.FindItem(UiRibbon.Panels.ExampleButtons.HelloButtonId)
            Dim ribbonButton As RibbonButton = CType(item, RibbonButton)
            'simulate a Ui click by calling Execute on the Ribbon button command handler
            ribbonButton.CommandHandler.Execute(ribbonButton)
            Call Windows.Forms.Application.DoEvents()

            Dim testEvidenceCreator = New TestEvidenceCreator()
            Dim windowIntPtr As IntPtr = testEvidenceCreator.ProcessesGetHandleFromUiTitle("Hello from Cadwiki v53")
            Dim evidence = New Evidence()
            testEvidenceCreator.TakeJpegScreenshot(windowIntPtr, "Title")
            Dim controlName = "ButtonOk"
            testEvidenceCreator.MicrosoftTestClickUiControl(windowIntPtr, controlName)

            evidence = testEvidenceCreator.GetEvidenceForCurrentTest()
            Assert.IsTrue(File.Exists(evidence.Images(0).FilePath), "jpeg was not created.")

            Assert.AreEqual(1, 1, "Test failed")
        End Sub

        <Test>
        Public Sub Test_ClickUiRibbonHelloWorld_ShouldAddSecondScreenShotToPdf()
            Dim ribbonControl = ComponentManager.Ribbon
            Dim appTab As RibbonTab = ribbonControl.FindTab("AppTab")
            Dim examplePanel As RibbonPanel = appTab.FindPanel(UiRibbon.Panels.Example.Id)
            Dim item As RibbonItem = examplePanel.FindItem(UiRibbon.Panels.ExampleButtons.HelloButtonId)
            Dim ribbonButton As RibbonButton = CType(item, RibbonButton)
            'simulate a Ui click by calling Execute on the Ribbon button command handler
            ribbonButton.CommandHandler.Execute(ribbonButton)
            Call Windows.Forms.Application.DoEvents()

            Dim testEvidenceCreator = New TestEvidenceCreator()
            Dim windowIntPtr As IntPtr = testEvidenceCreator.ProcessesGetHandleFromUiTitle("Hello from Cadwiki v53")
            testEvidenceCreator.TakeJpegScreenshot(windowIntPtr, "Title")
            Dim controlName = "ButtonOk"
            testEvidenceCreator.MicrosoftTestClickUiControl(windowIntPtr, controlName)
            Dim evidence = testEvidenceCreator.GetEvidenceForCurrentTest()
            Assert.IsTrue(File.Exists(evidence.Images(0).FilePath), "jpeg was not created.")
        End Sub

        <Test>
        Public Async Function Test_LongRunningLineDraw_ShouldAddSecondScreenShotToPdf() As Task(Of Object)
            Await DelayedWork()
            Dim doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument
            Dim parameters As List(Of Object) = New List(Of Object)() From {
    "_.LINE",
    "0,0",
    "1,1",
    ""
}

            Await Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.ExecuteInCommandContextAsync(Async Function(obj)
                                                                                                                         Await doc.Editor.CommandAsync(parameters.ToArray())
                                                                                                                     End Function, Nothing)

            parameters = New List(Of Object)() From {
    "_.ZOOM",
    "EXTENTS"
}

            Await Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.ExecuteInCommandContextAsync(Async Function(obj)
                                                                                                                         Await doc.Editor.CommandAsync(parameters.ToArray())
                                                                                                                     End Function, Nothing)

            Dim testEvidenceCreator = New TestEvidenceCreator()
            Dim windowIntPtr As IntPtr = testEvidenceCreator.ProcessesGetHandleFromUiTitle("Autodesk AutoCAD")
            testEvidenceCreator.TakeJpegScreenshot(windowIntPtr, "After draw line async")
            Dim evidence = testEvidenceCreator.GetEvidenceForCurrentTest()
            Assert.IsTrue(File.Exists(evidence.Images(0).FilePath), "jpeg was not created.")
            Return Nothing
        End Function

        Private Async Function DelayedWork() As Task
            Await Task.Delay(5000)
        End Function
    End Class






End Namespace


