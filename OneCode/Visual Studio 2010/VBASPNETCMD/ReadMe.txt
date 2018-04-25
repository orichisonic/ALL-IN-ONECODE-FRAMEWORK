========================================================================
    Web APPLICATION : VBASPNETCMD Project Overview
========================================================================

/////////////////////////////////////////////////////////////////////////////
Summary:

The sample demonstrates how to bind an ASP.NET CMD to run batch file or other 
command (command line) within ASP.NET. And how to interact with it.


/////////////////////////////////////////////////////////////////////////////
Demo:


Step 1. Right click RunCmd.aspx, choose "View in Browser".

Step 2. Input your identify informations at first. Including:
        DomainName, UserName and Password. 
        
Step 3. After that, you can send your command in two different ways.

        1) Click "Browse...", choose a .bat file in your computer, then Click 
		   "UploadAndRun" Button.

		2) Input the command directly in textbox, then click "RunCmd" button.


Step 4. If every thing is ok, you will see the result in output textarea.


/////////////////////////////////////////////////////////////////////////////
Implementation:

Step 1. Create a new VB.NET ASP.NET Empty Web Application named VBASPNETCMD.

Step 2. Add App_Code folder, add a class named BatchRunner, add some fields and properties,
        implement ExecuteBatch method.
           Try
            Dim psi As System.Diagnostics.ProcessStartInfo = GenerateProcessInfo(fileName)

            Dim processBatch As System.Diagnostics.Process = System.Diagnostics.Process.Start(psi)
            Dim myOutput As System.IO.StreamReader = processBatch.StandardOutput

            processBatch.WaitForExit()
            If processBatch.HasExited Then
                retOutPut = myOutput.ReadToEnd()
            End If
        Catch winException As System.ComponentModel.Win32Exception
            If winException.Message.Contains("bad password") Then
                retOutPut = winException.Message
            Else
                retOutPut = "Win32Exception occured"
                ' Log exception information
            End If
        Catch exception As System.Exception
            ' Log exception information
            retOutPut = exception.Message
        End Try

Step 3. Add an aspx page named RunCmd.aspx , add some controls to it, and also add some 
        validators for these controls.

Step 4. In RunCmd.aspx.cs, write some codes to handle button events.

            Protected Sub btnRunBatch_Click(ByVal sender As Object, ByVal e As EventArgs)
        ' Upload file
        Dim fileName As String = HttpContext.Current.Server.MapPath("Batchs\" & System.Guid.NewGuid().ToString() & "_" & FileUpload.FileName)
        FileUpload.SaveAs(fileName)

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
        Dim fileName As String = HttpContext.Current.Server.MapPath("Batchs\" & System.Guid.NewGuid().ToString() & ".bat")
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



/////////////////////////////////////////////////////////////////////////////
References:

ASP.NET processstartinfo Overview 
http://msdn.microsoft.com/en-us/library/system.diagnostics.processstartinfo(VS.81).aspx

/////////////////////////////////////////////////////////////////////////////