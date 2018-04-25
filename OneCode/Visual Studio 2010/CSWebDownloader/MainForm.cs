﻿/****************************** Module Header ******************************\
* Module Name:  MainForm.cs
* Project:	    CSWebDownloader
* Copyright (c) Microsoft Corporation.
* 
* This is the main form of this application. It is used to initialize the UI and 
* handle the events.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

using System;
using System.IO;
using System.Windows.Forms;

namespace CSWebDownloader
{
    public partial class MainForm : Form
    {
        HttpDownloadClient client = null;

        // Specify whether the download is paused.
        bool isPaused = false;

        DateTime lastNotificationTime;

        public MainForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Handle btnDownload Click event.
        /// </summary>
        private void btnDownload_Click(object sender, EventArgs e)
        {

            try
            {

                // Check whether the file exists.
                if (File.Exists(tbPath.Text.Trim()))
                {
                    string message = "There is already a file with the same name, "
                            + "do you want to delete it? "
                            + "If not, please change the local path. ";
                    var result = MessageBox.Show(
                        message,
                        "File name conflict: " + tbPath.Text.Trim(),
                        MessageBoxButtons.OKCancel);

                    if (result == System.Windows.Forms.DialogResult.OK)
                    {
                        File.Delete(tbPath.Text.Trim());
                    }
                    else
                    {
                        return;
                    }
                }

                // Construct the temporary file path.
                string tempPath = tbPath.Text.Trim() + ".tmp";

                // Delete the temporary file if it already exists.
                if (File.Exists(tempPath))
                {
                    File.Delete(tempPath);
                }

                // Initialize an instance of HttpDownloadClient.
                // Store the file to a temporary file first.
                client = new HttpDownloadClient(tbURL.Text, tempPath);

                //// Register the events of HttpDownloadClient.
                client.DownloadCompleted += new EventHandler<HttpDownloadCompletedEventArgs>(
                    DownloadCompleted);
                client.DownloadProgressChanged +=
                    new EventHandler<HttpDownloadProgressChangedEventArgs>(DownloadProgressChanged);
                client.StatusChanged += new EventHandler(StatusChanged);

                // Start to download file.
                client.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        /// <summary>
        /// Handle StatusChanged event.
        /// </summary>
        void StatusChanged(object sender, EventArgs e)
        {
            // Refresh the status.
            lbStatus.Text = client.Status.ToString();

            // Refresh the buttons.
            switch (client.Status)
            {
                case HttpDownloadClientStatus.Idle:
                case HttpDownloadClientStatus.Canceled:
                case HttpDownloadClientStatus.Completed:
                    btnDownload.Enabled = true;
                    btnPause.Enabled = false;
                    btnCancel.Enabled = false;
                    tbPath.Enabled = true;
                    tbURL.Enabled = true;
                    break;
                case HttpDownloadClientStatus.Downloading:
                    btnDownload.Enabled = false;
                    btnPause.Enabled = true;
                    btnCancel.Enabled = true;
                    tbPath.Enabled = false;
                    tbURL.Enabled = false;
                    break;
                case HttpDownloadClientStatus.Pausing:
                case HttpDownloadClientStatus.Canceling:
                    btnDownload.Enabled = false;
                    btnPause.Enabled = false;
                    btnCancel.Enabled = false;
                    tbPath.Enabled = false;
                    tbURL.Enabled = false;
                    break;
                case HttpDownloadClientStatus.Paused:
                    btnDownload.Enabled = false;
                    btnPause.Enabled = true;
                    btnCancel.Enabled = false;
                    tbPath.Enabled = false;
                    tbURL.Enabled = false;
                    break;
            }

            if (client.Status == HttpDownloadClientStatus.Paused)
            {
                lbSummary.Text =
                   String.Format("Received: {0}KB, Total: {1}KB, Time: {2}:{3}:{4}",
                   client.DownloadedSize / 1024, client.TotalSize / 1024,
                   client.TotalUsedTime.Hours, client.TotalUsedTime.Minutes,
                   client.TotalUsedTime.Seconds);
            }
        }

        /// <summary>
        /// Handle DownloadProgressChanged event.
        /// </summary>
        void DownloadProgressChanged(object sender, HttpDownloadProgressChangedEventArgs e)
        {
            // Refresh the summary every second.
            if (DateTime.Now > lastNotificationTime.AddSeconds(1))
            {
                lbSummary.Text = String.Format("Received: {0}KB, Total: {1}KB, Speed: {2}KB/s",
                    e.ReceivedSize / 1024, e.TotalSize / 1024, e.DownloadSpeed / 1024);
                prgDownload.Value = (int)(e.ReceivedSize * 100 / e.TotalSize);
                lastNotificationTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Handle DownloadCompleted event.
        /// </summary>
        void DownloadCompleted(object sender, HttpDownloadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                lbSummary.Text =
                    String.Format("Received: {0}KB, Total: {1}KB, Time: {2}:{3}:{4}",
                    e.DownloadedSize / 1024, e.TotalSize / 1024, e.TotalTime.Hours,
                    e.TotalTime.Minutes, e.TotalTime.Seconds);

                if (File.Exists(tbPath.Text.Trim()))
                {
                    File.Delete(tbPath.Text.Trim());
                }

                File.Move(tbPath.Text.Trim() + ".tmp", tbPath.Text.Trim());
                prgDownload.Value = 100;
            }
            else
            {
                lbSummary.Text = e.Error.Message;
                if (File.Exists(tbPath.Text.Trim() + ".tmp"))
                {
                    File.Delete(tbPath.Text.Trim() + ".tmp");
                }
                prgDownload.Value = 0;
            }

        }

        /// <summary>
        /// Handle btnCancel Click event.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            client.Cancel();
        }

        /// <summary>
        /// Handle btnPause Click event.
        /// </summary>
        private void btnPause_Click(object sender, EventArgs e)
        {
            if (isPaused)
            {
                client.Resume();
                btnPause.Text = "Pause";
            }
            else
            {
                client.Pause();
                btnPause.Text = "Resume";
            }
            isPaused = !isPaused;
        }
    }
}
