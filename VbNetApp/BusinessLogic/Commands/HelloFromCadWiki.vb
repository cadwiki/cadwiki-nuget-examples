Option Strict On
Option Infer Off
Option Explicit On
Imports Autodesk.AutoCAD.ApplicationServices
Imports Autodesk.AutoCAD.EditorInput
Imports Autodesk.AutoCAD.Windows
Imports cadwiki.WpfUi

Namespace Commands
    Public Class HelloFromCadWiki
        Public Class UserInput
            Public Selection As SelectionSet
        End Class

        Private Shared Function GetUserInput() As UserInput
            Return New UserInput()
        End Function

        Public Class Result
            Public Message As String
        End Class


        ' Keep this Run() method as short as possible
        ' Do all your logic in the RunWithUserInputMethod below

        ' IMPORTANT
        ' There can only be one Run() Method per command class that is invoked by the RibbonButtonCommandHandler
        ' If there are multiple Run() Methods in the same class, with different arguments/return values,
        ' Line 52 in the RibbonButtonCommandHandler will fail due to methodInfo.Invoke not being able to find the right Run() method
        Public Shared Function Run() As Result
            Try
                Dim doc As Document = Application.DocumentManager.MdiActiveDocument
                doc.Editor.WriteMessage("Hello from BusinessLogic.Commands.HelloFromCadWiki.Run()" + vbLf)
                Dim userInput As UserInput = GetUserInput()
                If userInput Is Nothing Then
                    doc.Editor.WriteMessage("User Input is invalid, exiting Example" + vbLf)
                Else
                    Return RunWithUserInput(userInput)
                End If
            Catch ex As Exception
                Commands.AutoCADExceptions.Handle(ex)
            End Try
            Return Nothing
        End Function


        ' IMPORTANT
        ' This Function name must differ from the one above due to how the RibbonButtonCommandHandler implementation works
        ' Line 52 in RibbonButtonCommandHandler routes the method calls to the correct assembly
        ' If there are two methods with Run() as their name,
        ' Line 52 in RibbonButtonCommandHandler methodInfo.Invoke(o, uiRouter.Parameters) will fail
        Public Shared Function RunWithUserInput(userInput As UserInput) As Result
            Try
                'Do whatever business logic is required with the user input here
                Dim wpfWindow As cadwiki.WpfUi.WindowCadWiki = New cadwiki.WpfUi.WindowCadWiki("This string was sent from the BusinessLogic project.")
                wpfWindow.Show()
                Return New Result()
            Catch ex As Exception
                Commands.AutoCADExceptions.Handle(ex)
            End Try
            Return Nothing
        End Function
    End Class

End Namespace
