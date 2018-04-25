=============================================================================
        CLASS LIBRARY : CSShellExtCopyHookHandler Project Overview
=============================================================================

/////////////////////////////////////////////////////////////////////////////
Summary:

The C# code sample demonstrates creating a Shell copy hook handler with .NET 
Framework 4.

Normally, users and applications can copy, move, delete, or rename folders 
with few restrictions. By implementing a copy hook handler, you can control 
whether or not these operations take place. For instance, implementing such a 
handler allows you to prevent critical folders from being renamed or deleted. 
Copy hook handlers can also be implemented for printer objects.

Copy hook handlers are global. The Shell calls all registered handlers every 
time an application or user attempts to copy, move, delete, or rename a 
folder or printer object. The handler does not perform the operation itself. 
It only approves or vetoes it. If all handlers approve, the Shell does the 
operation. If any handler vetoes the operation, it is canceled and the 
remaining handlers are not called. Copy hook handlers are not informed of the 
success or failure of the operation, so they cannot be used to monitor file 
operations.

Like most Shell extension handlers, copy hook handlers are in-process 
Component Object Model (COM) objects implemented as DLLs. They export one 
interface in addition to IUnknown: ICopyHook. The Shell initializes the 
handler directly.

Prior to .NET Framework 4, the development of in-process shell extensions 
using managed code is not officially supported because of the CLR limitation 
allowing only one .NET runtime per process. Jesse Kaplan, one of the CLR 
program managers, explains it in this MSDN forum thread: 
http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/1428326d-7950-42b4-ad94-8e962124043e.

In .NET 4, with the ability to have multiple runtimes in process with any 
other runtime, Microsoft can now offer general support for writing managed 
shell extensions—even those that run in-process with arbitrary applications 
on the machine. CSShellExtCopyHookHandler is such a managed shell extension 
code example. However, please note that you still cannot write shell 
extensions using any version earlier than .NET Framework 4 because those 
versions of the runtime do not load in-process with one another and will 
cause failures in many cases.

The example copy hook handler has the class ID (CLSID): 
    {0246F393-41BF-446C-B93A-9B58987259C3}

It hooks the renaming operation of folders in Windows Explorer. When you are 
renaming a folder whose name contains "Test" in the Shell, the copy hook 
handler pops up a message box, asking if the user really wants to rename the 
folder. If the user clicks "Yes", the operation will proceed. If the user 
clicks "No" or "Cancel", the renaming operation is cancelled. 


/////////////////////////////////////////////////////////////////////////////
Setup and Removal:

--------------------------------------
In the Development Environment

A. Setup

Run 'Visual Studio Command Prompt (2010)' (or 'Visual Studio x64 Win64 
Command Prompt (2010)' if you are on a x64 operating system) in the Microsoft 
Visual Studio 2010 \ Visual Studio Tools menu as administrator. Navigate to 
the folder that contains the build result CSShellExtCopyHookHandler.dll and 
enter the command:

    Regasm.exe CSShellExtCopyHookHandler.dll /codebase

The copy hook handler is registered successfully if the command prints:

    "Types registered successfully"

NOTE:
After the registration, you must restart Windows Explorer (explorer.exe) so 
that the Shell loads the new copy hook handler. 

    Official way to restart explorer
    http://weblogs.asp.net/whaggard/archive/2003/03/12/3729.aspx

The reason is that the shell builds and caches a list of registered copy hook 
handlers the first time copy hook handlers are called in a process. Once the
list is created, there is no mechanism for updating or flushing the cache 
other than terminating the process. The best option that we can offer at this 
point is to restart the explorer.exe process or reboot the system after the 
copy hook handler is registered.

B. Removal

Run 'Visual Studio Command Prompt (2010)' (or 'Visual Studio x64 Win64 
Command Prompt (2010)' if you are on a x64 operating system) in the Microsoft 
Visual Studio 2010 \ Visual Studio Tools menu as administrator. Navigate to 
the folder that contains the build result CSShellExtCopyHookHandler.dll and  
enter the command:

    Regasm.exe CSShellExtCopyHookHandler.dll /unregister

The copy hook handler is unregistered successfully if the command prints:

    "Types un-registered successfully"

NOTE:
After the registration, you still need to restart Windows Explorer 
(explorer.exe) or reboot the system in order that the copy hook handler is 
unloaded from the Shell.

--------------------------------------
In the Deployment Environment

A. Setup

Install CSShellExtCopyHookHandlerSetup(x86).msi, the output of the 
CSShellExtCopyHookHandlerSetup(x86) setup project, on a x86 operating 
system. If the target operating system is x64, install 
CSShellExtCopyHookHandlerSetup(x64).msi outputted by the 
CSShellExtCopyHookHandlerSetup(x64) setup project.

NOTE: 
The setup application will ask you to restart Windows Explorer (explore.exe) 
after the installation so that the Shell loads the new copy hook handler. 

    Official way to restart explorer
    http://weblogs.asp.net/whaggard/archive/2003/03/12/3729.aspx

B. Removal

Uninstall CSShellExtCopyHookHandlerSetup(x86).msi, the output of the 
CSShellExtCopyHookHandlerSetup(x86) setup project, on a x86 operating 
system. If the target operating system is x64, uninstall 
CSShellExtCopyHookHandlerSetup(x64).msi outputted by the 
CSShellExtCopyHookHandlerSetup(x64) setup project.

NOTE:
The setup application will attempt to restart Windows Explorer (explorer.exe)
during the uninstallation so that the copy hook handler is unloaded from the 
Shell. 


/////////////////////////////////////////////////////////////////////////////
Demo:

The following steps walk through a demonstration of the copy hook handler 
code sample.

Step1. After you successfully build the sample project in Visual Studio 2010, 
you will get a DLL: CSShellExtCopyHookHandler.dll. Run 'Visual Studio Command 
Prompt (2010)' (or 'Visual Studio x64 Win64 Command Prompt (2010)' if you are 
on a x64 operating system) in the Microsoft Visual Studio 2010 \ Visual 
Studio Tools menu as administrator. Navigate to the folder that contains the 
build result CSShellExtCopyHookHandler.dll and enter the command:

    Regasm.exe CSShellExtCopyHookHandler.dll /codebase

The copy hook handler is registered successfully if the command prints:

    "Types registered successfully"

NOTE:
After the registration, you must restart Windows Explorer (explorer.exe) so 
that the Shell loads the new copy hook handler. 

    Official way to restart explorer
    http://weblogs.asp.net/whaggard/archive/2003/03/12/3729.aspx

Step2. Find a folder whose name contains "Test" in the Windows Explorer (e.g. 
the "TestFolder" folder in the sample directory). Rename the folder. A 
message box will be prompted with the message:

    "Are you sure to rename the folder '<Path of TestFolder>' as 
    '<New folder name>' ?" 

If you click "Yes", the operation will proceed. If you click "No" or "Cancel", 
the renaming operation is cancelled. 

Step3. In the same Visual Studio command prompt, run the command 

    Regasm.exe CSShellExtCopyHookHandler.dll /unregister

to unregister the Shell copy hook handler.

NOTE:
After the registration, you still need to restart Windows Explorer 
(explorer.exe) or reboot the system in order that the copy hook handler is 
unloaded from the Shell.


/////////////////////////////////////////////////////////////////////////////
Implementation:

A. Creating and configuring the project

In Visual Studio 2010, create a Visual C# / Windows / Class Library project 
named "CSShellExtCopyHookHandler". Open the project properties, and in the 
Signing page, sign the assembly with a strong name key file. 

-----------------------------------------------------------------------------

B. Implementing a basic Component Object Model (COM) DLL

Shell extension handlers are all in-process COM objects implemented as DLLs. 
Making a basic .NET COM component is very straightforward. You just need to 
define a 'public' class with ComVisible(true), use the Guid attribute to 
specify its CLSID, and explicitly implements certain COM interfaces. For 
example, 

    [ClassInterface(ClassInterfaceType.None)]
    [Guid("0246F393-41BF-446C-B93A-9B58987259C3"), ComVisible(true)]
    public class SimpleObject : ISimpleObject
    {
        ... // Implements the interface
    }

You even do not need to implement IUnknown and class factory by yourself 
because .NET Framework handles them for you.

-----------------------------------------------------------------------------

C. Implementing the copy hook handler and registering it for folder objects.

-----------
Implementing the copy hook handler:

Like all Shell extension handlers, copy hook handlers are in-process COM 
objects implemented as DLLs. They export one interface in addition to 
IUnknown: ICopyHook. The Shell initializes the handler directly, so there is 
no need for an initialization interface such as IShellExtInit.

    [ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
    [Guid("000214FC-0000-0000-C000-000000000046")]
    internal interface ICopyHookW
    {
        [PreserveSig]
        uint CopyCallback(
            IntPtr hwnd,
            FILEOP fileOperation,
            uint flags,
            [MarshalAs(UnmanagedType.LPWStr)]
            string srcFile,
            uint srcAttribs,
            [MarshalAs(UnmanagedType.LPWStr)]
            string destFile,
            uint destAttribs);
    }

    [ClassInterface(ClassInterfaceType.None)]
    [Guid("0246F393-41BF-446C-B93A-9B58987259C3"), ComVisible(true)]
    public class FolderCopyHook : ICopyHookW
    {
        public uint CopyCallback(IntPtr hwnd, FILEOP fileOperation, uint flags, 
            string srcFile, uint srcAttribs, string destFile, uint destAttribs)
        {
            ...
        }
    }

When you write your own handler, you must create a new CLSID by using the 
"Create GUID" tool in the Tools menu for the shell extension class, and 
specify the value in the Guid attribute. 

    ...
    [Guid("0246F393-41BF-446C-B93A-9B58987259C3"), ComVisible(true)]
    public class FolderCopyHook : ...

  1. Implementing ICopyHook

  The ICopyHook interface has a single method, ICopyHook::CopyCallback. When 
  a folder is about to be moved, the Shell calls this method. It passes in a 
  variety of information, including:

    * The folder's name. 
    * The folder's destination or new name. 
    * The operation that is being attempted. 
    * The attributes of the source and destination folders. 
    * A window handle that can be used to display a user interface. 

  When your handler's ICopyHook.CopyCallback method is called, it returns 
  one of the three following values to indicate to the Shell how it should 
  proceed.

    * IDYES (DialogResult.Yes): Allows the operation 
    * IDNO (DialogResult.No): Prevents the operation on this folder. The 
      Shell can continue with any other operations that have been approved, 
      such as a batch copy operation. 
    * IDCANCEL (DialogResult.Cancel): Prevents the current operation and 
      cancels any pending operations.

  In this sample folder copy hook, we check if the folder is being renamed, 
  and if the original folder name contains "Test". If it's true, we prompt a 
  message box asking the user to confirm the operation. 
  
    public uint CopyCallback(IntPtr hwnd, FILEOP fileOperation, uint flags, 
        string srcFile, uint srcAttribs, string destFile, uint destAttribs)
    {
        NativeWindow owner = new NativeWindow();
        owner.AssignHandle(hwnd);
        try
        {
            DialogResult result = DialogResult.Yes;

            // If the file name contains "Test" and it is being renamed...
            if (srcFile.Contains("Test") && fileOperation == FILEOP.Rename)
            {
                result = MessageBox.Show(owner, 
                    String.Format("Are you sure to rename the folder {0} as {1} ?", 
                    srcFile, destFile), "CSShellExtCopyHookHandler", 
                    MessageBoxButtons.YesNoCancel);
            }
                
            Debug.Assert(result == DialogResult.Yes || result == DialogResult.No || 
                result == DialogResult.Cancel);
            return (uint)result;
        }
        finally
        {
            owner.ReleaseHandle();
        }
    }

A folder object can have multiple copy hook handlers. For example, even if 
the Shell already has a copy hook handler registered for a particular folder 
object, you can still register one of your own. If two or more copy hook 
handlers are registered for an object, the Shell simply calls each of them 
before performing one of the specified file system operations.

-----------
Registering the folder copy hook handler:

Copy hook handlers for folders are registered under the key:

    HKEY_CLASSES_ROOT\Directory\shellex\CopyHookHandlers

Create a subkey of CopyHookHandlers named for the handler, and set the 
subkey's default value to the string form of the handler's class identifier 
(CLSID) GUID.

The registration of the copy hook handler is implemented in the Register 
method of FolderCopyHook. The ComRegisterFunction attribute attached to the 
method enables the execution of user-written code other than the basic 
registration of the COM class. Register calls the 
ShellExtReg.RegisterShellExtFolderCopyHookHandler method in ShellExtLib.cs to 
register the folder copy hook handler. The following keys and values are 
added in the registration process of the sample handler. 

    HKCR
    {
        NoRemove Directory
        {
            NoRemove shellex
            {
                NoRemove CopyHookHandlers
                {
                    CSShellExtCopyHookHandler = 
                        s '{0246F393-41BF-446C-B93A-9B58987259C3}'
                }
            }
        }
    }

The shell builds and caches a list of registered copy hook handlers the first 
time copy hook handlers are called in a process. Once the list is created, 
there is no mechanism for updating or flushing the cache other than 
terminating the process. This applies to Windows Explorer and any other 
process that may call shell file functions, such as SHFileOperation. In other 
that your copy hook handler is recognized by the Shell after the handler is 
registered, the best option that we can offer at this point is to restart the 
Windows Explorer (explorer.exe) process or reboot the system.

The unregistration is implemented in the Unregister method of FolderCopyHook. 
Similar to the Register method, the ComUnregisterFunction attribute attached 
to the method enables the execution of user-written code during the 
unregistration process. It removes the CSShellExtCopyHookHandler key under 
HKCR\Directory\shellex\CopyHookHandlers. After the key is removed, you still 
need to restart the Windows Explorer (explorer.exe) process in order that the 
Shell unloads the copy hook handler.

Copy hook handlers for printer objects are registered in essentially the same 
way. The only difference is that you must register them under the 
HKEY_CLASSES_ROOT\Printers key. 

-----------------------------------------------------------------------------

D. Deploying the copy hook handler with a setup project.

  (1) In CSShellExtCopyHookHandler, add an Installer class (named 
  ProjectInstaller in this code sample) to define the custom actions in the 
  setup. The class derives from System.Configuration.Install.Installer. We 
  use the custom actions to register/unregister the COM-visible classes in 
  the current managed assembly when user installs/uninstalls the component. 

    [RunInstaller(true), ComVisible(false)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            // Call RegistrationServices.RegisterAssembly to register the classes in 
            // the current managed assembly to enable creation from COM.
            RegistrationServices regService = new RegistrationServices();
            regService.RegisterAssembly(
                this.GetType().Assembly, 
                AssemblyRegistrationFlags.SetCodeBase);

            // Inform the user to restart Windows Explorer (explorer.exe).
            MessageBox.Show("You need to restart Windows Explorer (explorer.exe) " +
                "to load the copy handler.  Please restart explorer.exe manually " +
                "after the installation.", "Restart Windows Explorer");
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            // Call RegistrationServices.UnregisterAssembly to unregister the classes 
            // in the current managed assembly.
            RegistrationServices regService = new RegistrationServices();
            regService.UnregisterAssembly(this.GetType().Assembly);
        }
    }

  In the overriden Install method, we use RegistrationServices.RegisterAssembly 
  to register the classes in the current managed assembly to enable creation 
  from COM. The method also invokes the static method marked with 
  ComRegisterFunctionAttribute to perform the custom COM registration steps. 
  The call is equivalent to running the command: 
  
    "Regasm.exe CSShellExtCopyHookHandler.dll /codebase"

  in the development environment. 

  (2) To add a deployment project that deploys the Shell extension handler to 
  a x86 operating system, on the File menu, point to Add, and then click New 
  Project. In the Add New Project dialog box, expand the Other Project Types 
  node, expand the Setup and Deployment Projects, click Visual Studio 
  Installer, and then click Setup Project. In the Name box, type 
  CSShellExtCopyHookHandlerSetup(x86). Click OK to create the project. In the 
  Properties dialog of the setup project, make sure that the TargetPlatform 
  property is set to x86. This setup project is to be deployed on x86 Windows 
  operating systems. 

  Right-click the setup project, and choose Add / Project Output ... 
  In the Add Project Output Group dialog box, CSShellExtCopyHookHandler will  
  be displayed in the Project list. Select Primary Output and click OK.

  Right-click the setup project again, and choose View / Custom Actions. 
  In the Custom Actions Editor, right-click the root Custom Actions node. On 
  the Action menu, click Add Custom Action. In the Select Item in Project 
  dialog box, double-click the Application Folder. Select Primary output from 
  CSShellExtCopyHookHandler. This adds the custom actions that we defined in
  ProjectInstaller of CSShellExtCopyHookHandler. 

  Right-click the setup project and select Properties. Click the 
  Prerequisites... button. In the Prerequisites dialog box, uncheck the 
  Microsoft .NET Framework 4 Client Profile (x86 and x64) option, and check 
  the Microsoft .NET Framework 4 (x86 and x64) option to match the .NET 
  Framework version of CSShellExtCopyHookHandler. 

  Build the setup project. If the build succeeds, you will get a .msi file 
  and a Setup.exe file. You can distribute them to your users to install or 
  uninstall your Shell extension handler. 

  (3) To deploy the Shell extension handler to a x64 operating system, you 
  must create a new setup project (e.g. CSShellExtCopyHookHandlerSetup(x64) 
  in this code sample), and set its TargetPlatform property to x64. 

  Although the TargetPlatform property is set to x64, the native shim 
  packaged with the .msi file is still a 32-bit executable. The Visual Studio 
  embeds the 32-bit version of InstallUtilLib.dll into the Binary table as 
  InstallUtil. So the custom actions will be run in the 32-bit, which is 
  unexpected in this code sample. To workaround this issue and ensure that 
  the custom actions run in the 64-bit mode, you either need to import the 
  appropriate bitness of InstallUtilLib.dll into the Binary table for the 
  InstallUtil record or - if you do have or will have 32-bit managed custom 
  actions add it as a new record in the Binary table and adjust the 
  CustomAction table to use the 64-bit Binary table record for 64-bit managed 
  custom actions. This blog article introduces how to do it manually with 
  Orca http://blogs.msdn.com/b/heaths/archive/2006/02/01/64-bit-managed-custom-actions-with-visual-studio.aspx

  In this code sample, we automate the modification of InstallUtil by using a 
  post-build javascript: Fix64bitInstallUtilLib.js. You can find the script 
  file in the CSShellExtCopyHookHandlerSetup(x64) project folder. To 
  configure the script to run in the post-build event, you select the 
  CSShellExtCopyHookHandlerSetup(x64) project in Solution Explorer, and find 
  the PostBuildEvent property in the Properties window. Specify its value to
  be 
  
	"$(ProjectDir)Fix64bitInstallUtilLib.js" "$(BuiltOuputPath)" "$(ProjectDir)"

  Repeat the rest steps in (2) to add the project output, set the custom 
  actions, configure the prerequisites, and build the setup project.


/////////////////////////////////////////////////////////////////////////////
References:

MSDN: Creating Copy Hook Handlers
http://msdn.microsoft.com/en-us/library/cc144063.aspx

MSDN: ICopyHook Interface
http://msdn.microsoft.com/en-us/library/bb776049.aspx


/////////////////////////////////////////////////////////////////////////////