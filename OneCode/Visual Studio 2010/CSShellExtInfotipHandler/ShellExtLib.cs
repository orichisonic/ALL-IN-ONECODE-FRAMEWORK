/********************************** Module Header **********************************\
Module Name:  ShellExtLib.cs
Project:      CSShellExtInfotipHandler
Copyright (c) Microsoft Corporation.

The file declares the imported Shell interfaces: IQueryInfo, and implements the 
helper functions for registering and unregistering a shell infotip handler.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***********************************************************************************/

using System;
using Microsoft.Win32;
using System.Runtime.InteropServices;


namespace CSShellExtInfotipHandler
{
    #region Shell Interfaces

    [ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("00021500-0000-0000-c000-000000000046")]
    internal interface IQueryInfo
    {
        // The original signature of GetInfoTip is 
        // HRESULT GetInfoTip(DWORD dwFlags, [out] PWSTR *ppwszTip);
        // According to the documentation, applications that implement this method 
        // must allocate memory for ppwszTip by calling CoTaskMemAlloc. Calling 
        // applications (the Shell in this case) calls CoTaskMemFree to free the 
        // memory when it is no longer needed. Here, we set PreserveSig to false 
        // (the default value in COM) to make the output parameter 'ppwszTip' the 
        // return value. We also marshal the string return value as LPWStr. The 
        // interop layer in CLR will call CoTaskMemAlloc to allocate memory and 
        // marshal the .NET string to the memory. 
        [return: MarshalAs(UnmanagedType.LPWStr)]
        string GetInfoTip(uint dwFlags);

        int GetInfoFlags();
    }

    #endregion


    #region Shell Registration

    internal class ShellExtReg
    {
        /// <summary>
        /// Register the shell infotip handler.
        /// </summary>
        /// <param name="clsid">The CLSID of the component.</param>
        /// <param name="fileType">
        /// The file type that the infotip handler is associated with. For 
        /// example, '*' means all file types; '.txt' means all .txt files. The 
        /// parameter must not be NULL or an empty string. 
        /// </param>
        /// <remarks>
        /// The function creates the following key in the registry.
        ///
        ///   HKCR
        ///   {
        ///      NoRemove &lt;File Type&gt;
        ///      {
        ///          NoRemove shellex
        ///          {
        ///              {00021500-0000-0000-C000-000000000046} = s '{&lt;CLSID&gt;}'
        ///          }
        ///      }
        ///   }
        /// </remarks>
        public static void RegisterShellExtInfotipHandler(Guid clsid, string fileType)
        {
            if (clsid == Guid.Empty)
            {
                throw new ArgumentException("clsid must not be empty");
            }
            if (string.IsNullOrEmpty(fileType))
            {
                throw new ArgumentException("fileType must not be null or empty");
            }

            // If fileType starts with '.', try to read the default value of the 
            // HKCR\<File Type> key which contains the ProgID to which the file type 
            // is linked.
            if (fileType.StartsWith("."))
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(fileType))
                {
                    if (key != null)
                    {
                        // If the key exists and its default value is not empty, use 
                        // the ProgID as the file type.
                        string defaultVal = key.GetValue(null) as string;
                        if (!string.IsNullOrEmpty(defaultVal))
                        {
                            fileType = defaultVal;
                        }
                    }
                }
            }

            // Create the registry key 
            // HKCR\<File Type>\shellex\{00021500-0000-0000-C000-000000000046}, and 
            // sets its default value to the CLSID of the handler.
            string keyName = string.Format(
                @"{0}\shellex\{{00021500-0000-0000-C000-000000000046}}", fileType);
            using (RegistryKey key = Registry.ClassesRoot.CreateSubKey(keyName))
            {
                // Set the default value of the key.
                if (key != null)
                {
                    key.SetValue(null, clsid.ToString("B"));
                }
            }
        }

        /// <summary>
        /// Unregister the shell infotip handler.
        /// </summary>
        /// <param name="fileType">
        /// The file type that the infotip handler is associated with. For 
        /// example, '*' means all file types; '.txt' means all .txt files. The 
        /// parameter must not be NULL or an empty string. 
        /// </param>
        /// <remarks>
        /// The method removes the registry key 
        /// HKCR\&lt;File Type&gt;\shellex\{00021500-0000-0000-C000-000000000046}.
        /// </remarks>
        public static void UnregisterShellExtInfotipHandler(string fileType)
        {
            if (string.IsNullOrEmpty(fileType))
            {
                throw new ArgumentException("fileType must not be null or empty");
            }

            // If fileType starts with '.', try to read the default value of the 
            // HKCR\<File Type> key which contains the ProgID to which the file type 
            // is linked.
            if (fileType.StartsWith("."))
            {
                using (RegistryKey key = Registry.ClassesRoot.OpenSubKey(fileType))
                {
                    if (key != null)
                    {
                        // If the key exists and its default value is not empty, use 
                        // the ProgID as the file type.
                        string defaultVal = key.GetValue(null) as string;
                        if (!string.IsNullOrEmpty(defaultVal))
                        {
                            fileType = defaultVal;
                        }
                    }
                }
            }

            // Remove the registry key:
            // HKCR\<File Type>\shellex\{00021500-0000-0000-C000-000000000046}.
            string keyName = string.Format(
                @"{0}\shellex\{{00021500-0000-0000-C000-000000000046}}", fileType);
            Registry.ClassesRoot.DeleteSubKeyTree(keyName, false);
        }
    }

    #endregion
}