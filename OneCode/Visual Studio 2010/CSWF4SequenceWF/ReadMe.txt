=============================================================================
    CONSOLE APPLICATION : CSWF4SequenceWF
=============================================================================
/////////////////////////////////////////////////////////////////////////////
Use:

This sample demonstrate the usage of WF4 Sequence workflow in a Guess Number
Guame workflow. this sample will also involve the usage of Variable, IFElse 
Activity, DoWhile Activity and Cutomized Activity. 

To run the sample:
1. Open CSWF4SequenceWF.sln with Visual Studio 2010
2. Press Ctrl+F5.


/////////////////////////////////////////////////////////////////////////////
Prerequisite

1. Visual Studio 2010
2. .NET Framework 4.0


/////////////////////////////////////////////////////////////////////////////
Creation:

1.Create A Workflow Console Application, name it CSWF4SequenceWF;
2.Create a CodeActivity name it ReadNumberActivity.cs. fill the file with code:
  using System;
  using System.Activities;

  namespace CSWF4SequenceWF
  {
      public sealed class ReadNumberActivity : CodeActivity
      {
          // Define an activity out argument of type int
          public OutArgument<int> playerInputNumber { get; set; }

          protected override void Execute(CodeActivityContext context)
          {
              playerInputNumber.Set(context,Int32.Parse(Console.ReadLine()));
          }
      }
  }
3.Delete the default created Workflow1.xaml and create a new Activity 
  GuessNumberGameSequenceWF.xaml.then,author this workflow
  just like the one already existed in the project. 
4.Open Prgram.cs file, chage the code as follow:
  using System.Activities;
  namespace CSWF4SequenceWF
  {
      class Program
      {
          static void Main(string[] args)
          {
              WorkflowInvoker.Invoke(new GuessNumberGameSequenceWF() );
          }
      }
  }  
5.Press Ctrl+F5 to run the workflow without debugging.

