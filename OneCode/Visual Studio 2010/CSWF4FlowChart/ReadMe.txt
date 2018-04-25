=============================================================================
    CONSOLE APPLICATION : CSWF4FlowChart
=============================================================================
/////////////////////////////////////////////////////////////////////////////
Use:

This sample demonstrate the usage of WF4 Sequence workflow in a Guess Number
Guame workflow. this sample will also involve the usage of Variable, IFElse 
Activity, DoWhile Activity and Cutomized Activity. 

To run the sample:
1. Open CSWF4FlowChart.sln with Visual Studio 2010.
2. Press Ctrl+F5.


/////////////////////////////////////////////////////////////////////////////
Prerequisite:

1. Visual Studio 2010
2. .NET Framework 4.0


/////////////////////////////////////////////////////////////////////////////
Creation:

1. Create a new Workflow Console Workflow named it CSWF4FlowChart;
2. Create a new code file name it ReadNumberActivity.cs, file the file with 
   the following code:
   using System;
   using System.Activities;

   namespace CS_WF4_SequenceWF
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
3. Delete the default created Workflow1.xaml file thenc reate a new Activity name 
   it GuessNumberGameInflowChart.xaml;author the workflow just like the one exsited 
   in the project. 
4. Open Program.cs file, change the code as follow:
   using System.Activities;
   using System.Activities.Statements;

   namespace CSWF4FlowChart
   {
       class Program
       {
           static void Main(string[] args)
           {
               WorkflowInvoker.Invoke(new GuessNumberGameInFlowChart());
           }
       }
   }