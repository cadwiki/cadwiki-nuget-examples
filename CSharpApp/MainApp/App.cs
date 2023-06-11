using System;
using System.Reflection;
using Autodesk.AutoCAD.Runtime;
using cadwiki.DllReloader.AutoCAD;
using Microsoft.VisualBasic;

namespace MainApp
{

    public class App : IExtensionApplication
    {

        //start here 1 - AutoCADAppDomainDllReloader
        //this variable handles routing the Ui clicks on a AutoCAD ribbon button to your methods found in an Assembly
        public static AutoCADAppDomainDllReloader AcadAppDomainDllReloader = new AutoCADAppDomainDllReloader();

        //start here 2 - IExtensionApplication.Initialize
        //once the AcadAppDomainDllReloader is configured with the current Assembly, it will be able to route Ui clicks
        //to the correct method
        public void Initialize()
        {
            // This Event Handler allows the IExtensionApplication to Resolve any Assemblies
            // The AssemblyResolve method finds the correct assembly in the AppDomain when there are multiple assemblies
            // with the same name and differing version number
            AppDomain.CurrentDomain.AssemblyResolve += AutoCADAppDomainDllReloader.AssemblyResolve;
            var iExtensionAppAssembly = Assembly.GetExecutingAssembly();
            var iExtensionAppVersion = cadwiki.NetUtils.AssemblyUtils.GetVersion(iExtensionAppAssembly);
            AcadAppDomainDllReloader.Configure(iExtensionAppAssembly);
            AcadAppDomainDllReloader.Reload(iExtensionAppAssembly);
            var doc = Autodesk.AutoCAD.ApplicationServices.Core.Application.DocumentManager.MdiActiveDocument;
            doc.Editor.WriteMessage(Constants.vbLf + "App " + iExtensionAppVersion.ToString() + " initialized...");
            doc.Editor.WriteMessage(Constants.vbLf);
            UiRibbon.Tabs.TabCreator.AddTabs(doc);
            BusinessLogic.App.Initialize();
        }


        //start here 3 - IExtensionApplication.Terminate
        //add a call to terminate the AcadAppDomainDllReloader
        public void Terminate()
        {
            AcadAppDomainDllReloader.Terminate();
        }

    }
}