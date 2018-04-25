'****************************** Module Header ******************************'
' Module Name:  ShellExtLib.vb
' Project:      VBShellExtInfotipHandler
' Copyright (c) Microsoft Corporation.
' 
' The file declares the imported Shell interfaces: IQueryInfo, and implements 
' the helper functions for registering and unregistering a shell infotip 
' handler.
' 
' This source is subject to the Microsoft Public License.
' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
' All other rights reserved.
' 
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'***************************************************************************'

Imports Microsoft.Win32
Imports System.Runtime.InteropServices


#Region "Shell Interfaces"

<ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), _
Guid("00021500-0000-0000-c000-000000000046")> _
Friend Interface IQueryInfo
    ' The original signature of GetInfoTip is 
    ' HRESULT GetInfoTip(DWORD dwFlags, [out] PWSTR *ppwszTip);
    ' According to the documentation, applications that implement this method 
    ' must allocate memory for ppwszTip by calling CoTaskMemAlloc. Calling 
    ' applications (the Shell in this case) calls CoTaskMemFree to free the 
    ' memory when it is no longer needed. Here, we set PreserveSig to false 
    ' (the default value in COM) to make the output parameter 'ppwszTip' the 
    ' return value. We also marshal the string return value as LPWStr. The 
    ' interop layer in CLR will call CoTaskMemAlloc to allocate memory and 
    ' marshal the .NET string to the memory. 
    Function GetInfoTip(ByVal dwFlags As UInt32) _
        As <MarshalAs(UnmanagedType.LPWStr)> String

    Function GetInfoFlags() As Integer
End Interface

#End Region


#Region "Shell Registration"

Friend Class ShellExtReg

    ''' <summary>
    ''' Register the shell infotip handler.
    ''' </summary>
    ''' <param name="clsid">The CLSID of the component.</param>
    ''' <param name="fileType">
    ''' The file type that the infotip handler is associated with. For 
    ''' example, '*' means all file types; '.txt' means all .txt files. The 
    ''' parameter must not be NULL or an empty string. 
    ''' </param>
    ''' <remarks>
    ''' The function creates the following key in the registry.
    '''
    '''   HKCR
    '''   {
    '''      NoRemove &lt;File Type&gt;
    '''      {
    '''          NoRemove shellex
    '''          {
    '''              {00021500-0000-0000-C000-000000000046} = s '{&lt;CLSID&gt;}'
    '''          }
    '''      }
    '''   }
    ''' </remarks>
    Public Shared Sub RegisterShellExtInfotipHandler( _
        ByVal clsid As Guid, _
        ByVal fileType As String)

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

        ' Create the registry key 
        ' HKCR\<File Type>\shellex\{00021500-0000-0000-C000-000000000046}, 
        ' and sets its default value to the CLSID of the handler.
        Dim keyName As String = String.Format( _
            "{0}\shellex\{{00021500-0000-0000-C000-000000000046}}", fileType)
        Using key As RegistryKey = Registry.ClassesRoot.CreateSubKey(keyName)
            If (Not key Is Nothing) Then
                key.SetValue(Nothing, clsid.ToString("B"))
            End If
        End Using
    End Sub


    ''' <summary>
    ''' Unregister the shell infotip handler.
    ''' </summary>
    ''' <param name="fileType">
    ''' The file type that the infotip handler is associated with. For 
    ''' example, '*' means all file types; '.txt' means all .txt files. The 
    ''' parameter must not be NULL or an empty string. 
    ''' </param>
    ''' <remarks>
    ''' The method removes the registry key 
    ''' HKCR\&lt;File Type&gt;\shellex\{00021500-0000-0000-C000-000000000046}.
    ''' </remarks>
    Public Shared Sub UnregisterShellExtInfotipHandler(ByVal fileType As String)
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

        ' Remove the registry key:
        ' HKCR\<File Type>\shellex\{00021500-0000-0000-C000-000000000046}.
        Dim keyName As String = String.Format( _
            "{0}\shellex\{{00021500-0000-0000-C000-000000000046}}", fileType)
        Registry.ClassesRoot.DeleteSubKeyTree(keyName, False)
    End Sub

End Class

#End Region

