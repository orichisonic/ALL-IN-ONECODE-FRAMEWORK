/****************************** Module Header ******************************\
* Module Name:  CppAutomatePowerPoint.cpp
* Project:      CppAutomatePowerPoint
* Copyright (c) Microsoft Corporation.
* 
* The CppAutomatePowerPoint example demonstrates how to write VC++ code to 
* create a Microsoft PowerPoint instance, create a new presentation, insert a 
* new slide, add some texts to the slide, save the presentation, quit 
* PowerPoint and then clean up unmanaged COM resources.
* 
* There are three basic ways you can write VC++ automation codes:
* 
* 1. Automating PowerPoint using the #import directive and smart pointers 
* (Solution1.h/cpp)
* 2. Automating PowerPoint using C++ and the COM APIs (Solution2.h/cpp)
* 3. Automating PowerPoint using MFC (This is not covered in this sample)
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
* EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
* WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/

#pragma region Includes
#include <stdio.h>
#include <windows.h>

#include "Solution1.h"		// The example of using the #import directive 
							// and smart pointers to automate PowerPoint
#include "Solution2.h"		// The example of using the raw COM API to  
							// automate PowerPoint
#pragma endregion


int wmain(int argc, wchar_t* argv[])
{
	HANDLE hThread;

	// Demonstrate automating PowerPoint using the #import directive and 
	// smart pointers in a separate thread.
	hThread = CreateThread(NULL, 0, AutomatePowerPointByImport, NULL, 0, NULL);
	WaitForSingleObject(hThread, INFINITE);
	CloseHandle(hThread);

	_putws(L"");

	// Demonstrate automating PowerPoint using C++ and the COM APIs in a 
	// separate thread.
	hThread = CreateThread(NULL, 0, AutomatePowerPointByCOMAPI, NULL, 0, NULL);
	WaitForSingleObject(hThread, INFINITE);
	CloseHandle(hThread);

	return 0;
}


//
//   FUNCTION: GetModuleDirectory(LPWSTR, DWORD);
//
//   PURPOSE: This is a helper function in this sample. It retrieves the 
//      fully-qualified path for the directory that contains the executable 
//      file of the current process. For example, "D:\Samples\".
//
//   PARAMETERS:
//      * pszDir - A pointer to a buffer that receives the fully-qualified 
//      path for the directory taht contains the executable file of the 
//      current process. If the length of the path is less than the size that 
//      the nSize parameter specifies, the function succeeds and the path is 
//      returned as a null-terminated string.
//      * nSize - The size of the lpFilename buffer, in characters.
//
//   RETURN VALUE: If the function succeeds, the return value is the length 
//      of the string that is copied to the buffer, in characters, not 
//      including the terminating null character. If the buffer is too small 
//      to hold the directory name, the function returns 0 and sets the last 
//      error to ERROR_INSUFFICIENT_BUFFER. If the function fails, the return 
//      value is 0 (zero). To get extended error information, call 
//      GetLastError.
//
DWORD GetModuleDirectory(LPWSTR pszDir, DWORD nSize)
{
	// Retrieve the path of the executable file of the current process.
	nSize = GetModuleFileName(NULL, pszDir, nSize);
	if (!nSize || GetLastError() == ERROR_INSUFFICIENT_BUFFER)
	{
		*pszDir = L'\0'; // Ensure it's NULL terminated
		return 0;
	}

	// Run through looking for the last slash in the file path.
    // When we find it, NULL it to truncate the following filename part.

    for (int i = nSize - 1; i >= 0; i--)
	{
        if (pszDir[i] == L'\\' || pszDir[i] == L'/')
		{
			pszDir[i + 1] = L'\0';
			nSize = i + 1;
            break;
		}
    }
	return nSize;
}