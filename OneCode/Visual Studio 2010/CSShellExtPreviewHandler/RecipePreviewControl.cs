/********************************** Module Header **********************************\
Module Name:  RecipePreviewControl.cs
Project:      CSShellExtPreviewHandler
Copyright (c) Microsoft Corporation.

The file defines the custom user control to populate the recipe information in the 
Windows Explorer Preview pane.

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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
#endregion


namespace CSShellExtPreviewHandler
{
    public partial class RecipePreviewControl : UserControl
    {
        public RecipePreviewControl()
        {
            InitializeComponent();
        }

        public void Populate(RecipeFileDefinition recipe)
        {
            this.lbTitle.Text = recipe.Title;
            this.tbComments.Text = recipe.Comments;

            byte[] buffer = Convert.FromBase64String(recipe.EncodedPicture);
            using (MemoryStream mstream = new MemoryStream(buffer))
            {
                this.pctRecipe.Image = Image.FromStream(mstream);
            }
        }
    }
}
