'****************************** Module Header ******************************'
' Module Name:  FolderCopyHook.vb
' Project:      VBShellExtCopyHookHandler
' Copyright (c) Microsoft Corporation.
' 
' The FolderCopyHook.vb file defines a folder copy hook handler by 
' implementing the ICopyHook interface.
' 
' This source is subject to the Microsoft Public License.
' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
' All other rights reserved.
' 
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'***************************************************************************'

Imports System.Runtime.InteropServices
Imports System.Windows.Forms


<ClassInterface(ClassInterfaceType.None), _
Guid("FE289D2A-8D8D-4927-8069-44852CBF88F5"), ComVisible(True)> _
Public Class FolderCopyHook
    Implements ICopyHookW

#Region "Shell Extension Registration"

    <ComRegisterFunction()> _
    Public Shared Sub Register(ByVal t As Type)
        Try
            ShellExtReg.RegisterShellExtFolderCopyHookHandler("VBShellExtCopyHookHandler", t.GUID)
        Catch regService As Exception
            Console.WriteLine(regService.Message) ' Log the error
            Throw ' Re-throw the exception
        End Try
    End Sub

    <ComUnregisterFunction()> _
    Public Shared Sub Unregister(ByVal t As Type)
        Try
            ShellExtReg.UnregisterShellExtFolderCopyHookHandler("VBShellExtCopyHookHandler")
        Catch regService As Exception
            Console.WriteLine(regService.Message) ' Log the error
            Throw ' Re-throw the exception
        End Try
    End Sub

#End Region


#Region "ICopyHookW Members"

    Public Function CopyCallback( _
        ByVal hwnd As IntPtr, _
        ByVal fileOperation As FILEOP, _
        ByVal flags As UInt32, _
        ByVal srcFile As String, _
        ByVal srcAttribs As UInt32, _
        ByVal destFile As String, _
        ByVal destAttribs As UInt32) _
    As UInt32 Implements ICopyHookW.CopyCallback

        Dim owner As New NativeWindow
        owner.AssignHandle(hwnd)
        Try
            Dim result As DialogResult = DialogResult.Yes

            ' If the file name contains "Test" and it is being renamed...
            If (srcFile.Contains("Test") AndAlso (fileOperation = FILEOP.Rename)) Then
                result = MessageBox.Show(owner, _
                    String.Format("Are you sure to rename the folder {0} as {1} ?", _
                    srcFile, destFile), "VBShellExtCopyHookHandler", _
                    MessageBoxButtons.YesNoCancel)
            End If

            Debug.Assert(result = DialogResult.Yes OrElse result = DialogResult.No _
                OrElse result = DialogResult.Cancel)
            Return UInteger.Parse(result)
        Finally
            owner.ReleaseHandle()
        End Try
    End Function

#End Region

End Class