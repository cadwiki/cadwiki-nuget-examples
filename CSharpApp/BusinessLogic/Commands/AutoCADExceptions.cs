using System;
using Microsoft.VisualBasic;

namespace BusinessLogic.Commands
{
    public class AutoCADExceptions
    {

        public static void Handle(Exception ex)
        {

            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            var stackTraceStringList = cadwiki.NetUtils.Exceptions.GetStackTraceLines(ex);
            string stackTraceString = cadwiki.NetUtils.Lists.StringListToString(stackTraceStringList, Environment.NewLine);
            foreach (string str in stackTraceStringList)
                doc.Editor.WriteMessage(Environment.NewLine + str);
            doc.Editor.WriteMessage(Environment.NewLine + "Exception :" + ex.Message);
            doc.Editor.WriteMessage(Environment.NewLine + Environment.NewLine);
        }

    }

}