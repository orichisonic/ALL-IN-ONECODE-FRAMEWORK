﻿'*************************** Module Header ******************************'
' Module Name:  HttpDownloadCompletedEventArgs.vb
' Project:	    VBWebDownloader
' Copyright (c) Microsoft Corporation.
' 
' The class HttpDownloadCompletedEventArgs defines the arguments used by
' the DownloadCompleted event of HttpDownloadClient.
' 
' This source is subject to the Microsoft Public License.
' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
' All other rights reserved.
' 
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'*************************************************************************'

Public Class HttpDownloadCompletedEventArgs
    Inherits EventArgs

    Private _downloadedSize As Int64

    Public Property DownloadedSize() As Int64
        Get
            Return _downloadedSize
        End Get
        Private Set(ByVal value As Int64)
            _downloadedSize = value
        End Set
    End Property

    Private _totalSize As Int64

    Public Property TotalSize() As Int64
        Get
            Return _totalSize
        End Get
        Private Set(ByVal value As Int64)
            _totalSize = value
        End Set
    End Property

    Private _error As Exception

    Public Property [Error]() As Exception
        Get
            Return _error
        End Get
        Private Set(ByVal value As Exception)
            _error = value
        End Set
    End Property

    Private _totalTime As TimeSpan

    Public Property TotalTime() As TimeSpan
        Get
            Return _totalTime
        End Get
        Private Set(ByVal value As TimeSpan)
            _totalTime = value
        End Set
    End Property

    Public Sub New(ByVal downloadedSize As Int64, ByVal totalSize As Int64,
                   ByVal totalTime As TimeSpan, ByVal ex As Exception)
        Me.DownloadedSize = downloadedSize
        Me.TotalSize = totalSize
        Me.TotalTime = totalTime
        Me.Error = ex
    End Sub
End Class
