'****************************** Module Header ******************************'
' Module Name:  RecipeFileDefinition.vb
' Project:      VBShellExtThumbnailHandler
' Copyright (c) Microsoft Corporation.
' 
' The RecipeFileDefinition class defines the key elements in a recipe file.
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


<ComVisible(False)> _
Public Class RecipeFileDefinition

    Public Sub New(ByVal stream As Stream)
        Dim doc As XDocument = XDocument.Load(stream)
        Me.Title = doc.Root.Element("Title").Value
        Me.Author = doc.Root.Element("Background").Element("Author").Value
        Me.RecipeKeywords = New List(Of String)
        For Each e As XElement In doc.Root.Element("RecipeKeywords").Elements("Keyword")
            Me.RecipeKeywords.Add(e.Value)
        Next
        Me.Info = New RecipeInfo(doc.Root.Element("RecipeInfo"))
        Me.Comments = doc.Root.Element("Comments").Value
        Me.Ingredients = New List(Of RecipeIngredient)
        For Each e As XElement In doc.Root.Element("Ingredients").Elements("Item")
            Me.Ingredients.Add(New RecipeIngredient(e))
        Next
        Me.Directions = New List(Of String)
        For Each e As XElement In doc.Root.Element("Directions").Elements("Step")
            Me.Directions.Add(e.Value)
        Next
        Dim pic As XElement = doc.Root.Element("Attachments").Element("Picture")
        Me.EncodedPicture = pic.Attribute("Source").Value
    End Sub

    ' Properties

    Private _title As String
    Property Title As String
        Get
            Return _title
        End Get
        Private Set(ByVal value As String)
            _title = value
        End Set
    End Property

    Private _author As String
    Property Author As String
        Get
            Return _author
        End Get
        Private Set(ByVal value As String)
            _author = value
        End Set
    End Property

    Private _recipeKeywords As List(Of String)
    Property RecipeKeywords As List(Of String)
        Get
            Return _recipeKeywords
        End Get
        Private Set(ByVal value As List(Of String))
            _recipeKeywords = value
        End Set
    End Property

    Private _info As RecipeInfo
    Property Info As RecipeInfo
        Get
            Return _info
        End Get
        Private Set(ByVal value As RecipeInfo)
            _info = value
        End Set
    End Property

    Private _comments As String
    Property Comments As String
        Get
            Return _comments
        End Get
        Private Set(ByVal value As String)
            _comments = value
        End Set
    End Property

    Private _ingredients As List(Of RecipeIngredient)
    Property Ingredients As List(Of RecipeIngredient)
        Get
            Return _ingredients
        End Get
        Set(ByVal value As List(Of RecipeIngredient))
            _ingredients = value
        End Set
    End Property

    Private _directions As List(Of String)
    Property Directions As List(Of String)
        Get
            Return _directions
        End Get
        Private Set(ByVal value As List(Of String))
            _directions = value
        End Set
    End Property

    Private _encodedPicture As String
    Property EncodedPicture As String
        Get
            Return _encodedPicture
        End Get
        Private Set(ByVal value As String)
            _encodedPicture = value
        End Set
    End Property

End Class


<ComVisible(False)> _
Public Class RecipeInfo

    Public Sub New(ByVal info As XElement)
        Dim diff As XElement = info.Element("Difficulty")
        If (Not diff Is Nothing) Then
            Me.Difficulty = diff.Value
        End If
        Dim prepTime As XElement = info.Element("PreparationTime")
        If (Not prepTime Is Nothing) Then
            Me.PreparationTime = Integer.Parse(prepTime.Value)
        End If
        Dim cookTime As XElement = info.Element("CookTime")
        If (Not cookTime Is Nothing) Then
            Me.CookTime = Integer.Parse(cookTime.Value)
        End If
    End Sub

    ' Properties
    Private _difficulty As String
    Property Difficulty As String
        Get
            Return _difficulty
        End Get
        Private Set(ByVal value As String)
            _difficulty = value
        End Set
    End Property

    Private _preparationTime As Integer
    Property PreparationTime As Integer
        Get
            Return _preparationTime
        End Get
        Private Set(ByVal value As Integer)
            _preparationTime = value
        End Set
    End Property

    Private _cookTime As Integer
    Property CookTime As Integer
        Get
            Return _cookTime
        End Get
        Private Set(ByVal value As Integer)
            _cookTime = value
        End Set
    End Property

End Class


<ComVisible(False)> _
Public Class RecipeIngredient

    Public Sub New(ByVal ingredient As XElement)
        Me.Item = ingredient.Value
        Dim quantity As XAttribute = ingredient.Attribute("Quantity")
        If (Not quantity Is Nothing) Then
            Me.Quantity = Single.Parse(quantity.Value)
        End If
        Dim unit As XAttribute = ingredient.Attribute("Unit")
        If (Not unit Is Nothing) Then
            Me.Unit = unit.Value
        End If
    End Sub

    ' Properties
    Private _item As String
    Property Item As String
        Get
            Return _item
        End Get
        Private Set(ByVal value As String)
            _item = value
        End Set
    End Property

    Private _quantity As Single
    Property Quantity As Single
        Get
            Return _quantity
        End Get
        Private Set(ByVal value As Single)
            _quantity = value
        End Set
    End Property

    Private _unit As String
    Property Unit As String
        Get
            Return _unit
        End Get
        Private Set(ByVal value As String)
            _unit = value
        End Set
    End Property

End Class