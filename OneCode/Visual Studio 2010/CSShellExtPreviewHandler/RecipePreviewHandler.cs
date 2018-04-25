/******************************** Module Header ********************************\
Module Name:  RecipePreviewHandler.cs
Project:      CSShellExtPreviewHandler
Copyright (c) Microsoft Corporation.

The .NET 4 code sample demonstrates the C# implementation of a preview 
handler for a new file type registered with the .recipe extension. 

Preview handlers are called when an item is selected to show a lightweight, 
rich, read-only preview of the file's contents in the view's reading pane. 
This is done without launching the file's associated application. Windows 
Vista and later operating systems support preview handlers. 

To be a valid preview handler, several interfaces must be implemented. This 
includes IPreviewHandler (shobjidl.h); IInitializeWithFile, 
IInitializeWithStream, or IInitializeWithItem (propsys.h); IObjectWithSite 
(ocidl.h); and IOleWindow (oleidl.h). There are also optional interfaces, 
such as IPreviewHandlerVisuals (shobjidl.h), that a preview handler can 
implement to provide extended support. Windows API Code Pack for Microsoft 
.NET Framework makes the implementation of these interfaces very easy in .NET.

The example preview handler has the class ID (CLSID): 
    {69FA02A4-19BE-4C49-8D8F-E284E6B01363}

It provides previews for .recipe files. The .recipe file type is simply an 
XML file registered as a unique file name extension. It includes the title of 
the recipe, its author, difficulty, preparation time, cook time, nutrition 
information, comments, an embedded preview image, and so on. The preview 
handler extracts the title, comments, and the embedded image, and display 
them in a preview window.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\*******************************************************************************/

#region Using directives
using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.ShellExtensions;
#endregion


namespace CSShellExtPreviewHandler
{
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("69FA02A4-19BE-4C49-8D8F-E284E6B01363"), ComVisible(true)]
    [PreviewHandler("RecipePreviewHandler", ".recipe", "{020368C1-D205-4699-BF4B-297C592086FD}")]
    public class RecipePreviewHandler : WinFormsPreviewHandler, IPreviewFromStream
    {
        public RecipePreviewHandler()
        {
            this.Control = new RecipePreviewControl();
        }


        #region IPreviewFromStream Members

        public void Load(Stream stream)
        {
            RecipeFileDefinition recipe = new RecipeFileDefinition(stream);
            ((RecipePreviewControl)Control).Populate(recipe);
        }

        #endregion
    }
}