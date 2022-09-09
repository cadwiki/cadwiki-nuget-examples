using Autodesk.AutoCAD.Runtime;

// This decorator tells the AutoCAD runtime to load the entire CommandDrivers class and look for <CommandMethods> to load as callable from the AutoCAD command line
[assembly: CommandClass(typeof(MainApp.CommandDrivers))]

namespace MainApp
{
    public class CommandDrivers
    {
        public class Constants
        {
            public const string GroupName = "MainAppCommands";
        }


        [CommandMethod(Constants.GroupName, "MyCommand", CommandFlags.Session)]
        public static void CommandMethod()
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            doc.Editor.WriteMessage("Hello from the command drivers class" + Microsoft.VisualBasic.Constants.vbLf);
            BusinessLogic.Commands.Example.Run();
        }

    }
}