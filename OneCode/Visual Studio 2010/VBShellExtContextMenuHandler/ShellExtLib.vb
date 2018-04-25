'****************************** Module Header ******************************'
' Module Name:  ShellExtLib.vb
' Project:      VBShellExtContextMenuHandler
' Copyright (c) Microsoft Corporation.
' 
' The file declares the imported Shell interfaces: IShellExtInit and 
' IContextMenu, implements the helper functions for registering and 
' unregistering a shell context menu handler, and declares the Win32 enums, 
' structs, consts, and functions used by the code sample.
' 
' This source is subject to the Microsoft Public License.
' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
' All other rights reserved.
' 
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'***************************************************************************'

#Region "Imports directives"

Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Runtime.InteropServices.ComTypes
Imports Microsoft.Win32

#End Region


#Region "Shell Interfaces"

<ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), _
Guid("000214e8-0000-0000-c000-000000000046")> _
Friend Interface IShellExtInit
    Sub Initialize( _
        ByVal pidlFolder As IntPtr, _
        ByVal pDataObj As IntPtr, _
        ByVal hKeyProgID As IntPtr)
End Interface


<ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), _
Guid("000214e4-0000-0000-c000-000000000046")> _
Friend Interface IContextMenu

    <PreserveSig()> _
    Function QueryContextMenu( _
        ByVal hMenu As IntPtr, _
        ByVal iMenu As UInt32, _
        ByVal idCmdFirst As UInt32, _
        ByVal idCmdLast As UInt32, _
        ByVal uFlags As UInt32) _
    As Integer

    Sub InvokeCommand(ByVal pici As IntPtr)

    Sub GetCommandString( _
        ByVal idCmd As UIntPtr, _
        ByVal uFlags As UInt32, _
        ByVal pReserved As IntPtr, _
        ByVal pszName As StringBuilder, _
        ByVal cchMax As UInt32)

End Interface

#End Region


#Region "Shell Registration"

Friend Class ShellExtReg

    ''' <summary>
    ''' Register the context menu handler.
    ''' </summary>
    ''' <param name="clsid">The CLSID of the component.</param>
    ''' <param name="fileType">
    ''' The file type that the context menu handler is associated with. For 
    ''' example, '*' means all file types; '.txt' means all .txt files. The 
    ''' parameter must not be NULL or an empty string. 
    ''' </param>
    ''' <param name="friendlyName">Friendly name of the component</param>
    Public Shared Sub RegisterShellExtContextMenuHandler( _
        ByVal clsid As Guid, _
        ByVal fileType As String, _
        ByVal friendlyName As String)

        If clsid = Guid.Empty Then
            Throw New ArgumentException("clsid must not be empty")
        End If
        If String.IsNullOrEmpty(fileType) Then
            Throw New ArgumentException("fileType must not be null or empty")
        End If

        ' If fileType starts with '.', try to read the default value of 
        ' the HKCR\<File Type> key which contains the ProgID to which the 
        ' file type is linked.
        If (fileType.StartsWith(".")) Then
            Using key As RegistryKey = Registry.ClassesRoot.OpenSubKey(fileType)
                If (key IsNot Nothing) Then
                    ' If the key exists and its default value is not empty, 
                    ' use the ProgID as the file type.
                    Dim defaultVal As String = key.GetValue(Nothing)
                    If (Not String.IsNullOrEmpty(defaultVal)) Then
                        fileType = defaultVal
                    End If
                End If
            End Using
        End If

        ' Create the HKCR\<File Type>\shellex\ContextMenuHandlers\{<CLSID>} key.
        Dim keyName As String = String.Format("{0}\shellex\ContextMenuHandlers\{1}", _
            fileType, clsid.ToString("B"))
        Using key As RegistryKey = Registry.ClassesRoot.CreateSubKey(keyName)
            ' Set the default value of the key.
            If ((key IsNot Nothing) AndAlso _
                (Not String.IsNullOrEmpty(friendlyName))) Then
                key.SetValue(Nothing, friendlyName)
            End If
        End Using
    End Sub


    ''' <summary>
    ''' Unregister the context menu handler.
    ''' </summary>
    ''' <param name="clsid">The CLSID of the component.</param>
    ''' <param name="fileType">
    ''' The file type that the context menu handler is associated with. For 
    ''' example, '*' means all file types; '.txt' means all .txt files. The 
    ''' parameter must not be NULL or an empty string. 
    ''' </param>
    Public Shared Sub UnregisterShellExtContextMenuHandler( _
        ByVal clsid As Guid, ByVal fileType As String)

        If clsid = Guid.Empty Then
            Throw New ArgumentException("clsid must not be empty")
        End If
        If String.IsNullOrEmpty(fileType) Then
            Throw New ArgumentException("fileType must not be null or empty")
        End If

        ' If fileType starts with '.', try to read the default value of 
        ' the HKCR\<File Type> key which contains the ProgID to which the 
        ' file type is linked.
        If (fileType.StartsWith(".")) Then
            Using key As RegistryKey = Registry.ClassesRoot.OpenSubKey(fileType)
                If (key IsNot Nothing) Then
                    ' If the key exists and its default value is not empty, 
                    ' use the ProgID as the file type.
                    Dim defaultVal As String = key.GetValue(Nothing)
                    If (Not String.IsNullOrEmpty(defaultVal)) Then
                        fileType = defaultVal
                    End If
                End If
            End Using
        End If

        ' Remove the HKCR\<File Type>\shellex\ContextMenuHandlers\{<CLSID>} key.
        Dim keyName As String = String.Format("{0}\shellex\ContextMenuHandlers\{1}", _
            fileType, clsid.ToString("B"))
        Registry.ClassesRoot.DeleteSubKeyTree(keyName, False)
    End Sub

End Class

#End Region


#Region "Enums & Structs"

Friend Enum GCS As UInt32
    GCS_VERBA = 0
    GCS_HELPTEXTA = 1
    GCS_VALIDATEA = 2
    GCS_HELPTEXTW = 5
    GCS_UNICODE = 4
    GCS_VERBW = 4
    GCS_VALIDATEW = 6
    GCS_VERBICONW = 20
End Enum


<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
Friend Structure CMINVOKECOMMANDINFO
    Public cbSize As UInt32
    Public fMask As CMIC
    Public hwnd As IntPtr
    Public verb As IntPtr
    <MarshalAs(UnmanagedType.LPStr)> _
    Public parameters As String
    <MarshalAs(UnmanagedType.LPStr)> _
    Public directory As String
    Public nShow As Integer
    Public dwHotKey As UInt32
    Public hIcon As IntPtr
End Structure


<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
Friend Structure CMINVOKECOMMANDINFOEX
    Public cbSize As UInt32
    Public fMask As CMIC
    Public hwnd As IntPtr
    Public verb As IntPtr
    <MarshalAs(UnmanagedType.LPStr)> _
    Public parameters As String
    <MarshalAs(UnmanagedType.LPStr)> _
    Public directory As String
    Public nShow As Integer
    Public dwHotKey As UInt32
    Public hIcon As IntPtr
    <MarshalAs(UnmanagedType.LPStr)> _
    Public title As String
    Public verbW As IntPtr
    Public parametersW As String
    Public directoryW As String
    Public titleW As String
    Private ptInvoke As POINT
End Structure


<Flags()> _
Friend Enum CMIC As UInt32
    CMIC_MASK_ICON = &H10
    CMIC_MASK_HOTKEY = &H20
    CMIC_MASK_NOASYNC = &H100
    CMIC_MASK_FLAG_NO_UI = &H400
    CMIC_MASK_UNICODE = &H4000
    CMIC_MASK_NO_CONSOLE = &H8000
    CMIC_MASK_ASYNCOK = &H100000
    CMIC_MASK_NOZONECHECKS = &H800000
    CMIC_MASK_FLAG_LOG_USAGE = &H4000000
    CMIC_MASK_SHIFT_DOWN = &H10000000
    CMIC_MASK_PTINVOKE = &H20000000
    CMIC_MASK_CONTROL_DOWN = &H40000000
End Enum


<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
Public Structure POINT
    Public X As Integer
    Public Y As Integer
End Structure


Friend Enum CLIPFORMAT As UInt32
    CF_TEXT = 1
    CF_BITMAP = 2
    CF_METAFILEPICT = 3
    CF_SYLK = 4
    CF_DIF = 5
    CF_TIFF = 6
    CF_OEMTEXT = 7
    CF_DIB = 8
    CF_PALETTE = 9
    CF_PENDATA = 10
    CF_RIFF = 11
    CF_WAVE = 12
    CF_UNICODETEXT = 13
    CF_ENHMETAFILE = 14
    CF_HDROP = 15
    CF_LOCALE = &H10
    CF_MAX = &H11

    CF_OWNERDISPLAY = &H80
    CF_DSPTEXT = &H81
    CF_DSPBITMAP = &H82
    CF_DSPMETAFILEPICT = &H83
    CF_DSPENHMETAFILE = &H8E

    CF_PRIVATEFIRST = &H200
    CF_PRIVATELAST = &H2FF

    CF_GDIOBJFIRST = &H300
    CF_GDIOBJLAST = &H3FF
End Enum


<Flags()> _
Friend Enum CMF
    CMF_NORMAL = 0
    CMF_DEFAULTONLY = 1
    CMF_VERBSONLY = 2
    CMF_EXPLORE = 4
    CMF_NOVERBS = 8
    CMF_CANRENAME = &H10
    CMF_NODEFAULT = &H20
    CMF_INCLUDESTATIC = &H40
    CMF_ITEMMENU = &H80
    CMF_EXTENDEDVERBS = &H100
    CMF_DISABLEDVERBS = &H200
    CMF_ASYNCVERBSTATE = &H400
    CMF_OPTIMIZEFORINVOKE = &H800
    CMF_SYNCCASCADEMENU = &H1000
    CMF_DONOTPICKDEFAULT = &H2000
    CMF_RESERVED = &HFFFF0000
End Enum


<StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)> _
Friend Structure MENUITEMINFO
    Public cbSize As UInt32
    Public fMask As MIIM
    Public fType As MFT
    Public fState As MFS
    Public wID As UInt32
    Public hSubMenu As IntPtr
    Public hbmpChecked As IntPtr
    Public hbmpUnchecked As IntPtr
    Public dwItemData As UIntPtr
    Public dwTypeData As String
    Public cch As UInt32
    Public hbmpItem As IntPtr
End Structure


<Flags()> _
Friend Enum MIIM As UInt32
    MIIM_STATE = 1
    MIIM_ID = 2
    MIIM_SUBMENU = 4
    MIIM_CHECKMARKS = 8
    MIIM_TYPE = &H10
    MIIM_DATA = &H20
    MIIM_STRING = &H40
    MIIM_BITMAP = &H80
    MIIM_FTYPE = &H100
End Enum


Friend Enum MFT As UInt32
    MFT_STRING = 0
    MFT_BITMAP = 4
    MFT_MENUBARBREAK = &H20
    MFT_MENUBREAK = &H40
    MFT_OWNERDRAW = &H100
    MFT_RADIOCHECK = &H200
    MFT_SEPARATOR = &H800
    MFT_RIGHTORDER = &H2000
    MFT_RIGHTJUSTIFY = &H4000
End Enum


Friend Enum MFS As UInt32
    MFS_ENABLED = 0
    MFS_UNCHECKED = 0
    MFS_UNHILITE = 0
    MFS_DISABLED = 3
    MFS_GRAYED = 3
    MFS_CHECKED = 8
    MFS_HILITE = &H80
    MFS_DEFAULT = &H1000
End Enum

#End Region


Friend Class NativeMethods

    ''' <summary>
    ''' Retrieve the names of dropped files that result from a successful 
    ''' drag-and-drop operation.
    ''' </summary>
    ''' <param name="hDrop">
    ''' Identifier of the structure that contains the file names of the 
    ''' dropped files.
    ''' </param>
    ''' <param name="iFile">
    ''' Index of the file to query. If the value of this parameter is 
    ''' 0xFFFFFFFF, DragQueryFile returns a count of the files dropped. 
    ''' </param>
    ''' <param name="pszFile">
    ''' The address of a buffer that receives the file name of a dropped 
    ''' file when the function returns.
    ''' </param>
    ''' <param name="cch">
    ''' The size, in characters, of the pszFile buffer.
    ''' </param>
    ''' <returns>A non-zero value indicates a successful call.</returns>
    <DllImport("shell32", CharSet:=CharSet.Unicode)> _
    Public Shared Function DragQueryFile( _
        ByVal hDrop As IntPtr, _
        ByVal iFile As UInt32, _
        ByVal pszFile As StringBuilder, _
        ByVal cch As Integer) As UInt32
    End Function


    ''' <summary>
    ''' Free the specified storage medium.
    ''' </summary>
    ''' <param name="pmedium">
    ''' Reference of the storage medium that is to be freed.
    ''' </param>
    <DllImport("ole32.dll", CharSet:=CharSet.Unicode)> _
    Public Shared Sub ReleaseStgMedium(ByRef pmedium As STGMEDIUM)
    End Sub


    ''' <summary>
    ''' Insert a new menu item at the specified position in a menu.
    ''' </summary>
    ''' <param name="hMenu">
    ''' A handle to the menu in which the new menu item is inserted. 
    ''' </param>
    ''' <param name="uItem">
    ''' The identifier or position of the menu item before which to 
    ''' insert the new item. The meaning of this parameter depends on the 
    ''' value of fByPosition.
    ''' </param>
    ''' <param name="fByPosition">
    ''' Controls the meaning of uItem. If this parameter is false, uItem 
    ''' is a menu item identifier. Otherwise, it is a menu item position. 
    ''' </param>
    ''' <param name="mii">
    ''' A reference of a MENUITEMINFO structure that contains information 
    ''' about the new menu item.
    ''' </param>
    ''' <returns>
    ''' If the function succeeds, the return value is true.
    ''' </returns>
    <DllImport("user32", CharSet:=CharSet.Unicode, SetLastError:=True)> _
    Public Shared Function InsertMenuItem( _
        ByVal hMenu As IntPtr, _
        ByVal uItem As UInt32, _
        <MarshalAs(UnmanagedType.Bool)> ByVal fByPosition As Boolean, _
        ByRef mii As MENUITEMINFO) _
    As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function


    Public Shared Function HighWord(ByVal number As Integer) As Integer
        Return If(((number And &H80000000) = &H80000000), _
            (number >> &H10), ((number >> &H10) And &HFFFF))
    End Function


    Public Shared Function LowWord(ByVal number As Integer) As Integer
        Return (number And &HFFFF)
    End Function

End Class


Friend Class WinError

    Public Const S_OK As Integer = 0
    Public Const S_FALSE As Integer = 1
    Public Const E_FAIL As Integer = -2147467259
    Public Const E_INVALIDARG As Integer = -2147024809
    Public Const E_OUTOFMEMORY As Integer = -2147024882
    Public Const STRSAFE_E_INSUFFICIENT_BUFFER As Integer = -2147024774

    Public Const SEVERITY_ERROR As UInt32 = 1
    Public Const SEVERITY_SUCCESS As UInt32 = 0

    ''' <summary>
    ''' Create an HRESULT value from component pieces.
    ''' </summary>
    ''' <param name="sev">The severity to be used</param>
    ''' <param name="fac">The facility to be used</param>
    ''' <param name="code">The error number</param>
    ''' <returns>A HRESULT constructed from the above 3 values</returns>
    Public Shared Function MAKE_HRESULT( _
        ByVal sev As UInt32, _
        ByVal fac As UInt32, _
        ByVal code As UInt32) As Integer
        Return CInt((((sev << &H1F) Or (fac << &H10)) Or code))
    End Function

End Class