using System;
using System.Diagnostics;

namespace MainApp.Commands
{
    public class NunitTestRunner
    {

        public void Run()
        {
            try
            {


                var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
                var ed = doc.Editor;


                var suiteResult = new IntegrationTests.ObservableTestSuiteResults();
                var window = new IntegrationTests.WindowTestRunner(suiteResult);

                // https://forums.autodesk.com/t5/net/how-to-set-a-focus-to-autocad-main-window-from-my-form-of-c-net/td-p/4680059
                Autodesk.AutoCAD.ApplicationServices.Core.Application.ShowModelessWindow(window);
                window.AddResult();
                var stopWatch = new Stopwatch();
                stopWatch.Start();
                var integrationTestsType = typeof(IntegrationTests.Tests);
                var integrationTestTypes = new[] { integrationTestsType };
                IntegrationTests.UiLogic.RunTestsFromType(window.ObservableResults, stopWatch, integrationTestTypes);
                window.UpdateResult();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception: " + ex.Message);
            }

        }


    }

}