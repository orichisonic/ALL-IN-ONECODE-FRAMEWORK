/****************************** Module Header ******************************\
* Module Name:  WinINet.cs
* Project:	    CSWebBrowserWithProxy
* Copyright (c) Microsoft Corporation.
* 
* This class is a simple .NET wrapper of wininet.dll. It contains 2 extern
* methods (InternetSetOption and InternetQueryOption) of wininet.dll. This 
* class can be used to set proxy, disable proxy, backup internet options 
* and restore internet options.
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
using System.Runtime.InteropServices;

namespace CSWebBrowserWithProxy
{
    public static class WinINet
    {
      

        /// <summary>
        /// Sets an Internet option.
        /// </summary>
        [DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern bool InternetSetOption(
            IntPtr hInternet,
            INTERNET_OPTION dwOption,
            IntPtr lpBuffer,
            int lpdwBufferLength);

        /// <summary>
        /// Queries an Internet option on the specified handle. The Handle will be always 0.
        /// </summary>
        [DllImport("wininet.dll", CharSet = CharSet.Ansi, SetLastError = true,
            EntryPoint = "InternetQueryOption")]
        private extern static bool InternetQueryOptionList(
            IntPtr Handle,
            INTERNET_OPTION OptionFlag,
            ref INTERNET_PER_CONN_OPTION_LIST OptionList,
            ref int size);

        /// <summary>
        /// Set the proxy server for LAN connection.
        /// </summary>
        public static bool SetConnectionProxy(string proxyServer)
        {
            // Create 3 options.
            INTERNET_PER_CONN_OPTION[] Options = new INTERNET_PER_CONN_OPTION[3];

            // Set PROXY flags.
            Options[0] = new INTERNET_PER_CONN_OPTION();
            Options[0].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_FLAGS;
            Options[0].Value.dwValue = (int)INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_PROXY;

            // Set proxy name.
            Options[1] = new INTERNET_PER_CONN_OPTION();
            Options[1].dwOption =
                (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_SERVER;
            Options[1].Value.pszValue = Marshal.StringToHGlobalAnsi(proxyServer);

            // Set proxy bypass.
            Options[2] = new INTERNET_PER_CONN_OPTION();
            Options[2].dwOption =
                (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_BYPASS;
            Options[2].Value.pszValue = Marshal.StringToHGlobalAnsi("local");

            // Allocate a block of memory of the options.
            System.IntPtr buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(Options[0])
                + Marshal.SizeOf(Options[1]) + Marshal.SizeOf(Options[2]));

            System.IntPtr current = buffer;

            // Marshal data from a managed object to an unmanaged block of memory.
            for (int i = 0; i < Options.Length; i++)
            {
                Marshal.StructureToPtr(Options[i], current, false);
                current = (System.IntPtr)((int)current + Marshal.SizeOf(Options[i]));
            }

            // Initialize a INTERNET_PER_CONN_OPTION_LIST instance.
            INTERNET_PER_CONN_OPTION_LIST option_list = new INTERNET_PER_CONN_OPTION_LIST();

            // Point to the allocated memory.
            option_list.pOptions = buffer;

            // Return the unmanaged size of an object in bytes.
            option_list.Size = Marshal.SizeOf(option_list);

            // IntPtr.Zero means LAN connection.
            option_list.Connection = IntPtr.Zero;

            option_list.OptionCount = Options.Length;
            option_list.OptionError = 0;
            int size = Marshal.SizeOf(option_list);

            // Allocate memory for the INTERNET_PER_CONN_OPTION_LIST instance.
            IntPtr intptrStruct = Marshal.AllocCoTaskMem(size);

            // Marshal data from a managed object to an unmanaged block of memory.
            Marshal.StructureToPtr(option_list, intptrStruct, true);

            // Set internet settings.
            bool bReturn = InternetSetOption(IntPtr.Zero,
                INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION, intptrStruct, size);

            // Free the allocated memory.
            Marshal.FreeCoTaskMem(buffer);
            Marshal.FreeCoTaskMem(intptrStruct);

            // Throw an exception if this operation failed.
            if (!bReturn)
            {
                throw new ApplicationException(" Set Internet Option Failed!");
            }

            // Notify the system that the registry settings have been changed and cause
            // the proxy data to be reread from the registry for a handle.
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_SETTINGS_CHANGED,
                IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_REFRESH,
                IntPtr.Zero, 0);

            return bReturn;
        }

        /// <summary>
        /// Disable Proxy Server for LAN connection.
        /// </summary>
        /// <returns></returns>
        public static bool DisableConnectionProxy()
        {
            // Make connection access to internet directly.
            INTERNET_PER_CONN_OPTION[] Options = new INTERNET_PER_CONN_OPTION[1];
            Options[0].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_FLAGS;
            Options[0].Value.dwValue = (int)INTERNET_OPTION_PER_CONN_FLAGS.PROXY_TYPE_DIRECT;

            // Marshal data from a managed object to an unmanaged block of memory.
            System.IntPtr buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(Options[0]));
            Marshal.StructureToPtr(Options[0], buffer, false);

            // Initialize a INTERNET_PER_CONN_OPTION_LIST instance.
            INTERNET_PER_CONN_OPTION_LIST request = new INTERNET_PER_CONN_OPTION_LIST();

            // Point to the allocated memory.
            request.pOptions = buffer;

            request.Size = Marshal.SizeOf(request);

            // IntPtr.Zero means LAN connection.
            request.Connection = IntPtr.Zero;
            request.OptionCount = Options.Length;
            request.OptionError = 0;
            int size = Marshal.SizeOf(request);

            // Allocate memory for the INTERNET_PER_CONN_OPTION_LIST instance.
            IntPtr intptrStruct = Marshal.AllocCoTaskMem(size);

            // Marshal data from a managed object to an unmanaged block of memory.
            Marshal.StructureToPtr(request, intptrStruct, true);

            // Set internet settings.
            bool bReturn = InternetSetOption(IntPtr.Zero,
        INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION, intptrStruct, size);

            // Free the allocated memory.
            Marshal.FreeCoTaskMem(buffer);
            Marshal.FreeCoTaskMem(intptrStruct);

            // Throw an exception if this operation failed.
            if (!bReturn)
            {
                throw new ApplicationException(" Set Internet Option Failed! ");
            }

            // Notify the system that the registry settings have been changed and cause
            // the proxy data to be reread from the registry for a handle.
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_SETTINGS_CHANGED,
                IntPtr.Zero, 0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_REFRESH,
                IntPtr.Zero, 0);

            return bReturn;
        }

        /// <summary>
        /// Backup the current options for LAN connection.
        /// Make sure free the memory after restoration. 
        /// </summary>
        public static INTERNET_PER_CONN_OPTION_LIST BackupConnectionProxy()
        {

            // Query following options. 
            INTERNET_PER_CONN_OPTION[] Options = new INTERNET_PER_CONN_OPTION[3];

            Options[0] = new INTERNET_PER_CONN_OPTION();
            Options[0].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_FLAGS;
            Options[1] = new INTERNET_PER_CONN_OPTION();
            Options[1].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_SERVER;
            Options[2] = new INTERNET_PER_CONN_OPTION();
            Options[2].dwOption = (int)INTERNET_PER_CONN_OptionEnum.INTERNET_PER_CONN_PROXY_BYPASS;

            // Allocate a block of memory of the options.
            System.IntPtr buffer = Marshal.AllocCoTaskMem(Marshal.SizeOf(Options[0])
                + Marshal.SizeOf(Options[1]) + Marshal.SizeOf(Options[2]));

            System.IntPtr current = (System.IntPtr)buffer;

            // Marshal data from a managed object to an unmanaged block of memory.
            for (int i = 0; i < Options.Length; i++)
            {
                Marshal.StructureToPtr(Options[i], current, false);
                current = (System.IntPtr)((int)current + Marshal.SizeOf(Options[i]));
            }

            // Initialize a INTERNET_PER_CONN_OPTION_LIST instance.
            INTERNET_PER_CONN_OPTION_LIST Request = new INTERNET_PER_CONN_OPTION_LIST();

            // Point to the allocated memory.
            Request.pOptions = buffer;

            Request.Size = Marshal.SizeOf(Request);

            // IntPtr.Zero means LAN connection.
            Request.Connection = IntPtr.Zero;

            Request.OptionCount = Options.Length;
            Request.OptionError = 0;
            int size = Marshal.SizeOf(Request);

            // Query internet options. 
            bool result = InternetQueryOptionList(IntPtr.Zero, 
                INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION, 
                ref Request, ref size);
            if (!result)
            {
                throw new ApplicationException(" Set Internet Option Failed! ");
            }

            return Request;
        }

        /// <summary>
        /// Restore the options for LAN connection.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool RestoreConnectionProxy(INTERNET_PER_CONN_OPTION_LIST request)
        {
            int size = Marshal.SizeOf(request);

            // Allocate memory. 
            IntPtr intptrStruct = Marshal.AllocCoTaskMem(size);

            // Convert structure to IntPtr 
            Marshal.StructureToPtr(request, intptrStruct, true);

            // Set internet options.
            bool bReturn = InternetSetOption(IntPtr.Zero,
                INTERNET_OPTION.INTERNET_OPTION_PER_CONNECTION_OPTION, 
                intptrStruct, size);

            // Free the allocated memory.
            Marshal.FreeCoTaskMem(request.pOptions);
            Marshal.FreeCoTaskMem(intptrStruct);

            if (!bReturn)
            {
                throw new ApplicationException(" Set Internet Option Failed! ");
            }

            // Notify the system that the registry settings have been changed and cause
            // the proxy data to be reread from the registry for a handle.
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_SETTINGS_CHANGED,
                IntPtr.Zero,0);
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION.INTERNET_OPTION_REFRESH,
                IntPtr.Zero,0);


            return bReturn;
        }
    }
}
