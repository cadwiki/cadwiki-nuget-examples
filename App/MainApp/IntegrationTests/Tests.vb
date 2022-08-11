Option Strict On
Option Infer Off
Option Explicit On

Imports NUnit.Framework

Namespace IntegrationTests
    <TestFixture>
    Public Class Tests

        <SetUp>
        Public Sub Init()
            Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.SendStringToExecute("(vla-startundomark (vla-get-ActiveDocument (vlax-get-acad-object)))" + vbLf, True, False, False)
        End Sub

        <TearDown>
        Public Sub TearDown()
            Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.SendStringToExecute("(vla-endundomark (vla-get-ActiveDocument (vlax-get-acad-object)))" + vbLf, True, False, False)
            Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument.SendStringToExecute("(command-s ""._undo"" ""back"" ""yes"")" + vbLf, True, False, False)


        End Sub

        <Test>
        Public Sub Test_1()
            Assert.AreEqual(1, 1, "Test failed")
        End Sub


    End Class
End Namespace
