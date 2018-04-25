/****************************** Module Header ******************************\
* Module Name:  ReadNumberActivity.cs
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
