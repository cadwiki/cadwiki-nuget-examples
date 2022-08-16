Option Strict On
Option Infer Off
Option Explicit On

Imports Autodesk.AutoCAD.Geometry
Imports NUnit.Framework

Namespace RegressionTests
    <TestFixture>
    Partial Public Class RegressionTests

        <Test>
        Public Sub Test_DoStringsMatch_ShouldPass()
            Dim expected As String = "Hello"
            Dim actual As String = "Hello"
            Assert.AreEqual(expected, actual, "Input strings don't match")
        End Sub

        <Test>
        Public Sub Test_DoStringsMatch_ShouldFail()
            Dim expected As String = "Hello"
            Dim actual As String = "World"
            Assert.AreEqual(expected, actual, "Input strings don't match")
        End Sub


    End Class
End Namespace
