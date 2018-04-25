'****************************** Module Header ******************************'
' Module Name:  ProjectInstaller.vb
' Project:      VBShellExtCopyHookHandler
' Copyright (c) Microsoft Corporation.
' 
' The installer class defines the custom actions in the setup. We use the 
' custom actions to register and unregister the COM-visible classes in the 
' current managed assembly.
' 
' This source is subject to the Microsoft Public License.
' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
' All other rights reserved.
' 
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'***************************************************************************'

Imports System.Configuration.Install
Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.Windows.Forms


<RunInstaller(True), ComVisible(False)> _
Public Class ProjectInstaller
    Inherits Installer

    Public Overrides Sub Install(ByVal stateSaver As IDictionary)
        MyBase.Install(stateSaver)

        ' Call RegistrationServices.RegisterAssembly to register the classes 
        ' in the current managed assembly to enable creation from COM.
        Dim regService As New RegistrationServices
        regService.RegisterAssembly( _
            Me.GetType.Assembly, _
            AssemblyRegistrationFlags.SetCodeBase)

        ' Inform the user to restart Windows Explorer (explorer.exe).
        ' The shell builds and caches a list of registered copy hook handlers 
        ' the first time copy hook handlers are called in a process. Once the 
        ' list is created, there is no mechanism for updating or flushing the 
        ' cache other than terminating the process. The best option that we 
        ' can offer at this point is to restart Windows Explorer after the 
        ' copy hook handler is registered.
        MessageBox.Show("You need to restart Windows Explorer (explorer.exe) " & _
            "to load the copy handler.  Please restart explorer.exe manually " & _
            "after the installation.", "Restart Windows Explorer")
    End Sub

    Public Overrides Sub Uninstall(ByVal savedState As IDictionary)
        MyBase.Uninstall(savedState)

        ' Call RegistrationServices.UnregisterAssembly to unregister the 
        ' classes in the current managed assembly.
        Dim regService As New RegistrationServices
        regService.UnregisterAssembly(Me.GetType.Assembly)
    End Sub

End Class