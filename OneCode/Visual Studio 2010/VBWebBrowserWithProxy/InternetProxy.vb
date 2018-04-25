'*************************** Module Header ******************************'
' Module Name:  InternetProxy.vb
' Project:	    VBWebBrowserWithProxy
' Copyright (c) Microsoft Corporation.
' 
' This class is used to describe a proxy server and the credential to connect to it.
' Please set the proxy servers in ProxyList.xml first.
' 
' This source is subject to the Microsoft Public License.
' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
' All other rights reserved.
' 
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'*************************************************************************'

Public Class InternetProxy
    Public Property ProxyName() As String
    Public Property Address() As String
    Public Property UserName() As String
    Public Property Password() As String

    Public Shared ReadOnly NoProxy As InternetProxy =
        New InternetProxy With
        {
            .Address = String.Empty,
            .Password = String.Empty,
            .ProxyName = String.Empty,
            .UserName = String.Empty
        }

    Private Shared _proxyList As List(Of InternetProxy) = Nothing
    Public Shared ReadOnly Property ProxyList() As List(Of InternetProxy)
        Get

            ' Gets the proxy servers in ProxyList.xml.
            If _proxyList Is Nothing Then
                _proxyList = New List(Of InternetProxy)()
                Try
                    Dim listXml As XElement = XElement.Load("ProxyList.xml")
                    For Each proxy In listXml.Elements("Proxy")
                        _proxyList.Add(
                            New InternetProxy With
                            {
                                .ProxyName = proxy.Element("ProxyName").Value,
                                .Address = proxy.Element("Address").Value,
                                .UserName = proxy.Element("UserName").Value,
                                .Password = proxy.Element("Password").Value
                            })
                    Next proxy
                Catch _exception As Exception
                End Try
            End If
            Return _proxyList
        End Get
    End Property
End Class
