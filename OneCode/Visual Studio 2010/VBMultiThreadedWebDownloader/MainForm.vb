'*************************** Module Header ******************************'
' Module Name:  MainForm.vb
' Project:	    VBMultiThreadedWebDownloader
' Copyright (c) Microsoft Corporation.
' 
' This is the main form of this application. It is used to initialize the UI and 
' handle the events.
' 
' This source is subject to the Microsoft Public License.
' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
' All other rights reserved.
' 
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'**************************************************************************'

Imports System.Configuration
Imports System.IO
Imports System.Net

Partial Public Class MainForm
    Inherits Form
    Private _downloader As MultiThreadedWebDownloader = Nothing

    ' Specify whether the download is paused.
    Private _isPaused As Boolean = False

    Private _lastNotificationTime As Date

    Private _proxy As WebProxy = Nothing

    Public Sub New()
        InitializeComponent()

        ' Initialize proxy from App.Config
        If Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("ProxyUrl")) Then
            _proxy = New WebProxy(
                System.Configuration.ConfigurationManager.AppSettings("ProxyUrl"))

            If (Not String.IsNullOrEmpty(ConfigurationManager.AppSettings("ProxyUser"))) _
                AndAlso (Not String.IsNullOrEmpty(
                         ConfigurationManager.AppSettings("ProxyPwd"))) Then
                Dim credential As New NetworkCredential(
                    ConfigurationManager.AppSettings("ProxyUser"),
                    ConfigurationManager.AppSettings("ProxyPwd"))

                _proxy.Credentials = credential
            Else
                _proxy.UseDefaultCredentials = True
            End If
        End If
    End Sub

    ''' <summary>
    ''' Check the file information.
    ''' </summary>
    Private Sub btnCheck_Click(ByVal sender As Object,
                               ByVal e As EventArgs) Handles btnCheck.Click

        ' Initialize an instance of MultiThreadedWebDownloader.
        _downloader = New MultiThreadedWebDownloader(tbURL.Text)
        _downloader.Proxy = Me._proxy

        Try
            _downloader.CheckFile()

            ' Update the UI.
            tbURL.Enabled = False
            btnCheck.Enabled = False
            tbPath.Enabled = True
            btnDownload.Enabled = True
        Catch
            ' If there is any exception, like System.Net.WebException or 
            ' System.Net.ProtocolViolationException, it means that there may be an 
            ' error while reading the information of the file and it cannot be 
            ' downloaded. 
            MessageBox.Show("There is an error while get the information of the file." _
                            & " Please make sure the url is accessiable.")
        End Try
    End Sub

    ''' <summary>
    ''' Handle btnDownload Click event.
    ''' </summary>
    Private Sub btnDownload_Click(ByVal sender As Object,
                                  ByVal e As EventArgs) Handles btnDownload.Click
        Try

            ' Check whether the file exists.
            If File.Exists(tbPath.Text.Trim()) Then
                Dim message As String = "There is already a file with the same name, " _
                                        & "do you want to delete it? " _
                                        & "If not, please change the local path. "
                Dim result = MessageBox.Show(
                    message, "File name conflict: " & tbPath.Text.Trim(),
                    MessageBoxButtons.OKCancel)

                If result = DialogResult.OK Then
                    File.Delete(tbPath.Text.Trim())
                Else
                    Return
                End If
            End If

            If File.Exists(tbPath.Text.Trim() & ".tmp") Then
                File.Delete(tbPath.Text.Trim() & ".tmp")
            End If

            ' Set the download path.
            _downloader.DownloadPath = tbPath.Text.Trim() & ".tmp"


            ' Register the events of HttpDownloadClient.
            AddHandler _downloader.DownloadCompleted, AddressOf DownloadCompleted
            AddHandler _downloader.DownloadProgressChanged,
                AddressOf DownloadProgressChanged
            AddHandler _downloader.StatusChanged, AddressOf StatusChanged
            AddHandler _downloader.ErrorOccurred, AddressOf ErrorOccurred
            ' Start to download file.
            _downloader.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Handle StatusChanged event.
    ''' </summary>
    Private Sub StatusChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Refresh the status.
        lbStatus.Text = _downloader.Status.ToString()

        ' Update the UI.
        Select Case _downloader.Status

            Case MultiThreadedWebDownloaderStatus.Idle, _
                MultiThreadedWebDownloaderStatus.Canceled, _
                MultiThreadedWebDownloaderStatus.Completed
                btnCheck.Enabled = True
                btnDownload.Enabled = False
                btnPause.Enabled = False
                btnCancel.Enabled = False
                tbPath.Enabled = False
                tbURL.Enabled = True
            Case MultiThreadedWebDownloaderStatus.Checked
                btnCheck.Enabled = False
                btnDownload.Enabled = True
                btnPause.Enabled = False
                btnCancel.Enabled = False
                tbPath.Enabled = True
                tbURL.Enabled = False
            Case MultiThreadedWebDownloaderStatus.Downloading
                btnCheck.Enabled = False
                btnDownload.Enabled = False
                btnPause.Enabled = True
                btnCancel.Enabled = True
                tbPath.Enabled = False
                tbURL.Enabled = False
            Case MultiThreadedWebDownloaderStatus.Pausing, _
                MultiThreadedWebDownloaderStatus.Canceling
                btnCheck.Enabled = False
                btnDownload.Enabled = False
                btnPause.Enabled = False
                btnCancel.Enabled = False
                tbPath.Enabled = False
                tbURL.Enabled = False
            Case MultiThreadedWebDownloaderStatus.Paused
                btnCheck.Enabled = False
                btnDownload.Enabled = False
                btnPause.Enabled = True
                btnCancel.Enabled = False
                tbPath.Enabled = False
                tbURL.Enabled = False
        End Select

        If _downloader.Status = MultiThreadedWebDownloaderStatus.Paused Then
            lbSummary.Text = String.Format(
                "Received: {0}KB, Total: {1}KB, Time: {2}:{3}:{4}",
                _downloader.DownloadedSize / 1024,
                _downloader.TotalSize / 1024,
                _downloader.TotalUsedTime.Hours,
                _downloader.TotalUsedTime.Minutes,
                _downloader.TotalUsedTime.Seconds)
        End If
    End Sub

    ''' <summary>
    ''' Handle DownloadProgressChanged event.
    ''' </summary>
    Private Sub DownloadProgressChanged(ByVal sender As Object,
                                        ByVal e As MultiThreadedWebDownloaderProgressChangedEventArgs)
        ' Refresh the summary every second.
        If Date.Now > _lastNotificationTime.AddSeconds(1) Then
            lbSummary.Text = String.Format(
                "Received: {0}KB, Total: {1}KB, Speed: {2}KB/s Thread: {3}",
                e.ReceivedSize \ 1024,
                e.TotalSize \ 1024,
                e.DownloadSpeed \ 1024,
                _downloader.DownloadThreadsCount)
            prgDownload.Value = CInt(Fix(e.ReceivedSize * 100 \ e.TotalSize))
            _lastNotificationTime = Date.Now
        End If
    End Sub

    ''' <summary>
    ''' Handle DownloadCompleted event.
    ''' </summary>
    Private Sub DownloadCompleted(ByVal sender As Object,
                                  ByVal e As MultiThreadedWebDownloaderCompletedEventArgs)
        lbSummary.Text = String.Format(
            "Received: {0}KB, Total: {1}KB, Time: {2}:{3}:{4}",
            e.DownloadedSize \ 1024,
            e.TotalSize \ 1024,
            e.TotalTime.Hours,
            e.TotalTime.Minutes,
            e.TotalTime.Seconds)

        File.Move(tbPath.Text.Trim() & ".tmp", tbPath.Text.Trim())

        prgDownload.Value = 100
    End Sub

    Private Sub ErrorOccurred(ByVal sender As Object, ByVal e As ErrorEventArgs)
        lbSummary.Text = e.Exception.Message
        prgDownload.Value = 0
    End Sub

    ''' <summary>
    ''' Handle btnCancel Click event.
    ''' </summary>
    Private Sub btnCancel_Click(ByVal sender As Object,
                                ByVal e As EventArgs) Handles btnCancel.Click
        _downloader.Cancel()
    End Sub

    ''' <summary>
    ''' Handle btnPause Click event.
    ''' </summary>
    Private Sub btnPause_Click(ByVal sender As Object,
                               ByVal e As EventArgs) Handles btnPause.Click
        If _isPaused Then
            _downloader.Resume()
            btnPause.Text = "Pause"
        Else
            _downloader.Pause()
            btnPause.Text = "Resume"
        End If
        _isPaused = Not _isPaused
    End Sub

End Class

