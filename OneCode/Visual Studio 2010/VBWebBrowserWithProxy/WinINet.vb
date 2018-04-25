'*************************** Module Header ******************************'
' Module Name:  WinINet.vb
' Project:	    VBWebBrowserWithProxy
' Copyright (c) Microsoft Corporation.
' 
' This class is a simple .NET wrapper of wininet.dll. It contains 2 extern
' methods (InternetSetOption and InternetQueryOption) of wininet.dll. This 
' class can be used to set proxy, disable proxy, backup internet options 
' and restore internet options.
' 
' This source is subject to the Microsoft Public License.
' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
' All other rights reserved.
' 
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'*************************************************************************'

Imports System.Runtime.InteropServices

Public NotInheritable Class WinINet
    ''' <summary>
    ''' Sets an Internet option.
    ''' </summary>
    <DllImport("wininet.dll", CharSet:=CharSet.Ansi, SetLastError:=True)>
    Private Shared Function InternetSetOption(ByVal hInternet As IntPtr,
                                                  ByVal dwOption As INTERNET_OPTION,
                                                  ByVal lpBuffer As IntPtr,
                                                  ByVal lpdwBufferLength As Integer) As Boolean
    End Function

    ''' <summary>
    ''' Queries an Internet option on the specified handle. The Handle will be always 0.
    ''' </summary>
    <DllImport("wininet.dll", EntryPoint:="InternetQueryOption",
        CharSet:=CharSet.Ansi, SetLastError:=True)>
    Private Shared Function InternetQueryOptionList(ByVal Handle As IntPtr,
                                                        ByVal OptionFlag As INTERNET_OPTION,
                                                        ByRef OptionList As INTERNET_PER_CONN_OPTION_LIST,
                                                        ByRef size As Integer) As Boolean
    End Function

    ''' <summary>
    ''' Set the proxy server for LAN connection.
    ''' </summary>
    Private Sub New()
    End Sub
    Public Shared Function SetConnectionProxy(ByVal proxyServer As String) As Boolean
        ' Create 3 options.
        Dim Options(2) As INTERNET_PER_CONN_OPTION

        ' Set PROXY flags.
        Options(0) = New INTERNET_PER_CONN_OPTION()
        Options(0).dwOption = CInt(INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_FLAGS)
        Options(0).Value.dwValue = CInt(INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_PROXY)

        ' Set proxy name.
        Options(1) = New INTERNET_PER_CONN_OPTION()
        Options(1).dwOption =
            CInt(INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_SERVER)
        Options(1).Value.pszValue = Marshal.StringToHGlobalAnsi(proxyServer)

        ' Set proxy bypass.
        Options(2) = New INTERNET_PER_CONN_OPTION()
        Options(2).dwOption =
            CInt(INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_BYPASS)
        Options(2).Value.pszValue = Marshal.StringToHGlobalAnsi("local")

        ' Allocate a block of memory of the options.
        Dim buffer As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(Options(0)) _
                                                      + Marshal.SizeOf(Options(1)) _
                                                      + Marshal.SizeOf(Options(2)))

        Dim current As IntPtr = buffer

        ' Marshal data from a managed object to an unmanaged block of memory.
        For i As Integer = 0 To Options.Length - 1
            Marshal.StructureToPtr(Options(i), current, False)
            current = CType(CInt(current) + Marshal.SizeOf(Options(i)), IntPtr)
        Next i

        ' Initialize a INTERNET_PER_CONN_OPTION_LIST instance.
        Dim option_list As New INTERNET_PER_CONN_OPTION_LIST()

        ' Point to the allocated memory.
        option_list.pOptions = buffer

        ' Return the unmanaged size of an object in bytes.
        option_list.Size = Marshal.SizeOf(option_list)

        ' IntPtr.Zero means LAN connection.
        option_list.Connection = IntPtr.Zero

        option_list.OptionCount = Options.Length
        option_list.OptionError = 0
        Dim size As Integer = Marshal.SizeOf(option_list)

        ' Allocate memory for the INTERNET_PER_CONN_OPTION_LIST instance.
        Dim intptrStruct As IntPtr = Marshal.AllocCoTaskMem(size)

        ' Marshal data from a managed object to an unmanaged block of memory.
        Marshal.StructureToPtr(option_list, intptrStruct, True)

        ' Set internet settings.
        Dim bReturn As Boolean =
            InternetSetOption(IntPtr.Zero,
                              INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION,
                              intptrStruct,
                              size)

        ' Free the allocated memory.
        Marshal.FreeCoTaskMem(buffer)
        Marshal.FreeCoTaskMem(intptrStruct)

        ' Throw an exception if this operation failed.
        If Not bReturn Then
            Throw New ApplicationException(" Set Internet Option Failed!")
        End If

        ' Notify the system that the registry settings have been changed and cause
        ' the proxy data to be reread from the registry for a handle.
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_SETTINGS_CHANGED,
                          IntPtr.Zero, 0)
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_REFRESH,
                          IntPtr.Zero, 0)

        Return bReturn
    End Function

    ''' <summary>
    ''' Disable Proxy Server for LAN connection.
    ''' </summary>
    ''' <returns></returns>
    Public Shared Function DisableConnectionProxy() As Boolean
        ' Make connection access to internet directly.
        Dim Options(0) As INTERNET_PER_CONN_OPTION
        Options(0).dwOption = CInt(INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_FLAGS)
        Options(0).Value.dwValue = CInt(INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_DIRECT)

        ' Marshal data from a managed object to an unmanaged block of memory.
        Dim buffer As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(Options(0)))
        Marshal.StructureToPtr(Options(0), buffer, False)

        ' Initialize a INTERNET_PER_CONN_OPTION_LIST instance.
        Dim request As New INTERNET_PER_CONN_OPTION_LIST()

        ' Point to the allocated memory.
        request.pOptions = buffer

        request.Size = Marshal.SizeOf(request)

        ' IntPtr.Zero means LAN connection.
        request.Connection = IntPtr.Zero
        request.OptionCount = Options.Length
        request.OptionError = 0
        Dim size As Integer = Marshal.SizeOf(request)

        ' Allocate memory for the INTERNET_PER_CONN_OPTION_LIST instance.
        Dim intptrStruct As IntPtr = Marshal.AllocCoTaskMem(size)

        ' Marshal data from a managed object to an unmanaged block of memory.
        Marshal.StructureToPtr(request, intptrStruct, True)

        ' Set internet settings.
        Dim bReturn As Boolean =
            InternetSetOption(IntPtr.Zero,
                              INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION,
                              intptrStruct,
                              size)

        ' Free the allocated memory.
        Marshal.FreeCoTaskMem(buffer)
        Marshal.FreeCoTaskMem(intptrStruct)

        ' Throw an exception if this operation failed.
        If Not bReturn Then
            Throw New ApplicationException(" Set Internet Option Failed! ")
        End If

        ' Notify the system that the registry settings have been changed and cause
        ' the proxy data to be reread from the registry for a handle.
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_SETTINGS_CHANGED,
                          IntPtr.Zero, 0)
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_REFRESH,
                          IntPtr.Zero, 0)

        Return bReturn
    End Function

    ''' <summary>
    ''' Backup the current options for LAN connection.
    ''' Make sure free the memory after restoration. 
    ''' </summary>
    Public Shared Function BackupConnectionProxy() As INTERNET_PER_CONN_OPTION_LIST

        ' Query following options. 
        Dim Options(2) As INTERNET_PER_CONN_OPTION

        Options(0) = New INTERNET_PER_CONN_OPTION()
        Options(0).dwOption =
            CInt(INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_FLAGS)

        Options(1) = New INTERNET_PER_CONN_OPTION()
        Options(1).dwOption =
            CInt(INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_SERVER)

        Options(2) = New INTERNET_PER_CONN_OPTION()
        Options(2).dwOption =
            CInt(INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_BYPASS)

        ' Allocate a block of memory of the options.
        Dim buffer As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(Options(0)) _
                                                      + Marshal.SizeOf(Options(1)) _
                                                      + Marshal.SizeOf(Options(2)))

        Dim current As IntPtr = CType(buffer, IntPtr)

        ' Marshal data from a managed object to an unmanaged block of memory.
        For i As Integer = 0 To Options.Length - 1
            Marshal.StructureToPtr(Options(i), current, False)
            current = CType(CInt(current) + Marshal.SizeOf(Options(i)), IntPtr)
        Next i

        ' Initialize a INTERNET_PER_CONN_OPTION_LIST instance.
        Dim Request As New INTERNET_PER_CONN_OPTION_LIST()

        ' Point to the allocated memory.
        Request.pOptions = buffer

        Request.Size = Marshal.SizeOf(Request)

        ' IntPtr.Zero means LAN connection.
        Request.Connection = IntPtr.Zero

        Request.OptionCount = Options.Length
        Request.OptionError = 0
        Dim size As Integer = Marshal.SizeOf(Request)

        ' Query internet options. 
        Dim result As Boolean =
            InternetQueryOptionList(IntPtr.Zero,
                                    INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION,
                                    Request,
                                    size)
        If Not result Then
            Throw New ApplicationException(" Set Internet Option Failed! ")
        End If

        Return Request
    End Function

    ''' <summary>
    ''' Restore the options for LAN connection.
    ''' </summary>
    ''' <param name="request"></param>
    ''' <returns></returns>
    Public Shared Function RestoreConnectionProxy(ByVal request As INTERNET_PER_CONN_OPTION_LIST) As Boolean
        Dim size As Integer = Marshal.SizeOf(request)

        ' Allocate memory. 
        Dim intptrStruct As IntPtr = Marshal.AllocCoTaskMem(size)

        ' Convert structure to IntPtr 
        Marshal.StructureToPtr(request, intptrStruct, True)

        ' Set internet options.
        Dim bReturn As Boolean =
            InternetSetOption(IntPtr.Zero,
                              INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION,
                              intptrStruct,
                              size)

        ' Free the allocated memory.
        Marshal.FreeCoTaskMem(request.pOptions)
        Marshal.FreeCoTaskMem(intptrStruct)

        If Not bReturn Then
            Throw New ApplicationException(" Set Internet Option Failed! ")
        End If

        ' Notify the system that the registry settings have been changed and cause
        ' the proxy data to be reread from the registry for a handle.
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_SETTINGS_CHANGED,
                          IntPtr.Zero, 0)
        InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_REFRESH,
                          IntPtr.Zero, 0)

        Return bReturn
    End Function
End Class
