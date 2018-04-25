/****************************** Module Header ******************************\
* Module Name:  WebBrowserControl.cs
* Project:	    CSWebBrowserWithProxy
* Copyright (c) Microsoft Corporation.
* 
* This WebBrowserControl class inherits WebBrowser class and has a feature 
* to set proxy server. 
* 
* The orginal internet options will be backup and the specified proxy will
* be used in Navigating event, and the original internet options will be 
* restored in Navigated event.
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
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Security.Permissions;
using System.ComponentModel;

namespace CSWebBrowserWithProxy
{
    public class WebBrowserWithProxy : WebBrowser
    {

        // The proxy server to connect.
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public InternetProxy Proxy { get; set; }

        // Store the original internet connection options so that after the the connection,
        // you can restore to it.
        INTERNET_PER_CONN_OPTION_LIST currentInternetSettings;

        [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
        public WebBrowserWithProxy()
        {          
        }
    

        /// <summary>
        /// Handle Navigating event. In Navigating event, the connection to internet has
        /// not started and you can edit it.
        /// </summary>
        [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
        protected override void OnNavigating(WebBrowserNavigatingEventArgs e)
        {
            base.OnNavigating(e);

            // Backup current internet connection options.
            currentInternetSettings = WinINet.BackupConnectionProxy();

            // Set the proxy or disable the proxy.
            if (Proxy != null && !string.IsNullOrEmpty(Proxy.Address))
            {
                WinINet.SetConnectionProxy(Proxy.Address);
            }
            else
            {
                WinINet.DisableConnectionProxy();
            }
        }

        /// <summary>
        /// Handle Navigated event. In Navigated event, the connection to internet is 
        /// completed. 
        /// </summary>
        [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
        protected override void OnNavigated(WebBrowserNavigatedEventArgs e)
        {
            base.OnNavigated(e);

            // Restore to original internet connection options.
            WinINet.RestoreConnectionProxy(currentInternetSettings); 
        }          

        /// <summary>
        /// Wrap the method Navigate and set the Proxy-Authorization header if needed.
        /// </summary>
        [PermissionSetAttribute(SecurityAction.LinkDemand, Name = "FullTrust")]
        public void Goto(string url)
        {
            System.Uri uri = null;
            bool result = System.Uri.TryCreate(url, UriKind.RelativeOrAbsolute, out uri);
            if (!result)
            {
                throw new ArgumentException("The url is not valid. ");
            }

            // If the proxy contains user name and password, then set the Proxy-Authorization
            // header of the request.
            if (Proxy != null && !string.IsNullOrEmpty(Proxy.UserName)
                && !string.IsNullOrEmpty(Proxy.Password))
            {

                // This header uses Base64String to store the credential.
                var credentialStringValue = string.Format("{0}:{1}",
                    Proxy.UserName, Proxy.Password);
                var credentialByteArray = ASCIIEncoding.ASCII.GetBytes(credentialStringValue);
                var credentialBase64String = Convert.ToBase64String(credentialByteArray);
                string authHeader = string.Format("Proxy-Authorization: Basic {0}",
                    credentialBase64String);

                Navigate(uri, string.Empty, null, authHeader);
            }
            else
            {
                Navigate(uri);
            }
        }
    }
}
