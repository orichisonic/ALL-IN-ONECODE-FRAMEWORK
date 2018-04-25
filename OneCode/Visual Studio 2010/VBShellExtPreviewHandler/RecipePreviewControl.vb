'****************************** Module Header ******************************'
' Module Name:  RecipePreviewControl.vb
' Project:      VBShellExtPreviewHandler
' Copyright (c) Microsoft Corporation.
' 
' The file defines the custom user control to populate the recipe information 
' in the Windows Explorer Preview pane.
' 
' This source is subject to the Microsoft Public License.
' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
' All other rights reserved.
' 
' THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
' EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
' WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'***************************************************************************'

Imports System.IO
Imports System.Drawing


Public Class RecipePreviewControl

    Public Sub Populate(ByVal recipe As RecipeFileDefinition)
        Me.lbTitle.Text = recipe.Title
        Me.tbComments.Text = recipe.Comments

        Dim buffer As Byte() = Convert.FromBase64String(recipe.EncodedPicture)
        Using mstream As New MemoryStream(buffer)
            Me.pctRecipe.Image = Image.FromStream(mstream)
        End Using
    End Sub

End Class
