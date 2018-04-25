/****************************** Module Header ******************************\
Module Name:  BatchRunner.cs
Project:      CSASPNETCMD
Copyright (c) Microsoft Corporation.

The Batch Runner Class is responsible for executing batch file.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#region Using directives
using System;
using System.Security;
#endregion

namespace CSASPNETCMD
{
    public class BatchRunner
    {
        #region fields
        private string domainName;
        private string userName;
        private string password;
        #endregion

        #region properties
        /// <summary>
        /// Set and get identify user's DomainName.
        /// </summary>
        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }
        /// <summary>
        /// Set and get identify user's UserName.
        /// </summary>
        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }
        /// <summary>
        /// Set and get identify user's Password.
        /// </summary>
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        #endregion       

        #region constructor
        /// <summary>
        /// Default constructor
        /// </summary>
        public BatchRunner()
        {
        }

        /// <summary>
        /// Constructor with identify information.
        /// </summary>
        /// <param name="domain">Identify user's DomainName.</param>
        /// <param name="username">Identify user's username.</param>
        /// <param name="password">Identify user's password.</param>
        public BatchRunner(string domain, string username,string password)
        {
            this.domainName = domain;
            this.userName = username;
            this.password = password;
        }

        #endregion

        /// <summary>
        /// Execute batch file.
        /// </summary>
        /// <param name="fileName">The batch file with fullname to execute.</param>
        /// <returns>Result of batch file execution.</returns>
        public string ExecuteBatch(string fileName)
        {
            if (!fileName.ToLower().EndsWith(".bat"))
            {
                throw new ArgumentException("the file's suffix must be .bat");
            }
            string retOutPut = "No Output!";

            try
            {
                System.Diagnostics.ProcessStartInfo psi = GenerateProcessInfo(fileName);

                System.Diagnostics.Process processBatch = System.Diagnostics.Process.Start(psi);
                System.IO.StreamReader myOutput = processBatch.StandardOutput;

                processBatch.WaitForExit();
                if (processBatch.HasExited)
                {
                    retOutPut = myOutput.ReadToEnd();
                }
            }
            catch (System.ComponentModel.Win32Exception winException)
            {
                if (winException.Message.Contains("bad password"))
                {
                    retOutPut = winException.Message;
                }
                else
                {
                    retOutPut = "Win32Exception occured";
                }
                // Log exception information.
            }
            catch (System.Exception exception)
            {
                retOutPut = exception.Message;

                // Log exception information.
            }
            return retOutPut;
        }

        /// <summary>
        /// Generate ProcessInfo with fields(domainName,userName,password).
        /// </summary>
        /// <param name="fileName">The batch file with fullname to execute.</param>
        /// <returns></returns>
        private System.Diagnostics.ProcessStartInfo GenerateProcessInfo(string fileName)
        {
            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo(fileName);
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            psi.UseShellExecute = false;
            if (!string.IsNullOrEmpty(domainName))
            {
                psi.Domain = domainName;
            }
            if (!string.IsNullOrEmpty(userName))
            {
                psi.UserName = userName;
            }
            if (!string.IsNullOrEmpty(password))
            {
                SecureString sstring = new SecureString();
                foreach (char c in password)
                {
                    sstring.AppendChar(c);
                }
                psi.Password = sstring;
            }
            return psi;
        } 

    }
}