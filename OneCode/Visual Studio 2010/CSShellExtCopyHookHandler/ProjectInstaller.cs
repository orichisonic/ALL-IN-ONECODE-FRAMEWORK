/********************************** Module Header **********************************\
Module Name:  ProjectInstaller.cs
Project:      CSShellExtCopyHookHandler
Copyright (c) Microsoft Corporation.

The installer class defines the custom actions in the setup. We use the custom 
actions to register and unregister the COM-visible classes in the current managed 
assembly.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***********************************************************************************/

#region Using directives
using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Runtime.InteropServices;
using System.Windows.Forms;
#endregion


namespace CSShellExtCopyHookHandler
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            // Call RegistrationServices.RegisterAssembly to register the classes in 
            // the current managed assembly to enable creation from COM.
            RegistrationServices regService = new RegistrationServices();
            regService.RegisterAssembly(
                this.GetType().Assembly,
                AssemblyRegistrationFlags.SetCodeBase);

            // Inform the user to restart Windows Explorer (explorer.exe).
            // The shell builds and caches a list of registered copy hook handlers the 
            // first time copy hook handlers are called in a process. Once the list is 
            // created, there is no mechanism for updating or flushing the cache other 
            // than terminating the process. The best option that we can offer at this 
            // point is to restart Windows Explorer after the copy hook handler is 
            // registered.
            MessageBox.Show("You need to restart Windows Explorer (explorer.exe) " +
                "to load the copy handler.  Please restart explorer.exe manually " +
                "after the installation.", "Restart Windows Explorer");
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            // Call RegistrationServices.UnregisterAssembly to unregister the classes 
            // in the current managed assembly.
            RegistrationServices regService = new RegistrationServices();
            regService.UnregisterAssembly(this.GetType().Assembly);
        }
    }
}