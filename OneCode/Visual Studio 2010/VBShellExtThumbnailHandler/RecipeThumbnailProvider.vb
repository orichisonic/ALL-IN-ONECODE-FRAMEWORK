'****************************** Module Header ******************************'
' Module Name:  RecipeThumbnailProvider.vb
' Project:      VBShellExtThumbnailHandler
' Copyright (c) Microsoft Corporation.
' 
' The RecipeThumbnailProvider.vb file defines a thumbnail handler for 
' .recipe files.
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
Imports System.Runtime.InteropServices
Imports Microsoft.WindowsAPICodePack.ShellExtensions


<ClassInterface(ClassInterfaceType.None), _
Guid("E27243E6-6013-48CE-84EC-76D88A4F5158"), ComVisible(True), _
ThumbnailProvider("RecipeThumbnailProvider", ".recipe")> _
Public Class RecipeThumbnailProvider
    Inherits ThumbnailProvider
    Implements IThumbnailFromStream

#Region "IThumbnailFromStream Members"

    Public Function ConstructBitmap(ByVal stream As Stream, ByVal sideSize As Integer) _
        As Bitmap Implements IThumbnailFromStream.ConstructBitmap

        Dim recipe As New RecipeFileDefinition(stream)
        Dim buffer As Byte() = Convert.FromBase64String(recipe.EncodedPicture)
        Using mstream As MemoryStream = New MemoryStream(buffer)
            Return New Bitmap(mstream)
        End Using
    End Function

#End Region

End Class