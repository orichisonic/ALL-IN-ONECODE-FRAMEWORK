using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SamplesModel.Enum;
using System.Diagnostics;

namespace SampleBrowser.UserControls
{
    /// <summary>
    /// Interaction logic for SearchControl.xaml
    /// </summary>
    public partial class SearchControl : UserControl
    {
        public SearchControl()
        {
            InitializeComponent();
        }

        private void serchControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.cbBoxLanguage.ItemsSource = GetLanguages();
            this.cbBoxTechnology.ItemsSource = GetTechnologies();
        }


        /// <summary>
        /// Get all languages enum and convert them to string type
        /// </summary>
        /// <returns></returns>
        List<string> GetLanguages()
        {
            List<string> lst = new List<string>();
            foreach (Language t in Enum.GetValues(typeof(Language)))
            {
                lst.Add(EnumOperator.GetLanguageStringValue(t));
            }
            lst.Sort();
            lst.Insert(0, "All Languages");
            return lst;
        }

        /// <summary>
        /// Get all technologies enum and convert them to string type
        /// </summary>
        /// <returns></returns>
        List<string> GetTechnologies()
        {
            List<string> lst = new List<string>();
            foreach (Technology t in Enum.GetValues(typeof(Technology)))
            {
                lst.Add(EnumOperator.GetTechnologyStringValue(t));
            }
            lst.Sort();
            lst.Insert(0, "All Technologies");
            return lst;
        }

        private void lanCkBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            string selectedText = cb.Content.ToString();
            if (cb.IsChecked == true)
            {
                if (selectedText == "All Languages")
                {
                    this.cbBoxLanguage.Text = selectedText;
                }
                else
                {
                    if (this.cbBoxLanguage.Text.Contains("All Languages"))
                    {
                        this.cbBoxLanguage.Text = this.cbBoxLanguage.Text.Replace("All Languages", "");
                    }
                    this.cbBoxLanguage.Text += "," + selectedText + ",";
                }
            }
            else
            {
                this.cbBoxLanguage.Text = this.cbBoxLanguage.Text.Replace(selectedText + ",", "").Replace(selectedText, "");
            }
            this.cbBoxLanguage.Text = this.cbBoxLanguage.Text.Trim(',');
        }

        private void techCkBox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            string selectedText = cb.Content.ToString();
            if (cb.IsChecked == true)
            {
                if (selectedText == "All Technologies")
                {
                    this.cbBoxTechnology.Text = selectedText;
                }
                else
                {
                    if (this.cbBoxTechnology.Text.Contains("All Technologies"))
                    {
                        this.cbBoxTechnology.Text = this.cbBoxTechnology.Text.Replace("All Technologies", "");
                    }
                    this.cbBoxTechnology.Text += "," + selectedText + ",";
                }
            }
            else
            {
                this.cbBoxTechnology.Text = this.cbBoxTechnology.Text.Replace(selectedText + ",", "").Replace(selectedText, "");
            }
            this.cbBoxTechnology.Text = this.cbBoxTechnology.Text.Trim(',');
        }
    }
}
