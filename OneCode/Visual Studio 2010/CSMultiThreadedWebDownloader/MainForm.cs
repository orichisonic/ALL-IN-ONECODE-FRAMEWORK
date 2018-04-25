/****************************** Module Header ******************************\
* Module Name:  MainForm.cs
* Project:	    CSMultiThreadedWebDownloader
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
using System.Configuration;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace CSMultiThreadedWebDownloader
{
    public partial class MainForm : Form
    {
        MultiThreadedWebDownloader downloader = null;

        // Specify whether the download is paused.
        bool isPaused = false;

        DateTime lastNotificationTime;

        WebProxy proxy = null;

        public MainForm()
        {
            InitializeComponent();

            // Initialize proxy from App.Config
            if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProxyUrl"]))
            {
                proxy = new WebProxy(
                    System.Configuration.ConfigurationManager.AppSettings["ProxyUrl"]);

                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProxyUser"])
                    && !string.IsNullOrEmpty(ConfigurationManager.AppSettings["ProxyPwd"]))
                {
                    NetworkCredential credential = new NetworkCredential(
                        ConfigurationManager.AppSettings["ProxyUser"],
                        ConfigurationManager.AppSettings["ProxyPwd"]);

                    proxy.Credentials = credential;
                }
                else
                {
                    proxy.UseDefaultCredentials = true;
                }
            }
        }

        /// <summary>
        /// Check the file information.
        /// </summary>
        private void btnCheck_Click(object sender, EventArgs e)
        {

            // Initialize an instance of MultiThreadedWebDownloader.
            downloader = new MultiThreadedWebDownloader(tbURL.Text);
            downloader.Proxy = this.proxy;

            try
            {
                downloader.CheckFile();

                // Update the UI.
                tbURL.Enabled = false;
                btnCheck.Enabled = false;
                tbPath.Enabled = true;
                btnDownload.Enabled = true;
            }
            catch
            {
                // If there is any exception, like System.Net.WebException or 
                // System.Net.ProtocolViolationException, it means that there may be an 
                // error while reading the information of the file and it cannot be 
                // downloaded. 
                MessageBox.Show("There is an error while get the information of the file."
                   + " Please make sure the url is accessible.");
            }
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

                if (File.Exists(tbPath.Text.Trim()+".tmp"))
                {
                    File.Delete(tbPath.Text.Trim() + ".tmp");
                }

                // Set the download path.
                downloader.DownloadPath = tbPath.Text.Trim() + ".tmp";


                // Register the events of HttpDownloadClient.
                downloader.DownloadCompleted += new EventHandler<MultiThreadedWebDownloaderCompletedEventArgs>(
                    DownloadCompleted);
                downloader.DownloadProgressChanged +=
                    new EventHandler<MultiThreadedWebDownloaderProgressChangedEventArgs>(DownloadProgressChanged);
                downloader.StatusChanged += new EventHandler(StatusChanged);
                downloader.ErrorOccurred += new EventHandler<ErrorEventArgs>(ErrorOccurred);
                // Start to download file.
                downloader.Start();
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
            lbStatus.Text = downloader.Status.ToString();

            // Update the UI.
            switch (downloader.Status)
            {

                case MultiThreadedWebDownloaderStatus.Idle:
                case MultiThreadedWebDownloaderStatus.Canceled:
                case MultiThreadedWebDownloaderStatus.Completed:
                    btnCheck.Enabled = true;
                    btnDownload.Enabled = false;
                    btnPause.Enabled = false;
                    btnCancel.Enabled = false;
                    tbPath.Enabled = false;
                    tbURL.Enabled = true;
                    break;
                case MultiThreadedWebDownloaderStatus.Checked:
                    btnCheck.Enabled = false;
                    btnDownload.Enabled = true;
                    btnPause.Enabled = false;
                    btnCancel.Enabled = false;
                    tbPath.Enabled = true;
                    tbURL.Enabled = false;
                    break;
                case MultiThreadedWebDownloaderStatus.Downloading:
                    btnCheck.Enabled = false;
                    btnDownload.Enabled = false;
                    btnPause.Enabled = true;
                    btnCancel.Enabled = true;
                    tbPath.Enabled = false;
                    tbURL.Enabled = false;
                    break;
                case MultiThreadedWebDownloaderStatus.Pausing:
                case MultiThreadedWebDownloaderStatus.Canceling:
                    btnCheck.Enabled = false;
                    btnDownload.Enabled = false;
                    btnPause.Enabled = false;
                    btnCancel.Enabled = false;
                    tbPath.Enabled = false;
                    tbURL.Enabled = false;
                    break;
                case MultiThreadedWebDownloaderStatus.Paused:
                    btnCheck.Enabled = false;
                    btnDownload.Enabled = false;
                    btnPause.Enabled = true;
                    btnCancel.Enabled = false;
                    tbPath.Enabled = false;
                    tbURL.Enabled = false;
                    break;
            }

            if (downloader.Status == MultiThreadedWebDownloaderStatus.Paused)
            {
                lbSummary.Text =
                   String.Format("Received: {0}KB, Total: {1}KB, Time: {2}:{3}:{4}",
                   downloader.DownloadedSize / 1024, downloader.TotalSize / 1024,
                   downloader.TotalUsedTime.Hours, downloader.TotalUsedTime.Minutes,
                   downloader.TotalUsedTime.Seconds);
            }
        }

        /// <summary>
        /// Handle DownloadProgressChanged event.
        /// </summary>
        void DownloadProgressChanged(object sender, MultiThreadedWebDownloaderProgressChangedEventArgs e)
        {
            // Refresh the summary every second.
            if (DateTime.Now > lastNotificationTime.AddSeconds(1))
            {
                lbSummary.Text = String.Format("Received: {0}KB Total: {1}KB Speed: {2}KB/s  Threads: {3}",
                    e.ReceivedSize / 1024, e.TotalSize / 1024, e.DownloadSpeed / 1024,
                    downloader.DownloadThreadsCount);
                prgDownload.Value = (int)(e.ReceivedSize * 100 / e.TotalSize);
                lastNotificationTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Handle DownloadCompleted event.
        /// </summary>
        void DownloadCompleted(object sender, MultiThreadedWebDownloaderCompletedEventArgs e)
        {
            lbSummary.Text =
                String.Format("Received: {0}KB, Total: {1}KB, Time: {2}:{3}:{4}",
                e.DownloadedSize / 1024, e.TotalSize / 1024, e.TotalTime.Hours,
                e.TotalTime.Minutes, e.TotalTime.Seconds);

            File.Move(tbPath.Text.Trim() + ".tmp", tbPath.Text.Trim());
            
            prgDownload.Value = 100;
        }

        void ErrorOccurred(object sender, ErrorEventArgs e)
        {
            lbSummary.Text = e.Error.Message;
            prgDownload.Value = 0;
        }

        /// <summary>
        /// Handle btnCancel Click event.
        /// </summary>
        private void btnCancel_Click(object sender, EventArgs e)
        {
            downloader.Cancel();
        }

        /// <summary>
        /// Handle btnPause Click event.
        /// </summary>
        private void btnPause_Click(object sender, EventArgs e)
        {
            if (isPaused)
            {
                downloader.Resume();
                btnPause.Text = "Pause";
            }
            else
            {
                downloader.Pause();
                btnPause.Text = "Resume";
            }
            isPaused = !isPaused;
        }

    }
}
