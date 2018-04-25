'****************************** Module Header ******************************'
' Module Name:  RecipePreviewHandler.vb
' Project:      VBShellExtPreviewHandler
' Copyright (c) Microsoft Corporation.
' 
' The .NET 4 code sample demonstrates the VB.NET implementation of a preview 
' handler for a new file type registered with the .recipe extension. 
' 
' Preview handlers are called when an item is selected to show a lightweight, 
' rich, read-only preview of the file's contents in the view's reading pane. 
' This is done without launching the file's associated application. Windows 
' Vista and later operating systems support preview handlers. 
' 
' To be a valid preview handler, several interfaces must be implemented. This 
' includes IPreviewHandler (shobjidl.h); IInitializeWithFile, 
' IInitializeWithStream, or IInitializeWithItem (propsys.h); IObjectWithSite 
' (ocidl.h); and IOleWindow (oleidl.h). There are also optional interfaces, 
' such as IPreviewHandlerVisuals (shobjidl.h), that a preview handler can 
' implement to provide extended support. Windows API Code Pack for Microsoft 
' .NET Framework makes the implementation of these interfaces very easy in .NET.
'
' The example preview handler has the class ID (CLSID): 
'     {9BF000CC-94E0-4CA6-960A-CE9338319A81}
'
' It provides previews for .recipe files. The .recipe file type is simply an 
' XML file registered as a unique file name extension. It includes the title of 
' the recipe, its author, difficulty, preparation time, cook time, nutrition 
' information, comments, an embedded preview image, and so on. The preview 
' handler extracts the title, comments, and the embedded image, and display 
' them in a preview window.
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
Imports System.Runtime.InteropServices
Imports Microsoft.WindowsAPICodePack.ShellExtensions


<ClassInterface(ClassInterfaceType.None), _
Guid("9BF000CC-94E0-4CA6-960A-CE9338319A81"), ComVisible(True), _
PreviewHandler("RecipePreviewHandler", ".recipe", "{963C8DF6-CF61-4A5D-B51B-951B5911DDD0}")> _
Public Class RecipePreviewHandler
    Inherits WinFormsPreviewHandler
    Implements IPreviewFromStream

    Sub New()
        Me.Control = New RecipePreviewControl
    End Sub

#Region "IPreviewFromStream Members"

    Public Sub Load(ByVal stream As Stream) Implements IPreviewFromStream.Load
        Dim recipe As New RecipeFileDefinition(stream)
        DirectCast(MyBase.Control, RecipePreviewControl).Populate(recipe)
    End Sub

#End Region

End Class