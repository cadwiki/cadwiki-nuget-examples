using Autodesk.AutoCAD.Runtime;
using System.Threading.Tasks;

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

        [CommandMethod(Constants.GroupName, "MyTests", CommandFlags.Session)]
        public static async Task MyTestsAsync()
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;

            var allRegressionTests = typeof(RegressionTests.RegressionTests);
            var allIntegrationTests = typeof(IntegrationTests.Tests);
            var allRegressionTestTypes = new[] { allRegressionTests, allIntegrationTests };

            var workflow = new Workflows.NUnitTestRunner();
            await workflow.Run(allRegressionTestTypes);

        }
    }
}