=============================================================================
         CLASS LIBRARY : VBShellExtInfotipHandler Project Overview
=============================================================================

/////////////////////////////////////////////////////////////////////////////
Summary:

The VB.NET code sample demonstrates creating a Shell infotip handler with 
.NET Framework 4.

An infotip handler is a shell extension handler that provides pop-up text 
when the user hovers the mouse pointer over the object. It is the most 
flexible way to customize infotips. The alternative way is to specify either 
a fixed string or a list of certain file properties to be displayed (See the 
Infotip Customization section in 
http://msdn.microsoft.com/en-us/library/cc144067.aspx)

Prior to .NET Framework 4, the development of in-process shell extensions 
using managed code is not officially supported because of the CLR limitation 
allowing only one .NET runtime per process. Jesse Kaplan, one of the CLR 
program managers, explains it in this MSDN forum thread: 
http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/1428326d-7950-42b4-ad94-8e962124043e.

In .NET 4, with the ability to have multiple runtimes in process with any 
other runtime, Microsoft can now offer general support for writing managed 
shell extensions—even those that run in-process with arbitrary applications 
on the machine. VBShellExtInfotipHandler is such a managed shell extension 
code example. However, please note that you still cannot write shell 
extensions using any version earlier than .NET Framework 4 because those 
versions of the runtime do not load in-process with one another and will 
cause failures in many cases.

The example infotip handler has the class ID (CLSID): 
    {44BDEF95-C00F-493E-A55B-17937DB1F04E}

It customizes the infotips of .vb file objects. When you hover your mouse 
pointer over a .vb file object in the Windows Explorer, you will see an 
infotip with the text:

    File: <File path, e.g. D:\Test.vb>
    Lines: <Line number, e.g. 123 or N/A>
    - Infotip displayed by VBShellExtInfotipHandler


/////////////////////////////////////////////////////////////////////////////
Setup and Removal:

--------------------------------------
In the Development Environment

A. Setup

Run 'Visual Studio Command Prompt (2010)' (or 'Visual Studio x64 Win64 
Command Prompt (2010)' if you are on a x64 operating system) in the Microsoft 
Visual Studio 2010 \ Visual Studio Tools menu as administrator. Navigate to 
the folder that contains the build result VBShellExtInfotipHandler.dll 
and enter the command:

    Regasm.exe VBShellExtInfotipHandler.dll /codebase

The infotip handler is registered successfully if the command prints:

    "Types registered successfully"

B. Removal

Run 'Visual Studio Command Prompt (2010)' (or 'Visual Studio x64 Win64 
Command Prompt (2010)' if you are on a x64 operating system) in the Microsoft 
Visual Studio 2010 \ Visual Studio Tools menu as administrator. Navigate to 
the folder that contains the build result VBShellExtInfotipHandler.dll 
and enter the command:

    Regasm.exe VBShellExtInfotipHandler.dll /unregister

The infotip handler is unregistered successfully if the command prints:

    "Types un-registered successfully"

--------------------------------------
In the Deployment Environment

A. Setup

Install VBShellExtInfotipHandlerSetup(x86).msi, the output of the 
VBShellExtInfotipHandlerSetup(x86) setup project, on a x86 operating 
system. If the target operating system is x64, install 
VBShellExtInfotipHandlerSetup(x64).msi outputted by the 
VBShellExtInfotipHandlerSetup(x64) setup project.

B. Removal

Uninstall VBShellExtInfotipHandlerSetup(x86).msi, the output of the 
VBShellExtInfotipHandlerSetup(x86) setup project, on a x86 operating 
system. If the target operating system is x64, uninstall 
VBShellExtInfotipHandlerSetup(x64).msi outputted by the 
VBShellExtInfotipHandlerSetup(x64) setup project.


/////////////////////////////////////////////////////////////////////////////
Demo:

The following steps walk through a demonstration of the infotip handler code 
sample.

Step1. After you successfully build the sample project in Visual Studio 2010, 
you will get a DLL: VBShellExtInfotipHandler.dll. Run 'Visual Studio Command 
Prompt (2010)' (or 'Visual Studio x64 Win64 Command Prompt (2010)' if you are 
on a x64 operating system) in the Microsoft Visual Studio 2010 \ Visual 
Studio Tools menu as administrator. Navigate to the folder that contains the 
build result VBShellExtInfotipHandler.dll and enter the command:

    Regasm.exe VBShellExtInfotipHandler.dll /codebase

The infotip handler is registered successfully if the command prints:

    "Types registered successfully"

Step2. Find a .vb file in the Windows Explorer (e.g. FileInfotipExt.vb in 
the sample folder), and hover the mouse pointer over it. you will see an 
infotip with the text:

    File: <File path, e.g. D:\VBShellExtInfotipHandler\FileInfotipExt.vb>
    Lines: <Line number, e.g. 123 or N/A>
    - Infotip displayed by VBShellExtInfotipHandler

Step3. In the same Visual Studio command prompt, run the command 

    Regasm.exe VBShellExtInfotipHandler.dll /unregister

to unregister the Shell infotip handler.


/////////////////////////////////////////////////////////////////////////////
Implementation:

A. Creating and configuring the project

In Visual Studio 2010, create a Visual Basic / Windows / Class Library 
project named "VBShellExtInfotipHandler". Open the project properties, and in 
the Signing page, sign the assembly with a strong name key file. 

-----------------------------------------------------------------------------

B. Implementing a basic Component Object Model (COM) DLL

Shell extension handlers are all in-process COM objects implemented as DLLs. 
Making a basic .NET COM component is very straightforward. You just need to 
define a 'Public' class with ComVisible(True), use the Guid attribute to 
specify its CLSID, and explicitly implements certain COM interfaces. For 
example, 

    <ClassInterface(ClassInterfaceType.None), _
    Guid("44BDEF95-C00F-493E-A55B-17937DB1F04E"), ComVisible(True)> _
    Public Class SimpleObject
        Implements ISimpleObject
        ... ' Implements the interface
    End Class

You even do not need to implement IUnknown and class factory by yourself 
because .NET Framework handles them for you.

-----------------------------------------------------------------------------

C. Implementing the infotip handler and registering it for a certain file 
class

-----------
Implementing the infotip handler:

The FileInfotipExt.vb file defines an infotip handler. The infotip handler 
must implement the IQueryInfo interface to create text for the tooltip, and 
implement the IPersistFile interface for initialization.

    <ComImport(), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), _
    Guid("00021500-0000-0000-c000-000000000046")> _
    Friend Interface IQueryInfo
        ' The original signature of GetInfoTip is 
        ' HRESULT GetInfoTip(DWORD dwFlags, [out] PWSTR *ppwszTip);
        ' According to the documentation, applications that implement this method 
        ' must allocate memory for ppwszTip by calling CoTaskMemAlloc. Calling 
        ' applications (the Shell in this case) calls CoTaskMemFree to free the 
        ' memory when it is no longer needed. Here, we set PreserveSig to false 
        ' (the default value in COM) to make the output parameter 'ppwszTip' the 
        ' return value. We also marshal the string return value as LPWStr. The 
        ' interop layer in CLR will call CoTaskMemAlloc to allocate memory and 
        ' marshal the .NET string to the memory. 
        Function GetInfoTip(ByVal dwFlags As UInt32) _
            As <MarshalAs(UnmanagedType.LPWStr)> String

        Function GetInfoFlags() As Integer
    End Interface

The IPersistFile interface is available in the 
System.Runtime.InteropServices.ComTypes namespace. 
http://msdn.microsoft.com/en-us/library/system.runtime.interopservices.comtypes.ipersistfile.aspx
	
    <ClassInterface(ClassInterfaceType.None), _
    Guid("44BDEF95-C00F-493E-A55B-17937DB1F04E"), ComVisible(True)> _
    Public Class FileInfotipExt
        Implements IPersistFile, IQueryInfo

        #Region "IPersistFile Members"

        Public Sub GetClassID(<Out()> ByRef pClassID As Guid) _
            Implements IPersistFile.GetClassID
            ...
        End Sub

        Public Sub GetCurFile(<Out()> ByRef ppszFileName As String) _
            Implements IPersistFile.GetCurFile
            ...
        End Sub

        Public Function IsDirty() As Integer _
            Implements IPersistFile.IsDirty
            ...
        End Function

        Public Sub Load(ByVal pszFileName As String, ByVal dwMode As Integer) _
            Implements IPersistFile.Load
            ...
        End Sub

        Public Sub Save(ByVal pszFileName As String, ByVal fRemember As Boolean) _
            Implements IPersistFile.Save
            ...
        End Sub

        Public Sub SaveCompleted(ByVal pszFileName As String) _
            Implements IPersistFile.SaveCompleted
            ...
        End Sub

    #End Region

    #Region "IQueryInfo Members"

        Public Function GetInfoTip(ByVal dwFlags As UInt32) As String _
            Implements IQueryInfo.GetInfoTip
            ...
        End Function

        Public Function GetInfoFlags() As Integer _
            Implements IQueryInfo.GetInfoFlags
            ...
        End Function

    #End Region

    End Class

When you write your own handler, you must create a new CLSID by using the 
"Create GUID" tool in the Tools menu for the shell extension class, and 
specify the value in the Guid attribute. 

    ...
    Guid("44BDEF95-C00F-493E-A55B-17937DB1F04E"), ComVisible(True)> _
    Public Class FileInfotipExt


  1. Implementing IPersistFile

  The Shell queries the extension for IPersistFile and calls its Load method 
  passing the file name of the item over which mouse is placed. 
  IPersistFile.Load opens the specified file and initializes the wanted data. 
  In this code sample, we save the absolute path of the file.

    Public Sub Load(ByVal pszFileName As String, ByVal dwMode As Integer) _
        Implements IPersistFile.Load
        ' pszFileName contains the absolute path of the file to be opened.
        Me.selectedFile = pszFileName
    End Sub

  The rest methods of IPersistFile are not used by the Shell for infotip 
  handlers, so we can simply make them blank or throw a NotImplementedException 
  exception.

  2. Implementing IQueryInfo

  After IPersistFile is queried, the Shell queries the IQueryInfo interface 
  and the GetInfoTip method is called. GetInfoTip returns a string containing 
  the tip to display.

  In this code sample, the example infotip is composed of the file path and 
  the count of text lines.

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

The IQueryInfo.GetInfoFlags method is not currently used. We simply throw  
a NotImplementedException exception in the method.

-----------
Registering the handler for a certain file class:

Infotip handlers can be associated with a file class. The handlers are 
registered by setting the default value of the following registry key to be 
the CLSID the handler class. 

    HKEY_CLASSES_ROOT\<File Type>\shellex\{00021500-0000-0000-C000-000000000046}

The registration of the infotip handler is implemented in the Register 
method of FileInfotipExt. The ComRegisterFunction attribute attached to 
the method enables the execution of user-written code other than the basic 
registration of the COM class. Register calls the 
ShellExtReg.RegisterShellExtInfotipHandler method in ShellExtLib.vb to 
associate the handler with a certain file type. If the file type starts with 
'.', it tries to read the default value of the HKCR\<File Type> key which may 
contain the Program ID to which the file type is linked. If the default value 
is not empty, use the Program ID as the file type to proceed the registration. 

For example, this code sample associates the handler with '.cs' files. 
HKCR\.cs has the default value 'VisualStudio.cs.10.0' by default when 
Visual Studio 2010 is installed, so we proceed to register the handler under 
HKCR\VisualStudio.cs.10.0\ instead of under HKCR\.cs. The following keys 
and values are added in the registration process of the sample handler. 

    HKCR
    {
        NoRemove .cs = s 'VisualStudio.cs.10.0'
        NoRemove VisualStudio.cs.10.0
        {
            NoRemove shellex
            {
                {00021500-0000-0000-C000-000000000046} = 
                    s '{44BDEF95-C00F-493E-A55B-17937DB1F04E}'
            }
        }
    }

The unregistration is implemented in the Unregister method of 
FileInfotipExt. Similar to the Register method, the ComUnregisterFunction 
attribute attached to the method enables the execution of user-written code 
during the unregistration process. It removes the registry key:
HKCR\<File Type>\shellex\{00021500-0000-0000-C000-000000000046}.

-----------------------------------------------------------------------------

D. Deploying the infotip handler with a setup project.

  (1) In VBShellExtInfotipHandler, add an Installer class (named 
  ProjectInstaller in this code sample) to define the custom actions in the 
  setup. The class derives from System.Configuration.Install.Installer. We 
  use the custom actions to register/unregister the COM-visible classes in 
  the current managed assembly when user installs/uninstalls the component. 

    <RunInstaller(True), ComVisible(False)> _
    Public Class ProjectInstaller
        Inherits Installer

        Public Overrides Sub Install(ByVal stateSaver As IDictionary)
            MyBase.Install(stateSaver)

            ' Call RegistrationServices.RegisterAssembly to register the classes 
            ' in the current managed assembly to enable creation from COM.
            Dim regService As New RegistrationServices
            regService.RegisterAssembly( _
                Me.GetType.Assembly, _
                AssemblyRegistrationFlags.SetCodeBase)
        End Sub

        Public Overrides Sub Uninstall(ByVal savedState As IDictionary)
            MyBase.Uninstall(savedState)

            ' Call RegistrationServices.UnregisterAssembly to unregister the 
            ' classes in the current managed assembly.
            Dim regService As New RegistrationServices
            regService.UnregisterAssembly(Me.GetType.Assembly)
        End Sub

    End Class

  In the overriden Install method, we use RegistrationServices.RegisterAssembly 
  to register the classes in the current managed assembly to enable creation 
  from COM. The method also invokes the static method marked with 
  ComRegisterFunctionAttribute to perform the custom COM registration steps. 
  The call is equivalent to running the command: 
  
    "Regasm.exe VBShellExtInfotipHandler.dll /codebase"

  in the development environment. 

  (2) To add a deployment project, on the File menu, point to Add, and then 
  click New Project. In the Add New Project dialog box, expand the Other 
  Project Types node, expand the Setup and Deployment Projects, click Visual 
  Studio Installer, and then click Setup Project. In the Name box, type 
  VBShellExtInfotipHandlerSetup(x86). Click OK to create the project. 
  In the Properties dialog of the setup project, make sure that the 
  TargetPlatform property is set to x86. This setup project is to be deployed 
  on x86 Windows operating systems. 

  If you want to deploy the Shell extension handler to a x64 operating system, 
  you must create a new setup project (e.g. VBShellExtInfotipHandlerSetup(x64) 
  in this code sample), and set its TargetPlatform property to x64. 

  Right-click the setup project, and choose Add / Project Output ... 
  In the Add Project Output Group dialog box, VBShellExtInfotipHandler will  
  be displayed in the Project list. Select Primary Output and click OK.

  Right-click the setup project again, and choose View / Custom Actions. 
  In the Custom Actions Editor, right-click the root Custom Actions node. On 
  the Action menu, click Add Custom Action. In the Select Item in Project 
  dialog box, double-click the Application Folder. Select Primary output from 
  VBShellExtInfotipHandler. This adds the custom actions that we defined 
  in ProjectInstaller of VBShellExtInfotipHandler. 

  Right-click the setup project and select Properties. Click the 
  Prerequisites... button. In the Prerequisites dialog box, uncheck the 
  Microsoft .NET Framework 4 Client Profile (x86 and x64) option, and check 
  the Microsoft .NET Framework 4 (x86 and x64) option to match the .NET 
  Framework version of VBShellExtInfotipHandler. 

  Build the setup project. If the build succeeds, you will get a .msi file 
  and a Setup.exe file. You can distribute them to your users to install or 
  uninstall your Shell extension handler. 

  (3) To deploy the Shell extension handler to a x64 operating system, you 
  must create a new setup project (e.g. VBShellExtInfotipHandlerSetup(x64) 
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
  file in the VBShellExtInfotipHandlerSetup(x64) project folder. To 
  configure the script to run in the post-build event, you select the 
  VBShellExtInfotipHandlerSetup(x64) project in Solution Explorer, and 
  find the PostBuildEvent property in the Properties window. Specify its 
  value to be 
  
	"$(ProjectDir)Fix64bitInstallUtilLib.js" "$(BuiltOuputPath)" "$(ProjectDir)"

  Repeat the rest steps in (2) to add the project output, set the custom 
  actions, configure the prerequisites, and build the setup project.


/////////////////////////////////////////////////////////////////////////////
References:

MSDN: Initializing Shell Extensions
http://msdn.microsoft.com/en-us/library/cc144105.aspx

MSDN: IQueryInfo Interface
http://msdn.microsoft.com/en-us/library/bb761359.aspx

The Complete Idiot's Guide to Writing Shell Extensions - Part III
http://www.codeproject.com/KB/shell/ShellExtGuide3.aspx


/////////////////////////////////////////////////////////////////////////////