'*************************** Module Header ******************************'
' Module Name:  MainForum.vb
' Project:	    VBWebDownloader
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
'*************************************************************************'

Imports System.IO

Partial Public Class MainForm
    Inherits Form

    Private _client As HttpDownloadClient = Nothing

    ' Specify whether the download is paused.
    Private _isPaused As Boolean = False

    Private _lastNotificationTime As Date

    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Handle btnDownload Click event.
    ''' </summary>
    Private Sub btnDownload_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnDownload.Click

        Try

            ' Check whether the file exists.
            If File.Exists(tbPath.Text.Trim()) Then
                Dim message As String = "There is already a file with the same name, " _
                        + "do you want to delete it? " _
                        + "If not, please change the local path."
                Dim result As DialogResult = MessageBox.Show(
                    message,
                    "File name conflict: " + tbPath.Text.Trim(),
                    MessageBoxButtons.OKCancel)

                If result = DialogResult.OK Then
                    File.Delete(tbPath.Text.Trim())
                Else
                    Return
                End If

            End If

            ' Construct the temporary file path.
            Dim tempPath As String = tbPath.Text.Trim() + ".tmp"

            ' Delete the temporary file if it already exists.
            If File.Exists(tempPath) Then
                File.Delete(tempPath)
            End If

            ' Initialize an instance of HttpDownloadClient.
            ' Store the file to a temporary file first.
            _client = New HttpDownloadClient(tbURL.Text, tbPath.Text.Trim() & ".tmp")

            '// Register the events of HttpDownloadClient.
            AddHandler _client.DownloadCompleted, AddressOf DownloadCompleted
            AddHandler _client.DownloadProgressChanged, AddressOf DownloadProgressChanged
            AddHandler _client.StatusChanged, AddressOf StatusChanged

            ' Start to download file.
            _client.Start()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    ''' <summary>
    ''' Handle StatusChanged event.
    ''' </summary>
    Private Sub StatusChanged(ByVal sender As Object, ByVal e As EventArgs)
        ' Refresh the status.
        lbStatus.Text = _client.Status.ToString()

        ' Refresh the buttons.
        Select Case _client.Status
            Case HttpDownloadClientStatus.Idle,
                HttpDownloadClientStatus.Canceled,
                HttpDownloadClientStatus.Completed

                btnDownload.Enabled = True
                btnPause.Enabled = False
                btnCancel.Enabled = False
                tbPath.Enabled = True
                tbURL.Enabled = True

            Case HttpDownloadClientStatus.Downloading
                btnDownload.Enabled = False
                btnPause.Enabled = True
                btnCancel.Enabled = True
                tbPath.Enabled = False
                tbURL.Enabled = False

            Case HttpDownloadClientStatus.Pausing,
                HttpDownloadClientStatus.Canceling

                btnDownload.Enabled = False
                btnPause.Enabled = False
                btnCancel.Enabled = False
                tbPath.Enabled = False
                tbURL.Enabled = False

            Case HttpDownloadClientStatus.Paused
                btnDownload.Enabled = False
                btnPause.Enabled = True
                btnCancel.Enabled = False
                tbPath.Enabled = False
                tbURL.Enabled = False
        End Select

        If _client.Status = HttpDownloadClientStatus.Paused Then
            lbSummary.Text = String.Format("Received: {0}KB, Total: {1}KB, Time: {2}:{3}:{4}",
                                           _client.DownloadedSize \ 1024,
                                           _client.TotalSize \ 1024,
                                           _client.TotalUsedTime.Hours,
                                           _client.TotalUsedTime.Minutes,
                                           _client.TotalUsedTime.Seconds)
        End If
    End Sub

    ''' <summary>
    ''' Handle DownloadProgressChanged event.
    ''' </summary>
    Private Sub DownloadProgressChanged(ByVal sender As Object,
                                        ByVal e As HttpDownloadProgressChangedEventArgs)
        ' Refresh the summary every second.
        If Date.Now > _lastNotificationTime.AddSeconds(1) Then
            lbSummary.Text = String.Format("Received: {0}KB, Total: {1}KB, Speed: {2}KB/s",
                                           e.ReceivedSize \ 1024, e.TotalSize \ 1024,
                                           e.DownloadSpeed \ 1024)
            prgDownload.Value = CInt(Fix(e.ReceivedSize * 100 \ e.TotalSize))
            _lastNotificationTime = Date.Now
        End If
    End Sub

    ''' <summary>
    ''' Handle DownloadCompleted event.
    ''' </summary>
    Private Sub DownloadCompleted(ByVal sender As Object,
                                  ByVal e As HttpDownloadCompletedEventArgs)
        If e.Error Is Nothing Then
            lbSummary.Text = String.Format("Received: {0}KB, Total: {1}KB, Time: {2}:{3}:{4}",
                                           e.DownloadedSize \ 1024, e.TotalSize \ 1024,
                                           e.TotalTime.Hours, e.TotalTime.Minutes,
                                           e.TotalTime.Seconds)

            If File.Exists(tbPath.Text.Trim()) Then
                File.Delete(tbPath.Text.Trim())
            End If

            File.Move(tbPath.Text.Trim() & ".tmp", tbPath.Text.Trim())
            prgDownload.Value = 100
        Else
            lbSummary.Text = e.Error.Message
            If File.Exists(tbPath.Text.Trim() & ".tmp") Then
                File.Delete(tbPath.Text.Trim() & ".tmp")
            End If
            prgDownload.Value = 0
        End If

    End Sub

    ''' <summary>
    ''' Handle btnCancel Click event.
    ''' </summary>
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnCancel.Click
        _client.Cancel()
    End Sub

    ''' <summary>
    ''' Handle btnPause Click event.
    ''' </summary>
    Private Sub btnPause_Click(ByVal sender As Object, ByVal e As EventArgs) _
        Handles btnPause.Click
        If _isPaused Then
            _client.Resume()
            btnPause.Text = "Pause"
        Else
            _client.Pause()
            btnPause.Text = "Resume"
        End If
        _isPaused = Not _isPaused
    End Sub
End Class
