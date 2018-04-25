/********************************** Module Header **********************************\
Module Name:  RecipeThumbnailProvider.cs
Project:      CSShellExtThumbnailHandler
Copyright (c) Microsoft Corporation.

The RecipeThumbnailProvider.cs file defines a thumbnail handler for .recipe files.

This source is subject to the Microsoft Public License.
See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***********************************************************************************/

#region Using directives
using System;
using System.IO;
using System.Drawing;
using System.Runtime.InteropServices;
using Microsoft.WindowsAPICodePack.ShellExtensions;
#endregion


namespace CSShellExtThumbnailHandler
{
    [ClassInterface(ClassInterfaceType.None)]
    [Guid("2A736503-DDE4-4876-801D-60063E9E2215"), ComVisible(true)]
    [ThumbnailProvider("RecipeThumbnailProvider", ".recipe")]
    public class RecipeThumbnailProvider : ThumbnailProvider, IThumbnailFromStream
    {
        #region IThumbnailFromStream Members

        public Bitmap ConstructBitmap(Stream stream, int sideSize)
        {
            RecipeFileDefinition recipe = new RecipeFileDefinition(stream);

            byte[] buffer = Convert.FromBase64String(recipe.EncodedPicture);
            using (MemoryStream mstream = new MemoryStream(buffer))
            {
                return new Bitmap(mstream);
            }
        }

        #endregion
    }
}