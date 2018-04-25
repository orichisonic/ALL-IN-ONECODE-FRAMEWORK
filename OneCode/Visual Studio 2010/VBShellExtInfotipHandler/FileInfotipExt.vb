'****************************** Module Header ******************************'
' Module Name:  FileInfotipExt.vb
' Project:      VBShellExtInfotipHandler
' Copyright (c) Microsoft Corporation.
' 
' The FileInfotipExt.vb file defines an infotip handler by implementing the 
' IPersistFile and IQueryInfo interfaces.
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
Imports System.Runtime.InteropServices.ComTypes


<ClassInterface(ClassInterfaceType.None), _
Guid("44BDEF95-C00F-493E-A55B-17937DB1F04E"), ComVisible(True)> _
Public Class FileInfotipExt
    Implements IPersistFile, IQueryInfo

    ' The name of the selected file.
    Private selectedFile As String


#Region "Shell Extension Registration"

    <ComRegisterFunction()> _
    Public Shared Sub Register(ByVal t As Type)
        Try
            ShellExtReg.RegisterShellExtInfotipHandler(t.GUID, ".vb")
        Catch ex As Exception
            Console.WriteLine(ex.Message) ' Log the error
            Throw ' Re-throw the exception
        End Try
    End Sub


    <ComUnregisterFunction()> _
    Public Shared Sub Unregister(ByVal t As Type)
        Try
            ShellExtReg.UnregisterShellExtInfotipHandler(".vb")
        Catch ex As Exception
            Console.WriteLine(ex.Message) ' Log the error
            Throw ' Re-throw the exception
        End Try
    End Sub

#End Region


#Region "IPersistFile Members"

    Public Sub GetClassID(<Out()> ByRef pClassID As Guid) _
        Implements IPersistFile.GetClassID
        Throw New NotImplementedException
    End Sub

    Public Sub GetCurFile(<Out()> ByRef ppszFileName As String) _
        Implements IPersistFile.GetCurFile
        Throw New NotImplementedException
    End Sub

    Public Function IsDirty() As Integer _
        Implements IPersistFile.IsDirty
        Throw New NotImplementedException
    End Function

    ''' <summary>
    ''' Opens the specified file and initializes an object from the file 
    ''' contents.
    ''' </summary>
    ''' <param name="pszFileName">
    ''' The absolute path of the file to be opened.
    ''' </param>
    ''' <param name="dwMode">
    ''' The access mode to be used when opening the file. 
    ''' </param>
    Public Sub Load(ByVal pszFileName As String, ByVal dwMode As Integer) _
        Implements IPersistFile.Load
        ' pszFileName contains the absolute path of the file to be opened.
        Me.selectedFile = pszFileName
    End Sub

    Public Sub Save(ByVal pszFileName As String, ByVal fRemember As Boolean) _
        Implements IPersistFile.Save
        Throw New NotImplementedException
    End Sub

    Public Sub SaveCompleted(ByVal pszFileName As String) _
        Implements IPersistFile.SaveCompleted
        Throw New NotImplementedException
    End Sub

#End Region


#Region "IQueryInfo Members"

    ''' <summary>
    ''' Gets the info tip text for an item.
    ''' </summary>
    ''' <param name="dwFlags">
    ''' Flags that direct the handling of the item from which you're 
    ''' retrieving the info tip text. This value is commonly zero 
    ''' (QITIPF_DEFAULT). 
    ''' </param>
    ''' <returns>
    ''' A Unicode string containing the tip to display.
    ''' </returns>
    Public Function GetInfoTip(ByVal dwFlags As UInt32) As String _
        Implements IQueryInfo.GetInfoTip

        ' Prepare the text of the infotip. The example infotip is composed of 
        ' the file path and the count of code lines.
        Dim lineNum As Integer = 0
        Using reader As StreamReader = File.OpenText(Me.selectedFile)
            Do While (Not reader.ReadLine Is Nothing)
                lineNum += 1
            Loop
        End Using

        Return "File: " & Me.selectedFile & Environment.NewLine & _
            "Lines: " & lineNum.ToString & Environment.NewLine & _
            "- Infotip displayed by VBShellExtInfotipHandler"
    End Function

    ''' <summary>
    ''' Gets the information flags for an item. The method is not currently 
    ''' used.
    ''' </summary>
    ''' <returns>Returns the flags for the item.</returns>
    Public Function GetInfoFlags() As Integer _
        Implements IQueryInfo.GetInfoFlags
        Throw New NotImplementedException
    End Function

#End Region

End Class