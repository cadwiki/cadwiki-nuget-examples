using Microsoft.VisualBasic;

namespace BusinessLogic
{

    public class App
    {
        public static void Initialize()
        {
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.CurrentDocument;
            doc.Editor.WriteMessage(Constants.vbLf + "Hello from the BusinessLogic.App.Initialize() Method." + Constants.vbLf);
        }
    }
}