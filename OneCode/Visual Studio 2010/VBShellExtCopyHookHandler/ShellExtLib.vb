'****************************** Module Header ******************************'
' Module Name:  ShellExtLib.vb
' Project:      VBShellExtCopyHookHandler
' Copyright (c) Microsoft Corporation.
' 
' The file declares the imported Shell interfaces: ICopyHookW, implements the 
' helper functions for registering and unregistering a shell copy hook 
' handler, and declares the Win32 enums, structs, consts, and functions used 
' by the code sample.
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
Imports Microsoft.Win32


#Region "Shell Interfaces"

<ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), _
Guid("000214FC-0000-0000-C000-000000000046")> _
Friend Interface ICopyHookW

    ''' <summary>
    ''' Determines whether the Shell will be allowed to move, copy, delete, or 
    ''' rename a folder or printer object.
    ''' </summary>
    ''' <param name="hwnd">
    ''' A window handle to the dialog box to display information about the status 
    ''' of the file operation.
    ''' </param>
    ''' <param name="fileOperation">
    ''' A value that indicates which operation to perform. 
    ''' </param>
    ''' <param name="flags">
    ''' The flags that control the operation. 
    ''' * For folder copy hooks, this parameter can be one ore more of the values 
    '''   in FILEOP_FLAGS.
    ''' * For printer copy hooks, this value is one of the values defined in 
    '''   PRINTEROP.
    ''' </param>
    ''' <param name="srcFile">
    ''' A string that contains the name of the source folder.
    ''' </param>
    ''' <param name="srcAttribs">
    ''' The attributes of the source folder. 
    ''' </param>
    ''' <param name="destFile">
    ''' A string that contains the name of the destination folder.
    ''' </param>
    ''' <param name="destAttribs">
    ''' The attributes of the destination folder.
    ''' </param>
    ''' <returns>
    ''' Returns an integer value that indicates whether the Shell should perform 
    ''' the operation. One of the following: 
    ''' * DialogResult.Yes - Allows the operation.
    ''' * DialogResult.No - Prevents the operation on this folder but continues 
    '''   with any other operations that have been approved (for example, a batch 
    '''   copy operation).
    ''' * DialogResult.Cancel - Prevents the current operation and cancels any 
    '''   pending operations.
    ''' </returns>
    <PreserveSig()> _
    Function CopyCallback( _
        ByVal hwnd As IntPtr, _
        ByVal fileOperation As FILEOP, _
        ByVal flags As UInt32, _
        <MarshalAs(UnmanagedType.LPWStr)> _
        ByVal srcFile As String, _
        ByVal srcAttribs As UInt32, _
        <MarshalAs(UnmanagedType.LPWStr)> _
        ByVal destFile As String, _
        ByVal destAttribs As UInt32) _
    As UInt32
End Interface

#End Region


#Region "Shell Registration"

Friend Class ShellExtReg

    ''' <summary>
    ''' Register the folder copy hook handler.
    ''' </summary>
    ''' <param name="name">The name of the copy hook handler.</param>
    ''' <param name="clsid">The CLSID of the component.</param>
    Public Shared Sub RegisterShellExtFolderCopyHookHandler( _
        ByVal name As String, ByVal clsid As Guid)

        If String.IsNullOrEmpty(name) Then
            Throw New ArgumentException("name must not be empty")
        End If
        If (clsid = Guid.Empty) Then
            Throw New ArgumentException("clsid must not be empty")
        End If

        ' Create the key HKCR\Directory\shellex\CopyHookHandlers\<Name>.
        Dim subkey As String = String.Format("Directory\ShellEx\CopyHookHandlers\{0}", name)
        Using key As RegistryKey = Registry.ClassesRoot.CreateSubKey(subkey)
            ' Set the default value of the key.
            key.SetValue(Nothing, clsid.ToString("B"))
        End Using
    End Sub

    ''' <summary>
    ''' Unregister the folder copy hook handler.
    ''' </summary>
    ''' <param name="name">The name of the copy hook handler.</param>
    Public Shared Sub UnregisterShellExtFolderCopyHookHandler(ByVal name As String)
        If String.IsNullOrEmpty(name) Then
            Throw New ArgumentException("name must not be null")
        End If

        ' Remove the key HKCR\Directory\shellex\CopyHookHandlers\<Name>.
        Dim ex As String = String.Format("Directory\ShellEx\CopyHookHandlers\{0}", name)
        Registry.ClassesRoot.DeleteSubKeyTree(ex, False)
    End Sub

End Class

#End Region


#Region "Enums & Structs"

Public Enum FILEOP
    Move = 1
    Copy = 2
    Delete = 3
    Rename = 4
End Enum

''' <summary>
''' Flags that control the file operation. 
''' </summary>
<Flags()> _
Friend Enum FILEOP_FLAGS
    FOF_MULTIDESTFILES = 1
    FOF_CONFIRMMOUSE = 2

    ''' <summary>
    ''' Don't display progress UI (confirm prompts may be displayed still).
    ''' </summary>
    FOF_SILENT = 4

    ''' <summary>
    ''' Automatically rename the source files to avoid the collisions.
    ''' </summary>
    FOF_RENAMEONCOLLISION = 8

    ''' <summary>
    ''' Don't display confirmation UI, assume "yes" for cases that can be 
    ''' bypassed, "no" for those that can not.
    ''' </summary>
    FOF_NOCONFIRMATION = &H10

    ''' <summary>
    ''' Fill in SHFILEOPSTRUCT.hNameMappings.
    ''' Must be freed using SHFreeNameMappings.
    ''' </summary>
    FOF_WANTMAPPINGHANDLE = &H20

    ''' <summary>
    ''' Enable undo including Recycle behavior for IFileOperation::Delete().
    ''' </summary>
    FOF_ALLOWUNDO = &H40

    ''' <summary>
    ''' Only operate on the files (non folders), both files and folders are 
    ''' assumed without this.
    ''' </summary>
    FOF_FILESONLY = &H80

    ''' <summary>
    ''' Means don't show names of files.
    ''' </summary>
    FOF_SIMPLEPROGRESS = &H100

    ''' <summary>
    ''' Don't dispplay confirmatino UI before making any needed directories, 
    ''' assume "Yes" in these cases.
    ''' </summary>
    FOF_NOCONFIRMMKDIR = &H200

    ''' <summary>
    ''' Don't put up error UI, other UI may be displayed, progress, confirmations.
    ''' </summary>
    FOF_NOERRORUI = &H400

    ''' <summary>
    ''' Don't copy file security attributes (ACLs).
    ''' </summary>
    FOF_NOCOPYSECURITYATTRIBS = &H800

    ''' <summary>
    ''' Don't recurse into directories for operations that would recurse.
    ''' </summary>
    FOF_NORECURSION = &H1000

    ''' <summary>
    ''' Don't operate on connected elements ("xxx_files" folders that go with 
    ''' .htm files).
    ''' </summary>
    FOF_NO_CONNECTED_ELEMENTS = &H2000

    ''' <summary>
    ''' During delete operation, warn if nuking instead of recycling (partially 
    ''' overrides FOF_NOCONFIRMATION).
    ''' </summary>
    FOF_WANTNUKEWARNING = &H4000

    ''' <summary>
    ''' Deprecated; the operations engine always does the right thing on 
    ''' FolderLink objects (symlinks, reparse points, folder shortcuts)
    ''' </summary>
    FOF_NORECURSEREPARSE = &H8000

    ''' <summary>
    ''' Don't display any UI at all.
    ''' </summary>
    FOF_NO_UI = FOF_SILENT Or FOF_NOCONFIRMATION Or FOF_NOERRORUI Or FOF_NOCONFIRMMKDIR
End Enum

Friend Enum PRINTEROP
    Delete = &H13
    Rename = &H14
    PortChange = &H20
    RenPort = &H34
End Enum

#End Region