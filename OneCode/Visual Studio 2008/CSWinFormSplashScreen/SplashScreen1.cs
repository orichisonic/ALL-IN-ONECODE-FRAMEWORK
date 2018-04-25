﻿/********************************* Module Header **********************************\
* Module Name:  SplashScreen1.cs
* Project:      CSWinFormSplashScreen
* Copyright (c) Microsoft Corporation.
* 
* This example demonstrates how to achieve splash screen effect in
* Windows Forms Application.
* 
* This source is subject to the Microsoft Public License.
* See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
* All other rights reserved.
* 
* History:
* * 7/14/2009 3:00 PM Bruce Zhou Created
\**********************************************************************************/

#region Using directives
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
#endregion


namespace CSWinFormSplashScreen
{
    public partial class SplashScreen1 : Form
    {
        System.Windows.Forms.Timer t = new System.Windows.Forms.Timer();
        bool fadeIn = true;
        bool fadeOut = false;

        public SplashScreen1()
        {
            InitializeComponent();
            ExtraFormSettings();
            // If we use solution2 we need to comment the following line.
            SetAndStartTimer();
        }

        private void SetAndStartTimer()
        {
            t.Interval = 100;
            t.Tick += new EventHandler(t_Tick);
            t.Start();
        }

        private void ExtraFormSettings()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.Opacity = 0.5;
            this.BackgroundImage = CSWinFormSplashScreen.Properties.Resources.SplashImage;
        }

      
        void t_Tick(object sender, EventArgs e)
        {
            // Fade in by increasing the opacity of the splash to 1.0
            if (fadeIn)
            {
                if (this.Opacity < 1.0)
                {
                    this.Opacity += 0.02;
                }
                // After fadeIn complete, begin fadeOut
                else
                {
                    fadeIn = false;
                    fadeOut = true;
                }
            }
            else if (fadeOut) // Fade out by increasing the opacity of the splash to 1.0
            {
                if (this.Opacity > 0)
                {
                    this.Opacity -= 0.02;
                }
                else
                {
                    fadeOut = false;
                }
            }

            // After fadeIn and fadeOut complete, stop the timer and close this splash.
            if (!(fadeIn || fadeOut))
            {
                t.Stop();
                this.Close();
            }
        }
    }
}
