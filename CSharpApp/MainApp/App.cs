using System;
using System.Reflection;
using Autodesk.AutoCAD.Runtime;
using cadwiki.DllReloader.AutoCAD;
using Microsoft.VisualBasic;

namespace MainApp
{

    public class App : IExtensionApplication
    {

        public static AutoCADAppDomainDllReloader AcadAppDomainDllReloader = new AutoCADAppDomainDllReloader();

        public void Initialize()
        {
            // This Event Handler allows the IExtensionApplication to Resolve any Assemblies
            // The AssemblyResolve method finds the correct assembly in the AppDomain when there are multiple assemblies
            // with the same name and differing version number
            AppDomain.CurrentDomain.AssemblyResolve += AutoCADAppDomainDllReloader.AssemblyResolve;
            var iExtensionAppAssembly = Assembly.GetExecutingAssembly();
            var iExtensionAppVersion = cadwiki.NetUtils.AssemblyUtils.GetVersion(iExtensionAppAssembly);
            AcadAppDomainDllReloader.Configure(iExtensionAppAssembly);
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            doc.Editor.WriteMessage(Constants.vbLf + "App " + iExtensionAppVersion.ToString() + " initialized...");
            doc.Editor.WriteMessage(Constants.vbLf);
            UiRibbon.Tabs.TabCreator.AddTabs(doc);
            BusinessLogic.App.Initialize();
        }


        public void Terminate()
        {
            AcadAppDomainDllReloader.Terminate();
        }

    }
}