=============================================================================
         CLASS LIBRARY : VBShellExtPreviewHandler Project Overview
=============================================================================

/////////////////////////////////////////////////////////////////////////////
Summary:

The .NET 4 code sample demonstrates the VB.NET implementation of a preview 
handler for a new file type registered with the .recipe extension. 

Preview handlers are called when an item is selected to show a lightweight, 
rich, read-only preview of the file's contents in the view's reading pane. 
This is done without launching the file's associated application. Windows 
Vista and later operating systems support preview handlers. 

To be a valid preview handler, several interfaces must be implemented. This 
includes IPreviewHandler (shobjidl.h); IInitializeWithFile, 
IInitializeWithStream, or IInitializeWithItem (propsys.h); IObjectWithSite 
(ocidl.h); and IOleWindow (oleidl.h). There are also optional interfaces, 
such as IPreviewHandlerVisuals (shobjidl.h), that a preview handler can 
implement to provide extended support. Windows API Code Pack for Microsoft 
.NET Framework makes the implementation of these interfaces very easy in .NET.

The example preview handler has the class ID (CLSID): 
    {9BF000CC-94E0-4CA6-960A-CE9338319A81}

It provides previews for .recipe files. The .recipe file type is simply an 
XML file registered as a unique file name extension. It includes the title of 
the recipe, its author, difficulty, preparation time, cook time, nutrition 
information, comments, an embedded preview image, and so on. The preview 
handler extracts the title, comments, and the embedded image, and display 
them in a preview window.


/////////////////////////////////////////////////////////////////////////////
Prerequisite:

1. The example preview handler must be registered on Windows Vista or newer 
operating systems.

2. The code sample references the Windows API Code Pack for Microsoft .NET 
Framework 1.1 which is available in the _External_Dependencies folder. The 
Windows API Code Pack for Microsoft .NET Framework can be downloaded from 
http://code.msdn.microsoft.com/WindowsAPICodePack


/////////////////////////////////////////////////////////////////////////////
Setup and Removal:

--------------------------------------
In the Development Environment

A. Setup

Run 'Visual Studio Command Prompt (2010)' (or 'Visual Studio x64 Win64 
Command Prompt (2010)' if you are on a x64 operating system) in the Microsoft 
Visual Studio 2010 \ Visual Studio Tools menu as administrator. Navigate to 
the folder that contains the build result VBShellExtPreviewHandler.dll and 
enter the command:

    Regasm.exe VBShellExtPreviewHandler.dll /codebase

The preview handler is registered successfully if the command prints:

    "Types registered successfully"

B. Removal

Run 'Visual Studio Command Prompt (2010)' (or 'Visual Studio x64 Win64 
Command Prompt (2010)' if you are on a x64 operating system) in the Microsoft 
Visual Studio 2010 \ Visual Studio Tools menu as administrator. Navigate to 
the folder that contains the build result VBShellExtPreviewHandler.dll and 
enter the command:

    Regasm.exe VBShellExtPreviewHandler.dll /unregister

The preview handler is unregistered successfully if the command prints:

    "Types un-registered successfully"

--------------------------------------
In the Deployment Environment

A. Setup

Install VBShellExtPreviewHandlerSetup(x86).msi, the output of the 
VBShellExtPreviewHandlerSetup(x86) setup project, on a x86 operating system. 
If the target operating system is x64, install 
VBShellExtPreviewHandlerSetup(x64).msi outputted by the 
VBShellExtPreviewHandlerSetup(x64) setup project.

B. Removal

Uninstall VBShellExtPreviewHandlerSetup(x86).msi, the output of the 
VBShellExtPreviewHandlerSetup(x86) setup project, on a x86 operating system. 
If the target operating system is x64, uninstall 
VBShellExtPreviewHandlerSetup(x64).msi outputted by the 
VBShellExtPreviewHandlerSetup(x64) setup project.


/////////////////////////////////////////////////////////////////////////////
Demo:

The following steps walk through a demonstration of the preview handler code 
sample.

Step1. After you successfully build the sample project in Visual Studio 2010, 
you will get a DLL: VBShellExtPreviewHandler.dll. Run 'Visual Studio Command 
Prompt (2010)' (or 'Visual Studio x64 Win64 Command Prompt (2010)' if you are 
on a x64 operating system) in the Microsoft Visual Studio 2010 \ Visual 
Studio Tools menu as administrator. Navigate to the folder that contains the 
build result VBShellExtPreviewHandler.dll and enter the command:

    Regasm.exe VBShellExtPreviewHandler.dll /codebase

The preview handler is registered successfully if the command prints:

    "Types registered successfully"

Step2. Find the chocolatechipcookies.recipe file in the sample folder. Turn 
on Windows Explorer Preview pane, and select the chocolatechipcookies.recipe 
file. You will see a picture of chocoate chip cookies, and the title and 
comments of the recipe in the preview pane. 

The .recipe file type is simply an XML file registered as a unique file name 
extension. It includes the title of the recipe, its author, difficulty, 
preparation time, cook time, nutrition information, comments, an embedded 
preview image, and so on. The preview handler extracts the title, comments, 
and the embedded image, and display them in a preview window.

Step3. In the same Visual Studio command prompt, run the command 

    Regasm.exe VBShellExtPreviewHandler.dll /unregister

to unregister the Shell preview handler.


/////////////////////////////////////////////////////////////////////////////
Implementation:

A. Creating and configuring the project

In Visual Studio 2010, create a Visual Basic / Windows / Class Library 
project named "VBShellExtPreviewHandler". Open the project properties, and in 
the Signing page, sign the assembly with a strong name key file. 

-----------------------------------------------------------------------------

B. Implementing a basic Component Object Model (COM) DLL

Shell extension handlers are COM objects implemented as DLLs. Making a basic 
.NET COM component is very straightforward. You just need to define a 
'public' class with ComVisible(true), use the Guid attribute to specify its 
CLSID, and explicitly implements certain COM interfaces. For example, 

    <ClassInterface(ClassInterfaceType.None)> _
    <Guid("9BF000CC-94E0-4CA6-960A-CE9338319A81"), ComVisible(True)> _
    Public Class SimpleObject
        Implements ISimpleObject
        ... ' Implements the interface
    End Class

You even do not need to implement IUnknown and class factory by yourself 
because .NET Framework handles them for you.

-----------------------------------------------------------------------------

C. Implementing the preview handler and registering it for a certain file 
class

-----------
Implementing the preview handler:

The RecipePreviewHandler.vb file defines the preview handler. The preview 
handler inherits from the class 

    Microsoft.WindowsAPICodePack.ShellExtensions.WinFormsPreviewHandler

and implements the interface

    Microsoft.WindowsAPICodePack.ShellExtensions.IPreviewFromStream

A PreviewHandlerAttribute is attached to the class. 

    <ClassInterface(ClassInterfaceType.None), _
    Guid("9BF000CC-94E0-4CA6-960A-CE9338319A81"), ComVisible(True), _
    PreviewHandler("RecipePreviewHandler", ".recipe", "{963C8DF6-CF61-4A5D-B51B-951B5911DDD0}")> _
    Public Class RecipePreviewHandler
        Inherits WinFormsPreviewHandler
        Implements IPreviewFromStream
    End Class

Microsoft.WindowsAPICodePack.ShellExtensions.WinFormsPreviewHandler is the 
base class for all WinForms-based preview handlers and provides their basic 
functionality. It allows you to create a custom preview handler that contains 
a WinForms user control.  If you want to create a custom preview handler that 
contains a WPF user control, use the WpfPreviewHandler base class instead. 
Both WinFormsPreviewHandler and WpfPreviewHandler derives from the abstract 
class Microsoft.WindowsAPICodePack.ShellExtensions.PreviewHandler which 
implements the basics of the ICustomQueryInterface, IPreviewHandler, 
IPreviewHandlerVisuals, IOleWindow, IObjectWithSite, IInitializeWithStream, 
IInitializeWithItem, IInitializeWithFile interfaces and the registration of 
the preview handler for you.

Microsoft.WindowsAPICodePack.ShellExtensions.IPreviewFromStream exposes the 
function for initializing the Preview Provider with a Stream. This interface 
can be used in conjunction with the other intialization interfaces, but only 
1 will be accessed according to the priorities preset by the Windows Shell:

    IPreviewFromStream (recommended)
    IPreviewFromShellObject
    IPreviewFromFile

The GuidAttribute is attached to the RecipePreviewHandler class to specify 
its CLSID. When you write your own handler, you must create a new CLSID by 
using the "Create GUID" tool in the Tools menu for the shell extension class, 
and specify the value in the Guid attribute. 

    ...
    <Guid("9BF000CC-94E0-4CA6-960A-CE9338319A81"), ComVisible(true)> _
    Public Class RecipePreviewHandler
        ...

The PreviewHandlerAttribute is attached to the Preview Provider to specify 
registration parameters.

    <PreviewHandler("RecipePreviewHandler", ".recipe", "{963C8DF6-CF61-4A5D-B51B-951B5911DDD0}")> _
    Public Class RecipePreviewHandler
        ...

Its Extensions property allows you to specify a semi-colon-separated list of 
extensions supported by the provider. 

In RecipePreviewHandler, you simply need to initialize the preview control in 
the class constructor, and populate the control in the Load method of the 
IPreviewFromStream interface. 

    Sub New()
        Me.Control = New RecipePreviewControl
    End Sub

    #Region "IPreviewFromStream Members"

    Public Sub Load(ByVal stream As Stream) Implements IPreviewFromStream.Load
        Dim recipe As New RecipeFileDefinition(stream)
        DirectCast(MyBase.Control, RecipePreviewControl).Populate(recipe)
    End Sub

    #End Region

The RecipePreviewControl class defines a custom user control used to display 
the preview. 

The Load method accepts the stream of the selected .recipe file, extracts the 
title, comments and embedded picture of the recipe, and populate the 
information on the preview control.

-----------
Registering the handler for a certain file class:

The registration and unregistration logic of the handler are implemented in 
the Microsoft.WindowsAPICodePack.ShellExtensions.PreviewHandler base class. 
Preview handlers can be associated with certain file classes. The MSDN article 
http://msdn.microsoft.com/en-us/library/cc144144.aspx illustrates the 
registration of preview handlers in detail. The handler is registered by 
setting the default value of the following registry key to be the CLSID the 
handler class. 

    HKEY_CLASSES_ROOT\<File Type>\shellex\{8895b1c6-b41f-4c1c-a562-0d564250836f}

For example, 

    HKCR
    {
        NoRemove .recipe
        {
            NoRemove shellex
            {
                {8895b1c6-b41f-4c1c-a562-0d564250836f} = 
                    s '{9BF000CC-94E0-4CA6-960A-CE9338319A81}'
            }
        }
    }

Next, add the subkey under CLSID for your preview handler. Create a new 
prevhost AppID so that the preview handler always runs in its own isolated 
process. An example is shown here. 

    HKCR
    {
        NoRemove CLSID
        {
            {9BF000CC-94E0-4CA6-960A-CE9338319A81} = 
                s 'VBShellExtPreviewHandler.RecipePreviewHandler'
            {
                val AppID = s '{963C8DF6-CF61-4A5D-B51B-951B5911DDD0}'
                val DisableLowILProcessIsolation = 0
                val DisplayName = s 'RecipePreviewHandler'
                InprocServer32 = s 'mscoree.dll'
                {
                    val Assembly = s 'VBShellExtPreviewHandler, Version=1.0.0.0, 
                        Culture=neutral, PublicKeyToken=b549cf1f7cd4a806'
                    val Class = s 'VBShellExtPreviewHandler.RecipePreviewHandler'
                    val CodeBase = s '<full path of VBShellExtPreviewHandler.dll>
                    val RuntimeVersion = s 'v4.0.30319'
                    val ThreadingModel = s 'Apartment'
                }
            }
        }
        NoRemove AppID
        {
            {963C8DF6-CF61-4A5D-B51B-951B5911DDD0}
            {
                val DllSurrogate = s '%SystemRoot%\system32\prevhost.exe'
            }
        }
    }

Finally, the preview handler must be added to the list of all preview 
handlers in the following registry key.

    HKLM or HKCU\Software\Microsoft\Windows\CurrentVersion\PreviewHandlers

This list is used as an optimization by the system to enumerate all 
registered preview handlers for display purposes. Again, the default value is 
not required, it simply aids in the debugging process.

    HKLM or HKCU
    {
        NoRemove SOFTWARE
        {
            NoRemove Microsoft
            {
                NoRemove Windows
                {
                    NoRemove CurrentVersion
                    {
                        NoRemove
                        {
                            PreviewHandlers
                            {
                                val {9bf000cc-94e0-4ca6-960a-ce9338319a81} = 
                                    s 'RecipePreviewHandler'
                            }
                        }
                    }
                }
            }
        }
    }

-----------------------------------------------------------------------------

D. Deploying the preview handler with a setup project.

  (1) In VBShellExtPreviewHandler, add an Installer class (named 
  ProjectInstaller in this code sample) to define the custom actions in the 
  setup. The class derives from System.Configuration.Install.Installer. We 
  use the custom actions to register/unregister the COM-visible classes in 
  the current managed assembly when user installs/uninstalls the component. 

    [RunInstaller(true), ComVisible(false)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);

            // Call RegistrationServices.RegisterAssembly to register the 
            // classes in the current managed assembly to enable creation 
            // from COM.
            RegistrationServices regService = new RegistrationServices();
            regService.RegisterAssembly(
                this.GetType().Assembly, 
                AssemblyRegistrationFlags.SetCodeBase);
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);

            // Call RegistrationServices.UnregisterAssembly to unregister 
            // the classes in the current managed assembly.
            RegistrationServices regService = new RegistrationServices();
            regService.UnregisterAssembly(this.GetType().Assembly);
        }
    }

  In the overriden Install method, we use RegistrationServices.RegisterAssembly 
  to register the classes in the current managed assembly to enable creation 
  from COM. The method also invokes the static method marked with 
  ComRegisterFunctionAttribute to perform the custom COM registration steps. 
  The call is equivalent to running the command: 
  
    "Regasm.exe VBShellExtPreviewHandler.dll /codebase"

  in the development environment. 

  (2) To add a deployment project, on the File menu, point to Add, and then 
  click New Project. In the Add New Project dialog box, expand the Other 
  Project Types node, expand the Setup and Deployment Projects, click Visual 
  Studio Installer, and then click Setup Project. In the Name box, type 
  VBShellExtPreviewHandlerSetup(x86). Click OK to create the project. 
  In the Properties dialog of the setup project, make sure that the 
  TargetPlatform property is set to x86. This setup project is to be deployed 
  on x86 Windows operating systems. 

  Right-click the setup project, and choose Add / Project Output ... 
  In the Add Project Output Group dialog box, VBShellExtPreviewHandler will  
  be displayed in the Project list. Select Primary Output and click OK.

  Right-click the setup project again, and choose View / Custom Actions. 
  In the Custom Actions Editor, right-click the root Custom Actions node. On 
  the Action menu, click Add Custom Action. In the Select Item in Project 
  dialog box, double-click the Application Folder. Select Primary output from 
  VBShellExtPreviewHandler. This adds the custom actions that we defined 
  in ProjectInstaller of VBShellExtPreviewHandler. 

  Right-click the setup project and select Properties. Click the 
  Prerequisites... button. In the Prerequisites dialog box, uncheck the 
  Microsoft .NET Framework 4 Client Profile (x86 and x64) option, and check 
  the Microsoft .NET Framework 4 (x86 and x64) option to match the .NET 
  Framework version of VBShellExtPreviewHandler. 

  Build the setup project. If the build succeeds, you will get a .msi file 
  and a Setup.exe file. You can distribute them to your users to install or 
  uninstall your Shell extension handler. 

  (3) To deploy the Shell extension handler to a x64 operating system, you 
  must create a new setup project (e.g. VBShellExtPreviewHandlerSetup(x64) 
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
  file in the VBShellExtPreviewHandlerSetup(x64) project folder. To 
  configure the script to run in the post-build event, you select the 
  VBShellExtPreviewHandlerSetup(x64) project in Solution Explorer, and 
  find the PostBuildEvent property in the Properties window. Specify its 
  value to be 
  
	"$(ProjectDir)Fix64bitInstallUtilLib.js" "$(BuiltOuputPath)" "$(ProjectDir)"

  Repeat the rest steps in (2) to add the project output, set the custom 
  actions, configure the prerequisites, and build the setup project.


/////////////////////////////////////////////////////////////////////////////
Diagnostic:

If you have followed the recommendations to implement your preview handler as 
an in-process server, to debug your preview handler, you can attach to 
Prevhost.exe. As mentioned earlier, be aware that there could be two 
instances of Prevhost.exe, one for normal low IL processes and one for those 
handlers that have opted out of running as a low IL process.

If you do not find Prevhost.exe in your list of available processes, it 
probably has not been loaded at that point. Clicking on a file for a preview 
loads the surrogate and it should then appear as an attachable process.


/////////////////////////////////////////////////////////////////////////////
References:

MSDN: Preview Handlers and Shell Preview Host
http://msdn.microsoft.com/en-us/library/cc144143.aspx

MSDN: Building Preview Handlers
http://msdn.microsoft.com/en-us/library/cc144139.aspx

MSDN: Registering Preview Handlers
http://msdn.microsoft.com/en-us/library/cc144144.aspx

Windows API Code Pack for Microsoft .NET Framework
http://code.msdn.microsoft.com/WindowsAPICodePack

View Data Your Way With Our Managed Preview Handler Framework
http://msdn.microsoft.com/en-us/magazine/cc163487.aspx


/////////////////////////////////////////////////////////////////////////////