using System;
using Autodesk.AutoCAD.EditorInput;
using cadwiki.WpfUi;
using Microsoft.VisualBasic;

namespace BusinessLogic.Commands
{
    public class HelloFromCadWiki
    {
        public class UserInput
        {
            public SelectionSet Selection;
        }

        private static UserInput GetUserInput()
        {
            return new UserInput();
        }

        public class Result
        {
            public string Message;
        }


        // Keep this Run() method as short as possible
        // Do all your logic in the RunWithUserInputMethod below

        // IMPORTANT
        // There can only be one Run() Method per command class that is invoked by the RibbonButtonCommandHandler
        // If there are multiple Run() Methods in the same class, with different arguments/return values,
        // Line 52 in the RibbonButtonCommandHandler will fail due to methodInfo.Invoke not being able to find the right Run() method
        public static Result Run()
        {
            try
            {
                var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
                doc.Editor.WriteMessage("Hello from BusinessLogic.Commands.HelloFromCadWiki.Run()" + Constants.vbLf);
                var userInput = GetUserInput();
                if (userInput == null)
                {
                    doc.Editor.WriteMessage("User Input is invalid, exiting Example" + Constants.vbLf);
                }
                else
                {
                    return RunWithUserInput(userInput);
                }
            }
            catch (Exception ex)
            {
                AutoCADExceptions.Handle(ex);
            }
            return null;
        }


        // IMPORTANT
        // This Function name must differ from the one above due to how the RibbonButtonCommandHandler implementation works
        // Line 52 in RibbonButtonCommandHandler routes the method calls to the correct assembly
        // If there are two methods with Run() as their name,
        // Line 52 in RibbonButtonCommandHandler methodInfo.Invoke(o, uiRouter.Parameters) will fail
        public static Result RunWithUserInput(UserInput userInput)
        {
            try
            {
                // Do whatever business logic is required with the user input here
                var wpfWindow = new WindowCadWiki("This string was sent from the BusinessLogic project.");
                wpfWindow.ShowDialog();
                return new Result();
            }
            catch (Exception ex)
            {
                AutoCADExceptions.Handle(ex);
            }
            return null;
        }
    }

}