/****************************** Module Header ******************************\
* Module Name:  Program.cs
* Project:		CSWF4SequenceWF
* Copyright (c) Microsoft Corporation.
* 
* A Guess Number Game workflow demostrate the useage of WF4 sequence woriflow.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* History:
* * 12/10/2009 8:50 AM Andrew Zhu Created
\***************************************************************************/
using System.Activities;

namespace CSWF4SequenceWF
{
    class Program
    {
        static void Main(string[] args)
        {
            WorkflowInvoker.Invoke(new GuessNumberGameSequenceWF());
        }
    }
}
