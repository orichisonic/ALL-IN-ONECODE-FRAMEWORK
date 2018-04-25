/****************************** Module Header ******************************\
* Module Name:  Program.cs
* Project:		CSWF4FlowChart
* Copyright (c) Microsoft Corporation.
* 
* A Guess Number Game workflow demostrate the useage of WF4 sequence woriflow.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* History:
* * 12/10/2009 1:30 PM Andrew Zhu Created
\***************************************************************************/
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
