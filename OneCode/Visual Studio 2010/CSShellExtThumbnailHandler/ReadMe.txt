=============================================================================
         CLASS LIBRARY : CSShellExtThumbnailHandler Project Overview
=============================================================================

/////////////////////////////////////////////////////////////////////////////
Summary:

The .NET 4 code sample demonstrates the C# implementation of a thumbnail 
handler for a new file type registered with the .recipe extension. 

A thumbnail image handler provides an image to represent the item. It lets 
you customize the thumbnail of files with a specific file extension. Windows 
Vista and newer operating systems make greater use of file-specific thumbnail 
images than earlier versions of Windows. Thumbnails of 32-bit resolution and 
as large as 256x256 pixels are often used. File format owners should be 
prepared to display their thumbnails at that size. 

The example thumbnail handler has the class ID (CLSID): 
    {2A736503-DDE4-4876-801D-60063E9E2215}

The handler implements the IThumbnailProvider and IInitializeWithStream 
interfaces, and provides thumbnails for .recipe files. Windows API Code Pack 
for Microsoft .NET Framework makes the implementation of IThumbnailProvider 
and IInitializeWithStream very easy. The .recipe file type is simply an XML 
file registered as a unique file name extension. It includes an element 
called "Picture", embedding an image file. The thumbnail handler extracts the 
embedded image and asks the Shell to display it as a thumbnail.


/////////////////////////////////////////////////////////////////////////////
Prerequisite:

1. The example thumbnail handler must be registered on Windows Vista or newer 
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
the folder that contains the build result CSShellExtThumbnailHandler.dll 
and enter the command:

    Regasm.exe CSShellExtThumbnailHandler.dll /codebase

The thumbnail handler is registered successfully if the command prints:

    "Types registered successfully"

B. Removal

Run 'Visual Studio Command Prompt (2010)' (or 'Visual Studio x64 Win64 
Command Prompt (2010)' if you are on a x64 operating system) in the Microsoft 
Visual Studio 2010 \ Visual Studio Tools menu as administrator. Navigate to 
the folder that contains the build result CSShellExtThumbnailHandler.dll 
and enter the command:

    Regasm.exe CSShellExtThumbnailHandler.dll /unregister

The thumbnail handler is unregistered successfully if the command prints:

    "Types un-registered successfully"

--------------------------------------
In the Deployment Environment

A. Setup

Install CSShellExtThumbnailHandlerSetup(x86).msi, the output of the 
CSShellExtThumbnailHandlerSetup(x86) setup project, on a x86 operating 
system. If the target operating system is x64, install 
CSShellExtThumbnailHandlerSetup(x64).msi outputted by the 
CSShellExtThumbnailHandlerSetup(x64) setup project.

B. Removal

Uninstall CSShellExtThumbnailHandlerSetup(x86).msi, the output of the 
CSShellExtThumbnailHandlerSetup(x86) setup project, on a x86 operating 
system. If the target operating system is x64, uninstall 
CSShellExtThumbnailHandlerSetup(x64).msi outputted by the 
CSShellExtThumbnailHandlerSetup(x64) setup project.


/////////////////////////////////////////////////////////////////////////////
Demo:

The following steps walk through a demonstration of the thumbnail handler 
code sample.

Step1. After you successfully build the sample project in Visual Studio 2010, 
you will get a DLL: CSShellExtThumbnailHandler.dll. Run 'Visual Studio 
Command Prompt (2010)' (or 'Visual Studio x64 Win64 Command Prompt (2010)' if 
you are on a x64 operating system) in the Microsoft Visual Studio 2010 \ 
Visual Studio Tools menu as administrator. Navigate to the folder that 
contains the build result CSShellExtThumbnailHandler.dll and enter the 
command:

    Regasm.exe CSShellExtThumbnailHandler.dll /codebase

The thumbnail handler is registered successfully if the command prints:

    "Types registered successfully"

Step2. Find the chocolatechipcookies.recipe file in the sample folder. You 
will see a picture of chocoate chip cookies as its thumbnail. 

The .recipe file type is simply an XML file registered as a unique file name 
extension. It includes an element called "Picture", embedding an image file. 
The thumbnail handler extracts the embedded image and asks the Shell to 
display it as a thumbnail. 

Step3. In the same Visual Studio command prompt, run the command 

    Regasm.exe CSShellExtThumbnailHandler.dll /unregister

to unregister the Shell thumbnail handler.


/////////////////////////////////////////////////////////////////////////////
Implementation:

A. Creating and configuring the project

In Visual Studio 2010, create a Visual C# / Windows / Class Library project 
named "CSShellExtThumbnailHandler". Open the project properties, and in the 
Signing page, sign the assembly with a strong name key file. 

-----------------------------------------------------------------------------

B. Implementing a basic Component Object Model (COM) DLL

Shell extension handlers are COM objects implemented as DLLs. Making a basic 
.NET COM component is very straightforward. You just need to define a 
'public' class with ComVisible(true), use the Guid attribute to specify its 
CLSID, and explicitly implements certain COM interfaces. For example, 

    [ClassInterface(ClassInterfaceType.None)]
    [Guid("2A736503-DDE4-4876-801D-60063E9E2215"), ComVisible(true)]
    public class SimpleObject : ISimpleObject
    {
        ... // Implements the interface
    }

You even do not need to implement IUnknown and class factory by yourself 
because .NET Framework handles them for you.

-----------------------------------------------------------------------------

C. Implementing the thumbnail handler and registering it for a certain file 
class

The RecipeThumbnailProvider.cs file defines the thumbnail handler. The 
thumbnail handler inherits from the class 

    Microsoft.WindowsAPICodePack.ShellExtensions.ThumbnailProvider

and implements the interface

    Microsoft.WindowsAPICodePack.ShellExtensions.IThumbnailFromStream

A ThumbnailProviderAttribute is attached to the class. 

    [ClassInterface(ClassInterfaceType.None)]
    [Guid("2A736503-DDE4-4876-801D-60063E9E2215"), ComVisible(true)]
    [ThumbnailProvider("RecipeThumbnailProvider", ".recipe")]
    public class RecipeThumbnailProvider : ThumbnailProvider, IThumbnailFromStream
    {
    }

Microsoft.WindowsAPICodePack.ShellExtensions.ThumbnailProvider is a class 
defined in Windows API Code Pack. It implements the basics of the 
IInitializeWithStream and IThumbnailProvider interfaces, and the registration 
of the thumbnail handler for you.

Microsoft.WindowsAPICodePack.ShellExtensions.IThumbnailFromStream exposes the 
function for initializing the Thumbnail Provider with a Stream. If this 
interfaces is not used, then the handler must opt out of process isolation.
This interface can be used in conjunction with the other intialization 
interfaces, but only 1 will be accessed according to the priorities preset by 
the Windows Shell:

    IThumbnailFromStream (recommended)
    IThumbnailFromShellObject
    IThumbnailFromFile

The GuidAttribute is attached to the RecipeThumbnailProvider class to specify 
its CLSID. When you write your own handler, you must create a new CLSID by 
using the "Create GUID" tool in the Tools menu for the shell extension class, 
and specify the value in the Guid attribute. 

    ...
    [Guid("2A736503-DDE4-4876-801D-60063E9E2215"), ComVisible(true)]
    public class RecipeThumbnailProvider : ...

The ThumbnailProviderAttribute is applied to a Thumbnail Provider to specify 
registration parameters and aesthetic attributes.

    [ThumbnailProvider("RecipeThumbnailProvider", ".recipe")]
    public class RecipeThumbnailProvider : ...

Its Extensions property allows you to specify a semi-colon-separated list of 
extensions supported by the provider. 

In RecipeThumbnailProvider, you simply need to implement the 
IThumbnailFromStream interface by providing your ConstructBitmap method. The 
method accepts the stream of the selected .recipe file, extracts the embedded 
picture, and returns the picture to be displayed as the thumbnail.

    public Bitmap ConstructBitmap(Stream stream, int sideSize)
    {
        RecipeFileDefinition recipe = new RecipeFileDefinition(stream);

        byte[] buffer = Convert.FromBase64String(recipe.EncodedPicture);
        using (MemoryStream mstream = new MemoryStream(buffer))
        {
            return new Bitmap(mstream);
        }
    }

The registration and unregistration logic of the handler are implemented in 
the Microsoft.WindowsAPICodePack.ShellExtensions.ThumbnailProvider base 
class. Thumbnail handlers can be associated with certain file classes. The 
handlers are registered by setting the default value of the following 
registry key to be the CLSID the handler class. 

    HKEY_CLASSES_ROOT\<File Type>\shellex\{e357fccd-a995-4576-b01f-234630154e96}

For example, this code sample associates the handler with '.recipe' files. 
The following keys and values are added in the registration process of the 
sample handler. 

    HKCR
    {
        NoRemove .recipe
        {
            NoRemove shellex
            {
                {e357fccd-a995-4576-b01f-234630154e96} = 
                    s '{2A736503-DDE4-4876-801D-60063E9E2215}'
            }
        }
    }

-----------------------------------------------------------------------------

D. Deploying the thumbnail handler with a setup project.

  (1) In CSShellExtThumbnailHandler, add an Installer class (named 
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

            // Call RegistrationServices.RegisterAssembly to register the classes in 
            // the current managed assembly to enable creation from COM.
            RegistrationServices regService = new RegistrationServices();
            regService.RegisterAssembly(
                this.GetType().Assembly, 
                AssemblyRegistrationFlags.SetCodeBase);
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
  
    "Regasm.exe CSShellExtThumbnailHandler.dll /codebase"

  in the development environment. 

  (2) To add a deployment project, on the File menu, point to Add, and then 
  click New Project. In the Add New Project dialog box, expand the Other 
  Project Types node, expand the Setup and Deployment Projects, click Visual 
  Studio Installer, and then click Setup Project. In the Name box, type 
  CSShellExtThumbnailHandlerSetup(x86). Click OK to create the project. 
  In the Properties dialog of the setup project, make sure that the 
  TargetPlatform property is set to x86. This setup project is to be deployed 
  on x86 Windows operating systems. 

  Right-click the setup project, and choose Add / Project Output ... 
  In the Add Project Output Group dialog box, CSShellExtThumbnailHandler will  
  be displayed in the Project list. Select Primary Output and click OK.

  Right-click the setup project again, and choose View / Custom Actions. 
  In the Custom Actions Editor, right-click the root Custom Actions node. On 
  the Action menu, click Add Custom Action. In the Select Item in Project 
  dialog box, double-click the Application Folder. Select Primary output from 
  CSShellExtThumbnailHandler. This adds the custom actions that we defined 
  in ProjectInstaller of CSShellExtThumbnailHandler. 

  Right-click the setup project and select Properties. Click the 
  Prerequisites... button. In the Prerequisites dialog box, uncheck the 
  Microsoft .NET Framework 4 Client Profile (x86 and x64) option, and check 
  the Microsoft .NET Framework 4 (x86 and x64) option to match the .NET 
  Framework version of CSShellExtThumbnailHandler. 

  Build the setup project. If the build succeeds, you will get a .msi file 
  and a Setup.exe file. You can distribute them to your users to install or 
  uninstall your Shell extension handler. 

  (3) To deploy the Shell extension handler to a x64 operating system, you 
  must create a new setup project (e.g. CSShellExtThumbnailHandlerSetup(x64) 
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
  file in the CSShellExtThumbnailHandlerSetup(x64) project folder. To 
  configure the script to run in the post-build event, you select the 
  CSShellExtThumbnailHandlerSetup(x64) project in Solution Explorer, and 
  find the PostBuildEvent property in the Properties window. Specify its 
  value to be 
  
	"$(ProjectDir)Fix64bitInstallUtilLib.js" "$(BuiltOuputPath)" "$(ProjectDir)"

  Repeat the rest steps in (2) to add the project output, set the custom 
  actions, configure the prerequisites, and build the setup project.


/////////////////////////////////////////////////////////////////////////////
Diagnostic:

Debugging thumbnail handlers is difficult for several reasons.

1) The Windows Explorer hosts thumbnail providers in an isolated process to 
get robustness and improve security. Because of this it is difficult to debug 
your handler as you cannot set breakpoints on your code in the explorer.exe 
process as it is not loaded there. The isolated process is DllHost.exe and 
this is used for other purposes so finding the right instance of this process 
is difficult. 

2) Once a thumbnail is computed for a particular file it is cached and your 
handler won't be called again for that item unless you invalidate the cache 
by updating the modification date of the file. Note that this cache works 
even if the files are renamed or moved.

Given all of these issues the easiest way to debug your code in a test 
application then once you have proven it works there test it in the context 
of the explorer. 

Another thing to do is to disable the process isolation feature of explorer. 
You can do this by putting the following named value on the CLSID of your 
handler

    HKCR\CLSID\{CLSID of Your Handler}
        DisableProcessIsolation=REG_DWORD:1

Be sure to not ship your handler with this on as customers require the 
security and robustness benefits of the isolated process feature.


/////////////////////////////////////////////////////////////////////////////
References:

MSDN: Thumbnail Handlers
http://msdn.microsoft.com/en-us/library/cc144118.aspx

MSDN: Building Thumbnail Handlers
http://msdn.microsoft.com/en-us/library/cc144114.aspx

MSDN: Thumbnail Handler Guidelines
http://msdn.microsoft.com/en-us/library/cc144115.aspx

MSDN: IThumbnailProvider Interface
http://msdn.microsoft.com/en-us/library/bb774614.aspx

Windows API Code Pack for Microsoft .NET Framework
http://code.msdn.microsoft.com/WindowsAPICodePack


/////////////////////////////////////////////////////////////////////////////