'****************************** Module Header ******************************'
' Module Name:  FileContextMenuExt.vb
' Project:      VBShellExtContextMenuHandler
' Copyright (c) Microsoft Corporation.
' 
' The FileContextMenuExt.vb file defines a context menu handler by 
' implementing the IShellExtInit and IContextMenu interfaces.
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

#End Region


<ClassInterface(ClassInterfaceType.None), _
Guid("1E25BCD5-F299-496A-911D-51FB901F7F40"), ComVisible(True)> _
Public Class FileContextMenuExt
    Implements IShellExtInit, IContextMenu

    ' The name of the selected file.
    Private selectedFile As String

    Private menuText As String = "&Display File Name (VB)"
    Private verb As String = "vbdisplay"
    Private verbCanonicalName As String = "VBDisplayFileName"
    Private verbHelpText As String = "Display File Name (VB)"
    Private IDM_DISPLAY As UInteger = 0


    Private Sub OnVerbDisplayFileName(ByVal hWnd As IntPtr)
        System.Windows.Forms.MessageBox.Show("The selected file is " & _
            Environment.NewLine & Environment.NewLine & Me.selectedFile, _
            "VBShellExtContextMenuHandler")
    End Sub


#Region "Shell Extension Registration"

    <ComRegisterFunction()> _
    Public Shared Sub Register(ByVal t As Type)
        Try
            ShellExtReg.RegisterShellExtContextMenuHandler(t.GUID, ".vb", _
                "VBShellExtContextMenuHandler.FileContextMenuExt Class")
        Catch ex As Exception
            Console.WriteLine(ex.Message) ' Log the error
            Throw ' Re-throw the exception
        End Try
    End Sub


    <ComUnregisterFunction()> _
    Public Shared Sub Unregister(ByVal t As Type)
        Try
            ShellExtReg.UnregisterShellExtContextMenuHandler(t.GUID, ".vb")
        Catch ex As Exception
            Console.WriteLine(ex.Message) ' Log the error
            Throw ' Re-throw the exception
        End Try
    End Sub

#End Region


#Region "IShellExtInit Members"

    ''' <summary>
    ''' Initialize the context menu handler.
    ''' </summary>
    ''' <param name="pidlFolder">
    ''' A pointer to an ITEMIDLIST structure that uniquely identifies a folder.
    ''' </param>
    ''' <param name="pDataObj">
    ''' A pointer to an IDataObject interface object that can be used to 
    ''' retrieve the objects being acted upon.
    ''' </param>
    ''' <param name="hKeyProgID">
    ''' The registry key for the file object or folder type.
    ''' </param>
    Public Sub Initialize( _
        ByVal pidlFolder As IntPtr, _
        ByVal pDataObj As IntPtr, _
        ByVal hKeyProgID As IntPtr) _
        Implements IShellExtInit.Initialize

        If (pDataObj = IntPtr.Zero) Then
            Throw New ArgumentException
        End If

        Dim fe As New FORMATETC
        With fe
            .cfFormat = CLIPFORMAT.CF_HDROP
            .ptd = IntPtr.Zero
            .dwAspect = DVASPECT.DVASPECT_CONTENT
            .lindex = -1
            .tymed = TYMED.TYMED_HGLOBAL
        End With

        Dim stm As New STGMEDIUM

        ' The pDataObj pointer contains the objects being acted upon. In this 
        ' example, we get an HDROP handle for enumerating the selected files 
        ' and folders.
        Dim dataObject As IDataObject = Marshal.GetObjectForIUnknown(pDataObj)
        dataObject.GetData(fe, stm)

        Try
            ' Get an HDROP handle.
            Dim hDrop As IntPtr = stm.unionmember
            If (hDrop = IntPtr.Zero) Then
                Throw New ArgumentException
            End If

            ' Determine how many files are involved in this operation.
            Dim nFiles As UInteger = NativeMethods.DragQueryFile(hDrop, _
                UInt32.MaxValue, Nothing, 0)

            ' This code sample displays the custom context menu item when only 
            ' one file is selected. 
            If (nFiles = 1) Then
                ' Get the path of the file.
                Dim fileName As New StringBuilder(260)
                If (0 = NativeMethods.DragQueryFile(hDrop, 0, fileName,
                    fileName.Capacity)) Then
                    Marshal.ThrowExceptionForHR(WinError.E_FAIL)
                End If
                Me.selectedFile = fileName.ToString
            Else
                Marshal.ThrowExceptionForHR(WinError.E_FAIL)
            End If

            ' [-or-]

            ' Enumerates the selected files and folders.
            'If (nFiles > 0) Then
            '    Dim selectedFiles As New StringCollection()
            '    Dim fileName As New StringBuilder(260)
            '    For i As UInteger = 0 To nFiles - 1
            '        ' Get the next file name.
            '        If (0 <> NativeMethods.DragQueryFile(hDrop, i, fileName, _
            '            fileName.Capacity)) Then
            '            ' Add the file name to the list.
            '            selectedFiles.Add(fileName.ToString())
            '        End If
            '    Next

            '    ' If we did not find any files we can work with, throw exception.
            '    If (selectedFiles.Count = 0) Then
            '        Marshal.ThrowExceptionForHR(WinError.E_FAIL)
            '    End If
            'Else
            '    Marshal.ThrowExceptionForHR(WinError.E_FAIL)
            'End If

        Finally
            NativeMethods.ReleaseStgMedium((stm))
        End Try
    End Sub

#End Region


#Region "IContextMenu Members"

    ''' <summary>
    ''' Add commands to a shortcut menu.
    ''' </summary>
    ''' <param name="hMenu">A handle to the shortcut menu.</param>
    ''' <param name="iMenu">
    ''' The zero-based position at which to insert the first new menu item.
    ''' </param>
    ''' <param name="idCmdFirst">
    ''' The minimum value that the handler can specify for a menu item ID.
    ''' </param>
    ''' <param name="idCmdLast">
    ''' The maximum value that the handler can specify for a menu item ID.
    ''' </param>
    ''' <param name="uFlags">
    ''' Optional flags that specify how the shortcut menu can be changed.
    ''' </param>
    ''' <returns>
    ''' If successful, returns an HRESULT value that has its severity value 
    ''' set to SEVERITY_SUCCESS and its code value set to the offset of the 
    ''' largest command identifier that was assigned, plus one.
    ''' </returns>
    Public Function QueryContextMenu( _
        ByVal hMenu As IntPtr, _
        ByVal iMenu As UInt32, _
        ByVal idCmdFirst As UInt32, _
        ByVal idCmdLast As UInt32, _
        ByVal uFlags As UInt32) As Integer _
        Implements IContextMenu.QueryContextMenu

        ' If uFlags include CMF_DEFAULTONLY then we should not do anything.
        If ((CMF.CMF_DEFAULTONLY And uFlags) <> 0) Then
            Return WinError.MAKE_HRESULT(WinError.SEVERITY_SUCCESS, 0, 0)
        End If

        ' Use either InsertMenu or InsertMenuItem to add menu items.

        Dim mii As New MENUITEMINFO
        With mii
            .cbSize = Marshal.SizeOf(mii)
            .fMask = MIIM.MIIM_TYPE Or MIIM.MIIM_STATE Or MIIM.MIIM_ID
            .wID = idCmdFirst + Me.IDM_DISPLAY
            .fType = MFT.MFT_STRING
            .dwTypeData = Me.menuText
            .fState = MFS.MFS_ENABLED
        End With
        If Not NativeMethods.InsertMenuItem(hMenu, iMenu, True, mii) Then
            Return Marshal.GetHRForLastWin32Error
        End If

        ' Add a separator.
        Dim sep As New MENUITEMINFO
        With sep
            .cbSize = Marshal.SizeOf(sep)
            .fMask = MIIM.MIIM_TYPE
            .fType = MFT.MFT_SEPARATOR
        End With
        If Not NativeMethods.InsertMenuItem(hMenu, iMenu + 1, True, sep) Then
            Return Marshal.GetHRForLastWin32Error
        End If

        ' Return an HRESULT value with the severity set to SEVERITY_SUCCESS. 
        ' Set the code value to the offset of the largest command identifier 
        ' that was assigned, plus one (1).
        Return WinError.MAKE_HRESULT(0, 0, (Me.IDM_DISPLAY + 1))
    End Function


    ''' <summary>
    ''' Carry out the command associated with a shortcut menu item.
    ''' </summary>
    ''' <param name="pici">
    ''' A pointer to a CMINVOKECOMMANDINFO or CMINVOKECOMMANDINFOEX structure 
    ''' containing information about the command. 
    ''' </param>
    Public Sub InvokeCommand(ByVal pici As IntPtr) _
        Implements IContextMenu.InvokeCommand

        Dim isUnicode As Boolean = False

        ' Determine which structure is being passed in, CMINVOKECOMMANDINFO or 
        ' CMINVOKECOMMANDINFOEX based on the cbSize member of lpcmi. Although 
        ' the lpcmi parameter is declared in Shlobj.h as a CMINVOKECOMMANDINFO 
        ' structure, in practice it often points to a CMINVOKECOMMANDINFOEX 
        ' structure. This struct is an extended version of CMINVOKECOMMANDINFO 
        ' and has additional members that allow Unicode strings to be passed.
        Dim ici As CMINVOKECOMMANDINFO = Marshal.PtrToStructure(pici, GetType(CMINVOKECOMMANDINFO))
        Dim iciex As New CMINVOKECOMMANDINFOEX
        If (ici.cbSize = Marshal.SizeOf(GetType(CMINVOKECOMMANDINFOEX))) Then
            If (ici.fMask And CMIC.CMIC_MASK_UNICODE) <> 0 Then
                isUnicode = True
                iciex = Marshal.PtrToStructure(pici, GetType(CMINVOKECOMMANDINFOEX))
            End If
        End If

        ' Determines whether the command is identified by its offset or verb.
        ' There are two ways to identify commands:
        ' 
        '   1) The command's verb string 
        '   2) The command's identifier offset
        ' 
        ' If the high-order word of lpcmi->lpVerb (for the ANSI case) or 
        ' lpcmi->lpVerbW (for the Unicode case) is nonzero, lpVerb or lpVerbW 
        ' holds a verb string. If the high-order word is zero, the command 
        ' offset is in the low-order word of lpcmi->lpVerb.

        ' For the ANSI case, if the high-order word is not zero, the command's 
        ' verb string is in lpcmi->lpVerb. 
        If (Not isUnicode AndAlso (NativeMethods.HighWord(ici.verb.ToInt32) <> 0)) Then
            ' Is the verb supported by this context menu extension?
            If (Marshal.PtrToStringAnsi(ici.verb) = Me.verb) Then
                OnVerbDisplayFileName(ici.hwnd)
            Else
                ' If the verb is not recognized by the context menu handler, 
                ' it must return E_FAIL to allow it to be passed on to the 
                ' other context menu handlers that might implement that verb.
                Marshal.ThrowExceptionForHR(WinError.E_FAIL)
            End If

        ElseIf (isUnicode AndAlso (NativeMethods.HighWord(iciex.verbW.ToInt32) <> 0)) Then
            ' For the Unicode case, if the high-order word is not zero, the 
            ' command's verb string is in lpcmi->lpVerbW. 

            ' Is the verb supported by this context menu extension?
            If (Marshal.PtrToStringUni(iciex.verbW) = Me.verb) Then
                OnVerbDisplayFileName(ici.hwnd)
            Else
                ' If the verb is not recognized by the context menu handler, 
                ' it must return E_FAIL to allow it to be passed on to the 
                ' other context menu handlers that might implement that verb.
                Marshal.ThrowExceptionForHR(WinError.E_FAIL)
            End If

        Else
            ' If the command cannot be identified through the verb string, 
            ' then check the identifier offset.

            ' Is the command identifier offset supported by this context menu 
            ' extension?
            If (NativeMethods.LowWord(ici.verb.ToInt32) = Me.IDM_DISPLAY) Then
                OnVerbDisplayFileName(ici.hwnd)
            Else
                ' If the verb is not recognized by the context menu handler, 
                ' it must return E_FAIL to allow it to be passed on to the 
                ' other context menu handlers that might implement that verb.
                Marshal.ThrowExceptionForHR(WinError.E_FAIL)
            End If
        End If
    End Sub


    ''' <summary>
    ''' Get information about a shortcut menu command, including the help 
    ''' string and the language-independent, or canonical, name for the 
    ''' command.
    ''' </summary>
    ''' <param name="idCmd">Menu command identifier offset.</param>
    ''' <param name="uFlags">
    ''' Flags specifying the information to return. This parameter can have 
    ''' one of the following values: GCS_HELPTEXTA, GCS_HELPTEXTW, 
    ''' GCS_VALIDATEA, GCS_VALIDATEW, GCS_VERBA, GCS_VERBW.
    ''' </param>
    ''' <param name="pReserved">Reserved. Must be IntPtr.Zero</param>
    ''' <param name="pszName">
    ''' The address of the buffer to receive the null-terminated string being 
    ''' retrieved.
    ''' </param>
    ''' <param name="cchMax">
    ''' Size of the buffer, in characters, to receive the null-terminated 
    ''' string.
    ''' </param>
    Public Sub GetCommandString( _
        ByVal idCmd As UIntPtr, _
        ByVal uFlags As UInt32, _
        ByVal pReserved As IntPtr, _
        ByVal pszName As StringBuilder, _
        ByVal cchMax As UInt32) _
        Implements IContextMenu.GetCommandString

        If (idCmd.ToUInt32 = Me.IDM_DISPLAY) Then
            Select Case DirectCast(uFlags, GCS)
                Case GCS.GCS_VERBW
                    If (Me.verbCanonicalName.Length > (cchMax - 1)) Then
                        Marshal.ThrowExceptionForHR(WinError.STRSAFE_E_INSUFFICIENT_BUFFER)
                    Else
                        pszName.Clear()
                        pszName.Append(Me.verbCanonicalName)
                    End If
                    Exit Select

                Case GCS.GCS_HELPTEXTW
                    If (Me.verbHelpText.Length > (cchMax - 1)) Then
                        Marshal.ThrowExceptionForHR(WinError.STRSAFE_E_INSUFFICIENT_BUFFER)
                    Else
                        pszName.Clear()
                        pszName.Append(Me.verbHelpText)
                    End If
                    Exit Select
            End Select
        End If
    End Sub

#End Region

End Class