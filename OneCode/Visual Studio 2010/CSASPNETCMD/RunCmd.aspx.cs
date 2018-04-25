/****************************** Module Header ******************************\
Module Name:  RunCmd.aspx.cs
Project:      CSASPNETCMD
Copyright (c) Microsoft Corporation.

The RunCmd Class is responsible for dealing inputs from user and output the result.
When user click "Save" button, save id and input text to session.
When user click "GetValueWithID" button, get input text from session by id. 

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#region Using directives

using System;
using System.Web.UI;
using System.IO;
using System.Web;
using System.Security;

#endregion

[assembly: SecurityCritical]
namespace CSASPNETCMD
{
    public partial class RunCmd : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }


        protected void btnRunBatch_Click(object sender, EventArgs e)
        {
            // Upload file
            string fileName = HttpContext.Current.Server.MapPath(@"Batchs\" +
                System.Guid.NewGuid().ToString() + "_" + fileUpload.FileName);
            fileUpload.SaveAs(fileName);

            // Run this batch file.
            string output = RunBatch(fileName);
            // Set result
            tbResult.Text = output;

            // Delete temp file.
            System.IO.File.Delete(fileName);
        }

        protected void btnRunCmd_Click(object sender, EventArgs e)
        {
            // Create a batch for these command.
            string commandLine = this.tbCommand.Text;
            string fileName = HttpContext.Current.Server.MapPath(@"Batchs\" +
                System.Guid.NewGuid().ToString() + ".bat");
            using (StreamWriter sw = System.IO.File.CreateText(fileName))
            {
                sw.Write(commandLine);
                sw.Flush();
            }

            // Run this batch file.
            string output = RunBatch(fileName);

            // Set result
            tbResult.Text = output;

            // Delete temp file.
            System.IO.File.Delete(fileName);
        }

        /// <summary>
        /// Use BatchRunner class to run this batch.
        /// </summary>
        /// <param name="fileName">the batch file with fullname.</param>
        /// <returns></returns>
        private string RunBatch(string fileName)
        {
            // Set batch execute information.
            BatchRunner batchRunner = new BatchRunner();
            batchRunner.DomainName = tbDomainName.Text.Trim();
            batchRunner.UserName = tbUserName.Text.Trim();
            batchRunner.Password = tbPassword.Text.Trim();

            // Run batch file.
            string output = batchRunner.ExecuteBatch(fileName);

            return output;
        }
    }
}
