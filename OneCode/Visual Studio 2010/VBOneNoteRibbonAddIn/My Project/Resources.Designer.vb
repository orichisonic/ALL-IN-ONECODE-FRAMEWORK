﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.1
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("VBOneNoteRibbonAddIn.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;
        '''&lt;customUI xmlns=&quot;http://schemas.microsoft.com/office/2006/01/customui&quot; loadImage=&quot;OnGetImage&quot;&gt;
        '''  &lt;ribbon&gt;
        '''	 &lt;tabs&gt;
        '''	  &lt;tab idMso=&quot;TabReviewWord&quot;&gt;
        '''		 &lt;group id=&quot;grpWordCounter&quot; label=&quot;Statistics&quot; insertAfterMso=&quot;GroupSpelling&quot;&gt;
        '''       &lt;button id=&quot;btnShowForm&quot; label=&quot;Show Form&quot; size=&quot;large&quot; onAction=&quot;ShowForm&quot; image=&quot;showform.png&quot; screentip=&quot;Show windows form.&quot; /&gt;
        '''		 &lt;/group &gt;
        '''	  &lt;/tab&gt;
        '''	 &lt;/tabs&gt;
        '''  &lt;/ribbon&gt;
        '''&lt;/customUI&gt;.
        '''</summary>
        Friend ReadOnly Property customUI() As String
            Get
                Return ResourceManager.GetString("customUI", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property showform() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("showform", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
    End Module
End Namespace
