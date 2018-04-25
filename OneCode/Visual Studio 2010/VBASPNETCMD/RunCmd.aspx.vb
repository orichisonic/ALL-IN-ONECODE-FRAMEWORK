'****************************** Module Header ******************************'
'Module Name:  RunCmd.aspx.cs
'Project:      CSASPNETCMD
'Copyright (c) Microsoft Corporation.
'
'The RunCmd Class is responsible for getting inputs from user and output the result.
'
'This source is subject to the Microsoft Public License.
'See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
'All other rights reserved.

'THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
'EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
'WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
'***************************************************************************'

Imports System.IO

Partial Public Class RunCmd
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)

    End Sub


    Protected Sub btnRunBatch_Click(ByVal sender As Object, ByVal e As EventArgs)

        ' Upload file
        Dim fileName As String = HttpContext.Current.Server.MapPath(
            "Batchs\" & System.Guid.NewGuid().ToString() & "_" & fileUpload.FileName)
        fileUpload.SaveAs(fileName)

        ' Run this batch file
        Dim output As String = RunBatch(fileName)

        ' Set result
        tbResult.Text = output

        ' Delete temp file
        System.IO.File.Delete(fileName)
    End Sub

    Protected Sub btnRunCmd_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Create a batch for these command
        Dim commandLine As String = Me.tbCommand.Text
        Dim fileName As String = HttpContext.Current.Server.MapPath(
            "Batchs\" & System.Guid.NewGuid().ToString() & ".bat")
        Using sw As StreamWriter = System.IO.File.CreateText(fileName)
            sw.Write(commandLine)
            sw.Flush()
        End Using

        ' Run this batch file
        Dim output As String = RunBatch(fileName)

        ' Set result
        tbResult.Text = output

        ' Delete temp file
        System.IO.File.Delete(fileName)
    End Sub

    ''' <summary>
    ''' Use BatchRunner class to run this batch
    ''' </summary>
    ''' <param name="fileName">The batch file with fullname</param>
    ''' <returns></returns>
    Private Function RunBatch(ByVal fileName As String) As String

        ' Set batch execute information
        Dim batchRunner As New BatchRunner()
        batchRunner.DomainName = tbDomainName.Text.Trim()
        batchRunner.UserName = tbUserName.Text.Trim()
        batchRunner.Password = tbPassword.Text.Trim()

        ' Run batch file
        Dim output As String = batchRunner.ExecuteBatch(fileName)

        Return output
    End Function
End Class