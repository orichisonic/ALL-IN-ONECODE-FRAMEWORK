/********************************** Module Header **********************************\
Module Name:  FolderCopyHook.cs
Project:      CSShellExtCopyHookHandler
Copyright (c) Microsoft Corporation.

The FolderCopyHook.cs file defines a folder copy hook handler by implementing the 
ICopyHook interface.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***********************************************************************************/

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;


namespace CSShellExtCopyHookHandler
{
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("0246F393-41BF-446C-B93A-9B58987259C3"), ComVisible(true)]
    public class FolderCopyHook : ICopyHookW
    {
        #region Shell Extension Registration

        [ComRegisterFunction()]
        public static void Register(Type t)
        {
            try
            {
                ShellExtReg.RegisterShellExtFolderCopyHookHandler(
                    "CSShellExtCopyHookHandler", t.GUID);
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
                ShellExtReg.UnregisterShellExtFolderCopyHookHandler(
                    "CSShellExtCopyHookHandler");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Log the error
                throw;  // Re-throw the exception
            }
        }

        #endregion


        #region ICopyHookW Members

        public uint CopyCallback(IntPtr hwnd, FILEOP fileOperation, uint flags, 
            string srcFile, uint srcAttribs, string destFile, uint destAttribs)
        {
            NativeWindow owner = new NativeWindow();
            owner.AssignHandle(hwnd);
            try
            {
                DialogResult result = DialogResult.Yes;

                // If the file name contains "Test" and it is being renamed...
                if (srcFile.Contains("Test") && fileOperation == FILEOP.Rename)
                {
                    result = MessageBox.Show(owner, 
                        String.Format("Are you sure to rename the folder {0} as {1} ?", 
                        srcFile, destFile), "CSShellExtCopyHookHandler", 
                        MessageBoxButtons.YesNoCancel);
                }
                
                Debug.Assert(result == DialogResult.Yes || result == DialogResult.No || 
                    result == DialogResult.Cancel);
                return (uint)result;
            }
            finally
            {
                owner.ReleaseHandle();
            }
        }

        #endregion
    }
}