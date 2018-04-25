/********************************** Module Header **********************************\
Module Name:  FileInfotipExt.cs
Project:      CSShellExtInfotipHandler
Copyright (c) Microsoft Corporation.

The FileInfotipExt.cs file defines an infotip handler by implementing the 
IPersistFile and IQueryInfo interfaces.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***********************************************************************************/

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;


namespace CSShellExtInfotipHandler
{
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("B8D98AB4-376B-45D0-9CF6-8BF22A588989"), ComVisible(true)]
    public class FileInfotipExt : IPersistFile, IQueryInfo
    {
        // The name of the selected file.
        private string selectedFile = null;


        #region Shell Extension Registration

        [ComRegisterFunction()]
        public static void Register(Type t)
        {
            try
            {
                ShellExtReg.RegisterShellExtInfotipHandler(t.GUID, ".cs");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw;  // Re-throw the exception
            }
        }

        [ComUnregisterFunction()]
        public static void Unregister(Type t)
        {
            try
            {
                ShellExtReg.UnregisterShellExtInfotipHandler(".cs");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw;  // Re-throw the exception
            }
        }

        #endregion


        #region IPersistFile Members

        public void GetClassID(out Guid pClassID)
        {
            throw new NotImplementedException();
        }

        public void GetCurFile(out string ppszFileName)
        {
            throw new NotImplementedException();
        }

        public int IsDirty()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens the specified file and initializes an object from the file contents.
        /// </summary>
        /// <param name="pszFileName">
        /// The absolute path of the file to be opened.
        /// </param>
        /// <param name="dwMode">
        /// The access mode to be used when opening the file. 
        /// </param>
        public void Load(string pszFileName, int dwMode)
        {
            // pszFileName contains the absolute path of the file to be opened.
            this.selectedFile = pszFileName;
        }

        public void Save(string pszFileName, bool fRemember)
        {
            throw new NotImplementedException();
        }

        public void SaveCompleted(string pszFileName)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region IQueryInfo Members

        /// <summary>
        /// Gets the info tip text for an item.
        /// </summary>
        /// <param name="dwFlags">
        /// Flags that direct the handling of the item from which you're retrieving 
        /// the info tip text. This value is commonly zero (QITIPF_DEFAULT). 
        /// </param>
        /// <returns>
        /// A Unicode string containing the tip to display.
        /// </returns>
        public string GetInfoTip(uint dwFlags)
        {
            // Prepare the text of the infotip. The example infotip is composed of 
            // the file path and the count of code lines.
            int lineNum = 0;
            using (StreamReader reader = File.OpenText(this.selectedFile))
            {
                while (reader.ReadLine() != null)
                {
                    lineNum++;
                }
            }

            return string.Format("File: {0}\nLines: {1}\n" +
                "- Infotip displayed by CSShellExtInfotipHandler",
                this.selectedFile, lineNum.ToString());
        }

        /// <summary>
        /// Gets the information flags for an item. The method is not currently used.
        /// </summary>
        /// <returns>Returns the flags for the item.</returns>
        public int GetInfoFlags()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}