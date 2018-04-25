'*************************** Module Header ******************************'
' Module Name:  WebBrowserControl.vb
' Project:	    VBWebBrowserWithProxy
' Copyright (c) Microsoft Corporation.
' 
' This WebBrowserControl class inherits WebBrowser class and has a feature 
' to set proxy server. 
' 
' The orginal internet options will be backup and the specified proxy will
' be used in Navigating event, and the original internet options will be 
' restored in Navigated event.
' 
' This source is subject to the Microsoft Public License.
' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
' All other rights reserved.
' 
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'*************************************************************************'

Imports System.Text
Imports Microsoft.Win32
Imports System.Security.Permissions
Imports System.ComponentModel

Public Class WebBrowserWithProxy
    Inherits WebBrowser

    ' The proxy server to connect.
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),
    Browsable(False)>
    Public Property Proxy() As InternetProxy

    ' Store the original internet connection options so that after the the connection,
    ' you can restore to it.
    Private currentInternetSettings As INTERNET_PER_CONN_OPTION_LIST

    <PermissionSetAttribute(SecurityAction.LinkDemand, Name:="FullTrust")>
    Public Sub New()
    End Sub


    ''' <summary>
    ''' Handle Navigating event. In Navigating event, the connection to internet has
    ''' not started and you can edit it.
    ''' </summary>
    <PermissionSetAttribute(SecurityAction.LinkDemand, Name:="FullTrust")>
    Protected Overrides Sub OnNavigating(ByVal e As WebBrowserNavigatingEventArgs)
        MyBase.OnNavigating(e)

        ' Backup current internet connection options.
        currentInternetSettings = WinINet.BackupConnectionProxy()

        ' Set the proxy or disable the proxy.
        If Proxy IsNot Nothing AndAlso (Not String.IsNullOrEmpty(Proxy.Address)) Then
            WinINet.SetConnectionProxy(Proxy.Address)
        Else
            WinINet.DisableConnectionProxy()
        End If
    End Sub

    ''' <summary>
    ''' Handle Navigated event. In Navigated event, the connection to internet is 
    ''' completed. 
    ''' </summary>
    <PermissionSetAttribute(SecurityAction.LinkDemand, Name:="FullTrust")>
    Protected Overrides Sub OnNavigated(ByVal e As WebBrowserNavigatedEventArgs)
        MyBase.OnNavigated(e)

        ' Restore to original internet connection options.
        WinINet.RestoreConnectionProxy(currentInternetSettings)
    End Sub

    ''' <summary>
    ''' Wrap the method Navigate and set the Proxy-Authorization header if needed.
    ''' </summary>
    <PermissionSetAttribute(SecurityAction.LinkDemand, Name:="FullTrust")>
    Public Sub [Goto](ByVal url As String)
        Dim uri As Uri = Nothing
        Dim result As Boolean = uri.TryCreate(url, UriKind.RelativeOrAbsolute, uri)
        If Not result Then
            Throw New ArgumentException("The url is not valid. ")
        End If

        ' If the proxy contains user name and password, then set the Proxy-Authorization
        ' header of the request.
        If Proxy IsNot Nothing AndAlso (Not String.IsNullOrEmpty(Proxy.UserName)) _
            AndAlso (Not String.IsNullOrEmpty(Proxy.Password)) Then

            ' This header uses Base64String to store the credential.
            Dim credentialStringValue = String.Format("{0}:{1}",
                                                      Proxy.UserName, Proxy.Password)
            Dim credentialByteArray = ASCIIEncoding.ASCII.GetBytes(credentialStringValue)
            Dim credentialBase64String = Convert.ToBase64String(credentialByteArray)
            Dim authHeader As String = String.Format("Proxy-Authorization: Basic {0}",
                                                     credentialBase64String)

            Navigate(uri, String.Empty, Nothing, authHeader)
        Else
            Navigate(uri)
        End If
    End Sub
End Class
