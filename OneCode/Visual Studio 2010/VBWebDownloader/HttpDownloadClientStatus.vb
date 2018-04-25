﻿'*************************** Module Header ******************************'
' Module Name:  HttpDownloadClientStatus.vb
' Project:	    VBWebDownloader
' Copyright (c) Microsoft Corporation.
' 
' The enum HttpDownloadClientStatus contains all status of HttpDownloadClient.
' 
' This source is subject to the Microsoft Public License.
' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
' All other rights reserved.
' 
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'*************************************************************************'

Public Enum HttpDownloadClientStatus
    Idle
    Downloading
    Pausing
    Paused
    Canceling
    Canceled
    Completed
End Enum

