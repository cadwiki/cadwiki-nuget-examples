using System;
using Microsoft.VisualBasic;


namespace MainApp.Workflows
{
    public class NUnitTestRunner
    {
        public void Run(Type[] regressionTestTypes)
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            var ed = doc.Editor;
            try
            {
                var results = new cadwiki.NUnitTestRunner.Results.ObservableTestSuiteResults();
                var driver = new cadwiki.NUnitTestRunner.Ui.Driver(results, regressionTestTypes);
                var window = driver.GetWindow();
                // https://forums.autodesk.com/t5/net/how-to-set-a-focus-to-autocad-main-window-from-my-form-of-c-net/td-p/4680059
                Autodesk.AutoCAD.ApplicationServices.Core.Application.ShowModelessWindow(window);
                driver.ExecuteTests();
            }
            catch (Exception ex)
            {
                ed.WriteMessage(Constants.vbLf + "Exception: " + ex.Message);
            }
        }
    }
}