========================================================================
    Web APPLICATION : CSASPNETCMD Project Overview
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

Step 1. Create a new C# ASP.NET Empty Web Application named CSASPNETCMD.

Step 2. Add App_Code folder, add a class named BatchRunner, add some fields and properties,
        implement ExecuteBatch method.
            try
            {
                System.Diagnostics.ProcessStartInfo psi = GenerateProcessInfo(fileName);

                System.Diagnostics.Process processBatch = System.Diagnostics.Process.Start(psi);
                System.IO.StreamReader myOutput = processBatch.StandardOutput;

                processBatch.WaitForExit();
                if (processBatch.HasExited)
                {
                    retOutPut = myOutput.ReadToEnd();
                }
            }

Step 3. Add an aspx page named RunCmd.aspx , add some controls to it, and also add some 
        validators for these controls.

Step 4. In RunCmd.aspx.cs, write some codes to handle button events.

        protected void btnRunBatch_Click(object sender, EventArgs e)
        {

            // Upload file
            string fileName = HttpContext.Current.Server.MapPath(@"Batchs\" + System.Guid.NewGuid().ToString() + "_" + fileUpload.FileName);
            fileUpload.SaveAs(fileName);

            // Run this batch file.
            string output = RunBatch(fileName);
            // Set result
            tbResult.Text = output;

            // Delete temp file.
            System.IO.File.Delete(fileName);
        }

        protected void btnRunCmd_Click(object sender, EventArgs e)
        {

            // Create a batch for these command.
            string commandLine = this.tbCommand.Text;
            string fileName = HttpContext.Current.Server.MapPath(@"Batchs\"+System.Guid.NewGuid().ToString() + ".bat");
            using (StreamWriter sw = System.IO.File.CreateText(fileName))
            {
                sw.Write(commandLine);
                sw.Flush();
            }

            // Run this batch file.
            string output = RunBatch(fileName);

            // Set result
            tbResult.Text = output;

            // Delete temp file.
            System.IO.File.Delete(fileName);
        }



/////////////////////////////////////////////////////////////////////////////
References:

Processstartinfo Overview 
http://msdn.microsoft.com/en-us/library/system.diagnostics.processstartinfo(VS.81).aspx

/////////////////////////////////////////////////////////////////////////////